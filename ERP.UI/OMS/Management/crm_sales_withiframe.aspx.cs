using System;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_crm_sales_withiframe : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            SalesDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Lrd.Attributes.Add("onclick", "All_CheckedChanged();");
                Erd.Attributes.Add("onclick", "Specific_CheckedChanged();");

            }
            BindGrid();
            //Page.ClientScript.RegisterStartupScript(GetType(), "SetHeight", "<script language='JavaScript'>height();</script>");
        }
        protected void SalesGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGrid();
        }
        void BindGrid()
        {
            int UserId = Convert.ToInt32(HttpContext.Current.Session["userid"]);//Session UserID
            SalesDataSource.SelectCommand = "Select tbl_trans_Sales.sls_sales_status AS Status,(isnull((SELECT cnt_firstName FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id),(select cnt_firstname from tbl_master_lead where cnt_internalid=tbl_trans_Sales.sls_contactlead_id))+' ' +isnull( isnull((SELECT cnt_middleName FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id),(select cnt_middlename from tbl_master_lead where cnt_internalid=tbl_trans_Sales.sls_contactlead_id)),' ')+' ' +isnull( isnull((SELECT cnt_lastName FROM tbl_master_contact WHERE cnt_internalId = tbl_trans_Sales.sls_contactlead_id),(select cnt_lastname from tbl_master_lead where cnt_internalid=tbl_trans_Sales.sls_contactlead_id)),' '))  AS Name,(SELECT Top 1 ISNULL(add_address1, '') + ' , ' + ISNULL(add_address2, '') + ' , ' + ISNULL(add_address3, '') + ' [ ' + ISNULL(add_landMark, '') + ' ], ' + ISNULL(add_pin, '') FROM tbl_master_address WHERE add_cntId = sls_contactlead_id) AS Address,(SELECT Top 1 phf_phoneNumber FROM tbl_master_phoneFax WHERE phf_cntId = sls_contactlead_id) AS Phone,CASE tbl_trans_Sales.sls_ProductType WHEN 'Mutual Fund' THEN 'frmSalesMutualFund1.aspx?id=' + cast(sls_id AS varchar)WHEN 'Insurance-life'  THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar)WHEN 'Insurance-general'  THEN 'frmSalesInsurance1.aspx?id=' + cast(sls_id AS varchar) ELSE 'frmSalesCommodity1.aspx?id=' + cast(sls_id AS varchar) END AS ProductTypePath,sls_ProductType as ProductType,tbl_trans_Sales.sls_id AS Id, tbl_trans_Sales.sls_estimated_value AS Amount,CASE isnull(sls_product, '') WHEN ''THEN tbl_trans_Sales.sls_productType ELSE (SELECT prds_description FROM tbl_master_products WHERE prds_internalId = sls_product) END AS Product,sls_contactlead_id as LeadId, case sls_nextvisitdate when '1/1/1900 12:00:00 AM' then ' ' else (convert(varchar(11),sls_nextvisitdate,113) +' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), sls_nextvisitdate, 22), 3))) end as NextVisit From  tbl_trans_Sales , tbl_trans_Activies Where tbl_trans_Sales.sls_activity_id = tbl_trans_Activies.act_id AND tbl_trans_Activies.act_assignedTo ='" + UserId.ToString() + "' and sls_sales_status=4 Order by convert(datetime,sls_nextvisitdate,101)";

        }
    }
}