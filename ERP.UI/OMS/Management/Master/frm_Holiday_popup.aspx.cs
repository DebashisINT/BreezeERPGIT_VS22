using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frm_Holiday_popup : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string chk = Convert.ToString(Request["chk"]) == null ? "" : Convert.ToString(Request["chk"]);
            string status = Convert.ToString(Request.QueryString["status"]);
            string str = Convert.ToString(Request.QueryString["id"]);
            string[] st = str.Split('~');
            string returnVal = "";
            if (status == "work")
            {
                //SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                string[] ids = chk.Split(',');
                string lsSql = " update tbl_master_holiday set hol_WorkingSchedule='" + chk + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "' where hol_id='" + Session["KeyVal"].ToString() + "'";
                lcon.Open();
                SqlCommand lcmd = new SqlCommand(lsSql, lcon);
                int NoOfRowsEffected = lcmd.ExecuteNonQuery();
                lsSql = "Delete from tbl_master_holiday_workingSchdule where holw_hol_id='" + Session["KeyVal"].ToString().Trim() + "'";
                SqlCommand lcmdD = new SqlCommand(lsSql, lcon);
                NoOfRowsEffected = lcmdD.ExecuteNonQuery();
                for (int i = 0; i < ids.Length; i++)
                {
                    lsSql = "insert into tbl_master_holiday_workingSchdule (holw_hol_id,holw_workId) values('" + Session["KeyVal"].ToString().Trim() + "','" + ids[i] + "') ";
                    SqlCommand lcmdI = new SqlCommand(lsSql, lcon);
                    NoOfRowsEffected = lcmdI.ExecuteNonQuery();
                }
                //oDBEngine.SetFieldValue("tbl_master_holiday", "hol_WorkingSchedule='" + chk + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " hol_id='" + Session["KeyVal"].ToString() + "'");
                lcon.Close();
                returnVal = Session["KeyVal"].ToString().Trim() + "~" + st[1] + "~" + chk + "~" + "work";
            }
            else
            {
                //SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                string[] ids = chk.Split(',');
                string lsSql = " update tbl_master_holiday set hol_exchange='" + chk + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "' where hol_id='" + Session["KeyVal"].ToString() + "'";
                lcon.Open();
                SqlCommand lcmd = new SqlCommand(lsSql, lcon);
                int NoOfRowsEffected = lcmd.ExecuteNonQuery();
                lsSql = "Delete from tbl_master_holiday_exchange where holE_hol_id='" + Session["KeyVal"].ToString().Trim() + "'";
                SqlCommand lcmdD = new SqlCommand(lsSql, lcon);
                NoOfRowsEffected = lcmdD.ExecuteNonQuery();
                for (int i = 0; i < ids.Length; i++)
                {
                    lsSql = "insert into tbl_master_holiday_exchange (holE_hol_id,holE_exchId) values('" + Session["KeyVal"].ToString().Trim() + "','" + ids[i] + "') ";
                    SqlCommand lcmdI = new SqlCommand(lsSql, lcon);
                    NoOfRowsEffected = lcmdI.ExecuteNonQuery();
                }
                lcon.Close();
                //oDBEngine.SetFieldValue("tbl_master_holiday", "hol_exchange='" + chk + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " hol_id='" + Session["KeyVal"].ToString() + "'");
                returnVal = Session["KeyVal"].ToString().Trim() + "~" + chk + "~" + st[2] + "~" + "exch";
            }
            string popUpscript = "";
            popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + returnVal + "');window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
        }
        public void ShowList()
        {
            string status = Request.QueryString["status"].ToString();
            string str = Request.QueryString["id"].ToString();
            string[] st = str.Split('~');
            string check_status = "";
            if (status == "work")
            {
                DataTable dtWorking = oDBEngine.GetDataTable("tbl_master_workingHours", "*", null);
                if (dtWorking.Rows.Count != 0)
                {
                    Response.Write("<table>");
                    for (int i = 0; i < dtWorking.Rows.Count; i++)
                    {
                        bool flag = true;
                        check_status = Convert.ToBoolean(Schedule_Status(st[2], dtWorking.Rows[i]["wor_id"].ToString())).ToString();
                        if (Convert.ToBoolean(check_status) == false)
                        {
                            check_status = "";
                        }
                        else
                        {
                            check_status = "checked='CHECKED'";
                        }
                        Response.Write("<tr><td><input " + check_status + " type='checkbox' id='chk' name='chk' value='" + dtWorking.Rows[i]["wor_id"].ToString() + "'></td><td>" + dtWorking.Rows[i]["wor_scheduleName"].ToString() + "</td></tr>");
                    }
                    Response.Write("</table>");
                }
            }
            else
            {
                DataTable dtExchange = oDBEngine.GetDataTable("tbl_master_exchange", "*", null);
                if (dtExchange.Rows.Count != 0)
                {
                    Response.Write("<table>");
                    for (int i = 0; i < dtExchange.Rows.Count; i++)
                    {
                        bool flag = true;
                        check_status = Convert.ToBoolean(Schedule_Status(st[1], dtExchange.Rows[i]["exh_id"].ToString())).ToString();
                        if (Convert.ToBoolean(check_status) == false)
                        {
                            check_status = "";
                        }
                        else
                        {
                            check_status = "checked='CHECKED'";
                        }
                        Response.Write("<tr><td><input " + check_status + " type='checkbox' id='chk' name='chk' value='" + dtExchange.Rows[i]["exh_id"].ToString() + "'></td><td>" + dtExchange.Rows[i]["exh_name"].ToString() + "</td></tr>");
                    }
                    Response.Write("</table>");
                }
            }
        }
        public bool Schedule_Status(string Schedule_collection, string sch)
        {
            bool status = false;
            string[] menus = null;
            if (Schedule_collection != null)
            {
                menus = Schedule_collection.Split(',');
                for (int j = 0; j <= menus.Length - 1; j++)
                {
                    if (menus[j] == sch)
                        status = true;
                }
            }
            return status;
        }
    }
}