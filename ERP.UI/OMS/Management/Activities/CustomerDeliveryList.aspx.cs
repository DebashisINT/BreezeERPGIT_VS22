using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerDeliveryList : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CustomerDeliveryBL objCustomerDeliveryBL = new CustomerDeliveryBL();

       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        SlaesActivitiesBL objSalesBL = new SlaesActivitiesBL();



        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/CustomerDeliveryList.aspx");

            gridCustDelivList.JSProperties["cpDelete"] = null;
            gridCustDelivList.JSProperties["cpExists"] = null;

            if (HttpContext.Current.Session["userid"] == null)
            {

            }
            if (!IsPostBack)
            {

                String finyear = "";
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                ASPxDateEditFrom.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                ASPxDateEditFrom.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                ASPxDateEditTo.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                ASPxDateEditTo.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);


                Session["exportval"] = null;
                string RequestType = Convert.ToString(Session["requesttype"]);
                ASPxDateEditFrom.Date = DateTime.Now;
                ASPxDateEditTo.Date = DateTime.Now;
                Session["DeliveryRouteList"] = null;
                GetBranch();
            }

            //BindGrid();


        }

        protected void BindGrid()
        {
            string branchID = Convert.ToString(ddlBranch.Value) == null ? "0" : Convert.ToString(ddlBranch.Value);
            string fromDate = "";
            string toDate = "";

            branchID = oDBEngine.getBranch(branchID, "") + branchID;

            DataSet dst = objCustomerDeliveryBL.GetCustomerDliveryDetailsListBackup("0", branchID, fromDate, toDate);

            gridCustDelivList.DataSource = dst.Tables[0];
            gridCustDelivList.DataBind();

            Session.Add("sessCustDelList", dst.Tables[0]);

        }

        protected void gridCustDelivList_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < gridCustDelivList.Columns.Count; i++)
                    if (gridCustDelivList.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 2;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd" || Convert.ToString(Session["PageAccess"]).Trim() == "Delete" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                        {
                            hyperlink.Enabled = true;
                            continue;
                        }
                        else
                        {
                            hyperlink.Enabled = false;
                            continue;
                        }
                    }
                }
            }
        }

        protected void gridCustDelivList_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!gridCustDelivList.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = gridCustDelivList.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }

        protected void gridCustDelivList_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string delivID = null;
            string custID = null;
            string deletecnt = "0"; //0:Delete Failed, 1:Delete Successful
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    delivID = Convert.ToString(e.Parameters).Split('~')[1];
                }

            }

            gridCustDelivList.JSProperties["cpDelete"] = null;
            if (Command == "Delete")
            {
                deletecnt = objCustomerDeliveryBL.DeleteCustomerDelivery(delivID, custID);

                if (deletecnt == "1")
                {
                    gridCustDelivList.JSProperties["cpDelete"] = "Success";
                    BindGrid();
                }
                else
                {
                    gridCustDelivList.JSProperties["cpDelete"] = "Fail";
                }
            }
            if (Command == "LoadGridOnBranchFilter")
            {
                string branchID = Convert.ToString(e.Parameters).Split('~')[1] == "null" ? "0" : Convert.ToString(e.Parameters).Split('~')[1];
                branchID = oDBEngine.getBranch(branchID, "") + branchID;
                string fromDate = Convert.ToString(ASPxDateEditFrom.Date.ToString("yyyy-MM-dd"));
                if (fromDate == "0001-01-01")
                    fromDate = "";
                string toDate = Convert.ToString(ASPxDateEditTo.Date.ToString("yyyy-MM-dd"));
                if (toDate == "0001-01-01")
                    toDate = "";

                DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(ASPxDateEditFrom.Value).ToString("yyyy-MM-dd"));
                DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(ASPxDateEditTo.Value).ToString("yyyy-MM-dd"));

                //BindGridFilteredView(branchID, fromDate, toDate);

                BindGridFilteredView(branchID, FromDate, ToDate);
            }
            if (Command == "FilterGridByDate")
            {
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string branchID = Convert.ToString(ddlBranch.Value);
                branchID = oDBEngine.getBranch(branchID, "") + branchID;

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);

                DateTime FromDate1 = Convert.ToDateTime(Convert.ToDateTime(ASPxDateEditFrom.Value).ToString("yyyy-MM-dd"));
                DateTime ToDate1 = Convert.ToDateTime(Convert.ToDateTime(ASPxDateEditTo.Value).ToString("yyyy-MM-dd"));

                BindGridFilteredView(branchID, FromDate1, ToDate1);
            }

        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SlNo";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]); 

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_Entity_CustomerDeliveryRouteLists
                            where d.Delievry_Date >= Convert.ToDateTime(strFromDate) && d.Delievry_Date <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.Delievry_BranchId))
                            orderby d.Delievry_Date descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_Entity_CustomerDeliveryRouteLists
                            where
                            d.Delievry_Date >= Convert.ToDateTime(strFromDate) && d.Delievry_Date <= Convert.ToDateTime(strToDate) &&
                            branchidlist.Contains(Convert.ToInt32(d.Delievry_BranchId))
                            orderby d.Delievry_Date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_Entity_CustomerDeliveryRouteLists
                        where d.Delievry_BranchId == '0'
                        orderby d.Delievry_Date descending
                        select d;
                e.QueryableSource = q;
            }

        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport.SelectedItem.Value));
            if (Filter != 0)
            {
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
            string filename = "Customer Delivery";
            exporter.FileName = filename;

            exporter.PageHeader.Left = filename;
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

        public void bindexport1(int Filter)
        {
            exporter.GridViewID = "gridCustDelivList";
            exporter.FileName = "Customer Delivery";

            exporter.PageHeader.Left = "Customer Delivery List";
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

        //Filteration part is done by Debojoyti in POS -- 
        public void GetBranch()
        {
            DataSet dst = new DataSet();
            string strBranchID = Convert.ToString(Session["userbranchID"]);

            dst = objSalesBL.GetAllDropDownDetailForSalesChallan(strBranchID); //This is existing functionality incorporated here. 

            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                DataView dv = dst.Tables[1].DefaultView;
                dv.Sort = "branch_description";
                DataTable sortedDT = dv.ToTable();

                ddlBranch.TextField = "branch_description";
                ddlBranch.ValueField = "branch_id";
                ddlBranch.DataSource = sortedDT;
                ddlBranch.DataBind();
            }

            dst.Clear();

            if (Session["userbranchID"] != null)
            {
                if (ddlBranch.Items.Count > 0)
                {
                    ddlBranch.Value = Convert.ToString(Session["userbranchID"]);
                }
            }
        }

        //This is already done by debojoty
        private void BindGridFilteredView(string brnchID, DateTime fromDT, DateTime toDT)
        {


            DataSet dst = objCustomerDeliveryBL.GetCustomerDliveryDetailsList("0", brnchID, fromDT, toDT);

            gridCustDelivList.DataSource = dst.Tables[0];
            Session["DeliveryRouteList"] = dst.Tables[0];
            gridCustDelivList.DataBind();

            Session.Add("sessCustDelList", dst.Tables[0]);

            
        }

        private void BindGridFilteredViewBackUp(string brnchID, string fromDT, string toDT)
        {


            //DataSet dst = objCustomerDeliveryBL.GetCustomerDliveryDetailsList("0", brnchID, fromDT, toDT);

            //gridCustDelivList.DataSource = dst.Tables[0];
            //gridCustDelivList.DataBind();

            //Session.Add("sessCustDelList", dst.Tables[0]);
        }



        //protected void gridCustDelivList_DataBinding(object sender, EventArgs e)
        //{
        //    //string branchID = Convert.ToString(e.Parameters).Split('~')[1] == "null" ? "0" : Convert.ToString(e.Parameters).Split('~')[1];
        //    string branchID = Convert.ToString(ddlBranch.Value);
        //    //branchID = oDBEngine.getBranch(branchID, "") + branchID;
        //    //string branchID = Convert.ToString(ddlBranch.Value);
        //    string fromDate = Convert.ToString(ASPxDateEditFrom.Date.ToString("yyyy-MM-dd"));
        //    if (fromDate == "0001-01-01")
        //        fromDate = "";
        //    string toDate = Convert.ToString(ASPxDateEditTo.Date.ToString("yyyy-MM-dd"));
        //    if (toDate == "0001-01-01")
        //        toDate = "";

        //    DateTime FromDate = Convert.ToDateTime(Convert.ToDateTime(ASPxDateEditFrom.Value).ToString("yyyy-MM-dd"));
        //    DateTime ToDate = Convert.ToDateTime(Convert.ToDateTime(ASPxDateEditTo.Value).ToString("yyyy-MM-dd"));

        //    //BindGridFilteredView(branchID, fromDate, toDate);

        //    //BindGridFilteredView(branchID, FromDate, ToDate);

        //    //DataSet dst = objCustomerDeliveryBL.GetCustomerDliveryDetailsList("0", branchID, FromDate, ToDate);
        //    if (Session["DeliveryRouteList"] != null)
        //    {
        //        gridCustDelivList.DataSource = (DataTable)Session["DeliveryRouteList"];
        //    }


        //}


        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            int ispermission = 0;
            ispermission = objCRMSalesOrderDtlBL.SalesOrderEditablePermission(Convert.ToInt32(ActiveUser));

            //}
            return Convert.ToString(ispermission);

        }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
               // BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtRptModules = new DataTable();
                //string query = "";
                //query = @"Select Design_Fullpath from tbl_trans_SetDefaultDesign_Report WHERE Module_Type='SI' order by Module_Id ";
                //dtRptModules = oDbEngine.GetDataTable(query);
                //if (dtRptModules.Rows.Count > 1)
                //{
                //    string Savereportname = Path.GetFileNameWithoutExtension(Convert.ToString(dtRptModules.Rows[0][0]));
                //    string Rname = "";
                //    if (Savereportname.Split('~').Length > 1)
                //    {
                //        Rname = Savereportname.Split('~')[0];
                //    }
                //    else
                //    {
                //        Rname = Savereportname;
                //    }
                //    string SavereportValue = Savereportname;
                //    CmbDesignName.Items.Add(Rname, SavereportValue);
                //}                

                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\CDelivery";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\CDelivery";
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
                    //if (reportValue != SavereportValue)
                    //{
                    CmbDesignName.Items.Add(name, reportValue);
                    //}
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
                    NoofCopy += 3 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
                //SelectPanel.JSProperties["cpSuccessPath"] = DesignFullPath;
                //HttpContext.Current.Response.Redirect(DesignFullPath + "?Previewrpt=" + reportName);
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("Total ={0}", e.Value);
        }

    }
}