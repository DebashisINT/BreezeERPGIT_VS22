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

namespace ServiceManagement.STBManagement.ReturnRequisition
{
    public partial class ReturnRequisition : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        string UniquePurchaseNumber = string.Empty;
        CommonBL ComBL = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("status"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }

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
                Session["DeviceDetails"] = null;
                string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
                string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinWalletRecharge");
                hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;

                string IsEntityInformationEditableInReturnReq = ComBL.GetSystemSettingsResult("IsEntityInformationEditableInReturnReq");
                hdnIsEntityInformationEditableInReturnReq.Value = IsEntityInformationEditableInReturnReq;
                if (!String.IsNullOrEmpty(MultiBranchNumberingScheme))
                {
                    if (MultiBranchNumberingScheme.ToUpper().Trim() == "YES")
                    {
                        userbranchHierachy = EmployeeBranchMap();
                    }
                    else
                    {
                        userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    }
                }
                else
                {
                    userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                }
                PopulateBranchByHierchy(userbranchHierachy);
                AddmodeExecuted();
                txtAmount.ClientEnabled = false;
                if (Request.QueryString["Key"].ToUpper() != "ADD")
                {
                    if (Request.QueryString["Key"].ToUpper() == "EDIT")
                    {
                        HeaderName.Text = "Modify Return Requisition";
                        hdAddEdit.Value = "edit";
                    }
                    else
                    {
                        HeaderName.Text = " View Return Requisition";
                        hdAddEdit.Value = "view";
                    }
                    string STBRequisitionID = Request.QueryString["id"];
                    hdnSTBRequisitionID.Value = Request.QueryString["id"];
                    EditModeExecute(STBRequisitionID);
                }
                else
                {
                    string fDate = null;
                    fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
                    FormDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    HeaderName.Text = "Add Return Requisition";
                    hdAddEdit.Value = "Add";
                }

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    divcross.Visible = false;
                    hdnIsAproval.Value = "Yes";

