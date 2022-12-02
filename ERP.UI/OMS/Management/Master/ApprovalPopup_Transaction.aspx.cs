using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class ApprovalPopup_Transaction : System.Web.UI.Page
    {
        ApprovalBL appbl = new ApprovalBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_Init(object sender, EventArgs e)
        {
            dsStatus.ConnectionString = Convert.ToString(Session["ErpConnection"]);
            // dsConfirm.ConnectionString = Convert.ToString(Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/ApprovalPopup_Transaction.aspx");
            if (!IsPostBack)
            {
                dtFrom.Date = DateTime.Now;
                dtTo.Date = DateTime.Now; 

                Session["Approval_Data"] = null;

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = oDBEngine.GetDataTable(@"SELECT [STATUS_ID], [STATUS_NAME] FROM [MASTER_APPROVAL_STATUS] WHERE ([STATUS_ID] <>'" + 1 + "')");



                ddlConfirm.DataSource = dt;
                ddlConfirm.TextField = "STATUS_NAME";
                ddlConfirm.ValueField = "STATUS_ID";
                ddlConfirm.DataBind();
                ddlConfirm.SelectedIndex = 0;






                //if (Request.QueryString["ModuleName"] == "AccountHead")
                //{
                //    dvHeader.Visible = false;
                //    Session["Approval_Data"] = appbl.GetListData("AccountHead");
                //    grid.DataBind();
                //}
                //else
                //{
                //    dvHeader.Visible = true;
                //}
               // dvHeader.Visible = false;
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["Approval_Data"] != null)
            {
                grid.DataSource = (DataTable)Session["Approval_Data"];
            }
        }

        private DataTable GetGridData()
        {

            return null;
            //Prc_Approval
        }
        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpStatus"] = null;
            if (e.Parameters.Split('~')[0].ToString() == "BindGrid")
            {
                if (e.Parameters.Split('~')[1].ToString() == "CashBankVoucherPayment")
                {
                   
                    Session["Approval_Data"] = appbl.GetListDataTransaction("CashBankVoucherPayment", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Journal")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("Journal", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "CashBankVoucherReceipt")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("CashBankVoucherReceipt", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "SaleOrder")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("SaleOrder", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "SaleInvoice")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("SaleInvoice", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }


                else if (e.Parameters.Split('~')[1].ToString() == "Posinvoice")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("Posinvoice", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "custpayment")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("custpayment", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "custreceipt")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("custreceipt", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "purchaseorder")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("purchaseorder", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "purchaseinvoice")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("purchaseinvoice", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "VendorPayment")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("VendorPayment", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "VendorReceipt")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("VendorReceipt", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "CustomerDebitNote")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("CustomerDebitNote", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "CustomerCreditNote")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("CustomerCreditNote", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "VendorDebitNote")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("VendorDebitNote", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "VendorCreditNote")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("VendorCreditNote", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }
                else if (e.Parameters.Split('~')[1].ToString() == "SalesReturnNormal")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("SalesReturnNormal", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else if (e.Parameters.Split('~')[1].ToString() == "PurcaseReturn")
                {

                    Session["Approval_Data"] = appbl.GetListDataTransaction("PurcaseReturn", e.Parameters.Split('~')[2].ToString(), e.Parameters.Split('~')[3].ToString(), e.Parameters.Split('~')[4].ToString());
                    grid.DataBind();
                }

                else
                {
                    dvHeader.Visible = true;
                }
            }

            else if (e.Parameters.Split('~')[0].ToString() == "SaveData")
            {
                if (e.Parameters.Split('~')[1].ToString() == "CashBankVoucherPayment")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if(ids!="")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "CashBankVoucherPayment", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("CashBankVoucherPayment", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }

                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }

                    
                }

                else if (e.Parameters.Split('~')[1].ToString() == "Journal")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if(ids!="")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "Journal", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("Journal", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }

                   
                }


                else if (e.Parameters.Split('~')[1].ToString() == "CashBankVoucherReceipt")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "CashBankVoucherReceipt", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("CashBankVoucherReceipt", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                        i++;
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }

                else if (e.Parameters.Split('~')[1].ToString() == "SaleOrder")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "SaleOrder", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("SaleOrder", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                        i++;
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }

                else if (e.Parameters.Split('~')[1].ToString() == "SaleInvoice")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "SaleInvoice", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("SaleInvoice", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "Posinvoice")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "Posinvoice", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("Posinvoice", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }



                else if (e.Parameters.Split('~')[1].ToString() == "custpayment")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "custpayment", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("custpayment", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "custreceipt")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "custreceipt", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("custreceipt", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "purchaseorder")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "purchaseorder", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("purchaseorder", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "purchaseinvoice")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "purchaseinvoice", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("purchaseinvoice", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "VendorPayment")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "VendorPayment", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("VendorPayment", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "VendorReceipt")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "VendorReceipt", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("VendorReceipt", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "CustomerDebitNote")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "CustomerDebitNote", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("CustomerDebitNote", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }

                else if (e.Parameters.Split('~')[1].ToString() == "CustomerCreditNote")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "CustomerCreditNote", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("CustomerCreditNote", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }

                else if (e.Parameters.Split('~')[1].ToString() == "VendorDebitNote")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "VendorDebitNote", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("VendorDebitNote", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }

                else if (e.Parameters.Split('~')[1].ToString() == "VendorCreditNote")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "VendorCreditNote", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("VendorCreditNote", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }

                else if (e.Parameters.Split('~')[1].ToString() == "SalesReturnNormal")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "SalesReturnNormal", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("SalesReturnNormal", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


                else if (e.Parameters.Split('~')[1].ToString() == "PurcaseReturn")
                {
                    string ids = "";
                    int i = 0;
                    foreach (var item in grid.GetSelectedFieldValues("EuniqueId"))
                    {
                        if (i == 0)
                            ids = Convert.ToString(item);
                        else
                            ids = ids + "," + Convert.ToString(item);
                        i++;
                    }

                    if (ids != "")
                    {
                        string FromDate = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
                        string ToDate = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");

                        //dvHeader.Visible = false;
                        appbl.SaveApprovaltransaction(ids, "PurcaseReturn", ddlConfirm.Value.ToString());
                        Session["Approval_Data"] = appbl.GetListDataTransaction("PurcaseReturn", ddlStatus.Value.ToString(), FromDate, ToDate);
                        grid.JSProperties["cpStatus"] = "1";
                        grid.DataBind();
                    }
                    else
                    {
                        grid.JSProperties["cpStatus"] = "2";
                    }


                }


               
                else
                {
                    dvHeader.Visible = true;
                }
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dsConfirm.DataBind();
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ddlConfirm.Items.Clear();
            string status_id = e.Parameter.Split('~')[0].ToString();

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDBEngine.GetDataTable(@"SELECT [STATUS_ID], [STATUS_NAME] FROM [MASTER_APPROVAL_STATUS] WHERE ([STATUS_ID] <>'" + status_id + "')");



            ddlConfirm.DataSource = dt;
            ddlConfirm.TextField = "STATUS_NAME";
            ddlConfirm.ValueField = "STATUS_ID";
            ddlConfirm.DataBind();
            ddlConfirm.SelectedIndex = 0;

        }
    }
}