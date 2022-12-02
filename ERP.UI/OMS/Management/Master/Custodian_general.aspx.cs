using System;
using System.Data;
using System.Web;
using System.Web.UI;
////using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Custodian_general : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.Attributes.Add("onclick", "javascript:return Validate();");
                ShowData();
            }
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["KeyVal_InternalID"].ToString() == "0")
            {
                string NameFLetter = txtName.Text.Substring(0, 1).ToUpper();
                string Prefix = "CU" + NameFLetter;
                string InternaliD = oDBEngine.GetInternalId(Prefix, "Master_Custodians", "Custodian_InternalID", " Custodian_InternalID");
                string[,] ShortName = oDBEngine.GetFieldValue("Master_Custodians", "Custodian_ShortName", " Custodian_ShortName='" + txtShortname.Text.Trim().ToString() + "'", 1);
                if (ShortName[0, 0] != "n")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='JavaScript'>alert('ShortName Not Duplicate');</script>");
                    return;
                }
                else
                {
                    string FieldName = "Custodian_InternalID,Custodian_Name,Custodian_ShortName,Custodian_SEBINo,Custodian_MAPIN,Custodian_PAN,Custodian_CreateUser,Custodian_CreateDateTime";
                    string FieldValue = "'" + InternaliD + "','" + txtName.Text + "','" + txtShortname.Text + "','" + txtSebiNo.Text + "','" + txtMapin.Text + "','" + txtPanNo.Text + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'";
                    Int32 NoofEffect = oDBEngine.InsurtFieldValue("master_custodians", FieldName, FieldValue);
                    if (NoofEffect > 0)
                        Response.Redirect("Custodian_general.aspx?id=" + InternaliD, false);
                }
            }
            else
            {
                string FieldName = "Custodian_Name='" + txtName.Text + "',Custodian_SEBINo='" + txtSebiNo.Text + "',Custodian_MAPIN='" + txtMapin.Text + "',Custodian_PAN='" + txtPanNo.Text + "',Custodian_ModifyUser='" + Session["userid"].ToString() + "',Custodian_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'";
                Int32 NoofRowsAffect = oDBEngine.SetFieldValue("master_custodians", FieldName, " Custodian_InternalID='" + Session["KeyVal_InternalID"].ToString() + "'");
            }
        }
        public void ShowData()
        {
            if (Request.QueryString["id"] != "ADD")
            {
                if (Request.QueryString["id"] != null)
                {
                    Session["KeyVal_InternalID"] = Request.QueryString["id"].ToString();
                }
                DataTable dtCustodian = oDBEngine.GetDataTable("master_custodians", "Custodian_Name,Custodian_ShortName,Custodian_SEBINo,Custodian_MAPIN,Custodian_PAN", " Custodian_InternalID='" + Session["KeyVal_InternalID"].ToString() + "'");
                if (dtCustodian.Rows.Count != 0)
                {
                    txtName.Text = dtCustodian.Rows[0][0].ToString();
                    txtShortname.Text = dtCustodian.Rows[0][1].ToString();
                    txtSebiNo.Text = dtCustodian.Rows[0][2].ToString();
                    txtMapin.Text = dtCustodian.Rows[0][3].ToString();
                    txtPanNo.Text = dtCustodian.Rows[0][4].ToString();
                    txtShortname.Enabled = false;
                }

                DisabledTabPages(true);
            }
            else
            {
                Session["KeyVal_InternalID"] = "0";
                DisabledTabPages(false);
            }
        }
        public void DisabledTabPages(bool onOff)
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Visible = onOff;
        }
    }
}
