using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Text;
using System.Reflection;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System.Collections.Specialized;
using DataAccessLayer;

namespace ERP.OMS.Management.Activities
{
    public partial class Sales_List : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        string[] lengthIndex;
        DateTime dtFrom;
        DateTime dtTo;
        protected void Page_Init(object sender, EventArgs e)
        {
            SalesDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SalesDetailsClosedGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SalesDetailsDocumentGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            SalesDetailsFutureGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SalesDetailsClosedGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SalesDetailsDocumentGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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
            if(!IsPostBack)
            {
               
                Session["exportval"] = null;
                Session["exportDetailsval"] = null;
                Session["exportFutureval"] = null;
                Session["exportclosedval"] = null;
                Session["exportdocumentval"] = null;
                
                Session["act_id"] = null;


                SaleGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesActivitySalesListDetailsGrid";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesActivitySalesListDetailsGrid');</script>");
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                if (Session["DtFrom_saleslist"] != null && Session["DtTo_saleslist"] != null)
                {
                    ASPxFromDate.Text = Convert.ToString(Session["DtFrom_saleslist"]);
                    ASPxToDate.Text = Convert.ToString(Session["DtTo_saleslist"]);

                    dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                    dtTo = Convert.ToDateTime(ASPxToDate.Date);
                }

                else
                {
                    ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                    ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                  
                }

            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }


            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/ActivityManagement/Sales_List.aspx");
            if (Session["act_id"] == null)
            {
                //BindGrid(dtFrom, dtTo);
            }
            else
            {
               
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void SalesGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Session["DtFrom_saleslist"] = dtFrom.ToString("dd-MM-yyyy");
            Session["DtTo_saleslist"] = dtTo.ToString("dd-MM-yyyy");
            //SaleGrid.DataBind();
            BindGrid(dtFrom, dtTo);
        }
        void BindGrid(DateTime FromDate, DateTime ToDate)
        {
            //int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            //string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID

            //// SalesDataSource.SelectCommand = "Select  tbl_trans_Sales.sls_sales_status AS Status,isnull((SELECT cnt_firstName+' ' +isnull(cnt_middleName,'')+' ' +isnull(cnt_lastName,'')FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id),(SELECT cnt_firstName+' ' +isnull(cnt_middleName,'')+' ' +isnull(cnt_lastName,'') FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id))  AS Name,(SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' + ISNULL(add_pin, '') FROM tbl_master_address WHERE add_cntId = sls_contactlead_id) AS Address,(SELECT Top 1 phf_phoneNumber FROM tbl_master_phoneFax WHERE phf_cntId = sls_contactlead_id) AS Phone,CASE tbl_trans_Sales.sls_ProductType WHEN 'Mutual Fund' THEN 'frmSalesMutualFund1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance-life'  THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance-general'  THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Sub Broker'	THEN 'frmSalesSubBroker.aspx?id='+cast(sls_id AS varchar) ELSE 'frmSalesCommodity1.aspx?id=' + cast(sls_id AS varchar) END AS ProductTypePath,sls_ProductType as ProductType,tbl_trans_Sales.sls_id AS Id, tbl_trans_Sales.sls_estimated_value AS Amount,CASE isnull(sls_product, '') WHEN ''THEN tbl_trans_Sales.sls_productType ELSE (SELECT prds_description FROM tbl_master_products WHERE prds_internalId = sls_product) END AS Product,sls_contactlead_id as LeadId, case sls_nextvisitdate when '1/1/1900 12:00:00 AM' then ' ' else (convert(varchar(11),sls_nextvisitdate,113) +' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) end as NextVisit From  tbl_trans_Sales , tbl_trans_Activies Where tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id AND tbl_trans_Activies.act_assignedTo ='" + UserId.ToString() + "' and sls_sales_status=4 Order by convert(datetime,sls_nextvisitdate,101) ";
            ////kaushik 19_12_2016 
            ////  SalesDataSource.SelectCommand = "Select  tbl_trans_Sales.sls_sales_status AS Status,isnull((SELECT cnt_firstName+' ' +isnull(cnt_middleName,'')+' ' +isnull(cnt_lastName,'')FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id),(SELECT cnt_firstName+' ' +isnull(cnt_middleName,'')+' ' +isnull(cnt_lastName,'') FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id))  AS Name,(SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' + ISNULL(add_pin, '') FROM tbl_master_address WHERE add_cntId = sls_contactlead_id) AS Address,(SELECT Top 1 phf_phoneNumber FROM tbl_master_phoneFax WHERE phf_cntId = sls_contactlead_id) AS Phone,CASE tbl_trans_Sales.sls_ProductType WHEN 'Mutual Fund' THEN 'frmSalesMutualFund1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance-life'  THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance-general'  THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Sub Broker'	THEN 'frmSalesSubBroker.aspx?id='+cast(sls_id AS varchar) ELSE 'frmSalesCommodity1.aspx?id=' + cast(sls_id AS varchar) END AS ProductTypePath,sls_ProductType as ProductType,tbl_trans_Sales.sls_id AS Id, tbl_trans_Sales.sls_estimated_value AS Amount,CASE isnull(sls_product, '') WHEN ''THEN tbl_trans_Sales.sls_productType ELSE (SELECT prds_description FROM tbl_master_products WHERE prds_internalId = sls_product) END AS Product,sls_contactlead_id as LeadId, case sls_nextvisitdate when '1/1/1900 12:00:00 AM' then ' ' else (convert(varchar(11),sls_nextvisitdate,113) +' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) end as NextVisit From  tbl_trans_Sales , tbl_trans_Activies Where tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id AND  sls_sales_status=4 Order by convert(datetime,sls_nextvisitdate,101) ";
            //SalesDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            //SalesDataSource.SelectCommand = "sp_Sales";
            //SalesDataSource.SelectParameters.Clear();
            //SalesDataSource.SelectParameters.Add("Mode", "GetSales");
            //SalesDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            //if (FromDate != null)
            //{
            //    SalesDataSource.SelectParameters.Add("Fromdate", FromDate.ToString("yyyy-MM-dd"));
            //}
            //if (ToDate != null)
            //{
            //    SalesDataSource.SelectParameters.Add("ToDate", ToDate.ToString("yyyy-MM-dd"));
            //}
            SaleGrid.DataBind();

        }
        public void BindGridDetails()
        {
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID
            SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsGridDataSource.SelectParameters.Clear();
            SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetSalesDetails");
            SalesDetailsGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesDetailsGrid.DataBind();
        }

