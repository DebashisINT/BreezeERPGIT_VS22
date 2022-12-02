using BusinessLogicLayer;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ScheduleDetailsList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ServiceTemplate objServiceTemplate = new ServiceTemplate();
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

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/ServiceMaterialIssueList.aspx");
            CommonBL ComBL = new CommonBL();
            

            if (!IsPostBack) 
            {
                LoadDataonPageLoad();            
            }
        }

        public void LoadDataonPageLoad()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            //FormDate.Date = DateTime.Now;
            //toDate.Date = DateTime.Now;
                                         

        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
         {
            e.KeyExpression = "DETAILS_ID";

            //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            string strCustomerId = Convert.ToString(hdnCustomerId.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    if (strCustomerId != "")
                    {
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.v_SCHEDULEDETAILS_Lists
                                where d.SCHEDULE_DATE >= Convert.ToDateTime(strFromDate) &&
                                      d.SCHEDULE_DATE <= Convert.ToDateTime(strToDate) &&
                                      d.CUSTOMER_ID == strCustomerId
                                orderby d.SCHEDULE_DATE descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                        branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                        var q = from d in dc.v_SCHEDULEDETAILS_Lists
                                where d.SCHEDULE_DATE >= Convert.ToDateTime(strFromDate) &&
                                      d.SCHEDULE_DATE <= Convert.ToDateTime(strToDate)
                                orderby d.SCHEDULE_DATE descending
                                select d;
                        e.QueryableSource = q;
                    }
                    
                }
                else
                {
                    if(strCustomerId!="")
                    {
                        if (strBranchID == "0")
                        {
                            branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                            var q = from d in dc.v_SCHEDULEDETAILS_Lists
                                    where d.SCHEDULE_DATE >= Convert.ToDateTime(strFromDate) &&
                                          d.SCHEDULE_DATE <= Convert.ToDateTime(strToDate) &&                                          
                                          d.CUSTOMER_ID == strCustomerId
                                    orderby d.SCHEDULE_DATE descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        else
                        {
                            branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                            var q = from d in dc.v_SCHEDULEDETAILS_Lists
                                    where d.SCHEDULE_DATE >= Convert.ToDateTime(strFromDate) &&
                                          d.SCHEDULE_DATE <= Convert.ToDateTime(strToDate) &&
                                          branchidlist.Contains(Convert.ToInt32(d.ASSIGNED_BRANCH)) &&
                                          d.CUSTOMER_ID == strCustomerId
                                    orderby d.SCHEDULE_DATE descending
                                    select d;
                            e.QueryableSource = q;
                        }
                        
                    }
                    else{
                        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                        var q = from d in dc.v_SCHEDULEDETAILS_Lists
                            where d.SCHEDULE_DATE >= Convert.ToDateTime(strFromDate) &&
                                  d.SCHEDULE_DATE <= Convert.ToDateTime(strToDate) &&
                                  branchidlist.Contains(Convert.ToInt32(d.ASSIGNED_BRANCH))                                
                            orderby d.SCHEDULE_DATE descending
                            select d;
                    e.QueryableSource = q;
                    }
                    
                }
            }
            else
            {
                var q = from d in dc.v_SCHEDULEDETAILS_Lists
                        where Convert.ToInt32(d.ASSIGNED_BRANCH) == '0'
                        orderby d.SCHEDULE_DATE descending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void gridAdvanceAdj_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = Convert.ToString(e.Parameters);
            if (param.Split('~')[0] == "Del")
            {
                int rowsNo = objServiceTemplate.DeleteServiceTemplate(param.Split('~')[1]);
             if (rowsNo > 0)
             {
                 gridAdvanceAdj.JSProperties["cpReturnMesg"] = "Document Deleted Successfully";
             }
            }
        }


        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
        //    if (Filter != 0)
        //    {
        //        bindexport(Filter); 
        //    }
        //}

        private void bindexport(int Filter)
        {
            //gridAdvanceAdj.Columns[7].Visible = false;
            string filename = "Material Issue";
            exporter.FileName = filename;
            exporter.FileName = "Material Issue";

            exporter.PageHeader.Left = "Material Issue";
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

    }
}