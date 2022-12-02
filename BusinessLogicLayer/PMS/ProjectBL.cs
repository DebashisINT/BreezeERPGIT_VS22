using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.PMS
{
   public class ProjectBL
    {
       public DataSet DropDownDetailForRole()
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("PRC_ProjectManagement_BIND");
           proc.AddVarcharPara("@Action", 200, "ProjectDetailsBind");
           ds = proc.GetDataSet();
           return ds;
       }

       public DataTable GetSalesOrder(string customerId, Int64 branchId, Int64 Proj_Id)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_ProjectManagement_SalesOrder");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddBigIntegerPara("@BranchId", branchId);
           proc.AddBigIntegerPara("@Proj_Id", Proj_Id);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable GetBranchList()
       {
           DataTable dt = new DataTable();
          
           ProcedureExecute proc = new ProcedureExecute("PRC_ProjectManagement_BIND");
           proc.AddVarcharPara("@Action", 200, "ProjectBranchBind");
           dt = proc.GetTable();
           return dt;
       }


       public DataTable DropDownNumberScheme()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_ProjectManagement_BIND");
           proc.AddVarcharPara("@Action", 200, "NumberschemeBind");
           proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           ds = proc.GetTable();
           return ds;
       }
       public DataTable GetProjectList()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_Project_List");
           ds = proc.GetTable();
           return ds;
       }

       public DataTable GetTermsConditionList()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_ProjectMTermsAndCondition");
           ds = proc.GetTable();
           return ds;
       }
       public DataTable ViewProjectDetails(Int64 Proj_Id)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_Project_VIEW");
           proc.AddBigIntegerPara("@ProjectId", Proj_Id);
           proc.AddVarcharPara("@Action", 200, "Edit");
           ds = proc.GetTable();
           return ds;
       }





       public int DeleteProjectData(Int64 Proj_Id)
       {
           int returnValue=0;
           ProcedureExecute proc = new ProcedureExecute("PRC_Project_VIEW");
           proc.AddBigIntegerPara("@ProjectId", Proj_Id);
           proc.AddVarcharPara("@Action", 200, "Delete");
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           proc.RunActionQuery();
           returnValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return returnValue;
       }
       public string SaveProjectManagementData(string Action, string Proj_Name, string Proj_Description, string Cnt_InternalId, Int64 Proj_Calender, Int64 Proj_Bracnchid, Int64 Proj_Managerid, string Proj_Statuscolor, DateTime Proj_EstimateStartdate, DateTime Proj_EstimateEnddate, decimal Proj_EstimatelabourCost, decimal Proj_EstimateExpenseCost, decimal proj_EstimateTotCost, DateTime Proj_ActualStartdate, DateTime Proj_ActualEndDate, string Proj_Code, Int64 Proj_Id, Int64 NumberSchemaId, Int64 Proj_estimateHH,
           Int64 Proj_estimateMM,Int64 Proj_Hierarchy, ref string OutputId, string Doc_No, string projStage_Desc, string Order_Id, string ProjectStatus,ref Int64 OutputProjectId,string BranchmapList)

        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Prc_Projectmanagement_AddEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                if (Action == "Edit")
                {
                    cmd.Parameters.AddWithValue("@Proj_Id", Proj_Id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                }
                cmd.Parameters.AddWithValue("@Proj_Name", Proj_Name);
                cmd.Parameters.AddWithValue("@Proj_Description", Proj_Description);
                cmd.Parameters.AddWithValue("@Cnt_InternalId", Cnt_InternalId);
                cmd.Parameters.AddWithValue("@Proj_Calender", Proj_Calender);
                cmd.Parameters.AddWithValue("@Proj_Bracnchid", Proj_Bracnchid);
                cmd.Parameters.AddWithValue("@Proj_Managerid", Proj_Managerid);
                cmd.Parameters.AddWithValue("@Proj_Statuscolor", Proj_Statuscolor);
                cmd.Parameters.AddWithValue("@Proj_EstimateStartdate", Proj_EstimateStartdate);
                cmd.Parameters.AddWithValue("@Proj_EstimateEnddate", Proj_EstimateEnddate);
               // cmd.Parameters.AddWithValue("@Proj_Estimatehours", Proj_Estimatehours);
                cmd.Parameters.AddWithValue("@Proj_EstimatelabourCost", Proj_EstimatelabourCost);
                cmd.Parameters.AddWithValue("@Proj_EstimateExpenseCost", Proj_EstimateExpenseCost);
                cmd.Parameters.AddWithValue("@proj_EstimateTotCost", proj_EstimateTotCost);
                cmd.Parameters.AddWithValue("@Proj_ActualStartdate", Proj_ActualStartdate);
                cmd.Parameters.AddWithValue("@Proj_ActualEndDate", Proj_ActualEndDate);
                cmd.Parameters.AddWithValue("@Proj_Code", Proj_Code);
                cmd.Parameters.AddWithValue("@NumberSchemaId", NumberSchemaId);
                cmd.Parameters.AddWithValue("@Proj_estimateHH", Proj_estimateHH);
                cmd.Parameters.AddWithValue("@Proj_estimateMM", Proj_estimateMM);
                cmd.Parameters.AddWithValue("@Doc_No", Doc_No);
                cmd.Parameters.AddWithValue("@projStage_Desc", projStage_Desc);
                cmd.Parameters.AddWithValue("@Order_Id", Order_Id);
                cmd.Parameters.AddWithValue("@ProjectStatus", ProjectStatus);
                cmd.Parameters.AddWithValue("@Hierarchy_ID", Proj_Hierarchy);
                cmd.Parameters.AddWithValue("@BranchmapList", BranchmapList);


                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                outputText.Direction = ParameterDirection.Output;
                SqlParameter outputprojIdId = new SqlParameter("@ReturnProjectId", SqlDbType.BigInt);
                outputprojIdId.Direction = ParameterDirection.Output;
                //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);
                cmd.Parameters.Add(outputprojIdId);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());

                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                OutputProjectId = Convert.ToInt64(cmd.Parameters["@ReturnProjectId"].Value);
                return Convert.ToString(strCPRID);
                //return Convert.ToString("Data save");
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

       public string TermsConditionSave(Int64 Doc_id, DateTime SaveTerms_DefectLibilityPeriodDate, string SaveTerms_DefectLibilityPeriodRemarks, string SaveTerms_LiqDamage, DateTime SaveTerms_LiqDamageAppDate, string SaveTerms_Payment, string SaveTerms_OrderType, string SaveTerms_NatureWork, DateTime SaveTerms_DefectLibilityPeriodToDate)
          {
              DataTable dt = (DataTable)HttpContext.Current.Session["bankDetails"];

           int i;
           string rtrnvalue = "";
           ProcedureExecute proc = new ProcedureExecute("Prc_Project_TermsConditions");
           proc.AddVarcharPara("@Action", 500, "Add");
           proc.AddBigIntegerPara("@Doc_id", Convert.ToInt64(Doc_id));
           proc.AddVarcharPara("@Doc_Type", 500, "Project");
           if (Convert.ToString(SaveTerms_DefectLibilityPeriodDate) != "1/1/0001 12:00:00 AM")
           {
               proc.AddDateTimePara("@Terms_DefectLibilityPeriodDate", SaveTerms_DefectLibilityPeriodDate);
           }

           if (Convert.ToString(SaveTerms_DefectLibilityPeriodToDate) != "1/1/0001 12:00:00 AM")
           {
               proc.AddDateTimePara("@Terms_DefectLibilityPeriodTODate", SaveTerms_DefectLibilityPeriodToDate);
           }
           proc.AddVarcharPara("@Terms_DefectLibilityPeriodRemarks", 500, SaveTerms_DefectLibilityPeriodRemarks);
           proc.AddVarcharPara("@Terms_LiqDamage", 100, SaveTerms_LiqDamage);

           if (Convert.ToString(SaveTerms_LiqDamageAppDate) != "1/1/0001 12:00:00 AM")
           {
               proc.AddDateTimePara("@Terms_LiqDamageAppDate", SaveTerms_LiqDamageAppDate);
           }
           proc.AddVarcharPara("@Terms_Payment", 100, SaveTerms_Payment);
           proc.AddVarcharPara("@Terms_OrderType", 100, SaveTerms_OrderType);
           proc.AddVarcharPara("@Terms_NatureWork", 100, SaveTerms_NatureWork);

           proc.AddIntegerPara("@Terms_CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
           proc.AddPara("@BGTABLE", dt);
           proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
           // return rtrnvalue;
           HttpContext.Current.Session["bankDetails"] = null;
           return rtrnvalue;
         }


       public DataSet GetTermsDetails(Int64 DocId, string DocType)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Prc_ProjectMaster_TermsConditionDetails");
           proc.AddBigIntegerPara("@Doc_Id", Convert.ToInt64(DocId));
           proc.AddVarcharPara("@doc_Type", 500, DocType);
           ds = proc.GetDataSet();
           return ds;
       }


       public string SavebankDetailsData(string Action, string Proj_Name, string Proj_Description, string Cnt_InternalId, Int64 Proj_Calender, Int64 Proj_Bracnchid, Int64 Proj_Managerid, string Proj_Statuscolor, DateTime Proj_EstimateStartdate, DateTime Proj_EstimateEnddate, decimal Proj_EstimatelabourCost, decimal Proj_EstimateExpenseCost, decimal proj_EstimateTotCost, DateTime Proj_ActualStartdate, DateTime Proj_ActualEndDate, string Proj_Code, Int64 Proj_Id, Int64 NumberSchemaId, Int64 Proj_estimateHH,
          Int64 Proj_estimateMM, Int64 Proj_Hierarchy, ref string OutputId, string Doc_No, string projStage_Desc, string Order_Id, string ProjectStatus)
       {
           try
           {
               DataSet dsInst = new DataSet();
               SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
               SqlCommand cmd = new SqlCommand("Prc_Projectmanagement_AddEdit", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", Action);
               cmd.Parameters.AddWithValue("@Proj_Id", Proj_Id);
               cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
              
               cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
               
               cmd.Parameters.AddWithValue("@Proj_Name", Proj_Name);
               cmd.Parameters.AddWithValue("@Proj_Description", Proj_Description);
               cmd.Parameters.AddWithValue("@Cnt_InternalId", Cnt_InternalId);
               cmd.Parameters.AddWithValue("@Proj_Calender", Proj_Calender);
               cmd.Parameters.AddWithValue("@Proj_Bracnchid", Proj_Bracnchid);
               cmd.Parameters.AddWithValue("@Proj_Managerid", Proj_Managerid);
              


               SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
               output.Direction = ParameterDirection.Output;

               SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
               outputText.Direction = ParameterDirection.Output;
               //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
               cmd.Parameters.Add(output);
               cmd.Parameters.Add(outputText);

               cmd.CommandTimeout = 0;
               SqlDataAdapter Adap = new SqlDataAdapter();
               Adap.SelectCommand = cmd;
               Adap.Fill(dsInst);
               cmd.Dispose();
               con.Dispose();
               OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());

               string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

               return Convert.ToString(strCPRID);
               //return Convert.ToString("Data save");
           }
           catch (Exception ex)
           {
               return "Please try again later.";
           }
       }




       public string SaveApprovalProData(string Action, Int64 Proj_Id, int Approved_by, DateTime Approved_On, string Remarks, string Proj_Code)
       {
           try
           {
               DataSet dsInst = new DataSet();
               SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
               SqlCommand cmd = new SqlCommand("Prc_Projectmanagement_AddEdit", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", Action);
               cmd.Parameters.AddWithValue("@Proj_Id", Proj_Id);
               cmd.Parameters.AddWithValue("@Approved_by", Approved_by);
               cmd.Parameters.AddWithValue("@Approved_On", Approved_On);
               cmd.Parameters.AddWithValue("@Remarks", Remarks);
               cmd.Parameters.AddWithValue("@Proj_Code", Proj_Code);
        
               cmd.CommandTimeout = 0;
               SqlDataAdapter Adap = new SqlDataAdapter();
               Adap.SelectCommand = cmd;
               Adap.Fill(dsInst);
               cmd.Dispose();
               con.Dispose();

               return Convert.ToString("Data save");
           }
           catch (Exception ex)
           {
               return "Please try again later.";
           }
       }
       public string SaveRejectedProData(string Action, Int64 Proj_Id, string Proj_Code)
       {
           try
           {
               DataSet dsInst = new DataSet();
               SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
               SqlCommand cmd = new SqlCommand("Prc_Projectmanagement_AddEdit", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Action", Action);
               cmd.Parameters.AddWithValue("@Proj_Id", Proj_Id);
               cmd.Parameters.AddWithValue("@Proj_Code", Proj_Code);
              

               cmd.CommandTimeout = 0;
               SqlDataAdapter Adap = new SqlDataAdapter();
               Adap.SelectCommand = cmd;
               Adap.Fill(dsInst);
               cmd.Dispose();
               con.Dispose();

               return Convert.ToString("Data save");
           }
           catch (Exception ex)
           {
               return "Please try again later.";
           }
       }
    }
}
