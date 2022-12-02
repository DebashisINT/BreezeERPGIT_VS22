using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BusinessLogicLayer;
using System.Net.Http;

namespace ERP.OMS.Management.Activities
{
    public partial class ReSchedule : System.Web.UI.Page
    {
        public static string filesrc = string.Empty;
        public static string filedoc = string.Empty;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        ServiceTemplate objServiceTemplate = new ServiceTemplate();
        string TechnicianID = "";
        Converter objConverter = new Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        string FolderName = String.Empty;
        string path = String.Empty;
        string FormFolderName = String.Empty;
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
            ddlBranch.DataSource = branchtable;
            ddlBranch.ValueField = "branch_id";
            ddlBranch.TextField = "branch_description";
            ddlBranch.DataBind();
            ddlBranch.SelectedIndex = 0;

            //FormDate.Date = DateTime.Now;
            //toDate.Date = DateTime.Now;
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select * from tbl_master_branch where branch_id in (" + Convert.ToString(Session["userbranchhierarchy"]) + ")");
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.DataSource = dst;
            cmbBranchfilter.DataBind();


            DataTable dstTech = new DataTable();
            dstTech = objDB.GetDataTable("select DISTINCT CNT.cnt_firstName NAME,ISNULL(user_id,0) ID from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId INNER JOIN TBL_MASTER_USER ON user_contactId=CNT1.cnt_internalId INNER JOIN Srv_master_TechnicianBranch_map ON Tech_InternalId=CNT.cnt_internalId where CNT.cnt_contactType='TM' AND TBL_MASTER_USER.IsSync=1");
            InProddlTechnician.TextField = "NAME";
            InProddlTechnician.ValueField = "ID";
            InProddlTechnician.DataSource = dstTech;
            InProddlTechnician.DataBind();



            OnHoldddlTechnician.TextField = "NAME";
            OnHoldddlTechnician.ValueField = "ID";
            OnHoldddlTechnician.DataSource = dstTech;
            OnHoldddlTechnician.DataBind();

            ReleaseHoldddlTechnician.TextField = "NAME";
            ReleaseHoldddlTechnician.ValueField = "ID";
            ReleaseHoldddlTechnician.DataSource = dstTech;
            ReleaseHoldddlTechnician.DataBind();



            CanddlTechnician.TextField = "NAME";
            CanddlTechnician.ValueField = "ID";
            CanddlTechnician.DataSource = dstTech;
            CanddlTechnician.DataBind();

            CompddlTechnician.TextField = "NAME";
            CompddlTechnician.ValueField = "ID";
            CompddlTechnician.DataSource = dstTech;
            CompddlTechnician.DataBind();


            InProStartDate.Date = DateTime.Now;
            OnHoldScheduleDate.Date = DateTime.Now;
            OnHoldHoldUpto.Date = DateTime.Now;
            CompStartDate.Date = DateTime.Now;
            CompEndDate.Date = DateTime.Now;
            CanScheduleDate.Date = DateTime.Now;
            ReleaseHoldHoldUpto.Date = DateTime.Now;
            ReleaseHoldScheduleDate.Date = DateTime.Now;

            DataTable dstUser = new DataTable();
            dstUser = objDB.GetDataTable("select user_id,user_name from TBL_MASTER_USER");

            CompleteUser.TextField = "user_name";
            CompleteUser.ValueField = "user_id";
            CompleteUser.DataSource = dstUser;
            CompleteUser.DataBind();



            ddlReSchedule.TextField = "user_name";
            ddlReSchedule.ValueField = "user_id";
            ddlReSchedule.DataSource = dstUser;
            ddlReSchedule.DataBind();

            InProgressUser.TextField = "user_name";
            InProgressUser.ValueField = "user_id";
            InProgressUser.DataSource = dstUser;
            InProgressUser.DataBind();

            OnHoldUser.TextField = "user_name";
            OnHoldUser.ValueField = "user_id";
            OnHoldUser.DataSource = dstUser;
            OnHoldUser.DataBind();

            CancelledUser.TextField = "user_name";
            CancelledUser.ValueField = "user_id";
            CancelledUser.DataSource = dstUser;
            CancelledUser.DataBind();

            ReleaseHoldUser.TextField = "user_name";
            ReleaseHoldUser.ValueField = "user_id";
            ReleaseHoldUser.DataSource = dstUser;
            ReleaseHoldUser.DataBind();



            ddlReSchedule.Value = Session["UserID"].ToString();
            CompleteUser.Value = Session["UserID"].ToString();
            InProgressUser.Value = Session["UserID"].ToString();
            OnHoldUser.Value = Session["UserID"].ToString();
            CancelledUser.Value = Session["UserID"].ToString();
            ReleaseHoldUser.Value = Session["UserID"].ToString();
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
                    if (strCustomerId != "")
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
                    else
                    {
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

        protected void ddlTechnician_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select CNT.cnt_firstName NAME,ISNULL(user_id,0) ID from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId INNER JOIN TBL_MASTER_USER ON user_contactId=CNT1.cnt_internalId INNER JOIN Srv_master_TechnicianBranch_map ON Tech_InternalId=CNT.cnt_internalId where CNT.cnt_contactType='TM' and  branch_id=" + cmbBranchfilter.Value.ToString());
            ddlTechnician.TextField = "NAME";
            ddlTechnician.ValueField = "ID";
            ddlTechnician.DataSource = dst;
            ddlTechnician.DataBind();

        }

