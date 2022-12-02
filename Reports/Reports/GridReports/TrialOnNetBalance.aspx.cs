using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class TrialOnNetBalance : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        CommonBL cbl = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        decimal TotalOPDebit = 0, TotalOPCredit = 0, TotalOPBal = 0;
        decimal TotalCloseDebit = 0, TotalCloseCredit = 0, TotalClBal=0;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/TrialOnNetBalance.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Trial On Net Balance";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                drdExport.SelectedIndex = 0;
                ddlExport2.SelectedIndex = 0;
                ddlExport3.SelectedIndex = 0;
                ddlstkdetails.SelectedIndex = 0;
                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_ledger"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["IsTrialOnNetBalFilter"] = null;
                Session["IsTrialOnNetBalDetFilter"] = null;
                Session["IsTrialOnNetBalDet2Filter"] = null;
                Session["IsTrialOnNetBalStkValDetFilter"] = null;
                Session["TBONNBGrid2LedgerId"] = null;
                Session["TBONNBGrid2Ledgertype"] = null;
                Session["TBONNBGrid3LedgerId"] = null;
                Session["TBONNBGrid3Ledgertype"] = null;
                
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                lookupGroup.DataBind();

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                radAsDate.Attributes.Add("OnClick", "DateAll('all')");
                radPeriod.Attributes.Add("OnClick", "DateAll('Selc')");
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                BranchHoOffice();
            }
        }

        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }

        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport(Filter);
                }
                drdExport.SelectedValue = "0";
            }

        }

        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLSX");
            options.Add("3", "RTF");
            options.Add("4", "CSV");

            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            drdExport.SelectedValue = "0";
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        public void bindexport(int Filter)
        {
            string filename = "TrialOnNetBalance";

            exporter.FileName = filename;
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            if (radAsDate.Checked == true)
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial On Net Balance" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            }
            else
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial On Net Balance" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            }

            exporter.RenderBrick += exporter_RenderBrick;
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
            }

        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }

        #region Main grid details
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CallbackPanel.JSProperties["cpPrintUrl"] = "";
            string branchid = Convert.ToString(e.Parameter.Split('~')[2]);
            bool is_asondate = false;
            string[] CallVal = e.Parameter.ToString().Split('~');
            is_asondate = Convert.ToBoolean(CallVal[1]);

            string isGridorPrint = Convert.ToString(e.Parameter.Split('~')[0]);

            string IsTrialOnNetBalFilter = Convert.ToString(hfIsTrialOnNetBalFilter.Value);
            Session["IsTrialOnNetBalFilter"] = IsTrialOnNetBalFilter;

            string asondate = "";
            if (is_asondate == false)
            {
                asondate = "N";
            }
            else
            {
                asondate = "Y";
            }

            Session["Isasondate"] = asondate;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);


            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);

            if ((ASPxFromDate.Date <= Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"])) && ASPxFromDate.Date >= Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]))) || (ASPxToDate.Date <= Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"])) && ASPxToDate.Date >= Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]))))
            {
                string FROMDATE = "";
                string TODATE = "";
                if (asondate == "Y")
                {
                    FROMDATE = dtTo.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                else
                {
                    FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }

                string BRANCH_ID = "";

                string BranComponent = "";
                List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Bran in BranList)
                {
                    BranComponent += "," + Bran;
                }
                BRANCH_ID = BranComponent.TrimStart(',');

                string GroupList = "";
                List<object> GrpList = lookupGroup.GridView.GetSelectedFieldValues("ID");
                foreach (object GrpIDs in GrpList)
                {
                    GroupList += "," + GrpIDs;
                }
                GroupList = GroupList.TrimStart(',');

                int checkshowzerobal = 0;
                if (chkZero.Checked == true)
                {
                    checkshowzerobal = 1;
                }
                else if (chkZero.Checked == false)
                {
                    checkshowzerobal = 0;
                }

                int checkCLStk = 0;
                if (chkCLStk.Checked == true)
                {
                    checkCLStk = 1;
                }
                else if (chkCLStk.Checked == false)
                {
                    checkCLStk = 0;
                }

                string Valtype = ddlValTech.SelectedValue;
                int Owmastvaltech = 0;
                if (chkOWMSTVT.Checked == true)
                {
                    Owmastvaltech = 1;
                }
                else if (chkOWMSTVT.Checked == false)
                {
                    Owmastvaltech = 0;
                }

                int PGrp = 0;
                if (chkPGRP.Checked == true)
                {
                    PGrp = 1;
                }
                else if (chkPGRP.Checked == false)
                {
                    PGrp = 0;
                }

                int Hierchy=0;
                if (chkHierarchy.Checked == true)
                {
                    Hierchy = 1;
                }
                else if (chkHierarchy.Checked == false)
                {
                    Hierchy = 0;
                }

                int ShwGrp = 0;
                if (chkGroup.Checked == true)
                {
                    ShwGrp = 1;
                }
                else if (chkGroup.Checked == false)
                {
                    ShwGrp = 0;
                }

                int ShwNetDrCrOpCl = 0;
                if (chkNetDrCrOpCl.Checked == true)
                {
                    ShwNetDrCrOpCl = 1;
                }
                else if (chkNetDrCrOpCl.Checked == false)
                {
                    ShwNetDrCrOpCl = 0;
                }

                int OpAsOnDt = 0;
                if (chkOpAsOnDt.Checked == true)
                {
                    OpAsOnDt = 1;
                }
                else if (chkOpAsOnDt.Checked == false)
                {
                    OpAsOnDt = 0;
                }

                int ConsLandCost = 0;
                if (chkConsLandCost.Checked == true)
                {
                    ConsLandCost = 1;
                }
                else if (chkConsLandCost.Checked == false)
                {
                    ConsLandCost = 0;
                }

                int ConsOverheadCost=0;
                if(chkConsOverHeadCost.Checked==true)
                {
                    ConsOverheadCost = 1;
                }
                else if(chkConsOverHeadCost.Checked==false)
                {
                    ConsOverheadCost = 0;
                }

                string BRANCH_NAME = "";
                string BranchNameComponent = "";
                List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
                foreach (object BranchName in BranchNameList)
                {
                    BranchNameComponent += "," + BranchName;
                }
                if (BranchNameList.Count > 1)
                {
                    BRANCH_NAME = "Multiple Branch Selected";
                    Session["BranchNames"] = BRANCH_NAME;
                }
                else
                {
                    BRANCH_NAME = BranchNameComponent.TrimStart(',');
                    Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
                }
                CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

                if (isGridorPrint == "Grid")
                {
                    Task PopulateStockTrialDataTask = new Task(() => GetTrialOnNetBaldata(FROMDATE, TODATE, BRANCH_ID, asondate, checkshowzerobal, branchid, GroupList, checkCLStk));
                    PopulateStockTrialDataTask.RunSynchronously();
                    ShowGrid.ExpandAll();
                }
                else
                {
                    string reportName = "";
                    string RptModuleName = "TRIALONNETBALSUMARY";
                    if (chkHierarchy.Checked == false && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == false && Convert.ToString(Session["Isasondate"]) == "Y")
                    {
                        if (chkPGRP.Checked == false)
                        {
                            reportName = "OnlyAsOnDate~D";
                        }
                        else if (chkPGRP.Checked == true)
                        {
                            reportName = "OnlyAsOnDate_ParentGrp~D";
                        }
                        
                    }
                    else if (chkHierarchy.Checked == false && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == false && Convert.ToString(Session["Isasondate"]) == "N")
                    {
                        if (chkPGRP.Checked == false)
                        {
                            reportName = "OnlyPeriod~D";
                        }
                        else if (chkPGRP.Checked == true)
                        {
                            reportName = "OnlyPeriod_ParentGrp~D";
                        }
                    }
                    else if (chkHierarchy.Checked == true && chkPGRP.Checked == false && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == false && Convert.ToString(Session["Isasondate"]) == "Y")
                    {
                        reportName = "OnlyAsOnDate_Hierarchial~D";
                    }
                    else if (chkHierarchy.Checked == false && chkPGRP.Checked == false && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == true && Convert.ToString(Session["Isasondate"]) == "Y")
                    {
                        reportName = "OnlyAsOnDate_Group~D";
                    }
                    else if (chkHierarchy.Checked == false && (chkPGRP.Checked == true || chkPGRP.Checked == false) && chkNetDrCrOpCl.Checked == true && chkGroup.Checked == false && Convert.ToString(Session["Isasondate"]) == "N")
                    {
                        if (chkPGRP.Checked == true)
                        {
                            reportName = "OnlyPeriod_NetDrCrParentGrp~D";
                        }
                        if (chkPGRP.Checked == false)
                        {
                            reportName = "OnlyPeriod_NetDrCrOpCl~D";
                        }
                    }
                    //CallbackPanel.JSProperties["cpPrintUrl"] = reportName + "\\" + FROMDATE + "\\" + TODATE + "\\" + BRANCH_ID + "\\" + asondate + "\\" + checkshowzerobal + "\\" + branchid + "\\" + GroupList + "\\" + checkCLStk + "\\" + Valtype + "\\" + Owmastvaltech + "\\" + PGrp + "\\" + Hierchy + "\\" + ShwGrp + "\\" + ShwNetDrCrOpCl + "\\" + OpAsOnDt + "\\" + RptModuleName;
                    //CallbackPanel.JSProperties["cpPrintUrl"] = reportName + "\\" + FROMDATE + "\\" + TODATE + "\\" + BRANCH_ID + "\\" + asondate + "\\" + checkshowzerobal + "\\" + branchid + "\\" + GroupList + "\\" + checkCLStk + "\\" + Valtype + "\\" + Owmastvaltech + "\\" + PGrp + "\\" + Hierchy + "\\" + ShwGrp + "\\" + ShwNetDrCrOpCl + "\\" + OpAsOnDt + "\\" + ConsLandCost + "\\" + RptModuleName;
                    CallbackPanel.JSProperties["cpPrintUrl"] = reportName + "\\" + FROMDATE + "\\" + TODATE + "\\" + BRANCH_ID + "\\" + asondate + "\\" + checkshowzerobal + "\\" + branchid + "\\" + GroupList + "\\" + checkCLStk + "\\" + Valtype + "\\" + Owmastvaltech + "\\" + PGrp + "\\" + Hierchy + "\\" + ShwGrp + "\\" + ShwNetDrCrOpCl + "\\" + OpAsOnDt + "\\" + ConsLandCost + "\\" + ConsOverheadCost+"\\"+RptModuleName;
                }
            }
            else
            {
                ShowGrid.JSProperties["cpErrorFinancial"] = "ErrorFinancial";
            }
        }


        public void GetTrialOnNetBaldata(string FROMDATE, string TODATE, string BRANCH_ID, string asondate, int checkshowzerobal, string HeadBranch, string GroupID, int checkCLStk)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TRIALONNETBALANCE_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@ASONDATE", asondate);
                cmd.Parameters.AddWithValue("@SHOWZEROBAL", checkshowzerobal);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@CONSCLOSESTK", checkCLStk);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWPARRENTGRP", (chkPGRP.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@HIERARCHICAL", (chkHierarchy.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWGROUP", (chkGroup.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWNETDRCROPCL", (chkNetDrCrOpCl.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@OPASONDATE", (chkOpAsOnDt.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@TABLENAME", "");
                cmd.Parameters.AddWithValue("@ISPRINT", 0);
                cmd.Parameters.AddWithValue("@LOADORPREVIEW", "P");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item.FieldName == "LEDGER")
            {
                e.Text = "Net Total";
            }
            else if (e.Item.FieldName == "GRPSUBGRPLEDGER")
            {
                e.Text = "Net Total";
            }
            else if (e.Item.FieldName == "OP_DR")
            {
                TotalOPDebit = Convert.ToDecimal(e.Value);
                e.Text = Convert.ToString(TotalOPDebit);
            }
            else if (e.Item.FieldName == "OP_CR")
            {
                TotalOPCredit = Convert.ToDecimal(e.Value);
                e.Text = Convert.ToString(TotalOPCredit);
            }
            else if (e.Item.FieldName == "OPBAL")
            {
                TotalOPBal = TotalOPDebit - TotalOPCredit;
                e.Text = Convert.ToString(Math.Abs((TotalOPBal)));
            }
            else if (e.Item.FieldName == "CLOSE_DR")
            {
                TotalCloseDebit = Convert.ToDecimal(e.Value);
                e.Text = Convert.ToString(TotalCloseDebit);
            }
            else if (e.Item.FieldName == "CLOSE_CR")
            {
                TotalCloseCredit = Convert.ToDecimal(e.Value);
                e.Text = Convert.ToString(TotalCloseCredit);
            }
            else if (e.Item.FieldName == "CLBAL")
            {
                TotalClBal = TotalCloseDebit - TotalCloseCredit;
                e.Text = Convert.ToString(Math.Abs((TotalClBal)));
            }
            else
            {
                e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
            }
        }

        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["dtLedger"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dtLedger"];
            }

            if (chkHierarchy.Checked==false && chkPGRP.Checked==false && chkNetDrCrOpCl.Checked==false && chkGroup.Checked==false)
            {
                ASPxGridView grid = (ASPxGridView)sender;
                if (Convert.ToString(Session["Isasondate"]) == "Y")
                {
                    foreach (GridViewDataColumn c in grid.Columns)
                    {
                        if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                        {
                            c.Visible = true;
                            c.Caption = "Debit Bal.";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                        {
                            c.Visible = true;
                            c.Caption = "Credit Bal.";
                            c.PropertiesEdit.DisplayFormatString = "#####,##,##,###0.00;";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                    }
                }
                else
                {
                    foreach (GridViewDataColumn c in grid.Columns)
                    {
                        if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                        {
                            c.Visible = true;
                            c.Caption = "Closing Bal(Dr)";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                        {
                            c.Visible = true;
                            c.Caption = "Closing Bal(Cr)";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                    }
                }
            }
            else if (chkHierarchy.Checked == false && chkPGRP.Checked == true && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == false)
            {
                ASPxGridView grid = (ASPxGridView)sender;
                if (Convert.ToString(Session["Isasondate"]) == "Y")
                {
                    foreach (GridViewDataColumn c in grid.Columns)
                    {
                        if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                        {
                            c.Visible = true;
                            c.Caption = "Debit Bal.";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                        {
                            c.Visible = true;
                            c.Caption = "Credit Bal.";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                    }
                }
                else
                {
                    foreach (GridViewDataColumn c in grid.Columns)
                    {
                        if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                        {
                            c.Visible = true;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                        {
                            c.Visible = true;
                            c.Caption = "Closing Bal(Dr)";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                        {
                            c.Visible = true;
                            c.Caption = "Closing Bal(Cr)";
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                        {
                            c.Visible = false;
                        }
                    }
                }
            }
            else if (chkHierarchy.Checked == false && (chkPGRP.Checked == true || chkPGRP.Checked == false) && chkNetDrCrOpCl.Checked == true && chkGroup.Checked == false && Convert.ToString(Session["Isasondate"]) == "N")
            {
                ASPxGridView grid = (ASPxGridView)sender;
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                    {
                        c.Visible = false;
                    }
                    if (chkPGRP.Checked == true)
                    {
                        if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                        {
                            c.Visible = true;
                        }
                    }
                    else if (chkPGRP.Checked == false)
                    {
                        if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                        {
                            c.Visible = false;
                        }
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                    {
                        c.Visible = true;
                    }
                }
            }
            else if (chkHierarchy.Checked == true && chkPGRP.Checked == false && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == false && Convert.ToString(Session["Isasondate"]) == "Y")
            {
                ASPxGridView grid = (ASPxGridView)sender;
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                    {
                        c.Visible = true;
                        c.Width = 1100;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                    {
                        c.Visible = true;
                        c.Caption = "Debit Bal.";
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                    {
                        c.Visible = true;
                        c.Caption = "Credit Bal.";
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                    {
                        c.Visible = false;
                    }
                }
            }
            else if (chkHierarchy.Checked == false && chkPGRP.Checked == false && chkNetDrCrOpCl.Checked == false && chkGroup.Checked == true && Convert.ToString(Session["Isasondate"]) == "Y")
            {
                ASPxGridView grid = (ASPxGridView)sender;
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("LEDGER"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("GRPSUBGRPLEDGER"))
                    {
                        c.Visible = true;
                        c.Width = 1100;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PARENTGRPNAME"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OPBAL"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OPDRCRTYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PR_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("PR_CR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLOSE_DR"))
                    {
                        c.Visible = false;
                        //c.Caption = "Debit Bal.";
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLOSE_CR"))
                    {
                        c.Visible = false;
                        //c.Caption = "Credit Bal.";
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLBAL"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CLDRCRTYPE"))
                    {
                        c.Visible = true;
                    }
                }
            }
        }

        protected void ShowGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
        }

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string strcellvalue = Convert.ToString(e.CellValue);
            if (e.CellValue != null)
            {
               if (e.CellValue.ToString().Contains("padding-left:32px"))
                {
                    e.Cell.Style.Add("padding-left", "32px");
                }
                else if (e.CellValue.ToString().Contains("padding-left:52px"))
                {
                    e.Cell.Style.Add("padding-left", "52px");
                }
            }
        }

        protected void ShowGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string Space1 = Convert.ToString(e.GetValue("TREESPACE"));
            if (Space1.Contains("~TREESPACE1"))
            {
            }
        }

        #endregion

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);

                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
            }
        }
        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

        #region ##### 2nd Level Grid Details #########
        protected void CallbackPanel2_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string asondatewise;
            string ledgerDesc;
            string ledgertype;
            string shortnametype;
            string[] CallVal2ndlevel = e.Parameter.ToString().Split('~');
            ledger = CallVal2ndlevel[0];
            Session["TBONNBGrid2LedgerId"] = ledger;
            asondatewise = Convert.ToString(CallVal2ndlevel[1]);
            ledgertype = CallVal2ndlevel[3];
            Session["TBONNBGrid2Ledgertype"] = ledgertype;
            DataTable dtshortnametype = null;
            shortnametype = "";

            string IsTrialOnNetBalDetFilter = Convert.ToString(hfIsTrialOnNetBalDetFilter.Value);
            Session["IsTrialOnNetBalDetFilter"] = IsTrialOnNetBalDetFilter;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            if (ledger != "null" && ledger != "0")
            {
                if (ledger != "SYSTM00010" && (ledgertype == "LEDG"))
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                    dtshortnametype = oDBEngine.GetDataTable("Select MainAccount_AccountCode from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                    shortnametype = dtshortnametype.Rows[0][0].ToString();
                }
                else if (ledger == "SYSTM00010" && ledgertype == "SUSP")
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where MainAccount_AccountCode='" + ledger + "'");
                    dtshortnametype = oDBEngine.GetDataTable("Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode='" + ledger + "'");
                    shortnametype = dtshortnametype.Rows[0][0].ToString();
                    CallbackPanel2.JSProperties["cpSuspLedger"] = "SYSTM00010";
                }
                else
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where branch_id='" + ledger + "'");
                    shortnametype = "BRAN";
                }
                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
                dtshortnametype = null;
                shortnametype = null;
            }

            Session["Grid2shortnametype"] = shortnametype;

            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = "";
                string TODATE = "";
                if (asondatewise == "Y")
                {
                    FROMDATE = dtTo.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                else
                {
                    FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }

                string BRANCH_ID = "";

                string BranComponent = "";
                List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Bran in BranList)
                {
                    BranComponent += "," + Bran;
                }
                BRANCH_ID = BranComponent.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[2]);

                GetTrialOnNetBalance2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, ledgertype, shortnametype);

                CallbackPanel2.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                CallbackPanel2.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                CallbackPanel2.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                CallbackPanel2.JSProperties["cpLedgerType"] = Convert.ToString(ledgertype);
            }
            else
            {
                ShowGridDetails2Level.DataSource = null;
                ShowGridDetails2Level.DataBind();
            }
        }

        private void GetTrialOnNetBalance2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, string ledgertype, string shortnametype)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TRIALONNETBALANCEDETAIL_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", ToDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@ASONDATE", asondatewise);
                cmd.Parameters.AddWithValue("@MAINACC_LEDGER", ledger);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@LEDGER_TYPE", ledgertype);
                cmd.Parameters.AddWithValue("@LEDGER_SHORTNAME", shortnametype);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
                //return null;
            }
        }
        protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if ((Convert.ToString(Session["Grid2shortnametype"]) == "SYSTM00003" || Convert.ToString(Session["Grid2shortnametype"]) == "SYSTM00006") && Convert.ToString(Session["TBONNBGrid2Ledgertype"]) == "LEDG")
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("TRN_REFID"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("ACL_TRNDT"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CUSTVENDNAME"))
                    {
                        c.Visible = true;
                        c.Width = 200;
                    }

                    if (Convert.ToString(Session["Grid2shortnametype"]) == "SYSTM00006")
                    {
                        if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                        {
                            c.Visible = true;
                            c.Width = 200;
                        }
                    }
                    else if (Convert.ToString(Session["Grid2shortnametype"]) == "SYSTM00003")
                    {
                        if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                        {
                            c.Visible = false;
                        }
                    }
                }
            }
            else if ((Convert.ToString(Session["Grid2shortnametype"]) != "SYSTM00003" || Convert.ToString(Session["Grid2shortnametype"]) != "SYSTM00006") && Convert.ToString(Session["TBONNBGrid2Ledgertype"]) == "LEDG")
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("CUSTVENDNAME"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("TRN_REFID"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("ACL_TRNDT"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                }
            }

            else if (Convert.ToString(Session["TBONNBGrid2Ledgertype"]) == "BRAN")
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("CUSTVENDNAME"))
                    {
                        c.Visible = true;
                    }

                    if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("TRN_REFID"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("ACL_TRNDT"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                }
            }
        }

        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void ddlExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport2.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details2(Filter);
            }
        }

        public void bindexport_Details2(int Filter)
        {
            string filename = "TrialOnNetBalanceDetail";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "TrialOnNetBalanceDetail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            if (radAsDate.Checked == true)
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial On Net Balance - Details" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
            }
            else
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial On Net Balance - Details" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            }

            exporterDetails.RenderBrick += exporter_RenderBrick;

            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails2Level";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WriteXlsxToResponse();
                    break;
                case 2:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 3:
                    exporterDetails.WriteCsvToResponse();
                    break;
                case 4:
                    exporterDetails.WriteRtfToResponse();
                    break;
                default:
                    return;
            }
        }

        #endregion

        #region ##### 2nd Level Grid Details for Stock #########
        protected void CallbackPanel3_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string asondatewise;
            string ledgerDesc;
            string ledgertype;
            string shortnametype;
            string[] CallVal3rdlevel = e.Parameter.ToString().Split('~');
            ledger = CallVal3rdlevel[0];
            Session["TBONNBGrid3LedgerId"] = ledger;
            asondatewise = Convert.ToString(CallVal3rdlevel[1]);
            ledgertype = CallVal3rdlevel[3];
            Session["TBONNBGrid3Ledgertype"] = ledgertype;
            DataTable dtshortnametype = null;
            shortnametype = "";

            string IsTrialOnNetBalDet2Filter = Convert.ToString(hfIsTrialOnNetBalDet2Filter.Value);
            Session["IsTrialOnNetBalDet2Filter"] = IsTrialOnNetBalDet2Filter;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            if (ledger != "null" && ledger != "0")
            {
                if (ledger != "SYSTM00010" && (ledgertype == "STKV"))
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                    dtshortnametype = oDBEngine.GetDataTable("Select MainAccount_AccountCode from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                    shortnametype = dtshortnametype.Rows[0][0].ToString();
                }
                else if (ledger == "SYSTM00010" && ledgertype == "SUSP")
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where MainAccount_AccountCode='" + ledger + "'");
                    dtshortnametype = oDBEngine.GetDataTable("Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode='" + ledger + "'");
                    shortnametype = dtshortnametype.Rows[0][0].ToString();
                    CallbackPanel3.JSProperties["cpSuspLedger"] = "SYSTM00010";
                }
                else
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where branch_id='" + ledger + "'");
                    shortnametype = "BRAN";
                }
                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
                dtshortnametype = null;
                shortnametype = null;
            }

            Session["Grid3shortnametype"] = shortnametype;

            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = "";
                string TODATE = "";
                if (asondatewise == "Y")
                {
                    FROMDATE = dtTo.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                else
                {
                    FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }

                string BRANCH_ID = "";

                string BranComponent = "";
                List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Bran in BranList)
                {
                    BranComponent += "," + Bran;
                }
                BRANCH_ID = BranComponent.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[2]);

                GetTrialOnNetBalance2ndLevelforStock(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, ledgertype, shortnametype);

                CallbackPanel3.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                CallbackPanel3.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                CallbackPanel3.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                CallbackPanel3.JSProperties["cpLedgerType"] = Convert.ToString(ledgertype);
            }
            else
            {
                ShowGridDetails3Level.DataSource = null;
                ShowGridDetails3Level.DataBind();
            }
        }

        private void GetTrialOnNetBalance2ndLevelforStock(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, string ledgertype, string shortnametype)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TRIALONNETBALANCEDETAIL_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", ToDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@ASONDATE", asondatewise);
                cmd.Parameters.AddWithValue("@MAINACC_LEDGER", ledger);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@LEDGER_TYPE", ledgertype);
                cmd.Parameters.AddWithValue("@LEDGER_SHORTNAME", shortnametype);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
                //return null;
            }
        }

        protected void ShowGridDetails3Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport3.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details3(Filter);
            }
        }

        public void bindexport_Details3(int Filter)
        {
            string filename = "TrialOnNetBalanceDetail";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "TrialOnNetBalanceDetail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            if (radAsDate.Checked == true)
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial On Net Balance - Details" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
            }
            else
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial On Net Balance - Details" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            }

            exporterDetails.RenderBrick += exporter_RenderBrick;

            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails3Level";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WriteXlsxToResponse();
                    break;
                case 2:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 3:
                    exporterDetails.WriteCsvToResponse();
                    break;
                case 4:
                    exporterDetails.WriteRtfToResponse();
                    break;
                default:
                    return;
            }
        }

        #endregion

        #region #####Stock Valuation Details#####
        protected void CallbackPanelStockDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsTrialOnNetBalStkValDetFilter = Convert.ToString(hfIsTrialOnNetBalStkValDetFilter.Value);
            Session["IsTrialOnNetBalStkValDetFilter"] = IsTrialOnNetBalStkValDetFilter;

            DateTime dtFrom;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            DateTime dtTo;
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = "";
            string TODATE = "";

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPopupgrid")
            {
                string branch = returnPara.Split('~')[1];
                string prodId = returnPara.Split('~')[2];
                string asondt = returnPara.Split('~')[3];

                if (asondt == "Y")
                {
                    FROMDATE = dtTo.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                else
                {
                    FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }

                GetStockValuationDet(prodId, branch, FROMDATE, TODATE);
            }

        }

        public void GetStockValuationDet(string ProductIds, string BRANCH_ID, string FROMDATE, string TODATE)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_ProductValuation_Report", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", ProductIds);
                cmd.Parameters.AddWithValue("@GetType", "Details");
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@Class", "");
                cmd.Parameters.AddWithValue("@Brand", "");
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOPASONDATE", "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@PAGELINK", "/GridReports/TrialOnNetBalanceDetail.aspx");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
                //return null;
            }
        }

        protected void grivaluation_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion

        #region Export Valuation Details
        public void cmbExportStkDet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlstkdetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportstkvaldet"] == null)
                {
                    Session["exportstkvaldet"] = Filter;
                    bindexportstkdet(Filter);
                }
                else if (Convert.ToInt32(Session["exportstkvaldet"]) != Filter)
                {
                    Session["exportstkvaldet"] = Filter;
                    bindexportstkdet(Filter);
                }
                ddlstkdetails.SelectedValue = "0";
            }
        }

        public void bindexportstkdet(int Filter)
        {
            string filename = "StockValuationdetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Stock Valuation Details" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "grivaluation";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
            }
        }
        #endregion

        protected void lookupGroup_DataBinding(object sender, EventArgs e)
        {
            lookupGroup.DataSource = GetGroupList();
        }

        public DataTable GetGroupList()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_ACCOUNTGROUPSELECTION_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }

        protected void GroupPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            lookupGroup.GridView.Selection.CancelSelection();
            lookupGroup.DataSource = GetGroupList();
            lookupGroup.DataBind();
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTrialOnNetBalFilter"]) == "Y")
            {
                var q = from d in dc.TRIALONNETBALANCE_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TRIALONNETBALANCE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }

            ShowGrid.ExpandAll();
        }

        protected void GenerateEntityServerModeDataSourceDetails_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTrialOnNetBalDetFilter"]) == "Y")
            {
                var q = from d in dc.TRIALONNETBALANCEDETAIL_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TRIALONNETBALANCEDETAIL_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateEntityServerModeDataSourceDetails2_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTrialOnNetBalDet2Filter"]) == "Y")
            {
                var q = from d in dc.TRIALONNETBALANCEDETAIL_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TRIALONNETBALANCEDETAIL_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateEntityServerStockDetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTrialOnNetBalStkValDetFilter"]) == "Y")
            {
                var q = from d in dc.STOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Details"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }

            string strlandcost = (chkConsLandCost.Checked) ? "1" : "0";

            if (ddlValTech.SelectedValue == "F")
            {
                grivaluation.Columns[8].Visible = true;
                grivaluation.Columns[9].Visible = true;
                grivaluation.Columns[10].Visible = true;
                if (Convert.ToString(strlandcost) == "1")
                {
                    grivaluation.Columns[11].Visible = true;
                }
                else if (Convert.ToString(strlandcost) == "0")
                {
                    grivaluation.Columns[11].Visible = false;
                }
                grivaluation.Columns[12].Visible = true;
                grivaluation.Columns[13].Visible = false;
                grivaluation.Columns[14].Visible = false;
                grivaluation.Columns[15].Visible = false;
                grivaluation.Columns[16].Visible = false;
                grivaluation.Columns[17].Visible = false;
                grivaluation.Columns[18].Visible = false;
                grivaluation.Columns[19].Visible = false;
                grivaluation.Columns[20].Visible = false;
                grivaluation.Columns[21].Visible = false;
                grivaluation.Columns[22].Visible = false;
            }
            else
            {
                grivaluation.Columns[8].Visible = false;
                grivaluation.Columns[9].Visible = true;
                grivaluation.Columns[10].Visible = false;
                if (Convert.ToString(strlandcost) == "1")
                {
                    grivaluation.Columns[11].Visible = true;
                }
                else if (Convert.ToString(strlandcost) == "0")
                {
                    grivaluation.Columns[11].Visible = false;
                }
                grivaluation.Columns[12].Visible = false;
                grivaluation.Columns[13].Visible = true;
                grivaluation.Columns[14].Visible = true;
                grivaluation.Columns[15].Visible = true;
                grivaluation.Columns[16].Visible = true;
                grivaluation.Columns[17].Visible = true;
                grivaluation.Columns[18].Visible = true;
                grivaluation.Columns[19].Visible = true;
                grivaluation.Columns[20].Visible = true;
                grivaluation.Columns[21].Visible = true;
                grivaluation.Columns[22].Visible = true;
            }
        }

        #endregion
    }
}