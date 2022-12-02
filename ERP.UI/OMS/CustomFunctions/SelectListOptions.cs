using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using BusinessLogicLayer;


namespace ERP.OMS.CustomFunctions
{
    public class SelectListOptions
    {
        DataTable DT = new DataTable();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
          BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public DataTable getRefrred(string search_param, object KeyVal_InternalID)
        {

            string reqStr = "%";
            DT = new DataTable();
            try
            {
                switch (search_param.Trim())
                {
                    case "1":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "2":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "5":
                    case "6":
                    case "7":
                    case "9":
                    case "11":
                    case "12":
                    case "13":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                        break;
                    case "0":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "3":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='DV' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "4":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM' or cnt_contactType='CL') and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "8":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='RA' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "10":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='RC' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "14":
                        if (KeyVal_InternalID != null)
                        {
                            DT = oDBEngine.GetDataTable("tbl_master_contact", " Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + ISNULL(cnt_shortName, '')+']') AS cnt_firstName,cnt_internalId", " cnt_internalId='" + KeyVal_InternalID.ToString() + "' and cnt_firstName Like '" + reqStr.Trim() + "%'");
                        }
                        break;
                    case "20":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_UCC, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='CL' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "24":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='SB' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    case "25":
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName,'')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='FR' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                    default:
                        DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='PR' and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                        break;
                };
            }
            catch (Exception ex)
            {

            }
            return DT;
        }

        public DataTable SearchByEmp(string search_param, object KeyVal_InternalID)
        {
            string reqStr = "%";
            DT = new DataTable();
            try
            {
                DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", " top 10 ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and tbl_master_employee.emp_contactId not in('" + KeyVal_InternalID.ToString() + "') and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
            }
            catch(Exception ex)
            {


            }
            return DT;
        }
    }
}