using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frm_SearchByAddress : System.Web.UI.Page
    {
        Management_BL oManagement_BL = new Management_BL();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
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
            //TrFilter.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "height1", "<script>SearchOpt();</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            //txtClientID.Attributes.Add("Onkeyup", "javascript:callValue(this);");
            //ChkBox.Attributes.Add("OnClick", "javascript:CallCheckBox('D',this.checked)");
            //btnSave.Attributes.Add("OnClick", "Javascript:return ValidatePage();");
            //txtClientID.Attributes.Add("onBlur", "javascript:callValueOnBlur(this);");


            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
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
            //    cmd.CommandText = "[ADDRESSSEARCH]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    //cmd.Parameters.AddWithValue("@SEARCH_TYPE", cmbDuplicate.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@SearchOnly", cmbDuplicate.SelectedItem.Value);
            //    cmd.Parameters.AddWithValue("@Param", txtClientID.Text.ToString().Trim());

            //    //if(cmbDuplicate.SelectedItem.Value.ToString().Trim()=="PANEXEMPT")
            //    //{
            //        //if (txtName.Text.ToString().Trim() != "ADD1" && txtName.Text.ToString().Trim() != "")
            //    cmd.Parameters.AddWithValue("@ADD1", txtName.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "ADD2")
            //    cmd.Parameters.AddWithValue("@ADD2", txtBranchName.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "ADD3")
            //    cmd.Parameters.AddWithValue("@ADD3", txtCode.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "Landmark")
            //    cmd.Parameters.AddWithValue("@Landmark", TxtTCODE.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "Country")
            //    cmd.Parameters.AddWithValue("@Country", txtPAN.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "State")
            //    cmd.Parameters.AddWithValue("@State", txtRelationManager.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "Area")
            //    cmd.Parameters.AddWithValue("@City", txtReferedBy.Text.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Area", txtPhNumber.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "ADD1")
            //    cmd.Parameters.AddWithValue("@Pin", txtpin.Text.ToString().Trim());
            //        //else if (txtName.Text.ToString().Trim() != "ADD1")
            //    //cmd.Parameters.AddWithValue("@Param5", txtName.Text.ToString().Trim());
            //   // }


            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(ds);
            //    ViewState["DatasetMain"] = ds;
            //    GridBind();

            //}
            ds = oManagement_BL.ADDRESSSEARCH(Convert.ToString(cmbDuplicate.SelectedItem.Value), Convert.ToString(txtClientID.Text), Convert.ToString(txtName.Text),
                Convert.ToString(txtBranchName.Text), Convert.ToString(txtCode.Text), Convert.ToString(TxtTCODE.Text), Convert.ToString(txtPAN.Text), Convert.ToString(txtRelationManager.Text),
                Convert.ToString(txtReferedBy.Text), Convert.ToString(txtPhNumber.Text), Convert.ToString(txtpin.Text));
            DataSet ds1 = new DataSet();

            DataTable newDt = new DataTable();

            newDt.Columns.Add("RowNum", typeof(long));
            newDt.Columns.Add("ClientName", typeof(string));
            newDt.Columns.Add("Ucc", typeof(string));
            newDt.Columns.Add("AddRess1", typeof(string));
            newDt.Columns.Add("AddRess2", typeof(string));
            newDt.Columns.Add("AddRess3", typeof(string));
            newDt.Columns.Add("landmark", typeof(string));
            newDt.Columns.Add("CountryName", typeof(string));
            newDt.Columns.Add("StateName", typeof(string));
            newDt.Columns.Add("CityName", typeof(string));
            newDt.Columns.Add("AreaName", typeof(string));
            newDt.Columns.Add("AddPin", typeof(string));

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                long RowCount = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    newDt.Rows.Add(++RowCount, Convert.ToString(ds.Tables[0].Rows[i]["ClientName"]), Convert.ToString(ds.Tables[0].Rows[i]["Ucc"]),
                                  Convert.ToString(ds.Tables[0].Rows[i]["AddRess1"]), Convert.ToString(ds.Tables[0].Rows[i]["AddRess2"]),
                                  Convert.ToString(ds.Tables[0].Rows[i]["AddRess3"]), Convert.ToString(ds.Tables[0].Rows[i]["landmark"]),
                                  Convert.ToString(ds.Tables[0].Rows[i]["CountryName"]), Convert.ToString(ds.Tables[0].Rows[i]["StateName"]),
                                  Convert.ToString(ds.Tables[0].Rows[i]["CityName"]), Convert.ToString(ds.Tables[0].Rows[i]["AreaName"]),
                                  Convert.ToString(ds.Tables[0].Rows[i]["AddPin"]));
                }
            }

            ds1.Tables.Add(newDt);

            ViewState["DatasetMain"] = ds1;
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
            //else 
            if (e.Parameters.ToString() == "All")
            {
                gridContract.FilterExpression = string.Empty;
            }

            GridBind();
        }
        protected void cmbDuplicate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDuplicate.SelectedItem.Value == "PANEXEMPT")
            {
                TrFilter.Visible = true;
            }
            else
                TrFilter.Visible = false;
        }
    }
}