using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using ERP.Models;
using System.Linq;

namespace ERP.OMS.Management.Activities
{
    public partial class InfluencerPaymentList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Payment_ID";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            //string connectionString = ConfigurationManager.ConnectionStrings["GECORRECTIONConnectionString"].ConnectionString;

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string ComponyId = Convert.ToString(Session["LastCompany"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    DateTime _fromDt = Convert.ToDateTime(strFromDate);
                    DateTime _todate = Convert.ToDateTime(strToDate);
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_InfluencerLists
                            where branchidlist.Contains(Convert.ToInt32(d.BranchID)) &&
                            d.Payment_TransactionDate >= _fromDt && d.Payment_TransactionDate <= _todate
                            && d.CompanyID == ComponyId
                            orderby d.SrlNo 
                            select d;

                    e.QueryableSource = q;
                    //var cnt = q.Count();
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_InfluencerLists
                            where d.Payment_TransactionDate >= Convert.ToDateTime(strFromDate) && d.Payment_TransactionDate <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.CompanyID == ComponyId
                            orderby d.Payment_ID descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_InfluencerLists
                        where d.BranchID == 0
                        orderby d.Payment_ID descending
                        select d;

                e.QueryableSource = q;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            String finyear = "";
            finyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_InfluencerPayment.Columns[13].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_InfluencerPayment.Columns[13].Visible = false;
                }
            }
            
            
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                Session["SaveModeVPR"] = null;
                Session["VendorPaymentReceiptDetails"] = null;
                //................Cookies..................
                //Grid_CustomerReceiptPayment.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_CustomerReceiptPaymentVendorPaymentReceipt";
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_CustomerReceiptPaymentVendorPaymentReceipt');</script>");
                ////...........Cookies End............... 
                Session.Remove("VendorPayRecListingDetails");
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                GetAllDropDownDetailForCashBank();
            }
            // FillGrid();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/InfluencerPaymentList.aspx");
        }
        public void GetAllDropDownDetailForCashBank()
        {
            DataSet dst = new DataSet();
            dst = AllDropDownDetailForCashBank();
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataSource = dst.Tables[0];
                cmbBranchfilter.DataBind();
                cmbBranchfilter.SelectedIndex = 0;
                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }
        }
        public DataSet AllDropDownDetailForCashBank()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_VendorReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ds = proc.GetDataSet();
            return ds;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            //exporter.GridView = Grid_ContraVoucher;

            exporter.GridViewID = "Grid_InfluencerPayment";
            string filename = "InfluencerPayment";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Influencer Payment";
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
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {

            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));

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

        protected void Grid_InfluencerPayment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            InfluencerBL infBl = new InfluencerBL();
            DataTable PurchaseOrderdt = new DataTable();
            string Command = Convert.ToString(e.Parameters).Split('~')[0];
            string Inf_ID = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    Inf_ID = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (Command == "Delete")
            {


                deletecnt = infBl.DeleteInfluencerPayment(Inf_ID);
                if (deletecnt == 1)
                {
                    Grid_InfluencerPayment.JSProperties["cpDelete"] = "Deleted successfully";
                }
                else
                {
                    Grid_InfluencerPayment.JSProperties["cpDelete"] = "Try again.";
                }
            }


        }








    }
}