using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_clientriskcategory : System.Web.UI.Page
    {
        Audit_Inspection audit_ins = new Audit_Inspection();
        protected void Page_Load(object sender, EventArgs e)
        {
            //TrFilter.Visible = false;
            //Page.ClientScript.RegisterStartupScript(GetType(), "height1", "<script>SearchOpt();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            //txtClientID.Attributes.Add("Onkeyup", "javascript:callValue(this);");
            //ChkBox.Attributes.Add("OnClick", "javascript:CallCheckBox('D',this.checked)");
            btnSave.Attributes.Add("OnClick", "Javascript:return ValidatePage();");
            //txtClientID.Attributes.Add("onBlur", "javascript:callValueOnBlur(this);");


            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            GridBind();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Procedure();
            exporter.WriteXlsToResponse();
        }


        protected void GridBind()
        {

            if (ViewState["DatasetMain"] != null)
            {
                DataSet dsNew = (DataSet)ViewState["DatasetMain"];
                gridContract.DataSource = dsNew.Tables[0];
                gridContract.DataBind();

            }
            //if (cmbType.SelectedItem.Value == "PIN Code")
            //{
            //gridContract.Columns[5].Caption = cmbType.SelectedItem.Text;
            //}
            //else
            //{
            //    gridContract.Columns[5].Caption = "Email";
            //}


        }
        protected void gridContract_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            if (e.Parameters.ToString() == "s")
            {
                gridContract.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters.ToString() == "All")
            {
                gridContract.FilterExpression = string.Empty;
            }

            GridBind();
        }
        protected void cmbDuplicate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void Procedure()
        {
            DataSet ds = new DataSet();
            ds = audit_ins.Riskcategory(cmbDuplicate.SelectedItem.Value);
            ViewState["DatasetMain"] = ds;
            GridBind();
        }

    }
}

