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
    public partial class CustomerOutstandingVSAgeingAnalysis : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ArrayList amountfields_caption = null;
        ArrayList amountfields_fields = null;
        ArrayList bandedfields = null;

        DataTable dtPartyTotal = null;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/CustomerOutstandingVSAgeingAnalysis.aspx");
            DateTime dtFrom;
            DateTime dtTo;
           
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Customer Outstanding Vs. Ageing Analysis";
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

                Session["SI_ComponentData_Branch"] = null;
                BranchHoOffice();
                Session["exportval"] = null;
                Session["CustOutvsAgeingAnalysis"] = null;
                Session["CustOutAgeingTotal"] = null;
                Session["Intervl"] = null;
                Session["HeadersAmountCaption"] = null;
                Session["HeadersAmountFields"] = null;
                Session["GridviewsBandedFields"] = null;
               

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxAsOnDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                chkallcust.Attributes.Add("OnClick", "CustAll('allcust')");
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            }

            if (ShowGridCustOutVsAgeingAnalysis.Columns["DOC_NO"] == null)
            {
                //==============By default Grid populate in load===================
                DataTable dt = new DataTable();
                dt.Columns.Add("BRANCH_DESCRIPTION");
                dt.Columns.Add("TOTAL_RECEIVABLES");
                dt.Columns.Add("PARTYID");
                dt.Columns.Add("DOC_NO");


                GridViewBandColumn bandColumn = new GridViewBandColumn();
                GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                col1.FieldName = "BRANCH_DESCRIPTION";
                col1.Caption = "Unit";
                col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                col1.Width = 350;
                bandColumn.Columns.Add(col1);
                GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                col2.FieldName = "TOTAL_RECEIVABLES";
                col2.Caption = "Total Receivables";
                col2.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                //Rev Debashis Refer:0019573
                col2.PropertiesTextEdit.DisplayFormatString = "0.00";
                //End of Rev Debashis
                col2.Width = 350;
                bandColumn.Columns.Add(col2);
                GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                col3.FieldName = "PARTYID";
                col3.Caption = "No. of Accounts";
                col3.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                col3.Width = 350;
                bandColumn.Columns.Add(col3);
                GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                col4.FieldName = "DOC_NO";
                col4.Caption = "No. Of Documents";
                col4.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                col4.Width = 350;
                bandColumn.Columns.Add(col4);
                bandColumn.Caption = "Customer Details";
                bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                ShowGridCustOutVsAgeingAnalysis.Columns.Add(bandColumn);

                ShowGridCustOutVsAgeingAnalysis.DataSource = dt;
                ShowGridCustOutVsAgeingAnalysis.DataBind();
                //=====================================================================
                
            }
            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

                string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
              
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();

            tcbl = cbl.GetDateFinancila(Finyear);
            if (tcbl.Rows.Count > 0)
            {
                ASPxAsOnDate.MaxDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                ASPxAsOnDate.MinDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxAsOnDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxAsOnDate.Date = TodayDate;
                }
            }

        }
        #region Export


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {

                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {

            string filename = "Customer Outstanding Vs. Ageing Analysis";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Customer Outstanding Vs. Ageing Analysis" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 24-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridCustOutVsAgeingAnalysis";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }

        }
        //Rev Subhra 24-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion

        #region Branch Populate

        public void BranchHoOffice()
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //tcbl = cbl.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            tcbl = cbl.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (tcbl.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = tcbl;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                //Rev Debashis && Hierarchy wise Head Branch Bind
                //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
                //End of Rev Debashis
            }
        }

        //Rev Debashis && Hierarchy wise Head Branch Bind
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
        //End of Rev Debashis

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
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

        #region Customer Outstanding Vs. Ageing Analysis grid
        protected void ShowGridCustOutVsAgeingAnalysis_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ShowGridCustOutVsAgeingAnalysis.DataSource = null;

           

            string returnPara = Convert.ToString(e.Parameters);
            string HEAD_BRANCH = returnPara.Split('~')[1];
           
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;

            dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Brnch in BranchList)
            {
                BranchComponent += "," + Brnch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            //string PARTY_TYPE = "C";
            string ALLPARTY = (chkallcust.Checked) ? "1" : "0";
            string CBVOUCHER = (chkcb.Checked) ? "1" : "0";
            string JVOUCHER = (chkjv.Checked) ? "1" : "0";
            string DNCNNOTE = (chkdncn.Checked) ? "1" : "0";
            //Rev Subhra 24-12-2018   0017670

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
            ShowGridCustOutVsAgeingAnalysis.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev
            Task PopulateStockTrialDataTask = new Task(() => GetCustOutVSAgeingAnalysisdata(ASONDATE, BRANCH_ID, ALLPARTY, CBVOUCHER, JVOUCHER, DNCNNOTE));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetCustOutVSAgeingAnalysisdata(string ASONDATE, string BRANCH_ID, string ALLPARTY, string CBVOUCHER, string JVOUCHER, string DNCNNOTE)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYWISEOUTSTANDINGVSAGEINGANASUMMARY_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                //Rev Subhra 10-01-2019 mail (Fwd: Customer Outstanding VS Ageing) 
                cmd.Parameters.AddWithValue("@ONDATE", ddlondate.SelectedValue);
                //End Subhra
                cmd.Parameters.AddWithValue("@ALLPARTY", ALLPARTY);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@NOOFDAYS", Convert.ToDecimal(txt_NoofDays.Text));
                cmd.Parameters.AddWithValue("@DAYSINTERVAL", Convert.ToDecimal(txt_DaysInterval.Text));
                cmd.Parameters.AddWithValue("@PARTY_TYPE", "C");
                cmd.Parameters.AddWithValue("@INCLUDECB", CBVOUCHER);
                cmd.Parameters.AddWithValue("@INCLUDEJV", JVOUCHER);
                cmd.Parameters.AddWithValue("@EXCLUDEDNCN", DNCNNOTE);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);


                cmd.Dispose();
                con.Dispose();
                //Rev Subhra 10-01-2019 mail (Fwd: Customer Outstanding VS Ageing) 
                //Session["CustOutvsAgeingAnalysis"] = ds.Tables[0];
                DataTable dt = new DataTable();
                dt = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("BRANCH_DESCRIPTION") != "Net Total:").CopyToDataTable();
                Session["CustOutvsAgeingAnalysis"] = dt;
                Session["Intervl"] = "Y";

                DataTable dt_nTot = new DataTable();
                dt_nTot = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("BRANCH_DESCRIPTION") == "Net Total:").CopyToDataTable();
                Session["CustOutAgeingTotal"] = dt_nTot;
                //End of Rev


                ShowGridCustOutVsAgeingAnalysis.DataSource = dt;
                ShowGridCustOutVsAgeingAnalysis.DataBind();

                
            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGridCustOutVsAgeingAnalysis_DataBinding(object sender, EventArgs e)
        {

            ShowGridCustOutVsAgeingAnalysis.DataSource = null;
            //int maxColumnIndex = ShowGridCustOutVsAgeingAnalysis.Columns.Count-1 ;
            //for (int i = maxColumnIndex; i >= 0; i--)
            //{
            //    ShowGridCustOutVsAgeingAnalysis.Columns.RemoveAt(i);
            //}

            DataTable dt_CustOutvsAgeingAnalysis = (DataTable)Session["CustOutvsAgeingAnalysis"];




            if (dt_CustOutvsAgeingAnalysis != null)
            {


                int nodys=Convert.ToInt32(txt_DaysInterval.Text);
                int dysint = Convert.ToInt32(txt_NoofDays.Text) / Convert.ToInt32(txt_DaysInterval.Text);
                int interval = Convert.ToInt32(txt_NoofDays.Text) / Convert.ToInt32(txt_DaysInterval.Text);


                for (int i = 1; i <= interval; i++)
                {
                    if (!dt_CustOutvsAgeingAnalysis.Columns.Contains(("No_of_Accounts" + "_" + nodys * i)))
                    {
                        Session["Intervl"] = "N";
                    }
                }



                //if (dt_CustOutvsAgeingAnalysis.Columns.Contains(("No_of_Accounts" + "_" + Convert.ToInt32(txt_DaysInterval.Text))))
                //{ 
                    if(Session["Intervl"]=="Y")
                    { 

                    string fieldname = "";
                    string fieldnamecheck = "";
                    GridViewBandColumn bandColumn = new GridViewBandColumn();
                    int gridvisibleindex =1;
                    if (ShowGridCustOutVsAgeingAnalysis.Columns["DOC_NO"] == null)
                    {

                        GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                        col1.FieldName = "BRANCH_DESCRIPTION";
                        col1.Caption = "Unit";
                        col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col1.Width = 250;
                        bandColumn.Columns.Add(col1);
                        GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                        col2.FieldName = "TOTAL_RECEIVABLES";
                        col2.Caption = "Total Receivables";
                        col2.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        //Rev Debashis Refer:0019573
                        col2.PropertiesTextEdit.DisplayFormatString = "0.00";
                        //End of Rev Debashis
                        col2.Width = 110;
                        col1.VisibleIndex = 2;
                        bandColumn.Columns.Add(col2);
                        GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                        col3.FieldName = "PARTYID";
                        col3.Caption = "No. of Accounts";
                        col3.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        col3.Width = 110;
                        bandColumn.Columns.Add(col3);
                        GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                        col4.FieldName = "DOC_NO";
                        col4.Caption = "No. Of Documents";
                        col4.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        col4.Width = 110;
                        bandColumn.Columns.Add(col4);
                        bandColumn.Caption = "Customer Details";
                        bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        ShowGridCustOutVsAgeingAnalysis.Columns.Add(bandColumn);

                    }

                    hdnNoBandedColumn.Value = "Customer Details";
                    hdnNoCaption.Value = "Total Receivables" + "~" + "No. of Accounts" + "~" + "No. Of Documents";
                    hdnNoFields.Value =  "TOTAL_RECEIVABLES" + "~" + "PARTYID" + "~" + "DOC_NO";


                        GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                        GridViewBandColumn bandColumndyn = new GridViewBandColumn();

                        int ends = Convert.ToInt32(txt_DaysInterval.Text);
                        for (int i = 1; i <= interval; i++)
                        {
                            int start = 1, end = Convert.ToInt32(txt_DaysInterval.Text);
                            if (i == 1)
                            {

                            }
                            else
                            {
                                start = ends * (i - 1) + 1;
                            }
                            end = end * i;
                            bandColumn = new GridViewBandColumn();
                            ShowGridCustOutVsAgeingAnalysis.Columns["BRANCH_DESCRIPTION"].Width=220;
                            ShowGridCustOutVsAgeingAnalysis.Columns["TOTAL_RECEIVABLES"].Width=110;
                            ShowGridCustOutVsAgeingAnalysis.Columns["PARTYID"].Width=110;
                            ShowGridCustOutVsAgeingAnalysis.Columns["DOC_NO"].Width = 110;

                            if (ShowGridCustOutVsAgeingAnalysis.Columns["No_of_Accounts" + "_" + end] == null)
                            {
                        
                                fieldname = "No_of_Accounts" + "_" + end;
                                coldyn = new GridViewDataTextColumn();
                                coldyn.FieldName = fieldname;
                                coldyn.Caption = "No. of Accounts";
                                coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                                coldyn.Width = 110;
                                bandColumn.Columns.Add(coldyn);
                                

                                fieldname = "Receivables" + "_" + end;
                                coldyn = new GridViewDataTextColumn();
                                coldyn.FieldName = fieldname;
                                coldyn.Caption = "Receivables";
                                coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                                //Rev Debashis Refer:0019573
                                coldyn.PropertiesTextEdit.DisplayFormatString = "0.00";
                                //End of Rev Debashis
                                coldyn.Width = 110;
                                bandColumn.Columns.Add(coldyn);

                                fieldname = "No_of_Documents" + "_" + end;
                                coldyn = new GridViewDataTextColumn();
                                coldyn.FieldName = fieldname;
                                coldyn.Caption = "No. of Documents";
                                coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                                coldyn.Width = 110;
                                bandColumn.Columns.Add(coldyn);


                                bandColumn.Caption = start + "-" + end + " DAYS";
                                bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;  
                                ShowGridCustOutVsAgeingAnalysis.Columns.Add(bandColumn);
                          }
                           // Rev Subhra 10-01-2019 mail (Fwd: Customer Outstanding VS Ageing)
                            hdnNoBandedColumn.Value = hdnNoBandedColumn.Value + "~" + start + "-" + end + " DAYS" ; 
                            hdnNoCaption.Value = hdnNoCaption.Value + "~" + "No. of Accounts" + "~" + "Receivables"+ "~" + "No. of Documents";
                            hdnNoFields.Value = hdnNoFields.Value + "~" + "No_of_Accounts_" + end + "~" + "Receivables" + "_" + end + "~" + "No_of_Documents" + "_" + end;
                            //End of Rev  
                           

                      }

                    bandColumn = new GridViewBandColumn();
                    if (ShowGridCustOutVsAgeingAnalysis.Columns["No_of_Accounts_A"] == null)
                    {
                        fieldname = "No_of_Accounts_A";
                        coldyn = new GridViewDataTextColumn();
                        coldyn.FieldName = fieldname;
                        coldyn.Caption = "No. of Accounts";
                        coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        bandColumn.Columns.Add(coldyn);

                        fieldname = "Receivables_A";
                        coldyn = new GridViewDataTextColumn();
                        coldyn.FieldName = fieldname;
                        coldyn.Caption = "Receivables";
                        coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        //Rev Debashis Refer:0019573
                        coldyn.PropertiesTextEdit.DisplayFormatString = "0.00";
                        //End of Rev Debashis
                        bandColumn.Columns.Add(coldyn);

                        fieldname = "No_of_Documents_A";
                        coldyn = new GridViewDataTextColumn();
                        coldyn.FieldName = fieldname;
                        coldyn.Caption = "No. of Documents";
                        coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        bandColumn.Columns.Add(coldyn);


                        bandColumn.Caption = "ABOVE  " + Convert.ToInt32(nodys * dysint) + " DAYS";
                        bandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        ShowGridCustOutVsAgeingAnalysis.Columns.Add(bandColumn);

                        // Rev Subhra 10-01-2019 mail (Fwd: Customer Outstanding VS Ageing)
                        hdnNoBandedColumn.Value = hdnNoBandedColumn.Value + "~" + "ABOVE  " + Convert.ToInt32(nodys * dysint) + " DAYS";
                        hdnNoCaption.Value = hdnNoCaption.Value + "~" + "No. of Accounts" + "~" + "Receivables" + "~" + "No. of Documents";
                        hdnNoFields.Value = hdnNoFields.Value + "~" + "No_of_Accounts_A" + "~" + "Receivables_A" + "~" + "No_of_Documents_A";
                        //End of Rev  
                    }


                    if (Session["GridviewsBandedFields"]==null)
                    {
                        Session["GridviewsBandedFields"] = hdnNoBandedColumn.Value;
                        Session["HeadersAmountCaption"] = hdnNoCaption.Value;
                        Session["HeadersAmountFields"] = hdnNoFields.Value;
                    }
                    
                ShowGridCustOutVsAgeingAnalysis.DataSource = dt_CustOutvsAgeingAnalysis;


            }
            //}

            }
        }

        protected void ShowGridCustOutVsAgeingAnalysis_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (IsPostBack)
            {
                DataTable dt_CustAgeingTotal = (DataTable)Session["CustOutAgeingTotal"];
                if (dt_CustAgeingTotal != null)
                {

                    if (amountfields_caption == null)
                    {
                        amountfields_caption = new ArrayList((Session["HeadersAmountCaption"].ToString()).Split(new char[] { '~' }));
                        amountfields_fields = new ArrayList((Session["HeadersAmountFields"].ToString()).Split(new char[] { '~' }));
                        bandedfields = new ArrayList((Session["GridviewsBandedFields"].ToString()).Split(new char[] { '~' }));
                    }
                    else
                    {
                        if (e.Column.Caption != "Unit")
                        {
                                if (e.Column.ParentBand.ToString() == bandedfields[e.Column.ParentBand.VisibleIndex].ToString())
                                {
                                    if (e.Column.Caption == amountfields_caption[0].ToString())
                                    {
                                        if (e.IsTotalFooter)
                                        {
                                            e.Cell.Text = dt_CustAgeingTotal.Rows[0][amountfields_fields[0].ToString()].ToString();
                                            amountfields_fields.RemoveAt(0);
                                            amountfields_caption.RemoveAt(0);
                                        }


                                    }
                                }
                        }
                    }

                }
                if (e.Column.Caption == "Unit")
                {
                    if (e.IsTotalFooter)
                    {
                        e.Cell.Text = "Net Total:";
                    }
                }
            }

        }

        }

     #endregion
    }
