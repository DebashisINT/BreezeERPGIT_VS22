using System;
using System.Data;
using System.Web;
using System.Web.UI;
using FreeTextBoxControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_TemplateMasterAddEdit : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {
            FreeTextBox FreeTextBox1 = new FreeTextBox();
            FreeTextBox1.ID = "FreeTextBox1";
            FreeTextBox1.Height = 340;
            FreeTextBox1.Width = 785;
            FreeTextBoxPlaceHolder.Controls.Add(FreeTextBox1);

            if (!IsPostBack)
            {
                // Replace .ToString() with Convert.ToString(..) By Sudip on 19122016

                //if (Request.QueryString["id"].ToString() != "ADD")
                if (Convert.ToString(Request.QueryString["id"]) != "ADD")
                {
                    ShowForm();

                }
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "pageld", "<script>TypeSet('" + cmbType.SelectedItem.Value + "');</script>");

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 19122016

            FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
            string disclaimer = Convert.ToString(textEditor.Text).Replace("'", "''");
            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                int NoofRowsAffect = oDBEngine.InsurtFieldValue("master_templateDetails", " Tmplt_UsedFor,Tmplt_UsedSegment,Tmplt_UsedCompay,Tmplt_ShortName,Tmplt_Content,Tmplt_CreateDate,Tmplt_CreateUser ", " '" + cmbType.Text + "','" + HttpContext.Current.Session["userlastsegment"] + "','" + HttpContext.Current.Session["LastCompany"] + "','" + Convert.ToString(txtShortName.Text).Trim() + "','" + Convert.ToString(disclaimer).Trim() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecalldd", "<script>parent.editwin.close();</script>");

                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User details saved sucessfully');window.location ='frm_TemplateMaster.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "jAlert('Try again later');", true);
                }
            }
            else
            {
                int NoofRowsAffect=oDBEngine.SetFieldValue("master_templateDetails", "  Tmplt_UsedFor='" + cmbType.SelectedItem.Value + "',Tmplt_ShortName='" + Convert.ToString(txtShortName.Text) + "',Tmplt_Content='" + Convert.ToString(disclaimer) + "',Tmplt_ModifyUser='" + Convert.ToString(Session["userid"]) + "',Tmplt_ModifyDate='" + Convert.ToString(oDBEngine.GetDate()) + "'", " Tmplt_ID='" + Convert.ToString(Request.QueryString["id"]) + "' ");
                //Page.ClientScript.RegisterStartupScript(GetType(), "pagecalldd", "<script>parent.editwin.close();</script>");

                if (NoofRowsAffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User details updated sucessfully');window.location ='frm_TemplateMaster.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "jAlert('Try again later');", true);
                }
            }
        }
        protected void ShowForm()
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 19122016

            DataTable DT = oDBEngine.GetDataTable("master_templateDetails", " Tmplt_ID,Tmplt_UsedFor,case when Tmplt_UsedFor='EM' then 'Employee' when Tmplt_UsedFor='CL' then 'Customer' else 'Both' end as  Tmplt_UsedFor ,Tmplt_UsedSegment,Tmplt_UsedCompay,Tmplt_ShortName,Tmplt_Description,Tmplt_Content,Tmplt_CreateUser,Tmplt_CreateDate ", "Tmplt_ID='" + Convert.ToString(Request.QueryString["id"]) + "'");
            if (DT.Rows.Count > 0)
            {
                cmbType.SelectedValue = Convert.ToString(DT.Rows[0]["Tmplt_UsedFor"]);
                txtShortName.Text = Convert.ToString(DT.Rows[0]["Tmplt_ShortName"]);
                FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("FreeTextBox1") as FreeTextBox;
                textEditor.Text = Convert.ToString(DT.Rows[0]["Tmplt_Content"]);
                // FreeTextBox1.Text = DT.Rows[0]["Tmplt_Content"].ToString();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_TemplateMaster.aspx");
        }
    }
}