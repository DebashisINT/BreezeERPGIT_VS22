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

namespace ERP.OMS.Management.Master
{
    public partial class AssignIndustry : ERP.OMS.ViewState_class.VSPage
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

                BindAvaibleIndustry("");
                BindAssignedIndustry();
                BindEntityUserName();

                SortSelectedItem();

            }
        }




        public void SetSelectedListItem()
        {
            if (lbAvailable.Items.Count > 0)
            {
                List<string> selectedProd = new List<string>();
                if (Session["SelectedItemIndustryList"] != null)
                {
                    selectedProd = (List<string>)Session["SelectedItemIndustryList"];
                }

                for (int i = 0; i < lbAvailable.Items.Count; i++)
                {
                    if (lbAvailable.Items[i].Selected)
                    {
                        if (!selectedProd.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                        {
                            selectedProd.Add(Convert.ToString(lbAvailable.Items[i].Value));
                        }
                    }
                    else
                    {
                        if (selectedProd.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                        {
                            selectedProd.Remove(Convert.ToString(lbAvailable.Items[i].Value));
                        }
                    }

                }

                Session["SelectedItemIndustryList"] = selectedProd;
            }
        }


        public void GetSelectedListItem()
        {
            if (lbAvailable.Items.Count > 0)
            {
                string variable = string.Empty;
                List<string> selectedProd = new List<string>();

                if (Session["SelectedItemIndustryList"] != null)
                {
                    selectedProd = (List<string>)Session["SelectedItemIndustryList"];

                }
                for (int i = 0; i < lbAvailable.Items.Count; i++)
                {
                    if (selectedProd.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                    {
                        lbAvailable.Items[i].Selected = true;
                        variable = lbAvailable.Items[i].Text.Replace("-", "") + ',' + variable;
                    }
                    else
                    {
                        lbAvailable.Items[i].Selected = false;
                    }
                }
                //  divqualilist.InnerHtml = variable.Substring(0, variable.Length - 1);
            }

        }

        public void SortSelectedItem()
        {
            if (lbAvailable.Items.Count > 0)
            {
                List<ListEditItem> SelectedItems = new List<ListEditItem>(); ;
                List<ListEditItem> RestItems = new List<ListEditItem>();
                for (int i = 0; i < lbAvailable.Items.Count; i++)
                {
                    if (lbAvailable.Items[i].Selected)
                    {
                        SelectedItems.Add(lbAvailable.Items[i]);
                    }
                    else
                    {
                        RestItems.Add(lbAvailable.Items[i]);
                    }
                }

                lbAvailable.Items.Clear();
                foreach (var obj in SelectedItems)
                {
                    lbAvailable.Items.Add(obj);
                }
                foreach (var obj in RestItems)
                {
                    lbAvailable.Items.Add(obj);
                }
            }

        }
        public void txtAvailable_TextChanged(object sender, EventArgs e)
        {


            if (!String.IsNullOrEmpty(txtAvailable.Text.Trim()))
            {
                string name = txtAvailable.Text.Trim();

                BindAvaibleIndustry(name);
            }
            else { BindAvaibleIndustry(""); }

            // BindAssignedIndustry();

            GetSelectedListItem();
            //SetSelectedListItem();
            //SortSelectedItem();
            SortSelectedItem();
            SetSelectedListItem();
        }



        public void BindAvaibleIndustry(string Search)
        {
            IndustryMap_BL IM = new IndustryMap_BL();
            try
            {

                string IndustryIds = string.Empty;
                string variable = string.Empty;
                string msg = string.Empty;
                string EntityId = string.Empty;
                int EntityTypeId = returnRequestTypeId();
                if (EntityTypeId == 999)
                {
                    Response.Redirect("login.aspx");
                }

                if (Request.QueryString["id1"] != null)
                {
                    EntityId = Convert.ToString(Request.QueryString["id1"]);
                }


                //string query = "select Ind_id,ind_industry from tbl_master_Industry where Ind_id not in (select IndustryMap_IndustryId from Master_IndustryMap where IndustryMap_EntityID='" + EntityId + "' and IndustryMap_EntityType=" + EntityTypeId + ")";
                //DT = oDBEngine.GetDataTable(query);



                DT = IM.BindAvailableIndustry(Search);
                lbAvailable.DataSource = DT;
                lbAvailable.ValueField = "Ind_id";
                lbAvailable.TextField = "ind_industry";
                lbAvailable.DataBind();



            }
            catch { }

        }

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
                            lblEntityUserName.Text = " (" + name + ")";
                        }
                    }
                    else if (Convert.ToString(Request.QueryString["EntType"]) == "product")
                    {
                        string query = "Select sProducts_Name as name  from Master_sProducts where sProducts_Code='" + EntityId.Replace("'", "''") + "' ";
                        DT = oDBEngine.GetDataTable(query);
                        if (DT.Rows.Count > 0)
                        {
                            string name = Convert.ToString(DT.Rows[0]["name"]);
                            lblEntityUserName.Text = " (" + name + ")";
                        }
                    }

                }
                else
                {

                    if (Request.QueryString["id1"] != null)
                    {
                        EntityId = Convert.ToString(Request.QueryString["id1"]);
                        string query = "Select (cnt_FirstName+' '+cnt_middleName+' '+cnt_lastName) as name  from tbl_Master_Contact where cnt_internalId='" + EntityId.Replace("'", "''") + "' ";
                        DT = oDBEngine.GetDataTable(query);
                        if (DT.Rows.Count > 0)
                        {
                            string name = Convert.ToString(DT.Rows[0]["name"]).Replace("  ", " ");
                            lblEntityUserName.Text = " (" + name + ")";
                        }
                    }
                }
            }
            catch { }
        }
        public void BindAssignedIndustry()
        {
            string variable = string.Empty;
            try
            {
                string EntityId = string.Empty;
                int EntityTypeId = returnRequestTypeId();

                if (Request.QueryString["id1"] != null)
                {
                    EntityId = Convert.ToString(Request.QueryString["id1"]);
                }


                string query = "select M.IndustryMap_EntityType,M.IndustryMap_EntityID,M.IndustryMap_IndustryId as Ind_id,I.ind_Industry from Master_IndustryMap M inner Join tbl_Master_Industry I on M.IndustryMap_IndustryId=I.ind_id where M.IndustryMap_EntityType=" + EntityTypeId + " and  IndustryMap_EntityID='" + EntityId.Replace("'", "''") + "'";
                DT = oDBEngine.GetDataTable(query);
                List<string> obj = new List<string>();

                foreach (DataRow dr in DT.Rows)
                {

                    obj.Add(Convert.ToString(dr["Ind_id"]));
                }


                for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                {
                    if (obj.Contains(lbAvailable.Items[i].Value))
                    {
                        lbAvailable.Items[i].Selected = true;
                        //variable = lbAvailable.Items[i].Text.Replace("-", "").Replace(" ", "") + ',' + variable;
                        variable = lbAvailable.Items[i].Text.Replace("-", "") + ',' + variable;
                    }
                }
                Session["SelectedItemIndustryList"] = obj;
                //  divqualilist.InnerHtml = variable.Substring(0, variable.Length - 1);
                //divqualilist.InnerHtml = "Agriculture<Cement<Economy,Cement<Economy,SampleIndustryParent<Agriculture<Cement<Economy,SampleIndustryChild<SampleIndustryParent<Agriculture<Cement<Economy,";
            }


            catch { }

        }


        //protected void cmbContactType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbContactType.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId    and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
        //            lbMap.DataSource = DT;
        //            lbMap.ValueField = "Id";
        //            lbMap.TextField = "Name";
        //            //  lbMap.ValueType = GetType(Integer);
        //            lbMap.DataBind();

        //            break;
        //        case 2:
        //            //    DT = oDBEngine.GetDataTable("tbl_master_contact,tbl_master_branch,tbl_master_contactstatus", " tbl_master_contact.cnt_id AS cnt_Id,ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' +    ISNULL(tbl_master_contact.cnt_lastName, '') +'['+tbl_master_contact.cnt_shortname+']' As Name ", "tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId cnt_internalId like 'LD%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") ");

        //            //if (DT.Rows.Count != 0)
        //            //{
        //            //    for (int i = 0; i < DT.Rows.Count; i++)
        //            //    {
        //            //        lstMap.Items.Add(DT.Rows[i][0].ToString());


        //            //    }
        //            //}
        //            //  EmployeeDataSource.SelectCommand = "select top 10 * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc";
        //            break;
        //        case 3:

        //            break;
        //        default:
        //            break;
        //    }
        //}

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
                    Response.Redirect("frmContactMain.aspx?requesttype=agent");
                }
            }
        }

        protected void goBackCrossBtn_Click(object sender, EventArgs e)
        {

            BackMainPage();

        }
        protected void btnsubmit_click(object sender, EventArgs e)
        {
            try
            {
                int EntityTypeId = 0;
                string IndustryIds = string.Empty;
                string variable = string.Empty;
                string msg = string.Empty;
                string EntityId = string.Empty;
                SetSelectedListItem();
                List<string> selectedProd = new List<string>();

                if (Session["SelectedItemIndustryList"] != null)
                {
                    selectedProd = (List<string>)Session["SelectedItemIndustryList"];

                }


                foreach (string value in selectedProd)
                {


                    variable = value + ',' + variable;
                }

                //foreach (List  item in selectedProd)
                //{
                //    variable = item.Value.ToString() + ',' + variable;
                //}



                if (variable.Length > 0)
                {
                    IndustryIds = variable.Substring(0, variable.Length - 1);
                }
                else
                {
                    IndustryIds = "";
                }

                EntityTypeId = returnRequestTypeId();

                if (Request.QueryString["id1"] != null)
                {
                    EntityId = Convert.ToString(Request.QueryString["id1"]);
                    msg = obj.InsertIndustryMapEntity_BL(EntityTypeId, IndustryIds, EntityId);
                    BackMainPage();
                    //ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Record saved Successfully');window.location ='frmContactMain.aspx?requesttype=customer';", true);

                }



            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('" + ex + "');", true);
            }
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
                //switch (requesttype)
                //{
                //    case "Customer/Client":
                //        EntityTypeID = 3;
                //        break;
                //    case "OtherEntity":
                //        EntityTypeID = 0;
                //        break;
                //    case "Sub Broker":
                //        EntityTypeID = 0;
                //        break;
                //    case "Franchisee":
                //        EntityTypeID = 0;
                //        break;
                //    case "Relationship Partner":
                //        EntityTypeID = 0;
                //        break;
                //    case "Broker":
                //        EntityTypeID = 0;
                //        break;
                //    case "Relationship Manager":
                //        EntityTypeID = 0;
                //        break;
                //    case "Data Vendor":
                //        EntityTypeID = 0;
                //        break;
                //    case "Vendor":
                //        EntityTypeID = 0;
                //        break;
                //    case "Partner":
                //        EntityTypeID = 0;
                //        break;
                //    case "Consultant":
                //        EntityTypeID = 0;
                //        break;
                //    case "shareholder":
                //        EntityTypeID = 0;
                //        break;
                //    case "Creditors":
                //        EntityTypeID = 0;
                //        break;
                //    case "Debtor":
                //        EntityTypeID = 0;
                //        break;
                //    case "Lead managers":
                //        EntityTypeID = 0;
                //        break;
                //    case "Book Runners":
                //        EntityTypeID = 0;
                //        break;
                //    case "Companies-Listed":
                //        EntityTypeID = 0;
                //        break;
                //    case "Business Partner":
                //        EntityTypeID = 0;
                //        break;
                //    //For Leads
                //    case "Lead":
                //        EntityTypeID = 2;
                //        break;
                //}
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