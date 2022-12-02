using System;
using System.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_ICEXRegulatoryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string dp = "ICEX - COMM";
            ViewRegulatoryReport view = new ViewRegulatoryReport();
            //DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
            //                                        " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
            //                                            " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"] + ")");

            //CompanyId = lastSegMemt.Rows[0][0].ToString();
            //dpId = lastSegMemt.Rows[0][2].ToString();
            //SegmentId = lastSegMemt.Rows[0][1].ToString();

            string CompanyID = Session["LastCompany"].ToString();
            string UserId = HttpContext.Current.Session["usersegid"].ToString();
            // view.viewdata(CompanyID, UserId, dp);
            //int crystalRptStatus =

        }
    }
}