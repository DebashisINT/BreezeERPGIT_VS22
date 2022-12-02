using DataAccessLayer;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Payroll.Repostiory.payrollAttendance
{
    public class AttendanceLogic : IAttendanceLogic
    { 
        public DataSet GetEmployeeAttendance(string strPayClassID, string strYYMM)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("proll_AttendanceEntry_Details");
                proc.AddVarcharPara("@PayClassID", 100, strPayClassID);
                proc.AddVarcharPara("@YYMM", 100, strYYMM);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetEmployeeAttendanceApproval(string strPayClassID, string strYYMM,string EmployeeId)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_PROLL_GetEmployeeAttandanceForApproval");
                proc.AddVarcharPara("@PayClassID", 100, strPayClassID);
                proc.AddVarcharPara("@YYMM", 100, strYYMM);
                proc.AddVarcharPara("@EmployeeId", 100, EmployeeId);

                ds = proc.GetDataSet();
                return ds;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetEmployeeLeaveSummary(string strPayClassID, string strYYMM)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_PROLL_GETLEAVECOUNT");
                proc.AddVarcharPara("@PayClassID", 100, strPayClassID);
                proc.AddVarcharPara("@YYMM", 100, strYYMM);
                proc.AddVarcharPara("@USER_ID", 100, Convert.ToString(HttpContext.Current.Session["userid"]));

                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet SaveEmployeeLeaveSummary(string strPayClassID, string strYYMM)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_PROLL_SAVELEAVECOUNT");
                proc.AddVarcharPara("@PayClassID", 100, strPayClassID);
                proc.AddVarcharPara("@YYMM", 100, strYYMM);
                proc.AddVarcharPara("@USER_ID", 100, Convert.ToString(HttpContext.Current.Session["userid"]));

                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveApprovalData(Approval model)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_PROLL_SAVAPPROVALDATA");
                proc.AddVarcharPara("@Action", 100, "Add");
                proc.AddVarcharPara("@EmployeeId", 100, model.EmployeeId);
                proc.AddVarcharPara("@YYMM", 100, model.yymm);
                proc.AddVarcharPara("@USER_ID", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                proc.AddPara("@xml", ToXML(model));

                ds = proc.GetTable();
                return "";
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string ToXML(Approval model)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<approvaldata>));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, model.data);
                }
                return textWriter.ToString(); //This is the output as a string
            }
        }



        public void SaveAttendanceData(string PayClassID, string Period,DataTable dt, ref int strIsComplete, ref string strMessage)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proll_AttendanceEntry_Modify", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ModifyAttendanceEntry");
                cmd.Parameters.AddWithValue("@PayClassID", PayClassID);
                cmd.Parameters.AddWithValue("@YYMM", Period);
                cmd.Parameters.AddWithValue("@dt_AttendanceData", dt);

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 500);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnMessage"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strMessage = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }


        public DataSet GetImportAttendance(DataTable dt, Int64 UserID, string payclassid, string periodid, String map)
        {
            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("proll_AttendanceImport");
                proc.AddVarcharPara("@PayClassID", 100, payclassid);
                proc.AddVarcharPara("@PeriodID", 100, periodid);
                proc.AddPara("@UserID",  UserID);
                proc.AddPara("@EmpMap", map);
                proc.AddPara("@udtAttendance", dt);
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}