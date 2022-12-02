using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using ERP.Models;

namespace ERP.OMS.Management.Master
{
    public partial class MapSalesmanToCustomer : ERP.OMS.ViewState_class.VSPage
    {
        DataTable DTChoosen = new DataTable();
        SqlConnection oSqlConnection = new SqlConnection();
        DataTable DT = new DataTable();
       // SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        IndustryMap_BL obj = new IndustryMap_BL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove("SelectedItemIndustryList");

                //txtAvailable.Attributes.Add("onkeypress", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('btnCancel').click();return false;}} else {return true}; ");
                if (Request.QueryString["EntType"] != null)
                {
                    if (Convert.ToString(Request.QueryString["EntType"]) == "productclass")
                    {
                        Session["requesttype"] = "productclass";
                        lblEntityType.Text = "Product Class";
                        hidbackPagerequesttype.Value = Convert.ToString(Session["requesttype"]);
                    }
                    else if (Convert.ToString(Request.QueryString["EntType"]) == "product")
                    {
                        Session["requesttype"] = "product";
                        lblEntityType.Text = "Product";
                        hidbackPagerequesttype.Value = Convert.ToString(Session["requesttype"]);
                    }
                    else if (Convert.ToString(Request.QueryString["EntType"]) == "Employee")
                    {
                        Session["requesttype"] = "Employee";
                        lblEntityType.Text = "Employee";
                        hidbackPagerequesttype.Value = Convert.ToString(Session["requesttype"]);
                    }
                }
                else
                {
                    if (Session["requesttype"] != null)
                    {
                        hidbackPagerequesttype.Value = Convert.ToString(Session["requesttype"]);
                        lblEntityType.Text = Convert.ToString(Session["requesttype"]);
                    }
                }
                //BindAssignedIndustry();
                BindEntityUserName();


            }
        }

        #region Project Code Bind
        //protected void EntityServerModeDataSalesOrder_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "cnt_id";

        //    //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

        //    string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
        //    BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();




        //    var q = from d in dc.V_CustomerListonSalesmanMaps
        //           // where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) //&& d.CustId == Convert.ToString(hdnCustomerId.Value)
        //            orderby d.cnt_id descending
        //            select d;

        //    e.QueryableSource = q;

        //}
        protected void btncancel_Click(object sender, EventArgs e)
        {
        }
             
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable tbl_IDList = new DataTable();
            string cntid = hdnid.Value.ToString();
            int salesmanid = 0;
            salesmanid = Convert.ToInt32(cntid);
            tbl_IDList.Columns.Add("SrlNo", typeof(int));
            tbl_IDList.Columns.Add("customerid", typeof(int));
            string listOfProduct = "";
            List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("cnt_id");
            int InitVal = 1;
            foreach (object Pobj in ComponentList)
            {

                //listOfProduct += "," + Pobj;
                tbl_IDList.Rows.Add(InitVal,Convert.ToInt32(Pobj));
                InitVal = InitVal + 1;
            }
            //listOfProduct = listOfProduct.TrimStart(',');
            Modifycustomermap(tbl_IDList, salesmanid);
        }
        public int Modifycustomermap(DataTable dtcustomer, int salesmanid)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                string Action = Convert.ToString(Session["ActionType"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_CustomerSalesManMap", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Add");
                cmd.Parameters.AddWithValue("@SalesManId", salesmanid);
                cmd.Parameters.AddWithValue("@CustomeridList", dtcustomer);
                cmd.Parameters.AddWithValue("@createdby", Convert.ToString(Session["userid"]));
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                  cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                hdndeselectall.Value = "0";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman With Customer Successfully Mapped!')", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallConfirmBox", "CallConfirmBox();", true);
                //Response.Redirect("SalesQuotationList.aspx");
                return ReturnValue;
            }
            catch(Exception ex) {
                hdndeselectall.Value = "0";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex .Message+ "')", true);
                return 0;
            }
        }
        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string taxId = e.Parameter;
            #region ###### Product ####

            if (e.Parameter.Split('~')[0] == "selectAll")
            {
                if (e.Parameter.Split('~')[1] == "true")
                {
                    for (int i = 0; i < GridLookup.GridView.VisibleRowCount; i++)
                    {
                        GridLookup.GridView.Selection.SelectRow(i);
                    }
                }
                else
                {
                    for (int i = 0; i < GridLookup.GridView.VisibleRowCount; i++)
                    {
                        GridLookup.GridView.Selection.UnselectRow(i);
                    }
                }
            }
            else
            {
                //DataTable newRecordDt = new DataTable();
                //newRecordDt = oTaxSchemeBl.GetProductList(Convert.ToInt32(taxId));
                //GridLookup.GridView.Selection.CancelSelection();
                //GridLookup.DataSource = newRecordDt;
                //Session["ProductTaxData"] = newRecordDt;
                //GridLookup.DataBind();

            }

            #endregion

        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //Grid Bind
             SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlDataAdapter da = new SqlDataAdapter("select cnt_id,cnt_firstName,cnt_internalId from tbl_master_contact where cnt_contactType='CL'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridLookup.DataSource = dt;
            //Grid Bind
            //Selected Rows Check
            string cntid = hdnid.Value.ToString();
            int salesmanid =0;
            salesmanid=  Convert.ToInt32(cntid);
            DataTable exsistingtable = oDBEngine.GetDataTable("select * from tbl_Salesman_Entitymap where SalesmanId='" + salesmanid + "'");
            if (exsistingtable.Rows.Count > 0 && hdndeselectall.Value!="1")
            { 
            foreach (DataRow dr in exsistingtable.Rows)
            {
                GridLookup.GridView.Selection.SelectRowByKey(dr["CustomerId"]);
            }
            }
            //Selected Rows Check
        }
        #endregion Project Code Bind
        private void BindEntityUserName()
        {
            try
            {
                string EntityId = string.Empty;

                if (Request.QueryString["EntType"] != null)
                {
                    EntityId = Convert.ToString(Request.QueryString["id1"]);

                    if (Convert.ToString(Request.QueryString["EntType"]) == "productclass")
                    {
                        string query = "Select ProductClass_Name as name  from Master_ProductClass where ProductClass_Code='" + EntityId.Replace("'", "''") + "' ";
                        DT = oDBEngine.GetDataTable(query);
                        if (DT.Rows.Count > 0)
                        {
                            string name = Convert.ToString(DT.Rows[0]["name"]);
                            lblEntityUserName.Text = " (" + name.ToString().Trim() + ")";
                        }
                    }
                    else if (Convert.ToString(Request.QueryString["EntType"]) == "product")
                    {
                        string query = "Select sProducts_Name as name  from Master_sProducts where sProducts_Code='" + EntityId.Replace("'", "''") + "' ";
                        DT = oDBEngine.GetDataTable(query);
                        if (DT.Rows.Count > 0)
                        {
                            string name = Convert.ToString(DT.Rows[0]["name"]);
                            lblEntityUserName.Text = " (" + name.ToString().Trim() + ")";
                        }
                    }

                }
                else
                {

                    if (Request.QueryString["id1"] != null)
                    {
                        EntityId = Convert.ToString(Request.QueryString["id1"]);
                        string query = "Select (cnt_FirstName+' '+cnt_middleName+' '+cnt_lastName) as name,cnt_id  from tbl_Master_Contact where cnt_internalId='" + EntityId.Replace("'", "''") + "' ";
                        DT = oDBEngine.GetDataTable(query);
                        if (DT.Rows.Count > 0)
                        {
                            string name = Convert.ToString(DT.Rows[0]["name"]).Replace("  ", " ");
                            string cnt_id = Convert.ToString(DT.Rows[0]["cnt_id"]).Replace("  ", " ");
                            int id = 0;
                            id = Convert.ToInt32(cnt_id);
                            lblEntityUserName.Text = " (" + name.ToString().Trim() + ")";
                            hdnid.Value = id.ToString(); ;
                        }
                    }
                }
            }
            catch { }
        }
    
        protected void txtAvailable_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
        public void BackMainPage()
        {
            string ContType = returnEntityType(Convert.ToString(hidbackPagerequesttype.Value));

            string EntType = Convert.ToString(Request.QueryString["EntType"]);
            switch (ContType)
            {
                case "Customer/Client":
                    ContType = "customer";
                    break;
                case "OtherEntity":
                    ContType = "OtherEntity";
                    break;
                case "Sub Broker":
                    ContType = "subbroker";
                    break;
                case "Franchisees":
                    ContType = "franchisee";
                    break;
                case "Relationship Partners":
                    ContType = "referalagent";
                    break;
                case "Broker":
                    ContType = "broker";
                    break;
                case "Relationship Manager":
                    ContType = "agent";
                    break;
                case "Data Vendor":
                    ContType = "datavendor";
                    break;
                case "Vendor":
                    ContType = "vendor";
                    break;
                case "Partner":
                    ContType = "partner";
                    break;
                case "Consultant":
                    ContType = "Consultant";
                    break;
                case "Share Holder":
                    ContType = "shareholder";
                    break;
                case "Creditor":
                    ContType = "Creditors";
                    break;
                case "Debtor":
                    ContType = "Debtors";
                    break;
                case "Lead managers":
                    ContType = "leadmanagers";
                    break;
                case "Book Runners":
                    ContType = "bookrunners";
                    break;
                case "Companies-Listed":
                    ContType = "listedcompanies";
                    break;
                case "Business Partner":
                    ContType = "businesspartner";
                    break;
                case "Lead":
                    ContType = "Lead";
                    break;
                case "product":
                    ContType = "Product";
                    break;
                case "productclass":
                    ContType = "Product Class/Group";
                    break;
            }

            if (ContType == "Product Class/Group")
            {
                Response.Redirect("../store/Master/sProductClass.aspx");
            }
            else if (ContType == "Product")
            {
                Response.Redirect("../store/Master/sProducts.aspx");
            }
            else if (ContType == "customer")
            {
                Response.Redirect("CustomerMasterList.aspx");
            }
            else
            {
                if (EntType == "Employee")
                {
                    Response.Redirect("Employee.aspx");
                }
                else
                {
                   // Response.Redirect("frmContactMain.aspx?requesttype=" + ContType);
                    Response.Redirect("frmContactMain.aspx?requesttype=agent");
                }
            }
        }

        protected void goBackCrossBtn_Click(object sender, EventArgs e)
        {

            BackMainPage();

        }
    

        public int returnRequestTypeId()
        {
            int EntityTypeID = 0;
            if (Session["requesttype"] != null)
            {
                string requesttype = Convert.ToString(Session["requesttype"]);

                string EntityType = returnEntityType(requesttype);
                string query = "select Id from tbl_entity where EntityName='" + EntityType + "'";
                DT = oDBEngine.GetDataTable(query);
                if (DT.Rows.Count > 0)
                {
                    EntityTypeID = Convert.ToInt32(DT.Rows[0]["Id"]);
                }
                #region
               
                #endregion
            }
            else { EntityTypeID = 999; }
            return EntityTypeID;
        }


        public string returnEntityType(string requesttype)
        {
            string ContType = "";
            switch (requesttype)
            {
                case "Customer/Client":
                    ContType = "customer";
                    this.Title = "Customer/Client";
                    break;
                case "OtherEntity":
                    ContType = "OtherEntity";
                    this.Title = "OtherEntity";
                    break;

                case "Sub Broker":
                    ContType = "subbroker";
                    this.Title = "Sub Broker";
                    break;
                case "Franchisee":
                    ContType = "Franchisees";
                    this.Title = "Franchisee";
                    break;
                case "Relationship Partners":
                    ContType = "referalagent";
                    this.Title = "Relationship Partners";
                    break;
                case "Broker":
                    ContType = "broker";
                    this.Title = "Broker";
                    break;
                case "Relationship Manager":
                    ContType = "agent";
                    this.Title = "Relationship Manager";
                    break;
                case "Data Vendor":
                    ContType = "datavendor";
                    this.Title = "Data Vendor";
                    break;
                case "Vendor":
                    ContType = "vendor";
                    this.Title = "Vendor";
                    break;
                case "Partner":
                    ContType = "partner";
                    this.Title = "Partner";
                    break;
                case "Consultant":
                    ContType = "Consultant";
                    this.Title = "Consultant";
                    break;
                case "Share Holder":
                    ContType = "shareholder";
                    this.Title = "Share Holder";
                    break;
                case "Creditor":
                    ContType = "Creditors";
                    this.Title = "Creditor";
                    break;
                case "Debtor":
                    ContType = "Debtors";
                    this.Title = "Debtor";
                    break;
                case "Lead managers":
                    ContType = "leadmanagers";
                    this.Title = "Lead managers";
                    break;
                case "Book Runners":
                    ContType = "bookrunners";
                    this.Title = "Book Runners";
                    break;
                case "Companies-Listed":
                    ContType = "listedcompanies";
                    this.Title = "Companies-Listed";
                    break;
                //case "Relationship Manager":
                //    ContType = "recruitmentagent";
                //    break;
                case "Business Partner":
                    ContType = "businesspartner";
                    this.Title = "Business Partner";
                    break;
                //For Leads
                case "Lead":
                    ContType = "Lead";
                    this.Title = "Lead";
                    break;
                case "product":
                    ContType = "Product";
                    this.Title = "product";
                    break;
                case "productclass":
                    ContType = "Product Class/Group";
                    this.Title = "productclass";
                    break;
            }
            return ContType;
        }


    }
}