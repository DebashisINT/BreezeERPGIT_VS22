using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogicLayer;
using System.Configuration;
using System.IO;
using DevExpress.Web;

namespace Reports.Reports.REPXReports
{
    public partial class RepxReportMain : System.Web.UI.Page
    {
        public DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] filePaths = new string[] { };
                TxtStartDate.EditFormatString = objConverter.GetDateFormat("Date");
                TxtEndDate.EditFormatString = objConverter.GetDateFormat("Date");
                string fDate = null;
                string tDate = null;
                fDate = Session["FinYearStart"].ToString();
                tDate = Session["FinYearEnd"].ToString();
                TxtStartDate.Value = Convert.ToDateTime(fDate); //oDBEngine.GetDate();
                TxtEndDate.Value = Convert.ToDateTime(tDate); //oDBEngine.GetDate();
                btnNewDesign.Visible = false;
                btnLoadDesign.Visible = false;
                //String RptModuleName = Convert.ToString(Request.QueryString["reportname"]);
                String RptModuleName = HttpContext.Current.Request.QueryString["reportname"];
                Session["NewRptModuleName"] = RptModuleName;

                string RptModuleType = "";
                string Module_name = "";
                switch (RptModuleName)
                {
                    case "StockTrial":
                        RptModuleType = "STOCKTRIAL";
                        Module_name = "STOCKTRIAL";
                        btnDocNo.Visible = false;
                        break;
                    case "Invoice":
                         RptModuleType = "Invoice";
                         Module_name = "SALETAX";
                         btnDocNo.Visible = false;
                         btnBranch.Visible = false;
                         break;
                    case "Invoice_POS":
                         RptModuleType = "Invoice_POS";
                         Module_name = "SALETAX";
                         btnDocNo.Visible = false;
                         btnBranch.Visible = false;
                         break;
                    case "Install_Coupon":
                         RptModuleType = "Install_Coupon";
                         Module_name = "INSCUPN";
                         btnDocNo.Visible = false;
                         btnBranch.Visible = false;
                         break;
                    case "Proforma":
                         RptModuleType = "Proforma";
                         btnBranch.Visible = false;
                         break;
                    case "LedgerPost":
                         RptModuleType = "LedgerPost";
                         Module_name = "LEDGERPOST";
                         btnDocNo.Visible = false;
                         break;
                    case "Porder":
                         RptModuleType = "Porder";
                         Module_name = "PORDER";
                         btnDocNo.Visible = false;
                         btnBranch.Visible = false;
                         break;
                    case "Sorder":
                         RptModuleType = "Sorder";
                         Module_name = "SORDER";
                         btnDocNo.Visible = false;
                         btnBranch.Visible = false;
                         break;
                    case "BranchReq":
                         RptModuleType = "BranchReq";
                         Module_name = "BRANCHREQ";
                         btnDocNo.Visible = false;
                         btnBranch.Visible = false;
                         break;
                }

                Session["Module_Name"] = Module_name;

