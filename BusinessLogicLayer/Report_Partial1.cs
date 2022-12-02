using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public partial class Reports
    {
        #region PositionFileSpanMCXCDX

        public DataSet ExportPosition_MCXCDX(string date, string segment, string companyid, string MasterSegment, string ClientsID,
            string ddlGroup, string ddlgrouptype)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_MCXCDX");

            proc.AddVarcharPara("@date", 30, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@companyid", 30, companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            if (ddlGroup == "0")
                proc.AddVarcharPara("@GrpType", -1, "Branch");
            else
                proc.AddVarcharPara("@GrpType", -1, ddlgrouptype);

            ds = proc.GetDataSet();
            return ds;
        }

        public void sp_Insert_ExportFiles(string segid, string file_name, string userid, string batch_number, string savefilepath)
        {

            ProcedureExecute proc = new ProcedureExecute("sp_Insert_ExportFiles");
            proc.AddVarcharPara("@segid", 10, segid);
            proc.AddVarcharPara("@file_type", 20, "FO Position txt");
            proc.AddVarcharPara("@file_name", 150, file_name);
            proc.AddVarcharPara("@userid", 30, userid);
            proc.AddVarcharPara("@batch_number", 10, batch_number);
            proc.AddVarcharPara("@file_path", 200, savefilepath);

            proc.RunActionQuery();

        }

        #endregion

        #region PositionFileSpanBSECDX

        public DataSet ExportPosition_BSECDX(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_BSECDX");

            proc.AddVarcharPara("@date", 30, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@companyid", 30, companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);


            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet PerformanceReportCM(string consolidatedchk, string companyid, string Segment, string idlist, string fromdate,
            string todate, string clients, string Seriesid, string FinYear, string grptyp, string clienttype, string closemethod,
            string sqroffchk, string ValuationDate)
        {
            DataSet ds = new DataSet();

            string CommandText;
            if (consolidatedchk.Trim() == "true")
            {
                CommandText = "PerformanceReportCM_JournalVoucher";
            }
            else
            {
                CommandText = "PerformanceReportCM";
            }

            ProcedureExecute proc = new ProcedureExecute(CommandText);

            proc.AddVarcharPara("@companyid", -1, companyid);
            proc.AddVarcharPara("@segment", -1, Segment);



            if (idlist.ToString().Trim() == "1")
            {
                proc.AddVarcharPara("@fromdate", 50, fromdate);
                proc.AddVarcharPara("@todate", 50, "NA");
            }
            else
            {
                proc.AddVarcharPara("@fromdate", 50, fromdate);
                proc.AddVarcharPara("@todate", 50, todate);
            }

            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@Seriesid", -1, Seriesid);
            proc.AddVarcharPara("@finyear", 50, FinYear);
            proc.AddVarcharPara("@grptype", 50, grptyp);
            proc.AddVarcharPara("@rpttype", 10, "1");
            proc.AddVarcharPara("@PRINTCHK", 200, "SHOW");
            proc.AddVarcharPara("@clienttype", 500, clienttype);
            proc.AddVarcharPara("@closemethod", 10, closemethod);
            if (sqroffchk.Trim() == "true")
            {
                proc.AddVarcharPara("@chkexcludesqr", 10, "CHK");
            }
            else
            {
                proc.AddVarcharPara("@chkexcludesqr", 10, "UNCHK");
            }
            if (consolidatedchk.Trim() == "true")
            {
                proc.AddVarcharPara("@jvcreate", 20, "NO");
                proc.AddVarcharPara("@SEGMENTJV", 50, "NA");
            }
            proc.AddVarcharPara("@chkstt", 10, "NA");
            proc.AddVarcharPara("@ValuationDate", 35, ValuationDate);


            ds = proc.GetDataSet();
            return ds;
        }


         
        #endregion

        #region NetPositionCDX

        public DataSet NetPositionCDX_CLIENTFETCHCDX(string ddldate, string todate, string fromdate, string segment, string MasterSegment,
            string Companyid, bool rdbClientALL, string HiddenField_Client, bool rdbScripall, string HiddenField_Scrip, string ddlGroup,
            string ddlgrouptype, string Branch, bool rdddlgrouptypeAll, string HiddenField_Group, bool chkopen)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CLIENTFETCHCDX");

            if (ddldate == "0")
            {
                proc.AddVarcharPara("@fromdate", 50, "NA");
                proc.AddVarcharPara("@todate", 50, todate);
            }
            else
            {
                proc.AddVarcharPara("@fromdate", 50, fromdate);
                proc.AddVarcharPara("@todate", 50, todate);
            }
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            if (rdbClientALL)
            {
                proc.AddVarcharPara("@ClientsID", -1, "ALL");
            }
            else
            {
                proc.AddVarcharPara("@ClientsID", -1, HiddenField_Client);
            }
            if (rdbScripall)
            {
                proc.AddVarcharPara("@INSTRUMENT", -1, "ALL");
            }
            else
            {
                proc.AddVarcharPara("@INSTRUMENT", -1, HiddenField_Scrip);
            }

            if (ddlGroup == "0")
            {
                proc.AddVarcharPara("@GRPTYPE", 50, "BRANCH");

            }
            else
            {
                proc.AddVarcharPara("@GRPTYPE", 50, ddlgrouptype);

            }
            proc.AddVarcharPara("@Branch", -1, Branch);
            if (rdddlgrouptypeAll)
            {
                proc.AddVarcharPara("@GROUP", -1, "ALL");
            }
            else
            {
                proc.AddVarcharPara("@GROUP", -1, HiddenField_Group);
            }
            if (chkopen)
            {
                proc.AddVarcharPara("@Open", 50, "CHK");
            }
            else
            {
                proc.AddVarcharPara("@Open", 50, "UNCHK");
            }


            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet NetPositionCDX_NetPositionCDX(string ddldate, string todate, string fromdate, string segment, string MasterSegment,
          string Companyid, string show, string ClientsID, string CLIENTS, bool rdbScripall, string HiddenField_Scrip, bool chkopen,
            bool chkopenbfpositive, string ddlrptview)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("NetPositionCDX");

            if (ddldate == "0")
            {
                proc.AddVarcharPara("@fromdate",50, "NA");
                proc.AddVarcharPara("@todate", 50, todate);
            }
            else
            {
                proc.AddVarcharPara("@fromdate", 50, fromdate);
                proc.AddVarcharPara("@todate", 50, todate);
            }
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@companyid", 30, Companyid);

            if (show == "SHOW")
            {
                proc.AddVarcharPara("@ClientsID",-1, ClientsID);
            }
            else
            {
                proc.AddVarcharPara("@ClientsID", -1, CLIENTS);
            }
            if (rdbScripall)
            {
                proc.AddVarcharPara("@INSTRUMENT", -1, "ALL");
            }
            else
            {
                proc.AddVarcharPara("@INSTRUMENT", -1, HiddenField_Scrip);
            }
            if (chkopen)
            {
                 proc.AddVarcharPara("@Open",50, "CHK");
            }
            else
            {
                proc.AddVarcharPara("@Open",50, "UNCHK");
            }
            if (chkopenbfpositive)
            {
                 proc.AddVarcharPara("@Sign",20, "CHK");
            }
            else
            {
                 proc.AddVarcharPara("@Sign",20, "UNCHK");
            }
             proc.AddVarcharPara("@RPTTYPE",50, ddlrptview);


            ds = proc.GetDataSet();
            return ds;
        }


        #endregion
    }
}
