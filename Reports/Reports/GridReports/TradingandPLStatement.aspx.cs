#region =======================Revision History=============================================================================================================
//1.0   v2 .0.37    Debashis    01/06/2023  Running Balance Required in the Third Level Zooming report for PL - Horizontal & BS Horizontal from any Ledger.
//                                          The Running Balance of the 3rd Level zooming of any ledger from PL - Horizontal & BS - Horizontal to be set as per Ledger Type
//                                          Refer: 0026252 & 0026318
#endregion =======================End of Revision History===================================================================================================

using BusinessLogicLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraReports.UI;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports.Reports.GridReports
{
    public partial class TradingandPLStatement : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DateTime dtFrom;
        DateTime dtTo;
        static int PageStart = 1;
        //Rev 1.0
        decimal TotalDebit = 0, TotalCredit = 0, TotalOpening = 0, TotalDBCR=0;
        string MainLedgerType = "";
        //End of Rev 1.0

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/TradingandPLStatement.aspx");
            if (!IsPostBack)
            {
                Session["PLZooming"] = null;
                TreeList.Columns["Percentage"].Visible = false;
                TreeList1.Columns["Percentage"].Visible = false;
                PageStart = 1;
                Session["TradingPLdata"] = null;
                //TreeList.CollapseAll();
                //TreeList1.CollapseAll();
                radAsDate.Attributes.Add("OnClick", "DateAll('all')");
                radPeriod.Attributes.Add("OnClick", "DateAll('Selc')");

                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                BranchHoOffice();
                
            }
            TreeList.DataBind();
            TreeList1.DataBind();
            PageStart = 2;

        }

        protected void ASPxTreeList1_DataBinding(object sender, EventArgs e)
        {

            DataSet dstPl = GetPlData();
            if (dstPl != null && dstPl.Tables.Count>1)
            {
                TreeList.DataSource = dstPl.Tables[0];
                TreeList1.DataSource = dstPl.Tables[1];
            }
        }

        protected void ASPxTreeList1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlRowEventArgs e)
        {
            if (e.GetValue("ID").ToString().Contains("G"))
            {
                e.Row.BackColor = Color.FromArgb(211, 235, 183);
                e.Row.CssClass += " MakeGreen";

            }
            if (e.GetValue("ID").ToString().Contains("NET TOT"))
            {
                e.Row.ForeColor = Color.Red;
                e.Row.Style.Add("font-weight", "bold");
                

            }
            if (e.GetValue("ID").ToString().Contains("Actual TOT"))
            {
                e.Row.ForeColor = Color.White;
                e.Row.BackColor = Color.Black;
                e.Row.Style.Add("font-weight", "bold");

                e.Row.CssClass += " MakeFooter";


            }

            if (e.GetValue("ID").ToString().Contains("NET TOTPROFIT") || e.GetValue("ID").ToString().Contains("NET TOTSPRO") || e.GetValue("ID").ToString().Contains("NET TOTPLEXP"))
            {
                e.Row.ForeColor = Color.Green;
                e.Row.Style.Add("font-weight", "bold");


            }


            if (e.GetValue("ID").ToString().Contains("M"))
            {
                // e.Row.Cells[3].Text = e.GetValue("AMOUNT").ToString() + " &nbsp;&nbsp;&nbsp;&nbsp         ";
            }



        }

        DataSet GetPlData()
        {
            if (PageStart != 1)
            {

                if (Session["TradingPLdata"] != null)
                {
                    return (DataSet)Session["TradingPLdata"];
                }
                else
                {
                    string branch = "";
                    List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
                    branch = string.Join(",", lstBranch);
                    bool Period = radPeriod.Checked;
                    
                    DataSet dsInst = new DataSet();
                    if (branch != "")
                    {
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcR_TradingPlStatement", con);
                        cmd.Parameters.AddWithValue("@FromDate", ASPxFromDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Todate", ASPxToDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Branch", branch);
                        cmd.Parameters.AddWithValue("@Valuation", ddlValTech.SelectedValue);
                        cmd.Parameters.AddWithValue("@ShowZero", chkZero.Checked);
                        cmd.Parameters.AddWithValue("@ConsiderClosingStock", chkClosingStock.Checked);
                        cmd.Parameters.AddWithValue("@chkConsiderOverhead", chkConsiderOverhead.Checked);
                        cmd.Parameters.AddWithValue("@Period", Period);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]));

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd;
                        Adap.Fill(dsInst);
                        cmd.Dispose();
                        con.Dispose();
                        Session["TradingPLdata"] = dsInst;
                    }
                    return dsInst;
                }
            }
            else
            {
                return null;
            }

        }

        protected void ASPxTreeList1_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            string parameter = e.Argument;

            if (parameter == "BindGrid")
            {
                Session["TradingPLdata"] = null;
                TreeList.DataBind();
                TreeList.CollapseAll();
                if (chkPercentage.Checked)
                {
                    TreeList.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList.Columns["Percentage"].Visible = false;
                }
            }
            else if (parameter == "ExpandAll")
            {
                TreeList.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList.CollapseAll();
            }

            else if (parameter == "ShowPercentage")
            {
                TreeList.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList.Columns["Percentage"].Visible = false;
            }

            ASPxTreeList2_CustomCallback(sender, e);
        }

        protected void ASPxTreeList2_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            string parameter = e.Argument;
            if (parameter == "BindGrid")
            {
                //Session["TradingPLdata"] = null;
                TreeList1.DataBind();
                if (chkPercentage.Checked)
                {
                    TreeList1.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList1.Columns["Percentage"].Visible = false;
                }

                TreeList1.CollapseAll();
            }
            else if (parameter == "ExpandAll")
            {
                TreeList1.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList1.CollapseAll();
            }
            else if (parameter == "ShowPercentage")
            {
                TreeList1.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList1.Columns["Percentage"].Visible = false;
            }

        }



        #region Copy Old PL

        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //stbill = bll1.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
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

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

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
        //End of Rev


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
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }


        #endregion

        //protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_PLLeve2"] != null)
        //    {
        //        ShowGridDetails2Level.DataSource = (DataTable)Session["dt_PLLeve2"];
        //    }
        //    else
        //    {
        //        ShowGridDetails2Level.DataSource = null;
        //    }
        //}

        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string asondatewise;
            string ledgerDesc;
            string ledgertype;
            string[] CallVal2ndlevel = e.Parameter.ToString().Split('~');
            ledger = CallVal2ndlevel[0];
            Session["LedgerId"] = ledger;
            asondatewise = Convert.ToString(CallVal2ndlevel[1]);
            ledgertype = CallVal2ndlevel[3];
            Session["ledgertype"] = ledgertype;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            string IsProfnLossDetailsFilter = Convert.ToString(hfIsProfnlossDetails.Value);
            Session["IsProfnLossDetailsFilter"] = IsProfnLossDetailsFilter;

            if (ledger != "null" && ledger != "0" && ledger != "")
            {
                if (ledgertype != "FOR BRANCH")
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                }
                else
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where branch_id='" + ledger + "'");
                }

                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
            }


            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                //Session.Remove("dt_PLLeve2");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";

                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[2]);

                DataTable dt = new DataTable();
                dt = GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, ledgertype);


            }
            else
            {

            }
        }


        private DataTable GetGeneralLedger2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, string ledgertype)
        {

            try
            {
                if (asondatewise == "Y")
                {
                    FromDate = ToDate;
                }
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PROFIT_AND_LOSS_STATEMENT_DETAILED_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", ToDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@AsonDate", asondatewise);
                cmd.Parameters.AddWithValue("@LEDGERID", ledger);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@LEGDERTYPE", ledgertype);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        #endregion

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {


            Int32 Filter = int.Parse(Convert.ToString(1));
            PrintingSystemBase ps = new PrintingSystemBase();

            PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
            link1.Component = ExpExporter;

            PrintableComponentLinkBase link2 = new PrintableComponentLinkBase(ps);
            link2.Component = IncomeExporter;



            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { link1, link2 });


            compositeLink.Landscape = true;
            compositeLink.CreateDocument();


            using (MemoryStream stream = new MemoryStream())
            {
                XlsxExportOptions options = new XlsxExportOptions();
                options.TextExportMode = TextExportMode.Text;
                options.ExportMode = XlsxExportMode.SingleFile;
                options.ShowGridLines = true;
               

                if (Filter == 1)
                {

                    compositeLink.PrintingSystemBase.ExportToPdf(stream);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/pdf");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.pdf");                    
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
                else if (Filter == 2)
                {

                    compositeLink.PrintingSystemBase.ExportToXlsx(stream,options);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/xlsx");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.xlsx");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
                else if (Filter == 3)
                {

                    compositeLink.PrintingSystemBase.ExportToRtf(stream);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/rtf");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.rtf");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
                else if (Filter == 4)
                {

                    compositeLink.PrintingSystemBase.ExportToCsv(stream);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/csv");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.csv");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }


               
            }
            ps.Dispose();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
           // TreeList.DataSource = ((DataSet)Session["TradingPLdata"]).Tables[0];
           // TreeList1.DataSource = ((DataSet)Session["TradingPLdata"]).Tables[0];


           // //TreeList.DataBind();
           // //TreeList1.DataBind();
            
           // XtraReportPLBS report = new XtraReportPLBS();
           // PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
           // link.Component = ExpExporter;

           // PrintableComponentLink link1 = new PrintableComponentLink(new PrintingSystem());
           // link1.Component = IncomeExporter;


           // report.printableComponentContainer1 = new PrintableComponentContainer();
           // report.printableComponentContainer2 = new PrintableComponentContainer();

           // report.printableComponentContainer1.PrintableComponent = link;
           // report.printableComponentContainer1.PrintableComponent = link1;

           // //report.printableComponentContainer1.PrintableComponent = TreeList;
           // //report.printableComponentContainer2.PrintableComponent = TreeList1;


           // report.printableComponentContainer1.BeforePrint += PrintableComponentContainer1_BeforePrint;
           // report.printableComponentContainer2.BeforePrint += PrintableComponentContainer2_BeforePrint;
           // report.printableComponentContainer1.AfterPrint += PrintableComponentContainer1_AfterPrint;
           // report.printableComponentContainer2.AfterPrint += PrintableComponentContainer2_AfterPrint;


           //// report.DataSource = (DataSet)Session["TradingPLdata"];

           // // Invoke a Print Preview for the created report document.   
           // using (MemoryStream stream = new MemoryStream())
           // {
           //     report.CreateDocument();
           //     report.PrintingSystem.ExportToPdf(stream);

           //     Response.ClearContent();
           //     Response.ContentType = "application/pdf";
           //     byte[] buffer = stream.ToArray();
           //     Response.OutputStream.Write(buffer, 0, (int)stream.Length);
           //     Response.Flush();
           // }
        }

        private void PrintableComponentContainer1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //this.TreeList.Width = (int)report.printableComponentContainer1.WidthF;
        }
        private void PrintableComponentContainer2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //this.TreeList1.Width = (int)report.printableComponentContainer2.WidthF;
        }
        private void PrintableComponentContainer1_AfterPrint(object sender, EventArgs e)
        {
            
        }
        private void PrintableComponentContainer2_AfterPrint(object sender, EventArgs e)
        {
            
        }

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string parameter = e.Parameter;

            if (parameter == "BindGrid")
            {
                Session["TradingPLdata"] = null;
                TreeList.DataBind();
                TreeList.CollapseAll();
                if (chkPercentage.Checked)
                {
                    TreeList.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList.Columns["Percentage"].Visible = false;
                }
            }
            else if (parameter == "ExpandAll")
            {
                TreeList.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList.CollapseAll();
            }

            else if (parameter == "ShowPercentage")
            {
                TreeList.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList.Columns["Percentage"].Visible = false;
            }

            if (parameter == "BindGrid")
            {
                //Session["TradingPLdata"] = null;
                TreeList1.DataBind();
                if (chkPercentage.Checked)
                {
                    TreeList1.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList1.Columns["Percentage"].Visible = false;
                }

                TreeList1.CollapseAll();
            }
            else if (parameter == "ExpandAll")
            {
                TreeList1.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList1.CollapseAll();
            }
            else if (parameter == "ShowPercentage")
            {
                TreeList1.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList1.Columns["Percentage"].Visible = false;
            }
        }

        protected void xpprt_Click(object sender, EventArgs e)
        {

            XtraReport report = new XtraReport();
            DetailBand db = new DetailBand();
            report.Bands.Add(db);
            PrintableComponentContainer pcc1 = new PrintableComponentContainer();
            pcc1.SizeF = new SizeF(350, 20);
            PrintableComponentContainer pcc2 = new PrintableComponentContainer();
            pcc2.SizeF = new SizeF(350, 20);
            pcc2.LocationF = new PointF(350, 0);
            db.Controls.Add(pcc1);
            db.Controls.Add(pcc2);
            pcc1.PrintableComponent = TreeList;
            pcc2.PrintableComponent = TreeList1;

            report.CreateDocument(true);
          //  report.ShowPreviewDialog();  
            // Link the required control with the PrintableComponentContainers of a report.
            //report.printa

            //// Invoke a Print Preview for the created report document. 
            ReportPrintTool preview = new ReportPrintTool(report);
            preview.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            preview.ShowRibbonPreview();

        }

        protected void cclStockDetails_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSStockDetails"] = null;
            gridStockDetials.DataBind();
        }
        protected void cclStockSummary_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSSTockSummary"] = null;
            gvStockSummary.DataBind();
        }
        protected void cCallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSZooming"] = null;
            ShowGridDetails2Level.DataBind();
        }

        protected void cCallbackPanelDetail1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSZooming1"] = null;
            ShowGridDetails1Level.DataBind();
        }

        protected void grivaluation_DataBinding(object sender, EventArgs e)
        {
            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            if (hdnProductiD.Value != "")
            {
                if (hdn_LedgerCode.Value == "SYSTM00011")
                    if (!radAsDate.Checked)
                    gridStockDetials.DataSource = GetGridDataSourceStockDetails(Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("yyy-MM-dd"), ASPxFromDate.Date.AddDays(-1).ToString("yyyy-MM-dd"), hdnBranchId.Value, radAsDate.Checked, ddlValTech.SelectedValue, hdnProductiD.Value);
                    else
                        gridStockDetials.DataSource = GetGridDataSourceStockDetails(Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("yyy-MM-dd"), Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]).ToString("yyy-MM-dd"), hdnBranchId.Value, radAsDate.Checked, ddlValTech.SelectedValue, hdnProductiD.Value);
                else
                    gridStockDetials.DataSource = GetGridDataSourceStockDetails(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), hdnBranchId.Value, radAsDate.Checked, ddlValTech.SelectedValue, hdnProductiD.Value);
            }
        }

        private DataTable GetGridDataSourceStockDetails(string FromDate, string Todate, string Branch, bool AsonDate, string Valuation, string productid)
        {
            try
            {
                if (Session["BSStockDetails"] != null)
                {
                    return (DataTable)Session["BSStockDetails"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(productid))
                    {
                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZooming", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "StockDetail");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", Todate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", Branch);
                        cmd.Parameters.AddWithValue("@Ason", AsonDate);
                        cmd.Parameters.AddWithValue("@Valuation", Valuation);
                        cmd.Parameters.AddWithValue("@chkConsiderOverhead", chkConsiderOverhead.Checked);
                        cmd.Parameters.AddWithValue("@Product", Convert.ToString(productid));
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSStockDetails"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        private DataTable GetGridDataSourceStockSummary(string FromDate, string Todate, string Branch, bool AsonDate, string Valuation)
        {
            try
            {
                if (Session["BSSTockSummary"] != null)
                {
                    return (DataTable)Session["BSSTockSummary"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(Branch))
                    {


                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZooming", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "StockSummary");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", Todate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", Branch);
                        cmd.Parameters.AddWithValue("@Ason", AsonDate);
                        cmd.Parameters.AddWithValue("@Valuation", Valuation);
                        cmd.Parameters.AddWithValue("@chkConsiderOverhead", chkConsiderOverhead.Checked);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSSTockSummary"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        //FileStreamResult CreateExcelExportResult(CompositeLinkBase link)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    link.PrintingSystemBase.ExportToXls(stream);
        //    stream.Position = 0;
        //    FileStreamResult result = new FileStreamResult(stream, "application/xls");
        //    result.FileDownloadName = "Trading and PL Statement";
        //    return result;
        //}


        private DataTable GetGridDataSource(string FromDate, string ToDate, string ledger, string BRANCH_ID, bool Ason, string Entity_Id)
        {

            try
            {
                if (Session["BSZooming"] != null)
                {
                    return (DataTable)Session["BSZooming"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(ledger))
                    {
                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZooming", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "FirstLevel");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", ToDate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                        cmd.Parameters.AddWithValue("@LEDGERID", ledger.Replace("M", ""));
                        cmd.Parameters.AddWithValue("@Ason", Ason);
                        cmd.Parameters.AddWithValue("@Entity_Id", Entity_Id);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSZooming"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        {
            // Session["PLZooming"] = null;
            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            ShowGridDetails2Level.DataSource = GetGridDataSource(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), hdnLedger.Value, branch, radAsDate.Checked, hdnEntity_Id.Value);
        }


        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //Rev 1.0
            //e.Text = string.Format("{0}", e.Value);
            MainLedgerType = hdnLedgertype.Value.ToString();
            if (e.Item.FieldName == "OPENING")
            {
                TotalOpening = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "PR_DR_AMT")
            {
                TotalDebit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "PR_CR_AMT")
            {
                TotalCredit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "CLOSING")
            {
                if (MainLedgerType == "Asset" || MainLedgerType == "Expense")
                {
                    TotalDBCR = (TotalOpening + TotalDebit) - TotalCredit;
                }
                else
                {
                    TotalDBCR = TotalDebit - (TotalOpening + TotalCredit);
                }
                e.Text = TotalDBCR.ToString();
            }
            else
            {
                e.Text = string.Format("{0}", e.Value);
            }
            //End of Rev 1.0
        }

        //Rev 1.0
        protected void ShowGridDetails3Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void gvStockSummary_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void gridStockDetials_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        //End of Rev 1.0

        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(ddlExport3.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    bindexport_Details3(Filter);
            //    ddlExport3.SelectedItem.Value = "0";

            //}
        }
        public void bindexport_Details3(int Filter)
        {
            string filename = "Trading & PL Statement Detail";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Trading & PL Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trading & PL Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporterDetails.WriteRtfToResponse();
                    break;
                case 4:
                    exporterDetails.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        protected void ShowGridDetails1Level_DataBinding(object sender, EventArgs e)
        {
            // Session["BSZooming"] = null;
            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            ShowGridDetails1Level.DataSource = GetGridDataSourceFirstLevel(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), hdnLedger.Value, branch, radAsDate.Checked);
        }
        protected void gvStockSummary_DataBinding(object sender, EventArgs e)
        {

            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            if (hdn_LedgerCode.Value == "SYSTM00011")
                gvStockSummary.DataSource = GetGridDataSourceStockSummary(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxFromDate.Date.AddDays(-1).ToString("yyyy-MM-dd"), branch, radAsDate.Checked, ddlValTech.SelectedValue);
            else
                gvStockSummary.DataSource = GetGridDataSourceStockSummary(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), branch, radAsDate.Checked, ddlValTech.SelectedValue);
        }

        private DataTable GetGridDataSourceFirstLevel(string FromDate, string ToDate, string ledger, string BRANCH_ID, bool Ason)
        {

            try
            {
                if (Session["BSZooming1"] != null)
                {
                    return (DataTable)Session["BSZooming1"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(ledger))
                    {
                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZooming", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "PreFirstLevel");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", ToDate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                        cmd.Parameters.AddWithValue("@LEDGERID", ledger.Replace("M", ""));
                        cmd.Parameters.AddWithValue("@Ason", Ason);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSZooming1"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ddlExport4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(ddlExport4.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    bindexport_Details4(Filter);
            //    ddlExport4.SelectedItem.Value = "0";
            //}
        }

        public void bindexport_Details4(int Filter)
        {
            string filename = "Trading & PL Statement Detail";
            Exporter1.FileName = filename;
            Exporter1.FileName = "Trading & PL Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trading & PL Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            Exporter1.RenderBrick += exporter_RenderBrick;

            Exporter1.PageHeader.Left = FileHeader;
            Exporter1.PageHeader.Font.Size = 10;
            Exporter1.PageHeader.Font.Name = "Tahoma";
            Exporter1.PageFooter.Center = "[Page # of Pages #]";
            Exporter1.PageFooter.Right = "[Date Printed]";
            Exporter1.GridViewID = "ShowGridDetails1Level";
            switch (Filter)
            {
                case 1:
                    Exporter1.WritePdfToResponse();
                    break;
                case 2:
                    Exporter1.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    Exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    Exporter1.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        protected void ddlExport5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(ddlExport5.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    bindexport_Details5(Filter);
            //    ddlExport5.SelectedItem.Value = "0";

            //}
        }

        public void bindexport_Details5(int Filter)
        {
            string filename = "Trading & PL Statement Detail";
            SummaryExporter.FileName = filename;
            SummaryExporter.FileName = "Trading & PL Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trading & PL Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            SummaryExporter.RenderBrick += exporter_RenderBrick;

            SummaryExporter.PageHeader.Left = FileHeader;
            SummaryExporter.PageHeader.Font.Size = 10;
            SummaryExporter.PageHeader.Font.Name = "Tahoma";
            SummaryExporter.PageFooter.Center = "[Page # of Pages #]";
            SummaryExporter.PageFooter.Right = "[Date Printed]";
            SummaryExporter.GridViewID = "gvStockSummary";
            switch (Filter)
            {
                case 1:
                    SummaryExporter.WritePdfToResponse();
                    break;
                case 2:
                    SummaryExporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    SummaryExporter.WriteRtfToResponse();
                    break;
                case 4:
                    SummaryExporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        protected void ddlExport6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////Int32 Filter = int.Parse(Convert.ToString(ddlExport6.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    bindexport_Details6(Filter);
            //    //ddlExport6.SelectedItem.Value = "0";

            //}
        }

        public void bindexport_Details6(int Filter)
        {
            string filename = "Trading & PL Statement Detail";
            DetailsExporter.FileName = filename;
            DetailsExporter.FileName = "Trading & PL Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trading & PL Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            DetailsExporter.RenderBrick += exporter_RenderBrick;

            DetailsExporter.PageHeader.Left = FileHeader;
            DetailsExporter.PageHeader.Font.Size = 10;
            DetailsExporter.PageHeader.Font.Name = "Tahoma";
            DetailsExporter.PageFooter.Center = "[Page # of Pages #]";
            DetailsExporter.PageFooter.Right = "[Date Printed]";
            DetailsExporter.GridViewID = "gridStockDetials";
            switch (Filter)
            {
                case 1:
                    DetailsExporter.WritePdfToResponse();
                    break;
                case 2:
                    DetailsExporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    DetailsExporter.WriteRtfToResponse();
                    break;
                case 4:
                    DetailsExporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }


        protected void FirstImport_Click(object sender, EventArgs e)
        {
            bindexport_Details4(2);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bindexport_Details5(2);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            bindexport_Details6(2);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            bindexport_Details3(2);
        }


        protected void Button6_Click(object sender, EventArgs e)
        {
            TreeList.ExpandAll();
            TreeList1.ExpandAll();


            
            
            PrintPLBS reports = new PrintPLBS();


            ExpExporter.Settings.ShowTreeLines = false;
            IncomeExporter.Settings.ShowTreeLines = false;

            //IncomeExporter.Settings.AutoWidth = true;
            //ExpExporter.Settings.AutoWidth = true;


            //ExpExporter.SkinID = TreeList.SkinID;
            //IncomeExporter.SkinID = TreeList1.SkinID;

            //ExpExporter.EnableTheming = true;
            //IncomeExporter.EnableTheming = true;



            reports.printableComponentContainer1.PrintableComponent = ExpExporter;
            reports.printableComponentContainer2.PrintableComponent = IncomeExporter ;
           


            //reports.CreateDocument();


            DataTable dtCompany = oDBEngine.GetDataTable("select cmp_Name,add_address1 +','+add_address2+','+add_address3 Address1,city.city_name + ' - ' + pinzip.pin_code Address2 from tbl_master_company com inner join  tbl_master_address addr on com.cmp_internalid=addr.add_cntId inner join tbl_master_city city on city.city_id=addr.add_city inner join tbl_master_pinzip pinzip on pinzip.pin_id=addr.add_pin where add_cntid='" + Convert.ToString(Session["LastCompany"]) + "' and add_addressType='Billing'");

            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                reports.txtCompanyName.Text = Convert.ToString(dtCompany.Rows[0]["cmp_Name"]);
                reports.AddressLine1.Text = Convert.ToString(dtCompany.Rows[0]["Address1"]);
                reports.AddressLine2.Text = Convert.ToString(dtCompany.Rows[0]["Address2"]);

            }

            reports.TypeLeft.Text = "D E B I T";
            reports.TypeRight.Text = "C R E D I T";

            if (radAsDate.Checked)
            {
                reports.AsOn.Text = "Trading and Profit & Loss  As On " + ASPxToDate.Date.ToString("dd-MM-yyyy");
            }
            else
            {
                reports.AsOn.Text = "Trading and Profit & Loss Period : " + ASPxFromDate.Date.ToString("dd-MM-yyyy") + " - " + ASPxToDate.Date.ToString("dd-MM-yyyy");
            }


            Session["report"] = reports;
            

            Response.ClearContent();
            //Response.Buffer = true;
            Response.Clear();
            Response.Redirect("PLBSViewer.aspx?Key=TradingandPLStatement");

            //Response.End();
        }

    }
}