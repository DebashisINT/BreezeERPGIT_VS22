using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Stocktransferjournal
    {
        public DataSet GetFromBranches(string branchHierchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("GetNumberingSchema_StockJournal");
            proc.AddVarcharPara("@userbranchlist", -1, branchHierchy);
            ds = proc.GetDataSet();
            return ds;
        }
        public int ExecuteNonqueryStockJournal(string User, string voucher, string Date, string frombranch, string tobranch, DataTable dtjournaldetails, DataTable dtjournalwarehouse, string inventory, string numberingscheme, string Finyear, string Companyid, string transfertiwarehouse, string Action, string narration,string journalId = null,Int64 Proj_id=0)
        {
            int i = 0;
            int x = 0;
            try
            {
                DataSet dsInst = new DataSet();
                //  dsInst = null; 
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                
                SqlCommand cmd = new SqlCommand("StockJournal_Modification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@User", User);
                cmd.Parameters.AddWithValue("@voucher", voucher);
                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@frombranch", frombranch);
                cmd.Parameters.AddWithValue("@tobranch", tobranch);
                cmd.Parameters.AddWithValue("@inventory", inventory);
                cmd.Parameters.AddWithValue("@numberingscheme", numberingscheme);
                cmd.Parameters.AddWithValue("@tblstockjournaldetails", dtjournaldetails);
                cmd.Parameters.AddWithValue("@WarehouseDetail", dtjournalwarehouse);
                cmd.Parameters.AddWithValue("@JournalID", journalId);
                cmd.Parameters.AddWithValue("@CompanyID", Companyid);
                cmd.Parameters.AddWithValue("@FinYear", Finyear);
                cmd.Parameters.AddWithValue("@TrnsferwarehouseID", transfertiwarehouse);
                cmd.Parameters.AddWithValue("@Narration", narration);
                cmd.Parameters.AddWithValue("@Project_Id", Proj_id);

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                x = Adap.Fill(dsInst);


                if (x > 0)
                {
                    i = 1;
                }


                cmd.Dispose();
                con.Dispose();

                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                return idFromString;

               
            }
            catch
            {
                i = 0;
                return i;
            }
        }


        public int DeleteJournal(string journalId)
        {
            int i;
            int retValue = 0;
            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddPara("@Action", "DeleteJournal");
            proc.AddPara("@JournalID", journalId);
            i = proc.RunActionQuery();
            return i;

        }


        public DataTable GetListJournals(string branchHierchy,string finyear)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddPara("@Action", "List");
            proc.AddPara("@userbranch", branchHierchy);
            proc.AddPara("@FinYear", finyear);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetListJournals(string branchHierchy, string finyear,string fromdate,string todate,string branch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddPara("@Action", "ListBranchwise");
            proc.AddPara("@userbranch", branchHierchy);
            proc.AddPara("@FinYear", finyear);
            proc.AddPara("@FromDate", fromdate);
            proc.AddPara("@Todate", todate);
            proc.AddPara("@Branch", branch);

            dt = proc.GetTable();

            return dt;
        }


        public DataSet GetListJournalsModifyList(string journalId)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddPara("@Action", "ModifyList");
            proc.AddPara("@JournalId", journalId);
            dt = proc.GetDataSet();
            return dt;
        }


        public DataSet GetListJournalsModifyStocksList(string journalId)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddPara("@Action", "ViewStocks");
            proc.AddPara("@JournalId", journalId);
            dt = proc.GetDataSet();
            return dt;
        }



        #region Warehouse

        public DataTable Gettransfertobranch(string Tobranch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Stockjournar_Warehousedetails");
            proc.AddPara("@Action", "ToWarehouse");
            proc.AddPara("@ToBranch", Tobranch);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable WarehouseGridbund(string Frombranch, string Tobranch, string productId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Stockjournar_Warehousedetails");
            proc.AddPara("@Action", "FromWarehouse");
            proc.AddPara("@FromBranch", Frombranch);
            proc.AddPara("@productId", productId);
            proc.AddPara("@ToBranch", Tobranch);
            dt = proc.GetTable();
            return dt;
        }


        #endregion


    }
}
