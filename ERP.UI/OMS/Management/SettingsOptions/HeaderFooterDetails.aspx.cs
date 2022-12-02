using System;
using System.Data;
using System.Web;
using System.Web.UI;
using FreeTextBoxControls;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_HeaderFooterDetails : System.Web.UI.Page
    {
        string[,] AllType;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        DataTable dtbl = null;
        string str = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FreeTextBox txtContent = new FreeTextBox();
            txtContent.ID = "txtContent";
            txtContent.Height = 300;
            txtContent.Width = 756;
            FreeTextBoxPlaceHolder.Controls.Add(txtContent);

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            str = Request.QueryString["id"].ToString();
            hdnID.Value = str;


            if (!IsPostBack)
            {
                if (str != "Add")
                {
                    dtbl = oDBEngine.GetDataTable("Master_HeaderFooter", "*", String.Format("HeaderFooter_Id={0}", str));



                    if (dtbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtbl.Rows)
                        {
                            FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("txtContent") as FreeTextBox;


                            ddlHeaderFooter.SelectedValue = dr["HeaderFooter_Type"].ToString();
                            txtHeading.Text = dr["HeaderFooter_ShortName"].ToString();
                            //  txtContent.Text = dr["HeaderFooter_Content"].ToString();
                            textEditor.Text = dr["HeaderFooter_Content"].ToString();
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (str != "Add")
            {
                //if (dtbl.Rows.Count > 0)
                //{
                FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("txtContent") as FreeTextBox;
                string disclaimer = textEditor.Text.ToString().Replace("'", "''");

                string UpdateField = "HeaderFooter_Type='" + ddlHeaderFooter.SelectedValue + "',HeaderFooter_ShortName='" + txtHeading.Text + "',HeaderFooter_Content='" + disclaimer + "'";
                oDBEngine.SetFieldValue("Master_HeaderFooter", UpdateField, "HeaderFooter_Id=" + hdnID.Value);


                // }
            }
            else
            {
                FreeTextBox textEditor = FreeTextBoxPlaceHolder.FindControl("txtContent") as FreeTextBox;
                string disclaimer = textEditor.Text.ToString().Replace("'", "''");

                //string UpdateField = "HeaderFooter_Type='" + ddlHeaderFooter.SelectedValue + "',HeaderFooter_ShortName='" + txtHeading.Text + "',HeaderFooter_Content='" + disclaimer + "'";
                //oDBEngine.SetFieldValue("Master_HeaderFooter", UpdateField, "HeaderFooter_Id=" + hdnID.Value);

                oDBEngine.InsurtFieldValue("Master_HeaderFooter ", " HeaderFooter_Type, HeaderFooter_ShortName, HeaderFooter_Content ", "'" + ddlHeaderFooter.SelectedValue + "','" + txtHeading.Text + "','" + disclaimer + "'");

            }

            Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>parent.editwin.close();</script>");
        }


    }
}