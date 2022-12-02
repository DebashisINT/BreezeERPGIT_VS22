using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using DataAccessLayer;
using System.Web;
using System.Data;
using System.Data.SqlClient;
 
namespace BusinessLogicLayer
{
   public class MasterDataCheckingBL
    {

       public DataTable SystemDuplcatePanChecking()
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("Prc_MShortNameCheckingDtl");
               proc.AddVarcharPara("@Action", 100, "SystemDuplcatePanChecking");
               dt = proc.GetTable();
               return dt;
           }
           catch
           {
               return null;
           }
       }
       public int DeleteCurrency(int id)
        {
            int i;
            int rtrnvalue = 0; 
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Currency");
            proc.AddIntegerPara("@id", id); 
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

       public int DeleteBuildingWareHouse(int id)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
           proc.AddNVarcharPara("@action", 100, "BuildingWareHouse");
           proc.AddIntegerPara("@id", id);
           proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;
       }
        public int DeleteMasterEducation(int educationid)
        {
            int i;
            int rtrnvalue = 0; 
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Education");
            proc.AddIntegerPara("@educationid", educationid); 
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMasterDocumentType(int documentid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "DocumentTypeMaster");
            proc.AddIntegerPara("@documentid", documentid);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMasterRemarkCategory(int categoryId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "RemarkCategory");
            proc.AddIntegerPara("@id", categoryId);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMasterPinCode(int Pin_id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "PinCode");
            proc.AddIntegerPara("@id", Pin_id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMasterregion(int region_id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "RegionDel");
            proc.AddIntegerPara("@id", region_id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMasterArea(int area_id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "AreaDEl");
            proc.AddIntegerPara("@id", area_id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMastercolor(int color_id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "colorDEl");
            proc.AddIntegerPara("@id", color_id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteMastersize(int size_id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "SizeDEl");
            proc.AddIntegerPara("@id", size_id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int checkFinancialYear(string FinYear)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "FinancialYear");
            proc.AddNVarcharPara("@finyear",30, FinYear);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteUdfGroup(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "UdfGroup");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteJobResponsibilities(string JobID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "JobResponsibilities");
            proc.AddNVarcharPara("@id", 30, JobID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteDesignation(string DesignationID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Designation");
            proc.AddNVarcharPara("@id", 30, DesignationID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int DeleteState(string StateID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "State");
            proc.AddNVarcharPara("@id", 30, StateID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int Deletecountry(string countryID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "country");
            proc.AddNVarcharPara("@id", 30, countryID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteCity(string CityID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "City");
            proc.AddNVarcharPara("@id", 30, CityID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #region MasterTDSTCS
        public int DeleteTDSTCS(string TDSTCSID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "TDSTCS");
            proc.AddNVarcharPara("@id", 30, TDSTCSID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        
        #endregion  MasterTDSTCS

        public int DeleteUdfData(string internalId, int UdfId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "DELETEREMARKSDATA");
            proc.AddNVarcharPara("@cnt_internalId", 50, internalId);
            proc.AddIntegerPara("@id", UdfId);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }


        public int DeleteFinancer(string internalId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "DeleteFinancer");
            proc.AddNVarcharPara("@cnt_internalId", 50, internalId); 
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #region MasterDeleteLeadOrContact
        public int DeleteLeadOrContact(string internalid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "DeleteLeadOrContact");
            proc.AddNVarcharPara("@cnt_internalId", 30, internalid);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #endregion  MasterDeleteLeadOrContact

        public int DeleteIndustry(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Industry");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int DeleteProduct(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "ProductMaster");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public int DeleteProductClass(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "ProductClass");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
         public int DeleteTask(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Task");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteTaxScheme(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "TaxScheme");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public int DeleteNumberingScheme(int id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "NumberingScheme");
            proc.AddIntegerPara("@id", id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #region Branch Added By Sam on 170202017 Start

        public int DeleteBranch(string Branch)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Branch");
            proc.AddNVarcharPara("@id", 30, Branch);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #endregion Branch Added By Sam on 170202017 End

        #region Currency Added and Modified By Sam on 170202017 Start

        public int DeleteCurrency(string CurrencyId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "Currency");
            proc.AddNVarcharPara("@id", 30, CurrencyId);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #endregion Currency Added By Sam on 170202017 End

        #region Vehicle Deletion : //080517-Kallol
        public int DeleteVehicle(string VehicleID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "VehicleMaster");
            proc.AddNVarcharPara("@id", 30, VehicleID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        #endregion Vehicle Deletion

        #region Tanmoy For STB model checking
        public int DeleteSTBModel(string STBModel_Id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "STBModel");
            proc.AddNVarcharPara("@id", 30, STBModel_Id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        #endregion

        #region Tanmoy For Manufacturer checking
        public int DeleteManufacturer(string Manufacturer_Id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CheckMasterData");
            proc.AddNVarcharPara("@action", 100, "STBManufacturer");
            proc.AddNVarcharPara("@id", 30, Manufacturer_Id);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        #endregion
        
    }       
}
