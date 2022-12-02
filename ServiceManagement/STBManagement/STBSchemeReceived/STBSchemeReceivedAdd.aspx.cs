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

namespace ServiceManagement.STBManagement.STBSchemeReceived
{
    public partial class STBSchemeReceivedAdd : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        string UniquePurchaseNumber = string.Empty;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CommonBL ComBL = new CommonBL();

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
                Hidden_add_edit.Value = "ADD";
                Session["DeviceDetails"] = null;
                string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
                string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinServiceManagement");
                hdnOnlinePrint.Value = AllowOnlinePrintinServiceManagement;

                string IsEntityInformationEditableInSTBSchemeReceived = ComBL.GetSystemSettingsResult("IsEntityInformationEditableInSTBSchemeReceived");
                hdnIsEntityInformationEditableInSTBSchemeReceived.Value = IsEntityInformationEditableInSTBSchemeReceived;

                string IsDuplicateSerialNoAllowedinSTBSchemeRec = ComBL.GetSystemSettingsResult("IsDuplicateSerialNoAllowedinSTBSchemeRec");
                hdnIsDuplicateSerialNoAllowedinSTBSchemeRec.Value = IsDuplicateSerialNoAllowedinSTBSchemeRec;

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
                    PopulateReceiptChalanDetailsById(Request.QueryString["id"]);

