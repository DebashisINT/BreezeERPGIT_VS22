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

namespace ServiceManagement.ServiceManagement.Transaction.STB
{
    public partial class stbList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SrvJobSheetEntryBL obj = new SrvJobSheetEntryBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/STB/stbList.aspx");
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
            Session["UserBranchMapID"] = userbranch;
            if (!IsPostBack)
            {
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;

                Session["SI_ComponentData_Branch"] = null;
                Session["TechnicianData"] = null;
                string user_id = Convert.ToString(Session["userid"]);
                DataTable dtusertyp = obj.GetUserType(user_id);
                if (dtusertyp != null && dtusertyp.Rows.Count > 0)
                {
                    hdnUserType.Value = dtusertyp.Rows[0]["contactType"].ToString();
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
        }

        [WebMethod]
        public static string DeleteSTB(String STBReceivedID)
        {
            string output = string.Empty;
            try
            {
                DataTable dt = new DataTable();

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBReceivingInsertUpdate");
                    proc.AddVarcharPara("@ACTION", 500, "DELETE");
                    proc.AddPara("@STBReceived_ID", Convert.ToString(STBReceivedID));
                    proc.AddPara("@UserID", user_id);
                    dt = proc.GetTable();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        output = dt.Rows[0]["msg"].ToString() + "~" + dt.Rows[0]["Status"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        [WebMethod]
        public static List<SrvSTBReceivedList> STBReceivedList(srv_STBReceivedInput model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/STB/stbList.aspx");
            List<SrvSTBReceivedList> listStatues = new List<SrvSTBReceivedList>();
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBReceivingDetails");
            proc.AddVarcharPara("@ACTION", 500, "List");
            proc.AddPara("@FromDate", model.FromDate);
            proc.AddPara("@ToDate", model.ToDate);
            if (model.Branch == "0")
            {
                proc.AddPara("@BRANCHID", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@BRANCHID", model.Branch);
            }
            ds = proc.GetDataSet();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    string Action = "";

                    if (rights.CanEdit)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='Edit(" + item["STBReceived_ID"].ToString() + ")'><i class='fa fa-pencil-square-o assig' data-toggle='tooltip' data-placement='bottom' title='Edit' ></i> </span>";
                    }
                    if (rights.CanView)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='View(" + item["STBReceived_ID"].ToString() + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='bottom' title='Details' ></i> </span>";
                    }
                    if (rights.CanDelete)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='Delete(" + item["STBReceived_ID"].ToString() + ")'><i class='fa fa-trash det' data-toggle='tooltip' data-placement='bottom' title='Delete' ></i> </span>";
                    }
                    if (rights.CanPrint)
                    {
                        Action = Action + " <span class='actionInput text-center' onclick='onPrintJv(" + item["STBReceived_ID"].ToString() + ")'><i class='fa fa-print det' data-toggle='tooltip' data-placement='bottom' title='Print' ></i> </span>";
                    }

                    listStatues.Add(new SrvSTBReceivedList
                    {
                        DocumentNo = item["DocumentNo"].ToString(),
                        Date = item["Date"].ToString(),
                        Unit = item["Unit"].ToString(),
                        Location = item["Location"].ToString(),
                        Remarks = item["Remarks"].ToString(),
                        Receivedby = item["Receivedby"].ToString(),
                        Receivedon = Convert.ToDateTime(item["CreateDate"].ToString()).ToString("dd-MM-yyyy HH:mm:ss"),
                        Action = Action
                    });
                }
            }

            return listStatues;
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

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SRVJobSheet\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SRVJobSheet\DocDesign\Designes";
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

        //#region Branch Populate

        //protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        DataTable ComponentTable = new DataTable();
        //        string Hoid = e.Parameter.Split('~')[1];
        //        if (Session["userbranchHierarchy"] != null)
        //        {
        //            // ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
        //            ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
        //        }
        //        if (ComponentTable.Rows.Count > 0)
        //        {
        //            Session["SI_ComponentData_Branch"] = ComponentTable;
        //            lookup_branch.DataSource = ComponentTable;
        //            lookup_branch.DataBind();
        //        }
        //        else
        //        {
        //            Session["SI_ComponentData_Branch"] = ComponentTable;
        //            lookup_branch.DataSource = null;
        //            lookup_branch.DataBind();
        //        }
        //    }
        //}

        //protected void lookup_branch_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["SI_ComponentData_Branch"] != null)
        //    {
        //        lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
        //    }
        //}

        //#endregion

        public class srv_STBReceivedInput
        {
            public String Action { get; set; }
            public String FromDate { get; set; }
            public String ToDate { get; set; }
            public String Branch { get; set; }
            public String SearchType { get; set; }
        }

        public class SrvSTBReceivedList
        {
            public String DocumentNo { get; set; }
            public String Date { get; set; }
            public String Unit { get; set; }
            public String Location { get; set; }
            public String Remarks { get; set; }
            public String Receivedby { get; set; }
            public String Receivedon { get; set; }
            public String Action { get; set; }
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;

            e.KeyExpression = "STBReceived_ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ServicveManagementDataClassesDataContext dc = new ServicveManagementDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                string BranchList = userbranch;// Convert.ToString(Session["userbranchHierarchy"]);

                if (strBranchID == "0")
                {
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.V_STBReceivingReportLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Create_date descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_STBReceivingReportLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Create_date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_STBReceivingReportLists
                        where d.Branch == -1
                        orderby d.Create_date descending
                        select d;
                e.QueryableSource = q;
            }
        }
    }
}