                    divApprovalSectionhdr.Attributes.Add("removeclass", "hide");
                    divApprovalSectiondtls.Attributes.Add("removeclass", "hide");
                    //divDetails1EntryLeven.Attributes.Add("class", "hide");
                }
                else
                {
                    divApprovalSectionhdr.Attributes.Add("class", "hide");
                    divApprovalSectiondtls.Attributes.Add("class", "hide");
                    hdnIsAproval.Value = "No";
                }
            }
        }

        private void EditModeExecute(string receiptID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
                    proc.AddVarcharPara("@ACTION", 300, "ShowDetails");
                    proc.AddVarcharPara("@ReturnReq_id", 100, receiptID.ToString().Trim());
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        CmbScheme.Value = ds.Tables[0].Rows[0]["NumberingScheme"].ToString();

                        divNumberingScheme.Style.Add("display", "none");
                        txtDocumentNumber.Text = ds.Tables[0].Rows[0]["DocumentNumber"].ToString();
                        // txtDocumentNumber.Style.Add("disable", "disable");
                        txtDocumentNumber.Enabled = false;
                        FormDate.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocumentDate"].ToString());
                        FormDate.ClientEnabled = false;

                        ddlBranch.Value = Convert.ToString(ds.Tables[0].Rows[0]["Branch"].ToString());
                        ddlBranch.ClientEnabled = false;

                        txtEntityCode.Value = ds.Tables[0].Rows[0]["EntityCode"].ToString();
                        txtEntityCode.Style.Add("disable", "disable");

                        txtNetworkName.Value = ds.Tables[0].Rows[0]["NetworkName"].ToString();
                        txtNetworkName.Style.Add("disable", "disable");

                        txtContactPerson.Value = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
                        txtContactPerson.Style.Add("disable", "disable");

                        txtContactNo.Value = ds.Tables[0].Rows[0]["ContactNumber"].ToString();
                        txtContactNo.Style.Add("disable", "disable");

                        txtDAS.Value = ds.Tables[0].Rows[0]["DAS"].ToString();
                        txtDAS.Style.Add("disable", "disable");

                        ddlRequisitionType.Value = Convert.ToString(ds.Tables[0].Rows[0]["RequisitionType_Id"].ToString());
                        ddlRequisitionType.ClientEnabled = false;

                        ddlRequisitionFor.Value = Convert.ToString(ds.Tables[0].Rows[0]["RequisitionFor_Id"].ToString());
                        ddlRequisitionFor.ClientEnabled = false;

                        if (Convert.ToString(ds.Tables[0].Rows[0]["IsNoPayment"].ToString()) == "True")
                        {
                            chkNoPayment.Checked = true;
                        }

                        if (Convert.ToString(ds.Tables[0].Rows[0]["IsPayUsingOnAcountBalance"].ToString()) == "True")
                        {
                            chkPayUsingOnAcountBalance.Checked = true;
                        }

                        txtPaymentRelatedRemarks.Text = Convert.ToString(ds.Tables[0].Rows[0]["PaymentRelatedRemarks"].ToString());

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtable = new DataTable();

                            dtable.Clear();
                            dtable.Columns.Add("HIddenID", typeof(System.Guid));
                            dtable.Columns.Add("Model", typeof(System.String));
                            dtable.Columns.Add("UnitPrice", typeof(System.String));
                            dtable.Columns.Add("Quantity", typeof(System.String));
                            dtable.Columns.Add("Amount", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));
                            dtable.Columns.Add("Model_ID", typeof(System.String));
                            dtable.Columns.Add("OSTBVendor", typeof(System.String));
                            dtable.Columns.Add("OSTBVendorID", typeof(System.String));

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                object[] trow = { Guid.NewGuid(), item["Model"].ToString(), item["UnitPrice"].ToString(), item["Quantity"].ToString(), item["Amount"].ToString(),
                                                        item["Remarks"].ToString(), item["Model_ID"].ToString(),item["OSTBVendor"].ToString(), item["OSTBVendorID"].ToString()};
                                dtable.Rows.Add(trow);
                            }

                            HttpContext.Current.Session["DeviceDetails"] = dtable;
                            GrdDevice.DataBind();
                        }

                        DataSet branchtable = dsFetchAll();
                        if (ddlRequisitionFor.Text == "STB-01")
                        {
                            ddlModel.DataSource = branchtable.Tables[2];
                            ddlModel.ValueField = "ModelID";
                            ddlModel.TextField = "ModelDesc";
                            ddlModel.DataBind();
                            ddlModel.SelectedIndex = 0;

                            ddlOSTBVendor.ClientEnabled = false;
                        }
                        else if (ddlRequisitionFor.Text == "STB-02")
                        {
                            ddlModel.DataSource = branchtable.Tables[4];
                            ddlModel.ValueField = "ModelID";
                            ddlModel.TextField = "ModelDesc";
                            ddlModel.DataBind();
                            ddlModel.SelectedIndex = 0;

                            ddlOSTBVendor.ClientEnabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
        }

        private void AddmodeExecuted()
        {
            CmbScheme.DataSource = BindNumberingScheme();
            CmbScheme.ValueField = "ID";
            CmbScheme.TextField = "SchemaName";
            CmbScheme.DataBind();
            CmbScheme.SelectedIndex = 0;

            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataSet branchtable = dsFetchAll();

            ddlRequisitionType.DataSource = branchtable.Tables[0];
            ddlRequisitionType.ValueField = "TypeId";
            ddlRequisitionType.TextField = "RequisitionType";
            ddlRequisitionType.DataBind();
            ddlRequisitionType.SelectedIndex = 0;

            ddlRequisitionFor.DataSource = branchtable.Tables[1];
            ddlRequisitionFor.ValueField = "ID";
            ddlRequisitionFor.TextField = "RequisitionFor";
            ddlRequisitionFor.DataBind();
            ddlRequisitionFor.SelectedIndex = 0;

            ddlModel.DataSource = branchtable.Tables[2];
            ddlModel.ValueField = "ModelID";
            ddlModel.TextField = "ModelDesc";
            ddlModel.DataBind();
            ddlModel.SelectedIndex = 0;

            //dddlApprovalEmployee.DataSource = branchtable.Tables[3];
            //dddlApprovalEmployee.ValueField = "cnt_internalId";
            //dddlApprovalEmployee.TextField = "DirectorName";
            //dddlApprovalEmployee.DataBind();
            //dddlApprovalEmployee.SelectedIndex = 0;

            ddlOSTBVendor.DataSource = branchtable.Tables[5];
            ddlOSTBVendor.ValueField = "cnt_internalId";
            ddlOSTBVendor.TextField = "OSTBVendor";
            ddlOSTBVendor.DataBind();
            ddlOSTBVendor.SelectedIndex = 0;

        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
            return ds;
        }

        public DataTable BindNumberingScheme()
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
            proc.AddVarcharPara("@ACTION", 500, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, userbranchHierachy);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            //proc.AddVarcharPara("@Type", 100, ReceiptType);
            proc.AddVarcharPara("@IsSplit", 100, "Y");
            ds = proc.GetTable();
            return ds;
        }

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            ddlBranch.DataSource = branchtable;
            ddlBranch.ValueField = "branch_id";
            ddlBranch.TextField = "branch_description";
            ddlBranch.DataBind();
            ddlBranch.SelectedIndex = 0;
        }

        public String EmployeeBranchMap()
        {
            String branches = null;
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeBranchMap");
            proc.AddVarcharPara("@USER_ID", 100, Session["userid"].ToString());
            ds = proc.GetTable();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        [WebMethod]
        public static string save(InsertReceipt apply)
        {

            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string output = string.Empty;
            string DocumentNo = "", schmID = "";
            string strPurchaseNumber = Convert.ToString(apply.DocumentNumber);
            schmID = apply.CmbScheme.Split('~')[0].ToString();
            //if (apply.Action != "edit")
            //{
            //    schmID = apply.CmbScheme.Split('~')[0].ToString();
            //    string docNo = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(schmID));

            //    if (docNo.Split('~')[0].ToString() == "ok")
            //    {
            //        DocumentNo = docNo.Split('~')[1].ToString();
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}
            //else
            //{
            //    DocumentNo = apply.DocumentNumber;
            //}


            DataTable dtview = new DataTable();
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
            bool STATUS = false;
            DataTable dt3 = new DataTable();
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    STATUS = true;
                }
                else
                {
                    output = "Please add STB details.~";
                    STATUS = false;
                }

                if (STATUS)
                {
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionInsertUpdate");
                        proc.AddPara("@NumberingScheme", Convert.ToString(apply.CmbScheme));
                        proc.AddPara("@DocumentNumber", Convert.ToString(strPurchaseNumber));
                        proc.AddPara("@DocumentDate", Convert.ToString(apply.date));
                        proc.AddPara("@Branch", Convert.ToString(apply.branch));
                        proc.AddPara("@RequisitionType", apply.RequisitionType);
                        proc.AddPara("@RequisitionFor", apply.RequisitionFor);
                        proc.AddPara("@EntityCode", apply.EntityCode);
                        proc.AddPara("@NetworkName", Convert.ToString(apply.NetworkName));
                        proc.AddPara("@ContactPerson", apply.ContactPerson);
                        proc.AddPara("@ContactNo", apply.ContactNo);
                        proc.AddPara("@DAS", apply.DAS);
                        proc.AddPara("@UDT_STBRequisitionDetails", dt);
                        proc.AddPara("@Action", Convert.ToString(apply.Action));
                        proc.AddPara("@SchemaID", schmID);
                        proc.AddPara("@USER_ID", user_id);
                        proc.AddPara("@ReturnRequisitionID", apply.STBRequisitionID);
                        proc.AddPara("@COMPANYID", strCompanyID);
                        proc.AddPara("@FINYEAR", FinYear);
                        proc.AddPara("@IsNoPayment", apply.IsNoPayment);
                        proc.AddPara("@PaymentRelatedRemarks", apply.PaymentRelatedRemarks);
                        proc.AddPara("@IsPayUsingOnAcountBalance", apply.IsPayUsingOnAcountBalance);
                        proc.AddPara("@SchemeId", schmID);
                        if (apply.IsApproval == "Yes")
                        {
                            proc.AddPara("@ApprovalAction", apply.ApprovalAction);
                            proc.AddPara("@ApprovalEmployee", apply.ApprovalEmployee);
                            proc.AddPara("@ApprovalRemarks", apply.ApprovalRemarks);
                        }
                        dtview = proc.GetTable();

                        if (dtview != null && dtview.Rows.Count > 0)
                        {
                            output = "true~" + dtview.Rows[0]["DocumentID"].ToString() + "~" + dtview.Rows[0]["DocumentNo"].ToString() + "~" + apply.Action;
                        }
                    }
                    else
                    {
                        output = "Logout";
                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            return output;
        }

        public static string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            DateTime date = DateTime.Today;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            string ddtat = DateTime.Today.ToString("ddMMyyyy");

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string UniqueQuotation = "";
            //  oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oDBEngine = new BusinessLogicLayer.DBEngine();


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
                    if (scheme_type == 2)
                    {
                        paddCounter = paddCounter + 8;
                    }
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.DocumentNumber) FROM STB_ReturnRequisitionHeader tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.DocumentNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.DocumentNumber))) = 1 and DocumentNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, Create_date) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.DocumentNumber) FROM STB_ReturnRequisitionHeader tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.DocumentNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.DocumentNumber))) = 1 and DocumentNumber like '" + prefCompCode + "%' and DocumentNumber like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Create_date) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = 0;
                        string uccCodeSubstring = "";
                        if (scheme_type == 2)
                        {
                            decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length) - 8;
                            uccCodeSubstring = uccCode.Substring(prefCompCode.Length + 8, decimalPartLen);
                        }
                        else
                        {
                            decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        }


                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange~";
                        }
                        else
                        {
                            if (scheme_type == 2)
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter - 8, '0');
                                UniqueQuotation = prefCompCode + ddtat.ToString() + paddedStr + sufxCompCode;
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            }

                            return "ok~" + UniqueQuotation;
                        }
                    }
                    else
                    {
                        if (scheme_type == 2)
                        {
                            UniqueQuotation = startNo.PadLeft(paddCounter - 8, '0');
                            UniqueQuotation = prefCompCode + ddtat.ToString() + paddedStr + sufxCompCode;
                        }
                        else
                        {
                            UniqueQuotation = startNo.PadLeft(paddCounter, '0');
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        }
                        return "ok~" + UniqueQuotation;
                    }
                }
                else
                {
                    sqlQuery = "SELECT DocumentNumber FROM STB_ReturnRequisitionHeader WHERE DocumentNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate~";
                    }

                    UniqueQuotation = manual_str.Trim();
                    return "ok~" + UniqueQuotation;
                }
            }
            else
            {
                return "noid~";
            }
        }

        #region Details1 Section

        [WebMethod]
        public static String AddData(string Model, string UnitPrice, string Quantity, string Amount, String Remarks, string Model_ID, String Guids,
            String OSTBVendor, String OSTBVendorID)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
                DataTable dt2 = new DataTable();

                if (dt == null)
                {
                    DataTable dtable = new DataTable();

                    dtable.Clear();
                    dtable.Columns.Add("HIddenID", typeof(System.Guid));
                    dtable.Columns.Add("Model", typeof(System.String));
                    dtable.Columns.Add("UnitPrice", typeof(System.String));
                    dtable.Columns.Add("Quantity", typeof(System.String));
                    dtable.Columns.Add("Amount", typeof(System.String));
                    dtable.Columns.Add("Remarks", typeof(System.String));
                    dtable.Columns.Add("Model_ID", typeof(System.String));
                    dtable.Columns.Add("OSTBVendor", typeof(System.String));
                    dtable.Columns.Add("OSTBVendorID", typeof(System.String));

                    object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, Remarks, Model_ID, OSTBVendor, OSTBVendorID };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 1)
                        {
                            object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, Remarks, Model_ID, OSTBVendor, OSTBVendorID };// Add new parameter Here
                            dt.Rows.Add(trow);
                        }
                        else
                        {
                            return "You can't add more STB details.";
                        }
                    }
                    else
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                if (Guids.ToString() == item["HIddenID"].ToString())
                                {
                                    item["Model"] = Model;
                                    item["UnitPrice"] = UnitPrice;
                                    item["Quantity"] = Quantity;
                                    item["Amount"] = Amount;
                                    item["Remarks"] = Remarks;
                                    item["Model_ID"] = Model_ID;
                                    item["OSTBVendor"] = OSTBVendor;
                                    item["OSTBVendorID"] = OSTBVendorID;
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["DeviceDetails"] = dt;
                }

                return "STB details Added Successfully.";
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static DeviceDetails EditData(String HiddenID)
        {
            DeviceDetails ret = new DeviceDetails();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (HiddenID.ToString() == item["HIddenID"].ToString())
                        {
                            ret.Model = item["Model"].ToString();
                            ret.UnitPrice = item["UnitPrice"].ToString();
                            ret.Quantity = item["Quantity"].ToString();
                            ret.Amount = item["Amount"].ToString();
                            ret.Remarks = item["Remarks"].ToString();
                            ret.Model_ID = item["Model_ID"].ToString();
                            ret.OSTBVendorID = item["OSTBVendorID"].ToString();
                            break;
                        }
                    }
                }
            }
            return ret;// "Holiday Remove Sucessfylly";
        }

        [WebMethod]
        public static String DeleteData(string HiddenID)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }
            return "STB Details Remove Successfully.";
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["DeviceDetails"] != null)
            {
                GrdDevice.DataSource = (DataTable)Session["DeviceDetails"];
            }
        }

        protected void GrdDevice_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Session["DeviceDetails"] != null)
            {
                GrdDevice.DataBind();
            }
        }

        #endregion

        [WebMethod]
        public static string FetchEntityFromMaster(String entity_code)
        {
            string EntityCodeDetails = "";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
            proc.AddVarcharPara("@Action", 500, "FetchEntityFromMaster");
            proc.AddPara("@EntityCode", Convert.ToString(entity_code));
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                EntityCodeDetails = dt.Rows[0]["Is_Active"].ToString();
            }
            return EntityCodeDetails;
        }

        [WebMethod]
        public static string UpdatePrintCount(String STBRequisitionID)
        {
            string EntityCodeDetails = "False";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
            proc.AddVarcharPara("@Action", 500, "UpdatePrintCount");
            proc.AddPara("@ReturnReq_id", Convert.ToString(STBRequisitionID));
            int i = proc.RunActionQuery();
            if (i > 0)
            {
                EntityCodeDetails = "True";
            }
            return EntityCodeDetails;
        }

        [WebMethod]
        public static string GetUnitPrice(String ModelId, String DAS, String RequisitionFor)
        {
            string UnitPrice = "0.00";
            ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
            proc.AddVarcharPara("@Action", 500, "GetPhaseValue");
            proc.AddPara("@MODEL_ID", Convert.ToString(ModelId));
            proc.AddPara("@DAS", Convert.ToString(DAS));
            proc.AddPara("@RequisitionFor", RequisitionFor);
            DataTable dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                UnitPrice = Convert.ToString(dt.Rows[0]["PhaseValue"]);
            }
            return UnitPrice;
        }

        [WebMethod]
        public static List<MoneyReceiptRequisitionHistory> MoneyReceiptRequisitionHistoryList(String report)
        {
            List<MoneyReceiptRequisitionHistory> listStatues = new List<MoneyReceiptRequisitionHistory>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ReturnRequisitionDetails");
            proc.AddVarcharPara("@ACTION", 500, report);
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new MoneyReceiptRequisitionHistory
                    {
                        DocumentNo = item["DocumentNo"].ToString(),
                        Date = item["Date"].ToString(),
                        PaymentType = item["PaymentType"].ToString(),
                        ReqType = item["ReqType"].ToString(),
                        ReqFor = item["ReqFor"].ToString(),
                        Amount = item["Amount"].ToString(),
                        Status = item["Status"].ToString()
                    });
                }
            }
            return listStatues;
        }

        public class MoneyReceiptRequisitionHistory
        {
            public String DocumentNo { get; set; }
            public String Date { get; set; }
            public String PaymentType { get; set; }
            public String ReqType { get; set; }
            public String ReqFor { get; set; }
            public String Amount { get; set; }
            public String Status { get; set; }
        }

        public class InsertReceipt
        {
            public String Action { get; set; }
            public String STBRequisitionID { get; set; }
            public String CmbScheme { get; set; }
            public String DocumentNumber { get; set; }
            public String branch { get; set; }
            public String date { get; set; }
            public String RequisitionType { get; set; }
            public String RequisitionFor { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String ContactNo { get; set; }
            public String DAS { get; set; }
            public String IsNoPayment { get; set; }
            public String PaymentRelatedRemarks { get; set; }
            public String IsPayUsingOnAcountBalance { get; set; }

            public String ApprovalAction { get; set; }
            public String DirectorApprovalRequired { get; set; }
            public String ApprovalEmployee { get; set; }
            public String ApprovalRemarks { get; set; }
            public String IsApproval { get; set; }

        }

        public class DeviceDetails
        {
            public String Model { get; set; }
            public String UnitPrice { get; set; }
            public String Quantity { get; set; }
            public String Amount { get; set; }
            public String Remarks { get; set; }
            public String Model_ID { get; set; }
            public String Guid { get; set; }
            public String OSTBVendor { get; set; }
            public String OSTBVendorID { get; set; }

        }

        protected void ddlModel_CustomCallback(object sender, CallbackEventArgsBase e)
        {
            DataSet branchtable = dsFetchAll();
            if (e.Parameter == "STB-01")
            {
                ddlModel.DataSource = branchtable.Tables[2];
                ddlModel.ValueField = "ModelID";
                ddlModel.TextField = "ModelDesc";
                ddlModel.DataBind();
                ddlModel.SelectedIndex = 0;
            }
            else if (e.Parameter == "STB-02")
            {
                ddlModel.DataSource = branchtable.Tables[4];
                ddlModel.ValueField = "ModelID";
                ddlModel.TextField = "ModelDesc";
                ddlModel.DataBind();
                ddlModel.SelectedIndex = 0;
            }
        }
    }
}