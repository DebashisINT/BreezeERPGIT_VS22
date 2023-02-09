//========================================================== Revision History ============================================================================================
//   1.0	V2.0.36	 PRITI  09-02-2023		0025266: Listing view upgradation required of TDS Nil Challan of Accounts & Finance
//========================================== End Revision History =======================================================================================================

using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.OMS.Management.Master.Mobileaccessconfiguration;

namespace ERP.OMS.Management.Activities
{
    public partial class TDSNilChallanList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        //CustomerReceiptAdjustmentBl customerReceiptAdjustment = new CustomerReceiptAdjustmentBl();
        TdsNillChallanBL blLayer = new TdsNillChallanBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CommonBL cbl = new CommonBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            String ctfinyear = "";
            ctfinyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(ctfinyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/TDSNilChallanList.aspx");
            
            if (!IsPostBack)
            {
                LoadDataonPageLoad();
            }
        }
        public void LoadDataonPageLoad()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            //DataTable branchtable = posSale.getBranchListByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            //cmbBranchfilter.DataSource = branchtable;
            //cmbBranchfilter.ValueField = "branch_id";
            //cmbBranchfilter.TextField = "branch_description";
            //cmbBranchfilter.DataBind();
            //cmbBranchfilter.SelectedIndex = 0;

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
         {
            e.KeyExpression = "TDSNillChallan_ID";

          //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            int userid = Convert.ToInt32(Session["UserID"]);  //---- REV 1.0
            //string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            //List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                //----REV 1.0
                //var q = from d in dc.v_TDSNilChallans
                //        where d.Payment_Date >= Convert.ToDateTime(strFromDate) &&
                //                  d.Payment_Date <= Convert.ToDateTime(strToDate)
                //        orderby d.Payment_Date descending
                //            select d;
                //    e.QueryableSource = q;

                var q = from d in dc.TDSNILLCHALLANLISTs
                        where d.USERID == userid
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
                //----END REV 1.0

                //if (strBranchID == "0")
                //{
                //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                //    var q = from d in dc.v_CreditNoteAdjustments
                //            where d.Adjustment_Date >= Convert.ToDateTime(strFromDate) &&
                //                  d.Adjustment_Date <= Convert.ToDateTime(strToDate)
                //            orderby d.Adjustment_Date descending
                //            select d;
                //    e.QueryableSource = q;
                //}
                //else
                //{
                //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                //    var q = from d in dc.v_CreditNoteAdjustments
                //            where d.Adjustment_Date >= Convert.ToDateTime(strFromDate) &&
                //                  d.Adjustment_Date <= Convert.ToDateTime(strToDate) &&
                //                  branchidlist.Contains(Convert.ToInt32(d.Branch))
                //            orderby d.Adjustment_Date descending
                //            select d;
                //    e.QueryableSource = q;
                //}
            }
            else
            {
                //---- REV 1.0
                //    var q = from d in dc.v_TDSNilChallans

                //            orderby d.Payment_Date descending
                //            select d;
                //    e.QueryableSource = q;

                var q = from d in dc.JOURNALENTRYLISTs
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
                //----END REV 1.0
            }
        }
        protected void gridAdvanceAdj_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Del")
            {
                int rowsNo = blLayer.DeleteData(param.Split('~')[1]);
                if (rowsNo > 0)
                {
                    gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Document Deleted Successfully";
                }
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }
        private void bindexport(int Filter)
        {
            gridAdvanceAdj.Columns[7].Visible = false;
            string filename = "TDS Nil Challan";
            exporter.FileName = filename;
            exporter.FileName = "TDS Nil Challan";
            exporter.PageHeader.Left = "TDS Nil Challan";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
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

        protected void gridAdvanceAdj_SummaryDisplayText(object sender, DevExpress.Web.ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }


        //REV 1.0

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(FormDate.Date);
            dtTo = Convert.ToDateTime(toDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            Task PopulateStockTrialDataTask = new Task(() => GetTDSNILLCHALLANdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetTDSNILLCHALLANdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TDSNILLCHALLAN_LIST", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                if (BRANCH_ID == "0")
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", Convert.ToString(Session["userbranchHierarchy"]));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                }
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        //END REV 1.0
    }
}