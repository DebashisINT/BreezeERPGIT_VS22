using System;
using System.Data;
using System.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frmLanguages : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string aa = Request.QueryString["id"].ToString();
        }
        public void ShowList()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_master_language", "*", null);
            string str = Request.QueryString["id"].ToString();
            //string[] st = str.Split(',');
            string check_status = "";
            if (dt.Rows.Count != 0)
            {
                Response.Write("<table>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool flag = true;
                    check_status = Convert.ToBoolean(Language_Status(str, dt.Rows[i]["lng_id"].ToString())).ToString();
                    if (Convert.ToBoolean(check_status) == false)
                    {
                        check_status = "";
                    }
                    else
                    {
                        check_status = "checked='CHECKED'";
                    }
                    Response.Write("<tr><td><input " + check_status + " type='checkbox' id='chk' name='chk' value='" + dt.Rows[i]["lng_id"].ToString() + "'></td><td>" + dt.Rows[i]["lng_language"].ToString() + "</td></tr>");
                }
                Response.Write("</table>");
            }
        }
        public bool Language_Status(string lng_collection, string lng)
        {
            bool status = false;
            string[] menus = null;
            if (lng_collection != null)
            {
                menus = lng_collection.Split(',');
                for (int j = 0; j <= menus.Length - 1; j++)
                {
                    if (menus[j] == lng)
                        status = true;
                }
            }
            return status;
        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string chk = Request["chk"].ToString();
            string str = getLanguage(chk);
            string popUpscript = "";
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";
            if (Request.QueryString["status"].ToString() == "speak")
            {
                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_speakLanguage='" + chk + "'", " cnt_internalId='" + InternalId + "'");
                popUpscript = "<script language='javascript'>";
                popUpscript += "window.opener.FillValues('" + str + "');window.close();</script>";
            }
            else
            {
                oDBEngine.SetFieldValue("tbl_master_contact", "cnt_writeLanguage='" + chk + "'", " cnt_internalId='" + InternalId + "'");
                popUpscript = "<script language='javascript'>";
                popUpscript += "window.opener.FillValues1('" + str + "');window.close();</script>";
            }
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
        }
        public string getLanguage(string chk)
        {
            string str = "";
            bool flag = false;
            string[] st = chk.Split(',');
            for (int i = 0; i < st.Length; i++)
            {
                DataTable dt = oDBEngine.GetDataTable("tbl_master_language", "lng_language", " lng_id=" + st[i]);
                if (dt.Rows.Count != 0)
                {
                    if (flag == true)
                    {
                        str += "," + dt.Rows[0][0].ToString();
                    }
                    else
                    {
                        flag = true;
                        str += dt.Rows[0][0].ToString();
                    }
                }
            }
            return str;
        }
    }
}