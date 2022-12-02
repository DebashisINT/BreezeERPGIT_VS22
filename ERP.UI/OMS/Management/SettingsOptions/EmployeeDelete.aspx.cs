using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_EmployeeDelete : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        GlobalSettings globalsetting = new GlobalSettings();

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
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            txtReportTo.Attributes.Add("onkeyup", "CallList(this,'SearchByEmpCont',event)");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtReportTo_hidden.Value != "")
            {
                if (txtReportTo.Text != "")
                {
                    DataTable dtUser = new DataTable();
                    //  string userid = string.Empty;
                    dtUser = oDBEngine.GetDataTable("tbl_master_user", " top 1 * ", "user_contactId='" + txtReportTo_hidden.Value + "'");

                    if (dtUser.Rows.Count > 0)
                    {
                        //userid = dtUser.Rows[0]["user_id"].ToString();


                        //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        //SqlConnection lcon = new SqlConnection(con);
                        //lcon.Open();
                        //SqlCommand lcmdEmplInsert = new SqlCommand("EmployeeDeleteBySelctName", lcon);
                        //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        //lcmdEmplInsert.Parameters.AddWithValue("@cnt_internalId", txtReportTo_hidden.Value);
                        //lcmdEmplInsert.Parameters.AddWithValue("@userID", dtUser.Rows[0]["user_id"]);
                        //lcmdEmplInsert.ExecuteNonQuery();
                        globalsetting.EmployeeDeleteBySelctName(txtReportTo_hidden.Value.ToString(), dtUser.Rows[0]["user_id"].ToString());
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "height12", "<script>alert('Employee has been deleted.');</script>");
                        txtReportTo_hidden.Value = "";
                        txtReportTo.Text = "";
                    }
                    else
                    {
                        //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                        //SqlConnection lcon = new SqlConnection(con);
                        //lcon.Open();
                        //SqlCommand lcmdEmplInsert = new SqlCommand("EmployeeDeleteBySelctName", lcon);
                        //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                        //lcmdEmplInsert.Parameters.AddWithValue("@cnt_internalId", txtReportTo_hidden.Value);
                        //lcmdEmplInsert.Parameters.AddWithValue("@userID", DBNull.Value);
                        //lcmdEmplInsert.ExecuteNonQuery();
                        globalsetting.EmployeeDeleteBySelctName(txtReportTo_hidden.Value.ToString(), DBNull.Value.ToString());
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "height15", "<script>alert('Employee has been deleted.');</script>");
                        txtReportTo_hidden.Value = "";
                        txtReportTo.Text = "";

                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "height154", "<script>alert('Please Select Employee.');</script>");
                }

            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height158", "<script>alert('Please Select Employee.');</script>");
            }
        }

    }
}