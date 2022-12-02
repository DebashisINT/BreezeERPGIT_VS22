using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace ERP.OMS.Management.Master
{
    public partial class IndustryMap : ERP.OMS.ViewState_class.VSPage //SSystem.Web.UI.Page
    {
        string AllUserCntId;
        SqlConnection oSqlConnection = new SqlConnection();
        DataTable DT = new DataTable();
        DataTable DTAvailable = new DataTable();
        DataTable DTChoosen = new DataTable();
        //SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        SqlConnection con = new SqlConnection();
        IndustryMap_BL obj = new IndustryMap_BL();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindEntityList();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["iname"]))
                {
                    ltrName.Text = '(' + Convert.ToString(Request.QueryString["iname"]) + ')';

                }
                Session.Remove("SelectedItemList");
              
            }

        }

        public void BindEntityList()
        {
            //Entity type "14" has been used for Product which is select from "tbl_Entity". Comment by Debashis Talukder on 30/04/2019
            DT = obj.BindEntityList();

            if (DT != null && DT.Rows.Count > 0)
            {
                cmbContactType.DataSource = DT;
                cmbContactType.ValueField = "Id";
                cmbContactType.TextField = "EntityName";

                cmbContactType.DataBind();
            }

        }
        protected void cmbContactType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Debjyoti 030217
            Session.Remove("SelectedItemList");
            //Debjyoti 030217

            int IndustryId = 0;
            IndustryId = Convert.ToInt32(Request.QueryString["Id"]);
            Int32 Filter = int.Parse(cmbContactType.SelectedItem.Value.ToString());

            Session["FilterId"] = cmbContactType.SelectedItem.Value.ToString();
            switch (Filter)
            {
                case 1:
                    DTAvailable = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();

                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and tbl_master_contact.cnt_internalId in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
                    List<string> obj1 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj1.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj1.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }



                    break;
                case 2:
                    DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj2 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj2.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj2.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 3:
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
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') AND tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                    List<string> obj3 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj3.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj3.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }


                    break;
                case 4:
                    DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj4 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj4.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj4.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 5:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj5 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj5.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj5.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 6:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj6 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj6.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj6.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 7:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj7 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj7.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj7.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 8:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj8 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj8.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj8.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }


                    break;
                case 9:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj9 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj9.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj9.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 10:
                    DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj10 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj10.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj10.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 11:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj11 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj11.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj11.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 12:
                    DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "Id";
                    lbAvailable.TextField = "Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                    List<string> obj12 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj12.Add(Convert.ToString(dr["Id"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj12.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 13:
                    DTAvailable = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B     ");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "ProductClass_Code";
                    lbAvailable.TextField = "ProductClass_Name";

                    lbAvailable.DataBind();
                    Cache["DTAvailableList"] = DTAvailable;
                    DTChoosen = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B where   ProductClass_Code  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') ");
                    List<string> obj13 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj13.Add(Convert.ToString(dr["ProductClass_Code"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj13.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }

                    break;
                case 14:
                    DTAvailable = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID ");
                    lbAvailable.DataSource = DTAvailable;
                    lbAvailable.ValueField = "sProducts_Code";
                    lbAvailable.TextField = "sProducts_Name";
                    Cache["DTAvailableList"] = DTAvailable;
                    lbAvailable.DataBind();
                    DTChoosen = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where MP.sProducts_Code   in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')");
                    List<string> obj14 = new List<string>();

                    foreach (DataRow dr in DTChoosen.Rows)
                    {

                        obj14.Add(Convert.ToString(dr["sProducts_Code"]));
                    }

                    for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                    {
                        if (obj14.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                            lbAvailable.Items[i].Selected = true;

                    }
                    break;
                default:
                    break;
            }

            SortSelectedItem();
            SetSelectedListItem();
        }

        public void SetSelectedListItem()
        {
            if(lbAvailable.Items.Count>0)
            {
            List<string> selectedProd = new List<string>();
            if (Session["SelectedItemList"] != null)
            {
                selectedProd = (List<string>)Session["SelectedItemList"];
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

            Session["SelectedItemList"] = selectedProd;
            }
        }


        public void GetSelectedListItem()
        {
            if(lbAvailable.Items.Count>0)
        { 
            List<string> selectedProd = new List<string>();
            
            if (Session["SelectedItemList"] != null)
            {
                selectedProd = (List<string>)Session["SelectedItemList"];

            }
                for (int i = 0; i <lbAvailable.Items.Count; i++)
                {
                    if (selectedProd.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                    {
                        lbAvailable.Items[i].Selected = true;
                    }
                    else
                    {
                        lbAvailable.Items[i].Selected = false;
                    }
                }

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
         
            int IndustryId = 0;
            Int32 Filter = int.Parse(Convert.ToString(Session["FilterId"]));
          
            IndustryId = Convert.ToInt32(Request.QueryString["Id"]);
           // SetSelectedListItem();
            if (!String.IsNullOrEmpty(txtAvailable.Text.Trim()))
            {


                switch (Filter)
                {
                    case 1:
                        DTAvailable = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate())   and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%' ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();

                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and tbl_master_contact.cnt_internalId in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  ");
                        List<string> obj1 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj1.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj1.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }



                        break;
                    case 2:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%'  and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj2 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj2.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj2.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 3:
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
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  ) as D order by CrDate desc ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') AND tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  ) as D order by CrDate desc ");
                        List<string> obj3 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj3.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj3.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 4:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj4 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj4.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj4.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 5:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj5 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj5.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj5.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 6:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj6 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj6.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj6.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 7:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj7 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj7.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj7.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 8:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj8 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj8.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj8.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 9:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj9 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj9.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj9.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 10:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%'  and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj10 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj10.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj10.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 11:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj11 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj11.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj11.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 12:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj12 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj12.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj12.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 13:
                        DTAvailable = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B  where B.[ProductClass_Name] like '%" + txtAvailable.Text.Trim() + "%'    ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "ProductClass_Code";
                        lbAvailable.TextField = "ProductClass_Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B where   ProductClass_Code  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "' and B.[ProductClass_Name] like '%" + txtAvailable.Text.Trim() + "%' ) ");
                        List<string> obj13 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj13.Add(Convert.ToString(dr["ProductClass_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj13.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 14:
                        DTAvailable = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where  MP.sProducts_Name like '%" + txtAvailable.Text.Trim() + "%' ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "sProducts_Code";
                        lbAvailable.TextField = "sProducts_Name";
                        Cache["DTAvailableList"] = DTAvailable;
                        lbAvailable.DataBind();
                        DTChoosen = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where MP.sProducts_Code   in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "' and MP.sProducts_Name like '%" + txtAvailable.Text.Trim() + "%')");
                        List<string> obj14 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj14.Add(Convert.ToString(dr["sProducts_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj14.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }
                        break;
                    default:
                        break;
                }
              
            }
            else
            {

                switch (Filter)
                {
                    case 1:
                        DTAvailable = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();

                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and tbl_master_contact.cnt_internalId in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
                        List<string> obj1 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj1.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj1.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }



                        break;
                    case 2:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj2 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj2.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj2.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 3:
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
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') AND tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                        List<string> obj3 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj3.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj3.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 4:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj4 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj4.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj4.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 5:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj5 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj5.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj5.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 6:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj6 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj6.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj6.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 7:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj7 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj7.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj7.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 8:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj8 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj8.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj8.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 9:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj9 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj9.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj9.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 10:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj10 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj10.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj10.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 11:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj11 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj11.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj11.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 12:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj12 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj12.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj12.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 13:
                        DTAvailable = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B     ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "ProductClass_Code";
                        lbAvailable.TextField = "ProductClass_Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B where   ProductClass_Code  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') ");
                        List<string> obj13 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj13.Add(Convert.ToString(dr["ProductClass_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj13.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 14:
                        DTAvailable = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "sProducts_Code";
                        lbAvailable.TextField = "sProducts_Name";
                        Cache["DTAvailableList"] = DTAvailable;
                        lbAvailable.DataBind();
                        DTChoosen = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where MP.sProducts_Code   in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')");
                        List<string> obj14 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj14.Add(Convert.ToString(dr["sProducts_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj14.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }
                        break;
                    default:
                        break;
                }
            }
            //else { BindAvaibleIndustry(""); }

            //Debjyoti 030217
            GetSelectedListItem();
            //SetSelectedListItem();
            //SortSelectedItem();
            SortSelectedItem();
            SetSelectedListItem();
        }
        protected void btncancel_click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("industrySector.aspx", false);
            }
            catch { }
        }

        protected void btnsubmit_click(object sender, EventArgs e)
        {
            try
            {
                //bellow Line is for Clear The all filter
             //   txtAvailable.Text = "";
            //  txtAvailable_TextChanged(txtAvailable, null);
                //Filter End Here
            SetSelectedListItem();

                string msg = string.Empty;
                int EntityTypeId = 0;
                string EntityIds = string.Empty;
                int IndustryId = 0;
                string variable = string.Empty;
                EntityTypeId = int.Parse(cmbContactType.SelectedItem.Value.ToString());
                IndustryId = Convert.ToInt32(Request.QueryString["Id"]);

                //foreach (ListEditItem item in ((ASPxListBox)lbAvailable).SelectedItems)
                //{

                //    variable = item.Value.ToString() + ',' + variable;

                //}

                List<string> selectedProd = new List<string>();

                if (Session["SelectedItemList"] != null)
                {
                    selectedProd = (List<string>)Session["SelectedItemList"];

                }


                foreach (string value in selectedProd)
                {


                    variable = value + ',' + variable;
                }

                if (variable.Length > 0)
                {
                    EntityIds = variable.Substring(0, variable.Length - 1);
                }

                //if (EntityIds=="")
                //{

                //    Page.ClientScript.RegisterStartupScript(GetType(), "Error", "<script>Error();</script>");
                //    return;
                //}
                msg = obj.InsertIndustryMap_BL(EntityTypeId, EntityIds, IndustryId);




                BindAvailable();
            }
            catch { }
        }




        public void BindAvailable()
        {
          //  Session.Remove("SelectedItemList");
            int IndustryId = 0;
            Int32 Filter = int.Parse(Convert.ToString(Session["FilterId"]));

            IndustryId = Convert.ToInt32(Request.QueryString["Id"]);
            // SetSelectedListItem();
            if (!String.IsNullOrEmpty(txtAvailable.Text.Trim()))
            {


                switch (Filter)
                {
                    case 1:
                        DTAvailable = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate())   and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%' ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();

                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and tbl_master_contact.cnt_internalId in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  ");
                        List<string> obj1 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj1.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj1.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }



                        break;
                    case 2:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%'  and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj2 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj2.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj2.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 3:
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
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  ) as D order by CrDate desc ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') AND tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  ) as D order by CrDate desc ");
                        List<string> obj3 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj3.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj3.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 4:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj4 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj4.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj4.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 5:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj5 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj5.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj5.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 6:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj6 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj6.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj6.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 7:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj7 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj7.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj7.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 8:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj8 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj8.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj8.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 9:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'   and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj9 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj9.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj9.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 10:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%'  and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj10 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj10.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj10.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 11:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj11 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj11.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj11.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 12:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')  +'['+cnt_shortName+']' like '%" + txtAvailable.Text.Trim() + "%'  AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj12 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj12.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj12.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 13:
                        DTAvailable = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B  where B.[ProductClass_Name] like '%" + txtAvailable.Text.Trim() + "%'    ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "ProductClass_Code";
                        lbAvailable.TextField = "ProductClass_Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B where   ProductClass_Code  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "' and B.[ProductClass_Name] like '%" + txtAvailable.Text.Trim() + "%' ) ");
                        List<string> obj13 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj13.Add(Convert.ToString(dr["ProductClass_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj13.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 14:
                        DTAvailable = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where  MP.sProducts_Name like '%" + txtAvailable.Text.Trim() + "%' ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "sProducts_Code";
                        lbAvailable.TextField = "sProducts_Name";
                        Cache["DTAvailableList"] = DTAvailable;
                        lbAvailable.DataBind();
                        DTChoosen = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where MP.sProducts_Code   in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "' and MP.sProducts_Name like '%" + txtAvailable.Text.Trim() + "%')");
                        List<string> obj14 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj14.Add(Convert.ToString(dr["sProducts_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj14.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }
                        break;
                    default:
                        break;
                }

            }
            else
            {

                switch (Filter)
                {
                    case 1:
                        DTAvailable = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();

                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("tbl_master_employee,tbl_master_contact,tbl_trans_employeeCTC ", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId and tbl_master_contact.cnt_internalId in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')  and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) ");
                        List<string> obj1 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj1.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj1.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }



                        break;
                    case 2:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'LD%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj2 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj2.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj2.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 3:
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
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE, (select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select top 1 ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_UCC as Code,(select top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where ( tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') AND tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") or tbl_master_contact.cnt_RelationShipManager in ('" + AllUserCntId + "') or tbl_master_contact.cnt_salesrepresentative in ('" + AllUserCntId + "')) and cnt_internalId like 'CL%' ) as D order by CrDate desc ");
                        List<string> obj3 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj3.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj3.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 4:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj4 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj4.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj4.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 5:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'FR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj5 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj5.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj5.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 6:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join  tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'PR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj6 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj6.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj6.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 7:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CS%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj7 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj7.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj7.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 8:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'SH%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj8 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj8.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj8.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }


                        break;
                    case 9:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'DR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj9 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj9.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj9.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 10:
                        DTAvailable = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%'  and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>''  and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'CR%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj10 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj10.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj10.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 11:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select  top 1 ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'RA%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj11 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj11.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj11.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 12:
                        DTAvailable = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "Id";
                        lbAvailable.TextField = "Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("select  * from(select tbl_master_contact.cnt_id AS cnt_Id,'' as CRG_TCODE,(select top 1 crg_number  from tbl_master_contactRegistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalId) as PanNumber,tbl_master_contact.cnt_internalId AS Id,(select top 1 ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' +ISNULL(phf_phoneNumber,'') from tbl_master_phonefax where phf_phoneNumber is not null and LTRIM(RTRIM(phf_phoneNumber)) <>'' and tbl_master_contact.cnt_internalid=phf_cntId) as phf_phoneNumber,(select top 1  ISNULL(eml_email,'') from tbl_master_email where eml_email is not null and LTRIM(RTRIM(eml_email)) <>'' and ltrim(rtrim(eml_type))='official' and tbl_master_contact.cnt_internalid=eml_cntId) as eml_email,(select ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') +'['+con.cnt_shortname+']' from tbl_master_contact con,tbl_master_contact con1 where con.cnt_internalId=con1.cnt_referedBy and con1.cnt_internalId=tbl_master_contact.cnt_internalId) AS Reference,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,tbl_master_contact.cnt_shortName as Code,(select ISNULL(contact.cnt_firstName, '') + ' ' + ISNULL(contact.cnt_middleName, '') + ' ' + ISNULL(contact.cnt_lastName, '') +'['+contact.cnt_shortname+']' AS Name from tbl_master_contact contact,tbl_trans_contactInfo info where contact.cnt_internalId=info.Rep_partnerid and info.cnt_internalId=tbl_master_contact.cnt_internalId and info.ToDate is null) as RM,case tbl_master_contact.cnt_Lead_Stage when 1 then 'Lead' when 2 then 'Opportunity' when 3 then 'Sales/Pipeline' when 4 then 'Converted' when 5 then 'Lost' End as Status,tbl_master_contactstatus.cntstu_contactStatus,case when tbl_master_contact.lastmodifydate is null then  tbl_master_contact.createdate else tbl_master_contact.lastmodifydate end as CrDate from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id  inner join tbl_master_contactstatus on tbl_master_contact.cnt_contactStatus=tbl_master_contactstatus.cntstu_id where  cnt_internalId like 'XC%' AND tbl_master_contact.cnt_internalId  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') and tbl_master_contact.cnt_branchid in(" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as D order by CrDate desc");
                        List<string> obj12 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj12.Add(Convert.ToString(dr["Id"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj12.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 13:
                        DTAvailable = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B     ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "ProductClass_Code";
                        lbAvailable.TextField = "ProductClass_Name";

                        lbAvailable.DataBind();
                        Cache["DTAvailableList"] = DTAvailable;
                        DTChoosen = oDBEngine.GetDataTable("SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime] FROM [Master_ProductClass] B where   ProductClass_Code  in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "') ");
                        List<string> obj13 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj13.Add(Convert.ToString(dr["ProductClass_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj13.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }

                        break;
                    case 14:
                        DTAvailable = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID ");
                        lbAvailable.DataSource = DTAvailable;
                        lbAvailable.ValueField = "sProducts_Code";
                        lbAvailable.TextField = "sProducts_Name";
                        Cache["DTAvailableList"] = DTAvailable;
                        lbAvailable.DataBind();
                        DTChoosen = oDBEngine.GetDataTable("SELECT MP.sProducts_Code,MP.sProducts_Name	FROM Master_sProducts MP left join Master_ProductClass MPC on MP.ProductClass_Code=MPC.ProductClass_ID where MP.sProducts_Code   in(select IndustryMap_EntityID from Master_IndustryMap where IndustryMap_IndustryID='" + IndustryId + "' and IndustryMap_EntityType='" + Filter + "')");
                        List<string> obj14 = new List<string>();

                        foreach (DataRow dr in DTChoosen.Rows)
                        {

                            obj14.Add(Convert.ToString(dr["sProducts_Code"]));
                        }

                        for (int i = lbAvailable.Items.Count - 1; i >= 0; i--)
                        {
                            if (obj14.Contains(Convert.ToString(lbAvailable.Items[i].Value)))
                                lbAvailable.Items[i].Selected = true;

                        }
                        break;
                    default:
                        break;
                }
            }
            //else { BindAvaibleIndustry(""); }

            //Debjyoti 030217
            SetSelectedListItem();
            GetSelectedListItem();
            //SetSelectedListItem();
            //SortSelectedItem();
            SortSelectedItem();
            
        }
    }
}