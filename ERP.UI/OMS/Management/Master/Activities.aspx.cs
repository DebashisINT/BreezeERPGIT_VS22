using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using DataAccessLayer;
using ERP.Models;
using ClsDropDownlistNameSpace;
using System.Web.Script.Services;
using System.Net.Mail;
using System.Net;

namespace ERP.OMS.Management.Master
{
    public partial class Activities : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        TotalvaluationClass objvaluation = new TotalvaluationClass();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Activities.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["SessionActivityProduct"] = null;
                this.cmbActivity.Attributes.Add("onchange", "return ddlICRChange();");
                this.cmbPriority.Attributes.Add("onchange", "return ddlPriorityChange();");
                this.cmbType.Attributes.Add("onchange", "return ddlTypeChange();");

                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                //RptHeading.Text = "Activities";
                //GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                //CompName.Text = GridHeader;

                //GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                //CompAdd.Text = GridHeader;

                //GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                //CompOth.Text = GridHeader;

                //GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                //CompPh.Text = GridHeader;

                //GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                //CompAccPrd.Text = GridHeader;

                Session["SI_ComponentData"] = null;
                Session["IsActivityFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;

                string[,] DataActivity = oDBEngine.GetFieldValue("Lead_Activity", "Id, Lead_ActivityName", null, 2, "Lead_ActivityName");
                oclsDropDownList.AddDataToDropDownList(DataActivity, cmbActivity);
                ListItem LST_ActivityName = new ListItem("--Select--", "0");
                cmbActivity.Items.Insert(0, LST_ActivityName);
                cmbActivity.SelectedIndex = 0;

                string[,] DataSalesActivityAssignTo = oDBEngine.GetFieldValue("tbl_master_user", "user_id, user_name", "user_inactive='N'", 2, "user_name");
                oclsDropDownList.AddDataToDropDownList(DataSalesActivityAssignTo, cmbSalesActivityAssignTo);
                ListItem LST_ActivityAssignTo = new ListItem("--Select--", "0");
                cmbSalesActivityAssignTo.Items.Insert(0, LST_ActivityAssignTo);
                cmbSalesActivityAssignTo.SelectedIndex = 0;

                //string[,] DataDuration = oDBEngine.GetFieldValue("Lead_Duration", "Id, DurationName", null, 2, "Id");
                //oclsDropDownList.AddDataToDropDownList(DataDuration, cmbDuration);
                //ListItem LST_Duration = new ListItem("--Select--", "0");
                //cmbDuration.Items.Insert(0, LST_Duration);
                //cmbDuration.SelectedIndex = 0;

                string[,] DataPriority = oDBEngine.GetFieldValue("Lead_Priority", "Id, PriorityName", null, 2, "PriorityName");
                oclsDropDownList.AddDataToDropDownList(DataPriority, cmbPriority);
                ListItem LST_Priority = new ListItem("--Select--", "0");
                cmbPriority.Items.Insert(0, LST_Priority);
                cmbPriority.SelectedIndex = 0;


                string[,] DataContactType = oDBEngine.GetFieldValue("tbl_master_contactType", "cnt_prefix, cnttpy_contactType", "cnt_prefix in('LD','CL','DV','AG','EM','TR','RA','DI')", 2, "cnttpy_contactType");
                oclsDropDownList.AddDataToDropDownList(DataContactType, cmbContactType);
                ListItem LST_ContactType = new ListItem("--Select--", "0");
                cmbContactType.Items.Insert(0, LST_ContactType);
                cmbContactType.SelectedIndex = 0;


                #region time
                DtxtDue.TimeSectionProperties.Visible = true;
                DtxtDue.UseMaskBehavior = true;
                DtxtDue.EditFormatString = "dd-MM-yyyy hh:mm tt";

                dtActivityDate.TimeSectionProperties.Visible = true;
                dtActivityDate.UseMaskBehavior = true;
                dtActivityDate.EditFormatString = "dd-MM-yyyy hh:mm tt";
                #endregion

                DtxtDue.MinDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            }

        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }



        #region Export Valuation Summary
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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

        public void bindexport(int Filter)
        {
            string filename = "Activity";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Activity Listing" + Environment.NewLine;

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion


        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Load")
            {
                string IsActivityFilter = Convert.ToString(hfActivityFilter.Value);
                Session["IsActivityFilter"] = IsActivityFilter;
            }
            //strCustList = hdnUserId.Value;
        }


        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #region LinQ
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
       {
            e.KeyExpression = "RID";

            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            int Userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            DateTime Nowtime=DateTime.Now;

            // Mantis Issue 24545, 24058
            BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
            string ShowActivitiesDataForAllUsers = ComBL.GetSystemSettingsResult("ShowActivitiesDataForAllUsers");

            if (ShowActivitiesDataForAllUsers.ToUpper() == "NO")
            {
                // End of Mantis Issue 24545, 24058
                if (hfActivityFilter.Value == "TodayFollowup")
                {
                    var q = from d in dc.v_ActivityLists
                            where d.asFollwDate.Date == Nowtime.Date && (d.Assign_To == Userid || d.Created_by == Userid) 
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                else if (hfActivityFilter.Value == "YesterdayFollowup")
                {
                    var q = from d in dc.v_ActivityLists
                            where d.asFollwDate.Date < Nowtime.Date && (d.Assign_To == Userid || d.Created_by == Userid)
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                else if (Convert.ToString(Session["IsActivityFilter"]) == "Y" && hfActivityFilter.Value != "TodayFollowup" && hfActivityFilter.Value != "YesterdayFollowup")
                {
                    var q = from d in dc.v_ActivityLists
                            where d.Assign_To == Userid || d.Created_by == Userid
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    var q = from d in dc.v_ActivityLists
                            where Convert.ToString(d.RID) == "0"
                            &&( d.Assign_To == Userid || d.Created_by == Userid)
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                // Mantis Issue 24545, 24058
            }
            else
            {
                if (hfActivityFilter.Value == "TodayFollowup")
                {
                    var q = from d in dc.v_ActivityLists
                            where d.asFollwDate.Date == Nowtime.Date 
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                else if (hfActivityFilter.Value == "YesterdayFollowup")
                {
                    var q = from d in dc.v_ActivityLists
                            where d.asFollwDate.Date < Nowtime.Date 
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                else if (Convert.ToString(Session["IsActivityFilter"]) == "Y" && hfActivityFilter.Value != "TodayFollowup" && hfActivityFilter.Value != "YesterdayFollowup")
                {
                    var q = from d in dc.v_ActivityLists
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    var q = from d in dc.v_ActivityLists
                            where Convert.ToString(d.RID) == "0"
                            orderby d.RID
                            select d;
                    e.QueryableSource = q;
                }
            }
           // End of Mantis Issue 24545, 24058
        }
        #endregion

        #region ADD

        protected void CallbackPanelActivity_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];

            if (strSplitCommand == "Add")
            {
                List<ActivityProductDetailsBL> finalResultblankprod = new List<ActivityProductDetailsBL>();
                CallbackPanelActivity.JSProperties["cpActivityDate"] = "";
                CallbackPanelActivity.JSProperties["cpActivityFor"] = "";
                CallbackPanelActivity.JSProperties["cpContactName"] = "";
                CallbackPanelActivity.JSProperties["cpContactID"] = "";
                CallbackPanelActivity.JSProperties["cpActivity"] = "";
                CallbackPanelActivity.JSProperties["cpType"] = "";
                CallbackPanelActivity.JSProperties["cpSubject"] = "";
                CallbackPanelActivity.JSProperties["cpDetails"] = "";
                CallbackPanelActivity.JSProperties["cpDuration"] = "";
                CallbackPanelActivity.JSProperties["cpPriority"] = "";
                CallbackPanelActivity.JSProperties["cpDueDate"] = "";
                // Rev Mantis Issue 22801 [Duration_New]
                CallbackPanelActivity.JSProperties["cpDuration_New"] = "00:00:00";
                // End of Rev Mantis Issue 22801
                CallbackPanelActivity.JSProperties["cpAssignto"] = HttpContext.Current.Session["userid"].ToString();
                CallbackPanelActivity.JSProperties["cpProductDetails"] = finalResultblankprod;
                CallbackPanelActivity.JSProperties["cpStatusActivity"] = "Add";
            }
            else if (strSplitCommand == "Activity_Type")
            {
                int leadactivityid = Convert.ToInt32(e.Parameter.Split('~')[1]);
                string[,] DataActivityType = oDBEngine.GetFieldValue("Lead_ActivityType", "Id, Lead_ActivityTypeName", "LeadActivityId='" + leadactivityid + "'", 2, "Lead_ActivityTypeName");
                oclsDropDownList.AddDataToDropDownList(DataActivityType, cmbType);
                ListItem LST_ActivityType = new ListItem("--Select--", "0");
                cmbType.Items.Insert(0, LST_ActivityType);
                cmbType.SelectedIndex = 0;
                CallbackPanelActivity.JSProperties["cpStatusActivity"] = "Activity_Type";
            }
            else if (strSplitCommand == "Save")
            {
                string leadactivityid = Convert.ToString(e.Parameter.Split('~')[1]);
                int activity = Convert.ToInt32(e.Parameter.Split('~')[2]);
                int activity_type = Convert.ToInt32(e.Parameter.Split('~')[3]);
                string subject = Convert.ToString(e.Parameter.Split('~')[4]);
                string details = Convert.ToString(e.Parameter.Split('~')[5]);
                int assignto = Convert.ToInt32(e.Parameter.Split('~')[6]);
                int duration = Convert.ToInt32(e.Parameter.Split('~')[7]);
                int priority = Convert.ToInt32(e.Parameter.Split('~')[8]);
                string contacttype = Convert.ToString(e.Parameter.Split('~')[9]);
                // Rev Mantis Issue 22801
                string duration_new = Convert.ToString(e.Parameter.Split('~')[12]);
                // End of Rev Mantis Issue 22801
                int emailid = 0;
                int smsid = 0;
                if (e.Parameter.Split('~')[10] != "")
                {
                    emailid = Convert.ToInt32(e.Parameter.Split('~')[10]);
                }
                else
                {
                    emailid = 0;
                }
                if (e.Parameter.Split('~')[11] != "")
                {
                    smsid = Convert.ToInt32(e.Parameter.Split('~')[11]);
                }
                else
                {
                    smsid = 0;
                }

                DataTable dt_activityproducts = new DataTable();
                dt_activityproducts.Columns.Add("Id", typeof(string));
                dt_activityproducts.Columns.Add("ProdId", typeof(Int32));
                dt_activityproducts.Columns.Add("Act_Prod_Qty", typeof(Decimal));
                dt_activityproducts.Columns.Add("Act_Prod_Rate", typeof(Decimal));
                dt_activityproducts.Columns.Add("Act_Prod_Remarks", typeof(String));

                if (HttpContext.Current.Session["SessionActivityProduct"] != null)
                {
                    List<ActivityProductDetailsBL> obj = new List<ActivityProductDetailsBL>();
                    obj = (List<ActivityProductDetailsBL>)HttpContext.Current.Session["SessionActivityProduct"];
                    foreach (var item in obj)
                    {
                        dt_activityproducts.Rows.Add(item.guid, item.ProductId, item.Quantity, item.Rate, item.Remarks);
                    }
                }



                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ACTION_TYPE", "SAVE");
                cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", activity);
                cmd.Parameters.AddWithValue("@TYPEID", activity_type);
                cmd.Parameters.AddWithValue("@LEADSUBJECT", subject);
                cmd.Parameters.AddWithValue("@LEADDETAILS", details);
                cmd.Parameters.AddWithValue("@ASSIGNTO", assignto);
                cmd.Parameters.AddWithValue("@DURATIONID", duration);
                cmd.Parameters.AddWithValue("@PRIORITYID", priority);
                cmd.Parameters.AddWithValue("@DUEDATE", DtxtDue.Date);
                cmd.Parameters.AddWithValue("@ACTIVITYDATE", dtActivityDate.Date);
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", leadactivityid);
                cmd.Parameters.AddWithValue("@MODULENAME", "Activity");
                cmd.Parameters.AddWithValue("@CONTACTTYPE", contacttype);
                cmd.Parameters.AddWithValue("@ActivityProducts", dt_activityproducts);
                cmd.Parameters.AddWithValue("@EMAILID", emailid);
                cmd.Parameters.AddWithValue("@SMSID", smsid);

                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));
                // Rev Mantis Issue 22801
                cmd.Parameters.AddWithValue("@DURATION_NEW", duration_new);
                // End of Rev Mantis Issue 22801

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());

                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());
                CallbackPanelActivity.JSProperties["cpStatusActivity"] = "Save";
            }
            else if (strSplitCommand == "Edit")
            {
                int _id = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataSet dtchkassign = new DataSet();

                int OutputId = 0;
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", "EDIT");
                cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", _id);
                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(Session["userid"]));

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dtchkassign);
                cmd.Dispose();
                con.Dispose();


                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                int leadactivityid = Convert.ToInt32(e.Parameter.Split('~')[1]);
                string[,] DataActivityType = oDBEngine.GetFieldValue("Lead_ActivityType", "Id, Lead_ActivityTypeName", "LeadActivityId='" + Convert.ToInt32(dtchkassign.Tables[0].Rows[0][5]) + "'", 2, "Lead_ActivityTypeName");
                oclsDropDownList.AddDataToDropDownList(DataActivityType, cmbType);
                ListItem LST_ActivityType = new ListItem("--Select--", "0");
                cmbType.Items.Insert(0, LST_ActivityType);
                cmbType.SelectedIndex = 0;


                if (dtchkassign.Tables[0].Rows.Count > 0)
                {
                    CallbackPanelActivity.JSProperties["cpActivityDate"] = Convert.ToDateTime(dtchkassign.Tables[0].Rows[0][3]);
                    CallbackPanelActivity.JSProperties["cpActivityFor"] = dtchkassign.Tables[0].Rows[0][4].ToString();
                    CallbackPanelActivity.JSProperties["cpContactName"] = dtchkassign.Tables[0].Rows[0][2].ToString();
                    CallbackPanelActivity.JSProperties["cpContactID"] = dtchkassign.Tables[0].Rows[0][1].ToString();
                    CallbackPanelActivity.JSProperties["cpActivity"] = dtchkassign.Tables[0].Rows[0][5].ToString();
                    CallbackPanelActivity.JSProperties["cpType"] = dtchkassign.Tables[0].Rows[0][7].ToString();
                    CallbackPanelActivity.JSProperties["cpSubject"] = dtchkassign.Tables[0].Rows[0][9].ToString();
                    CallbackPanelActivity.JSProperties["cpDetails"] = dtchkassign.Tables[0].Rows[0][10].ToString();
                    CallbackPanelActivity.JSProperties["cpDuration"] = dtchkassign.Tables[0].Rows[0][13].ToString();
                    CallbackPanelActivity.JSProperties["cpPriority"] = dtchkassign.Tables[0].Rows[0][11].ToString();
                    // Rev Mantis Issue 22801 [Duration_New]
                    CallbackPanelActivity.JSProperties["cpDuration_New"] = dtchkassign.Tables[0].Rows[0][19].ToString();
                    // End of Rev Mantis Issue 22801
                    if (dtchkassign.Tables[0].Rows[0][17] != System.DBNull.Value)
                        CallbackPanelActivity.JSProperties["cpDueDate"] = Convert.ToDateTime(dtchkassign.Tables[0].Rows[0][17]);

                    if (Convert.ToString(dtchkassign.Tables[0].Rows[0][15]) != "")
                    {
                        CallbackPanelActivity.JSProperties["cpAssignto"] = dtchkassign.Tables[0].Rows[0][15].ToString();
                    }
                    if (dtchkassign.Tables[1].Rows.Count > 0)
                    {
                        List<ActivityProductDetailsBL> finalResult = DbHelpers.ToModelList<ActivityProductDetailsBL>(dtchkassign.Tables[1]);
                        CallbackPanelActivity.JSProperties["cpProductDetails"] = finalResult;
                    }
                }
                else
                {
                    List<ActivityProductDetailsBL> finalResultblankprod = new List<ActivityProductDetailsBL>();

                    CallbackPanelActivity.JSProperties["cpActivityDate"] = "";
                    CallbackPanelActivity.JSProperties["cpActivityFor"] = "";
                    CallbackPanelActivity.JSProperties["cpContactName"] = "";
                    CallbackPanelActivity.JSProperties["cpContactID"] = "";
                    CallbackPanelActivity.JSProperties["cpActivity"] = "";
                    CallbackPanelActivity.JSProperties["cpType"] = "";
                    CallbackPanelActivity.JSProperties["cpSubject"] = "";
                    CallbackPanelActivity.JSProperties["cpDetails"] = "";
                    CallbackPanelActivity.JSProperties["cpDuration"] = "";
                    CallbackPanelActivity.JSProperties["cpPriority"] = "";
                    CallbackPanelActivity.JSProperties["cpDueDate"] = "";
                    // Rev Mantis Issue 22801 [Duration_New]
                    CallbackPanelActivity.JSProperties["cpDuration_New"] = "00:00:00";
                    // End of Rev Mantis Issue 22801
                    CallbackPanelActivity.JSProperties["cpAssignto"] = HttpContext.Current.Session["userid"].ToString();
                    CallbackPanelActivity.JSProperties["cpProductDetails"] = finalResultblankprod;
                }


                CallbackPanelActivity.JSProperties["cpStatusActivity"] = "Edit";

            }
            else if (strSplitCommand == "Delete")
            {
                int _id = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dtchkassign = new DataTable();

                int OutputId = 0;
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", "DELETE");
                cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", _id);

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dtchkassign);
                cmd.Dispose();
                con.Dispose();

                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                CallbackPanelActivity.JSProperties["cpStatusActivity"] = "Delete";
            }

            else if (strSplitCommand == "Cancel")
            {
                int _id = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dtchkassign = new DataTable();

                int OutputId = 0;
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", "Cancel");
                cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", _id);

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dtchkassign);
                cmd.Dispose();
                con.Dispose();

                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                CallbackPanelActivity.JSProperties["cpStatusActivity"] = "Cancel";
            }


        }

        [WebMethod]
        public static string SaveActivityProductDetails(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ActivityProductDetailsBL> finalResult = jsSerializer.Deserialize<List<ActivityProductDetailsBL>>(list);
            HttpContext.Current.Session["SessionActivityProduct"] = finalResult;

            return null;
        }

        [WebMethod]
        public static string ButtonCountShow()
        {
            string today = "0";
            string Yesterday = "0";
            string All = "0";
            DateTime nowdate=DateTime.Now;
            DBEngine odfng = new DBEngine();

            DataTable dtstoday = new DataTable();
            DataTable dtsYesterday = new DataTable();
            DataTable dtsAll = new DataTable();
            dtstoday = odfng.GetDataTable("select COUNT(RID) as TodayFollow from v_ActivityList where cast(asFollwDate as date)=cast(GETDATE() as date)");
            dtsYesterday = odfng.GetDataTable("select COUNT(RID) as YesterdayFollow from v_ActivityList where cast(asFollwDate as date)<cast(GETDATE() as date)");
            dtsAll = odfng.GetDataTable("select COUNT(RID) as AllFollow from v_ActivityList");

            if (dtstoday != null && dtstoday.Rows.Count > 0)
            {
                today = Convert.ToString(dtstoday.Rows[0]["TodayFollow"]);
            }
            if (dtsYesterday != null && dtsYesterday.Rows.Count > 0)
            {
                Yesterday = Convert.ToString(dtsYesterday.Rows[0]["YesterdayFollow"]);
            }
            if (dtsAll != null && dtsAll.Rows.Count > 0)
            {
                All = Convert.ToString(dtsAll.Rows[0]["AllFollow"]);
            }
            return today + '~' + Yesterday + '~' + All;
        }
        //[WebMethod]
        //public static string ButtonYesterdayCountShow()
        //{
        //    string cls = "0";
        //    DateTime nowdate = DateTime.Now;
        //    DBEngine odfng = new DBEngine();
        //    DataTable dts = new DataTable();
        //    dts = odfng.GetDataTable("select COUNT(RID) as TodayFollow from v_ActivityList where cast(asFollwDate as date)<cast(GETDATE() as date)");


        //    if (dts != null && dts.Rows.Count > 0)
        //    {
        //        cls = Convert.ToString(dts.Rows[0]["TodayFollow"]);
        //    }
        //    return cls;
        //}

        [WebMethod]
        public static object GetActivityProductDetails(int id)
        {
            // SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();

            int _id = id;
            DataTable dtchkassign = new DataTable();

            int OutputId = 0;
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_LEAD_SALESACTIVITY", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ACTION_TYPE", "EDITPROD");
            cmd.Parameters.AddWithValue("@LEAD_ACTIVITYID", _id);

            SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
            output.Direction = ParameterDirection.Output;
            SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
            outputText.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(output);
            cmd.Parameters.Add(outputText);

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dtchkassign);
            cmd.Dispose();
            con.Dispose();

            List<ActivityProductDetailsBL> finalResult = DbHelpers.ToModelList<ActivityProductDetailsBL>(dtchkassign);
            return finalResult;

        }
        public class ActivityProductDetailsBL
        {
            public string guid { get; set; }
            public int ActivityId { get; set; }
            public string Lead_Entity_id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
            public decimal Rate { get; set; }
            public string Remarks { get; set; }
        }

        #endregion

        #region  Activity Email
        protected void CallbackPanelEmail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string lead_entityid = e.Parameter.Split('~')[1];
            if (strSplitCommand == "Load")
            {
                string[,] DataEmail = oDBEngine.GetFieldValue("tbl_master_email", "eml_email", "eml_cntId='" + lead_entityid + "'", 1, "eml_email");
                CallbackPanelEmail.JSProperties["cpStatusActivity"] = "Load";
                if (DataEmail[0, 0]!="n")
                {
                    CallbackPanelEmail.JSProperties["cpEmail"] = DataEmail[0, 0];
                }
                else
                {
                    CallbackPanelEmail.JSProperties["cpEmail"] = "";
                }
                
            }


        }
        [WebMethod(EnableSession = true)]
        //public static object SaveEmails(string ActionType, string Contactid, string ToEmail, string CCEmail, string BCCEmail, string Subject, string EmailBody, HttpContext context)
        public static object SaveEmails(string ActionType, string Contactid, string ToEmail, string CCEmail, string BCCEmail, string Subject, string EmailBody)
        {
            try
            {
                int OutputId = 0;
                string ErrorMsg = "";
                DataTable dtchkassign = new DataTable();

                //HttpFileCollection files = null;
                //if (context.Request.Files.Count > 0)
                //{
                //    files = context.Request.Files;
                //}
                #region  Email Saved
                //Email Saved
                HtmlString theEnvelopePlease = new HtmlString(EmailBody);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_EMAIL_ACTIVITY", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", ActionType);
                cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", Contactid);
                cmd.Parameters.AddWithValue("@EMAILTO", ToEmail);
                cmd.Parameters.AddWithValue("@EMAILCC", CCEmail);
                cmd.Parameters.AddWithValue("@EMAILBCC", BCCEmail);
                cmd.Parameters.AddWithValue("@EMAILSUBJECT", Subject);
                cmd.Parameters.AddWithValue("@EMAILDETAILS", EmailBody);
                cmd.Parameters.AddWithValue("@EMAIL_STATUS", "Success");
                cmd.Parameters.AddWithValue("@MODULENAME", "Activity");
                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()));

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dtchkassign);
                cmd.Dispose();
                con.Dispose();

                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                //End of Email Saved
                #endregion

                #region Email Sent
                try
                {
                    string retemail = EmailSent(ToEmail, CCEmail, BCCEmail, Subject, EmailBody);
                }
                catch
                {
                    ErrorMsg = "Failed";
                }

                #endregion Email Sent

                #region Update email
                //Update recently added document with error statement and email no if error occure.
                if (ErrorMsg != "")
                {
                    ErrorMsg = ErrorMsg + "  Failed";
                    SqlConnection conemail = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmdemail = new SqlCommand("PRC_EMAIL_ACTIVITY", conemail);

                    cmdemail.CommandType = CommandType.StoredProcedure;
                    cmdemail.Parameters.AddWithValue("@ACTION_TYPE", "UPDATE");
                    cmdemail.Parameters.AddWithValue("@EMAIL_STATUS", ErrorMsg);
                    cmdemail.Parameters.AddWithValue("@EMAILID", OutputId);

                    SqlParameter outputsms = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    outputsms.Direction = ParameterDirection.Output;
                    SqlParameter outputTextsms = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputTextsms.Direction = ParameterDirection.Output;
                    cmdemail.Parameters.Add(outputsms);
                    cmdemail.Parameters.Add(outputTextsms);

                    cmdemail.CommandTimeout = 0;
                    SqlDataAdapter Adapsms = new SqlDataAdapter();
                    Adapsms.SelectCommand = cmdemail;
                    Adapsms.Fill(dtchkassign);
                    cmdemail.Dispose();
                    conemail.Dispose();
                }

                // End of Update
                #endregion
                return OutputId;
            }
            catch (Exception EX)
            {
                return null;
            }
        }

        public static string EmailSent(string ToEmail, string CCEmail, string BCCEmail, string Subject, string EmailBody)
        {
            string strreturnmsgemail = "";
            try
            {

                //Email sent

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtFromEmailDet = new DataTable();
                dtFromEmailDet = oDBEngine.GetDataTable("select top(1) EmailAccounts_EmailID,EmailAccounts_Password,EmailAccounts_FromName,LTRIM(RTRIM(EmailAccounts_SMTP)) AS EmailAccounts_SMTP,LTRIM(RTRIM(EmailAccounts_SMTPPort)) AS EmailAccounts_SMTPPort from Config_EmailAccounts where EmailAccounts_InUse='Y'");
                //var Email = dtFromEmailDet.Rows[0][0].ToString();
                //var Password = dtFromEmailDet.Rows[0][1].ToString();
                var Email = "subhra.mukherjee@indusnet.co.in";
                var Password = "subhra@12345";
                var FromWhere = dtFromEmailDet.Rows[0][2].ToString();
                var OutgoingSMTPHost = dtFromEmailDet.Rows[0][3].ToString();
                var OutgoingPort = dtFromEmailDet.Rows[0][4].ToString();

                ////var Rpt = ASPxDocumentViewer1.Report;
                ////// Create a new memory stream and export the report into it as PDF.
                ////MemoryStream mem = new MemoryStream();
                ////Rpt.ExportToPdf(mem);

                ////// Create a new attachment and put the PDF report into it.
                ////mem.Seek(0, System.IO.SeekOrigin.Begin);
                ////Attachment att = new Attachment(mem, Rpt.DisplayName + ".pdf", "application/pdf");

                // Create a new message and attach the PDF report to it.
                MailMessage mail = new MailMessage();
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                SmtpClient smtp = new SmtpClient(OutgoingSMTPHost);
                //mail.Attachments.Add(att);

                var FromAdd = Email;
                var ToAdd = ToEmail;
                var CcAdd = CCEmail;
                var BccAdd = BCCEmail;
                var Body = EmailBody;
                var EmailSubject = Subject;

                // Specify sender and recipient options for the e-mail message.
                //mail.From = new MailAddress("bcool4u@gmail.com","Debashis");
                //mail.To.Add("debashis.talukder@indusnet.co.in");
                //mail.CC.Add("subhra.mukherjee@indusnet.co.in");
                //mail.Subject = "This is a Test Mail";
                mail.From = new MailAddress(FromAdd, FromWhere);
                mail.To.Add(ToAdd);
                if (CcAdd != "")
                {
                    mail.CC.Add(CcAdd);
                }
                if (BccAdd != "")
                {
                    mail.Bcc.Add(BccAdd);
                }
                mail.Subject = EmailSubject;
                mail.IsBodyHtml = true;
                mail.Body = Body;
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                smtp.Host = OutgoingSMTPHost.Trim();
                smtp.Port = Convert.ToInt32(OutgoingPort);
                smtp.Credentials = new System.Net.NetworkCredential(FromAdd, Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                //// att.Dispose();
                smtp.Dispose();
                mail.Dispose();
                // Close the memory stream.
                ////mem.Close();
                strreturnmsgemail = "success";
                //Email
            }
            catch (Exception ex)
            {
                strreturnmsgemail = "failed";
            }
            return strreturnmsgemail;
        }
        #endregion

        #region Activity SMS
        protected void CallbackPanelSMS_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string lead_entityid = e.Parameter.Split('~')[1];
            if (strSplitCommand == "Load")
            {
                string[,] DataSMS = oDBEngine.GetFieldValue("tbl_master_phonefax", "phf_phoneNumber", "phf_cntId='" + lead_entityid + "'", 1, "phf_phoneNumber");
                CallbackPanelSMS.JSProperties["cpStatusActivity"] = "Load";
                if (DataSMS[0, 0] != "n")
                {
                    CallbackPanelSMS.JSProperties["cpSMS"] = DataSMS[0, 0];
                }
                else
                {
                    CallbackPanelSMS.JSProperties["cpSMS"] = "";
                }
            }


        }

        [WebMethod]
        public static object SaveSMS(string ActionType, string Contactid, string MobileNo, string SmsContent)
        {
            try
            {
                string ErrorMsg = "";
                //Save 
                DataTable dtchkassignsms = new DataTable();

                int OutputId = 0;
                SqlConnection con1 = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SMS_ACTIVITY", con1);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION_TYPE", ActionType);
                cmd.Parameters.AddWithValue("@ENTITY_ID", Contactid);
                cmd.Parameters.AddWithValue("@MOBILENO", MobileNo);
                cmd.Parameters.AddWithValue("@SMSCONTENT", SmsContent);
                cmd.Parameters.AddWithValue("@SMS_STATUS", "Success");
                cmd.Parameters.AddWithValue("@MODULENAME", "Activity");
                cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()));

                SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                outputText.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dtchkassignsms);
                cmd.Dispose();
                con1.Dispose();

                OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

                //End of Save




                //SMS sent
                string status = string.Empty;
                String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                String username = System.Configuration.ConfigurationSettings.AppSettings["username"];
                String password = System.Configuration.ConfigurationSettings.AppSettings["password"];
                String Provider = System.Configuration.ConfigurationSettings.AppSettings["Provider"];
                String sender = System.Configuration.ConfigurationSettings.AppSettings["sender"];
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];

                string[] mobiles = MobileNo.Split(',');

                foreach (string mob in mobiles)
                {
                    try
                    {
                        string res = SmsSent(username, password, Provider, sender, MobileNo, SmsContent, "Text");
                    }
                    catch
                    {
                        if (ErrorMsg == "")
                            ErrorMsg = MobileNo;
                        else
                            ErrorMsg = ErrorMsg + "," + mob;
                    }
                }

                //End of SMS Sent

                //Update recently added document with error statement and mobile no if error occure.
                if (ErrorMsg != "")
                {
                    ErrorMsg = ErrorMsg + "  Failed";
                    SqlConnection consms = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmdsms = new SqlCommand("PRC_SMS_ACTIVITY", consms);

                    cmdsms.CommandType = CommandType.StoredProcedure;
                    cmdsms.Parameters.AddWithValue("@ACTION_TYPE", "UPDATE");
                    cmdsms.Parameters.AddWithValue("@SMS_STATUS", ErrorMsg);
                    cmdsms.Parameters.AddWithValue("@SMSID", OutputId);

                    SqlParameter outputsms = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
                    outputsms.Direction = ParameterDirection.Output;
                    SqlParameter outputTextsms = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
                    outputTextsms.Direction = ParameterDirection.Output;
                    cmdsms.Parameters.Add(outputsms);
                    cmdsms.Parameters.Add(outputTextsms);

                    cmdsms.CommandTimeout = 0;
                    SqlDataAdapter Adapsms = new SqlDataAdapter();
                    Adapsms.SelectCommand = cmdsms;
                    Adapsms.Fill(dtchkassignsms);
                    cmdsms.Dispose();
                    consms.Dispose();
                }

                // End of Update

                return OutputId;
            }
            catch (Exception EX)
            {
                //status = "300";
                //return status;
                return null;
            }
        }

        public static string SmsSent(string username, string password, string Provider, string senderId, string mobile, string message, string type)
        {

            // http://5.189.187.82/sendsms/sendsms.php?username=QHEkaruna&password=baj8piv3&type=Text&sender=KARUNA&mobile=9831892083&message=HELO
            string response = "";
            string url = Provider + "?username=" + username + "&password=" + password + "&type=" + type + "&sender=" + senderId + "&mobile=" + mobile + "&message=" + message;
            if (mobile.Trim() != "")
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    response = httpResponse.StatusCode.ToString();
                }
                catch
                {
                    return "0";
                }
            }
            return response;
        }

        #endregion

        protected void ShowGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string available = Convert.ToString(e.GetValue("isCancel"));
            if (available.ToUpper() == "TRUE")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                e.Row.Font.Strikeout = true;
            }           
            else
            {
                e.Row.ForeColor = System.Drawing.Color.Black;
            }
        }

        
    }
}