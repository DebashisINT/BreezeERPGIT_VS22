using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class OtherDetails : System.Web.UI.Page
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // string employeecode="hhh";
                PopulateEmployeeOtherDetailsById(HttpContext.Current.Session["KeyVal_InternalID"].ToString());
                ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(HttpContext.Current.Session["KeyVal_InternalID"].ToString());
        }

        #endregion

        #region Custom Methods
        private void PopulateEmployeeOtherDetailsById(string EmployeeCode)
        {
            try
            {

                DataTable dt = new DataTable();
                EmployeeStatutoryBal obj = new EmployeeStatutoryBal();
                string return_msg = "";
                dt = obj.EmployeeOtherDetailsById(ref return_msg, EmployeeCode);

                if (return_msg == "Success")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        empcode.Value = dt.Rows[0]["EmployeeCode"].ToString().Trim();
                        empid.Value = dt.Rows[0]["EmployeeID"].ToString().Trim();
                        txtunit.Text = dt.Rows[0]["unitdesc"].ToString().Trim();
                        txtdept.Text = dt.Rows[0]["deptdesc"].ToString().Trim();
                        txtdesig.Text = dt.Rows[0]["desigdesc"].ToString().Trim();
                        txtgrade.Text = dt.Rows[0]["gradedesc"].ToString().Trim();
                        if (Convert.ToString(dt.Rows[0]["JoiningDate"]) != "") ASPxDateEditJoining.Date = Convert.ToDateTime(dt.Rows[0]["JoiningDate"]);
                        if (Convert.ToString(dt.Rows[0]["LeavingDate"]) != "") ASPxDateEditLeaving.Date = Convert.ToDateTime(dt.Rows[0]["LeavingDate"]);
                        Hdnunit.Value = Convert.ToString(dt.Rows[0]["Unit"]);
                        Hdndept.Value = Convert.ToString(dt.Rows[0]["Department"]);
                        Hdndesig.Value = Convert.ToString(dt.Rows[0]["Designation"]);
                        Hdngrade.Value = Convert.ToString(dt.Rows[0]["Grade"]);
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
                DateTime? ValidJoiningDate = null;
                DateTime? ValidLeavingDate = null;
                if (ASPxDateEditJoining.Text != "")
                {
                    ValidJoiningDate = ASPxDateEditJoining.Date;
                }

                if (ASPxDateEditLeaving.Text != "")
                {
                    ValidLeavingDate = ASPxDateEditLeaving.Date;
                }

                output = obj.SaveEmpOtherDtls(EmployeeCode, ValidJoiningDate,ValidLeavingDate, Convert.ToString(Hdnunit.Value), Convert.ToString(Hdndept.Value), Convert.ToString(Hdndesig.Value), Convert.ToString(Hdngrade.Value), ref ReturnCode, Convert.ToString(HttpContext.Current.Session["userid"]));
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