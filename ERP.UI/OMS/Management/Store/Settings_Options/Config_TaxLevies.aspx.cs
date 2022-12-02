using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
////using DevExpress.Web.ASPxClasses;
using DevExpress.Web;
using BusinessLogicLayer;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;

namespace ERP.OMS.Management.Store.Settings_Options
{
    public partial class Management_Accounts_Master_Config_TaxLevies : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {

        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.TaxSchemeBl oTaxSchemeBl = new TaxSchemeBl();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
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
            //Remove the next page session debjyoti
            Session.Remove("ProductTaxData");

            // txtTaxRates_DateFrom.Value = DateTime.Now;
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Store/Settings_Options/Config_TaxLevies.aspx");
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            cityGrid.JSProperties["cpInsertionValid"] = null;

            // Session Handel by Sudip 20-12-2016

            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    BindCmbTaxRates_TaxCode();
                    BindCmbTaxRates_RateOrSlab();
                    BindCmbTaxSlab_Code();
                    BindCmbTaxRates_ProductClass();
                    BindCmbTaxRates_SurchargeApplicable();
                    BindCmbtxtTaxRates_SurchargeCriteria();
                    BindCmbTaxRates_SurchargeOn();
                    BindCountry();
                    BindState(1);
                    BindCity(1);
                    txtTaxRates_DateFrom.Value = DateTime.Now;

                    Session["exportval"] = null;
                }
                BindGrid();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void BindCountry()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select cou_id as id,cou_country as name from tbl_master_country order By cou_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", 0);

