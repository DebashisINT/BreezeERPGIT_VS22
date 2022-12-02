using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EntityLayer.CommonELS;
namespace ERP.OMS.Management.SettingsOptions
{
    public partial class Servicetax : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            //Added By :Subhabrata
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/settingsoptions/servicetax.aspx");
            //End

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //ServiceTaxDataSource.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    ServiceTaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //ServiceTaxDataSource.ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    ServiceTaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string segid = "0";
            string[,] seg = oDBEngine.GetFieldValue("(select A.EXCH_INTERNALID AS exch_internalId ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS segment_name from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as K", "exch_internalId", " ltrim(rtrim(segment_name)) in(select ltrim(rtrim(seg_name)) from tbl_master_segment where seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "')", 1);
            if (seg[0, 0] != "n")
            {
                segid = seg[0, 0];
            }
            if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
            {
                ServiceTaxDataSource.SelectCommand = "SELECT b.*,c.ddd FROM (SELECT a.*,MC.cmp_Name  FROM (SELECT ST.ServTax_CompanyID,ST.ServTax_ID,Convert(varchar(12),ServTax_DateTo,106) as ServTax_DateTo,Convert(varchar(12),ServTax_DateFrom,106) as ServTax_DateFrom,cast(ST.ServTax_HgrEduCess as decimal(18,2))as ServTax_HgrEduCess,cast(ST.ServTax_Rate as decimal(18,2))as ServTax_Rate,cast(ST.ServTax_EduCess as decimal(18,2))as ServTax_EduCess ,ST.ServTax_ExchangeSegmentID,(select ChargeGroup_Name from Master_ChargeGroup as mcg where mcg.ChargeGroup_Code=ST.ServTax_ChargeGroupID and mcg.ChargeGroup_Type='2')as r FROM Config_ServTax AS ST) AS a LEFT OUTER JOIN tbl_master_company as MC ON a.ServTax_CompanyID=MC.cmp_internalid)as b LEFT OUTER JOIN(SELECT exch_internalId, exch_membershipType as 'ddd' FROM tbl_master_companyExchange where exch_exchid is null) AS C ON b.ServTax_ExchangeSegmentID=c.exch_internalId where (ddd='NSDL'or ddd='CDSL') and ServTax_ExchangeSegmentID=" + segid + "";
            }
            else if (Session["userlastsegment"].ToString() == "3")
            {
                ServiceTaxDataSource.SelectCommand = "SELECT b.*,c.ddd FROM (SELECT a.*,MC.cmp_Name  FROM (SELECT ST.ServTax_CompanyID,ST.ServTax_ID,Convert(varchar(12),ServTax_DateTo,106) as ServTax_DateTo,Convert(varchar(12),ServTax_DateFrom,106) as ServTax_DateFrom,ServTax_DateFrom as date,cast(ST.ServTax_HgrEduCess as decimal(18,2))as ServTax_HgrEduCess,cast(ST.ServTax_Rate as decimal(18,2))as ServTax_Rate,cast(ST.ServTax_EduCess as decimal(18,2))as ServTax_EduCess ,ST.ServTax_ExchangeSegmentID,(select ChargeGroup_Name from Master_ChargeGroup as mcg where mcg.ChargeGroup_Code=ST.ServTax_ChargeGroupID and mcg.ChargeGroup_Type='2')as r FROM Config_ServTax AS ST) AS a LEFT OUTER JOIN tbl_master_company as MC ON a.ServTax_CompanyID=MC.cmp_internalid)as b LEFT OUTER JOIN(SELECT TMCE.exch_internalID, convert(varchar,TME.exh_shortName)+'_' +convert(varchar,TMCE.exch_segmentId) as 'ddd' FROM tbl_master_companyExchange AS TMCE LEFT OUTER JOIN tbl_master_exchange AS TME ON TME.exh_cntId=TMCE.exch_exchId) AS C ON b.ServTax_ExchangeSegmentID=c.exch_internalID where (ddd is null) and ServTax_ExchangeSegmentID=" + Session["userlastsegment"].ToString() + " order by ServTax_CompanyID,date";
            }
            else
            {
                ServiceTaxDataSource.SelectCommand = "SELECT b.*,c.ddd FROM (SELECT a.*,MC.cmp_Name  FROM (SELECT ST.ServTax_CompanyID,ST.ServTax_ID,Convert(varchar(12),ServTax_DateTo,106) as ServTax_DateTo,Convert(varchar(12),ServTax_DateFrom,106) as ServTax_DateFrom,cast(ST.ServTax_HgrEduCess as decimal(18,2))as ServTax_HgrEduCess,cast(ST.ServTax_Rate as decimal(18,2))as ServTax_Rate,cast(ST.ServTax_EduCess as decimal(18,2))as ServTax_EduCess ,ST.ServTax_ExchangeSegmentID,(select ChargeGroup_Name from Master_ChargeGroup as mcg where mcg.ChargeGroup_Code=ST.ServTax_ChargeGroupID and mcg.ChargeGroup_Type='2')as r FROM Config_ServTax AS ST) AS a LEFT OUTER JOIN tbl_master_company as MC ON a.ServTax_CompanyID=MC.cmp_internalid)as b LEFT OUTER JOIN(SELECT TMCE.exch_internalID, convert(varchar,TME.exh_shortName) as 'ddd' FROM tbl_master_companyExchange AS TMCE LEFT OUTER JOIN tbl_master_exchange AS TME ON TME.exh_cntId=TMCE.exch_exchId) AS C ON b.ServTax_ExchangeSegmentID=c.exch_internalID where (ddd!='NSDL'or ddd!='CDSL') and ServTax_ExchangeSegmentID=" + Session["userlastsegment"].ToString() + "";
            }

            ServiceTaxGrid.DataBind();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        protected void ServiceTaxGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ServiceTaxGrid.ClearSort();
            ServiceTaxGrid.DataBind();
        }
        protected void ServiceTaxGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = "2";
        }
        protected void ServiceTaxGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            int rowindex = e.VisibleIndex;
            //if (Session["LastCompany"].ToString() == null)
            //    e.Enabled = false;
            string datetrade = ServiceTaxGrid.GetRowValues(rowindex, "ServTax_DateTo").ToString();
            if (datetrade != "")
            {
                //GridViewCommandColumn ccol = (GridViewCommandColumn)(gridBrokerage.Columns[10]);

                e.Enabled = false;




            }
        }
    }
}