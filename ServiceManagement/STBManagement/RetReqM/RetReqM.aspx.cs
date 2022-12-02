using BusinessLogicLayer;
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

namespace ServiceManagement.STBManagement.RetReqM
{
    public partial class RetReqM : System.Web.UI.Page
    {
        string userbranchHierachy = null;
        string UniquePurchaseNumber = string.Empty;
        CommonBL ComBL = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string MultiBranchNumberingScheme = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string DBName = "";
                string EmployeeId = "";
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["UniqueKey"])))
                    DBName = Convert.ToString(Request.QueryString["UniqueKey"]);

                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["AU"])))
                    EmployeeId = Convert.ToString(Request.QueryString["AU"]);

                DataTable usriddt = GetDataTable("select user_id from tbl_master_user where user_contactid='" + EmployeeId + "'", DBName);
                if (usriddt != null && usriddt.Rows.Count > 0)
                {
                    Session["userid"] = usriddt.Rows[0]["user_id"].ToString();
                }

                Session["DBName"] = DBName;

                string oSql = Convert.ToString(GetConnectionString(DBName));
                Session["oSql"] = oSql;

                txtAmount.ClientEnabled = false;
                txtDetails2Amount.ClientEnabled = false;
                txtDetails2Quantity.ClientEnabled = false;
                txtDetails2UnitPrice.ClientEnabled = false;

                txtUnitPrice.ClientEnabled = false;
                txtQuantity.ClientEnabled = false;

                //if (Request.QueryString["Key"].ToUpper() != "ADD")
                //{
                hdnIsAproval.Value = "Yes";
                hdAddEdit.Value = "edit";
                string STBRequisitionID = Request.QueryString["id"];
                hdnSTBRequisitionID.Value = Request.QueryString["id"];
                //DataTable dt = GetDataTable("select * from STB_STBRequisitionHeader where STBRequisition_id=" + STBRequisitionID + " and ApprovalAction=1 and IsApprove=1", DBName);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    //Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "CloseWindow();",true);
                //    divApproved.Attributes.Add("removeclass", "hide");
                //    divApprovalSection.Attributes.Add("addclass", "hide");
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "CloseWindow()", true);
                //}
                //else
                //{
                    divApproved.Attributes.Add("addclass", "hide");
                    divApprovalSection.Attributes.Add("removeclass", "hide");

                    EditModeExecute(STBRequisitionID, DBName);
                //}
                // }
            }
        }

        public String EmployeeBranchMap(string DBName)
        {
            String branches = null;
            DataTable ds = new DataTable();

            string oSql = Convert.ToString(GetConnectionString(DBName));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("PRC_EmployeeBranchMap", oSqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", Session["userid"].ToString());
            cmd.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            oSqlConnection.Close();
            if (ds != null && ds.Rows.Count > 0)
            {
                branches = ds.Rows[0]["BranchId"].ToString();
            }
            return branches;
        }

        private void AddmodeExecuted(string DBName)
        {

            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataSet branchtable = dsFetchAll(DBName);

            //ddlRequisitionType.DataSource = branchtable.Tables[0];
            //ddlRequisitionType.ValueField = "TypeId";
            //ddlRequisitionType.TextField = "RequisitionType";
            //ddlRequisitionType.DataBind();
            //ddlRequisitionType.SelectedIndex = 0;

            //ddlRequisitionFor.DataSource = branchtable.Tables[1];
            //ddlRequisitionFor.ValueField = "ID";
            //ddlRequisitionFor.TextField = "RequisitionFor";
            //ddlRequisitionFor.DataBind();
            //ddlRequisitionFor.SelectedIndex = 0;

            //ddlModel.DataSource = branchtable.Tables[2];
            //ddlModel.ValueField = "ModelID";
            //ddlModel.TextField = "ModelDesc";
            //ddlModel.DataBind();
            //ddlModel.SelectedIndex = 0;

            //ddlDetails2Model.DataSource = branchtable.Tables[2];
            //ddlDetails2Model.ValueField = "ModelID";
            //ddlDetails2Model.TextField = "ModelDesc";
            //ddlDetails2Model.DataBind();
            //ddlDetails2Model.SelectedIndex = 0;

            //ddlOSTBVendor.DataSource = branchtable.Tables[4];
            //ddlOSTBVendor.ValueField = "cnt_internalId";
            //ddlOSTBVendor.TextField = "OSTBVendor";
            //ddlOSTBVendor.DataBind();
            //ddlOSTBVendor.SelectedIndex = 0;
        }

        private void PopulateBranchByHierchy(string userbranchhierchy, string DBName)
        {
            // PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = new DataTable();//posSale.getBranchListByHierchy(userbranchhierchy);

            string oSql = Convert.ToString(GetConnectionString(DBName));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("prc_PosSalesInvoice", oSqlConnection);
            cmd.Parameters.AddWithValue("@Action", "getBranchListbyHierchy");
            cmd.Parameters.AddWithValue("@BranchList", userbranchhierchy);
            cmd.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = cmd;
            adapter.Fill(branchtable);
            oSqlConnection.Close();

            //ddlBranch.DataSource = branchtable;
            //ddlBranch.ValueField = "branch_id";
            //ddlBranch.TextField = "branch_description";
            //ddlBranch.DataBind();
            //ddlBranch.SelectedIndex = 0;
        }

        public DataSet dsFetchAll(string DBName)
        {
            DataSet ds = new DataSet();
            string oSql = Convert.ToString(GetConnectionString(DBName));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("PRC_STBRequisitionDetails", oSqlConnection);
            cmd.Parameters.AddWithValue("@ACTION", "FETCH");
            cmd.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            oSqlConnection.Close();
            return ds;
        }

        #region Details1 Section

        [WebMethod]
        public static String AddData(string Model, string UnitPrice, string Quantity, string Amount, String Remarks, string Model_ID, String Guids)
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

                    object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, Remarks, Model_ID };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 1)
                        {
                            object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, Remarks, Model_ID };// Add new parameter Here
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
                // GrdDevice.DataSource = (DataTable)Session["DeviceDetails"];
            }
        }

        protected void GrdDevice_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Session["DeviceDetails"] != null)
            {
                //GrdDevice.DataBind();
            }
        }

        #endregion

        public class DeviceDetails
        {
            public String Model { get; set; }
            public String UnitPrice { get; set; }
            public String Quantity { get; set; }
            public String Amount { get; set; }
            public String Remarks { get; set; }
            public String Model_ID { get; set; }
            public String Guid { get; set; }
            public String ostbVendor { get; set; }
            public String ostbVendorID { get; set; }
        }

        #region Details2 section
        [WebMethod]
        public static String AddDetails2Data(string Model, string UnitPrice, string Quantity, string Amount, String Remarks, string Model_ID, String Guids, String ostbVendor, String ostbVendorID)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails2"];
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
                    dtable.Columns.Add("ostbVendor", typeof(System.String));
                    dtable.Columns.Add("ostbVendorID", typeof(System.String));

                    object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, Remarks, Model_ID, ostbVendor, ostbVendorID };// Add new parameter Here
                    dtable.Rows.Add(trow);
                    HttpContext.Current.Session["DeviceDetails2"] = dtable;
                }
                else
                {
                    if (string.IsNullOrEmpty(Guids))
                    {
                        if (dt.Rows.Count < 1)
                        {
                            object[] trow = { Guid.NewGuid(), Model, UnitPrice, Quantity, Amount, Remarks, Model_ID, ostbVendor, ostbVendorID };// Add new parameter Here
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
                                    item["ostbVendor"] = ostbVendor;
                                    item["ostbVendorID"] = ostbVendorID;
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["DeviceDetails2"] = dt;
                }

                return "STB details Added Successfully.";
            }
            else
            {
                return "Logout";
            }
        }

        [WebMethod]
        public static DeviceDetails EditDetails2Data(String HiddenID)
        {
            DeviceDetails ret = new DeviceDetails();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails2"];

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
                            ret.ostbVendor = item["ostbVendor"].ToString();
                            ret.ostbVendorID = item["ostbVendorID"].ToString();
                            break;
                        }
                    }
                }
            }
            return ret;// "Holiday Remove Sucessfylly";
        }

        [WebMethod]
        public static String DeleteDetails2Data(string HiddenID)
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["DeviceDetails2"];
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

        protected void gridDeviceDetails2_DataBinding(object sender, EventArgs e)
        {
            if (Session["DeviceDetails2"] != null)
            {
                // gridDeviceDetails2.DataSource = (DataTable)Session["DeviceDetails2"];
            }
        }

        protected void gridDeviceDetails2_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (Session["DeviceDetails2"] != null)
            {
                // gridDeviceDetails2.DataBind();
            }
        }

        #endregion

        private void EditModeExecute(string receiptID, string dbName)
        {
            string output = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    string oSql = Convert.ToString(GetConnectionString(dbName));
                    SqlConnection oSqlConnection = new SqlConnection(oSql);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand("PRC_STBReturnRequisitionApproval", oSqlConnection);
                    cmd.Parameters.AddWithValue("@ACTION", "ShowDetails");
                    cmd.Parameters.AddWithValue("@ReturnReq_id", receiptID.ToString().Trim());
                    cmd.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                    oSqlConnection.Close();

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        lblDocumentNumber.InnerHtml = ds.Tables[0].Rows[0]["DocumentNumber"].ToString();

                        lblDate.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["DocumentDate"]);

                        lblBranch.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["branch_description"].ToString());

                        lblEntityCode.InnerHtml = ds.Tables[0].Rows[0]["EntityCode"].ToString();

                        lblNetworkName.InnerHtml = ds.Tables[0].Rows[0]["NetworkName"].ToString();

                        lblRequisitionType.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["RequisitionType"].ToString());

                        lblRequisitionFor.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["RequisitionFor"].ToString());

                        lblDAS.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["DAS"].ToString());

                        if (Convert.ToString(ds.Tables[0].Rows[0]["IsNoPayment"].ToString()) == "True")
                        {
                            chkNoPayment.Checked = true;
                        }

                        if (Convert.ToString(ds.Tables[0].Rows[0]["IsPayUsingOnAcountBalance"].ToString()) == "True")
                        {
                            chkPayUsingOnAcountBalance.Checked = true;
                        }

                        lblPaymentRelatedRemarks.InnerHtml = Convert.ToString(ds.Tables[0].Rows[0]["PaymentRelatedRemarks"].ToString());

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            lblModel1.InnerHtml = ds.Tables[1].Rows[0]["Model"].ToString();
                            txtUnitPrice.Value = ds.Tables[1].Rows[0]["UnitPrice"].ToString();
                            txtQuantity.Value = ds.Tables[1].Rows[0]["Quantity"].ToString();
                            txtAmount.Value = ds.Tables[1].Rows[0]["Amount"].ToString();
                            lblOSTBVendors.InnerHtml = ds.Tables[1].Rows[0]["OSTBVendor"].ToString();
                            lblRemarks.InnerHtml = ds.Tables[1].Rows[0]["Remarks"].ToString();
                        }

                        //if (Convert.ToString(ds.Tables[0].Rows[0]["RequisitionFor_Id"].ToString()) != "1")
                        //{
                        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "Details2Hide('" + Convert.ToString(ds.Tables[0].Rows[0]["RequisitionFor"].ToString()) + "')", true);
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
        }

        [WebMethod]
        public static string GetUnitPrice(String ModelId, String DAS)
        {
            string UnitPrice = "0.00";
            ProcedureExecute proc = new ProcedureExecute("PRC_STBRequisitionDetails");
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

        [WebMethod]
        public static string save(InsertReceipt apply)
        {
            string output = string.Empty;
            string strPurchaseNumber = Convert.ToString(apply.DocumentNumber);
            DataTable dtview = new DataTable();          
            bool STATUS = true;
            try
            {
                if (STATUS)
                {
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                        string oSql = HttpContext.Current.Session["oSql"].ToString();
                        SqlConnection oSqlConnection = new SqlConnection(oSql);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        SqlCommand cmd = new SqlCommand("PRC_STBReturnRequisitionApproval", oSqlConnection);
                        cmd.Parameters.AddWithValue("@Action", Convert.ToString(apply.Action));
                        cmd.Parameters.AddWithValue("@USER_ID", user_id);
                        cmd.Parameters.AddWithValue("@ReturnReq_id", apply.STBRequisitionID);
                        cmd.Parameters.AddWithValue("@unitPrice", apply.unitPrice);
                        cmd.Parameters.AddWithValue("@UnitQty", apply.UnitQty);
                        cmd.Parameters.AddWithValue("@UnitAmount", apply.UnitAmount);
                        if (apply.IsApproval == "Yes")
                        {
                            cmd.Parameters.AddWithValue("@ApprovalAction", apply.ApprovalAction);
                            cmd.Parameters.AddWithValue("@ApprovalRemarks", apply.ApprovalRemarks);
                        }

                        cmd.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dtview);
                        oSqlConnection.Close();

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


            public String unitPrice { get; set; }
            public String UnitQty { get; set; }
            public String UnitAmount { get; set; }
            public String unitDetails2Price { get; set; }
            public String UnitDetails2Qty { get; set; }
            public String UnitDetails2Amount { get; set; }
        }

        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;
            }
            connectionString.ConnectTimeout = 950;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);
            string str = connectionString.ConnectionString;
            return str;
        }

        private DataTable GetDataTable(string lcSql, string dbname)
        {
            string oSql = Convert.ToString(GetConnectionString(dbname));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }
    }
}