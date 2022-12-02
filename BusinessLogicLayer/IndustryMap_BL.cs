using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Web;
using System.Data;


namespace BusinessLogicLayer
{
    public class IndustryMap_BL
    {

        public string InsertIndustryMap_BL(int IndustryMap_EntityType, string IndustryMap_EntityIDs, int IndustryMap_IndustryID)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("sp_IndustryMap"))
                {
                    proc.AddIntegerPara("@IndustryMap_EntityType", IndustryMap_EntityType);

                    proc.AddNVarcharPara("@IndustryMap_EntityIDs", 4000, IndustryMap_EntityIDs);

                    proc.AddIntegerPara("@IndustryMap_IndustryID", IndustryMap_IndustryID);
                    proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@Mode", 50, "Insert");
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


        public DataTable BindEntityList()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_IndustryMap");

            proc.AddVarcharPara("@Mode", 50, "SelectEntity");

            dt = proc.GetTable();
            return dt;
        }
        public string InsertIndustryMapEntity_BL(int IndustryMap_EntityType, string IndustryMap_IndustryIDs, string IndustryMap_EntityID)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("sp_IndustryMapWithEntity"))
                {
                    proc.AddIntegerPara("@IndustryMap_EntityType", IndustryMap_EntityType);
                    proc.AddNVarcharPara("@IndustryMap_IndustryIDs", 4000, IndustryMap_IndustryIDs);
                    proc.AddNVarcharPara("@IndustryMap_EntityID", 50, IndustryMap_EntityID);
                    proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@Mode", 100, "Insert");
                    int NoOfRowEffected = proc.RunActionQuery();
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
        //public DataTable FetchIndustryByEntityID(int IndustryMap_EntityID)
        //{

        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("FetchIndustryByEntityType");
        //    proc.AddIntegerPara("@IndustryMap_EntityID", IndustryMap_EntityID);
        //    proc.AddVarcharPara("@Mode", 50, "Select");

        //    dt = proc.GetTable();
        //    return dt;
        //}

        public DataTable BindAvailableIndustry(string Search)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIndustryList");
            proc.AddNVarcharPara("@searchTexT", 50, Search);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable BindIndustryList()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");

            proc.AddVarcharPara("@Module", 50, "GetAllIndustry");

            dt = proc.GetTable();
            return dt;
        }


        public DataTable BindAllConsumerByIndustryId(string IndustryId, string EntityTypeID)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");

            proc.AddVarcharPara("@Module", 50, "GetAllConsumerByIndustryId");
            proc.AddIntegerPara("@IndustryID", Convert.ToInt32(IndustryId));
            proc.AddIntegerPara("@EntityTypeID", Convert.ToInt32(EntityTypeID));

            
            dt = proc.GetTable();
            return dt;
        }
    }
}