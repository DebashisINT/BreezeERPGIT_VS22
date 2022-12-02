using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Collections;
using DevExpress.Web.Data;
using System.ComponentModel;
using EntityLayer;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Drawing;
using ERP.OMS.Tax_Details.ClassFile;
using System.Net;
using ERP.OMS.ViewState_class;
using ERP.Models;
using System.Web.Script.Services;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Activities
{
    public partial class PosSalesinvoiceAdminEdit : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string userbranch = "";
        PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();
        protected void Page_Init(object sender, EventArgs e)
        {
            PaymentDetails.doc_type = "POS";
            if (Request.QueryString["key"] != "ADD")
            {
                PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["id"]));
            }
     
        }
        public void BindBranch()
        {
            try
            {
                string constr = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlConnection con = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter("select BANKBRANCH_ID,BANKBRANCH_NAME from ( SELECT BRANCH_id AS BANKBRANCH_ID , " +
                                                       "RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME FROM TBL_MASTER_BRANCH where BRANCH_id " +
                                                       " in(" + Convert.ToString(Session["userbranchHierarchy"]) + "))tbl order by BANKBRANCH_NAME asc", con);
                DataTable dtfillbranch = new DataTable();
                da.Fill(dtfillbranch);
                ddl_Branch.DataSource = dtfillbranch;
                ddl_Branch.DataBind();
            }
            catch(Exception ex)
            { 
            
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)    
            {
                btnmanualReceipt.Enabled = false;               
                if (Request.QueryString["id"] != null)
                {
                    string ID = Request.QueryString["id"];
                    SetInvoiceDetails(ID);
                    divSelectField.Style.Add("display", "block");
                    divUpdateButton.Style.Add("display", "block");
                    BindBranch();
                    getposforgst();
                }
                else
                {

                }

                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);

                }
                GetAllDropDownDetailForSalesQuotation(userbranch);
            }
        }
        public void getposforgst()
        {
            string constr = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(constr);
            var invoiceid = Request.QueryString["id"];
            SqlDataAdapter da = new SqlDataAdapter("select PosForGst from tbl_trans_salesinvoice where Invoice_Id='" + invoiceid.ToString()+ "'", con);
            DataTable dtfillbranch = new DataTable();
            da.Fill(dtfillbranch);
            hdnposforgst.Value = dtfillbranch.Rows[0]["PosForGst"].ToString();
        }
        //-----------------------------new-------------------
        public class AllDetailsByBranch
        {
            public List<KeyValueClass> SalesMan { get; set; }
            public List<KeyValueClass> ChallanNumberScheme { get; set; }
            public List<KeyValueClass> Financer { get; set; }
            public List<KeyValueClass> Executive { get; set; }
        }
        public class KeyValueClass
        {
            public string Id { get; set; }
            public String Name { get; set; }
            public string otherDetails { get; set; }
        }
        //Rev Rajdip
        [WebMethod]
        public static object GetsalesinvoiceproductsDetails(string invoiceid)
        {
            // PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();
            //string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            DataSet AllDetailsByBranch = GetsalesinvoiceproductsDetail(invoiceid);
            //SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
            AllDetailsbyinvoiceproductsDetails allDetailsByBranch = new AllDetailsbyinvoiceproductsDetails();
            DataTable salsemanData = AllDetailsByBranch.Tables[0];
            List<Getsalesinvoice> SalesMan = new List<Getsalesinvoice>();
            SalesMan = (from DataRow dr in salsemanData.Rows
                        select new Getsalesinvoice()
                        {
                            TotalQuantity = dr["TotalQuantity"].ToString(),
                            TaxableAmount = dr["TaxableAmount"].ToString(),
                            TaxAmount = dr["TaxAmount"].ToString(),
                            AmountWithTax = dr["AmountWithTax"].ToString(),
                            //POSForGSt = dr["PosForGst"].ToString()
                        }).ToList();

            allDetailsByBranch.DetailsofInvoice = SalesMan;
            return allDetailsByBranch;
        }
        public class AllDetailsbyinvoiceproductsDetails
        {
            public List<Getsalesinvoice> DetailsofInvoice { get; set; }
        }
        public class Getsalesinvoice
        {
            public string TotalQuantity { get; set; }
            public string TaxableAmount { get; set; }
            public string TaxAmount { get; set; }
            public string AmountWithTax { get; set; }
            //public string POSForGSt { get; set; }
        }
        public static DataSet GetsalesinvoiceproductsDetail(string Invoiceid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_GetsalesinvoiceproductsDetails");
            proc.AddVarcharPara("@Action", 100, "Getdetails");
            proc.AddVarcharPara("@InvoiceId", 10, Invoiceid);
            //proc.AddVarcharPara("@branch", 4000, BranchID);
            //proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 4000, BranchID);
            //proc.AddVarcharPara("@FinYear", 100, strFinYear);
            //proc.AddVarcharPara("@Type", 100, "10");
            ds = proc.GetDataSet();
            return ds;
        }
        //End Rev Rajdip
        #region populate all data with Branch
        [WebMethod]
        public static object GetAllDetailsByBranch(string BranchId, string EntryType,string Invoiceid)
        {
           // PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
   
            DataSet AllDetailsByBranch = GetAllDetailsByBranch(Invoiceid); //PosData.GetAllDetailsByBranch(BranchId, strCompanyID, FinYear);

            //SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();

            AllDetailsByBranch allDetailsByBranch = new AllDetailsByBranch();

            DataTable salsemanData = AllDetailsByBranch.Tables[0];

            List<KeyValueClass> SalesMan = new List<KeyValueClass>();
            SalesMan = (from DataRow dr in salsemanData.Rows
                        select new KeyValueClass()
                        {
                            Id = dr["cnt_internalId"].ToString(),
                            Name = dr["Name"].ToString()
                        }).ToList();

            allDetailsByBranch.SalesMan = SalesMan;





            DataTable Schemadt = AllDetailsByBranch.Tables[1]; // objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, BranchId, FinYear, "10", "Y");

            List<KeyValueClass> ChallanNumberScheme = new List<KeyValueClass>();
            ChallanNumberScheme = (from DataRow dr in Schemadt.Rows
                                   select new KeyValueClass()
                                   {
                                       Id = dr["Id"].ToString(),
                                       Name = dr["SchemaName"].ToString()
                                   }).ToList();

            allDetailsByBranch.ChallanNumberScheme = ChallanNumberScheme;

            DataTable Financer = AllDetailsByBranch.Tables[2];

            List<KeyValueClass> financer = new List<KeyValueClass>();
            financer = (from DataRow dr in Financer.Rows
                        select new KeyValueClass()
                        {
                            Id = dr["cnt_internalId"].ToString(),
                            Name = dr["cnt_firstName"].ToString(),
                            otherDetails = Convert.ToString(dr["cnt_mainAccount"])
                        }).ToList();

            allDetailsByBranch.Financer = financer;


            DataTable Executive = AllDetailsByBranch.Tables[3];
            List<KeyValueClass> executive = new List<KeyValueClass>();
            executive = (from DataRow dr in Executive.Rows
                         select new KeyValueClass()
                         {
                             Id = dr["cnt_internalId"].ToString(),
                             Name = dr["ExecName"].ToString(),
                             otherDetails = Convert.ToString(dr["Fin_InternalId"])
                         }).ToList();

            allDetailsByBranch.Executive = executive;


            return allDetailsByBranch;
        }
