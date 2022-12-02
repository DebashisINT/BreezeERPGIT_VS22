using System;
using System.Configuration;
using System.Data;
using BusinessLogicLayer;
namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_UserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserList();
            }
        }
        public void BindUserList()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();



            string AllUser = oDBEngine.getChildUser_for_AllEmployee(Convert.ToString(Session["userid"]), "");
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("tbl_master_user", "user_id as Id,user_name as UserList", " user_id in (" + AllUser + ")");
            if (dt != null)
            {
                Response.Write("<table width='100%'>");
                Response.Write("<tr style='background-color:#DDECFE'><td style='font-family: Tahoma,Arial, Verdana, sans-serif;color:Blue;font-size:15px' colspan='3' align='center'>User List</td></tr>");
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (i % 3 == 0)
                    {
                        if (i != 0)
                        {
                            Response.Write("</tr>");
                        }
                        Response.Write("<tr style='background-color:#DDECFE'>");
                    }
                    if (ViewState["val"] != null)
                    {
                        if (ViewState["val"].ToString() == "true")
                        {
                            if (txtUserList.Value == "")
                            {
                                txtUserList.Value = dt.Rows[i][1].ToString();
                                hd1User.Value = dt.Rows[i][0].ToString();
                            }
                            else
                            {
                                txtUserList.Value += "," + dt.Rows[i][1].ToString();
                                hd1User.Value += "," + dt.Rows[i][0].ToString();
                            }
                            Response.Write("<td style='font-family: Tahoma,Arial, Verdana, sans-serif;color:Blue;font-size:10px; width:300px'><input type='checkbox' checked ='checked'  id='chk_" + i + "' value='" + dt.Rows[i]["Id"].ToString() + "~" + dt.Rows[i]["UserList"].ToString() + "' onclick='javascript:checkevent(this)'>" + dt.Rows[i]["UserList"].ToString() + "</td>");
                        }
                    }
                    else
                    {
                        Response.Write("<td style='font-family: Tahoma,Arial, Verdana, sans-serif;color:Blue;font-size:10px; width:300px'><input type='checkbox' id='chk_" + i + "' value='" + dt.Rows[i]["Id"].ToString() + "~" + dt.Rows[i]["UserList"].ToString() + "' onclick='javascript:checkevent(this)'>" + dt.Rows[i]["UserList"].ToString() + "</td>");
                    }
                }
                Response.Write("</tr>");
               
                //Response.Write("<tr style='background-color:#DDECFE'><td colspan='3' align='center' ><input type='button' id='btnSubmit' value='Submit' class='btnUpdate' onclick='javascript:CloseWindow1()'></td></tr></table>");
                Response.Write("<tr style='background-color:#DDECFE'><td colspan='3' align='center' ><input type='button' id='btnSubmit' value='Submit' class='btnUpdate' onclick='CloseWindow1()'></td></tr></table>");
            }
        }
        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            //Response.Redirect("UserList.aspx?val=true");
            ViewState["val"] = "true";
            BindUserList();
        }
    }
}