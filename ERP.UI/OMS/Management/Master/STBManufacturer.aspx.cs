using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class STBManufacturer : System.Web.UI.Page
    {
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.GenericMethod oGenericMethod;
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/STBManufacturer.aspx");

            GridManufacturer.JSProperties["cpEdit"] = null;
            GridManufacturer.JSProperties["cpinsert"] = null;
            GridManufacturer.JSProperties["cpUpdate"] = null;
            GridManufacturer.JSProperties["cpDelete"] = null;
            GridManufacturer.JSProperties["cpExists"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            BindManufacturerGrid();

            if (!IsPostBack)
            {
                Session["exportval"] = null;
            }
        }

        protected void BindManufacturerGrid()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable("SELECT * FROM SRV_master_Manufacturer order by Manufacturer_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                GridManufacturer.DataSource = dtFillGrid;
                GridManufacturer.DataBind();
            }
        }


        protected void btnSearch(object sender, EventArgs e)
        {
            GridManufacturer.Settings.ShowFilterRow = true;
        }

        protected void GridManufacturer_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!GridManufacturer.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = GridManufacturer.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }
        protected void GridManufacturer_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int deletecont = 0;
            int updtcnt = 0;
            int insertcount = 0;
            GridManufacturer.JSProperties["cpEdit"] = null;
            GridManufacturer.JSProperties["cpinsert"] = null;
            GridManufacturer.JSProperties["cpUpdate"] = null;
            GridManufacturer.JSProperties["cpDelete"] = null;
            GridManufacturer.JSProperties["cpExists"] = null;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                WhichType = Convert.ToString(e.Parameters).Split('~')[1];
            if (e.Parameters == "s")
                GridManufacturer.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
            {
                GridManufacturer.FilterExpression = string.Empty;
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable("select Manufacturer_Id,Manufacturer_Name from SRV_master_Manufacturer where Manufacturer_Id=" + WhichType + "");
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string Manufacturer = Convert.ToString(dtEdit.Rows[0]["Manufacturer_Name"]);
                    int Manufacturer_Id = Convert.ToInt32(dtEdit.Rows[0]["Manufacturer_Id"]);
                    GridManufacturer.JSProperties["cpEdit"] = Manufacturer + "~" + Manufacturer_Id;
                }
            }

            if (WhichCall == "updateManufacturer")
            {
                updtcnt = oGenericMethod.Update_Table("SRV_master_Manufacturer", "Manufacturer_Name='" + txtManufacturerName.Text + "'", "Manufacturer_Id=" + WhichType + "");
                if (updtcnt > 0)
                {
                    GridManufacturer.JSProperties["cpUpdate"] = "Success";
                    BindManufacturerGrid();
                }
                else
                    GridManufacturer.JSProperties["cpUpdate"] = "fail";

            }
            if (WhichCall == "saveManufacturer")
            {
                string[,] countrecord = oGenericMethod.GetFieldValue("SRV_master_Manufacturer", "Manufacturer_Name", "Manufacturer_Name='" + txtManufacturerName.Text + "'", 1);
                if (countrecord[0, 0] != "n")
                {
                    GridManufacturer.JSProperties["cpExists"] = "Exists";
                }
                else
                {

                    insertcount = oGenericMethod.Insert_Table("SRV_master_Manufacturer", "Manufacturer_Name,CREATED_ON,CREATED_BY",
                       "'" + txtManufacturerName.Text + "','" + oGenericMethod.GetDate(110) + "'," + Session["userid"]);
                    if (insertcount > 0)
                    {
                        GridManufacturer.JSProperties["cpinsert"] = "Success";
                        BindManufacturerGrid();
                    }
                    else
                        GridManufacturer.JSProperties["cpinsert"] = "fail";
                }
            }

            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                int i = masterdata.DeleteManufacturer(Convert.ToString(id));
                if (i == 1)
                {
                    GridManufacturer.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    GridManufacturer.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";

                }

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

        public void bindexport(int Filter)
        {
            GridManufacturer.Columns[2].Visible = false;

            string filename = "Manufacturer";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Manufacturer";
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
            //Page.Response.End();
        }

        [WebMethod]
        public static string CheckWorkingRoster(string module_ID)
        {
            CommonBL ComBL = new CommonBL();
            string STBTransactionsRestrictBeyondTheWorkingDays = ComBL.GetSystemSettingsResult("STBTransactionsRestrictBeyondTheWorkingDays");
            string output = string.Empty;
            try
            {
                if (HttpContext.Current.Session["userid"] != null)
                {
                    if (STBTransactionsRestrictBeyondTheWorkingDays.ToUpper() == "YES")
                    {
                        ProcedureExecute proc = new ProcedureExecute("PRC_STBModuleRosterStatus");
                        proc.AddPara("@ModuleId", module_ID);
                        DataSet ds = proc.GetDataSet();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "true")
                            {
                                output = "true";
                            }
                            else if (ds.Tables[0].Rows[0]["returnvalue"].ToString() == "false")
                            {

                                output = "false~" + ds.Tables[1].Rows[0]["BeginTime"].ToString() + "~" + ds.Tables[1].Rows[0]["EndTime"].ToString();
                            }

                        }
                        else
                        {
                            output = "false";
                        }
                    }
                    else
                    {
                        output = "true";
                    }
                }
                else
                {
                    output = "Logout";
                }
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
    }
}