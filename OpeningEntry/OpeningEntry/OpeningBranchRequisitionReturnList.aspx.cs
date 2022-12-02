using BusinessLogicLayer;
using EntityLayer.CommonELS;
using OpeningEntry.OpeningEntry.DBML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.OpeningEntry
{
    public partial class OpeningBranchRequisitionReturnList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        PurchaseReturnRequestBL objPurchaseOrderBL = new PurchaseReturnRequestBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "BR_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI


            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string companyid = Convert.ToString(Session["LastCompany"]);
            //    BR_Company='''+@CampanyID+''' 
            //and (BR_BranchIdFor in ('+@userbranchHierarchy+')
            //or BR_BranchIdTo in ('+@userbranchHierarchy+'))
            //and BR_FinYear='''+@FinYear+'''
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                    var q = from d in dc.v_BRRequestLists
                            where d.BR_RequisitionDate >= Convert.ToDateTime(strFromDate) && d.BR_RequisitionDate <= Convert.ToDateTime(strToDate)
                            && (branchidlist.Contains(Convert.ToInt32(d.BranchIdTo)) || branchidlist.Contains(Convert.ToInt32(d.BranchIdFor)))
                            && d.BR_Company == companyid
                            orderby d.BR_Id descending
                            select d;

                    e.QueryableSource = q;
                    var cnt = q.Count();
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                    var q = from d in dc.v_BRRequestLists
                            where d.BR_RequisitionDate >= Convert.ToDateTime(strFromDate) && d.BR_RequisitionDate <= Convert.ToDateTime(strToDate)
                            && (branchidlist.Contains(Convert.ToInt32(d.BranchIdTo)) || branchidlist.Contains(Convert.ToInt32(d.BranchIdFor)))
                            && d.BR_Company == companyid
                            orderby d.BR_Id descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                var q = from d in dc.v_BRRequestLists
                        where (d.BranchIdTo == 0 || d.BranchIdFor == 0)
                        orderby d.BR_Id descending
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                //................Cookies..................
                GridPurchaseReturnREquest.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_GridPurchaseReturnREquestPageGridPurchaseReturnREquest";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_GridPurchaseReturnREquestPageGridPurchaseReturnREquest');</script>");
                //...........Cookies End............... 
                Session["SaveModePO"] = null;
                Session["exportval"] = null;
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);

                //FormDate.Date = DateTime.Now;
                //toDate.Date = DateTime.Now;
                DateTime fromDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]);
                fromDate = fromDate.AddDays(-1);


                toDate.Date = fromDate;
                FormDate.Date = fromDate;
                toDate.MaxDate = fromDate;
                FormDate.MaxDate = fromDate;

            }

            // FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/BranchRequisitionReturnList.aspx");
        }


        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        #region Export Grid Section Start
        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
        //    if (Filter != 0)
        //    {
        //        if (Session["exportval"] == null)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //        else if (Convert.ToInt32(Session["exportval"]) != Filter)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //    }
        //}
        public void bindexport(int Filter)
        {
            //GridPurchaseReturnREquest.Columns[7].Visible = false;
            //string filename = "Purchase Order";
            //exporter.FileName = filename;
            exporter.GridViewID = "GridPurchaseReturnREquest";
            exporter.FileName = "PurchaseReturnREquest";

            exporter.PageHeader.Left = "Purchase Return REquest";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        #endregion Export Grid Section End
        public void FillGrid(string StartDate, string EndDate, string userbranchID)
        {

            //DataTable dtdata = GetPurchaseOrderListGridData( StartDate,  EndDate,  userbranchID);


            //Session["BranchRequisitionReturnListDataList"] = dtdata;
            //    GridPurchaseReturnREquest.DataSource = dtdata;
            //    GridPurchaseReturnREquest.DataBind();

        }
        //public DataTable GetPurchaseOrderListGridData(string StartDate, string EndDate, string userbranchID)
        //{

        //DataTable dt = new DataTable();
        //ProcedureExecute proc = new ProcedureExecute("prc_PurchaseReturnRequestDetailsList");
        //proc.AddVarcharPara("@Action", 500, "PurchaseReturnRequestDetail");
        //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
        //proc.AddVarcharPara("@CampanyID", 500, Convert.ToString(Session["LastCompany"]));
        //proc.AddNTextPara("@userbranchHierarchy", Convert.ToString(Session["userbranchHierarchy"]));
        //proc.AddVarcharPara("@FromDate",10,StartDate);
        //proc.AddVarcharPara("@ToDate",10,EndDate);
        //proc.AddVarcharPara("@Branch", 500, userbranchID);          
        //dt = proc.GetTable();
        //return dt;
        //}
        protected void GridPurchaseReturnREquest_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string PRRID = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    PRRID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (Command == "Delete")
            {
                deletecnt = objPurchaseOrderBL.DeletePurchaseReturnRequest(PRRID);

                if (deletecnt == 1)
                {
                    GridPurchaseReturnREquest.JSProperties["cpDelete"] = "Deleted successfully";

                }

            }

            //else if (Command == "FilterGridByDate")
            //{

            //    string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
            //    string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
            //    string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);



            //    DataTable dtdata = new DataTable();

            //    FillGrid( FromDate, ToDate,BranchID);


            //}
        }

        protected void SelectPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseRetRec\DocDesign\Normal";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseRetRec\DocDesign\Normal";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

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
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
                SelectPanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                string NoofCopy = "";
                if (selectOriginal.Checked == true)
                {
                    NoofCopy += 1 + ",";
                }
                if (selectDuplicate.Checked == true)
                {
                    NoofCopy += 2 + ",";
                }
                if (selectTriplicate.Checked == true)
                {
                    NoofCopy += 4 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
                //SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        protected void GridPurchaseReturnREquest_DataBinding(object sender, EventArgs e)
        {
            //DataTable dtdata=  (DataTable) Session["BranchRequisitionReturnListDataList"];
            //if (dtdata !=null)
            //GridPurchaseReturnREquest.DataSource = dtdata;
        }


    }
}
