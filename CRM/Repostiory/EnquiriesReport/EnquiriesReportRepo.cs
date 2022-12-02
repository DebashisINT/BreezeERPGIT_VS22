using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;
using System.Data;
using DataAccessLayer;
using DataAccessLayer.Model;
using System.Data.SqlClient;
using System.Collections;

namespace CRM.Repostiory.EnquiriesReport
{
    public class EnquiriesReportRepo : IEnquiriesReport
    {
        public void GetEnqListing(string EnquiriesFrom, string FromDate, string ToDate)
        {
            string action = string.Empty;
            DataTable formula_dtls = new DataTable();
            DataSet dsInst = new DataSet();

            try
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_ENQUIRIES_REPORT";
                paramList.Add(new KeyObj("@USERID", user_id));
                paramList.Add(new KeyObj("@ENQUIRIESFROM", EnquiriesFrom));
                paramList.Add(new KeyObj("@FROMDATE", FromDate));
                paramList.Add(new KeyObj("@TODATE", ToDate));

                execProc.param = paramList;
                execProc.ExecuteProcedureNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Rev Subhra 11-04-2019
        public string Restore(string ActionType, string uniqueid, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            DataTable formula_dtls = new DataTable();

            try
            {

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                    paramList.Add(new KeyObj("@ACTION_TYPE", ActionType));
                    paramList.Add(new KeyObj("@CRM_ID", uniqueid));//ADDEDITCATEGORYIMPORT


                    paramList.Add(new KeyObj("@ReturnMessage", output, true));
                    paramList.Add(new KeyObj("@ReturnCode", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }
        public string PermanentDelete(string ActionType, string uniqueid, ref int ReturnCode)
        {
            string output = string.Empty;
            string action = string.Empty;
            int NoOfRowEffected = 0;

            DataTable formula_dtls = new DataTable();

            try
            {

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                    paramList.Add(new KeyObj("@ACTION_TYPE", ActionType));
                    paramList.Add(new KeyObj("@CRM_ID", uniqueid));


                    paramList.Add(new KeyObj("@RETURNMESSAGE", output, true));
                    paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));
                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                    paramList.Clear();
                    NoOfRowEffected = execProc.NoOfRows;
                    output = Convert.ToString(execProc.outputPara[0].value);
                    ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);
                }


            }
            catch (Exception ex)
            {
                throw;
            }



            return output;

        }
        //End of Rev Subhra

    }

}