using System;
using System.Configuration;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{

    public partial class management_master_AddArea_PopUp : ERP.OMS.ViewState_class.VSPage
    {
        DBEngine oDBEngine = new DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                try
                {
                    string city = Request.QueryString["name"].ToString();
                    int cityid = Convert.ToInt32(Request.QueryString["id"]);
                    lblCity.Text = city;
                }
                catch { }
            }
        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //    int rowEff = oDBEngine.InsurtFieldValue("tbl_master_area", "area_name,city_id,CreateDate,CreateUser", "'" + txtArea.Text.Trim().Replace("'", "`") + "'" + "," + Request.QueryString["id"].ToString() + "," + "getDate()" + "," + Session["userid"].ToString());
        //    string script = "<script>window.opener.OnChildCall('cmbCity');window.close(this);</script>";
        //    Page.RegisterStartupScript("_close", script);
        //}

        public void btnNewSave_Click(object sender, EventArgs e)
        {
            //....................... Code Added By Sam on 10112016 to check duplicate area name............................
            DBEngine oDBEngine = new DBEngine();
            //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("tbl_master_area", "area_name", " city_id=" + Convert.ToInt32(Request.QueryString["id"]) + " and area_name='" + txtArea.Text.Trim() + "'");
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Area for the selected city already exists. Cannot Proceed.');", true);
                txtArea.Text = "";
                txtArea.Focus();
                return;
            }
            else
            {
                int rowEff = oDBEngine.InsurtFieldValue("tbl_master_area", "area_name,city_id,CreateDate,CreateUser", "'" + txtArea.Text.Trim().Replace("'", "`") + "'" + "," + Request.QueryString["id"].ToString() + "," + "getDate()" + "," + Session["userid"].ToString());


                //....................... Code above Added By Sam on 10112016 to check duplicate area name............................


                //................................. Code Added By Sam on 08112016 due to use the modalpopup instead of window popup...............................
                //string script = "<script>window.opener.OnChildCall('cmbCity');window.close(this);</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('Successfully saved.');", true);
                txtArea.Text = "";
                //string script = "<script>window.parent.OnChildCall('cmbCity');window.parent.HidePopupAndShowInfo();</script>";
                string script = "<script>window.parent.OnChildCall('cmbCity');window.close(this);</script>";

                //................................. Code Added By Sam on 08112016........................................................

                Page.RegisterStartupScript("_close", script);
            }
        }
    }
}