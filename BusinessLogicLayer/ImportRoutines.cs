using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public partial class ImportRoutines
    {
        public void ImportFile_ClosingRate(string Module, string FilePath, string Date)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try  
            {
                using (proc = new ProcedureExecute("AdressDummyInsert"))
                {
                    proc.AddVarcharPara("@Module", 50, Module);
                    proc.AddVarcharPara("@FilePath", 100, FilePath);
                    proc.AddVarcharPara("@Date", 50, Date);
                      
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
 
        public int XMLVarMarginBSE(string varmargin, string Date, string CompanyId, string ModifyUser)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("XMLVarMarginBSE"))
                { 
                    proc.AddVarcharPara("@varmargin", -1, varmargin);
                    proc.AddVarcharPara("@Date", 100, Date);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddIntegerPara("@ModifyUser", Convert.ToInt32(ModifyUser));
                    rtrnvalue = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
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

        public int XML_NSEIllSecurities(string XML, string Year, string Month, string ModifyUser)
        { 
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("XML_NSEIllSecurities"))
                {
                    proc.AddVarcharPara("@varmargin", 100, XML);
                    proc.AddVarcharPara("@Date", 100, Year);
                    proc.AddVarcharPara("@CompanyId", 100, Month);
                    proc.AddVarcharPara("@ModifyUser", 100, ModifyUser);
                    rtrnvalue = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
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


        public DataSet ExportPosition_CM_New(string date, int segment, string companyid, string MasterSegment, string ClientsID)
        { 
         
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_CM_New");
            proc.AddVarcharPara("@date", 100, date);
            proc.AddIntegerPara("@segment", segment);
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@ClientsID", 100, ClientsID);
            ds = proc.GetDataSet();
              
            return ds;
        }  










    }
}
