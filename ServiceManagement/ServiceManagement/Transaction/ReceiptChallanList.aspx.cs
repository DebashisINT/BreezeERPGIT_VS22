using BusinessLogicLayer;
using BusinessLogicLayer.ServiceManagement;
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
using UtilityLayer;

namespace ServiceManagement.ServiceManagement.Transaction
{
    public partial class ReceiptChallanList : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvAssignJobBL obj = new SrvAssignJobBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/ReceiptChallanList.aspx");
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
                //string output = string.Empty;
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanInsertUpdate");
                    proc.AddVarcharPara("@Action", 500, "ModuleType");
                    proc.AddIntegerPara("@USER_ID", user_id);
                    dt = proc.GetTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (item["ModuleType_ID"].ToString() == "1")
                            {
                                liToken.Style.Add("display", "!inline-block");
                            }
                            else if (item["ModuleType_ID"].ToString() == "2")
                            {
                                liChallan.Style.Add("display", "!inline-block");
                            }
                            else if (item["ModuleType_ID"].ToString() == "3")
                            {
                                liWorksheet.Style.Add("display", "!inline-block");
                            }
                        }
                    }

                    //DataTable dtTech = obj.GetAssignJobDetails(userbranch);
                    //if (dtTech != null && dtTech.Rows.Count > 0)
                    //{
                    //    ddlAssignTechnician.DataSource = dtTech;
                    //    ddlAssignTechnician.DataValueField = "cnt_id";
                    //    ddlAssignTechnician.DataTextField = "cnt_firstName";
                    //    ddlAssignTechnician.DataBind();
                    //    ddlAssignTechnician.SelectedIndex = 0;
                    //}

                }
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
        public static string DeleteReceiptChallan(int ReceiptChallan_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanInsertUpdate");
                    proc.AddIntegerPara("@ReceiptChallan_ID", ReceiptChallan_ID);
                    proc.AddVarcharPara("@Action", 500, "delete");
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

            e.KeyExpression = "ReceiptChallan_ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ServicveManagementDataClassesDataContext dc = new ServicveManagementDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                string BranchList = userbranch;// Convert.ToString(Session["userbranchHierarchy"]);

                if (strBranchID == "0")
                {
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.V_ReceiptChallanReportLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Create_date descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_ReceiptChallanReportLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Create_date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_ReceiptChallanReportLists
                        where d.Branch == -1
                        orderby d.Create_date descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        [WebMethod]
        public static string SingleAssign(String Technician_ID, String ReceiptChallan_ID, String Remarks)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
                    proc.AddVarcharPara("@Action", 500, "SingleJobInsert");
                    proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddVarcharPara("@Technician_Id", 100, Convert.ToString(Technician_ID));
                    proc.AddPara("@USER_ID", user_id);
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                    }
                    output = "true";
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

        [WebMethod]
        public static string SendSMS(int ReceiptChallan_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanInsertUpdate");
                    proc.AddIntegerPara("@ReceiptChallan_ID", ReceiptChallan_ID);
                    proc.AddVarcharPara("@Action", 500, "SendSMS");
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

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\ReceiptChallan\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\ReceiptChallan\DocDesign\Designes";
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
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        [WebMethod]
        public static ServiceRegisterReports SingleAssignTechnician(String ChallanID)
        {

            ServiceRegisterReports reports = new ServiceRegisterReports();
            List<Tecchnician> TecchnicianList = new List<Tecchnician>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_AssignJobDetails");
            proc.AddVarcharPara("@Action", 500, "SingleAssignTechnician");
            proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ChallanID));
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                TecchnicianList = APIHelperMethods.ToModelList<Tecchnician>(dt);
                reports.TecchnicianList = TecchnicianList;
            }
            return reports;
        }
    }
}