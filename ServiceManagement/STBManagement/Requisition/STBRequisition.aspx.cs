using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ServiceManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.Requisition
{
    public partial class STBRequisition : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Requisition/STBRequisition.aspx");
            DataTable dt = new DataTable();
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranch = EmployeeBranchMap();
                }
                else
                {
                    userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            if (!IsPostBack)
            {
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
            }
        }

        public String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        [WebMethod]
        public static string DeleteSTBRequisition(int STBRequisitionID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionInsertUpdate");
                    proc.AddIntegerPara("@STBRequisitionID", STBRequisitionID);
                    proc.AddVarcharPara("@Action", 500, "delete");
                    proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
                    NoOfRowEffected = proc.RunActionQuery();

                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;

            e.KeyExpression = "STBRequisition_id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ServicveManagementDataClassesDataContext dc = new ServicveManagementDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                string BranchList = userbranch;// Convert.ToString(Session["userbranchHierarchy"]);

                if (strBranchID == "0")
                {
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.V_STB_STBRequisitionLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.STBRequisition_id descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_STB_STBRequisitionLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.STBRequisition_id descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_STB_STBRequisitionLists
                        where d.Branch == -1
                        orderby d.DocumentDate descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Status = "true";
            ProcedureExecute proc = new ProcedureExecute("PRC_STBModuleRosterStatus");
            proc.AddPara("@ModuleId", "3");
            DataTable dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                Status = "true";
            }
            else
            {
                Status = "false";
            }

            if (Status == "true")
            {
                exporter.GridViewID = "GrdSTBRequisition";
                string filename = "STBRequisition";
                exporter.FileName = filename;

                exporter.PageHeader.Left = "STB Requisition";
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
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

            //cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }

        //protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string strSplitCommand = e.Parameter.Split('~')[0];
        //    if (strSplitCommand == "Bindalldesignes")
        //    {
        //        string[] filePaths = new string[] { };
        //        string DesignPath = "";
        //        if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
        //        {
        //            DesignPath = @"Reports\Reports\RepxReportDesign\STBRequisition\DocDesign\Designes";
        //        }
        //        else
        //        {
        //            DesignPath = @"Reports\RepxReportDesign\STBRequisition\DocDesign\Designes";
        //        }
        //        string fullpath = Server.MapPath("~");
        //        fullpath = fullpath.Replace("ERP.UI\\", "");
        //        string DesignFullPath = fullpath + DesignPath;
        //        filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

        //        foreach (string filename in filePaths)
        //        {
        //            string reportname = Path.GetFileNameWithoutExtension(filename);
        //            string name = "";
        //            if (reportname.Split('~').Length > 1)
        //            {
        //                name = reportname.Split('~')[0];
        //            }
        //            else
        //            {
        //                name = reportname;
        //            }
        //            string reportValue = reportname;
        //            CmbDesignName.Items.Add(name, reportValue);
        //        }
        //        CmbDesignName.SelectedIndex = 0;
        //        SelectPanel.JSProperties["cpChecked"] = "";
        //    }
        //    else
        //    {
        //        string DesignPath = @"Reports\Reports\REPXReports";
        //        string fullpath = Server.MapPath("~");
        //        fullpath = fullpath.Replace("ERP.UI\\", "");
        //        string filename = @"\RepxReportViewer.aspx";
        //        string DesignFullPath = fullpath + DesignPath + filename;
        //        string reportName = Convert.ToString(CmbDesignName.Value);

        //        string NoofCopy = "";
        //        if (selectOriginal.Checked == true)
        //        {
        //            NoofCopy += 1 + ",";
        //        }
        //        if (selectDuplicate.Checked == true)
        //        {
        //            NoofCopy += 2 + ",";
        //        }
        //        SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
        //        SelectPanel.JSProperties["cpChecked"] = "Checked";

        //    }
        //}


        [WebMethod]
        public static string CancelSTBRequisitionOnRequest(string keyValue, string Reason)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            int CancelRecharge = 0;
            CancelRecharge = ClosedCancelSTBRequisitionOnRequest("CancelSTBRequisition", keyValue, Reason);
            return Convert.ToString(CancelRecharge);

        }

        public static int ClosedCancelSTBRequisitionOnRequest(String Action, string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddPara("@Action", Action);
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            proc.AddPara("@USER_ID", user_id);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        [WebMethod]
        public static string CheckWorkingRoster(string module_ID)
        {
            CommonBL ComBL = new CommonBL();
            string STBTransactionsRestrictBeyondTheWorkingDays = ComBL.GetSystemSettingsResult("STBTransactionsRestrictBeyondTheWorkingDays");
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    if (STBTransactionsRestrictBeyondTheWorkingDays.ToUpper() == "YES")
                    {
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBModuleRosterStatus");
                        proc.AddPara("@ModuleId", module_ID);
                        proc.AddPara("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));
                        DataSet ds = proc.GetDataSet();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "true")
                            {
                                output = "true";
                            }
                            else if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "false")
                            {

                                output = "false~" + ds.Tables[1].Rows[0]["BeginTime"].ToString() + "~" + ds.Tables[1].Rows[0]["EndTime"].ToString();
                            }

                        }
                        else
                        {
                            output = "false";
                        }
                    }
                    else
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            //string available = Convert.ToString(e.GetValue("IsCancel"));
            ////string availableClosed = Convert.ToString(e.GetValue("IsClosed"));
            //if (available.ToUpper() == "TRUE")
            //{
            //    e.Row.ForeColor = System.Drawing.Color.Red;
            //    e.Row.Font.Strikeout = true;
            //}
            ////else if (availableClosed.ToUpper() == "TRUE")
            ////{

            ////    e.Row.ForeColor = System.Drawing.Color.Gray;

            ////}
            //else
            //{
            //    e.Row.ForeColor = System.Drawing.Color.Black;
            //}

        }
    }
}