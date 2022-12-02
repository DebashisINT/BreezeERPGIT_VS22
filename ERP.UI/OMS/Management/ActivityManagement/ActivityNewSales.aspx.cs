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
namespace ERP.OMS.Management.ActivityManagement
{
    public partial class ActivityNewSales : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        string[] lengthIndex;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SalesDetailsGridDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {

               

                string EditPermitValue = string.Empty;

                string cnt_id = Convert.ToString(Session["cntId"]);

                EditPermitValue = EditPermissionShow(cnt_id);

                if (EditPermitValue == "1")
                {


                    ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = false;
                }
                else
                {

                    ASPxPageControl1.TabPages.FindByName("Document Collection").ClientEnabled = true;
                }
                Session["exportval"] = null;
                SalesDetailsGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesActivityNewSalesDetailsGrid";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesActivityNewSalesDetailsGrid');</script>");

            }
            Session["export"] = null;
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/ActivityManagement/Sales_List.aspx");

            string id1 = Convert.ToString(Request.QueryString["id1"]);
            string Aid = Convert.ToString(Request.QueryString["Aid"]);
            // BindGrid();

           // BindLookupProduct(id1);
            BindGrid(id1, Aid);

           
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        void BindLookupProduct(string id1)
        {

         

            //if (DtProduct != null && DtProduct.Rows.Count > 0)
            //{
            //    GridLookup.DataSource = DtProduct;
            //    GridLookup.DataBind();

            //}
        }

        public string EditPermissionShow(string cntid)
        {


            string EditPermissionval = "0";
            try
            {
                BusinessLogicLayer.Others objOtherBL = new BusinessLogicLayer.Others();

                DataTable PermissionDt = new DataTable();
                PermissionDt = objOtherBL.GetSalesManDeactivateDocomentActivity(cntid);
                if (PermissionDt != null && PermissionDt.Rows.Count > 0)
                {
                    EditPermissionval = Convert.ToString(PermissionDt.Rows[0]["EditPermission"]);

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Server Internal Error.Please try again');", true);
                //return;
            }


            return EditPermissionval;
        }
       
        void BindGrid(string id1, string Aid)
        {
            hdnSalesAc.Value = id1;
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID
            SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
            SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
            SalesDetailsGridDataSource.SelectParameters.Clear();
            SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetSalesDetails");
            SalesDetailsGridDataSource.SelectParameters.Add("SalesActivityID", id1);
            SalesDetailsGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
            SalesDetailsGrid.DataBind();

        }



        protected void SalesDetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int ISEmail = 0;
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);//Session usercontactID
            int i = 0;
            SalesDetailsGrid.JSProperties["cpSave"] = null;
            string id1 = Convert.ToString(Request.QueryString["id1"]);
            try
            {
                string[] CallVal = e.Parameters.ToString().Split('~');
                lengthIndex = e.Parameters.Split('~');



                if (CallVal[0].ToString() == "Reassign")
                {
                    if (chkMail.Checked)
                    {
                        ISEmail = 1;
                    }

                    string Id = Convert.ToString(CallVal[1].ToString());
                    string UId = Convert.ToString(CallVal[2].ToString());
                    string Remarks = Convert.ToString(CallVal[3].ToString());
                    //   string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                    string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);
                    BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
                    i = obl.ReassignedActivity(UId, Id, UserId, Remarks);
                    if (i > 0)
                    {
                        // BindGridDetails();
                        SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
                        SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
                        SalesDetailsGridDataSource.SelectParameters.Clear();
                        SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetSalesDetails");
                        SalesDetailsGridDataSource.SelectParameters.Add("SalesActivityID", id1);
                        SalesDetailsGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
                        SalesDetailsGrid.DataBind();
                        SalesDetailsGrid.JSProperties["cpSave"] = "Y";

                        //Done By:Subhabrata on 23-01-2017
                        #region EmailSendSalesActivity
                       
                        if(ISEmail==1)
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
                            dtActivityName = objemployeebal.GetEmailAccountConfigDetails(id1,5);
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

                            if(!string.IsNullOrEmpty(Remarks))
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
                        SalesDetailsGridDataSource.SelectCommandType = System.Web.UI.WebControls.SqlDataSourceCommandType.StoredProcedure;
                        SalesDetailsGridDataSource.SelectCommand = "sp_Sales";
                        SalesDetailsGridDataSource.SelectParameters.Clear();
                        SalesDetailsGridDataSource.SelectParameters.Add("Mode", "GetSalesDetails");
                        SalesDetailsGridDataSource.SelectParameters.Add("SalesActivityID", id1);
                        SalesDetailsGridDataSource.SelectParameters.Add("cnt_internalId", cnt_internalId);
                        SalesDetailsGrid.DataBind();
                        SalesDetailsGrid.JSProperties["cpSave"] = "N";
                    }
                    hdnAssign.Value = "";

                }
                if (CallVal[0].ToString() == "Feedback")
                {
                    int Isemail = 0;
                    if (chkmailfeedback.Checked)
                    {
                        Isemail = 1;
                    }

                    string Id = Convert.ToString(CallVal[1].ToString());

                    string Remarks = Convert.ToString(CallVal[2].ToString());
                    string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);

                    BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
                   /// i = obl.FeedbackActivity(Id, UserId, Remarks);
                    if (i > 0)
                    {
                        // BindGridDetails();

                        SalesDetailsGrid.JSProperties["cpFeedSave"] = "Y";

                        #region MailSendFeedback
                        if(Isemail==1)
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

                            dt_AssignedUserDetails = objemployeebal.GetEmailAccountConfigDetails(Id, 6);
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

                            if (!string.IsNullOrEmpty(dtActivityName.Rows[0].Field<string>("act_activityName")))
                            {
                                ActvName = dtActivityName.Rows[0].Field<string>("act_activityName");
                            }
                            else
                            {
                                ActvName = "";
                            }

                            ListDictionary replacements = new ListDictionary();
                            if (dtbl_AssignedTo.Rows.Count > 0)
                            {
                                replacements.Add("@AssigneeTo@", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                            }
                            else
                            {
                                replacements.Add("@AssigneeTo@", "");
                            }
                            if (dtbl_AssignedBy.Rows.Count > 0)
                            {
                                replacements.Add("@AssignedBy@", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                            }
                            else
                            {
                                replacements.Add("@AssignedBy@", "");
                            }
                            replacements.Add("@TimeOfError@", Convert.ToString(DateTime.Now));

                            if (!string.IsNullOrEmpty(Remarks))
                            {
                                replacements.Add("@Feedback@", Remarks);
                            }
                            else
                            {
                                replacements.Add("@Feedback@", "");
                            }

                            replacements.Add("@ActivityName@", ActvName);

                            if (!string.IsNullOrEmpty(RecvEmail))
                            {
                                //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName, 5);
                                ExceptionLogging.SendEmailToAssigneeByUser(RecvEmail, "", replacements, dt_EmailConfig, ActvName, 6);
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


        protected void SalesDetailsGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (Session["export"] == null)
            {
                LinkButton lnkReassign = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkReassign");

                LinkButton lnkProduct = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkProduct");
                Label lblProduct = (Label)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblProduct");


                Label lblProductClass = (Label)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblProductClass");
                LinkButton lnkProductClass = (LinkButton)SalesDetailsGrid.FindRowCellTemplateControl(e.VisibleIndex, null, "lnkProductClass");
                string fl = Convert.ToString(e.GetValue("FLAG"));
                if (fl == "N")
                { e.Row.Cells[6].Attributes.Add("style", "color:Red;font-weight:bold"); }

                string sls_assignedTo = Convert.ToString(e.GetValue("sls_assignedTo"));
                string type = Convert.ToString(e.GetValue("act_Type"));
                string act_assignedTo = Convert.ToString(e.GetValue("act_assignedTo"));
                string ProductName = Convert.ToString(e.GetValue("ProductName"));
                string ProductClassName = Convert.ToString(e.GetValue("ProductClasName"));
               string sls_id = Convert.ToString(e.GetValue("sls_id"));
                string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);
                string ProductMultipleName = Convert.ToString(e.GetValue("MultipleProduct"));
                string ProductMultipleClassName = Convert.ToString(e.GetValue("MultipleProductClassName"));
                if ((sls_assignedTo == UserId) || (act_assignedTo == UserId))
                {
                    lnkReassign.Visible = false; 

                  
                }
                else
                {
                    lnkReassign.Visible = true;
                  //  lnkReassign.OnClientClick = string.Format("ShowDetailReassign('{0}');", sls_id);
                   
                }

                if (type == "2" || type == "3")
                {
                    if (ProductMultipleName.IndexOf(",") > 0)
                    {

                        if (lnkProduct != null)
                        { lnkProduct.Visible = true; }
                        if (lblProduct != null)
                        {
                            lblProduct.Visible = false;
                        }
                    }
                    else
                    {
                        if (lnkProduct != null)
                        { lnkProduct.Visible = false; }
                        if (lblProduct != null)
                        {
                            lblProduct.Visible = true;
                            lblProduct.Text = ProductMultipleName;
                        }
                    }

                    if (ProductMultipleClassName.IndexOf(",") > 0)
                    {
                        if (lnkProductClass != null)
                        {
                            lnkProductClass.Visible = true;
                        }
                        if (lblProductClass != null)
                        {
                            lblProductClass.Visible = false;
                        }

                    }
                    else
                    {
                        if (lnkProductClass != null)
                        {
                            lnkProductClass.Visible = false;
                        }
                        if (lblProductClass != null)
                        {
                            lblProductClass.Visible = true;
                            lblProductClass.Text = ProductMultipleClassName;
                        }
                    }

                }
                else
                {
                    if (lnkProduct != null)
                    {
                        lnkProduct.Visible = false;
                    }
                    if (lblProduct != null)
                    {
                        lblProduct.Visible = true;
                        lblProduct.Text = ProductName;
                    }
                    if (lblProductClass != null)
                    {

                        lnkProductClass.Visible = false;
                        lblProductClass.Visible = true;
                        lblProductClass.Text = ProductClassName;
                    }
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

            exporter.GridViewID = "SalesDetailsGrid";
            exporter.FileName = "AssignedSalesActivityDetails";
            exporter.PageHeader.Left = "Assigned Sales Activity Details";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            //SalesDetailsGrid.Columns[12].Visible = false;
            //SalesDetailsGrid.Columns[13].Visible = false;
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

        //[WebMethod]
        //public static List<string> GetAllUserListBeforeSelect(string UserId)
        //{
        //    StringBuilder filter = new StringBuilder();
        //    Employee_BL objemployeebal = new Employee_BL();
        //    string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
        //    DataTable dtbl = new DataTable();

        //    dtbl = objemployeebal.GetReassignUser(owninterid, UserId);


        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["cnt_id"]));
        //    }



        //    return obj;
        //}

        protected void AspxProductGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable DtProduct = new DataTable();
            DtProduct = objBL.GetProductByActivity(Convert.ToString(e.Parameters));


            AspxProductGrid.DataSource = DtProduct;
            AspxProductGrid.DataBind();
        }
        protected void AspxProductclassGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable DtProduct = new DataTable();
            DtProduct = objBL.GetProductCellsByActivity(Convert.ToString(e.Parameters));


            ASPxGridProductClass.DataSource = DtProduct;
            ASPxGridProductClass.DataBind();
        }


    }
}