using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class DailyTask_frmImportNsdlBPListBL
    {
        public DataSet ImportNsdlBPList1(string file_path, string userid, string create_modify_datetime, string version)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlBPList1");
            proc.AddVarcharPara("@file_path", 300, file_path);
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime); 
            proc.AddNVarcharPara("@version", 100, version); 
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
