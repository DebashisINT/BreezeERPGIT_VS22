using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.IO;
using ERP.Models;
using System.Linq;
using iTextSharp.text;
namespace ERP.OMS.Management.Activities
{
    public partial class PaymentRequisitionList : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Button Wise Right Access Section Start
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PaymentRequisitionList.aspx");
            #endregion Button Wise Right Access Section End

            CommonBL cSOrder = new CommonBL();
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");



            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule.ToUpper().Trim()== "YES")
                {
                   // GrdOrder.Columns[4].Visible = true;
                    GrdPayReq.Columns[5].Width = 250;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                   // GrdOrder.Columns[4].Visible = false;
                    GrdPayReq.Columns[5].Width = 0;
                }
            }
            if (!IsPostBack)
            {
                Session["Entry_Type"] = null;
                String finyear = "";
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

                FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
                toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
                toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);




                Session["exportval"] = null;
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("PendingApproval");
                Session.Remove("UserWiseERPDocCreation");
                #endregion Session Remove Section End
                //ConditionWiseShowStatusButton();
                #endregion Sandip Section For Approval Dtl Section End
                string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //DataTable watingInvoice = objCRMSalesOrderDtlBL.SalesOrderBasketDetails(userbranchHierachy);

                //waitingOrderCount.Value = Convert.ToString(watingInvoice.Rows.Count);
               // lblQuoteweatingCount.Text = Convert.ToString(watingInvoice.Rows.Count);
               // watingOrdergrid.DataSource = watingInvoice;
               // watingOrdergrid.DataBind();
               // if (objCRMSalesOrderDtlBL.GetUserwiseDocumentFiltered(Convert.ToString(Session["userid"])))
               // {
                //    hdnIsUserwiseFilter.Value = "1";
                //}
                //else
                //{
                //    hdnIsUserwiseFilter.Value = "0";
               // }
               // Session["InvoicePopUpPanel"] = null;

                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                PopulateBranchByHierchy(userbranch);
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
            }
            //#region Sandip Section For Approval Section Start
            //if (divPendingWaiting.Visible == true)
            //{
            //    PopulateUserWiseERPDocCreation();
            //    PopulateApprovalPendingCountByUserLevel();
            //    PopulateERPDocApprovalPendingListByUserLevel();
            //}
            //#endregion Sandip Section For Approval Dtl Section End


        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
      
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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
            //GrdQuotation.Columns[6].Visible = false;
            string filename = "Cash / Fund Requisition";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Cash / Fund Requisition";
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
        protected void GrdPayReq_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string id = Convert.ToString(e.Parameters).Split('~')[1];
            if (WhichCall == "Delete")
            {
                deletedata(id, WhichCall);
                //deletecnt = objCRMSalesOrderDtlBL.DeleteSalesChallan(WhichType);
                //if (deletecnt == 1)
                //{
                //    GrdOrder.JSProperties["cpDelete"] = "Deleted successfully";
                //}
                //else if (WhichCall == "-2")
                //{
                //    GrdOrder.JSProperties["cpDelete"] = "Sales return made.can not delete.";
                //}
                //else
                //{
                //    GrdOrder.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                //}

            }
        }
        public int deletedata(string ID, string Action)
        {
             try
            {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlDataAdapter da = new SqlDataAdapter("select Paymentrequisition_Number from Trans_Paymentreqhead where Paymentreqhead_Id='" + ID + "'",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string docno = string.Empty;
            if (dt.Rows.Count > 0)
            {
                 docno = dt.Rows[0]["Paymentrequisition_Number"].ToString();
            }
            SqlCommand cmd = new SqlCommand("prc_PaymentRequision", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", Action);
            cmd.Parameters.AddWithValue("@docNo", docno);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@GeneratedDocNumber", SqlDbType.VarChar, 50);
            cmd.Parameters["@GeneratedDocNumber"].Direction = ParameterDirection.Output;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
            GrdPayReq.JSProperties["cpReturnValue"] = ReturnValue;
            GrdPayReq.JSProperties["cpDocNo"] = cmd.Parameters["@GeneratedDocNumber"].Value.ToString();
            return ReturnValue;
            }
             catch (Exception ex)
             {
                 GrdPayReq.JSProperties["cpReturnValue"] = ex;
                 return -1;
                 //return GrdPayReq.JSProperties["cpReturnValue"] = ex;
             }
        }
        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }

        protected void gridpaymentreq_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Paymentreqhead_Id";

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            Int64 branchid = Convert.ToInt64(strBranchID);
            string DlvType = Convert.ToString(Request.QueryString["type"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            int User_id = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                    if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                    {
                        //var q = from d in dc.v_GetpaymentRequisitionLists
                        //        where dDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                        //        orderby d.Order_CheckDate descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.v_GetpaymentRequisitionLists
                                where d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                               // && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                               //&& d.Branch_ID == branchid
                                //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                //orderby d.Paymentreqhead_Id descending
                                orderby d.CreatedOn descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        //var q1 = from d in dc.v_GetpaymentRequisitionLists
                        //         where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                        //         && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                        //         orderby d.Order_CheckDate descending
                        //         select d;
                        //e.QueryableSource = q1;
                        var q1 = from d in dc.v_GetpaymentRequisitionLists
                                 where d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                // && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                               // && d.Branch_ID == branchid
                                // && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                 //orderby d.Paymentreqhead_Id descending
                                 orderby d.CreatedOn descending
                                 select d;
                        e.QueryableSource = q1;
                    }



                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                    {
                        //var q = from d in dc.v_GetSalesOrderEntityLists
                        //        where
                        //        d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                        //        branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                        //        orderby d.Order_CheckDate descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.v_GetpaymentRequisitionLists
                                where
                                d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                // && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                && d.Branch_ID == branchid
                                //orderby d.Paymentreqhead_Id descending
                                orderby d.CreatedOn descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        //var q1 = from d in dc.v_GetSalesOrderEntityLists
                        //         where
                        //         d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                        //         branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                        //         orderby d.Order_CheckDate descending
                        //         select d;
                        //e.QueryableSource = q1;

                        var q1 = from d in dc.v_GetpaymentRequisitionLists
                                 where
                                 d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                 //&& branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                 //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                 && d.Branch_ID == branchid
                                // orderby d.Paymentreqhead_Id descending
                                 orderby d.CreatedOn descending
                                 select d;
                        e.QueryableSource = q1;
                    }
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_GetSalesOrderEntityLists
                //        where d.Order_BranchId == -1
                //        orderby d.Order_CheckDate descending
                //        select d;

                //e.QueryableSource = q;

                var q1 = from d in dc.v_GetpaymentRequisitionLists
                         where
                         d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                         //&& branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                         //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                         && d.Branch_ID == branchid
                         //orderby d.Paymentreqhead_Id descending
                         orderby d.CreatedOn descending
                         select d;
                e.QueryableSource = q1;
            }


        }
        //---------------------For Details Grid------------------------
        protected void GrdPayReqdetails_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;
            string availableClosed = Convert.ToString(e.GetValue("IsApprove"));
            //string available = Convert.ToString(e.GetValue("IsCancel"));
            //string availableClosed = Convert.ToString(e.GetValue("IsClosed"));
            if (availableClosed.ToString() == "Rejected")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                e.Row.Font.Strikeout = true;
            }
            else if (availableClosed.ToString() == "Approved")
            {
                e.Row.ForeColor = System.Drawing.Color.Gray;

            }
            //else if (availableClosed.ToUpper() == "TRUE")
            //{

            //    e.Row.ForeColor = System.Drawing.Color.Gray;

            //}
            //else
            //{
            //    e.Row.ForeColor = System.Drawing.Color.Black;
            //}
        }
        protected void cGrdPayReqGrdPayReqdetails(object sender, ASPxGridViewTableRowEventArgs e)
        { }
        protected void GrdPayReqdetails_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            //string id = Convert.ToString(e.Parameters).Split('~')[1];
            //if (WhichCall == "Delete")
            //{
            //    deletedata(id, WhichCall);
                //deletecnt = objCRMSalesOrderDtlBL.DeleteSalesChallan(WhichType);
                //if (deletecnt == 1)
                //{
                //    GrdOrder.JSProperties["cpDelete"] = "Deleted successfully";
                //}
                //else if (WhichCall == "-2")
                //{
                //    GrdOrder.JSProperties["cpDelete"] = "Sales return made.can not delete.";
                //}
                //else
                //{
                //    GrdOrder.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                //}

          //  }
        }
        [WebMethod]
        public static string Rejectrequitision(string keyValue, string Reason, string status)
        {
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string strIsComplete =string.Empty;
            int User_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            SqlCommand cmd = new SqlCommand("prc_updatestatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", status);
            cmd.Parameters.AddWithValue("@Id", keyValue);
            cmd.Parameters.AddWithValue("@Userid", User_id);
            cmd.Parameters.AddWithValue("@Remarks", Reason);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            DataSet dsInst = new DataSet();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            strIsComplete = cmd.Parameters["@ReturnValue"].Value.ToString();
            return strIsComplete;
        }
        protected void GrdPayReqdetails_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Paymentreqhead_Id";

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            Int64 branchid = Convert.ToInt64(strBranchID);
            string DlvType = Convert.ToString(Request.QueryString["type"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            int User_id = Convert.ToInt32(Session["userid"]);
            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

                    if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                    {
                        //var q = from d in dc.v_GetpaymentRequisitionLists
                        //        where dDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                        //        && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                        //        orderby d.Order_CheckDate descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.v_GetpaymentRequisitionDetailsLists
                                where d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                // && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                //&& d.Branch_ID == branchid
                                //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                orderby d.Paymentreqhead_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        //var q1 = from d in dc.v_GetpaymentRequisitionLists
                        //         where d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate)
                        //         && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                        //         orderby d.Order_CheckDate descending
                        //         select d;
                        //e.QueryableSource = q1;
                        var q1 = from d in dc.v_GetpaymentRequisitionDetailsLists
                                 where d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                 // && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                 // && d.Branch_ID == branchid
                                 // && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                 orderby d.Paymentreqhead_Id descending
                                 select d;
                        e.QueryableSource = q1;
                    }



                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    if (Convert.ToString(hdnIsUserwiseFilter.Value) == "0")
                    {
                        //var q = from d in dc.v_GetSalesOrderEntityLists
                        //        where
                        //        d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                        //        branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //        && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                        //        orderby d.Order_CheckDate descending
                        //        select d;
                        //e.QueryableSource = q;
                        var q = from d in dc.v_GetpaymentRequisitionDetailsLists
                                where
                                d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                    // && branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                    //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear
                                && d.Branch_ID == branchid
                                orderby d.Paymentreqhead_Id descending
                                select d;
                        e.QueryableSource = q;
                    }
                    else
                    {
                        //var q1 = from d in dc.v_GetSalesOrderEntityLists
                        //         where
                        //         d.Order_CheckDate >= Convert.ToDateTime(strFromDate) && d.Order_CheckDate <= Convert.ToDateTime(strToDate) &&
                        //         branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                        //         && d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                        //         orderby d.Order_CheckDate descending
                        //         select d;
                        //e.QueryableSource = q1;

                        var q1 = from d in dc.v_GetpaymentRequisitionDetailsLists
                                 where
                                 d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                                     //&& branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                                     //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                                 && d.Branch_ID == branchid
                                 orderby d.Paymentreqhead_Id descending
                                 select d;
                        e.QueryableSource = q1;
                    }
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                //var q = from d in dc.v_GetSalesOrderEntityLists
                //        where d.Order_BranchId == -1
                //        orderby d.Order_CheckDate descending
                //        select d;

                //e.QueryableSource = q;

                var q1 = from d in dc.v_GetpaymentRequisitionDetailsLists
                         where
                         d.Date >= Convert.ToDateTime(strFromDate) && d.Date <= Convert.ToDateTime(strToDate)
                             //&& branchidlist.Contains(Convert.ToInt32(d.Order_BranchId))
                             //&& d.Order_CompanyID == lastCompany && d.Order_FinYear == FinYear && d.CreatedBy == User_id
                         && d.Branch_ID == branchid
                         orderby d.Paymentreqhead_Id descending
                         select d;
                e.QueryableSource = q1;
            }


        }
        //-------------------------------------------------------


        protected void gridPayReq_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;
            string availableClosed = Convert.ToString(e.GetValue("IsApprove"));
            //string available = Convert.ToString(e.GetValue("IsCancel"));
            //string availableClosed = Convert.ToString(e.GetValue("IsClosed"));
            if (availableClosed.ToString() == "Rejected")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
                e.Row.Font.Strikeout = true;
            }
            else if (availableClosed.ToString() == "Approved")
            {
                e.Row.ForeColor = System.Drawing.Color.Gray;

            }
            //else
            //{
            //    e.Row.ForeColor = System.Drawing.Color.Black;
            //}

        }
        
    }
}