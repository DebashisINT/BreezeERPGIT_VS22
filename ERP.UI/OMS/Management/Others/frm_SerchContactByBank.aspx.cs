using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Others
{
    public partial class management_Others_frm_SerchContactByBank : System.Web.UI.Page
    {
        BusinessLogicLayer.Others oOthers = new BusinessLogicLayer.Others();

       // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DBEngine oDBEngine = new DBEngine();
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
            //Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");

            btnSave.Attributes.Add("OnClick", "Javascript:return ValidatePage();");



            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}

            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Pageld", "<script>PageLoad();</script>");

            }

            GridBind();

        }
        public void Procedure()
        {


            DataSet ds = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[SEARCH_CONTACT_BANK]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@SEARCH_TYPE", cmbDuplicate.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@BNK_LIKE", txtClientID.Text);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(ds);
            //    ViewState["DatasetMain"] = ds;
            //    GridBind();

            //}
            ds = oOthers.SearchContactBank(cmbDuplicate.SelectedItem.Value, txtClientID.Text);
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
                if (dsNew.Tables[0].Rows.Count > 0)
                {
                    gridContract.DataSource = dsNew.Tables[0];
                    gridContract.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Script1", "alert('No Record Found');", true);
                }

            }


        }



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