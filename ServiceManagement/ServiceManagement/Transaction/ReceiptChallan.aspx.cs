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

namespace ServiceManagement.ServiceManagement.Transaction
{
    public partial class ReceiptChallan : System.Web.UI.Page
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


                string IsEntityInformationEdiableinReceiptChallan = ComBL.GetSystemSettingsResult("IsEntityInformationEdiableinReceiptChallan");
                hdnIsEntityInformationEdiableinReceiptChallan.Value = IsEntityInformationEdiableinReceiptChallan;
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
                //ddlBranch.Value = Convert.ToString(Session["userbranchID"]);
                PopulateEntryType();

                if (Request.QueryString["Key"] != "ADD")
                {
                    PopulateReceiptChalanDetailsById(Request.QueryString["id"]);

                    Hidden_add_edit.Value = Request.QueryString["Key"];
                    hdnReceiptChalanID.Value = Request.QueryString["id"];

                    //if (Request.QueryString["mdl"] == "T")
                    //{
                    //    if (Request.QueryString["Key"] == "edit")
                    //    {
                    //        HeaderName.Text = " Edit Receipt Challan - Token";
                    //    }
                    //    else
                    //    {
                    //        HeaderName.Text = " View Receipt Challan - Token";
                    //    }
                    //}
                    //else if (Request.QueryString["mdl"] == "C")
                    //{
                    //    if (Request.QueryString["Key"] == "edit")
                    //    {
                    //        HeaderName.Text = " Edit Receipt Challan - Challan";
                    //    }
                    //    else
                    //    {
                    //        HeaderName.Text = " View Receipt Challan - Challan";
                    //    }
                    //}
                    //else if (Request.QueryString["mdl"] == "W")
                    //{
                    if (Request.QueryString["Key"] == "edit")
                    {
                        HeaderName.Text = " Edit Receipt Challan";
                    }
                    else
                    {
                        HeaderName.Text = " View Receipt Challan";
                    }
                    // }
                }
                else
                {
                    if (Request.QueryString["mdl"] == "T")
                    {
                        HeaderName.Text = " Add Receipt Challan - Token";
                        hdnEntryTypeID.Value = "1";
                        CmbScheme.DataSource = BindNumberingScheme("1");
                        CmbScheme.ValueField = "ID";
                        CmbScheme.TextField = "SchemaName";
                        CmbScheme.DataBind();
                        CmbScheme.SelectedIndex = 0;
                    }
                    else if (Request.QueryString["mdl"] == "C")
                    {
                        HeaderName.Text = " Add Receipt Challan - Challan";
                        hdnEntryTypeID.Value = "2";
                        CmbScheme.DataSource = BindNumberingScheme("2");
                        CmbScheme.ValueField = "ID";
                        CmbScheme.TextField = "SchemaName";
                        CmbScheme.DataBind();
                        CmbScheme.SelectedIndex = 0;
                    }
                    else if (Request.QueryString["mdl"] == "W")
                    {
                        HeaderName.Text = " Add Receipt Challan - Worksheet";
                        hdnEntryTypeID.Value = "3";
                        CmbScheme.DataSource = BindNumberingScheme("3");
                        CmbScheme.ValueField = "ID";
                        CmbScheme.TextField = "SchemaName";
                        CmbScheme.DataBind();
                        CmbScheme.SelectedIndex = 0;
                    }

                    Hidden_add_edit.Value = "add";
                    //   HeaderName.Text = " Add Receipt Challan";
                    hdnReceiptChalanID.Value = "0";
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
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
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
        }

        private void PopulateEntryType()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataSet branchtable = dsFetchAll();
            //ddlEntryType.DataSource = branchtable.Tables[0];
            //ddlEntryType.ValueField = "ID";
            //ddlEntryType.TextField = "TYPE";
            //ddlEntryType.DataBind();
            //ddlEntryType.SelectedIndex = 0;

            DiviceTyp.DataSource = branchtable.Tables[1];
            DiviceTyp.ValueField = "ID";
            DiviceTyp.TextField = "DeviceType";
            DiviceTyp.DataBind();
            DiviceTyp.SelectedIndex = 1;

            ddlProblem.DataSource = branchtable.Tables[2];
            ddlProblem.ValueField = "ProblemID";
            ddlProblem.TextField = "ProblemDesc";
            ddlProblem.DataBind();
            ddlProblem.SelectedIndex = 0;

