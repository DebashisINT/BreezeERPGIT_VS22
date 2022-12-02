using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ServiceManagement.ServiceManagement.Transaction.Warranty
{
    public partial class WarrantyList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        CommonBL ComBL = new CommonBL();
        string userbranchHierachy = null;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
            if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
            {
                if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                {
                    userbranchHierachy = EmployeeBranchMap();
                }
                else
                {
                    userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
            }
            else
            {
                userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            }
            Session["UserBranchMapID"] = userbranchHierachy;
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Warranty/WarrantyList.aspx");
            if (!IsPostBack)
            {
               // string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranchHierachy);
                //ddlBranch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                string user_id = Convert.ToString(Session["userid"]);

            }
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            //PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            //DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            //ddlBranch.DataSource = branchtable;
            //ddlBranch.DataValueField = "branch_id";
            //ddlBranch.DataTextField = "branch_description";
            //ddlBranch.DataBind();
            //ddlBranch.SelectedIndex = 0;
        }


        [WebMethod]
        public static List<WarrantyListdetails> WarrantyListDetails(string Branch, String FromDate, String ToDate)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Warranty/WarrantyList.aspx");
            List<WarrantyListdetails> ret = new List<WarrantyListdetails>();
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            proc.AddVarcharPara("@ACTION", 500, "WarrantyList");
            proc.AddPara("@FromDate", FromDate);
            proc.AddPara("@ToDate", ToDate);
            if (Branch == "")
            {
                //proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                proc.AddPara("@Location", Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]));
            }
            else
            {
                proc.AddPara("@Location", Branch);
            }
            proc.AddPara("@UserId", user_id);

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                ret = APIHelperMethods.ToModelList<WarrantyListdetails>(dt);
            }
            return ret;
        }

        [WebMethod]
        public static string DeleteWarranty(String WarrantyUpdateID)
        {
            string output = string.Empty;
            try
            {
                int NoOfRowEffected = 0;

                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
                    proc.AddVarcharPara("@ACTION", 500, "DeleteWarranty");
                    proc.AddPara("@WarrantyUpdateID", Convert.ToString(WarrantyUpdateID));
                    proc.AddPara("@UserId", user_id);
                    NoOfRowEffected = proc.RunActionQuery();
                    if (NoOfRowEffected > 0)
                    {
                        output = "true";
                    }

                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }

        public class WarrantyListdetails
        {
            public String WarrantyUpdateID { get; set; }
            public String ReceiptChallanID { get; set; }
            public String SrvEntryID { get; set; }
            public String SrvEntryDtlsId { get; set; }
            public String ReceiptChallanNo { get; set; }
            public String SerialNo { get; set; }
            public String NewSerialNo { get; set; }
            public String UpdateWarrantyDate { get; set; }
            public String ProblemDesc { get; set; }
            public String branch_description { get; set; }
            public String Branch { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String EnteredBy { get; set; }
            public String EnteredOn { get; set; }
            public String UpdatedBy { get; set; }
            public String UpdatedOn { get; set; }
            public String Actions { get; set; }
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

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    // ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["UserBranchMapID"]) + ") order by branch_description asc");
                }
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
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }
        #endregion
    }
}