                if (RptModuleType == "STOCKTRIAL")
                {
                    //string[] filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign"));
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial"),"*.repx");
                }
                else if (RptModuleType == "Invoice")
                {
                    //string[] filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign"));
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal"), "*.repx");
                }
                else if (RptModuleType == "Invoice_POS")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS"), "*.repx");
                }
                else if (RptModuleType == "Install_Coupon")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon"), "*.repx");
                }
                else if (RptModuleType == "Proforma")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/Proforma"), "*.repx");
                }
                else if (RptModuleType == "LedgerPost")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/Ledger"), "*.repx");
                }
                else if (RptModuleType == "Porder")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes"), "*.repx");
                }
                else if (RptModuleType == "Sorder")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes"), "*.repx");
                }
                else if (RptModuleType == "BranchReq")
                {
                    filePaths = System.IO.Directory.GetFiles(Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes"), "*.repx");
                }
                //ddReportName.Items.Add("--Select--");
                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    ddReportName.Items.Add(name, reportValue);
                }
                ddReportName.SelectedIndex = 0;

                //bindGrid();
            }
        }

        //private void bindGrid()
        //{
        //    DataTable dtdata = GetDocumentListGridData();
        //    grid_Documents.DataSource = dtdata;
        //    grid_Documents.DataBind();
        //}

        protected void btnNewDesign_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect("~/Reports/REPXReports/ReportDesignerRepx.aspx");
        }
        protected void btnLoadDesign_Click(object sender, EventArgs e)
        {
            string reportName = Convert.ToString(ddReportName.Value);
            RptName.Value = Convert.ToString(ddReportName.Value);
            string ReportModuleNM = Convert.ToString(Request.QueryString["reportname"]);
            string Module_Name = Convert.ToString(Request.QueryString["Module_Name"]);
            if (reportName == "--Select--")
            {
                return;
            }
            HttpContext.Current.Response.Redirect("~/Reports/REPXReports/ReportDesignerRepx.aspx?LoadrptName=" + reportName + "&&reportname=" + Convert.ToString(Request.QueryString["reportname"]));
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            string reportName = Convert.ToString(ddReportName.Value);
            string ReportModuleNM = Convert.ToString(Request.QueryString["reportname"]);
            if (reportName == "--Select--")
            {
                return;
            }
            string SelectedDocList = "";

            if (ReportModuleNM == "StockTrial" || ReportModuleNM == "LedgerPost")
            {
                List<object> docList = grid_Branch.GetSelectedFieldValues("ID");
                foreach (object Dobj in docList)
                {
                    SelectedDocList += "," + Dobj;
                }
            }
            else if (ReportModuleNM=="Proforma")
            {
                List<object> docList = grid_Documents.GetSelectedFieldValues("ID");
                foreach (object Dobj in docList)
                {
                    SelectedDocList += "," + Dobj;
                }
            }            
            
            SelectedDocList = SelectedDocList.TrimStart(',');
            if (SelectedDocList.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Please Select Some Document(s)')", true);
            }
            else
            {
                Session["SelectedDocumentList"] = SelectedDocList;
            }
            StartDate = TxtStartDate.Text;
            EndDate = TxtEndDate.Text;
            HttpContext.Current.Response.Redirect("~/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&&StartDate=" + StartDate + "&&EndDate=" + EndDate + "&&reportname=" + Convert.ToString(Request.QueryString["reportname"]));
        }


        protected void btnNewFileSave_Click(object sender, EventArgs e)
        {
            string reportName = txtFileName.Text;
            HttpContext.Current.Response.Redirect("~/Reports/REPXReports/ReportDesignerRepx.aspx?NewReport=" + reportName);
        }        

        protected void cgridDocuments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindDocumentsDetails")
            {
                //DataTable dtdata = GetGridData();
                DataTable dtdata = GetDocumentListGridData();
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                    //grid_Documents.DataSource = dtdata;
                    Session["ReportMain_DocumentList"] = dtdata;
                    grid_Documents.DataBind();
                }
                else
                {
                    grid_Documents.DataSource = null;
                    grid_Documents.DataBind();
                }
            }


            else if (strSplitCommand == "SelectAndDeSelectDocuments")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }

            else
            {
                string SelectedDocList = "";

                List<object> docList = grid_Documents.GetSelectedFieldValues("ID");
                foreach (object Dobj in docList)
                {
                    SelectedDocList += "," + Dobj;
                }
                SelectedDocList = SelectedDocList.TrimStart(',');
                if (SelectedDocList.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), SelectedDocList, "jAlert('Please Select Some Document(s)')", true);
                }
                else
                {
                    Session["SelectedDocumentList"] = SelectedDocList;
                }
            }
        }

        protected void cgridBranch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string userbranch = "";
            if (Session["userbranchHierarchy"] != null)
            {
                userbranch = Convert.ToString(Session["userbranchHierarchy"]);
            }
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindDocumentsDetails")
            {
                DataTable dtdata = GetDocumentListGridData();
                if (dtdata != null && dtdata.Rows.Count > 0)
                {
                 //   grid_Branch.DataSource = dtdata;
                    Session["ReportMain_BranchList"] = dtdata;
                    grid_Branch.DataBind();
                }
                else
                {
                    grid_Branch.DataSource = null;
                    grid_Branch.DataBind();
                }
            }
            else if (strSplitCommand == "SelectAndDeSelectDocuments")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }

            else
            {
                string SelectedDocList = "";

                List<object> docList = grid_Branch.GetSelectedFieldValues("ID");
                foreach (object Dobj in docList)
                {
                    SelectedDocList += "," + Dobj;
                }
                SelectedDocList = SelectedDocList.TrimStart(',');
                if (SelectedDocList.Trim() == "")
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Abc", "Alert('Please Select Some Document(s)')", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Abc", "Alert('Please Select Some Document(s)')", true);
                }
                else
                {
                    Session["SelectedDocumentList"] = SelectedDocList;
                }
            }
        }

        public DataTable GetDocumentListGridData()
        {
            string query = "";
            string ReportModule = string.Empty;
            //string ReturntoMainPg = Convert.ToString(Session["NewRptModuleName"]);
            ReportModule=Convert.ToString(Request.QueryString["reportname"]);
            //ReturnPage.Value = Convert.ToString(Request.QueryString["reportname"]);
            if (ReportModule == "StockTrial" || ReportModule == "LedgerPost")
            {
                query = @"Select ROW_NUMBER() over(order by branch_id) SrlNo, branch_id AS ID, branch_code as Doc_Code, branch_description as Description from tbl_master_branch WHERE branch_id IN(" + Convert.ToString(Session["userbranchHierarchy"]) + ") ORDER BY branch_description";
            }
            else if (ReportModule == "Proforma")
            {
                query = @"Select ROW_NUMBER() over(order by Quote_Id) SrlNo, Quote_Id AS ID, Quote_Number, CONVERT(VARCHAR(11),Quote_Expiry, 105) as Quote_Date from tbl_trans_Quotation order by Quote_Number ";
            }
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable dt = oDbEngine.GetDataTable(query);
            return dt;
        }

        protected void grid_Branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["ReportMain_BranchList"] != null)
            {
                grid_Branch.DataSource = Session["ReportMain_BranchList"];
            }
        }

        protected void grid_Documents_DataBinding(object sender, EventArgs e)
        {
            if (Session["ReportMain_DocunmentList"] != null)
            {
                grid_Branch.DataSource = Session["ReportMain_DocunmentList"];
            }
        }
    }
}