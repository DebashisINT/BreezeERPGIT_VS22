using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frmAttchServerFile : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }


            FillGridView();

        }

        public void FillGridView()
        {

            DataTable dt;
            dt = oDBEngine.GetDataTable("(select * from (select * from tbl_master_forms where frm_private='0') as A  union select * from (select * from tbl_master_forms where frm_private='1' and CreateUser=" + HttpContext.Current.Session["userid"] + ") as B) As C", "C.frm_id as formID,frm_FormName as FormName ,C.frm_source as FilePath,C.frm_desc as Description", null);
            //if ((Request.QueryString["private"].ToString().ToUpper()) == "YES")
            //{
            //    lblP.Text = "Personal";
            //    lblP.Visible = true;
            //    dt = oDBEngine.GetDataTable("tbl_master_forms", "frm_id as formID,frm_FormName as FormName ,frm_source as FilePath,frm_desc as Description", "frm_private=1 and CreateUser='" + Session["userid"] + "'");
            //}
            //else//chinmoy
            //{
            //    lblP.Text = "Common";
            //    lblP.Visible = true;
            //    dt = oDBEngine.GetDataTable("tbl_master_forms", "frm_id as formID,frm_FormName as FormName ,frm_source as FilePath,frm_desc as Description", "frm_private=0");
            //}

            if (dt.Rows.Count != 0)
            {
                grdDocuments.DataSource = dt.DefaultView;
                grdDocuments.DataBind();
            }

        }

        protected void grdDocuments_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grdDocuments.ClearSort();
            FillGridView();
            if (e.Parameters == "s")
                grdDocuments.Settings.ShowFilterRow = true;

            if (e.Parameters == "All")
            {
                grdDocuments.FilterExpression = string.Empty;
            }
        }
        protected void grdDocuments_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            DBEngine oDBEngine = new DBEngine(string.Empty);
            string ID = e.Keys[0].ToString();
            oDBEngine.DeleteValue("tbl_master_forms", " frm_id=" + ID + "");
            e.Cancel = true;
            FillGridView();
        }
    }
}