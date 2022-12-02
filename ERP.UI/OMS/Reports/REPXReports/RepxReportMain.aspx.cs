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
using System.Collections.Generic;

namespace ERP.OMS.Reports.REPXReports
{
    public partial class RepxReportMain : System.Web.UI.Page
    {
        public DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
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
               
               // string[] filePaths = System.IO.Directory.GetFiles(@"D:\VSS ERP\ERP.UI\OMS\Reports\RepxReportDesign");
                TxtStartDate.EditFormatString = objConverter.GetDateFormat("Date");
                TxtEndDate.EditFormatString = objConverter.GetDateFormat("Date");
                TxtStartDate.Value = oDBEngine.GetDate();
                TxtEndDate.Value = oDBEngine.GetDate();

                string[] filePaths = System.IO.Directory.GetFiles(Server.MapPath("/oms/Reports/RepxReportDesign"));
                ddReportName.Items.Add("--Select--");                
                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    ddReportName.Items.Add(name, reportValue);                    
                }
                ddReportName.SelectedIndex = 0;

                bindGrid();
            }
            //FillGrid();
        }

        private void bindGrid()
        {
            DataTable dtdata = GetDocumentListGridData();
            grid_Documents.DataSource = dtdata;
            grid_Documents.DataBind();
        }

        protected void btnNewDesign_Click(object sender, EventArgs e)
        {             
            HttpContext.Current.Response.Redirect("~/OMS/Reports/REPXReports/ReportDesignerRepx.aspx");
        }
        protected void btnLoadDesign_Click(object sender, EventArgs e)
        {
            string reportName = Convert.ToString(ddReportName.Value);
            RptName.Value = Convert.ToString(ddReportName.Value);
            if (reportName == "--Select--")
            {
                return;
            }
            HttpContext.Current.Response.Redirect("~/OMS/Reports/REPXReports/ReportDesignerRepx.aspx?LoadrptName=" + reportName);
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string reportName = Convert.ToString(ddReportName.Value);
            if (reportName=="--Select--")
            {
                return;
            }
            string SelectedDocList = "";

            List<object> docList =  grid_Documents.GetSelectedFieldValues("Quote_Id");
            foreach (object Dobj in docList)
            {
                SelectedDocList += "," + Dobj;
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
            HttpContext.Current.Response.Redirect("~/OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName);
        }


        protected void btnNewFileSave_Click(object sender, EventArgs e)
        {
            string reportName = txtFileName.Text;
            HttpContext.Current.Response.Redirect("~/OMS/Reports/REPXReports/ReportDesignerRepx.aspx?NewReport=" + reportName);
        }

        protected void BtnDocNo_Click(object sender, EventArgs e)
        {

        }

        protected void cgridDocuments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindDocumentsDetails")
            {
                    //DataTable dtdata = GetGridData();
                    //DataTable dtdata = GetDocumentListGridData();
                    //if (dtdata != null && dtdata.Rows.Count > 0)
                    //{
                    //    grid_Documents.DataSource = dtdata;
                    //    grid_Documents.DataBind();
                    //}
                    //else
                    //{
                    //    grid_Documents.DataSource = null;
                    //    grid_Documents.DataBind();
                    //}
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

                List<object> docList =  grid_Documents.GetSelectedFieldValues("Quote_Id");
                foreach (object Dobj in docList)
                {
                    SelectedDocList += "," + Dobj;
                }
                SelectedDocList = SelectedDocList.TrimStart(',');
                if (SelectedDocList.Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), UniqueID, "jAlert('Please Select Some Document(s)')", true);
                }
                else
                {
                    Session["SelectedDocumentList"] = SelectedDocList;
                    //Response.Redirect("~/OMS/Reports/XtraReports/ProductReportViewer.aspx");
                }
            }
        }

        //public void FillGrid()
        //{
        //    //DataTable dtdata = GetGridData();
        //    DataTable dtdata = GetDocumentListGridData();
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        grid_Documents.DataSource = dtdata;
        //        grid_Documents.DataBind();
        //    }
        //    else
        //    {
        //        grid_Documents.DataSource = null;
        //        grid_Documents.DataBind();
        //    }
        //}

        public DataTable GetDocumentListGridData()
        {

            //DataTable dt = new DataTable();
            string query = @"Select ROW_NUMBER() over(order by Quote_Id) SrlNo, Quote_Id, CONVERT(VARCHAR(11),Quote_Date, 105) as Quote_Date from tbl_trans_Quotation";
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable dt = oDbEngine.GetDataTable(query);
            return dt;
        }
    }
}