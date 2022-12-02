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

namespace ServiceManagement.ServiceManagement.Transaction.STB
{
    public partial class stbAdd : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        string UniquePurchaseNumber = string.Empty;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CommonBL ComBL = new CommonBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Hidden_add_edit.Value = "ADD";
                Session["DeviceDetails"] = null;
                string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
                string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinServiceManagement");
                hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;
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
                PopulateEntryType();

                if (Request.QueryString["Key"] != "ADD")
                {
                    PopulateSTBReceivedDetailsById(Request.QueryString["id"]);

                    Hidden_add_edit.Value = Request.QueryString["Key"];
                    hdnSTBReceivedID.Value = Request.QueryString["id"];
                    if (Request.QueryString["Key"] == "edit")
                    {
                        HeaderName.InnerHtml = " Edit STB Received";
                    }
                    else
                    {
                        HeaderName.InnerHtml = " View STB Received";
                    }
                }
                else
                {

                    HeaderName.InnerHtml = " Add STB Received";
                    hdnEntryTypeID.Value = "1";
                    CmbScheme.DataSource = BindNumberingScheme("1");
                    CmbScheme.ValueField = "ID";
                    CmbScheme.TextField = "SchemaName";
                    CmbScheme.DataBind();
                    CmbScheme.SelectedIndex = 0;
                    Hidden_add_edit.Value = "add";
                    hdnSTBReceivedID.Value = "0";
                }
            }
        }

        public DataTable BindNumberingScheme(string ReceiptType)
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string strBranchID = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            //DataTable Schemadt = GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, ReceiptType, "Y");
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBReceivingDetails");
            proc.AddVarcharPara("@ACTION", 500, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, userbranchHierachy);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@Type", 100, ReceiptType);
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

            ddlLocation.DataSource = branchtable;
            ddlLocation.ValueField = "branch_id";
            ddlLocation.TextField = "branch_description";
            ddlLocation.DataBind();
            ddlLocation.SelectedIndex = 0;
        }

        private void PopulateEntryType()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataSet branchtable = dsFetchAll();

            DiviceTyp.DataSource = branchtable.Tables[3];
            DiviceTyp.ValueField = "ID";
            DiviceTyp.TextField = "STBType";
            DiviceTyp.DataBind();
            DiviceTyp.SelectedIndex = 0;

            ddlMSO.DataSource = branchtable.Tables[5];
            ddlMSO.ValueField = "cnt_internalId";
            ddlMSO.TextField = "cnt_firstName";
            ddlMSO.DataBind();
            ddlMSO.SelectedIndex = 0;

            STBModel.DataSource = branchtable.Tables[4];
            STBModel.ValueField = "STBModel_Id";
            STBModel.TextField = "STBModel_Name";
            STBModel.DataBind();
            STBModel.SelectedIndex = 0;

            //ddlProblem.DataSource = branchtable.Tables[2];
            //ddlProblem.ValueField = "ProblemID";
            //ddlProblem.TextField = "ProblemDesc";
            //ddlProblem.DataBind();
            //ddlProblem.SelectedIndex = 0;
        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
            return ds;
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
        public static String AddData(string ChallanDate, string DeviceType, string ChallanNumber, string LCOCode, String LCOName, string Remarks, string MSO, string Quantity,
            String Guids, String Location, string DeviceTypeId, String LocationId, string MSOId, String STBModelID, String STBModel)
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
                    dtable.Columns.Add("DeviceTypeId", typeof(System.String));
                    dtable.Columns.Add("LocationId", typeof(System.String));
                    dtable.Columns.Add("MSOId", typeof(System.String));
                    dtable.Columns.Add("STBModelID", typeof(System.String));
                    dtable.Columns.Add("Location", typeof(System.String));
                    dtable.Columns.Add("ChallanDate", typeof(DateTime));
                    dtable.Columns.Add("ChallanNumber", typeof(System.String));
                    dtable.Columns.Add("LCOCode", typeof(System.String));
                    dtable.Columns.Add("LCOName", typeof(System.String));
                    dtable.Columns.Add("MSO", typeof(System.String));
                    dtable.Columns.Add("STBModel", typeof(System.String));
                    dtable.Columns.Add("DeviceType", typeof(System.String));
                    dtable.Columns.Add("Quantity", typeof(System.String));
                    dtable.Columns.Add("Remarks", typeof(System.String));
                    object[] trow = { Guid.NewGuid(), DeviceTypeId, LocationId, MSOId, STBModelID, Location, ChallanDate, ChallanNumber, LCOCode, LCOName, MSO, STBModel, DeviceType, Quantity, Remarks };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 25)
                        {
                            dt2 = null;
                            dt2 = dt;
                            //DataView dv = new DataView(dt2);
                            //dv.RowFilter = "DeviceNumber=" +Convert.ToString(DeviceNumber.ToString());
                            //var row = dt2.AsEnumerable().FirstOrDefault(r => r.Field<string>("DeviceNumber") == DeviceNumber);
                            //if (row == null)
                            //{
                            object[] trow = { Guid.NewGuid(), DeviceTypeId, LocationId, MSOId, STBModelID, Location, ChallanDate, ChallanNumber, LCOCode, LCOName, MSO,STBModel, DeviceType, Quantity, Remarks };// Add new parameter Here
                            dt.Rows.Add(trow);
                            //}
                            //else
                            //{
                            //    return "Serial number already exists.";
                            //}
                        }
                        else
                        {
                            return "You can't add more device.";
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
                                    item["DeviceTypeId"] = DeviceTypeId;
                                    item["LocationId"] = LocationId;
                                    item["MSOId"] = MSOId;
                                    item["STBModelID"] = STBModelID;
                                    item["Location"] = Location;
                                    item["ChallanDate"] = ChallanDate;
                                    item["ChallanNumber"] = ChallanNumber;
                                    item["LCOCode"] = LCOCode;
                                    item["LCOName"] = LCOName;
                                    item["MSO"] = MSO;
                                    item["STBModel"] = STBModel;
                                    item["DeviceType"] = DeviceType;
                                    item["Quantity"] = Quantity;
                                    item["Remarks"] = Remarks;
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["DeviceDetails"] = dt;
                }

                return "Device Added Successfully.";
            }
            else
            {
                return "Logout";
            }
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
            return "Device Remove Successfully.";
        }

        [WebMethod]
        public static DetailsList EditData(String HiddenID)
        {
            DetailsList ret = new DetailsList();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (HiddenID.ToString() == item["HIddenID"].ToString())
                        {
                            ret.DeviceTypeId = item["DeviceTypeId"].ToString();
                            ret.LocationId = item["LocationId"].ToString();
                            ret.Location = item["Location"].ToString();
                            ret.Guid = item["HIddenID"].ToString();
                            ret.ChallanDate = Convert.ToDateTime(item["ChallanDate"].ToString());
                            ret.ChallanNumber = item["ChallanNumber"].ToString();
                            ret.LCOCode = item["LCOCode"].ToString();
                            ret.LCOName = item["LCOName"].ToString();
                            ret.MSO = item["MSO"].ToString();
                            ret.DeviceType = item["DeviceType"].ToString();
                            ret.Quantity = item["Quantity"].ToString();
                            ret.Remarks = item["Remarks"].ToString();
                            ret.MSOId = item["MSOId"].ToString();
                            ret.STBModelID = item["STBModelID"].ToString();
                            ret.STBModel = item["STBModel"].ToString();
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        [WebMethod]
        public static string GeSerialValidation(String DeviceNumber)
        {
            String status = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
            proc.AddVarcharPara("@ACTION", 500, "SerialValidation");
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                status = dt.Rows[0]["STATUS"].ToString();
            }
            return status;
        }


        [WebMethod]
        public static string save(InsertReceipt apply)
        {
            CommonBL SrvEntry = new CommonBL();


            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string output = string.Empty;
            string DocumentNo = "";
            string strPurchaseNumber = Convert.ToString(apply.DocumentNumber);
            if (apply.Action != "Edit")
            {
                string schmID = apply.CmbScheme.Split('~')[0].ToString();
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
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBReceivingInsertUpdate");
                        proc.AddPara("@ACTION", Convert.ToString(apply.Action));
                        proc.AddPara("@NumberingScheme", Convert.ToString(apply.CmbScheme));
                        proc.AddPara("@DocumentNumber", Convert.ToString(DocumentNo));
                        proc.AddPara("@Document_Date", Convert.ToString(apply.date));
                        proc.AddPara("@Branch_ID", Convert.ToString(apply.branch));
                        proc.AddPara("@Remarks", apply.Remarks);
                        proc.AddPara("@COMPANYID", strCompanyID);
                        proc.AddPara("@FINYEAR", FinYear);
                        proc.AddPara("@CREATED_USER", user_id);
                        proc.AddPara("@STBReceived_ID", apply.STBReceived_ID);
                        proc.AddPara("@UDT_STBReceiving", dt);
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
                    output = "Please Add Device.~";
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

                    sqlQuery = "SELECT max(tjv.DocumentNumber) FROM SRV_STBReceivedHeader tjv WHERE dbo.RegexMatch('";
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
                        sqlQuery = "SELECT max(tjv.DocumentNumber) FROM SRV_STBReceivedHeader tjv WHERE dbo.RegexMatch('";
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
                    sqlQuery = "SELECT DocumentNumber FROM SRV_STBReceivedHeader WHERE DocumentNumber LIKE '" + manual_str.Trim() + "'";
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

        private void PopulateSTBReceivedDetailsById(string receiptID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBReceivingDetails");
                    proc.AddVarcharPara("@ACTION", 300, "ShowDetails");
                    proc.AddVarcharPara("@STBReceived_ID", 100, receiptID.ToString().Trim());
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        divNumberingScheme.Style.Add("display", "none");
                        txtDocumentNumber.Text = ds.Tables[0].Rows[0]["DocumentNumber"].ToString();
                        txtDocumentNumber.Enabled = false;
                        FormDate.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["Document_Date"].ToString());
                        FormDate.ClientEnabled = false;
                        ddlBranch.Value = Convert.ToString(ds.Tables[0].Rows[0]["Branch_ID"].ToString());
                        ddlBranch.ClientEnabled = false;
                        txtHeaderRemarks.Value = ds.Tables[0].Rows[0]["Remarks"].ToString();

                        ddlLocation.Value = Convert.ToString(ds.Tables[1].Rows[0]["Location_ID"].ToString());

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtable = new DataTable();

                            dtable.Clear();
                            dtable.Columns.Add("HIddenID", typeof(System.Guid));
                            dtable.Columns.Add("DeviceTypeId", typeof(System.String));
                            dtable.Columns.Add("LocationId", typeof(System.String));
                            dtable.Columns.Add("MSOId", typeof(System.String));
                            dtable.Columns.Add("STBModelID", typeof(System.String));
                            dtable.Columns.Add("Location", typeof(System.String));
                            dtable.Columns.Add("ChallanDate", typeof(DateTime));
                            dtable.Columns.Add("ChallanNumber", typeof(System.String));
                            dtable.Columns.Add("LCOCode", typeof(System.String));
                            dtable.Columns.Add("LCOName", typeof(System.String));
                            dtable.Columns.Add("MSO", typeof(System.String));
                            dtable.Columns.Add("STBModel", typeof(System.String));
                            dtable.Columns.Add("DeviceType", typeof(System.String));
                            dtable.Columns.Add("Quantity", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                object[] trow = { Guid.NewGuid(), item["STBType_ID"].ToString(), item["Location_ID"].ToString(),item["MSO_ID"].ToString(),item["STBModel_Id"].ToString(),
                                                    item["Location"].ToString(), item["Challan_Date"].ToString(),
                                                        item["ChallanNumber"].ToString(), item["LCO_Code"].ToString(), item["LCO_Name"].ToString(), item["MSO"].ToString(),item["STBModel_Name"].ToString(),
                                                        item["STBType"].ToString(), item["Quantity"].ToString(), item["Remarks"].ToString() };
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

        public class DetailsList
        {
            public String DeviceTypeId { get; set; }
            public String LocationId { get; set; }
            public String MSOId { get; set; }
            public String STBModelID { get; set; }
            public String Location { get; set; }
            public DateTime ChallanDate { get; set; }
            public String ChallanNumber { get; set; }
            public String LCOCode { get; set; }
            public String LCOName { get; set; }
            public String MSO { get; set; }
            public String STBModel { get; set; }
            public String Guid { get; set; }
            public String DeviceType { get; set; }
            public String Quantity { get; set; }
            public String Remarks { get; set; }
        }

        public class InsertReceipt
        {
            public String Action { get; set; }
            public String CmbScheme { get; set; }
            public String DocumentNumber { get; set; }
            public String branch { get; set; }
            public String date { get; set; }
            public String Remarks { get; set; }
            public String STBReceived_ID { get; set; }           
        }
    }
}