using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmShowForm : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
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
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>height()();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            FillGridView();

        }
        public void FillGridView()
        {
            DataTable dt;
            if (Request.QueryString["private"] != null)
            {
                TextBox1.Text = Request.QueryString["private"].ToString();
                if ((Request.QueryString["private"].ToString().ToUpper()) == "YES")
                {
                    lblP.Text = "Personal";
                    lblP.Visible = true;
                    dt = oDBEngine.GetDataTable("tbl_master_forms", "frm_id as formID,frm_FormName as FormName ,frm_source as FilePath,frm_desc as Description", "frm_private=1 and CreateUser='" + Session["userid"] + "'");
                }
                else
                {
                    lblP.Text = "Common";
                    lblP.Visible = true;
                    dt = oDBEngine.GetDataTable("tbl_master_forms", "frm_id as formID,frm_FormName as FormName ,frm_source as FilePath,frm_desc as Description", "frm_private=0");
                }

                if (dt.Rows.Count != 0)
                {
                    grdDocuments.DataSource = dt.DefaultView;
                    grdDocuments.DataBind();
                }
                else
                {
                    grdDocuments.DataSource = null;
                    grdDocuments.DataBind();
                }
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_master_forms", "frm_id as formID,frm_FormName as FormName ,frm_source as FilePath,frm_desc as Description", "frm_private=0");
                if (dt.Rows.Count != 0)
                {
                    grdDocuments.DataSource = dt.DefaultView;
                    grdDocuments.DataBind();
                }
                else
                {
                    grdDocuments.DataSource = null;
                    grdDocuments.DataBind();
                }
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
        //protected void grdDocuments_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    //DBEngine oDBEngine = new DBEngine(string.Empty);
        //    string ID = e.Keys[0].ToString();
        //    oDBEngine.DeleteValue("tbl_master_forms", " frm_id=" + ID + "");
        //    e.Cancel = true;
        //    FillGridView();
        //}

        protected void grdDocuments_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            string ID = Convert.ToString(e.KeyValue);
            string commanName = e.CommandArgs.CommandName;

            if (commanName == "delete")
            {
                oDBEngine.DeleteValue("tbl_master_forms", " frm_id=" + ID + "");
                //e.Cancel = true;
                FillGridView();
            }
        }
    }
}