            // Mantis Issue 24413
            selModel.DataSource = branchtable.Tables[6];
            selModel.ValueField = "ModelID";
            selModel.TextField = "ModelDesc";
            selModel.DataBind();
            selModel.SelectedIndex = 0;
            // End of Mantis Issue 24413
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
        public static String AddData(string DeviceType, string Model, string DeviceNumber, string Warranty, String Problem, string Remarks, string Remote, string CardAdaptor,
            String Guids, String DeviceTypeId, string ProblemID)
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
                    dtable.Columns.Add("ProblemID", typeof(System.String));
                    dtable.Columns.Add("DeviceType", typeof(System.String));
                    dtable.Columns.Add("Model", typeof(System.String));
                    dtable.Columns.Add("DeviceNumber", typeof(System.String));
                    dtable.Columns.Add("Warranty", typeof(System.String));
                    dtable.Columns.Add("Problem", typeof(System.String));
                    dtable.Columns.Add("Remarks", typeof(System.String));
                    dtable.Columns.Add("Remote", typeof(System.String));
                    dtable.Columns.Add("CardAdaptor", typeof(System.String));
                    object[] trow = { Guid.NewGuid(), DeviceTypeId, ProblemID, DeviceType, Model, DeviceNumber, Warranty, Problem, Remarks, Remote, CardAdaptor };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 10)
                        {
                            dt2 = null;
                            dt2 = dt;
                            //DataView dv = new DataView(dt2);
                            //dv.RowFilter = "DeviceNumber=" +Convert.ToString(DeviceNumber.ToString());
                            var row = dt2.AsEnumerable().FirstOrDefault(r => r.Field<string>("DeviceNumber") == DeviceNumber);
                            if (row == null)
                            {
                                object[] trow = { Guid.NewGuid(), DeviceTypeId, ProblemID, DeviceType, Model, DeviceNumber, Warranty, Problem, Remarks, Remote, CardAdaptor };// Add new parameter Here
                                dt.Rows.Add(trow);
                            }
                            else
                            {
                                return "Serial number already exists.";
                            }
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
                                    item["ProblemID"] = ProblemID;
                                    item["DeviceType"] = DeviceType;
                                    item["Model"] = Model;
                                    item["DeviceNumber"] = DeviceNumber;
                                    item["Warranty"] = Warranty;
                                    item["Problem"] = Problem;
                                    item["Remarks"] = Remarks;
                                    item["Remote"] = Remote;
                                    item["CardAdaptor"] = CardAdaptor;
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
                            ret.DeviceType = item["DeviceType"].ToString();
                            ret.Model = item["Model"].ToString();
                            ret.DeviceNumber = item["DeviceNumber"].ToString();
                            ret.Guid = item["HIddenID"].ToString();
                            ret.Warranty = item["Warranty"].ToString();
                            ret.Problem = item["Problem"].ToString();
                            ret.Remarks = item["Remarks"].ToString();
                            ret.Remote = item["Remote"].ToString();
                            ret.CardAdaptor = item["CardAdaptor"].ToString();
                            ret.DeviceTypeId = item["DeviceTypeId"].ToString();
                            ret.ProblemID = item["ProblemID"].ToString();
                            break;
                        }
                    }
                }
            }
            return ret;// "Holiday Remove Sucessfylly";
        }

        [WebMethod]
        public static List<ListItem> bindScheme(string ReceiptType)
        {
            List<ListItem> listStatues = new List<ListItem>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                string type = "0";
                if (ReceiptType == "1")
                {
                    type = "134";
                }
                else if (ReceiptType == "2")
                {
                    type = "135";
                }
                else if (ReceiptType == "3")
                {
                    type = "136";
                }

                Scheme obj = new Scheme();
                string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string strBranchID = Convert.ToString(HttpContext.Current.Session["userbranchID"]);
                string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //DataTable Schemadt = GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, ReceiptType, "Y");
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
                proc.AddVarcharPara("@ACTION", 500, "GetNumberingSchema");
                proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
                proc.AddVarcharPara("@BranchID", 4000, "2");
                proc.AddVarcharPara("@FinYear", 100, FinYear);
                proc.AddVarcharPara("@Type", 100, type);
                proc.AddVarcharPara("@IsSplit", 100, "Y");
                ds = proc.GetTable();
                foreach (DataRow item in ds.Rows)
                {
                    //obj = new ListItem;
                    //obj.Id = Convert.ToString(item["Id"]);
                    //obj.SchemaName = Convert.ToString(item["SchemaName"]);
                    //listStatues.Add(obj);
                    listStatues.Add(new ListItem
                    {
                        Value = item["ID"].ToString(),
                        Text = item["SchemaName"].ToString()
                    });
                }
            }
            return listStatues;
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
        public static string save(InsertReceipt apply)
        {
            CommonBL SrvEntry = new CommonBL();
            string SendSMSforServiceManagement = SrvEntry.GetSystemSettingsResult("SendSMSforServiceManagement");
            if (!String.IsNullOrEmpty(SendSMSforServiceManagement))
            {
                if (SendSMSforServiceManagement == "No")
                {
                    apply.sendSMS = "No";
                }
            }
            else
            {
                apply.sendSMS = "No";
            }

            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string output = string.Empty;
            string DocumentNo = "";
            string strPurchaseNumber = Convert.ToString(apply.DocumentNumber);
            if (apply.Action != "edit")
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

            string type = "0";
            if (apply.EntryType == "1")
            {
                type = "134";
            }
            else if (apply.EntryType == "2")
            {
                type = "135";
            }
            else if (apply.EntryType == "3")
            {
                type = "136";
            }

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanInsertUpdate");
                        proc.AddVarcharPara("@EntryType", 25, Convert.ToString(apply.EntryType));
                        proc.AddVarcharPara("@NumberingScheme", 100, Convert.ToString(apply.CmbScheme));
                        proc.AddVarcharPara("@DocumentNumber", 500, Convert.ToString(DocumentNo));
                        proc.AddVarcharPara("@DocumentDate", 10, Convert.ToString(apply.date));
                        proc.AddVarcharPara("@Branch", 10, Convert.ToString(apply.branch));
                        proc.AddVarcharPara("@EntityCode", 300, apply.EntityCode);
                        proc.AddPara("@UDT_ReceiptChallanDetails", dt);
                        proc.AddVarcharPara("@Action", 30, Convert.ToString(apply.Action));
                        proc.AddVarcharPara("@NetworkName", 25, Convert.ToString(apply.NetworkName));
                        proc.AddPara("@ContactPerson", apply.ContactPerson);
                        proc.AddPara("@NumberSchemaId", type);
                        proc.AddPara("@USER_ID", user_id);
                        proc.AddPara("@ReceiptChallan_ID", apply.ReceiptChallan_ID);
                        proc.AddPara("@ContactNo", apply.ContactNo);

                        proc.AddPara("@COMPANYID", strCompanyID);
                        proc.AddPara("@FINYEAR", FinYear);

                        proc.AddPara("@sendSMS", apply.sendSMS);
                        //  proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                        dtview = proc.GetTable();
                        // output = Convert.ToString(proc.GetParaValue("@is_success"));
                        //if (NoOfRowEffected > 0)
                        //{
                        if (dtview != null && dtview.Rows.Count > 0)
                        {
                            output = "true~" + dtview.Rows[0]["DocumentID"].ToString() + "~" + DocumentNo + "~" + apply.Action;
                        }

                        //}
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

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
            return ds;
        }

        protected void ddlNumberingScheme_Callback(object sender, CallbackEventArgsBase e)
        {
            string command = e.Parameter;
            if (command == "1")
            {
                Bind_NumberingScheme("134");
                // ddlNumberingScheme.Value = "0";
            }
            else if (command == "2")
            {
                Bind_NumberingScheme("135");
                // ddlNumberingScheme.Value = "0";
            }
            else if (command == "3")
            {
                Bind_NumberingScheme("136");
                //  ddlNumberingScheme.Value = "0";
            }
        }

        public void Bind_NumberingScheme(String strType)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, strType, "Y");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                // ddlNumberingScheme.TextField = "SchemaName";
                // ddlNumberingScheme.ValueField = "Id";
                // ddlNumberingScheme.DataSource = Schemadt;
                // ddlNumberingScheme.DataBind();
            }
        }

        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
            proc.AddVarcharPara("@ACTION", 500, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }

        private void PopulateReceiptChalanDetailsById(string receiptID)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
                    proc.AddVarcharPara("@ACTION", 300, "ShowDetails");
                    proc.AddVarcharPara("@ReceiptChallan_ID", 100, receiptID.ToString().Trim());
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        ddlEntryType.SelectedValue = ds.Tables[0].Rows[0]["EntryType"].ToString();
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

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtable = new DataTable();

                            dtable.Clear();
                            dtable.Columns.Add("HIddenID", typeof(System.Guid));
                            dtable.Columns.Add("DeviceTypeId", typeof(System.String));
                            dtable.Columns.Add("ProblemID", typeof(System.String));
                            dtable.Columns.Add("DeviceType", typeof(System.String));
                            dtable.Columns.Add("Model", typeof(System.String));
                            dtable.Columns.Add("DeviceNumber", typeof(System.String));
                            dtable.Columns.Add("Warranty", typeof(System.String));
                            dtable.Columns.Add("Problem", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));
                            dtable.Columns.Add("Remote", typeof(System.String));
                            dtable.Columns.Add("CardAdaptor", typeof(System.String));

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                object[] trow = { Guid.NewGuid(), item["DeviceType"].ToString(), item["Problem"].ToString(), item["DeviceTypeDesc"].ToString(), item["Model"].ToString(),
                                                        item["DeviceNumber"].ToString(), item["Warranty"].ToString(), item["ProblemDesc"].ToString(), item["Remarks"].ToString(),
                                                        item["REMOTE_STATUS"].ToString(), item["CardAdaptor_STATUS"].ToString() };
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

                    sqlQuery = "SELECT max(tjv.DocumentNumber) FROM SRV_ReceiptChallanHeader tjv WHERE dbo.RegexMatch('";
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
                        sqlQuery = "SELECT max(tjv.DocumentNumber) FROM SRV_ReceiptChallanHeader tjv WHERE dbo.RegexMatch('";
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
                    sqlQuery = "SELECT DocumentNumber FROM SRV_ReceiptChallanHeader WHERE DocumentNumber LIKE '" + manual_str.Trim() + "'";
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
        public static List<ServiceEntryHistory> ServiceEntryHistoryList(String model, String DeviceNumber)
        {
            List<ServiceEntryHistory> listStatues = new List<ServiceEntryHistory>();
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryHistory");
            proc.AddPara("@ModelNumber", model);
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    listStatues.Add(new ServiceEntryHistory
                    {
                        EntityCode = item["EntityCode"].ToString(),
                        ReceiptNo = item["DocumentNumber"].ToString(),
                        ServiceAction = item["SrvActionDesc"].ToString(),
                        Remarks = item["Remarks"].ToString(),
                        Billable = item["Billable"].ToString()
                    });
                }
            }
            return listStatues;
        }

        [WebMethod]
        public static ServiceEntryCount ServiceEntryCountShow(String model, String DeviceNumber)
        {
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ServiceDataDetails");
            proc.AddVarcharPara("@ACTION", 500, "ServiceEntryCount");
            proc.AddPara("@ModelNumber", model);
            proc.AddPara("@DeviceNumber", DeviceNumber);
            ds = proc.GetTable();
            ServiceEntryCount ret = new ServiceEntryCount();
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow item in ds.Rows)
                {
                    ret.Repaired = item["Repaired"].ToString();
                    ret.Exchanged = item["Exchanged"].ToString();
                    break;
                }
            }
            return ret;
        }

        [WebMethod]
        public static string GetWarrantyDate(String DeviceNumber)
        {
            String warranty = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            proc.AddVarcharPara("@ACTION", 500, "GetWarrantyDate");
            proc.AddPara("@DeviceNumber", DeviceNumber);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                warranty = dt.Rows[0]["Warranty"].ToString();
            }
            return warranty;
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

        // Mantis Issue 24412
        [WebMethod]
        public static string GetNetworkName(String EntityCode)
        {
            String NetworkName = "";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVWarrantyUpdateFetch");
            proc.AddVarcharPara("@ACTION", 500, "GetNetworkName");
            proc.AddPara("@EntityCode", EntityCode);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                NetworkName = dt.Rows[0]["NetworkName"].ToString();
            }
            return NetworkName;
        }
        // End of Mantis Issue 24412

        public class DeviceDetails
        {
            public String DeviceType { get; set; }
            public String Model { get; set; }
            public String DeviceNumber { get; set; }
            public String Warranty { get; set; }
            public String Problem { get; set; }
            public String Remarks { get; set; }
            public String Remote { get; set; }
            public String CardAdaptor { get; set; }
            public String Guid { get; set; }
            public String DeviceTypeId { get; set; }
            public String ProblemID { get; set; }
        }

        public class Scheme
        {
            public String Id { get; set; }
            public String SchemaName { get; set; }
        }

        public class InsertReceipt
        {
            public String EntryType { get; set; }
            public String CmbScheme { get; set; }
            public String DocumentNumber { get; set; }
            public String branch { get; set; }
            public String date { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String Action { get; set; }
            public String ReceiptChallan_ID { get; set; }
            public String ContactNo { get; set; }
            public String sendSMS { get; set; }
        }

        public class ServiceEntryHistory
        {
            public String EntityCode { get; set; }
            public String ReceiptNo { get; set; }
            public String ServiceAction { get; set; }
            public String Remarks { get; set; }
            public String Billable { get; set; }
        }

        public class ServiceEntryCount
        {
            public String Repaired { get; set; }
            public String Exchanged { get; set; }
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
    }
}