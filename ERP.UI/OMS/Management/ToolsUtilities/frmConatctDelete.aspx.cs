using ClsDropDownlistNameSpace;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_ToolsUtilities_frmConatctDelete : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList clsdrp = new clsDropDownList();
        string[,] AllType;

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
            if (!IsPostBack)
            {
                AllType = oDBEngine.GetFieldValue(" tbl_master_contactType ", " cnt_prefix,cnttpy_contactType ", "cnt_prefix not in ('CD','ND','EM','LD')", 2, " cnttpy_contactType ");
                // oDBEngine.AddDataToDropDownList(AllType, DDLAddData);
                clsdrp.AddDataToDropDownList(AllType, DDLAddData);
                DDLAddData.Items.Insert(0, new ListItem("--Select--", "0"));
            }

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            txtReportTo.Attributes.Add("onkeyup", "showOptions(this,'SearchContactByType',event)");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable CustTrades = new DataTable();
            DataTable CustComTrad = new DataTable();
            DataTable CustExchange = new DataTable();
            DataTable ComExchange = new DataTable();
            DataTable CashBank = new DataTable();
            DataTable JournalVoucher = new DataTable();
            string ErrorText = "";
            if (txtReportTo_hidden.Value != "" || txtReportTo.Text != "")
            {
                DataSet ds = new DataSet();
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
                using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("Fetch_ForContctDelete", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@cnt_internalId", txtReportTo_hidden.Value.ToString());
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);
                    }
                }
                int j = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                int k = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                int n = Convert.ToInt32(ds.Tables[2].Rows[0][0].ToString());
                int p = Convert.ToInt32(ds.Tables[3].Rows[0][0].ToString());
                int q = Convert.ToInt32(ds.Tables[4].Rows[0][0].ToString());
                int r = Convert.ToInt32(ds.Tables[5].Rows[0][0].ToString());
                if (j > 0)
                {
                    ErrorText += "\\n" + "Deletion is not allowed due to customer trades!..";

                }
                if (k > 0)
                {
                    ErrorText += "\\n" + ErrorText + "\\n" + "Deletion is not allowed due to commodity trades!..";

                }
                if (n > 0)
                {
                    ErrorText += "\\n" + "Deletion is not allowed due to  exchange trades!..";

                }
                if (p > 0)
                {
                    ErrorText += "\\n" + "Deletion is not allowed due to  commodity exchange trades!..";

                }
                if (q > 0)
                {
                    ErrorText += "\\n" + "Deletion is not allowed due to  cashbank detail!..";

                }
                if (r > 0)
                {
                    ErrorText += "\\n" + "Deletion is not allowed due to journal voucher sub account!..";
                }

                if (ErrorText == "")
                {

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("DeleteContct", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_internalId", txtReportTo_hidden.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@userID", DBNull.Value);
                    lcmdEmplInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802
                    txtReportTo_hidden.Value = "";
                    txtReportTo.Text = "";
                    ErrorText = "Succussfully Deleted!..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript16", "Validate('" + ErrorText + "');", true);

                }
                else
                {
                    ErrorText = "Transaction exists deletion is not allowed!..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript15", "Validate('" + ErrorText + "');", true);
                }
            }
            else
            {
                ErrorText = "Please Select Contact!..";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript18", "Validate('" + ErrorText + "');", true);
            }
        }
        protected void DDLAddData_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlText.Value = DDLAddData.SelectedItem.Text.ToString().Trim();
            ddlValue.Value = DDLAddData.SelectedItem.Value.ToString().Trim();

        }
    }
}