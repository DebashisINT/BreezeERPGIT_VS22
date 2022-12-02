using System;
using System.Data;
using System.Web;
using System.Web.UI;
//////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_City : ERP.OMS.ViewState_class.VSPage
    {
        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/City.aspx");
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                BindCountry();
                BindState(1);
               
            }
            BindGrid();
        }

        protected void BindCountry()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select cou_id as id,cou_country as name from tbl_master_country order By cou_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", "India");

        }

        protected void BindState(int countryID)        
        {
            CmbState.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
           dtCmb = oGenericMethod.GetDataTable("Select id,state as name From tbl_master_STATE Where countryID=" + countryID + " Order By Name");//+ " Order By state "
            
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                oAspxHelper.Bind_Combo(CmbState, dtCmb, "name", "id", 0);
            }
            else
                CmbState.Enabled = false;
        }

        protected void BindGrid()
        {

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable(@" SELECT city_id,state_id,cou_id,city_name,state,cou_country,City_NSECode,City_BSECode,City_MCXCode,
		                                                   City_MCXSXCode,City_NCDEXCode,City_CDSLCode,City_NSDLCode,City_NDMLCode,City_CVLCode,City_DotExCode
                                                    FROM tbl_master_state INNER JOIN tbl_master_country ON countryId = cou_id 
                                                                          INNER JOIN tbl_master_city ON id = state_id order by city_id desc");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                cityGrid.DataSource = dtFillGrid;
                cityGrid.DataBind();
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
            cityGrid.Columns[6].Visible = false;

            string filename = "District";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "District";
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

        protected void btnSearch(object sender, EventArgs e)
        {
            cityGrid.Settings.ShowFilterRow = true;
        }

        protected void cityGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < cityGrid.Columns.Count; i++)
                    if (cityGrid.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                    return;
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex - 2;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd" || Convert.ToString(Session["PageAccess"]).Trim() == "Delete" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                        {
                            hyperlink.Enabled = true;
                            continue;
                        }
                        else
                        {
                            hyperlink.Enabled = false;
                            continue;
                        }
                    }
                }
            }
        }

        protected void cityGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            if (!cityGrid.IsNewRowEditing)
            {
                ASPxGridViewTemplateReplacement RT = cityGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
                if (Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "Modify" || Convert.ToString(Session["PageAccess"]).Trim() == "All")
                    RT.Visible = true;
                else
                    RT.Visible = false;
            }

        }

        protected void cityGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;

            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

            if (e.Parameters == "s")
                cityGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                cityGrid.FilterExpression = string.Empty;

            if (WhichCall == "savecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                string[,] countrecord = oGenericMethod.GetFieldValue("tbl_master_city", "city_name", "city_name='" + txtcityName.Text.Trim() + "'", 1);

                if (countrecord[0, 0] != "n")
                    cityGrid.JSProperties["cpExists"] = "Exists";
                else
                {
                    insertcount = oGenericMethod.Insert_Table("tbl_master_city", "city_name,state_id", "'" + txtcityName.Text.Trim() + "','" + CmbState.Value + "'");

                    if (insertcount > 0)
                    {
                        cityGrid.JSProperties["cpinsert"] = "Success";
                        BindGrid();
                        cityGrid.DataBind();
                    }
                    else
                        cityGrid.JSProperties["cpinsert"] = "fail";
                }
            }
            if (WhichCall == "updatecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                int stateID = 0;
                if (CmbState.Items.Count > 0)
                    if (CmbState.SelectedItem != null)
                        stateID = Convert.ToInt32(CmbState.SelectedItem.Value.ToString());
                if (stateID != 0)
                {
                    updtcnt = oGenericMethod.Update_Table("tbl_master_city", "city_name='" + txtcityName.Text.Trim() + "',state_id='" + stateID + "'", "city_id=" + WhichType + "");
                    if (updtcnt > 0)
                    {
                        cityGrid.JSProperties["cpUpdate"] = "Success";
                        BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpUpdate"] = "fail";
                }
                else
                    cityGrid.JSProperties["cpUpdateValid"] = "StateInvalid";

            }
            if (WhichCall == "Delete")
            {
                MasterDataCheckingBL masterdata = new MasterDataCheckingBL();
                int id = Convert.ToInt32(WhichType);
                int i = masterdata.DeleteCity(Convert.ToString(id));
                if (i == 1)
                {
                    cityGrid.JSProperties["cpDelete"] = "Succesfully Deleted";
                }
                else
                {
                    cityGrid.JSProperties["cpDelete"] = "Used in other modules. Cannot Delete.";

                }
                //below line comment by sanjib due to check other module 23122016

                //deletecnt = oGenericMethod.Delete_Table("tbl_master_city", "city_id=" + WhichType + "");
                //if (deletecnt > 0)
                //{
                //    cityGrid.JSProperties["cpDelete"] = "Success";
                //    BindGrid();
                //}
                //else
                //    cityGrid.JSProperties["cpDelete"] = "Fail";
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable(@"Select city_name,state_id,(select countryId from tbl_master_state where id=state_id) as country_id,
	                                                                City_NSECode,City_BSECode,City_MCXCode,City_MCXSXCode,City_NCDEXCode,City_CDSLCode,City_NSDLCode,
                                                                    City_NDMLCode,City_CVLCode,City_DotExCode	        			 
                                                              From tbl_master_city Where city_id=" + WhichType + "");

                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string city = Convert.ToString(dtEdit.Rows[0]["city_name"]);
                    string stateID = Convert.ToString(dtEdit.Rows[0]["state_id"]);
                    string countryID = Convert.ToString(dtEdit.Rows[0]["country_id"]);
                    BindState(Convert.ToInt32(countryID));
                    string nsecode = Convert.ToString(dtEdit.Rows[0]["City_NSECode"]);
                    string bsecode = Convert.ToString(dtEdit.Rows[0]["City_BSECode"]);
                    string mcxcode = Convert.ToString(dtEdit.Rows[0]["City_MCXCode"]);
                    string mcxsxcode = Convert.ToString(dtEdit.Rows[0]["City_MCXSXCode"]);
                    string ncdexcode = Convert.ToString(dtEdit.Rows[0]["City_NCDEXCode"]);
                    string cdslcode = Convert.ToString(dtEdit.Rows[0]["City_CDSLCode"]);
                    string nsdlcode = Convert.ToString(dtEdit.Rows[0]["City_NSDLCode"]);
                    string ndmlcode =Convert.ToString( dtEdit.Rows[0]["City_NDMLCode"]);
                    string cvlcode = Convert.ToString(dtEdit.Rows[0]["City_CVLCode"]);
                    string dotexcode = Convert.ToString(dtEdit.Rows[0]["City_DotExCode"]);

                    cityGrid.JSProperties["cpEdit"] = city + "~" + stateID + "~" + countryID + "~" + nsecode + "~" + bsecode + "~" + mcxcode + "~" + mcxsxcode + "~" + ncdexcode + "~" + cdslcode + "~" + nsdlcode + "~" + ndmlcode + "~" + cvlcode + "~" + dotexcode + "~" + WhichType;
                }
            }
        }

        protected void CmbState_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindState")
            {
                int countryID = Convert.ToInt32(Convert.ToString(e.Parameter.Split('~')[1]));
                BindState(countryID);
            }
        }
    }
}