using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_FRM_SearchByPAN : System.Web.UI.Page
    {
        Management_BL oManagement_BL = new Management_BL();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            //txtClientID.Attributes.Add("Onkeyup", "javascript:callValue(this);");
            //ChkBox.Attributes.Add("OnClick", "javascript:CallCheckBox('D',this.checked)");
            //btnSave.Attributes.Add("OnClick", "Javascript:return ValidatePage();");
            //txtClientID.Attributes.Add("onBlur", "javascript:callValueOnBlur(this);");


            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}

            GridBind();

        }
        public void Procedure()
        {
            DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[SEARCH_CONTACT_BYPAN]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@SEARCH_TYPE", cmbDuplicate.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@SEARCH_ENTITY", cmbType.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@EMAIL_LIKE", txtClientID.Text.ToString().Trim());
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(ds);
            //    ViewState["DatasetMain"] = ds;
            //    GridBind();

            //}
            ds = oManagement_BL.SEARCH_CONTACT_BYPAN(Convert.ToString(cmbDuplicate.SelectedItem.Value), Convert.ToString(cmbType.SelectedItem.Value),
                Convert.ToString(txtClientID.Text));
            ViewState["DatasetMain"] = ds;
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
            gridContract.Columns[5].Caption = cmbType.SelectedItem.Text;
            //}
            //else
            //{
            //    gridContract.Columns[5].Caption = "Email";
            //}


        }

        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;

        //    }

        //}

        protected void gridContract_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            //if (e.Parameters.ToString() == "s")
            //{
            //    gridContract.Settings.ShowFilterRow = true;
            //}
            //else if (e.Parameters.ToString() == "All")
            //{
            //    gridContract.FilterExpression = string.Empty;
            //}

            if (e.Parameters.ToString() == "All")
            {
                gridContract.FilterExpression = string.Empty;
            }

            GridBind();
        }
    }
}