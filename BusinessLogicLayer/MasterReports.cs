using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class MasterReports
    {
        public DataSet Fetch_ClientMaster(string companyid,
            string segment, string IsBranchGroup,
           string BranchGroupValue, string fromdate,
           string todate, string RPTTYPE, string CLIENTS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_ClientMaster");

            proc.AddVarcharPara("@companyid", 50, companyid);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@IsBranchGroup", 50, IsBranchGroup);
            proc.AddVarcharPara("@BranchGroupValue", -1, BranchGroupValue);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@RPTTYPE", 20, RPTTYPE);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            ds = proc.GetDataSet();
            return ds;
        }

        public string InsertTransEmail(string Emails_SenderEmailID,
           string Emails_Subject, string Emails_Content,
          string Emails_HasAttachement, string Emails_CreateApplication,
          string Emails_CreateUser, string Emails_Type, string Emails_CompanyID,
            string Emails_Segment)
        {
            DataSet ds = new DataSet();
            string rtrnvalue = "";
            ProcedureExecute proc = new ProcedureExecute("InsertTransEmail");

            proc.AddVarcharPara("@Emails_SenderEmailID", 150, Emails_SenderEmailID);
            proc.AddVarcharPara("@Emails_Subject", 150, Emails_Subject);
            proc.AddVarcharPara("@Emails_Content", -1, Emails_Content);
            proc.AddCharPara("@Emails_HasAttachement", 1, Convert.ToChar(Emails_HasAttachement));
            proc.AddIntegerPara("@Emails_CreateApplication", Convert.ToInt32(Emails_CreateApplication));
            proc.AddIntegerPara("@Emails_CreateUser", Convert.ToInt32(Emails_CreateUser));
            proc.AddCharPara("@Emails_Type", 1, Convert.ToChar(Emails_Type));
            proc.AddCharPara("@Emails_CompanyID", 10, Convert.ToChar(Emails_CompanyID));
            proc.AddCharPara("@Emails_Segment", 10, Convert.ToChar(Emails_Segment));
            int i = proc.RunActionQuery();
            rtrnvalue = proc.GetParaValue("@result").ToString();
            return rtrnvalue;
        }

        public DataSet Fetch_ChargeSetupDetails1(string AsOnDate,
           string ChargeType, string ChargeBasis,
          string SEGMENT, string ComPanyID,
          string GRPTYPE, string Groupby, string CLIENTS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_ChargeSetupDetails1");

            proc.AddVarcharPara("@AsOnDate", 100, AsOnDate);
            proc.AddVarcharPara("@ChargeType", 10, ChargeType);
            proc.AddVarcharPara("@ChargeBasis", 10, ChargeBasis);
            proc.AddVarcharPara("@SEGMENT", 5, SEGMENT);
            proc.AddVarcharPara("@ComPanyID", 20, ComPanyID);
            proc.AddVarcharPara("@GRPTYPE", -1, GRPTYPE);
            proc.AddVarcharPara("@Groupby", -1, Groupby);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet POACLIENT(string COMPANYID, string SEGMENT, string FROMDATE, string TODATE, string BranchHierchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("POACLIENT");

            proc.AddVarcharPara("@COMPANYID", 50, COMPANYID);
            proc.AddVarcharPara("@SEGMENT", 50, SEGMENT);
            proc.AddVarcharPara("@FROMDATE", 50, FROMDATE);
            proc.AddVarcharPara("@TODATE", 50, TODATE);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ClientsHavingTradesInAPeriod(string companyid, string Segment, string fromdate, string todate, string RPTTYPE, string CLIENTS, string BranchHierchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ClientsHavingTradesInAPeriod");

            proc.AddVarcharPara("@companyid", 20, companyid);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@fromdate", 35, fromdate);
            proc.AddVarcharPara("@todate", 35, todate);
            proc.AddVarcharPara("@RPTTYPE", 10, RPTTYPE);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_ApprovedIlliquidSecurities(string AsOnDate, string Year, string Month, string ReportView)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ApprovedIlliquidSecurities");

            proc.AddVarcharPara("@AsOnDate", 35, AsOnDate);
            proc.AddVarcharPara("@Year", 15, Year);
            proc.AddVarcharPara("@Month", 10, Month);
            proc.AddVarcharPara("@ReportView", 10, ReportView);

            ds = proc.GetDataSet();
            return ds;
        }

        #region Client master

        public DataSet Fetch_NSDLClientMaster(string SelectedClient, string DateRangeSelection, string AccountStatus, string FromDate,
            string ToDate, string AccType, string AccSubType, string BenCategory, string BenOCP, string ShowNHolder, string ShowNPOA,
            string ShowAcMinor, string ShowAcNNom, string ShowAcMNom, bool chkGroup, string drpGroupType, string LastFinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_NSDLClientMaster");

            proc.AddVarcharPara("@SelectedClient", -1, SelectedClient);
            proc.AddVarcharPara("@DateRangeSelection", 1, DateRangeSelection);
            proc.AddVarcharPara("@AccountStatus", 1, AccountStatus);
            proc.AddVarcharPara("@FromDate", 100, FromDate);
            proc.AddVarcharPara("@ToDate", 100, ToDate);
            proc.AddVarcharPara("@AccType", -1, AccType);
            proc.AddVarcharPara("@AccSubType", -1, AccSubType);
            proc.AddVarcharPara("@BenCategory", 10, BenCategory);
            proc.AddVarcharPara("@BenOcp", 10, BenOCP);
            proc.AddVarcharPara("@ShowNHolder", 1, ShowNHolder);
            proc.AddVarcharPara("@ShowNPOA", 1, ShowNPOA);
            proc.AddVarcharPara("@ShowAcMinor", 1, ShowAcMinor);
            proc.AddVarcharPara("@ShowAcNNom", 1, ShowAcNNom);
            proc.AddVarcharPara("@ShowAcMNom", 1, ShowAcMNom);
            if (chkGroup == true)
                proc.AddVarcharPara("@ShowGroup", 1, "y");
            else
                proc.AddVarcharPara("@ShowGroup", 1, "n");

            proc.AddVarcharPara("@GroupType", 100, drpGroupType);
            proc.AddVarcharPara("@FinYear", 12, LastFinYear);

            ds = proc.GetDataSet();
            return ds;
        }

        #endregion


    }
}
