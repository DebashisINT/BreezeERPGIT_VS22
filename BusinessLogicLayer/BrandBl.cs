using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BrandBl
    {
        public bool insertBrand(string brandName,bool isActive,string contactNo,string email, int userId)
        {
            bool retValue = true;
            ProcedureExecute proc = new ProcedureExecute("Prc_master_brand");
            proc.AddVarcharPara("@Action", 5, "Ins");
            proc.AddVarcharPara("@Brand_Name", 50, brandName);
            proc.AddBooleanPara("@Brand_IsActive", isActive);
            proc.AddIntegerPara("@Brand_CreateUser", userId);
            proc.AddVarcharPara("@Brand_contactNo", 100, contactNo);
            proc.AddVarcharPara("@Brand_email", 100, email);
            try
            {
                int RowsNo = proc.RunActionQuery();
            }
            catch(Exception e)
            {
                retValue = false;
            }

            return true;
        }

        public bool updateBrand(string brandName, bool isActive, int brandId, string contactNo, string email)
        {
            bool retValue = true;
            ProcedureExecute proc = new ProcedureExecute("Prc_master_brand");
            proc.AddVarcharPara("@Action", 5, "Upd");
            proc.AddVarcharPara("@Brand_Name", 50, brandName);
            proc.AddBooleanPara("@Brand_IsActive", isActive);
            proc.AddIntegerPara("Brand_Id", brandId);
            proc.AddVarcharPara("@Brand_contactNo", 100, contactNo);
            proc.AddVarcharPara("@Brand_email", 100, email);
            try
            {
                int RowsNo = proc.RunActionQuery();
            }
            catch (Exception e)
            {
                retValue = false;
            }

            return true;
        }


        public int deleteBrand(int brandId)
        {
            int RowsNo = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_master_brand");
            proc.AddVarcharPara("@Action", 5, "Del"); 
            proc.AddIntegerPara("Brand_Id", brandId);
            try
            {
              RowsNo = proc.RunActionQuery();
            }
            catch (Exception e)
            {
                
            }

            return RowsNo;
        }

        public DataTable GetBrandDetails(int BrandId )
        {
            DataTable Dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_master_brand");
            proc.AddVarcharPara("@Action", 5, "Edit");
            proc.AddIntegerPara("@Brand_Id", BrandId);
            try
            {
                  Dt= proc.GetTable();
            }
            catch (Exception e)
            {
                
            }

            return Dt;
        }

    }
}
