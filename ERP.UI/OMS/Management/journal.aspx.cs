using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_journal : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        static string s1 = null;
        Management_BL oManagement_BL = new Management_BL();
        string ReturnValue = "2";
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlJournal.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //if (HttpContext.Current.Session["userlastsegment"].ToString() == "5")
            //{
            //    if (s1 == null)
            //        SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_CreateUser=" + Session["userid"].ToString() + " and JournalVoucher_ExchangeSegmentId in(0) and Left(JournalVoucher_VoucherNumber,2) not in('XE','XC','XP','XO','XQ','XB','XS','XV','XA','XF','XG','XT','XU','XY','XI','XJ','XK','XL','XM','XD','XH','XN','XR','XW','XX','XZ','YA','YB','YC','YD','YE','YG','YH','YI','YJ','YK','YL','YM','YN','YO','YP','YQ','YR','YS','YT','YU','YV','YW','YX','YY','YZ','ZA','ZB','ZC','ZE','ZD','ZF','ZG','ZH','ZI','ZJ','ZK','ZL','ZM','ZN','ZO','ZP','ZQ','ZR','ZS','ZT','ZU','ZV','ZW','ZX','ZY','ZZ')  order by JournalVoucher_CreateDateTime desc";
            //    else
            //        SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_ExchangeSegmentId in(0) and Left(JournalVoucher_VoucherNumber,2) not in('XE','XC','XP','XO','XQ','XB','XS','XV','XA','XF','XG','XT','XU','XY','XI','XJ','XK','XL','XM','XD','XH','XN','XR','XW','XX','XZ','YA','YB','YC','YD','YE','YG','YH','YI','YJ','YK','YL','YM','YN','YO','YP','YQ','YR','YS','YT','YU','YV','YW','YX','YY','YZ','ZA','ZB','ZC','ZE','ZD','ZF','ZG','ZH','ZI','ZJ','ZK','ZL','ZM','ZN','ZO','ZP','ZQ','ZR','ZS','ZT','ZU','ZV','ZW','ZX','ZY','ZZ')  order by JournalVoucher_CreateDateTime desc";
            //}
            //else
            //{
            if (s1 == null)
                SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_CreateUser=" + Session["userid"].ToString() + " and JournalVoucher_ExchangeSegmentId in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(JournalVoucher_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and JournalVoucher_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Left(JournalVoucher_VoucherNumber,2) not in('XE','XC','XP','XO','XQ','XB','XS','XV','XA','XF','XG','XT','XU','XY','XI','XJ','XK','XL','XM','XD','XH','XN','XR','XW','XX','XZ','YA','YB','YC','YD','YE','YG','YH','YI','YJ','YK','YL','YM','YN','YO','YP','YQ','YR','YS','YT','YU','YV','YW','YX','YY','YZ','ZA','ZB','ZC','ZE','ZD','ZF','ZG','ZH','ZI','ZJ','ZK','ZL','ZM','ZN','ZO','ZP','ZQ','ZR','ZS','ZT','ZU','ZV','ZW','ZX','ZY','ZZ') order by JournalVoucher_CreateDateTime desc";
            else
                SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where  JournalVoucher_ExchangeSegmentId in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(JournalVoucher_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and JournalVoucher_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Left(JournalVoucher_VoucherNumber,2) not in('XE','XC','XP','XO','XQ','XB','XS','XV','XA','XF','XG','XT','XU','XY','XI','XJ','XK','XL','XM','XD','XH','XN','XR','XW','XX','XZ','YA','YB','YC','YD','YE','YG','YH','YI','YJ','YK','YL','YM','YN','YO','YP','YQ','YR','YS','YT','YU','YV','YW','YX','YY','YZ','ZA','ZB','ZC','ZE','ZD','ZF','ZG','ZH','ZI','ZJ','ZK','ZL','ZM','ZN','ZO','ZP','ZQ','ZR','ZS','ZT','ZU','ZV','ZW','ZX','ZY','ZZ') order by JournalVoucher_CreateDateTime desc";
            //}
        }
        protected void gridJournal_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //if (HttpContext.Current.Session["userlastsegment"].ToString() == "5")
            //{
            //    SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_CreateUser=" + Session["userid"].ToString() + " and JournalVoucher_ExchangeSegmentId in(0) and Left(JournalVoucher_VoucherNumber,2) not in('XE','XC','XP','XO','XQ','XB','XS','XV','XA','XF','XG','XT','XU','XY','XI','XJ','XK','XL','XM','XD','XH','XN','XR','XW','XX','XZ','YA','YB','YC','YD','YE','YG','YH','YI','YJ','YK','YL','YM','YN','YO','YP','YQ','YR','YS','YT','YU','YV','YW','YX','YY','YZ','ZA','ZB','ZC','ZE','ZD','ZF','ZG','ZH','ZI','ZJ','ZK','ZL','ZM','ZN','ZO','ZP','ZQ','ZR','ZS','ZT','ZU','ZV','ZW','ZX','ZY','ZZ') order by JournalVoucher_CreateDateTime desc";
            //}
            //else
            //{
            SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_CreateUser=" + Session["userid"].ToString() + " and JournalVoucher_ExchangeSegmentId in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(JournalVoucher_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and JournalVoucher_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and Left(JournalVoucher_VoucherNumber,2) not in('XE','XC','XP','XO','XQ','XB','XS','XV','XA','XF','XG','XT','XU','XY','XI','XJ','XK','XL','XM','XD','XH','XN','XR','XW','XX','XZ','YA','YB','YC','YD','YE','YG','YH','YI','YJ','YK','YL','YM','YN','YO','YP','YQ','YR','YS','YT','YU','YV','YW','YX','YY','YZ','ZA','ZB','ZC','ZE','ZD','ZF','ZG','ZH','ZI','ZJ','ZK','ZL','ZM','ZN','ZO','ZP','ZQ','ZR','ZS','ZT','ZU','ZV','ZW','ZX','ZY','ZZ') order by JournalVoucher_CreateDateTime desc";
            //}
            //SqlJournal.SelectCommand = "select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,isnull((select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID),(select exch_membershipType from tbl_master_companyExchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID)) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_BranchID in(" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ") and JournalVoucher_ExchangeSegmentId in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(JournalVoucher_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and JournalVoucher_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' order by JournalVoucher_CreateDateTime desc";//"select JournalVoucher_ID,JournalVoucher_VoucherNumber as VoucherNumber,convert(varchar(11),JournalVoucher_TransactionDate,113) as TDate,JournalVoucher_TransactionDate,JournalVoucher_ExchangeSegmentID,(select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_JournalVoucher.JournalVoucher_ExchangeSegmentID) as Segment,JournalVoucher_BranchID,JournalVoucher_CompanyID from Trans_JournalVoucher where JournalVoucher_BranchID in(" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ") order by JournalVoucher_CreateDateTime desc";
            gridJournal.DataBind();
            if (e.Parameters == "s")
            {
                gridJournal.Settings.ShowFilterRow = true;
                s1 = "s";
            }

            if (e.Parameters == "All")
            {
                gridJournal.FilterExpression = string.Empty;
                s1 = null;
            }
        }
        protected void gridJournal_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string ID = e.Keys[0].ToString();
            DateTime TDate = Convert.ToDateTime(e.Values[5].ToString());
            string SegmentID = e.Values[4].ToString();
            string BranchID = e.Values[6].ToString();
            string VNo = e.Values[1].ToString();
            string CompID = e.Values[7].ToString();
            //oDBEngine.DeleteValue("trans_journalvoucher", " JournalVoucher_ID=" + ID + "");
            //oDBEngine.DeleteValue("trans_journalvoucherdetail", " journalvoucherdetail_VoucherNumber='" + VNo + "' and journalvoucherdetail_BranchID='" + BranchID + "' and journalvoucherdetail_ExchangeSegmentID='" + SegmentID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,journalvoucherdetail_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime)");
            //oDBEngine.DeleteValue("trans_accountsledger", " accountsledger_TransactionReferenceID='" + VNo + "' and accountsledger_BranchID='" + BranchID + "' and accountsledger_ExchangeSegmentID='" + SegmentID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime)");
            DataSet DS = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("JournalVoucherDelete", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@ID", ID);
            //cmd3.Parameters.AddWithValue("@TDate", Convert.ToDateTime(TDate));
            //cmd3.Parameters.AddWithValue("@SegmentID", SegmentID);
            //cmd3.Parameters.AddWithValue("@BranchID", BranchID);
            //cmd3.Parameters.AddWithValue("@VNo", VNo);
            //cmd3.Parameters.AddWithValue("@CompID", CompID);
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(DS);
            DS = oManagement_BL.JournalVoucherDelete(ID, Convert.ToDateTime(TDate).ToString("yyyy-MM-dd"), Convert.ToInt32(SegmentID), Convert.ToInt32(BranchID),
                    VNo, CompID);
            ReturnValue = "3";
            //cmd3.Dispose();
            e.Cancel = true;
        }
        protected void gridJournal_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = ReturnValue;
        }
    }
}