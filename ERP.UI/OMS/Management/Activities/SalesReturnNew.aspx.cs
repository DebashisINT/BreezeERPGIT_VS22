using BusinessLogicLayer;
using BusinessLogicLayer.Budget;
using BusinessLogicLayer.Replacement;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ERP.OMS.Management.Activities
{
    public partial class SalesReturnNew : ERP.OMS.ViewState_class.VSPage
    {
       Replacement objreplacement1 = new Replacement();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        string strCompanyID = "";
        string strBranchID = "";
        string FinYear = "";
        string userbranch = "";
        string userbranchHierarchy = "";
        DataTable dst = new DataTable();
        string UniqueQuotation = string.Empty;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        public string replcetitle = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                sqltaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                ddl_Branch.Enabled = false;

                strCompanyID = Convert.ToString(Session["LastCompany"]);
                strBranchID = Convert.ToString(Session["userbranchID"]);
                FinYear = Convert.ToString(Session["LastFinYear"]);
                userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                NumberingScheme();
                Branchpopulate();
                dt_date.Value = DateTime.Now;
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                if (Request.QueryString["key"] != "ADD")
                {
                    rights.CanEdit = true;
                    GridReplacementBind(Request.QueryString["key"]);
                    lbltitle.Text = "Modify Replacement Note";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OnAddNewClickMod()", true);
                    ///   
                    hdnsaveorsaveexists.Value = "3";
                }
                else
                {
                    Session["ReplaceMentNotesDetails"] = null;
                    lbltitle.Text = "Add Replacement Note";
                }
            }
        }

        #region Numbering Scheme
        protected void NumberingScheme()
        {


            DataTable Schemadt = objreplacement1.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "13", "Y");

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }


        [WebMethod]
        public static bool CheckUniqueCode(string QuoteNo)
        {
            Replacement objreplacement1 = new Replacement();
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objreplacement1.CheckUnique(QuoteNo, "0", "Replacement");
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }



        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            //   oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.Replacement_Number) FROM tbl_trans_SalesReplacement tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                   // sqlQuery += "?$', LTRIM(RTRIM(tjv.Replacement_Number))) = 1";

                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Replacement_Number))) = 1 and Replacement_Number like '" + prefCompCode + "%'";

                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, Replacement_Date) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Replacement_Number) FROM tbl_trans_SalesReplacement tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                       // sqlQuery += "?$', LTRIM(RTRIM(tjv.Replacement_Number))) = 1";

                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Replacement_Number))) = 1 and Replacement_Number like '" + prefCompCode + "%'";

                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Replacement_Date) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange";
                        }
                        else
                        {
                            paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniqueQuotation = startNo.PadLeft(paddCounter, '0');
                        UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Replacement_Number FROM tbl_trans_SalesReplacement WHERE Replacement_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniqueQuotation = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }


        #endregion

        #region Branch Populate
        protected void Branchpopulate()
        {
            // dst = objreplacement1.GetAllDropDownDetailForSalesInvoice(userbranch, strCompanyID, strBranchID);
            dst = objreplacement1.GetBranch(Convert.ToInt32(HttpContext.Current.Session["userbranchID"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            if (dst.Rows.Count > 0)
            {
                //ddl_Branch.DataTextField = "branch_code";
                //ddl_Branch.DataValueField = "branch_id";
                //ddl_Branch.DataSource = dst.Tables[1];
                //ddl_Branch.DataBind();

                ddl_Branch.DataSource = dst;
                ddl_Branch.DataTextField = "branch_code";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataBind();
                ddl_Branch.SelectedValue = strBranchID;

                //ddl_Branch.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        #endregion

        #region GetUDF
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='REP'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        #endregion

        #region Modify Grid replacement Bind
        public void GridReplacementBind(string ReplacementId)
        {
            DataSet ds = new DataSet();
            ds = objreplacement1.GetReplacementPopulateModify(ReplacementId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_numberingScheme.Enabled = false;
                    txt_replcno.ReadOnly = true;



                    lookup_Customer.ReadOnly = true;
                    lookup_quotation.ReadOnly = true;


                    txt_replcno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Replacement_Number"]);
                    //      dt_date.Value = Convert.ToDateTime(ds.Tables[0].Rows["Replacement_Date"]);
                    string setdate = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["Replacement_Date"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["Replacement_Date"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[0]["Replacement_Date"]).Year);
                    dt_date.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string date2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["Replacement_Date"]).ToString("yyyy-MM-dd");

                    ddl_Branch.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Replacement_BranchId"]);
                    lookup_Customer.Value = Convert.ToString(ds.Tables[0].Rows[0]["Customer_Id"]);



                    DataTable ComponentTable = objreplacement1.GetComponentInvoice(lookup_Customer.Value.ToString(), date2, FinYear, ddl_Branch.SelectedValue);
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData"] = ComponentTable;

                        lookup_quotation.DataSource = ComponentTable;
                        lookup_quotation.DataBind();



                        txt_InvoiceDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["Invoicedate"]);



                        string[] eachReplac = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceIds"]).Split(',');
                        if (eachReplac.Length > 0)//More tha one quotation
                        {
                            //   dt_date.Text = "Multiple Select Quotation Dates";

                            foreach (string val in eachReplac)

                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }

                }





                if (ds.Tables[1].Rows.Count > 0)
                {
                    Session["ReplaceMentNotesDetails"] = ds.Tables[1];
                    gridReplacement.DataSource = ds.Tables[1];
                    gridReplacement.DataBind();
                }
            }

        }

        #endregion

        #region Gridview Product Replacement  Items

        protected void gridReplacement_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];

            if (strSplitCommand == "GridPopulatenull")
            {
                Session["ReplaceMentNotesDetails"] = null;
                gridReplacement.DataSource = null;
                gridReplacement.DataBind();

            }

            if (strSplitCommand == "BindGridOnQuotation")
            {
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                // string strType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                string ComponentDetailsIDs = string.Empty;
                //    string strAction = "";

                for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                {
                    ComponentDetailsIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                }
                ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');

                string strInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);
                if (Session["ReplaceMentNotesDetails"] != null)
                {
                    Session["ReplaceMentNotesDetails"] = null;
                }
                DataTable dt_QuotationDetails = new DataTable();
                dt_QuotationDetails.Clear();
                dt_QuotationDetails = objreplacement1.GetComponentInvoiceProductList(ComponentDetailsIDs, "Grid");

                Session["ReplaceMentNotesDetails"] = dt_QuotationDetails;


                //     gridReplacement.DataSource = objreplacement1.GetReplacementDetails(dt_QuotationDetails);
                gridReplacement.DataSource = dt_QuotationDetails;

                gridReplacement.DataBind();

            }
            else if (strSplitCommand == "Delete")
            {
                if (Session["ReplaceMentNotesDetails"] != null)
                {

                    string ComponentDetailsIDs = string.Empty;
                    for (int i = 0; i < gridReplacement.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        ComponentDetailsIDs += "," + Convert.ToString(gridReplacement.GetSelectedFieldValues("ComponentDetailsID")[i]);
                    }
                    ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');
                    DataTable dt_QuotationDetails = new DataTable();
                    dt_QuotationDetails = objreplacement1.GetComponentInvoiceProductList(ComponentDetailsIDs, "Delete");

                    Session["ReplaceMentNotesDetails"] = dt_QuotationDetails;

                    gridReplacement.DataSource = dt_QuotationDetails;

                    gridReplacement.DataBind();

                }
            }

        }

        protected void gridReplacement_DataBinding(object sender, EventArgs e)
        {
            if (Session["ReplaceMentNotesDetails"] != null)
            {

                DataTable dt = (DataTable)Session["ReplaceMentNotesDetails"];
                gridReplacement.DataSource = dt;

            }
        }

        protected void gridReplacement_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "Sl")
            {
                e.DisplayText = (e.VisibleRowIndex + 1).ToString();
            }
        }


        protected void gridReplacement_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            ReplacementGridsavedata model = new ReplacementGridsavedata();

            string replacementnumber = txt_replcno.Text;
            DateTime Replacementdate = Convert.ToDateTime(dt_date.Date);
            string Branch = ddl_Branch.SelectedValue;
            string Customer = lookup_Customer.Value.ToString();

            string FinYear = String.Empty;
            string User = String.Empty;
            string Company = String.Empty;
            if (Session["LastFinYear"] != null)
            {
                FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            }
            if (Session["userid"] != null)
            {
                User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
            }

            if (Session["LastCompany"] != null)
            {
                Company = Convert.ToString(Session["LastCompany"]);
            }
            string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
            string Validate = "";
            if (replacementnumber == "Auto")
            {
                string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                Validate = checkNMakeJVCode(replacementnumber, Convert.ToInt32(SchemeList[0]));
                if (Validate == "ok")
                {
                    if (UniqueQuotation != "")
                    {
                        replacementnumber = UniqueQuotation;
                    }

                }
            }
            string s_Invoiceid = "";
            List<ReplacementGridData> replacementList = new List<ReplacementGridData>();

            DataTable dtreplace = (DataTable)Session["ReplaceMentNotesDetails"];


            if (Session["ReplaceMentNotesDetails"] == null)
            {
                foreach (var args in e.UpdateValues)
                {

                    string ComponentDetailsIDs = string.Empty;

                    replacementList.Add(new ReplacementGridData()
                    {
                        ProductID = Convert.ToString(args.NewValues["ProductID"]),
                        ComponentNumber = Convert.ToString(args.NewValues["ComponentNumber"]),
                        InputQuantity = Convert.ToString(args.NewValues["QuantityInput"]),
                        Quantity = Convert.ToString(args.NewValues["Quantity"]),
                        Rate = Convert.ToString(args.NewValues["Rate"]),
                        ProductDescription = Convert.ToString(args.NewValues["ProductDescription"]),
                        Serials = Convert.ToString(args.NewValues["ProductSerials"]),
                        InvoicedetailId = Convert.ToInt64(args.NewValues["ComponentDetailsID"]),
                        InvoiceId = Convert.ToString(args.NewValues["Invoice_Id"])
                    });

                    s_Invoiceid = Convert.ToString(args.NewValues["Invoice_Id"]);
                }
            }

            else
            {

                for (int i = 0; i < dtreplace.Rows.Count; i++)
                {
                    replacementList.Add(new ReplacementGridData()
                    {
                        ProductID = Convert.ToString(dtreplace.Rows[i]["ProductID"]),
                        ComponentNumber = Convert.ToString(dtreplace.Rows[i]["ComponentNumber"]),
                        InputQuantity = Convert.ToString(dtreplace.Rows[i]["QuantityInput"]),
                        Quantity = Convert.ToString(dtreplace.Rows[i]["Quantity"]),
                        Rate = Convert.ToString(dtreplace.Rows[i]["Rate"]),
                        ProductDescription = Convert.ToString(dtreplace.Rows[i]["ProductDescription"]),
                        Serials = Convert.ToString(dtreplace.Rows[i]["ProductSerials"]),
                        InvoicedetailId = Convert.ToInt64(dtreplace.Rows[i]["ComponentDetailsID"]),
                        InvoiceId = Convert.ToString(dtreplace.Rows[i]["Invoice_Id"])
                    });

                    s_Invoiceid = Convert.ToString(dtreplace.Rows[i]["Invoice_Id"]);
                }


                foreach (var args in e.UpdateValues)
                {
                    var repPrimaryid = replacementList.Where(t => t.InvoicedetailId == Convert.ToInt64(args.NewValues["ComponentDetailsID"])).FirstOrDefault();
                    if (repPrimaryid != null)
                    {
                        repPrimaryid.ProductID = Convert.ToString(args.NewValues["ProductID"]);
                        repPrimaryid.ComponentNumber = Convert.ToString(args.NewValues["ComponentNumber"]);
                        repPrimaryid.InputQuantity = Convert.ToString(args.NewValues["QuantityInput"]);
                        repPrimaryid.Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        repPrimaryid.Rate = Convert.ToString(args.NewValues["Rate"]);
                        repPrimaryid.ProductDescription = Convert.ToString(args.NewValues["ProductDescription"]);
                        repPrimaryid.Serials = Convert.ToString(args.NewValues["ProductSerials"]);
                        repPrimaryid.InvoicedetailId = Convert.ToInt64(args.NewValues["ComponentDetailsID"]);
                        repPrimaryid.InvoiceId = Convert.ToString(args.NewValues["Invoice_Id"]);
                    };

                }
                foreach (var args in e.DeleteValues)
                {
                    var repPrimaryiddel = replacementList.Where(t => t.InvoicedetailId == Convert.ToInt64(args.Keys["ComponentDetailsID"])).FirstOrDefault();
                    if (repPrimaryiddel != null)
                    {
                        replacementList.Remove(repPrimaryiddel);
                    
                    }

                }

            }



            if (replacementList.Count != 0 && replacementList != null)
            {

                string budgetXML = Customerbudget.ConvertToXml(replacementList, 0);

                // DataTable Of Billing Address


                List<ReplacementBillingShippingAddress> replacementListbillingshipping = new List<ReplacementBillingShippingAddress>();

                DataTable tempBillAddress = new DataTable();
                tempBillAddress = null; //BillingShippingControl.SaveBillingShippingControlData();
                for (int i = 0; i < tempBillAddress.Rows.Count; i++)
                {
                    replacementListbillingshipping.Add(new ReplacementBillingShippingAddress()
             {
                 AddressType = Convert.ToString(tempBillAddress.Rows[i]["AddressType"]),
                 Address1 = Convert.ToString(tempBillAddress.Rows[i]["Address1"]),
                 Address2 = Convert.ToString(tempBillAddress.Rows[i]["Address2"]),
                 Address3 = Convert.ToString(tempBillAddress.Rows[i]["Address3"]),
                 LandMark = Convert.ToString(tempBillAddress.Rows[i]["LandMark"]),
                 CountryID = Convert.ToString(tempBillAddress.Rows[i]["CountryID"]),
                 StateID = Convert.ToString(tempBillAddress.Rows[i]["StateID"]),
                 CityID = Convert.ToString(tempBillAddress.Rows[i]["CityID"]),
                 Pincode = Convert.ToString(tempBillAddress.Rows[i]["Pincode"]),
                 Area = Convert.ToString(tempBillAddress.Rows[i]["Area"]),
                 GSTIN = Convert.ToString(tempBillAddress.Rows[i]["GSTIN"]),
                 ShipToParty = Convert.ToString(tempBillAddress.Rows[i]["ShipToParty"])
             });

                }

                string billingshippingXML = Customerbudget.ConvertToXml(replacementListbillingshipping, 0);

                model.CreatedBy = User;
                model.FiscalYear = FinYear;
                model.Branch = Branch;
                model.Customer = Customer;
                model.ReplacementDate = Replacementdate;
                model.ReplacementNumber = replacementnumber;
                model.lstreplacementXML = budgetXML;
                model.Company = Company;
                model.lstreplacementaddressXML = billingshippingXML;
                model.InvoiceId = s_Invoiceid;

                int i2 = 0;
                if (Request.QueryString["key"] != "ADD")
                {
                    model.ReplacementId = Convert.ToInt32(Request.QueryString["key"]);
                    i2 = objreplacement1.InsertReplacementDetails(model, "Modify");
                }
                else
                {



                    i2 = objreplacement1.InsertReplacementDetails(model, "Insert");
                }
                if (i2 > 0)
                {
                    Session["ReplaceMentNotesDetails"] = null;
                    gridReplacement.JSProperties["cpSaveSuccessOrFail"] = "Success";
                    gridReplacement.JSProperties["cpReplacementNo"] = replacementnumber;
                }
            }

        }

        protected void gridReplacement_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridReplacement_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridReplacement_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        #region Lookup Customer
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {

        }
        #endregion

        #region Grid Products After Invoice Product
        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {


        }

        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent = "";
                //List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ComponentID");
                //foreach (object Quo in QuoList)
                //{
                //    QuoComponent += "," + Quo;
                //}
                //QuoComponent = QuoComponent.TrimStart(',');
                QuoComponent = Convert.ToString(lookup_quotation.Value);

                // string strInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);

                DataTable dtDetails = objreplacement1.GetComponentInvoiceProductList(QuoComponent, "Invoice");

                if (dtDetails.Rows.Count > 0)
                {

                    txt_InvoiceDate.Text = Convert.ToString(dtDetails.Rows[0]["Invoice_Date"]);
                    grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(dtDetails.Rows[0]["Invoice_Date"]);

                }
                grid_Products.DataSource = dtDetails;
                grid_Products.DataBind();

            }

            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }

        }

        #endregion

        #region Popup Invoice lookup Grid
        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
            }
        }


        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string Customer = string.Empty;
            string InvoicecreatedDate = string.Empty;
            string BranchId = string.Empty;
            string ComponentType = string.Empty;
            string Action = string.Empty;
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];
                if (e.Parameter.Split('~')[2] != null) InvoicecreatedDate = e.Parameter.Split('~')[2];
                if (e.Parameter.Split('~')[2] != null) BranchId = e.Parameter.Split('~')[3];

                string date2 = Convert.ToDateTime(InvoicecreatedDate).ToString("yyyy-MM-dd");

                DataTable ComponentTable = objreplacement1.GetComponentInvoice(Customer, date2, FinYear, BranchId);
                if (ComponentTable.Rows.Count > 0)
                {



                    // grid_Products.JSProperties["cptxt_InvoiceDate"] = Convert.ToString(ComponentTable.Rows[0]["Invoice_Date"]);


                    Session["SI_ComponentData"] = ComponentTable;

                    lookup_quotation.DataSource = ComponentTable;
                    lookup_quotation.DataBind();
                }
                else
                {
                    Session["SI_ComponentData"] = null;
                    lookup_quotation.DataSource = null;
                    lookup_quotation.DataBind();

                }



            }

        }
        #endregion

        #region WareHouse
        protected void GrdWarehouse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            GrdWarehouse.JSProperties["cpIsSave"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];
            string Type = "", Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0";
            bool IsDelete = false;

            DataTable Warehousedt = new DataTable();
            if (Session["PC_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["PC_WarehouseData"];
            }
            else
            {
                Warehousedt.Columns.Add("ReplacementId", typeof(string));
                Warehousedt.Columns.Add("ComponentdetailsIdtId", typeof(string));
                Warehousedt.Columns.Add("ComponentNumber", typeof(string));
                Warehousedt.Columns.Add("Product_Id", typeof(string));
                Warehousedt.Columns.Add("SrlNo", typeof(int));
                Warehousedt.Columns.Add("WarehouseID", typeof(string));
                Warehousedt.Columns.Add("WarehouseName", typeof(string));
                Warehousedt.Columns.Add("Quantity", typeof(string));
                Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                Warehousedt.Columns.Add("BatchID", typeof(string));
                Warehousedt.Columns.Add("BatchNo", typeof(string));
                Warehousedt.Columns.Add("MfgDate", typeof(string));
                Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                Warehousedt.Columns.Add("SerialNo", typeof(string));
                Warehousedt.Columns.Add("LoopID", typeof(string));
                Warehousedt.Columns.Add("Status", typeof(string));
                Warehousedt.Columns.Add("ViewMfgDate", typeof(string));
                Warehousedt.Columns.Add("ViewExpiryDate", typeof(string));


            }

            if (strSplitCommand == "Display")
            {
                objreplacement1.GetProductType(hdfProductID.Value, ref Type);


                string ProductID = Convert.ToString(hdfProductID.Value);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[1]);

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }
                changeGridOrder();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int rows2 = GrdWarehouse.VisibleRowCount;
                //objreplacement1.GetProductType(hdfProductID.Value, ref Type);

                int loopId = Convert.ToInt32(Session["PC_LoopWarehouse"]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);
                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);
                string MfgDate = Convert.ToString(e.Parameters.Split('~')[4]);
                string ExpiryDate = Convert.ToString(e.Parameters.Split('~')[5]);
                string SerialNo = Convert.ToString(e.Parameters.Split('~')[6]);
                string Qty = Convert.ToString(e.Parameters.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameters.Split('~')[8]);
                string ProductID = Convert.ToString(e.Parameters.Split('~')[9]);

                string PType = Convert.ToString(e.Parameters.Split('~')[10]);
                // string ProductID = Convert.ToString(hdfProductID.Value);

                string comdetailsid = Convert.ToString(e.Parameters.Split('~')[10]);
                string componntnmber = Convert.ToString(e.Parameters.Split('~')[11]);
                string ProductSerialID = Convert.ToString(hdfProductSerialID.Value);

                string ReplacementId = Convert.ToString(Request.QueryString["key"]);



                objreplacement1.GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                if (editWarehouseID == "0")
                {
                    if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");
                        // Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);

                        Warehousedt.Rows.Add(ReplacementId, comdetailsid, componntnmber, ProductID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);



                    }
                }
                else
                {

                }


                Session["PC_WarehouseData"] = Warehousedt;
                Session["PC_LoopWarehouse"] = loopId + 1;
                //changeGridOrder();

                Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";

                GrdWarehouse.DataSource = Warehousedt.DefaultView.ToTable();
                GrdWarehouse.DataBind();
            }


            else if (strSplitCommand == "Delete")
            {
                DataRow[] delResult = Warehousedt.Select("SrlNo ='" + 0 + "'");
                foreach (DataRow delrow in delResult)
                {
                    delrow.Delete();
                }

                Session["PC_WarehouseData"] = Warehousedt;
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
            }
        }


        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["PC_WarehouseData"] != null)
            {
                string Type = "";
                objreplacement1.GetProductType(hdfProductID.Value, ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["PC_WarehouseData"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                }
            }
        }

        public void changeGridOrder()
        {
            string Type = "";
            objreplacement1.GetProductType(hdfProductID.Value, ref Type);
            if (Type == "W")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = false;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WB")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WBS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "B")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "S")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = false;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "WS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = false;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "BS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }


        }




        public int CheckSerialNoExists(string SerialNo)
        {
            DataTable SerialCount = oDBEngine.GetDataTable("Select * From Trans_StockSerialMapping Where Stock_BranchSerialNumber='" + SerialNo.Trim() + "'");
            return SerialCount.Rows.Count;
        }




        public string GetWarehouseMaxValue(DataTable dt)
        {
            string maxValue = "1";
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> myList = new List<int>();
                foreach (DataRow rrow in dt.Rows)
                {
                    string value = (Convert.ToString(rrow["SrlNo"]) != "") ? Convert.ToString(rrow["SrlNo"]) : "0";
                    myList.Add(Convert.ToInt32(value));
                }

                maxValue = Convert.ToString(Convert.ToInt32(myList.Max()) + 1);
            }

            return maxValue;
        }


        protected void CmbWarehouseID_Callback(object sender, CallbackEventArgsBase e)
        {

            strCompanyID = Convert.ToString(Session["LastCompany"]);
            strBranchID = Convert.ToString(Session["userbranchID"]);
            FinYear = Convert.ToString(Session["LastFinYear"]);

            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindWarehouse")
            {



                string Product = e.Parameter.Split('~')[1];

                DataTable dt = objreplacement1.GetWarehouseData("0", Product, FinYear, strBranchID, strCompanyID);

                CmbWarehouseID.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouseID.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
            }
        }
        #endregion

    }
}