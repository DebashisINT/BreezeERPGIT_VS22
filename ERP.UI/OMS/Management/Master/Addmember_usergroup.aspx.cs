using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Addmember_usergroup : ERP.OMS.ViewState_class.VSPage
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        string id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //if (!IsPostBack)
            //{
            id = Request.QueryString["id"].ToString();
            string id1 = "^^" + id + '%';
            //Session["id"] = id1.ToString();
            //Session["id"] = null;
            HiddenField1.Value = id1.ToString();
            //}

            //Page.ClientScript.RegisterStartupScript(GetType(), "jScript", "<script language='javascript'>ShowFunction('CL');</script>");
            //}

            //txtReportTo.Attributes.Add("onkeyup", "CallList(this,'SearchByEmpCont',event)");
        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            string name = txtReportTo_hidden.Text.ToString().Trim();
            DataTable dtusergroup = oDBEngine.GetDataTable("select user_group from tbl_master_user where user_id=" + name + "");
            string particular = dtusergroup.Rows[0]["user_group"].ToString();
            string addparticular = particular + ',' + id;
            int nowofrows = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + addparticular + "'", "user_id=" + name + "");
            //int now = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + addparticular + "'", "user_id=" + name + "");
            string[] p = particular.Split(',');
            string[,] count = oDBEngine.GetFieldValue("tbl_master_usergroup", "grp_segmentid", "grp_id =" + id + " ", 1);
            DataTable dtuser = oDBEngine.GetDataTable("select grp_segmentid,grp_id from tbl_master_usergroup where grp_segmentid='" + count[0, 0] + "'");
            for (int i = 0; i < dtuser.Rows.Count; i++)
            {
                string dtusergrpid = dtuser.Rows[i]["grp_id"].ToString().Trim();
                //string usergroup = dtuser.Rows[i]["grp_id"].ToString();
                string[] s = particular.Split(',');
                for (int k = 0; k < s.Length; k++)
                {
                    if (s[k].ToString().Trim() == dtusergrpid.ToString())
                    {

                        string lessparticular = particular.Replace(dtusergrpid, "");
                        if (lessparticular.Contains(",,"))
                        {
                            lessparticular = lessparticular.Replace(",,", ",");
                        }
                        if (lessparticular.StartsWith(","))
                        {
                            //string sub= compare.Substring(0,1);
                            lessparticular = lessparticular.Remove(0, 1);
                        }
                        if (lessparticular.EndsWith(","))
                        {
                            lessparticular = lessparticular.Remove(lessparticular.Length - 1) + "";
                        }
                        string forinsert = lessparticular + ',' + id;
                        int nowofrowsaffested = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + forinsert + "'", "user_id=" + name + "");
                        //int now = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + forinsert + "'", "user_id=" + name + "");
                    }
                    //else
                    //{
                    //    int nowofrows = oDBEngine.SetFieldValue("tbl_master_user", "user_group='" + addparticular + "'", "user_id=" + name + "");
                    //}

                }


            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript341", "<script language='javascript'>alert('Succesfully Added');</script>");
            //Response.Redirect("Member_usergroup.aspx?id=" + id, true);
            //Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>FillValues('"+ id +"');</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript341", "<script language='javascript'>alert('Succesfully Added');</script>");
            string p1 = id + "/" + "0" + "/" + "1";
            string popUpscript = "";
            popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
            string popUpscript1 = "";
            popUpscript1 = "alert('Successfully Saved'); parent.editwin.close();";
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
        }
    }

}