using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.Parameters;
using BusinessLogicLayer;
using System.Data;

namespace ERP.OMS.Reports.XtraReports.Viewer
{
    public partial class VendorCRDRNoteReportViewer : System.Web.UI.Page
    {
        DBEngine odbEngine = new DBEngine();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                odbEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            VendorCRDRNoteXtraReport jv = new VendorCRDRNoteXtraReport();
            //QuotationXtraReportNew jv = new QuotationXtraReportNew();


            // jv.CompanyName = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string companyInternalId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            DataTable dt = odbEngine.GetDataTable("select c.cmp_id,c.cmp_internalid,UPPER(c.cmp_Name) as cmp_Name,c.cmp_CIN,c.cmp_bigLogo,c.cmp_gstin,c.cmp_panNo,phone=(select top 1 phf_phoneNumber from tbl_master_phonefax where phf_cntId=c.cmp_internalid),email=(select top 1 eml_email from tbl_master_email where eml_cntId=c.cmp_internalid)  "
            + ",ISNULL(a.add_address1,'')+','+ISNULL(a.add_address2,'')+','+ISNULL(a.add_address3,'')+'    '+case when (select ISNULL(city_name,'') from tbl_master_city where city_id=a.add_city)<>'' then (select ISNULL(city_name,'') from tbl_master_city where city_id=a.add_city)+'-'+(select pin_code from tbl_master_pinzip where pin_id=a.add_pin) else '' end as 'Address'"
            + ",(select city_name from tbl_master_city where city_id=a.add_city)as add_city,(select pin_code from tbl_master_pinzip where pin_id=a.add_pin) as add_pin"
            + "  from tbl_master_company c  Left join tbl_master_address a on c.cmp_internalid=a.add_cntId  where c.cmp_internalid='" + companyInternalId + "'");



            if (dt.Rows.Count > 0)
            {
                jv.CompanyName = Convert.ToString(dt.Rows[0]["cmp_Name"]);
                jv.CompanybigLogo = Convert.ToString(dt.Rows[0]["cmp_bigLogo"]);
                jv.LocalCurr = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                //jv.CompanyCIN = Convert.ToString(dt.Rows[0]["cmp_CIN"]);
                //jv.CompanyPan = Convert.ToString(dt.Rows[0]["cmp_panNo"]);
                //jv.Companygstin = Convert.ToString(dt.Rows[0]["cmp_gstin"]);
                //jv.CompanyAddress = Convert.ToString(dt.Rows[0]["Address"]);
                //jv.Companyadd_city = Convert.ToString(dt.Rows[0]["add_city"]);
                //jv.Companyadd_pin = Convert.ToString(dt.Rows[0]["add_pin"]);
                //jv.Companyadd_phone = Convert.ToString(dt.Rows[0]["phone"]);
                //jv.Companyadd_email = Convert.ToString(dt.Rows[0]["email"]);
            }


            if (Request.QueryString["id"] != null)
            {
                var param1 = jv.Parameters.GetByName("Vendor_CRDR_id");
                param1.Value = Convert.ToInt32(Request.QueryString["id"]);
                var paramcompId = jv.Parameters.GetByName("Company_Id");
                paramcompId.Value = companyInternalId;
            }


            ASPxDocumentViewer1.Report = jv; 
        }
    }
}