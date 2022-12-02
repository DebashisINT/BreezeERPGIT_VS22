using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using System.Web;
using DevExpress.Web;
using System.Collections.Specialized;
using DataAccessLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_Activities_ShowHistory_Phonecall : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";
        SlaesActivitiesBL oblSales = new SlaesActivitiesBL();
        DateTime dtFrom;
        DateTime dtTo;
        protected void Page_Init(object sender, EventArgs e)
        {
            SalesDetailsGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ActivityTypeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                Session["exportval"] = null;
                GetdetailsAssigned();
                if (Request.QueryString["status"] != null)
                {
                    if (Request.QueryString["status"] == "1")
                    {
                        crsbuttn.Attributes.Add("style", "display:none");
                       
                    }
                }
                else
                {
                    crsbuttn.Attributes.Add("style", "display:block");
                }
                
            }
            ShowHistory();

            // ShowAssignTo();
            string previousPageUrl = string.Empty;
            if (Request.UrlReferrer != null)
                previousPageUrl = Request.UrlReferrer.AbsoluteUri;

            if (Request.QueryString["frmdate"] != null && Request.QueryString["todate"] != null)
            {
                if (!previousPageUrl.Contains("?"))
                {
                    previousPageUrl = previousPageUrl + "?frmdate=" + Request.QueryString["frmdate"] + "&todate=" + Request.QueryString["todate"];

                }
                else
                {

                    var Urlarr = previousPageUrl.Split('?');

                    previousPageUrl = Urlarr[0] + "?frmdate=" + Request.QueryString["frmdate"] + "&todate=" + Request.QueryString["todate"];

                }
               
            }
            else
                previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

            ViewState["previousPageUrl"] = previousPageUrl;
            goBackCrossBtn.NavigateUrl = previousPageUrl;
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
                if (Request.QueryString["status"] != null)
                {
                    if (Request.QueryString["status"] == "1")
                    {
                        this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";                        
                        //divcross.Visible = false;
                    }
                }
                else
                {
                    this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                    //divcross.Visible = true;
                }
            }
        }


        #region Assigned BY  and  Assigned To
        public void GetdetailsAssigned()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id1"]))
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("SP_GetAssignedByandAssignedtoActivityHistory");
                proc.AddVarcharPara("@TsalesId", 500, Request.QueryString["id1"]);

                dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    lblassignedBy.Text = Convert.ToString(dt.Rows[0]["AssignedByName"]);
                    lblassignedto.Text = Convert.ToString(dt.Rows[0]["AssignedToName"]);
                }

            }

        }
        #endregion


        //public void ShowAssignTo()
        //{

        //    Employee_BL objemployeebal = new Employee_BL();
        //    string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
        //    DataTable dtbl = new DataTable();

        //    dtbl = objemployeebal.GetAssignedEmployeeDetailByReportingTo(owninterid);

        //    if(dtbl.Rows.Count>0)
        //    {

        //        AssignedToDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
        //        AssignedToDataSource.SelectCommand = "prc_EmployeeAllDtl";
        //        AssignedToDataSource.SelectParameters.Clear();
        //        AssignedToDataSource.SelectParameters.Add("action", "GetAssignedEmployeeDetailByReportingTo");
        //        AssignedToDataSource.SelectParameters.Add("cnt_internalId", owninterid);
        //        AssignedToDataSource.DataBind();
        //    }

        //}
        public void ShowHistory()
        {
            string ID = string.Empty;

            if (Request.QueryString["id1"] != null)
            {
                string Id = Convert.ToString(Request.QueryString["id1"]);
                int n;
                bool isNumeric = int.TryParse(Convert.ToString(Request.QueryString["id1"]), out n);

                if (isNumeric)
                {


                    lblHead.Text = "Activity History Details";
                    string SId = Convert.ToString(Request.QueryString["id1"]);
                    DataTable Dt = new DataTable();
                    Dt = oblSales.GetContactLeadCustomerId(SId);
                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        Id = Convert.ToString(Dt.Rows[0]["phc_leadcotactId"]);
                        BindOtherHistory(Id, SId);

                        headerid.Visible = true;
                    }
                    else
                    {
                        headerid.Visible = false;
                    }

                }
                else
                {

                    lblHead.Text = "Lead History";
                    BindHistory(Id);
                }




            }
            else
            {
                if (Session["InternalId"] != null)
                {
                    string Id = Convert.ToString(Session["InternalId"]);
                    BindHistory(Id);
                }
            }
        }




        #region BindOtherHistory
        public void BindOtherHistory(string Id, string Sid)
        {



            //int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            //FutureSalesDataSource.SelectCommand = "Select tbl_trans_Sales.sls_sales_status AS Status,(SELECT cnt_firstName+' '+isnull(cnt_middleName,'')+ ' ' +isnull(cnt_lastName,'') FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id) AS Name,(SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], '  + ISNULL(add_pin, '') FROM tbl_master_address WHERE add_cntId = sls_contactlead_id) AS Address,(SELECT Top 1 phf_phoneNumber FROM tbl_master_phoneFax WHERE phf_cntId = sls_contactlead_id) AS Phone,CASE tbl_trans_Sales.sls_ProductType WHEN 'IPO' THEN 'frmSalesIPO1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Mutual Fund' THEN 'frmSalesMutualFund1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance' THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) ELSE 'frmSalesCommodity1.aspx?id=' + cast(sls_id AS varchar) END AS ProductTypePath,sls_ProductType as ProductType,tbl_trans_Sales.sls_id AS Id,tbl_trans_Sales.sls_estimated_value AS Amount, CASE isnull(sls_product, '') WHEN '' THEN tbl_trans_Sales.sls_productType ELSE (SELECT prds_description FROM tbl_master_products WHERE prds_internalId = sls_product) END AS Product,sls_contactlead_id as LeadId,case sls_nextvisitdate when '1/1/1900 12:00:00 AM' then ' ' else (convert(varchar(11),sls_nextvisitdate,113) +' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) end as NextVisit From  tbl_trans_Sales ,tbl_trans_Activies  Where tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id AND tbl_trans_Activies.act_assignedTo ='" + UserId.ToString() + "' AND sls_sales_status in (3) Order by convert(datetime,sls_nextvisitdate,101)";
            SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsGridDataSource.SelectCommand = "prc_SalesActivity";
            SalesDetailsGridDataSource.SelectParameters.Clear();
            SalesDetailsGridDataSource.SelectParameters.Add("action", "GetHistorydtl");
            SalesDetailsGridDataSource.SelectParameters.Add("LeadContactId", Id);
            SalesDetailsGridDataSource.SelectParameters.Add("trsid", Sid);
            SalesDetailsGrid.DataBind();


            string SId = Convert.ToString(Request.QueryString["id1"]);
            string lcid = Id.Substring(0, 2);
            string[,] Name;
            if (lcid == "LD")
            {
                // Code  Added and Commented By Priti on 09122016 to use tbl_master_contact instead of tbl_master_lead(Invalid table) 
                //Name = oDBEngine.GetFieldValue("tbl_master_lead", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
                Name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                Name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
            }

            Session["CLName"] = Name;
            if (lcid == "LD")
            {
                lblName.Text = "Lead Name : " + Name[0, 0];
            }
            else
            {
                lblName.Text = "Customer Name : " + Name[0, 0];
            }

        }

        #endregion BindOtherHistory
        #region BindHistory
        public void BindHistory(string Id)
        {
            //int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            //FutureSalesDataSource.SelectCommand = "Select tbl_trans_Sales.sls_sales_status AS Status,(SELECT cnt_firstName+' '+isnull(cnt_middleName,'')+ ' ' +isnull(cnt_lastName,'') FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id) AS Name,(SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], '  + ISNULL(add_pin, '') FROM tbl_master_address WHERE add_cntId = sls_contactlead_id) AS Address,(SELECT Top 1 phf_phoneNumber FROM tbl_master_phoneFax WHERE phf_cntId = sls_contactlead_id) AS Phone,CASE tbl_trans_Sales.sls_ProductType WHEN 'IPO' THEN 'frmSalesIPO1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Mutual Fund' THEN 'frmSalesMutualFund1.aspx?id=' + cast(sls_id AS varchar) WHEN 'Insurance' THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) ELSE 'frmSalesCommodity1.aspx?id=' + cast(sls_id AS varchar) END AS ProductTypePath,sls_ProductType as ProductType,tbl_trans_Sales.sls_id AS Id,tbl_trans_Sales.sls_estimated_value AS Amount, CASE isnull(sls_product, '') WHEN '' THEN tbl_trans_Sales.sls_productType ELSE (SELECT prds_description FROM tbl_master_products WHERE prds_internalId = sls_product) END AS Product,sls_contactlead_id as LeadId,case sls_nextvisitdate when '1/1/1900 12:00:00 AM' then ' ' else (convert(varchar(11),sls_nextvisitdate,113) +' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) end as NextVisit From  tbl_trans_Sales ,tbl_trans_Activies  Where tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id AND tbl_trans_Activies.act_assignedTo ='" + UserId.ToString() + "' AND sls_sales_status in (3) Order by convert(datetime,sls_nextvisitdate,101)";
            SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsGridDataSource.SelectCommand = "prc_SalesActivity";
            SalesDetailsGridDataSource.SelectParameters.Clear();
            SalesDetailsGridDataSource.SelectParameters.Add("action", "GetHistoryLeaddtl");
            SalesDetailsGridDataSource.SelectParameters.Add("LeadContactId", Id);

            SalesDetailsGrid.DataBind();


            string SId = Convert.ToString(Request.QueryString["id1"]);
            string lcid = Id.Substring(0, 2);
            string[,] Name;
            if (lcid == "LD")
            {
                // Code  Added and Commented By Priti on 09122016 to use tbl_master_contact instead of tbl_master_lead(Invalid table) 
                //Name = oDBEngine.GetFieldValue("tbl_master_lead", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
                Name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                Name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
            }
            if (Name[0, 0] != "n")
            {
                if (lcid == "LD")
                {
                    lblName.Text = "Lead Name : " + Name[0, 0];
                }
                else
                {
                    lblName.Text = "Customer Name : " + Name[0, 0];
                }
            }

        }

        #endregion BindHistory

        protected void grdShowHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Text = Convert.ToString(Server.HtmlDecode(e.Row.Cells[i].Text));
                }
            }
        }


        protected void grdShowSalesVisitHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Text = Convert.ToString(Server.HtmlDecode(e.Row.Cells[i].Text));
                }
            }
        }

        protected void grdOtherHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Text = Convert.ToString(Server.HtmlDecode(e.Row.Cells[i].Text));
                }
            }
        }


        protected void drdSalesActivityDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["export"] = "1";
            Int32 Filter = int.Parse(Convert.ToString(drdSalesActivityDetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindSalesActivityDetailsexport(Filter);
                }
            }
        }
        public void bindSalesActivityDetailsexport(int Filter)
        {
            SalesDetailsGrid.Columns[12].Visible = false;
            exporter.GridViewID = "SalesDetailsGrid";
            exporter.FileName = "SalesActivityHistoryDetails";
            exporter.PageHeader.Left = "Sales Activity History Details";
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

        #region Gridview Callback
        protected void AspxPFeedbackdetails_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string[] CallVal = e.Parameters.ToString().Split('~');

            if (CallVal[0].ToString() == "Feedbackdetails")
            {
                BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
                string DetailId = Convert.ToString(CallVal[1].ToString());
                string Tid = Convert.ToString(CallVal[2].ToString());
                string Sid = Convert.ToString(CallVal[3].ToString());

               
                DataTable DtProduct = new DataTable();
                DtProduct = objBL.GetFeedbackgethistorydetails(Sid, Tid);

                if (DtProduct.Rows.Count > 0)
                {
                    AspxFeedGrid.DataSource = DtProduct;
                    AspxFeedGrid.DataBind();
                }
                else
                {
                    AspxFeedGrid.DataSource = null;
                    AspxFeedGrid.DataBind();
                }
            }
        }
        protected void SalesDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int ISEmail = 0;
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["cntId"]);//Session usercontactID
            int i = 0;
            SalesDetailsGrid.JSProperties["cpSave"] = null;
            string id1 = Convert.ToString(Request.QueryString["id1"]);
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                //  lengthIndex = e.Parameters.Split('~');




                if (CallVal[0].ToString() == "Feedback")
                {
                    int Isemail = 0;
                    if (chkmailfeedback.Checked)
                    {
                        Isemail = 1;
                    }

                    string Id = Convert.ToString(CallVal[1].ToString());

                    string Remarks = Convert.ToString(CallVal[2].ToString());
                    string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);

                    string Tid = Convert.ToString(CallVal[3].ToString());
                    string Stid = Convert.ToString(CallVal[4].ToString());


                    string Supervisor = Convert.ToString(CallVal[5].ToString());
                    string ActivityNote = Convert.ToString(CallVal[6].ToString());
                    string Outcome = Convert.ToString(CallVal[7].ToString());

                    string NextCall = Convert.ToString(CallVal[8].ToString());
                    string CallDate = Convert.ToString(CallVal[9].ToString());
                    var str = lblName.Text.ToString();
                    string CustomerLead = str.Substring(str.LastIndexOf(':') + 1);
                   
                    string ProductClass = string.Empty;
                    string Industry = string.Empty;
                    BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
                    i = obl.FeedbackActivity(Id, UserId, Remarks, Tid, Stid);
                    if (i > 0)
                    {
                        // BindGridDetails();

                        SalesDetailsGrid.JSProperties["cpFeedSave"] = "Y";

                        #region MailSendFeedback
                        if (Isemail == 1)
                        {
                            string RecvEmail = string.Empty;
                            string ActvName = string.Empty;

                            DataTable dtbl_AssignedTo = new DataTable();
                            DataTable dtbl_AssignedBy = new DataTable();
                            DataTable dtEmail_To = new DataTable();
                            DataTable dtActivityName = new DataTable();
                            DataTable dt_EmailConfig = new DataTable();
                            DataTable dt_AssignedUserDetails = new DataTable();

                            Employee_BL objemployeebal = new Employee_BL();

                            dt_AssignedUserDetails = objemployeebal.GetEmailAccountConfigDetails(Stid, 6);

                            ProductClass = Convert.ToString(dt_AssignedUserDetails.Rows[0]["ProductClass_Name"]);

                            Industry = Convert.ToString(dt_AssignedUserDetails.Rows[0]["Industry"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo"))))
                            {
                                dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo")), 2);
                            }
                            dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(UserId, 3);
                            dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo")), 4);
                            dtActivityName = objemployeebal.GetEmailAccountConfigDetails(id1, 5);
                            dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(Convert.ToString(dt_AssignedUserDetails.Rows[0].Field<decimal>("sls_assignedTo")), 1);


                            if (dtEmail_To.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                {
                                    RecvEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                }
                                else
                                {
                                    RecvEmail = "";
                                }
                            }

                            if (dtActivityName != null && dtActivityName.Rows.Count>0)
                            {
                                if (!string.IsNullOrEmpty(dtActivityName.Rows[0].Field<string>("act_activityName")))
                                {
                                    ActvName = dtActivityName.Rows[0].Field<string>("act_activityName");
                                }
                                else
                                {
                                    ActvName = "";
                                }
                            }
                            else
                            {
                                ActvName = "";
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
                                replacements.Add("<%Feedback%>", Remarks);
                            }
                            else
                            {
                                replacements.Add("<%Feedback%>", "");
                            }

                            replacements.Add("<%ActivityName%>", ActvName);


                            if (!string.IsNullOrEmpty(CustomerLead))
                            {
                                replacements.Add("<%CustomerLeadName%>", CustomerLead);
                            }
                            else
                            {
                                replacements.Add("<%CustomerLeadName%>", "");
                            }

                            if (!string.IsNullOrEmpty(ProductClass))
                            {
                                replacements.Add("<%ProductClass%>", ProductClass);
                            }
                            else
                            {
                                replacements.Add("<%ProductClass%>", "");
                            }

                            if (!string.IsNullOrEmpty(Industry))
                            {
                                replacements.Add("<%Industry%>", Industry);
                            }
                            else
                            {
                                replacements.Add("<%Industry%>", "");
                            }

                            if (!string.IsNullOrEmpty(CallDate))
                            {
                                replacements.Add("<%CallVisitDate%>", CallDate);
                            }
                            else
                            {
                                replacements.Add("<%CallVisitDate%>", "");
                            }

                            if (!string.IsNullOrEmpty(NextCall))
                            {
                                replacements.Add("<%NextActivityDate%>", NextCall);
                            }
                            else
                            {
                                replacements.Add("<%NextActivityDate%>", "");
                            }

                            if (!string.IsNullOrEmpty(ActivityNote))
                            {
                                replacements.Add("<%ActivityNote%>", ActivityNote);
                            }
                            else
                            {
                                replacements.Add("<%ActivityNote%>", "");
                            }

                            if (!string.IsNullOrEmpty(Outcome))
                            {
                                replacements.Add("<%Outcome%>", Outcome);
                            }
                            else
                            {
                                replacements.Add("<%Outcome%>", "");
                            }

                            //if (!string.IsNullOrEmpty(Supervisor))
                            //{
                            //    replacements.Add("<%supervisor%>", Supervisor);
                            //}
                            //else
                            //{
                            //    replacements.Add("<%supervisor%>", "");
                            //}

                            if (!string.IsNullOrEmpty(RecvEmail))
                            {

                                ExceptionLogging.SendMailSupervisorFunctionality(RecvEmail, "", replacements, dt_EmailConfig, ActvName, 6);
                            }
                        }


                        #endregion
                    }
                    else
                    {
                        SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
                        SalesDetailsGrid.JSProperties["cpFeedSave"] = "N";
                    }

                }





            }
            catch { }
        }
        #endregion
    }
}