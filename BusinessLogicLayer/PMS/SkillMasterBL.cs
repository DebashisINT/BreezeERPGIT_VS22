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
    public class SkillMasterBL
    {
        public DataSet DropDownDetailForSkill()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_MASTERVVALUE");
            ds = proc.GetDataSet();
            return ds;
        }
        public void SaveSkillData(string skill_id, string SkillName, string Description, string Charecteristic_Type, string Branch, DataTable prod, ref String ReturnMsg)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PMS_INSERTSKILLMASTER", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramProd = cmd.Parameters.Add("@SKILLSET", SqlDbType.Structured);
                paramProd.Value = prod;
                cmd.Parameters.AddWithValue("@SkillMaster_ID", skill_id);
                cmd.Parameters.AddWithValue("@SkillMaster_NAME", SkillName);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@Charecteristic_Type", Charecteristic_Type);
                cmd.Parameters.AddWithValue("@Branch_ID", Branch);
                cmd.Parameters.AddWithValue("@Create_By",  Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@Update_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                //cmd.Parameters.AddWithValue("@ReturnMsg", ReturnMsg); //, QueryParameterDirection.Output

                SqlParameter output = new SqlParameter("@ReturnMsg", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
              
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
               
                cmd.Dispose();
                con.Dispose();

                ReturnMsg = Convert.ToString(cmd.Parameters["@ReturnMsg"].Value.ToString());


                //return Convert.ToString("Data save");
            }
            catch (Exception ex)
            {
                ReturnMsg = ex.Message;
               // return "Please try again later.";
            }
        }

        public DataTable GetSkillMasterList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSSKILLMASTER_VIEW");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewSkillMaster(string SkillMaster_ID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMS_VIEWSKILLMASTER");
            proc.AddNVarcharPara("@SkillMaster_ID", 10, SkillMaster_ID);
            ds = proc.GetTable();
            return ds;
        }

        public void DeleteSkillMaster(string skill_id, ref string ReturnMsg)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_PMS_DELETESKILLMASTER");
            proc.AddNVarcharPara("@SkillMaster_ID", 10, skill_id);
            proc.AddNVarcharPara("@ReturnMsg", 500, ReturnMsg, QueryParameterDirection.Output);  
            ret = proc.RunActionQuery();
            //proc.RunActionQuery();
            ReturnMsg = Convert.ToString(proc.GetParaValue("@ReturnMsg"));
           // return ret;
        }
    }
}
