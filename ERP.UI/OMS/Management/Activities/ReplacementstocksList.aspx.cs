using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;

using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using BusinessLogicLayer.Replacement;


namespace ERP.OMS.Management.Activities
{
    public partial class ReplacementstocksList : ERP.OMS.ViewState_class.VSPage
    {
        Replacement objreplacement1 = new Replacement();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;

        public string keystokinorout = "";
        #region Page Load Section Start
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);

                //     Bind_BranchCombo();

            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ReplacementstocksList.aspx");

            #region Jsproperties Section Initialized Section Start
            GrdReplacement.JSProperties["cpinsert"] = null;
            GrdReplacement.JSProperties["cpEdit"] = null;
            GrdReplacement.JSProperties["cpUpdate"] = null;
            GrdReplacement.JSProperties["cpDelete"] = null;
            GrdReplacement.JSProperties["cpExists"] = null;
            GrdReplacement.JSProperties["cpUpdateValid"] = null;
            #endregion Jsproperties Section Initialized Section Start

            if (!IsPostBack)
            {
                Bind_BranchCombo();
                GetReplacementListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));


            }
        }
        #endregion Page Load Section End



        #region Main Grid Event Section Start
        public void GetReplacementListGridData(string userbranch, string lastCompany)
        {
            DataTable dtdata = new DataTable();
            string finYear = Convert.ToString(Session["LastFinYear"]);
            dtdata = objreplacement1.GetReplacementListProductList(userbranch, lastCompany, finYear);

            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GrdReplacement.DataSource = dtdata;
                GrdReplacement.DataBind();
            }
            else
            {
                GrdReplacement.DataSource = null;
                GrdReplacement.DataBind();
            }

        }


        protected void gridReplacement_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "Sl")
            {
                e.DisplayText = (e.VisibleRowIndex + 1).ToString();
            }
        }


        private void Bind_BranchCombo()
        {

            DataTable data = objreplacement1.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (data != null && data.Rows.Count > 0)
            {
                //CmbBranch.Items.Clear();
                CmbBranch.DataSource = data;
                CmbBranch.TextField = "branch_code";
                CmbBranch.ValueField = "branch_id";
                CmbBranch.DataBind();
                //  CmbBranch.Items.Insert(0, new ListEditItem("Select Branch", 0));
            }
        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string SaveAssignedBranch(Branchassign_replacement assignbranch)   ///If require use static
        {
            try
            {
                assignbranch.financial_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                assignbranch.assign_from_branch = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                assignbranch.assigned_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                assignbranch.company_Id = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

                return Replacement.AssignedBranch((object)assignbranch);
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }


        protected void GrdReplacement_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            GrdReplacement.JSProperties["cpinsert"] = null;
            GrdReplacement.JSProperties["cpEdit"] = null;
            GrdReplacement.JSProperties["cpUpdate"] = null;
            GrdReplacement.JSProperties["cpDelete"] = null;
            GrdReplacement.JSProperties["cpExists"] = null;
            GrdReplacement.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;

            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (WhichCall == "Edit")
            {

                DataTable dtQuotationStatus = objCRMSalesDtlBL.GetQuotationStatusByQuotationID(WhichType);


                if (dtQuotationStatus.Rows.Count > 0 && dtQuotationStatus != null)
                {
                    string quoteid = Convert.ToString(dtQuotationStatus.Rows[0]["quoteid"]);
                    string quoteNumber = Convert.ToString(dtQuotationStatus.Rows[0]["quoteNumber"]);
                    string Status = Convert.ToString(dtQuotationStatus.Rows[0]["Status"]);
                    string Remarks = Convert.ToString(dtQuotationStatus.Rows[0]["Remarks"]);
                    string CustomerName = Convert.ToString(dtQuotationStatus.Rows[0]["CustomerName"]);
                    GrdReplacement.JSProperties["cpEdit"] = quoteid + "~"
                                                    + quoteNumber + "~"
                                                    + Status + "~"
                                                    + Remarks + "~"
                                                    + CustomerName;

                }
            }
            if (WhichCall == "update")
            {
                if (Convert.ToString(e.Parameters).Contains("~"))
                {
                    if (Convert.ToString(e.Parameters).Split('~')[2] != "")
                    {
                        QuoteStatus = Convert.ToString(e.Parameters).Split('~')[2];
                        remarks = Convert.ToString(e.Parameters).Split('~')[3];
                    }
                }
                int dtQuotationStatus = objCRMSalesDtlBL.UpdateQuotationStatusByCustomer(Convert.ToInt32(WhichType), Convert.ToInt32(QuoteStatus), remarks);
                if (dtQuotationStatus == 1)
                {
                    GrdReplacement.JSProperties["cpUpdate"] = "Save Successfully";
                    GetReplacementListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                }
                else
                {
                    GrdReplacement.JSProperties["cpUpdate"] = "Save unsuccessful";
                }


            }
            if (WhichCall == "Delete")
            {

                deletecnt = objreplacement1.DeleteReplacement(WhichType);
                if (deletecnt > 0)
                {
                    GrdReplacement.JSProperties["cpDelete"] = "Deleted successfully";
                    GetReplacementListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                }
                else
                {
                    GrdReplacement.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }

            if (WhichCall == "Display")
            {
                GetReplacementListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));

            }

        }

        #endregion Main Grid Event Section End


        #region Web Method For Child Page Section Start

        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

           // BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }




        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));
            return Convert.ToString(ispermission);
        }

        #endregion Web Method For Child Page Section End

        #region Export Grid Section Start

        public void bindexport(int Filter)
        {
            //GrdReplacement.Columns[6].Visible = false;
            string filename = "Replacement";
            exporter.FileName = filename;
            exporter.FileName = "GrdReplacement";

            exporter.PageHeader.Left = "Replacement Note";
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

        #endregion Export Grid Section End



    }
}