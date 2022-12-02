using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class ExportRoutines
    {
        public DataSet ShowBatchNsdl(string batchdate, string DpID) 
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("show_batch_nsdl");
            proc.AddVarcharPara("@batchdate", 300, batchdate);
            proc.AddNVarcharPara("@DpID", 100, DpID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet NsdlExport(string batch_num, string operatorid, string batchdate, string dpid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_nsdl_export");
            proc.AddVarcharPara("@batch_num", 300, batch_num);
            proc.AddNVarcharPara("@operatorid", 100, operatorid);
            proc.AddVarcharPara("@batchdate", 300, batchdate);
            proc.AddNVarcharPara("@dpid", 100, dpid);
            ds = proc.GetDataSet();
            return ds;
        }

        public void InsertExportFiles(string userid, string segid, string file_type, string file_name, string batch_number, string file_path) 
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("sp_Insert_ExportFiles"))
                {
                    proc.AddVarcharPara("@userid", 100, userid);
                    proc.AddVarcharPara("@segid", 100, segid);
                    proc.AddVarcharPara("@file_type", 100, file_type);
                    proc.AddVarcharPara("@file_name", 100, file_name);
                    proc.AddVarcharPara("@batch_number", 100, batch_number);
                    proc.AddVarcharPara("@file_path", 500, file_path);
                    int i = proc.RunActionQuery();
                    //return i;
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

        public DataSet NsdlExportNew(string batch_num, string BatchGenerateUser, string batchdate, string dpid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_nsdl_export_new");
            proc.AddVarcharPara("@batch_num", 300, batch_num);
            proc.AddNVarcharPara("@BatchGenerateUser", 100, BatchGenerateUser);
            proc.AddVarcharPara("@batchdate", 300, batchdate);
            proc.AddNVarcharPara("@dpid", 100, dpid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ExportCDSL(string DPID, string TranDate, int BatchNo, string UserId, string FinYear, string BranchID, string CompanyId,
            string strTranType, string FileExt, int TotalNoRecord)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Export_CDSL");
            proc.AddVarcharPara("@DPID", 300, DPID);
            proc.AddNVarcharPara("@TranDate", 100, TranDate); 
            proc.AddIntegerPara("@BatchNo", BatchNo);
            proc.AddNVarcharPara("@UserId", 100, UserId); 
            proc.AddVarcharPara("@FinYear", 300, FinYear);
            proc.AddNVarcharPara("@BranchID", 100, BranchID); 
            proc.AddVarcharPara("@CompanyId", 300, CompanyId);
            proc.AddNVarcharPara("@strTranType", 100, strTranType); 
            proc.AddVarcharPara("@FileExt", 300, FileExt);
            proc.AddIntegerPara("@TotalNoRecord", TotalNoRecord);
            ds = proc.GetDataSet();
            return ds;
        }

        public void UpdateSlips(string benacc, string dpid)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("sp_update_slips"))
                {
                    proc.AddVarcharPara("@benacc", 100, benacc);
                    proc.AddVarcharPara("@dpid", 100, dpid);
                    int i = proc.RunActionQuery();
                    //return i;
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

        public DataSet ShowBatchCDSL(string batchdate, string DpID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("show_batch_cdsl");
            proc.AddVarcharPara("@batchdate", 300, batchdate);
            proc.AddNVarcharPara("@DpID", 100, DpID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ReportExchangeMarginReportingFile(string segment, string Companyid, string Finyear, string branch, string date, string CValuationdate, string CHoldingdate,
            string SYSTM1date, string SYSTM2date, string Considercmseg, string chkhaircut, string chkunapproved, decimal unapprovedshares, string AppMrgnOrVarMrgn, string IntialMargin)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_ExchangeMarginReportingFile]");
            proc.AddVarcharPara("@segment", 300, segment);
            proc.AddNVarcharPara("@Companyid", 100, Companyid); 
            proc.AddVarcharPara("@Finyear", 300, Finyear);
            proc.AddNVarcharPara("@branch", 100, branch); 
            proc.AddVarcharPara("@date", 300, date);
            proc.AddNVarcharPara("@CValuationdate", 100, CValuationdate);  
            proc.AddVarcharPara("@CHoldingdate", 300, CHoldingdate);
            proc.AddNVarcharPara("@SYSTM1date", 100, SYSTM1date); 
            proc.AddVarcharPara("@SYSTM2date", 300, SYSTM2date);
            proc.AddNVarcharPara("@Considercmseg", 100, Considercmseg);
            proc.AddVarcharPara("@chkhaircut", 300, chkhaircut); 
            proc.AddVarcharPara("@chkunapproved", 300, chkunapproved);
            proc.AddDecimalPara("@unapprovedshares",28,2, unapprovedshares); 
            proc.AddVarcharPara("@AppMrgnOrVarMrgn", 300, AppMrgnOrVarMrgn);
            proc.AddNVarcharPara("@IntialMargin", 100, IntialMargin); 
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ExportPosition_NSEFO(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[ExportPosition_NSEFO]");
            proc.AddVarcharPara("@date", 300, date);
            proc.AddNVarcharPara("@segment", 100, segment); 
            proc.AddVarcharPara("@companyid", 300, companyid);
            proc.AddNVarcharPara("@MasterSegment", 100, MasterSegment); 
            proc.AddVarcharPara("@ClientsID", 300, ClientsID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ExportRoutineTdays(string date, int days, int exchsegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Tdays");
            proc.AddVarcharPara("@date", 300, date);
            proc.AddIntegerPara("@days", days);
            proc.AddIntegerPara("@exchsegment", exchsegment);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_FRONTENDEXPORTFILES(string date, string segment, string Companyid, string Clients, string PndngPurchase, string rmscategory,
            string rolname, string category, string Finyear, string Tradedate, string SYSTM1date, string SYSTM2date, string closepricedate, string varmargindate,
            string Product, string collateralholdingdate, int iscollateral, int evaluationmethod, decimal capitaltorms, decimal amountto, int age,
            string segmentforrmslimot, string chkrmslimtaging, string unclearban, int unclearbandays, string exchangeuseridCM, string exchangeuseridFO,
            string expirydate, string exchangeuserinfo, string enabledflag, string POAORNOT, string chkunapproved, string unapprovedshares, string ExportType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_FRONTENDEXPORTFILES");
            proc.AddVarcharPara("@date", 300, date);
            proc.AddVarcharPara("@segment",300, segment);
            proc.AddVarcharPara("@Companyid",300, Companyid);
            proc.AddVarcharPara("@Clients", -1, Clients);
            proc.AddVarcharPara("@PndngPurchase",300, PndngPurchase);
            proc.AddVarcharPara("@rmscategory",300, rmscategory);
            proc.AddVarcharPara("@rolname", 300, rolname);
            proc.AddVarcharPara("@category", 300, category);
            proc.AddVarcharPara("@Finyear", 300, Finyear);
            proc.AddVarcharPara("@Tradedate", 300, Tradedate);
            proc.AddVarcharPara("@SYSTM1date", 300, SYSTM1date);
            proc.AddVarcharPara("@SYSTM2date", 300, SYSTM2date); 
            proc.AddVarcharPara("@closepricedate", 300, closepricedate);
            proc.AddVarcharPara("@varmargindate", 300, varmargindate);

            proc.AddVarcharPara("@Product", 300, Product);
            proc.AddVarcharPara("@collateralholdingdate", 300, collateralholdingdate);
            proc.AddIntegerPara("@iscollateral", iscollateral);
            proc.AddIntegerPara("@evaluationmethod", evaluationmethod);
            //proc.AddDecimalPara("@capitaltorms",28,2, capitaltorms);
            //proc.AddDecimalPara("@amountto", 28, 2, amountto);

            proc.AddIntegerPara("@capitaltorms",Convert.ToInt32( Convert.ToDecimal(capitaltorms)));
            proc.AddIntegerPara("@amountto", Convert.ToInt32( Convert.ToDecimal( amountto)));

            proc.AddIntegerPara("@age", age);
            proc.AddVarcharPara("@segmentforrmslimot", 300, segmentforrmslimot);
            proc.AddVarcharPara("@chkrmslimtaging", 300, chkrmslimtaging);
            proc.AddVarcharPara("@unclearban", 300, unclearban);
            proc.AddIntegerPara("@unclearbandays", unclearbandays); 
            proc.AddVarcharPara("@exchangeuseridCM", 300, exchangeuseridCM);
            proc.AddVarcharPara("@exchangeuseridFO", 300, exchangeuseridFO);
            proc.AddVarcharPara("@expirydate", 300, expirydate);
            proc.AddVarcharPara("@exchangeuserinfo", 300, exchangeuserinfo);
            proc.AddVarcharPara("@enabledflag", 300, enabledflag);
            proc.AddVarcharPara("@POAORNOT", 300, POAORNOT);
            proc.AddVarcharPara("@chkunapproved", 300, chkunapproved);
            proc.AddVarcharPara("@unapprovedshares", 300, unapprovedshares);
            proc.AddVarcharPara("@ExportType",100, ExportType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ReportExchangeMarginReportingFile_Com(string segment, string Companyid, string Finyear, string branch, string date, string CValuationdate, string CHoldingdate,
          string SYSTM1date, string SYSTM2date, string MasterSegment, string chkhaircut, string chkunapproved, string unapprovedshares,  string IntialMargin)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_ExchangeMarginReportingFile_Com]");
            proc.AddVarcharPara("@segment", 300, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Finyear", 300, Finyear);
            proc.AddVarcharPara("@branch", -1, branch);
            proc.AddVarcharPara("@date", 300, date);
            proc.AddVarcharPara("@CValuationdate", 100, CValuationdate);
            proc.AddVarcharPara("@CHoldingdate", 300, CHoldingdate);
            proc.AddVarcharPara("@SYSTM1date", 100, SYSTM1date);
            proc.AddVarcharPara("@SYSTM2date", 300, SYSTM2date);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@chkhaircut", 300, chkhaircut);
            proc.AddVarcharPara("@chkunapproved", 300, chkunapproved);
            proc.AddDecimalPara("@unapprovedshares", 28, 2, Convert.ToDecimal(unapprovedshares));         
            proc.AddNVarcharPara("@IntialMargin", 100, IntialMargin);
           
           
            ds = proc.GetDataSet();
            return ds;
        }


        public void sp_Insert_ExportFilesNew( string segid,string file_type,string file_name,string userid,string batch_number,string file_path)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("sp_Insert_ExportFiles"))
                {
                    proc.AddVarcharPara("@segid", 100, segid);
                    proc.AddVarcharPara("@file_type", 100, file_type);
                    proc.AddVarcharPara("@file_name", 100, file_name);
                    proc.AddVarcharPara("@userid", 100, userid);
                    proc.AddVarcharPara("@batch_number", 100, batch_number);
                    proc.AddVarcharPara("@file_path", 100, file_path);
                                    
                    int i = proc.RunActionQuery();
                    //return i;
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
