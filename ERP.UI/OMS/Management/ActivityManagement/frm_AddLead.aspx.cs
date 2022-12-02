using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frm_AddLead : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList drp = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDown();
                FillGrid(false, true);
                string today = ObjConvert.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
                ASPxDateFrom.EditFormatString = ObjConvert.GetDateFormat("Date");
                ASPxDateTo.EditFormatString = ObjConvert.GetDateFormat("Date");
                ASPxDateFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                ASPxDateTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                CheckBoxContainer.Visible = false;
            }
        }
        public void FillGrid(bool bVal, bool bVal1)
        {
            //change table name tbl_master_lead to tbl_master_contact 13/12/2016
            try
            {
                DataTable dt = new DataTable();
                string branchId = oDBEngine.getBranch(HttpContext.Current.Session["userbranchID"].ToString(), "");
                branchId = branchId + HttpContext.Current.Session["userbranchID"].ToString();
                string str = "";
                if (bVal == false)
                {
                    switch (Request.QueryString["Call"].ToString())
                    {
                        case "PhoneCall":
                            lblTitle.Text = "Lead Data";
                            chkLead.Checked = true;
                            chkPhoneCall.Visible = false;
                            chkSalesVisit.Visible = false;
                            btnAddLead.Visible = false;
                            btnAddPhoneCall.Visible = false;
                            dt = oDBEngine.GetDataTable("tbl_master_contact", "top 0 cnt_internalId AS id, ISNULL(cnt_firstName, '') + ISNULL(cnt_middleName, '') AS name, '' AS Phoneid, '' AS NextVisitDate, CASE  (SELECT top 1 isnull(add_Activityid, '')   FROM          tbl_master_address    WHERE      add_cntid = cnt_internalid) WHEN '' THEN   (SELECT     TOP 1 isnull(add_address1, '')    FROM          tbl_master_address     WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS Address1, CASE    (SELECT top 1 isnull(add_Activityid, '')    FROM          tbl_master_address   WHERE      add_cntid = cnt_internalid) WHEN '' THEN  (SELECT     TOP 1 isnull(add_address2, '')   FROM          tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS Address2, CASE    (SELECT   top 1 isnull(add_Activityid, '')     FROM          tbl_master_address    WHERE      add_cntid = cnt_internalid) WHEN '' THEN     (SELECT     TOP 1 isnull(add_address3, '')   FROM          tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS Address3, CASE   (SELECT top 1 isnull(add_Activityid, '')   FROM          tbl_master_address    WHERE      add_cntid = cnt_internalid) WHEN '' THEN  (SELECT     TOP 1 isnull(add_City, '')   FROM          tbl_master_address   WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS City, CASE  (SELECT  top 1 isnull(add_Activityid, '')    FROM          tbl_master_address   WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_Pin, '')  FROM          tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS Pin, CASE  (SELECT  top 1 isnull(add_Activityid, '')   FROM          tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN    (SELECT     TOP 1 isnull(add_Landmark, '')    FROM          tbl_master_address   WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS LandMark ,'' AS ProdcuctType, '' AS Productid,'' as PrdId , 0 as Amount", "cnt_status='due' and cnt_branchid in(" + branchId + ") order by NextVisitDate, Address3");
                            break;
                        case "SalesVisit":
                            lblTitle.Text = "Phone Call Data";
                            chkSalesVisit.Visible = false;
                            chkPhoneCall.Checked = false;
                            dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN   tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN  tbl_trans_offeredProduct ON tbl_trans_offeredProduct.ofp_actId = tbl_trans_phonecall.phc_activityId AND    tbl_trans_offeredProduct.ofp_leadId = tbl_trans_phonecall.phc_leadcotactId", "tbl_master_contact.cnt_internalId AS Id, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') AS Name,   tbl_trans_phonecall.phc_id AS phoneid, tbl_trans_phonecall.phc_nextCall as NextVisitDate, CASE    (SELECT   TOP 1  isnull(add_Activityid, '')  FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN    (SELECT TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_address1, '')  FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address1, CASE (SELECT  TOP 1   isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE  (SELECT   TOP 1  isnull(add_address2, '') FROM  tbl_master_address  WHERE add_activityid  = phc_id AND add_cntid = phc_leadcotactId) END AS Address2, CASE (SELECT   TOP 1  isnull(add_Activityid, '') FROM tbl_master_address   WHERE add_cntid = cnt_internalid) WHEN '' THEN    (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE   (SELECT   TOP 1  isnull(add_address3, '') FROM  tbl_master_address  WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address3, CASE  (SELECT   TOP 1  isnull(add_Activityid, '')  FROM tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_City, '') FROM tbl_master_address   WHERE add_cntid = cnt_internalid) ELSE    (SELECT   TOP 1  isnull(add_city, '') FROM  tbl_master_address  WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS City, CASE (SELECT  TOP 1   isnull(add_Activityid, '') FROM tbl_master_address   WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address   WHERE      add_cntid = cnt_internalid) ELSE   (SELECT  TOP 1 isnull(add_pin, '')   FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Pin, CASE (SELECT  TOP 1 isnull(add_Activityid, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN   (SELECT     TOP 1 isnull(add_Landmark, '') FROM tbl_master_address   WHERE add_cntid = cnt_internalid) ELSE (SELECT TOP 1    isnull(add_landmark, '')  FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS LandMark , isnull(tbl_trans_offeredProduct.ofp_productTypeId, '')  AS ProdcuctType, CASE ISNULL(tbl_trans_offeredProduct.ofp_productId, '0') WHEN '0' THEN ISNULL(tbl_trans_offeredProduct.ofp_productTypeId, '') ELSE (SELECT     prds_description FROM tbl_master_products WHERE      (prds_internalId = tbl_trans_offeredProduct.ofp_productId)) END Productid, isnull(tbl_trans_offeredProduct.ofp_id, '') AS PrdId, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "tbl_trans_phonecall.phc_callDispose = 9  AND tbl_trans_phonecall.phc_branchId IN (" + branchId + ") And left(tbl_trans_offeredProduct.ofp_actId,2)<>'SL' and (phc_NextActivityId is null) order by NextVisitDate, Address3");
                            break;
                        case "Sales":
                            lblTitle.Text = "Sales Visit Data";
                            chkSalesVisit.Checked = true;
                            dt = oDBEngine.GetDataTable("tbl_trans_salesVisit INNER JOIN    tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN   tbl_trans_offeredProduct ON tbl_master_contact.cnt_internalId = tbl_trans_offeredProduct.ofp_leadId AND   tbl_trans_offeredProduct.ofp_id = tbl_trans_salesVisit.slv_productoffered", "tbl_master_contact.cnt_internalId as Id, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, tbl_trans_salesVisit.slv_id AS Phoneid, tbl_trans_salesVisit.slv_nextvisitdatetime AS NextVisitDate, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address1, '') FROM tbl_master_address   WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address1, '') FROM tbl_master_address  WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address1, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address2, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address2, '') FROM tbl_master_address  WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address2, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address   WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address  WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address3, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_City, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_city, '') FROM tbl_master_address  WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS City, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_pin, '')  FROM tbl_master_address  WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Pin, CASE  (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_Landmark, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_landmark, '')  FROM tbl_master_address  WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS LandMark , tbl_trans_offeredProduct.ofp_productTypeId AS ProdcuctType,  ISNULL(tbl_trans_offeredProduct.ofp_productId, '') AS ProductId, tbl_trans_offeredProduct.ofp_id AS Prdid, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "LEFT(tbl_trans_offeredProduct.ofp_actId, 2) <> 'SL' AND tbl_trans_salesVisit.slv_salesvisitoutcome = 8 AND tbl_master_contact.cnt_branchid IN (" + branchId + ") order by NextVisitDate, Address3");
                            break;
                    }

                }
                else
                {
                    if (bVal1 == true)
                    {
                        str = "tbl_master_contact.cnt_firstname like '%" + txtNameSerach.Text + "%'";
                    }
                    else
                    {
                        str += "";
                        if (txtCon1.Text != "")
                        {
                            str = "(tbl_master_address.add_address1 like '%" + txtCon1.Text + "%'";
                            str += " Or tbl_master_address.add_address2 like '%" + txtCon1.Text + "%'";
                            str += " Or tbl_master_address.add_address3 like '%" + txtCon1.Text + "%'";
                            str += " Or tbl_master_address.add_city like '%" + txtCon1.Text + "%'";
                            str += " Or tbl_master_address.add_pin like '%" + txtCon1.Text + "%'";
                            str += " ) ";
                        }
                        if (txtCon2.Text != "")
                        {
                            if (str != "")
                            {
                                str += drpAndOr1.Text;
                            }
                            str += "(tbl_master_address.add_address1 like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_address2 like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_address3 like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_city like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_pin like '%" + txtCon2.Text + "%'";
                            str += " ) ";
                        }
                        if (txtCon3.Text != "")
                        {
                            if (str != "")
                            {
                                str += drpAndOr2.Text;
                            }
                            str += "(tbl_master_address.add_address1 like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_address2 like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_address3 like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_city like '%" + txtCon2.Text + "%'";
                            str += " Or tbl_master_address.add_pin like '%" + txtCon2.Text + "%'";
                            str += " ) ";
                        }
                        if (drpUser.SelectedValue != "0")
                        {
                            if (str != "")
                            {
                                str += drp_Cond_User.SelectedItem.Text.ToString();
                            }
                            str += " tbl_master_contact.createuser =" + drpUser.SelectedValue;
                        }
                        if (drpSourceType.SelectedValue != "0")
                        {
                            if (str != "")
                            {
                                str += drp_Cond_SourceType.SelectedValue;
                            }
                            str += " tbl_master_contact.cnt_contactsource = " + drpSourceType.SelectedValue + " AND  tbl_master_contact.cnt_referedby = '" + drpReferBy.SelectedValue + "'";
                        }

                        str += "" + drp_Cond_CrateDate.Text.ToString() + "" + " " + "tbl_master_contact.CreateDate>='" + ASPxDateFrom.Value + "' and tbl_master_contact.CreateDate<='" + ASPxDateTo.Value + "'";
                    }
                    if (chkLead.Checked == true)
                    {
                        lblTitle.Text = "Lead Data";
                        if (str == "")
                        {
                            dt = oDBEngine.GetDataTable("tbl_master_contact inner join tbl_master_address on tbl_master_contact.cnt_internalid=tbl_master_address.add_cntid", "top " + txtNoCont.Text + " cnt_internalId AS id, ISNULL(cnt_firstName, '') + ISNULL(cnt_middleName, '') AS name, '' AS Phoneid, '' AS NextVisitDate, CASE  (SELECT top 1 isnull(add_Activityid, '')   FROM  tbl_master_address    WHERE      add_cntid = cnt_internalid) WHEN '' THEN   (SELECT     TOP 1 isnull(add_address1, '')    FROM          tbl_master_address     WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS Address1, CASE    (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Address2, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address3, '')FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Address3, CASE   (SELECT top 1 isnull(add_Activityid, '')   FROM tbl_master_address WHERE  add_cntid = cnt_internalid) WHEN '' THEN  (SELECT TOP 1 isnull(add_City, '')   FROM  tbl_master_address   WHERE add_cntid = cnt_internalid) ELSE '0' END AS City, CASE  (SELECT  top 1 isnull(add_Activityid, '') FROM tbl_master_address   WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '')  FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Pin, CASE  (SELECT  top 1 isnull(add_Activityid, '')   FROM          tbl_master_address  WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT  TOP 1 isnull(add_Landmark, '') FROM  tbl_master_address   WHERE      add_cntid = cnt_internalid) ELSE 'no' END AS LandMark ,'' AS ProdcuctType, '' AS Productid,'' as PrdId,0 as Amount", "cnt_status='due' and cnt_branchid in(" + branchId + ") order by NextVisitDate, Address3");
                        }
                        else
                        {
                            if (txtNameSerach.Text == "")
                            {
                                dt = oDBEngine.GetDataTable("tbl_master_contact inner join tbl_master_address on tbl_master_contact.cnt_internalid=tbl_master_address.add_cntid", "cnt_internalId AS id, ISNULL(cnt_firstName, '') + ISNULL(cnt_middleName, '')+ ' ' + ISNULL(cnt_lastName, '') AS name, '' AS Phoneid, '' AS NextVisitDate, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Address1,CASE(SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Address2, CASE (SELECT top 1 isnull(add_Activityid, '') FROM  tbl_master_address WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT  TOP 1 isnull(add_address3, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Address3, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN  (SELECT TOP 1 isnull(add_City, '')  FROM  tbl_master_address  WHERE  add_cntid = cnt_internalid) ELSE '0' END AS City, CASE  (SELECT  top 1 isnull(add_Activityid, '') FROM   tbl_master_address   WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT  TOP 1 isnull(add_Pin, '')  FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Pin, CASE (SELECT  top 1 isnull(add_Activityid, '')   FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Landmark, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE 'no' END AS LandMark ,'' AS ProdcuctType, '' AS Productid,'' as PrdId,0 as Amount", "cnt_status='due' and " + str + " and cnt_branchid in(" + branchId + ") order by NextVisitDate, Address3");
                            }
                            else
                            {
                                dt = oDBEngine.GetDataTable("tbl_master_contact inner join tbl_master_address on tbl_master_contact.cnt_internalid=tbl_master_address.add_cntid", " distinct cnt_internalId AS id, ISNULL(cnt_firstName, '') +' '+ ISNULL(cnt_middleName, '')+' '+ ISNULL(cnt_lastName, '') AS name, '' AS Phoneid, '' AS NextVisitDate, CASE  (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE 'no' END AS Address1, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) WHEN '' THEN  (SELECT TOP 1 isnull(add_address2, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Address2, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT  TOP 1 isnull(add_address3, '')   FROM tbl_master_address  WHERE  add_cntid = cnt_internalid) ELSE 'no' END AS Address3, CASE (SELECT top 1 isnull(add_Activityid, '') FROM  tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN  (SELECT  TOP 1 isnull(add_City, '') FROM  tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE '0' END AS City, CASE  (SELECT  top 1 isnull(add_Activityid, '') FROM  tbl_master_address  WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE 'no' END AS Pin, CASE(SELECT  top 1 isnull(add_Activityid, '') FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN(SELECT TOP 1 isnull(add_Landmark, '') FROM tbl_master_address   WHERE add_cntid = cnt_internalid) ELSE 'no' END AS LandMark ,'' AS ProdcuctType, '' AS Productid,'' as PrdId,0 as Amount", "cnt_status='due' And " + str + " and cnt_branchid in(" + branchId + ") order by NextVisitDate, Address3");
                            }
                        }
                    }
                    else
                    {
                        if (chkPhoneCall.Checked == true)
                        {
                            if (Request.QueryString["Call"].ToString() == "Sales")
                            {
                                if (str == "")
                                {
                                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN  tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN  tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId INNER JOIN  tbl_trans_offeredProduct ON tbl_trans_offeredProduct.ofp_actId = tbl_trans_phonecall.phc_activityId AND tbl_trans_offeredProduct.ofp_leadId = tbl_trans_phonecall.phc_leadcotactId", "tbl_master_contact.cnt_internalId AS Id, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') AS Name,   tbl_trans_phonecall.phc_id AS phoneid, tbl_trans_phonecall.phc_nextCall as NextVisitDate, CASE (SELECT Top 1 isnull(add_Activityid, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address1, '') FROM  tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT   Top 1 isnull(add_address1, '') FROM tbl_master_address WHERE  add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address1, CASE (SELECT  Top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE  (SELECT  Top 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_activityid  = phc_id AND add_cntid = phc_leadcotactId) END AS Address2, CASE(SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address3, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1  isnull(add_address3, '') FROM tbl_master_address  WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address3, CASE (SELECT top 1 isnull(add_Activityid, '')  FROM tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_City, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_city, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS City, CASE (SELECT  top 1 isnull(add_Activityid, '') FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_pin, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Pin, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Landmark, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE      (SELECT   top 1  isnull(add_landmark, '')  FROM          tbl_master_address    WHERE      add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS LandMark , isnull(tbl_trans_offeredProduct.ofp_productTypeId, '')  AS ProdcuctType, isnull(tbl_trans_offeredProduct.ofp_productId, '') AS Productid, isnull(tbl_trans_offeredProduct.ofp_id, '') AS PrdId, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "tbl_trans_phonecall.phc_callDispose = 10  AND tbl_trans_phonecall.phc_branchId IN (" + branchId + ") And left(tbl_trans_offeredProduct.ofp_actId,2)<>'SL'  order by NextVisitDate, Address3");
                                }
                                else
                                {
                                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN  tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN  tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId INNER JOIN  tbl_trans_offeredProduct ON tbl_trans_offeredProduct.ofp_actId = tbl_trans_phonecall.phc_activityId AND tbl_trans_offeredProduct.ofp_leadId = tbl_trans_phonecall.phc_leadcotactId", "tbl_master_contact.cnt_internalId AS Id, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') AS Name,   tbl_trans_phonecall.phc_id AS phoneid, tbl_trans_phonecall.phc_nextCall as NextVisitDate, CASE (SELECT Top 1 isnull(add_Activityid, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT   Top 1 isnull(add_address1, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address1, CASE (SELECT  Top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_activityid  = phc_id AND add_cntid = phc_leadcotactId) END AS Address2, CASE(SELECT    top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address3, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_address3, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address3, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_City, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_city, '') FROM  tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS City, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_pin, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Pin, CASE (SELECT  top 1 isnull(add_Activityid, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Landmark, '') FROM  tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT  top 1 isnull(add_landmark, '') FROM  tbl_master_address  WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS LandMark , isnull(tbl_trans_offeredProduct.ofp_productTypeId, '')  AS ProdcuctType, isnull(tbl_trans_offeredProduct.ofp_productId, '') AS Productid, isnull(tbl_trans_offeredProduct.ofp_id, '') AS PrdId, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "tbl_trans_phonecall.phc_callDispose = 10  And " + str + " AND tbl_trans_phonecall.phc_branchId IN (" + branchId + ") And left(tbl_trans_offeredProduct.ofp_actId,2)<>'SL' order by NextVisitDate, Address3");
                                }
                            }
                            else
                            {
                                lblTitle.Text = "Phone Call Data";
                                if (str == "")
                                {
                                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN  tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId INNER JOIN  tbl_trans_offeredProduct ON tbl_trans_offeredProduct.ofp_actId = tbl_trans_phonecall.phc_activityId AND tbl_trans_offeredProduct.ofp_leadId = tbl_trans_phonecall.phc_leadcotactId", "tbl_master_contact.cnt_internalId AS Id, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') AS Name, tbl_trans_phonecall.phc_id AS phoneid, tbl_trans_phonecall.phc_nextCall as NextVisitDate, CASE (SELECT Top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN  (SELECT TOP 1 isnull(add_address1, '') FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 isnull(add_address1, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address1, CASE (SELECT  Top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN(SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE  (SELECT  Top 1 isnull(add_address2, '') FROM tbl_master_address  WHERE add_activityid  = phc_id AND add_cntid = phc_leadcotactId) END AS Address2, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address3, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE (SELECT  top 1  isnull(add_address3, '') FROM  tbl_master_address  WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address3, CASE (SELECT  top 1 isnull(add_Activityid, '') FROM   tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_City, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_city, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS City, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT  TOP 1 isnull(add_Pin, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1  isnull(add_pin, '') FROM  tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Pin, CASE (SELECT  top 1 isnull(add_Activityid, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Landmark, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT  top 1 isnull(add_landmark, '') FROM  tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS LandMark , isnull(tbl_trans_offeredProduct.ofp_productTypeId, '')  AS ProdcuctType,  CASE ISNULL(tbl_trans_offeredProduct.ofp_productId, '0') WHEN '0' THEN ISNULL(tbl_trans_offeredProduct.ofp_productTypeId, '') ELSE (SELECT prds_description FROM tbl_master_products WHERE (prds_internalId = tbl_trans_offeredProduct.ofp_productId)) END Productid, isnull(tbl_trans_offeredProduct.ofp_id, '') AS PrdId, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "tbl_trans_phonecall.phc_callDispose = 9  AND tbl_trans_phonecall.phc_branchId IN (" + branchId + ") And left(tbl_trans_offeredProduct.ofp_actId,2)<>'SL' AND (tbl_trans_phonecall.phc_NextActivityId IS NULL) order by NextVisitDate, Address3");
                                }
                                else
                                {
                                    dt = oDBEngine.GetDataTable("tbl_trans_phonecall INNER JOIN tbl_master_contact ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN    tbl_trans_Activies ON tbl_trans_phonecall.phc_activityId = tbl_trans_Activies.act_id INNER JOIN  tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId INNER JOIN  tbl_trans_offeredProduct ON tbl_trans_offeredProduct.ofp_actId = tbl_trans_phonecall.phc_activityId AND tbl_trans_offeredProduct.ofp_leadId = tbl_trans_phonecall.phc_leadcotactId", "tbl_master_contact.cnt_internalId AS Id, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') AS Name, tbl_trans_phonecall.phc_id AS phoneid, tbl_trans_phonecall.phc_nextCall as NextVisitDate, CASE (SELECT Top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN  (SELECT TOP 1 isnull(add_address1, '') FROM  tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT Top 1 isnull(add_address1, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address1, CASE (SELECT  Top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN(SELECT TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE  (SELECT  Top 1 isnull(add_address2, '') FROM tbl_master_address  WHERE add_activityid  = phc_id AND add_cntid = phc_leadcotactId) END AS Address2, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_address3, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE (SELECT  top 1  isnull(add_address3, '') FROM  tbl_master_address  WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Address3, CASE (SELECT  top 1 isnull(add_Activityid, '') FROM   tbl_master_address  WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_City, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1 isnull(add_city, '') FROM tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS City, CASE (SELECT top 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT  TOP 1 isnull(add_Pin, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT top 1  isnull(add_pin, '') FROM  tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS Pin, CASE (SELECT  top 1 isnull(add_Activityid, '')  FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Landmark, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT  top 1 isnull(add_landmark, '') FROM  tbl_master_address WHERE add_activityid = phc_id AND add_cntid = phc_leadcotactId) END AS LandMark , isnull(tbl_trans_offeredProduct.ofp_productTypeId, '')  AS ProdcuctType,  CASE ISNULL(tbl_trans_offeredProduct.ofp_productId, '0') WHEN '0' THEN ISNULL(tbl_trans_offeredProduct.ofp_productTypeId, '') ELSE (SELECT prds_description FROM tbl_master_products WHERE (prds_internalId = tbl_trans_offeredProduct.ofp_productId)) END Productid, isnull(tbl_trans_offeredProduct.ofp_id, '') AS PrdId, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "tbl_trans_phonecall.phc_callDispose = 9  And " + str + " AND tbl_trans_phonecall.phc_branchId IN (" + branchId + ") And left(tbl_trans_offeredProduct.ofp_actId,2)<>'SL' AND (tbl_trans_phonecall.phc_NextActivityId IS NULL) order by NextVisitDate, Address3");
                                }
                            }
                        }
                        else
                        {
                            lblTitle.Text = "Sales Visit Data";
                            if (str == "")
                            {
                                dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN  tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN  tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId INNER JOIN   tbl_trans_offeredProduct ON tbl_master_contact.cnt_internalId = tbl_trans_offeredProduct.ofp_leadId AND   tbl_trans_offeredProduct.ofp_id = tbl_trans_salesVisit.slv_productoffered", "tbl_master_contact.cnt_internalId, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, tbl_trans_salesVisit.slv_id AS Phoneid, tbl_trans_salesVisit.slv_nextvisitdatetime AS NextVisitDate, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address1, '') FROM tbl_master_address  WHERE add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE  add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address1, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address2, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address3, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_City, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_city, '') FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS City, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_pin, '')  FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Pin, CASE  (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_Landmark, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_landmark, '')  FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS LandMark , tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, ISNULL(tbl_trans_offeredProduct.ofp_productId, '') AS ProductId, tbl_trans_offeredProduct.ofp_id AS Prdid, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "LEFT(tbl_trans_offeredProduct.ofp_actId, 2) <> 'SL' AND tbl_trans_salesVisit.slv_salesvisitoutcome = 8 AND tbl_master_contact.cnt_branchid IN (" + branchId + ") order by NextVisitDate, Address3");
                            }
                            else
                            {
                                dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN  tbl_master_contact ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_contact.cnt_internalId INNER JOIN  tbl_master_address ON tbl_master_contact.cnt_internalId = tbl_master_address.add_cntId INNER JOIN   tbl_trans_offeredProduct ON tbl_master_contact.cnt_internalId = tbl_trans_offeredProduct.ofp_leadId AND   tbl_trans_offeredProduct.ofp_id = tbl_trans_salesVisit.slv_productoffered", "tbl_master_contact.cnt_internalId, ISNULL(tbl_master_contact.cnt_firstName, '') + ISNULL(tbl_master_contact.cnt_middleName, '') + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, tbl_trans_salesVisit.slv_id AS Phoneid, tbl_trans_salesVisit.slv_nextvisitdatetime AS NextVisitDate, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address1, '') FROM tbl_master_address WHERE  add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address1, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE  add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address2, '') FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address2, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address  WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_address3, '') FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Address3, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_City, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_city, '') FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS City, CASE (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE add_cntid = cnt_internalid) WHEN '' THEN (SELECT TOP 1 isnull(add_Pin, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_pin, '')  FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS Pin, CASE  (SELECT     TOP 1 isnull(add_Activityid, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) WHEN '' THEN (SELECT     TOP 1 isnull(add_Landmark, '') FROM tbl_master_address WHERE      add_cntid = cnt_internalid) ELSE (SELECT     TOP 1 isnull(add_landmark, '')  FROM tbl_master_address WHERE      add_activityid = slv_id AND add_cntid = slv_leadcotactId) END AS LandMark , tbl_trans_offeredProduct.ofp_productTypeId AS ProductType, ISNULL(tbl_trans_offeredProduct.ofp_productId, '') AS ProductId, tbl_trans_offeredProduct.ofp_id AS Prdid, ISNULL(tbl_trans_offeredProduct.ofp_probableAmount, 0) AS Amount", "LEFT(tbl_trans_offeredProduct.ofp_actId, 2) <> 'SL' And " + str + " AND tbl_trans_salesVisit.slv_salesvisitoutcome = 8 AND tbl_master_contact.cnt_branchid IN (" + branchId + ") order by NextVisitDate, Address3");
                            }
                        }
                    }
                }
                if (dt != null)
                {
                    //DataView dtView = new DataView(dt);
                    //dtView.Sort = "NextVisitDate,Address3";
                    grdLead.DataSource = dt;
                    //dt = ((DataView)grdLead.DataSource).Table;
                    ViewState["temp_dt"] = dt;
                    grdLead.DataBind();
                    if (dt.Rows.Count != 0)
                    {
                        btnSubmit.Visible = true;
                        txtNoCont.Enabled = false;
                        btnSubmit1.Visible = false;
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                        txtNoCont.Enabled = true;
                        btnSubmit1.Visible = true;
                    }
                }
            }
            catch
            {
            }
        }
        public void SaveData()
        {
            //change table name tbl_master_lead to tbl_master_contact 13/12/2016

            DataTable dt = new DataTable();
            Session["Count"] = txtNoCont.Text;
            string str = "";
            int tempCount = 0;
            int count = 1;
            string BranchId = "";
            string[] temp12 = Request.QueryString["user"].ToString().Split(',');
            if (temp12.Length > 1)
            {
                count = temp12.Length * Convert.ToInt32(txtNoCont.Text);
            }
            else
            {
                count = Convert.ToInt32(txtNoCont.Text);
            }
            tempCount = count;
            if (chkLead.Checked == true && txtCon1.Text == "" && txtCon2.Text == "" && txtCon3.Text == "" && txtNameSerach.Text == "")
            {
                BranchId = oDBEngine.getBranch(HttpContext.Current.Session["userbranchID"].ToString(), "");
                BranchId = BranchId + HttpContext.Current.Session["userbranchID"].ToString();
                dt = oDBEngine.GetDataTable("tbl_master_contact", "top " + count + " cnt_internalId AS id, ISNULL(cnt_firstName, '') + ISNULL(cnt_middleName, '') AS name, '' AS Phoneid, '' AS NextVisitDate ,'' AS ProdcuctType, '' AS Productid,'' as PrdId", "cnt_status='due' and cnt_branchid in(" + BranchId + ") order by NextVisitDate");
            }
            else
            {
                dt = (DataTable)ViewState["temp_dt"];
            }
            if (txtNoCont.Text != "" && Convert.ToInt32(txtNoCont.Text) > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    string tempPhoneId = dt.Rows[i]["Id"].ToString();
                    if (chkPhoneCall.Checked == true)
                    {
                        oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId = 'allot',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " phc_id ='" + dt.Rows[i]["phoneid"].ToString() + "'");
                    }
                    if (chkSalesVisit.Checked == true)
                    {
                        oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_NextActivityId = 'allot',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " slv_id='" + dt.Rows[i]["phoneid"].ToString() + "'");
                    }
                    str += tempPhoneId + "|" + dt.Rows[i]["name"].ToString() + "|" + dt.Rows[i]["ProdcuctType"].ToString() + "|" + dt.Rows[i]["Productid"].ToString() + "|" + dt.Rows[i]["phoneid"].ToString() + "|" + dt.Rows[i]["prdid"].ToString() + ",";
                    if (i >= (tempCount - 1))
                    {
                        break;
                    }
                }
            }
            else
            {

            }
            str = oDBEngine.sepComma(str);
            Session["lead"] = str;

            //end table name change.
        }
        protected void btnCondition_Click(object sender, EventArgs e)
        {
            pnlSearchCriteria.Visible = true;
            txtNameSerach.Visible = false;
            lblSerachName.Visible = false;
            tdname.Visible = false;
            txtCon1.Visible = true;
            txtCon2.Visible = true;
            txtCon3.Visible = true;
            btnGoSerach.Visible = false;
            txtNameSerach.Text = "";
            drpAndOr1.Visible = true;
            drpAndOr2.Visible = true;
        }
        protected void btnNameSearch_Click(object sender, EventArgs e)
        {
            pnlSearchCriteria.Visible = false;
            txtNameSerach.Visible = true;
            lblSerachName.Visible = true;
            tdname.Visible = true;
            txtNameSerach.Text = "";
            drpAndOr1.Visible = false;
            drpAndOr2.Visible = false;
            txtCon1.Text = "";
            txtCon2.Text = "";
            txtCon3.Text = "";
            txtCon1.Visible = false;
            txtCon2.Visible = false;
            txtCon3.Visible = false;
            btnGoSerach.Visible = true;
        }
        protected void btnAddPhoneCall_Click(object sender, EventArgs e)
        {

        }
        protected void btnAddLead_Click(object sender, EventArgs e)
        {

        }

        protected void grdLead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (chkLead.Checked == true)
            {

                grdLead.Columns[3].Visible = false;
                grdLead.Columns[6].Visible = false;
                grdLead.Columns[9].Visible = false;
            }
            else
            {
                if (chkSalesVisit.Checked == true)
                {
                    grdLead.Columns[2].Visible = false;
                    grdLead.Columns[3].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkLead");
                Label lblLeadid = (Label)e.Row.FindControl("lblId");
                Label lblName = (Label)e.Row.FindControl("lbl23");
                Label lblProductType = (Label)e.Row.FindControl("lblProdType");
                Label lblProdId1 = (Label)e.Row.FindControl("lblProdId");
                Label lblPhoneid1 = (Label)e.Row.FindControl("lbl12");
                Label lblPrdId1 = (Label)e.Row.FindControl("lblprdId");
                Label lblAmt1 = (Label)e.Row.FindControl("lblAmt");
                chk.Attributes.Add("onclick", "javascript:chkclicked(this,'" + lblLeadid.Text + "','" + lblName.Text + "','" + lblProductType.Text + "','" + lblProdId1.Text + "','" + lblPhoneid1.Text + "','" + lblPrdId1.Text + "','" + lblAmt1.Text + "');");
            }
        }
        protected void chkLead_CheckedChanged(object sender, EventArgs e)
        {
            chkLead.Checked = true;
            chkSalesVisit.Checked = false;
            chkPhoneCall.Checked = false;
            FillGrid(true, false);
            Session["call"] = "lead";
        }
        protected void chkPhoneCall_CheckedChanged(object sender, EventArgs e)
        {
            chkLead.Checked = false;
            chkSalesVisit.Checked = false;
            chkPhoneCall.Checked = true;
            FillGrid(true, false);
            Session["call"] = "PhoneSales";
        }
        protected void chkSalesVisit_CheckedChanged(object sender, EventArgs e)
        {
            chkLead.Checked = false;
            chkSalesVisit.Checked = true;
            chkPhoneCall.Checked = false;
            FillGrid(true, false);
        }
        protected void grdSelectedCandidate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (chkLead.Checked == true)
            {
                grdSelectedCandidate.Columns[3].Visible = false;
                grdSelectedCandidate.Columns[4].Visible = false;
                grdSelectedCandidate.Columns[5].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkSelectedCandidate");
                Label lblLeadId = (Label)e.Row.FindControl("lblCandidateId");
                Label lblName = (Label)e.Row.FindControl("lblCandidateName");
                Label lblProductType = (Label)e.Row.FindControl("lblSelectedProdType");
                Label lblProdId1 = (Label)e.Row.FindControl("lblSelectedProdId");
                Label lblPhoneid1 = (Label)e.Row.FindControl("lblSelectedPhoneid");
                Label lblPrdId12 = (Label)e.Row.FindControl("lblSelectedprdId");
                Label lblAmt1 = (Label)e.Row.FindControl("lblSelectedAmt");
                chk.Attributes.Add("onclick", "javascript:chkclicked(this,'" + lblLeadId.Text + "','" + lblName.Text + "','" + lblProductType.Text + "','" + lblProdId1.Text + "','" + lblPhoneid1.Text + "','" + lblPrdId12.Text + "','" + lblAmt1.Text + "');");
            }
        }
        public void FillDropDown()
        {
            string userid1 = oDBEngine.getChildUser(Convert.ToString(Session["userid"]), "");
            string[,] User = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_id in(" + userid1 + ")", 2);
            if (User[0, 0] != "n")
            {
                //oDBEngine.AddDataToDropDownList(User, drpUser);
                drp.AddDataToDropDownList(User, drpUser);
            }
            string[,] sourceType = oDBEngine.GetFieldValue("tbl_master_contactSource", "cntsrc_id,cntsrc_sourceType", null, 2);
            if (sourceType[0, 0] != "n")
            {
                //oDBEngine.AddDataToDropDownList(sourceType, drpSourceType);
                drp.AddDataToDropDownList(sourceType, drpSourceType);
            }
            FillGrid(false, false);
        }
        protected void grdLead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLead.PageIndex = e.NewPageIndex;
            if (txtNameSerach.Text != "" || txtCon1.Text != "")
            {
                FillGrid(true, true);
            }
            else
            {
                FillGrid(true, false);
            }
        }
        protected void drpSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropDownListReferedBy();
        }
        public void FillDropDownListReferedBy()
        {
            //change table name tbl_master_lead to tbl_master_contact 13/12/2016

            string[,] refer;
            int str = Convert.ToInt32(drpSourceType.SelectedValue);
            string[,] ReferClear = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactType='AB'", 2, "cnt_firstName");
            //oDBEngine.AddDataToDropDownList(ReferClear, drpReferBy);
            drp.AddDataToDropDownList(ReferClear, drpReferBy);
            switch (str)
            {

                case 3:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactType='DV'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        //oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;


                case 8:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactType='RA'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy); 
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 10:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactType='RC'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 14:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_user ON tbl_master_contact.cnt_internalId = tbl_master_user.user_contactId", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " user_id='" + Session["userid"].ToString() + "'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        //oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 15:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact ", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactsource='15'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        //oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 18:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact ", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactsource='18'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 19:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", "cnt_contactType='LD'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 20:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", " cnt_contactType='CL'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 21:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", " cnt_contactType='BP'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 24:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", " cnt_contactType='SB'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 9:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact ", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS cnt_firstName", " cnt_contactsource='9'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                case 25:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", " cnt_contactType='FR'", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
                default:
                    refer = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name", " cnt_contactType not in ('AG','BS','BO','BP','CD','LC','CS','CR','CL','DV','DR','EM','FR','LD','LM','ND','PR','RC','RA','SH','SB','VR')", 2, "cnt_firstName");
                    if (refer[0, 0] != "n")
                    {
                        // oDBEngine.AddDataToDropDownList(refer, drpReferBy);
                        drp.AddDataToDropDownList(refer, drpReferBy);
                    }
                    break;
            }

            //end table name change.
        }
        protected void btnGoSerach_Click(object sender, EventArgs e)
        {
            FillGrid(true, true);
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            FillGrid(true, false);
        }
        protected void btnSubmit1_Click(object sender, EventArgs e)
        {
            SaveData();
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", "<script language='JavaScript'>CallParent();</script>");
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveData1();
            ClientScript.RegisterStartupScript(GetType(), "PopupScript1", "<script language='JavaScript'>CallParent();</script>");
        }
        public void SaveData1()
        {
            string str = "";
            int count = 0;
            foreach (GridViewRow row in grdLead.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkLead");
                Label lblid = (Label)row.FindControl("lblId");
                Label lblname = (Label)row.FindControl("lbl23");
                if (chk.Checked == true)
                {
                    str += lblid.Text + "|" + lblname.Text + "||||" + ",";
                    count = count + 1;
                }
            }
            str = oDBEngine.sepComma(str);
            Session["Count"] = count;
            Session["lead"] = str;
        }
    }
}