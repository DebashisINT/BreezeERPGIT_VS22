using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class Schema_master :ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Sqlfinyear.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlcomp.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqlbranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            FillGrid();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!Page.IsPostBack)
                {
                    //SchemaGrid.Columns["finyearid"].Visible = false;
                    SchemaGrid.Columns[9].Visible = false;

                    //To check User Rights 
                    Session["exportval"] = null;
                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Schema_master.aspx");
                    //SchemaGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesSchemaMasterSchemaGrid";
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesSchemaMasterSchemaGrid');</script>");

                }
            }
        }

     

        #region Export event
       
        public void bindexport(int Filter)
        {
            SchemaGrid.Columns[16].Visible = false;
            //SchemaGrid.Columns[11].Visible = false;
            //SchemaGrid.Columns[12].Visible = false;
            string filename = "Numbering Scheme";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Numbering Scheme";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        #endregion

        #region grid event



        protected void SchemaGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] CallVal = Convert.ToString(e.Parameters).Split('~');

            if (Convert.ToString(CallVal[0]) == "Delete")
            {
                DataTable dtEx = oDBEngine.GetDataTable("select COUNT(*) as 'cnt' from tbl_master_company where onrole_schema_id=" + Convert.ToString(CallVal[1]) + " or offrole_schema_id=" + Convert.ToString(CallVal[1]) + "");
                if (Convert.ToString(dtEx.Rows[0]["cnt"])=="0")
                {
                    string id = Convert.ToString(CallVal[1]);


                    int checkinvalue = masterChecking.DeleteNumberingScheme(Convert.ToInt32(id));

                    if (checkinvalue > 0)
                    {

                        DataSet dsEmail = new DataSet();
                        //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                        String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                        SqlConnection con = new SqlConnection(conn);
                        SqlCommand cmd3 = new SqlCommand("prc_Schemamaster", con);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@schemaID", Convert.ToInt64(id));

                        cmd3.Parameters.AddWithValue("@actiontype", 3);

                        cmd3.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd3;
                        Adap.Fill(dsEmail);
                        cmd3.Dispose();
                        con.Dispose();
                        GC.Collect();
                        FillGrid();
                    }
                    else
                    {
                        SchemaGrid.JSProperties["cpDelete"] = "FK";
                        SchemaGrid.JSProperties["cpMsg"] = Convert.ToString(ConfigurationManager.AppSettings["DeleteErrorMessage"]);
                    }
                }
                else {
                    SchemaGrid.JSProperties["cpDelete"] = "FK";
                    SchemaGrid.JSProperties["cpMsg"] = Convert.ToString(ConfigurationManager.AppSettings["DeleteErrorMessage"]);
                    
                }

                SchemaGrid.CancelEdit();

            }


        }
  
        #endregion

        #region privateevent
        private void FillGrid()
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("exec prc_Schemamaster @actiontype=0");//it is use for action like 0/1/2/3/4 ->select/UPDATE/insert/delete/selectbyid

            SchemaGrid.DataSource = dt.DefaultView;
            SchemaGrid.DataBind();
        }

        #endregion
    }
}