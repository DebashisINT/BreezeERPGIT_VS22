using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_changeuomsetting : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);Multi
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        string id = "";
        string[] id1 = new string[0];
        DataTable DT = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //btnNo.Attributes.Add("Onclick", "javascript:OnCloseButtonClick();");
            id = Request.QueryString["id"].ToString();
            if (id.Contains("("))
            {
                tr_1.Visible = true;
                tr_2.Visible = true;
                tr_3.Visible = true;
                tr_11.Visible = false;
                tr_12.Visible = false;
                id1 = Request.QueryString["id"].Split('(');
                DataTable dt2 = oDBEngine.GetDataTable("select  uom_name from Master_UOM where UOM_id='" + id1[1] + "'");
                DataTable dt3 = oDBEngine.GetDataTable("select  uom_name from Master_UOM where UOM_id='" + id1[2] + "'");
                DataTable dt4 = oDBEngine.GetDataTable("select  Conversion_Multiplier from Config_Conversion where Conversion_id='" + id1[0] + "'");
                litSegment.InnerText = dt2.Rows[0]["uom_name"].ToString();
                litSegment1.InnerText = dt3.Rows[0]["uom_name"].ToString();
                litSegment2.InnerText = dt4.Rows[0]["Conversion_Multiplier"].ToString();
                HiddenField1.Value = id1.ToString();
            }
            else
            {

                txtfromname.Attributes.Add("onkeyup", "FunCallAjaxList(this,event,'ProductFo')");
                txttoname.Attributes.Add("onkeyup", "FunCallAjaxList(this,event,'ProductFo1')");
                tr_1.Visible = false;
                tr_2.Visible = false;
                tr_3.Visible = false;
                tr_11.Visible = true;
                tr_12.Visible = true;
            }

            if (!IsPostBack)
            {
                SetFromTo();
                SetTo();
            }
        }

        public void SetFromTo()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("Master_uom", "ltrim(rtrim(UOM_ID)) code, ltrim(rtrim(UOM_Name)) Name", null);
            txtfromname.DataSource = DT;
            txtfromname.DataMember = "Code";
            txtfromname.DataTextField = "Name";
            txtfromname.DataValueField = "Code";
            txtfromname.DataBind();
        }
        public void SetTo()
        {
            //objEngine
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("Master_uom", "ltrim(rtrim(UOM_ID)) code, ltrim(rtrim(UOM_Name)) Name", null);
            txttoname.DataSource = DT;
            txttoname.DataMember = "Code";
            txttoname.DataTextField = "Name";
            txttoname.DataValueField = "Code";
            txttoname.DataBind();
        }


        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (id != "Add")
            {
                if (txtproduct.Text.Length > 0)
                {
                    int noofrow = oDBEngine.SetFieldValue("Config_Conversion", "Conversion_Multiplier='" + txtproduct.Text + "'", "Conversion_id='" + id1[0] + "'");
                    string p1 = id;// +"/" + "0" + "/" + "1";
                    string popUpscript = "";
                    popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
                    string popUpscript1 = "";
                    popUpscript1 = "alert('Successfully Saved'); window.parent.grid.PerformCallback();window.parent.popup.Hide();";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('Please Insert Multiplier ');$('#Mandatory_txtProduct').css({ 'display': 'block' });</script>");
                }
            } 
            else
            {
                if (txtproduct.Text.Length > 0)
                {
                    DataTable dtfind = oDBEngine.GetDataTable("select * from Config_Conversion where (Conversion_FromUOM='" + txtfromname.SelectedValue + "' and  Conversion_ToUOM='" + txttoname.SelectedValue + "' ) or (Conversion_FromUOM='" + txttoname.SelectedValue + "' and  Conversion_ToUOM='" + txtfromname.SelectedValue + "' )");
                    if (dtfind.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript156", "<script language='javascript'>alert('The Combination Already Exist ');</script>");
                        txtfromname_hidden.Text = "";
                        txttoname_hidden.Text = "";
                        //txtfromname.Text = "";
                        txtproduct.Text = "";
                        //txttoname.Text = "";
                        txtfromname.Focus();
                    }
                    else
                    {
                        int noinsert = oDBEngine.InsurtFieldValue("Config_Conversion", "Conversion_FromUOM,Conversion_ToUOM,Conversion_Multiplier", "'" + txtfromname.SelectedValue + "','" + txttoname.SelectedValue + "','" + txtproduct.Text + "'");
                        string p1 = id;
                        string popUpscript = "";
                        popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
                        ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
                        string popUpscript1 = "";
                        popUpscript1 = "alert('Successfully Saved'); window.parent.grid.PerformCallback();window.parent.popup.Hide();";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript156", "<script language='javascript'>alert('Please Insert Multiplier ');$('#Mandatory_txtProduct').css({ 'display': 'block' });</script>");
                }

            }


        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>window.parent.grid.PerformCallback();window.parent.popup.Hide(); </script>");

        }
    }
}