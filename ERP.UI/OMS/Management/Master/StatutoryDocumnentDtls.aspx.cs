using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP
{
    public partial class StatutoryDocumnentDtls : System.Web.UI.Page
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
                dt = obj.PopulateStatutoryDtlsById(ref return_msg, EmployeeCode);

                if (return_msg == "Success")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        empcode.Value = dt.Rows[0]["EmployeeCode"].ToString().Trim();
                        empid.Value = dt.Rows[0]["EmployeeID"].ToString().Trim();
                        ASPxTxtPan.Text = dt.Rows[0]["PanNumber"].ToString().Trim();
                        txtpassport.Text = dt.Rows[0]["PassportNumber"].ToString().Trim();
                        if (Convert.ToString(dt.Rows[0]["ValidUpTo"])!="") ASPxDateEditValid.Date = Convert.ToDateTime(dt.Rows[0]["ValidUpTo"]);
                        txtEpic.Text = dt.Rows[0]["EpicNumber"].ToString().Trim();
                        txtaadhaar.Text = dt.Rows[0]["AadhaarNumber"].ToString().Trim();
                        txtNameAsPerPan.Text = dt.Rows[0]["NameAsPerPAN"].ToString().Trim();
                        cmbDeducteestat.Value = Convert.ToString(dt.Rows[0]["DeducteeStatus"].ToString().Trim());



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

                DateTime? ValidUpTo = null;
                if (ASPxDateEditValid.Text != "")
                {
                    ValidUpTo = ASPxDateEditValid.Date;
                }
                string NameAsperPan = "";
                string DeducteeStatus = "";
                if (txtNameAsPerPan.Text != "")
                {
                    NameAsperPan = txtNameAsPerPan.Text;
                }
                if (Convert.ToString(cmbDeducteestat.Value) != "")
                {
                    DeducteeStatus = Convert.ToString(cmbDeducteestat.Value);
                }

                output = obj.Save(EmployeeCode, ASPxTxtPan.Text, txtpassport.Text, ValidUpTo, txtEpic.Text, txtaadhaar.Text, ref ReturnCode, Convert.ToString(HttpContext.Current.Session["userid"]), NameAsperPan, DeducteeStatus);
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