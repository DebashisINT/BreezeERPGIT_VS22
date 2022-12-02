using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ERP.OMS.Management.Master
{
    public partial class eInvoice_configuration : System.Web.UI.Page
    {
        eInvoice_ConfigurationBL objBL = new eInvoice_ConfigurationBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL cSOrder = new CommonBL();
            DefaultEinvoiceSellerAddress.Value = cSOrder.GetSystemSettingsResult("DefaultEinvoiceSellerAddress");
            MasterSettings objmaster = new MasterSettings();
            hdnActiveEInvoice.Value = objmaster.GetSettings("ActiveEInvoice");

            if (hdnActiveEInvoice.Value == "0")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
            }

            if (!IsPostBack)
            {
                Session["bindCompanyBranch"] = null;
                BindCompanyBranch();
                bindGSP();
            }
        }

        public void bindGSP()
        {
            GSPOnBoardingBL posSale = new GSPOnBoardingBL();
            DataTable dtGSP = posSale.getGSPValue();
            ddlGSP.DataSource = dtGSP;
            ddlGSP.DataValueField = "GSP_CODE";
            ddlGSP.DataTextField = "GSP_NAME";
            ddlGSP.DataBind();
            ddlGSP.SelectedIndex = 0;
        }

        public void BindCompanyBranch()
        {
            DataSet ds = objBL.BindCompanyBranch("ALL");
            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranch.DataSource = ds.Tables[0];
                    ddlBranch.DataValueField = "CompanyBranch_Id";
                    ddlBranch.DataTextField = "Entity";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
                }

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    ddlCompany.DataSource = ds.Tables[1];
                    ddlCompany.DataValueField = "CompanyBranch_Id";
                    ddlCompany.DataTextField = "Entity";
                    ddlCompany.DataBind();
                    ddlCompany.SelectedIndex = 0;
                }

                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    ddlUserGroup.DataSource = ds.Tables[2];
                    ddlUserGroup.DataValueField = "grp_id";
                    ddlUserGroup.DataTextField = "grp_name";
                    ddlUserGroup.DataBind();
                    ddlUserGroup.SelectedIndex = 0;
                }
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["bindCompanyBranch"] != null)
            {
                GrdDevice.DataSource = (DataTable)Session["bindCompanyBranch"];
            }
            else
            {
                GrdDevice.DataSource = null;
            }
        }

        protected void GrdDevice_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //if (Session["bindCompanyBranch"] != null)
            {
                GrdDevice.DataBind();
            }
        }

        [WebMethod]
        public static String changesbind(string Type)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
                proc.AddPara("@Action", Convert.ToString(Type));
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    HttpContext.Current.Session["bindCompanyBranch"] = dt;
                }
                else
                {
                    HttpContext.Current.Session["bindCompanyBranch"] = null;
                }
                return "OK";
            }
            else
            {
                return "Logout";
            }
        }

        protected void lookup_SalesDiscount_DataBinding(object sender, EventArgs e)
        {
            if (Session["SalesDiscount"] != null)
            {
                lookup_SalesDiscount.DataSource = (DataTable)Session["SalesDiscount"];

            }
        }

        protected void PanelSalesDiscount_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindSalesDiscountGrid")
            {
                PanelSalesDiscount.JSProperties["cpEdit"] = null;

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable GRNOverhead = oDBEngine.GetDataTable("select Taxes_ID,Taxes_Code,Taxes_Description from Master_Taxes where Taxes_ApplicableFor in ('S','B') AND TaxTypeCode='O' ");

                //DataTable BindValue = oDBEngine.GetDataTable("select top(1)SalesDiscount_id from Einvoice_ChargesMap");


                lookup_SalesDiscount.GridView.Selection.CancelSelection();

                if (GRNOverhead != null && GRNOverhead.Rows.Count > 0)
                {
                    Session["SalesDiscount"] = GRNOverhead;
                    lookup_SalesDiscount.DataSource = GRNOverhead;
                    lookup_SalesDiscount.DataBind();
                }
                else
                {
                    Session["SalesDiscount"] = null;
                    lookup_SalesDiscount.DataSource = GRNOverhead;
                    lookup_SalesDiscount.DataBind();
                }

                //if (BindValue != null && BindValue.Rows.Count > 0)
                //{
                //   // PanelSalesDiscount.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["SalesDiscount_id"]);
                //}
            }
            else if (e.Parameter.Split('~')[0] == "SetSalesDiscountGrid")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable BindValue = oDBEngine.GetDataTable("select top(1)SalesDiscount_id from Einvoice_ChargesMap");

                if (BindValue != null && BindValue.Rows.Count > 0 && Convert.ToString(BindValue.Rows[0]["SalesDiscount_id"]) != "")
                {
                    string[] eachQuo = Convert.ToString(BindValue.Rows[0]["SalesDiscount_id"]).Split(',');
                    if (eachQuo.Length > 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_SalesDiscount.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_SalesDiscount.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }

                    // PanelSalesDiscount.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["SalesDiscount_id"]);
                }
            }
        }

        protected void lookup_SalesRoundOff_DataBinding(object sender, EventArgs e)
        {
            if (Session["SalesRoundOff"] != null)
            {
                lookup_SalesRoundOff.DataSource = (DataTable)Session["SalesRoundOff"];

            }
        }

        protected void PanelSalesRoundOff_Callback(object sender, CallbackEventArgsBase e)
        {

            if (e.Parameter.Split('~')[0] == "BindSalesRoundOffGrid")
            {
                PanelSalesRoundOff.JSProperties["cpEdit"] = null;
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable SalesRoundOff = oDBEngine.GetDataTable("select Taxes_ID,Taxes_Code,Taxes_Description from Master_Taxes where Taxes_ApplicableFor in ('S','B') AND TaxTypeCode='O' ");

                lookup_SalesRoundOff.GridView.Selection.CancelSelection();

                if (SalesRoundOff != null && SalesRoundOff.Rows.Count > 0)
                {
                    Session["SalesRoundOff"] = SalesRoundOff;
                    lookup_SalesRoundOff.DataSource = SalesRoundOff;
                    lookup_SalesRoundOff.DataBind();
                }
                else
                {
                    Session["GRNOverhead"] = null;
                    lookup_SalesRoundOff.DataSource = SalesRoundOff;
                    lookup_SalesRoundOff.DataBind();
                }

                //DataTable BindValue = oDBEngine.GetDataTable("select top(1)SalesRoundOff_id from Einvoice_ChargesMap");
                //if (BindValue != null && BindValue.Rows.Count > 0)
                //{
                //    PanelSalesRoundOff.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["SalesRoundOff_id"]);
                //}
            }
            else if (e.Parameter.Split('~')[0] == "SetSalesRoundOffGrid")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable BindValue = oDBEngine.GetDataTable("select top(1)SalesRoundOff_id from Einvoice_ChargesMap");

                if (BindValue != null && BindValue.Rows.Count > 0 && Convert.ToString(BindValue.Rows[0]["SalesRoundOff_id"]) != "")
                {
                    string[] eachQuo = Convert.ToString(BindValue.Rows[0]["SalesRoundOff_id"]).Split(',');
                    if (eachQuo.Length > 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_SalesRoundOff.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                    else if (eachQuo.Length == 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_SalesRoundOff.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                }
            }
        }

        protected void lookup_SalesTCS_DataBinding(object sender, EventArgs e)
        {
            if (Session["SalesTCS"] != null)
            {
                lookup_SalesTCS.DataSource = (DataTable)Session["SalesTCS"];

            }
        }

        protected void PanelSalesTCS_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindSalesTCSGrid")
            {
                PanelSalesTCS.JSProperties["cpEdit"] = null;
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable SalesTCS = oDBEngine.GetDataTable("select Taxes_ID,Taxes_Code,Taxes_Description from Master_Taxes where Taxes_ApplicableFor in ('S','B') AND TaxTypeCode='O' ");

                lookup_SalesTCS.GridView.Selection.CancelSelection();

                if (SalesTCS != null && SalesTCS.Rows.Count > 0)
                {
                    Session["SalesTCS"] = SalesTCS;
                    lookup_SalesTCS.DataSource = SalesTCS;
                    lookup_SalesTCS.DataBind();
                }
                else
                {
                    Session["SalesTCS"] = null;
                    lookup_SalesTCS.DataSource = SalesTCS;
                    lookup_SalesTCS.DataBind();
                }

                //DataTable BindValue = oDBEngine.GetDataTable("select top(1)SalesOtherCharges_id from Einvoice_ChargesMap");
                //if (BindValue != null && BindValue.Rows.Count > 0)
                //{
                //    PanelSalesTCS.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["SalesOtherCharges_id"]);
                //}
            }
            else if (e.Parameter.Split('~')[0] == "SetSalesTCSGrid")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable BindValue = oDBEngine.GetDataTable("select top(1)SalesOtherCharges_id from Einvoice_ChargesMap");

                if (BindValue != null && BindValue.Rows.Count > 0 && Convert.ToString(BindValue.Rows[0]["SalesOtherCharges_id"]) != "")
                {
                    string[] eachQuo = Convert.ToString(BindValue.Rows[0]["SalesOtherCharges_id"]).Split(',');
                    if (eachQuo.Length > 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_SalesTCS.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                    else if (eachQuo.Length == 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_SalesTCS.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                }
            }
        }

        protected void lookup_PurchaseDiscount_DataBinding(object sender, EventArgs e)
        {
            if (Session["PurchaseDiscount"] != null)
            {
                lookup_PurchaseDiscount.DataSource = (DataTable)Session["PurchaseDiscount"];
            }
        }

        protected void PanelPurchaseDiscount_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindPurchaseDiscountGrid")
            {
                PanelPurchaseDiscount.JSProperties["cpEdit"] = null;
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable PurchaseDiscount = oDBEngine.GetDataTable("select Taxes_ID,Taxes_Code,Taxes_Description from Master_Taxes where Taxes_ApplicableFor in ('P','B') AND TaxTypeCode='O' ");

                lookup_PurchaseDiscount.GridView.Selection.CancelSelection();
                if (PurchaseDiscount != null && PurchaseDiscount.Rows.Count > 0)
                {
                    Session["PurchaseDiscount"] = PurchaseDiscount;
                    lookup_PurchaseDiscount.DataSource = PurchaseDiscount;
                    lookup_PurchaseDiscount.DataBind();
                }
                else
                {
                    Session["PurchaseDiscount"] = null;
                    lookup_PurchaseDiscount.DataSource = PurchaseDiscount;
                    lookup_PurchaseDiscount.DataBind();
                }

                //DataTable BindValue = oDBEngine.GetDataTable("select top(1)PurchaseDiscount_id from Einvoice_ChargesMap");
                //if (BindValue != null && BindValue.Rows.Count > 0)
                //{
                //    PanelPurchaseDiscount.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["PurchaseDiscount_id"]);
                //}
            }
            else if (e.Parameter.Split('~')[0] == "SetPurchaseDiscountGrid")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable BindValue = oDBEngine.GetDataTable("select top(1)PurchaseDiscount_id from Einvoice_ChargesMap");

                if (BindValue != null && BindValue.Rows.Count > 0 && Convert.ToString(BindValue.Rows[0]["PurchaseDiscount_id"]) != "")
                {
                    string[] eachQuo = Convert.ToString(BindValue.Rows[0]["PurchaseDiscount_id"]).Split(',');
                    if (eachQuo.Length > 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_PurchaseDiscount.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                    else if (eachQuo.Length == 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_PurchaseDiscount.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                }
            }
        }

        protected void lookup_PurchaseRoundoff_DataBinding(object sender, EventArgs e)
        {
            if (Session["PurchaseRoundoff"] != null)
            {
                lookup_PurchaseRoundoff.DataSource = (DataTable)Session["PurchaseRoundoff"];
            }
        }

        protected void PanelPurchaseRoundoff_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindPurchaseRoundoffGrid")
            {
                PanelPurchaseRoundoff.JSProperties["cpEdit"] = null;
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable PurchaseRoundoff = oDBEngine.GetDataTable("select Taxes_ID,Taxes_Code,Taxes_Description from Master_Taxes where Taxes_ApplicableFor in ('P','B') AND TaxTypeCode='O' ");

                lookup_PurchaseRoundoff.GridView.Selection.CancelSelection();

                if (PurchaseRoundoff != null && PurchaseRoundoff.Rows.Count > 0)
                {
                    Session["PurchaseRoundoff"] = PurchaseRoundoff;
                    lookup_PurchaseRoundoff.DataSource = PurchaseRoundoff;
                    lookup_PurchaseRoundoff.DataBind();
                }
                else
                {
                    Session["PurchaseRoundoff"] = null;
                    lookup_PurchaseRoundoff.DataSource = PurchaseRoundoff;
                    lookup_PurchaseRoundoff.DataBind();
                }

                //DataTable BindValue = oDBEngine.GetDataTable("select top(1)PurchaseRoundOff_id from Einvoice_ChargesMap");
                //if (BindValue != null && BindValue.Rows.Count > 0)
                //{
                //    PanelPurchaseRoundoff.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["PurchaseRoundOff_id"]);
                //}
            }
            else if (e.Parameter.Split('~')[0] == "SetPurchaseRoundoffGrid")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable BindValue = oDBEngine.GetDataTable("select top(1)PurchaseRoundOff_id from Einvoice_ChargesMap");

                if (BindValue != null && BindValue.Rows.Count > 0 && Convert.ToString(BindValue.Rows[0]["PurchaseRoundOff_id"]) != "")
                {
                    string[] eachQuo = Convert.ToString(BindValue.Rows[0]["PurchaseRoundOff_id"]).Split(',');
                    if (eachQuo.Length > 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_PurchaseRoundoff.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                    else if (eachQuo.Length == 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_PurchaseRoundoff.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                }
            }
        }

        protected void lookup_PurchaseTCS_DataBinding(object sender, EventArgs e)
        {
            if (Session["PurchaseTCS"] != null)
            {
                lookup_PurchaseTCS.DataSource = (DataTable)Session["PurchaseTCS"];
            }
        }

        protected void PanelPurchaseTCS_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindPurchaseTCSGrid")
            {
                PanelPurchaseTCS.JSProperties["cpEdit"] = null;
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable PurchaseTCS = oDBEngine.GetDataTable("select Taxes_ID,Taxes_Code,Taxes_Description from Master_Taxes where Taxes_ApplicableFor in ('P','B') AND TaxTypeCode='O' ");

                lookup_PurchaseTCS.GridView.Selection.CancelSelection();

                if (PurchaseTCS != null && PurchaseTCS.Rows.Count > 0)
                {
                    Session["PurchaseTCS"] = PurchaseTCS;
                    lookup_PurchaseTCS.DataSource = PurchaseTCS;
                    lookup_PurchaseTCS.DataBind();
                }
                else
                {
                    Session["PurchaseTCS"] = null;
                    lookup_PurchaseTCS.DataSource = PurchaseTCS;
                    lookup_PurchaseTCS.DataBind();
                }

                //DataTable BindValue = oDBEngine.GetDataTable("select top(1)PurchaseOtherCharges_id from Einvoice_ChargesMap");
                //if (BindValue != null && BindValue.Rows.Count > 0)
                //{
                //    PanelPurchaseTCS.JSProperties["cpEdit"] = Convert.ToString(BindValue.Rows[0]["PurchaseOtherCharges_id"]);
                //}
            }
            else if (e.Parameter.Split('~')[0] == "SetPurchaseTCSGrid")
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable BindValue = oDBEngine.GetDataTable("select top(1)PurchaseOtherCharges_id from Einvoice_ChargesMap");

                if (BindValue != null && BindValue.Rows.Count > 0 && Convert.ToString(BindValue.Rows[0]["PurchaseOtherCharges_id"]) != "")
                {
                    string[] eachQuo = Convert.ToString(BindValue.Rows[0]["PurchaseOtherCharges_id"]).Split(',');
                    if (eachQuo.Length > 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_PurchaseTCS.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                    else if (eachQuo.Length == 1)
                    {
                        foreach (string val in eachQuo)
                        {
                            if (val != "")
                            {
                                lookup_PurchaseTCS.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static String SaveTaxCharges(List<string> SalesDiscount, List<string> SalesRoundOff, List<string> SalesTCS, List<string> purchaseDiscount, List<string> purchaseRoundOff, List<string> purchaseTCS)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                String SalesDiscountstr = "";
                int k = 1;
                if (SalesDiscount != null && SalesDiscount.Count > 0)
                {
                    foreach (string item in SalesDiscount)
                    {
                        if (k > 1)
                            SalesDiscountstr = SalesDiscountstr + "," + item;
                        else
                            SalesDiscountstr = item;
                        k++;
                    }
                }

                String SalesRoundOffstr = "";
                k = 1;
                if (SalesRoundOff != null && SalesRoundOff.Count > 0)
                {
                    foreach (string item in SalesRoundOff)
                    {
                        if (k > 1)
                            SalesRoundOffstr = SalesRoundOffstr + "," + item;
                        else
                            SalesRoundOffstr = item;
                        k++;
                    }
                }

                String SalesTCSstr = "";
                k = 1;
                if (SalesTCS != null && SalesTCS.Count > 0)
                {
                    foreach (string item in SalesTCS)
                    {
                        if (k > 1)
                            SalesTCSstr = SalesTCSstr + "," + item;
                        else
                            SalesTCSstr = item;
                        k++;
                    }
                }

                String purchaseDiscountstr = "";
                k = 1;
                if (purchaseDiscount != null && purchaseDiscount.Count > 0)
                {
                    foreach (string item in purchaseDiscount)
                    {
                        if (k > 1)
                            purchaseDiscountstr = purchaseDiscountstr + "," + item;
                        else
                            purchaseDiscountstr = item;
                        k++;
                    }
                }

                String purchaseRoundOffstr = "";
                k = 1;
                if (purchaseRoundOff != null && purchaseRoundOff.Count > 0)
                {
                    foreach (string item in purchaseRoundOff)
                    {
                        if (k > 1)
                            purchaseRoundOffstr = purchaseRoundOffstr + "," + item;
                        else
                            purchaseRoundOffstr = item;
                        k++;
                    }
                }

                String purchaseTCSstr = "";
                k = 1;
                if (purchaseTCS != null && purchaseTCS.Count > 0)
                {
                    foreach (string item in purchaseTCS)
                    {
                        if (k > 1)
                            purchaseTCSstr = purchaseTCSstr + "," + item;
                        else
                            purchaseTCSstr = item;
                        k++;
                    }
                }

                string USER_ID = HttpContext.Current.Session["userid"].ToString();
                eInvoice_ConfigurationBL objBL = new eInvoice_ConfigurationBL();
                int i = 0;
                i = objBL.InsertUpdateTaxCharges("ALL", SalesDiscountstr, SalesRoundOffstr, SalesTCSstr, purchaseDiscountstr, purchaseRoundOffstr, purchaseTCSstr, USER_ID);
                if (i > 0)
                {
                    return "OK";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static object UserGroupChange(string UserGroupID)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                List<userList> lst = new List<userList>();
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
                proc.AddPara("@Action", "FetchUserByUserGroup");
                proc.AddPara("@Group_ID", Convert.ToString(UserGroupID));
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    lst = APIHelperMethods.ToModelList<userList>(dt);
                }
                return lst;
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static object BindUserGroup(string UserGroupID)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                List<userList> lst = new List<userList>();
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
                proc.AddPara("@Action", "BindUserGroup");
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    lst = APIHelperMethods.ToModelList<userList>(dt);
                }
                return lst;
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static String SaveGroupUser(List<userList> selecteduser, String User_Group)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("user_id", typeof(String));

                foreach (var s2 in selecteduser)
                {
                    object[] trow = { s2.value };
                    dt.Rows.Add(trow);
                }


                string USER_ID = HttpContext.Current.Session["userid"].ToString();
                eInvoice_ConfigurationBL objBL = new eInvoice_ConfigurationBL();
                int i = 0;
                i = objBL.InsertUpdateGroupUser("Insert", dt, USER_ID, User_Group);
                if (i > 0)
                {
                    return "OK";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static String UpdateeInvoiceActivation(string companyBranchID, String GSTIN, String eInvoice)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (HttpContext.Current.Session["bindCompanyBranch"] != null)
                {
                    DataTable dt = (DataTable)HttpContext.Current.Session["bindCompanyBranch"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (companyBranchID == item["CompanyBranch_Id"].ToString())
                            {
                                item["GSTIN"] = GSTIN;
                                item["eInvoice"] = eInvoice;
                            }
                        }
                    }

                    HttpContext.Current.Session["bindCompanyBranch"] = dt;
                }
                return "OK";
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static string SaveEInvoiceActivation(String Action)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["bindCompanyBranch"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataTable dtnew = new DataTable();
                    dtnew.Columns.Add("Map_EInvoice_Id", typeof(String));
                    dtnew.Columns.Add("CompanyBranch_Id", typeof(String));
                    dtnew.Columns.Add("GSTIN", typeof(String));
                    dtnew.Columns.Add("eInvoice", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        object[] trow = { dr["Map_EInvoice_Id"], dr["CompanyBranch_Id"], dr["GSTIN"], dr["eInvoice"] };
                        dtnew.Rows.Add(trow);
                    }


                    string USER_ID = HttpContext.Current.Session["userid"].ToString();
                    eInvoice_ConfigurationBL objBL = new eInvoice_ConfigurationBL();
                    int i = 0;
                    i = objBL.InsertUpdateeInvoiceActivation("Insert", dtnew, USER_ID);
                    if (i > 0)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "GridBlank";
                }
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static object GSPOnBoardingSetValues(string Action, String companyBranchType, String companyBranchID)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                List<GSPOnboardings> lst = new List<GSPOnboardings>();
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
                proc.AddPara("@Action", "FetchGSPOnBoarding");
                proc.AddPara("@companyBranchType", companyBranchType);
                proc.AddPara("@companyBranchID", companyBranchID);
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    lst = APIHelperMethods.ToModelList<GSPOnboardings>(dt);
                }
                return lst;
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static string GSPOnBoardingSave(String gsp, String FirsName, String LastName, String MobileNo, String Email, String Password, String OTP, String ApiType,
            String BasicBaseURL, String EnrichedBaseURL, String IRP_API_Version, String IRP_Name, String GSP_API_Version, String Organization_Id,
            string companyBranchID, String eInvoice, String GSTIN, String companyBranchType)
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string output = string.Empty;
            int i = 0;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                    ProcedureExecute proc = new ProcedureExecute("PROC_EinvoiceGSPOnBoarding");
                    proc.AddPara("@Action", "InsertData");
                    proc.AddPara("@GSP_CODE", gsp);
                    proc.AddPara("@FIRST_NAME", FirsName);
                    proc.AddPara("@LAST_NAME", LastName);
                    proc.AddPara("@CONTACT_NO", MobileNo);
                    proc.AddPara("@EMAIL", Email);
                    proc.AddPara("@PASSWORD", Password);
                    proc.AddPara("@OTP", OTP);
                    proc.AddPara("@ApiType", ApiType);
                    proc.AddPara("@USER_ID", user_id);
                    proc.AddPara("@COMPANYID", strCompanyID);
                    proc.AddPara("@FINYEAR", FinYear);

                    proc.AddPara("@BasicBaseURL", BasicBaseURL);
                    proc.AddPara("@EnrichedBaseURL", EnrichedBaseURL);
                    proc.AddPara("@IRP_API_Version", IRP_API_Version);
                    proc.AddPara("@IRP_Name", IRP_Name);
                    proc.AddPara("@GSP_API_Version", GSP_API_Version);
                    proc.AddPara("@Organization_Id", Organization_Id);

                    proc.AddPara("@GSTIN", GSTIN);
                    proc.AddPara("@IsEInvoice", eInvoice);
                    proc.AddPara("@CompanyBranch_Id", companyBranchID);
                    proc.AddPara("@Type", companyBranchType);

                    i = proc.RunActionQuery();
                    if (i > 0)
                    {
                        output = "OK";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            return output;
        }

        [WebMethod]
        public static String DeleteGSPOnBoarding(String Action, String companyBranchType, String companyBranchID)
        {
            String output = "";
            if (HttpContext.Current.Session["userid"] != null)
            {

                string USER_ID = HttpContext.Current.Session["userid"].ToString();
                ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
                proc.AddPara("@Action", Action);
                proc.AddPara("@companyBranchID", companyBranchID);
                proc.AddPara("@companyBranchType", companyBranchType);

                int i = proc.RunActionQuery();
                if (i > 0)
                {
                    output = "OK";
                }
                else
                {
                    output = "";
                }
            }
            else
            {
                output = "Logout";
            }
            return output;
        }

        [WebMethod]
        public static String GSTINSetValues(String Action, String companyBranchType, String companyBranchID)
        {
            String output = "";
            if (HttpContext.Current.Session["userid"] != null)
            {

                string USER_ID = HttpContext.Current.Session["userid"].ToString();
                ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
                proc.AddPara("@Action", Action);
                proc.AddPara("@companyBranchID", companyBranchID);
                proc.AddPara("@companyBranchType", companyBranchType);
                DataTable dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    output = Convert.ToString(dt.Rows[0]["GSTIN"]) + "~" + Convert.ToString(dt.Rows[0]["eInvoice"]);
                }
                else
                {
                    output = "";
                }
            }
            else
            {
                output = "Logout";
            }
            return output;
        }
    }

    public class userList
    {
        public String item { get; set; }
        public String value { get; set; }
        public bool selected { get; set; }
        public String User_Group { get; set; }
    }

    public class GSPOnboardings
    {
        public String GSP_CODE { get; set; }
        public String FIRST_NAME { get; set; }
        public String LAST_NAME { get; set; }
        public String CONTACT_NO { get; set; }
        public String EMAIL { get; set; }
        public String PASSWORD { get; set; }
        public String OTP { get; set; }
        public String ApiType { get; set; }
        public String BasicBaseURL { get; set; }
        public String EnrichedBaseURL { get; set; }
        public String IRP_API_Version { get; set; }
        public String IRP_Name { get; set; }
        public String GSP_API_Version { get; set; }
        public String Organization_Id { get; set; }

    }
}