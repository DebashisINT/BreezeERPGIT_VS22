using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_portfolioperformancecm : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {

        PortfolioBL portrep = new PortfolioBL();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.GenericMethod oGenericMethod = null;

        //New Addtion For Company Filter
        string strAllCompany = String.Empty;
        string strAllSegment = String.Empty;
        string CombinedQuery = String.Empty;
        string strCompanyName = String.Empty;


        string data;
        int pageindex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            oGenericMethod.GetSegments();
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                fn_Segment();
                date();
                clienttype();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//


            //Add Avg Cost/Market Lower ListItem in ddlCloseMethod
            if (Request.QueryString["type"].ToString() == "01")
                ddlclosmethod.Items.Insert(3, new ListItem("Avg Cost/Market Lower", "3"));
        }

        public int CurrentPage
        {

            get
            {
                if (this.ViewState["CurrentPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
            }

            set
            {
                this.ViewState["CurrentPage"] = value;
            }

        }

        public int LastPage
        {
            get
            {
                if (this.ViewState["LastPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["LastPage"].ToString());
            }
            set
            {
                this.ViewState["LastPage"] = value;
            }

        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0] != "Clients")
                {
                    string[] val = cl[i].Split(';');
                    if (str == "")
                    {
                        str = val[0];
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += "," + val[0];
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
                else
                {

                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {

                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }

            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }

            else if (idlist[0] == "Product")
            {
                data = "Product~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "BranchGroup")
            {
                data = "BranchGroup~" + str;
            }
            else if (idlist[0] == "Company")
            {
                DataTable DtCompanySegments = new DataTable();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string CompaniesName = "";
                str = "'" + str.Replace(",", "','") + "'";
                DtCompanySegments = oGenericMethod.FullDetail_Company("D", ref CombinedQuery, 0,
                            "CompanyID in (" +
                            str + ")",
                            "dbo.fnSplit(ValueField,'~',3) ValueField,Ltrim(Rtrim(TextField)) TextField",
                            "TextField", null, null, 2);
                foreach (DataRow DrCompanySegment in DtCompanySegments.Rows)
                {
                    if (strAllSegment == String.Empty)
                        strAllSegment = DrCompanySegment[0].ToString();
                    else
                        strAllSegment = strAllSegment + "," + DrCompanySegment[0].ToString();

                    //Get Company Name
                    if (CompaniesName == string.Empty)
                        CompaniesName = DrCompanySegment[1].ToString();
                    else
                    {
                        if (!CompaniesName.Contains(DrCompanySegment[1].ToString()))
                            CompaniesName = "Multiple Comp Selected.";
                    }
                }
                data = "Company~" + str.Replace("','", ",").Replace("'", "") + "|" + strAllSegment + '~' + CompaniesName;
            }
        }
        void fn_Segment()
        {
            HiddenField_Segment.Value = "";
            DataTable DtSeg = new DataTable();
            if (rdbSegmentAll.Checked == true)
            {
                DtSeg = oDbEngine.GetDataTable("tbl_master_companyexchange", "case when exch_exchid='EXN0000002' and exch_segmentid='CM' then 'NSE-CM' WHEN exch_exchid='EXB0000001' and exch_segmentid='CM' THEN 'BSE-CM' WHEN exch_exchid='EXM0000002' and exch_segmentid='CM' THEN 'MCXSX-CM' WHEN exch_exchid='EXA0000001' and exch_segmentid is null THEN 'Accounts' ELSE NULL END,EXCH_INTERNALID", "(exch_segmentid like 'CM%' or exch_membershiptype='Accounts') and exch_compid='" + Session["LastCompany"].ToString() + "'", null);
                if (DtSeg.Rows.Count > 0)
                {
                    for (int i = 0; i < DtSeg.Rows.Count; i++)
                    {
                        if (HiddenField_Segment.Value == "")
                            HiddenField_Segment.Value = DtSeg.Rows[i][1].ToString();
                        else
                            HiddenField_Segment.Value += "," + DtSeg.Rows[i][1].ToString();
                    }
                    litSegmentMain.InnerText = DtSeg.Rows[0][0].ToString();
                }
            }
            else
            {
                HiddenField_Segment.Value = Session["usersegid"].ToString();
            }
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                litSegmentMain.InnerText = "MCXSX-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "16")
                litSegmentMain.InnerText = "Accounts";

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtValuationDate.EditFormatString = oconverter.GetDateFormat("Date");
            //dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            //dtto.EditFormatString = oconverter.GetDateFormat("Date");

            //DateTime firstDayOfThePrevMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1);
            //DateTime lastDayOfThePrevMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //lastDayOfThePrevMonth = lastDayOfThePrevMonth.AddDays(-(lastDayOfThePrevMonth.Day));

            dtfor.Value = Convert.ToDateTime(DateTime.Today);
            dtValuationDate.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            //dtfrom.Value = Convert.ToDateTime(firstDayOfThePrevMonth);
            //dtto.Value = Convert.ToDateTime(lastDayOfThePrevMonth);
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString() == "")
                {
                    BindGroup();
                }
            }
        }
        void clienttype()
        {
            if (Request.QueryString["type"].ToString() == "01")
            {
                ViewState["clienttype"] = "cnt_clienttype='Pro Trading'";
            }
            else if (Request.QueryString["type"].ToString() == "02")
            {
                ViewState["clienttype"] = "cnt_clienttype='Pro Investment'";
            }
            else
            {
                ViewState["clienttype"] = "(cnt_clienttype not in ('Pro Investment','Pro Trading') or cnt_clienttype is null)";
            }
        }
        void fn_Client()
        {

            DataTable dtclient = new DataTable();
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                        }
                        else
                        {
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", " DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select distinct gpm_id from tbl_master_groupmaster ))");

                        }
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", " DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value + ")");
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "2")
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers) ");
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + HiddenField_BranchGroup.Value + "))");
                    }
                }
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                        }
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers) ");
                    }
                    else
                    {
                        dtclient = oDbEngine.GetDataTable("tbl_master_contact", "DISTINCT cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") and cnt_branchid in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + HiddenField_BranchGroup.Value + "))");
                    }
                }
            }
            string Clients = null;
            if (dtclient.Rows.Count > 0)
            {
                for (int i = 0; i < dtclient.Rows.Count; i++)
                {
                    if (Clients == null)
                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                    else
                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                }

            }
            if (Clients != null)
            {
                HiddenField_Client.Value = Clients;
            }
        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDbEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();
            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CurrentPage = 0;
                        ddlbandforgroup();
                        ddlbandforClient();
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
        }
        void procedure()
        {
            //Server Debugger Variable
            string[,] strParam = new string[19, 2];
            string SpName = String.Empty;
            fn_Client();
            fn_Segment();

            string WhichCall = String.Empty;

            if (Request.QueryString["type"].ToString() == "01")
            {
                string strProclient = String.Empty;
                DataTable DtProClient = oDbEngine.GetDataTable("tbl_master_Contact", "Cnt_InternalID", "cnt_clienttype='Pro Trading'");
                foreach (DataRow Dr in DtProClient.Rows)
                {
                    strProclient = strProclient + "'" + Dr[0].ToString() + "',";
                }
                strProclient = strProclient.Substring(0, strProclient.LastIndexOf(","));
                HiddenField_Client.Value = strProclient;

                SpName = "[PerformanceReportCM_JournalVoucher]";//SD Code
            }
            else
            {
                SpName = "[PerformanceReportCM]";//SD Code
            }
            string companyid = "";
            string segment = "";
            string jvcreate = "";
            string SEGMENTJV = "";
            string CreateUser = "";
            string chk_excludesqr = "";
            string chk_stt = "";
            string ReportView = "";

            if (Request.QueryString["type"].ToString() == "01")
            {
                //New Option For Closing Stock Summary (Multiple Company Selection)
                string strCompanyFilter = String.Empty;
                strCompanyFilter = (rdbOnlyQty.Checked) ? "OnlyQty" : "WithValuation";
                if (ddlrpttype.SelectedItem.Value == "3")
                {
                    if (rdbCompanySelected.Checked) //Selected Company
                    {
                        strAllCompany = HiddenField_Company.Value.Split('|')[0];
                        strAllSegment = HiddenField_Company.Value.Split('|')[1];
                    }
                    else
                    {
                        DataTable DtCompany = new DataTable();
                        oGenericMethod = new BusinessLogicLayer.GenericMethod();


                        DtCompany = oGenericMethod.FullDetail_Company("D", ref CombinedQuery, 0,
                        "(ISNULL(Exh_ShortName,'')='Accounts' Or ISNULL(exch_segmentId,'')='CM')",
                        "dbo.fnSplit(ValueField,'~',1)+'~'+dbo.fnSplit(ValueField,'~',3) ValueField",
                        "TextField", null, null, 2);

                        foreach (DataRow DrCompany in DtCompany.Rows)
                        {
                            if (!strAllCompany.Contains(DrCompany[0].ToString().Split('~')[0]))
                            {
                                if (strAllCompany == String.Empty)
                                {
                                    strAllCompany = DrCompany[0].ToString().Split('~')[0];
                                    strAllSegment = DrCompany[0].ToString().Split('~')[1];
                                }
                                else
                                {
                                    strAllCompany = strAllCompany + "," + DrCompany[0].ToString().Split('~')[0];
                                    strAllSegment = strAllSegment + "," + DrCompany[0].ToString().Split('~')[1];
                                }
                            }
                        }
                    }

                    companyid = (strAllCompany + "|" + strCompanyFilter);
                    segment = strAllSegment;

                    strParam[0, 0] = "companyid"; strParam[0, 1] = "'" + (strAllCompany + "|" + strCompanyFilter) + "'";//SDCode
                    strParam[1, 0] = "segment"; strParam[1, 1] = "'" + strAllSegment + "'";//SDCode
                }
                else
                {
                    companyid = Session["LastCompany"].ToString();
                    segment = HiddenField_Segment.Value.ToString().Trim();

                    strParam[0, 0] = "companyid"; strParam[0, 1] = "'" + Session["LastCompany"].ToString() + "'";//SDCode
                    strParam[1, 0] = "segment"; strParam[1, 1] = "'" + HiddenField_Segment.Value.ToString().Trim() + "'";//SDCode
                }
            }
            else
            {
                companyid = Session["LastCompany"].ToString();
                segment = HiddenField_Segment.Value.ToString().Trim();

                strParam[0, 0] = "companyid"; strParam[0, 1] = "'" + Session["LastCompany"].ToString() + "'";//SDCode
                strParam[1, 0] = "segment"; strParam[1, 1] = "'" + HiddenField_Segment.Value.ToString().Trim() + "'";//SDCode
            }

            strParam[2, 0] = "fromdate"; strParam[2, 1] = "'" + dtfor.Value.ToString() + "'";//SDCode
            strParam[3, 0] = "todate"; strParam[3, 1] = "'NA'";//SDCode
            strParam[4, 0] = "clients"; strParam[4, 1] = "''" + HiddenField_Client.Value.ToString().Replace(",", "','") + "''";//SDCode

            if (rdbunderlyingall.Checked)
            {
                strParam[5, 0] = "Seriesid"; strParam[5, 1] = "'ALL'";//SDCode
            }
            else
            {
                strParam[5, 0] = "Seriesid"; strParam[5, 1] = "'" + HiddenField_Product.Value.ToString().Trim() + "'";//SDCode
            }
            strParam[6, 0] = "finyear"; strParam[6, 1] = "'" + HttpContext.Current.Session["LastFinYear"].ToString() + "'";//SDCode

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                strParam[7, 0] = "grptype"; strParam[7, 1] = "'BRANCH'";//SDCode
            }
            else
            {
                strParam[7, 0] = "grptype"; strParam[7, 1] = "'" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "'";//SDCode
            }
            strParam[8, 0] = "rpttype"; strParam[8, 1] = "'" + ddlrpttype.SelectedItem.Value.ToString().Trim() + "'";//SDCode

            string PRINTCHK = "";

            if (rbScreen.Checked || (rbExcel.Checked && (ddlrpttype.SelectedItem.Value.ToString() == "1" ||
                ddlrpttype.SelectedItem.Value.ToString() == "4")))
            {
                PRINTCHK = "SHOW";
                strParam[9, 0] = "PRINTCHK"; strParam[9, 1] = "'SHOW'";//SDCode
            }
            else
            {
                PRINTCHK = "PRINT";
                strParam[9, 0] = "PRINTCHK"; strParam[9, 1] = "'PRINT'";//SDCode
            }
            strParam[10, 0] = "clienttype"; strParam[10, 1] = "'" + ViewState["clienttype"].ToString().Replace("'", "''") + "'";//SDCode
            strParam[11, 0] = "closemethod"; strParam[11, 1] = "'" + ddlclosmethod.SelectedItem.Value.ToString().Trim() + "'";//SDCode

            if (Request.QueryString["type"].ToString() == "01")
            {
                if (ddldate.SelectedValue == "1")
                {
                    jvcreate = "NO ~CallBill";
                    strParam[12, 0] = "jvcreate"; strParam[12, 1] = "'NO ~CallBill'";//SDCode
                }
                if (ddldate.SelectedValue == "0")
                {
                    jvcreate = "NO";
                    strParam[12, 0] = "jvcreate"; strParam[12, 1] = "'NO'";//SDCode
                }
                SEGMENTJV = "NA";
                strParam[13, 0] = "SEGMENTJV"; strParam[13, 1] = "'NO'";//SDCode

                CreateUser = HttpContext.Current.Session["userid"].ToString();
                strParam[14, 0] = "CreateUser"; strParam[14, 1] = "'" + HttpContext.Current.Session["userid"].ToString() + "'";//SDCode
            }
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0" || ddlrpttype.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (chkexcludesqr.Checked)
                {
                    chk_excludesqr = "CHK";
                    strParam[15, 0] = "chkexcludesqr"; strParam[15, 1] = "'CHK'";//SDCode
                }
                else
                {
                    chk_excludesqr = "UNCHK";
                    strParam[15, 0] = "chkexcludesqr"; strParam[15, 1] = "'UNCHK'";//SDCode
                }
            }
            else
            {
                chk_excludesqr = "CHK";
                strParam[15, 0] = "chkexcludesqr"; strParam[15, 1] = "'CHK'";//SDCode
            }
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0" || ddlrpttype.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (chkstt.Checked)
                {
                    chk_stt = "CHK";
                    strParam[16, 0] = "chkstt"; strParam[16, 1] = "'CHK'";//SDCode
                }
                else
                {
                    chk_stt = "UNCHK";
                    strParam[16, 0] = "chkstt"; strParam[16, 1] = "'UNCHK'";//SDCode
                }
            }
            else
            {
                chk_stt = "CHK";
                strParam[16, 0] = "chkstt"; strParam[16, 1] = "'CHK'";//SDCode
            }
            strParam[17, 0] = "ValuationDate"; strParam[17, 1] = "'" + dtValuationDate.Value.ToString() + "'";//SDCode

            if (Request.QueryString["type"].ToString() == "01" || Request.QueryString["type"].ToString() == "03")
            {
                if (rbExcel.Checked)
                {
                    ReportView = "E";
                    strParam[18, 0] = "ReportView"; strParam[18, 1] = "'E'";//SDCode
                }
                else
                {
                    ReportView = "N";
                    strParam[18, 0] = "ReportView"; strParam[18, 1] = "'N'";//SDCode
                }
            }

            //For Server Debugging Purpose
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
            {
                string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                string FilePath = "../ExportFiles/ServerDebugging/" + SpName + strDateTime + ".txt";
                oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, SpName), FilePath, false);
            }
            if (Request.QueryString["type"].ToString() == "01")
            {
                ds = portrep.PerformanceReportCM_JournalVoucher(companyid, segment, dtfor.Value.ToString(), "NA", HiddenField_Client.Value.ToString().Trim(),
              rdbunderlyingall.Checked ? "ALL" : HiddenField_Product.Value.ToString().Trim(), HttpContext.Current.Session["LastFinYear"].ToString(),
              ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(), ddlrpttype.SelectedItem.Value.ToString().Trim(),
              PRINTCHK, ViewState["clienttype"].ToString(), ddlclosmethod.SelectedItem.Value.ToString().Trim(), jvcreate, SEGMENTJV, CreateUser, chk_excludesqr,
              chk_stt, dtValuationDate.Value.ToString(), ReportView);
            }
            else
            {
                ds = portrep.PerformanceReportCM(companyid, segment, dtfor.Value.ToString(), "NA", HiddenField_Client.Value.ToString().Trim(),
              rdbunderlyingall.Checked ? "ALL" : HiddenField_Product.Value.ToString().Trim(), HttpContext.Current.Session["LastFinYear"].ToString(),
              ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(), ddlrpttype.SelectedItem.Value.ToString().Trim(),
              PRINTCHK, ViewState["clienttype"].ToString(), ddlclosmethod.SelectedItem.Value.ToString().Trim(), chk_excludesqr, chk_stt, dtValuationDate.Value.ToString(), ReportView);
            }

            if (ds.Tables.Count > 0)
            {
                ViewState["dataset"] = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GROUPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GROUPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
        }
        void ddlbandforClient()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "GROUPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "CUSTOMERID";
                cmbclient.DataTextField = "CLIENTNAME";
                cmbclient.DataBind();

            }
            ViewState["clients"] = Distinctclient;
            LastPage = Distinctclient.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            Distinctclient = (DataTable)ViewState["clients"];
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + Distinctclient.Rows.Count.ToString() + " Record.";

            }
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "3")/////Closing Stock [Summary]
            {
                htmltable_3();
            }
            else
            {
                htmltable();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            ShowHidePreviousNext_of_Clients();
        }
        void ShowHidePreviousNext_of_Clients()
        {
            if (LastPage == 0 || LastPage == -1)
            {
                ASPxFirst.Style["Display"] = "none";
                ASPxPrevious.Style["Display"] = "none";
                ASPxNext.Style["Display"] = "none";
                ASPxLast.Style["Display"] = "none";

            }
            else
            {
                ASPxFirst.Style["Display"] = "Display";
                ASPxPrevious.Style["Display"] = "Display";
                ASPxNext.Style["Display"] = "Display";
                ASPxLast.Style["Display"] = "Display";

            }

            if (CurrentPage == LastPage && LastPage != 0)
            {

                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;

            }
            else
                if (CurrentPage == 0 && LastPage != 0)
                {
                    ASPxFirst.Enabled = false;
                    ASPxPrevious.Enabled = false;

                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;


                }
                else
                {
                    ASPxFirst.Enabled = true;
                    ASPxPrevious.Enabled = true;
                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;
                }
        }
        protected void ASPxFirst_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = 0;
            bind_Details();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }
        void htmltable()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count - 3;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            //if (ddldate.SelectedItem.Value.ToString() == "0")
            //{
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            //}
            //else
            //{
            //    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            //}
            strHtml1 = "<table width=\"1750px\"  border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"1200px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + 19 + ">Client Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            if (ddlrpttype.SelectedItem.Value.ToString() == "0")
            {

                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" ><b>Product</b></td>";
                strHtml += "<td align=\"center\" ><b>Op. </br> Qty</b></td>";
                strHtml += "<td align=\"center\" ><b>Op. </br> Value</b></td>";
                strHtml += "<td align=\"center\"><b>Avg </br> Cost</b></td>";
                strHtml += "<td align=\"center\"><b>Buy </br> Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Buy </br> Value</b></td>";
                strHtml += "<td align=\"center\"><b>Buy  </br> Avg</b></td>";
                strHtml += "<td align=\"center\"><b>Sell </br> Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Sell  </br> Value</b></td>";
                strHtml += "<td align=\"center\" ><b>Sell </br>  Avg.</b></td>";
                if (chkexcludesqr.Checked == false)
                {
                    strHtml += "<td align=\"center\" ><b>Sqr  </br> Qty</b></td>";
                    strHtml += "<td align=\"center\" ><b>Sqr-Of</br> P/L</b></td>";
                }
                strHtml += "<td align=\"center\"><b>Short</br> Term </br>  P/L </b></td>";
                strHtml += "<td align=\"center\"><b>Long </br> Term  </br> P/L</b></td>";
                if (chkstt.Checked == false)
                {
                    strHtml += "<td align=\"center\"><b>STT</b></td>";
                }
                strHtml += "<td align=\"center\"><b>Net </br>  P/L </b></td>";
                strHtml += "<td align=\"center\"><b>Close </br>  Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Close.  </br> Price</b></td>";
                strHtml += "<td align=\"center\"><b>Close.   </br> Val</b></td>";

            }
            else
            {
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" ><b>Segmnt</b></td>";
                strHtml += "<td align=\"center\" ><b>Security</b></td>";
                strHtml += "<td align=\"center\" ><b>Date</b></td>";
                strHtml += "<td align=\"center\" ><b>SettNo</b></td>";
                strHtml += "<td align=\"center\"><b>Buy </br> Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Sell </br> Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Rate</td>";
                strHtml += "<td align=\"center\"><b>Buy </br> Value</b></td>";
                strHtml += "<td align=\"center\"><b>Sell  </br> Value</b></td>";
                strHtml += "<td align=\"center\" ><b>Type</b></td>";
                strHtml += "<td align=\"center\"><b>Short</br> Term </br>  P/L </b></td>";
                strHtml += "<td align=\"center\"><b>Long </br> Term  </br> P/L</b></td>";
                strHtml += "<td align=\"center\"><b>Close </br>  Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Close.  </br> Price</b></td>";
                strHtml += "<td align=\"center\"><b>Close.   </br> Val</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            if (ddlrpttype.SelectedItem.Value.ToString() == "0")
            {
                decimal STT = decimal.Zero;
                //To Calculate The Sum 
                decimal BuyValueTotal = decimal.Zero;
                decimal SellValueTotal = decimal.Zero;
                decimal NetValueTotal = decimal.Zero;
                decimal CloseValueTotal = decimal.Zero;
                decimal SqrPL = decimal.Zero;

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" title=\"Clieck To View Detail \"><a href=javascript:void(0); onClick=javascript:OnMoreInfoClick('" + dt1.Rows[i]["CUSTOMERID"].ToString().Trim() + "','" + Request.QueryString["type"].ToString() + "','" + HiddenField_Segment.Value.ToString() + "','" + dt1.Rows[i]["MASTERPRODUCTID"].ToString() + "')>" + dt1.Rows[i]["PRODUCTNAME"].ToString() + "</a></td>";
                    if (dt1.Rows[i]["OPQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["OPQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["OPVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["OPVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["OPENINGAVGCOST"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["OPENINGAVGCOST"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["BUYAVGCOST"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYAVGCOST"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["SELLAVGCOST"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLAVGCOST"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    /////////////exclude sqr begin
                    if (chkexcludesqr.Checked == false)
                    {
                        if (dt1.Rows[i]["SQRQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SQRQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";


                        if (dt1.Rows[i]["SQRPL"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SQRPL"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        //Sum Of SqrPL
                        if (dt1.Rows[i]["SQRPL"] != null && dt1.Rows[i]["SQRPL"].ToString().Trim() != String.Empty)
                            SqrPL = SqrPL + Convert.ToDecimal(dt1.Rows[i]["SQRPL"].ToString().Replace(",", ""));

                    }
                    /////////////exclude sqr end
                    if (dt1.Rows[i]["STPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["STPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["LTPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["LTPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (chkstt.Checked == false)
                    {
                        if (dt1.Rows[i]["sttaxtotalstt"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["sttaxtotalstt"].ToString() + "</td>";
                            STT = STT + Convert.ToDecimal(dt1.Rows[i]["sttaxtotalstt"].ToString());
                        }
                        else
                        {
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        }

                    }
                    if (dt1.Rows[i]["NETPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["NETPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSEQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEAvgCost"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSEAvgCost"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSEVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";



                    strHtml += "</tr>";



                    //Sum Of Buy Value
                    if (dt1.Rows[i]["BuyValue1"] != null && dt1.Rows[i]["BuyValue1"].ToString().Trim() != String.Empty)
                        BuyValueTotal = BuyValueTotal + Convert.ToDecimal(dt1.Rows[i]["BuyValue1"].ToString());
                    //Sum Of Sell Value
                    if (dt1.Rows[i]["SellValue1"] != null && dt1.Rows[i]["SellValue1"].ToString().Trim() != String.Empty)
                        SellValueTotal = SellValueTotal + Convert.ToDecimal(dt1.Rows[i]["SellValue1"].ToString());
                    //Sum Of Net Value
                    if (dt1.Rows[i]["NETPL"] != null && dt1.Rows[i]["NETPL"].ToString().Trim() != String.Empty)
                        NetValueTotal = NetValueTotal + Convert.ToDecimal(dt1.Rows[i]["NETPL"].ToString().Replace(",", ""));
                    //Sum Of Close Value
                    if (dt1.Rows[i]["CLOSEVALUE"] != null && dt1.Rows[i]["CLOSEVALUE"].ToString().Trim() != String.Empty)
                        CloseValueTotal = CloseValueTotal + Convert.ToDecimal(dt1.Rows[i]["CLOSEVALUE"].ToString().Replace(",", ""));


                }
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


                if (chkexcludesqr.Checked == false)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" colspan=5><b>Total  :</b></td>";



                    //Buy Value Total Display
                    if (BuyValueTotal != decimal.Zero)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close. Val\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(BuyValueTotal.ToString())) + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                    //Sell Value Total Dispaly
                    if (SellValueTotal != decimal.Zero)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close. Val\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(SellValueTotal.ToString())) + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                    //SqrPL Total Display
                    if (SqrPL != decimal.Zero)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sqr-Of P/L\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(SqrPL.ToString())) + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";



                }
                else
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" colspan=5><b>Total  :</b></td>";


                    //Buy Value Total Display
                    if (BuyValueTotal != decimal.Zero)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close. Val\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(BuyValueTotal.ToString())) + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                    //Sell Value Total Dispaly
                    if (SellValueTotal != decimal.Zero)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close. Val\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(SellValueTotal.ToString())) + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                }


                if (dt1.Rows[0]["STPL2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Short Term P/L\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[0]["STPL2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[0]["LTPL2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Long Term P/L\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[0]["LTPL2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (chkstt.Checked == false)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"STT\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(STT)) + "</td>";
                }

                //Net PL Total Display
                if (NetValueTotal != decimal.Zero)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"STT\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(NetValueTotal)) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";


                strHtml += "<td align=\"left\" >&nbsp;</td>";
                strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (CloseValueTotal != decimal.Zero)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close. Val\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(CloseValueTotal)) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";


                strHtml += "</tr>";
            }

            else
            {
                string MASTERPRODUCT = null;
                int i;
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    if (MASTERPRODUCT != null)
                    {
                        if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                        {
                            if (chkexcludesqr.Checked == false)
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" colspan=7>Sqr Qty :<b>" + dt1.Rows[i - 1]["SQRQTY"].ToString() + "</b> Sqr-Of P/L :<b>" + dt1.Rows[i - 1]["SQRPL"].ToString() + "</b></td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" colspan=7><b>Total:</b></td>";
                            }

                            if (dt1.Rows[i - 1]["BUYVALUE2"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["BUYVALUE2"].ToString())) + "</td>";
                            else
                                strHtml += "<td align=\"left\" >&nbsp;</td>";

                            if (dt1.Rows[i - 1]["SELLVALUE2"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["SELLVALUE2"].ToString())) + "</td>";
                            else
                                strHtml += "<td align=\"left\" >&nbsp;</td>";

                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                            if (dt1.Rows[i - 1]["STPL2"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["STPL2"].ToString())) + "</td>";
                            else
                                strHtml += "<td align=\"left\" >&nbsp;</td>";

                            if (dt1.Rows[i - 1]["LTPL2"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["LTPL2"].ToString())) + "</td>";
                            else
                                strHtml += "<td align=\"left\" >&nbsp;</td>";

                            if (dt1.Rows[i - 1]["CLOSINGQTY2"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["CLOSINGQTY2"].ToString())) + "</td>";
                            else
                                strHtml += "<td align=\"left\" >&nbsp;</td>";

                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                            if (dt1.Rows[i - 1]["CLOSINGVALUE2"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["CLOSINGVALUE2"].ToString())) + "</td>";
                            else
                                strHtml += "<td align=\"left\" >&nbsp;</td>";




                            strHtml += "</tr>";
                            flag = flag + 1;
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        }

                    }
                    MASTERPRODUCT = dt1.Rows[i]["MASTERPRODUCTID"].ToString();
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["SEGMENT"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["TICKERSYMBOL"].ToString() + " " + dt1.Rows[i]["SERIESANDCODE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["DATE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["SETTNO"].ToString() + "</td>";

                    if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["RATE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["RATE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">" + dt1.Rows[i]["BRKGTYPE"].ToString() + "</td>";


                    if (dt1.Rows[i]["STPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["STPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["LTPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["LTPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSINGQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSINGQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSINGAVGCOST"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSINGAVGCOST"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSINGVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CLOSINGVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";



                    strHtml += "</tr>";
                }
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                if (chkexcludesqr.Checked == false)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" colspan=7>Sqr Qty :<b>" + dt1.Rows[i - 1]["SQRQTY"].ToString() + "</b> Sqr-Of P/L :<b>" + dt1.Rows[i - 1]["SQRPL"].ToString() + "</b></td>";

                }
                else
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap\" colspan=7><b>Total:</b></td>";

                }
                if (dt1.Rows[i - 1]["BUYVALUE2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["BUYVALUE2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i - 1]["SELLVALUE2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["SELLVALUE2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i - 1]["STPL2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["STPL2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i - 1]["LTPL2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["LTPL2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i - 1]["CLOSINGQTY2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["CLOSINGQTY2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i - 1]["CLOSINGVALUE2"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i - 1]["CLOSINGVALUE2"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";


                strHtml += "</tr>";

            }

            strHtml += "</table>";
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;

        }
        void htmltable_3()
        {
            ds = (DataSet)ViewState["dataset"];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    String strHtml = String.Empty;
                    String strHtml1 = String.Empty;
                    int colcount = ds.Tables[0].Columns.Count - 3;
                    string str = null;
                    //CompanyFilter
                    string[] strCompanies = null;
                    if (ddlGroup.SelectedItem.Value.ToString() == "1")
                    {
                        str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
                    }

                    //if (ddldate.SelectedItem.Value.ToString() == "0")
                    //{
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
                    //}
                    //else
                    //{
                    //    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    //}
                    strHtml1 = "<table width=\"1000px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

                    strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "CUSTOMERID='" + cmbclient.SelectedItem.Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();

                    strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                    strHtml += "<td align=\"left\" colspan=" + 10 + ">Client Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Instrument</b></td>";
                    strHtml += "<td align=\"center\" ><b>Closing </br> Stock</b></td>";
                    strHtml += "<td align=\"center\" ><b>Value At </br> Cost</b></td>";
                    strHtml += "<td align=\"center\" ><b>Avg. </br> Cost</b></td>";
                    strHtml += "<td align=\"center\"><b>Mkt. </br> Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Market </br> Value</b></td>";
                    strHtml += "<td align=\"center\"><b>P/L </br> @Mkt</td>";
                    strHtml += "<td align=\"center\"><b>Perf(%)</b></td>";
                    strHtml += "<td align=\"center\"><b>Close  </br> Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Close</br> Value</b></td>";

                    ///CompanyFilter
                    if (HiddenField_Company.Value.Split('|')[0].Split(',').GetLength(0) > 1)
                    {
                        strCompanies = ds.Tables[1].Rows[0][0].ToString().Split(',');
                        foreach (string strcomp in strCompanies)
                            strHtml += "<td align=\"center\" ><b>" + strcomp + "</b></td>";
                    }


                    int flag = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["PRODUCTNAME"].ToString() + "</td>";

                        if (HiddenField_Company.Value.Split('|')[0].Split(',').GetLength(0) > 1)
                        {
                            foreach (string strcomp in strCompanies)
                                if (dt1.Rows[i][strcomp] != DBNull.Value)
                                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i][strcomp].ToString() + "</td>";
                        }

                        if (dt1.Rows[i]["CLOSINGQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSINGQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["CLOSINGVALUE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSINGVALUE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["AVGCOST"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["AVGCOST"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["CLOSEPRICE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["MKTVALUE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["MKTVALUE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["PLMKTVALUE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["PLMKTVALUE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["PL"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["PL"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["METHODCLOSEPRICE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["METHODCLOSEPRICE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["METHODCLOSEVALUE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["METHODCLOSEVALUE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        strHtml += "</tr>";

                    }

                    strHtml += "</table>";
                    DIVdisplayPERIOD.InnerHtml = strHtml1;
                    display.InnerHtml = strHtml;
                }
            }
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
            int curentIndex = cmbgroup.SelectedIndex;
            int totalNo = cmbgroup.Items.Count;
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    curentIndex = curentIndex + 1;
                    break;
                case "Prev":
                    curentIndex = curentIndex - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalGrp.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            cmbgroup.SelectedIndex = curentIndex;
            ddlbandforClient();
            bind_Details();
        }

        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlbandforClient();
        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            ds.Tables[0].Columns.Add("stplsum", typeof(decimal));
            ds.Tables[0].Columns.Add("ltplsum", typeof(decimal));
            ds.Tables[0].Columns.Add("buyvaluesum1", typeof(decimal));
            ds.Tables[0].Columns.Add("sellvaluesum1", typeof(decimal));
            if (CHKLOGOPRINT.Checked == false)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows[i]["Image"] = logoinByte;


                        if (ds.Tables[0].Rows[i]["STPL"] != null && ds.Tables[0].Rows[i]["STPL"].ToString().Trim() != String.Empty)
                            ds.Tables[0].Rows[i]["stplsum"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["STPL"].ToString().Replace(",", ""));
                        if (ds.Tables[0].Rows[i]["LTPL"] != null && ds.Tables[0].Rows[i]["LTPL"].ToString().Trim() != String.Empty)
                            ds.Tables[0].Rows[i]["ltplsum"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["LTPL"].ToString().Replace(",", ""));

                        if (ds.Tables[0].Rows[i]["BUYVALUE"] != null && ds.Tables[0].Rows[i]["BUYVALUE"].ToString().Trim() != String.Empty)
                            ds.Tables[0].Rows[1]["buyvaluesum1"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["BUYVALUE"].ToString().Replace(",", ""));
                        if (ds.Tables[0].Rows[i]["SELLVALUE"] != null && ds.Tables[0].Rows[i]["SELLVALUE"].ToString().Trim() != String.Empty)
                            ds.Tables[0].Rows[i]["sellvaluesum1"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["SELLVALUE"].ToString().Replace(",", ""));


                        ds.AcceptChanges();


                    }
                }
            }
            else
            {
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    if (ds.Tables[0].Rows[m]["STPL"] != null && ds.Tables[0].Rows[m]["STPL"].ToString().Trim() != String.Empty)
                        ds.Tables[0].Rows[m]["stplsum"] = Convert.ToDecimal(ds.Tables[0].Rows[m]["STPL"].ToString().Replace(",", ""));
                    if (ds.Tables[0].Rows[m]["LTPL"] != null && ds.Tables[0].Rows[m]["LTPL"].ToString().Trim() != String.Empty)
                        ds.Tables[0].Rows[m]["ltplsum"] = Convert.ToDecimal(ds.Tables[0].Rows[m]["LTPL"].ToString().Replace(",", ""));
                    if (ds.Tables[0].Rows[m]["BUYVALUE"] != null && ds.Tables[0].Rows[m]["BUYVALUE"].ToString().Trim() != String.Empty)
                        ds.Tables[0].Rows[m]["buyvaluesum1"] = Convert.ToDecimal(ds.Tables[0].Rows[m]["BUYVALUE"].ToString().Replace(",", ""));
                    if (ds.Tables[0].Rows[m]["SELLVALUE"] != null && ds.Tables[0].Rows[m]["SELLVALUE"].ToString().Trim() != String.Empty)
                        ds.Tables[0].Rows[m]["sellvaluesum1"] = Convert.ToDecimal(ds.Tables[0].Rows[m]["SELLVALUE"].ToString().Replace(",", ""));

                    ds.AcceptChanges();

                }
            }


            //ds.Tables[0].WriteXmlSchema("D:\\portfoliocm.xsd");
            ReportDocument report = new ReportDocument();
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\portfoliocm.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["detail"].SetDataSource(ds.Tables[0]);
            report.VerifyDatabase();
            if (ddlrpttype.SelectedItem.Value.ToString() == "0")
            {
                report.SetParameterValue("@Field", (object)"SUMMERY");
            }
            else
            {
                report.SetParameterValue("@Field", (object)"DETAIL");
            }
            if (ddlrpttype.SelectedItem.Value.ToString() == "0")
            {
                report.SetParameterValue("@REPORTNAME", (object)"Portfolio Performance Report : [Summary]");
            }
            else if (ddlrpttype.SelectedItem.Value.ToString() == "1")
            {
                report.SetParameterValue("@REPORTNAME", (object)"Portfolio Performance Report : [Detail]");
            }
            else if (ddlrpttype.SelectedItem.Value.ToString() == "4")
            {
                report.SetParameterValue("@REPORTNAME", (object)"Portfolio Performance Report : Closing Stock [Detail]");
            }

            if (ChkDISTRIBUTION.Checked)
            {
                report.SetParameterValue("@Field1", (object)"CHK");
            }
            else
            {
                report.SetParameterValue("@Field1", (object)"UNCHK");
            }
            if (ddlclosmethod.SelectedItem.Value.ToString() == "0")
            {
                report.SetParameterValue("@Field2", (object)"Up To Date :  " + oconverter.ArrangeDate2(dtfor.Value.ToString()));
            }
            else
            {
                report.SetParameterValue("@Field2", (object)"Up To Date :  " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + " / Valuation Date :" + oconverter.ArrangeDate2(dtValuationDate.Value.ToString()));

            }
            report.SetParameterValue("@Field3", (object)ddlclosmethod.SelectedItem.Text.ToString().Trim());
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Portfolio Performance Report");
            report.Dispose();
            GC.Collect();
        }
        private void AddTotalRow(DataTable dt, string[] colname)
        {
            DataRow dr = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++) dr[i] = String.Empty;
            string ComputeArg = String.Empty;
            Double ColTotal = 0;
            foreach (string col in colname)
            {
                if (dt.Columns.Contains(col))
                {
                    foreach (DataRow drSum in dt.Rows)
                    {
                        ColTotal = ColTotal + Convert.ToDouble(drSum[col] != DBNull.Value ? Convert.ToDouble(drSum[col].ToString().Replace(",", "")) : 0);
                    }
                    dr[col] = ColTotal.ToString();
                    ColTotal = 0;
                }
            }
            dr[0] = "Total : ";
            dt.Rows.Add(dr);
        }
        private void RemoveColumn(DataTable dt, string[] colname)
        {
            foreach (string col in colname)
            {
                if (dt.Columns.Contains(col))
                    dt.Columns.Remove(col);
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if ((ddlrpttype.SelectedValue == "4" || ddlrpttype.SelectedValue == "1" || ddlrpttype.SelectedValue == "0") && rbExcel.Checked)
                        {
                            if (ds.Tables.Count > 1)
                                if (ds.Tables[1].Rows.Count > 0)
                                    Export(ds.Tables[1]);
                                else
                                    Export(ds);
                            else
                                Export(ds);
                        }
                        else
                            Export(ds);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);

        }
        void Ds_TotalOfColumn(DataSet Ds, string[] Column)
        {

        }
        void Export(DataSet ds)
        {
            DataTable dtExport = new DataTable();

            //if (Request.QueryString["type"].ToString() == "03")
            //{
            //    Gds = new GenericDataSet();
            //    DataSet DsToExport = new DataSet();
            //    string[] Name = {"ClientName","UCC","ProductName","OpQty","OPValue","OpeningAvgCost","BuyQty","BuyValue","BuyAvgCost","SellQty","SellValue","SellAvgCost","SqrQty","SqrPL","STPL","LTPL","STTaxTotalStt","NetPL","CloseQty","CloseValue","CloseAvgCost"};
            //    string[] Type = {"V","V","V","N","N","N","N","N","N","N","N","N","N","N","N","N","N","N","N","N","N"};
            //    string[] Size = {"250","20","200","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4","18,4"};
            //    //string ColToShowName = @"Client,UCC,Product,Open,Op.Value,Op.Avg,Buy,Buy.Value,Buy.Avg,Sell,Sell.Value,Sell.Avg,Sq.Qty,Sq.PL,ST.PL,LT.PL,STT,Net.P/L,Close,Close.Value,Close.Avg";
            //    Gds.Ds_Select(ds, Name, Type, Size);



            //    //DsToExport = Gds.Ds_RemoveUnWantedColumn(ds, ColToShow, ColToShowName);
            //    //string ColToShow = @"OpQty,OPValue,BuyQty,SellValue,SqrPL,STPL,LTPL,NetPL,CloseValue";
            //    //DsToExport = Gds.Ds_ReplaceCharFromColumnField(ds, ColToShow, ",", "");
            //    //Gds.Ds_ColumnSum(DsToExport, ColToShow, "abc", "abc");

            //    //dtExport = DsToExport.Tables[0].Copy();
            //}
            //else
            dtExport = ds.Tables[0].Copy();
            string str = null;

            str = "Portfolio Performance Report || Report Type : " + ddlrpttype.SelectedItem.Text.Trim() + " || Valuation Method : " + ddlclosmethod.SelectedItem.Text.Trim() + " || Report As On : " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            if (ddlclosmethod.SelectedValue == "1" || ddlclosmethod.SelectedValue == "2" | ddlclosmethod.SelectedValue == "3")
            {
                str = str + " || Valuation Date : " + oconverter.ArrangeDate2(dtValuationDate.Value.ToString());
            }

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Portfolio Performance Report", "Total :", dtReportHeader, dtReportFooter);

        }
        void Export(DataTable dtExport)
        {
            string str = null;

            if (Request.QueryString["type"].ToString() == "01")
                str = "Portfolio Performance Report(Trading A/c) || ";
            if (Request.QueryString["type"].ToString() == "03")
                str = "Portfolio Performance Report(Client) || ";
            str = str + "Report Type : " + ddlrpttype.SelectedItem.Text.Trim() + " || Valuation Method : " + ddlclosmethod.SelectedItem.Text.Trim() + " || Report As On : " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            if (ddlclosmethod.SelectedValue == "1" || ddlclosmethod.SelectedValue == "2" | ddlclosmethod.SelectedValue == "3")
            {
                str = str + " || Valuation Date : " + oconverter.ArrangeDate2(dtValuationDate.Value.ToString());
            }

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();
            DataTable CompanyName = null;
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            if (ddlrpttype.SelectedValue != "3")
            {
                CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            }
            else
            {
                CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId in (" + HiddenField_Company.Value + ")");

                foreach (DataRow DrCompany in CompanyName.Rows)
                {
                    if (strCompanyName == string.Empty)
                        strCompanyName = DrCompany[0].ToString();
                    else
                        strCompanyName = strCompanyName + DrCompany[0].ToString();
                }
                HeaderRow[0] = strCompanyName;
            }
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Portfolio Performance Report", "Branch,Total :,Grand Total :", dtReportHeader, dtReportFooter);
        }

    }
}