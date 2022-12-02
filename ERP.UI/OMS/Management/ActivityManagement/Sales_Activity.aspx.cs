using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Text;
using System.Reflection;
using DevExpress.Web;
using System.Collections.Specialized;
using System.IO;


namespace ERP.OMS.Management.ActivityManagement
{
    public partial class Sales_Activity : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        string Sclient = "";
        int MailStatus;
        string AllUserCntId;
        string product = "";
        string productfinal = "";
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        BusinessLogicLayer.Others obl = new BusinessLogicLayer.Others();
        SlaesActivitiesBL Slobl = new SlaesActivitiesBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Response.Write(); 
                // gridStatusDataSource.
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {  // SetDateFormat();
                hdnOldProductClass.Value = "0";
                //  BindLead();
                string prevur = string.Empty;
                string sRet = string.Empty;
                if (Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]) != null)
                {
                    prevur = Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]);


                    sRet = Path.GetFileName(prevur);

                    if (sRet == "Sales_List.aspx")
                    {
                        //   Session["pCross"] = "N";
                        Session["pCrossUrl"] = sRet;
                    }
                    else
                    {
                        //  Session["pCross"] = "Y";
                        Session["pCrossUrl"] = "../Master/" + sRet;
                    }
                }
                hdntab.Value = "";
                bindAllTasks();


                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["idslsact"])))
                {
                    BindSalesActivity();
                    hdnidslsact.Value = "1";
                }

                else if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
                {
                    hdnEditIndustry.Value = "";
                    string clid = Convert.ToString(Request.QueryString["id"]);
                    hdnCustomerLead.Value = clid;
                    hdnlead.Value = clid;


                    BindLeadIndustry(clid);
                    BindCustomerLead(clid);
                    // BindLeadProduct(clid);
                    lbClientAvailable.Enabled = false;
                    //  lstIndustry.Enabled = false;
                    RadioButtonActivityList.Enabled = false;
                    txtClient.Enabled = false;
                }
                else
                {

                    txtStartDate.ReadOnly = false;
                    txtEndDate.ReadOnly = false;
                    drpPriority.Enabled = true;
                    taskList.Enabled = true;
                    RadioButtonActivityList.Enabled = true;
                    txtInstNote.Enabled = true;
                    chkMail.Enabled = true;
                    btnSubmit.Visible = true;
                    hdnEditAssignTo.Value = "";

                    hdnEditSupervisor.Value = "";
                    hdnEditActivityType.Value = "";
                    hdnEditIndustry.Value = "";
                    hdnEditClientType.Value = "";
                    hdnEditCustomerLead.Value = "";
                    hdnEditProduct.Value = "";
                    txtAvailableProduct.Enabled = true;
                    lbAvailable.Enabled = true;
                
                }
            }
          



            //kaushik 11-10-2016 time section open in calender
            #region time
            txtStartDate.TimeSectionProperties.Visible = true;
            txtStartDate.UseMaskBehavior = true;
            txtStartDate.EditFormatString = "dd-MM-yyyy hh:mm tt";

            txtEndDate.TimeSectionProperties.Visible = true;
            txtEndDate.UseMaskBehavior = true;
            txtEndDate.EditFormatString = "dd-MM-yyyy hh:mm tt";
            #endregion
        }
        //kaushik 18_1_2017 populate industry with repect to client
        public void BindLeadIndustry(string LeadCustomercnt)
        {
            SlaesActivitiesBL obl = new SlaesActivitiesBL();
            DataTable DT = new DataTable();
            DT = Slobl.GetIndustry(LeadCustomercnt);
            if (DT != null && DT.Rows.Count > 0)
            {
                hdnEditIndustry.Value = Convert.ToString(DT.Rows[0]["ind_id"]);
                var indid = hdnEditIndustry.Value;

                BindAllProductClassGroupByIndustryID(indid);
               // BindAllProductByIndustryID(indid);


            }
            //else
            //{
            //    BindAllProduct();
            //    hdnEditIndustry.Value = "0";
            //  //  hdnEditIndustry.Value = "0";
            //}

        }


        public void BindAllProductClassGroupByIndustryID(string IndustryId)
        {
            StockDetails objBL = new StockDetails();

            DataTable DT = objBL.GetPreferredProduct(IndustryId);
            lbAvailable.DataSource = DT;
            lbAvailable.ValueField = "sProducts_ID";
            lbAvailable.TextField = "sProducts_Name";
            lbAvailable.DataBind();
            //  Cache["ProductList"] = DT;
        }
        public void BindAllProductByIndustryID(string IndustryId)
        {
            StockDetails objBL = new StockDetails();

            DataTable DT = objBL.GetPreferredProduct(IndustryId);
            lbAvailable.DataSource = DT;
            lbAvailable.ValueField = "sProducts_ID";
            lbAvailable.TextField = "sProducts_Name";
            lbAvailable.DataBind();
            //   Cache["ProductList"] = DT;
        }
        //kaushik 11_1_2017  pre populate customer lead name with radio button populate
        public void BindCustomerLead(string LeadCustomercnt)
        {
            string rad = string.Empty;
            string lcid = LeadCustomercnt.Substring(0, 2);
            if (lcid == "LD")
            { rad = "2"; }
            else { rad = "1"; }

            hdnEditCustomerLead.Value = LeadCustomercnt;
            RadioButtonActivityList.SelectedValue = rad;
            SlaesActivitiesBL obl = new SlaesActivitiesBL();
            DataTable DIndT = new DataTable();
            DIndT = Slobl.GetIndustry(LeadCustomercnt);
            if (DIndT != null && DIndT.Rows.Count > 0)
            {
                var indid = Convert.ToString(DIndT.Rows[0]["ind_id"]);
                hdnIndustry.Value = indid;
                BindClientByIndustryId(lbClientAvailable, indid, rad);


            }



            DataTable DT = new DataTable();
            DT = Slobl.GetLeadCustomerName(LeadCustomercnt);

            string client = "";
            if (DT.Rows.Count > 0)
            { client = Convert.ToString(DT.Rows[0]["Name"]); }

            SelectedClient(Convert.ToString(LeadCustomercnt));


            divclientlist.InnerHtml = client;
        }


        //kaushik 02-01-2016 activity details view
        public void BindSalesActivity()
        {
            var client = "";
            var product = "";

            DataSet ds = new DataSet();
            DataTable dtActivity = new DataTable();
            DataTable dtClient = new DataTable();
            DataTable dtProduct = new DataTable();
            SlaesActivitiesBL obl = new SlaesActivitiesBL();

            string ActivityId = Convert.ToString(Request.QueryString["idslsact"]);
            ds = obl.GetSalesActvityViewDetails(ActivityId);
            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    dtActivity = ds.Tables[0];
                    txtActivityName.Text = Convert.ToString(dtActivity.Rows[0]["act_activityName"]);
                    txtStartDate.Date = Convert.ToDateTime(Convert.ToString(dtActivity.Rows[0]["StartTime"]));
                    txtEndDate.Date = Convert.ToDateTime(Convert.ToString(dtActivity.Rows[0]["EndTime"]));
                    drpPriority.SelectedValue = Convert.ToString(dtActivity.Rows[0]["act_priority"]);

                    lblAssignTo.Text = Convert.ToString(dtActivity.Rows[0]["AssignedName"]);
                    lblSupervisor.Text = Convert.ToString(dtActivity.Rows[0]["SupervisorName"]);
                  
                    taskList.SelectedValue = Convert.ToString(dtActivity.Rows[0]["act_assign_task"]);
                    txtInstNote.Text = Convert.ToString(dtActivity.Rows[0]["act_instruction"]);
                    hdnEditAssignTo.Value = Convert.ToString(dtActivity.Rows[0]["act_assignedTo"]);
                    hdnEditSupervisor.Value = Convert.ToString(dtActivity.Rows[0]["act_supervisor"]);

                    hdnEditActivityType.Value = Convert.ToString(dtActivity.Rows[0]["act_activityTypes"]);
                    string type = (Convert.ToString(dtActivity.Rows[0]["act_type"]) == null) ? "1" : Convert.ToString(dtActivity.Rows[0]["act_type"]);
                    hdnEditIndustry.Value = Convert.ToString(dtActivity.Rows[0]["act_industryid"]);
                    hdnEditProductClassGroup.Value = Convert.ToString(dtActivity.Rows[0]["act_productClassGroup"]);
                    hdnEditClientType.Value = Convert.ToString(dtActivity.Rows[0]["act_clientType"]);
                    BindProductByIndustryId(lbAvailable, hdnEditIndustry.Value);

                    BindClientByIndustryId(lbClientAvailable, hdnEditIndustry.Value, hdnEditClientType.Value);
                  

                    if (Convert.ToString(dtActivity.Rows[0]["IsEmail"]) == "False")
                    { chkMail.Checked = false; }
                    else { chkMail.Checked = true; }

                    hdnEditClientType.Value = (Convert.ToString(dtActivity.Rows[0]["act_clientType"]) == null ? "2" : Convert.ToString(dtActivity.Rows[0]["act_clientType"]));
                    RadioButtonActivityList.SelectedValue = hdnEditClientType.Value;
                    RdnType.SelectedValue = type;
                    hdnType.Value = type;
                }
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {

                    dtClient = ds.Tables[1];

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        if (i == 0)
                        { client += ds.Tables[1].Rows[i]["Name"]; }
                        else { client += "," + ds.Tables[1].Rows[i]["Name"]; }
                        SelectedClient(Convert.ToString(ds.Tables[1].Rows[i]["sls_contactlead_id"]));

                    }
                    divclientlist.InnerHtml = client;
                    txtClient.Enabled = false;
                    lbClientAvailable.Enabled = false;
                    SortClientSelectedItem();
                }

                if (hdnType.Value == "1")
                {
                    if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {

                        dtProduct = ds.Tables[2];
                        for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                        {
                            if (j == 0)
                            { product += ds.Tables[2].Rows[j]["sProducts_Name"]; }
                            else { product += "," + ds.Tables[2].Rows[j]["sProducts_Name"]; }

                            SelectedProduct(Convert.ToString(ds.Tables[2].Rows[j]["sls_product_id"]));
                        }
                        divprodlist.InnerHtml = product;
                        txtAvailableProduct.Enabled = false;
                        lbAvailable.Enabled = false;
                        //  hdnEditProduct.Value = product;
                        SortProductSelectedItem();
                    }
                }
                else
                {
                    if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {

                        dtProduct = ds.Tables[3];
                        for (int j = 0; j < ds.Tables[3].Rows.Count; j++)
                        {
                            if (j == 0)
                            { product += ds.Tables[3].Rows[j]["sProducts_Name"]; }
                            else { product += "," + ds.Tables[3].Rows[j]["sProducts_Name"]; }

                            SelectedProduct(Convert.ToString(ds.Tables[3].Rows[j]["sls_product_id"]));
                        }
                        divprodlist.InnerHtml = product;
                        txtAvailableProduct.Enabled = false;
                        lbAvailable.Enabled = false;
                        SortProductSelectedItem();
                        //  hdnEditProduct.Value = product;
                    }
                }
            }
            txtActivityName.Enabled = false;
            txtStartDate.ReadOnly = true;
            txtEndDate.ReadOnly = true;
            drpPriority.Enabled = false;
            taskList.Enabled = false;
            RadioButtonActivityList.Enabled = false;
            txtInstNote.Enabled = false;
            chkMail.Enabled = false;
            btnSubmit.Visible = false;
            btnSubmitExit.Visible = false;
            lstSupervisor.Visible = false;
            lstAssignTo.Visible = false;
            RdnType.Enabled = false;
            ListBoxProductClass.Enabled = false;
        }


        public void SelectedProduct(string pitems)
        {

            var product = "";
            int list_counter = lbAvailable.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (Convert.ToString(lbAvailable.Items[i].Value) == pitems)
                {
                    lbAvailable.Items[i].Selected = true;




                }
                // else { lbAvailable.Items[i].Selected = false; }
            }




        }





        public void SelectedExistProductClassByVal(string pitems, bool IsSelected, object sender)
        {


            ASPxListBox finalList = sender as ASPxListBox;
            int list_counter = finalList.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (Convert.ToString(finalList.Items[i].Value) == pitems)
                {
                    finalList.Items[i].Selected = IsSelected;




                }
                
            }
           
        }
        public void SelectedProductClassByVal(string pitems, bool IsSelected, object sender)
        {


            ASPxListBox finalList = sender as ASPxListBox;
            int list_counter = finalList.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (Convert.ToString(finalList.Items[i].Value) == pitems)
                {
                    finalList.Items[i].Selected = IsSelected;
                    if (IsSelected)
                    {

                        if (!product.Contains(finalList.Items[i].Value.ToString()) && !productfinal.Contains(finalList.Items[i].Value.ToString()))
                        {
                            product += finalList.Items[i].Value + ",";
                        }
                    }



                }
                
            }
            hdnProduct.Value = product;
        }
        public void UnSelectedProduct(string pitems)
        {

            var product = "";
            int list_counter = lbAvailable.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (Convert.ToString(lbAvailable.Items[i].Value) != pitems)
                {
                    lbAvailable.Items[i].Selected = false;




                }
              
            }




        }
        public void SelectedClient(string pitems)
        {

            var client = "";
            int list_counter = lbClientAvailable.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (Convert.ToString(lbClientAvailable.Items[i].Value) == pitems)
                {
                    lbClientAvailable.Items[i].Selected = true;


                    client += Convert.ToString(lbClientAvailable.Items[i].Text) + ",";

                }
            }




        }


        public void SelectedC(string pitems)
        {


            int list_counter = lbClientAvailable.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                if (Convert.ToString(lbClientAvailable.Items[i].Value) == pitems)
                {
                    lbClientAvailable.Items[i].Selected = true;


                    Sclient += Convert.ToString(lbClientAvailable.Items[i].Text) + ",";

                }
            }




        }
        public void RemoveSelectedProduct()
        {

            var product = "";
            int list_counter = lbAvailable.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {

                lbAvailable.Items[i].Selected = false;


            }




        }


        public void RemoveSelectedCustomer()
        {


            int list_counter = lbClientAvailable.Items.Count;
            for (int i = 0; i < list_counter; i++)
            {
                lbClientAvailable.Items[i].Selected = false;
            }
        }
        public void BindProductByIndustryId(ASPxListBox lstproduct, string IndustryId)
        {
            StockDetails objBL = new StockDetails();
            DataTable DT = new DataTable();

            string selectedValues = string.Empty;

            if (IndustryId == "0")
            {
                DT = objBL.GetAllProduct();

            }
            else
            {

                DT = objBL.GetPreferredProduct(IndustryId);
            }
            //  Cache["ProductList"] = DT;
            lstproduct.DataSource = DT;
            lstproduct.ValueField = "sProducts_ID";
            lstproduct.TextField = "sProducts_Name";
            lstproduct.DataBind();
        }

        public void BindProductClassGroupByIndustryId(ASPxListBox lstproductClassGroup, string IndustryId)
        {
            StockDetails objBL = new StockDetails();
            DataTable DT = new DataTable();

            string selectedValues = string.Empty;
            DT = objBL.GetPreferredProductClassGroup(IndustryId);
            Cache["ProductClassGroupList"] = DT;
            lstproductClassGroup.DataSource = DT;
            lstproductClassGroup.ValueField = "ProductClass_ID";
            lstproductClassGroup.TextField = "ProductClass_Name";
            lstproductClassGroup.DataBind();
        }
        public void BindClientByIndustryId(ASPxListBox lstClient, string IndustryId, string EntityTypeID)
        {

            DataTable DT = new DataTable();
            IndustryMap_BL objbl = new IndustryMap_BL();
            string selectedValues = string.Empty;

            if (IndustryId == "0")
            {

                if (EntityTypeID == "2")
                {
                    DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) ) as D order by CrDate desc");


                }
                else
                {
                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                        }
                        else
                        {
                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                        }

                    }
                    DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                }

            }
            else
            {
                // DT = objbl.BindAllConsumerByIndustryId(IndustryId, EntityTypeID);


                if (EntityTypeID == "2")
                {
                    DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5)) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");


                }
                else
                {
                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                        }
                        else
                        {
                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                        }

                    }
                    DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");
                }
            }


            Cache["ClientList"] = DT;
            lstClient.DataSource = DT;
            lstClient.ValueField = "Id";
            lstClient.TextField = "name";
            lstClient.DataBind();


        }
        public void bindAllTasks()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("SELECT task_id, task_title FROM tbl_master_task");
            taskList.DataSource = DT;
            taskList.DataValueField = "task_id";
            taskList.DataTextField = "task_title";
            taskList.DataBind();          
            taskList.Items.Insert(0, new ListItem("Select Task", "0"));

        }

        public void BindConsumer()
        {
            string AllUserCntId = string.Empty;
           // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
            for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                }
                else
                {
                    AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                }
            }

            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else    isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CL%' ) as D order by CrDate desc ");


            lbClientAvailable.DataSource = DT;
            lbClientAvailable.ValueField = "Id";
            lbClientAvailable.TextField = "name";
            lbClientAvailable.DataBind();

            Cache["ClientList"] = DT;
        }
        public void BindLead()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) ) as D order by CrDate desc");
            lbClientAvailable.DataSource = DT;
            lbClientAvailable.ValueField = "Id";
            lbClientAvailable.TextField = "name";
            lbClientAvailable.DataBind();

            Cache["ClientList"] = DT;
        }
        public void BindAllProduct()
        {
            StockDetails objBL = new StockDetails();
            DataTable DT = objBL.GetAllProduct();

            lbAvailable.DataSource = DT;
            lbAvailable.ValueField = "sProducts_ID";
            lbAvailable.TextField = "sProducts_Name";
            lbAvailable.DataBind();
            // Cache["ProductList"] = DT;

        }






        //Assign To drop down list is bind kaushik 15-11-2016
        [WebMethod]
        public static List<string> GetAllUserListBeforeSelect()
        {
            StringBuilder filter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();

            dtbl = objemployeebal.GetAssignedEmployeeDetailByReportingTo(owninterid);


            List<string> obj = new List<string>();
            if (dtbl != null && dtbl.Rows.Count > 0)
            {
                foreach (DataRow dr in dtbl.Rows)
                {

                    obj.Add(Convert.ToString(dr["name"]) + "(" + Convert.ToString(dr["designation"]) + ")" + "|" + Convert.ToString(dr["cnt_id"]));
                }

            }

            return obj;
        }


        //Supervisor drop down list is bind kaushik 15-11-2016
        [WebMethod]
        public static List<string> GetAllSupervisorUserListBeforeSelect()
        {
            StringBuilder filter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();

            dtbl = objemployeebal.GetSupervisorEmployeeDetailByReportingTo(owninterid);


            List<string> obj = new List<string>();

            if (dtbl != null && dtbl.Rows.Count > 0)
            {
                foreach (DataRow dr in dtbl.Rows)
                {

                    obj.Add(Convert.ToString(dr["name"]) + "(" + Convert.ToString(dr["designation"]) + ")" + "|" + Convert.ToString(dr["cnt_id"]));
                }

            }

            return obj;
        }


        //Supervisor drop down list is bind kaushik 15-11-2016
        [WebMethod]
        public static List<string> GetAllUserListAfterSelect(string Uid)
        {
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            Employee_BL objemployeebal = new Employee_BL();
            string owninterid = Convert.ToString(HttpContext.Current.Session["owninternalid"]);
            DataTable dtbl = new DataTable();

            dtbl = objemployeebal.GetSupervisorEmployeeDetailByReportingTo(owninterid);

            filter.Append("cnt_id <> '" + Uid + "'");
            DataView dv = dtbl.DefaultView;
            dv.RowFilter = filter.ToString();
            dtbl = dv.ToTable();

            List<string> obj = new List<string>();
            if (dtbl != null && dtbl.Rows.Count > 0)
            {
                foreach (DataRow dr in dtbl.Rows)
                {

                    obj.Add(Convert.ToString(dr["name"]) + "(" + Convert.ToString(dr["designation"]) + ")" + "|" + Convert.ToString(dr["cnt_id"]));
                }

            }

            return obj;
        }


        protected void ddlAssignTo_Callback(object sender, CallbackEventArgsBase e)
        {

        }
        public void txtAvailableProduct_TextChanged(object sender, EventArgs e)
        {

            var indid = hdnIndustry.Value;
            StockDetails objBL = new StockDetails();

            DataTable dtCache = objBL.GetPreferredProduct(indid);
            if (!String.IsNullOrEmpty(txtAvailableProduct.Text.Trim()))
            {
                string name = txtAvailableProduct.Text.Trim();
                //   DataTable dtCache = (DataTable)Cache["ProductList"];


                DataView dv = new DataView(dtCache, "sProducts_Name like '%" + name + "%'", "", DataViewRowState.CurrentRows);
                dtCache = dv.ToTable();
                lbAvailable.DataSource = dtCache;
                lbAvailable.ValueField = "sProducts_ID";
                lbAvailable.TextField = "sProducts_Name";
                lbAvailable.DataBind();
                //  BindAvaibleIndustry(name);
            }
            else
            {
                // DataTable dtCache = (DataTable)Cache["ProductList"];
                lbAvailable.DataSource = dtCache;
                lbAvailable.ValueField = "sProducts_ID";
                lbAvailable.TextField = "sProducts_Name";
                lbAvailable.DataBind();


                //BindAvaibleIndustry("");
            }
        }


        //Preferred product list is bind kaushik 22-11-2016

        [WebMethod]
        public static List<string> GetProductList(string IndustryID)
        {
            List<string> obj = new List<string>();
            StockDetails objBL = new StockDetails();
            DataTable DT = new DataTable();
            int index = 0;


            string selectedValues = string.Empty;

            if (IndustryID == "0")
            {
                DT = objBL.GetAllProduct();
            }
            else
            {


                DT = objBL.GetPreferredProduct(IndustryID);
            }

            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["sProducts_Name"]) + "|" + Convert.ToString(dr["sProducts_ID"]));
            }
            return obj;
        }

        //Lead or consumer list is bound kaushik 23-11-2016
        [WebMethod]
        public static List<string> GetLeadConsumerList(string Type)
        {

            string AllUserCntId = string.Empty;
           // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
            for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                }
                else
                {
                    AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                }

            }





            List<string> obj = new List<string>();
            DataTable DT = new DataTable();
            if (Type == "2")
            {
                DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) ) as D order by CrDate desc");



                foreach (DataRow dr in DT.Rows)
                {

                    obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["Id"]));
                }

            }
            else
            {
                DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CL%' ) as D order by CrDate desc ");



                foreach (DataRow dr in DT.Rows)
                {

                    obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["Id"]));
                }
            }

            return obj;
        }

        [WebMethod]
        public static List<string> GetConsumer()
        {

            string AllUserCntId = string.Empty;
          //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


            DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
            for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                }
                else
                {
                    AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                }

            }



            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else    isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where cnt_internalId like 'CL%' ) as D order by CrDate desc ");

            List<string> obj = new List<string>();

            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["cnt_id"]));
            }



            return obj;
        }




        [WebMethod]
        public static List<string> GetIndustryList(string hdnEditCustomerLeadValue)
        {
            SlaesActivitiesBL Slobl = new SlaesActivitiesBL();
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            IndustryMap_BL objbl = new IndustryMap_BL();
            DataTable dtbl = new DataTable();
            if (String.IsNullOrEmpty(hdnEditCustomerLeadValue))
            { dtbl = objbl.BindIndustryList(); }
            else
            {
                dtbl = Slobl.GetIndustry(hdnEditCustomerLeadValue);
            }



            List<string> obj = new List<string>();
            //kaushik 25-1-2016 all removed from industry list
            //  obj.Add("ALL" + "|" + "0");
            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["ind_industry"]) + "|" + Convert.ToString(dr["ind_id"]));
            }



            return obj;
        }


        [WebMethod]
        public static List<string> GetConsumerByIndustryIdList(string IndustryId, string EntityTypeID)
        {
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            IndustryMap_BL objbl = new IndustryMap_BL();
            DataTable dtbl = new DataTable();
           // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            string AllUserCntId = string.Empty;
            if (IndustryId == "0")
            {

                if (EntityTypeID == "2")
                {
                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' ) as D order by CrDate desc");


                }
                else
                {
                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                        }
                        else
                        {
                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                        }

                    }
                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                }


            }
            else
            {
                dtbl = objbl.BindAllConsumerByIndustryId(IndustryId, EntityTypeID);
            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["name"]) + "|" + Convert.ToString(dr["Id"]));
            }



            return obj;
        }

        //kaushik 7_01_2017 save and retain in same page
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var ProductValues = "";







                string sStartDate = Convert.ToDateTime(txtStartDate.Value.ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                string sEndDate = Convert.ToDateTime(txtEndDate.Value.ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                string AssignTo;

                string SuperVisorTo;
                string[] StartDateArr = sStartDate.Split(' ');
                string startDate = StartDateArr[0].ToString();
                string startTime = StartDateArr[1].ToString();
                string IndustryId = hdnIndustry.Value;
                string[] EndDateArr = sEndDate.Split(' ');
                string endDate = EndDateArr[0].ToString();
                string endTime = EndDateArr[1].ToString();
                //Added By:Subhabrata
                string ReceiverEmail = string.Empty;
                string AssignedTo_Email = string.Empty;
                string AssignedBy_Email = string.Empty;
                //End
                string ActivityTypes = hdnActivityType.Value;
                int StartVal = 0;
                string CreateDate = oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mmm:ss");
                string _id = "";
                string _idSales = "";
                string _actNo;
                string CreateUserId = Convert.ToString(HttpContext.Current.Session["userid"]);//Session UserID;
                //kaushik 30-12-2016
                string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);
                ViewState["Status"] = "CreateNew";
                string[] chrk1;
                string[] _tempProd;
                string[] _tempProdClass;
                string[] _tempProdDetail;
                String DataValue = string.Empty;
                string ActivityList = hdnActivityType.Value;
                string[] ProductType;

                string clientType = RadioButtonActivityList.SelectedValue;
                string plType = RdnType.SelectedValue;
                switch (ViewState["Status"].ToString())
                {
                    case "CreateNew":
                        int IsEmail = 0;
                        string client;
                        DateTime date1 = oDBEngine.GetDate();
                        string date = Convert.ToDateTime(date1.ToShortDateString()).ToString("yyyy-MM-dd");
                        string[] _temp;
                        //string Lead = Session["lead"].ToString();
                        client = hdnCustomerLead.Value.TrimStart(',');
                        _temp = client.Split(',');
                        string _sendStr = "";
                        int ij = 0;
                        bool _bVal = false;
                        AssignTo = hdnAssign.Value;
                        SuperVisorTo = hdnSupervisor.Value;
                        int count12 = 0;
                        string product;
                        string productclass;
                        string productID;
                        string productText;
                        if (Session["Count"] != null)
                        {
                            count12 = Convert.ToInt32(Session["Count"].ToString());
                        }
                        else
                        {
                            count12 = 0;
                        }
                        if (chkMail.Checked)
                        {
                            IsEmail = 1;
                        }


                        _actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                        product = hdnProduct.Value.Trim(',');
                        //Add Activity when product radiobutton is selected 
                        if (plType == "1")
                        {

                            oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + hdnProductClass.Value + "'");
                            string[,] Data = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            for (int ijk = 0; ijk < _temp.Length; ijk++)
                            {
                                chrk1 = _temp[0].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {


                                    // productText = hdnProductText.Value;
                                    _tempProd = product.Split(',');
                                    //kaushik 24_1_2016 Add Activity for product radiobutton

                                    for (int k = 0; k < _tempProd.Length; k++)
                                    {
                                        //  productID = obl.GetProductType(Convert.ToString(_tempProd[k]));

                                        string _Prod = "";

                                        oDBEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_product_id,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser,sls_assignedBy,sls_assignedTo", "'" + _id.ToString() + "','" + _temp[ijk].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','','','" + Convert.ToString(_tempProd[k]) + "','','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + UserId.ToString() + "','" + AssignTo + "'");
                                        // oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + _temp[k].ToString() + "'");
                                        string[,] DataSales = oDBEngine.GetFieldValue("tbl_trans_Sales", "max(sls_id)", null, 1);
                                        string CheckDataSales = DataSales[0, 0];
                                        if (CheckDataSales != "n")
                                        {
                                            _idSales = CheckDataSales.ToString();
                                        }
                                        //insert into tbl_trans_phonecall

                                        int indexPhone = ActivityList.IndexOf("1");
                                        int indexEmail = ActivityList.IndexOf("2");
                                        int indexSms = ActivityList.IndexOf("3");
                                        int indexSalesVisit = ActivityList.IndexOf("4");
                                        int indexMeeting = ActivityList.IndexOf("5");
                                        int indexSales = ActivityList.IndexOf("6");
                                        if (indexPhone >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");

                                        }
                                        DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + _temp[ijk].ToString() + "' And phc_NextActivityId='allot'");
                                        string tempPhonecallid = "";
                                        string nextvisitdateTime = "";
                                        if (dt != null & dt.Rows.Count > 0)
                                        {
                                            tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                            nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                        }

                                        //insert into tbl_trans_salesVisit
                                        if (indexSalesVisit >= 0)
                                        {

                                            if (tempPhonecallid == "")
                                            {
                                                oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                            }
                                            else
                                            {
                                                oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                            }

                                        }
                                        //insert into tbl_trans_OtherActivity
                                        if (indexEmail >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','2'");

                                        }

                                        if (indexSms >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','3'");

                                        }
                                        if (indexMeeting >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','5'");

                                        }

                                    }


                                    //Subhabrata:Done By isEmail marked as 101 to restrict execution of this code.Later it will be assigned as 1
                                    if (IsEmail == 1)
                                    {
                                        Employee_BL objemployeebal = new Employee_BL();
                                        AssignedTo_Email = AssignTo;
                                        AssignedBy_Email = UserId.ToString();
                                        string ActivityName = string.Empty;

                                        DataTable dtbl_AssignedTo = new DataTable();
                                        DataTable dtbl_AssignedBy = new DataTable();
                                        DataTable dtEmail_To = new DataTable();
                                        DataTable dt_EmailConfig = new DataTable();
                                        DataTable dt_ActivityName = new DataTable();


                                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 1);
                                        dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 2);
                                        dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);
                                        dt_ActivityName = objemployeebal.GetEmailAccountConfigDetails(CheckData, 10);

                                        if (dtEmail_To.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                            {
                                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                            }
                                            else
                                            {
                                                ReceiverEmail = "";
                                            }
                                        }

                                        foreach (var item in dtbl_AssignedTo.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(item.ToString()))
                                            {
                                                AssignedTo_Email += item.ToString() + "|";
                                            }

                                        }

                                        if (dt_ActivityName != null && dt_ActivityName.Rows.Count > 0)
                                        {
                                            ActivityName = Convert.ToString(dt_ActivityName.Rows[0].Field<string>("act_activityNo"));
                                        }
                                        else
                                        {
                                            ActivityName = "";
                                        }

                                        ListDictionary replacements = new ListDictionary();
                                        if (dtbl_AssignedTo.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssigneeTo%>", "");
                                        }
                                        if (dtbl_AssignedBy.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssignedBy%>", "");
                                        }
                                        replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));
                                        replacements.Add("<%ActivityStartDate%>", Convert.ToString(startDate));
                                        replacements.Add("<%ActivityCompletionDate%>", Convert.ToString(endDate));
                                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                                        if (!string.IsNullOrEmpty(ReceiverEmail))
                                        {
                                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 4);
                                        }


                                    }
                                    //End

                                }

                                catch
                                {
                                }

                            }

                        }
                        // Add Activity when Industry Radio button is selected
                        else if (plType == "2")
                        {


                            oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");
                            string[,] Data = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            for (int ijk = 0; ijk < _temp.Length; ijk++)
                            {
                                chrk1 = _temp[0].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {

                                    product = hdnProduct.Value.TrimStart(',');
                                    // productText = hdnProductText.Value;
                                    _tempProd = product.Split(',');
                                    //kaushik 24_1_2016 Add Activity for product radiobutton

                                    //for (int k = 0; k < _tempProd.Length; k++)
                                    //{
                                    //  productID = obl.GetProductType(Convert.ToString(_tempProd[k]));

                                    string _Prod = "";

                                    oDBEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_product_id,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser,sls_assignedBy,sls_assignedTo", "'" + _id.ToString() + "','" + _temp[ijk].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','','','','','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + UserId.ToString() + "','" + AssignTo + "'");
                                    // oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + _temp[k].ToString() + "'");
                                    string[,] DataSales = oDBEngine.GetFieldValue("tbl_trans_Sales", "max(sls_id)", null, 1);
                                    string CheckDataSales = DataSales[0, 0];
                                    if (CheckDataSales != "n")
                                    {
                                        _idSales = CheckDataSales.ToString();
                                    }
                                    //insert into tbl_trans_phonecall

                                    int indexPhone = ActivityList.IndexOf("1");
                                    int indexEmail = ActivityList.IndexOf("2");
                                    int indexSms = ActivityList.IndexOf("3");
                                    int indexSalesVisit = ActivityList.IndexOf("4");
                                    int indexMeeting = ActivityList.IndexOf("5");
                                    int indexSales = ActivityList.IndexOf("6");
                                    if (indexPhone >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");

                                    }
                                    DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + _temp[ijk].ToString() + "' And phc_NextActivityId='allot'");
                                    string tempPhonecallid = "";
                                    string nextvisitdateTime = "";
                                    if (dt != null & dt.Rows.Count > 0)
                                    {
                                        tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                        nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                    }

                                    //insert into tbl_trans_salesVisit
                                    if (indexSalesVisit >= 0)
                                    {

                                        if (tempPhonecallid == "")
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }

                                    }
                                    //insert into tbl_trans_OtherActivity
                                    if (indexEmail >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','2'");

                                    }

                                    if (indexSms >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','3'");

                                    }
                                    if (indexMeeting >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','5'");

                                    }

                                    // }


                                    //Subhabrata:Done By isEmail marked as 101 to restrict execution of this code.Later it will be assigned as 1
                                    if (IsEmail == 1)
                                    {
                                        Employee_BL objemployeebal = new Employee_BL();
                                        AssignedTo_Email = AssignTo;
                                        AssignedBy_Email = UserId.ToString();
                                        string ActivityName = string.Empty;

                                        DataTable dtbl_AssignedTo = new DataTable();
                                        DataTable dtbl_AssignedBy = new DataTable();
                                        DataTable dtEmail_To = new DataTable();
                                        DataTable dt_EmailConfig = new DataTable();
                                        DataTable dt_ActivityName = new DataTable();



                                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 1);
                                        dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 2);
                                        dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);
                                        dt_ActivityName = objemployeebal.GetEmailAccountConfigDetails(CheckData, 10);
                                        if (dtEmail_To.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                            {
                                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                            }
                                            else
                                            {
                                                ReceiverEmail = "";
                                            }
                                        }

                                        foreach (var item in dtbl_AssignedTo.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(item.ToString()))
                                            {
                                                AssignedTo_Email += item.ToString() + "|";
                                            }

                                        }
                                        if (dt_ActivityName != null && dt_ActivityName.Rows.Count > 0)
                                        {
                                            ActivityName = Convert.ToString(dt_ActivityName.Rows[0].Field<string>("act_activityNo"));
                                        }
                                        else
                                        {
                                            ActivityName = "";
                                        }

                                        ListDictionary replacements = new ListDictionary();
                                        if (dtbl_AssignedTo.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssigneeTo%>", "");
                                        }
                                        if (dtbl_AssignedBy.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssignedBy%>", "");
                                        }
                                        replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));
                                        replacements.Add("<%ActivityStartDate%>", Convert.ToString(startDate));
                                        replacements.Add("<%ActivityCompletionDate%>", Convert.ToString(endDate));
                                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                                        if (!string.IsNullOrEmpty(ReceiverEmail))
                                        {
                                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 4);
                                        }


                                    }
                                    //End

                                }

                                catch
                                {
                                }

                            }

                        }

                            //Add when pproduct class is selected
                        else
                        {

                            oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");
                            string[,] Data = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            for (int ijk = 0; ijk < _temp.Length; ijk++)
                            {
                                chrk1 = _temp[0].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {

                                    productclass = hdnProductClass.Value.TrimStart(',');
                                    // productText = hdnProductText.Value;
                                    _tempProdClass = productclass.Split(',');
                                    //kaushik 24_1_2016 Add Activity for product radiobutton

                                    // for (int k = 0; k < _tempProdClass.Length; k++)
                                    //  {
                                    //  productID = obl.GetProductType(Convert.ToString(_tempProdClass[k]));

                                    string _Prod = "";

                                    oDBEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_product_id,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser,sls_assignedBy,sls_assignedTo", "'" + _id.ToString() + "','" + _temp[ijk].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','','','','','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + UserId.ToString() + "','" + AssignTo + "'");
                                    // oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + _temp[k].ToString() + "'");
                                    string[,] DataSales = oDBEngine.GetFieldValue("tbl_trans_Sales", "max(sls_id)", null, 1);
                                    string CheckDataSales = DataSales[0, 0];
                                    if (CheckDataSales != "n")
                                    {
                                        _idSales = CheckDataSales.ToString();
                                    }
                                    //insert into tbl_trans_phonecall

                                    int indexPhone = ActivityList.IndexOf("1");
                                    int indexEmail = ActivityList.IndexOf("2");
                                    int indexSms = ActivityList.IndexOf("3");
                                    int indexSalesVisit = ActivityList.IndexOf("4");
                                    int indexMeeting = ActivityList.IndexOf("5");
                                    int indexSales = ActivityList.IndexOf("6");
                                    if (indexPhone >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");

                                    }
                                    DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + _temp[ijk].ToString() + "' And phc_NextActivityId='allot'");
                                    string tempPhonecallid = "";
                                    string nextvisitdateTime = "";
                                    if (dt != null & dt.Rows.Count > 0)
                                    {
                                        tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                        nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                    }

                                    //insert into tbl_trans_salesVisit
                                    if (indexSalesVisit >= 0)
                                    {

                                        if (tempPhonecallid == "")
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }

                                    }
                                    //insert into tbl_trans_OtherActivity
                                    if (indexEmail >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + CreateUserId + "','" + _idSales + "','2'");

                                    }

                                    if (indexSms >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + CreateUserId + "','" + _idSales + "','3'");

                                    }
                                    if (indexMeeting >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + CreateUserId + "','" + _idSales + "','5'");

                                    }

                                    //  }


                                    //Subhabrata:Done By isEmail marked as 101 to restrict execution of this code.Later it will be assigned as 1
                                    if (IsEmail == 1)
                                    {
                                        Employee_BL objemployeebal = new Employee_BL();
                                        AssignedTo_Email = AssignTo;
                                        AssignedBy_Email = UserId.ToString();
                                        string ActivityName = string.Empty;

                                        DataTable dtbl_AssignedTo = new DataTable();
                                        DataTable dtbl_AssignedBy = new DataTable();
                                        DataTable dtEmail_To = new DataTable();
                                        DataTable dt_EmailConfig = new DataTable();
                                        DataTable dt_ActivityName = new DataTable();



                                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 1);
                                        dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 2);
                                        dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);
                                        dt_ActivityName = objemployeebal.GetEmailAccountConfigDetails(CheckData, 10);
                                        if (dtEmail_To.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                            {
                                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                            }
                                            else
                                            {
                                                ReceiverEmail = "";
                                            }
                                        }

                                        foreach (var item in dtbl_AssignedTo.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(item.ToString()))
                                            {
                                                AssignedTo_Email += item.ToString() + "|";
                                            }

                                        }
                                        if (dt_ActivityName != null && dt_ActivityName.Rows.Count > 0)
                                        {
                                            ActivityName = Convert.ToString(dt_ActivityName.Rows[0].Field<string>("act_activityNo"));
                                        }
                                        else
                                        {
                                            ActivityName = "";
                                        }

                                        ListDictionary replacements = new ListDictionary();
                                        if (dtbl_AssignedTo.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssigneeTo%>", "");
                                        }
                                        if (dtbl_AssignedBy.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssignedBy%>", "");
                                        }
                                        replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));
                                        replacements.Add("<%ActivityStartDate%>", Convert.ToString(startDate));
                                        replacements.Add("<%ActivityCompletionDate%>", Convert.ToString(endDate));
                                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                                        if (!string.IsNullOrEmpty(ReceiverEmail))
                                        {
                                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 4);
                                        }


                                    }
                                    //End

                                }

                                catch
                                {
                                }

                            }

                        }
                        hdnProduct.Value = "";
                        hdnCustomerLead.Value = "";
                        hdnProductClass.Value = "";
                        hdnActivityType.Value = "";
                        txtStartDate.Text = "";
                        txtEndDate.Text = "";
                        txtActivityName.Text = "";
                        bindAllTasks();
                        RemoveSelectedProduct();
                        RemoveSelectedCustomer();
                        txtInstNote.Text = "";
                        RadioButtonActivityList.SelectedValue = "2";
                        RdnType.SelectedValue = "1";
                        drpPriority.SelectedValue = "2";
                        chkMail.Checked = false;
                        BindLead("-1");
                        BindCustomer("-1");
                        //loader stop
                        //Page.ClientScript.RegisterStartupScript(GetType(), "StopProgress", "<script>StopProgress();</script>");
                        //Added by:Subhabrata for send mail functionality


                        if (IsEmail == 1)
                        {

                            if (MailStatus == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpNotRedirect('Successfully saved and mail send');", true);
                                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "StopProgress();alert('Successfully saved and mail send');  txtActivityName.focus();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpNotRedirect('Successfully saved but mail not send');", true);
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "StopProgress();alert('Successfully saved but mail not send');  txtActivityName.focus();", true);
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpNotRedirect('Successfully saved');", true);

                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "StopProgress();alert('Successfully saved');  txtActivityName.focus();", true);
                        }


                        break;

                }
            }
            catch { }
        }


        public void BindCustomer(string industryId)
        {
            DataTable DT = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID=" + industryId + ")order by CrDate desc");


            lbClientAvailable.DataSource = DT;
            lbClientAvailable.ValueField = "Id";
            lbClientAvailable.TextField = "name";
            lbClientAvailable.DataBind();
        }

        public void BindLead(string industryId)
        {
            StockDetails objBL = new StockDetails();
            DataTable DT = objBL.GetPreferredProduct(industryId);
            lbAvailable.DataSource = DT;
            lbAvailable.ValueField = "sProducts_ID";
            lbAvailable.TextField = "sProducts_Name";
            lbAvailable.DataBind();

        }
        //kaushik 7_01_2017 save and go to list page
        protected void btnSubmitExit_Click(object sender, EventArgs e)
        {
            try
            {

                string sStartDate = Convert.ToDateTime(txtStartDate.Value.ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                string sEndDate = Convert.ToDateTime(txtEndDate.Value.ToString()).ToString("yyyy-MM-dd hh:mmm:ss");
                string AssignTo;
                string SuperVisorTo;
                string[] StartDateArr = sStartDate.Split(' ');
                string startDate = StartDateArr[0].ToString();
                string startTime = StartDateArr[1].ToString();
                string IndustryId = hdnIndustry.Value;
                string[] EndDateArr = sEndDate.Split(' ');
                string endDate = EndDateArr[0].ToString();
                string endTime = EndDateArr[1].ToString();
                //Added By:Subhabrata
                string ReceiverEmail = string.Empty;
                string AssignedTo_Email = string.Empty;
                string AssignedBy_Email = string.Empty;
                //End
                string ActivityTypes = hdnActivityType.Value;
                int StartVal = 0;
                string CreateDate = oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mmm:ss");
                string _id = "";
                string _idSales = "";
                string _actNo;
                string CreateUserId = Convert.ToString(HttpContext.Current.Session["userid"]);//Session UserID;
                //kaushik 30-12-2016
                string UserId = Convert.ToString(HttpContext.Current.Session["cntId"]);
                ViewState["Status"] = "CreateNew";
                string[] chrk1;
                string[] _tempProd;
                string[] _tempProdClass;
                string[] _tempProdDetail;
                String DataValue = string.Empty;
                string ActivityList = hdnActivityType.Value;
                string[] ProductType;

                string clientType = RadioButtonActivityList.SelectedValue;

                string plType = RdnType.SelectedValue;
                switch (ViewState["Status"].ToString())
                {
                    case "CreateNew":
                        int IsEmail = 0;
                        string client;
                        DateTime date1 = oDBEngine.GetDate();
                        string date = Convert.ToDateTime(date1.ToShortDateString()).ToString("yyyy-MM-dd");
                        string[] _temp;
                        //string Lead = Session["lead"].ToString();
                        client = hdnCustomerLead.Value.TrimStart(',');
                        _temp = client.Split(',');
                        string _sendStr = "";
                        int ij = 0;
                        bool _bVal = false;
                        AssignTo = hdnAssign.Value;
                        SuperVisorTo = hdnSupervisor.Value;
                        int count12 = 0;
                        string product;
                        string productclass;
                        string productID;
                        string productText;
                        if (Session["Count"] != null)
                        {
                            count12 = Convert.ToInt32(Session["Count"].ToString());
                        }
                        else
                        {
                            count12 = 0;
                        }
                        if (chkMail.Checked)
                        {
                            IsEmail = 1;
                        }


                        _actNo = oDBEngine.GetInternalId("SL", "tbl_trans_Activies", "act_activityNo", "act_activityNo");
                        product = hdnProduct.Value.TrimStart(',');
                        //Add Activity when product radiobutton is selected 
                        if (plType == "1")
                        {

                            ///Sudip Extra column Require in tbl_trans_Activies  Agentcnt_Id Db Migration V 1.0.110 [instructed By Debashis]
             
                            DataTable dtagent = oDBEngine.GetDataTable("select cont_agent.cnt_id from tbl_master_contact as cont cross join tbl_master_contact as cont_agent where cont.cnt_internalId=cont_agent.cnt_AssociatedEmp and cont.cnt_id=" + AssignTo);

                            //Rev Debashis
                            //oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,Agentcnt_Id,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + Convert.ToString(dtagent.Rows[0]["cnt_id"]) + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + hdnProductClass.Value + "'");
                            oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,Agentcnt_Id,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + Convert.ToString(dtagent.Rows[0]["cnt_id"]) + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");
                            //End of Rev Debashis
                          //oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + hdnProductClass.Value + "'");

                            ///Sudip Extra column Require in tbl_trans_Activies  Agentcnt_Id Db Migration V 1.0.110 [instructed By Debashis]

                            string[,] Data = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            for (int ijk = 0; ijk < _temp.Length; ijk++)
                            {
                                chrk1 = _temp[0].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {


                                    // productText = hdnProductText.Value;
                                    _tempProd = product.Split(',');

                                    for (int k = 0; k < _tempProd.Length; k++)
                                    {
                                        //  productID = obl.GetProductType(Convert.ToString(_tempProd[k]));

                                        string _Prod = "";

                                        oDBEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_product_id,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser,sls_assignedBy,sls_assignedTo", "'" + _id.ToString() + "','" + _temp[ijk].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','','','" + Convert.ToString(_tempProd[k]) + "','','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + UserId.ToString() + "','" + AssignTo + "'");
                                        // oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + _temp[k].ToString() + "'");
                                        string[,] DataSales = oDBEngine.GetFieldValue("tbl_trans_Sales", "max(sls_id)", null, 1);
                                        string CheckDataSales = DataSales[0, 0];
                                        if (CheckDataSales != "n")
                                        {
                                            _idSales = CheckDataSales.ToString();
                                        }
                                        //insert into tbl_trans_phonecall

                                        int indexPhone = ActivityList.IndexOf("1");
                                        int indexEmail = ActivityList.IndexOf("2");
                                        int indexSms = ActivityList.IndexOf("3");
                                        int indexSalesVisit = ActivityList.IndexOf("4");
                                        int indexMeeting = ActivityList.IndexOf("5");
                                        int indexSales = ActivityList.IndexOf("6");
                                        if (indexPhone >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");

                                        }
                                        DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + _temp[ijk].ToString() + "' And phc_NextActivityId='allot'");
                                        string tempPhonecallid = "";
                                        string nextvisitdateTime = "";
                                        if (dt != null & dt.Rows.Count > 0)
                                        {
                                            tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                            nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                        }

                                        //insert into tbl_trans_salesVisit
                                        if (indexSalesVisit >= 0)
                                        {

                                            if (tempPhonecallid == "")
                                            {
                                                oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                            }
                                            else
                                            {
                                                oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                            }

                                        }
                                        //insert into tbl_trans_OtherActivity
                                        if (indexEmail >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','2'");

                                        }

                                        if (indexSms >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','3'");

                                        }
                                        if (indexMeeting >= 0)
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','5'");

                                        }

                                    }

                                    //Subhabrata:Done By isEmail marked as 101 to restrict execution of this code.Later it will be assigned as 1
                                    if (IsEmail == 1)
                                    {
                                        Employee_BL objemployeebal = new Employee_BL();
                                        AssignedTo_Email = AssignTo;
                                        AssignedBy_Email = UserId.ToString();
                                        string ActivityName = string.Empty;

                                        DataTable dtbl_AssignedTo = new DataTable();
                                        DataTable dtbl_AssignedBy = new DataTable();
                                        DataTable dtEmail_To = new DataTable();
                                        DataTable dt_EmailConfig = new DataTable();
                                        DataTable dt_ActivityName = new DataTable();



                                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 1);
                                        dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 2);
                                        dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);
                                        dt_ActivityName = objemployeebal.GetEmailAccountConfigDetails(CheckData, 10);

                                        if (dtEmail_To.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                            {
                                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                            }
                                            else
                                            {
                                                ReceiverEmail = "";
                                            }
                                        }

                                        foreach (var item in dtbl_AssignedTo.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(item.ToString()))
                                            {
                                                AssignedTo_Email += item.ToString() + "|";
                                            }

                                        }
                                        if (dt_ActivityName != null && dt_ActivityName.Rows.Count > 0)
                                        {
                                            ActivityName = Convert.ToString(dt_ActivityName.Rows[0].Field<string>("act_activityNo"));
                                        }
                                        else
                                        {
                                            ActivityName = "";
                                        }

                                        ListDictionary replacements = new ListDictionary();
                                        if (dtbl_AssignedTo.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssigneeTo%>", "");
                                        }
                                        if (dtbl_AssignedBy.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssignedBy%>", "");
                                        }
                                        replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));
                                        replacements.Add("<%ActivityStartDate%>", Convert.ToString(startDate));
                                        replacements.Add("<%ActivityCompletionDate%>", Convert.ToString(endDate));
                                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                                        if (!string.IsNullOrEmpty(ReceiverEmail))
                                        {
                                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 4);
                                        }


                                    }
                                    //End

                                }

                                catch
                                {
                                }

                            }
                        }
                        //Add Activity when industry radiobutton is selected 
                        else if (plType == "2")
                        {

                            ///Sudip Extra column Require in tbl_trans_Activies  Agentcnt_Id Db Migration V 1.0.110 [instructed By Debashis]

                            DataTable dtagent = oDBEngine.GetDataTable("select cont_agent.cnt_id from tbl_master_contact as cont cross join tbl_master_contact as cont_agent where cont.cnt_internalId=cont_agent.cnt_AssociatedEmp and cont.cnt_id=" + AssignTo);


                            oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,Agentcnt_Id,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + Convert.ToString(dtagent.Rows[0]["cnt_id"]) + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");

                           /// oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");
                            ///Sudip Extra column Require in tbl_trans_Activies  Agentcnt_Id Db Migration V 1.0.110 [instructed By Debashis]
   
                            
                            
                            string[,] Data = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            for (int ijk = 0; ijk < _temp.Length; ijk++)
                            {
                                chrk1 = _temp[0].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {


                                    // productText = hdnProductText.Value;
                                    _tempProd = product.Split(',');

                                    //for (int k = 0; k < _tempProd.Length; k++)
                                    //{
                                    //  productID = obl.GetProductType(Convert.ToString(_tempProd[k]));

                                    string _Prod = "";

                                    oDBEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_product_id,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser,sls_assignedBy,sls_assignedTo", "'" + _id.ToString() + "','" + _temp[ijk].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','','','','','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + UserId.ToString() + "','" + AssignTo + "'");
                                    // oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + _temp[k].ToString() + "'");
                                    string[,] DataSales = oDBEngine.GetFieldValue("tbl_trans_Sales", "max(sls_id)", null, 1);
                                    string CheckDataSales = DataSales[0, 0];
                                    if (CheckDataSales != "n")
                                    {
                                        _idSales = CheckDataSales.ToString();
                                    }
                                    //insert into tbl_trans_phonecall

                                    int indexPhone = ActivityList.IndexOf("1");
                                    int indexEmail = ActivityList.IndexOf("2");
                                    int indexSms = ActivityList.IndexOf("3");
                                    int indexSalesVisit = ActivityList.IndexOf("4");
                                    int indexMeeting = ActivityList.IndexOf("5");
                                    int indexSales = ActivityList.IndexOf("6");
                                    if (indexPhone >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");

                                    }
                                    DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + _temp[ijk].ToString() + "' And phc_NextActivityId='allot'");
                                    string tempPhonecallid = "";
                                    string nextvisitdateTime = "";
                                    if (dt != null & dt.Rows.Count > 0)
                                    {
                                        tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                        nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                    }

                                    //insert into tbl_trans_salesVisit
                                    if (indexSalesVisit >= 0)
                                    {

                                        if (tempPhonecallid == "")
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }

                                    }
                                    //insert into tbl_trans_OtherActivity
                                    if (indexEmail >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','2'");

                                    }

                                    if (indexSms >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','3'");

                                    }
                                    if (indexMeeting >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','5'");

                                    }

                                    //   }

                                    //Subhabrata:Done By isEmail marked as 101 to restrict execution of this code.Later it will be assigned as 1
                                    if (IsEmail == 1)
                                    {
                                        Employee_BL objemployeebal = new Employee_BL();
                                        AssignedTo_Email = AssignTo;
                                        AssignedBy_Email = UserId.ToString();
                                        string ActivityName = string.Empty;

                                        DataTable dtbl_AssignedTo = new DataTable();
                                        DataTable dtbl_AssignedBy = new DataTable();
                                        DataTable dtEmail_To = new DataTable();
                                        DataTable dt_EmailConfig = new DataTable();
                                        DataTable dt_ActivityName = new DataTable();



                                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 1);
                                        dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 2);
                                        dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);
                                        dt_ActivityName = objemployeebal.GetEmailAccountConfigDetails(CheckData, 10);

                                        if (dtEmail_To.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                            {
                                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                            }
                                            else
                                            {
                                                ReceiverEmail = "";
                                            }
                                        }

                                        foreach (var item in dtbl_AssignedTo.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(item.ToString()))
                                            {
                                                AssignedTo_Email += item.ToString() + "|";
                                            }

                                        }
                                        if (dt_ActivityName != null && dt_ActivityName.Rows.Count > 0)
                                        {
                                            ActivityName = Convert.ToString(dt_ActivityName.Rows[0].Field<string>("act_activityNo"));
                                        }
                                        else
                                        {
                                            ActivityName = "";
                                        }

                                        ListDictionary replacements = new ListDictionary();
                                        if (dtbl_AssignedTo.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssigneeTo%>", "");
                                        }
                                        if (dtbl_AssignedBy.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssignedBy%>", "");
                                        }
                                        replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));
                                        replacements.Add("<%ActivityStartDate%>", Convert.ToString(startDate));
                                        replacements.Add("<%ActivityCompletionDate%>", Convert.ToString(endDate));
                                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                                        if (!string.IsNullOrEmpty(ReceiverEmail))
                                        {
                                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 4);
                                        }


                                    }
                                    //End

                                }

                                catch
                                {
                                }

                            }
                        }
                        //Add when product class is selected
                        else
                        {

                            ///Sudip Extra column Require in tbl_trans_Activies  Agentcnt_Id Db Migration V 1.0.110 [instructed By Debashis]

                            DataTable dtagent = oDBEngine.GetDataTable("select cont_agent.cnt_id from tbl_master_contact as cont cross join tbl_master_contact as cont_agent where cont.cnt_internalId=cont_agent.cnt_AssociatedEmp and cont.cnt_id=" + AssignTo);

                            oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,Agentcnt_Id,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + Convert.ToString(dtagent.Rows[0]["cnt_id"]) + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");

                       //     oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_instruction,CreateDate,CreateUser,act_supervisor,IsEmail,act_industryid,act_assign_task,act_activityTypes,act_clientType,act_activityName,act_type,act_productIds,act_productClassGroup", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + "0" + "','" + UserId.ToString() + "','" + AssignTo + "','" + date.ToString() + "','1','" + startDate.ToString() + "','" + endDate.ToString() + "','" + _actNo.ToString() + "','" + drpPriority.SelectedValue.ToString() + "','" + startTime.ToString() + "','" + endTime.ToString() + "','" + txtInstNote.Text + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + SuperVisorTo + "','" + IsEmail + "','" + IndustryId + "','" + taskList.SelectedValue.ToString() + "','" + ActivityTypes + "','" + clientType + "','" + txtActivityName.Text.Trim() + "','" + plType + "','" + product + "','" + hdnProductClass.Value + "'");

                            ///Sudip Extra column Require in tbl_trans_Activies  Agentcnt_Id Db Migration V 1.0.110 [instructed By Debashis]
 
                            string[,] Data = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                            string CheckData = Data[0, 0];
                            for (int ijk = 0; ijk < _temp.Length; ijk++)
                            {
                                chrk1 = _temp[0].Split('|');
                                if (CheckData != "n")
                                {
                                    _id = CheckData.ToString();
                                }
                                try
                                {

                                    // productclass = hdnProductClass.Value.TrimStart(',');
                                    // productText = hdnProductText.Value;
                                    //  _tempProdClass = productclass.Split(',');
                                    //kaushik 24_1_2016 Add Activity for product radiobutton

                                    //for (int k = 0; k < _tempProdClass.Length; k++)
                                    //{
                                    //  productID = obl.GetProductType(Convert.ToString(_tempProdClass[k]));

                                    string _Prod = "";

                                    oDBEngine.InsurtFieldValue("tbl_trans_Sales", "sls_activity_id,sls_contactlead_id,sls_branch_id,sls_sales_status,sls_date_closing,sls_ProductType,sls_product,sls_product_id,sls_estimated_value,sls_nextvisitdate,CreateDate,CreateUser,sls_assignedBy,sls_assignedTo", "'" + _id.ToString() + "','" + _temp[ijk].ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','4','" + endDate.ToString() + "','','','','','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateDate.ToString() + "','" + CreateUserId.ToString() + "','" + UserId.ToString() + "','" + AssignTo + "'");
                                    // oDBEngine.SetFieldValue("tbl_master_contact", "cnt_Status='" + _actNo.ToString() + "',LastModifyDate='" + CreateDate.ToString() + "',LastModifyUser='" + UserId.ToString() + "'", " cnt_internalId='" + _temp[k].ToString() + "'");
                                    string[,] DataSales = oDBEngine.GetFieldValue("tbl_trans_Sales", "max(sls_id)", null, 1);
                                    string CheckDataSales = DataSales[0, 0];
                                    if (CheckDataSales != "n")
                                    {
                                        _idSales = CheckDataSales.ToString();
                                    }
                                    //insert into tbl_trans_phonecall

                                    int indexPhone = ActivityList.IndexOf("1");
                                    int indexEmail = ActivityList.IndexOf("2");
                                    int indexSms = ActivityList.IndexOf("3");
                                    int indexSalesVisit = ActivityList.IndexOf("4");
                                    int indexMeeting = ActivityList.IndexOf("5");
                                    int indexSales = ActivityList.IndexOf("6");
                                    if (indexPhone >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_phonecall", "phc_activityId,phc_branchId,phc_leadcotactId,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");

                                    }
                                    DataTable dt = oDBEngine.GetDataTable("tbl_trans_phonecall", "phc_id,phc_nextCall", " phc_leadcotactId='" + _temp[ijk].ToString() + "' And phc_NextActivityId='allot'");
                                    string tempPhonecallid = "";
                                    string nextvisitdateTime = "";
                                    if (dt != null & dt.Rows.Count > 0)
                                    {
                                        tempPhonecallid = dt.Rows[0]["phc_id"].ToString();
                                        nextvisitdateTime = dt.Rows[0]["phc_nextCall"].ToString();
                                    }

                                    //insert into tbl_trans_salesVisit
                                    if (indexSalesVisit >= 0)
                                    {

                                        if (tempPhonecallid == "")
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_salesvisitoutcome,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','9','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }
                                        else
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_trans_salesVisit", "slv_activityId,slv_branchid,slv_leadcotactId,slv_PreviousActivityId,slv_nextvisitdatetime,slv_productoffered,CreateDate,CreateUser,tr_sid", "'" + _id.ToString() + "','" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + chrk1.GetValue(0).ToString() + "','" + tempPhonecallid + "','" + oDBEngine.GetDate().AddDays(1).ToString("yyyy-MM-dd hh:mm:ss") + "','" + "" + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "'");
                                        }

                                    }
                                    //insert into tbl_trans_OtherActivity
                                    if (indexEmail >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','2'");

                                    }

                                    if (indexSms >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','3'");

                                    }
                                    if (indexMeeting >= 0)
                                    {
                                        oDBEngine.InsurtFieldValue("tbl_trans_OtherActivity", "othActivity_activityId,othActivity_branchId,othActivity_leadcotactId,CreateDate,CreateUser,tr_sid,othActivity_Type", "'" + _id.ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userbranchID"]) + "','" + _temp[ijk].ToString() + "','" + oDBEngine.GetDate().ToString("yyyy-MM-dd hh:mm:ss") + "','" + CreateUserId + "','" + _idSales + "','5'");

                                    }

                                    // }


                                    //Subhabrata:Done By isEmail marked as 101 to restrict execution of this code.Later it will be assigned as 1
                                    if (IsEmail == 1)
                                    {
                                        Employee_BL objemployeebal = new Employee_BL();
                                        AssignedTo_Email = AssignTo;
                                        AssignedBy_Email = UserId.ToString();
                                        string ActivityName = string.Empty;

                                        DataTable dtbl_AssignedTo = new DataTable();
                                        DataTable dtbl_AssignedBy = new DataTable();
                                        DataTable dtEmail_To = new DataTable();
                                        DataTable dt_EmailConfig = new DataTable();
                                        DataTable dt_ActivityName = new DataTable();



                                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 1);
                                        dtbl_AssignedTo = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 2);
                                        dtbl_AssignedBy = objemployeebal.GetEmailAccountConfigDetails(CreateUserId, 3);
                                        dtEmail_To = objemployeebal.GetEmailAccountConfigDetails(AssignTo, 4);
                                        dt_ActivityName = objemployeebal.GetEmailAccountConfigDetails(CheckData, 10);
                                        if (dtEmail_To.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email")))
                                            {
                                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email"));
                                            }
                                            else
                                            {
                                                ReceiverEmail = "";
                                            }
                                        }

                                        foreach (var item in dtbl_AssignedTo.Rows)
                                        {
                                            if (!string.IsNullOrEmpty(item.ToString()))
                                            {
                                                AssignedTo_Email += item.ToString() + "|";
                                            }

                                        }
                                        if (dt_ActivityName != null && dt_ActivityName.Rows.Count > 0)
                                        {
                                            ActivityName = Convert.ToString(dt_ActivityName.Rows[0].Field<string>("act_activityNo"));
                                        }
                                        else
                                        {
                                            ActivityName = "";
                                        }

                                        ListDictionary replacements = new ListDictionary();
                                        if (dtbl_AssignedTo.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssigneeTo%>", Convert.ToString(dtbl_AssignedTo.Rows[0].Field<string>("AssigneTo")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssigneeTo%>", "");
                                        }
                                        if (dtbl_AssignedBy.Rows.Count > 0)
                                        {
                                            replacements.Add("<%AssignedBy%>", Convert.ToString(dtbl_AssignedBy.Rows[0].Field<string>("AssignedBy")));
                                        }
                                        else
                                        {
                                            replacements.Add("<%AssignedBy%>", "");
                                        }
                                        replacements.Add("<%TimeOfError%>", Convert.ToString(DateTime.Now));
                                        replacements.Add("<%ActivityStartDate%>", Convert.ToString(startDate));
                                        replacements.Add("<%ActivityCompletionDate%>", Convert.ToString(endDate));
                                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                                        if (!string.IsNullOrEmpty(ReceiverEmail))
                                        {
                                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 4);
                                        }


                                    }
                                    //End

                                }

                                catch
                                {
                                }

                            }

                        }

                        hdnProduct.Value = "";
                        hdnProductClass.Value = "";
                        hdnCustomerLead.Value = "";
                        hdnActivityType.Value = "";
                        txtStartDate.Text = "";
                        txtEndDate.Text = "";
                        txtActivityName.Text = "";
                        txtInstNote.Text = "";
                        hdnidslsact.Value = "";
                        //loader stop

                        Page.ClientScript.RegisterStartupScript(GetType(), "StopProgress", "<script>StopProgress();</script>");
                        //Added by:Subhabrata for send mail functionality


                        if (IsEmail == 1)
                        {

                            if (MailStatus == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpRedirect('Successfully saved and mail send','Sales_List.aspx');", true);
                                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "StopProgress();alert('Successfully saved and mail send');  txtActivityName.focus();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpRedirect('Successfully saved but mail not send','Sales_List.aspx');", true);
                                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "StopProgress();alert('Successfully saved but mail not send');  txtActivityName.focus();", true);
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "popUpRedirect('Successfully saved','Sales_List.aspx');", true);

                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "StopProgress();alert('Successfully saved');  txtActivityName.focus();", true);
                        }


                        //End

                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "jAlert('Successfully Submitted');", true);
                        //   Response.Redirect("Sales_List.aspx", false);

                        break;

                }
            }
            catch { }
        }




        //public void SetDateFormat()
        //{
        //    txtStartDate.TimeSectionProperties.Visible = true;
        //    txtStartDate.UseMaskBehavior = true;
        //    txtStartDate.EditFormatString = "dd-MM-yyyy";
        //    txtStartDate.DisplayFormatString = "dd-MM-yyyy";


        //    txtEndDate.TimeSectionProperties.Visible = true;
        //    txtEndDate.UseMaskBehavior = true;
        //    txtEndDate.EditFormatString = "dd-MM-yyyy";
        //    txtEndDate.DisplayFormatString = "dd-MM-yyyy";

        //}


        [WebMethod]
        public static List<string> GetActivityTypeList(String NoteId)
        {
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            if (NoteId.Trim() == "")
            {
                dtbl = objbl.GetActivityTypeList();
            }
            else
            {
                dtbl = objbl.GetActivityTypeList(NoteId);
            }


            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["aty_activityType"]) + "|" + Convert.ToString(dr["aty_id"]));
            }



            return obj;
        }



        [WebMethod]
        public static List<string> GetProductClassList(String IndustryID)
        {
            StockDetails objBL = new StockDetails();
            DataTable dtbl = new DataTable();
            dtbl = objBL.GetPreferredProductClassGroup(IndustryID);


            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["ProductClass_Name"]) + "|" + Convert.ToString(dr["ProductClass_ID"]));
            }


            return obj;
        }

        protected void lbAvailable_Callback(object sender, CallbackEventArgsBase e)
        {
            var indid = hdnIndustry.Value;
            StockDetails objBL = new StockDetails();

            DataTable dtCache = objBL.GetPreferredProduct(indid);
            ASPxListBox lbProduct = sender as ASPxListBox;
            string[] CallVal = e.Parameter.ToString().Split('~');

            if (CallVal[0].ToString() == "productlist")
            {
                if (!String.IsNullOrEmpty(txtAvailableProduct.Text.Trim()))
                {
                    string name = txtAvailableProduct.Text.Trim();
                    //  DataTable dtCache = (DataTable)Cache["ProductList"];


                    DataView dv = new DataView(dtCache, "sProducts_Name like '%" + CallVal[1].ToString() + "%'", "", DataViewRowState.CurrentRows);
                    dtCache = dv.ToTable();
                    lbAvailable.DataSource = dtCache;
                    lbAvailable.ValueField = "sProducts_ID";
                    lbAvailable.TextField = "sProducts_Name";
                    lbAvailable.DataBind();
                    //  BindAvaibleIndustry(name);
                }
                else
                {
                    // DataTable dtCache = (DataTable)Cache["ProductList"];
                    lbAvailable.DataSource = dtCache;
                    lbAvailable.ValueField = "sProducts_ID";
                    lbAvailable.TextField = "sProducts_Name";
                    lbAvailable.DataBind();


                    //BindAvaibleIndustry("");
                }
                SetAllSelectedItems(hdnProduct.Value);

            }

            else if (CallVal[0].ToString() == "ProductGroup")
            {
                //  DataTable dtCache = (DataTable)Cache["ProductList"];
                lbAvailable.DataSource = dtCache;
                lbAvailable.ValueField = "sProducts_ID";
                lbAvailable.TextField = "sProducts_Name";
                lbAvailable.DataBind();
                var Status = CallVal[1].ToString();

                productfinal = hdnProduct.Value;
                //  RemoveSelectedProduct();
                SelectProductByClassGroup(Status, sender);

                SortProductSelectedItem();
            }
            else if (CallVal[0].ToString() == "ProductGroupFinal")
            {
                // DataTable dtCache = (DataTable)Cache["ProductList"];
                lbAvailable.DataSource = dtCache;
                lbAvailable.ValueField = "sProducts_ID";
                lbAvailable.TextField = "sProducts_Name";
                lbAvailable.DataBind();
                // var Status = CallVal[1].ToString();

                productfinal = hdnProduct.Value;
                SelectChkUnchkProductByClassGroup(sender);
                SortProductSelectedItem();
            }

            else if (CallVal[0].ToString() == "RemoveProduct")
            {
                BindProductByIndustryId(lbProduct, "-1");
            }
            else { BindProductByIndustryId(lbProduct, e.Parameter); }

            lbAvailable.JSProperties["cphdnProduct"] = hdnProduct.Value;
        }

        public void SelectChkUnchkProductByClassGroup(object sender)
        {

            StockDetails objBL = new StockDetails();
            DataTable dtOldbl = new DataTable();
            DataTable dtNewbl = new DataTable();

            string IndustryID = hdnIndustry.Value;
            string ProductNewClassIds = hdnProductClass.Value;
            string ProductOldClassIds = hdnOldProductClass.Value;
            string replaceNewval = "";
            string replaceOldval = "";

            replaceNewval = ProductNewClassIds;
            replaceOldval = ProductOldClassIds;

            dtOldbl = objBL.GetEmployeePrefereedProductsByProductClass(IndustryID, replaceOldval.Trim(','));
            dtNewbl = objBL.GetEmployeePrefereedProductsByProductClass(IndustryID, replaceNewval.Trim(','));


            if (dtOldbl != null && dtOldbl.Rows.Count > 0)
            {
                bool selected = false;
                SetAllSelectedItemsByVal(Convert.ToString(dtOldbl.Rows[0]["ProductIds"]), selected, sender);
            }
            if (dtNewbl != null && dtNewbl.Rows.Count > 0)
            {
                bool selected = true;
                SetAllSelectedItemsByVal(Convert.ToString(dtNewbl.Rows[0]["ProductIds"]), selected, sender);
            }


            //  dtbl = objBL.GetEmployeePrefereedProductsByProductClass(IndustryID, ProductClassIds);
            //if(dtbl !=null && dtbl.Rows.Count>0)
            //{
            //    if (string.IsNullOrEmpty(ProductNewClassIds))
            //        { 
            //        SetAllUnSelectedItems("");
            //    }
            //    else {

            //    SetAllSelectedItems(Convert.ToString(dtbl.Rows[0]["ProductIds"]));

            //    SetAllSelectedItems(hdnProduct.Value);
            //    }

            //}

        }
        public void SortProductSelectedItem()
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


        public void SortClientSelectedItem()
        {
            List<ListEditItem> SelectedItems = new List<ListEditItem>(); ;
            List<ListEditItem> RestItems = new List<ListEditItem>();
            for (int i = 0; i < lbClientAvailable.Items.Count; i++)
            {
                if (lbClientAvailable.Items[i].Selected)
                {
                    SelectedItems.Add(lbClientAvailable.Items[i]);
                }
                else
                {
                    RestItems.Add(lbClientAvailable.Items[i]);
                }
            }

            lbClientAvailable.Items.Clear();
            foreach (var obj in SelectedItems)
            {
                lbClientAvailable.Items.Add(obj);
            }
            foreach (var obj in RestItems)
            {
                lbClientAvailable.Items.Add(obj);
            }
        }
        public void SelectProductByClassGroup(string Status, object sender)
        {

            StockDetails objBL = new StockDetails();
            DataTable dtbl = new DataTable();
            string IndustryID = hdnIndustry.Value;
            string ProductNewClassIds = hdnProductClass.Value;
            string ProductOldClassIds = hdnOldProductClass.Value;
            string replaceval = "";

            if (Status == "checked")
            {
                //if (ProductOldClassIds != "")
                //{ replaceval = ProductNewClassIds.Replace(ProductOldClassIds, ""); }
                //else { replaceval = ProductNewClassIds; }

                replaceval = ProductNewClassIds;
            }
            else
            {
                if (ProductNewClassIds != "")
                {
                    replaceval = ProductOldClassIds.Replace(ProductNewClassIds, "");
                }
                else { replaceval = ProductOldClassIds; }
            }
            dtbl = objBL.GetEmployeePrefereedProductsByProductClass(IndustryID, replaceval.Trim(','));

            if (dtbl != null && dtbl.Rows.Count > 0)
            {
                bool selected = false;
                if (Status == "checked")
                { selected = true; }
                SetAllSelectedItemsByVal(Convert.ToString(dtbl.Rows[0]["ProductIds"]), selected, sender);
            }
            //else { SetAllSelectedItemsByVal("0", false, sender); }

            //  dtbl = objBL.GetEmployeePrefereedProductsByProductClass(IndustryID, ProductClassIds);
            //if(dtbl !=null && dtbl.Rows.Count>0)
            //{
            //    if (string.IsNullOrEmpty(ProductNewClassIds))
            //        { 
            //        SetAllUnSelectedItems("");
            //    }
            //    else {

            //    SetAllSelectedItems(Convert.ToString(dtbl.Rows[0]["ProductIds"]));

            //    SetAllSelectedItems(hdnProduct.Value);
            //    }

            //}

        }

        protected void lbClientAvailable_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                var LeadCustomer = "";
                string BranchID = Convert.ToString(Session["userbranchHierarchy"]);
                ASPxListBox lbProduct = sender as ASPxListBox;
                string[] CallVal = e.Parameter.ToString().Split('~');
                DataTable dtbl = new DataTable();

                string clid = Convert.ToString(Request.QueryString["id"]);

                if (String.IsNullOrEmpty(clid))
                {
                    //if (CallVal[0].ToString() == "ClientselectAlllist")
                    //{
                    //  //  Cache["ClientList"] = dtbl;

                    //    DataTable DT = new DataTable();
                    //    DT = (DataTable)Cache["ClientList"];
                    //    lbClientAvailable.DataSource = DT;
                    //    lbClientAvailable.ValueField = "Id";
                    //    lbClientAvailable.TextField = "name";
                    //    lbClientAvailable.DataBind();

                    //    var client = "";
                    //    int list_counter = lbClientAvailable.Items.Count;
                    //    for (int i = 0; i < list_counter; i++)
                    //    {

                    //            client += Convert.ToString(lbClientAvailable.Items[i].Text) + ",";


                    //    }
                    //    hdnCustomerLead.Value = client;
                    //    lbClientAvailable.JSProperties["cphdnCustomer"] = hdnCustomerLead.Value;
                    //}

                    //else if (CallVal[0].ToString() == "ClientUnselectAlllist")
                    //{
                    //    DataTable DT = new DataTable();
                    //    DT = (DataTable)Cache["ClientList"];
                    //    lbClientAvailable.DataSource = DT;
                    //    lbClientAvailable.ValueField = "Id";
                    //    lbClientAvailable.TextField = "name";
                    //    lbClientAvailable.DataBind();
                    //    var client = "";
                    //    int list_counter = lbClientAvailable.Items.Count;
                    //    for (int i = 0; i < list_counter; i++)
                    //    {
                    //        //if (Convert.ToString(lbClientAvailable.Items[i].Value) == pitems)
                    //        //{
                    //        //    lbClientAvailable.Items[i].Selected = true;


                    //        client += Convert.ToString(lbClientAvailable.Items[i].Text) + ",";

                    //        // }
                    //    }
                    //    hdnCustomerLead.Value = client;
                    //}


                    if (CallVal[0].ToString() == "Clientlist")
                    {
                        string IndustryId = CallVal[1].ToString();
                        string EntityTypeID = CallVal[2].ToString();
                        string SelectTypeID = CallVal[3].ToString();
                        IndustryMap_BL objbl = new IndustryMap_BL();

                       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                        string AllUserCntId = string.Empty;
                        //customer products
                        if (SelectTypeID == "4")
                        {
                            dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID='0')order by CrDate desc");
                            Cache["ClientList"] = dtbl;
                            lbClientAvailable.DataSource = dtbl;
                            lbClientAvailable.ValueField = "Id";
                            lbClientAvailable.TextField = "name";
                            lbClientAvailable.DataBind();
                        }
                        else if (SelectTypeID == "1" || SelectTypeID == "3")
                        {
                            if (IndustryId == "0")
                            {

                                if (EntityTypeID == "3")
                                {
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' ) as D order by CrDate desc");


                                }
                                else
                                {
                                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }
                                        else
                                        {
                                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }

                                    }
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where cnt_internalId like 'CL%' )  as D order by CrDate desc");
                                    //  dtbl = obl.GetAllCustomer(BranchID, AllUserCntId);
                                    // 
                                    //  dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                                }


                            }
                            else
                            {
                                if (EntityTypeID == "3")
                                {
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");


                                }
                                else
                                {
                                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }
                                        else
                                        {
                                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }

                                    }
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where  cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");

                                    //  dtbl = obl.GetAllCustomerByIndustryId(BranchID, AllUserCntId, IndustryId);
                                    // 
                                    //   dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");
                                }
                                // dtbl = objbl.BindAllConsumerByIndustryId(IndustryId, EntityTypeID);
                            }


                            Cache["ClientList"] = dtbl;
                            lbClientAvailable.DataSource = dtbl;
                            lbClientAvailable.ValueField = "Id";
                            lbClientAvailable.TextField = "name";
                            lbClientAvailable.DataBind();


                        }
                        //industry
                        else if (SelectTypeID == "2")
                        {

                            if (IndustryId == "0")
                            {

                                if (EntityTypeID == "3")
                                {
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' ) as D order by CrDate desc");


                                }
                                else
                                {
                                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }
                                        else
                                        {
                                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }

                                    }
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where  cnt_internalId like 'CL%' )  as D order by CrDate desc");
                                    //  dtbl = obl.GetAllCustomer(BranchID, AllUserCntId);
                                    // 
                                    //  dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                                }


                            }
                            else
                            {
                                if (EntityTypeID == "3")
                                {
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");


                                }
                                else
                                {
                                    DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                    for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }
                                        else
                                        {
                                            AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                        }

                                    }
                                    dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where  cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");

                                    //  dtbl = obl.GetAllCustomerByIndustryId(BranchID, AllUserCntId, IndustryId);
                                    // 
                                    //   dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");
                                }
                                // dtbl = objbl.BindAllConsumerByIndustryId(IndustryId, EntityTypeID);
                            }


                            Cache["ClientList"] = dtbl;
                            lbClientAvailable.DataSource = dtbl;
                            lbClientAvailable.ValueField = "Id";
                            lbClientAvailable.TextField = "name";
                            lbClientAvailable.DataBind();

                            for (int i = 0; i < dtbl.Rows.Count; i++)
                            {
                                SelectedClient(Convert.ToString(dtbl.Rows[i]["Id"]));
                                if (i == 0)
                                { LeadCustomer += dtbl.Rows[i]["Id"]; }
                                else { LeadCustomer += "," + dtbl.Rows[i]["Id"]; }
                                //SelectedClient(Convert.ToString(ds.Tables[1].Rows[i]["sls_contactlead_id"]));

                            }
                            hdnCustomerLead.Value = LeadCustomer;

                            lbClientAvailable.JSProperties["cphdnCustomer"] = hdnCustomerLead.Value;
                        }
                        else
                        {

                            //selected list
                            DataTable dt = new DataTable();
                            string Id = Convert.ToString(CallVal[4]);

                            if (Id != "null")
                            {


                                if (IndustryId == "0")
                                {

                                    if (EntityTypeID == "3")
                                    {
                                        dt = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' ) as D order by CrDate desc");


                                    }
                                    else
                                    {
                                        DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                        for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                            }
                                            else
                                            {
                                                AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                            }

                                        }
                                        dt = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where  cnt_internalId like 'CL%' )  as D order by CrDate desc");
                                        //  dtbl = obl.GetAllCustomer(BranchID, AllUserCntId);
                                        // 
                                        //  dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                                    }


                                }
                                else
                                {


                                    if (EntityTypeID == "3")
                                    {
                                        dt = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");


                                    }
                                    else
                                    {
                                        DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                        for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                        {
                                            if (i == 0)
                                            {
                                                AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                            }
                                            else
                                            {
                                                AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                            }

                                        }
                                        dt = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");

                                        //  dtbl = obl.GetAllCustomerByIndustryId(BranchID, AllUserCntId, IndustryId);
                                        // 
                                        //   dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where (tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");
                                    }
                                    // dtbl = objbl.BindAllConsumerByIndustryId(IndustryId, EntityTypeID);
                                }

                            }
                            else { dt = null; }



                            //selected list


                            if (EntityTypeID == "3")
                            {
                                dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_Lead_Stage not in(4,5) and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=2 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");

                            }
                            else
                            {
                                DataSet CntId = oDBEngine.PopulateData("user_contactid", "tbl_master_user", " user_id in(" + HttpContext.Current.Session["userchildHierarchy"] + ")");
                                for (int i = 0; i < CntId.Tables[0].Rows.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        AllUserCntId = CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                    }
                                    else
                                    {
                                        AllUserCntId += "," + CntId.Tables[0].Rows[i]["user_contactid"].ToString();
                                    }

                                }
                                dtbl = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_internalId AS Id,case when( cnt_middleName is null  or cnt_middleName='') then isnull(cnt_firstName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' else   isnull(cnt_firstName,'NA')+' '+ isnull(cnt_middleName,'NA')+' ' +isnull(cnt_LastName,'NA')+' ' end as name ,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id  where  cnt_internalId like 'CL%' )  as D where D.id in (  select tmc.cnt_internalId  from tbl_master_contact  tmc inner join Master_IndustryMap mip on tmc.cnt_internalId=mip.IndustryMap_EntityID where mip.IndustryMap_EntityType=3 and mip.IndustryMap_IndustryID=" + IndustryId + ")order by CrDate desc");

                            }

                            Cache["ClientList"] = dtbl;
                            lbClientAvailable.DataSource = dtbl;
                            lbClientAvailable.ValueField = "Id";
                            lbClientAvailable.TextField = "name";
                            lbClientAvailable.DataBind();

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    SelectedClient(Convert.ToString(dt.Rows[i]["Id"]));
                                    if (i == 0)
                                    { LeadCustomer += dt.Rows[i]["Id"]; }
                                    else { LeadCustomer += "," + dt.Rows[i]["Id"]; }
                                    //SelectedClient(Convert.ToString(ds.Tables[1].Rows[i]["sls_contactlead_id"]));

                                }
                            }
                            else { LeadCustomer = ""; }
                            hdnCustomerLead.Value = LeadCustomer;

                            lbClientAvailable.JSProperties["cphdnCustomer"] = hdnCustomerLead.Value;
                        }

                    }

                    if (CallVal[0].ToString() == "SearchClientlist")
                    {
                        if (!String.IsNullOrEmpty(txtClient.Text.Trim()))
                        {
                            string name = txtClient.Text.Trim();
                            DataTable dtCache = (DataTable)Cache["ClientList"];


                            DataView dv = new DataView(dtCache, "name like '%" + CallVal[1].ToString() + "%'", "", DataViewRowState.CurrentRows);
                            dtCache = dv.ToTable();
                            lbClientAvailable.DataSource = dtCache;
                            lbClientAvailable.ValueField = "Id";
                            lbClientAvailable.TextField = "name";
                            lbClientAvailable.DataBind();
                            //  BindAvaibleIndustry(name);
                        }
                        else
                        {
                            DataTable dtCache = (DataTable)Cache["ClientList"];
                            lbClientAvailable.DataSource = dtCache;
                            lbClientAvailable.ValueField = "Id";
                            lbClientAvailable.TextField = "name";
                            lbClientAvailable.DataBind();


                            //BindAvaibleIndustry("");
                        }
                        SetAllClientSelectedItems(hdnCustomerLead.Value);

                    }
                }
                // else { BindProductByIndustryId(lbProduct, e.Parameter); }

            }
            catch { }
        }
        protected void SetAllSelectedItems(string items)
        {
            var product = "";
            string[] options = items.Split(',');
            string aa = hdnProduct.Value;
            for (int i = 0; i < options.Length; i++)
            {
                SelectedProduct(Convert.ToString(options[i]));
                product += Convert.ToString(options[i]) + ",";

            }
            //SelectedProduct(hdnProduct.Value.Trim(','));
            //product += hdnProduct.Value.Trim(',') + ",";
            //hdnProduct.Value = product;

            // hdnIndustry.Value = "400";
        }


        protected void SetAllSelectedItemsByVal(string items, bool IsSelected, object sender)
        {
            // var product = "";
            string[] options = items.Split(',');
            productfinal = hdnProduct.Value.Trim(',').Replace(",,", ",");
            string[] optionsFinal = productfinal.Split(',');
            for (int j = 0; j < optionsFinal.Length; j++)
            {
                SelectedExistProductClassByVal(Convert.ToString(optionsFinal[j]), true, sender);
                // product += Convert.ToString(options[i]) + ",";

            }
            for (int i = 0; i < options.Length; i++)
            {
                SelectedProductClassByVal(Convert.ToString(options[i]), IsSelected, sender);
                // product += Convert.ToString(options[i]) + ",";

            }

            hdnProduct.Value = productfinal + ',' + hdnProduct.Value;
            hdnProduct.Value = hdnProduct.Value.Trim(',').Replace(",,", ",");


            if (!IsSelected)
            {

                var client = "";
                int list_counter = lbAvailable.Items.Count;
                for (int i = 0; i < list_counter; i++)
                {
                    if (lbAvailable.Items[i].Selected)
                    {



                        client += Convert.ToString(lbAvailable.Items[i].Value) + ",";

                    }
                }
                hdnProduct.Value = client.Trim(',');
            }



            //SelectedProductClassByVal(hdnProduct.Value.Trim(','), IsSelected);
            //product += hdnProduct.Value.Trim(',') + ",";
            //  hdnProduct.Value = product;

            // hdnIndustry.Value = "400";
        }

        protected void SetAllUnSelectedItems(string items)
        {
            var product = "";
            string[] options = items.Split(',');

            for (int i = 0; i < options.Length; i++)
            {
                UnSelectedProduct(Convert.ToString(options[i]));
                //  product += Convert.ToString(lbAvailable.Items[i].Value) + ",";
                if (!String.IsNullOrEmpty(Convert.ToString(options[i])))
                {
                    hdnProduct.Value.Replace(Convert.ToString(options[i]), "");
                }
            }
            // SelectedProduct(hdnProduct.Value.TrimEnd(','));
            //hdnProduct.Value =hdnProduct.Value + product;



            string aa = hdnProduct.Value.Trim(',');

            SelectedProduct(hdnProduct.Value.Trim(','));
        }

        protected void SetAllClientSelectedItems(string items)
        {
            string[] options = items.Split(',');

            for (int i = 0; i < options.Length; i++)
            {
                SelectedClient(Convert.ToString(options[i]));

            }

        }
    }
}