        protected void listBox_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (cmbBranchfilter.Value != null)
            {
                ASPxListBox s = sender as ASPxListBox;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select CNT.cnt_firstName NAME,ISNULL(user_id,0) ID from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId INNER JOIN TBL_MASTER_USER ON user_contactId=CNT1.cnt_internalId INNER JOIN Srv_master_TechnicianBranch_map ON Tech_InternalId=CNT.cnt_internalId where CNT.cnt_contactType='TM' and  branch_id=" + cmbBranchfilter.Value.ToString());
                s.TextField = "NAME";
                s.ValueField = "ID";
                s.DataSource = dst;
                s.DataBind();
                if (e.Parameter.Split('~')[0] == "ShowEditData")
                {
                    DataTable dt = new DataTable();
                    dt = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(hdnScheduleId.Value) + "'");
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["SUB_TECHNICIANID"])))
                    {
                        string SUB_TECHNICIANID = Convert.ToString(dt.Rows[0]["SUB_TECHNICIANID"]);
                        foreach (var item in SUB_TECHNICIANID.Split(','))
                        {
                            s.Items.FindByValue(item).Selected = true;
                        }

                    }
                }
            }
        }

        protected void ReSchedulePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "ShowData")
            {
                string id = hdnScheduleId.Value;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(id) + "'");

                DataTable dt = new DataTable();
                dt = objDB.GetDataTable("select * from tbl_master_branch where branch_id in (" + Convert.ToString(Session["userbranchhierarchy"]) + ")");
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.DataSource = dt;
                cmbBranchfilter.DataBind();
                if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_BRANCH"])))
                {
                    cmbBranchfilter.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_BRANCH"]);
                    DataTable dt1 = new DataTable();
                    dt1 = objDB.GetDataTable("select CNT.cnt_firstName NAME,ISNULL(user_id,0) ID from tbl_master_contact CNT INNER JOIN tbl_master_contact CNT1 ON CNT.cnt_mainAccount=CNT1.cnt_internalId INNER JOIN TBL_MASTER_USER ON user_contactId=CNT1.cnt_internalId INNER JOIN Srv_master_TechnicianBranch_map ON Tech_InternalId=CNT.cnt_internalId where CNT.cnt_contactType='TM' and  branch_id=" + cmbBranchfilter.Value.ToString());
                    ddlTechnician.TextField = "NAME";
                    ddlTechnician.ValueField = "ID";
                    ddlTechnician.DataSource = dt1;
                    ddlTechnician.DataBind();
                    if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"])))
                    {
                        ddlTechnician.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"]);
                    }

                    DataTable dst1 = new DataTable();

                    dst1 = objDB.GetDataTable("select top 1 * from TBL_AssignScheduleStatus  where SCH_ID='" + Convert.ToString(hdnScheduleId.Value) + "' and STATUS=6 order by ID desc ");

                    if (dst1.Rows.Count > 0 && dst1 != null)
                    {
                        DataTable dst2 = new DataTable();

                        dst2 = objDB.GetDataTable("select * from TBL_AssignScheduleStatusAttachment  where AssignScheduleStatusId='" + Convert.ToString(dst1.Rows[0]["ID"]) + "'");

                        if (dst2.Rows.Count > 0 && dst2 != null)
                        {
                            string VersionBaseURL = ConfigurationManager.AppSettings["VersionBaseURL"];
                            if (!string.IsNullOrEmpty(Convert.ToString(dst2.Rows[0]["Attachment_Name"])))
                            {
                                String attachmentFile = VersionBaseURL + "/CommonFolder/ScheduleAttachment/" + Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                                filesrc = VersionBaseURL + "/CommonFolder/ScheduleAttachment/";
                                filedoc = Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                                FileReSeheduleFile.Attributes.Add("class", "hide");

                                divReSeheduleFile.Visible = true;

                                hddnReSeheduleFile.Value = filedoc;
                            }
                        }
                        else
                        {
                            FileReSeheduleFile.Attributes.Add("class", "show");

                        }
                    }

                }
            }
        }

        protected void InProPanelPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "ShowData")
            {
                string id = hdnScheduleId.Value;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(id) + "'");

                if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"])))
                {
                    InProddlTechnician.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"]);
                }

                DataTable dst1 = new DataTable();

                dst1 = objDB.GetDataTable("select top 1 * from TBL_AssignScheduleStatus  where SCH_ID='" + Convert.ToString(hdnScheduleId.Value) + "' and STATUS=1 order by ID desc ");

                if (dst1.Rows.Count > 0 && dst1 != null)
                {
                    InProStartDate.Date = Convert.ToDateTime(dst1.Rows[0]["DATE"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dst1.Rows[0]["TIME"])))
                    {
                        string Value = Convert.ToString(dst1.Rows[0]["TIME"]);
                        InProPanelPanel.JSProperties["cpInProTime"] = Value;
                        hdnInprogressTime.Value = Value;
                        //txtHrs.Text = Convert.ToString(Value.Split(':')[0]);
                        //string _min = Value.Split(':')[1];

                        //txtMin.Text = Convert.ToString(_min.Split(' ')[0]);
                        //ddlTime.SelectedValue = Convert.ToString(_min.Split(' ')[1]);
                    }
                    txtRemarks.Text = Convert.ToString(dst1.Rows[0]["REMARKS"]);

                    DataTable dst2 = new DataTable();

                    dst2 = objDB.GetDataTable("select * from TBL_AssignScheduleStatusAttachment  where AssignScheduleStatusId='" + Convert.ToString(dst1.Rows[0]["ID"]) + "'");

                    if (dst2.Rows.Count > 0 && dst2 != null)
                    {
                        string VersionBaseURL = ConfigurationManager.AppSettings["VersionBaseURL"];
                        if (!string.IsNullOrEmpty(Convert.ToString(dst2.Rows[0]["Attachment_Name"])))
                        {
                            String attachmentFile = VersionBaseURL + "/CommonFolder/ScheduleAttachment/" + Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            filesrc = VersionBaseURL + "/CommonFolder/ScheduleAttachment/";
                            filedoc = Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            Attachmentdiv.Attributes.Add("class", "hide");

                            divfile.Visible = true;

                            hidFilenameInProgress.Value = filedoc;
                        }
                    }
                    else
                    {
                        Attachmentdiv.Attributes.Add("class", "SHOW");

                    }
                }

            }
        }
        public class AssignJobOutPut
        {
            public string status { get; set; }
            public string message { get; set; }
            public string id { get; set; }
        }
        public class IsProgress
        {
            public string session_token { get; set; }
            public string user_id { get; set; }
            public string job_id { get; set; }
            public string start_date { get; set; }
            public string start_time { get; set; }
            public string service_due { get; set; }
            public string service_completed { get; set; }
            public string next_date { get; set; }
            public string next_time { get; set; }
            public string remarks { get; set; }
            public string date_time { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string address { get; set; }
        }

        public class OnHold
        {
            public string session_token { get; set; }

            public string user_id { get; set; }

            public string job_id { get; set; }

            public string hold_date { get; set; }

            public string hold_time { get; set; }

            public string reason_hold { get; set; }

            public string remarks { get; set; }

            public string date_time { get; set; }

            public string latitude { get; set; }

            public string longitude { get; set; }

            public string address { get; set; }
        }

        public class UnHold
        {
            public string session_token { get; set; }

            public string user_id { get; set; }

            public string job_id { get; set; }

            public string unhold_date { get; set; }

            public string unhold_time { get; set; }

            public string reason_unhold { get; set; }

            public string remarks { get; set; }

            public string date_time { get; set; }

            public string latitude { get; set; }

            public string longitude { get; set; }

            public string address { get; set; }
        }
        protected void OnHoldPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "ShowData")
            {
                string id = hdnScheduleId.Value;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(id) + "'");

                if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"])))
                {
                    OnHoldddlTechnician.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"]);
                }
                DataTable dst1 = new DataTable();
                dst1 = objDB.GetDataTable("select top 1 * from TBL_AssignScheduleStatus  where SCH_ID='" + Convert.ToString(hdnScheduleId.Value) + "' and STATUS=2 order by ID desc ");

                if (dst1.Rows.Count > 0 && dst1 != null)
                {
                    OnHoldHoldUpto.Date = Convert.ToDateTime(dst1.Rows[0]["DATE"]);
                    txtHoldRemarks.Text = Convert.ToString(dst1.Rows[0]["REMARKS"]);

                    DataTable dst2 = new DataTable();
                    dst2 = objDB.GetDataTable("select * from TBL_AssignScheduleStatusAttachment  where AssignScheduleStatusId='" + Convert.ToString(dst1.Rows[0]["ID"]) + "'");

                    if (dst2.Rows.Count > 0 && dst2 != null)
                    {
                        string VersionBaseURL = ConfigurationManager.AppSettings["VersionBaseURL"];
                        if (!string.IsNullOrEmpty(Convert.ToString(dst2.Rows[0]["Attachment_Name"])))
                        {
                            String attachmentFile = VersionBaseURL + "/CommonFolder/ScheduleAttachment/" + Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            filesrc = VersionBaseURL + "/CommonFolder/ScheduleAttachment/";
                            filedoc = Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            DivfileOnHold.Attributes.Add("class", "hide");
                            divOnHoldAttachView.Visible = true;
                            hidFilenameOnHold.Value = filedoc;
                        }
                    }
                    else
                    {
                        DivfileOnHold.Attributes.Add("class", "show");
                    }
                }

            }
        }

        public class Cancelled
        {
            public string session_token { get; set; }

            public string user_id { get; set; }

            public string job_id { get; set; }

            public string date { get; set; }

            public string time { get; set; }

            public string cancel_reason { get; set; }

            public string remarks { get; set; }

            public string date_time { get; set; }

            public string latitude { get; set; }

            public string longitude { get; set; }

            public string address { get; set; }

            public string cancelled_by { get; set; }

            public string user { get; set; }
        }
        protected void CancelledPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "ShowData")
            {
                string id = hdnScheduleId.Value;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(id) + "'");

                if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"])))
                {
                    CanddlTechnician.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"]);
                }
                DataTable dst1 = new DataTable();
                dst1 = objDB.GetDataTable("select top 1 * from TBL_AssignScheduleStatus  where SCH_ID='" + Convert.ToString(hdnScheduleId.Value) + "' and STATUS=4 order by ID desc ");

                if (dst1.Rows.Count > 0 && dst1 != null)
                {
                    CanScheduleDate.Date = Convert.ToDateTime(dst1.Rows[0]["DATE"]);
                    txtReason.Text = Convert.ToString(dst1.Rows[0]["CANCEL_REASON"]);

                    DataTable dst2 = new DataTable();
                    dst2 = objDB.GetDataTable("select * from TBL_AssignScheduleStatusAttachment  where AssignScheduleStatusId='" + Convert.ToString(dst1.Rows[0]["ID"]) + "'");

                    if (dst2.Rows.Count > 0 && dst2 != null)
                    {
                        string VersionBaseURL = ConfigurationManager.AppSettings["VersionBaseURL"];
                        if (!string.IsNullOrEmpty(Convert.ToString(dst2.Rows[0]["Attachment_Name"])))
                        {
                            String attachmentFile = VersionBaseURL + "/CommonFolder/ScheduleAttachment/" + Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            filesrc = VersionBaseURL + "/CommonFolder/ScheduleAttachment/";
                            filedoc = Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            DivfileCancelled.Attributes.Add("class", "hide");
                            divCanAttachView.Visible = true;
                            hddnCanAttach.Value = filedoc;
                        }
                    }
                    else
                    {
                        DivfileCancelled.Attributes.Add("class", "show");
                    }
                }

            }
        }
        public class Completed
        {
            public string session_token { get; set; }

            public string user_id { get; set; }

            public string job_id { get; set; }

            public string finish_date { get; set; }

            public string finish_time { get; set; }

            public string remarks { get; set; }

            public string date_time { get; set; }

            public string latitude { get; set; }

            public string longitude { get; set; }

            public string address { get; set; }
            public string phone_no { get; set; }
        }
        protected void CompletePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "ShowData")
            {
                string id = hdnScheduleId.Value;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(id) + "'");

                if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"])))
                {
                    InProddlTechnician.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"]);
                }
                DataTable dst1 = new DataTable();
                dst1 = objDB.GetDataTable("select top 1 * from TBL_AssignScheduleStatus  where SCH_ID='" + Convert.ToString(hdnScheduleId.Value) + "' and STATUS=3 order by ID desc ");

                if (dst1.Rows.Count > 0 && dst1 != null)
                {
                    CompStartDate.Date = Convert.ToDateTime(dst1.Rows[0]["DATE"]);
                    CompEndDate.Date = Convert.ToDateTime(dst1.Rows[0]["DATE"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dst1.Rows[0]["TIME"])))
                    {
                        string Value = Convert.ToString(dst1.Rows[0]["TIME"]);
                        CompletePanel.JSProperties["cpCompleateTime"] = Value;
                        hdnComEndTime.Value = Value;

                    }
                    txtCompleteRemarks.Text = Convert.ToString(dst1.Rows[0]["REMARKS"]);

                    DataTable dst2 = new DataTable();
                    dst2 = objDB.GetDataTable("select * from TBL_AssignScheduleStatusAttachment  where AssignScheduleStatusId='" + Convert.ToString(dst1.Rows[0]["ID"]) + "'");

                    if (dst2.Rows.Count > 0 && dst2 != null)
                    {
                        string VersionBaseURL = ConfigurationManager.AppSettings["VersionBaseURL"];
                        if (!string.IsNullOrEmpty(Convert.ToString(dst2.Rows[0]["Attachment_Name"])))
                        {
                            String attachmentFile = VersionBaseURL + "/CommonFolder/ScheduleAttachment/" + Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            filesrc = VersionBaseURL + "/CommonFolder/ScheduleAttachment/";
                            filedoc = Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            CompAttachmentdiv.Attributes.Add("class", "hide");
                            divCompAttachView.Visible = true;
                            hidFilenameCompAttach.Value = filedoc;
                        }
                    }
                    else
                    {
                        CompAttachmentdiv.Attributes.Add("class", "show");
                    }
                }

            }
        }

        protected void BtnSaveInProgress_Click(object sender, EventArgs e)
        {
            int year = oDBEngine.GetDate().Year;
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select FSM_userid from tbl_master_user where user_id=" + Convert.ToString(InProddlTechnician.Value) + "");
            if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["FSM_userid"])))
            {
                TechnicianID = Convert.ToString(dst.Rows[0]["FSM_userid"]);
            }

            IsProgress objTechAssign = new IsProgress();
            objTechAssign.session_token = Convert.ToString("asfdgfhfxcbvcbnvgfjghkjghk");
            objTechAssign.user_id = Convert.ToString(TechnicianID);
            objTechAssign.job_id = Convert.ToString(hdnScheduleId.Value);
            objTechAssign.start_date = Convert.ToString(InProStartDate.Date);
            objTechAssign.start_time = Convert.ToString(hdnInprogressTime.Value);
            objTechAssign.service_due = Convert.ToString("");
            objTechAssign.service_completed = Convert.ToString("");
            objTechAssign.next_date = Convert.ToString(InProStartDate.Date);
            objTechAssign.next_time = Convert.ToString("0");
            objTechAssign.remarks = Convert.ToString(txtRemarks.Text);
            objTechAssign.date_time = Convert.ToString(DateTime.Now);
            objTechAssign.latitude = Convert.ToString("0");
            objTechAssign.longitude = Convert.ToString("0");
            objTechAssign.address = Convert.ToString("Address");


            string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];
            string data = JsonConvert.SerializeObject(objTechAssign);

            AssignJobOutPut oview = new AssignJobOutPut();


            //path = HttpContext.Current.Server.MapPath(@"..\Documents\");
            //string fulpath = path + TechnicianID;
            //if (!System.IO.Directory.Exists(fulpath))
            //{
            //    Directory.CreateDirectory(fulpath);
            //}

            //FolderName = path + TechnicianID + "\\" + year;
            //if (!System.IO.Directory.Exists(FolderName))
            //{
            //    Directory.CreateDirectory(FolderName);
            //}
            if (InProAttachment.PostedFile.FileName != "")
            {
                JobStatusUpdateMultipart model = new JobStatusUpdateMultipart();
                //string FName = Path.GetFileName(InProAttachment.PostedFile.FileName);
                //string sd = objConverter.GetAutoGenerateNo();

                //string filename = Convert.ToString(HttpContext.Current.Session["userid"]) + sd + FName;
                //string FLocation = Server.MapPath("../Documents/") + filename;

                //FolderName = path + TechnicianID + "\\" + year;

                //string total = FolderName + "\\" + filename;

                //InProAttachment.PostedFile.SaveAs(total);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] fileBytes = new byte[InProAttachment.PostedFile.InputStream.Length + 1];
                var fileContent = new StreamContent(InProAttachment.PostedFile.InputStream);
                form.Add(new StringContent(data), "data");
                form.Add(fileContent, "attachments", InProAttachment.PostedFile.FileName);
                var result = httpClient.PostAsync(BaseUrl + "CustomerJobStatus/WorkInProgressSubmit", form).Result;

                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(BaseUrl + "API/JobCustomer/WorkInProgressSubmit", data);
                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            }

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_ServiceWorkStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "SaveInProgress");
            cmd.Parameters.AddWithValue("@FSMID", oview.id);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            //txtHrs.Text = "0";
            //txtMin.Text = "0";
            txtRemarks.Text = "";

            if (oview.id != "0")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Update Successfully'); ShowInProgress();</script>");
            }




        }
        public class JobStatusUpdateMultipart
        {
            public string data { get; set; }
            public List<HttpPostedFileBase> attachments { get; set; }
        }
        protected void btnOnCompleate_Click1(object sender, EventArgs e)
        {
            int year = oDBEngine.GetDate().Year;
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select FSM_userid from tbl_master_user where user_id=" + Convert.ToString(CompddlTechnician.Value) + "");
            if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["FSM_userid"])))
            {
                TechnicianID = Convert.ToString(dst.Rows[0]["FSM_userid"]);
            }

            Completed objTechAssign = new Completed();
            objTechAssign.session_token = Convert.ToString("asfdgfhfxcbvcbnvgfjghkjghk");
            objTechAssign.user_id = Convert.ToString(TechnicianID);
            objTechAssign.job_id = Convert.ToString(hdnScheduleId.Value);
            objTechAssign.finish_date = Convert.ToString(CompEndDate.Date);
            objTechAssign.finish_time = Convert.ToString(hdnComEndTime.Value);
            objTechAssign.remarks = Convert.ToString(txtCompleteRemarks.Text);
            objTechAssign.date_time = Convert.ToString(DateTime.Now);
            objTechAssign.latitude = Convert.ToString("0");
            objTechAssign.longitude = Convert.ToString("0");
            objTechAssign.address = Convert.ToString("Address");
            objTechAssign.phone_no = Convert.ToString("0");

            string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];
            string data = JsonConvert.SerializeObject(objTechAssign);
            // WebClient client = new WebClient();
            //  client.Headers["Content-type"] = "application/json";
            //   client.Encoding = Encoding.UTF8;
            //  string json = client.UploadString(BaseUrl + "/API/JobCustomer/WorkOnCompletedSubmit", data);
            AssignJobOutPut oview = new AssignJobOutPut();
            //  oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);



            //path = HttpContext.Current.Server.MapPath(@"..\Documents\");
            //string fulpath = path + TechnicianID;
            //if (!System.IO.Directory.Exists(fulpath))
            //{
            //    Directory.CreateDirectory(fulpath);
            //}

            //FolderName = path + TechnicianID + "\\" + year;
            //if (!System.IO.Directory.Exists(FolderName))
            //{
            //    Directory.CreateDirectory(FolderName);
            //}
            if (CompAttachment.PostedFile.FileName != "")
            {
                JobStatusUpdateMultipart model = new JobStatusUpdateMultipart();
                //string FName = Path.GetFileName(CompAttachment.PostedFile.FileName);
                //string sd = objConverter.GetAutoGenerateNo();

                //string filename = Convert.ToString(HttpContext.Current.Session["userid"]) + sd + FName;
                //string FLocation = Server.MapPath("../Documents/") + filename;

                //FolderName = path + TechnicianID + "\\" + year;

                //string total = FolderName + "\\" + filename;

                //CompAttachment.PostedFile.SaveAs(total);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] fileBytes = new byte[CompAttachment.PostedFile.InputStream.Length + 1];
                var fileContent = new StreamContent(CompAttachment.PostedFile.InputStream);
                form.Add(new StringContent(data), "data");
                form.Add(fileContent, "attachments", CompAttachment.PostedFile.FileName);
                var result = httpClient.PostAsync(BaseUrl + "CustomerJobStatus/WorkOnCompletedSubmit", form).Result;

                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(BaseUrl + "API/JobCustomer/WorkOnCompletedSubmit", data);
                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            }


            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_ServiceWorkStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "SaveCompleted");
            cmd.Parameters.AddWithValue("@FSMID", oview.id);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();


            txtCompleteRemarks.Text = "";

            if (oview.id != "0")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Update Successfully'); ShowInProgress();</script>");
            }


        }
        protected void btnReAssign_Click(object sender, EventArgs e)
        {
            int year = oDBEngine.GetDate().Year;
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_ServiceSchedule", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", "SaveReAssign");
            cmd.Parameters.AddWithValue("@DETAILS_ID", hdnScheduleId.Value);
            cmd.Parameters.AddWithValue("@TECH_ID", ddlTechnician.Value);
            cmd.Parameters.AddWithValue("@SUB_TECHNICIANID", hdnSubTech.Value);
            cmd.Parameters.AddWithValue("@START_DATE", dtAssign.Date);
            cmd.Parameters.AddWithValue("@BRANCH_ID", cmbBranchfilter.Value);
            cmd.Parameters.AddWithValue("@USER_id", Convert.ToString(Session["userid"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            TechAssignUpdate objTechAssign = new TechAssignUpdate();
            objTechAssign.job_id = Convert.ToString(dt.Rows[0]["DETAILS_ID"]);
            objTechAssign.subtechnician_id = Convert.ToString(dt.Rows[0]["FSM_SUBUSERID"]);
            objTechAssign.technician_id = Convert.ToString(dt.Rows[0]["assigned_to"]);
            objTechAssign.date = Convert.ToDateTime(dt.Rows[0]["created_on"]);
            string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];
            string data = JsonConvert.SerializeObject(objTechAssign);
            AssignJobOutPut oview = new AssignJobOutPut();
            //  WebClient client = new WebClient();
            //  client.Headers["Content-type"] = "application/json";
            //  client.Encoding = Encoding.UTF8;
            //  string json = client.UploadString(BaseUrl + "/API/JobCustomer/UpdateAssignJob", data);


            //path = HttpContext.Current.Server.MapPath(@"..\Documents\");
            //string fulpath = path + TechnicianID;
            //if (!System.IO.Directory.Exists(fulpath))
            //{
            //    Directory.CreateDirectory(fulpath);
            //}

            //FolderName = path + TechnicianID + "\\" + year;
            //if (!System.IO.Directory.Exists(FolderName))
            //{
            //    Directory.CreateDirectory(FolderName);
            //}
            if (fileReSchedule.PostedFile.FileName != "")
            {
                JobStatusUpdateMultipart model = new JobStatusUpdateMultipart();
                //string FName = Path.GetFileName(fileCancelled.PostedFile.FileName);
                //string sd = objConverter.GetAutoGenerateNo();

                //string filename = Convert.ToString(HttpContext.Current.Session["userid"]) + sd + FName;
                //string FLocation = Server.MapPath("../Documents/") + filename;

                //FolderName = path + TechnicianID + "\\" + year;

                //string total = FolderName + "\\" + filename;

                //fileCancelled.PostedFile.SaveAs(total);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] fileBytes = new byte[fileCancelled.PostedFile.InputStream.Length + 1];
                var fileContent = new StreamContent(fileCancelled.PostedFile.InputStream);
                form.Add(new StringContent(data), "data");
                form.Add(fileContent, "attachments", fileCancelled.PostedFile.FileName);
                var result = httpClient.PostAsync(BaseUrl + "CustomerJobStatus/UpdateAssignJob", form).Result;

                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(BaseUrl + "API/JobCustomer/UpdateAssignJob", data);
                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Update Successfully'); ShowHold();</script>");


        }
        protected void btnOnHold_Click(object sender, EventArgs e)
        {
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select FSM_userid from tbl_master_user where user_id=" + Convert.ToString(OnHoldddlTechnician.Value) + "");
            if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["FSM_userid"])))
            {
                TechnicianID = Convert.ToString(dst.Rows[0]["FSM_userid"]);
            }

            string date = Convert.ToString(DateTime.Now);
            string[] time = date.Split(' ');

            OnHold objTechAssign = new OnHold();
            objTechAssign.session_token = Convert.ToString("asfdgfhfxcbvcbnvgfjghkjghk");
            objTechAssign.user_id = Convert.ToString(TechnicianID);
            objTechAssign.job_id = Convert.ToString(hdnScheduleId.Value);
            objTechAssign.hold_date = Convert.ToString(OnHoldHoldUpto.Date);
            objTechAssign.hold_time = Convert.ToString("00:00 AM");
            objTechAssign.reason_hold = Convert.ToString("Reason");
            objTechAssign.remarks = Convert.ToString(txtHoldRemarks.Text);
            objTechAssign.date_time = Convert.ToString(DateTime.Now);
            objTechAssign.latitude = Convert.ToString("0");
            objTechAssign.longitude = Convert.ToString("0");
            objTechAssign.address = Convert.ToString("Address");


            string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];
            string data = JsonConvert.SerializeObject(objTechAssign);
            // WebClient client = new WebClient();
            // client.Headers["Content-type"] = "application/json";
            // client.Encoding = Encoding.UTF8;
            // string json = client.UploadString(BaseUrl + "/API/JobCustomer/WorkOnHoldSubmit", data);

            AssignJobOutPut oview = new AssignJobOutPut();
            // oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            //int year = oDBEngine.GetDate().Year;
            //path = HttpContext.Current.Server.MapPath(@"..\Documents\");
            //string fulpath = path + TechnicianID;
            //if (!System.IO.Directory.Exists(fulpath))
            //{
            //    Directory.CreateDirectory(fulpath);
            //}

            //FolderName = path + TechnicianID + "\\" + year;
            //if (!System.IO.Directory.Exists(FolderName))
            //{
            //    Directory.CreateDirectory(FolderName);
            //}
            if (fileOnHold.PostedFile.FileName != "")
            {
                JobStatusUpdateMultipart model = new JobStatusUpdateMultipart();
                //string FName = Path.GetFileName(fileOnHold.PostedFile.FileName);
                //string sd = objConverter.GetAutoGenerateNo();

                //string filename = Convert.ToString(HttpContext.Current.Session["userid"]) + sd + FName;
                //string FLocation = Server.MapPath("../Documents/") + filename;

                //FolderName = path + TechnicianID + "\\" + year;

                //string total = FolderName + "\\" + filename;

                //fileOnHold.PostedFile.SaveAs(total);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] fileBytes = new byte[fileOnHold.PostedFile.InputStream.Length + 1];
                var fileContent = new StreamContent(fileOnHold.PostedFile.InputStream);
                form.Add(new StringContent(data), "data");
                form.Add(fileContent, "attachments", fileOnHold.PostedFile.FileName);
                var result = httpClient.PostAsync(BaseUrl + "CustomerJobStatus/WorkOnHoldSubmit", form).Result;

                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(BaseUrl + "API/JobCustomer/WorkOnHoldSubmit", data);
                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            }
            if (oview.id != "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceWorkStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveOnHold");
                cmd.Parameters.AddWithValue("@FSMID", oview.id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                cmd.Dispose();
                con.Dispose();

                txtHoldRemarks.Text = "";


                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Update Successfully'); ShowInProgress();</script>");

            }
        }
        protected void btnCancelled_Click(object sender, EventArgs e)
        {
            int year = oDBEngine.GetDate().Year;
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select FSM_userid from tbl_master_user where user_id=" + Convert.ToString(CanddlTechnician.Value) + "");
            if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["FSM_userid"])))
            {
                TechnicianID = Convert.ToString(dst.Rows[0]["FSM_userid"]);
            }

            string date = Convert.ToString(DateTime.Now);
            string[] time = date.Split(' ');


            Cancelled objTechAssign = new Cancelled();
            objTechAssign.session_token = Convert.ToString("asfdgfhfxcbvcbnvgfjghkjghk");
            objTechAssign.user_id = Convert.ToString(TechnicianID);
            objTechAssign.job_id = Convert.ToString(hdnScheduleId.Value);
            objTechAssign.date = Convert.ToString(CanScheduleDate.Date);
            objTechAssign.time = Convert.ToString("00:00 AM");
            objTechAssign.cancel_reason = Convert.ToString(txtReason.Text);
            objTechAssign.remarks = Convert.ToString("");
            objTechAssign.date_time = Convert.ToString(DateTime.Now);
            objTechAssign.latitude = Convert.ToString("0");
            objTechAssign.longitude = Convert.ToString("0");
            objTechAssign.address = Convert.ToString("Address");
            objTechAssign.cancelled_by = Convert.ToString(HttpContext.Current.Session["userid"]);
            objTechAssign.user = Convert.ToString(CancelledUser.Text);

            string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];
            string data = JsonConvert.SerializeObject(objTechAssign);


            AssignJobOutPut oview = new AssignJobOutPut();



            if (fileCancelled.PostedFile.FileName != "")
            {
                JobStatusUpdateMultipart model = new JobStatusUpdateMultipart();

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] fileBytes = new byte[fileCancelled.PostedFile.InputStream.Length + 1];
                var fileContent = new StreamContent(fileCancelled.PostedFile.InputStream);
                form.Add(new StringContent(data), "data");
                form.Add(fileContent, "attachments", fileCancelled.PostedFile.FileName);
                var result = httpClient.PostAsync(BaseUrl + "CustomerJobStatus/WorkCancelledSubmit", form).Result;

                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(BaseUrl + "API/JobCustomer/WorkCancelledSubmit", data);
                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            }

            if (oview.id != "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceWorkStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveCancelled");
                cmd.Parameters.AddWithValue("@FSMID", oview.id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                cmd.Dispose();
                con.Dispose();

                txtReason.Text = "";

                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Update Successfully'); ShowInProgress();</script>");
            }
        }

        protected void ReleaseHoldPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "ShowData")
            {
                string id = hdnScheduleId.Value;
                DataTable dst = new DataTable();
                DBEngine objDB = new DBEngine();
                dst = objDB.GetDataTable("select * from TBL_SCHEDULEDETAILS where details_id='" + Convert.ToString(id) + "'");

                if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"])))
                {
                    ReleaseHoldddlTechnician.Value = Convert.ToString(dst.Rows[0]["ASSIGNED_TECHNICIAN"]);
                }
                DataTable dst1 = new DataTable();
                dst1 = objDB.GetDataTable("select top 1 * from TBL_AssignScheduleStatus  where SCH_ID='" + Convert.ToString(hdnScheduleId.Value) + "' and STATUS=7 order by ID desc ");

                if (dst1.Rows.Count > 0 && dst1 != null)
                {
                    ReleaseHoldHoldUpto.Date = Convert.ToDateTime(dst1.Rows[0]["DATE"]);
                    txtReleaseHoldRemarks.Text = Convert.ToString(dst1.Rows[0]["REMARKS"]);

                    DataTable dst2 = new DataTable();
                    dst2 = objDB.GetDataTable("select * from TBL_AssignScheduleStatusAttachment  where AssignScheduleStatusId='" + Convert.ToString(dst1.Rows[0]["ID"]) + "'");

                    if (dst2.Rows.Count > 0 && dst2 != null)
                    {
                        string VersionBaseURL = ConfigurationManager.AppSettings["VersionBaseURL"];
                        if (!string.IsNullOrEmpty(Convert.ToString(dst2.Rows[0]["Attachment_Name"])))
                        {
                            String attachmentFile = VersionBaseURL + "/CommonFolder/ScheduleAttachment/" + Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            filesrc = VersionBaseURL + "/CommonFolder/ScheduleAttachment/";
                            filedoc = Convert.ToString(dst2.Rows[0]["Attachment_Name"]);

                            DivFileUnHold.Attributes.Add("class", "hide");
                            DivViewUnhold.Visible = true;
                            hddnReleaseHoldFile.Value = filedoc;
                        }
                    }
                    else
                    {
                        DivFileUnHold.Attributes.Add("class", "show");
                    }
                }

            }
        }

        protected void btnReleaseHold_Click(object sender, EventArgs e)
        {
            DataTable dst = new DataTable();
            DBEngine objDB = new DBEngine();
            dst = objDB.GetDataTable("select FSM_userid from tbl_master_user where user_id=" + Convert.ToString(ReleaseHoldddlTechnician.Value) + "");
            if (!string.IsNullOrEmpty(Convert.ToString(dst.Rows[0]["FSM_userid"])))
            {
                TechnicianID = Convert.ToString(dst.Rows[0]["FSM_userid"]);
            }

            string date = Convert.ToString(DateTime.Now);
            string[] time = date.Split(' ');

            UnHold objTechAssign = new UnHold();
            objTechAssign.session_token = Convert.ToString("asfdgfhfxcbvcbnvgfjghkjghk");
            objTechAssign.user_id = Convert.ToString(TechnicianID);
            objTechAssign.job_id = Convert.ToString(hdnScheduleId.Value);
            objTechAssign.unhold_date = Convert.ToString(ReleaseHoldHoldUpto.Date);
            objTechAssign.unhold_time = Convert.ToString("00:00 AM");
            objTechAssign.reason_unhold = Convert.ToString("Reason");
            objTechAssign.remarks = Convert.ToString(txtReleaseHoldRemarks.Text);
            objTechAssign.date_time = Convert.ToString(DateTime.Now);
            objTechAssign.latitude = Convert.ToString("0");
            objTechAssign.longitude = Convert.ToString("0");
            objTechAssign.address = Convert.ToString("Address");


            string BaseUrl = ConfigurationManager.AppSettings["BaseUrlService"];
            string data = JsonConvert.SerializeObject(objTechAssign);

            AssignJobOutPut oview = new AssignJobOutPut();


            if (FileReleaseHold.PostedFile.FileName != "")
            {
                JobStatusUpdateMultipart model = new JobStatusUpdateMultipart();

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                byte[] fileBytes = new byte[FileReleaseHold.PostedFile.InputStream.Length + 1];
                var fileContent = new StreamContent(FileReleaseHold.PostedFile.InputStream);
                form.Add(new StringContent(data), "data");
                form.Add(fileContent, "attachments", FileReleaseHold.PostedFile.FileName);
                var result = httpClient.PostAsync(BaseUrl + "CustomerJobStatus/SubmitWorkUnhold", form).Result;

                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(result.Content.ReadAsStringAsync().Result);
            }
            else
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(BaseUrl + "API/JobCustomer/SubmitWorkUnhold", data);
                oview = JsonConvert.DeserializeObject<AssignJobOutPut>(json);
            }
            if (oview.id != "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ServiceWorkStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveOnHold");
                cmd.Parameters.AddWithValue("@FSMID", oview.id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                cmd.Dispose();
                con.Dispose();

                txtReleaseHoldRemarks.Text = "";


                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Update Successfully'); ShowHold();</script>");
            }


        }
    }
}