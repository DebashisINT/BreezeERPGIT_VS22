using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using System.Collections.Generic;
using System.Web.Services;
using ERP.Models;
using System.Linq;
using DevExpress.Web;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_groupmasterPopUp : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        string[,] AllType;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //GenericMethod oGenericMethod;

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        clsDropDownList OclsDropDownList = new clsDropDownList();

        protected void Page_Init(object sender, EventArgs e)
        {
            SelectName.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            //txtContact.Attributes.Add("onkeyup", "showOptions(this,'SearchGroupCustomer',event)");
            //txtContact.Attributes.Add("onfocus", "showOptions(this,'SearchGroupCustomer',event)");
            //txtContact.Attributes.Add("onclick", "showOptions(this,'SearchGroupCustomer',event)");
            if (!IsPostBack)
            {

                AllType = oDBEngine.GetFieldValue(" tbl_master_contactType ", " cnt_prefix,cnttpy_contactType ", null, 2, " cnttpy_contactType ");
                // AllType = oDBEngine.GetFieldValue(" tbl_master_contactType ", " cnttpy_id,cnttpy_contactType ", null, 2, " cnttpy_contactType ");
                // oDBEngine.AddDataToDropDownList(AllType, DDLAddData);
                OclsDropDownList.AddDataToDropDownList(AllType, DDLAddData);
                DDLAddData.Items.Insert(0, new ListItem("--Select--", "0"));
                txtID.Value = Request.QueryString["id"];
                MType.Value = Request.QueryString["type"];
                //DDLAddData.SelectedValue = oDBEngine.GetFieldValue1("tbl_master_groupmaster", "gpm_Type", "MType.Value", 1);
                DataTable dt = oDBEngine.GetDataTable("select gpm_Type from tbl_master_groupmaster where gpm_Description='" + MType.Value + "'");

                DDLAddData.SelectedItem.Text = Convert.ToString(dt.Rows[0]["gpm_Type"]);
                hdnCntType.Value = DDLAddData.SelectedItem.Text;

                //oGenericMethod = new GenericMethod();
                BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

                DataTable DT = new DataTable();
                DT.Rows.Clear();
                //DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + firstname + "%'  and con.cnt_branchid=b.branch_id");
                List<string> obj = new List<string>();



                //string[] param = Convert.ToString(obj4).Split('~');


                string id = Request.QueryString["id"];

                DataTable dtGrp = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_MemberType,gpm_type", "gpm_id='" + id + "'");
                //string MType = Convert.ToString(dtGrp.Rows[0]["gpm_MemberType"]);
                string GType = Convert.ToString(dtGrp.Rows[0]["gpm_type"]);
                string Cnt_type = "";
                if (GType == "Customers")
                {
                    Cnt_type = "CL";
                }
                else if (GType == "Vendors")
                {
                    Cnt_type = "DV";
                }

                else if ((GType == "Relationship Partner") || GType == "RelationshipPartner")
                {
                    Cnt_type = "RA";
                }
                else if (GType == "DriverTransporter")
                {
                    Cnt_type = "TR";
                }


                //string cnt_prefix = Convert.ToString(param[2]);
                //DataTable dtContype = oDBEngine.GetDataTable("tbl_master_contactType", "cnttpy_contactType", "cnt_prefix='" + cnt_prefix + "'");
                //string cnttpy_contactType = Convert.ToString(dtContype.Rows[0]["cnttpy_contactType"]);

                DataTable dtgroup = oDBEngine.GetDataTable("select cnt_firstName+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name,cnt_internalId  from  tbl_master_contact where cnt_contactType='" + Cnt_type + "'");


                //DataTable dtSeg = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), Session["usersegid"].ToString());
                //if (dtSeg.Rows[0]["Exh_ShortName"].ToString() == "NSDL")
                //{
                //    DDLAddData.SelectedValue = "ND";
                //    DDLAddData.Enabled = false;
                //    ddlText.Value = "NSDL Client";
                //    ddlValue.Value = "ND";
                //    txtContact.Attributes.Add("onkeyup", "showOptions(this,'NSDLClientsGroupMember',event)");
                //}
                //else if (dtSeg.Rows[0]["Exh_ShortName"].ToString() == "CDSL")
                //{
                //    DDLAddData.SelectedValue = "CD";
                //    DDLAddData.Enabled = false;
                //    ddlText.Value = "CDSL Client";
                //    ddlValue.Value = "CD";
                //    txtContact.Attributes.Add("onkeyup", "showOptions(this,'CDSLClientsGroupMember',event)");
                //}
                //else
                //{               
                //    DDLAddData.SelectedValue = "CL";
                //    ddlText.Value = "Customers";
                //    ddlValue.Value = "CL";

                //    DDLAddData.Enabled = true;
                //    txtContact.Attributes.Add("onkeyup", "showOptions(this,'SearchGroupCustomer',event)");
                //}

                //DataTable dtSeg = oGenericMethod.GetCompanyDetail(LastCompany, userId);
                //if (dtSeg.Rows[0]["Exh_ShortName"].ToString() == "NSDL")
                //{
                //    DDLAddData.SelectedValue = "ND";
                //    DDLAddData.Enabled = false;
                //    ddlText.Value = "NSDL Client";
                //    ddlValue.Value = "ND";
                //    txtContact.Attributes.Add("onkeyup", "showOptions(this,'NSDLClientsGroupMember',event)");
                //}
                //else if (dtSeg.Rows[0]["Exh_ShortName"].ToString() == "CDSL")
                //{
                //    DDLAddData.SelectedValue = "CD";
                //    DDLAddData.Enabled = false;
                //    ddlText.Value = "CDSL Client";
                //    ddlValue.Value = "CD";
                //    txtContact.Attributes.Add("onkeyup", "showOptions(this,'CDSLClientsGroupMember',event)");
                //}
                //else
                //{               
                //    DDLAddData.SelectedValue = "CL";
                //    ddlText.Value = "Customers";
                //    ddlValue.Value = "CL";

                //    DDLAddData.Enabled = true;
                //    txtContact.Attributes.Add("onkeyup", "showOptions(this,'SearchGroupCustomer',event)");
                //}

                ViewState["GoBackTo"] = Request.UrlReferrer;
            }
            BindGrid();
            //litGpName.Text = Request.QueryString["name"].ToString();
            litGpName.Text = Convert.ToString(Request.QueryString["name"]);
            // ScriptManager.RegisterStartupScript(this, GetType(), "JScript1", "Page_Load();", true);
        }

        [WebMethod]
        public static List<string> GetgroupmasterPopUp(string reqStr, string obj4)
        {
            // Code  Added  By Priti on 14122016 to use Convert.ToString instead of ToString()
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            //DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + firstname + "%'  and con.cnt_branchid=b.branch_id");
            List<string> obj = new List<string>();



            string[] param = Convert.ToString(obj4).Split('~');


            string id = Convert.ToString(param[0]);

            DataTable dtGrp = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_MemberType,gpm_type", "gpm_id='" + id + "'");
            string MType = Convert.ToString(dtGrp.Rows[0]["gpm_MemberType"]);
            string GType = Convert.ToString(dtGrp.Rows[0]["gpm_type"]);

            string cnt_prefix = Convert.ToString(param[2]);
            DataTable dtContype = oDBEngine.GetDataTable("tbl_master_contactType", "cnttpy_contactType", "cnt_prefix='" + cnt_prefix + "'");
            string cnttpy_contactType = Convert.ToString(dtContype.Rows[0]["cnttpy_contactType"]);

            DataTable dtgroup = oDBEngine.GetDataTable("select cnt_firstName+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name,cnt_internalId  from  tbl_master_contact where cnt_contactType='" + GType + "'");


            //string cong = "";
            //string con1 = "";
            //string con2 = "";
            //string con3 = "";
            //con2 = " in (select emp_contactId from tbl_master_employee where (emp_DateofLeaving is null OR emp_DateofLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving >=getdate()) and emp_dateOfjoining <= getdate())";
            //con1 = " select cnt_firstName+isnull(cnt_middleName,'')+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']' from tbl_master_contact where cnt_internalid=";
            //switch (MType)
            //{
            //    case "Contacts":
            //        con3 = " cnt_internalid ";
            //        break;
            //    case "Addresses":
            //        con3 = " add_cntId ";
            //        break;
            //    case "Emails":
            //        con3 = " eml_cntId ";
            //        break;
            //    case "Phones":
            //        con3 = " phf_cntId ";
            //        break;
            //}
            //// switch (param[2].ToString().Trim())
            ////switch (cnttpy_contactType.ToString().Trim())
            //switch (Convert.ToString(cnttpy_contactType).Trim())
            //{
            //    //case "EM":
            //    //    cong = con3 + con2 + " and cnt_internalid like '" + param[3].ToString().Trim() + "%' and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') ";
            //    //    break;
            //    //default:
            //    //    cong = con3 + " like '" + param[3].ToString().Trim() + "%' and " + con3 + " not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "') and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')";
            //    //    break;
            //    case "EM":
            //        cong = con3 + con2 + " and cnt_internalid like '" + cnt_prefix.Trim() + "%' and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%') ";
            //        break;
            //    default:
            //        // Code  Added and Commented By Priti on 14122016 to remove the code "and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')"
            //        //cong = con3 + " like '" + cnt_prefix.Trim() + "%' and " + con3 + " not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "') and (cnt_UCC  Like '" + reqStr + "%' or cnt_firstName Like '" + reqStr + "%')";
            //        cong = con3 + " like '" + cnt_prefix.Trim() + "%' and " + con3 + " not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "')";
            //       //..........end............
            //        break;

            //}
            //Chinmoy comment start
            //if (cong != "" && con1 != "")
            //{
            //    switch (MType)
            //    {
            //        case "Contacts":
            //            DT = oDBEngine.GetDataTable("tbl_master_contact", " cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']'  as Name, cnt_InternalId as Id", "  " + cong + " and cnt_internalid not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "')   order by cnt_firstName");
            //            break;
            //        case "Addresses":
            //            DT = oDBEngine.GetDataTable("tbl_master_address", "  add_cntId as Id, ((" + con1 + " add_cntId ) + ' | ' + add_addressType + ' | ' + case when add_address1 is null then '' else add_address1 end) as Name", cong);
            //            break;
            //        case "Emails":
            //            DT = oDBEngine.GetDataTable("tbl_master_email", "  eml_cntId as Id, ((" + con1 + " eml_cntId ) + ' | ' + eml_type + ' | ' + case when eml_email is null then '' else eml_email end) as Name", cong);
            //            break;
            //        case "Phones":
            //            DT = oDBEngine.GetDataTable("tbl_master_phonefax", " phf_cntId as Id, ((" + con1 + " phf_cntId ) + ' | ' + phf_type + ' | ' + case when phf_phoneNumber is null then '' else phf_phoneNumber end) as Name", cong);
            //            break;
            //    }

            //}
            //End
            // if (param[2].ToString().Trim() == "CDSL Client")cnttpy_contactType
            //if (cnttpy_contactType.ToString().Trim() == "CDSL Client")
            //if (Convert.ToString(cnttpy_contactType).Trim() == "CDSL Client")
            //{
            //    DT = oDBEngine.GetDataTable("Master_CDSLClients", " CDSLClients_FirstHolderName as Name,CDSLClients_BOID as Id", " CDSLClients_FirstHolderName Like '" + reqStr + "%'");

            //}
            ////if (param[2].ToString().Trim() == "NSDL Client")
            ////if (cnttpy_contactType.ToString().Trim() == "NSDL Client")
            //if (Convert.ToString(cnttpy_contactType).Trim() == "NSDL Client")
            //{
            //    DT = oDBEngine.GetDataTable("Master_NSDLClients", "  NSDLClients_BenFirstHolderName as Name, (NsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10))) as Id", " NSDLClients_BenFirstHolderName Like '" + reqStr + "%'");
            //}
            //foreach (DataRow dr in DT.Rows)
            //{
            //    string[] name = Convert.ToString(dr["Name"]).Split('|');

            //    //obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            //    obj.Add(Convert.ToString(name[0]) + "|" + Convert.ToString(dr["Id"]));
            //}
            return obj;

        }
        //protected void AddMember_Click(object sender, EventArgs e)
        //{
        //    Panel1.Visible = true;
        //    GridPanel.Visible = false;
        //}




        public void BindAddMember()
        {
            string id = Convert.ToString(ViewState["id"]);
            DataSet ds = oDBEngine.PopulateData("gpm_MemberType,gpm_type", "tbl_master_groupmaster", "gpm_id=" + id);
            string MType = Convert.ToString(ds.Tables[0].Rows[0]["gpm_MemberType"]);
            ViewState["GroupType"] = Convert.ToString(ds.Tables[0].Rows[0]["gpm_type"]);

            ViewState["MType"] = Convert.ToString(MType);
            // ShowMember(MType);
        }
        #region
        //public void ShowMember(string id1)
        //{
        //    DataSet ds1 = new DataSet();
        //    try
        //    {
        //        string id = ViewState["id"].ToString();
        //        string MType = ViewState["MType"].ToString();

        //        //DataSet ds = oDBEngine.PopulateData("gpm_type", "tbl_master_groupMaster", "gpm_id=" + id);
        //        string GType = ViewState["GroupType"].ToString();
        //        string con = "", con1 = "", con2="",con3="";
        //        con2 = " in (select emp_contactId from tbl_master_employee where (emp_DateofLeaving is null OR emp_DateofLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving >=getdate()) and emp_dateOfjoining <= getdate())";
        //        con1 = " select cnt_firstName+isnull(cnt_middleName,'')+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']' from tbl_master_contact where cnt_internalid=";
        //        switch (MType)
        //        {
        //            case "Contacts":
        //                con3 = " cnt_internalid ";
        //                break;
        //            case "Addresses":
        //                con3 = " add_cntId ";
        //                break;
        //            case "Emails":
        //                con3 = " eml_cntId ";
        //                break;
        //            case "Phones":
        //                con3 = " phf_cntId ";
        //                break;
        //        }
        //        switch (DDLAddData.SelectedItem.Text.Trim())
        //        {
        //            case "EM":
        //                con = con3 + con2 + " and cnt_internalid like '" + DDLAddData.SelectedItem.Value.Trim() + "%'";
        //                break;
        //            default:
        //                con = con3 + " like '" + DDLAddData.SelectedItem.Value.Trim() + "%' and " + con3 + " not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "')";
        //                break;
        //            //case "exc":
        //            //    con = "select exh_name from tbl_master_exchange where exh_cntId=";
        //            //    con1 = "exh_cntId NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + " or grp_groupType='" + GType + "')";
        //            //    break;
        //            //case "dp":
        //            //    con = "select dp_dpName from tbl_master_depositoryParticipants where dp_cntId=";
        //            //    con1 = "dp_cntId NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + " or grp_groupType='" + GType + "')";
        //            //    break;
        //            //case "bank":
        //            //    con = "select bnk_bankName from tbl_master_Bank where bnk_internalId=";
        //            //    con1 = "bnk_internalId NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + " or grp_groupType='" + GType + "')";
        //            //    break;
        //            //case "contact":
        //            //    con = "select cnt_firstName+isnull(cnt_middleName,'')+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']' as cnt_firstName from tbl_master_contact where cnt_internalId=";
        //            //    con1 = "cnt_internalId NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + " or grp_groupType='" + GType + "')";
        //            //    break;
        //            //case "lead":
        //            //    con = "select cnt_firstName from tbl_master_lead where cnt_internalId=";
        //            //    con1 = "cnt_internalId NOT IN (SLECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + ") or grp_groupType='" + GType + "'";
        //            //    break;
        //            //case "rta":
        //            //    con = "select rta_name from tbl_registrarTransferAgent where rta_code=";
        //            //    con1 = "rta_code NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + ") or grp_groupType='" + GType + "'";
        //            //    break;
        //            //case "amc":
        //            //    con = "select amc_nameOfMutualFund from tbl_master_AssetsManagementCompanies where amc_amcCode=";
        //            //    con1 = "amc_amcCode NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + ") or grp_groupType='" + GType + "'";
        //            //    break;
        //            //case "insurance":
        //            //    con = "select insu_nameOfCompany from tbl_master_insurerName where insu_internalId=";
        //            //    con1 = "insu_internalId NOT IN (SELECT grp_contactId FROM tbl_trans_group WHERE grp_groupMaster = " + id + ") or grp_groupType='" + GType + "'";
        //            //    break;
        //        }
        //        if (con != "" && con1 != "")
        //        {
        //            switch (MType)
        //            {
        //                case "Contacts":
        //                    ds1 = oDBEngine.PopulateData("cnt_firstName+' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+'['+case cnt_contactType when 'CL' then cnt_UCC else cnt_shortName end+']'  as Name, cnt_InternalId as Id", "tbl_master_contact", con + " and cnt_internalid not in (SELECT grp_contactId FROM tbl_trans_group WHERE  grp_groupType='" + GType + "') order by cnt_firstName");
        //                    break;
        //                case "Addresses":
        //                    ds1 = oDBEngine.PopulateData("add_cntId as Id, ((" + con1 + " add_cntId ) + ' | ' + add_addressType + ' | ' + case when add_address1 is null then '' else add_address1 end) as Name", "tbl_master_address", con );
        //                    break;
        //                case "Emails":
        //                    ds1 = oDBEngine.PopulateData("eml_cntId as Id, ((" + con1 + " eml_cntId ) + ' | ' + eml_type + ' | ' + case when eml_email is null then '' else eml_email end) as Name", "tbl_master_email", con  );
        //                    break;
        //                case "Phones":
        //                    ds1 = oDBEngine.PopulateData("phf_cntId as Id, ((" + con1 + " phf_cntId ) + ' | ' + phf_type + ' | ' + case when phf_phoneNumber is null then '' else phf_phoneNumber end) as Name", "tbl_master_phonefax", con );
        //                    break;
        //            }
        //            try
        //            {
        //                if (ds1.Tables[0].Rows.Count != 0)
        //                {
        //                    LLbAddData.Items.Clear();
        //                    LLbAddData.DataTextField = "Name";
        //                    LLbAddData.DataValueField = "Id";
        //                    LLbAddData.DataSource = ds1;
        //                    LLbAddData.DataBind();
        //                }
        //                else
        //                    LLbAddData.Items.Clear();
        //            }
        //            catch
        //            {
        //                LLbAddData.Items.Clear();
        //            }
        //        }
        //        if (DDLAddData.SelectedItem.Text == "CDSL Client")
        //        {
        //            ds1 = oDBEngine.PopulateData("CDSLClients_ID as Id,CDSLClients_FirstHolderName as Name", "Master_CDSLClients", null);

        //        }
        //        if (DDLAddData.SelectedItem.Text == "NSDL Client")
        //        {
        //            ds1 = oDBEngine.PopulateData("NSDLClients_ID as Id,NSDLClients_BenFirstHolderName as Name", "Master_NSDLClients", null);
        //        }
        //        if (ds1.Tables[0].Rows.Count != 0)
        //        {
        //            LLbAddData.Items.Clear();
        //            LLbAddData.DataTextField = "Name";
        //            LLbAddData.DataValueField = "Id";
        //            LLbAddData.DataSource = ds1;
        //            LLbAddData.DataBind();
        //        }
        //        else
        //            LLbAddData.Items.Clear();
        //    }
        //    catch
        //    {
        //        LLbAddData.Items.Clear();
        //    }
        //}

        #endregion


        protected void gridEdit_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            // gridEdit.JSProperties["cpUpdatedGrid"] = null;

            if (hdngridstatus.Value == "Edit")
            {
                gridEdit.JSProperties["cpUpdatedGrid"] = "UpdateWithRefresh";
                hdngridstatus.Value = null;
            }
            else
            {
                gridEdit.JSProperties["cpUpdatedGrid"] = null;

            }

            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindEditGrid")
            {

                var grpValue = gridEdit.GetSelectedFieldValues("ContactId");

                // DataTable dtc = oDBEngine.GetDataTable("tbl_trans_group", "gpm_MemberType,gpm_type", "gpm_id='" + id + "'");

                //var unselected = allKeys.Except(selected).ToList();
                //DataTable Quotationdt = (DataTable)Session["OrderDetails"];
                //DataView dvData = new DataView(Quotationdt);

                //gridEdit.DataSource = GetSalesOrder(dvData.ToTable());
                List<object> selected = gridEdit.GetSelectedFieldValues("ContactId");
                gridEdit.Selection.SelectAll();
                List<object> allKeys = gridEdit.GetSelectedFieldValues("ContactId");
                List<object> unselected = allKeys.Except(selected).ToList();

                for (int i = 0; i < unselected.Count; i++)
                {

                    // oDBEngine.DeleteValue("tbl_trans_group", "grp_contactId=" + unselected[i]);
                    int Status = oDBEngine.ExeInteger("delete from tbl_trans_group where grp_contactId='" + unselected[i] + "'");
                }




            }
            else
            {
                gridEdit.DataBind();
            }



            

        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            e.KeyExpression = "ContactId";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dcGroup = new ERPDataClassesDataContext(connectionString);
            //if (hdngridstatus.Value == "Edit")
            //{
            //    gridEdit.JSProperties["cpUpdatedGrid"] = "UpdateWithRefresh";
            //    hdngridstatus.Value = null;
            //}
            //else
            //{
            //    gridEdit.JSProperties["cpUpdatedGrid"] = null;

            //}
            //var q = from d in dcGroup.v_GetGroupmemberLists
            //        where d.gpm_id == Convert.ToDecimal(Request.QueryString["id"])
            //        select d;

            //          e.QueryableSource = q;

            
        }

        protected void gridEdit_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = oDBEngine.GetDataTable("select * from v_GetGroupmemberList where gpm_id='" + Convert.ToDecimal(Request.QueryString["id"]) + "'");
            gridEdit.DataSource = dt;
            // gridEdit.DataBind();
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            DateTime CreateDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            int CreateUser = Convert.ToInt32(Session["userid"]);
            string id = Convert.ToString(ViewState["id"]);

            string GType = Convert.ToString(ViewState["GroupType"]);

            string cntid = Convert.ToString(txtContact_hidden.Value);

            //............code comment by Priti on 2-12-2016
            //cntid = "3";
            //if (txtContact.Text.ToString().Trim() != "")
            //{
            //    if (cntid != "")
            //    {
            //        oDBEngine.InsurtFieldValue("tbl_trans_group", "grp_contactId,grp_groupMaster,grp_groupType,CreateDate,CreateUser", "'" + cntid + "','" + id + "','" + GType + "','" + CreateDate + "','" + CreateUser + "'");
            //        Response.Redirect(ViewState["GoBackTo"].ToString());
            //        //Response.Redirect("GroupMaster.aspx");
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "JScript1", "alert('Please Select Contact!!');", true);
            //    }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "JScript17", "alert('Please Select Contact');", true);
            //}
            //cntid = "";
            //txtContact_hidden.Value = "";
            //txtContact.Text = "";
            //ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "CallGrid();", true);

            //............end..............

            //cntid = "3";
            //if (txtContact.Text.ToString().Trim() != "")
            //{
            if (cntid != "")
            {
                oDBEngine.InsurtFieldValue("tbl_trans_group", "grp_contactId,grp_groupMaster,grp_groupType,CreateDate,CreateUser", "'" + cntid + "','" + id + "','" + GType + "','" + CreateDate + "','" + CreateUser + "'");
                // oDBEngine.InsurtFieldValue("tbl_master_GroupMapping", "Group_id,Customer_internalId", "'" + id + "','" + cntid + "'");
                //Response.Redirect(Convert.ToString(ViewState["GoBackTo"]));
                //Response.Redirect("GroupMaster.aspx");
                Popup_AddGroup.ShowOnPageLoad = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript1", "JAlert('Please Select Contact!!');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript12", " callOnLoad();", true);

            }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "JScript17", "alert('Please Select Contact');", true);
            //}
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
            cntid = "";
            txtContact_hidden.Value = "";
            // txtContact.Text = "";
            //ScriptManager.RegisterStartupScript(this, GetType(), "JScript19", "CallGrid();", true);
        }
        public void BindGrid()
        {
            string id = Request.QueryString["id"];
            string aa = Request.QueryString["type"];
            if (id != null)
            {
                ViewState["id"] = Convert.ToString(id);
                BindAddMember();
                //GridPanel.Visible = true;
                if (Request.QueryString["type"] == "CDSL Accounts")
                {
                    SelectName.SelectCommand = "select (select CDSLClients_FirstHolderName from Master_CDSLClients where cast(CDSLClients_BOID as varchar(20))=tbl_trans_group.grp_contactId) as Name,grp_contactId as Id,grp_id from tbl_trans_group where grp_groupMaster=" + id;
                }
                else if (Request.QueryString["type"] == "NSDL Accounts")
                {
                    SelectName.SelectCommand = "select (select NSDLClients_BenFirstHolderName from Master_NSDLClients where (NsdlClients_DPID+ cast(NsdlClients_BenAccountID as varchar(10)))=tbl_trans_group.grp_contactId) as Name,grp_contactId as Id,grp_id from tbl_trans_group where grp_groupMaster=" + id;
                }
                else   //Contact
                {
                    if (DDLAddData.SelectedItem.Text == "CDSL Client")
                        SelectName.SelectCommand = "Select (LTRIM(rtrim(CdslClients_FirstHolderName))+' '+LTRIM(rtrim(CdslClients_FirstHolderMiddleName))+' '+LTRIM(rtrim(CdslClients_FirstHolderLastName))+' ['+cast(ltrim(rtrim(right(CdslClients_BOID,8))) as varchar(10))+']') as Name,cast(ltrim(rtrim(right(CdslClients_BOID,8))) as varchar(10)) as Id,grp_id From (Select gpm_code,gpm_Type,grp_contactId,grp_id from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type=(select gpm_Type from tbl_master_groupMaster where gpm_id=" + id + ")) T1 inner join (Select * from Master_CdslClients Where ltrim(rtrim(right(CdslClients_BOID,8))) ='" + Session["usersegid"].ToString() + "' ) T2 On CdslClients_BOID=grp_contactId and gpm_code=(select gpm_code from tbl_master_groupMaster where gpm_id=" + id + ")";
                    else if (DDLAddData.SelectedItem.Text == "NSDL Client")
                        SelectName.SelectCommand = "Select (LTRIM(rtrim(NsdlClients_BenFirstHolderName))+' ['+cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10))+']') as Name,cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10)) as Id,grp_id From (Select gpm_code,gpm_Type,grp_contactId,grp_id from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type=(select gpm_Type from tbl_master_groupMaster where gpm_id=" + id + ")) T1 inner join (Select * from Master_NsdlClients Where NsdlClients_DPID='" + Session["usersegid"].ToString() + "') T2 On ltrim(rtrim(NsdlClients_DPID))+cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10))=grp_contactId and gpm_code=(select gpm_code from tbl_master_groupMaster where gpm_id=" + id + ")";
                    else
                        SelectName.SelectCommand = "select (ltrim(rtrim(ISNULL(cnt_firstName, ''))) + ' ' + ltrim(rtrim(ISNULL(cnt_middleName, ''))) + ' ' + ltrim(rtrim(ISNULL(cnt_lastName, '')))+'['+case cnt_contactType when 'CL' then ltrim(rtrim(isnull(cnt_UCC,''))) else ltrim(rtrim(isnull(cnt_shortName,''))) end+']') as Name,cnt_internalId as Id,grp_id from tbl_trans_group INNER JOIN tbl_master_contact ON tbl_trans_group.grp_contactId = tbl_master_contact.cnt_internalId where grp_groupMaster=" + id + " order By tbl_trans_group.createDate";
                }
                GridName.DataBind();
            }
        }
        protected void DDLAddData_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlText.Value = Convert.ToString(DDLAddData.SelectedItem.Text).Trim();
            ddlValue.Value = Convert.ToString(DDLAddData.SelectedItem.Value).Trim();
            // string id = ViewState["MType"].ToString();
            //  ShowMember(id);
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            //GridPanel.Visible = true;
            //Panel1.Visible = false;
            string id = Request.QueryString["id"];
            if (Convert.ToString(Request.QueryString["type"]) == "CDSL Accounts")
            {
                SelectName.SelectCommand = "select (select CDSLClients_FirstHolderName from Master_CDSLClients where cast(CDSLClients_ID as varchar(20))=tbl_trans_group.grp_contactId) as Name,grp_contactId as Id,grp_id from tbl_trans_group where grp_groupMaster=" + id;
            }
            else if (Convert.ToString(Request.QueryString["type"]) == "NSDL Accounts")
            {
                SelectName.SelectCommand = "select (select NSDLClients_BenFirstHolderName from Master_NSDLClients where cast(NSDLClients_ID as varchar(20))=tbl_trans_group.grp_contactId) as Name,grp_contactId as Id,grp_id from tbl_trans_group where grp_groupMaster=" + id;
            }
            else
            {
                if (DDLAddData.SelectedItem.Text == "CDSL Client")
                    SelectName.SelectCommand = "Select (LTRIM(rtrim(CdslClients_FirstHolderName))+' '+LTRIM(rtrim(CdslClients_FirstHolderMiddleName))+' '+LTRIM(rtrim(CdslClients_FirstHolderLastName))+' ['+cast(ltrim(rtrim(right(CdslClients_BOID,8))) as varchar(10))+']') as Name,cast(ltrim(rtrim(right(CdslClients_BOID,8))) as varchar(10)) as Id,grp_id From (Select gpm_code,gpm_Type,grp_contactId,grp_id from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type=(select gpm_Type from tbl_master_groupMaster where gpm_id=" + id + ")) T1 inner join (Select * from Master_CdslClients Where ltrim(rtrim(right(CdslClients_BOID,8))) ='" + Session["usersegid"].ToString() + "' and CdslClients_ContactID is not null) T2 On CdslClients_BOID=grp_contactId and gpm_code=(select gpm_code from tbl_master_groupMaster where gpm_id=" + id + ")";
                else if (DDLAddData.SelectedItem.Text == "NSDL Client")
                    SelectName.SelectCommand = "Select (LTRIM(rtrim(NsdlClients_BenFirstHolderName))+' ['+cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10))+']') as Name,cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10)) as Id,grp_id From (Select gpm_code,gpm_Type,grp_contactId,grp_id from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and gpm_Type=(select gpm_Type from tbl_master_groupMaster where gpm_id=" + id + ")) T1 inner join (Select * from Master_NsdlClients Where NsdlClients_DPID='" + Session["usersegid"].ToString() + "' and NsdlClients_ContactID is not null) T2 On ltrim(rtrim(NsdlClients_DPID))+cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10))=grp_contactId and gpm_code=(select gpm_code from tbl_master_groupMaster where gpm_id=" + id + ")";
                else
                    SelectName.SelectCommand = "select ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') as Name,cnt_internalId as Id,grp_id from tbl_trans_group INNER JOIN tbl_master_contact ON tbl_trans_group.grp_contactId = tbl_master_contact.cnt_internalId where grp_groupMaster=" + id;
            }
            // txtContact.Text = "";
            txtContact_hidden.Value = "";
            Response.Redirect(ViewState["GoBackTo"].ToString());
            //ScriptManager.RegisterStartupScript(this, GetType(), "JScript41", "parent.editwin.close();", true);

        }

        protected void GridName_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            GridName.DataBind();
            BindGrid();

        }


        protected void GridName_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string Code = Convert.ToString(e.Values[2]);
            string[,] Count = new string[2, 0];
            Count = oDBEngine.GetFieldValue("tbl_trans_group", "grp_id", "grp_id=" + Code, 1);
            if (Count[0, 0] != "n")
            {
                oDBEngine.DeleteValue("tbl_trans_group", "grp_id=" + Code);
            }

            //e.Cancel = true;
            GridName.DataBind();
            BindGrid();
            e.Cancel = true;

            //Response.Redirect("groupmasterPopUp.aspx?id=" + Request.QueryString["id"] + "type=" + Convert.ToString(Request.QueryString["type"]) + "name=" + Convert.ToString(Request.QueryString["name"]), false);
            //string id = Request.QueryString["id"];
            //string aa = Convert.ToString(Request.QueryString["type"]);
            //Convert.ToString(Request.QueryString["name"])
        }

        protected void goBackCrossBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("GroupMaster.aspx");
            //try
            //{
            //    if (ViewState["GoBackTo"] != null)
            //    {
            //        Response.Redirect(ViewState["GoBackTo"].ToString());
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }
    }
}