using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.WalletRecharge
{
    public partial class WalletAdd : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        string UniquePurchaseNumber = string.Empty;
        CommonBL ComBL = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["DeviceDetails"] = null;
                string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
                string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinWalletRecharge");
                hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;
                string IsEntityInformationEditableInWRMR = ComBL.GetSystemSettingsResult("IsEntityInformationEditableInWR/MR");
                hdnIsEntityInformationEditableInWRMR.Value = IsEntityInformationEditableInWRMR;
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
                if (Request.QueryString["Key"] != "Add")
                {
                    if (Request.QueryString["Key"] == "edit")
                    {
                        HeaderName.Text = "Modify Wallet Recharge";
                        hdAddEdit.Value = "edit";
                    }
                    else
                    {
                        HeaderName.Text = " View Wallet Recharge";
                        hdAddEdit.Value = "view";
                    }
                    string WalletRechargeID = Request.QueryString["id"];
                    hdnWalletRechargeID.Value = Request.QueryString["id"];
                    EditModeExecute(WalletRechargeID);
                }
                else
                {
                    string fDate = null;
                    fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);

                    HeaderName.Text = "Add Wallet Recharge";
                    hdAddEdit.Value = "Add";
                    FormDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
                    proc.AddVarcharPara("@ACTION", 300, "ShowDetails");
                    proc.AddVarcharPara("@WalletRecharge_ID", 100, receiptID.ToString().Trim());
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

                        txtContactNo.Value = ds.Tables[0].Rows[0]["ContactNo"].ToString();
                        txtContactNo.Style.Add("disable", "disable");

                        txtDAS.Value = ds.Tables[0].Rows[0]["DAS"].ToString();
                        txtDAS.Style.Add("disable", "disable");

                        hdnIsCancel.Value = ds.Tables[0].Rows[0]["IsCancel"].ToString();
                        txtReasonCancel.Value = ds.Tables[0].Rows[0]["ReasonForCancel"].ToString();
                        txtReasonCancelMadeBy.Value = ds.Tables[0].Rows[0]["Cancel_Create_by"].ToString();
                        txtReasonCancelMadeOn.Value = ds.Tables[0].Rows[0]["Cancel_Create_date"].ToString();
                        lblPrintCount.InnerHtml = ds.Tables[0].Rows[0]["Print_Count"].ToString();
                        DiviceTyp.Value = ds.Tables[0].Rows[0]["Type"].ToString();
                        if (ds.Tables[0].Rows[0]["IsCancel"].ToString()=="True")
                        {
                            lblCancelMsg.InnerHtml = "Cancellation In Progress";
                        }
                        else
                        {
                            lblCancelMsg.InnerHtml = "";
                        }

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtable = new DataTable();

                            dtable.Clear();
                            dtable.Columns.Add("HIddenID", typeof(System.Guid));
                            dtable.Columns.Add("Payment_Mode", typeof(System.String));
                            dtable.Columns.Add("Payment_Amount", typeof(System.String));
                            dtable.Columns.Add("Cheque_No", typeof(System.String));
                            dtable.Columns.Add("Cheque_date", typeof(System.String));
                            dtable.Columns.Add("Ref_No", typeof(System.String));
                            dtable.Columns.Add("Bank_ID", typeof(System.String));
                            dtable.Columns.Add("PaymentDetails_BankName", typeof(System.String));
                            dtable.Columns.Add("PaymentDetails_BranchName", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                object[] trow = { Guid.NewGuid(), item["Payment_Mode"].ToString(), item["Payment_Amount"].ToString(), item["Cheque_No"].ToString(), item["Cheque_date"].ToString(),
                                                        item["Ref_No"].ToString(), item["Bank_ID"].ToString(), item["Payment_BankName"].ToString(), item["Payment_BranchName"].ToString(),
                                                        item["Remarks"].ToString()};
                                dtable.Rows.Add(trow);
                            }

                            HttpContext.Current.Session["DeviceDetails"] = dtable;
                            GrdDevice.DataBind();
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

            DiviceTyp.DataSource = branchtable.Tables[0];
            DiviceTyp.ValueField = "ID";
            DiviceTyp.TextField = "TYPE";
            DiviceTyp.DataBind();
            DiviceTyp.SelectedIndex = 1;

            ddlBank.DataSource = branchtable.Tables[1];
            ddlBank.ValueField = "bnk_id";
            ddlBank.TextField = "bnk_bankName";
            ddlBank.DataBind();
            ddlBank.SelectedIndex = 0;

        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
            return ds;
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

        public DataTable BindNumberingScheme()
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
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
            if (apply.Action != "edit")
            {
                schmID = apply.CmbScheme.Split('~')[0].ToString();
                string docNo = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(schmID));

                if (docNo.Split('~')[0].ToString() == "ok")
                {
                    DocumentNo = docNo.Split('~')[1].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                DocumentNo = apply.DocumentNumber;
            }


            DataTable dtview = new DataTable();
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];


            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Columns.Remove("HIddenID");
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("PRC_STB_WalletRechargeInsertUpdate");
                        proc.AddPara("@NumberingScheme", Convert.ToString(apply.CmbScheme));
                        proc.AddPara("@DocumentNumber", Convert.ToString(DocumentNo));
                        proc.AddPara("@DocumentDate",Convert.ToString(apply.date));
                        proc.AddPara("@Branch", Convert.ToString(apply.branch));
                        proc.AddPara("@EntityCode", apply.EntityCode);
                        proc.AddPara("@DAS", apply.DAS);
                        proc.AddPara("@UDT_WalletRechargeDetails", dt);
                        proc.AddPara("@Action", Convert.ToString(apply.Action));
                        proc.AddPara("@NetworkName", Convert.ToString(apply.NetworkName));
                        proc.AddPara("@ContactPerson", apply.ContactPerson);
                        proc.AddPara("@SchemaID", schmID);
                        proc.AddPara("@USER_ID", user_id);
                        proc.AddPara("@WalletRecharge_ID", apply.WalletRecharge_ID);
                        proc.AddPara("@ContactNo", apply.ContactNo);
                        proc.AddPara("@COMPANYID", strCompanyID);
                        proc.AddPara("@FINYEAR", FinYear);
                        proc.AddPara("@Type", apply.Type);
                        dtview = proc.GetTable();

                        if (dtview != null && dtview.Rows.Count > 0)
                        {
                            output = "true~" + dtview.Rows[0]["DocumentID"].ToString() + "~" + DocumentNo + "~" + apply.Action;
                        }
                    }
                    else
                    {
                        output = "Logout";
                    }
                }
                else
                {
                    output = "Please Payment details.~";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            return output;
        }

        public class InsertReceipt
        {
            public String Action { get; set; }
            public String WalletRecharge_ID { get; set; }
            public String CmbScheme { get; set; }
            public String DocumentNumber { get; set; }
            public String branch { get; set; }
            public String date { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String ContactNo { get; set; }
            public String Type { get; set; }
            public String DAS { get; set; }


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

                    sqlQuery = "SELECT max(tjv.DocumentNumber) FROM STB_WalletRechargeHeader tjv WHERE dbo.RegexMatch('";
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
                        sqlQuery = "SELECT max(tjv.DocumentNumber) FROM STB_WalletRechargeHeader tjv WHERE dbo.RegexMatch('";
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
                    sqlQuery = "SELECT DocumentNumber FROM STB_WalletRechargeHeader WHERE DocumentNumber LIKE '" + manual_str.Trim() + "'";
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

        [WebMethod]
        public static String AddData(string Mode, string Amount, string RefNo, string ChequeNo, String ChequeDate, string BankID, string BankName, string Branch,
            String Remark, String Guids)
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
                    dtable.Columns.Add("Payment_Mode", typeof(System.String));
                    dtable.Columns.Add("Payment_Amount", typeof(System.String));
                    dtable.Columns.Add("Cheque_No", typeof(System.String));
                    dtable.Columns.Add("Cheque_date", typeof(System.String));
                    dtable.Columns.Add("Ref_No", typeof(System.String));
                    dtable.Columns.Add("Bank_ID", typeof(System.String));
                    dtable.Columns.Add("PaymentDetails_BankName", typeof(System.String));
                    dtable.Columns.Add("PaymentDetails_BranchName", typeof(System.String));
                    dtable.Columns.Add("Remarks", typeof(System.String));

                    object[] trow = { Guid.NewGuid(), Mode, Amount, ChequeNo, ChequeDate, RefNo, BankID, BankName, Branch, Remark };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 1)
                        {
                            object[] trow = { Guid.NewGuid(), Mode, Amount, ChequeNo, ChequeDate, RefNo, BankID, BankName, Branch, Remark };// Add new parameter Here
                            dt.Rows.Add(trow);
                        }
                        else
                        {
                            return "You can't add more Payment details.";
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
                                    item["Payment_Mode"] = Mode;
                                    item["Payment_Amount"] = Amount;
                                    item["Cheque_No"] = ChequeNo;
                                    item["Cheque_date"] = ChequeDate;
                                    item["Ref_No"] = RefNo;
                                    item["Bank_ID"] = BankID;
                                    item["PaymentDetails_BankName"] = BankName;
                                    item["PaymentDetails_BranchName"] = Branch;
                                    item["Remarks"] = Remark;
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["DeviceDetails"] = dt;
                }

                return "Payment details Added Successfully.";
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
                            ret.Payment_Mode = item["Payment_Mode"].ToString();
                            ret.Amount = item["Payment_Amount"].ToString();
                            ret.RefNo = item["Ref_No"].ToString();
                            ret.ChequeNo = item["Cheque_No"].ToString();
                            ret.ChequeDate = item["Cheque_date"].ToString();
                            ret.BankID = item["Bank_ID"].ToString();
                            ret.BankName = item["PaymentDetails_BankName"].ToString();
                            ret.BranchName = item["PaymentDetails_BranchName"].ToString();
                            ret.Remark = item["Remarks"].ToString();

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
            return "Payment Details Remove Successfully.";
        }

        [WebMethod]
        public static string FetchEntityFromMaster(String entity_code)
        {
            string EntityCodeDetails = "";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
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
        public static string UpdatePrintCount(String WalletRechargeID)
        {
            string EntityCodeDetails = "False";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBWalletRechargeDetails");
            proc.AddVarcharPara("@Action", 500, "UpdatePrintCount");
            proc.AddPara("@WalletRecharge_ID", Convert.ToString(WalletRechargeID));
            int i = proc.RunActionQuery();
            if (i>0)
            {
                EntityCodeDetails = "True";
            }
            return EntityCodeDetails;
        }

        public class DeviceDetails
        {
            public String Payment_Mode { get; set; }
            public String Amount { get; set; }
            public String RefNo { get; set; }
            public String ChequeNo { get; set; }
            public String ChequeDate { get; set; }

            public String BankID { get; set; }
            public String BankName { get; set; }
            public String BranchName { get; set; }

            public String Remark { get; set; }
            public String Guid { get; set; }

        }
    }
}