using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
////using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;
using System.Web.Services;
using System.Text;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.IO;

namespace ERP.OMS.Management.Store.Master
{
    public partial class management_DailyTask_pOrder : System.Web.UI.Page
    {
        public string pageAccess = "";
        bool isNotEditables = false;
        string EditIds = string.Empty;
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        DataTable dtXML = new DataTable();
        public string PXMLPATH
        {
            get { return (string)Session["CashBankVoucherFile_XMLPATH"]; }
            set { Session["CashBankVoucherFile_XMLPATH"] = value; }
        }
        public int PCounter
        {
            get { return (int)ViewState["Counter"]; }
            set { ViewState["Counter"] = value; }
        }

        private void Page_Error(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('Sorry!! There is some error') </script>", false);
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            #region check Edit path from directory if file exist with is user ID
            PXMLPATH = Server.MapPath("~/OMS/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_E");
            DirectoryInfo d = new DirectoryInfo(Server.MapPath("~/OMS/Management/Documents/"));//Assuming Documents is your Folder
            FileInfo[] Files = d.GetFiles();

            bool isNotEditable = false;
            string session = Convert.ToString(Session["userid"]).Trim();
            foreach (var item in Files)
            {
                try
                {
                    string[] Fnames = item.ToString().Split('_');
                    string Uid = Fnames[1];
                    string Pid = Fnames[2];
                    string Mode = Fnames[3];
                    //check with condition
                    if (Uid.Equals(session) && Mode.Equals("E") && !isNotEditable)
                    {
                        EditIds = Pid;
                        isNotEditables = true;
                        PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + Pid + "_E");
                    }
                }
                catch { }
            }
            #endregion
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
                // btnNewEntry.Focus();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetCurrentDate();
                Bindbranch_code();
                bind_Order_Type();
                BindParentOrderRefNo();
                //BindCmbpOrder_DeliveryAddress("");
                BindCmbpOrder_DeliveryWareHouse();
                BindCmbpOrder_DeliveryBranch();
                if (DeliveryAtDataFromXML().Equals(""))
                {
                    bind_Delivery_At("P");
                }
                else
                {
                    bind_Delivery_At(DeliveryAtDataFromXML());
                }
                if (Request.QueryString["ID"] == null)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> $(document).ready(function() {cPopup_IsEdit.Show();});</script>", false);
                }
                if (isNotEditables)
                {
                    pnlSearch.Visible = false;
                }
            }



            //new code block for showing key from resource page start

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/CreateOrders.resx")))
            {
                ResourceReader resReader = new ResourceReader(Server.MapPath("~/Management/DailyTask/ResourceFiles/CreateOrders.resx"));

                foreach (DictionaryEntry d in resReader)
                {
                    Label currLBL = new Label();
                    currLBL = (Label)Page.FindControl(d.Key.ToString());

                    if (currLBL == null) { currLBL = (Label)Parent.FindControl(d.Key.ToString()); }

                    currLBL.Text = d.Value.ToString();
                }

                resReader.Close();
            }
            //new code block for showing key from resource page end 
        }

        private void BindParentOrderRefNo()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select distinct pOrder_ID,pOrder_RefNumber from Trans_pOrder where isnull(pOrder_ParentOrderNo,0)=0");


            AspxHelper oAspxHelper = new AspxHelper();

            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(ParentOrderNo, dtCmb, "pOrder_RefNumber", "pOrder_ID", 0);

            ParentOrderNo.Items.Add("No Parent", "0");
            ParentOrderNo.SelectedIndex = ParentOrderNo.Items.Count - 1;

        }
        protected void xcbContracts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void Bindbranch_code()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select LTRIM(RTRIM(branch_id)) branch_id, LTRIM(RTRIM(branch_code))+'('+SUBSTRING(branch_description,1,20)+'..)' as branch_code from tbl_master_branch");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbpOrder_Branch, dtCmb, "branch_code", "branch_id", 0);

        }
        protected void BindCmbpOrder_DeliveryAddress(string clntid)
        {
            //  / oGenericMethod = new GenericMethod();
            string WhereClouse = string.Empty;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string ClientId;// = txttype_UserAccount_hidden.Text.Trim();
            if (clntid != "" && clntid != null)
            { ClientId = clntid; }
            else
            { ClientId = txttype_UserAccount_hidden.Text.Trim(); }

            if (!ClientId.Equals(""))
            {
                WhereClouse = " and add_cntId = (select cnt_internalId from tbl_master_contact where cnt_internalId ='" + ClientId + "')";
            }
            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select add_id,add_addressType + ' ('+ SUBSTRING (add_address1,1,5) +'..)' as add_addressType from tbl_master_address where add_address1<>''" + WhereClouse);

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbpOrder_DeliveryAddress, dtCmb, "add_addressType", "add_id", 0);
                CmbpOrder_DeliveryAddress.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
                CmbpOrder_DeliveryAddress.SelectedIndex = 0;
            }
            else
            {
                CmbpOrder_DeliveryAddress.DataSource = null;
                CmbpOrder_DeliveryAddress.DataBind();
            }

        }
        protected void BindCmbpOrder_DeliveryWareHouse()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select bui_id as WareHouse_ID,bui_Name as WareHouse_Name from tbl_master_building");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbpOrder_DeliveryWareHouse, dtCmb, "WareHouse_Name", "WareHouse_ID", 0);

            CmbpOrder_DeliveryWareHouse.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
            CmbpOrder_DeliveryWareHouse.SelectedIndex = 0;

        }
        protected void BindCmbpOrder_DeliveryBranch()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select LTRIM(RTRIM(branch_id)) as BankBranch_ID, LTRIM(RTRIM(branch_code))+'('+SUBSTRING(branch_description,1,20)+'..)' as BankBranch_Name from tbl_master_branch");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbpOrder_DeliveryBranch, dtCmb, "BankBranch_Name", "BankBranch_ID", 0);

            CmbpOrder_DeliveryBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
            CmbpOrder_DeliveryBranch.SelectedIndex = 0;

        }
        protected void bind_Order_Type()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");



            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "J";
            drsession["name"] = "Job work to Vendors";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "I";
            drsession["name"] = "Job work from Customer";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "P";
            drsession["name"] = "Purchase Order To Vendors";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "S";
            drsession["name"] = "Purchase Order Of Customer";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "R";
            drsession["name"] = "Requisition for Stock [Inter-Location]";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "O";
            drsession["name"] = "Samples to Customers as Business Promotion";
            dtCmb.Rows.Add(drsession);

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbddlOrderType, dtCmb, "name", "id", 0);
            //CmbddlOrderType.SelectedIndex = 0;
            //CmbddlOrderType.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
            CmbddlOrderType.SelectedIndex = 0;
        }
        protected void bind_Delivery_At(string type)
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();

            if (type.Trim() == "P" || type.Trim() == "R")
            {
                drsession["id"] = "B";
                drsession["name"] = "Branch";
                dtCmb.Rows.Add(drsession);

                drsession = dtCmb.NewRow();
                drsession["id"] = "O";
                drsession["name"] = "Other Location";
                dtCmb.Rows.Add(drsession);

                drsession = dtCmb.NewRow();
                drsession["id"] = "W";
                drsession["name"] = "WareHouse";
                dtCmb.Rows.Add(drsession);
            }
            else if (type.Trim() == "S" || type.Trim() == "O" || type.Trim() == "I")
            {
                drsession = dtCmb.NewRow();
                drsession["id"] = "A";
                drsession["name"] = "Customer's Address";
                dtCmb.Rows.Add(drsession);

                drsession = dtCmb.NewRow();
                drsession["id"] = "O";
                drsession["name"] = "Other Location";
                dtCmb.Rows.Add(drsession);
            }
            else if (type.Trim() == "J")
            {
                drsession = dtCmb.NewRow();
                drsession["id"] = "A";
                drsession["name"] = "Vendor's Address";
                dtCmb.Rows.Add(drsession);

                drsession = dtCmb.NewRow();
                drsession["id"] = "O";
                drsession["name"] = "Other Location";
                dtCmb.Rows.Add(drsession);

            }

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(ddlDeliveryAt_pOrderType, dtCmb, "name", "id", 0);

            ddlDeliveryAt_pOrderType.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
            ddlDeliveryAt_pOrderType.SelectedIndex = 0;
        }

        #region popup show hide button event
        protected void btnNewEntry_Click(object sender, EventArgs e)
        {
            //CmbpOrder_Branch.Focus();
            txtOrder_RefNumber.Focus();
            //  ClientScript.RegisterClientScriptBlock(GetType(), "skas", "<script> $(document).ready(function() {cPopup_IsEdit.Hide();});</script>", false);
            Popup_IsEdit.ShowOnPageLoad = false;
            pnlSearch.Visible = false;
            pnlAdd.Visible = true;
        }
        protected void btnEditExt_Click(object sender, EventArgs e)
        {
            Popup_IsEdit.ShowOnPageLoad = false;
            ASPxComboBox a = (ASPxComboBox)this.ucprod.FindControl("CmbpOrder_Branch");
            a.Focus();
            // ClientScript.RegisterClientScriptBlock(GetType(), "saks", "<script> $(document).ready(function() { alert('Please search the item and click edit to continue'); $('html, body').animate({scrollTop: $('#ucprod_btnsubmit').offset().top}, 2000)  });</script>", false);
            pnlSearch.Visible = true;
            pnlAdd.Visible = false;
        }

        protected void btnAprvOrdr_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/management/store/Master/AppOrders.aspx");
        }

        #endregion


        protected void BTNSave_clicked(object sender, EventArgs e)
        {
            string[] key = Convert.ToString(KeyField.Text).Split(',');
            string[] value = Convert.ToString(ValueField.Text).Split(',');
            string RexName = Convert.ToString(RexPageName.Text).Trim();

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx")))
            {
                File.Delete(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            }

            ResourceWriter resourceWriter = new ResourceWriter(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            for (int i = 0; i < key.Length; i++)
            {
                resourceWriter.AddResource(key[i].Trim(), value[i].Trim());
            }
            resourceWriter.Generate();
            resourceWriter.Close();

            Response.Redirect("");
        }

        protected void BTNSaveUC_clicked(object sender, EventArgs e)
        {
            string[] key = Convert.ToString(KeyFieldUC.Text).Split(',');
            string[] value = Convert.ToString(ValueFieldUC.Text).Split(',');
            string RexName = Convert.ToString(RexPageNameUC.Text).Trim();

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx")))
            {
                File.Delete(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            }

            ResourceWriter resourceWriter = new ResourceWriter(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            for (int i = 0; i < key.Length; i++)
            {
                resourceWriter.AddResource(key[i].Trim(), value[i].Trim());
            }
            resourceWriter.Generate();
            resourceWriter.Close();

            Response.Redirect("");
        }

        protected void ddlDeliveryAt_pOrderType_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "bind_Delivery_At")
            {
                string countryID = e.Parameter.Split('~')[1].ToString();
                bind_Delivery_At(countryID);
            }
        }

        protected void CmbpOrder_DeliveryAddress_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindCmbpOrder_DeliveryAddress")
            {
                string countryID = e.Parameter.Split('~')[1].ToString();
                BindCmbpOrder_DeliveryAddress(countryID);
            }
        }

        [WebMethod]
        public static string GetAutofillValue(string id)
        {
            StringBuilder str = new StringBuilder();
            str.Append(" WHERE ");
            if (id.Trim() != "" && id.Trim() != "0")
            { str.Append("P.sProducts_ID = " + id + " and"); }

            string queary = string.Empty;

            if (str.ToString().Trim().EndsWith(" and"))
            {
                int aa = str.ToString().LastIndexOf("and");
                queary = str.ToString().Substring(0, aa); ;
            }

            else if (str.ToString().Trim().EndsWith("WHERE"))
            {
                queary = "";
            }
            else if (str.ToString().Trim().EndsWith(" WHERE )"))
            {
                queary = "";
            }
            else
            {
                queary = str.ToString();
            }

            string AutofillValue = null;
            DataTable dt = new DataTable();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            dt = oGenericMethod.GetDataTable(@"SELECT C.Currency_ID
                                          ,C.Currency_Name
                                          ,P.sProducts_QuoteLot
                                          ,U.UOM_ID
                                          ,U.UOM_Name
                                          ,P.sProducts_DeliveryLot
                                    FROM Master_sProducts P
                                    LEFT JOIN master_currency C ON P.sProducts_QuoteCurrency = C.Currency_ID
                                    LEFT JOIN Master_UOM U ON P.sProducts_QuoteLotUnit = U.UOM_ID" + queary);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    var value = item.ItemArray;
                    AutofillValue = Convert.ToString(value[0]) + "," + Convert.ToString(value[1]) + "," + Convert.ToString(value[2]) + "," + Convert.ToString(value[3]) + "," + Convert.ToString(value[4] + "," + Convert.ToString(value[5]));
                }
            }
            return AutofillValue;
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            string EditVal = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                EditVal = Convert.ToString(Request.QueryString["ID"]);
            }

            if (EditVal.Equals("") && !isNotEditables)
            {
                PXMLPATH = "../../Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C";
                string CreatePath = PXMLPATH;
                PXMLPATH = Server.MapPath("~/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
                string MapPath = PXMLPATH;
                XMLCreateUpdate(CreatePath, MapPath);
                // ClientScript.RegisterClientScriptBlock(GetType(), "saks", "<script> $(document).ready(function() { alert('Please search the item and click edit to continue'); fn_PopOpen();  });</script>", false);
            }
            else if (isNotEditables)
            {
                PXMLPATH = "../../Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E";
                string CreatePath = PXMLPATH;
                PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
                string MapPath = PXMLPATH;
                XMLCreateUpdate(CreatePath, MapPath);
            }
            else
            {
                PXMLPATH = "../../Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E";
                string CreatePath = PXMLPATH;
                PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
                string MapPath = PXMLPATH;
                XMLCreateUpdate(CreatePath, MapPath);
            }
            var urls = Request.RawUrl;
            ClientScript.RegisterStartupScript(GetType(), "alert", "fn_PopOpen();", true);
        }

        private void XMLCreateUpdate(string CreatePath, string MapPath)
        {
            PXMLPATH = MapPath;
            string pOrderCompany = "NULL";
            int pOrderBranch = 0;
            string pOrderDate;
            string pOrderFinYear = "NULL";
            string pOrderType = "NULL";
            string contactname = "NULL";
            string pOrderContactID = "NULL";
            string pOrderRefNumber = "NULL";
            string pOrderParentRefNumber = "NULL";
            string pOrderNumber = "NULL";
            string agentname = "NULL";
            string pOrderAgentID = "NULL";
            string pOrderInstructions = "NULL";
            string pOrderPaymentTerm = "NULL";
            string pOrderPaymentDate = "NULL";
            string pOrderDeliveryDate = "NULL";
            string pOrderDeliveryAt = "NULL";
            int? pOrderDeliveryBranch = 0;
            int? pOrderDeliveryWareHouse = 0;
            int? pOrderDeliveryAddress = 0;
            string pOrderDeliveryOther = "NULL";
            string pOrderVerified = "NULL";
            string pOrderVerifyRemarks = "NULL";
            string pOrderVerifyUser = "NULL";
            string pOrderVerifyTime = "NULL";
            string pOrderApprovUser1 = "NULL";
            string pOrderApprovUser1Time = "NULL";
            string pOrderApprovUser2 = "NULL";
            string pOrderApprovUser2Time = "NULL";
            string pOrderApprovUser3 = "NULL";
            string pOrderApprovUser3Time = "NULL";
            string pOrder_PaymentDays = "NULL";

            string[] dateformating;
            string day;
            string month;
            string year;
            if (ParentOrderNo.SelectedItem != null)
            {
                pOrderParentRefNumber = Convert.ToString(ParentOrderNo.SelectedItem.Value);
            }
            if (CmbpOrder_Branch.SelectedItem != null)
            {
                pOrderBranch = Convert.ToInt32(CmbpOrder_Branch.SelectedItem.Value);
            }
            if (txtTaxRates_DateFrom.Text != "" && txtTaxRates_DateFrom.Text.Contains("/"))
            {
                dateformating = txtTaxRates_DateFrom.Text.Split('/');
                day = dateformating[0];
                month = dateformating[1];
                year = dateformating[2];
                pOrderDate = year + "-" + month + "-" + day;
                pOrderFinYear = year;
                pOrderFinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim();
            }
            else if (txtTaxRates_DateFrom.Text != "" && txtTaxRates_DateFrom.Text.Contains("-"))
            {
                dateformating = txtTaxRates_DateFrom.Text.Split('-');
                day = dateformating[0];
                month = dateformating[1];
                year = dateformating[2];
                pOrderDate = year + "-" + month + "-" + day;
                pOrderFinYear = year;
                pOrderFinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim();
            }
            else { pOrderDate = Convert.ToString(System.DateTime.Now); }
            if (CmbddlOrderType.SelectedItem != null)
            {
                pOrderType = Convert.ToString(CmbddlOrderType.SelectedItem.Value);
            }
            contactname = txttype_UserAccount.Text.Trim();
            pOrderContactID = txttype_UserAccount_hidden.Text.Trim();
            pOrderRefNumber = txtOrder_RefNumber.Text.Trim();
            pOrderNumber = "NULL";
            agentname = txt_pOrder_AgentID.Text.Trim();
            pOrderAgentID = txt_pOrder_AgentID_hidden.Text.Trim();
            pOrderInstructions = ASPxMemo1.Text.Trim();
            if (CmbpOrder_PaymentTerm.SelectedItem != null)
            {
                pOrderPaymentTerm = Convert.ToString(CmbpOrder_PaymentTerm.SelectedItem.Value);
            }
            if (txtpOrder_PaymentDate.Text != "" && txtpOrder_PaymentDate.Text.Contains("/"))
            {
                dateformating = txtpOrder_PaymentDate.Text.Split('/');
                day = dateformating[0];
                month = dateformating[1];
                year = dateformating[2];
                pOrderPaymentDate = year + "-" + month + "-" + day;
                //pOrderDeliveryDate = year + "-" + month + "-" + day;
            }
            else if (txtpOrder_PaymentDate.Text != "" && txtpOrder_PaymentDate.Text.Contains("-"))
            {
                dateformating = txtpOrder_PaymentDate.Text.Split('-');
                day = dateformating[0];
                month = dateformating[1];
                year = dateformating[2];
                pOrderPaymentDate = year + "-" + month + "-" + day;
            }

            if (ASPxDateEdit1.Text != "" && ASPxDateEdit1.Text.Contains("/"))
            {
                dateformating = ASPxDateEdit1.Text.Split('/');
                day = dateformating[0];
                month = dateformating[1];
                year = dateformating[2];
                pOrderDeliveryDate = year + "-" + month + "-" + day;
            }
            else if (ASPxDateEdit1.Text != "" && ASPxDateEdit1.Text.Contains("-"))
            {
                dateformating = ASPxDateEdit1.Text.Split('-');
                day = dateformating[0];
                month = dateformating[1];
                year = dateformating[2];
                pOrderDeliveryDate = year + "-" + month + "-" + day;
            }

            if (ddlDeliveryAt_pOrderType.SelectedItem != null)
            {
                pOrderDeliveryAt = Convert.ToString(ddlDeliveryAt_pOrderType.SelectedItem.Value);
            }
            if (CmbpOrder_DeliveryBranch.SelectedItem != null)
            {
                pOrderDeliveryBranch = Convert.ToInt32(CmbpOrder_DeliveryBranch.SelectedItem.Value);
            }
            if (CmbpOrder_DeliveryWareHouse.SelectedItem != null)
            {
                pOrderDeliveryWareHouse = Convert.ToInt32(CmbpOrder_DeliveryWareHouse.SelectedItem.Value);
            }
            if (CmbpOrder_DeliveryAddress.SelectedItem != null)
            {
                pOrderDeliveryAddress = Convert.ToInt32(CmbpOrder_DeliveryAddress.SelectedItem.Value);
            }
            pOrderDeliveryOther = txtpOrder_DeliveryOther.Text.Trim();
            if (txtpOrder_PaymentDays != null)
            {
                pOrder_PaymentDays = txtpOrder_PaymentDays.Text.Trim();
            }

            DataSet dsToAddXML = new DataSet();
            PXMLPATH = MapPath;
            if (File.Exists(PXMLPATH))
            {
                //if (dsToAddXML.Tables.Count > 0)
                //{
                //    dsToAddXML.Tables.Remove(dsToAddXML.Tables[0]);
                //    dsToAddXML.Clear();
                //}

                dsToAddXML.ReadXml(PXMLPATH);
                if (dsToAddXML.Tables[0].TableName == "DtPerchesOrderVoucher")
                {
                    if (dsToAddXML.Tables[0].Rows.Count > 0)
                    {
                        PCounter = Convert.ToInt32(dsToAddXML.Tables[0].Rows[dsToAddXML.Tables[0].Rows.Count - 1]["pOrderRecordID"].ToString()) + 1;
                    }
                    DataRow drXML = dsToAddXML.Tables[0].Rows[0];
                    drXML[0] = PCounter;
                    drXML[1] = pOrderCompany;
                    drXML[2] = pOrderBranch;
                    drXML[3] = pOrderDate;
                    drXML[4] = pOrderFinYear;
                    drXML[5] = pOrderType;
                    drXML[6] = pOrderContactID;
                    drXML[7] = pOrderRefNumber;
                    drXML[8] = pOrderNumber;
                    drXML[9] = pOrderAgentID;
                    drXML[10] = pOrderInstructions;
                    drXML[11] = pOrderPaymentTerm;
                    drXML[12] = pOrderPaymentDate;
                    drXML[13] = pOrderDeliveryDate;
                    drXML[14] = pOrderDeliveryAt;
                    drXML[15] = pOrderDeliveryBranch;
                    drXML[16] = pOrderDeliveryWareHouse;
                    drXML[17] = pOrderDeliveryAddress;
                    drXML[18] = pOrderDeliveryOther;
                    drXML[19] = pOrderVerified;
                    drXML[20] = pOrderVerifyRemarks;
                    drXML[21] = pOrderVerifyUser;
                    drXML[22] = pOrderVerifyTime;
                    drXML[23] = pOrderApprovUser1;
                    drXML[24] = pOrderApprovUser1Time;
                    drXML[25] = pOrderApprovUser2;
                    drXML[26] = pOrderApprovUser2Time;
                    drXML[27] = pOrderApprovUser3;
                    drXML[28] = pOrderApprovUser3Time;
                    drXML[29] = contactname;
                    drXML[30] = agentname;

                    try
                    {
                        drXML[32] = pOrder_PaymentDays;
                    }
                    catch
                    {
                        drXML[31] = pOrder_PaymentDays;
                    }
                    drXML[33] = pOrderParentRefNumber;

                    //dsToAddXML.Tables[0].Rows.Add(drXML);
                    dsToAddXML.Tables[0].AcceptChanges();
                    //dsToAddXML.Tables[0].WriteXml(PXMLPATH);
                    dsToAddXML.WriteXml(PXMLPATH);
                    pOderId_hidden.Value = Convert.ToString(drXML[0]);
                    dsToAddXML.Dispose();
                }
            }
            else
            {
                if (dsToAddXML.Tables.Count > 0)
                {
                    dsToAddXML.Tables.Remove(dsToAddXML.Tables[0]);
                    dsToAddXML.Clear();
                }
                dtXML = dsToAddXML.Tables.Add();
                dtXML.Columns.Add(new DataColumn("pOrderRecordID", typeof(int))); //0
                dtXML.Columns.Add(new DataColumn("pOrderCompany", typeof(string)));//1
                dtXML.Columns.Add(new DataColumn("pOrderBranch", typeof(string)));//2
                dtXML.Columns.Add(new DataColumn("pOrderDate", typeof(string)));//3
                dtXML.Columns.Add(new DataColumn("pOrderFinYear", typeof(string)));//4
                dtXML.Columns.Add(new DataColumn("pOrderType", typeof(string)));//5
                dtXML.Columns.Add(new DataColumn("pOrderContactID", typeof(string)));//6
                dtXML.Columns.Add(new DataColumn("pOrderRefNumber", typeof(string)));//7
                dtXML.Columns.Add(new DataColumn("pOrderNumber", typeof(string)));//8
                dtXML.Columns.Add(new DataColumn("pOrderAgentID", typeof(string)));//9
                dtXML.Columns.Add(new DataColumn("pOrderInstructions", typeof(string)));//10
                dtXML.Columns.Add(new DataColumn("pOrderPaymentTerm", typeof(string)));//11
                dtXML.Columns.Add(new DataColumn("pOrderPaymentDate", typeof(string)));//12
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryDate", typeof(string)));//13
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryAt", typeof(string)));//14
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryBranch", typeof(string)));//15
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryWareHouse", typeof(string)));//16
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryAddress", typeof(string)));//17
                dtXML.Columns.Add(new DataColumn("pOrderDeliveryOther", typeof(string)));//18
                dtXML.Columns.Add(new DataColumn("pOrderVerified", typeof(string)));//19
                dtXML.Columns.Add(new DataColumn("pOrderVerifyRemarks", typeof(string)));//20
                dtXML.Columns.Add(new DataColumn("pOrderVerifyUser", typeof(string)));//21
                dtXML.Columns.Add(new DataColumn("pOrderVerifyTime", typeof(string)));//22
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser1", typeof(string)));//23
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser1Time", typeof(string)));//24 
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser2", typeof(string)));//25 
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser2Time", typeof(string)));//26
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser3", typeof(string)));//27 
                dtXML.Columns.Add(new DataColumn("pOrderApprovUser3Time", typeof(string)));//28
                dtXML.Columns.Add(new DataColumn("contactname", typeof(string)));//29 
                dtXML.Columns.Add(new DataColumn("agentname", typeof(string)));//30
                dtXML.Columns.Add(new DataColumn("pOrder_PaymentDays", typeof(string)));//31
                dtXML.Columns.Add(new DataColumn("pOrderParentRefNumber", typeof(string)));//32


                DataRow drXML = dtXML.NewRow();

                drXML[0] = 1;
                drXML[1] = pOrderCompany;
                drXML[2] = pOrderBranch;
                drXML[3] = pOrderDate;
                drXML[4] = pOrderFinYear;
                drXML[5] = pOrderType;
                drXML[6] = pOrderContactID;
                drXML[7] = pOrderRefNumber;
                drXML[8] = pOrderNumber;
                drXML[9] = pOrderAgentID;
                drXML[10] = pOrderInstructions;
                drXML[11] = pOrderPaymentTerm;
                drXML[12] = pOrderPaymentDate;
                drXML[13] = pOrderDeliveryDate;
                drXML[14] = pOrderDeliveryAt;
                drXML[15] = pOrderDeliveryBranch;
                drXML[16] = pOrderDeliveryWareHouse;
                drXML[17] = pOrderDeliveryAddress;
                drXML[18] = pOrderDeliveryOther;
                drXML[19] = pOrderVerified;
                drXML[20] = pOrderVerifyRemarks;
                drXML[21] = pOrderVerifyUser;
                drXML[22] = pOrderVerifyTime;
                drXML[23] = pOrderApprovUser1;
                drXML[24] = pOrderApprovUser1Time;
                drXML[25] = pOrderApprovUser2;
                drXML[26] = pOrderApprovUser2Time;
                drXML[27] = pOrderApprovUser3;
                drXML[28] = pOrderApprovUser3Time;
                drXML[29] = contactname;
                drXML[30] = agentname;
                drXML[31] = pOrder_PaymentDays;
                drXML[32] = pOrderParentRefNumber;
                dtXML.Rows.Add(drXML);
                dtXML.AcceptChanges();
                dsToAddXML.Tables[0].TableName = "DtPerchesOrderVoucher";
                dsToAddXML.WriteXml(PXMLPATH);
                pOderId_hidden.Value = Convert.ToString(drXML[0]);
                dsToAddXML.Dispose();
            }
        }

        protected void rbShowHide_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbShowHide.SelectedItem.Value.Equals("0"))
            {
                pnlAdd.Visible = true;
                pnlSearch.Visible = false;
            }
            else if (rbShowHide.SelectedItem.Value.Equals("1"))
            {
                pnlAdd.Visible = false;
                pnlSearch.Visible = true;
            }
            else if (rbShowHide.SelectedItem.Value.Equals("2"))
            {
                pnlAdd.Visible = true;
                pnlSearch.Visible = true;
            }
        }
        private string DeliveryAtDataFromXML()
        {
            string DeliveryAtValue = string.Empty;
            DataSet ds = new DataSet();
            string EditVal = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                EditVal = Convert.ToString(Request.QueryString["ID"]);
            }
            if (EditVal.Equals("") && !isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/Management/Documents/" + "POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_C");
            }
            else if (isNotEditables)
            {
                PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditIds + "_E");
            }
            else
            {
                PXMLPATH = Server.MapPath("~/Management/Documents/POS_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "_" + EditVal + "_E");
            }
            if (File.Exists(PXMLPATH))
            {
                ds.ReadXml(PXMLPATH);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DeliveryAtValue = Convert.ToString(dt.Rows[0]["pOrderType"]);
                }
            }
            return DeliveryAtValue;
        }
        private void SetCurrentDate()
        {
            txtTaxRates_DateFrom.Date = DateTime.Today;
            ASPxDateEdit1.Date = DateTime.Today;
            txtpOrder_PaymentDate.Date = DateTime.Today;
        }

        [WebMethod]
        public static WorkStock[] PageLoadMoreInfoGrigBind(string id)
        {
            DataTable dt = new DataTable();
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            List<WorkStock> oWorkStock = new List<WorkStock>();
            dt = oGeneric.GetDataTable(@"SELECT A.JWorkStock_ID
	                                                ,A.JWorkStock_ProductID
	                                                ,P.sProducts_Name AS JWorkStock_ProductName
                                                    ,A.JWorkStock_Brand
                                                    ,A.JWorkStock_Size
	                                                ,S.Size_Name AS JWorkStock_SizeName
                                                    ,A.JWorkStock_Color 
	                                                ,C.Color_Name AS JWorkStock_ColorName
	                                                ,A.JWorkStock_Quantity
	                                                ,UOM.UOM_Name
                                            FROM Trans_JWorkStock A
                                            LEFT JOIN Master_sProducts P ON P.sProducts_ID = A.JWorkStock_ProductID
                                            LEFT JOIN Master_Size S ON s.Size_ID = A.JWorkStock_Size
                                            LEFT JOIN Master_Color C ON C.Color_ID = A.JWorkStock_Color
                                            LEFT JOIN Master_UOM UOM ON UOM.UOM_ID = A.JWorkStock_QuantityUnit
                                            WHERE A.JWorkStock_OrderID = '" + id + "'");

            foreach (DataRow dtRow in dt.Rows)
            {
                WorkStock DataObj = new WorkStock();
                DataObj.JWorkStock_ID = Convert.ToString(dtRow["JWorkStock_ID"]);
                DataObj.JWorkStock_ProductName = Convert.ToString(dtRow["JWorkStock_ProductName"]);
                DataObj.JWorkStock_Brand = Convert.ToString(dtRow["JWorkStock_Brand"]);
                DataObj.JWorkStock_SizeName = Convert.ToString(dtRow["JWorkStock_SizeName"]);
                DataObj.JWorkStock_ColorName = Convert.ToString(dtRow["JWorkStock_ColorName"]);
                DataObj.JWorkStock_Quantity = Convert.ToString(dtRow["JWorkStock_Quantity"]);
                DataObj.UOM_Name = Convert.ToString(dtRow["UOM_Name"]);
                oWorkStock.Add(DataObj);
            }
            return oWorkStock.ToArray();

        }

        [WebMethod]
        public static bool DeleteMoreInfo(string id)
        {
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            int JWorkStock_ID = Convert.ToInt32(id);
            int Count = 0;
            bool flag = false;
            Count = oGeneric.Delete_Table("Trans_JWorkStock", "JWorkStock_ID = " + Convert.ToString(JWorkStock_ID));
            if (Count > 0)
            {
                flag = true;
            }
            return flag;
        }
        [WebMethod]
        public static bool CheckUniqueRefNumber(string RefNumber)
        {
            bool IsPresent = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            DataTable dt = oGeneric.GetDataTable("SELECT COUNT(pOrder_RefNumber) AS RefNoCount FROM Trans_pOrder WHERE pOrder_RefNumber='" + RefNumber + "'");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["RefNoCount"]) > 0)
                {
                    IsPresent = true;
                }
            }
            return IsPresent;
        }

        public class WorkStock
        {
            public string JWorkStock_ID { get; set; }
            public string JWorkStock_ProductName { get; set; }
            public string JWorkStock_Brand { get; set; }
            public string JWorkStock_Size { get; set; }
            public string JWorkStock_SizeName { get; set; }
            public string JWorkStock_Color { get; set; }
            public string JWorkStock_ColorName { get; set; }
            public string JWorkStock_Quantity { get; set; }
            public string UOM_Name { get; set; }
        }
    }
}