#endregion
        public static DataSet GetAllDetailsByBranch(string Invoiceid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoiceGet");
            proc.AddVarcharPara("@Action", 100, "GetAllDetailsByBranch");
            proc.AddVarcharPara("@InvoiceId", 10, Invoiceid);
            //proc.AddVarcharPara("@branch", 4000, BranchID);
            //proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 4000, BranchID);
            //proc.AddVarcharPara("@FinYear", 100, strFinYear);
            //proc.AddVarcharPara("@Type", 100, "10");
            ds = proc.GetDataSet();
            return ds;
        }

        //---------------------------------------------------
        //--------------------Rev Rajdip---------------------
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
            {
            string strSplitCommand = e.Parameters.Split('~')[0];
            //if (strSplitCommand == "Display")
            //{
                DataTable TaxDetailsdt = new DataTable();
                if (Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] == null)
                {
                    Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";
                    //Rev Subhra 09-05-2019
                    //if (lblShippingStateText.Value != "" && lblBillingStateText.Value != "")
                    //{
                    //    if (ddl_PosGst.Value == "S")
                    //    {
                    //        ShippingState = lblShippingStateText.Value;
                    //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //    }
                    //    else
                    //    {
                    //        ShippingState = lblBillingStateText.Value;
                    //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //    }
                    //}
                    //End of Rev

                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU             DELHI                  Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "7" || ShippingState == "31" || ShippingState == "34")
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                        {
                                            dr.Delete();
                                        }
                                    }

                                }
                                else
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();
                            }
                            else
                            {
                                foreach (DataRow dr in TaxDetailsdt.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();

                            }

                        }
                    }

                    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                    if (compGstin[0].Trim() == "")
                    {
                        foreach (DataRow dr in TaxDetailsdt.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                dr.Delete();
                            }
                        }
                        TaxDetailsdt.AcceptChanges();
                    }

                    #endregion






                    //gridTax.DataSource = GetTaxes();
                    var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                    var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                    gridTax.DataSource = taxChargeDataSource;
                    gridTax.DataBind();
                    gridTax.JSProperties["cpJsonChargeData"] = createJsonForChargesTax(TaxDetailsdt);
                }
            //}
            //else if (strSplitCommand == "SaveGst")
            //{
            //    DataTable TaxDetailsdt = new DataTable();
            //    if (Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] != null)
            //    {
            //        TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])];
            //    }
            //    else
            //    {
            //        TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
            //        TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
            //        TaxDetailsdt.Columns.Add("Percentage", typeof(string));
            //        TaxDetailsdt.Columns.Add("Amount", typeof(string));
            //        //ForGst
            //        TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
            //    }
            //    DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
            //    if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
            //    {
            //        if (existingRow.Length > 0)
            //        {
            //            TaxDetailsdt.Rows.Remove(existingRow[0]);
            //        }
            //    }
            //    else
            //    {
            //        if (existingRow.Length > 0)
            //        {
            //            existingRow[0]["Percentage"] = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1] : "0";
            //            existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
            //            existingRow[0]["AltTax_Code"] = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0] : "0";

            //        }
            //        else
            //        {
            //            string GstTaxId = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0] : "0";
            //            string GstPerCentage = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1] : "0";

            //            string GstAmount = txtGstCstVatCharge.Text;
            //            DataRow gstRow = TaxDetailsdt.NewRow();
            //            gstRow["Taxes_ID"] = 0;
            //            gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
            //            gstRow["Percentage"] = GstPerCentage;
            //            gstRow["Amount"] = GstAmount;
            //            gstRow["AltTax_Code"] = GstTaxId;
            //            TaxDetailsdt.Rows.Add(gstRow);
            //        }

            //        Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] = TaxDetailsdt;
            //    }
            //}
        }
        public void GetAllDropDownDetailForSalesQuotation(string userbranch)
        {
            #region Schema Drop Down Start
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            dst = PosData.GetAllDropDownDetailForSalesInvoice(userbranch, strCompanyID, strBranchID);

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "12", "Y");
            //if (Schemadt != null && Schemadt.Rows.Count > 0)
            //{
            //    ddl_numberingScheme.DataTextField = "SchemaName";
            //    ddl_numberingScheme.DataValueField = "Id";
            //    ddl_numberingScheme.DataSource = Schemadt;
            //    ddl_numberingScheme.DataBind();
            //}
            #endregion Schema Drop Down Start

            #region Branch Drop Down Start
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_Branch.DataTextField = "branch_description";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataSource = dst.Tables[1];
                ddl_Branch.DataBind();
                //ddl_Branch.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (Session["userbranchID"] != null)
            {
                if (ddl_Branch.Items.Count > 0)
                {
                    int branchindex = 0;
                    int cnt = 0;
                    foreach (ListItem li in ddl_Branch.Items)
                    {
                        if (li.Value == Convert.ToString(Session["userbranchID"]))
                        {
                            cnt = 1;
                            break;
                        }
                        else
                        {
                            branchindex += 1;
                        }
                    }
                    if (cnt == 1)
                    {
                        ddl_Branch.SelectedIndex = branchindex;
                    }
                    else
                    {
                        ddl_Branch.SelectedIndex = cnt;
                    }
                }
            }

            #endregion Branch Drop Down End

            #region Delivered from Drop Down Start
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                //ddDeliveredFrom.DataTextField = "branch_description";
                //ddDeliveredFrom.DataValueField = "branch_id";
                //ddDeliveredFrom.DataSource = dst.Tables[1];
                //ddDeliveredFrom.DataBind();
            }
            //if (Session["userbranchID"] != null)
            //{
            //    if (ddDeliveredFrom.Items.Count > 0)
            //    {
            //        int branchindex = 0;
            //        int cnt = 0;
            //        foreach (ListItem li in ddDeliveredFrom.Items)
            //        {
            //            if (li.Value == Convert.ToString(Session["userbranchID"]))
            //            {
            //                cnt = 1;
            //                break;
            //            }
            //            else
            //            {
            //                branchindex += 1;
            //            }
            //        }
            //        if (cnt == 1)
            //        {
            //            ddDeliveredFrom.SelectedIndex = branchindex;
            //        }
            //        else
            //        {
            //            ddDeliveredFrom.SelectedIndex = cnt;
            //        }
            //    }
            //}

            #endregion Branch Drop Down End

            #region Saleman DropDown Start
            //if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            //{
            //    ddl_SalesAgent.TextField = "Name";
            //    ddl_SalesAgent.ValueField= "cnt_internalId";
            //    ddl_SalesAgent.DataSource = dst.Tables[2];
            //    ddl_SalesAgent.DataBind();
            //}
            //ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));
            #endregion Saleman DropDown End

            #region Currency Drop Down Start

            //if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            //{
            //    ddl_Currency.DataTextField = "Currency_Name";
            //    ddl_Currency.DataValueField = "Currency_ID";
            //    ddl_Currency.DataSource = dst.Tables[3];
            //    ddl_Currency.DataBind();
            //}
            int currencyindex = 1;
            int no = 0;
            //if (Session["ActiveCurrency"] != null)
            //{
            //    if (ddl_Currency.Items.Count > 0)
            //    {
            //        string[] ActCurrency = new string[] { };
            //        string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
            //        ActCurrency = currency.Split('~');
            //        foreach (ListItem li in ddl_Currency.Items)
            //        {
            //            if (li.Value == Convert.ToString(ActCurrency[0]))
            //            {
            //                //ddl_Currency.Items.Remove(li);
            //                no = 1;
            //                break;
            //            }
            //            else
            //            {
            //                currencyindex += 1;
            //            }
            //        }
            //    }
            //    ddl_Currency.Items.Insert(0, new ListItem("Select", "0"));
            //    if (no == 1)
            //    {
            //        ddl_Currency.SelectedIndex = currencyindex;
            //    }
            //    else
            //    {
            //        ddl_Currency.SelectedIndex = no;
            //    }
            //}

            #endregion Currency Drop Down End

            #region TaxGroupType DropDown Start
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataSource = dst.Tables[4];
                ddl_AmountAre.DataBind();
                ddl_AmountAre.Value = "2";
            }
            #endregion TaxGroupType DropDown Start




            #region Financer DropDown Start
            //if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
            //{
            //    cmbFinancer.TextField = "cnt_firstName";
            //    cmbFinancer.ValueField = "cnt_internalId";
            //    cmbFinancer.DataSource = dst.Tables[7];
            //    cmbFinancer.DataBind();
            //}
            #endregion Financer DropDown Start

            #region  productDD
            if (Session["ProductDetailsListPOS"] == null)
                Session["ProductDetailsListPOS"] = dst.Tables[7];

            #endregion Product


            #region  productDD
            if (Session["CustomerDetailsListPOS"] == null)
                Session["CustomerDetailsListPOS"] = dst.Tables[8];

            #endregion Product

        }

        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
        }
        public string createJsonForChargesTax(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }

        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
        }

        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();

            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Taxes Taxes = new Taxes();
                Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                TaxList.Add(Taxes);
            }

            return TaxList;
        }
        public IEnumerable GetTaxes(DataTable DT)
        {
            List<Taxes> TaxList = new List<Taxes>();

            decimal totalParcentage = 0;
            foreach (DataRow dr in DT.Rows)
            {
                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                {
                    totalParcentage += Convert.ToDecimal(dr["Percentage"]);
                }
            }




            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (Convert.ToString(DT.Rows[i]["Taxes_ID"]) != "0")
                {
                    Taxes Taxes = new Taxes();
                    Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                    Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                    Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                    Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                    if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "G")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdAmt.Value);
                    }
                    else if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "N")
                    {
                        //rev rajdip
                        int value = 0;
                   //     HdChargeProdNetAmt.Value = value.ToString();
                        //end rev rajdip
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdNetAmt.Value);
                    }
                    else
                    {
                        Taxes.calCulatedOn = 0;
                    }

                    //Set Amount Value 
                    if (Taxes.Amount == "0.00")
                    {
                        Taxes.Amount = Convert.ToString(Taxes.calCulatedOn * (Convert.ToDecimal(Taxes.Percentage) / 100));
                    }


                    if (Convert.ToString(ddl_AmountAre.Value) == "2")
                    {

                        if (Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "SGST")
                        {
                            decimal finalCalCulatedOn = 0;
                            decimal backProcessRate = (1 + (totalParcentage / 100));
                            finalCalCulatedOn = Taxes.calCulatedOn / backProcessRate;
                            Taxes.calCulatedOn = Math.Round(finalCalCulatedOn);
                        }

                    }


                    TaxList.Add(Taxes);
                }
            }

            return TaxList;
        }

        protected void gridTax_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable TaxDetailsdt = new DataTable();
         //   if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] != null)
            if (Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] != null)
            {
                //TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
                TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])];
            }
            else
            {
                TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                TaxDetailsdt.Columns.Add("Amount", typeof(string));
                //ForGst
                TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
            }

            foreach (var args in e.UpdateValues)
            {
                string TaxID = Convert.ToString(args.Keys["TaxID"]);
                string TaxName = Convert.ToString(args.NewValues["TaxName"]);
                string Percentage = Convert.ToString(args.NewValues["Percentage"]);
                string Amount = Convert.ToString(args.NewValues["Amount"]);

                bool Isexists = false;
                foreach (DataRow drr in TaxDetailsdt.Rows)
                {
                    string OldTaxID = Convert.ToString(drr["Taxes_ID"]);

                    if (OldTaxID == TaxID)
                    {
                        Isexists = true;

                        drr["Percentage"] = Percentage;
                        drr["Amount"] = Amount;

                        break;
                    }
                }

                if (Isexists == false)
                {
                    TaxDetailsdt.Rows.Add(TaxID, TaxName, Percentage, Amount, 0);
                }
            }

            if (cmbGstCstVatcharge.Value != null)
            {
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {

                        existingRow[0]["Percentage"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0]; ;

                    }
                    else
                    {
                        string GstTaxId = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0];
                        string GstPerCentage = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }
                }
            }

            //Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = TaxDetailsdt;
            Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] = TaxDetailsdt;
            
            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlDataAdapter davalidate = new SqlDataAdapter("select Pos_EntryType from tbl_trans_SalesInvoice where Invoice_Id='" + Convert.ToString(Request.QueryString["id"]) + "'", con);
            DataTable dtvalidate = new DataTable();
            davalidate.Fill(dtvalidate);
            if (dtvalidate.Rows[0]["Pos_EntryType"].ToString() == "Crd")
            {
                ModifyQuatation();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('Global Tax Can'tBe For Entry Type CRD');</script>");
            }
        }

        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
               
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }
        public void ModifyQuatation()
        {
            DataTable TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])];
            DataTable dt = new DataTable();
            int strIsComplete = 0;
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("proc_AddEditSalesInvoiceTax", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "Add");
            cmd.Parameters.AddWithValue("@taxdetailsdt", TaxDetailsdt);
         //   cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@Invoice_Id", Request.QueryString["id"]);
            cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
            cmd.Parameters.AddWithValue("@BranchID",  Convert.ToString(ddl_Branch.SelectedValue));
            cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Convert.ToInt32(Session["userid"])));
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dt);
            //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('Global Tax Updated Successfully');</script>");
            strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
            gridTax.JSProperties["cpfinalMsg"] = strIsComplete;
            cmd.Dispose();
            con.Dispose();
        }
        protected void gridTax_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        
        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@InvoiceID", 500, Request.QueryString["id"]);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchID", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }


        public List<Taxes> setChargeCalculatedOn(List<Taxes> gridSource, DataTable taxDt)
        {
            foreach (Taxes taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() == Convert.ToString(dependOn[0]["Taxes_Name"]).Replace("(+)", "").Replace("(-)", "").Trim()))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["SI_TaxDetails" + Convert.ToString(Request.QueryString["id"])] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
        }
        public void PopulateChargeGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadChargeGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }

        //--------------------End Rev Rajdip-----------------
        protected void SetInvoiceDetails(string invoiceId)
       {
           try
           {
               DataSet ds = new DataSet();
               ProcedureExecute proc2 = new ProcedureExecute("prc_posAdminUpdate");
               proc2.AddVarcharPara("@action", 50, "Postingdate");
               proc2.AddVarcharPara("@InvoiceId", 10, invoiceId);
               ds = proc2.GetDataSet();
               DateTime mindate = Convert.ToDateTime(Session["FinYearStart"]);
               DateTime maxdate = Convert.ToDateTime(Session["FinYearEnd"]);
               dt_PLQuote.MinDate = mindate;
               dt_PLQuote.MaxDate = maxdate;
               if (ds.Tables[0].Rows.Count > 0)
               {
                   DataRow invoiceRow2 = ds.Tables[0].Rows[0];
                   if (invoiceRow2["Invoice_Date"].ToString() != "")
                   { dt_PLQuote.Text = invoiceRow2["Invoice_Date"].ToString(); }
               }
               ProcedureExecute proc1 = new ProcedureExecute("prc_posAdminUpdate");
               proc1.AddVarcharPara("@action", 50, "GetCustomer");
               proc1.AddVarcharPara("@InvoiceId", 10, invoiceId);
               ds = proc1.GetDataSet();
               if (ds.Tables[0].Rows.Count > 0)
               {
                   DataRow invoiceRow1 = ds.Tables[0].Rows[0];
                   if (invoiceRow1["cnt_internalId"].ToString() != "") { hdnCustomerId.Value = invoiceRow1["cnt_internalId"].ToString(); }
                   if (invoiceRow1["cnt_firstName"].ToString() != "") { txtCustName.Text = invoiceRow1["cnt_firstName"].ToString(); }
               }
               ProcedureExecute procbranch = new ProcedureExecute("prc_posAdminUpdate");
               procbranch.AddVarcharPara("@action", 50, "Setbranchcode");
               procbranch.AddVarcharPara("@InvoiceId", 10, invoiceId);
               ds = procbranch.GetDataSet();
               if (ds.Tables[0].Rows.Count > 0)
               {
                   DataRow invoiceRow3 = ds.Tables[0].Rows[0];
                   if (invoiceRow3["Invoice_BranchId"].ToString() != "")
                   { ddl_Branch.SelectedValue = invoiceRow3["Invoice_BranchId"].ToString(); }
               }
               ProcedureExecute proc = new ProcedureExecute("prc_posAdminUpdate");
               proc.AddVarcharPara("@action", 50, "GetInvoiceDetails");
               proc.AddVarcharPara("@InvoiceId", 10, invoiceId);
               ds = proc.GetDataSet();

               if (ds.Tables[0].Rows.Count > 0)
               {

                   DataRow invoiceRow = ds.Tables[0].Rows[0];
                   txtInvoiceNumber.Text = invoiceRow["Invoice_Number"].ToString();
                   PaymentDetails.Setbranchvalue(invoiceRow["Invoice_BranchId"].ToString());
                   txtDownPayment.Text = invoiceRow["Pos_downPayment"].ToString();
                   txtEmiCard.Text = invoiceRow["Pos_EmiOther_charges"].ToString();
                   txtProcFee.Text = invoiceRow["Pos_ProcFee"].ToString();
                   hddnsalesmanId.Value = invoiceRow["Invoice_SalesmanId"].ToString();
                   txt_Refference.Text = invoiceRow["Invoice_Reference"].ToString();
                   txtRemarks.Text = invoiceRow["Remarks"].ToString();
                   txtVehicles.Text = invoiceRow["VECHICLE_NO"].ToString();
                   hdnvehicleid.Value = invoiceRow["VECHICLE_ID"].ToString();

                   chkNocommission.Checked = Convert.ToBoolean(invoiceRow["IsNoCommission"]);


                   DateTime invoicedate = Convert.ToDateTime(invoiceRow["Invoice_Date"]);
                   if (invoiceRow["pos_deliveryDate"].ToString() != "")
                   {
                       deliveryDate.MinDate = invoicedate;
                       deliveryDate.Text = invoiceRow["pos_deliveryDate"].ToString();
                   }
                   if (invoiceRow["Pos_EntryType"].ToString() != "Fin")
                   {
                       txtDownPayment.Enabled = false;
                       txtEmiCard.Enabled = false;
                       txtProcFee.Enabled = false;
                   }
                   else
                   {
                       txtDownPayment.Enabled = true;
                       txtEmiCard.Enabled = true;
                       txtProcFee.Enabled = true;
                   }
               }
               if (ds.Tables[1].Rows.Count > 0)
               {
                   DataRow billingRow = ds.Tables[1].Rows[0];
                   txtBillingAddress1.Text = Convert.ToString(billingRow["InvoiceAdd_address1"]);
                   txtBillingAddress2.Text = Convert.ToString(billingRow["InvoiceAdd_address2"]);
                   txtBillingAddress3.Text = Convert.ToString(billingRow["InvoiceAdd_address3"]);
                   txtBillingLandMark.Text = Convert.ToString(billingRow["InvoiceAdd_landMark"]);
                   txtBillingPin.Text = Convert.ToString(billingRow["pin_code"]);
               }
               if (ds.Tables[2].Rows.Count > 0)
               {
                   DataRow billingRow = ds.Tables[2].Rows[0];
                   txtShippingAddress1.Text = Convert.ToString(billingRow["InvoiceAdd_address1"]);
                   txtShippingAddress2.Text = Convert.ToString(billingRow["InvoiceAdd_address2"]);
                   txtShippingAddress3.Text = Convert.ToString(billingRow["InvoiceAdd_address3"]);
                   txtShippingLandmark.Text = Convert.ToString(billingRow["InvoiceAdd_landMark"]);
                   txtShippingPin.Text = Convert.ToString(billingRow["pin_code"]);
               }
           }
           catch (Exception ex)
           { 
           
           }

        }

        protected void btn_Update_Click(object sender, EventArgs e)
        {
            string updateValue = UpdateField.SelectedValue.ToString();
            switch (updateValue)
            {
                case "docNo":
                    UpdateDocumentNumber();
                    break;
                case "PaymnetDetails":
                    UpdatePaymentDetails();
                    break;
                case "BillingShipping":
                    updateBillingShipping();
                    break;
                case "FinanceBlock":
                    UpdateFinancerDetails();
                    break;
                case "repost":
                    RePostBalance();
                    break;
                case "Salesman":
                    UpdateSalesman();
                    break;
                case "Customer":
                    UpdateCustomer();
                    break;
                case "PostingDateofDeliver":
                    UpdatePostingdate();
                    break;
                case "Branch":
                    UpdateBranch();
                    break;
                case "NoCommission":
                    UpdateNoCommission();
                    break;
 
            }
            UpdateField.SelectedIndex = 0;
        }
        protected void UpdateBranch()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateBranch");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddVarcharPara("@BranchId", 10, ddl_Branch.SelectedValue.ToString());
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();
            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        }


        protected void UpdateNoCommission()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateNoCommission");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddPara("@Nocommission", chkNocommission.Checked);
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();
            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        }

        protected void UpdatePostingdate()
        {
            
            string Postingdate = dt_PLQuote.Text.ToString();
            if (Postingdate == "")
            {
                string msg = "Please Enter a Date";
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + msg + "');</script>");
                return;
            }
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdatePostingDate");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddDateTimePara("@Postingdate",dt_PLQuote.Date);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();
            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        }
        protected void UpdateCustomer()
        {
            //if (txt_Refference.Text.ToString() == "")
            //{
            //    string message = "Please Enter Postingdate";
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + message + "');</script>");
            //    return;
            //}
            string CustomerName = txtCustName.Text.ToString();
            string CustomerId = hdnCustomerId.Value.ToString();
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateCustomer");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddVarcharPara("@CustomerId", 30, CustomerId);
            proc.AddVarcharPara("@CustomerName", 30, CustomerName);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();

            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        }
        protected void UpdateSalesman()
        {
            try
            {
                //if (ddl_SalesAgent.SelectedIndex == 0)
                //{
                //    string message = "Please Select salesman";
                //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + message + "');</script>");
                //    return;
                //}
                //if (txt_Refference.Text.ToString() == "")
                //{
                //    string message = "Please Enter Refference";
                //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + message + "');</script>");
                //    return;
                //}
                //if (txtVehicles.Text.ToString() == "")
                //{
                //    string message = "Please Enter Vehicle No";
                //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + message + "');</script>");
                //}
                //if (txtRemarks.Text.ToString() == "")
                //{
                //    string message = "Please Enter Remarks";
                //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + message + "');</script>");
                //}
                string Salesman = ddl_SalesAgent.Value.ToString().Trim();
                string Refference = txt_Refference.Text.ToString().Trim();
                DateTime? DeliveryDate;
                if (deliveryDate.Text.ToString() == "")
                {
                    DeliveryDate = null;
                }
                else
                {
                    DeliveryDate = Convert.ToDateTime(deliveryDate.Date);
                }
                string Vehicles = txtVehicles.Text.ToString().Trim();
                string Remarks = txtRemarks.Text.ToString().Trim();
                string vehiclesid = hdnvehicleid.Value;
                ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
                proc.AddVarcharPara("@action", 50, "UpdateSalesman");
                proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
                proc.AddVarcharPara("@Salesman", 30, Salesman);
                proc.AddVarcharPara("@Refference", 30, Refference);
                proc.AddPara("@DeliveryDate",  DeliveryDate);
                proc.AddVarcharPara("@Remarks", 30, Remarks);
                proc.AddVarcharPara("@Vehicles", 30, Vehicles);
                proc.AddVarcharPara("@VehiclesId", 30, vehiclesid);
                proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
                proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
                proc.RunActionQuery();

                string OutputMsg = proc.GetParaValue("@output").ToString();
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('Salesman is loading..,please try after few seconds');</script>");
            }
        }
        protected void RePostBalance()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "RePostbalance");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();

            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        }
        protected void UpdateFinancerDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateFinanceDetails");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddDecimalPara("@processeingFee", 2, 18, Convert.ToDecimal(txtProcFee.Text));
            proc.AddDecimalPara("@EmiCardCharges", 2, 18, Convert.ToDecimal(txtEmiCard.Text));
            proc.AddDecimalPara("@DownPayment", 2, 18, Convert.ToDecimal(txtDownPayment.Text));
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();

            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");

        }
        protected void updateBillingShipping() 
        {
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateBillingShipping");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@BillingPinCode", 6, txtBillingPin.Text.Trim());
            proc.AddVarcharPara("@Billingaddress1", 80, txtBillingAddress1.Text.Trim());
            proc.AddVarcharPara("@Billingaddress2", 80, txtBillingAddress2.Text.Trim());
            proc.AddVarcharPara("@Billingaddress3", 80, txtBillingAddress3.Text.Trim());
            proc.AddVarcharPara("@Billinglandmark", 80, txtBillingLandMark.Text.Trim());

            proc.AddVarcharPara("@ShippingPinCode", 6, txtShippingPin.Text.Trim());
            proc.AddVarcharPara("@Shippingaddress1", 80, txtShippingAddress1.Text.Trim());
            proc.AddVarcharPara("@Shippingaddress2", 80, txtShippingAddress2.Text.Trim());
            proc.AddVarcharPara("@Shippingaddress3", 80, txtShippingAddress3.Text.Trim());
            proc.AddVarcharPara("@Shippinglandmark", 80, txtShippingLandmark.Text.Trim());

            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();

            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        
        }
        protected void UpdatePaymentDetails()
        {

            DataSet dsInst = new DataSet();
            DataTable paymentDetails = PaymentDetails.GetPaymentTable();


           // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            SqlCommand cmd = new SqlCommand("prc_updatePosAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", "UpdatePaymentDetails");
            cmd.Parameters.AddWithValue("@InvoiceId", Request.QueryString["id"]);
            cmd.Parameters.AddWithValue("@userId", Convert.ToInt32(Session["userid"]));
            cmd.Parameters.AddWithValue("@paymentDetails", paymentDetails);
            cmd.Parameters.Add("@output", SqlDbType.VarChar, 100);
            cmd.Parameters["@output"].Direction = ParameterDirection.Output;
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            string OutputMsg = cmd.Parameters["@output"].Value.ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
            cmd.Dispose();
            con.Dispose();
        }
        protected void UpdateDocumentNumber()
        {
            string InvoiceNumber = txtInvoiceNumber.Text.Trim();

            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateDocumentNumber");
            proc.AddVarcharPara("@InvoiceId", 10, Request.QueryString["id"]);
            proc.AddVarcharPara("@InvoiceNumber", 30, InvoiceNumber);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();

            string OutputMsg = proc.GetParaValue("@output").ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>ShowMsg('" + OutputMsg + "');</script>");
        }

        protected void UpdateManualReceiptNumber()
        {
            string oldReceiptNumber = txtOldReceiptNumber.Text.Trim();
            string newReceiptNUmber = txtNewReceiptNumber.Text.Trim();
            string IbRef = "CPR_" + Session["userid"].ToString() + "_" + Convert.ToString(hdRecPayType.Value) + "_" + newReceiptNUmber.Replace("/", "");
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "UpdateManualReceipt");
            proc.AddVarcharPara("@OldNumber", 30, oldReceiptNumber);
            proc.AddVarcharPara("@NewNumber", 30, newReceiptNUmber);
            proc.AddVarcharPara("@IbRef", 30, IbRef);
            proc.AddIntegerPara("@userId", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@output", 100, "", QueryParameterDirection.Output);
            proc.RunActionQuery();

            string OutputMsg = proc.GetParaValue("@output").ToString();
            lblWrongReceipt.Text = OutputMsg;
            lblWrongReceipt.Visible = true;
            txtNewReceiptNumber.Text = "";
            btnmanualReceipt.Enabled = false;
        }


        [WebMethod]
        public static object GetInvoiceDetails(string invoiceNumber)
        {
            string invoiceId = "";
            ProcedureExecute proc = new ProcedureExecute("prc_updatePosAdmin");
            proc.AddVarcharPara("@action", 50, "GetInvoiceDetails");
            proc.AddVarcharPara("@InvoiceNumber", 30, invoiceNumber);
            DataTable dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                invoiceId = dt.Rows[0]["Invoice_Id"].ToString();
            }
            var returnObject = new { status = "Ok", invoiceId = invoiceId };

            return returnObject;
        }

        protected void ManualReceipt_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            lblWrongReceipt.Visible = false;
            string callBackPara = e.Parameter;
            if (callBackPara == "validateReceiptNumber")
            {

                string OldCustRecPayNumber = txtOldReceiptNumber.Text.Trim();
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_posAdminUpdate");
                proc.AddVarcharPara("@action", 50, "validateCustRecpay");
                proc.AddVarcharPara("@DocumentNumber", 30, OldCustRecPayNumber);
                ds = proc.GetTable();

                if (ds.Rows.Count > 0)
                {
                    txtNewReceiptNumber.Text = OldCustRecPayNumber;
                    btnmanualReceipt.Enabled = true;
                    txtOldReceiptNumber.Enabled = false;
                    hdRecPayType.Value = Convert.ToString(ds.Rows[0]["ReceiptPayment_TransactionType"]).Trim();
                }
                else
                {
                    lblWrongReceipt.Visible = true;
                    hdRecPayType.Value = "";
                }
            }
            else if (callBackPara == "UpdateManualReceipt")
            {
                UpdateManualReceiptNumber();

            }
        }

    }
}