        protected void SGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {

                DateTime dtFrom;
                DateTime dtTo;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);


                Session["DtFrom_saleslist"] = dtFrom.ToString("dd-MM-yyyy");
                Session["DtTo_saleslist"] = dtTo.ToString("dd-MM-yyyy");

                BindGrid(dtFrom, dtTo);

                string[] CallVal = e.Parameters.ToString().Split('~');
                lengthIndex = e.Parameters.Split('~');

                if (CallVal[0].ToString() == "Delete")
                {
                    string Id = Convert.ToString(CallVal[1].ToString());
                    int retValue = objBL.DeleteSalesActivity(Convert.ToInt32(Id));
                    if (retValue > 0)
                    {
                        Session["KeyVal"] = "Succesfully Deleted";
                        SaleGrid.JSProperties["cpDelmsg"] = "Succesfully Deleted";

                        BindGrid(dtFrom, dtTo);
                    }
                    else
                    {
                        Session["KeyVal"] = " Cannot Delete.";
                        SaleGrid.JSProperties["cpDelmsg"] = "Activity already done, cannot delete.";
                        //  SGrid.DataBind();
                        BindGrid(dtFrom, dtTo);

                    }
                }
                if (CallVal[0].ToString() == "Reassign")
                {

                    string suId = Convert.ToString(CallVal[1].ToString());
                    string AId = Convert.ToString(CallVal[2].ToString());
                    string sOuId = Convert.ToString(CallVal[3].ToString());
                    //   string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                    string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);
                    BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();


                    int retValue = obl.ReassignedSupervisorActivity(suId, AId, UserId, sOuId);
                    if (retValue > 0)
                    {

                        SaleGrid.JSProperties["cpSupmsg"] = "Supervisor succesfully reassigned";

                        BindGrid(dtFrom, dtTo);
                    }
                    else
                    {

                        SaleGrid.JSProperties["cpSupmsg"] = "Please try Again.";
                        //  SGrid.DataBind();
                        BindGrid(dtFrom, dtTo);

                    }
                }

