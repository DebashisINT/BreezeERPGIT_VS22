using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class PFESIDtls : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                // string employeecode="hhh";
                PopulateStatutoryDtlsById(HttpContext.Current.Session["KeyVal_InternalID"].ToString());
                ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
               
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(HttpContext.Current.Session["KeyVal_InternalID"].ToString());
        }
     
        #endregion
        #region Custom Methods
        private void PopulateStatutoryDtlsById(string EmployeeCode)
        {
            try
            {

                DataTable dt = new DataTable();
                EmployeeStatutoryBal obj = new EmployeeStatutoryBal();
                string return_msg = "";
                dt = obj.PfESIDtlsById(ref return_msg, EmployeeCode);

                if (return_msg == "Success")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        empcode.Value = dt.Rows[0]["EmployeeCode"].ToString().Trim();
                        empid.Value = dt.Rows[0]["EmployeeID"].ToString().Trim();
                        cmbpf.SelectedValue = Convert.ToInt32(dt.Rows[0]["PfApplicable"]).ToString();
                        txtpf.Text = dt.Rows[0]["PfNumber"].ToString().Trim();
                        txtuan.Text = dt.Rows[0]["UANNumber"].ToString().Trim();
                        cmbesiappcbl.SelectedValue = Convert.ToInt32(dt.Rows[0]["EsiApplicable"]).ToString();
                        txtesino.Text = dt.Rows[0]["EsicNumber"].ToString().Trim();



                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + return_msg.Replace("'", "") + "');</script>");
                }


            }

            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + ex.Message.Replace("'", "") + "');</script>");
            }
        }

        private void Save(string EmployeeCode)
        {
            string output = string.Empty;
            string ReturnCode = string.Empty;
            EmployeeStatutoryBal obj = new EmployeeStatutoryBal();
            try
            {

                output = obj.SavePFESI(EmployeeCode, Convert.ToInt32(cmbpf.SelectedValue), txtpf.Text, txtuan.Text, Convert.ToInt32(cmbesiappcbl.SelectedValue), txtesino.Text, ref ReturnCode, Convert.ToString(HttpContext.Current.Session["userid"]));
                if (output == "Success" && ReturnCode == "1")
                {

                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + output.Replace("'", "") + "');</script>");

                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>alertify('" + ex.Message.Replace("'", "") + "');</script>");
            }

        }

        #endregion

       

       
    }
}