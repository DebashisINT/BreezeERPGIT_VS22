using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace BreezeChat.controls
{
    public class DBEngine
    {
        string strAppConnection = String.Empty;
        ProcedureExecute proc = new ProcedureExecute();
        // Internal reference to the Connection that is created.
        SqlConnection oSqlConnection = new SqlConnection();



        private string list_of_ids;     //______This is in use of message delete____//

        SQLProcedures oSqlProcedures = new SQLProcedures();
        GenericMethod oGenericMethod = null;


        public enum DateConvertFrom
        {
            UTCToDateTime, UTCToOnlyDate, UTCToOnlyTime, UTCToDateTimeHour, UTCToDateTimeHourSecond, UTCToDateTimeHourSecondMiliSecond
        }

        // Constructor
        public DBEngine(String cConnectionString)
        {

        }

        // Destructor
        ~DBEngine()
        {

        }


        public DBEngine()
        {
            //  strAppConnection = ConfigurationSettings.AppSettings["DBConnectionDefault"];  //---Multi Connection String


        }


        // ------------------------------------------------------------------------ //
        // This method returns a SqlConnection object
        // ------------------------------------------------------------------------ //      
        public void GetConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
            }
            else
            {
                if (HttpContext.Current.Session["EntryProfileType"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                    {//DBReadOnlyConnection


                        //oSqlConnection.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];

                        oSqlConnection.ConnectionString = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);
                    }
                    else
                    {
                        //oSqlConnection.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];

                        oSqlConnection.ConnectionString = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

                    }
                    oSqlConnection.Open();
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/OMS/Login.aspx");
                }
            }
        }

        public string ReturnConnectionstring()
        {
            string connection = string.Empty;

            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
            }
            else
            {
                if (HttpContext.Current.Session["EntryProfileType"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                    {//DBReadOnlyConnection
                        //  connection = ConfigurationSettings.AppSettings["DBReadOnlyConnection"]; --MULTI

                        connection = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);
                    }
                    else
                    {
                        //connection = ConfigurationSettings.AppSettings["DBConnectionDefault"]; --MULTI

                        connection = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

                    }
                }


            }
            return connection;
        }
        public void CloseConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
                oSqlConnection.Close();
            }
        }

        // ------------------------------------------------------------------------ //
        // This generic method returns a SqlDataReader object from the passed
        // SQL command.
        // ------------------------------------------------------------------------ //
        public SqlDataReader GetReader(String cSql)
        {
            // Now we create a Command object and execute the command.

            GetConnection();
            SqlDataReader lsdr;

            SqlCommand lcmd = new SqlCommand(cSql, oSqlConnection);

            lsdr = lcmd.ExecuteReader();
            return lsdr;
        }

        // ------------------------------------------------------------------------ //
        // This generic method returns a SqlDataReader object from the passed
        // SQL command and Connection.
        // ------------------------------------------------------------------------ //
        public SqlDataReader GetReader1(String cSql, string SqlCOnnnection)
        {
            // calling GetConnection Method to connect database           
            GetConnection();
            // Now we create a Command object and execute the command.          
            SqlCommand lcmd = new SqlCommand(cSql, oSqlConnection);
            SqlDataReader lsdr;
            lsdr = lcmd.ExecuteReader();
            return lsdr;
        }

        // --------------------------------------------------------------------------//
        // This method returns the value of ANY field from ANY table in the database.
        // Example Usage :
        // GetFieldValue("tbl_master_bank", "bnk_bankName", "bnk_id=7075")
        // NOT in Use Right now, Because of ONE DIAMENTINAL ARRAY!
        // --------------------------------------------------------------------------//
        public string[] GetFieldValue1(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,     // The name if the field whose value needs to be returned
                String cWhereClause,    // Optional : WHERE condition [if any]
                int NoField)   // Number of field value to selection
        {
            // Return Value
            string[] vRetVal = new string[NoField];

            // Now we construct a SQL command that will fetch the field from the table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }

            SqlDataReader lsdr;
            lsdr = GetReader(lcSql);

            if (lsdr.HasRows)
            {
                while (lsdr.Read())
                {
                    for (int i = 0; i < NoField; i++)
                    {
                        vRetVal[i] = lsdr[i].ToString();
                    }
                }
            }
            else
            {
                vRetVal[0] = "n";
            }

            // We close the DataReader
            lsdr.Close();

            return vRetVal;
        }

        //-------------------------------------------------------------------------------------------------------
        // This Method authenticate an user and depennd on the Authentication this method creats a session of the
        // user with datafield; like: user_id,user_password, user_name, usercontact_Id, userbatch_id, user_group,
        // user_lastsegement.
        // Example Usage :
        // AuthenticateUser(amit, amit)
        //-------------------------------------------------------------------------------------------------------

        public string getCompanyList(string companyId, string ListOfCompany)
        {
            DataTable DtSecond = GetDataTable(" tbl_master_company ", " cmp_internalid ", " cmp_parentid= " + companyId);
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfCompany += "'" + DtSecond.Rows[i][0].ToString() + "'" + ",";
                    actual += "'" + DtSecond.Rows[i][0].ToString() + "'" + ",";
                    getCompanyList("'" + DtSecond.Rows[i][0].ToString() + "'", ListOfCompany);
                }
            }
            return actual;
        }

        public void GetChildCompany(string parentcompany)
        {
            string CompanyList = getCompanyList(parentcompany, "");
            string AllCompany = "";
            if (CompanyList == "")
            {
                AllCompany = parentcompany;
            }
            else
            {
                CompanyList = CompanyList.TrimEnd(',');
                AllCompany = parentcompany + "," + CompanyList;
            }
            HttpContext.Current.Session["CompanyHierarchy"] = AllCompany;
        }

        public string AuthenticateUser(string UserName, String Password)
        {
            //HttpContext.Current.Session["ErpConnection"] = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString; //Need to remove.
            // This null password option can help to get password option!



            string whereClause = "user_loginId ='" + UserName + "'";
            if (Password != null)
            {
                whereClause += " and cast(user_password as varbinary) =cast('" + Password + "' as varbinary)" + " and cnt_internalid = user_contactid and emp_contactId=cnt_internalid and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>=getdate()) ";
            }

            // ............. Code Commented by Sam on 07112016 due to get user its own cnt_internalid....................
            //            string[,] ValidUser = GetFieldValue("tbl_master_user,tbl_master_contact,tbl_master_employee ",
            //                @"user_id,user_name, user_password,user_contactId, cnt_branchid, user_group,user_lastsegement,
            //            cnt_contactType,user_TimeForTickerRefrsh, cnt_shortName,user_superUser ,
            //            (select top 1 emp_Organization from tbl_trans_employeeCTC where emp_cntId=user_contactid
            //            and (emp_effectiveuntil is null or emp_effectiveuntil='1/1/1900 12:00:00 AM'
            //            OR emp_effectiveuntil>=getdate()) order by emp_effectiveDate desc) as emp_Organization,
            //            user_EntryProfile,user_status,user_lastIP,emp_id,user_AllowAccessIP",

            // .............................Code Commented and Added by Sam on 28122016 to fetch cnt_id column of tbl_master_contact for sales module. ..................................... 
            //            string[,] ValidUser = GetFieldValue("tbl_master_user,tbl_master_contact,tbl_master_employee ",
            //                @"user_id,user_name, user_password,user_contactId, cnt_branchid, user_group,user_lastsegement,
            //            cnt_contactType,user_TimeForTickerRefrsh, cnt_shortName,user_superUser ,
            //            (select top 1 emp_Organization from tbl_trans_employeeCTC where emp_cntId=user_contactid
            //            and (emp_effectiveuntil is null or emp_effectiveuntil='1/1/1900 12:00:00 AM'
            //            OR emp_effectiveuntil>=getdate()) order by emp_effectiveDate desc) as emp_Organization,
            //            user_EntryProfile,user_status,user_lastIP,emp_id,user_AllowAccessIP,cnt_internalId",

            string[,] ValidUser = GetFieldValue("tbl_master_user,tbl_master_contact,tbl_master_employee ",
                @"user_id,user_name,
            user_password,user_contactId, cnt_branchid, user_group,user_lastsegement,
            cnt_contactType,user_TimeForTickerRefrsh, cnt_shortName,user_superUser ,
            (select top 1 emp_Organization from tbl_trans_employeeCTC where emp_cntId=user_contactid
            and (emp_effectiveuntil is null or emp_effectiveuntil='1/1/1900 12:00:00 AM'
            OR emp_effectiveuntil>=getdate()) order by emp_effectiveDate desc) as emp_Organization,
            user_EntryProfile,user_status,user_lastIP,emp_id,user_AllowAccessIP,cnt_internalId,cnt_id,
            CONVERT (varchar(10),last_login_date, 105) AS Lastlogindate,CONVERT(varchar,last_login_date,8) As Lastlogintime,Isnull(emp_image,'') emp_image",

            // .............................Code Above Commented and Added by Sam on 28122016 to fetch cnt_id column of tbl_master_contact for sales module. ..................................... 


          // ............. Code Above Commented by Sam on 07112016 due to get user its own cnt_internalid....................

                whereClause, 22);

            if (ValidUser[0, 0] == "n")
            {
                if (Password != null)
                {
                    return ValidUser[0, 0] = "Invalid UserID or Password!";
                }
                else
                    return ValidUser[0, 0] = "Invalid UserID";
            }
            else
            {
                if (ValidUser[0, 13].ToString() == "100")
                {
                    return ValidUser[0, 0] = "You are Already Logged In " + ValidUser[0, 14].ToString() + "";
                }
                else
                {
                    if (Password != null)
                    {
                        string[,] IsActive = GetFieldValue("tbl_master_user", "RTRIM(LTRIM(user_inactive))", "user_loginId ='" + UserName + "' and user_password='" + Password + "'", 1);
                        if (IsActive[0, 0].ToString() == "N")
                        {


                            oGenericMethod = new GenericMethod();

                            HttpContext.Current.Session["UserName"] = UserName;
                            HttpContext.Current.Session["userid"] = ValidUser[0, 0].ToString();
                            HttpContext.Current.Session["username"] = ValidUser[0, 1].ToString();
                            HttpContext.Current.Session["userpassword"] = ValidUser[0, 2].ToString();
                            HttpContext.Current.Session["usercontactID"] = ValidUser[0, 3].ToString();
                            HttpContext.Current.Session["userbranchID"] = ValidUser[0, 4].ToString();
                            HttpContext.Current.Session["usergoup"] = ValidUser[0, 5].ToString();
                            HttpContext.Current.Session["userlastsegment"] = ValidUser[0, 6].ToString();
                            HttpContext.Current.Session["userContactType"] = ValidUser[0, 7].ToString();
                            HttpContext.Current.Session["TimeForTickerDisplay"] = ValidUser[0, 8].ToString();
                            HttpContext.Current.Session["EmployeeID"] = ValidUser[0, 9].ToString();
                            HttpContext.Current.Session["EntryProfileType"] = ValidUser[0, 12].ToString();
                            HttpContext.Current.Session["userAllowAccessIP"] = ValidUser[0, 16].ToString();
                            // ............. Code Commented by Sam on 07112016 due to get user its own cnt_internalid....................
                            HttpContext.Current.Session["owninternalid"] = Convert.ToString(ValidUser[0, 17]);
                            HttpContext.Current.Session["cntId"] = Convert.ToString(ValidUser[0, 18]);

                            // ............. Code Above Commented by Sam on 07112016 due to get user its own cnt_internalid....................
                            HttpContext.Current.Session["LastCompany"] = string.Empty;
                            HttpContext.Current.Session["LastFinYear"] = string.Empty;
                            HttpContext.Current.Session["LastSettNo"] = string.Empty;
                            HttpContext.Current.Session["DataTable_Menu"] = GetDataTable("tbl_trans_menu", "mnu_id, mnu_menuName, mnu_menuLink, mun_parentId, mnu_segmentId, mnu_image", "mnu_menulink not like '%frmwip%'", null);
                            string[,] EmployeereportTo = GetFieldValue(" tbl_trans_employeeCTC ", " emp_reportTo ", " emp_cntId='" + HttpContext.Current.Session["usercontactID"] + "'", 1);

                            if (EmployeereportTo[0, 0].Trim() == "0")
                                HttpContext.Current.Session["superuser"] = "Y";
                            else
                                HttpContext.Current.Session["superuser"] = "";
                            if (ValidUser[0, 10].ToString().Trim() == "Y")  //_must be treated as superuser: taking top level Hierarchy!
                            {
                                //CommonBL cbl = new CommonBL();
                                //string DefaultBranchInLogin = cbl.GetSystemSettingsResult("DefaultBranchInLogin");
                                //if (DefaultBranchInLogin.ToUpper() == "YES")
                                //{
                                /////NewMethodwFindOUtEmployeeHierarch
                                HttpContext.Current.Session["userchildHierarchy"] = GeEmployeeHierarchy(ValidUser[0, 15].ToString());
                                string branch = getBranch(HttpContext.Current.Session["userbranchID"].ToString(), "") + HttpContext.Current.Session["userbranchID"].ToString();
                                actual = "";
                                HttpContext.Current.Session["userbranchHierarchy"] = branch;
                                //}
                                //else
                                //{
                                //    string branchs = GeEmployeeHierarchyBranch(ValidUser[0, 3].ToString());

                                //    if (string.IsNullOrEmpty(branchs))
                                //    {
                                //        branchs = HttpContext.Current.Session["userbranchID"].ToString();
                                //    }


                                //    HttpContext.Current.Session["userbranchHierarchy"] = branchs;
                                //}
                            }
                            else
                            {
                                CommonBL cbl = new CommonBL();
                                string DefaultBranchInLogin = "YES";
                                if (DefaultBranchInLogin.ToUpper() == "YES")
                                {
                                    //Commented by Jitendra to test 
                                    string userlist = getChildUserNotColleague(ValidUser[0, 0].ToString(), "");

                                    actual = "";
                                    HttpContext.Current.Session["userchildHierarchy"] = userlist;

                                    DataSet dsbranchhrchy = new DataSet();
                                    string[] strSpParam = new string[1];
                                    strSpParam[0] = "branchid|" + GenericStoreProcedure.ParamDBType.Int + "|10|" + HttpContext.Current.Session["userbranchID"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;

                                    GenericStoreProcedure oGenericStoreProcedure = new GenericStoreProcedure();
                                    try
                                    {
                                        dsbranchhrchy = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Hr_GetBranchSubTree");
                                        HttpContext.Current.Session["userbranchHierarchy"] = dsbranchhrchy.Tables[0].Rows[0][0].ToString();
                                    }
                                    catch { }
                                }
                                else
                                {
                                    string branchs = GeEmployeeHierarchyBranch(ValidUser[0, 3].ToString());

                                    if (string.IsNullOrEmpty(branchs))
                                    {
                                        branchs = HttpContext.Current.Session["userbranchID"].ToString();
                                    }


                                    HttpContext.Current.Session["userbranchHierarchy"] = branchs;
                                }
                            }
                            //___This will get all accessebla pages for reaspective segment

                            #region Lastlogindate & image
                            HttpContext.Current.Session["Lastlogindate"] = ValidUser[0, 19].ToString();
                            HttpContext.Current.Session["Lastlogintime"] = ValidUser[0, 20].ToString();
                            HttpContext.Current.Session["UserImage"] = ValidUser[0, 21].ToString();

                            #endregion
                            getAccessPages();
                            string[] segmentname = proc.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                            string[] sname = segmentname[0].Split('-');
                            if (sname.Length > 1)
                            {
                                string[] ExchangeSegmentID = proc.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='" + sname[1] + "'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
                                HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
                            }
                            if (sname[0] == "Accounts")
                            {
                                string[] ExchangeSegmentID = proc.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='ACC'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
                                HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
                            }
                            //Subhabrata
                            if (Convert.ToString(HttpContext.Current.Session["userbranchID"]) != "")
                            {
                                DataTable dt1 = new DataTable();
                                ProcedureExecute proc1 = new ProcedureExecute("GetSiblingsBranchIds");

                                proc1.AddIntegerPara("@BranchId", Convert.ToInt32(HttpContext.Current.Session["userbranchID"]));

                                dt1 = proc1.GetTable();
                                string SibLingBranch = string.Empty;

                                if (dt1 != null && dt1.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        SibLingBranch += Convert.ToString(dt1.Rows[i]["branch_id"]) + ",";
                                    }
                                }


                                HttpContext.Current.Session["UserSiblingBranchHierarchy"] = HttpContext.Current.Session["userbranchHierarchy"] + "," + SibLingBranch;

                            }//End

                            if (HttpContext.Current.Session["userlastsegment"].ToString() != "")
                            {
                                DataTable UserLastSegmentInfo = oGenericMethod.GetUserLastSegmentDetail();
                                if (UserLastSegmentInfo.Rows.Count > 0)
                                {
                                    //HttpContext.Current.Session["LastCompany"] = "COR0000002";
                                    HttpContext.Current.Session["usersegid"] = UserLastSegmentInfo.Rows[0][0].ToString().Trim();
                                    HttpContext.Current.Session["LastCompany"] = UserLastSegmentInfo.Rows[0][1].ToString();
                                    // Code Added by Sam to fetch All child Company Of Parent Company
                                    // To show Account Head belongs to Parent and Child Company
                                    // Version 1.0.0.1
                                    string parentcompany = "'" + HttpContext.Current.Session["LastCompany"].ToString() + "'";
                                    GetChildCompany(parentcompany);
                                    //string CompanyList = getCompanyList(parentcompany, "");
                                    //string AllCompany = "";
                                    //if (CompanyList=="")
                                    //{
                                    //    AllCompany = parentcompany;
                                    //}
                                    //else
                                    //{
                                    //    CompanyList = CompanyList.TrimEnd(',');
                                    //    AllCompany = parentcompany + "," + CompanyList;
                                    //}
                                    //HttpContext.Current.Session["CompanyHierarchy"] = AllCompany;


                                    // Version 1.0.0.1 End

                                    HttpContext.Current.Session["LastFinYear"] = UserLastSegmentInfo.Rows[0][2].ToString();
                                    HttpContext.Current.Session["LastSettNo"] = UserLastSegmentInfo.Rows[0][3].ToString();
                                    HttpContext.Current.Session["LedgerView"] = UserLastSegmentInfo.Rows[0][6].ToString();
                                    HttpContext.Current.Session["StartdateFundsPayindate"] = UserLastSegmentInfo.Rows[0][5].ToString();// fetch startdate and FundsPayin from Master_Settlements
                                    ////////////////////////Entry Lock System Session Creation/////////////////////////////
                                    string SegmentID = null;
                                    DataTable DtLockEntrySystem = null;
                                    string UserLastSegment = HttpContext.Current.Session["userlastsegment"].ToString();
                                    if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6")
                                    {
                                        if (UserLastSegment == "9" || UserLastSegment == "10")
                                        {
                                            DtLockEntrySystem = GetDataTable("tbl_master_CompanyExchange", "Exch_InternalID", "Exch_CompID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Exch_TMCode='" + HttpContext.Current.Session["UserSegID"].ToString() + "'");
                                            SegmentID = DtLockEntrySystem.Rows[0][0].ToString();
                                            DtLockEntrySystem = null;
                                        }
                                        else
                                        {
                                            SegmentID = HttpContext.Current.Session["UserSegID"].ToString();
                                        }

                                        //======================================Expiry Module==========================
                                        //Set Expiry From Encrypted File
                                        if (HttpContext.Current.Session["UserLastSegment"] != null && HttpContext.Current.Session["LastCompany"] != null
                                            && HttpContext.Current.Session["UserLastSegment"] != "" && HttpContext.Current.Session["LastCompany"] != "")
                                            oGenericMethod.EncryptDecript(1, "SetExpiryDate~", System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");
                                        //======================================End Expiry Module==========================

                                        // Variable For All "GS_LCKALL"
                                        // 1.Session CashBank -->"GS_LCKBNK",
                                        // 2.Session Demate -->"GS_LCKDEMAT",
                                        // 3.Session Trade -->"GS_LCKTRADE",
                                        // 4.Session JounalVoucher -->"GS_LCKJV",
                                        DtLockEntrySystem = GetDataTable("(Select GlobalSettings_Name," +
                                                                                @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate)))
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKBNK'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
                                                            Union
                                                            Select 'GS_LCKDEMAT' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
                                                            Select GlobalSettings_Name," +
                                                                                @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate)))
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKDEMAT'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
                                                            Union
                                                            Select 'GS_LCKTRADE' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
                                                            Select GlobalSettings_Name," +
                                                                                @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate)))
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKTRADE'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2
                                                            Union
                                                            Select 'GS_LCKJV' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate From(
                                                            Select GlobalSettings_Name," +
                                                                                @"Case
                                                                When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
                                                                Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate)))
                                                                End as LockDaysOrDate
                                                                From
                                                                (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKJV'
                                                                union
                                                                Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
                                                                GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
                                                                Where GlobalSettings_SegmentID=" + SegmentID + @"
                                                                and GlobalSettings_Name='GS_LCKALL') as t1) as t2", "'GS_LCKBNK' as GlobalSettings_Name,Max(LockDaysOrDate) as MaxLockDate", null);

                                        if (DtLockEntrySystem.Rows.Count > 0)
                                        {
                                            DataRow row;
                                            DtLockEntrySystem.PrimaryKey = new DataColumn[] { DtLockEntrySystem.Columns["GlobalSettings_Name"] };
                                            row = DtLockEntrySystem.Rows.Find("GS_LCKBNK");
                                            if (row != null)
                                            {
                                                if (row[1].ToString() != string.Empty)
                                                {
                                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                                        HttpContext.Current.Session["LCKBNK"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                                    else
                                                        HttpContext.Current.Session["LCKBNK"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                                                }
                                                else
                                                {
                                                    HttpContext.Current.Session["LCKBNK"] = null;
                                                    row = null;
                                                }
                                            }
                                            row = DtLockEntrySystem.Rows.Find("GS_LCKDEMAT");
                                            if (row != null)
                                            {
                                                if (row[1].ToString() != string.Empty)
                                                {
                                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                                        HttpContext.Current.Session["LCKDEMAT"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                                    else
                                                        HttpContext.Current.Session["LCKDEMAT"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                                                }
                                                else
                                                {
                                                    HttpContext.Current.Session["LCKDEMAT"] = null;
                                                    row = null;
                                                }
                                            }
                                            row = DtLockEntrySystem.Rows.Find("GS_LCKTRADE");
                                            if (row != null)
                                            {
                                                if (row[1].ToString() != string.Empty)
                                                {
                                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                                        HttpContext.Current.Session["LCKTRADE"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                                    else
                                                        HttpContext.Current.Session["LCKTRADE"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                                                }
                                                else
                                                {
                                                    HttpContext.Current.Session["LCKTRADE"] = null;
                                                    row = null;
                                                }
                                            }
                                            row = DtLockEntrySystem.Rows.Find("GS_LCKJV");
                                            if (row != null)
                                            {
                                                if (row[1].ToString() != string.Empty)
                                                {
                                                    if (Convert.ToDateTime(row[1].ToString()) > Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString()))
                                                        HttpContext.Current.Session["LCKJV"] = Convert.ToDateTime(HttpContext.Current.Session["ExpireDate"].ToString());
                                                    else
                                                        HttpContext.Current.Session["LCKJV"] = row[1].ToString().Trim() != String.Empty ? row[1].ToString() : null; row = null;
                                                }
                                                else
                                                {
                                                    HttpContext.Current.Session["LCKJV"] = null;
                                                    row = null;
                                                }
                                            }
                                        }
                                    }
                                    //End Session For CashBankEntry
                                    //////////////////////End Entry Lock System Session Creation/////////////////////////////
                                }
                                //}

                                //string[,] segId = GetFieldValue("tbl_trans_LastSegment", "ls_lastdpcoid,ls_lastCompany,ls_lastFinYear,ls_lastSettlementNo,(select cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar) from Master_Settlements where  Ltrim(RTRIM(settlements_Number))=ls_lastSettlementNo and ltrim(RTRIM(settlements_TypeSuffix))=ls_lastSettlementType)", "ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"] + "' and ls_cntId='" + HttpContext.Current.Session["usercontactID"] + "'", 5);
                                //if (segId[0, 0] != "n")
                                //{
                                //    HttpContext.Current.Session["usersegid"] = segId[0, 0].ToString().Trim();
                                //    HttpContext.Current.Session["LastCompany"] = segId[0, 1].ToString();
                                //    HttpContext.Current.Session["LastFinYear"] = segId[0, 2].ToString();
                                //    HttpContext.Current.Session["LastSettNo"] = segId[0, 3].ToString();
                                //    HttpContext.Current.Session["StartdateFundsPayindate"] = segId[0, 4].ToString();// fetch startdate and FundsPayin from Master_Settlements

                                //}
                            }
                            //Now finding the company Hierarchy --ValidUser[0, 11].ToString();
                            HttpContext.Current.Session["userCompanyHierarchy"] = GetAllCompanyInHierarchy(ValidUser[0, 11].ToString());


                            SetFinYearStartandEndDate();
                        }
                        else
                        {
                            return ValidUser[0, 0] = "Inactive UserID";
                        }
                    }
                    return "Y";
                }
            }
        }

        public string AuthenticateVendorUser(string UserName, String Password)
        {
            // This null password option can help to get password option!
            string whereClause = "user_loginId ='" + UserName + "'";
            if (Password != null)
            {
                whereClause += " and cast(user_password as varbinary) =cast('" + Password + "' as varbinary)" + " and cnt_internalid = user_contactid";
            }


            string[,] ValidUser = GetFieldValue("tbl_master_VendorUser,tbl_master_contact",
                @"user_id,user_name, user_password,user_contactId, cnt_branchid, user_group,user_lastsegement,
            cnt_contactType,user_TimeForTickerRefrsh, cnt_shortName,user_superUser ,            
            user_EntryProfile,user_status,user_lastIP,user_AllowAccessIP,cnt_internalId,cnt_id",
                whereClause, 17);

            if (ValidUser[0, 0] == "n")
            {
                if (Password != null)
                {
                    return ValidUser[0, 0] = "Invalid UserID or Password!";
                }
                else
                    return ValidUser[0, 0] = "Invalid UserID";
            }
            else
            {
                if (ValidUser[0, 13].ToString() == "100")
                {
                    return ValidUser[0, 0] = "You are Already Logged In " + ValidUser[0, 14].ToString() + "";
                }
                else
                {
                    if (Password != null)
                    {
                        string[,] IsActive = GetFieldValue("tbl_master_VendorUser", "RTRIM(LTRIM(user_inactive))", "user_loginId ='" + UserName + "' and user_password='" + Password + "'", 1);
                        if (IsActive[0, 0].ToString() == "N")
                        {
                            oGenericMethod = new GenericMethod();
                            HttpContext.Current.Session["userType"] = "DV";
                            HttpContext.Current.Session["userid"] = ValidUser[0, 0].ToString();
                            HttpContext.Current.Session["username"] = ValidUser[0, 1].ToString();
                            HttpContext.Current.Session["userpassword"] = ValidUser[0, 2].ToString();
                            HttpContext.Current.Session["usercontactID"] = ValidUser[0, 3].ToString();
                            HttpContext.Current.Session["userbranchID"] = ValidUser[0, 4].ToString();
                            HttpContext.Current.Session["usergoup"] = ValidUser[0, 5].ToString();
                            HttpContext.Current.Session["userlastsegment"] = ValidUser[0, 6].ToString();
                            HttpContext.Current.Session["userContactType"] = ValidUser[0, 7].ToString();
                            HttpContext.Current.Session["TimeForTickerDisplay"] = ValidUser[0, 8].ToString();
                            //HttpContext.Current.Session["EmployeeID"] = ValidUser[0, 9].ToString();
                            HttpContext.Current.Session["EntryProfileType"] = ValidUser[0, 12].ToString();
                            //HttpContext.Current.Session["userAllowAccessIP"] = ValidUser[0, 16].ToString();

                            // HttpContext.Current.Session["owninternalid"] = Convert.ToString(ValidUser[0, 17]);
                            //  HttpContext.Current.Session["cntId"] = Convert.ToString(ValidUser[0, 18]);

                            // ............. Code Above Commented by Sam on 07112016 due to get user its own cnt_internalid....................
                            HttpContext.Current.Session["LastCompany"] = string.Empty;
                            HttpContext.Current.Session["LastFinYear"] = string.Empty;
                            HttpContext.Current.Session["LastSettNo"] = string.Empty;
                            //   string[,] EmployeereportTo = GetFieldValue(" tbl_trans_employeeCTC ", " emp_reportTo ", " emp_cntId='" + HttpContext.Current.Session["usercontactID"] + "'", 1);

                            //if (EmployeereportTo[0, 0].Trim() == "0")
                            //    HttpContext.Current.Session["superuser"] = "Y";
                            //else
                            //    HttpContext.Current.Session["superuser"] = "";

                            if (ValidUser[0, 10].ToString().Trim() == "Y")  //_must be treated as superuser: taking top level Hierarchy!
                            {
                                /////NewMethodwFindOUtEmployeeHierarch
                                //HttpContext.Current.Session["userchildHierarchy"] = GeEmployeeHierarchy(ValidUser[0, 15].ToString());
                                // string branch = getBranch(HttpContext.Current.Session["userbranchID"].ToString(), "") + HttpContext.Current.Session["userbranchID"].ToString();
                                //actual = "";
                                // HttpContext.Current.Session["userbranchHierarchy"] = branch;
                            }
                            else
                            {
                                //string userlist = getChildUserNotColleague(ValidUser[0, 0].ToString(), "");
                                //actual = "";
                                //HttpContext.Current.Session["userchildHierarchy"] = userlist;

                                //DataSet dsbranchhrchy = new DataSet();
                                //string[] strSpParam = new string[1];
                                //strSpParam[0] = "branchid|" + GenericStoreProcedure.ParamDBType.Int + "|10|" + HttpContext.Current.Session["userbranchID"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;

                                //GenericStoreProcedure oGenericStoreProcedure = new GenericStoreProcedure();
                                //try
                                //{
                                //    dsbranchhrchy = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Hr_GetBranchSubTree");
                                //    HttpContext.Current.Session["userbranchHierarchy"] = dsbranchhrchy.Tables[0].Rows[0][0].ToString();
                                //}
                                //catch { }
                            }
                            //___This will get all accessebla pages for reaspective segment
                            //getAccessPages();
                            //string[] segmentname = proc.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                            //string[] sname = segmentname[0].Split('-');
                            //if (sname.Length > 1)
                            //{
                            //    string[] ExchangeSegmentID = proc.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='" + sname[1] + "'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
                            //HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
                            HttpContext.Current.Session["ExchangeSegmentID"] = "1";
                            // }
                            //if (sname[0] == "Accounts")
                            //{
                            //    string[] ExchangeSegmentID = proc.GetFieldValue1("Master_ExchangeSegments MES,Master_Exchange ME", "MES.ExchangeSegment_ID", "MES.ExchangeSegment_Code='ACC'And MES.ExchangeSegment_ExchangeID=ME.Exchange_ID AND ME.Exchange_ShortName='" + sname[0] + "'", 1);
                            //    HttpContext.Current.Session["ExchangeSegmentID"] = ExchangeSegmentID[0].ToString();
                            //}

                            if (HttpContext.Current.Session["userlastsegment"].ToString() != "")
                            {
                                DataTable UserLastSegmentInfo = oGenericMethod.GetUserLastSegmentDetail();
                                if (UserLastSegmentInfo.Rows.Count > 0)
                                {
                                    //HttpContext.Current.Session["LastCompany"] = "COR0000002";
                                    HttpContext.Current.Session["usersegid"] = UserLastSegmentInfo.Rows[0][0].ToString().Trim();
                                    HttpContext.Current.Session["LastCompany"] = UserLastSegmentInfo.Rows[0][1].ToString();
                                    // Code Added by Sam to fetch All child Company Of Parent Company
                                    // To show Account Head belongs to Parent and Child Company
                                    // Version 1.0.0.1
                                    string parentcompany = "'" + HttpContext.Current.Session["LastCompany"].ToString() + "'";
                                    GetChildCompany(parentcompany);



                                    // Version 1.0.0.1 End

                                    HttpContext.Current.Session["LastFinYear"] = UserLastSegmentInfo.Rows[0][2].ToString();
                                    HttpContext.Current.Session["LastSettNo"] = UserLastSegmentInfo.Rows[0][3].ToString();
                                    HttpContext.Current.Session["LedgerView"] = UserLastSegmentInfo.Rows[0][6].ToString();
                                    HttpContext.Current.Session["StartdateFundsPayindate"] = UserLastSegmentInfo.Rows[0][5].ToString();// fetch startdate and FundsPayin from Master_Settlements

                                }

                            }
                            //Now finding the company Hierarchy --ValidUser[0, 11].ToString();
                            //  HttpContext.Current.Session["userCompanyHierarchy"] = GetAllCompanyInHierarchy(ValidUser[0, 11].ToString());



                        }
                        else
                        {
                            return ValidUser[0, 0] = "Inactive UserID";
                        }
                    }
                    return "Y";
                }
            }
        }

        #region Data Table creation

        //-------------------------------------------------------------------------------------------//
        // This will returm a DATATABLE frompassed parameter
        // Example Usage:
        // GetDataTable("tbl_trans_menu", "mnu_id, mnu_menuName, mnu_menuLink, mun_parentId, mnu_segmentId, mnu_image", null);
        //-------------------------------------------------------------------------------------------//
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause)   // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        //////////////////////////11/02/2012 developed by Subhadeep/////////

        public DataTable GetDataTable(
                           String query)    // TableName from which the field value is to be fetched
        // The name if the field whose value needs to be returned
        // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = query;

            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable getquery = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(getquery);
            oSqlConnection.Close();
            return getquery;
        }

        //////////////////////11/02/2012 developed/////////////////////////////
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string cOrderBy)       // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable GetDataTableGroup(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy)       // Group by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy,         // Gropu by condition
                            string cOrderBy)        // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        #endregion Data Table creation

        // This function will send a mail to the user who forgot password by generating random
        // password for the user AND update password in the database with the new password generated.
        public string ExeSclar(string query)
        {

            GetConnection();

            SqlCommand cmd = new SqlCommand(query, oSqlConnection);

            cmd.CommandText = query;
            string retval = (string)cmd.ExecuteScalar();



            return retval;
        }



        public int ExeInteger(string query)
        {

            GetConnection();
            SqlCommand cmd = new SqlCommand(query, oSqlConnection);
            cmd.CommandText = query;
            int countDis = Convert.ToInt32(cmd.ExecuteScalar());
            return countDis;
        }


        public string SendMailToUser(string userId)
        {
            string[,] EmailId = GetFieldValue("tbl_master_email A, tbl_master_user B", "A.eml_email, B.user_password",
                                            "B.user_contactId = A.eml_cntId and A.eml_type ='official " +
                                            "' and B.user_leavedate='true' and B.user_loginId='" + userId + "'", 2);
            if (EmailId != null)
            {
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("influxcrm@gmail.com", "password");
                Message.To.Add(new MailAddress(EmailId[0, 0].ToString()));
                Message.CC.Add(new MailAddress("influxcrm@gmail.com"));
                Message.Subject = "Your Login-ID/ Password Detail";
                Message.IsBodyHtml = true;
                Random randomgenerator = new Random();
                int randumNumber;
                //string body;
                randumNumber = randomgenerator.Next(111111, 999999);
                // password creation that include as:
                // UserID + N + RandomNumber
                // Example Usage: amitN425487
                string pass = userId + "N" + randumNumber;
                Message.Body = "<table border='0' + cellpadding='3' cellspacing='3' width= '100%' font-weight= 'normal' font-size= '8pt'  color= 'black' font-style= normal font-family= 'Verdana'>";
                Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >   -: UserID/Password Information :- </td></tr>";
                Message.Body += "<tr><td bordercolor='tan' borderwidth='1px' valign='top'  background-color='antiquewhite' text-align= 'center'> User Id :-   </td><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align = 'left' > " + userId + " </td></tr>";
                Message.Body += "<tr><td border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center;  >Previous Password :- </td><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align= 'left' >" + EmailId[0, 1] + " </td></tr>";
                Message.Body += "<tr><td border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center;  >New Password :- </td><td border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; >" + pass + " </td></tr>";
                Message.Body += "<tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '>   -: CRM :- </td></tr>";
                Message.Body += "</table>";
                SetFieldValue("tbl_master_user", "user_password = '" + pass + "'", " user_loginId='" + userId + "'");
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Credentials = new System.Net.NetworkCredential("influxcrm@gmail.com", "influxcrm2009");
                //client.send(Message);
                client.Send(Message);

                //return " Get your new password in offcial mail account ";
                return pass;
            }
            else
            {
                return " User ID does not exist ";
            }
        }

        public void DailyMailCreation()
        {
            //_Query to filter records
            string lsSql_salesVisit = "select slv_id, act_assignedby, act_assignedto,act_id,slv_leadCotactId," +
                                    "(select cnt_firstName+' '+isnull(cnt_MiddleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_shortName+']' from tbl_master_contact where cnt_internalId in (select user_contactId from tbl_master_user where user_id= act_assignedby)) as assignUser," +
                                    "(select cnt_firstName+' '+isnull(cnt_MiddleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_shortName+']' from tbl_master_contact where cnt_internalId in (select user_contactId from tbl_master_user where user_id = act_assignedto)) as targetUser," +
                                    "(select user_contactId from tbl_master_user where user_id= act_assignedby) as SenderContactId," +
                                    "(select user_contactId from tbl_master_user where user_id= act_assignedto) as TargetContactId," +
                                    "(select top 1 (select slv_salesvisitOutcome from tbl_master_SalesVisitOutcome where slv_id= tbl_trans_SalesVisitdetail.slv_salesVisitOutcome)+'['+slv_notes+']' from tbl_trans_SalesVisitdetail where slv_salesvisitId=tbl_trans_salesvisit.slv_id) as note," +
                                    "(select top 1 case slv_nextactivityType when '1' then 'Phone Follow Up' when '2' then 'Meeting' else '' end from tbl_trans_SalesVisitdetail where slv_salesvisitId=tbl_trans_salesvisit.slv_id) as VisitType," +
                                    "(select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_internalId+']'  from tbl_master_lead where cnt_internalId= slv_leadCotactId)as LeadName," +
                                    "(select phf_phoneNumber from tbl_master_phonefax where phf_Type='Office' and phf_cntId=slv_leadCotactId) as phone," +
                                    "(select isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(add_landmark,'')+','+(select cou_country from tbl_master_country where cou_id=add_country)+','+(select state from tbl_master_state where id=add_state)+','+(select city_name from tbl_master_city where city_id=add_city)+','+(select area_name from tbl_master_area where area_id=add_area) from tbl_master_address where add_id in (select top 1 slv_visitplace from tbl_trans_SalesVisitdetail where slv_salesvisitId=tbl_trans_salesvisit.slv_id)) as address," +
                                    "convert(varchar(20),slv_nextvisitdatetime,100) as Nextvisitdate," +
                                    "(LTRIM(SUBSTRING(CONVERT( VARCHAR(20), slv_nextvisitdatetime, 22), 10, 5) + RIGHT(CONVERT(VARCHAR(20), slv_nextvisitdatetime, 22), 3))) as visitTime," +
                                    " (select top 1 convert(varchar(20),slv_visitDatetime,100) from tbl_trans_SalesVisitdetail where slv_salesvisitId=tbl_trans_salesvisit.slv_id) as PrevVisitDate," +
                                    "act_activityNo," +
                                    "(select eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where user_id= tbl_trans_activies.act_assignedto) and eml_type='Office') as email " +
                                    " from  tbl_trans_salesvisit inner join tbl_trans_activies on slv_activityid=act_id" +
                                    " where slv_nextactivityId is null and convert(varchar,slv_nextvisitdatetime,103) = convert(varchar,dateadd(dd,1,getdate()),103)";
            string lsSql_Sales = " select act_id, act_assignedby, act_assignedto,sls_contactlead_id," +
                                "(select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_internalId+']'  from tbl_master_lead where cnt_internalId= sls_contactlead_id)as LeadName," +
                                "(select phf_phoneNumber from tbl_master_phonefax where phf_Type='Office' and phf_cntId=sls_contactlead_id) as phone," +
                                "(select cnt_firstName+' '+isnull(cnt_MiddleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_shortName+']' from tbl_master_contact where cnt_internalId in (select user_contactId from tbl_master_user where user_id = act_assignedto)) as targetUser," +
                //(select isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+','+isnull(add_landmark,'')+','+(select cou_country from tbl_master_country where cou_id=add_country)+','+(select state from tbl_master_state where id=add_state)+','+(select city_name from tbl_master_city where city_id=add_city)+','+(select area_name from tbl_master_area where area_id=add_area) from tbl_master_address where add_id in (select top 1 slv_visitplace from tbl_trans_SalesVisitdetail where slv_salesvisitId=tbl_trans_salesvisit.slv_id)) as address,"+
                                "(select top 1 case sad_nextactivityType when '1' then 'Phone Follow Up' when '2' then 'Meeting' else '' end from tbl_trans_SalesDetails where sad_salesId=tbl_trans_sales.sls_id) as VisitType," +
                                "(select top 1 sad_Notes from tbl_trans_SalesDetails where sad_salesId=tbl_trans_sales.sls_id order by sad_nextvisitdate desc) as note," +
                                "(select top 1 convert(varchar(20),cast(sad_visitdate as datetime),100) from tbl_trans_SalesDetails where sad_salesId=tbl_trans_sales.sls_id order by sad_nextvisitdate desc) as LastVisitDate," +
                                "convert(varchar(20),sls_nextvisitdate,100) as Nextvisitdate," +
                                "(LTRIM(SUBSTRING(CONVERT( VARCHAR(20), sls_nextvisitdate, 22), 10, 5) + RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) as visitTime," +
                                "act_activityNo " +
                                " from tbl_trans_sales inner join tbl_trans_activies on sls_activity_id=act_id" +
                                " where sls_sales_status !='2' and convert(varchar,sls_nextvisitdate,103) = convert(varchar,dateadd(dd,1,getdate()),103)";

            string lsSql_PhoneCall = "select act_id,act_assignedby, act_assignedto,phc_leadcotactId," +
                                    "(select cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_internalId+']'  from tbl_master_lead where cnt_internalId= phc_leadcotactID)as LeadName," +
                                    "(select phf_phoneNumber from tbl_master_phonefax where phf_Type='Office' and phf_cntId=phc_leadcotactId) as phone," +
                                    "(select cnt_firstName+' '+isnull(cnt_MiddleName,'')+' '+isnull(cnt_lastName,'')+'['+cnt_shortName+']' from tbl_master_contact where cnt_internalId in (select user_contactId from tbl_master_user where user_id = act_assignedto)) as targetUser," +
                                    "(select top 1 (select call_dispositions from tbl_master_calldispositions where call_id=tbl_trans_phonecalldetails.phd_calldispose)+'['+phd_note+']' from tbl_trans_phonecalldetails where phd_phonecallId=tbl_trans_phoneCall.phc_id order by phd_nextcall desc) as note," +
                                    "(select top 1 convert(varchar(20),cast(phd_callDate as datetime),100) from tbl_trans_phonecalldetails where phd_phonecallId=tbl_trans_phoneCall.phc_id order by phd_nextcall desc) as LastVisitDate," +
                                    "convert(varchar(20),phc_nextCall,100) as Nextvisitdate," +
                                    "(LTRIM(SUBSTRING(CONVERT( VARCHAR(20), phc_nextCall, 22), 10, 5) + RIGHT(CONVERT(VARCHAR(20), phc_nextCall, 22), 3))) as visitTime," +
                                    " act_activityNo " +
                                    " from tbl_trans_phoneCall inner join tbl_trans_activies on phc_activityid=act_id" +
                                    " where phc_callDispose not in ('9','10') and convert(varchar,phc_nextCall,103) = convert(varchar,dateadd(dd,1,getdate()),103)";
            string lsSql_users = "select distinct act_assignedTo as ID from tbl_trans_salesvisit inner join tbl_trans_activies on slv_activityId=act_id where slv_nextactivityId is null and convert(varchar(10),convert(datetime,slv_nextvisitdatetime,101),101) = convert(varchar(10),dateadd(dd,1,getdate()),101) union all" +
                                " select distinct act_assignedTo as ID from tbl_trans_phonecall inner join tbl_trans_activies on phc_activityId=act_id where phc_nextActivityId is null and convert(varchar(10),convert(datetime,phc_NextCall,101),101)=convert(varchar(10),dateadd(dd,1,getdate()),101) union all" +
                                " select distinct act_assignedTo as ID from tbl_trans_sales inner join tbl_trans_activies on sls_activity_Id=act_id where convert(varchar(10),convert(datetime,sls_nextVisitdate,101),101)=convert(varchar(10),dateadd(dd,1,getdate()),101) and sls_sales_status !='2'";

            //__ creating data tables
            GetConnection();

            DataTable DT_Sales = new DataTable();
            DataTable DT_SalesVisit = new DataTable();
            DataTable DT_Phonecall = new DataTable();
            DataTable DT_users = new DataTable();

            SqlDataAdapter lda = new SqlDataAdapter(lsSql_salesVisit, oSqlConnection);
            lda.Fill(DT_SalesVisit);
            lda = new SqlDataAdapter(lsSql_Sales, oSqlConnection);
            lda.Fill(DT_Sales);
            lda = new SqlDataAdapter(lsSql_PhoneCall, oSqlConnection);
            lda.Fill(DT_Phonecall);
            lda = new SqlDataAdapter(lsSql_users, oSqlConnection);
            lda.Fill(DT_users);

            DT_users = findDistincttable(DT_users, "ID");
            for (int i = 0; i < DT_users.Rows.Count; i++)
            {
                string HTMLdataTble = "<table >";
                HTMLdataTble += "<tr>";
                //___Headers
                HTMLdataTble += "<td  >Sl. No.</td>";
                HTMLdataTble += "<td  >Time</td>";
                HTMLdataTble += "<td  >Task Type</td>";
                HTMLdataTble += "<td  >Lead</td>";
                HTMLdataTble += "<td  >Phone</td>";
                HTMLdataTble += "<td  >Address</td>";
                HTMLdataTble += "<td  >product</td>";
                HTMLdataTble += "<td  >Last Act. date</td>";
                HTMLdataTble += "<td  >Last OutCome</td>";
                HTMLdataTble += "</tr>";

                //______Creating data for SalesVisit!
                string expression = "act_assignedto='" + DT_users.Rows[i]["ID"].ToString().Trim() + "'";
                DataRow[] SalesVisitData = DT_SalesVisit.Select(expression);
                for (int k = 0; k < SalesVisitData.Length; k++)
                {
                    HTMLdataTble += "<tr>";
                    HTMLdataTble += "<td  >" + (k + 1) + "</td>";
                    HTMLdataTble += "<td  >" + SalesVisitData[k][15] + "</td>";
                    HTMLdataTble += "<td  >" + SalesVisitData[k][10] + "</td>";
                    HTMLdataTble += "<td  >" + SalesVisitData[k][11] + "</td>";
                    HTMLdataTble += "<td  >" + SalesVisitData[k][12] + "</td>";
                    HTMLdataTble += "<td  >" + SalesVisitData[k][13] + "</td>";

                    string lsSql_Product = "select (case ofp_productId when '' then ofp_productTypeid when null then ofp_productTypeid else (select prds_description from tbl_master_products where prds_internalId=ofp_productId) end) as product from tbl_trans_offeredProduct where ofp_activityId='" + SalesVisitData[k][17].ToString().Trim() + "'";
                    DataTable DT_product = new DataTable();
                    lda = new SqlDataAdapter(lsSql_Product, oSqlConnection);
                    lda.Fill(DT_product);
                    string product = "";
                    if (DT_product.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT_product.Rows.Count; j++)
                        {
                            if (product == "")
                                product = "(" + (j + 1) + ") " + DT_product.Rows[j]["product"];
                            else
                                product += ", (" + (j + 1) + ") " + DT_product.Rows[j]["product"];
                        }
                    }

                    HTMLdataTble += "<td  >" + product + "</td>";
                    HTMLdataTble += "<td  ></td>";
                    HTMLdataTble += "<td  >" + SalesVisitData[k][8] + "</td>";
                    HTMLdataTble += "</tr>";
                }
                string email_subject = "Daily task Sheet.";
                string email_Header = "Dear " + SalesVisitData[0][6] + " Following is the list of task to be performed by as on " + SalesVisitData[0][14] + ". Please update the outcome of the undementioned task by 24 hour.";
                string email_receipient = SalesVisitData[0][18].ToString();

                //____Creating data fro Sales
                expression = "act_assignedto='" + DT_users.Rows[i]["ID"].ToString().Trim() + "'";
                DataRow[] SalesData = DT_Sales.Select(expression);
                for (int k = 0; k < SalesData.Length; k++)
                {
                    HTMLdataTble += "<tr>";
                    HTMLdataTble += "<td  >" + (k + 1) + "</td>";
                    HTMLdataTble += "<td  >" + SalesData[k][11] + "</td>";
                    HTMLdataTble += "<td  >" + SalesData[k][7] + "</td>";
                    HTMLdataTble += "<td  >" + SalesData[k][4] + "</td>";
                    HTMLdataTble += "<td  >" + SalesData[k][5] + "</td>";
                    HTMLdataTble += "<td  >add</td>";

                    string lsSql_Product = "select (case ofp_productId when '' then ofp_productTypeid when null then ofp_productTypeid else (select prds_description from tbl_master_products where prds_internalId=ofp_productId) end) as product from tbl_trans_offeredProduct where ofp_activityId='" + SalesData[k][12].ToString().Trim() + "'";
                    DataTable DT_product = new DataTable();
                    lda = new SqlDataAdapter(lsSql_Product, oSqlConnection);
                    lda.Fill(DT_product);
                    string product = "";
                    if (DT_product.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT_product.Rows.Count; j++)
                        {
                            if (product == "")
                                product = "(" + (j + 1) + ") " + DT_product.Rows[j]["product"];
                            else
                                product += ", (" + (j + 1) + ") " + DT_product.Rows[j]["product"];
                        }
                    }

                    HTMLdataTble += "<td  >" + product + "</td>";
                    HTMLdataTble += "<td  >date</td>";
                    HTMLdataTble += "<td  >" + SalesData[k][9] + "</td>";
                    HTMLdataTble += "</tr>";
                }
                expression = "act_assignedto='" + DT_users.Rows[i]["ID"].ToString().Trim() + "'";
                DataRow[] PhoneCalldata = DT_Phonecall.Select(expression);
                for (int k = 0; k < PhoneCalldata.Length; k++)
                {
                    HTMLdataTble += "<tr>";
                    HTMLdataTble += "<td  >" + (k + 1) + "</td>";
                    HTMLdataTble += "<td  >" + PhoneCalldata[k][10] + "</td>";
                    HTMLdataTble += "<td  >PhoneCall</td>";
                    HTMLdataTble += "<td  >" + PhoneCalldata[k][4] + "</td>";
                    HTMLdataTble += "<td  >" + PhoneCalldata[k][5] + "</td>";
                    HTMLdataTble += "<td  >add</td>";

                    string lsSql_Product = "select (case ofp_productId when '' then ofp_productTypeid when null then ofp_productTypeid else (select prds_description from tbl_master_products where prds_internalId=ofp_productId) end) as product from tbl_trans_offeredProduct where ofp_activityId='" + PhoneCalldata[k][11].ToString().Trim() + "'";
                    DataTable DT_product = new DataTable();
                    lda = new SqlDataAdapter(lsSql_Product, oSqlConnection);
                    lda.Fill(DT_product);
                    string product = "";
                    if (DT_product.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT_product.Rows.Count; j++)
                        {
                            if (product == "")
                                product = "(" + (j + 1) + ") " + DT_product.Rows[j]["product"];
                            else
                                product += ", (" + (j + 1) + ") " + DT_product.Rows[j]["product"];
                        }
                    }

                    HTMLdataTble += "<td  >" + product + "</td>";
                    HTMLdataTble += "<td  >PrevDate</td>";
                    HTMLdataTble += "<td  >" + PhoneCalldata[k][7] + "</td>";
                    HTMLdataTble += "</tr>";
                }
                HTMLdataTble += "<tr><td>   -: Generated By CRM :- </td></tr>";
                HTMLdataTble += "</table>";
                HTMLdataTble = SepCharectores(HTMLdataTble);

                string lsSql_finalInsert = "insert into tbl_trans_AutoEmails (am_Sub,am_RecipientEmailId,am_Header,am_Body,am_status,CrateDate)" +
                " values('" + email_subject + "','" + email_receipient + "','" + email_Header + "','" + HTMLdataTble + "','N',getdate())";
                SqlCommand lcmd1 = new SqlCommand(lsSql_finalInsert, oSqlConnection);
                Int32 rowsEffected = 0;
                try
                {
                    rowsEffected = lcmd1.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                }
            }

            oSqlConnection.Close();
        }

        //___Find disticntvalue from table
        public DataTable findDistincttable(DataTable tableName, string fieldName)
        {
            DataTable DT_return = new DataTable();
            DataColumn col1 = new DataColumn("ID");
            DT_return.Columns.Add(col1);

            string LocalFieldName = "";
            //___Before filter distinct record i have to short the records of table!
            DataView DT_view = new DataView(tableName);
            DT_view.Sort = "ID DESC";

            foreach (DataRowView view in DT_view)
            {
                if (view["ID"].ToString().Trim() != LocalFieldName.Trim())
                {
                    LocalFieldName = view["ID"].ToString().Trim();
                    DataRow row = DT_return.NewRow();
                    row["ID"] = view[0].ToString().Trim();
                    DT_return.Rows.Add(row);
                }
            }

            return DT_return;
        }

        //____For Testing Only___//
        public string SystemGeneratedMails(string activityID, string actType)
        {
            string message = "";
            if (actType == "Sales")
            {
                string[,] EmailId = GetFieldValue(" tbl_trans_salesvisit inner join tbl_trans_activies on slv_activityid=act_id ",
                                                " slv_id, act_assignedby, act_assignedto,(select user_name from tbl_master_user where user_id= act_assignedby) as assignUser," +
                                                "(select user_name from tbl_master_user where user_id = act_assignedto) as targetUser," +
                                                "(select user_contactId from tbl_master_user where user_id= act_assignedby) as SenderContactId," +
                                                "(select user_contactId from tbl_master_user where user_id= act_assignedto) as TargetContactId",
                                                " slv_id=" + activityID + " and slv_salesvisitoutcome = 9 and getdate() between (convert(varchar(10),act_scheduleddate,101) + ' ' + act_scheduledtime) and (convert(varchar(10),act_expecteddate,101) + ' ' + act_expectedtime)and slv_lastdatevisit is null ", 7);
                string[,] emailsender = GetFieldValue(" tbl_master_email ", " top 1 eml_email ", "eml_cntId ='" + EmailId[0, 5] + "'", 1);
                string[,] emailtarget = GetFieldValue(" tbl_master_email ", " top 1 eml_email ", "eml_cntId ='" + EmailId[0, 6] + "'", 1);
                string[,] messageBody = GetFieldValue(" tbl_master_template ", " tem_msg ", " tem_id=1", 1);
                if (EmailId != null)
                {
                    MailMessage Message = new MailMessage();
                    //Message.From = new MailAddress(emailsender[0, 0].ToString(), EmailId[0, 3].ToString());
                    //Message.To.Add(new MailAddress(emailtarget[0, 0].ToString(), EmailId[0, 4].ToString()));
                    Message.From = new MailAddress("influxcrm@gmail.com", "INFLUX CRM");
                    //Message.To.Add(new MailAddress("bberlia@gmail.com", "Mr. Bharat"));
                    Message.To.Add(new MailAddress("asitbera@gmail.com", "Mr. asit"));
                    Message.Subject = "Sales Activity";
                    Message.IsBodyHtml = true;

                    Message.Body = "<table border='0' + cellpadding='3' cellspacing='3' width= '100%' font-weight= 'normal' font-size= '8pt'  color= 'black' font-style= normal font-family= 'Verdana'>";
                    Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >   -: Activity Information :- </td></tr>";
                    Message.Body += "<tr><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align = 'left' > " + messageBody[0, 0].ToString() + " </td></tr>";
                    Message.Body += "<tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '>   -: Generated By smifsCRM :- </td></tr>";
                    Message.Body += "</table>";

                    SmtpClient client = new SmtpClient("smtp.gmail.com");
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("influxcrm@gmail.com", "influxcrm2009");
                    client.EnableSsl = true;
                    //InsurtFieldValue(" tbl_trans_email ", " hem_receipient = '" + emailsender[0, 0].ToString() + "'", " user_loginId='" + userId + "'");
                    try
                    {
                        client.Send(Message);
                        message = "done";
                    }
                    catch (SmtpException ex)
                    {
                        message = ex.Message;
                    }
                }
            }
            return message;
        }

        public string SendMailByComposer(string TargetIds, string sendermail, DataTable attachmentDetails, string subject, string message, string CC, string BCc, string template)
        {
            string rtn = "";
            MailMessage Message = new MailMessage();
            MailAddressCollection mailAdds = new MailAddressCollection();

            string[] idWlist = TargetIds.Split(',');
            string mailaddress = "";
            string mailname = "";
            for (int i = 0; i < idWlist.Length; i++)
            {
                string[] idWname = idWlist[i].Split('<');
                if (idWname.Length == 1)
                {
                    mailaddress = idWname[0];
                    mailname = "";
                }
                else
                {
                    mailaddress = idWname[1].Substring(0, idWname[1].Length - 1);
                    mailname = idWname[0].Trim();
                }
                Message.To.Add(new MailAddress(mailaddress, mailname));
            }

            Message.From = new MailAddress(sendermail);

            //_____adding CC & BCc email ids____//

            if (CC != "")
            {
                string[] idWlist1 = CC.Split(',');
                string mailaddress1 = "";
                string mailname1 = "";
                for (int i = 0; i < idWlist1.Length; i++)
                {
                    string[] idWname = idWlist1[i].Split('<');
                    if (idWname.Length == 1)
                    {
                        mailaddress1 = idWname[0];
                        mailname1 = "";
                    }
                    else
                    {
                        mailaddress1 = idWname[1].Substring(0, idWname[1].Length - 1);
                        mailname1 = idWname[0].Trim();
                    }
                    Message.CC.Add(new MailAddress(mailaddress1, mailname1));
                }
            }
            if (BCc != "")
            {
                string[] idWlist2 = BCc.Split(',');
                string mailaddress2 = "";
                string mailname2 = "";
                for (int i = 0; i < idWlist2.Length; i++)
                {
                    string[] idWname = idWlist2[i].Split('<');
                    if (idWname.Length == 1)
                    {
                        mailaddress2 = idWname[0];
                        mailname2 = "";
                    }
                    else
                    {
                        mailaddress2 = idWname[1].Substring(0, idWname[1].Length - 1);
                        mailname2 = idWname[0].Trim();
                    }
                    Message.Bcc.Add(new MailAddress(mailaddress2, mailname2));
                }
            }

            //______________End CC &BCc_________//

            Message.Subject = subject;
            if (attachmentDetails != null)
            {
                for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                {
                    Message.Attachments.Add(new Attachment(attachmentDetails.Rows[i]["filepathServer"].ToString().Trim()));
                }
            }

            Message.IsBodyHtml = true;

            Message.Body = "<table border='0' + cellpadding='3' cellspacing='3' width= '100%' font-weight= 'normal' font-size= '8pt'  color= 'black' font-style= normal font-family= 'Verdana'>";
            Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >    </td></tr>";
            Message.Body += "<tr><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align = 'left' > " + message + " </td></tr>";
            Message.Body += "<tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '>   -: Generated By smifsCRM. :- </td></tr>";
            Message.Body += "</table>";

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("influxcrm@gmail.com", "influxcrm2009");
            client.EnableSsl = true;
            //InsurtFieldValue(" tbl_trans_email ", " hem_receipient = '" + emailsender[0, 0].ToString() + "'", " user_loginId='" + userId + "'");
            try
            {
                client.Send(Message);
                rtn = "done";
                //__inter in tbl_trans_email__//
                string tem_id = "";
                string tem_SenderType = "";
                if (template != "")
                {
                    string[] part = template.Split('^');
                    tem_id = part[1];
                    tem_SenderType = part[2];
                }
                int NoOfRowsEffected = InsurtFieldValue(" tbl_trans_email ", " hem_senderemail,hem_mailmsg,hem_mailsub,hem_temid,hem_sendertype,hem_activityid, CreateDate,CreateUser ", "'" + sendermail + "','" + message + "','" + subject + "','" + tem_id + "','" + tem_SenderType + "','','" + DateTime.Now + "', '" + HttpContext.Current.Session["userid"] + "'");
                //_______End____//
                //_______inserting Data in tbl_trans_email_recipient___//
                string[,] lastID = GetFieldValue(" tbl_trans_email ", " top 1 hem_id ", " CreateUser=" + HttpContext.Current.Session["userid"] + " and hem_mailsub='" + subject + "'", 1, " CreateDate desc ");
                if (lastID[0, 0] != "n")
                {
                    EmailIDUpdate(TargetIds, lastID[0, 0], "To");
                    if (CC != "")
                        EmailIDUpdate(CC, lastID[0, 0], "CC");
                    if (BCc != "")
                        EmailIDUpdate(BCc, lastID[0, 0], "Bcc");
                }
                //___________________end___________________________________________________//
                //_______inserting Data in tbl_trans_email_attachments___//
                if (attachmentDetails != null)
                {
                    for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                    {
                        NoOfRowsEffected = InsurtFieldValue(" tbl_trans_email_attachment ", " hem_id,hem_attachment ", lastID[0, 0] + ",'" + attachmentDetails.Rows[i]["filepathServer"].ToString().Trim() + "'");
                    }
                }
            }
            catch (SmtpException ex)
            {
                rtn = ex.Message;
            }
            return rtn;
        }

        public string SendBulkMail(string TemplateIdForRecipientEmailId, DataTable attachmentDetails)
        {
            string rtn = "";
            string[,] data = GetFieldValue(" tbl_master_bulkmail ", " * ", " bem_id=" + TemplateIdForRecipientEmailId, 19);
            string[,] internalids = GetFieldValue(" tbl_trans_emailsubscriptionlist ", " esl_cntid ", " esl_masterid=" + TemplateIdForRecipientEmailId, 1);

            MailMessage Message = new MailMessage();
            MailAddressCollection mailAdds = new MailAddressCollection();

            if (internalids[0, 0] != "n")
            {
                for (int i = 0; i < internalids.Length; i++)
                {
                    string[,] emailidlist = GetFieldValue(" tbl_master_email,tbl_master_contact ", " top 1 eml_email,(cnt_firstname + ' ' + cnt_middlename+' '+cnt_lastname) as name ", " eml_cntId=cnt_internalId and eml_email is not null and eml_email !='' and cnt_internalId='" + internalids[i, 0] + "'", 2, " eml_type desc ");

                    Message.To.Add(new MailAddress(emailidlist[0, 0], emailidlist[0, 1]));
                }
            }
            else
            {
                rtn = "This Template Does not have any subscription!";
                return rtn;
            }

            Message.From = new MailAddress(data[0, 7], data[0, 8]);
            Message.Subject = data[0, 2];
            if (attachmentDetails != null)
            {
                for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                {
                    Message.Attachments.Add(new Attachment(attachmentDetails.Rows[i]["filepathServer"].ToString().Trim()));
                }
            }

            Message.IsBodyHtml = true;
            //Message.Headers.Add("header", data[0, 4].ToString());
            Message.Body = "<table border='0' + cellpadding='3' cellspacing='3' width= '100%' font-weight= 'normal' font-size= '8pt'  color= 'black' font-style= normal font-family= 'Verdana'>";
            Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >" + data[0, 4].ToString() + "</td></tr>";
            Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >    </td></tr>";
            if (data[0, 3] == "0")
            {
                Message.Body += "<tr><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align = 'left' ><textarea id='id' > " + data[0, 5] + " </textarea></td></tr>";
            }
            else
            {
                if (attachmentDetails != null)
                {
                    for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                    {
                        if (attachmentDetails.Rows[i]["bodyFile"].ToString().Trim() == "1")
                        {
                            string pathfile = attachmentDetails.Rows[i]["filepathServer"].ToString().Trim();
                            string extention = Path.GetExtension(pathfile);
                            if (extention == ".txt")
                            {
                                Message.Body = attachmentDetails.Rows[i]["filepathServer"].ToString();
                            }
                            if (extention == ".Doc")
                            {
                                Message.Body = attachmentDetails.Rows[i]["filepathServer"].ToString();
                            }
                        }
                    }
                }
            }
            Message.Body += "<tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '>   -: Generated By smifsCRM. :- </td></tr>";
            Message.Body += "</table>";
            Message.Body += "<br /><br /><table><tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '> " + data[0, 6] + "</td></tr></table>";
            //Message.Body

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("influxcrm@gmail.com", "influxcrm2009");
            client.EnableSsl = true;
            //InsurtFieldValue(" tbl_trans_email ", " hem_receipient = '" + emailsender[0, 0].ToString() + "'", " user_loginId='" + userId + "'");
            try
            {
                if (data[0, 11].ToString() == "0")
                {
                    client.Send(Message);
                    rtn = "done";
                    //___ inter in tbl_trans_BulkEmail __//
                    //___ status: Y--> Sebded, N--> Send At perticular Time, C--> Cancel The mail

                    int NoOfRowsEffected = InsurtFieldValue(" tbl_trans_BulkEmail ", " be_BulkTemplateId,be_status,be_send_createTime,be_send_createUser ", "'" + TemplateIdForRecipientEmailId + "','Y','" + System.DateTime.Now + "','" + HttpContext.Current.Session["userid"] + "'");
                    //_______End____//

                    //_______inserting Data in tbl_trans_email_attachments___//
                    string[,] lastID = GetFieldValue(" tbl_trans_BulkEmail ", " top 1 be_Id ", " be_send_createUser=" + HttpContext.Current.Session["userid"] + " and be_BulkTemplateId='" + TemplateIdForRecipientEmailId + "'", 1, " be_send_createTime desc ");
                    if (lastID[0, 0] != "n")
                    {
                        if (attachmentDetails != null)
                        {
                            for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                            {
                                NoOfRowsEffected = InsurtFieldValue(" tbl_trans_BulkAttachment ", " be_id,be_fileAttachment ", lastID[0, 0] + ",'" + attachmentDetails.Rows[i]["filepathServer"].ToString().Trim() + "'");
                            }
                        }
                    }
                }
                else
                {
                    int NoOfRowsEffected = InsurtFieldValue(" tbl_trans_BulkEmail ", " be_BulkTemplateId,be_status,be_send_createTime,be_send_createUser ", "'" + TemplateIdForRecipientEmailId + "','N','" + System.DateTime.Now + "','" + HttpContext.Current.Session["userid"] + "'");

                    //_______inserting Data in tbl_trans_email_attachments___//
                    string[,] lastID = GetFieldValue(" tbl_trans_BulkEmail ", " top 1 be_Id ", " be_send_createUser=" + HttpContext.Current.Session["userid"] + " and be_BulkTemplateId='" + TemplateIdForRecipientEmailId + "'", 1, " be_send_createTime desc ");
                    if (lastID[0, 0] != "n")
                    {
                        if (attachmentDetails != null)
                        {
                            for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                            {
                                NoOfRowsEffected = InsurtFieldValue(" tbl_trans_BulkAttachment ", " be_id,be_fileAttachment ", lastID[0, 0] + ",'" + attachmentDetails.Rows[i]["filepathServer"].ToString().Trim() + "'");
                            }
                        }
                    }
                }
            }
            catch (SmtpException ex)
            {
                rtn = ex.Message;
            }
            return rtn;
        }

        private void EmailIDUpdate(string TargetIds, string lastID, string type) //______inserting ID w.r.t. Email Sended
        {
            string[] idWlist = TargetIds.Split(',');
            string mailID = "";
            for (int i = 0; i < idWlist.Length; i++)
            {
                string[] idWname = idWlist[i].Split('<');
                if (idWname.Length == 1)
                    mailID = idWname[0];
                else
                    mailID = idWname[1].Substring(0, idWname[1].Length - 1);
                int NoOfRowsEffected = InsurtFieldValue(" tbl_trans_email_recipient ", " hem_id,hem_recipient,hem_type ", lastID + ",'" + mailID + "','" + type + "'");
            }
        }

        // This table update ANY table field When called!
        // Example Usage :
        //
        public Int32 SetFieldValue(
                String cTableName,      // TableName to which the field value is to be updated
                String cFieldName,      // Name of the field and values whose data needs to be upadted
                String cWhereClause    // WHERE condition
                )
        {
            cFieldName = SepCharectores(cFieldName);
            if (cWhereClause != null)
            {
                String lSsql = "Update " + cTableName + " SET " + cFieldName + " Where " + cWhereClause;
                //SqlConnection oSqlConnection = GetConnection();
                GetConnection();
                SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
                Int32 rowsEffected = 0;
                try
                {
                    rowsEffected = lcmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    // For Read Only User Error Notification Start-----------------------------------------

                    if (e.Message.Contains("The INSERT permission was denied on the object"))
                    {
                        oSqlConnection.Close();
                        if (HttpContext.Current != null)
                        {

                            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                        }

                    }


                    else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                    {
                        oSqlConnection.Close();
                        if (HttpContext.Current != null)
                        {

                            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                        }

                    }

                    else if (e.Message.Contains("The DELETE permission was denied on the object"))
                    {
                        oSqlConnection.Close();
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                        }

                    }

                    // For Read Only User Error Notification End-----------------------------------------

                    oSqlConnection.Close();
                    return rowsEffected;
                }
                oSqlConnection.Close();
                return rowsEffected;
            }
            return 0;
        }

        // this will insert Any Data to any table
        //
        public Int32 InsurtFieldValue(
                    String cTableName,      // TableName to which the field value is to be Insert
                    String cFieldName,      // Name of the field and values whose data needs to be Insert
                    String cFieldValue     // Value of fields
                    )
        {
            cFieldValue = SepCharectores(cFieldValue);
            String lSsql = "INSERT INTO " + cTableName + "(" + cFieldName + ")" + "values(" + cFieldValue + ")";
            GetConnection();
            SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
            Int32 rowsEffected = 0;
            try
            {
                rowsEffected = lcmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


                oSqlConnection.Close();
                return rowsEffected;
            }
            oSqlConnection.Close();
            return rowsEffected;
        }

        //__insert data from one table to another Table
        public Int32 InsertDataFromAnotherTable(
                    string TargetTable,     //__table name in which data is getting inserted
                    string SourceTable,     //__Table from which data is beeing taken
                    string fieldsName,      //__ fields name from source table
                    string wherecondition   //__On which condition data is beeing inserted
            )
        {
            string lssql = " INSERT INTO " + TargetTable + " select " + fieldsName + " from " + SourceTable;
            if (wherecondition != null)
                lssql += " where " + wherecondition;
            GetConnection();
            SqlCommand lcmd = new SqlCommand(lssql, oSqlConnection);
            Int32 rowsEffected = 0;
            try
            {
                rowsEffected = lcmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


                oSqlConnection.Close();
                return rowsEffected;
            }
            oSqlConnection.Close();
            return rowsEffected;
        }

        //Insurt data in one table by specifying field name and value from another table
        public Int32 InsertDataFromAnotherTable(
                    string TargetTable,     //__table name in which data is getting inserted
                    string TargetFilds,           //__Target table Fields Name
                    string SourceTable,     //__Table from which data is beeing taken
                    string fieldsName,      //__ fields name from source table
                    string wherecondition   //__On which condition data is beeing inserted
            )
        {
            string lssql = " INSERT INTO " + TargetTable + " (" + TargetFilds + ") " + " select " + fieldsName;
            if (SourceTable != null)
                lssql += " from " + SourceTable;
            if (wherecondition != null)
                lssql += " where " + wherecondition;
            GetConnection();
            SqlCommand lcmd = new SqlCommand(lssql, oSqlConnection);
            Int32 rowsEffected = 0;
            try
            {
                rowsEffected = lcmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


                oSqlConnection.Close();
                return rowsEffected;
            }
            oSqlConnection.Close();
            return rowsEffected;
        }

        //------------------------------------------------------------------------------------//
        // This method returns the value of ANY field from ANY table in the database.
        // Example Usage :
        //
        //------------------------------------------------------------------------------------//
        public string[,] GetFieldValue(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,      // The name of the field whose value needs to be returned
                String cWhereClause,    // Optional : WHERE condition [if any]
                int NoField)            // Number of field value to selection
        {
            // Return Value
            string[,] vRetVal;
            String lcSql;
            // Count how many rows of data is there?
            // according to that we can define array lenth!           
            lcSql = "Select Count(*) from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            SqlDataReader lsdr;
            lsdr = proc.GetReader(lcSql);
            int CountNoRow = 0;
            while (lsdr.Read())
            {
                CountNoRow = (int)lsdr[0];
            }
            lsdr.Close();

            if (CountNoRow > 0)
            {
                //Now define length of an array
                vRetVal = new string[CountNoRow, NoField];

                // Now we construct a SQL command that will fetch the field from the table
                lcSql = "Select " + cFieldName + " from " + cTableName;
                if (cWhereClause != null)
                {
                    lcSql += " WHERE " + " " + cWhereClause;
                }


                lsdr = proc.GetReader(lcSql);

                if (lsdr.HasRows)
                {

                    int i = 0;
                    while (lsdr.Read())
                    {
                        for (int j = 0; j < NoField; j++)
                        {
                            vRetVal[i, j] = lsdr[j].ToString();

                        }
                        i = i + 1;
                    }
                }
                else
                {
                    string[,] messg = new string[1, 1];
                    messg[0, 0] = "n";
                    // We close the DataReader
                    lsdr.Close();

                    return messg;
                }
            }
            else
            {
                string[,] messg = new string[1, 1];
                messg[0, 0] = "n";
                // We close the DataReader
                lsdr.Close();

                return messg;
            }

            // We close the DataReader
            lsdr.Close();
            proc.CloseConnection();

            return vRetVal;
        }

        public string[,] GetFieldValue(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,      // The name of the field whose value needs to be returned
                String cWhereClause,    // Optional : WHERE condition [if any]
                int NoField,             // Number of field value to selection
                string cOrderbyName)    // Order by field
        {
            // Return Value
            string[,] vRetVal;
            String lcSql;
            // Count how many rows of data is there?
            // according to that we can define array lenth!


            lcSql = "Select Count(*) from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            SqlDataReader lsdr;
            lsdr = proc.GetReader(lcSql);
            int CountNoRow = 0;
            if (lsdr.HasRows)
            {
                while (lsdr.Read())
                {
                    CountNoRow = (int)lsdr[0];
                }
                lsdr.Close();
            }
            else
            {
                lsdr.Close();
            }
            //Now define length of an array
            vRetVal = new string[CountNoRow, NoField];

            // Now we construct a SQL command that will fetch the field from the table
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (cOrderbyName != null)
            {
                lcSql += " Order By " + cOrderbyName;
            }

            lsdr = proc.GetReader(lcSql);

            if (lsdr.HasRows)
            {

                int i = 0;
                while (lsdr.Read())
                {
                    for (int j = 0; j < NoField; j++)
                    {
                        vRetVal[i, j] = lsdr[j].ToString();

                    }
                    i = i + 1;
                }
            }
            else
            {
                string[,] messg = new string[1, 1];
                messg[0, 0] = "n";
                lsdr.Close();

                return messg;
            }

            // We close the DataReader
            lsdr.Close();
            return vRetVal;
        }

        //__This function help to enter ',= thease charectore in database as a string content
        // Example usage:
        //
        public string SepCharectores(string stringitem)
        {
            string rtn = "";
            for (int i = 0; i < stringitem.Length; i++)
            {
                if (stringitem[i].ToString() == "'")
                {
                    if (i != 0 && i != stringitem.Length - 1)
                    {
                        if (stringitem[i + 1].ToString() == "," || stringitem[i - 1].ToString() == "," || stringitem[i + 1].ToString() == "=" || stringitem[i - 1].ToString() == "=" || stringitem[i + 1].ToString() == "'" || stringitem[i - 1].ToString() == "'" || stringitem[i + 1].ToString() == " " || stringitem[i - 1].ToString() == " ")
                        {
                            rtn += stringitem[i];
                        }
                        else
                            rtn += stringitem[i] + "'";
                    }
                    else
                        rtn += stringitem[i];
                }
                else
                    rtn += stringitem[i];
            }
            return rtn;
        }

        #region
        // /*  For Drop Down Use clsDropDownList In Appcode------------------------------------

        //This function is ude to add ANY noumber of Items to a data holder
        // Example Usage:
        //
        //public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox)
        //{
        //    ComboBox.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);             
        //        for (int i = 0; i < length1; i++)
        //        {
        //            ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));                   
        //        }
        //    }
        //}

        //public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox,
        //                                    Boolean ZeroPositionItem //__pass true/false if you want to place a 'Select' text with null value at start___//
        //                                 )
        //{
        //    ComboBox.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);
        //        if (ZeroPositionItem)
        //        {
        //            string[,] item = new string[1, 2];
        //            item[0, 0] = "Select";
        //            item[0, 1] = string.Empty;
        //            ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
        //        }
        //        for (int i = 0; i < length1; i++)
        //        {
        //            ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));                   
        //        }
        //    }
        //}

        //public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox,
        //                                    string ZeroPosition_All //__pass true/false if you want to place a 'All' text with 'All' value at start___//
        //                                 )
        //{
        //    ComboBox.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);
        //        if (ZeroPosition_All == "All")
        //        {
        //            string[,] item = new string[1, 2];
        //            item[0, 0] = "All";
        //            item[0, 1] = "All";
        //            ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
        //        }
        //        for (int i = 0; i < length1; i++)
        //        {
        //            ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));                    
        //        }
        //    }
        //}

        //public void AddDataToDropDownListToAspx(string[,] listItems, ASPxComboBox ComboBox1,
        //                                    Boolean ZeroPositionItem //__pass true/false if you want to place a 'Select' text with null value at start___//
        //                                 )
        //{
        //    ComboBox1.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);
        //        if (ZeroPositionItem)
        //        {
        //            string[,] item = new string[1, 2];
        //            item[0, 0] = "Select";
        //            item[0, 1] = string.Empty;
        //            ComboBox1.Items.Add(new ListEditItem(item[0, 0], item[0, 1]));
        //        }
        //        for (int i = 0; i < length1; i++)
        //        {
        //            ComboBox1.Items.Add(new ListEditItem(listItems[i, 1], listItems[i, 0]));
        //        }
        //    }
        //}

        //public void AddDataToDropDownList(string[,] listItems, ASPxComboBox ComboBox,
        //                                    string ZeroPosition_All //__pass true/false if you want to place a 'All' text with 'All' value at start___//
        //                                 )
        //{
        //    ComboBox.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);
        //        if (ZeroPosition_All == "All")
        //        {
        //            string[,] item = new string[1, 2];
        //            item[0, 0] = "All";
        //            item[0, 1] = "All";
        //            ComboBox.Items.Add(new ListEditItem(item[0, 0], item[0, 1]));
        //        }
        //        for (int i = 0; i < length1; i++)
        //        {
        //            ComboBox.Items.Add(new ListEditItem(listItems[i, 1], listItems[i, 0]));
        //        }
        //    }
        //}

        //public void AddDataToListBox(string[,] listItems, ListBox ListBx)
        //{
        //    ListBx.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);

        //        for (int i = 0; i < length1; i++)
        //        {
        //            ListBx.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));

        //        }
        //    }
        //}

        //public void AddDataToListBox(string[,] listItems, ListBox ListBx, string All_Selection)
        //{
        //    ListBx.Items.Clear();
        //    if (listItems[0, 0] != "n")
        //    {
        //        int length1 = listItems.GetLength(0);
        //        int lenght2 = listItems.GetLength(1);
        //        if (All_Selection == "All")
        //        {
        //            ListBx.Items.Add(new ListItem("All", "All"));
        //        }
        //        for (int i = 0; i < length1; i++)
        //        {
        //            ListBx.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));

        //        }
        //    }
        //}

        // Function Overloading for dropDown List----//
        //public void AddDataToDropDownList(string[,] listItems, DropDownList ComboBox, int SelectedValue)
        //{
        //    ComboBox.Items.Clear();
        //    int length1 = listItems.GetLength(0);
        //    int lenght2 = listItems.GetLength(1);
        //    //string[,] item = new string[1, 2];
        //    //item[0, 0] = "Select";
        //    //item[0, 1] = string.Empty;
        //    //ComboBox.Items.Add(new ListItem(item[0, 0], item[0, 1]));
        //    for (int i = 0; i < length1; i++)
        //    {
        //        if (Int32.Parse(listItems[i, 0].ToString()) == SelectedValue)
        //        {
        //            ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
        //            ComboBox.SelectedValue = listItems[i, 0].ToString();
        //        }
        //        else
        //        {
        //            ComboBox.Items.Add(new ListItem(listItems[i, 1], listItems[i, 0]));
        //        }
        //        //ComboBox.DataTextField =
        //    }
        //    //ComboBox.SelectedIndex = indexno-1;
        //}

        //  */
        #endregion
        public void PopulateMenu(Menu MenuID, string SegmentNumber)
        {
            MenuID.Items.Clear();

            //_______taking valid menue from tbl_trans_access _____//

            DataTable GroupIds = GetDataTable(" tbl_master_usergroup ", " grp_id ", " grp_segmentId=" + SegmentNumber + " and grp_id IN (" + HttpContext.Current.Session["usergoup"] + ")");

            string groupIDlist = "";
            string menuIDlist = "";
            if (GroupIds.Rows.Count > 0)
            {
                for (int i = 0; i < GroupIds.Rows.Count; i++)
                {
                    if (groupIDlist != "")
                        groupIDlist += "," + GroupIds.Rows[i][0].ToString();
                    else
                        groupIDlist = GroupIds.Rows[i][0].ToString();
                }
            }
            if (groupIDlist != "")
            {
                DataTable menuID;
                if (HttpContext.Current.Session["userlastsegment"] != SegmentNumber)
                {
                    menuID = GetDataTable(" tbl_trans_access ", " Distinct acc_menuId ", " acc_userGroupId in (" + groupIDlist + ")");

                    HttpContext.Current.Session["userlastsegment"] = SegmentNumber;
                    //__this will use in page validation
                    getAccessPages();
                }
                else
                {
                    menuID = GetDataTable(" tbl_trans_access ", " Distinct acc_menuId ", " acc_userGroupId in (" + groupIDlist + ")");
                }
                for (int i = 0; i < menuID.Rows.Count; i++)
                {
                    if (menuIDlist != "")
                        menuIDlist += "," + menuID.Rows[i][0].ToString();
                    else
                        menuIDlist = menuID.Rows[i][0].ToString();
                }
            }
            //_____________________________________________________//
            // creating a datatable and taking all values from session table!
            DataTable MenuDataTable = (DataTable)HttpContext.Current.Session["DataTable_Menu"];

            // now creating table row!
            DataRow[] DatarowHead;// = new DataRow();
            // Now expression to filterout data from table!
            String Expression = "mnu_segmentId =" + SegmentNumber + " and mun_parentId =0 and mnu_id in (" + menuIDlist + ")";
            //__adding home page____//
            MenuItem menuHome;
            menuHome = createmenuitem("Home", "Welcome.aspx", "");
            MenuID.Items.Add(menuHome);
            //______________________//
            //Now taking Data to DataRowHead!
            DatarowHead = MenuDataTable.Select(Expression);

            for (int i = 0; i < DatarowHead.Length; i++)
            {
                // creating menu Head : horizontal
                MenuItem MenuHead;
                MenuHead = createmenuitem((string)DatarowHead[i][1], (string)DatarowHead[i][2], (string)DatarowHead[i][5]);

                // Creating SumMenu : Optional

                //Creating Expression to Filter sub menu!
                String ExpressionForSubmenu = "mnu_segmentId =" + SegmentNumber + " and mun_parentId =" + DatarowHead[i][0] + " and mnu_id in (" + menuIDlist + ")";
                DataRow[] SubMenuRow;// = new DataRow();
                SubMenuRow = MenuDataTable.Select(ExpressionForSubmenu);

                if (SubMenuRow.Length != null)
                {
                    for (int j = 0; j < SubMenuRow.Length; j++)
                    {
                        MenuItem SubMenu;
                        SubMenu = createmenuitem((string)SubMenuRow[j][1], (string)SubMenuRow[j][2], (string)SubMenuRow[j][5]);

                        //Creating SubMenu2 of SubMenu here : optional
                        //Creating Expression to Filter submenu of SubMenu!
                        String ExpressionForSubSubmenu = "mnu_segmentId =" + SegmentNumber + " and mun_parentId =" + SubMenuRow[j][0] + " and mnu_id in (" + menuIDlist + ")";
                        DataRow[] SubSubMenuRow;// = new DataRow();
                        SubSubMenuRow = MenuDataTable.Select(ExpressionForSubSubmenu);
                        if (SubSubMenuRow.Length != null)
                        {
                            for (int k = 0; k < SubSubMenuRow.Length; k++)
                            {
                                MenuItem SubMenu2;
                                SubMenu2 = createmenuitem((string)SubSubMenuRow[k][1], (string)SubSubMenuRow[k][2], (string)SubSubMenuRow[k][5]);
                                // Adding submenu2 to SubMenu
                                SubMenu.ChildItems.Add(SubMenu2);
                            }
                        }
                        //lsdr2.Close();
                        // Adding submenu to Head Menu

                        MenuHead.ChildItems.Add(SubMenu);
                    }
                }

                // Adding MenuHead to the Menu to be display!
                MenuID.Items.Add(MenuHead);
                //lsdr1.Close();
            }
            //lsdr.Close();
        }


        public void PopulateAllMenu(Menu MenuID)
        {

            MenuID.Items.Clear();

            List<string> MenuList = new List<string>();
            //_______taking valid menue from tbl_trans_access _____//

            DataTable GroupIds = GetDataTable(" tbl_master_usergroup ", " grp_id ", "grp_id IN (" + HttpContext.Current.Session["usergoup"] + ")");

            string groupIDlist = "";
            string menuIDlist = "";
            if (GroupIds.Rows.Count > 0)
            {
                for (int i = 0; i < GroupIds.Rows.Count; i++)
                {
                    if (groupIDlist != "")
                        groupIDlist += "," + GroupIds.Rows[i][0].ToString();
                    else
                        groupIDlist = GroupIds.Rows[i][0].ToString();
                }
            }
            if (groupIDlist != "")
            {
                DataTable menuID;
                //if (HttpContext.Current.Session["userlastsegment"] != SegmentNumber)
                //{
                //    menuID = GetDataTable(" tbl_trans_access ", " Distinct acc_menuId ", " acc_userGroupId in (" + groupIDlist + ")");

                //    HttpContext.Current.Session["userlastsegment"] = SegmentNumber;
                //    //__this will use in page validation
                //    getAccessPages();
                //}
                //else
                //{
                menuID = GetDataTable(" tbl_trans_access ", " Distinct acc_menuId ", " acc_userGroupId in (" + groupIDlist + ")");
                // }
                for (int i = 0; i < menuID.Rows.Count; i++)
                {
                    if (menuIDlist != "")
                        menuIDlist += "," + menuID.Rows[i][0].ToString();
                    else
                        menuIDlist = menuID.Rows[i][0].ToString();
                }
            }
            //_____________________________________________________//
            // creating a datatable and taking all values from session table!
            DataTable MenuDataTable = (DataTable)HttpContext.Current.Session["DataTable_Menu"];

            // now creating table row!
            DataRow[] DatarowHead;// = new DataRow();
            // Now expression to filterout data from table!
            String Expression = "mun_parentId =0 and mnu_id in (" + menuIDlist + ")";
            //__adding home page____//
            MenuItem menuHome;
            menuHome = createmenuitem("Home", "Welcome.aspx", "");
            MenuID.Items.Add(menuHome);
            //______________________//
            //Now taking Data to DataRowHead!
            DatarowHead = MenuDataTable.Select(Expression);

            for (int i = 0; i < DatarowHead.Length; i++)
            {
                // creating menu Head : horizontal
                MenuItem MenuHead;
                MenuHead = createmenuitem((string)DatarowHead[i][1], (string)DatarowHead[i][2], (string)DatarowHead[i][5]);

                // Creating SumMenu : Optional

                //Creating Expression to Filter sub menu!
                String ExpressionForSubmenu = " mun_parentId =" + DatarowHead[i][0] + " and mnu_id in (" + menuIDlist + ")";
                DataRow[] SubMenuRow;// = new DataRow();
                SubMenuRow = MenuDataTable.Select(ExpressionForSubmenu);

                if (SubMenuRow.Length != null)
                {
                    for (int j = 0; j < SubMenuRow.Length; j++)
                    {
                        MenuItem SubMenu;
                        SubMenu = createmenuitem((string)SubMenuRow[j][1], (string)SubMenuRow[j][2], (string)SubMenuRow[j][5]);

                        //Creating SubMenu2 of SubMenu here : optional
                        //Creating Expression to Filter submenu of SubMenu!
                        String ExpressionForSubSubmenu = " mun_parentId =" + SubMenuRow[j][0] + " and mnu_id in (" + menuIDlist + ")";
                        DataRow[] SubSubMenuRow;// = new DataRow();
                        SubSubMenuRow = MenuDataTable.Select(ExpressionForSubSubmenu);
                        if (SubSubMenuRow.Length != null)
                        {
                            for (int k = 0; k < SubSubMenuRow.Length; k++)
                            {
                                MenuItem SubMenu2;
                                SubMenu2 = createmenuitem((string)SubSubMenuRow[k][1], (string)SubSubMenuRow[k][2], (string)SubSubMenuRow[k][5]);
                                // Adding submenu2 to SubMenu
                                SubMenu.ChildItems.Add(SubMenu2);
                            }
                        }
                        //lsdr2.Close();
                        // Adding submenu to Head Menu

                        MenuHead.ChildItems.Add(SubMenu);
                    }
                }

                // Adding MenuHead to the Menu to be display!
                MenuID.Items.Add(MenuHead);
                //lsdr1.Close();
            }
            //lsdr.Close();
        }
        public MenuItem createmenuitem(string TEXT, string URL, string ImageURL)
        {
            // Creating a new meniItem Object
            MenuItem MenuItem = new MenuItem();

            // Setting parameters to the menuitem
            MenuItem.Text = "<i class='fa fa-user-plus' data-toggle='tooltip' title='' data-original-title='eRecruitment'></i><span class='navfltext'>" + TEXT + "</span>";
            MenuItem.NavigateUrl = URL;
            //MenuItem.NavigateUrl = URL;
            //MenuItem.
            //MenuItem.ImageUrl = "../images/" + ImageURL;
            return MenuItem;
        }

        //Populating Add Field For various tables
        public int populateFeild(Table TableID, string TableName)
        {
            string[,] TableAndSectionIDs = GetFieldValue("tbl_section", "sec_id, sec_table_id", "section_table_name = '" + TableName + "'", 2);
            string lsSql = "Select * from tbl_master_section where sec_id=" + TableAndSectionIDs[0, 0] + "and table_id=" + TableAndSectionIDs[0, 1];
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlCommand lcmd = new SqlCommand(lsSql, oSqlConnection);
            SqlDataReader lsdr = lcmd.ExecuteReader();
            int i = 0;
            while (lsdr.Read())
            {
                TableRow TblRow = new TableRow();

                if ((string)lsdr[5] == "TextBox")
                {
                    TableCell TblCell = new TableCell();
                    TextBox txtbox1 = new TextBox();
                    txtbox1.ID = "text" + i.ToString();
                    Label LblName = new Label();
                    LblName.Text = lsdr[2].ToString();
                    TblCell.Controls.Add(LblName);
                    TblRow.Cells.Add(TblCell);
                    TableCell TblCell1 = new TableCell();
                    TblCell1.Controls.Add(txtbox1);
                    TblRow.Cells.Add(TblCell1);
                }

                TableID.Rows.Add(TblRow);
                /*    if (lsdr[5].ToString = "DropDownList")
                    {
                    } */

                i = i + 1;
            }
            lsdr.Close();
            oSqlConnection.Close();
            return (i - 1);
        }

        #region To get UserList for Lead

        public string getChildUser_for_report(string UserId, string ListOfUser)
        {
            DataTable DtFirst = GetDataTable("tbl_master_employee, tbl_master_user", "emp_id,emp_contactid", "tbl_master_user.user_id =" + UserId + " and tbl_master_employee.emp_contactId = tbl_master_user.user_contactId");
            if (DtFirst.Rows.Count != 0)
            {
                ListOfUser = getUser(DtFirst.Rows[0][0].ToString(), ListOfUser);
                actual = "";//________making global variable null___
            }
            if (ListOfUser != "")
            {
                int lastno = ListOfUser.LastIndexOf(',');
                string listOfUser1 = ListOfUser.Substring(0, lastno);
                ListOfUser = "";
                DataTable DtThird = GetDataTable("tbl_master_employee, tbl_master_user", "tbl_master_user.user_id", "tbl_master_employee.emp_id in (" + listOfUser1 + ") and tbl_master_employee.emp_contactId = tbl_master_user.user_contactId");
                if (DtThird.Rows.Count != 0)
                {
                    for (int i = 0; i < DtThird.Rows.Count; i++)
                    {
                        ListOfUser += DtThird.Rows[i][0].ToString() + ",";
                    }
                }
            }
            return ListOfUser;
        }

        public string getChildUserNotColleague(string UserId, string temp)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "emp_id,emp_contactid", " user_id = '" + UserId + "'");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp);
                cntid = dt.Rows[0][1].ToString();
                actual = "";//________making global variable null___
            }
            if (temp != "")
            {
                string AllUser1 = "";
                string AllUser2 = "";
                string[] st = temp.Split(',');
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    AllUser1 += "," + "'" + st[i] + "'";
                    int ii = AllUser1.LastIndexOf(",,");
                    AllUser2 = AllUser1.Substring(ii + 2);
                }
                int ii1 = AllUser2.LastIndexOf(",");
                temp = AllUser2.Substring(0, ii1);

                DataTable dt_temp = new DataTable();
                dt_temp = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "tbl_master_user.user_id", " tbl_master_employee.emp_id in (" + temp + ")");
                if (dt_temp.Rows.Count != 0)
                {
                    temp = "";
                    for (int j = 0; j < dt_temp.Rows.Count; j++)
                    {
                        temp += dt_temp.Rows[j][0].ToString() + ",";
                    }
                }
                else
                {
                    temp += ",";
                }
            }
            temp += UserId;
            return temp;
        }

        public string getChildInternalIdNotColleague(string InternalId, string temp)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            dt = GetDataTable("tbl_master_employee ", "emp_id,emp_contactid", " emp_contactid = '" + InternalId + "'");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp);
                cntid = dt.Rows[0][0].ToString();
                actual = "";//________making global variable null___
            }
            //returns employee id emp_id
            temp += cntid;
            return temp;
        }

        public string getChildUserNotColleagueEmp(string UserId, string temp)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "emp_id,emp_contactid", " user_id = '" + UserId + "'");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp);
                cntid = dt.Rows[0][1].ToString();
                actual = "";//________making global variable null___
            }
            if (temp != "")
            {
                string[] userlist = temp.Split(',');
                temp = "";
                for (int i = 0; i < userlist.Length - 1; i++)
                {
                    DataTable Dt_temp = GetDataTable(" tbl_master_employee LEFT OUTER JOIN   tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId ", " tbl_master_user.user_id ", " tbl_master_employee.emp_id =" + userlist[i].Trim());
                    if (Dt_temp.Rows.Count > 0)
                    {
                        for (int j = 0; j < Dt_temp.Rows.Count; j++)
                        {
                            temp += Dt_temp.Rows[j][0].ToString() + ",";
                        }
                    }
                }
            }
            temp += UserId;
            return temp;
        }

        public string getChildUserNotColleagueEmp(string UserId, string temp, string BranchId, string company)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            //if(BranchId !="All" ||company !="All")

            dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId inner join tbl_trans_employeeCTC on tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId ", "tbl_master_employee.emp_id,emp_contactid,emp_branch", " user_id = '" + UserId + "' and emp_branch= '" + BranchId + "' and tbl_trans_employeeCTC.emp_effectiveuntil is null");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp);
                cntid = dt.Rows[0][1].ToString();
                actual = "";//________making global variable null___
            }
            if (temp != "")
            {
                string[] userlist = temp.Split(',');
                temp = "";
                for (int i = 0; i < userlist.Length - 1; i++)
                {
                    DataTable Dt_temp = GetDataTable(" tbl_master_employee LEFT OUTER JOIN   tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId ", " tbl_master_user.user_id ", " tbl_master_employee.emp_id =" + userlist[i].Trim());
                    if (Dt_temp.Rows.Count > 0)
                    {
                        for (int j = 0; j < Dt_temp.Rows.Count; j++)
                        {
                            temp += Dt_temp.Rows[j][0].ToString() + ",";
                        }
                    }
                }
            }
            temp += UserId;
            return temp;
        }

        public string getChildUserNotColleague(string UserId, string temp, Boolean s)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "emp_id,emp_contactid", " user_id = '" + UserId + "'");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp);
                cntid = dt.Rows[0][1].ToString();
                actual = "";//________making global variable null___
            }
            if (temp != "")
            {
                string AllUser1 = "";
                string AllUser2 = "";
                string[] st = temp.Split(',');
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    AllUser1 += "," + "'" + st[i] + "'";
                    int ii = AllUser1.LastIndexOf(",,");
                    AllUser2 = AllUser1.Substring(ii + 2);
                }
                int ii1 = AllUser2.LastIndexOf(",");
                temp = AllUser2.Substring(0, ii1);

                DataTable dt_temp = new DataTable();
                dt_temp = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "tbl_master_user.user_id", " tbl_master_employee.emp_id in (" + temp + ")");
                if (dt_temp.Rows.Count != 0)
                {
                    temp = "";
                    for (int j = 0; j < dt_temp.Rows.Count; j++)
                    {
                        temp += dt_temp.Rows[j][0].ToString() + ",";
                    }
                }
                else
                {
                    temp += ",";
                }
            }
            if (s == true)
            {
                DataTable DT1 = new DataTable();
                DT1 = GetDataTable("tbl_trans_employeeCTC", "emp_colleague", " emp_cntId='" + cntid + "'");
                if (DT1.Rows.Count != 0)
                {
                    temp += DT1.Rows[0][0].ToString() + ",";
                }
            }
            temp += UserId;
            return temp;
        }

        public string getChildUser(string UserId, string temp)
        {
            DataSet _dt = new DataSet();
            string cntid = "";
            _dt = PopulateData(" emp_id,emp_contactid ", " tbl_master_employee , tbl_master_user", "tbl_master_employee.emp_contactId = tbl_master_user.user_contactId and user_id = '" + UserId + "'");
            if (_dt.Tables["TableName"].Rows.Count > 0)
            {
                temp = getUser(_dt.Tables["TableName"].Rows[0][0].ToString(), temp);
                cntid = _dt.Tables["TableName"].Rows[0][1].ToString();
                actual = "";//________making global variable null___
            }
            DataSet _dt_col = new DataSet();

            _dt_col = PopulateData("emp_colleague", "tbl_trans_employeeCTC", "emp_cntId='" + cntid + "'");
            if (_dt_col.Tables["TableName"].Rows.Count > 0)
                temp += _dt_col.Tables["TableName"].Rows[0][0].ToString() + ",";
            if (temp != "")
            {
                string[] str = temp.Split(',');
                temp = "";
                for (int i = 0; i < str.Length - 1; i++)
                {
                    DataSet _dt_temp = new DataSet();
                    int EmpId = Convert.ToInt32(str[i].ToString());
                    _dt_temp = PopulateData("tbl_master_user.user_id", "tbl_master_employee , tbl_master_user", "tbl_master_employee.emp_id ='" + EmpId + "' And tbl_master_employee.emp_contactId = tbl_master_user.user_contactId");
                    if (_dt_temp.Tables["TableName"].Rows.Count > 0)
                    {
                        temp += _dt_temp.Tables["TableName"].Rows[0][0] + ",";
                    }
                }
            }
            temp += UserId;
            return temp;
        }

        //This function is taking userID AND WILL RETURN all employee(emp_id) Lower in hierarchy tree
        //using single connection to fetch all the ids! must follow in all above functions!!
        public string getAllEmployeeInHierarchy(string userId, string temp)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            GetConnection();
            string lsSql = " select emp_id,emp_contactid from tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId where user_id = '" + userId + "'";
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, oSqlConnection);
            lda.Fill(dt);
            //dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "emp_id,emp_contactid", " user_id = '" + userId + "'");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp, oSqlConnection) + dt.Rows[0][0].ToString();
                cntid = dt.Rows[0][1].ToString();
                actual = "";//________making global variable null___
            }
            dt.Dispose();
            lda.Dispose();
            oSqlConnection.Close();
            return temp;
        }

        public string getAllEmployeeInHierarchy(string userId, string temp, string BranchId, string companyid)
        {
            DataTable dt = new DataTable();
            string cntid = "";
            string lsSql = "";
            GetConnection();
            if (companyid == "All")
            {
                lsSql = " select tbl_master_employee.emp_id,emp_contactid,emp_branch from tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId inner join tbl_trans_employeeCTC on tbl_trans_employeeCTC.emp_cntId=tbl_master_employee.emp_contactId  where  tbl_trans_employeeCTC.emp_effectiveuntil is null and user_id = '" + userId + "'";
            }
            else
            {
                lsSql = " select tbl_master_employee.emp_id,emp_contactid,emp_branch from tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId inner join tbl_trans_employeeCTC on tbl_trans_employeeCTC.emp_cntId=tbl_master_employee.emp_contactId  where  tbl_trans_employeeCTC.emp_effectiveuntil is null and user_id = '" + userId + "' and tbl_trans_employeeCTC.emp_organization='" + companyid + "'";// and emp_branch='" + BranchId + "'";
            }
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, oSqlConnection);
            lda.Fill(dt);
            //dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "emp_id,emp_contactid", " user_id = '" + userId + "'");
            if (dt.Rows.Count != 0)
            {
                temp = getUser(dt.Rows[0][0].ToString(), temp, oSqlConnection, BranchId, companyid) + dt.Rows[0][0].ToString();
                cntid = dt.Rows[0][1].ToString();
                actual = "";//________making global variable empty___
            }
            dt.Dispose();
            lda.Dispose();
            oSqlConnection.Close();
            return temp;
        }

        public string GetAllCompanyInHierarchy(string compID)
        {
            DataTable dt = new DataTable();
            string lsSql = "";
            GetConnection();
            lsSql = " select cmp_internalid from tbl_master_company where cmp_id=" + compID;
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, oSqlConnection);
            lda.Fill(dt);
            actual = "";//________making global variable empty___
            string Companies = getCompany(dt.Rows[0]["cmp_internalid"].ToString(), oSqlConnection, "") + "'" + dt.Rows[0]["cmp_internalid"].ToString() + "'";
            actual = "";//________making global variable empty___
            dt.Dispose();
            lda.Dispose();
            oSqlConnection.Close();
            return Companies;
        }

        string actual = "";

        public string getUser(string UserId, string ListOfUser)
        {
            DataTable DtSecond = GetDataTable(" tbl_trans_employeeCTC,tbl_master_employee ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) ");
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfUser += DtSecond.Rows[i][0].ToString() + ",";
                    actual += DtSecond.Rows[i][0].ToString() + ",";
                    getUser(DtSecond.Rows[i][0].ToString(), ListOfUser);
                }
            }

            return actual;
        }

        public string getUser(string UserId, string ListOfUser, SqlConnection lcon, string BranchId, string CompanyId)
        {
            string lsSql = "";

            //DataTable DtSecond = GetDataTable(" tbl_trans_employeeCTC,tbl_master_employee ", " tbl_master_employee.emp_id,tbl_trans_employeeCTC.emp_branch ", " tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) and tbl_trans_employeeCTC.emp_branch='" + BranchId  + "'");
            DataTable DtSecond = new DataTable();
            if (CompanyId != "All" && BranchId != "All")
            {
                lsSql = " select tbl_master_employee.emp_id  from tbl_trans_employeeCTC,tbl_master_employee where  tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId  and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) and tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_trans_employeeCTC.emp_branch='" + BranchId + "' and tbl_trans_employeeCTC.emp_organization='" + CompanyId + "'";
            }
            else if (CompanyId == "All" && BranchId != "All")
            {
                lsSql = " select tbl_master_employee.emp_id  from tbl_trans_employeeCTC,tbl_master_employee where  tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId  and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) and tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_trans_employeeCTC.emp_branch='" + BranchId + "'";
            }
            else if (CompanyId != "All" && BranchId == "All")
            {
                lsSql = " select tbl_master_employee.emp_id  from tbl_trans_employeeCTC,tbl_master_employee where  tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId  and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) and tbl_trans_employeeCTC.emp_reportTo=" + UserId + " ";
            }
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DtSecond);
            //DataTable DtSecond = GetDataTable(" tbl_trans_employeeCTC,tbl_master_employee ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) ");
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfUser += DtSecond.Rows[i][0].ToString() + ",";
                    actual += DtSecond.Rows[i][0].ToString() + ",";
                    lda.Dispose();
                    getUser(DtSecond.Rows[i][0].ToString(), ListOfUser, lcon, BranchId, CompanyId);
                }
            }

            return actual;
        }

        public string getCompany(string CompInternalID, SqlConnection lcon, string listOfCompanies)
        {
            string lsSql = "";
            DataTable DtSecond = new DataTable();

            lsSql = " select cmp_internalid from tbl_master_company where cmp_parentid='" + CompInternalID + "'";
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DtSecond);
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    listOfCompanies += DtSecond.Rows[i]["cmp_internalid"].ToString() + ",";
                    actual += "'" + DtSecond.Rows[i]["cmp_internalid"].ToString() + "',";
                    getCompany(DtSecond.Rows[i]["cmp_internalid"].ToString(), lcon, listOfCompanies);
                }
            }
            lda.Dispose();
            return actual;
        }

        public string getUser(string UserId, string ListOfUser, SqlConnection lcon)
        {
            DataTable DtSecond = new DataTable();
            string lsSql = " select tbl_master_employee.emp_id  from tbl_trans_employeeCTC,tbl_master_employee where tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) ";
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DtSecond);
            //DataTable DtSecond = GetDataTable(" tbl_trans_employeeCTC,tbl_master_employee ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) ");
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfUser += DtSecond.Rows[i][0].ToString() + ",";
                    actual += DtSecond.Rows[i][0].ToString() + ",";
                    lda.Dispose();
                    getUser(DtSecond.Rows[i][0].ToString(), ListOfUser, lcon);
                }
            }

            return actual;
        }

        public string getBranch(string BranchId, string ListOfUser)
        {

            DataTable DtSecond = GetDataTable(" tbl_master_branch ", " branch_id ", " branch_parentId= " + BranchId);
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfUser += DtSecond.Rows[i][0].ToString() + ",";
                    actual += DtSecond.Rows[i][0].ToString() + ",";
                    getBranch(DtSecond.Rows[i][0].ToString(), ListOfUser);
                }
            }


            return actual;
        }


        //



        #endregion To get UserList for Lead

        public DataSet PopulateData(
             String columns, //Columns Names in a table which are requered
             String tableName, //Table Name
             String whereCondition // Condition of sql query
            )
        {
            String sqlQuery = "Select " + columns + " From " + tableName;
            if (whereCondition != null)
            {
                sqlQuery += " Where " + whereCondition;
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter(sqlQuery, oSqlConnection);
            DataSet ds = new DataSet();
            sda.Fill(ds, "TableName");
            oSqlConnection.Close();
            return ds;
        }

        //___________________Getting INTERNAL ID ______________________________//
        public string GetInternalId(String prefix, String table, String fields, String WhereCondition)
        {
            String lSsql = "select max(" + fields + ") as ID from " + table + " where " + WhereCondition + " like '" + prefix + "%'";
            SqlDataReader lsdr = proc.GetReader(lSsql);
            string IdGenerated = "";
            if (lsdr.HasRows)
            {
                string ID = "abc0000000"; // dummy -- if reader has a null value!
                while (lsdr.Read())
                {
                    if (lsdr[0].ToString() != "")
                    {
                        ID = lsdr[0].ToString();
                    }
                    else
                    {
                        break;
                    }
                }
                lsdr.Close();

                int length = ID.Length;
                int intpart = int.Parse(ID.Substring(length - 7, 7));
                int newNo = intpart + 1;
                if (newNo <= 9)
                {
                    IdGenerated = prefix + "000000" + newNo.ToString();
                }
                else if (newNo <= 99 && newNo > 9)
                {
                    IdGenerated = prefix + "00000" + newNo.ToString();
                }
                else if (newNo <= 999 && newNo > 99)
                {
                    IdGenerated = prefix + "0000" + newNo.ToString();
                }
                else if (newNo <= 9999 && newNo > 999)
                {
                    IdGenerated = prefix + "000" + newNo.ToString();
                }
                else if (newNo <= 99999 && newNo > 9999)
                {
                    IdGenerated = prefix + "00" + newNo.ToString();
                }
                else if (newNo <= 999999 && newNo > 99999)
                {
                    IdGenerated = prefix + "0" + newNo.ToString();
                }
                else if (newNo <= 9999999 && newNo > 999999)
                {
                    IdGenerated = prefix + "" + newNo.ToString();
                }
            }
            else
            {
                IdGenerated = prefix + "0000001";
            }
            return IdGenerated;
        }

        public string GetInternalId_Rfa(String prefix, String table, String fields, String WhereCondition)
        {
            String lSsql = "select max(" + fields + ") as ID from " + table + " where " + WhereCondition + " like '" + prefix + "%'";
            SqlDataReader lsdr = proc.GetReader(lSsql);
            string IdGenerated = "";
            if (lsdr.HasRows)
            {
                string ID = "abc000"; // dummy -- if reader has a null value!
                while (lsdr.Read())
                {
                    if (lsdr[0].ToString() != "")
                    {
                        ID = lsdr[0].ToString();
                    }
                    else
                    {
                        break;
                    }
                }
                lsdr.Close();
                int length = ID.Length;
                int intpart = int.Parse(ID.Substring(length - 3, 3));
                int newNo = intpart + 1;
                if (newNo <= 9)
                {
                    IdGenerated = prefix + "00" + newNo.ToString();
                }
                else if (newNo <= 99 && newNo > 9)
                {
                    IdGenerated = prefix + "0" + newNo.ToString();
                }
                else if (newNo <= 999 && newNo > 99)
                {
                    IdGenerated = prefix + "" + newNo.ToString();
                }
            }
            else
            {
                IdGenerated = prefix + "001";
            }
            return IdGenerated;
        }

        public string GetInternalId_Req(String prefix, String table, String fields, String WhereCondition)
        {
            String lSsql = "select max(" + fields + ") as ID from " + table + " where " + WhereCondition + " like '" + prefix + "%'";
            SqlDataReader lsdr = proc.GetReader(lSsql);
            string IdGenerated = "";
            if (lsdr.HasRows)
            {
                string ID = "abc00000"; // dummy -- if reader has a null value!
                while (lsdr.Read())
                {
                    if (lsdr[0].ToString() != "")
                    {
                        ID = lsdr[0].ToString();
                    }
                    else
                    {
                        break;
                    }
                }
                lsdr.Close();
                int length = ID.Length;
                int intpart = int.Parse(ID.Substring(length - 5, 5));
                int newNo = intpart + 1;
                if (newNo <= 9)
                {
                    IdGenerated = prefix + "0000" + newNo.ToString();
                }
                else if (newNo <= 99 && newNo > 9)
                {
                    IdGenerated = prefix + "000" + newNo.ToString();
                }
                else if (newNo <= 999 && newNo > 99)
                {
                    IdGenerated = prefix + "00" + newNo.ToString();
                }
                else if (newNo <= 9999 && newNo > 999)
                {
                    IdGenerated = prefix + "0" + newNo.ToString();
                }
                else if (newNo <= 99999 && newNo > 9999)
                {
                    IdGenerated = prefix + "" + newNo.ToString();
                }
            }
            else
            {
                IdGenerated = prefix + "0000001";
            }
            return IdGenerated;
        }

        //___________________GetGroupFieldValue_______________________________//

        public string[,] GetGroupFieldValue(
                String cTableName,      // TableName from which the field value is to be fetched
                String cFieldName,      // The name of the field whose value needs to be returned
                String cGroupClause,    // Optional : Group By condition [if any]
                String CField,          // The name of the field Used For Count
                int NoField)            // Number of field value to selection
        {
            string[,] vRetVal;
            String lcSql;
            lcSql = "Select Count(distinct " + CField + ") from " + cTableName;

            SqlDataReader lsdr;
            lsdr = proc.GetReader(lcSql);
            int CountNoRow = 0;
            while (lsdr.Read())
            {
                CountNoRow = (int)lsdr[0];
            }
            lsdr.Close();

            vRetVal = new string[CountNoRow, NoField];
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cGroupClause != null)
            {
                lcSql += " group by " + cGroupClause;
            }

            lsdr = proc.GetReader(lcSql);

            if (lsdr.HasRows)
            {
                int i = 0;
                while (lsdr.Read())
                {
                    for (int j = 0; j < NoField; j++)
                    {
                        vRetVal[i, j] = lsdr[j].ToString();
                    }
                    i = i + 1;
                }
            }
            else
            {
                string[,] messg = new string[1, 1];
                messg[0, 0] = "n";
                return messg;
            }

            lsdr.Close();

            return vRetVal;
        }

        public Int32 DeleteValue(
                String cTableName,      // TableName to which the field value is to be updated
                String cWhereClause    // WHERE condition
                )
        {

            int rowscount = 0;



            String lSsql = "Delete from " + cTableName + " Where " + cWhereClause;
            //SqlConnection oSqlConnection = GetConnection();
            GetConnection();
            SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
            try
            {
                rowscount = lcmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {

                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------

                oSqlConnection.Close();
                return rowscount;
            }

            oSqlConnection.Close();
            return rowscount;
        }

        public bool IsDate(string str)
        {
            bool result = true;
            DateTime date;

            try
            {
                date = Convert.ToDateTime(str);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public string sepComma(string s)
        {
            string sTemp = "";
            try
            {
                int i = s.LastIndexOf(",");
                sTemp = s.Substring(0, i);
            }
            catch
            {
            }
            return sTemp;
        }

        public string getEndDate(string startDate,
                        int totalData, 	// Total call assigned
                        int currentData	// represents number of activity executed by respective user
                        )
        {
            try
            {
                int _totalMinutes = 0;
                int iDiff = 0;
                System.DateTime tCurrentTime = default(System.DateTime);
                System.DateTime tStartTime = default(System.DateTime);
                TimeSpan _dDiff = default(TimeSpan);
                if (IsDate(startDate) == true)
                {
                    System.DateTime dDate = Convert.ToDateTime(startDate);
                    if (totalData != 0 & currentData != 0)
                    {
                        if (dDate.ToShortDateString() != DateTime.Now.ToShortDateString())
                        {
                            iDiff = DateTime.Now.Subtract(dDate).Days;
                            if (iDiff > 1)
                            {
                                _totalMinutes = (iDiff - 1) * 480;
                                tCurrentTime = Convert.ToDateTime(dDate.ToShortDateString() + " 06:00 PM");
                                tStartTime = Convert.ToDateTime(startDate);
                                _dDiff = tCurrentTime.Subtract(tStartTime);
                                iDiff = Convert.ToInt32(_dDiff.TotalMinutes);
                                _totalMinutes += iDiff;
                                tCurrentTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 10:00 AM");
                                tStartTime = DateTime.Now;
                                _dDiff = tCurrentTime.Subtract(tStartTime);
                                iDiff = Convert.ToInt32(_dDiff.TotalMinutes);
                                _totalMinutes += iDiff;
                            }
                            else
                            {
                                tCurrentTime = Convert.ToDateTime(dDate.ToShortDateString() + " 06:00 PM");
                                tStartTime = Convert.ToDateTime(startDate);
                                _dDiff = tCurrentTime.Subtract(tStartTime);
                                iDiff = Convert.ToInt32(_dDiff.TotalMinutes);
                                _totalMinutes += iDiff;
                                tCurrentTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 10:00 AM");
                                tStartTime = DateTime.Now;
                                _dDiff = tStartTime.Subtract(tCurrentTime);
                                iDiff = Convert.ToInt32(_dDiff.TotalMinutes);
                                _totalMinutes += iDiff;
                            }
                        }
                        else
                        {
                            tCurrentTime = Convert.ToDateTime(dDate.ToShortDateString() + " 06:00 PM");
                            tStartTime = Convert.ToDateTime(startDate);
                            _dDiff = tCurrentTime.Subtract(tStartTime);
                            iDiff = Convert.ToInt32(_dDiff.TotalMinutes);
                            _totalMinutes += iDiff;
                        }
                    }
                }
                double _timePerCall;
                if (_totalMinutes != 0 && currentData != 0)
                {
                    _timePerCall = _totalMinutes / currentData;
                }
                else
                {
                    _timePerCall = 0.0;
                }
                System.DateTime dExpectedEndDate = DateTime.Now;
                if (_timePerCall > 0)
                {
                    int _totalTimeReq = Convert.ToInt32(_timePerCall * (totalData - currentData));
                    tCurrentTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 06:00 PM");
                    tStartTime = DateTime.Now;
                    _dDiff = tCurrentTime.Subtract(tStartTime);
                    iDiff = Convert.ToInt32(_dDiff.TotalMinutes);

                    if (_totalTimeReq < iDiff)
                    {
                        dExpectedEndDate = dExpectedEndDate.AddMinutes(_totalTimeReq);
                        _totalTimeReq -= _totalTimeReq;
                    }
                    else
                    {
                        _totalTimeReq -= iDiff;
                    }
                    while (_totalTimeReq != 0)
                    {
                        if (_totalTimeReq > 480)
                        {
                            dExpectedEndDate = dExpectedEndDate.AddDays(1);
                            _totalTimeReq -= 480;
                        }
                        else
                        {
                            dExpectedEndDate = dExpectedEndDate.AddDays(1);
                            dExpectedEndDate = Convert.ToDateTime(dExpectedEndDate.ToShortDateString() + " 10:00 AM");
                            dExpectedEndDate = dExpectedEndDate.AddMinutes(_totalTimeReq);
                            _totalTimeReq -= _totalTimeReq;
                        }
                    }
                }

                return dExpectedEndDate.ToString();
            }
            catch (Exception ex)
            {
                return DateTime.Today.Date.ToString();
            }
        }

        public void InsertPhoneCall(string Procedure, string[,] ProcedureData)
        {
            //SqlConnection oSqlConnection = GetConnection();
            GetConnection();
            SqlCommand lcmd = new SqlCommand(Procedure, oSqlConnection);
            lcmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < (ProcedureData.Length / 3); i++)
            {
                string PVariable = ProcedureData[i, 0];
                string DbType = ProcedureData[i, 1];
                string PValue = ProcedureData[i, 2];
                if (DbType == "Int")
                {
                    lcmd.Parameters.Add(PVariable, SqlDbType.Int).Value = PValue;
                }
                else if (DbType == "VarChar")
                {
                    lcmd.Parameters.Add(PVariable, SqlDbType.VarChar).Value = PValue;
                }
            }

            try
            {

                Int32 rowscount = lcmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {

                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


            }

            oSqlConnection.Close();
        }

        public void ExecuteProcedure(string Procedure, string[,] ProcedureData)
        {
            //SqlConnection oSqlConnection = GetConnection();
            GetConnection();
            SqlCommand lcmd = new SqlCommand(Procedure, oSqlConnection);
            lcmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < (ProcedureData.Length / 3); i++)
            {
                string PVariable = ProcedureData[i, 0];
                string DbType = ProcedureData[i, 1];
                string PValue = ProcedureData[i, 2];
                if (DbType == "Int")
                {
                    lcmd.Parameters.Add(PVariable, SqlDbType.Int).Value = PValue;
                }
                else if (DbType == "VarChar")
                {
                    lcmd.Parameters.Add(PVariable, SqlDbType.VarChar).Value = PValue;
                }
            }

            try
            {

                Int32 rowscount = lcmd.ExecuteNonQuery();

            }

            catch (Exception e)
            {

                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


            }

            oSqlConnection.Close();
        }

        public void ExecuteProcedure(string Procedure)
        {
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlCommand lcmd = new SqlCommand(Procedure, oSqlConnection);
            lcmd.CommandType = CommandType.StoredProcedure;
            //if (ProcedureData[0, 0] != "n")
            //{
            //    for (int i = 0; i < (ProcedureData.Length / 3); i++)
            //    {
            //        string PVariable = ProcedureData[i, 0];
            //        string DbType = ProcedureData[i, 1];
            //        string PValue = ProcedureData[i, 2];
            //        if (DbType == "Int")
            //        {
            lcmd.Parameters.Add("@dbname", SqlDbType.VarChar).Value = "newTable";
            //        }
            //        else if (DbType == "VarChar")
            //        {
            //            lcmd.Parameters.Add(PVariable, SqlDbType.VarChar).Value = PValue;
            //        }

            //    }
            //}

            try
            {


                Int32 rowscount = lcmd.ExecuteNonQuery();
            }

            catch (Exception e)
            {

                // For Read Only User Error Notification Start-----------------------------------------

                if (e.Message.Contains("The INSERT permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }


                else if (e.Message.Contains("The UPDATE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {

                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");

                    }

                }

                else if (e.Message.Contains("The DELETE permission was denied on the object"))
                {
                    oSqlConnection.Close();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Redirect("~/ErrorPage.aspx");
                    }

                }

                // For Read Only User Error Notification End-----------------------------------------


            }

            oSqlConnection.Close();
        }

        public DataTable GetDatatable_StoredProcedure(string Procedure, string[,] ProcedureData)
        {
            DataTable ObjDT = new DataTable();
            GetConnection();
            SqlCommand lcmd = new SqlCommand(Procedure, oSqlConnection);
            lcmd.CommandType = CommandType.StoredProcedure;
            if (ProcedureData != null)
            {
                for (int i = 0; i < (ProcedureData.Length / 3); i++)
                {
                    string PVariable = ProcedureData[i, 0];
                    string DbType = ProcedureData[i, 1];
                    string PValue = ProcedureData[i, 2];
                    if (DbType == "Int")
                    {
                        lcmd.Parameters.Add(PVariable, SqlDbType.Int).Value = PValue;
                    }
                    else if (DbType == "VarChar")
                    {
                        lcmd.Parameters.Add(PVariable, SqlDbType.VarChar).Value = PValue;
                    }
                }
            }
            using (SqlDataAdapter da = new SqlDataAdapter(lcmd))
            {
                //Populate the DataTable with the results
                da.Fill(ObjDT);
            }
            oSqlConnection.Close();
            return ObjDT;
        }

        public string getDateTimeFormat(string str)
        {
            try
            {
                if ((str == null) | (string.IsNullOrEmpty(str))) return str;
                str = str.Trim();
                int st = str.LastIndexOf(":");
                string st1 = str.Substring(0, 10);
                string hour1 = str.Substring(11, 2);
                int hour = Convert.ToInt32(hour1);
                string min1 = str.Substring(14);
                int min = Convert.ToInt32(min1);
                string ampm = "";
                if (hour > 12)
                {
                    hour = hour - 12;
                    ampm = "PM";
                }
                else
                {
                    if (hour == 12)
                    {
                        ampm = "PM";
                    }
                    else
                    {
                        ampm = "AM";
                    }
                }

                string h = hour.ToString();
                if (h.Length < 2)
                {
                    h = "0" + h;
                }
                string m = min.ToString();
                if (m.Length < 2)
                {
                    m = "0" + m;
                }
                return st1 + " " + h + ":" + m + " " + ampm;
            }
            catch (Exception ex)
            {
                return str;
            }
        }

        #region ___When ever Activity table (tbl_trans_activies) will update this function must be fired to update message table (tbl_mastert_message)____//

        public void messageTableUpdate(string targetUser, string createUser, string ActTypeName, string startTime, string endTime, string priority, string contents,
                                       string sourceID, //__Only for Activity update else pass null
                                       string TypeOfAct_pass_message_or_activity)
        {
            string[,] data = GetFieldValue(" tbl_master_user ", " user_id,user_name  ", " user_id IN ( " + targetUser + "," + createUser + ")", 2);
            string TagetUserName = "";
            string CreateUserName = "";
            if (data.Length > 0)
            {
                if (targetUser == createUser)
                {
                    TagetUserName = data[0, 1].ToString();
                    CreateUserName = data[0, 1].ToString();
                }
                else
                {
                    for (int i = 0; i < data.Length / 2; i++)
                    {
                        if (data[i, 0].ToString() == targetUser)
                            TagetUserName = data[i, 1].ToString();
                        if (data[i, 0].ToString() == createUser)
                            CreateUserName = data[i, 1].ToString();
                    }
                }
            }
            string message = "Hello " + TagetUserName + " ! " + CreateUserName + " Has allotted a New [ " + ActTypeName + " ] type of activity to you, to be started by " + startTime + " and finished by " + endTime + " with " + priority + " priority.  Have a great day !! Note :- " + contents;
            string fields = " msg_createuser, msg_createdate, msg_targetuser, msg_content, msg_messageread, msg_sourceid, CreateDate, CreateUser";
            string Values = "";
            if (TypeOfAct_pass_message_or_activity == "message")
                Values = createUser + ",'" + System.DateTime.Now + "'," + targetUser + ",'" + message + "',0,0,'" + System.DateTime.Now + "'," + HttpContext.Current.Session["userid"];
            if (TypeOfAct_pass_message_or_activity == "activity")
                Values = "0,'" + System.DateTime.Now + "'," + targetUser + ",'" + message + "',0," + sourceID + ",'" + System.DateTime.Now + "'," + HttpContext.Current.Session["userid"];
            int NoOfRowsEffected = InsurtFieldValue(" tbl_master_message ", fields, Values);
        }

        #endregion ___When ever Activity table (tbl_trans_activies) will update this function must be fired to update message table (tbl_mastert_message)____//

        #region Data Transfer from database to Xml

        //public string transfertDataToXml(string messageID)
        //{
        //    string result = "";
        //    string[] IDs = messageID.Split(',');
        //    for (int i = 0; i < IDs.Length; i++)
        //    {
        //        string listofIDs = "";
        //        string listIds = GetparentIdList(IDs[i], listofIDs);
        //        if (listIds != "")
        //            listIds += "," + IDs[i];
        //        else
        //            listIds = IDs[i];
        //        DataTable DT = GetDataTable(" tbl_master_message ", " * ", " msg_Id in (" + listIds + ") ");
        //        if (DT.Rows.Count > 0)
        //        {
        //            for (int j = 0; j < DT.Rows.Count; j++)
        //            {
        //                XmlDocument Xml_sender = new XmlDocument();
        //                XmlDocument XmlDoc = new XmlDocument();
        //                XmlDeclaration XmlDeclaration = XmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        //                XmlDocument xml_receiver = new XmlDocument();

        //                Converter oConverter = new Converter();
        //                string createdate = oConverter.DateConverter_d_m_y(DT.Rows[j]["msg_createdate"].ToString(), "mm/dd/yyyy");
        //                createdate = "D" + createdate.Replace('/', '.');
        //                try
        //                {
        //                    string filepath = "..\\Messages\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    if (File.Exists(filepath) == true)
        //                    {
        //                        //____Use Existing file______________//
        //                        string path = "..\\Messages\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                        Xml_sender.Load(path);
        //                        if (Xml_sender.SelectSingleNode("\\" + createdate) == null)
        //                        {
        //                            XmlElement parentNode_1;
        //                            XmlElement parentNode_2;
        //                            XmlElement parentNode_3;
        //                            XmlElement childNode1;
        //                            XmlElement childNode2;
        //                            XmlElement childNode3;
        //                            XmlElement childNode4;
        //                            XmlElement childNode5;

        //                            XmlElement rootNode = (XmlElement)Xml_sender.SelectSingleNode("Message");

        //                            parentNode_1 = Xml_sender.CreateElement(createdate);
        //                            rootNode.AppendChild(parentNode_1);

        //                            parentNode_2 = Xml_sender.CreateElement("U" + DT.Rows[j]["msg_targetuser"].ToString());
        //                            parentNode_1.AppendChild(parentNode_2);

        //                            parentNode_3 = Xml_sender.CreateElement("M" + DT.Rows[j]["msg_Id"].ToString());
        //                            parentNode_2.AppendChild(parentNode_3);

        //                            childNode1 = Xml_sender.CreateElement("Content");
        //                            childNode1.InnerText = DT.Rows[j]["msg_content"].ToString().Trim();
        //                            parentNode_3.AppendChild(childNode1);

        //                            childNode2 = Xml_sender.CreateElement("CreateDate");
        //                            childNode2.InnerText = DT.Rows[j]["msg_createdate"].ToString().Trim();
        //                            parentNode_3.AppendChild(childNode2);

        //                            childNode3 = Xml_sender.CreateElement("ReadDate");
        //                            childNode3.InnerText = DT.Rows[j]["msg_updateddate"].ToString().Trim();
        //                            parentNode_3.AppendChild(childNode3);

        //                            childNode4 = Xml_sender.CreateElement("DeleteDate");
        //                            childNode4.InnerText = System.DateTime.Now.ToString().Trim();
        //                            parentNode_3.AppendChild(childNode4);

        //                            childNode5 = Xml_sender.CreateElement("ParentId");
        //                            if (DT.Rows[j]["msg_parent_msg_Id"].ToString() != "")
        //                                childNode5.InnerText = DT.Rows[j]["msg_createdate"].ToString().Trim();
        //                            else
        //                                childNode5.InnerText = "0";
        //                            parentNode_3.AppendChild(childNode5);

        //                            string path1 = "..\\Messages\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                            Xml_sender.Save(path1.Trim());
        //                            result = "Done";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //________Creating Xml File_________//

        //                        XmlElement parentNode_1;
        //                        XmlElement parentNode_2;
        //                        XmlElement parentNode_3;
        //                        XmlElement childNode1;
        //                        XmlElement childNode2;
        //                        XmlElement childNode3;
        //                        XmlElement childNode4;
        //                        XmlElement childNode5;

        //                        XmlElement rootNode = XmlDoc.CreateElement("Message");
        //                        XmlDoc.InsertBefore(XmlDeclaration, XmlDoc.DocumentElement);
        //                        XmlDoc.AppendChild(rootNode);

        //                        parentNode_1 = XmlDoc.CreateElement(createdate);
        //                        rootNode.AppendChild(parentNode_1);

        //                        parentNode_2 = XmlDoc.CreateElement("U" + DT.Rows[j]["msg_targetuser"].ToString());
        //                        parentNode_1.AppendChild(parentNode_2);

        //                        parentNode_3 = XmlDoc.CreateElement("M" + DT.Rows[j]["msg_Id"].ToString());
        //                        parentNode_2.AppendChild(parentNode_3);

        //                        childNode1 = XmlDoc.CreateElement("Content");
        //                        childNode1.InnerText = DT.Rows[j]["msg_content"].ToString().Trim();
        //                        parentNode_3.AppendChild(childNode1);

        //                        childNode2 = XmlDoc.CreateElement("CreateDate");
        //                        childNode2.InnerText = DT.Rows[j]["msg_createdate"].ToString().Trim();
        //                        parentNode_3.AppendChild(childNode2);

        //                        childNode3 = XmlDoc.CreateElement("ReadDate");
        //                        childNode3.InnerText = DT.Rows[j]["msg_updateddate"].ToString().Trim();
        //                        parentNode_3.AppendChild(childNode3);

        //                        childNode4 = XmlDoc.CreateElement("DeleteDate");
        //                        childNode4.InnerText = System.DateTime.Now.ToString().Trim();
        //                        parentNode_3.AppendChild(childNode4);

        //                        childNode5 = XmlDoc.CreateElement("ParentId");
        //                        if (DT.Rows[j]["msg_parent_msg_Id"].ToString() != "")
        //                            childNode5.InnerText = DT.Rows[j]["msg_createdate"].ToString().Trim();
        //                        else
        //                            childNode5.InnerText = "0";
        //                        parentNode_3.AppendChild(childNode5);

        //                        string path = "..\\Messages\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                        XmlDoc.Save(path.Trim());

        //                        result = "Done";
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    result = "Error";
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        private string GetparentIdList(string messageID, string ListId)
        {
            DataTable DT1 = GetDataTable(" tbl_master_message ", " msg_parent_msg_Id ", " msg_Id=" + messageID);
            if (DT1.Rows.Count > 0)
            {
                if (DT1.Rows[0][0].ToString() != "")
                {
                    if (ListId != "")
                        ListId += "," + DT1.Rows[0][0].ToString();
                    else
                        ListId = DT1.Rows[0][0].ToString();
                    string data = ListId;
                    GetparentIdList(DT1.Rows[0][0].ToString(), data);
                }
                else
                    list_of_ids = ListId;
            }

            return list_of_ids;
        }

        #endregion Data Transfer from database to Xml

        #region transfer data from tbl_master_message to tbl_master_message_backup

        public string transferToBackupMessage(string messageID)
        {
            string[] IDs = messageID.Split(',');
            string result = "";
            for (int i = 0; i < IDs.Length; i++)
            {
                string listIds = GetparentIdList(IDs[i], "");
                if (listIds != "")
                    listIds += "," + IDs[i];
                else
                    listIds = IDs[i];

                String lSsql = "INSERT INTO tbl_master_message_backup (msg_Id,msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid,msg_updateddate,msg_parent_msg_Id,CreateDate,CreateUser,LastModifyDate,LastModifyUser) select msg_Id,msg_createuser,msg_createdate,msg_targetuser,msg_content,msg_messageread,msg_sourceid,msg_updateddate,msg_parent_msg_Id,CreateDate,CreateUser,LastModifyDate,LastModifyUser from tbl_master_message where msg_id in (" + listIds + ")";
                //SqlConnection oSqlConnection = GetConnection();
                GetConnection();
                SqlCommand lcmd = new SqlCommand(lSsql, oSqlConnection);
                Int32 rowsEffected = lcmd.ExecuteNonQuery();
                if (rowsEffected > 0)
                {
                    rowsEffected = DeleteValue(" tbl_master_message ", " msg_id in (" + listIds + ")");
                    if (rowsEffected > 0)
                        result = "Done";
                }
                oSqlConnection.Close();
            }
            return result;
        }

        #endregion transfer data from tbl_master_message to tbl_master_message_backup

        public string getChildUser_for_AllEmployee(string UserId, string temp)
        {
            string cntId = "";
            DataTable dt = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "emp_id,emp_contactid", " user_id='" + UserId + "'");
            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    temp = getUser(dt.Rows[0][0].ToString(), temp);
                    actual = "";//________making global variable null___
                    DataTable dt_temp = GetDataTable("tbl_master_employee", "emp_id", " emp_replaceuser in (" + temp + dt.Rows[0][0].ToString() + ")");
                    for (int i = 0; i < dt_temp.Rows.Count; i++)
                    {
                        DataTable dt_temp1 = GetDataTable("tbl_trans_employeeCTC INNER JOIN tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId", "tbl_master_employee.emp_id", " (tbl_trans_employeeCTC.emp_reportTo = " + dt_temp.Rows[i]["emp_id"].ToString() + ") AND (tbl_trans_employeeCTC.emp_effectiveuntil <> (SELECT top 1 emp_effectiveuntil FROM tbl_trans_employeectc WHERE emp_cntid = (select emp_contactid from tbl_master_employee where emp_id=" + dt_temp.Rows[i]["emp_id"].ToString() + ") ORDER BY emp_id DESC))");
                        for (int j = 0; j < dt_temp1.Rows.Count; j++)
                        {
                            temp += dt_temp1.Rows[j]["emp_id"].ToString() + ",";
                        }
                    }
                    temp = getxEmployee(dt.Rows[0][0].ToString(), temp);
                    cntId = dt.Rows[0][1].ToString();
                }
            }
            if (temp != "")
            {
                string[] str = temp.Split(',');
                temp = "";
                for (int i = 0; i < str.Length - 1; i++)
                {
                    DataTable dt_temp = GetDataTable("tbl_master_employee INNER JOIN tbl_master_user ON tbl_master_employee.emp_contactId = tbl_master_user.user_contactId", "tbl_master_user.user_id", " tbl_master_employee.emp_id ='" + str.GetValue(i).ToString() + "'");
                    if (dt_temp != null)
                    {
                        if (dt_temp.Rows.Count != 0)
                        {
                            temp += dt_temp.Rows[0][0].ToString() + ",";
                        }
                    }
                }
            }
            temp += UserId;
            return temp;
        }

        public string getxEmployee(string UserId, string temp)
        {
            string user = "";
            temp = temp + UserId;
            DataTable dt = GetDataTable("tbl_trans_employeeCTC INNER JOIN tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId", "tbl_master_employee.emp_id", " tbl_trans_employeeCTC.emp_reportTo in (" + temp + ") and tbl_master_employee.emp_dateofleaving <> '' and tbl_master_employee.emp_dateofleaving is not null");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    user += dt.Rows[i][0].ToString() + ",";
                }
            }
            string[] str = temp.Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                DataTable dtTemp = GetDataTable("tbl_master_employee", "emp_id,emp_contactid", " emp_replaceUser='" + str.GetValue(i).ToString() + "'");
                for (int j = 0; j < dtTemp.Rows.Count; j++)
                {
                    string EUntill = "";
                    string[,] EUntill1 = GetFieldValue("tbl_trans_employeectc", "emp_effectiveuntil", " emp_cntid='" + dtTemp.Rows[j][1].ToString() + "'", 1);
                    if (EUntill1[0, 0] != "n")
                    {
                        EUntill = EUntill1[0, 0];
                    }
                    DataTable dt1 = GetDataTable("tbl_trans_employeeCTC INNER JOIN tbl_master_employee ON tbl_trans_employeeCTC.emp_cntId = tbl_master_employee.emp_contactId INNER JOIN tbl_master_contact ON tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId", "tbl_master_employee.emp_id", " tbl_trans_employeeCTC.emp_reportTo=" + UserId + " and tbl_trans_employeeCTC.emp_effectiveuntil <> '" + EUntill + "'");
                    if (dt1 != null)
                    {
                        for (int k = 0; k < dt1.Rows.Count; k++)
                        {
                            user += dt1.Rows[k][0].ToString() + ",";
                        }
                    }
                }
            }
            temp = temp + user;
            return temp;
        }

        //_The accecc table contains acc_menuId,acc_view and url
        public string CheckPageAccessebility(string PageNameWithDefaultQueryString)
        {
            getAccessPages();
            DataTable DT_pageWaccesss = (DataTable)HttpContext.Current.Session["DataTable_MenuAccess"];
            string expression = " url like '%" + PageNameWithDefaultQueryString + "%'";
            DataRow[] data = DT_pageWaccesss.Select(expression);
            if (data.Length > 0)
            {
                if (data[0]["acc_view"].ToString() != "")
                    return data[0]["acc_view"].ToString();
                else
                    return "All";
            }
            else
                return "N";
        }

        public void getAccessPages()
        {
            string[,] groups = GetFieldValue(" tbl_master_user ", " user_group ", " user_id=" + HttpContext.Current.Session["userid"], 1);
            string wherecondition = "  grp_segmentId =" + HttpContext.Current.Session["userlastsegment"] + " AND grp_id IN (" + groups[0, 0] + ")";
            string[,] usergroupCurrent = GetFieldValue(" tbl_master_userGroup ", " grp_id ", wherecondition, 1);

            HttpContext.Current.Session["DataTable_MenuAccess"] = GetDataTable(" tbl_trans_access ", " Distinct acc_menuId,acc_view,(select mnu_menuLink from tbl_trans_menu where mnu_id=acc_menuId ) as url ", " acc_userGroupId in (" + usergroupCurrent[0, 0] + ")");
        }

        //public string transfertDataToXmlforMainAcc()
        //{
        //    string result = "";
        //    DataTable DT = GetDataTable(" Master_MainAccount ", " * ", " MainAccount_AccountCode like 'SYSTM%' ");
        //    if (DT.Rows.Count > 0)
        //    {
        //        for (int j = 0; j < DT.Rows.Count; j++)
        //        {
        //            XmlDocument Xml_sender = new XmlDocument();
        //            XmlDocument XmlDoc = new XmlDocument();
        //            XmlDeclaration XmlDeclaration = XmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        //            XmlDocument xml_receiver = new XmlDocument();

        //            Converter oConverter = new Converter();
        //            //string createdate = DateTime.Now.ToString();
        //            string createdate = oConverter.DateConverter_d_m_y(DateTime.Now.ToString(), "mm/dd/yyyy");
        //            createdate = "D" + createdate.Replace('/', '.');
        //            try
        //            {
        //                string filepath = "..\\Documents\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                if (File.Exists(filepath) == true)
        //                {
        //                    //____Use Existing file______________//
        //                    string path = "..\\Documents\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    Xml_sender.Load(path);

        //                    XmlElement parentNode_1;
        //                    XmlElement parentNode_2;
        //                    XmlElement parentNode_3;
        //                    XmlElement childNode1;
        //                    XmlElement childNode2;
        //                    XmlElement childNode3;
        //                    XmlElement childNode4;
        //                    XmlElement childNode5;
        //                    XmlElement childNode6;
        //                    XmlElement childNode7;
        //                    XmlElement childNode8;
        //                    XmlElement childNode9;
        //                    XmlElement childNode10;
        //                    XmlElement childNode11;
        //                    XmlElement childNode12;
        //                    XmlElement childNode13;
        //                    XmlElement childNode14;
        //                    XmlElement childNode15;
        //                    XmlElement childNode16;
        //                    XmlElement childNode17;
        //                    XmlElement childNode18;
        //                    XmlElement childNode19;

        //                    XmlElement rootNode = (XmlElement)Xml_sender.SelectSingleNode("Message");

        //                    parentNode_1 = Xml_sender.CreateElement(createdate);
        //                    rootNode.AppendChild(parentNode_1);

        //                    childNode1 = Xml_sender.CreateElement("MainAccount_ReferenceID");
        //                    childNode1.InnerText = DT.Rows[j]["MainAccount_ReferenceID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode1);

        //                    childNode2 = Xml_sender.CreateElement("MainAccount_AccountType");
        //                    childNode2.InnerText = DT.Rows[j]["MainAccount_AccountType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode2);

        //                    childNode3 = Xml_sender.CreateElement("MainAccount_BankCashType");
        //                    childNode3.InnerText = DT.Rows[j]["MainAccount_BankCashType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode3);

        //                    childNode4 = Xml_sender.CreateElement("MainAccount_BankAccountType");
        //                    childNode4.InnerText = DT.Rows[j]["MainAccount_BankAccountType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode4);

        //                    childNode5 = Xml_sender.CreateElement("MainAccount_ExchangeSegment");
        //                    childNode5.InnerText = DT.Rows[j]["MainAccount_ExchangeSegment"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode5);

        //                    childNode6 = Xml_sender.CreateElement("MainAccount_AccountCode");
        //                    childNode6.InnerText = DT.Rows[j]["MainAccount_AccountCode"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode6);

        //                    childNode7 = Xml_sender.CreateElement("MainAccount_AccountGroup");
        //                    childNode7.InnerText = DT.Rows[j]["MainAccount_AccountGroup"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode7);

        //                    childNode8 = Xml_sender.CreateElement("MainAccount_Name");
        //                    childNode8.InnerText = DT.Rows[j]["MainAccount_Name"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode8);

        //                    childNode9 = Xml_sender.CreateElement("MainAccount_BankAcNumber");
        //                    childNode9.InnerText = DT.Rows[j]["MainAccount_BankAcNumber"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode9);

        //                    childNode10 = Xml_sender.CreateElement("MainAccount_SubLedgerType");
        //                    childNode10.InnerText = DT.Rows[j]["MainAccount_SubLedgerType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode10);

        //                    childNode11 = Xml_sender.CreateElement("MainAccount_IsTDS");
        //                    childNode11.InnerText = DT.Rows[j]["MainAccount_IsTDS"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode11);

        //                    childNode12 = Xml_sender.CreateElement("MainAccount_TDSRate");
        //                    childNode12.InnerText = DT.Rows[j]["MainAccount_TDSRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode12);

        //                    childNode13 = Xml_sender.CreateElement("MainAccount_IsFBT");
        //                    childNode13.InnerText = DT.Rows[j]["MainAccount_IsFBT"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode13);

        //                    childNode14 = Xml_sender.CreateElement("MainAccount_FBTRate");
        //                    childNode14.InnerText = DT.Rows[j]["MainAccount_FBTRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode14);

        //                    childNode15 = Xml_sender.CreateElement("MainAccount_RateOfInterest");
        //                    childNode15.InnerText = DT.Rows[j]["MainAccount_RateOfInterest"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode15);

        //                    childNode16 = Xml_sender.CreateElement("MainAccount_ContactID");
        //                    childNode16.InnerText = DT.Rows[j]["MainAccount_ContactID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode16);

        //                    childNode17 = Xml_sender.CreateElement("MainAccount_Nature");
        //                    childNode17.InnerText = DT.Rows[j]["MainAccount_Nature"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode17);

        //                    childNode18 = Xml_sender.CreateElement("CreateDate");
        //                    childNode18.InnerText = DateTime.Now.ToString().Trim();
        //                    parentNode_1.AppendChild(childNode18);

        //                    childNode19 = Xml_sender.CreateElement("CreateUser");
        //                    childNode19.InnerText = HttpContext.Current.Session["userid"].ToString();
        //                    parentNode_1.AppendChild(childNode19);

        //                    string path1 = "..\\Documents\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    Xml_sender.Save(path1.Trim());
        //                    result = "Done";
        //                }
        //                else
        //                {
        //                    //________Creating Xml File_________//

        //                    XmlElement parentNode_1;
        //                    XmlElement childNode1;
        //                    XmlElement childNode2;
        //                    XmlElement childNode3;
        //                    XmlElement childNode4;
        //                    XmlElement childNode5;
        //                    XmlElement childNode6;
        //                    XmlElement childNode7;
        //                    XmlElement childNode8;
        //                    XmlElement childNode9;
        //                    XmlElement childNode10;
        //                    XmlElement childNode11;
        //                    XmlElement childNode12;
        //                    XmlElement childNode13;
        //                    XmlElement childNode14;
        //                    XmlElement childNode15;
        //                    XmlElement childNode16;
        //                    XmlElement childNode17;
        //                    XmlElement childNode18;
        //                    XmlElement childNode19;

        //                    XmlElement rootNode = XmlDoc.CreateElement("Message");
        //                    XmlDoc.InsertBefore(XmlDeclaration, XmlDoc.DocumentElement);
        //                    XmlDoc.AppendChild(rootNode);

        //                    parentNode_1 = XmlDoc.CreateElement(createdate);
        //                    rootNode.AppendChild(parentNode_1);

        //                    childNode1 = XmlDoc.CreateElement("MainAccount_ReferenceID");
        //                    childNode1.InnerText = DT.Rows[j]["MainAccount_ReferenceID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode1);

        //                    childNode2 = XmlDoc.CreateElement("MainAccount_AccountType");
        //                    childNode2.InnerText = DT.Rows[j]["MainAccount_AccountType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode2);

        //                    childNode3 = XmlDoc.CreateElement("MainAccount_BankCashType");
        //                    childNode3.InnerText = DT.Rows[j]["MainAccount_BankCashType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode3);

        //                    childNode4 = XmlDoc.CreateElement("MainAccount_BankAccountType");
        //                    childNode4.InnerText = DT.Rows[j]["MainAccount_BankAccountType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode4);

        //                    childNode5 = XmlDoc.CreateElement("MainAccount_ExchangeSegment");
        //                    childNode5.InnerText = DT.Rows[j]["MainAccount_ExchangeSegment"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode5);

        //                    childNode6 = XmlDoc.CreateElement("MainAccount_AccountCode");
        //                    childNode6.InnerText = DT.Rows[j]["MainAccount_AccountCode"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode6);

        //                    childNode7 = XmlDoc.CreateElement("MainAccount_AccountGroup");
        //                    childNode7.InnerText = DT.Rows[j]["MainAccount_AccountGroup"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode7);

        //                    childNode8 = XmlDoc.CreateElement("MainAccount_Name");
        //                    childNode8.InnerText = DT.Rows[j]["MainAccount_Name"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode8);

        //                    childNode9 = XmlDoc.CreateElement("MainAccount_BankAcNumber");
        //                    childNode9.InnerText = DT.Rows[j]["MainAccount_BankAcNumber"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode9);

        //                    childNode10 = XmlDoc.CreateElement("MainAccount_SubLedgerType");
        //                    childNode10.InnerText = DT.Rows[j]["MainAccount_SubLedgerType"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode10);

        //                    childNode11 = XmlDoc.CreateElement("MainAccount_IsTDS");
        //                    childNode11.InnerText = DT.Rows[j]["MainAccount_IsTDS"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode11);

        //                    childNode12 = XmlDoc.CreateElement("MainAccount_TDSRate");
        //                    childNode12.InnerText = DT.Rows[j]["MainAccount_TDSRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode12);

        //                    childNode13 = XmlDoc.CreateElement("MainAccount_IsFBT");
        //                    childNode13.InnerText = DT.Rows[j]["MainAccount_IsFBT"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode13);

        //                    childNode14 = XmlDoc.CreateElement("MainAccount_FBTRate");
        //                    childNode14.InnerText = DT.Rows[j]["MainAccount_FBTRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode14);

        //                    childNode15 = XmlDoc.CreateElement("MainAccount_RateOfInterest");
        //                    childNode15.InnerText = DT.Rows[j]["MainAccount_RateOfInterest"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode15);

        //                    childNode16 = XmlDoc.CreateElement("MainAccount_ContactID");
        //                    childNode16.InnerText = DT.Rows[j]["MainAccount_ContactID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode16);

        //                    childNode17 = XmlDoc.CreateElement("MainAccount_Nature");
        //                    childNode17.InnerText = DT.Rows[j]["MainAccount_Nature"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode17);

        //                    childNode18 = XmlDoc.CreateElement("CreateDate");
        //                    childNode18.InnerText = DateTime.Now.ToString().Trim();
        //                    parentNode_1.AppendChild(childNode18);

        //                    childNode19 = XmlDoc.CreateElement("CreateUser");
        //                    childNode19.InnerText = HttpContext.Current.Session["userid"].ToString();
        //                    parentNode_1.AppendChild(childNode19);

        //                    string path1 = "..\\Documents\\" + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    XmlDoc.Save(path1.Trim());
        //                    result = "Done";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                result = "Error";
        //            }
        //        }
        //    }

        //    return result;
        //}

        //public string transfertDataToXmlforSubAcc()
        //{
        //    string result = "";
        //    DataTable DT = GetDataTable(" master_subaccount ", " * ", " subaccount_mainacreferenceid in(select mainaccount_referenceid from master_mainaccount where mainaccount_accountcode like 'SYSTM%') ");
        //    if (DT.Rows.Count > 0)
        //    {
        //        for (int j = 0; j < DT.Rows.Count; j++)
        //        {
        //            XmlDocument Xml_sender = new XmlDocument();
        //            XmlDocument XmlDoc = new XmlDocument();
        //            XmlDeclaration XmlDeclaration = XmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        //            XmlDocument xml_receiver = new XmlDocument();

        //            Converter oConverter = new Converter();
        //            //string createdate = DateTime.Now.ToString();
        //            string createdate = oConverter.DateConverter_d_m_y(DateTime.Now.ToString(), "mm/dd/yyyy");
        //            createdate = "D" + createdate.Replace('/', '.');
        //            try
        //            {
        //                string filepath = "..\\Documents\\" + " Sub " + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                if (File.Exists(filepath) == true)
        //                {
        //                    //____Use Existing file______________//
        //                    string path = "..\\Documents\\" + " Sub " + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    Xml_sender.Load(path);

        //                    XmlElement parentNode_1;
        //                    XmlElement childNode1;
        //                    XmlElement childNode2;
        //                    XmlElement childNode3;
        //                    XmlElement childNode4;
        //                    XmlElement childNode5;
        //                    XmlElement childNode6;
        //                    XmlElement childNode7;
        //                    XmlElement childNode8;
        //                    XmlElement childNode9;
        //                    XmlElement childNode10;
        //                    XmlElement childNode11;
        //                    XmlElement childNode12;

        //                    XmlElement rootNode = (XmlElement)Xml_sender.SelectSingleNode("Message");

        //                    parentNode_1 = Xml_sender.CreateElement(createdate);
        //                    rootNode.AppendChild(parentNode_1);

        //                    childNode1 = Xml_sender.CreateElement("SubAccount_ReferenceID");
        //                    childNode1.InnerText = DT.Rows[j]["SubAccount_ReferenceID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode1);

        //                    childNode2 = Xml_sender.CreateElement("SubAccount_MainAcReferenceID");
        //                    childNode2.InnerText = DT.Rows[j]["SubAccount_MainAcReferenceID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode2);

        //                    childNode3 = Xml_sender.CreateElement("SubAccount_Code");
        //                    childNode3.InnerText = DT.Rows[j]["SubAccount_Code"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode3);

        //                    childNode4 = Xml_sender.CreateElement("SubAccount_Name");
        //                    childNode4.InnerText = DT.Rows[j]["SubAccount_Name"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode4);

        //                    childNode5 = Xml_sender.CreateElement("SubAccount_IsTDS");
        //                    childNode5.InnerText = DT.Rows[j]["SubAccount_IsTDS"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode5);

        //                    childNode6 = Xml_sender.CreateElement("SubAccount_TDSRate");
        //                    childNode6.InnerText = DT.Rows[j]["SubAccount_TDSRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode6);

        //                    childNode7 = Xml_sender.CreateElement("SubAccount_IsFBT");
        //                    childNode7.InnerText = DT.Rows[j]["SubAccount_IsFBT"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode7);

        //                    childNode8 = Xml_sender.CreateElement("SubAccount_FBTRate");
        //                    childNode8.InnerText = DT.Rows[j]["SubAccount_FBTRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode8);

        //                    childNode9 = Xml_sender.CreateElement("SubAccount_RateOfInterest");
        //                    childNode9.InnerText = DT.Rows[j]["SubAccount_RateOfInterest"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode9);

        //                    childNode10 = Xml_sender.CreateElement("SubAccount_ContactID");
        //                    childNode10.InnerText = DT.Rows[j]["SubAccount_ContactID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode10);

        //                    childNode11 = Xml_sender.CreateElement("CreateDate");
        //                    childNode11.InnerText = DT.Rows[j]["CreateDate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode11);

        //                    childNode12 = Xml_sender.CreateElement("CreateUser");
        //                    childNode12.InnerText = DT.Rows[j]["CreateUser"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode12);

        //                    string path1 = "..\\Documents\\" + " Sub " + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    Xml_sender.Save(path1.Trim());
        //                    result = "Done";
        //                }
        //                else
        //                {
        //                    //________Creating Xml File_________//

        //                    XmlElement parentNode_1;
        //                    XmlElement childNode1;
        //                    XmlElement childNode2;
        //                    XmlElement childNode3;
        //                    XmlElement childNode4;
        //                    XmlElement childNode5;
        //                    XmlElement childNode6;
        //                    XmlElement childNode7;
        //                    XmlElement childNode8;
        //                    XmlElement childNode9;
        //                    XmlElement childNode10;
        //                    XmlElement childNode11;
        //                    XmlElement childNode12;

        //                    XmlElement rootNode = XmlDoc.CreateElement("Message");
        //                    XmlDoc.InsertBefore(XmlDeclaration, XmlDoc.DocumentElement);
        //                    XmlDoc.AppendChild(rootNode);

        //                    parentNode_1 = XmlDoc.CreateElement(createdate);
        //                    rootNode.AppendChild(parentNode_1);

        //                    childNode1 = XmlDoc.CreateElement("SubAccount_ReferenceID");
        //                    childNode1.InnerText = DT.Rows[j]["SubAccount_ReferenceID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode1);

        //                    childNode2 = XmlDoc.CreateElement("SubAccount_MainAcReferenceID");
        //                    childNode2.InnerText = DT.Rows[j]["SubAccount_MainAcReferenceID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode2);

        //                    childNode3 = XmlDoc.CreateElement("SubAccount_Code");
        //                    childNode3.InnerText = DT.Rows[j]["SubAccount_Code"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode3);

        //                    childNode4 = XmlDoc.CreateElement("SubAccount_Name");
        //                    childNode4.InnerText = DT.Rows[j]["SubAccount_Name"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode4);

        //                    childNode5 = XmlDoc.CreateElement("SubAccount_IsTDS");
        //                    childNode5.InnerText = DT.Rows[j]["SubAccount_IsTDS"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode5);

        //                    childNode6 = XmlDoc.CreateElement("SubAccount_TDSRate");
        //                    childNode6.InnerText = DT.Rows[j]["SubAccount_TDSRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode6);

        //                    childNode7 = XmlDoc.CreateElement("SubAccount_IsFBT");
        //                    childNode7.InnerText = DT.Rows[j]["SubAccount_IsFBT"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode7);

        //                    childNode8 = XmlDoc.CreateElement("SubAccount_FBTRate");
        //                    childNode8.InnerText = DT.Rows[j]["SubAccount_FBTRate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode8);

        //                    childNode9 = XmlDoc.CreateElement("SubAccount_RateOfInterest");
        //                    childNode9.InnerText = DT.Rows[j]["SubAccount_RateOfInterest"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode9);

        //                    childNode10 = XmlDoc.CreateElement("SubAccount_ContactID");
        //                    childNode10.InnerText = DT.Rows[j]["SubAccount_ContactID"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode10);

        //                    childNode11 = XmlDoc.CreateElement("CreateDate");
        //                    childNode11.InnerText = DT.Rows[j]["CreateDate"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode11);

        //                    childNode12 = XmlDoc.CreateElement("CreateUser");
        //                    childNode12.InnerText = DT.Rows[j]["CreateUser"].ToString().Trim();
        //                    parentNode_1.AppendChild(childNode12);

        //                    string path1 = "..\\Documents\\" + " Sub " + HttpContext.Current.Session["userid"].ToString() + ".xml";
        //                    XmlDoc.Save(path1.Trim());
        //                    result = "Done";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                result = "Error";
        //            }
        //        }
        //    }

        //    return result;
        //}

        public void Call_CheckPageaccessebility(string URL)
        {
            HttpCookie ERPACTIVEURL = new HttpCookie("ERPACTIVEURL");
            ERPACTIVEURL.Value = "1";
            HttpContext.Current.Response.Cookies.Add(ERPACTIVEURL);

            if ((HttpContext.Current.Session["userid"] != null) && HttpContext.Current.Session["usergoup"] != null)
            {
                string[] PageName = URL.ToString().Split('/');
                if (PageName[4] != "SignOff.aspx")
                {
                    string pageAccess = CheckPageAccessebility(PageName[PageName.Length - 1].Split('?')[0]); //Code Changed Problem for Pop up Page Master-->Equity
                    if (pageAccess != "N")
                    {

                        string uri = (new Uri(URL, UriKind.Absolute)).PathAndQuery;
                        //  HttpContext.Current.Session["LastLandingUri"] = uri; 
                        HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()] = uri;
                        HttpContext.Current.Session["PageAccess"] = pageAccess;
                        //Session["PageAccess"] = "All";
                    }
                    else
                    {
                        HttpContext.Current.Session["PageAccess"] = "N";
                    }
                }
            }
            else
            {
                string uri = (new Uri(URL, UriKind.Absolute)).PathAndQuery;
                //HttpContext.Current.Response.Redirect("/OMS/Login.aspx?rurl=" + uri);

                // .............................Code Commented and Added by Sam on 29122016.to avoid error during redirect ..................................... 

                //  HttpContext.Current.Response.Redirect("/OMS/Login.aspx?rurl=" + uri);
                //   HttpContext.Current.Response.Redirect("/OMS/Login.aspx?rurl=" + uri, false);

                // .............................Code Above Commented and Added by Sam on 29122016...................................... 
                //..........New Code added by Debjyoti
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Redirect("/OMS/Login.aspx", true);

                //...............End Here

            }
        }

        public void Call_CheckUserSession(string URL)
        {
            if ((HttpContext.Current.Session["userid"] != null) && HttpContext.Current.Session["usergoup"] != null)
            {

            }
            else
            {
                string uri = (new Uri(URL, UriKind.Absolute)).PathAndQuery;
                //HttpContext.Current.Response.Redirect("/OMS/Login.aspx?rurl=" + uri);

                HttpContext.Current.Response.Redirect("/OMS/Login.aspx?rurl=" + uri);
            }
        }

        public DataTable OpeningBalance(
                          String MainAcID,     // Main Account ID
                          String SubAccountID,     // Sub Account ID
                          DateTime Date,          // Date
                          string ExchangeSegmentID,  // ExchangeSegment ID
                          string CompanyID)       // Company ID
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            string lcSql = null;
            int Day = Date.Day;
            int month = Date.Month;
            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid='" + MainAcID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid='" + MainAcID + "' and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid='" + MainAcID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid='" + MainAcID + "') as op";
                }
            }
            else
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + Date + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable OpeningBalance(
                           String MainAcID,     // Main Account ID
                           String SubAccountID,     // Sub Account ID
                           DateTime FDate,          // Date
                           string ExchangeSegmentID,  // ExchangeSegment ID
                           string CompanyID,            // Company ID
                           DateTime TDate)       // Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            string lcSql = null;
            int Day = FDate.Day;
            int month = FDate.Month;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "'  and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "') as op";
                }
            }
            else if (SubAccountID == null && MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "'  and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "') as op";
                }
            }
            else if (MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            else
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable OpeningBalanceOnlyJournal(
                            String MainAcID,     // Main Account ID
                            String SubAccountID,     // Sub Account ID
                            DateTime FDate,          // Date
                            string ExchangeSegmentID,  // ExchangeSegment ID
                            string CompanyID,            // Company ID
                            DateTime TDate)       // Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            String lcSql;
            int Day = FDate.Day;
            int month = FDate.Month;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null && MainAcID != null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and accountsledger_mainaccountid in(" + MainAcID + ")) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid in(" + MainAcID + ")   and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and accountsledger_mainaccountid in(" + MainAcID + ")) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid in(" + MainAcID + ")) as op";
                }
            }
            else if (SubAccountID == null && MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'    and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  ) as op";
                }
            }
            else if (MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            else
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ") and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable OpeningBalance_a(

                          String SubAccountID,     // Sub Account ID
                          DateTime FDate,          // Date

                          DateTime TDate,          // Date
                          string ExchangeSegmentID,  // ExchangeSegment ID
                          string CompanyID)            // Company ID
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            string lcSql = null;
            if (ExchangeSegmentID == "0")
            {
                lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "')";
            }
            else
            {
                lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in('" + ExchangeSegmentID + "') and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "'and AccountsLedger_ExchangeSegmentID in('" + ExchangeSegmentID + "') and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "')";
            }

            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable OpeningBalanceJournal1(
                           String MainAcID,     // Main Account ID
                           String SubAccountID,     // Sub Account ID
                           DateTime FDate,          // Date
                           string ExchangeSegmentID,  // ExchangeSegment ID
                           string CompanyID,            // Company ID
                           DateTime TDate)       // Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            DateTime FinStartDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"].ToString());
            String lcSql;
            SubAccountID = (SubAccountID != null) ? (SubAccountID.Trim() == String.Empty) ? null : SubAccountID : SubAccountID;

            if (SubAccountID == null && MainAcID != null)
            {
                if (FinStartDate == FDate)
                {
                    lcSql = @"Select (Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + @") and isnull(AccountsLedger_SubAccountID,'')=''
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear +
                    "' and AccountsLedger_TransactionDate<='" + TDate + "' and AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") ) as cl,
                (Select sum(accountsledger_amountCr-accountsledger_amountDr) from Trans_AccountsLedger
                where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_CompanyID='" + CompanyID + @"' and
                AccountsLedger_FinYear='" + FinYear + @"' and AccountsLedger_TransactionType like 'OP%' and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + ") ) as op";
                }
                else
                {
                    lcSql = @"Select (Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + @") and isnull(AccountsLedger_SubAccountID,'')=''
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear +
                        "' and AccountsLedger_TransactionDate<='" + TDate + "' and AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") ) as cl,
                (Select sum(accountsledger_amountCr-accountsledger_amountDr) from Trans_AccountsLedger
                where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_CompanyID='" + CompanyID + @"' and
                AccountsLedger_FinYear='" + FinYear + "' and AccountsLedger_TransactionDate<'" + FDate + @"' and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + ") ) as op";
                }
            }
            else
            {
                if (FinStartDate == FDate)
                {
                    lcSql = @"Select (Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + @") and
                AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_CompanyID='" + CompanyID + @"' and
                AccountsLedger_FinYear='" + FinYear + "' and AccountsLedger_TransactionDate<='" + TDate + @"' and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") ) as cl,
                (Select sum(accountsledger_amountCr-accountsledger_amountDr) from Trans_AccountsLedger where
                AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + @"' and
                AccountsLedger_CompanyID='" + CompanyID + @"' and AccountsLedger_TransactionType like 'OP%' and
                AccountsLedger_TransactionDate<'" + FDate + "' and AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + ") ) as op";
                }
                else
                {
                    lcSql = @"Select (Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + @") and
                AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_CompanyID='" + CompanyID + @"' and
                AccountsLedger_FinYear='" + FinYear + "' and AccountsLedger_TransactionDate<='" + TDate + @"' and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") ) as cl,
                (Select sum(accountsledger_amountCr-accountsledger_amountDr) from Trans_AccountsLedger where
                AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + @"' and
                AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"' and
                AccountsLedger_TransactionDate<'" + FDate + "' and AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + ") ) as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable OpeningBalanceOnlyJournalMultipleClient(
                           String MainAcID,     // Main Account ID
                           String SubAccountID,     // Sub Account ID
                           DateTime FDate,          // Date
                           string ExchangeSegmentID,  // ExchangeSegment ID
                           string CompanyID,            // Company ID
                           DateTime TDate, string Selected)       // Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            int month = FDate.Month;
            String lcSql;
            int day = FDate.Day;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (month == FinStartMonth && day == FinStartDay)
            {
                if (Selected == "All")
                {
                    lcSql = "select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1),AccountsLedger_SubAccountID from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' group by AccountsLedger_SubAccountID";
                }
                else
                {
                    lcSql = "select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1),AccountsLedger_SubAccountID from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID in(" + SubAccountID + ") and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' group by AccountsLedger_SubAccountID";
                }
            }
            else
            {
                if (Selected == "All")
                {
                    lcSql = "select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1),AccountsLedger_SubAccountID from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' group by AccountsLedger_SubAccountID order by AccountsLedger_SubAccountID";
                }
                else
                {
                    lcSql = "select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1),AccountsLedger_SubAccountID from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID in(" + SubAccountID + ") and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' group by AccountsLedger_SubAccountID order by AccountsLedger_SubAccountID";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public int insertSignedDocument(String CompanyID, String Segment_OR_DPID, String module, String strDate,
                                          String BranchID, String ContactID_OR_BenAccNumber,
                                             String DocumentType, String DocumentName, String DocumentPath,
                                                   String Signatory, String RecipientEmailID, String LastFinYear, String user, int EmailCreateAppMenuId)
        {
            //using (SqlConnection connn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection connn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand("insert_SignedDocuments", connn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FinancialYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@Segment_OR_DPID", Segment_OR_DPID);
                cmd.Parameters.AddWithValue("@Segment_Name", module);
                cmd.Parameters.AddWithValue("@ContractDate", Convert.ToDateTime(strDate).ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@ContactID_OR_BenAccNumber", ContactID_OR_BenAccNumber.Split('.').GetValue(0));
                cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                cmd.Parameters.AddWithValue("@DocumentName", DocumentName);
                cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@Signatory", Signatory);
                cmd.Parameters.AddWithValue("@RecipientEmailID", RecipientEmailID);
                cmd.Parameters.AddWithValue("@EmailCreateAppMenuId", EmailCreateAppMenuId);

                cmd.CommandTimeout = 0;

                if (connn.State == ConnectionState.Open)
                {
                    connn.Close();
                }

                connn.Open();
                int i = cmd.ExecuteNonQuery();
                connn.Close();

                return i;
            }
        }

        public int insertSignedDocument1(String CompanyID, String Segment_OR_DPID, String module, String strDate, String strDate2,
                                          String BranchID, String ContactID_OR_BenAccNumber,
                                             String DocumentType, String DocumentName, String DocumentPath,
                                                   String Signatory, String RecipientEmailID, String LastFinYear, String user, int EmailCreateAppMenuId)
        {
            //using (SqlConnection connn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection connn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand("insert_quaterlydocuments", connn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FinancialYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@Segment_OR_DPID", Segment_OR_DPID);
                cmd.Parameters.AddWithValue("@Segment_Name", module);
                cmd.Parameters.AddWithValue("@ContractDate", Convert.ToDateTime(strDate).ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@ContractDate2", Convert.ToDateTime(strDate2).ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@ContactID_OR_BenAccNumber", ContactID_OR_BenAccNumber.Split('.').GetValue(0));
                cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                cmd.Parameters.AddWithValue("@DocumentName", DocumentName);
                cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@Signatory", Signatory);
                cmd.Parameters.AddWithValue("@RecipientEmailID", RecipientEmailID);
                cmd.Parameters.AddWithValue("@EmailCreateAppMenuId", EmailCreateAppMenuId);

                cmd.CommandTimeout = 0;

                if (connn.State == ConnectionState.Open)
                {
                    connn.Close();
                }

                connn.Open();
                int i = cmd.ExecuteNonQuery();
                connn.Close();

                return i;
            }
        }

        public int getCount(
            String cTableName,      // TableName from which the count is to be fetched
            String cWhereClause)    // Optional : WHERE condition [if any]
        {
            String lcSql;
            lcSql = "Select Count(*) from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            GetConnection();

            SqlCommand lcmd = new SqlCommand(lcSql, oSqlConnection);

            int obj = (int)lcmd.ExecuteScalar();
            return obj;
        }

        public int insertSignedDocument(String CompanyID, String Segment_OR_DPID,
                                          String BranchID, String ContactID_OR_BenAccNumber,
                                             String DocumentType, String DocumentName, String DocumentPath,
                                                   String Signatory, String RecipientEmailID, String LastFinYear, String user)
        {
            //using (SqlConnection connn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
            using (SqlConnection connn = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                SqlCommand cmd = new SqlCommand("insert_SignedDocuments", connn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FinancialYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@Segment_OR_DPID", Segment_OR_DPID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@ContactID_OR_BenAccNumber", ContactID_OR_BenAccNumber.Split('.').GetValue(0));
                cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                cmd.Parameters.AddWithValue("@DocumentName", DocumentName);
                cmd.Parameters.AddWithValue("@DocumentPath", DocumentPath);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@Signatory", Signatory);
                cmd.Parameters.AddWithValue("@RecipientEmailID", RecipientEmailID);

                cmd.CommandTimeout = 0;

                if (connn.State == ConnectionState.Open)
                {
                    connn.Close();
                }

                connn.Open();
                int i = cmd.ExecuteNonQuery();
                connn.Close();

                return i;
            }
        }

        public void ActivityMailCreation()
        {
            //_Query to filter records
            string lsSql_EmailActivity = "Select * from ( select 'Sales Visit' as ActivityType, A.act_id as ActivityID,A.act_assignedBy as AssignedBy,A.act_assignedTo as AssignedTo,SV.slv_leadcotactId as LeadContactID,  " +
                                  " (select  top 1 eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where  user_id=A.act_assignedTo)and eml_type='Official') as Email, " +
                                  " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=SV.slv_leadcotactId)as Name, " +
                                  " (Select slv_SalesVisitOutcome from tbl_master_salesvisitoutcome where slv_Id=SV.slv_salesvisitoutcome ) as OutCome, " +
                                  " SV.slv_nextvisitdatetime as SheduleDateTime," +
                                  " (select phf_phonenumber from tbl_master_phonefax where phf_cntId=SV.slv_leadcotactId  and phf_type='Mobile')as PhoneNumber, " +
                                  " (Select Top 1 (LTRIM(RTRIM(add_address1))+LTRIM(RTRIM(ISNULL(add_address2,'')))+LTRIM(RTRIM(ISNULL(add_address3,'')))+LTRIM(RTRIM(ISNULL(add_landMark,'')))+ ISNULL((select cou_country from tbl_master_country where cou_id=add_country),'')+', '+ ISNULL((select state from tbl_master_state where id=add_state),'')+', '+ ISNULL((select city_name from tbl_master_city where city_id=add_city),'')+', '+ ISNULL((select area_name  from tbl_master_area where area_id= add_area),'')+', Pin-'+ add_pin) as add2  from tbl_master_address where add_cntId=SV.slv_leadcotactId and add_addressType='Office') as VisitPlace,  " +
                                  " (select dbo.GetOfferedID(A.act_id )) as productName,'' as LastOutcomeDetails  " +
                                  "  from tbl_trans_activies as A ,tbl_trans_salesvisit as SV  " +
                                  "  where A.act_id=SV.slv_activityId and  Convert(varchar(11),slv_nextvisitdatetime,113)= Convert(varchar(11),Getdate()+1,113) and SV.slv_salesvisitoutcome='9' union " +
                                   "  select 'Sales Visit' as ActivityType,A.act_id as ActivityID,A.act_assignedBy as AssignedBy,A.act_assignedTo as AssignedTo,SV.slv_leadcotactId as LeadContactID, " +
                                   " (select  top 1 eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where  user_id=A.act_assignedTo)and eml_type='Official') as Email, " +
                                   " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=SV.slv_leadcotactId)as Name, " +
                                   " (Select slv_SalesVisitOutcome from tbl_master_salesvisitoutcome where slv_Id=SV.slv_salesvisitoutcome ) as OutCome, " +
                                   " SV.slv_nextvisitdatetime as SheduleDateTime,  " +
                                   " (select phf_phonenumber from tbl_master_phonefax where phf_cntId=SV.slv_leadcotactId  and phf_type='Mobile')as PhoneNumber,  " +
                                   " (Select Top 1 (LTRIM(RTRIM(add_address1))+LTRIM(RTRIM(ISNULL(add_address2,'')))+LTRIM(RTRIM(ISNULL(add_address3,'')))+LTRIM(RTRIM(ISNULL(add_landMark,'')))+ ISNULL((select cou_country from tbl_master_country where cou_id=add_country),'')+', '+ ISNULL((select state from tbl_master_state where id=add_state),'')+', '+ ISNULL((select city_name from tbl_master_city where city_id=add_city),'')+', '+ ISNULL((select area_name  from tbl_master_area where area_id= add_area),'')+', Pin-'+ add_pin) as add2  from tbl_master_address where add_id=SVD.slv_VisitPlace) as VisitPlace, " +
                                   " (select dbo.GetOfferedID(A.act_id )) as productName ,SVD.slv_notes as LastOutcomeDetails  " +
                                   "  from tbl_trans_activies as A ,tbl_trans_salesvisit as SV,tbl_trans_SalesVisitDetail as SVD  " +
                                   "  where A.act_id=SV.slv_activityId and  Convert(varchar(11),SV.slv_nextvisitdatetime,113)= Convert(varchar(11),Getdate()+1,113) and SV.slv_salesvisitoutcome !=9 and SV.slv_nextvisitdatetime=SVD.slv_nextVisit Union " +
                //Sales
                                    " select 'Sales' as ActivityType,A.act_id as ActivityID,A.act_assignedBy as AssignedBy,A.act_assignedTo as AssignedTo,S.sls_contactlead_id as LeadContactID,  " +
                                    " (select  top 1 eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where  user_id=A.act_assignedTo)and eml_type='Official') as Email, " +
                                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=S.sls_contactlead_id)as Name, " +
                                    " (select sls_status from tbl_master_SalesStatus where sls_id=S.sls_sales_status) as OutCome,  " +
                                    " S.sls_nextvisitdate as SheduleDateTime,  " +
                                    " (select phf_phonenumber from tbl_master_phonefax where phf_cntId=S.sls_contactlead_id  and phf_type='Mobile')as PhoneNumber," +
                                    " (Select Top 1 (LTRIM(RTRIM(add_address1))+LTRIM(RTRIM(ISNULL(add_address2,'')))+LTRIM(RTRIM(ISNULL(add_address3,'')))+LTRIM(RTRIM(ISNULL(add_landMark,'')))+ ISNULL((select cou_country from tbl_master_country where cou_id=add_country),'')+', '+ ISNULL((select state from tbl_master_state where id=add_state),'')+', '+ ISNULL((select city_name from tbl_master_city where city_id=add_city),'')+', '+ ISNULL((select area_name  from tbl_master_area where area_id= add_area),'')+', Pin-'+ add_pin) as add2  from tbl_master_address where add_cntId=S.sls_contactlead_id and add_addressType='Office') as VisitPlace,  " +
                                    " (select dbo.GetOfferedID(A.act_id )) as productName,  " +
                                    " '' as LastOutcomeDetails " +
                                    "  from tbl_trans_activies as A,  " +
                                    " tbl_trans_sales as S" +
                                    "  where A.act_id=S.sls_activity_id and Convert(varchar(11),S.sls_nextvisitdate,113)= Convert(varchar(11),Getdate()+1,113) and S.sls_sales_status=4 union" +
                                    " select 'Sales' as ActivityType,A.act_id as ActivityID,A.act_assignedBy as AssignedBy,A.act_assignedTo as AssignedTo,S.sls_contactlead_id as LeadContactID, " +
                                    " (select  top 1 eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where  user_id=A.act_assignedTo)and eml_type='Official') as Email, " +
                                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=S.sls_contactlead_id)as Name, " +
                                    "(select sls_status from tbl_master_SalesStatus where sls_id=S.sls_sales_status) as OutCome," +
                                    " S.sls_nextvisitdate as SheduleDateTime,  " +
                                    " (select phf_phonenumber from tbl_master_phonefax where phf_cntId=S.sls_contactlead_id  and phf_type='Mobile')as PhoneNumber," +
                                    " (Select Top 1 (LTRIM(RTRIM(add_address1))+LTRIM(RTRIM(ISNULL(add_address2,'')))+LTRIM(RTRIM(ISNULL(add_address3,'')))+LTRIM(RTRIM(ISNULL(add_landMark,'')))+ ISNULL((select cou_country from tbl_master_country where cou_id=add_country),'')+', '+ ISNULL((select state from tbl_master_state where id=add_state),'')+', '+ ISNULL((select city_name from tbl_master_city where city_id=add_city),'')+', '+ ISNULL((select area_name  from tbl_master_area where area_id= add_area),'')+', Pin-'+ add_pin) as add2  from tbl_master_address where add_cntId=S.sls_contactlead_id) as VisitPlace, " +
                                    " (select dbo.GetOfferedID(A.act_id )) as productName,  " +
                                    " SD.sad_Notes as LastOutcomeDetails  " +
                                    "  from tbl_trans_activies as A,tbl_trans_sales as S,tbl_trans_salesDetails as SD  " +
                                    "  where A.act_id=S.sls_activity_id and Convert(varchar(11),S.sls_nextvisitdate,113)= Convert(varchar(11),Getdate()+1,113) and    S.sls_sales_status !=4 and S.sls_nextvisitdate=SD.sad_nextvisitdate  Union " +
                //Phone call
                                          "  select 'Phone Calls' as ActivityType,A.act_id as ActivityID,A.act_assignedBy as AssignedBy,A.act_assignedTo as AssignedTo,PH.phc_leadcotactId as LeadContactID, " +
                                           " (select  top 1 eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where  user_id=A.act_assignedTo)and eml_type='Official') as Email, " +
                                           " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=PH.phc_leadcotactId)as Name, " +
                                           " (select call_dispositions from tbl_master_callDispositions where call_id=PH.phc_callDispose) as OutCome," +
                                           " A.act_scheduledDate as SheduleDateTime ,  " +
                                           " (select phf_phonenumber from tbl_master_phonefax where phf_cntId=PH.phc_leadcotactId  and phf_type='Mobile')as PhoneNumber, " +
                                           " '' as VisitPlace, " +
                                           " (select dbo.GetOfferedID(A.act_id )) as productName ,'' as  LastOutcomeDetails " +
                                           "  from  tbl_trans_activies as A,tbl_trans_phonecall as  PH " +
                                           "  where A.act_id=PH.phc_activityId and Convert(varchar(11),A.act_scheduledDate,113)= Convert(varchar(11),Getdate()+1,113) and PH.phc_callDispose='11' union  " +
                                            " select 'Phone Calls' as ActivityType, A.act_id as ActivityID, " +
                                           "  A.act_assignedBy as AssignedBy, " +
                                           "  A.act_assignedTo as AssignedTo,PH.phc_leadcotactId as LeadContactID,  " +
                                           " (select  top 1 eml_email from tbl_master_email where eml_cntId in (select user_contactId from tbl_master_user where  user_id=A.act_assignedTo)and eml_type='Official') as Email, " +
                                           " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=PH.phc_leadcotactId)as Name, " +
                                           " (select call_dispositions from tbl_master_callDispositions where call_id=PH.phc_callDispose) as OutCome," +
                                           " PH.phc_nextCall as SheduleDateTime , " +
                                           " (select phf_phonenumber from tbl_master_phonefax where phf_cntId=PH.phc_leadcotactId  and phf_type='Mobile')as PhoneNumber, " +
                                           " '' as VisitPlace, " +
                                           " (select dbo.GetOfferedID(A.act_id )) as productName, " +
                                           " PHD.phd_note as LastOutcomeDetails " +
                                           "  from  tbl_trans_activies as A,tbl_trans_phonecall as  PH ,tbl_trans_phonecalldetails as PHD " +
                                           "  where A.act_id=PH.phc_activityId and Convert(varchar(11),PH.phc_nextCall,113)= Convert(varchar(11),Getdate()+1,113) and PH.Phc_nextCall=PHD.phd_nextCall and PH.phc_callDispose!='11' ) as D Order By AssignedTo";

            GetConnection();
            DataTable DT_ActivitySPH = new DataTable();
            SqlDataAdapter lda = new SqlDataAdapter(lsSql_EmailActivity, oSqlConnection);
            lda.Fill(DT_ActivitySPH);
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("influxcrm@gmail.com", "password");
            //Message.To.Add(new MailAddress(DT_ActivitySPH.Rows[i]["Email"]));
            //Message.CC.Add(new MailAddress("influxcrm@gmail.com"));
            //Message.Subject = "Your Activity Detail for Today";
            string userID = "";
            string ContID = "";
            string mailid = "";
            Message.IsBodyHtml = true;
            Message.Body = "<table cellspacing=\"1\" cellpadding=\"1\" border=\"1\"  style=\"background-color:#dcdcdc\">";
            Message.Body += "<tr><td>Activity Type</td><td>Name of Lead</td><td>Outcome</td><td>Shedule Datetime</td><td>Phone Number</td><td>Visit Place</td><td>Product Name</td><td>Previous Feedback</td></tr>";
            for (int i = 0; i < DT_ActivitySPH.Rows.Count; i++)
            {
                if (userID == "")
                {
                    userID = DT_ActivitySPH.Rows[i]["AssignedTo"].ToString();
                    Message.Body += "<tr  style=\"background-color:#ffffff;\"><td>" + DT_ActivitySPH.Rows[i]["ActivityType"] + "</td><td>" + DT_ActivitySPH.Rows[i]["Name"] + "</td><td>" + DT_ActivitySPH.Rows[i]["OutCome"] + "</td><td>" + DT_ActivitySPH.Rows[i]["SheduleDateTime"] + "</td><td>" + DT_ActivitySPH.Rows[i]["PhoneNumber"] + "</td><td>" + DT_ActivitySPH.Rows[i]["VisitPlace"] + "</td><td>" + DT_ActivitySPH.Rows[i]["productName"] + " </td><td>" + DT_ActivitySPH.Rows[i]["LastOutcomeDetails"] + "</td></tr>";
                }
                else
                {
                    if (DT_ActivitySPH.Rows[i]["AssignedTo"].ToString() == userID)
                    {
                        mailid = DT_ActivitySPH.Rows[i]["Email"].ToString();
                        ContID = DT_ActivitySPH.Rows[i]["LeadContactID"].ToString();
                        userID = DT_ActivitySPH.Rows[i]["AssignedTo"].ToString();
                        Message.Body += "<tr  style=\"background-color:#ffffff;\"><td>" + DT_ActivitySPH.Rows[i]["ActivityType"] + "</td><td>" + DT_ActivitySPH.Rows[i]["Name"] + "</td><td>" + DT_ActivitySPH.Rows[i]["OutCome"] + "</td><td>" + DT_ActivitySPH.Rows[i]["SheduleDateTime"] + "</td><td>" + DT_ActivitySPH.Rows[i]["PhoneNumber"] + "</td><td>" + DT_ActivitySPH.Rows[i]["VisitPlace"] + "</td><td>" + DT_ActivitySPH.Rows[i]["productName"] + " </td><td>" + DT_ActivitySPH.Rows[i]["LastOutcomeDetails"] + "</td></tr>";
                    }
                    else
                    {
                        mailid = DT_ActivitySPH.Rows[i - 1]["Email"].ToString();
                        ContID = DT_ActivitySPH.Rows[i - 1]["LeadContactID"].ToString();
                        userID = DT_ActivitySPH.Rows[i - 1]["AssignedTo"].ToString();
                        Message.Body += "</table>";

                        //String cnn = ConfigurationSettings.AppSettings["DBConnectionDefault"]; --MULTI

                        String cnn = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

                        SqlConnection lcon = new SqlConnection(cnn);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", "noreplay@gmail.com");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", "Todays Activity");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", Message.Body);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", "");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", "");
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        int actid1 = 1235;
                        string fValues = "'" + InternalID + "','" + ContID + "','" + mailid + "','" + "TO" + "','" + Convert.ToDateTime(DateTime.Today) + "','" + "P" + "','" + actid1 + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues);
                        mailid = DT_ActivitySPH.Rows[i]["Email"].ToString();
                        ContID = DT_ActivitySPH.Rows[i]["LeadContactID"].ToString();
                        userID = DT_ActivitySPH.Rows[i]["AssignedTo"].ToString();
                        Message.Body = "";
                        Message.Body = "<table cellspacing=\"1\" cellpadding=\"1\" border=\"1\" style=\"background-color:#dcdcdc\">";
                        Message.Body += "<tr><td>Activity Type</td><td>Name of Lead</td><td>Outcome</td><td>Shedule Datetime</td><td>Phone Number</td><td>Visit Place</td><td>Product Name</td><td>Previous Feedback</td></tr>";
                        Message.Body += "<tr  style=\"background-color:#ffffff;\"><td>" + DT_ActivitySPH.Rows[i]["ActivityType"] + "</td><td>" + DT_ActivitySPH.Rows[i]["Name"] + "</td><td>" + DT_ActivitySPH.Rows[i]["OutCome"] + "</td><td>" + DT_ActivitySPH.Rows[i]["SheduleDateTime"] + "</td><td>" + DT_ActivitySPH.Rows[i]["PhoneNumber"] + "</td><td>" + DT_ActivitySPH.Rows[i]["VisitPlace"] + "</td><td>" + DT_ActivitySPH.Rows[i]["productName"] + " </td><td>" + DT_ActivitySPH.Rows[i]["LastOutcomeDetails"] + "</td></tr>";
                        //mailid = "";
                        //ContID = "";
                        //userID = "";
                    }
                }
            }
            Message.Body += "</table>";


            //String cnn1 = ConfigurationSettings.AppSettings["DBConnectionDefault"]; --MULTI

            String cnn1 = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

            SqlConnection lcon1 = new SqlConnection(cnn1);
            lcon1.Open();
            SqlCommand lcmdEmplInsert1 = new SqlCommand("InsertTransEmail", lcon1);
            lcmdEmplInsert1.CommandType = CommandType.StoredProcedure;
            lcmdEmplInsert1.Parameters.AddWithValue("@Emails_SenderEmailID", "noreplay@gmail.com");
            lcmdEmplInsert1.Parameters.AddWithValue("@Emails_Subject", "Todays Activity");
            lcmdEmplInsert1.Parameters.AddWithValue("@Emails_Content", Message.Body);
            lcmdEmplInsert1.Parameters.AddWithValue("@Emails_HasAttachement", "N");
            lcmdEmplInsert1.Parameters.AddWithValue("@Emails_CreateApplication", "");
            lcmdEmplInsert1.Parameters.AddWithValue("@Emails_CreateUser", "");
            SqlParameter parameter1 = new SqlParameter("@result", SqlDbType.BigInt);
            parameter1.Direction = ParameterDirection.Output;
            lcmdEmplInsert1.Parameters.Add(parameter1);
            lcmdEmplInsert1.ExecuteNonQuery();
            string InternalID1 = parameter1.Value.ToString();
            int actid = 1235;
            string fValues1 = "'" + InternalID1 + "','" + ContID + "','" + mailid + "','" + "TO" + "','" + Convert.ToDateTime(DateTime.Today) + "','" + "P" + "','" + actid + "'";
            InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues1);
            oSqlConnection.Close();
        }

        public DataTable NetFundAdjustment(
                            String MainAcID,     // Main Account ID
                            String SubAccountID,     // Sub Account ID
                            string ExchangeSegmentID,  // ExchangeSegment ID
                            string CompanyID,            // Company ID
                            DateTime TDate)       // Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            String lcSql;
            int day = TDate.Day;
            int month = TDate.Month;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null && MainAcID != null)
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance'  and accountsledger_mainaccountid in(" + MainAcID + ")) as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance'  and accountsledger_mainaccountid in(" + MainAcID + ")) as cl";
                }
            }
            else if (SubAccountID == null && MainAcID == null)
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl";
                }
            }
            else if (MainAcID == null)
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
            }
            else
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable NetFundAdjustment(
                           String MainAcID,     // Main Account ID
                           String SubAccountID,     // Sub Account ID
                           string ExchangeSegmentID,  // ExchangeSegment ID
                            string CompanyID,            // Company ID
                              DateTime FromDate,              // From Date
                                DateTime ToDate)                   // To Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            int month = FromDate.Month;
            String lcSql;
            int day = FromDate.Day;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null && MainAcID != null)
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime)and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and accountsledger_mainaccountid in(" + MainAcID + ")) as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and accountsledger_mainaccountid in(" + MainAcID + ")) as cl";
                }
            }
            else if (SubAccountID == null && MainAcID == null)
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl";
                }
            }
            else if (MainAcID == null)
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
            }
            else
            {
                if (month == FinStartMonth && day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + FromDate + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + ToDate + "')) as datetime) and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and Left(AccountsLedger_TransactionReferenceID,1) not in('X','Y','Z') and   accountsledger_TransactionType<>'OpeningBalance' and AccountsLedger_CompanyID='" + CompanyID + "') as cl";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        //Send Email....Generic function

        public Boolean SendReport(string emailbdy, string contactid, string billdate, string Subject)
        {
            //   DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                DataTable dt1 = GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                string mailid = "";
                string ccmail = "";
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    mailid = Convert.ToString(dt1.Rows[0]["eml_email"]);
                    ccmail = Convert.ToString(dt1.Rows[0]["eml_ccemail"]);
                }

                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + contactid + "' ");
                    string ClientName = dtname.Rows[0]["ClientName"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //  SqlConnection lcon = new SqlConnection(con);
                    // using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + contactid + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }
                if (ccmail != "")
                {
                    if (mailid != "")
                    {
                        string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                        DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                        string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                        DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                        string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                        DataTable dtname = GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + contactid + "' ");
                        string ClientName = dtname.Rows[0]["ClientName"].ToString();

                        string senderemail = "";
                        string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                        if (data[0, 0] != "n")
                        {
                            senderemail = data[0, 0];
                        }

                        //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        //  SqlConnection lcon = new SqlConnection(con);
                        //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                        using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                        {
                            lcon.Open();
                            SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                            lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                            SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                            parameter.Direction = ParameterDirection.Output;
                            lcmdEmplInsert.Parameters.Add(parameter);
                            lcmdEmplInsert.ExecuteNonQuery();
                            string InternalID = parameter.Value.ToString();
                            //  ###########---recipients-----------------

                            //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                            //string mailid = dt1.Rows[0]["eml_email"].ToString();

                            string fValues3 = "'" + InternalID + "','" + contactid + "','" + ccmail + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                            InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportdemat(string emailbdy, string mailid, string billdate, string Subject, string branchContact)
        {
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }

            try
            {
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        // SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + branchContact + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //Send Mail Branch/Group wise
        public Boolean SendReportBranchWise(string emailbdy, string branchid, string billdate, string Subject, string strType)
        {
            //   DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                string mailid = "";
                string ccmail = "";
                string typename = "";
                if (strType == "Branch")
                {
                    DataTable dt1 = GetDataTable(" tbl_master_branch ", "top 1 *", "branch_id='" + branchid + "'");

                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        mailid = Convert.ToString(dt1.Rows[0]["branch_cpEmail"]).Trim();
                        typename = Convert.ToString(dt1.Rows[0]["branch_description"]);
                    }
                }
                else if (strType == "Group")
                {
                    DataTable dt1 = GetDataTable(" tbl_master_groupmaster ", "top 1 *", "gpm_id='" + branchid + "'");

                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        mailid = Convert.ToString(dt1.Rows[0]["gpm_emailid"]).Trim();
                        typename = Convert.ToString(dt1.Rows[0]["GPM_DESCRIPTION"]);
                    }
                }

                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    //DataTable dtname = GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + contactid + "' ");
                    //string ClientName = dtname.Rows[0]["ClientName"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //  SqlConnection lcon = new SqlConnection(con);
                    // using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + typename + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + branchid + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportBrportfolio(string emailbdy, string mailid, string billdate, string Subject, string branchContact)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI

            DBEngine oDBEngine = new DBEngine(Convert.ToString(HttpContext.Current.Session["ErpConnection"]));
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                //DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                //string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();
                if (mailid != "")
                {
                    DataTable dtnew = GetDataTable("tbl_master_branch", "branch_cpemail", "rtrim(ltrim(branch_description)) +'['+rtrim(ltrim(branch_code))+']' = '" + mailid.ToString().Trim() + "'");
                    mailid = dtnew.Rows[0]["branch_cpemail"].ToString();
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    // SqlConnection lcon = new SqlConnection(con);
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + branchContact + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }

                DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                if (dt1.Rows.Count > 0)
                {
                    string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();

                    if (ccmail != "")
                    {
                        if (mailid != "")
                        {
                            string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                            DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                            string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                            DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                            string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                            DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                            string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                            string senderemail = "";
                            string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                            if (data[0, 0] != "n")
                            {
                                senderemail = data[0, 0];
                            }

                            //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            //  SqlConnection lcon = new SqlConnection(con);
                            // using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                            using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                            {
                                lcon.Open();
                                SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                                lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                                SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                                parameter.Direction = ParameterDirection.Output;
                                lcmdEmplInsert.Parameters.Add(parameter);
                                lcmdEmplInsert.ExecuteNonQuery();
                                string InternalID = parameter.Value.ToString();
                                //  ###########---recipients-----------------

                                //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                                //string mailid = dt1.Rows[0]["eml_email"].ToString();

                                string fValues3 = "'" + InternalID + "','" + branchContact + "','" + ccmail + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                                InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportBrportfoliogroup(string emailbdy, string mailid, string billdate, string Subject, string branchContact)
        {
            // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            DBEngine oDBEngine = new DBEngine(Convert.ToString(HttpContext.Current.Session["ErpConnection"]));

            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                //DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                //string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();
                //if (mailid != "")
                DataTable dtnew = GetDataTable("tbl_master_groupmaster", "gpm_emailid", "gpm_id='" + branchContact + "'");

                if (dtnew.Rows[0]["gpm_emailid"].ToString().Trim().Length > 0)
                {
                    //DataTable dtnew = GetDataTable("tbl_master_groupmaster", "gpm_emailid", "gpm_id='" + branchContact + "'");
                    mailid = dtnew.Rows[0]["gpm_emailid"].ToString();
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    // SqlConnection lcon = new SqlConnection(con);
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI
                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + branchContact + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }

                DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                if (dt1.Rows.Count > 0)
                {
                    string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();

                    if (ccmail != "")
                    {
                        if (mailid != "")
                        {
                            string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                            DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                            string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                            DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                            string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                            DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                            string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                            string senderemail = "";
                            string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                            if (data[0, 0] != "n")
                            {
                                senderemail = data[0, 0];
                            }

                            //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            //  SqlConnection lcon = new SqlConnection(con);
                            //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                            using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                            {
                                lcon.Open();
                                SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                                lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                                SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                                parameter.Direction = ParameterDirection.Output;
                                lcmdEmplInsert.Parameters.Add(parameter);
                                lcmdEmplInsert.ExecuteNonQuery();
                                string InternalID = parameter.Value.ToString();
                                //  ###########---recipients-----------------

                                //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                                //string mailid = dt1.Rows[0]["eml_email"].ToString();

                                string fValues3 = "'" + InternalID + "','" + branchContact + "','" + ccmail + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                                InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportSt(string emailbdy, string contactid, string billdate, string Subject)
        {
            //  DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                DataTable dt1 = GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                string mailid = dt1.Rows[0]["eml_email"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        // SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + contactid + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportSt1(string emailbdy, string contactid, string billdate, string Subject)
        {
            //  DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                DataTable dt1 = GetDataTable(" tbl_master_branch ", "top 1 *", "branch_cpemail='" + contactid + "' ");
                string mailid = dt1.Rows[0]["branch_cpemail"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        // SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportSt2(string emailbdy, string contactid, string billdate, string Subject)
        {
            //  DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                DataTable dt1 = GetDataTable(" tbl_master_groupmaster ", "top 1 *", "gpm_emailid='" + contactid + "' ");
                string mailid = dt1.Rows[0]["gpm_emailid"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        // SqlConnection lcon = new SqlConnection(con);
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportSegNew(string emailbdy, string contactid, string Subject, string segment)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            DBEngine oDBEngine = new DBEngine(Convert.ToString(HttpContext.Current.Session["ErpConnection"])); //MULTI

            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            //// DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5].ToString().Split('?')[0] + "%' and mnu_segmentid=(select seg_id from tbl_master_segment where seg_name='" + segment + "')");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                string mailid = dt1.Rows[0]["eml_email"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    //DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    //string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; --MULTI
                    String con = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segment + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segment);
                    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                    parameter.Direction = ParameterDirection.Output;
                    lcmdEmplInsert.Parameters.Add(parameter);
                    lcmdEmplInsert.ExecuteNonQuery();
                    string InternalID = parameter.Value.ToString();
                    //  ###########---recipients-----------------

                    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                    string fValues3 = "'" + InternalID + "','" + contactid + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportBrNew(string emailbdy, string mailid, string Subject, string branchContact, string segment)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            DBEngine oDBEngine = new DBEngine(Convert.ToString(HttpContext.Current.Session["ErpConnection"]));

            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            ////DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5].ToString().Split('?')[0] + "%' and mnu_segmentid=(select seg_id from tbl_master_segment where seg_name='" + segment + "')");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //string mailid = dt1.Rows[0]["eml_email"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    //// DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id=(select seg_id from tbl_master_segment where seg_name='" + segment + "')");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    // String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];

                    String con = Convert.ToString(HttpContext.Current.Session["ErpConnection"]);

                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                    parameter.Direction = ParameterDirection.Output;
                    lcmdEmplInsert.Parameters.Add(parameter);
                    lcmdEmplInsert.ExecuteNonQuery();
                    string InternalID = parameter.Value.ToString();
                    //  ###########---recipients-----------------

                    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                    string fValues3 = "'" + InternalID + "','" + branchContact + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportBr(string emailbdy, string mailid, string billdate, string Subject, string branchContact)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DBEngine oDBEngine = new DBEngine(Convert.ToString(HttpContext.Current.Session["ErpConnection"]));


            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                //DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                //string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    // SqlConnection lcon = new SqlConnection(con);
                    //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + branchContact + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }

                DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                if (dt1.Rows.Count > 0)
                {
                    string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();

                    if (ccmail != "")
                    {
                        if (mailid != "")
                        {
                            string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                            DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                            string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                            DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                            string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                            DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                            string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                            string senderemail = "";
                            string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                            if (data[0, 0] != "n")
                            {
                                senderemail = data[0, 0];
                            }

                            //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            //  SqlConnection lcon = new SqlConnection(con);
                            //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                            using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                            {
                                lcon.Open();
                                SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                                lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                                SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                                parameter.Direction = ParameterDirection.Output;
                                lcmdEmplInsert.Parameters.Add(parameter);
                                lcmdEmplInsert.ExecuteNonQuery();
                                string InternalID = parameter.Value.ToString();
                                //  ###########---recipients-----------------

                                //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                                //string mailid = dt1.Rows[0]["eml_email"].ToString();

                                string fValues3 = "'" + InternalID + "','" + branchContact + "','" + ccmail + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                                InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Boolean SendReportBrpayout(string emailbdy, string mailid, string billdate, string Subject, string branchContact, string forgroupby, string last)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            DBEngine oDBEngine = new DBEngine(Convert.ToString(HttpContext.Current.Session["ErpConnection"]));

            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();
            }
            else
            {
                menuId = "";
            }
            try
            {
                //DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "top 1 *", "gpm_emailID='" + mailid.ToString().Trim() + "' ");
                //string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();
                if (mailid != "")
                {
                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                    string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                    string senderemail = "";
                    string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];
                    }

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                    // SqlConnection lcon = new SqlConnection(con);
                    // using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                    using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                    {
                        lcon.Open();
                        SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                        lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                        lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                        SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                        parameter.Direction = ParameterDirection.Output;
                        lcmdEmplInsert.Parameters.Add(parameter);
                        lcmdEmplInsert.ExecuteNonQuery();
                        string InternalID = parameter.Value.ToString();
                        //  ###########---recipients-----------------

                        //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                        //string mailid = dt1.Rows[0]["eml_email"].ToString();

                        string fValues3 = "'" + InternalID + "','" + branchContact + "','" + mailid + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                        InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    }
                }

                DataTable dt1 = oDBEngine.GetDataTable(" TBL_MASTER_GROUPMASTER ", "gpm_ccemailid", "gpm_code = '" + last + "'");
                if (dt1.Rows.Count > 0)
                {
                    string ccmail = dt1.Rows[0]["gpm_ccemailID"].ToString();

                    if (ccmail != "")
                    {
                        if (mailid != "")
                        {
                            string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                            DataTable dtcmp = GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                            string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                            DataTable dtsg = GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                            string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                            DataTable dtname = GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                            string CompanyName = dtname.Rows[0]["cmp_Name"].ToString();

                            string senderemail = "";
                            string[,] data = GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                            if (data[0, 0] != "n")
                            {
                                senderemail = data[0, 0];
                            }

                            //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            //  SqlConnection lcon = new SqlConnection(con);
                            //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"])) MULTI

                            using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                            {
                                lcon.Open();
                                SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                                lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Company Name: " + CompanyName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                                SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                                parameter.Direction = ParameterDirection.Output;
                                lcmdEmplInsert.Parameters.Add(parameter);
                                lcmdEmplInsert.ExecuteNonQuery();
                                string InternalID = parameter.Value.ToString();
                                //  ###########---recipients-----------------

                                //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                                //string mailid = dt1.Rows[0]["eml_email"].ToString();

                                string fValues3 = "'" + InternalID + "','" + branchContact + "','" + ccmail + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                                InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void SendattachmentMail(string TargetIds, string sendermail, DataTable attachmentDetails, string subject, string message, string CC, string BCc, string template)
        {
            string rtn = "";
            MailMessage Message = new MailMessage();
            MailAddressCollection mailAdds = new MailAddressCollection();

            string[] idWlist = TargetIds.Split(',');
            string mailaddress = "";
            string mailname = "";
            for (int i = 0; i < idWlist.Length; i++)
            {
                string[] idWname = idWlist[i].Split('<');
                if (idWname.Length == 1)
                {
                    mailaddress = idWname[0];
                    mailname = "";
                }
                else
                {
                    mailaddress = idWname[1].Substring(0, idWname[1].Length - 1);
                    mailname = idWname[0].Trim();
                }
                Message.To.Add(new MailAddress(mailaddress, mailname));
            }

            Message.From = new MailAddress(sendermail);

            //_____adding CC & BCc email ids____//

            if (CC != "")
            {
                string[] idWlist1 = CC.Split(',');
                string mailaddress1 = "";
                string mailname1 = "";
                for (int i = 0; i < idWlist1.Length; i++)
                {
                    string[] idWname = idWlist1[i].Split('<');
                    if (idWname.Length == 1)
                    {
                        mailaddress1 = idWname[0];
                        mailname1 = "";
                    }
                    else
                    {
                        mailaddress1 = idWname[1].Substring(0, idWname[1].Length - 1);
                        mailname1 = idWname[0].Trim();
                    }
                    Message.CC.Add(new MailAddress(mailaddress1, mailname1));
                }
            }
            if (BCc != "")
            {
                string[] idWlist2 = BCc.Split(',');
                string mailaddress2 = "";
                string mailname2 = "";
                for (int i = 0; i < idWlist2.Length; i++)
                {
                    string[] idWname = idWlist2[i].Split('<');
                    if (idWname.Length == 1)
                    {
                        mailaddress2 = idWname[0];
                        mailname2 = "";
                    }
                    else
                    {
                        mailaddress2 = idWname[1].Substring(0, idWname[1].Length - 1);
                        mailname2 = idWname[0].Trim();
                    }
                    Message.Bcc.Add(new MailAddress(mailaddress2, mailname2));
                }
            }

            //______________End CC &BCc_________//

            Message.Subject = subject;
            if (attachmentDetails != null)
            {
                for (int i = 0; i < attachmentDetails.Rows.Count; i++)
                {
                    Message.Attachments.Add(new Attachment(attachmentDetails.Rows[i]["filepathServer"].ToString().Trim()));
                }
            }

            Message.IsBodyHtml = true;

            Message.Body = "<table border='0' + cellpadding='3' cellspacing='3' width= '100%' font-weight= 'normal' font-size= '8pt'  color= 'black' font-style= normal font-family= 'Verdana'>";
            Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >    </td></tr>";
            Message.Body += "<tr><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align = 'left' > " + message + " </td></tr>";
            //Message.Body += "<tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '>   -: Generated By smifsCRM. :- </td></tr>";
            Message.Body += "</table>";

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("selfservice@influxerp.com", "123influx456");
            client.EnableSsl = true;
            //InsurtFieldValue(" tbl_trans_email ", " hem_receipient = '" + emailsender[0, 0].ToString() + "'", " user_loginId='" + userId + "'");
            try
            {
                client.Send(Message);
                Message.Dispose();
            }
            catch (Exception ex)
            {
                rtn = ex.Message;
                Message.Dispose();
            }
        }

        public string getLastActivity(string BranchId, string ListOfUser, int Count)
        {
            if (Count == 0)
                actual = "";
            Count = 1;
            DataTable DtSecond = GetDataTable(" tbl_trans_menu ", " mun_parentid,mnu_menuName ", " mnu_id= " + BranchId);
            if (DtSecond.Rows.Count != 0)
            {
                for (int i = 0; i < DtSecond.Rows.Count; i++)
                {
                    ListOfUser += DtSecond.Rows[i][0].ToString() + ",";
                    actual += DtSecond.Rows[i][1].ToString() + ",";
                    getLastActivity(DtSecond.Rows[i][0].ToString(), ListOfUser, Count);
                }
            }
            return actual;
        }

        /// <summary>
        /// New Features in DBEngine
        /// </summary>

        public string GenerateGenericTemplate(string TepmlateID, string contactID)
        {
            DataSet dsCnt = new DataSet();
            DataTable dtCnt = new DataTable();
            // using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]))  MULTI
            using (SqlConnection con = new SqlConnection(Convert.ToString(HttpContext.Current.Session["ErpConnection"])))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_TemplateReservedWord", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@TemplateID", TepmlateID);
                    da.SelectCommand.Parameters.AddWithValue("@ContactID", contactID);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    dsCnt.Reset();
                    da.Fill(dsCnt);
                    dtCnt = dsCnt.Tables[0];
                }
            }
            DataTable dtTemp = GetDataTable("master_templateDetails", " TMPLT_USEDFOR,TMPLT_CONTENT  ", "Tmplt_ID='" + TepmlateID + "'");
            string TempContent = string.Empty;
            if (dtTemp.Rows.Count > 0)
            {
                //if(dtTemp.Rows[0]["Tmplt_usedFor"].ToString()=="CL")
                //{
                if (dtCnt.Rows.Count > 0)
                {
                    TempContent = dtTemp.Rows[0]["TMPLT_CONTENT"].ToString();
                    if (dtTemp.Rows[0]["TMPLT_USEDFOR"].ToString() == "CL" || dtTemp.Rows[0]["TMPLT_USEDFOR"].ToString() == "EM")
                    {
                        // DataTable dtCnt = GetDataTable("tbl_master_contact ", " isnull(ltrim(rtrim(cnt_firstname)),'') as FirstName,isnull(ltrim(rtrim(cnt_middlename)),'') as MiddleName,isnull(ltrim(rtrim(cnt_lastname)),'') as LastName,isnull(ltrim(rtrim(cnt_ucc)),'') as [ClientCode],(select top 1 isnull(ltrim(rtrim(add_address1)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres1,(select top 1 isnull(ltrim(rtrim(add_address2)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres2,(select top 1 isnull(ltrim(rtrim(add_address3)),'') from tbl_master_address where add_cntId=cnt_internalID) as Addres3,(select top 1 isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id in (select top 1 add_City from tbl_master_address where add_cntId=cnt_internalID)) as City,(select  top 1 isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id in (select top 1 add_state from tbl_master_address where add_cntId=cnt_internalID)) as State,(select top 1 isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id in (select top 1 add_country from tbl_master_address where add_cntId=cnt_internalID)) as Country,(select  top 1 isnull(ltrim(rtrim(add_pin)),'') from tbl_master_address where add_cntId=cnt_internalID) as Pin,(select top 1  isnull(ltrim(rtrim(phf_countryCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [ISDCode],(select top 1  isnull(ltrim(rtrim(phf_areaCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [STDCode],(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type<>'Mobile') as [TelephoneNumber],(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type='Mobile') as [MobNumber],REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-')  as DateOfBirth,(select top 1 REPLACE(CONVERT(VARCHAR(11), crg_regisDate, 106), ' ', '-') from tbl_master_contactExchange  where crg_cntID=cnt_internalID) as ClientAgrementDate,(select top 1 crg_Number from tbl_master_contactRegistration where crg_cntid=cnt_internalID and crg_type='Pancard') as PANNumber,convert(varchar,getdate(),106) as CurrentDate ", " cnt_internalid ='" + contactID + "'");
                        TempContent = TempContent.ToString().Replace("#Salutation#", dtCnt.Rows[0]["Salutation"].ToString());
                        TempContent = TempContent.ToString().Replace("#FirstName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#MiddleName#", dtCnt.Rows[0]["MiddleName"].ToString());
                        TempContent = TempContent.ToString().Replace("#LastName#", dtCnt.Rows[0]["LastName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#State#", dtCnt.Rows[0]["State"].ToString());
                        TempContent = TempContent.ToString().Replace("#Country#", dtCnt.Rows[0]["Country"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#ISDCode#", dtCnt.Rows[0]["ISDCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#STDCode#", dtCnt.Rows[0]["STDCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#MobNumber#", dtCnt.Rows[0]["MobNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#DateOfBirth#", dtCnt.Rows[0]["DateOfBirth"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientAgrementDate#", dtCnt.Rows[0]["ClientAgrementDate"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                    }
                    else if (dtTemp.Rows[0]["TMPLT_USEDFOR"].ToString() == "CD")
                    {
                        TempContent = TempContent.ToString().Replace("#ClientName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#State#", dtCnt.Rows[0]["State"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                    }
                    else if (dtTemp.Rows[0]["TMPLT_USEDFOR"].ToString() == "ND")
                    {
                        TempContent = TempContent.ToString().Replace("#ClientName#", dtCnt.Rows[0]["FirstName"].ToString());
                        TempContent = TempContent.ToString().Replace("#ClientID#", dtCnt.Rows[0]["ClientCode"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres1#", dtCnt.Rows[0]["Addres1"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres2#", dtCnt.Rows[0]["Addres2"].ToString());
                        TempContent = TempContent.ToString().Replace("#Addres3#", dtCnt.Rows[0]["Addres3"].ToString());
                        TempContent = TempContent.ToString().Replace("#City#", dtCnt.Rows[0]["City"].ToString());
                        TempContent = TempContent.ToString().Replace("#Pin#", dtCnt.Rows[0]["Pin"].ToString());
                        TempContent = TempContent.ToString().Replace("#TelephoneNumber#", dtCnt.Rows[0]["TelephoneNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#PANNumber#", dtCnt.Rows[0]["PANNumber"].ToString());
                        TempContent = TempContent.ToString().Replace("#CurrentDate#", dtCnt.Rows[0]["CurrentDate"].ToString());
                    }
                }

                // }
            }

            return TempContent;
        }

        #region GetDate 06-March-2012 (Around 12:50)

        public DateTime GetDate()
        {
            DateTime Date = new DateTime();
            Date = Convert.ToDateTime("1900-01-01");
            string Query = "Select Getdate()";
            SqlDataReader Dr = proc.GetReader(Query);
            try
            {
                while (Dr.Read())
                    if (Dr[0] != null)
                        Date = Convert.ToDateTime(Dr[0].ToString());
                Dr.Close();
            }
            catch { Dr.Close(); }
            finally { Dr.Close(); }
            return Date;
        }

        public string GetDate(int StringFormatNumericValue)
        {
            string Date;
            Date = String.Empty;
            string Query = "Select Convert(Varchar,Getdate()," + StringFormatNumericValue.ToString().Trim() + ")";
            SqlDataReader Dr = proc.GetReader(Query);
            try
            {
                while (Dr.Read())
                    if (Dr[0] != null)
                        Date = Dr[0].ToString();
                Dr.Close();
            }
            catch { Dr.Close(); }
            finally { Dr.Close(); }
            return Date;
        }

        //Convert UTC DateTime To DataBase DateTime Format
        public string GetDate(Enum DateConvertFrom, string DateString)
        {
            string Date;
            Date = String.Empty;
            string Query = String.Empty;
            if (DateString.Trim() != String.Empty && DateString != null)
            {
                if (DateConvertFrom.ToString() == "UTCToDateTime")
                {
                    Query = @"SELECT
            CAST(SUBSTRING('" + DateString + "', 5, CHARINDEX('UTC','" + DateString + "',1)-6) + SUBSTRING('" + DateString + "',CHARINDEX('UTC','" + DateString + "',1)+8,8000) AS DATETIME)";
                }
                else if (DateConvertFrom.ToString() == "UTCToOnlyDate")
                {
                    Query = @"Select (DATEADD(dd, 0, DATEDIFF(dd, 0, CAST(SUBSTRING('" + DateString + "', 5, CHARINDEX('UTC','" + DateString + "',1)-6) + SUBSTRING('" + DateString + "',CHARINDEX('UTC','" + DateString + "',1)+8,8000) AS DATETIME))))";
                }
                else if (DateConvertFrom.ToString() == "UTCToOnlyTime")
                {
                    Query = @"SELECT
            Convert(Varchar(8),CAST(SUBSTRING('" + DateString + "', 5, CHARINDEX('UTC','" + DateString + "',1)-6) + SUBSTRING('" + DateString + "',CHARINDEX('UTC','" + DateString + "',1)+8,8000) AS DATETIME),108) [DateTime]";
                }
                SqlDataReader Dr = proc.GetReader(Query);
                try
                {
                    while (Dr.Read())
                        if (Dr[0] != null)
                            Date = Dr[0].ToString();
                    Dr.Close();
                }
                catch { Dr.Close(); }
                finally { Dr.Close(); }
            }
            return Date;
        }

        #endregion GetDate 06-March-2012 (Around 12:50)

        public void ClearAllSession()
        {
            HttpContext.Current.Session["userid"] = null;
            HttpContext.Current.Session["username"] = null;
            HttpContext.Current.Session["userpassword"] = null;
            HttpContext.Current.Session["usercontactID"] = null;
            HttpContext.Current.Session["userbranchID"] = null;
            HttpContext.Current.Session["usergoup"] = null;
            HttpContext.Current.Session["userlastsegment"] = null;
            HttpContext.Current.Session["userContactType"] = null;
            HttpContext.Current.Session["TimeForTickerDisplay"] = null;
            HttpContext.Current.Session["EmployeeID"] = null;
            HttpContext.Current.Session["EntryProfileType"] = null;
            HttpContext.Current.Session["LastCompany"] = null;
            HttpContext.Current.Session["LastFinYear"] = null;
            HttpContext.Current.Session["LastSettNo"] = null;
        }

        #region Currency Settings For Multi Currency Project

        public DataTable OpeningBalance(
                          String MainAcID,     // Main Account ID
                          String SubAccountID,     // Sub Account ID
                          DateTime FDate,          // Date
                          string ExchangeSegmentID,  // ExchangeSegment ID
                          string CompanyID,            // Company ID
                          DateTime TDate,
                          int CurrencyID,//Active Currency
                          int TradeCurrencyID)// Trade Currency
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            string lcSql = null;
            int Day = FDate.Day;
            int month = FDate.Month;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "'  and AccountsLedger_TransactionType='OpeningBalance' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
            }
            else if (SubAccountID == null && MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "'  and AccountsLedger_TransactionType='OpeningBalance' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType='Cash_Bank' and accountsledger_mainaccountid='" + MainAcID + "'  and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_TransactionType<>'journal' and accountsledger_mainaccountid='" + MainAcID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
            }
            else if (MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and AccountsLedger_TransactionType='OpeningBalance' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
            }
            else
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + " and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and AccountsLedger_TransactionType='OpeningBalance' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_TransactionType='Cash_Bank' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + " and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID='" + MainAcID + "' and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_TransactionType<>'journal' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and isnull(accountsLedger_Currency," + TradeCurrencyID + ")=" + CurrencyID.ToString().Trim() + ") as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        /// <summary>
        /// Currency Also Include In this function
        /// </summary>
        /// <param name="MainAcID"></param>
        /// <param name="SubAccountID"></param>
        /// <param name="FDate"></param>
        /// <param name="ExchangeSegmentID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="TDate"></param>
        /// <returns></returns>
        public DataTable OpeningBalanceJournal1(
                        String MainAcID,     // Main Account ID
                        String SubAccountID,     // Sub Account ID
                        DateTime FDate,          // Date
                        string ExchangeSegmentID,  // ExchangeSegment ID
                        string CompanyID,            // Company ID
                        DateTime TDate,
                       int CurrencyID)       // Date
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            DateTime FinStartDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"].ToString());
            string TradeCurrency = HttpContext.Current.Session["ActiveCurrency"].ToString().Split('~')[0];
            String lcSql;
            SubAccountID = (SubAccountID != null) ? (SubAccountID.Trim() == String.Empty) ? null : SubAccountID : SubAccountID;
            if (SubAccountID == null && MainAcID != null)
            {
                if (FinStartDate == FDate)
                {
                    lcSql = @"Select isnull((Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + @") and isnull(AccountsLedger_SubAccountID,'')=''
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"' and
                AccountsLedger_TransactionDate<='" + TDate + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + @")
                and   isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + @"),'0.00') as cl,
                isnull((Select sum(accountsledger_amountCr-accountsledger_amountDr)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_CompanyID='" + CompanyID + @"'
                and AccountsLedger_FinYear='" + FinYear + @"'  and AccountsLedger_TransactionType like 'OP%'  and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") and
                isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + "),'0.00') as op";
                }
                else
                {
                    lcSql = @"Select isnull((Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + @") and isnull(AccountsLedger_SubAccountID,'')=''
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"' and
                AccountsLedger_TransactionDate<='" + TDate + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + @")
                and   isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + @"),'0.00') as cl,
                isnull((Select sum(accountsledger_amountCr-accountsledger_amountDr)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_CompanyID='" + CompanyID + @"'
                and AccountsLedger_FinYear='" + FinYear + "' and AccountsLedger_TransactionDate<'" + FDate + @"' and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") and
                isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + "),'0.00') as op";
                }
            }
            else
            {
                if (FinStartDate == FDate)
                {
                    lcSql = @"Select isnull((Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from
                Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + @"'
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"' and
                AccountsLedger_TransactionDate<='" + TDate + "' and AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @")
                and   isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + @"),'0.00') as cl,
                isnull((Select sum(accountsledger_amountCr-accountsledger_amountDr)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + @"'
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"'
                and AccountsLedger_TransactionType like 'OP%'  and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") and
                isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + @"),'0.00') as op";
                }
                else
                {
                    lcSql = @"Select isnull((Select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from
                Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + @"'
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"' and
                AccountsLedger_TransactionDate<='" + TDate + "' and AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @")
                and   isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + @"),'0.00') as cl,
                isnull((Select sum(accountsledger_amountCr-accountsledger_amountDr)
                from Trans_AccountsLedger where AccountsLedger_MainAccountID in (" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + @"'
                and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_FinYear='" + FinYear + @"' and
                AccountsLedger_TransactionDate<'" + FDate + @"' and
                AccountsLedger_ExchangeSegmentID in (" + ExchangeSegmentID + @") and
                isnull(AccountsLedger_Currency," + TradeCurrency + ")=" + CurrencyID + @"),'0.00') as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable OpeningBalanceOnlyJournal(
                               String MainAcID,     // Main Account ID
                               String SubAccountID,     // Sub Account ID
                               DateTime FDate,          // Date
                               string ExchangeSegmentID,  // ExchangeSegment ID
                               string CompanyID,            // Company ID
                               DateTime TDate,    // Date
                            int CurrencyID)
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table
            //First value for Closing and Second Value for Opening
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            String lcSql;
            int Day = FDate.Day;
            int month = FDate.Month;

            string FinStartDate = HttpContext.Current.Session["FinYearStart"].ToString();
            int FinStartDay = Convert.ToDateTime(FinStartDate).Day;
            int FinStartMonth = Convert.ToDateTime(FinStartDate).Month;

            if (SubAccountID == null && MainAcID != null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and accountsledger_mainaccountid in(" + MainAcID + ")) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid in(" + MainAcID + ")   and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  and accountsledger_mainaccountid in(" + MainAcID + ")) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and accountsledger_mainaccountid in(" + MainAcID + ")) as op";
                }
            }
            else if (SubAccountID == null && MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'    and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' ) as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'  ) as op";
                }
            }
            else if (MainAcID == null)
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where  AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            else
            {
                if (month == FinStartMonth && Day == FinStartDay)
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_TransactionDate<='" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "'   and AccountsLedger_TransactionType='OpeningBalance') as op";
                }
                else
                {
                    if (SubAccountID.ToString() != "")
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ") and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                    else
                        lcSql = "select (select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ")  and AccountsLedger_SubAccountID='" + SubAccountID + "' and AccountsLedger_TransactionDate<='" + TDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "' and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as cl,(select convert(varchar(50),cast(isnull(sum(accountsledger_amountCr-accountsledger_amountDr),0) as money),1) from trans_accountsledger where accountsledger_MainAccountID in(" + MainAcID + ") and AccountsLedger_TransactionDate<'" + FDate + "' and ltrim(rtrim(AccountsLedger_FinYear))='" + FinYear + "'  and AccountsLedger_ExchangeSegmentID in(" + ExchangeSegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "') as op";
                }
            }
            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        #endregion Currency Settings For Multi Currency Project

        #region Domain Intensive

        #region Company Detail

        /// <summary>
        /// All Info About Company like Companies Name,
        /// Companies Exchanges,Company Exchanges ShortName
        /// Companies Segmensts, Companies Segments ShortName
        /// 20120622
        /// </summary>
        /// <returns></returns>

        /////Get Detail Of All Company's All ExchangeSegment
        public DataTable GetCompanyDetail()
        {
            DataTable DtCompanyDetail = new DataTable();
            DtCompanyDetail = GetDataTable("Tbl_Master_Exchange,Tbl_Master_CompanyExchange", @"Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID
                and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID", @"Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                Where Exchange_ShortName=Exh_ShortName");
            return DtCompanyDetail;
        }

        ////Get Detail Of All Company's Specific Segment
        public DataTable GetCompanyDetail(string ExchangeSegmentID, Boolean IsTopOne)
        {
            DataTable DtCompanyDetail = new DataTable();
            if (!IsTopOne)
            {
                DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID
                and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange WHERE Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                Where Exchange_ShortName=Exh_ShortName) T1", "*", @"Session_ExchangeSegmentID=" + ExchangeSegmentID.ToString());
            }
            else
            {
                DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID
                and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange WHERE Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                Where Exchange_ShortName=Exh_ShortName) T1", "Top 1 *", @"Session_ExchangeSegmentID=" + ExchangeSegmentID);
            }
            return DtCompanyDetail;
        }

        //////Get Detail Of Specific Company All ExchangeSegment
        public DataTable GetCompanyDetail(string CompanyID)
        {
            DataTable DtCompanyDetail = new DataTable();
            DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange
                Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                Where Exchange_ShortName=Exh_ShortName) as T1", "*", @"CompanyID='" + CompanyID.Trim() + "'");
            return DtCompanyDetail;
        }

        //////Get Detail Of Specific Company Specific UserSegID Like (CM or F0 Etc..)
        public DataTable GetCompanyDetail(string CompanyID, string UserSegID)
        {
            DataTable DtCompanyDetail = new DataTable();
            if (UserSegID.Length != 8)
            {
                DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                    (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                    (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                    Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                    (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange
                    Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                    Where Exchange_ShortName=Exh_ShortName) as T1", "*", @"CompanyID='" + CompanyID.Trim() + @"'
                    and Session_UserSegID='" + UserSegID + "'");
            }
            else
            {
                DtCompanyDetail = GetDataTable("tbl_master_companyExchange", @"Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                    (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                    null as [Session_ExchangeSegmentID],
                    null as [Session_UserSegID], exch_membershipType as Exh_ShortName,null as Exch_SegmentID",
                "exch_compId='" + CompanyID + "' and exch_TMCode='" + UserSegID + "'");
            }
            return DtCompanyDetail;
        }

        public DataTable GetCompanyDetailInformation(string CompanyID)
        {
            DataTable DtCompanyDetail = new DataTable();
            //if (UserSegID.Length != 8)
            //{
            DtCompanyDetail = GetDataTable(@"(Select Ltrim(Rtrim(Exch_CompID)) as CompanyID,
                    (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
                    (Select ExchangeSegment_ID From Master_ExchangeSegments Where ExchangeSegment_ExchangeID=Exchange_ID and ExchangeSegment_Code=Exch_SegmentID) as [Session_ExchangeSegmentID],
                    Exch_InternalID as [Session_UserSegID],Exh_ShortName,Exch_SegmentID from
                    (Select Exch_CompID,Exch_InternalID,Exh_ShortName,Exch_SegmentID from Tbl_Master_Exchange,Tbl_Master_CompanyExchange
                    Where Exh_CntId=Exch_ExchID) as T1,Master_Exchange
                    Where Exchange_ShortName=Exh_ShortName) as T1", "*", @"CompanyID='" + CompanyID.Trim() + @"'
                    ");
            //  }
            //            else
            //            {
            //                DtCompanyDetail = GetDataTable("tbl_master_companyExchange", @"Ltrim(Rtrim(Exch_CompID)) as CompanyID,
            //                    (Select Ltrim(RTrim(Cmp_Name)) from Tbl_Master_Company Where Cmp_InternalID=Exch_CompID) as Company,
            //                    null as [Session_ExchangeSegmentID],
            //                    null as [Session_UserSegID], exch_membershipType as Exh_ShortName,null as Exch_SegmentID",
            //                "exch_compId='" + CompanyID + "' and exch_TMCode='" + UserSegID + "'");
            //            }
            return DtCompanyDetail;
        }
        #endregion Company Detail

        #region Product_Type_SubType_Detail

        public DataTable GetProductType_UnderLyingAssets()
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "1,5";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "5,8,10";
                }
                else
                {
                    ProductTypeID = "10";
                }
            }

            return GetDataTable("Master_ProductTypes", "ProductType_ID,Ltrim(Rtrim(ProductType_Name)),Ltrim(Rtrim(Ltrim(ProductType_Code))",
                "ProductType_ID in (" + ProductTypeID + ")");
        }

        public DataTable GetProductType_UnderLyingAssets(int Param_ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Param_ExchangeSegmentID;
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "1,5";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "5,8,10";
                }
                else
                {
                    ProductTypeID = "10";
                }
            }

            return GetDataTable("Master_ProductTypes", "ProductType_ID,Ltrim(Rtrim(ProductType_Name)),Ltrim(Rtrim(Ltrim(ProductType_Code))",
                "ProductType_ID in (" + ProductTypeID + ")");
        }

        public DataTable GetProductType_DerivativeOnAssets()
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            return GetDataTable("Master_ProductTypes", "ProductType_ID,Ltrim(Rtrim(ProductType_Name)),Ltrim(Rtrim(Ltrim(ProductType_Code))",
                "ProductType_ID in (" + ProductTypeID + ")");
        }

        public DataTable GetProductType_DerivativeOnAssets(int Param_ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Param_ExchangeSegmentID;
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            return GetDataTable("Master_ProductTypes", "ProductType_ID,Ltrim(Rtrim(ProductType_Name)),Ltrim(Rtrim(Ltrim(ProductType_Code))",
                "ProductType_ID in (" + ProductTypeID + ")");
        }

        public DataTable GetProductSubType_UnderLyingAssets()
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "1,5";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "5,8,10";
                }
                else
                {
                    ProductTypeID = "10";
                }
            }

            return GetDataTable("Master_ProductSubTypes", "ProductSubType_ID,LTRIM(Rtrim(ProductSubType_Name)),ProductSubType_ApplicableType as ProductTypeID",
                " ProductSubType_ApplicableType in (" + ProductTypeID + ")");
        }

        public DataTable GetProductSubType_UnderLyingAssets(int Param_ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Param_ExchangeSegmentID;
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "1,5";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "10";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "5,8,10";
                }
                else
                {
                    ProductTypeID = "10";
                }
            }

            return GetDataTable("Master_ProductSubTypes", "ProductSubType_ID,LTRIM(Rtrim(ProductSubType_Name)),ProductSubType_ApplicableType as ProductTypeID",
                " ProductSubType_ApplicableType in (" + ProductTypeID + ")");
        }

        public DataTable GetProductSubType_DerivativeOnAssets()
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            return GetDataTable("Master_ProductSubTypes", "ProductSubType_ID,LTRIM(Rtrim(ProductSubType_Name)),ProductSubType_ApplicableType as ProductTypeID",
                " ProductSubType_ApplicableType in (" + ProductTypeID + ")");
        }

        public DataTable GetProductSubType_DerivativeOnAssets(int Param_ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Param_ExchangeSegmentID;
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            return GetDataTable("Master_ProductSubTypes", "ProductSubType_ID,LTRIM(Rtrim(ProductSubType_Name)),ProductSubType_ApplicableType as ProductTypeID",
                " ProductSubType_ApplicableType in (" + ProductTypeID + ")");
        }

        #endregion Product_Type_SubType_Detail

        #region Get Products and IDs For Different Exchange Segments

        public DataTable GetUnderLyingAssets()
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            return GetDataTable("Master_Products", @"Ltrim(Rtrim(Products_Name)) +' ['+
            (Select Ltrim(Rtrim(ProductsType_Name)) from Master_ProductTypes Where ProductType_ID=Products_ProductTypeID)+']' as dd,Products_ID",
               " ProductSubType_ApplicableType in (" + ProductTypeID + ")");
        }

        public DataTable GetUnderLyingAssets(int Param_ExchangeSegmentID)
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Param_ExchangeSegmentID;
            if (ExchangeSegmentID == 1)//for CM
            {
                ProductTypeID = "1";
            }
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                ProductTypeID = "4,6";
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13)//for CDX
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 14)// For SPOT
            {
                ProductTypeID = "11";
            }
            if (ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    ProductTypeID = "6,9,11";
                }
                else
                {
                    ProductTypeID = "11";
                }
            }

            return GetDataTable("Master_Products", @"Ltrim(Rtrim(Products_Name)) +' ['+
            (Select Ltrim(Rtrim(ProductType_Name)) from Master_ProductTypes Where ProductType_ID=Products_ProductTypeID)+']' as dd,Products_ID",
               " ProductSubType_ApplicableType in (" + ProductTypeID + ")");
        }

        public DataTable GetDerivativeOnAssets(int SpecificExchange) //This Parameter is For INMX Segment Pass 0 In Any Other Exchange
        {
            string ProductTypeID = String.Empty;
            int ExchangeSegmentID = Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString());
            if (ExchangeSegmentID == 2 || ExchangeSegmentID == 5)//for FO
            {
                return GetDataTable("Master_Equity", @"TickerSymbol,Equity_SeriesID from (select (case when Equity_StrikePrice=0.0 then isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+
	        ' '+isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6) else isnull(rtrim(ltrim(Equity_TickerSymbol)),'')+' '+
	        isnull(rtrim(ltrim(Equity_Series)),'')+' '+convert(varchar(9),Equity_EffectUntil,6)+' '+
	        cast(cast(round(Equity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Equity_SeriesID,Equity_StrikePrice,
	        Equity_TickerSymbol,Equity_Series,Equity_EffectUntil",
                @"Equity_ExchSegmentID=" + ExchangeSegmentID.ToString());
            }
            if (ExchangeSegmentID == 3 || ExchangeSegmentID == 6 || ExchangeSegmentID == 8 || ExchangeSegmentID == 13 //For CDX
                || ExchangeSegmentID == 14 // For SPOT
                || ExchangeSegmentID == 7 || ExchangeSegmentID == 9 || ExchangeSegmentID == 10 || ExchangeSegmentID == 11
                || ExchangeSegmentID == 12 || ExchangeSegmentID == 17 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 24 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)//for COMM
            {
                if (ExchangeSegmentID == 10 || ExchangeSegmentID == 18 || ExchangeSegmentID == 21 || ExchangeSegmentID == 22 || ExchangeSegmentID == 23 || ExchangeSegmentID == 25 || ExchangeSegmentID == 26 || ExchangeSegmentID == 27)// INMX/DGCX/BFX/INSX/INFX/INBX/INAX/INEX
                {
                    return GetDataTable("master_commodity", @"(ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+
                ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+
                convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID ",
                    "Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString() + " and Commodity_Exchange=" + SpecificExchange.ToString());
                }
                else
                {
                    return GetDataTable("master_commodity", @"(ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+'  '+ltrim(rtrim(isnull(Commodity_Identifier,'')))+'  '+
                ltrim(rtrim(isnull(Commodity_TickerSeries,'')))+'  '+cast(cast(isnull(Commodity_StrikePrice,0.00) as numeric(16,2)) as varchar)+'  '+
                convert(varchar(11),Commodity_EffectiveDate,113)) as Commodity_Product,ltrim(rtrim(Commodity_ProductSeriesID)) as Commodity_ProductSeriesID",
                    "Commodity_ExchangeSegmentID=" + ExchangeSegmentID.ToString());
                }
            }
            return null;
        }

        #endregion Get Products and IDs For Different Exchange Segments

        #region Employee hierarchy

        public string GeEmployeeHierarchy(string EmployeeID)
        {
            string[] InputName = new string[1];
            string[] InputType = new string[1];
            string[] InputValue = new string[1];

            DataSet DsEmployeeSubTree = new DataSet();

            InputName[0] = "empid";
            InputType[0] = "I";
            InputValue[0] = EmployeeID;
            DsEmployeeSubTree = SQLProcedures.SelectProcedureArrDS("Hr_GetEmployeeSubTree", InputName, InputType, InputValue);
            if (DsEmployeeSubTree.Tables.Count > 0)
                if (DsEmployeeSubTree.Tables[0].Rows.Count > 0)
                    return DsEmployeeSubTree.Tables[0].Rows[0][0].ToString();

            return "";
        }

        public string GeEmployeeHierarchyBranch(string EmployeeID)
        {
            string branchs = "";
            DataTable DtSecond = GetDataTable(" SRV_EMPLOYEEBRANCHMAP ", " BranchId ", " EmployeeId= '" + EmployeeID + "'");
            if (DtSecond.Rows.Count != 0)
            {
                var SelectedValues = DtSecond.AsEnumerable().Select(s => s.Field<System.Int64>("BranchId")).ToArray();

                branchs = string.Join(",", SelectedValues);
            }

            return branchs;
        }


        #endregion Employee hierarchy

        #endregion Domain Intensive

        #region Error For Read Only User

        public void ReadOnlyErrorNotification(string vMessage)
        {
            string sMessage;

            if (Convert.ToString(vMessage) == "")
            {
                sMessage = "You do not have permission to perform this operation";
            }
            else
            {
                sMessage = vMessage;
            }

            // HttpContext.Current.Response.Write("<script>window.open('http://www.google.com/','_blank');</script>");

            HttpContext.Current.Response.Write("<script> alert('You do not have permission to perform this operation');</script>");

            //   HttpContext.Current.Response.Write("<script> alert('" + sMessage.Replace("'", @"\'").Replace("\n", @"\n") + "');</script>");


        }


        public void ReadOnlyErrorNotificationWithClientScript(string vMessage)
        {
            string sMessage;

            if (Convert.ToString(vMessage) == "")
            {
                sMessage = "You do not have permission to perform this operation";
            }
            else
            {
                sMessage = vMessage;
            }


            sMessage = "alert('" + sMessage.Replace("'", @"\'").Replace("\n", @"\n") + "');";

            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;

                p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);


                //if (ScriptManager.GetCurrent(p) != null)
                //{
                //     ScriptManager.RegisterStartupScript(p, typeof(Page), "Message", sMessage, true);
                //}
                //else
                //{
                //    p.ClientScript.RegisterStartupScript(typeof(Page), "Message", sMessage, true);
                //}
            }
        }

        #endregion
        public DataTable SelectProcedureArr(string ProcedureName, string[] InputName, string[] InputType, string[] InputValue)
        {
            GetConnection();
            SqlDataAdapter MyDataAdapter = new SqlDataAdapter(ProcedureName, oSqlConnection);
            DataTable DT = new DataTable();

            try
            {

                int LoopCnt;

                //Set the command type as StoredProcedure.
                MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                //Create and add a parameter to Parameters collection for the stored procedure.

                for (LoopCnt = 0; LoopCnt < InputName.Length; LoopCnt++)
                {
                    if (InputType[LoopCnt] == "C")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Char, 10));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToString(InputValue[LoopCnt]);
                    }
                    if (InputType[LoopCnt] == "I")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Int, 4));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToInt32(InputValue[LoopCnt], 10);
                    }
                    else if (InputType[LoopCnt] == "V")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.VarChar, 255));
                        //Assign the search value to the parameter.
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    else if (InputType[LoopCnt] == "T")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Text, 2000000));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }

                    else if (InputType[LoopCnt] == "D")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.DateTime, 8));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = Convert.ToDateTime(InputValue[LoopCnt]);
                    }
                    else if (InputType[LoopCnt] == "DE")
                    {
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@" + InputName[LoopCnt], SqlDbType.Decimal, 14));
                        MyDataAdapter.SelectCommand.Parameters["@" + InputName[LoopCnt]].Value = InputValue[LoopCnt];
                    }
                    //MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@musicname", SqlDbType.Decimal, 23));
                }
                //Create and add an output parameter to the Parameters collection. 

                //Set the direction for the parameter. This parameter returns the Rows that are returned.
                //MyDataAdapter.SelectCommand.Parameters["@" + OutputName].Direction = ParameterDirection.Output;
                MyDataAdapter.SelectCommand.CommandTimeout = 0;
                MyDataAdapter.Fill(DT);
            }

            catch (Exception ex)
            {


            }
            finally
            {
                MyDataAdapter.Dispose();
                oSqlConnection.Close();
            }
            oSqlConnection.Close();
            return DT;
        }



        public void SetFinYearStartandEndDate()
        {

            string[,] data = GetFieldValue(" tbl_trans_LastSegment ",
                                "(select FinYear_StartDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearStart,(select FinYear_EndDate from Master_FinYear where FinYear_Code=ls_LastFinYear) as FinYearEnd ",
                                " ls_userId='" + HttpContext.Current.Session["userid"].ToString() + "' and ls_lastSegment='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'", 2);

            HttpContext.Current.Session["FinYearStart"] = data[0, 0];
            HttpContext.Current.Session["FinYearEnd"] = data[0, 1];


        }
    }
}