                    Hidden_add_edit.Value = Request.QueryString["Key"];
                    hdnSchemeReceivedID.Value = Request.QueryString["id"];
                    if (Request.QueryString["Key"] == "edit")
                    {
                        HeaderName.Text = " Edit STB Scheme - Received";
                    }
                    else
                    {
                        HeaderName.Text = " View STB Scheme - Received";
                    }
                }
                else
                {
                    HeaderName.Text = " Add STB Scheme - Received";
                    hdnEntryTypeID.Value = "1";
                    CmbScheme.DataSource = BindNumberingScheme("1");
                    CmbScheme.ValueField = "ID";
                    CmbScheme.TextField = "SchemaName";
                    CmbScheme.DataBind();
                    CmbScheme.SelectedIndex = 0;

                    Hidden_add_edit.Value = "add";
                    hdnSchemeReceivedID.Value = "0";

                    string fDate = null;
                    fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
                    FormDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    divcross.Visible = false;
                    hdnIsAproval.Value = "Yes";
                    divApprovalSectionhdr.Attributes.Add("removeclass", "hide");
                    divApprovalSectiondtls.Attributes.Add("removeclass", "hide");
                }
                else
                {
                    divApprovalSectionhdr.Attributes.Add("class", "hide");
                    divApprovalSectiondtls.Attributes.Add("class", "hide");
                    hdnIsAproval.Value = "No";
                }
            }
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

        public DataTable BindNumberingScheme(string ReceiptType)
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
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

        private void PopulateEntryType()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataSet branchtable = dsFetchAll();

            DiviceTyp.DataSource = branchtable.Tables[1];
            DiviceTyp.ValueField = "ID";
            DiviceTyp.TextField = "DeviceType";
            DiviceTyp.DataBind();
            DiviceTyp.SelectedIndex = 1;
        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_SRVReceiptChallanDetails");
            proc.AddVarcharPara("@ACTION", 500, "FETCH");
            DataSet ds = proc.GetDataSet();
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
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
                    proc.AddVarcharPara("@ACTION", 300, "ShowDetails");
                    proc.AddVarcharPara("@SchemeReceived_ID", 100, receiptID.ToString().Trim());
                    ds = proc.GetDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
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
                            dtable.Columns.Add("DeviceType", typeof(System.String));
                            dtable.Columns.Add("Model", typeof(System.String));
                            dtable.Columns.Add("DeviceNumber", typeof(System.String));
                            dtable.Columns.Add("Rate", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));
                            dtable.Columns.Add("Remote", typeof(System.String));
                            dtable.Columns.Add("CardAdaptor", typeof(System.String));

                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                object[] trow = { Guid.NewGuid(), item["DeviceType"].ToString(), item["DeviceTypeDesc"].ToString(), item["Model"].ToString(),
                                                        item["DeviceNumber"].ToString(), item["Rate"].ToString(),item["Remarks"].ToString(),
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
        public static String AddData(string DeviceType, string Model, string DeviceNumber, String Rate, string Remarks, string Remote, string CardAdaptor,
            String Guids, String DeviceTypeId)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails"];
                DataTable dt2 = new DataTable();

                if (dt == null)
                {
                    if (FetchDeviceNumber(DeviceNumber))
                    {
                        DataTable dtable = new DataTable();

                        dtable.Clear();
                        dtable.Columns.Add("HIddenID", typeof(System.Guid));
                        dtable.Columns.Add("DeviceTypeId", typeof(System.String));
                        dtable.Columns.Add("DeviceType", typeof(System.String));
                        dtable.Columns.Add("Model", typeof(System.String));
                        dtable.Columns.Add("DeviceNumber", typeof(System.String));
                        dtable.Columns.Add("Rate", typeof(System.String));
                        dtable.Columns.Add("Remarks", typeof(System.String));
                        dtable.Columns.Add("Remote", typeof(System.String));
                        dtable.Columns.Add("CardAdaptor", typeof(System.String));
                        
                        object[] trow = { Guid.NewGuid(), DeviceTypeId, DeviceType, Model, DeviceNumber, Rate, Remarks, Remote, CardAdaptor };// Add new parameter Here
                        dtable.Rows.Add(trow);
                        HttpContext.Current.Session["DeviceDetails"] = dtable;
                    }
                    else
                    {
                        return "Serial number already exists.";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 20)
                        {
                            if (FetchDeviceNumber(DeviceNumber))
                            {
                                dt2 = null;
                                dt2 = dt;
                                var row = dt2.AsEnumerable().FirstOrDefault(r => r.Field<string>("DeviceNumber") == DeviceNumber);
                                if (row == null)
                                {
                                    object[] trow = { Guid.NewGuid(), DeviceTypeId, DeviceType, Model, DeviceNumber, Rate, Remarks, Remote, CardAdaptor };// Add new parameter Here
                                    dt.Rows.Add(trow);
                                }
                                else
                                {
                                    return "Serial number already exists.";
                                }
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
                                    item["DeviceType"] = DeviceType;
                                    item["Model"] = Model;
                                    item["DeviceNumber"] = DeviceNumber;
                                    item["Rate"] = Rate;
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
                            ret.Rate = item["Rate"].ToString();
                            ret.Remarks = item["Remarks"].ToString();
                            ret.Remote = item["Remote"].ToString();
                            ret.CardAdaptor = item["CardAdaptor"].ToString();
                            ret.DeviceTypeId = item["DeviceTypeId"].ToString();
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
            return "Device Remove Successfully.";
        }

        public class DeviceDetails
        {
            public String DeviceType { get; set; }
            public String Model { get; set; }
            public String DeviceNumber { get; set; }
            public String Rate { get; set; }
            public String Remarks { get; set; }
            public String Remote { get; set; }
            public String CardAdaptor { get; set; }
            public String Guid { get; set; }
            public String DeviceTypeId { get; set; }
        }

        [WebMethod]
        public static string GetModelPrice(String Model, String Remote, String CordAdapter)
        {
            String Price = "0";
            int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
            proc.AddVarcharPara("@ACTION", 500, "GetPricefromModel");
            proc.AddPara("@Remote", Remote);
            proc.AddPara("@CordAdapter", CordAdapter);
            proc.AddPara("@Model", Model);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Price = dt.Rows[0]["Price"].ToString();
            }
            return Price;
        }

        [WebMethod]
        public static string save(InsertReceipt apply)
        {
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            string output = string.Empty;
            string DocumentNo = "", schmID = "";
            string strPurchaseNumber = Convert.ToString(apply.DocumentNumber);
            if (apply.Action == "add")
            {
                schmID = apply.CmbScheme.Split('~')[0].ToString();
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
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedInsertUpdate");
                        proc.AddVarcharPara("@NumberingScheme", 100, Convert.ToString(apply.CmbScheme));
                        proc.AddVarcharPara("@DocumentNumber", 500, Convert.ToString(DocumentNo));
                        proc.AddVarcharPara("@DocumentDate", 10, Convert.ToString(apply.date));
                        proc.AddVarcharPara("@Branch", 10, Convert.ToString(apply.branch));
                        proc.AddVarcharPara("@EntityCode", 300, apply.EntityCode);
                        proc.AddPara("@udt_STBSchemeReceivedDetails", dt);
                        proc.AddVarcharPara("@Action", 30, Convert.ToString(apply.Action));
                        proc.AddVarcharPara("@NetworkName", 25, Convert.ToString(apply.NetworkName));
                        proc.AddPara("@ContactPerson", apply.ContactPerson);
                        proc.AddPara("@NumberSchemaId", schmID);
                        proc.AddPara("@USER_ID", user_id);
                        proc.AddPara("@SchemeReceived_ID", apply.SchemeReceived_ID);
                        proc.AddPara("@ContactNo", apply.ContactNo);
                        proc.AddPara("@COMPANYID", strCompanyID);
                        proc.AddPara("@FINYEAR", FinYear);
                        if (apply.IsApproval == "Yes")
                        {
                            proc.AddPara("@ApprovalAction", apply.ApprovalAction);
                            proc.AddPara("@ApprovalRemarks", apply.ApprovalRemarks);
                        }

                        dtview = proc.GetTable();
                        if (dtview != null && dtview.Rows.Count > 0)
                        {
                            output = "true~" + dtview.Rows[0]["DocumentID"].ToString() + "~" + dtview.Rows[0]["DocumentNumber"].ToString() + "~" + apply.Action;
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

        public class InsertReceipt
        {
            public String CmbScheme { get; set; }
            public String DocumentNumber { get; set; }
            public String branch { get; set; }
            public String date { get; set; }
            public String EntityCode { get; set; }
            public String NetworkName { get; set; }
            public String ContactPerson { get; set; }
            public String Action { get; set; }
            public String SchemeReceived_ID { get; set; }
            public String ContactNo { get; set; }

            public String ApprovalAction { get; set; }
            public String ApprovalRemarks { get; set; }
            public String IsApproval { get; set; }
        }

        [WebMethod]
        public static string FetchEntityFromMaster(String entity_code)
        {
            //List<EntityCodeList> EntityCodeList = new List<EntityCodeList>();
            string EntityCodeDetails = "";
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
            proc.AddVarcharPara("@Action", 500, "FetchEntityFromMaster");
            proc.AddPara("@Module", 13);
            proc.AddPara("@EntityCode", Convert.ToString(entity_code));
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                EntityCodeDetails = dt.Rows[0]["cnt_UCC"].ToString() + '~' + dt.Rows[0]["cnt_firstName"].ToString() + '~' + dt.Rows[0]["cnt_ContactPerson"].ToString() + '~' + dt.Rows[0]["cnt_ContactNo"].ToString() + '~' + dt.Rows[0]["Is_Active"].ToString();
            }
            return EntityCodeDetails;
        }

        public static bool FetchDeviceNumber(String DeviceNumber)
        {
            CommonBL ComBL = new CommonBL();
            string IsDuplicateSerialNoAllowedinSTBSchemeRec = ComBL.GetSystemSettingsResult("IsDuplicateSerialNoAllowedinSTBSchemeRec");

            bool DeviceNumberStatus = true;
            if (IsDuplicateSerialNoAllowedinSTBSchemeRec=="No")
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeReceivedDetails");
                proc.AddVarcharPara("@Action", 500, "FetchDeviceNumber");
                proc.AddPara("@DeviceNumber", Convert.ToString(DeviceNumber));
                dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DeviceNumberStatus = false;
                }
            }           
            return DeviceNumberStatus;
        }
    }
}