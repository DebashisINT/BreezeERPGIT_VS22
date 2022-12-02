using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class DailyTask_frmImportNsdlCalendar
    {
        public void ImportNsdlCalendartest(string file_path, string userid, string create_modify_datetime, string fromdate, string todate)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("sp_Import_NsdlCalendar_test"))
                {
                    proc.AddVarcharPara("@file_path", 300, file_path);
                    proc.AddVarcharPara("@userid", 100, userid);
                    proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
                    proc.AddVarcharPara("@fromdate",100, fromdate);
                    proc.AddVarcharPara("@todate",100, todate);
                    int i = proc.RunActionQuery();
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