                if (CallVal[0].ToString() == "SalesmanReassign")
                {
                    int ISEmail = 0;

                    string AId = Convert.ToString(CallVal[1].ToString());
                    string UId = Convert.ToString(CallVal[2].ToString());
                    string Remarks = Convert.ToString(CallVal[3].ToString());
                    //   string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                    string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);
                    BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
                    int retValue = obl.ReassignedSalesmanActivity(UId, AId, UserId, Remarks);

                    if (chkMail.Checked)
                    {
                        ISEmail = 1;
                    }



                    // int retValue = obl.ReassignedSupervisorActivity(suId, AId, UserId, sOuId);
                    if (retValue > 0)
                    {

                        SaleGrid.JSProperties["cpSManmsg"] = "Salesman succesfully reassigned";

                        BindGrid(dtFrom, dtTo);




                        //Done By:Subhabrata on 23-01-2017
                        #region EmailSendSalesActivity

                        if (ISEmail == 1)
                        {
                            //Done By:Subhabrata
                            string CreateUserId = Convert.ToString(HttpContext.Current.Session["userid"]);//Session UserID;
                            string ReceiverEmail = string.Empty;
                            string ActivityName = string.Empty;

                            DataTable dtbl_AssignedTo = new DataTable();
                            DataTable dtbl_AssignedBy = new DataTable();
                            DataTable dtEmail_To = new DataTable();
                            DataTable dtActivityName = new DataTable();
                            DataTable dt_EmailConfig = new DataTable();

                            Employee_BL objemployeebal = new Employee_BL();

                            dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(UId, 2);
                            dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                            dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(UId, 4);
                            //  dtActivityName = objemployeebal.GetEmailAccountConfigDetails(id1, 5);
                            dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(UId, 1);

                            if (dtEmail_To.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                {
                                    ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                }
                                else
                                {
                                    ReceiverEmail = "";
                                }
                            }

                            if (!string.IsNullOrEmpty(dtActivityName.Rows[0].Field<string>("act_activityName")))
                            {
                                ActivityName = dtActivityName.Rows[0].Field<string>("act_activityName");
                            }
                            else
                            {
                                ActivityName = "";
                            }

                            ListDictionary replacements = new ListDictionary();
                            if (dtbl_AssignedTo.Rows.Count > 0)
                            {
                                replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                            }
                            else
                            {
                                replacements.Add("<%AssigneeTo%>", "");
                            }
                            if (dtbl_AssignedBy.Rows.Count > 0)
                            {
                                replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                            }
                            else
                            {
                                replacements.Add("<%AssignedBy%>", "");
                            }
                            replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));

                            if (!string.IsNullOrEmpty(Remarks))
                            {
                                replacements.Add("<%Remarks %>", Remarks);
                            }
                            else
                            {
                                replacements.Add("<%Remarks %>", "");
                            }

                            replacements.Add("<%ActivityName%>", ActivityName);

                            //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                            if (!string.IsNullOrEmpty(ReceiverEmail))
                            {
                                //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName, 5);
                                ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 5);
                            }


                        }

                        #endregion
                        //End
                    }
                    else
                    {

                        SaleGrid.JSProperties["cpSManmsg"] = "Please try Again.";
                        //  SGrid.DataBind();
                        BindGrid(dtFrom, dtTo);

                    }
                }
                // Mantis Issue 24211
                if (CallVal[0].ToString() == "CreateOpportunities")
                {
                    int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
                    string returnmessage = "";

                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("PRC_CRMCREATEOPPORTUNITY", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "AddOpportunities");
                    cmd.Parameters.AddWithValue("@cnt_internalId", cnt_internalId);
                    cmd.Parameters.AddWithValue("@ActivityTypeId", "3");
                    cmd.Parameters.AddWithValue("@FROMDATE", dtFrom);
                    cmd.Parameters.AddWithValue("@TODATE", dtTo);
                    cmd.Parameters.AddWithValue("@USERID", UserId);

                    cmd.Parameters.Add("@RETURNMESSAGE", SqlDbType.Char, 500);
                    cmd.Parameters["@RETURNMESSAGE"].Direction = ParameterDirection.Output;

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();

                    returnmessage = (string)cmd.Parameters["@RETURNMESSAGE"].Value;

                    con.Dispose();

                    if (returnmessage.Trim() == "-20"){
                        SaleGrid.JSProperties["cpSManmsg"] = "Open Opportunity exist. Cannot Create new Opportunities.";
                    }
                    else if (returnmessage == "-10")
                    {
                        SaleGrid.JSProperties["cpSManmsg"] = "Failed to Opportunities.";
                    }
                    else
                    {
                        SaleGrid.JSProperties["cpSManmsg"] = "Opportunity created for " + returnmessage + " Future Sales Record.";
                    }
                    
                   
                }
                // End of Mantis Issue 24211

            }
            catch { }
        }


        protected void SalesDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int i = 0;
            SalesDetailsGrid.JSProperties["cpSave"] = null;
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            BindGrid(dtFrom, dtTo);

            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                lengthIndex = e.Parameters.Split('~');

                if (CallVal[0].ToString() == "Details")
                {
                    string Id = Convert.ToString(CallVal[1].ToString());
                    string AssignedId = Convert.ToString(CallVal[2].ToString());
                    SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
                    SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
                    SalesDetailsGridDataSource.SelectParameters.Clear();
                    SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetSalesDetails");
                    SalesDetailsGridDataSource.SelectParameters.Add("SalesActivityID", Id);
                    SalesDetailsGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
                    SalesDetailsGrid.DataBind();

                    Session["CacheId"] = Id; 
                    Session["AssignedTaskId"] = AssignedId;
                    Session["act_id"] = Id;
                }

              
                if (CallVal[0].ToString() == "CreateActivity")
                {
                    string LeadId = Convert.ToString(CallVal[1].ToString());
                    string sls_id = Convert.ToString(CallVal[2].ToString());

                    string sls_activity_id = Convert.ToString(CallVal[3].ToString());
                    string act_assignedTo = Convert.ToString(CallVal[4].ToString());
                    string act_activityNo = Convert.ToString(CallVal[5].ToString());
                    string act_assign_task = Convert.ToString(CallVal[6].ToString());
                    Session["AssignedActivity"] = LeadId;
                    Session["AssignedSalesId"] = sls_id;

                    Session["AssignedTaskId"] = act_assignedTo;
                    Session["act_id"] = sls_activity_id;
                    Session["act_activityNo"] = act_activityNo;
                    Session["act_assign_task"] = act_assign_task;
                    Session["CrmPhoneCallActivityUrl"] = "Sales_List.aspx";
                    SalesDetailsGrid.JSProperties["cpredirect"] = "CrmPhoneCallActivityWithIFrame.aspx";

                    //Response.Redirect("../ActivityManagement/CrmPhoneCallActivityWithIFrame.aspx");
                }


               
            }
            catch { }
        }


       
        protected void drdSalesActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdSalesActivity.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityexport(Filter);
                }
            }
        }
        public void bindSalesActivityexport(int Filter)
        {

            exporter.GridViewID = "SaleGrid";
            exporter.FileName = "Sales Activity";
            exporter.PageHeader.Left = "Sales Activity";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            SaleGrid.Columns[8].Visible = false;
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


        protected void drdSalesDocumentDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdSalesDocumentDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportdocumentval"] == null)
                {
                    Session["exportdocumentval"] = Filter;
                    bindSalesActivitydocumentexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportdocumentval"]) != Filter)
                {
                    Session["exportdocumentval"] = Filter;
                    bindSalesActivitydocumentexport(Filter);
                }
            }
        }
        public void bindSalesActivitydocumentexport(int Filter)
        {

            exporter.GridViewID = "SalesDocumentDetailsGrid";
            var gv = exporter.GridView;

            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


            SalesDetailsDocumentGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsDocumentGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsDocumentGridDataSource.SelectParameters.Clear();
            SalesDetailsDocumentGridDataSource.SelectParameters.Add("Mode", "GetSalesDocumentDetails");
            SalesDetailsDocumentGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDetailsDocumentGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesDocumentDetailsGrid.DataBind();
            gv.DataBind();
            exporter.FileName = "SalesActivityDocumentDetails";
            SalesDocumentDetailsGrid.Columns[13].Visible = false;
           
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


        protected void drdsalesClosedDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdsalesClosedDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportclosedval"] == null)
                {
                    Session["exportclosedval"] = Filter;
                    bindSalesActivityClosedexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportclosedval"]) != Filter)
                {
                    Session["exportclosedval"] = Filter;
                    bindSalesActivityClosedexport(Filter);
                }
            }
        }
        public void bindSalesActivityClosedexport(int Filter)
        {

            exporter.GridViewID = "SalesClosedDetailsGrid";
            var gv = exporter.GridView;

            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


            SalesDetailsClosedGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsClosedGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsClosedGridDataSource.SelectParameters.Clear();
            SalesDetailsClosedGridDataSource.SelectParameters.Add("Mode", "GetSalesClosedDetails");
            SalesDetailsClosedGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDetailsClosedGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesClosedDetailsGrid.DataBind();
            gv.DataBind();
            exporter.FileName = "SalesActivityClosedDetails";
            SalesClosedDetailsGrid.Columns[13].Visible = false;
         
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




        protected void drdSalesFutureDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdSalesFutureDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportFutureval"] == null)
                {
                    Session["exportDetailsval"] = Filter;
                    bindSalesActivityFutureDetailsexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportFutureval"]) != Filter)
                {
                    Session["exportFutureval"] = Filter;
                    bindSalesActivityFutureDetailsexport(Filter);
                }
            }
        }
        public void bindSalesActivityFutureDetailsexport(int Filter)
        {

            exporter.GridViewID = "SalesFutureDetailsGrid";
            var gv = exporter.GridView;

            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


            SalesDetailsFutureGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsFutureGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsFutureGridDataSource.SelectParameters.Clear();
            SalesDetailsFutureGridDataSource.SelectParameters.Add("Mode", "GetSalesFutureDetails");
            SalesDetailsFutureGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDetailsFutureGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesFutureDetailsGrid.DataBind();
            gv.DataBind();
            exporter.FileName = "SalesActivityFutureDetails";
            SalesFutureDetailsGrid.Columns[13].Visible = false;
          
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
     
        protected void drdSalesActivityDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdSalesActivityDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportDetailsval"] == null)
                {
                    Session["exportDetailsval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportDetailsval"]) != Filter)
                {
                    Session["exportDetailsval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
            }
        }
        public void bindSalesActivityDetailsexport(int Filter)
        {

            exporter.GridViewID = "SalesDetailsGrid";
          var  gv = exporter.GridView;

          string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID


          SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
          SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
          SalesDetailsGridDataSource.SelectParameters.Clear();
          SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetSalesDetails");
          SalesDetailsGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
          SalesDetailsGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
          SalesDetailsGrid.DataBind();
          gv.DataBind();
            exporter.FileName = "SalesActivityDetails";
            SalesDetailsGrid.Columns[13].Visible = false;
            SalesDetailsGrid.Columns[14].Visible = false;
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
        [WebMethod]
        public static List<string> GetAllUserListBeforeSelect()
        {
            StringBuilder filter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();

            dtbl = objemployeebal.GetAssignedEmployeeDetailByReportingTo(owninterid);


            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["cnt_id"]));
            }



            return obj;
        }


        [WebMethod]
        public static List<string> GetAllSalesmanListBeforeSelect(string UserId)
        {
            StringBuilder filter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();

            dtbl = objemployeebal.GetReassignUser(owninterid, UserId);


            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["cnt_id"]));
            }



            return obj;
        }
        protected void btnActivity_Click(object sender, EventArgs e)
        {
            string temp = string.Empty;
            DataTable dt = new DataTable();
            for (int i = 0; i < SalesDetailsGrid.VisibleRowCount; i++)
            {
            
                GridViewDataColumn col1 = SalesDetailsGrid.Columns[0] as GridViewDataColumn;
                ASPxCheckBox chkIsVal = SalesDetailsGrid.FindRowCellTemplateControl(i, col1, "chkDetail") as ASPxCheckBox;

                if (chkIsVal != null)
                {
                    //if (chkDetail.Checked == true)
                    if (chkIsVal.Checked == true)
                    {
                     
                        ASPxTextBox lbls = SalesDetailsGrid.FindRowCellTemplateControl(i, col1, "lblActNo") as ASPxTextBox;
                        string lbl = Convert.ToString(lbls.Text);
                        if (lbl != string.Empty)
                        {
                         
                            //ViewState["edit"] = lbl.Text;
                            temp += lbl;
                         
                          //  break;
                        }
                    }
                }
            }

            Session["AssignedActivity"] = temp;
            Response.Redirect("CrmPhoneCallActivityWithIFrame.aspx");
        }

        protected void SalesDocumentDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID

            SalesDetailsDocumentGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsDocumentGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsDocumentGridDataSource.SelectParameters.Clear();
            SalesDetailsDocumentGridDataSource.SelectParameters.Add("Mode", "GetSalesDocumentDetails");
            SalesDetailsDocumentGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDetailsDocumentGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesDocumentDetailsGrid.DataBind();
        }

        protected void SalesClosedDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID

            SalesDetailsClosedGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsClosedGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsClosedGridDataSource.SelectParameters.Clear();
            SalesDetailsClosedGridDataSource.SelectParameters.Add("Mode", "GetSalesClosedDetails");
            SalesDetailsClosedGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDetailsClosedGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesClosedDetailsGrid.DataBind();
        }
        
        protected void SalesFutureDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID

            SalesDetailsFutureGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsFutureGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsFutureGridDataSource.SelectParameters.Clear();
            SalesDetailsFutureGridDataSource.SelectParameters.Add("Mode", "GetSalesFutureDetails");
            SalesDetailsFutureGridDataSource.SelectParameters.Add("SalesActivityID", Convert.ToString(Session["CacheId"]));
            SalesDetailsFutureGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesFutureDetailsGrid.DataBind();
        }

        protected void propanel_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] data = e.Parameter.Split('~');
            if (data[0] == "ShowSupervisorHistory")
            {
               // BusinessLogicLayer.Others objBL1 = new BusinessLogicLayer.Others();
                DataTable productdt = objBL.PopulateSupervisorHistory(Convert.ToString(data[1]));
                grdproduct.DataSource = productdt;
                grdproduct.DataBind();

            }
        }
        [WebMethod]
        public static List<string> GetAllNewSupervisorList(string activityid, string supervisorId, string assignedToId)
        {
            StringBuilder filter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();

            dtbl = objemployeebal.GetNewSupervisorList(owninterid, activityid, supervisorId, assignedToId);


            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["cnt_id"]));
            }



            return obj;
        }

        protected void SaleGrid_DataBinding(object sender, EventArgs e)
        {
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            SaleGrid.DataSource = GetDataSource(dtFrom,dtTo);

        }

        DataTable GetDataSource(DateTime FromDate, DateTime ToDate)
        {
            //DataTable dt = new DataTable();
            int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID

            //SalesDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            //SalesDataSource.SelectCommand = "sp_Sales";
            //SalesDataSource.SelectParameters.Clear();
            //SalesDataSource.SelectParameters.Add("Mode", "GetSales");
            //SalesDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            //if (FromDate != null)
            //{
            //    SalesDataSource.SelectParameters.Add("Fromdate", FromDate.ToString("yyyy-MM-dd"));
            //}
            //if (ToDate != null)
            //{
            //    SalesDataSource.SelectParameters.Add("ToDate", ToDate.ToString("yyyy-MM-dd"));
            //}

            //return dt;


            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddVarcharPara("@Mode", 100, "GetSales");
            proc.AddVarcharPara("@cnt_internalId", 100, cnt_internalId);
            if (FromDate != null)
            {
                proc.AddVarcharPara("@Fromdate",10, FromDate.ToString("yyyy-MM-dd"));
            }
            if (ToDate != null)
            {
                proc.AddVarcharPara("@ToDate",10, ToDate.ToString("yyyy-MM-dd"));
            }

            ds = proc.GetTable();
            return ds;
        }
        
    }
}