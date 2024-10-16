/***********************************************************************************************************************************
 * Rev 1.0      Sanchita    16/10/2024      0027747: Need to Implement existing SMS sending to Normal link instead of Bitly for GTPL
 * *********************************************************************************************************************************/
using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceManagement.STBManagement.STBSchemeRequisition
{
    public partial class STBSchemeRequisition : System.Web.UI.Page
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
                Session["DeviceDetails2"] = null;
                string MultiBranchNumberingScheme = ComBL.GetSystemSettingsResult("NumberingSchemeWithMultiBranch");
                string AllowOnlinePrintinServiceManagement = ComBL.GetSystemSettingsResult("AllowOnlinePrintinWalletRecharge");
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
                AddmodeExecuted();
                txtAmount.ClientEnabled = false;
                if (Request.QueryString["Key"].ToUpper() != "ADD")
                {
                    if (Request.QueryString["Key"].ToUpper() == "EDIT")
                    {
                        HeaderName.Text = "Modify STB Scheme - Requisition";
                        hdAddEdit.Value = "edit";
                    }
                    else
                    {
                        HeaderName.Text = " View STB Scheme - Requisition";
                        hdAddEdit.Value = "view";
                    }
                    string STBRequisitionID = Request.QueryString["id"];
                    hdnSTBRequisitionID.Value = Request.QueryString["id"];
                    EditModeExecute(STBRequisitionID);
                }
                else
                {
                    HeaderName.Text = "Add STB Scheme - Requisition";
                    hdAddEdit.Value = "Add";
                    string fDate = null;
                    fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
                    FormDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeRequisitionDetails");
                        proc.AddVarcharPara("@ACTION", 300, "GetSchemeReceived");
                        proc.AddVarcharPara("@SchemeReceived_id", 100, Request.QueryString["Rcvid"]);
                        DataTable dt = proc.GetTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            txtSTBSchemeReceived.Value = Convert.ToString(dt.Rows[0]["DocumentNumber"]);
                            hdnSTBSchemeReceived.Value = Convert.ToString(dt.Rows[0]["SchemeReceived_ID"]);
                        }
                    }
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
                    ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeRequisitionDetails");
                    proc.AddVarcharPara("@ACTION", 300, "ShowDetails");
                    proc.AddVarcharPara("@SchemeRequisition_id", 100, receiptID.ToString().Trim());
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

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtable = new DataTable();

                            dtable.Clear();
                            dtable.Columns.Add("HIddenID", typeof(System.Guid));
                            dtable.Columns.Add("Model", typeof(System.String));
                            dtable.Columns.Add("UnitPrice", typeof(System.String));
                            dtable.Columns.Add("Quantity", typeof(System.String));
                            dtable.Columns.Add("Amount", typeof(System.String));
                            dtable.Columns.Add("OSTBVendor", typeof(System.String));
                            dtable.Columns.Add("Remarks", typeof(System.String));
                            dtable.Columns.Add("Model_ID", typeof(System.String));
                            dtable.Columns.Add("OSTBVendor_ID", typeof(System.String));


                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                object[] trow = { Guid.NewGuid(), item["Model"].ToString(), item["UnitPrice"].ToString(), item["Quantity"].ToString(), item["Amount"].ToString(),
                                                        item["Remarks"].ToString(), item["Model_ID"].ToString()};
                                dtable.Rows.Add(trow);
                            }

                            HttpContext.Current.Session["DeviceDetails"] = dtable;
                            GrdDevice.DataBind();
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[0]["RequisitionFor_Id"].ToString()) != "1")
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "Details2Hide('" + Convert.ToString(ds.Tables[0].Rows[0]["RequisitionFor"].ToString()) + "')", true);
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

            ddlOSTBVendor.DataSource = branchtable.Tables[4];
            ddlOSTBVendor.ValueField = "cnt_internalId";
            ddlOSTBVendor.TextField = "OSTBVendor";
            ddlOSTBVendor.DataBind();
            ddlOSTBVendor.SelectedIndex = 0;

        }

        public DataSet dsFetchAll()
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeRequisitionDetails");
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
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeRequisitionDetails");
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
            DataTable dt2 = (DataTable)HttpContext.Current.Session["DeviceDetails2"];
            bool STATUS = false;
            DataTable dt3 = new DataTable();
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (apply.RequisitionFor == "1")
                    {
                        if (HttpContext.Current.Session["DeviceDetails2"] == null)
                        {
                            dt3.Clear();
                            dt3.Columns.Add("HIddenID", typeof(System.Guid));
                            dt3.Columns.Add("Model", typeof(System.String));
                            dt3.Columns.Add("UnitPrice", typeof(System.String));
                            dt3.Columns.Add("Quantity", typeof(System.String));
                            dt3.Columns.Add("Amount", typeof(System.String));
                            dt3.Columns.Add("Remarks", typeof(System.String));
                            dt3.Columns.Add("Model_ID", typeof(System.String));
                            dt3.Columns.Add("ostbVendor", typeof(System.String));
                            dt3.Columns.Add("ostbVendorID", typeof(System.String));
                        }
                        STATUS = true;
                    }
                    else
                    {
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            dt3 = dt2;
                            if (Convert.ToDecimal(dt2.Rows[0]["Quantity"]) == Convert.ToDecimal(dt.Rows[0]["Quantity"]))
                            {
                                STATUS = true;
                            }
                            else
                            {
                                output = "Both Quantity must be same.~";
                                STATUS = false;
                            }
                        }
                        else
                        {
                            output = "Please add STB details.~";
                            STATUS = false;
                        }
                    }
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
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionInsertUpdate");
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
                        proc.AddPara("@UDT_STBRequisitionReceivingDetails", dt3);
                        proc.AddPara("@Action", Convert.ToString(apply.Action));
                        proc.AddPara("@SchemaID", schmID);
                        proc.AddPara("@USER_ID", user_id);
                        proc.AddPara("@STBRequisitionID", apply.STBRequisitionID);
                        proc.AddPara("@COMPANYID", strCompanyID);
                        proc.AddPara("@FINYEAR", FinYear);
                        proc.AddPara("@IsNoPayment", apply.IsNoPayment);
                        proc.AddPara("@PaymentRelatedRemarks", apply.PaymentRelatedRemarks);
                        proc.AddPara("@IsPayUsingOnAcountBalance", apply.IsPayUsingOnAcountBalance);
                        if (apply.IsApproval == "Yes")
                        {
                            proc.AddPara("@ApprovalAction", apply.ApprovalAction);
                            //proc.AddPara("@DirectorApprovalRequired", apply.DirectorApprovalRequired);
                            proc.AddPara("@ApprovalEmployee", apply.ApprovalEmployee);
                            proc.AddPara("@ApprovalRemarks", apply.ApprovalRemarks);
                        }
                        proc.AddPara("@IsInventory", apply.IsInventory);

                        proc.AddPara("@SchemeId", schmID);

                        dtview = proc.GetTable();

                        if (dtview != null && dtview.Rows.Count > 0)
                        {
                            output = "true~" + dtview.Rows[0]["DocumentID"].ToString() + "~" + dtview.Rows[0]["DocumentNo"].ToString() + "~" + apply.Action;
                            if (apply.DirectorApprovalRequired == "1")
                            {
                                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                                string DataBase = con.Database;

                                string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
                                // Rev 1.0
                                //string LongURL = baseUrl + "/STBManagement/reqApprovalM/reqApproval.aspx?id=" + dtview.Rows[0]["DocumentID"].ToString() + "&AU=" + Convert.ToString(apply.ApprovalEmployee)
                                //                    + "&UniqueKey=" + Convert.ToString(DataBase);

                                //string tinyURL = ShortURL(LongURL);

                                string LongURL = baseUrl + "/STBManagement/reqApprovalM/reqApproval.aspx?id=" + dtview.Rows[0]["DocumentID"].ToString() + "%26AU=" + Convert.ToString(apply.ApprovalEmployee)
                                                    + "%26UniqueKey=" + Convert.ToString(DataBase);

                                // End of Rev 1.0

                                ProcedureExecute proc1 = new ProcedureExecute("PRC_STBRequisitionInsertUpdate");
                                proc1.AddPara("@Action", Convert.ToString("ApprovalSendSMS"));
                                // Rev 1.0
                                //proc1.AddPara("@tinyURL", Convert.ToString(tinyURL));
                                proc1.AddPara("@longURL", Convert.ToString(LongURL));
                                // End of Rev 1.0
                                proc1.AddPara("@DocumentNumber", Convert.ToString(dtview.Rows[0]["DocumentNo"].ToString()));
                                proc1.AddPara("@ApprovalEmployee", apply.ApprovalEmployee);
                                dtview = proc1.GetTable();
                            }
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

        private static string ShortURL(string LongUrl)
        {
            System.Uri address = new System.Uri("http://tinyurl.com/api-create.php?url=" + LongUrl);
            System.Net.WebClient client = new System.Net.WebClient();
            string tinyUrl = client.DownloadString(address);
            return tinyUrl;
        }

        #region Details1 Section

        [WebMethod]
        public static String AddData(string Model, string UnitPrice, string Quantity, string Amount, String Remarks, string Model_ID, String Guids
            , String OSTBVendor, String OSTBVendor_ID)
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
                    dtable.Columns.Add("OSTBVendor", typeof(System.String));
                    dtable.Columns.Add("Remarks", typeof(System.String));
                    dtable.Columns.Add("Model_ID", typeof(System.String));
                    dtable.Columns.Add("OSTBVendor_ID", typeof(System.String));

                    object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, OSTBVendor, Remarks, Model_ID, OSTBVendor_ID };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 1)
                        {
                            object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, OSTBVendor, Remarks, Model_ID, OSTBVendor_ID };// Add new parameter Here
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
                                    item["OSTBVendor"] = OSTBVendor;
                                    item["Remarks"] = Remarks;
                                    item["Model_ID"] = Model_ID;
                                    item["OSTBVendor_ID"] = OSTBVendor_ID;
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
                            ret.OSTBVendor = item["OSTBVendor"].ToString();
                            ret.Remarks = item["Remarks"].ToString();
                            ret.Model_ID = item["Model_ID"].ToString();
                            ret.OSTBVendor_ID = item["OSTBVendor_ID"].ToString();
                            break;
                        }
                    }
                }
            }
            return ret;
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
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeRequisitionDetails");
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
        public static string GetUnitPrice(String ModelId, String DAS)
        {
            string UnitPrice = "0.00";
            ProcedureExecute proc = new ProcedureExecute("PRC_STBSchemeRequisitionDetails");
            proc.AddVarcharPara("@Action", 500, "GetPhaseValue");
            proc.AddPara("@MODEL_ID", Convert.ToString(ModelId));
            proc.AddPara("@DAS", Convert.ToString(DAS));
            DataTable dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                UnitPrice = Convert.ToString(dt.Rows[0]["PhaseValue"]);
            }
            return UnitPrice;
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
            public String IsInventory { get; set; }
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
            public String OSTBVendor_ID { get; set; }
        }
    }
}