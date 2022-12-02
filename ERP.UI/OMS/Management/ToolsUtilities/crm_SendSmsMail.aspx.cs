using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Data;
using System.Net;
using System.Web.Services;
using ERP.Models;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class crm_SendSmsMail : System.Web.UI.Page
    {
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        crm_SendSmsMailBL bls = new crm_SendSmsMailBL();
        SendSmsBL ObjSendSMS = new SendSmsBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //   grid.DataSource = BatchGridData;
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = bls.PopulateCustomer(ddlType.SelectedValue.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                grid.DataSource = dt;
                grid.DataBind();
            }

        }

        [WebMethod]
        public static string SendSMS(string Mobiles, string messagetext)
        {
            string status = string.Empty;
            SendSmsBL ObjSendSMS = new SendSmsBL();
            try
            {
                //String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                //String username = System.Configuration.ConfigurationSettings.AppSettings["username"];
                //String password = System.Configuration.ConfigurationSettings.AppSettings["password"];
                //String Provider = System.Configuration.ConfigurationSettings.AppSettings["Provider"];
                //String sender = System.Configuration.ConfigurationSettings.AppSettings["sender"];

                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];

                // string res = SmsSent(username, password, Provider, sender, Mobiles, messagetext, "Text");
                string res = ObjSendSMS.sendSMS(Mobiles, messagetext);
                if (res != "0")
                {
                    status = "200";
                    return status;
                }
                else
                {
                    status = "300";
                    return status;
                }
                return status;
            }
            catch
            {
                status = "300";
                return status;
            }
            return status;
        }

        public string SmsSent(string username, string password, string Provider, string senderId, string mobile, string message, string type)
        {

            //  http://5.189.187.82/sendsms/sendsms.php?username=QHEkaruna&password=baj8piv3&type=Text&sender=KARUNA&mobile=9831892083&message=HELO
            string response = "";
            string url = Provider + "?username=" + username + "&password=" + password + "&type=" + type + "&sender=" + senderId + "&mobile=" + mobile + "&message=" + message;
            if (mobile.Trim() != "")
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    response = httpResponse.StatusCode.ToString();
                }
                catch
                {
                    return "0";
                }
            }
            return response;
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_id";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string cust_type = ddlType.SelectedValue;
            int userid = Convert.ToInt32(Session["UserID"]);
            List<int> branchidlist;
            bool i = objCRMSalesOrderDtlBL.GetUserwiseDocumentFiltered(Convert.ToString(Session["UserID"]));
            if (i)
            {
                if (cust_type.ToUpper() != "SELECT")
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_EntityLists
                            where d.ContactType == cust_type
                            orderby d.Name
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_EntityLists
                            where d.ContactType == cust_type
                            orderby d.Name
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                if (cust_type.ToUpper() != "SELECT")
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_EntityLists
                            where d.ContactType == cust_type
                            orderby d.Name
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_EntityLists
                            where d.ContactType == cust_type
                            orderby d.Name
                            select d;
                    e.QueryableSource = q;
                }
            }
        }

        protected void btnSms_Click(object sender, EventArgs e)
        {
            String Phone_no = "";
            for (int i = 0; i < grid.GetSelectedFieldValues("phf_phoneNumber").Count; i++)
            {
                if (Phone_no == "")
                {
                    Phone_no = Convert.ToString(grid.GetSelectedFieldValues("phf_phoneNumber")[i]);
                }
                else
                {
                    Phone_no += "," + Convert.ToString(grid.GetSelectedFieldValues("phf_phoneNumber")[i]);
                }
            }
         
            //   String Mobiles = txtCustomeSMS.Text.Trim();
            String status = "";
            string res = "";
            if (!string.IsNullOrEmpty(txtSms.Text.Trim()))
            {
                res = ObjSendSMS.sendSMS(Phone_no, txtSms.Text.Trim());
            }
            if (res != "0")
            {
                status = "200";

            }
            else
            {
                status = "300";

            }
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            String Phone_no = "";
            for (int i = 0; i < grid.GetSelectedFieldValues("phf_phoneNumber").Count; i++)
            {
                if (Phone_no == "")
                {
                    Phone_no = Convert.ToString(grid.GetSelectedFieldValues("phf_phoneNumber")[i]);
                }
                else
                {
                    Phone_no += "," + Convert.ToString(grid.GetSelectedFieldValues("phf_phoneNumber")[i]);
                }
            }
            if (ddlType.SelectedValue == "Cus")
            {
                Phone_no = txtCustomeSMS.Text.Trim();
            }

           // grid.Selection.UnselectAll();
            String status = "";
            string res = "";
            if (!string.IsNullOrEmpty(txtSms.Text.Trim()))
            {
                res = ObjSendSMS.sendSMS(Phone_no, txtSms.Text.Trim());
            }
            if (res != "0")
            {
                ddlType.SelectedValue = "Select";
                txtSms.Value = "";
                txtSms.Text = "";
                txtCustomeSMS.Text = "";
                txtCustomeSMS.Value = "";
                //Response.Write("<script>alert('SMS Sent Successfully.');</script>");
               // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SMS Sent Successfully.')", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('SMS Sent Successfully.')</script>");
                grid.JSProperties["cpResult"] = "True";
            }
            else
            {
                //Response.Write("<script>alert('SMS not Sent please try again later.');</script>");
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SMS not Sent please try again later.')", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('SMS not Sent please try again later.')</script>");
                grid.JSProperties["cpResult"] = "False";
            }
        }
    }
}