using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using ServiceManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.STBSchemeReceived
{
    public partial class STBSchemeReceivedList : System.Web.UI.Page
    {
        CommonBL ComBL = new CommonBL();
        String userbranch = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/STBSchemeReceived/STBSchemeReceivedList.aspx");
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

                DataSet branchtable = dsFetchAll();
                dddlApprovalEmployee.DataSource = branchtable.Tables[3];
                dddlApprovalEmployee.ValueField = "cnt_internalId";
                dddlApprovalEmployee.TextField = "DirectorName";
                dddlApprovalEmployee.DataBind();
                dddlApprovalEmployee.SelectedIndex = 0;
            }
        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
            return ds;
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
        public static string DeleteSchemeReceived(int SchemeReceived_ID)
        {
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedInsertUpdate");
                    proc.AddIntegerPara("@SchemeReceived_ID", SchemeReceived_ID);
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

            e.KeyExpression = "SchemeReceived_ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            int userid = Convert.ToInt32(Session["UserID"]);

            ServicveManagementDataClassesDataContext dc = new ServicveManagementDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                string BranchList = userbranch;// Convert.ToString(Session["userbranchHierarchy"]);

                if (strBranchID == "0")
                {
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.V_STBSchemeReceivedReportLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Create_date descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_STBSchemeReceivedReportLists
                            where d.DocumentDate >= Convert.ToDateTime(strFromDate) && d.DocumentDate <= Convert.ToDateTime(strToDate)
                                && branchidlist.Contains(Convert.ToInt32(d.Branch))
                            orderby d.Create_date descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_STBSchemeReceivedReportLists
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
        }

        [WebMethod]
        public static string DircetorApproval(String ApprovalEmployee, String SchemeReceived_id, String Remarks)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedInsertUpdate");
                    proc.AddVarcharPara("@Action", 500, "DirectorApproval");
                    proc.AddPara("@SchemeReceived_ID", Convert.ToString(SchemeReceived_id));
                    proc.AddPara("@Remarks", Convert.ToString(Remarks));
                    proc.AddVarcharPara("@ApprovalEmployee", 100, Convert.ToString(ApprovalEmployee));
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

        
    }
}