            CmbCountryName.Items.Insert(0, new DevExpress.Web.ListEditItem("Any", "0"));

        }
        protected void BindCmbTaxRates_TaxCode()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Name as name from Master_Taxes order By Taxes_Code");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxRates_TaxCode, dtCmb, "name", "id", 0);

            CmbTaxRates_TaxCode.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));      ///.Insert(0, new ListItem("Select Country", "0"));
        }
        protected void BindCmbTaxSlab_Code()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select distinct TaxSlab_Code from Master_TaxSlab order By TaxSlab_Code");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxSlab_Code, dtCmb, "TaxSlab_Code", "TaxSlab_Code", 0);

            CmbTaxSlab_Code.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));      ///.Insert(0, new ListItem("Select Country", "0"));
        }
        protected void BindCmbTaxRates_RateOrSlab()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "R";
            drsession["name"] = "Rate";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "S";
            drsession["name"] = "Slab";
            dtCmb.Rows.Add(drsession);
            //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxRates_RateOrSlab, dtCmb, "name", "id", 0);

            CmbTaxRates_RateOrSlab.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));

        }
        protected void BindCmbTaxRates_SurchargeApplicable()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "Y";
            drsession["name"] = "Yes";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "N";
            drsession["name"] = "No";
            dtCmb.Rows.Add(drsession);
            //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxRates_SurchargeApplicable, dtCmb, "name", "id", 0);

            CmbTaxRates_SurchargeApplicable.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));

        }
        protected void BindCmbTaxRates_SurchargeOn()
        {
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "F";
            drsession["name"] = "Full amount";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "D";
            drsession["name"] = "Differential amount";
            dtCmb.Rows.Add(drsession);
            //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxRates_SurchargeOn, dtCmb, "name", "id", 0);

            CmbTaxRates_SurchargeOn.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        }
        protected void BindCmbtxtTaxRates_SurchargeCriteria()
        {
            DataTable dtCmb = new DataTable();

            dtCmb.Columns.Add("id");
            dtCmb.Columns.Add("name");
            DataRow drsession = dtCmb.NewRow();
            drsession["id"] = "G";
            drsession["name"] = "Gross Value";
            dtCmb.Rows.Add(drsession);

            drsession = dtCmb.NewRow();
            drsession["id"] = "T";
            drsession["name"] = "Tax component";
            dtCmb.Rows.Add(drsession);
            //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Code as name from Master_Taxes order By Taxes_Code");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbtxtTaxRates_SurchargeCriteria, dtCmb, "name", "id", 0);

            CmbtxtTaxRates_SurchargeCriteria.Items.Insert(0, new DevExpress.Web.ListEditItem("", "0"));
        }
        protected void BindCmbTaxRates_ProductClass()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select ProductClass_ID as id,ProductClass_Name as name from Master_ProductClass order By ProductClass_Code");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
                oAspxHelper.Bind_Combo(CmbTaxRates_ProductClass, dtCmb, "name", "id", 0);

            CmbTaxRates_ProductClass.Items.Insert(0, new DevExpress.Web.ListEditItem("Any", "0"));

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

            CmbState.Items.Insert(0, new DevExpress.Web.ListEditItem("Any", "0"));


        }
        protected void BindCity(int stateID)
        {
            CmbCity.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select city_id,city_name From tbl_master_city Where state_id=" + stateID + " Order By city_name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                oAspxHelper.Bind_Combo(CmbCity, dtCmb, "city_name", "city_id", 0);
            }
            //else
            //    CmbCity.Enabled = false;

            CmbCity.Items.Insert(0, new DevExpress.Web.ListEditItem("Any", "0"));
        }
        protected void BindGrid()
        {

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtFillGrid = new DataTable();
            dtFillGrid = oGenericMethod.GetDataTable(@" select [TaxRates_ID]
                                                          ,[TaxRates_TaxCode]
                                                          , [Taxes_Name] as [Taxes_Code]
                                                          , case when [TaxRates_ProductClass]='0' Then 'Any' else mpc.[ProductClass_Name] end as [ProductClass_Code]
                                                          , case when [TaxRates_Country]='0' Then 'Any' else tmcoun.[cou_country] end as [cou_country]
                                                          ,case when [TaxRates_State]='0' Then 'Any' else tms.[state] end as [state]
                                                          ,case when [TaxRates_City]='0' Then 'Any' else tmcity.[city_name] end as [city_name]
                                                          ,[TaxRates_ProductClass]
                                                          ,[TaxRates_State]
                                                          ,[TaxRates_Country]
                                                          ,[TaxRates_City]
                                                          ,tmcoun.[cou_id]     
                                                          , convert(NVARCHAR, [TaxRates_DateFrom], 106) as [TaxRates_DateFrom]   
                                                          ,[TaxRates_DateTo]
                                                          , case when TaxRates_RateOrSlab = 'R' then 'Rate'  when TaxRates_RateOrSlab = 'S' then 'Slab' end as  [TaxRates_RateOrSlab]
                                                          ,[TaxRates_Rate]
                                                          ,[TaxRates_MinAmount]
                                                          ,[TaxRates_SlabCode]
                                                          ,[TaxRates_SurchargeApplicable]
                                                          ,[TaxRates_SurchargeCriteria]
                                                          ,[TaxRates_SurchargeAbove]
                                                          ,[TaxRates_SurchargeOn]
                                                          ,[Taxes-SurchargeRate]
                                                          ,[TaxRates_MainAccount]
                                                          ,[TaxRates_SubAccount]
                                                          ,[TaxRates_CreateUser]
                                                          ,[TaxRates_CreateTime]
                                                          ,[TaxRates_ModifyUser]
                                                          ,[TaxRates_ModifyTime]
                                                           ,TaxRatesSchemeName
                                                         
                                                     from Config_TaxRates ctr 

                                                         left JOIN tbl_master_country tmcoun ON ctr.TaxRates_Country = tmcoun.cou_id 
                                                         left join tbl_master_state tms on  ctr.TaxRates_State=tms.id
                                                         left join tbl_master_city tmcity on ctr.TaxRates_City=tmcity.city_id 
                                                         left join Master_Taxes mt on ctr.TaxRates_TaxCode=mt.Taxes_ID 
                                                         left join Master_ProductClass mpc on ctr.TaxRates_ProductClass=mpc.ProductClass_ID 
                                                          order by ctr.TaxRates_id  desc ");

            AspxHelper oAspxHelper = new AspxHelper();
             
                cityGrid.DataSource = dtFillGrid;
                cityGrid.DataBind();
            
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
            cityGrid.Columns[11].Visible = false;
            string filename = "Config TaxLevies";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Config TaxLevies";
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
            // Replace .ToString() with Convert.ToString(..) By Sudip on 20-12-2016

            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            cityGrid.JSProperties["cpInsertionValid"] = null;


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

            int cityID = 0;
            string strSlabCode = "0";

            if (WhichCall == "savecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                int TaxRates_TaxCode = Convert.ToInt32(Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value));

                //if (CmbCity.Items.Count > 0)
                //    if (CmbCity.SelectedItem != null)
                //        cityID = Convert.ToInt32(CmbCity.SelectedItem.Value.ToString());

                /*Code  Added  By Sudip on 14122016*/

                if (CmbCity.Items.Count > 0)
                {
                    if (hdnCityId.Value != null && hdnCityId.Value != "")
                    {
                        cityID = Convert.ToInt32(Convert.ToString(hdnCityId.Value));
                    }
                }

                if (CmbTaxSlab_Code.Items.Count > 0)
                {
                    if (CmbTaxSlab_Code.SelectedItem != null)
                    {
                        strSlabCode = Convert.ToString(CmbTaxSlab_Code.SelectedItem.Value).Trim();
                    }
                }

                //-----------End

                if (TaxRates_TaxCode != 0)
                {
                    //if (CmbCity.SelectedItem.Value.ToString() != "")
//                    if (Convert.ToString(hdnCityId.Value) != "")
//                    {
//                        if (txtTaxRates_DateFrom.Text.Trim() != "" && txtTaxRates_DateFrom.Text.Trim() != null)
//                        {


//                            DataTable dtEdit = oGenericMethod.GetDataTable(@"SELECT [TaxRates_ID]
//                                                          ,[TaxRates_TaxCode]
//                                                          ,[TaxRates_ProductClass]
//                                                          ,[TaxRates_Country]
//                                                          ,[TaxRates_State]
//                                                          ,[TaxRates_City]
//                                                          ,[TaxRates_DateFrom]
//                                                      FROM [dbo].[Config_TaxRates] ctr
// 
//                                                      Where TaxRates_TaxCode='" + Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value).Trim() +
//                                                                       "' and TaxRates_ProductClass='" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value).Trim() +
//                                                                       "' and TaxRates_Country='" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() +
//                                                                       "' and TaxRates_State= '" + Convert.ToString(CmbState.SelectedItem.Value).Trim() +
//                                                                       "' and TaxRates_City='" + Convert.ToString(cityID).Trim() + "'  order by TaxRates_DateFrom desc");
//                            if (dtEdit.Rows.Count > 0)
//                            {
//                                string date = Convert.ToString(dtEdit.Rows[0]["TaxRates_DateFrom"]);
//                                if (Convert.ToDateTime(Convert.ToString(txtTaxRates_DateFrom.Value)).Date > Convert.ToDateTime(date).Date)
//                                {
//                                    string sTaxRates_MainAccount = Convert.ToString(hndTaxRates_MainAccount_hidden.Value).Trim();

//                                    string sTaxRates_SubAccount = "NULL";
//                                    if (hndTaxRates_SubAccount_hidden.Value != null && hndTaxRates_SubAccount_hidden.Value != "")
//                                    {
//                                        sTaxRates_SubAccount = Convert.ToString(hndTaxRates_SubAccount_hidden.Value).Trim();
//                                    }

//                                    updtcnt = oGenericMethod.Update_Table("Config_TaxRates", "TaxRates_DateTo ='" + Convert.ToDateTime(Convert.ToString(txtTaxRates_DateFrom.Value)).AddDays(-1) + "'", "TaxRates_ID=" + Convert.ToString(dtEdit.Rows[0]["TaxRates_ID"]) + "");

//                                    insertcount = oGenericMethod.Insert_Table("Config_TaxRates", "TaxRates_TaxCode,TaxRates_ProductClass,TaxRates_Country,TaxRates_State,TaxRates_City,TaxRates_DateFrom,TaxRates_RateOrSlab,TaxRates_Rate,TaxRates_MinAmount,TaxRates_SlabCode,TaxRates_SurchargeApplicable,TaxRates_SurchargeCriteria,TaxRates_SurchargeAbove,TaxRates_SurchargeOn,[Taxes-SurchargeRate],TaxRates_CreateUser,TaxRates_CreateTime,TaxRates_MainAccount,TaxRates_SubAccount,TaxRatesSchemeName,Exempted",
//                                            "'" + Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbState.SelectedItem.Value).Trim() + "','" + Convert.ToString(cityID).Trim() +
//                                            "','" + Convert.ToString(txtTaxRates_DateFrom.Value).Trim() + "','" + Convert.ToString(CmbTaxRates_RateOrSlab.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxRates_Rate.Text).Trim() + "','" + Convert.ToString(txtTaxRates_MinAmount.Text).Trim() + "','" + Convert.ToString(strSlabCode).Trim() + "','"
//                                            + Convert.ToString(CmbTaxRates_SurchargeApplicable.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbtxtTaxRates_SurchargeCriteria.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxRates_SurchargeAbove.Text).Trim() + "','" + Convert.ToString(CmbTaxRates_SurchargeOn.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "',getdate(),'" + sTaxRates_MainAccount.Trim() + "','" + sTaxRates_SubAccount.Trim() + "','" + txt_schemename.Text+ "',"+rdpExempted.SelectedItem.Value+"");

//                                    if (insertcount > 0)
//                                    {
//                                        cityGrid.JSProperties["cpinsert"] = "Success";
//                                        BindGrid();
//                                    }
//                                    else
//                                        cityGrid.JSProperties["cpinsert"] = "fail";
//                                }
//                                else
//                                    cityGrid.JSProperties["cpinsert"] = "Invalid";
//                            }
//                            else
//                            {
//                                
                               string sTaxRates_MainAccount = Convert.ToString(hndTaxRates_MainAccount_hidden.Value).Trim();
                                string sTaxRates_SubAccount = "NULL";
                                if (hndTaxRates_SubAccount_hidden.Value != null && hndTaxRates_SubAccount_hidden.Value != "")
                                {
                                    sTaxRates_SubAccount = Convert.ToString(hndTaxRates_SubAccount_hidden.Value).Trim();
                                }

                                string TaxRates_MinAmount = txtTaxRates_MinAmount.Text == "" ? "0.0" : txtTaxRates_MinAmount.Text;

                                insertcount = oGenericMethod.Insert_Table("Config_TaxRates", "TaxRates_TaxCode,TaxRates_ProductClass,TaxRates_Country,TaxRates_State,TaxRates_City,TaxRates_DateFrom,TaxRates_RateOrSlab,TaxRates_Rate,TaxRates_MinAmount,TaxRates_SlabCode,TaxRates_SurchargeApplicable,TaxRates_SurchargeCriteria,TaxRates_SurchargeAbove,TaxRates_SurchargeOn,[Taxes-SurchargeRate],TaxRates_CreateUser,TaxRates_CreateTime,TaxRates_MainAccount,TaxRates_SubAccount,TaxRatesSchemeName,Exempted",
                                        "'" + Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbState.SelectedItem.Value).Trim() + "','" + Convert.ToString(cityID).Trim() +
                                        "','" + Convert.ToString(txtTaxRates_DateFrom.Value).Trim() + "','" + Convert.ToString(CmbTaxRates_RateOrSlab.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxRates_Rate.Text).Trim() + "','"
                                        + Convert.ToDecimal(TaxRates_MinAmount) + "','"
                                        + Convert.ToString(strSlabCode).Trim() + "','" + Convert.ToString(CmbTaxRates_SurchargeApplicable.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbtxtTaxRates_SurchargeCriteria.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxRates_SurchargeAbove.Text).Trim() + "','" + Convert.ToString(CmbTaxRates_SurchargeOn.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "',getdate(),'" + sTaxRates_MainAccount.Trim() + "','" + sTaxRates_SubAccount.Trim() + "','" + txt_schemename.Text + "',"+rdpExempted.SelectedItem.Value+"");

                                if (insertcount > 0)
                                {
                                    cityGrid.JSProperties["cpinsert"] = "Success";
                                    BindGrid();
                                }
                                else
                                    cityGrid.JSProperties["cpinsert"] = "fail";
                }
                else
                    cityGrid.JSProperties["cpUpdateValid"] = "TaxCodeInvalid";
            }
            if (WhichCall == "updatecity")
            {
                // oGenericMethod = new GenericMethod();
                oGenericMethod = new BusinessLogicLayer.GenericMethod();

                if (CmbCity.Items.Count > 0)
                {
                    if (hdnCityId.Value != null && hdnCityId.Value != "")
                    {
                        cityID = Convert.ToInt32(Convert.ToString(hdnCityId.Value));
                    }
                }

                if (CmbTaxSlab_Code.Items.Count > 0)
                {
                    if (CmbTaxSlab_Code.SelectedItem != null)
                    {
                        strSlabCode = Convert.ToString(CmbTaxSlab_Code.SelectedItem.Value).Trim();
                    }
                }

                int stateID = 0;
                if (CmbState.Items.Count > 0)
                    if (CmbState.SelectedItem != null)
                        stateID = Convert.ToInt32(Convert.ToString(CmbState.SelectedItem.Value));

                string strSurchargeCriteria = "0";
                if (CmbtxtTaxRates_SurchargeCriteria.Items.Count > 0)
                    if (CmbtxtTaxRates_SurchargeCriteria.SelectedItem != null)
                        strSurchargeCriteria = Convert.ToString(CmbtxtTaxRates_SurchargeCriteria.SelectedItem.Value);


                string sTaxRates_MainAccount = Convert.ToString(hndTaxRates_MainAccount_hidden.Value).Trim();
                string sTaxRates_SubAccount = "NULL";
                if (hndTaxRates_SubAccount_hidden.Value != null && hndTaxRates_SubAccount_hidden.Value != "")
                {
                    sTaxRates_SubAccount = Convert.ToString(hndTaxRates_SubAccount_hidden.Value).Trim();
                }

                //if (stateID != 0)  //comment by sanjib due to logic changed.
                //{
                    if (txtTaxRates_DateFrom.Value != null)
                    {
                        /*Code  Added  By Sudip on 14122016 for Edit function*/

                        updtcnt = oGenericMethod.Update_Table("Config_TaxRates", "TaxRates_MainAccount='" + sTaxRates_MainAccount + "',TaxRates_SubAccount=" + sTaxRates_SubAccount + ",Exempted=" + rdpExempted.SelectedItem.Value + ",TaxRatesSchemeName='" + txt_schemename.Text + "',TaxRates_TaxCode ='" + Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value).Trim() + "', TaxRates_ProductClass ='" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value) + "', TaxRates_DateFrom ='" + Convert.ToString(txtTaxRates_DateFrom.Value).Trim() + "', TaxRates_RateOrSlab  ='" + Convert.ToString(CmbTaxRates_RateOrSlab.SelectedItem.Value).Trim() + "', TaxRates_Rate  ='" + Convert.ToDecimal(Convert.ToString(txtTaxRates_Rate.Text)) + "', TaxRates_MinAmount  ='" + Convert.ToString(txtTaxRates_MinAmount.Text).Trim() + "', TaxRates_SlabCode  ='" + Convert.ToString(strSlabCode).Trim() + "', TaxRates_SurchargeApplicable ='" + Convert.ToString(CmbTaxRates_SurchargeApplicable.SelectedItem.Value).Trim() + "', TaxRates_SurchargeCriteria  ='" + Convert.ToString(strSurchargeCriteria).Trim() + "', TaxRates_SurchargeAbove  ='" + Convert.ToString(txtTaxRates_SurchargeAbove.Text).Trim() + "', TaxRates_SurchargeOn  ='" + Convert.ToString(CmbTaxRates_SurchargeOn.SelectedItem.Value).Trim() + "', [Taxes-SurchargeRate]  ='" + Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() + "', TaxRates_State ='" + Convert.ToString(CmbState.SelectedItem.Value).Trim() + "', TaxRates_City ='" + Convert.ToString(cityID).Trim() + "', TaxRates_Country ='" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() + "',TaxRates_ModifyUser='" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "', TaxRates_ModifyTime=getdate()", "TaxRates_ID=" + WhichType + "");
                        if (updtcnt > 0)
                        {
                            cityGrid.JSProperties["cpUpdate"] = "Success";
                            BindGrid();
                        }
                        else
                            cityGrid.JSProperties["cpUpdate"] = "fail";
                    }
                    else
                        cityGrid.JSProperties["cpUpdateValid"] = "dateInvalid";
               


            }
            if (WhichCall == "Delete")
            {
                if (masterChecking.DeleteTaxScheme(Convert.ToInt32(WhichType)) > 0)
                {
                    deletecnt = oTaxSchemeBl.DelProdTaxWise(Convert.ToInt32(WhichType));
                    //    deletecnt = oGenericMethod.Delete_Table("Config_TaxRates", "TaxRates_ID=" + WhichType + "");


                    if (deletecnt > 0)
                    {
                        cityGrid.JSProperties["cpDelete"] = "Success";
                        BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpDelete"] = "Fail";
                }
                else {
                    cityGrid.JSProperties["cpDelete"] = "inUse";
                }
            }
            if (WhichCall == "Edit")
            {
                DataTable dtEdit = oGenericMethod.GetDataTable(@"SELECT [TaxRates_ID]
                                                          ,[Taxes_Code]
                                                          ,[TaxRates_TaxCode]
                                                          ,[TaxRates_ProductClass]
                                                          ,[ProductClass_Code]
                                                          ,[TaxRates_Country]
                                                          ,tmcoun.[cou_id]
	                                                      ,tmcoun.[cou_country]
	                                                      ,tms.[state]
	                                                      ,tmcity.[city_name]
                                                          ,[TaxRates_State]
                                                          ,[TaxRates_City]
                                                          ,[TaxRates_DateFrom]
                                                          ,[TaxRates_DateTo]
                                                          ,[TaxRates_RateOrSlab]
                                                          ,[TaxRates_Rate]
                                                          ,[TaxRates_MinAmount]
                                                          ,[TaxRates_SlabCode]
                                                          ,[TaxRates_SurchargeApplicable]
                                                          ,[TaxRates_SurchargeCriteria]
                                                          ,[TaxRates_SurchargeAbove]
                                                          ,[TaxRates_SurchargeOn]
                                                          ,[Taxes-SurchargeRate]
                                                          ,[TaxRates_MainAccount]
                                                          ,[TaxRates_SubAccount]
                                                          ,[TaxRates_CreateUser]
                                                          ,[TaxRates_CreateTime]
                                                          ,[TaxRates_ModifyUser]
                                                          ,[TaxRates_ModifyTime]
                                                          ,TaxRatesSchemeName,
                                                          
                                                            ,case when Exempted=1 then 0 else 1 end as Exempted
                                                      FROM [dbo].[Config_TaxRates] ctr
                                                      
                                                         left JOIN tbl_master_country tmcoun ON ctr.TaxRates_Country = tmcoun.cou_id 
                                                         left join tbl_master_state tms on  ctr.TaxRates_State=tms.id
                                                         left join tbl_master_city tmcity on ctr.TaxRates_City=tmcity.city_id 
                                                         left join Master_Taxes mt on ctr.TaxRates_TaxCode=mt.Taxes_ID 
                                                         left join Master_ProductClass mpc on ctr.TaxRates_ProductClass=mpc.ProductClass_ID  
                                                      Where TaxRates_ID=" + WhichType + "");

                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string TaxRates_TaxCode = Convert.ToString(dtEdit.Rows[0]["TaxRates_TaxCode"]);
                    string TaxRates_ProductClass = Convert.ToString(dtEdit.Rows[0]["TaxRates_ProductClass"]);
                    string TaxRates_DateFrom = Convert.ToDateTime(Convert.ToString(dtEdit.Rows[0]["TaxRates_DateFrom"])).Date.ToString("dd-MM-yyyy");
                    string TaxRates_DateTo = Convert.ToString(dtEdit.Rows[0]["TaxRates_DateTo"]);
                    string TaxRates_RateOrSlab = Convert.ToString(dtEdit.Rows[0]["TaxRates_RateOrSlab"]);
                    string TaxRates_Rate = Convert.ToString(dtEdit.Rows[0]["TaxRates_Rate"]);
                    string TaxRates_MinAmount = Convert.ToString(dtEdit.Rows[0]["TaxRates_MinAmount"]);
                    string TaxRates_SlabCode = Convert.ToString(dtEdit.Rows[0]["TaxRates_SlabCode"]).Trim();
                    string TaxRates_SurchargeApplicable = Convert.ToString(dtEdit.Rows[0]["TaxRates_SurchargeApplicable"]);
                    string TaxRates_SurchargeCriteria = Convert.ToString(dtEdit.Rows[0]["TaxRates_SurchargeCriteria"]);
                    string TaxRates_SurchargeAbove = Convert.ToString(dtEdit.Rows[0]["TaxRates_SurchargeAbove"]);
                    string TaxRates_SurchargeOn = Convert.ToString(dtEdit.Rows[0]["TaxRates_SurchargeOn"]);
                    string Taxes_SurchargeRate = Convert.ToString(dtEdit.Rows[0]["Taxes-SurchargeRate"]);
                    string TaxRates_MainAccount = Convert.ToString(dtEdit.Rows[0]["TaxRates_MainAccount"]);
                    string TaxRates_SubAccount = Convert.ToString(dtEdit.Rows[0]["TaxRates_SubAccount"]);

                    string TaxRates_State = Convert.ToString(dtEdit.Rows[0]["TaxRates_State"]);
                    string TaxRates_City = Convert.ToString(dtEdit.Rows[0]["TaxRates_City"]);
                    string TaxRates_Country = Convert.ToString(dtEdit.Rows[0]["TaxRates_Country"]);
                    string TaxRatesSchemeName = Convert.ToString(dtEdit.Rows[0]["TaxRatesSchemeName"]);
                    string Exempted = Convert.ToString(dtEdit.Rows[0]["Exempted"]);
                    //  string TaxRates_TaxSlab_Code = dtEdit.Rows[0]["TaxRates_TaxSlab_Code"].ToString();

                    BindCity(Convert.ToInt32(Convert.ToString(TaxRates_State)));
                    BindCmbTaxSlab_Code();
                    lstTaxRates_MainAccount.SelectedValue = Convert.ToString(dtEdit.Rows[0]["TaxRates_MainAccount"]);
                    lstTaxRates_SubAccount.SelectedValue = Convert.ToString(dtEdit.Rows[0]["TaxRates_SubAccount"]);


                    cityGrid.JSProperties["cpEdit"] = TaxRates_TaxCode + "~" + TaxRates_ProductClass + "~" + TaxRates_DateFrom + "~"
                        + TaxRates_DateTo + "~" + TaxRates_RateOrSlab + "~" + TaxRates_Rate + "~" + TaxRates_MinAmount + "~" + TaxRates_SlabCode + "~"
                        + TaxRates_SurchargeApplicable + "~" + TaxRates_SurchargeCriteria + "~" + TaxRates_SurchargeAbove + "~" + TaxRates_SurchargeOn + "~" +
                       Taxes_SurchargeRate + "~" + TaxRates_State + "~" + TaxRates_City + "~" + TaxRates_Country + "~" + TaxRates_MainAccount + "~" + TaxRates_SubAccount + "~" + WhichType + "~" + TaxRatesSchemeName + "~" + Exempted;
                }
            }

            //BindGrid();
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
        protected void CmbCity_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindCity")
            {
                int countryID = Convert.ToInt32(Convert.ToString(e.Parameter.Split('~')[1]));
                BindCity(countryID);
            }
        }

        /*Code  Added  By Sudip on 14122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> GetMainAccountList(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            //DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode+'-'+MainAccount_SubLedgerType as MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");
            DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["MainAccount_Name"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
            }
            return obj;
        }
        [WebMethod]
        public static List<string> GetSubAccountList(string reqStr, string mainreqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable("Master_SubAccount", "SubAccount_Name,SubAccount_ReferenceID ", " SubAccount_MainAcReferenceID = '" + mainreqStr + "' and SubAccount_Name like '" + reqStr + "%'");
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["SubAccount_Name"]) + "|" + Convert.ToString(dr["SubAccount_ReferenceID"]));
            }
            return obj;
        }
        /*...............code end........*/
    }
}