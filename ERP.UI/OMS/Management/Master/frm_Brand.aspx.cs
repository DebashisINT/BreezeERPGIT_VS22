using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;
using System.IO;
using EntityLayer.CommonELS;
using System.Web.Services;
namespace ERP.OMS.Management.Master
{
    public partial class management_frm_Brand : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        public string pageAccess = ""; 
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BrandBl masterBrandBl = new BrandBl();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
       


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/frm_Brand.aspx");

        
            
            BindCountryGrid();

            if(!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }


        protected void BindCountryGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM tbl_master_brand order by Brand_Id Desc");
            AspxHelper oAspxHelper = new AspxHelper();

            GridBrand.DataSource = dtFillGrid;
            GridBrand.DataBind();
           
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridBrand.Settings.ShowFilterRow = true;
        }

        protected void GridBrand_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridBrand.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridBrand.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void GridBrand_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod(); 
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridBrand.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridBrand.FilterExpression = string.Empty;
            }
            if (WhichCall == "Edit")
            {

                DataTable dtBrand = masterBrandBl.GetBrandDetails(Convert.ToInt32(WhichType));
                if (dtBrand.Rows.Count > 0) {
                    GridBrand.JSProperties["cpEdit"] = dtBrand.Rows[0]["Brand_Name"] + "|@|" + dtBrand.Rows[0]["Brand_ContactNo"] + "|@|" + dtBrand.Rows[0]["Brand_Email"];
                }

            }

            if (WhichCall == "updateBrand")
            {

                if (masterBrandBl.updateBrand(txtBrandNBame.Text.Trim(), true, Convert.ToInt32(hdBrandId.Value), txtContactNo.Text, txtEmail.Text))
                {
                 GridBrand.JSProperties["cpMsg"] = "Updated Successfully";
                }
                else{
                GridBrand.JSProperties["cpMsg"] = "please Try-Again";
                }

            }
            if (WhichCall == "saveBrand")
            {
                if (masterBrandBl.insertBrand(txtBrandNBame.Text.Trim(), true,txtContactNo.Text,txtEmail.Text, Convert.ToInt32(Session["UserId"])))
                {
                    GridBrand.JSProperties["cpMsg"] = "Saved Successfully";
                }
                else {
                    GridBrand.JSProperties["cpMsg"] = "please Try-Again";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                int i = masterBrandBl.deleteBrand(Convert.ToInt32(id));
                if (i == 1)
                {
                    GridBrand.JSProperties["cpMsg"] = "Succesfully Deleted";
                }
                else
                {
                    GridBrand.JSProperties["cpMsg"] = "Used in other modules. Cannot Delete.";

                }
                
            }
            BindCountryGrid();
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

        public void bindexport(int Filter)
        {
            GridBrand.Columns[2].Visible = false;

            string filename = "Brand";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Brand";
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
        protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
            if (stream.Length > 0)
                Page.Response.BinaryWrite(stream.ToArray()); 
        }

        [WebMethod]
        public static bool CheckUniqueName(string BrandName, int BrandCode)
        {
            DataTable dt = new DataTable();
            BrandName = BrandName.Replace("'", "''");
            bool IsPresent = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (BrandCode == 0)
            {
                dt = oGeneric.GetDataTable("SELECT COUNT(Brand_Name) AS Brand_Name FROM tbl_master_brand WHERE Brand_Name = '" + BrandName.ToUpper() + "'");
            }
            else
            {
                dt = oGeneric.GetDataTable("SELECT COUNT(Brand_Name) AS Brand_Name FROM tbl_master_brand WHERE Brand_Name = '" + BrandName.ToUpper() + "' and Brand_Id<>" + BrandCode + "");
            }
            //DataTable dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "'");

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Brand_Name"]) > 0)
                {
                    IsPresent = true;
                }
            }
            return IsPresent;
        }
    }
}
