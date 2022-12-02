using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
//////using DevExpress.Web.ASPxClasses;
//using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_City : System.Web.UI.Page
    {
        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

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
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
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
                                                                          INNER JOIN tbl_master_city ON id = state_id");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtFillGrid.Rows.Count > 0)
            {
                cityGrid.DataSource = dtFillGrid;
                cityGrid.DataBind();
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
                        if (Session["PageAccess"].ToString().Trim() == "DelAdd" || Session["PageAccess"].ToString().Trim() == "Delete" || Session["PageAccess"].ToString().Trim() == "All")
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
                if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
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

            string WhichCall = e.Parameters.ToString().Split('~')[0];
            string WhichType = null;
            if (e.Parameters.ToString().Contains("~"))
                if (e.Parameters.ToString().Split('~')[1] != "")
                    WhichType = e.Parameters.ToString().Split('~')[1];

            if (e.Parameters == "s")
                cityGrid.Settings.ShowFilterRow = true;
            if (e.Parameters == "All")
                cityGrid.FilterExpression = string.Empty;

            if (WhichCall == "savecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                string[,] countrecord = oGenericMethod.GetFieldValue("tbl_master_city", "city_name", "city_name='" + txtcityName.Text + "'", 1);

                if (countrecord[0, 0] != "n")
                    cityGrid.JSProperties["cpExists"] = "Exists";
                else
                {
                    insertcount = oGenericMethod.Insert_Table("tbl_master_city", "city_name,state_id,City_NSECode,City_BSECode,City_MCXCode,City_MCXSXCode,City_NCDEXCode,City_CDSLCode,City_NSDLCode,City_NDMLCode,City_CVLCode,City_DotExCode",
                       "'" + txtcityName.Text + "','" + CmbState.SelectedItem.Value + "','" + txtNseCode.Text + "','" + txtBseCode.Text + "','" + txtMcxCode.Text + "','" + txtMcsxCode.Text + "','" + txtNcdexCode.Text + "','" + txtCdslCode.Text
                           + "','" + txtNsdlCode.Text + "','" + txtNdmlCode.Text + "','" + txtCvlCode.Text + "','" + txtDotexCode.Text + "'");

                    if (insertcount > 0)
                    {
                        cityGrid.JSProperties["cpinsert"] = "Success";
                        BindGrid();
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
                    updtcnt = oGenericMethod.Update_Table("tbl_master_city", "city_name='" + txtcityName.Text + "',state_id='" + stateID + "',City_NSECode='" + txtNseCode.Text + "',City_BSECode='" + txtBseCode.Text + "',City_MCXCode='" + txtMcxCode.Text + "',City_MCXSXCode='" + txtMcsxCode.Text + "',City_NCDEXCode='" + txtNcdexCode.Text + "',City_CDSLCode='" + txtCdslCode.Text + "',City_NSDLCode='" + txtNsdlCode.Text + "',City_NDMLCode='" + txtNdmlCode.Text + "',City_CVLCode='" + txtCvlCode.Text + "',City_DotExCode='" + txtDotexCode.Text + "'", "city_id=" + WhichType + "");
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
                deletecnt = oGenericMethod.Delete_Table("tbl_master_city", "city_id=" + WhichType + "");
                if (deletecnt > 0)
                {
                    cityGrid.JSProperties["cpDelete"] = "Success";
                    BindGrid();
                }
                else
                    cityGrid.JSProperties["cpDelete"] = "Fail";
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable(@"Select city_name,state_id,(select countryId from tbl_master_state where id=state_id) as country_id,
	                                                                City_NSECode,City_BSECode,City_MCXCode,City_MCXSXCode,City_NCDEXCode,City_CDSLCode,City_NSDLCode,
                                                                    City_NDMLCode,City_CVLCode,City_DotExCode	        			 
                                                              From tbl_master_city Where city_id=" + WhichType + "");

                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string city = dtEdit.Rows[0]["city_name"].ToString();
                    string stateID = dtEdit.Rows[0]["state_id"].ToString();
                    string countryID = dtEdit.Rows[0]["country_id"].ToString();

                    string nsecode = dtEdit.Rows[0]["City_NSECode"].ToString();
                    string bsecode = dtEdit.Rows[0]["City_BSECode"].ToString();
                    string mcxcode = dtEdit.Rows[0]["City_MCXCode"].ToString();
                    string mcxsxcode = dtEdit.Rows[0]["City_MCXSXCode"].ToString();
                    string ncdexcode = dtEdit.Rows[0]["City_NCDEXCode"].ToString();
                    string cdslcode = dtEdit.Rows[0]["City_CDSLCode"].ToString();
                    string nsdlcode = dtEdit.Rows[0]["City_NSDLCode"].ToString();
                    string ndmlcode = dtEdit.Rows[0]["City_NDMLCode"].ToString();
                    string cvlcode = dtEdit.Rows[0]["City_CVLCode"].ToString();
                    string dotexcode = dtEdit.Rows[0]["City_DotExCode"].ToString();

                    cityGrid.JSProperties["cpEdit"] = city + "~" + stateID + "~" + countryID + "~" + nsecode + "~" + bsecode + "~" + mcxcode + "~" + mcxsxcode + "~" + ncdexcode + "~" + cdslcode + "~" + nsdlcode + "~" + ndmlcode + "~" + cvlcode + "~" + dotexcode + "~" + WhichType;
                }
            }
        }

        protected void CmbState_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindState")
            {
                int countryID = Convert.ToInt32(e.Parameter.Split('~')[1].ToString());
                BindState(countryID);
            }
        }
    }
}