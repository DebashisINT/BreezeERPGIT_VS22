//================================================== Revision History ==========================================================================
//1.0  25-09-2023    V2.0.39    Priti   0026836 :Opening Account module required the following columns (Log). Entered By, Entered On, Updated By, Updated On.
//====================================================== Revision History ======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OpeningBalanceBl
    {
        public void UpdateOpeningBalance(DataTable finalDt)
        {
            
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_updateOpeningBalance", con);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@opTable", finalDt);
            //Rev 1.0
            cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["userid"]));
            //Rev 1.0 End
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }
        public void UpdateProjectOpeningBalance(DataTable finalDt)
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_updateProjectOpeningBalance", con);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@opTable", finalDt);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }
        public DataTable GetOpeningBalanceDetails(string BranchHierarchy)
        {

            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("prc_GetOpeningbalanceDetails"))
                {
                    proc.AddVarcharPara("@BranchList", 2000, BranchHierarchy);
                    rtrnvalue = proc.GetTable();
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

        public DataTable GetProjectOpeningBalanceDetails(string BranchHierarchy)
        {

            ProcedureExecute proc;
            DataTable rtrnvalue;
            try
            {
                using (proc = new ProcedureExecute("prc_GetProjectOpeningbalanceDetails"))
                {
                    proc.AddVarcharPara("@BranchList", 2000, BranchHierarchy);
                    rtrnvalue = proc.GetTable();
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
    }
}
