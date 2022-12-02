using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class serviceScheduleList : System.Web.UI.Page
    {
        MasterSettings objmaster = new MasterSettings();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select 'Select' state,0 as id union all select state,id from tbl_master_state where countryId=1");
            cmbStatefilter.TextField = "state";
            cmbStatefilter.ValueField = "id";
            cmbStatefilter.DataSource = dst;
            cmbStatefilter.DataBind();
            cmbStatefilter.SelectedIndex = 0;

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;

            hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");

            if (hdnDocumentSegmentSettings.Value == "0")
            {
                DivSegment1.Attributes.Add("style", "display:none");
                DivSegment2.Attributes.Add("style", "display:none");
                DivSegment3.Attributes.Add("style", "display:none");
                DivSegment4.Attributes.Add("style", "display:none");
                DivSegment5.Attributes.Add("style", "display:none");

            }

        }

        protected void Grdstockjournal_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        }
        protected void gridJournal_DataBinding(object sender, EventArgs e)
        {

        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strStateID = (Convert.ToString(hfStateID.Value) == "") ? "0" : Convert.ToString(hfStateID.Value);
            string strCustomerId = Convert.ToString(hdnCustomerId.Value);

            e.KeyExpression = "ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            int userid=Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //if (strStateID == "0")
            //{
            //    if (strCustomerId != "")
            //    {
            //        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //        var q = from d in dc.V_SCHEDULELISTs
            //                where d.Order_Date >= Convert.ToDateTime(strFromDate) &&
            //                d.Order_Date <= Convert.ToDateTime(strToDate) &&
            //                d.cnt_internalid == strCustomerId
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //    else
            //    {
            //        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //        var q = from d in dc.V_SCHEDULELISTs
            //                where d.Order_Date >= Convert.ToDateTime(strFromDate) &&
            //                d.Order_Date <= Convert.ToDateTime(strToDate)
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //}
            //else
            //{
            //    if (strCustomerId != "")
            //    {
            //        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //        var q = from d in dc.V_SCHEDULELISTs
            //                where d.Order_Date >= Convert.ToDateTime(strFromDate) &&
            //                d.Order_Date <= Convert.ToDateTime(strToDate) &&
            //                d.cnt_internalid == strCustomerId &&
            //                d.add_state == Convert.ToInt32(strStateID)
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //    else
            //    {
            //        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            //        var q = from d in dc.V_SCHEDULELISTs
            //                where d.Order_Date >= Convert.ToDateTime(strFromDate) &&
            //                d.Order_Date <= Convert.ToDateTime(strToDate) &&
            //                d.add_state == Convert.ToInt32(strStateID)
            //                select d;
            //        e.QueryableSource = q;
            //    }
            //}
            if (hfIsFilter.Value=="Y")
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.TBL_SCHEDULELISTs
                        where d.create_user == userid
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.TBL_SCHEDULELISTs
                        where d.create_user == 0
                        select d;
                e.QueryableSource = q;
            }
          
        }

        [WebMethod(EnableSession = true)]
        public static object GetSServiceScheduleList(string FromDate, String ToDate, String CustomerId, string segment1, string segment2, string segment3, string segment4, string segment5)
        {        
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("PRC_ServiceScheduleList");
                proc.AddPara("@Action", "GetList");
                proc.AddPara("@Customer_id", CustomerId);
                proc.AddPara("@Segment_Id1", segment1);
                proc.AddPara("@Segment_Id2", segment2);
                proc.AddPara("@Segment_Id3", segment3);
                proc.AddPara("@Segment_Id4", segment4);
                proc.AddPara("@Segment_Id5", segment5);
                proc.AddPara("@USER_ID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                proc.AddPara("@FromDate", FromDate);
                proc.AddPara("@ToDate", ToDate);
                DataTable Address = proc.GetTable();
            }
            return "OK";
        }
    }
}