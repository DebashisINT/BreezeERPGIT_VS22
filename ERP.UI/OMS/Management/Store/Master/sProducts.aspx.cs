using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
//Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using Color = System.Drawing.Color;
//Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP

namespace ERP.OMS.Management.Store.Master
{
    public partial class management_master_Store_sProducts : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page    
    {
        public string pageAccess = "";
        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        public ProductComponentBL prodComp = new ProductComponentBL();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.MasterSettings objmaster = new BusinessLogicLayer.MasterSettings();
        //Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
        string ContType;
        private static String path, path1, FileName, s, time, cannotParse;
        string FilePath = "";
        public string[] InputName = new string[20];
        public string[] InputType = new string[20];
        public string[] InputValue = new string[20];
        //Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            tdstcs.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Tanmoy 24-04-2020
             string mastersettings = objmaster.GetSettings("isServiceManagementRequred");
            //Tanmoy 24-04-2020
            //Surojit 01-03-2019
            string isProductMasterComponentMandatoryVisible = objmaster.GetSettings("ProductComponentMandatoryInAllCompany");
            hdnProductMasterComponentMandatoryVisible.Value = isProductMasterComponentMandatoryVisible;
            //Surojit 01-03-2019

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sProducts.aspx");

            CommonBL cbl = new CommonBL();
            string AltNameMandatory = cbl.GetSystemSettingsResult("AltNameMandatory");
            string UniqueAutoNumberProductMaster = cbl.GetSystemSettingsResult("UniqueAutoNumberProductMaster");
            string PackagingQtyZeroProductMaster = cbl.GetSystemSettingsResult("PackagingQtyZeroProductMaster");
            if (!String.IsNullOrEmpty(AltNameMandatory))
            {
                if (AltNameMandatory == "Yes")
                {
                    hdnAltNameMandatory.Value = "1";
                    
                }
                else if (AltNameMandatory.ToUpper().Trim()=="NO")
                {
                    hdnAltNameMandatory.Value = "0";
                }
            }

            string ProductTypeMandatory = cbl.GetSystemSettingsResult("ProductTypeMandatory");
            if (!String.IsNullOrEmpty(ProductTypeMandatory))
            {
                if (ProductTypeMandatory == "Yes")
                {
                    hdnProductTypeMandatory.Value = "1";

                }
                else if (ProductTypeMandatory.ToUpper().Trim() == "NO")
                {
                    hdnProductTypeMandatory.Value = "0";
                }
            }
            string UOMConversionMandatoryProductMaster = cbl.GetSystemSettingsResult("UOMConversionMandatoryProductMaster");
            if (!String.IsNullOrEmpty(UOMConversionMandatoryProductMaster))
            {
                if (UOMConversionMandatoryProductMaster == "Yes")
                {
                    hdnUOMConverMandatoryProductMaster.Value = "1";

                }
                else if (UOMConversionMandatoryProductMaster.ToUpper().Trim() == "NO")
                {
                    hdnUOMConverMandatoryProductMaster.Value = "0";
                }
            }
            if (!String.IsNullOrEmpty(PackagingQtyZeroProductMaster))
            {
                if (PackagingQtyZeroProductMaster == "Yes")
                {
                    hdnPackagingQtyZeroProductMaster.Value = "1";

                }
                else if (PackagingQtyZeroProductMaster.ToUpper().Trim() == "NO")
                {
                    hdnPackagingQtyZeroProductMaster.Value = "0";
                }
            }


            //uom for non inventory/service item start 
            string UOMNoninventory = cbl.GetSystemSettingsResult("UomOnService/NonInventory");
            if (!String.IsNullOrEmpty(UOMNoninventory))
            {
                if (UOMNoninventory == "Yes")
                {
                    hdnUOMNoninventory.Value = "1";

                }
                else if (UOMNoninventory.ToUpper().Trim() == "NO")
                {
                    hdnUOMNoninventory.Value = "0";
                }
            }
            //End
            cityGrid.JSProperties["cpinsert"] = null;
            cityGrid.JSProperties["cpEdit"] = null;
            cityGrid.JSProperties["cpUpdate"] = null;
            cityGrid.JSProperties["cpDelete"] = null;
            cityGrid.JSProperties["cpExists"] = null;
            cityGrid.JSProperties["cpUpdateValid"] = null;
            cityGrid.JSProperties["cpCopy"] = null;
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (mastersettings == "0")
            {
                DivServiceComponent.Style.Add("display", "none");
                DivServiceModel.Style.Add("display", "none");
                DivReplaceable.Style.Add("display", "none");
                //cityGrid.Columns[19].Visible = false;
                cityGrid.Columns[19].Width = 0;
                cityGrid.Columns[20].Width = 0;
            }
            else
            {
                DivServiceComponent.Style.Add("display", "!inline-block");
                DivServiceModel.Style.Add("display", "!inline-block");
                DivReplaceable.Style.Add("display", "!inline-block");
                //cityGrid.Columns[19].Visible = true;
                cityGrid.Columns[19].Width = 130;
                cityGrid.Columns[20].Width = 130;
            }

            if (!IsPostBack)
            {
                

                if (!String.IsNullOrEmpty(UniqueAutoNumberProductMaster))
                {
                    if (UniqueAutoNumberProductMaster == "Yes")
                    {
                        hdnAutoNumStg.Value = "PDAutoNum1";
                        hdnTransactionType.Value = "PD";

                        dvShortName.Style.Add("display", "none");
                        
                        ddl_Num.Style.Add("display", "block");
                        dvCustDocNo.Style.Add("display", "block");
                        NumberingSchemeBind();
                    }
                    else if (UniqueAutoNumberProductMaster.ToUpper().Trim() == "NO")
                    {
                        hdnAutoNumStg.Value = "PDAutoNum0";
                        hdnTransactionType.Value = "";

                        dvShortName.Style.Add("display", "block");

                        ddl_Num.Style.Add("display", "none");
                        dvCustDocNo.Style.Add("display", "none");
                    }
                }


                ProcedureExecute proc = new ProcedureExecute("PRC_ALLMASTERPAGELISTING");
                proc.AddVarcharPara("@WHICHMODULE", 100, "PRODUCTS");
                proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                proc.RunActionQuery();


               string InstSetting= objmaster.GetSettings("ShowPOSAttributeinProductMaster");

                if(InstSetting=="1"){
                    divPosInstallation.Style.Add("display", "block");
                    divPosOldUnit.Style.Add("display", "block");
                }
                else
                {
                    divPosInstallation.Style.Add("display", "none");
                    divPosOldUnit.Style.Add("display", "none");
                }
            
                //BindCountry();
                //BindState(1); 
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                Session["exportval"] = null;

                BindProType();
                BindProductSize();
               // BindProClassCode();
                BindProductColor();
                BindQuoteCurrency();
                BindTradingLotUnits();
                BindBarCodeType();
                BindTaxCode("S", CmbTaxCodeSale);
                BindTaxCode("P", CmbTaxCodePur);
                BindTaxScheme();
                BindServiceTax();
                BindBrand();
                bindMainAccounts();
                #region Rajdip
                bindproductseries();
                bindsurface();
                bindsubcategory();
                bindapplication();
                bindNature();
                #endregion Rajdip
                //BindHsnCode();
            }
            //BindGrid();
    
            //new code block for showing key from resource page start

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/ProductValues.resx")))
            {
                ResourceReader resReader = new ResourceReader(Server.MapPath("~/Management/DailyTask/ResourceFiles/ProductValues.resx"));

                foreach (DictionaryEntry d in resReader)
                {
                    Label currLBL = new Label();
                    currLBL = (Label)Page.FindControl(Convert.ToString(d.Key));

                    if (currLBL == null) { currLBL = (Label)Popup_Empcitys.FindControl(Convert.ToString(d.Key)); }

                    currLBL.Text = Convert.ToString(d.Value);
                }

                resReader.Close();
            }

            //new code block for showing key from resource page end

            if (!IsPostBack && Request.QueryString["DirectEdit"] != null)
            {
                //Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
                //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "Js1", "fn_Editcity(" + Request.QueryString["DirectEdit"] + ");", true);
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as System.Web.UI.Page, HttpContext.Current.GetType(), "Js1", "fn_Editcity(" + Request.QueryString["DirectEdit"] + ");", true);
                //Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
            }
            //Rev Subhra 01-04-2019
            hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
            hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");
            //End of Rev 

            //Rev Bapi 27-09-2021
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='AllowPreferredVendors' AND IsActive=1");
            if (DT != null && DT.Rows.Count > 0)
            {
                string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                //objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                objEngine = new BusinessLogicLayer.DBEngine();
                DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Transporter' AND IsActive=1");
                if (Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim() == "No")
                {
                    divVendors.Style.Add("display", "none");
                }
            }


            //end of Rev

        }



        //binding dropdowns start
        #region Rajdip
        protected void bindsubcategory()
        {
            AspxHelper oAspxHelper = new AspxHelper();
            DataTable GetProductSeries = Getdropdownproproductattributes("GetProductSubcategory");
            if (GetProductSeries.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(ddlsubcat, GetProductSeries, "ProductSubcategory_Name", "ProductSubcategory_ID", "");
            }
        }
        protected void bindapplication()
        {
            AspxHelper oAspxHelper = new AspxHelper();
            DataTable GetProductSeries = Getdropdownproproductattributes("GetProductApplication");
            if (GetProductSeries.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(ddlproductapplication, GetProductSeries, "ProductApplication_Name", "ProductApplication_ID", "");
            }
        }
        protected void bindNature()
        {
            AspxHelper oAspxHelper = new AspxHelper();
            DataTable GetProductSeries = Getdropdownproproductattributes("GetProductNature");
            if (GetProductSeries.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(ddlproductnature, GetProductSeries, "ProductNature_Name", "ProductNature_ID", "");
            }
        }
        protected void bindsurface()
        {
            AspxHelper oAspxHelper = new AspxHelper();
            DataTable GetProductSeries = Getdropdownproproductattributes("GetProductSurface");
            if (GetProductSeries.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(ddlfinish, GetProductSeries, "ProductSurface_Name", "ProductSurface_ID", "");
            }
        }
        protected void bindproductseries()
        {
            AspxHelper oAspxHelper = new AspxHelper();
            DataTable GetProductSeries = Getdropdownproproductattributes("GetProductSeries");
            if (GetProductSeries.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(ddlSeries, GetProductSeries, "ProductSeries_Name", "ProductSeries_ID", "");
            }
        }
        public DataTable Getdropdownproproductattributes(string Action)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Getprodactattributedropdownvalue");
            proc.AddVarcharPara("@Action", 100, Action);
            ds = proc.GetTable();
            return ds;
        }
        #endregion Rajdip
        protected void BindProType()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("SELECT name,sProducts_Type FROM ( SELECT 'Raw Material' AS name, 'A' AS sProducts_Type UNION SELECT 'Work-In-Process' AS name, 'B' AS sProducts_Type UNION SELECT 'Finished Goods' AS name, 'C' AS sProducts_Type) X order by sProducts_Type  ");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbProType, dtCmb, "name", "sProducts_Type", "");
            }

        }


        protected void bindMainAccounts()
        {
            //BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            //DataTable dtCmb = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            //proc.AddVarcharPara("@action", 20, "GetMainAccount"); 
            //dtCmb = proc.GetTable();



            //cmbsalesInvoice.DataSource = dtCmb;
            //cmbsalesInvoice.TextField = "MainAccount_Name";
            //cmbsalesInvoice.ValueField = "MainAccount_AccountCode";
            //cmbsalesInvoice.DataBind();

            //cmbPurInvoice.DataSource = dtCmb;
            //cmbPurInvoice.TextField = "MainAccount_Name";
            //cmbPurInvoice.ValueField = "MainAccount_AccountCode";
            //cmbPurInvoice.DataBind();

            //cmbSalesReturn.DataSource = dtCmb;
            //cmbSalesReturn.TextField = "MainAccount_Name";
            //cmbSalesReturn.ValueField = "MainAccount_AccountCode";
            //cmbSalesReturn.DataBind();

            //cmbPurReturn.DataSource = dtCmb;
            //cmbPurReturn.TextField = "MainAccount_Name";
            //cmbPurReturn.ValueField = "MainAccount_AccountCode";
            //cmbPurReturn.DataBind();


        }


        protected void BTNSave_clicked(object sender, EventArgs e)
        {
            string[] key = Convert.ToString(KeyField.Text).Split(',');
            string[] value = Convert.ToString(ValueField.Text).Split(',');
            string RexName = Convert.ToString(RexPageName.Text).Trim();

            if (File.Exists(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx")))
            {
                File.Delete(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            }

            ResourceWriter resourceWriter = new ResourceWriter(Server.MapPath("~/Management/DailyTask/ResourceFiles/" + RexName + ".resx"));
            for (int i = 0; i < key.Length; i++)
            {
                resourceWriter.AddResource(key[i].Trim(), value[i].Trim());
            }
            resourceWriter.Generate();
            resourceWriter.Close();

            Response.Redirect("");
        }



        //chinmoy comment 19-07-2019
        //protected void BindProClassCode()
        //{
        //    //  / oGenericMethod = new GenericMethod();
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        //    DataTable dtCmb = new DataTable();
        //    dtCmb = oGenericMethod.GetDataTable("SELECT ProductClass_ID,ProductClass_Name FROM Master_ProductClass order by ProductClass_Name");
        //    AspxHelper oAspxHelper = new AspxHelper();
        //    if (dtCmb.Rows.Count > 0)
        //    {
        //        oAspxHelper.Bind_Combo(CmbProClassCode, dtCmb, "ProductClass_Name", "ProductClass_ID", "");
        //    }

        //}
        //End
        //Tax Code bind here Debjyoti 05-01-2017
        protected void BindTaxCode(string taxType, ASPxComboBox cmb)
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            //dtCmb = oGenericMethod.GetDataTable("select 0  Taxes_ID,'-Select-' Taxes_Name union all select Taxes_ID,Taxes_Name from Master_Taxes where Taxes_ApplicableFor in('B','" + taxType.Trim() + "')");
            dtCmb = oGenericMethod.GetDataTable("select 0  Taxes_SchemeID,'--Select--' Taxes_SchemeName union all select TaxRates_ID,TaxRatesSchemeName from Config_TaxRates ct inner join Master_Taxes mt on ct.TaxRates_TaxCode=mt.Taxes_ID where TaxRates_TaxCode in (select Taxes_ID from Master_Taxes where Taxes_ApplicableFor in ('B','" + taxType + "')) and mt.TaxTypeCode<>'O' order by Taxes_SchemeName");

            AspxHelper oAspxHelper = new AspxHelper();

            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(cmb, dtCmb, "Taxes_SchemeName", "Taxes_SchemeID");
            }



        }

        //tax Scheme bind here debjyoti 05-01-2017

        protected void BindTaxScheme()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();

            dtCmb = oGenericMethod.GetDataTable("select 0  TaxRates_ID,'-Select-' TaxRates_Scheme union all select TaxRates_ID,isnull(TaxRatesSchemeName,'') from Config_TaxRates");
            AspxHelper oAspxHelper = new AspxHelper();

            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbTaxScheme, dtCmb, "TaxRates_Scheme", "TaxRates_ID");
            }



        }

        //BarCode tye added here Debjyoti 30-12-2016
        protected void BindBarCodeType()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select id,Symbology from tbl_master_BarCodeSymbology where isActive=1");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbBarCodeType, dtCmb, "Symbology", "id");
            }

        }


        protected void BindHsnCode()
        {
            //BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            //DataTable dtCmb = new DataTable();
            //dtCmb = oGenericMethod.GetDataTable("select HSN_id,Code+'  ['+Description+']' as Description  from  tbl_HSN_Master");
            //aspxHsnCode.DataSource = dtCmb;
            //aspxHsnCode.ValueField = "HSN_id";
            //aspxHsnCode.TextField = "Description";
            //aspxHsnCode.DataBind();

        }


        protected void BindTradingLotUnits()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select UOM_ID,UOM_Name from Master_UOM  order by UOM_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbTradingLotUnits, dtCmb, "UOM_Name", "UOM_ID", "");

                oAspxHelper.Bind_Combo(CmbQuoteLotUnit, dtCmb, "UOM_Name", "UOM_ID", "");

                oAspxHelper.Bind_Combo(CmbDeliveryLotUnit, dtCmb, "UOM_Name", "UOM_ID", "");

                //added for stock uom
                oAspxHelper.Bind_Combo(cmbStockUom, dtCmb, "UOM_Name", "UOM_ID", "");

                //Added for packing uom
                oAspxHelper.Bind_Combo(cmbPackingUom, dtCmb, "UOM_Name", "UOM_ID", "");

                // oAspxHelper.Bind_Combo(ddlCovgUOM, dtCmb, "UOM_Name", "UOM_ID", "");


                oAspxHelper.Bind_Combo(ddlSize, dtCmb, "UOM_Name", "UOM_ID", "");



            }

        }


        protected void BindQuoteCurrency()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select Currency_ID, Currency_Name  from Master_Currency order by Currency_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbQuoteCurrency, dtCmb, "Currency_Name", "Currency_ID", "");
            }

        }

        protected void BindProductColor()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            //................CODE UPDATED BY sAM ON 18102016.................................................
            //dtCmb = oGenericMethod.GetDataTable("SELECT [Color_ID],[Color_Name] FROM [dbo].[Master_Color] UNION SELECT 0 AS [Color_ID],'None' AS [Color_Name] UNION SELECT NULL AS [Color_ID],'' AS [Color_Name] ORDER BY [Color_ID]");
            dtCmb = oGenericMethod.GetDataTable("SELECT [Color_ID],[Color_Name] FROM [dbo].[Master_Color] UNION SELECT 0 AS [Color_ID],'Select' AS [Color_Name] ");

            //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbProductColor, dtCmb, "Color_Name", "Color_ID", "");
                CmbProductColor.SelectedIndex = 0;
            }

        }

        protected void BindBrand()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select Brand_Id ,Brand_Name from tbl_master_brand where Brand_IsActive=1");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(cmbBrand, dtCmb, "Brand_Name", "Brand_Id", "");
            }

        }



        protected void BindProductSize()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            //................CODE UPDATED BY sAM ON 18102016.................................................
            //dtCmb = oGenericMethod.GetDataTable("SELECT [Size_ID],[Size_Name] FROM [dbo].[Master_Size] UNION SELECT 0 AS [Size_ID],'None' AS [Size_Name] UNION SELECT NULL AS [Size_ID],'' AS [Size_Name]");
            dtCmb = oGenericMethod.GetDataTable("SELECT [Size_ID],[Size_Name] FROM [dbo].[Master_Size] UNION SELECT 0 AS [Size_ID],'Select' AS [Size_Name]");
            //................CODE aBOVE UPDATED BY sAM ON 18102016.................................................
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbProductSize, dtCmb, "Size_Name", "Size_ID", "");
                CmbProductSize.SelectedIndex = 0;
            }

        }

        [WebMethod]
        public static bool CheckUniqueNumberingCode(string uccName, string Type)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(uccName, "0", Type);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

       
        public class VendorModel
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }



        [WebMethod]
        public static bool CheckUniqueName(string ProductName, int procode)
        {
            DataTable dt = new DataTable();
            ProductName = ProductName.Replace("'", "''");
            bool IsPresent = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (procode == 0)
            {
                dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "'");
            }
            else
            {
                dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "' and sProducts_ID<>" + procode + "");
            }
            //DataTable dt = oGeneric.GetDataTable("SELECT COUNT(sProducts_Code) AS sProducts_Name FROM Master_sProducts WHERE sProducts_Code = '" + ProductName + "'");

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["sProducts_Name"]) > 0)
                {
                    IsPresent = true;
                }
            }
            return IsPresent;
        }
        //binding dropdown ends

        protected void BindCountry()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select cou_id as id,cou_country as name from tbl_master_country order By cou_country");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //oAspxHelper.Bind_Combo(CmbCountryName, dtCmb, "name", "id", "India");
            }

        }

        protected void BindState(int countryID)
        {
            //CmbState.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select id,state as name From tbl_master_STATE Where countryID=" + countryID + " Order By Name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                //oAspxHelper.Bind_Combo(CmbState, dtCmb, "name", "id", 0);
            }
            else
            {
                //CmbState.Enabled = false;
            }
        }
        protected void BindCity(int stateID)
        {
            //CmbCity.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select city_id,city_name From tbl_master_city Where state_id=" + stateID + " Order By city_name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                // oAspxHelper.Bind_Combo(CmbCity, dtCmb, "city_name", "city_id", 0);
            }
            else
            {
                //CmbCity.Enabled = false;
            }
        }

        //protected void BindGrid()
        //{
        //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        //    Store_MasterBL oStore_MasterBL = new Store_MasterBL();
        //    DataTable dtFillGrid = new DataTable();
        //    string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
        //  //  dtFillGrid = oStore_MasterBL.GetsProductList();
        //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
        //    ProcedureExecute proc = new ProcedureExecute("PRC_ALLMASTERPAGELISTING");
        //    proc.AddVarcharPara("@WHICHMODULE", 100, "PRODUCTS");
        //    proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
        //    proc.RunActionQuery();

        //    var q = from d in dc.ALLMASTERPAGELISTINGs where Convert.ToString(d.USERID)==Userid   
        //              orderby d.SLNO select d;
        ////                    //where branchidlist.Contains(Convert.ToInt32(d.BranchID))
        ////                    //&& d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)

        ////                    orderby d.Products_Name descending
        ////                    select d;

        //    AspxHelper oAspxHelper = new AspxHelper();
        //    List<ALLMASTERPAGELISTING> Prolist = q.ToList();
        //    //if (dtFillGrid.Rows.Count > 0)
        //    //{
        //    cityGrid.DataSource = Prolist;
        //    cityGrid.DataBind();
        //    //}
        //}

        //bind service tax


        protected void BindGrid()
        {
          

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            ProcedureExecute proc = new ProcedureExecute("PRC_ALLMASTERPAGELISTING");
            proc.AddVarcharPara("@WHICHMODULE", 100, "PRODUCTS");
            proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.RunActionQuery();

           


                   
                    var q = from d in dc.ALLMASTERPAGELISTINGs
                            where Convert.ToString(d.USERID) == Userid && d.REPORTTYPE == "PRODUCTS"
                            orderby d.sProducts_ID descending 
                            select d;

                    List<ALLMASTERPAGELISTING> Prolist = q.ToList();
              
            // cityGrid.DataSource = Prolist;
             // cityGrid.DataBind();
            
            
             
        }

        

        protected void BindServiceTax()
        {
            DataTable serviceTax = oDBEngine.GetDataTable("SELECT TAX_ID,SERVICE_CATEGORY_CODE,SERVICE_TAX_NAME,ACCOUNT_HEAD_TAX_RECEIPTS,ACCOUNT_HEAD_OTHERS_RECEIPTS,ACCOUNT_HEAD_PENALTIES,ACCOUNT_HEAD_DeductRefund FROM TBL_MASTER_SERVICE_TAX");
            AspxServiceTax.DataSource = serviceTax;
            AspxServiceTax.DataBind();
        }

        public void NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForProductMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }

        public DataTable GetAllDropDownDetailForCustomerMaster(string UserBranch, string Qry)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, Qry);
            proc.AddVarcharPara("@userbranchlist", 4000, UserBranch);
            ds = proc.GetTable();
            return ds;
        }
        public void bindexport(int Filter)
        {
            cityGrid.Columns[6].Visible = false;

            //MainAccountGrid.Columns[20].Visible = false;
            // MainAccountGrid.Columns[21].Visible = false;
            string filename = "Products";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Products";
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
            int cisv = 0;

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
            //Rev Rajdip
            if (chkConseiderInstockval.Checked)
            {
                cisv = 1;
            }
            //End Rev Rajdip

            if (WhichCall == "savecity")
            {
                //Rev Rajdip
                decimal MinSale = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                decimal maxsale = Convert.ToDecimal(txtMrp.Text.Trim());
                if (MinSale > maxsale)
                {
                    valid.Text = "*";
                    cityGrid.JSProperties["cpinsert"] = "Validation";
                    return;
                }
                //End Rev Rajdip
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();
                int TradingLot = 0;
                int QuoteLot = 0;
                int DeliveryLot = 0;
                int productSize = 0;
                int ProductColor = 0;
                string strMsg = "fail";
                //-----Arindam
                txtQuoteLot.Text = "1";
                txtTradingLot.Text = "1";
                txtDeliveryLot.Text = "1";

                string lenght = txtHeight.Text;
                string width = txtWidth.Text;
                string Thickness = txtThickness.Text;
                string size = ddlSize.Value.ToString();
                string SUOM = SizeUOM.Value;
                //Rev Rajdip
                //string series = txtSeries.Text;
                //string Finish = txtFinish.Text;
                //string subcat = txtSubCat.Text;
                string series = string.Empty;
                string Finish = string.Empty;
                string subcat = string.Empty;
                int Application = 0;
                int Nature = 0;
                if (ddlSeries.Value == null)
                {
                    series = "";
                }
                else {
                    series = ddlSeries.Value.ToString();  
                }
                if (ddlfinish.Value == null)
                {
                    Finish = "";
                }
                else
                {
                    Finish = ddlfinish.Value.ToString();  
                }
                if (ddlsubcat.Value == null)
                {
                    subcat = "";
                }
                else
                {
                    subcat = ddlsubcat.Value.ToString();
                }
                if (ddlproductapplication.Value == null)
                {
                    Application = 0;
                }
                else
                {
                    Application =Convert.ToInt32(ddlproductapplication.Value.ToString());
                }
                if (ddlproductnature.Value == null)
                {
                    Nature = 0;
                }
                else
                {
                    Nature = Convert.ToInt32(ddlproductnature.Value.ToString());
                }
                string strdimension = string.Empty;
                string strpedestalno = string.Empty;
                string strcatno = string.Empty;
                string strwarranty = string.Empty;
                strdimension = txtdimension.Text.ToString();
                strpedestalno = txtpedestalno.Text.ToString();
                strcatno = txtcatno.Text.ToString();
                strwarranty = txtwarranty.Text.ToString();
                //End Rev Rajdip
                
                string LeadTime = txtLeadtime.Text;
                string Coverage = txtCoverage.Value;
                string covuom = dvCovg.InnerText;
                string volume = txtVolumn.Value;
                string volumeuom = dvvolume.InnerText;
                string wight = txtWeight.Text;
               
                string UniqueName = "";

                if ((hdnAutoNumStg.Value == "PDAutoNum1") && (hdnTransactionType.Value == "PD"))
                {
                    UniqueName = hddnDocNo.Value.Trim();
                }
                else
                {
                    UniqueName = txtPro_Code.Text.Trim();
                }

                //--Arindam for tryparse
                /*insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                    Convert.ToString(CmbProType.SelectedItem.Value), Convert.ToInt32(CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text,
                    TradingLot, Convert.ToInt32(CmbTradingLotUnits.SelectedItem.Value));*/
                if (!string.IsNullOrEmpty(UniqueName) && !string.IsNullOrEmpty(txtPro_Name.Text.Trim()))
                {

                    if (int.TryParse(txtQuoteLot.Text, out QuoteLot))
                    {
                        if (int.TryParse(txtTradingLot.Text, out TradingLot))
                        {
                            if (int.TryParse(txtDeliveryLot.Text, out DeliveryLot))
                            {
                                //if (CmbProductSize.SelectedItem.Value != "") //28.12.2016 commented by Subhra because it's getting error
                                if (CmbProductSize.Text != "")
                                {
                                    productSize = Convert.ToInt32(CmbProductSize.SelectedItem.Value);
                                }
                                if (CmbProductColor.Text != "")
                                //if (CmbProductColor.SelectedItem.Value != "") //28.12.2016 commented by Subhra because it's getting error
                                {
                                    ProductColor = Convert.ToInt32(CmbProductColor.SelectedItem.Value);
                                }
                                Boolean sizeapplicable = false;
                                Boolean colorapplicable = false;
                                Boolean isInventory = false;
                                Boolean Replaceable = false;
                                Boolean autoApply = false;
                                Boolean isInstall = false;
                                Boolean isOldUnit = false;
                                Boolean IsServiceItem = false;
                                Boolean FurtheranceToBusiness = false;//Subhabrata
                                Boolean OverideConvertion = false; //Surojit 08-02-2019
                                Boolean IsMandatory = false; //Surojit 11-02-2019
                                decimal saleprice = 0;
                                decimal MinSaleprice = 0;
                                decimal purPrice = 0;
                                //rev srijeeta
                                decimal packageqty = 0;
                                //end of rev srijeeta
                                decimal MRP = 0;
                                decimal minLvl = 0;
                                decimal maxLvl = 0;
                                decimal reorderLvl = 0;
                                decimal reorder_qty = 0;
                                
                                Boolean isCapitalGoods = false;
                                int tdscode = 0;
                                int ComponentService = 0;
                                String ModelList = "";
                                decimal CostPrice = 0;
                                if (txtCostPrice.Text.Trim() != "")
                                {
                                    CostPrice = Convert.ToDecimal(txtCostPrice.Text.Trim());
                                }
                                //Rev Start Tanmoy for Component of Service
                                if (chkComponentService.Checked)
                                {
                                    ComponentService = 1;
                                }

                                List<object> modList = lookup_Model.GridView.GetSelectedFieldValues("ModelID");
                                foreach (object mod in modList)
                                {
                                    ModelList += "," + mod;
                                }

                                //Rev End Tanmoy for Component of Service

                                if (cmb_tdstcs.Value != null)
                                {
                                    tdscode = Convert.ToInt32(cmb_tdstcs.Value);
                                }

                                if (chkFurtherance.Checked)
                                {
                                    FurtheranceToBusiness = true;
                                }

                                if (ChkAutoApply.Checked)
                                    autoApply = true;

                                if (chkOverideConvertion.Checked) //Surojit 08-02-2019
                                    OverideConvertion = true;

                                if (chkIsMandatory.Checked) //Surojit 11-02-2019
                                    IsMandatory = true;

                                if (txtReorderLvl.Text.Trim() != "")
                                {
                                    reorderLvl = Convert.ToDecimal(txtReorderLvl.Text.Trim());
                                }

                                if (txtReorderQty.Text.Trim() != "")
                                {
                                    reorder_qty = Convert.ToDecimal(txtReorderQty.Text.Trim());

                                }
                               

                                if (txtMinLvl.Text.Trim() != "")
                                {
                                    minLvl = Convert.ToDecimal(txtMinLvl.Text.Trim());
                                }


                                if (txtMaxLvl.Text.Trim() != "")
                                {
                                    maxLvl = Convert.ToDecimal(txtMaxLvl.Text.Trim());
                                }




                                if (txtMrp.Text.Trim() != "")
                                {
                                    MRP = Convert.ToDecimal(txtMrp.Text.Trim());
                                }

                                if (txtPurPrice.Text.Trim() != "")
                                {
                                    purPrice = Convert.ToDecimal(txtPurPrice.Text.Trim());
                                }
                                //rev srijeeta
                                if (txtpackageqty.Text.Trim() != "")
                                {
                                    packageqty = Convert.ToDecimal(txtpackageqty.Text.Trim());
                                }
                                //end of rev srijeeta

                                if (txtMinSalePrice.Text.Trim() != "")
                                {
                                    MinSaleprice = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                                }



                                if (txtSalePrice.Text.Trim() != "")
                                {
                                    saleprice = Convert.ToDecimal(txtSalePrice.Text.Trim());
                                }
                                if (rdblappColor.Items[0].Selected)
                                {
                                    colorapplicable = true;
                                }
                                else
                                {
                                    colorapplicable = false;
                                }


                                if (rdblapp.Items[0].Selected)
                                {
                                    sizeapplicable = true;
                                }
                                else
                                {
                                    sizeapplicable = false;
                                }

                                if (Convert.ToString(cmbIsInventory.SelectedItem.Value) == "1")
                                    isInventory = true;
                                if (Convert.ToString(cmbReplaceable.SelectedItem.Value) == "1")
                                    Replaceable = true;

                                //Get Product Componnet details
                                String ProdComponent = "";
                                List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("sProducts_ID");
                                foreach (object Pobj in ComponentList)
                                {
                                    ProdComponent += "," + Pobj;
                                }
                                ProdComponent = ProdComponent.TrimStart(',');

                                if (Convert.ToString(aspxInstallation.SelectedItem.Value) == "1")
                                    isInstall = true;

                                if (Convert.ToString(cmbOldUnit.SelectedItem.Value) == "1")
                                    isOldUnit = true;


                                if (Convert.ToString(cmbIsCapitalGoods.SelectedItem.Value) == "1")
                                    isCapitalGoods = true;

                                if (Convert.ToString(cmbServiceItem.SelectedItem.Value) == "1")
                                    IsServiceItem = true;

                                if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "Prd"))
                                {
                                    cityGrid.JSProperties["cpinsert"] = "UDFManddratory";
                                    return;

                                }
                                string ShortName="";
                                int numberingId = 0;
                                if ((hdnAutoNumStg.Value == "PDAutoNum1") && (hdnTransactionType.Value=="PD"))
                                {
                                    ShortName = hddnDocNo.Value.Trim();
                                    numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                }
                                else
                                {
                                    ShortName = txtPro_Code.Text.Trim();
                                }
                                int ApplicationArea = Convert.ToInt32(cmbAppliArea.Value);
                                string Movement = Convert.ToString(txtMovement.Text);
                                string _DesignNo= Convert.ToString(txtDesignNo.Text);
                                string _RevisionNo=Convert.ToString(txtRevisionNo.Text);
                                string _ItemType = Convert.ToString(cmbItemType.Value);
                                //Rev Bapi
                                string _Vendors = Convert.ToString(hdVendorsID.Value);
                                //End of rev Bapi
                                if (HttpContext.Current.Session["userid"] != null)
                                {
                                    //string _ProductName=txtPro_Name.Text.Replace("\"", "");

                                    insertcount = oStore_MasterBL.InsertProducts(strdimension, strpedestalno, strcatno, strwarranty, cisv, ShortName, txtPro_Name.Text, txtPro_Description.Text,
                                     Convert.ToString(CmbProType.Value == null ? 0 : CmbProType.SelectedItem.Value), 
                                    //chinmoy cooment 19-07-2019
                                     // Convert.ToInt32(CmbProClassCode.Value == null ? 0 : CmbProClassCode.SelectedItem.Value), 
                                     (ClassId.Value == "" ? 0 : Convert.ToInt32(ClassId.Value)),
                                     //End

                                     txtGlobalCode.Text,
                                     1, Convert.ToInt32(CmbTradingLotUnits.Value == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
                                     1, 1, 1, 1, Convert.ToInt32(CmbDeliveryLotUnit.Value == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value), ProductColor,
                                     productSize, Convert.ToInt32(HttpContext.Current.Session["userid"]), sizeapplicable, colorapplicable,
                                     Convert.ToInt32(CmbBarCodeType.Value == null ? 0 : CmbBarCodeType.SelectedItem.Value), txtBarCodeNo.Text.Trim(),
                                     isInventory, Convert.ToString(CmbStockValuation.SelectedItem.Value), saleprice, MinSaleprice, purPrice, 
                                     //rev srijeeta
                                     packageqty,
                                     //end of rev srijeeta
                                     MRP,
                                     Convert.ToInt32(cmbStockUom.Value == null ? 0 : cmbStockUom.SelectedItem.Value), minLvl, reorderLvl,
                                     Convert.ToString(cmbNegativeStk.SelectedItem.Value), Convert.ToInt32(CmbTaxCodeSale.Value == null ? 0 : CmbTaxCodeSale.SelectedItem.Value),
                                     Convert.ToInt32(CmbTaxCodePur.Value == null ? 0 : CmbTaxCodePur.SelectedItem.Value), Convert.ToInt32(CmbTaxScheme.Value == null ? 0 : CmbTaxScheme.SelectedItem.Value),
                                     autoApply, Convert.ToString(fileName.Value), ProdComponent, Convert.ToString(CmbStatus.SelectedItem.Value), 
                                     //chinmoy edited 22-07-2019
                                     //start
                                     //Convert.ToString(HsnLookUp.Text).Trim(),
                                     Convert.ToString(hdnHSN.Value).Trim(),
                                     //end
                                     Convert.ToInt32(AspxServiceTax.Value == null ? 0 : AspxServiceTax.Value),
                                     Convert.ToDecimal(txtPackingQty.Text.Trim()), Convert.ToDecimal(txtpacking.Text.Trim()), Convert.ToInt32(cmbPackingUom.Value != null ? cmbPackingUom.Value : 0),
                                     OverideConvertion, //Surojit 08-02-2019
                                     IsMandatory, //Surojit 11-02-2019
                                     isInstall, Convert.ToInt32(cmbBrand.Value == null ? 0 : cmbBrand.Value), isCapitalGoods, tdscode, Convert.ToString(Session["LastFinYear"]), isOldUnit,
                                     hdnSIMainAccount.Value == null ? "" : Convert.ToString(hdnSIMainAccount.Value), hdnSRMainAccount.Value == null ? "" : Convert.ToString(hdnSRMainAccount.Value),
                                     hdnPIMainAccount.Value == null ? "" : Convert.ToString(hdnPIMainAccount.Value), hdnPRMainAccount.Value == null ? "" : Convert.ToString(hdnPRMainAccount.Value), FurtheranceToBusiness, IsServiceItem, reorder_qty
                                        
                                    , maxLvl, lenght, width, Thickness, size, SUOM, series, Finish, LeadTime, Coverage, covuom, volume, volumeuom, wight, txtPro_Printname.Text, subcat, ComponentService, ModelList, _DesignNo, _RevisionNo, _ItemType, Replaceable, numberingId
                                     , Application, Nature, ApplicationArea, Movement, CostPrice, _Vendors );



                                    //chinmoy added 31-03-2020 Start	
                                    if (hdnAutoNumStg.Value == "PDAutoNum1")
                                    {
                                        if ((insertcount != 0))
                                        {
                                            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
                                            DataTable dts = new DataTable();
                                            DataTable delete = new DataTable();
                                            dts = BEngine.GetDataTable("select isnull(sProducts_Code,'') sProducts_Code from master_sproducts where sProducts_ID='" + insertcount + "'");
                                            if (dts.Rows.Count == 1)
                                            {
                                                if (Convert.ToString(dts.Rows[0]["sProducts_Code"]) == "Auto")
                                                {
                                                    delete = BEngine.GetDataTable("delete from master_sproducts where sProducts_ID='" + insertcount + "'");
                                                    if (hdnAutoNumStg.Value == "LDAutoNum1")
                                                    {
                                                        txt_CustDocNo.Text = "Auto";
                                                        txt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    else
                                                    {
                                                        txt_CustDocNo.Text = "Auto";
                                                        txt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Either Unique ID Exists OR Unique ID Exhausted.')", true);
                                                }
                                            }
                                        }
                                    }
                                    //End

                                    //Udf Add mode
                                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                    if (udfTable != null)
                                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("Prd", "ProductMaster" + Convert.ToString(insertcount), udfTable, Convert.ToString(Session["userid"]));

                                    //insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                                    //Convert.ToString(CmbProType.SelectedItem.Value), Convert.ToInt32(CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text,
                                    //TradingLot, Convert.ToInt32(CmbTradingLotUnits.SelectedItem.Value),
                                    //Convert.ToInt32(CmbQuoteCurrency.SelectedItem.Value), QuoteLot,
                                    //Convert.ToInt32(CmbQuoteLotUnit.SelectedItem.Value), DeliveryLot,
                                    //Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem.Value), ProductColor,
                                    //productSize, Convert.ToInt32(HttpContext.Current.Session["userid"]));


                                    strMsg = "Success";
                                   // RefereshApplicationProductData();
                                }
                                else
                                {
                                    strMsg = "Your session is end";
                                }

                            }
                            //else
                            //{
                            //    strMsg = "Delivery lot must be numeric value";
                            //}
                        }
                        //else
                        //{
                        //    strMsg = "Trading lot must be numeric value";
                        //}

                    }
                    //else
                    //{
                    //    strMsg = "Quote lot must be numeric value";
                    //}
                }
                //else
                //{
                //    strMsg = "Product Short Name (Unique) and Name is required";
                //}



                if (insertcount > 0)
                {
                    cityGrid.JSProperties["cpinsert"] = "Success";
                    //BindGrid();
                }
                else
                {
                    cityGrid.JSProperties["cpinsert"] = strMsg;
            
                }
            }
            if (WhichCall == "updatecity")
            {

                //Rev Rajdip
                decimal MinSale = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                decimal maxsale = Convert.ToDecimal(txtMrp.Text.Trim());
                if (MinSale > maxsale)
                {
                    valid.Text = "*";
                    cityGrid.JSProperties["cpinsert"] = "Validation";
                    return;
                }
                //End Rev Rajdip
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();
                Boolean Replaceable = false;
                Boolean sizeapplicable = false;
                Boolean colorapplicable = false;
                Boolean isInventory = false;
                Boolean autoApply = false;
                Boolean isInstall = false;
                Boolean isOldUnit = false;
                Boolean IsServiceItem = false;
                Boolean FurtheranceToBusiness = false;//Subhabrata
                Boolean OverideConvertion = false; //Surojit 08-02-2019
                Boolean IsMandatory = false; //Surojit 11-02-2019
                decimal saleprice = 0;
                decimal MinSaleprice = 0;
                decimal purPrice = 0;
                //rev srijeeta
                decimal packageqty = 0;
                //end of rev srijeeta
                decimal MRP = 0;
                decimal MinLvl = 0;
                decimal maxLvl = 0;
                decimal reorderLvl = 0;
                decimal reorder_qty = 0;
                
                Boolean isCapitalGoods = false;
                int tdscode = 0;
                decimal CostPrice = 0;


                string lenght = txtHeight.Text;
                string width = txtWidth.Text;
                string Thickness = txtThickness.Text;
                string size = null;
                if (ddlSize.Value != null)
                {
                    size = ddlSize.Value.ToString();
                }
                string SUOM = SizeUOM.Value;
                //Rev Rajdip
                //string series = txtSeries.Text;
                // string Finish = txtFinish.Text;
                //string subcat = txtSubCat.Text;
                string series = string.Empty;
                string Finish = string.Empty;
                string subcat = string.Empty;
                int Application = 0;
                int Nature = 0;
                if (ddlSeries.Value == null)
                {
                    series = "";
                }
                else
                {
                    series = ddlSeries.Value.ToString();
                }
                if (ddlfinish.Value == null)
                {
                    Finish = "";
                }
                else
                {
                    Finish = ddlfinish.Value.ToString();
                }
                if (ddlsubcat.Value == null)
                {
                    subcat = "";
                }
                else
                {
                    subcat = ddlsubcat.Value.ToString();
                }
                if (ddlproductapplication.Value == null)
                {
                    Application = 0;
                }
                else
                {
                    Application = Convert.ToInt32(ddlproductapplication.Value.ToString());
                }
                if (ddlproductnature.Value == null)
                {
                    Nature = 0;
                }
                else
                {
                    Nature = Convert.ToInt32(ddlproductnature.Value.ToString());
                }
                string strdimension = string.Empty;
                string strpedestalno = string.Empty;
                string strcatno = string.Empty;
                string strwarranty = string.Empty;
                strdimension = txtdimension.Text.ToString();
                strpedestalno = txtpedestalno.Text.ToString();
                strcatno = txtcatno.Text.ToString();
                strwarranty = txtwarranty.Text.ToString();
                //End Rev Rajdip
                
                string LeadTime = txtLeadtime.Text;
                string Coverage = txtCoverage.Value;
                string covuom = dvCovg.InnerText;
                string volume = txtVolumn.Value;
                string volumeuom = dvvolume.InnerText;
                string wight = txtWeight.Text;

                int ComponentService = 0;
                String ModelList = "";
                //Rev Start Tanmoy for Component of Service
                if (chkComponentService.Checked)
                {
                    ComponentService = 1;
                }

                List<object> modList = lookup_Model.GridView.GetSelectedFieldValues("ModelID");
                foreach (object mod in modList)
                {
                    ModelList += "," + mod;
                }
                //Rev End Tanmoy for Component of Service

                if (chkFurtherance.Checked)
                {
                    FurtheranceToBusiness = true;
                }

                if (cmb_tdstcs.Value != null)
                {
                    tdscode = Convert.ToInt32(cmb_tdstcs.Value);
                }

                if (ChkAutoApply.Checked)
                    autoApply = true;

                if (chkOverideConvertion.Checked) //Surojit 08-02-2019
                    OverideConvertion = true;

                if (chkIsMandatory.Checked) //Surojit 11-02-2019
                    IsMandatory = true;

                if (txtReorderLvl.Text.Trim() != "")
                {
                    reorderLvl = Convert.ToDecimal(txtReorderLvl.Text.Trim());
                }

                if (txtReorderQty.Text.Trim() != "")
                {
                    reorder_qty = Convert.ToDecimal(txtReorderQty.Text.Trim());

                }
                
                if (txtMinLvl.Text.Trim() != "")
                {
                    MinLvl = Convert.ToDecimal(txtMinLvl.Text.Trim());
                }

                if (txtMaxLvl.Text.Trim() != "")
                {
                    maxLvl = Convert.ToDecimal(txtMaxLvl.Text.Trim());
                }


                if (txtMrp.Text.Trim() != "")
                {
                    MRP = Convert.ToDecimal(txtMrp.Text.Trim());
                }
                if (txtPurPrice.Text.Trim() != "")
                {
                    purPrice = Convert.ToDecimal(txtPurPrice.Text.Trim());
                }
                //rev srijeeta
                if (txtpackageqty.Text.Trim() != "")
                {
                    packageqty = Convert.ToDecimal(txtpackageqty.Text.Trim());
                }
                //end of rev srijeeta
                if (txtCostPrice.Text.Trim() != "")
                {
                    CostPrice = Convert.ToDecimal(txtCostPrice.Text.Trim());
                }
                if (txtMinSalePrice.Text.Trim() != "")
                {
                    MinSaleprice = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                }

                if (txtSalePrice.Text.Trim() != "")
                {
                    saleprice = Convert.ToDecimal(txtSalePrice.Text.Trim());
                }
                if (rdblappColor.Items[0].Selected)
                {
                    colorapplicable = true;
                }
                else
                {
                    colorapplicable = false;
                }


                if (rdblapp.Items[0].Selected)
                {
                    sizeapplicable = true;
                }
                else
                {
                    sizeapplicable = false;
                }

                if (Convert.ToString(cmbIsInventory.SelectedItem.Value) == "1")
                    isInventory = true;

                if (Convert.ToString(cmbServiceItem.SelectedItem.Value) == "1")
                    IsServiceItem = true;

                if (Convert.ToString(cmbOldUnit.SelectedItem.Value) == "1")
                    isOldUnit = true;
                //Get Product Componnet details for update 
                String ProdComponent = "";
                List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("sProducts_ID");
                foreach (object Pobj in ComponentList)
                {
                    ProdComponent += "," + Pobj;
                }
                ProdComponent = ProdComponent.TrimStart(',');





                //  Delete Existing file in Update

                string[] filePath = oDBEngine.GetFieldValue1("Master_sProducts", "sProduct_ImagePath", "sProducts_ID=" + WhichType, 1);
                if (filePath[0] != "")
                {
                    if (filePath[0].Trim() != fileName.Value.Trim())
                    {
                        if ((System.IO.File.Exists(Server.MapPath(filePath[0]))))
                        {
                            System.IO.File.Delete(Server.MapPath(filePath[0]));
                        }
                    }
                }
                //

                if (Convert.ToString(aspxInstallation.SelectedItem.Value) == "1")
                    isInstall = true;

                if (Convert.ToString(cmbIsCapitalGoods.SelectedItem.Value) == "1")
                    isCapitalGoods = true;

                string ShortName = "";
                //int numberingId = 0;
                if ((hdnAutoNumStg.Value == "PDAutoNum1") && (hdnTransactionType.Value == "PD"))
                {
                    ShortName = hddnDocNo.Value.Trim();
                    //numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                }
                else
                {
                    ShortName = txtPro_Code.Text.Trim();
                }
                if (Convert.ToString(cmbReplaceable.SelectedItem.Value) == "1")
                    Replaceable = true;

                int ApplicationArea = Convert.ToInt32(cmbAppliArea.Value);
                string Movement = Convert.ToString(txtMovement.Text);
                //Rev Bapi

                string VendorIDs = Convert.ToString(hdVendorsID.Value);
                //End rev Bapi

                updtcnt = oStore_MasterBL.UpdateProducts(strdimension, strpedestalno, strcatno, strwarranty, cisv, Convert.ToInt32(WhichType), ShortName, txtPro_Name.Text, txtPro_Description.Text, Convert.ToString(CmbProType.SelectedItem == null ? 0 : CmbProType.SelectedItem.Value),
                //chinmoy edited 19-07-2019
                    //Convert.ToInt32(CmbProClassCode.SelectedItem == null ? 0 : CmbProClassCode.SelectedItem.Value),
                   (ClassId.Value == "" ? 0 : Convert.ToInt32(ClassId.Value)),
                    //End
                 txtGlobalCode.Text, 1, Convert.ToInt32(CmbTradingLotUnits.SelectedItem == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
                 1, 1, 1, 1,
                 Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value),
                 Convert.ToInt32(CmbProductColor.SelectedItem == null ? 0 : CmbProductColor.SelectedItem.Value),
                 Convert.ToInt32(CmbProductSize.SelectedItem == null ? 0 : CmbProductSize.SelectedItem.Value),
                 Convert.ToInt32(HttpContext.Current.Session["userid"]), sizeapplicable, colorapplicable,
                 Convert.ToInt32(CmbBarCodeType.SelectedItem == null ? 0 : CmbBarCodeType.SelectedItem.Value), txtBarCodeNo.Text.Trim(),
                 isInventory, Convert.ToString(CmbStockValuation.SelectedItem == null ? "" : CmbStockValuation.SelectedItem.Value), saleprice, MinSaleprice, purPrice,
                 //rev srijeeta
                 packageqty,
                 //end of rev srijeeta
                 MRP,
                 Convert.ToInt32(cmbStockUom.SelectedItem == null ? 0 : cmbStockUom.SelectedItem.Value), MinLvl, reorderLvl,
                 Convert.ToString(cmbNegativeStk.SelectedItem.Value), Convert.ToInt32(CmbTaxCodeSale.SelectedItem == null ? 0 : CmbTaxCodeSale.SelectedItem.Value),
                 Convert.ToInt32(CmbTaxCodePur.SelectedItem == null ? 0 : CmbTaxCodePur.SelectedItem.Value), Convert.ToInt32(CmbTaxScheme.SelectedItem == null ? 0 : CmbTaxScheme.SelectedItem.Value),
                 autoApply, Convert.ToString(fileName.Value), ProdComponent, Convert.ToString(CmbStatus.SelectedItem.Value),

                  //chinmoy edited 22-07-2019
                    //start
                    //Convert.ToString(HsnLookUp.Text).Trim(),
                                     Convert.ToString(hdnHSN.Value).Trim(),
                    //end

                 Convert.ToInt32(AspxServiceTax.Value == null ? 0 : AspxServiceTax.Value),
                 Convert.ToDecimal(txtPackingQty.Text.Trim()), Convert.ToDecimal(txtpacking.Text.Trim()), Convert.ToInt32(cmbPackingUom.Value != null ? cmbPackingUom.Value : 0),
                 OverideConvertion, //Surojit 08-02-2019
                 IsMandatory, //Surojit 11-02-2019
                 isInstall, Convert.ToInt32(cmbBrand.Value == null ? 0 : cmbBrand.Value), isCapitalGoods, tdscode, isOldUnit,
                 hdnSIMainAccount.Value == null ? "" : Convert.ToString(hdnSIMainAccount.Value), hdnSRMainAccount.Value == null ? "" : Convert.ToString(hdnSRMainAccount.Value),
                 hdnPIMainAccount.Value == null ? "" : Convert.ToString(hdnPIMainAccount.Value), hdnPRMainAccount.Value == null ? "" : Convert.ToString(hdnPRMainAccount.Value), FurtheranceToBusiness, IsServiceItem, reorder_qty,
               
                 maxLvl, lenght, width, Thickness, size, SUOM, series, Finish, LeadTime, Coverage, covuom, volume, volumeuom, wight, txtPro_Printname.Text, subcat
                 
                 //Rev Tanmoy
                 , ComponentService, ModelList
                 //Rev Tanmoy End
                  , Convert.ToString(txtDesignNo.Text), Convert.ToString(txtRevisionNo.Text)
                  , Convert.ToString(cmbItemType.Value)
                  , Replaceable
                 //Rev rajdip
                 , Application, Nature, ApplicationArea, Movement, CostPrice,
                 //End rev rajdip
                 //Rev Bapi
                  VendorIDs
                  //End rev Bapi
                   
                 );
                //updtcnt = oStore_MasterBL.UpdateProduct(Convert.ToInt32(WhichType), txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text, Convert.ToString(CmbProType.SelectedItem == null ? 0 : CmbProType.SelectedItem.Value),
                //   Convert.ToInt32(CmbProClassCode.SelectedItem == null ? 0 : CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text, Convert.ToInt32(txtTradingLot.Text), Convert.ToInt32(CmbTradingLotUnits.SelectedItem == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
                //   Convert.ToInt32(CmbQuoteCurrency.SelectedItem == null ? 0 : CmbQuoteCurrency.SelectedItem.Value), Convert.ToInt32(txtQuoteLot.Text), Convert.ToInt32(CmbQuoteLotUnit.SelectedItem == null ? 0 : CmbQuoteLotUnit.SelectedItem.Value), Convert.ToInt32(txtDeliveryLot.Text),
                //   Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value), Convert.ToInt32(CmbProductColor.SelectedItem == null ? 0 : CmbProductColor.SelectedItem.Value), Convert.ToInt32(CmbProductSize.SelectedItem == null ? 0 : CmbProductSize.SelectedItem.Value), Convert.ToInt32(HttpContext.Current.Session["userid"]));

                if (updtcnt > 0)
                {
                    cityGrid.JSProperties["cpUpdate"] = "Success";
                   // RefereshApplicationProductData();
                    //BindGrid();
                }
                else
                {
                    cityGrid.JSProperties["cpUpdate"] = "fail";
                }


            }
            //-------------------Rev Rajdip--------------------
            if (WhichCall == "savecopy")
            {

                //Rev Rajdip
                decimal MinSale = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                decimal maxsale = Convert.ToDecimal(txtMrp.Text.Trim());
                if (MinSale > maxsale)
                {
                    valid.Text = "*";
                    cityGrid.JSProperties["cpinsert"] = "Validation";
                    return;
                }
                //End Rev Rajdip
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();
                int TradingLot = 0;
                int QuoteLot = 0;
                int DeliveryLot = 0;
                int productSize = 0;
                int ProductColor = 0;
                string strMsg = "fail";
                //-----Arindam
                txtQuoteLot.Text = "1";
                txtTradingLot.Text = "1";
                txtDeliveryLot.Text = "1";

                string lenght = txtHeight.Text;
                string width = txtWidth.Text;
                string Thickness = txtThickness.Text;
                string size = ddlSize.Value.ToString();
                string SUOM = SizeUOM.Value;
                //Rev Rajdip
                //string series = txtSeries.Text;
                //string subcat = txtSubCat.Text;
                //string Finish = txtFinish.Text;
                string series = string.Empty;
                string Finish = string.Empty;
                string subcat = string.Empty;
                int Application = 0;
                int Nature = 0;
                if (ddlSeries.Value == null)
                {
                    series = "";
                }
                else
                {
                    series = ddlSeries.Value.ToString();
                }
                if (ddlfinish.Value == null)
                {
                    Finish = "";
                }
                else
                {
                    Finish = ddlfinish.Value.ToString();
                }
                if (ddlsubcat.Value == null)
                {
                    subcat = "";
                }
                else
                {
                    subcat = ddlsubcat.Value.ToString();
                }
                if (ddlproductapplication.Value == null)
                {
                    Application = 0;
                }
                else
                {
                    Application = Convert.ToInt32(ddlproductapplication.Value.ToString());
                }
                if (ddlproductnature.Value == null)
                {
                    Nature =0;
                }
                else
                {
                    Nature = Convert.ToInt32(ddlproductnature.Value.ToString());
                }
                string strdimension = string.Empty;
                string strpedestalno = string.Empty;
                string strcatno = string.Empty;
                string strwarranty = string.Empty;
                strdimension = txtdimension.Text.ToString();
                strpedestalno = txtpedestalno.Text.ToString();
                strcatno = txtcatno.Text.ToString();
                strwarranty = txtwarranty.Text.ToString();
                //Rev Rajdip
                
                string LeadTime = txtLeadtime.Text;
                string Coverage = txtCoverage.Value;
                string covuom = dvCovg.InnerText;
                string volume = txtVolumn.Value;
                string volumeuom = dvvolume.InnerText;
                string wight = txtWeight.Text;
                
                string UniqueName = "";

                if ((hdnAutoNumStg.Value == "PDAutoNum1") && (hdnTransactionType.Value == "PD"))
                {
                    UniqueName = hddnDocNo.Value.Trim();
                }
                else
                {
                    UniqueName = txtPro_Code.Text.Trim();
                }
                //--Arindam for tryparse
                /*insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                    Convert.ToString(CmbProType.SelectedItem.Value), Convert.ToInt32(CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text,
                    TradingLot, Convert.ToInt32(CmbTradingLotUnits.SelectedItem.Value));*/
                if (!string.IsNullOrEmpty(UniqueName) && !string.IsNullOrEmpty(txtPro_Name.Text.Trim()))
                {

                    if (int.TryParse(txtQuoteLot.Text, out QuoteLot))
                    {
                        if (int.TryParse(txtTradingLot.Text, out TradingLot))
                        {
                            if (int.TryParse(txtDeliveryLot.Text, out DeliveryLot))
                            {
                                //if (CmbProductSize.SelectedItem.Value != "") //28.12.2016 commented by Subhra because it's getting error
                                if (CmbProductSize.Text != "")
                                {
                                    productSize = Convert.ToInt32(CmbProductSize.SelectedItem.Value);
                                }
                                if (CmbProductColor.Text != "")
                                //if (CmbProductColor.SelectedItem.Value != "") //28.12.2016 commented by Subhra because it's getting error
                                {
                                    ProductColor = Convert.ToInt32(CmbProductColor.SelectedItem.Value);
                                }
                                Boolean Replaceable = false;
                                Boolean sizeapplicable = false;
                                Boolean colorapplicable = false;
                                Boolean isInventory = false;
                                Boolean autoApply = false;
                                Boolean isInstall = false;
                                Boolean isOldUnit = false;
                                Boolean IsServiceItem = false;
                                Boolean FurtheranceToBusiness = false;//Subhabrata
                                Boolean OverideConvertion = false; //Surojit 08-02-2019
                                Boolean IsMandatory = false; //Surojit 11-02-2019
                                decimal saleprice = 0;
                                decimal MinSaleprice = 0;
                                decimal purPrice = 0;
                                //rev srijeeta
                                decimal packageqty = 0;
                                //end of rev srijeeta
                                decimal MRP = 0;
                                decimal minLvl = 0;
                                decimal maxLvl = 0;
                                decimal reorderLvl = 0;
                                decimal reorder_qty = 0;
                               
                                Boolean isCapitalGoods = false;
                                int tdscode = 0;
                                int ComponentService = 0;
                                String ModelList = "";
                                decimal CostPrice = 0;
                                if (cmb_tdstcs.Value != null)
                                {
                                    tdscode = Convert.ToInt32(cmb_tdstcs.Value);
                                }

                                //Rev Start Tanmoy for Component of Service
                                if (chkComponentService.Checked)
                                {
                                    ComponentService = 1;
                                }

                                List<object> modList = lookup_Model.GridView.GetSelectedFieldValues("ModelID");
                                foreach (object mod in modList)
                                {
                                    ModelList += "," + mod;
                                }
                                //Rev End Tanmoy for Component of Service


                                if (chkFurtherance.Checked)
                                {
                                    FurtheranceToBusiness = true;
                                }

                                if (ChkAutoApply.Checked)
                                    autoApply = true;

                                if (chkOverideConvertion.Checked) //Surojit 08-02-2019
                                    OverideConvertion = true;

                                if (chkIsMandatory.Checked) //Surojit 11-02-2019
                                    IsMandatory = true;

                                if (txtReorderLvl.Text.Trim() != "")
                                {
                                    reorderLvl = Convert.ToDecimal(txtReorderLvl.Text.Trim());
                                }

                                if (txtReorderQty.Text.Trim() != "")
                                {
                                    reorder_qty = Convert.ToDecimal(txtReorderQty.Text.Trim());

                                }
                                 if (txtMinLvl.Text.Trim() != "")
                                {
                                    minLvl = Convert.ToDecimal(txtMinLvl.Text.Trim());
                                }


                                if (txtMaxLvl.Text.Trim() != "")
                                {
                                    maxLvl = Convert.ToDecimal(txtMaxLvl.Text.Trim());
                                }




                                if (txtMrp.Text.Trim() != "")
                                {
                                    MRP = Convert.ToDecimal(txtMrp.Text.Trim());
                                }

                                if (txtPurPrice.Text.Trim() != "")
                                {
                                    purPrice = Convert.ToDecimal(txtPurPrice.Text.Trim());
                                }
                                //rev srijeetatxtpackageqty
                                if (txtpackageqty.Text.Trim() != "")
                                {
                                   packageqty = Convert.ToDecimal(txtpackageqty.Text.Trim());
                                }
                                //end of rev srijeeta
                                if (txtCostPrice.Text.Trim() != "")
                                {
                                    CostPrice = Convert.ToDecimal(txtCostPrice.Text.Trim());
                                }
                                if (txtMinSalePrice.Text.Trim() != "")
                                {
                                    MinSaleprice = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                                }

                                if (txtSalePrice.Text.Trim() != "")
                                {
                                    saleprice = Convert.ToDecimal(txtSalePrice.Text.Trim());
                                }
                                if (rdblappColor.Items[0].Selected)
                                {
                                    colorapplicable = true;
                                }
                                else
                                {
                                    colorapplicable = false;
                                }


                                if (rdblapp.Items[0].Selected)
                                {
                                    sizeapplicable = true;
                                }
                                else
                                {
                                    sizeapplicable = false;
                                }

                                if (Convert.ToString(cmbIsInventory.SelectedItem.Value) == "1")
                                    isInventory = true;

                                //Get Product Componnet details
                                String ProdComponent = "";
                                List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("sProducts_ID");
                                foreach (object Pobj in ComponentList)
                                {
                                    ProdComponent += "," + Pobj;
                                }
                                ProdComponent = ProdComponent.TrimStart(',');

                                if (Convert.ToString(aspxInstallation.SelectedItem.Value) == "1")
                                    isInstall = true;

                                if (Convert.ToString(cmbOldUnit.SelectedItem.Value) == "1")
                                    isOldUnit = true;


                                if (Convert.ToString(cmbIsCapitalGoods.SelectedItem.Value) == "1")
                                    isCapitalGoods = true;

                                if (Convert.ToString(cmbServiceItem.SelectedItem.Value) == "1")
                                    IsServiceItem = true;

                                if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "Prd"))
                                {
                                    cityGrid.JSProperties["cpinsert"] = "UDFManddratory";
                                    return;

                                }

                                string ShortName = "";
                                int numberingId = 0;
                                if ((hdnAutoNumStg.Value == "PDAutoNum1") && (hdnTransactionType.Value == "PD"))
                                {
                                    ShortName = hddnDocNo.Value.Trim();
                                    numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                }
                                else
                                {
                                    ShortName = txtPro_Code.Text.Trim();
                                }
                                if (Convert.ToString(cmbReplaceable.SelectedItem.Value) == "1")
                                    Replaceable = true;


                                int ApplicationArea = Convert.ToInt32(cmbAppliArea.Value);
                                string Movement = Convert.ToString(txtMovement.Text);

                                if (HttpContext.Current.Session["userid"] != null)
                                {

                                    insertcount = oStore_MasterBL.InsertProductAfterCopy(strdimension, strpedestalno, strcatno, strwarranty, cisv, Convert.ToInt32(WhichType), ShortName, txtPro_Name.Text, txtPro_Description.Text,
                                     Convert.ToString(CmbProType.Value == null ? 0 : CmbProType.SelectedItem.Value),
                                        //chinmoy cooment 19-07-2019
                                        // Convert.ToInt32(CmbProClassCode.Value == null ? 0 : CmbProClassCode.SelectedItem.Value), 
                                     (ClassId.Value == "" ? 0 : Convert.ToInt32(ClassId.Value)),
                                        //End
                                     txtGlobalCode.Text,
                                     1, Convert.ToInt32(CmbTradingLotUnits.Value == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
                                     1, 1, 1, 1, Convert.ToInt32(CmbDeliveryLotUnit.Value == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value), ProductColor,
                                     productSize, Convert.ToInt32(HttpContext.Current.Session["userid"]), sizeapplicable, colorapplicable,
                                     Convert.ToInt32(CmbBarCodeType.Value == null ? 0 : CmbBarCodeType.SelectedItem.Value), txtBarCodeNo.Text.Trim(),
                                     isInventory, Convert.ToString(CmbStockValuation.SelectedItem.Value), saleprice, MinSaleprice, purPrice, 
                                     //rev srijeeta
                                     packageqty,
                                     //end of rev srijeeta
                                     MRP,
                                     Convert.ToInt32(cmbStockUom.Value == null ? 0 : cmbStockUom.SelectedItem.Value), minLvl, reorderLvl,
                                     Convert.ToString(cmbNegativeStk.SelectedItem.Value), Convert.ToInt32(CmbTaxCodeSale.Value == null ? 0 : CmbTaxCodeSale.SelectedItem.Value),
                                     Convert.ToInt32(CmbTaxCodePur.Value == null ? 0 : CmbTaxCodePur.SelectedItem.Value), Convert.ToInt32(CmbTaxScheme.Value == null ? 0 : CmbTaxScheme.SelectedItem.Value),
                                     autoApply, Convert.ToString(fileName.Value), ProdComponent, Convert.ToString(CmbStatus.SelectedItem.Value),
                                        //chinmoy edited 22-07-2019
                                        //start
                                        //Convert.ToString(HsnLookUp.Text).Trim(),
                                     Convert.ToString(hdnHSN.Value).Trim(),
                                        //end
                                     Convert.ToInt32(AspxServiceTax.Value == null ? 0 : AspxServiceTax.Value),
                                     Convert.ToDecimal(txtPackingQty.Text.Trim()), Convert.ToDecimal(txtpacking.Text.Trim()), Convert.ToInt32(cmbPackingUom.Value != null ? cmbPackingUom.Value : 0),
                                     OverideConvertion, //Surojit 08-02-2019
                                     IsMandatory, //Surojit 11-02-2019
                                     isInstall, Convert.ToInt32(cmbBrand.Value == null ? 0 : cmbBrand.Value), isCapitalGoods, tdscode, Convert.ToString(Session["LastFinYear"]), isOldUnit,
                                     hdnSIMainAccount.Value == null ? "" : Convert.ToString(hdnSIMainAccount.Value), hdnSRMainAccount.Value == null ? "" : Convert.ToString(hdnSRMainAccount.Value),
                                     hdnPIMainAccount.Value == null ? "" : Convert.ToString(hdnPIMainAccount.Value), hdnPRMainAccount.Value == null ? "" : Convert.ToString(hdnPRMainAccount.Value), FurtheranceToBusiness, IsServiceItem, reorder_qty
                                        
                                     , maxLvl, lenght, width, Thickness, size, SUOM, series, Finish, LeadTime, Coverage, covuom, volume, volumeuom, wight, txtPro_Printname.Text, subcat, ComponentService, ModelList
                                     , Convert.ToString(txtDesignNo.Text), Convert.ToString(txtRevisionNo.Text), Replaceable
                                     , numberingId, Application, Nature, ApplicationArea, Movement, CostPrice
                                     );
                                    


                                    //chinmoy added 31-03-2020 Start	
                                    if (hdnAutoNumStg.Value == "PDAutoNum1")
                                    {
                                        if ((insertcount != 0))
                                        {
                                            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
                                            DataTable dts = new DataTable();
                                            DataTable delete = new DataTable();
                                            dts = BEngine.GetDataTable("select isnull(sProducts_Code,'') sProducts_Code from master_sproducts where sProducts_ID='" + insertcount + "'");
                                            if (dts.Rows.Count == 1)
                                            {
                                                if (Convert.ToString(dts.Rows[0]["sProducts_Code"]) == "Auto")
                                                {
                                                    delete = BEngine.GetDataTable("delete from master_sproducts where sProducts_ID='" + insertcount + "'");
                                                    if (hdnAutoNumStg.Value == "LDAutoNum1")
                                                    {
                                                        txt_CustDocNo.Text = "Auto";
                                                        txt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    else
                                                    {
                                                        txt_CustDocNo.Text = "Auto";
                                                        txt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Either Unique ID Exists OR Unique ID Exhausted.')", true);
                                                }
                                            }
                                        }
                                    }
                                    //End


                                    //Udf Add mode
                                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                    if (udfTable != null)
                                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("Prd", "ProductMaster" + Convert.ToString(insertcount), udfTable, Convert.ToString(Session["userid"]));

                                    //insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                                    //Convert.ToString(CmbProType.SelectedItem.Value), Convert.ToInt32(CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text,
                                    //TradingLot, Convert.ToInt32(CmbTradingLotUnits.SelectedItem.Value),
                                    //Convert.ToInt32(CmbQuoteCurrency.SelectedItem.Value), QuoteLot,
                                    //Convert.ToInt32(CmbQuoteLotUnit.SelectedItem.Value), DeliveryLot,
                                    //Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem.Value), ProductColor,
                                    //productSize, Convert.ToInt32(HttpContext.Current.Session["userid"]));


                                    strMsg = "Success";
                                    // RefereshApplicationProductData();
                                }
                                else
                                {
                                    strMsg = "Your session is end";
                                }

                            }
                            //else
                            //{
                            //    strMsg = "Delivery lot must be numeric value";
                            //}
                        }
                        //else
                        //{
                        //    strMsg = "Trading lot must be numeric value";
                        //}

                    }
                    //else
                    //{
                    //    strMsg = "Quote lot must be numeric value";
                    //}
                }
                //else
                //{
                //    strMsg = "Product Short Name (Unique) and Name is required";
                //}



                if (insertcount > 0)
                {
                    //Rev Rajdip For Copy Products
                    if (insertcount == 2)
                    {
                        cityGrid.JSProperties["cpinsert"] = "Duplicate Product";
                        //REV RAJDIP
                        return;
                        //END REV RAJDIP
                    }
                    //End Rev Rajdip
                    else
                    { 
                    cityGrid.JSProperties["cpinsert"] = "Success";            
                    //BindGrid();
                    }
                }
                else
                {
                    cityGrid.JSProperties["cpinsert"] = strMsg;
                }
            }
            //-------------------End Rev rajdip----------------
            /*---------------Edit for Active and dormant  Arindam 05-03-2019--------------------------------------------------------*/

            if (WhichCall == "updatecity_active")
            {
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();


                decimal MinLvl = 0;
                decimal maxLvl = 0;
                decimal reorderLvl = 0;
                decimal reorder_qty = 0;
                


                if (ASPxTextBox13.Text.Trim() != "")
                {
                    reorderLvl = Convert.ToDecimal(ASPxTextBox13.Text.Trim());
                }

                if (ASPxTextBox14.Text.Trim() != "")
                {
                    reorder_qty = Convert.ToDecimal(ASPxTextBox14.Text.Trim());

                }
               
                if (ASPxTextBox11.Text.Trim() != "")
                {
                    MinLvl = Convert.ToDecimal(ASPxTextBox11.Text.Trim());
                }

                if (ASPxTextBox12.Text.Trim() != "")
                {
                    maxLvl = Convert.ToDecimal(ASPxTextBox12.Text.Trim());
                }



                decimal MinSalesPrice  =0;
                decimal SalesPrice    = 0;
                decimal MRPSalesPrice = 0;
                decimal ActivePurPrice = 0;
                //rev srijeeta
                decimal activepackageqty = 0;
                //end of rev srijeeta
                decimal ActiveCostPrice = 0;
                if (txtActiveMinSalesPrice.Text.Trim() != "")
                {
                    MinSalesPrice = Convert.ToDecimal(txtActiveMinSalesPrice.Text.Trim());

                }

                if (txtActiveSalesPrice.Text.Trim() != "")
                {
                    SalesPrice = Convert.ToDecimal(txtActiveSalesPrice.Text.Trim());
                }

                if (txtActiveMRPSalesPrice.Text.Trim() != "")
                {
                    MRPSalesPrice = Convert.ToDecimal(txtActiveMRPSalesPrice.Text.Trim());
                }

                if (txtActivePurPrice.Text.Trim() != "")
                {
                    ActivePurPrice = Convert.ToDecimal(txtActivePurPrice.Text.Trim());
                }
                //rev srijeeta
                 if (txtactivepackageqty.Text.Trim() != "")
                {
                   activepackageqty = Convert.ToDecimal(txtactivepackageqty.Text.Trim());
                }
                //end of rev srijeeta
                if (txtActiveCostPrice.Text.Trim() != "")
                {
                    ActiveCostPrice = Convert.ToDecimal(txtActiveCostPrice.Text.Trim());
                }


                updtcnt = oStore_MasterBL.UpdateActiveDormant(Convert.ToInt32(WhichType), MinLvl, reorderLvl, hdnSIMainAccount.Value == null ? "" : Convert.ToString(hdnSIMainAccount.Value), hdnSRMainAccount.Value == null ? "" : Convert.ToString(hdnSRMainAccount.Value),
                 hdnPIMainAccount.Value == null ? "" : Convert.ToString(hdnPIMainAccount.Value), hdnPRMainAccount.Value == null ? "" : Convert.ToString(hdnPRMainAccount.Value), reorder_qty,
                  maxLvl, Convert.ToInt32(HttpContext.Current.Session["userid"]), MinSalesPrice, SalesPrice, MRPSalesPrice, ActivePurPrice,
                 ActiveCostPrice
                  
                );


                if (updtcnt > 0)
                {
                    cityGrid.JSProperties["cpUpdate"] = "Success";
                    RefereshApplicationProductData();
                   // BindGrid();
                }
                else
                {
                    cityGrid.JSProperties["cpUpdate"] = "fail";
                }


            }







            /*---------------Edit for Active and dormant Arindam 05-03-2019--------------------------------------------------------*/






            if (WhichCall == "Delete")
            {
                int checkinvalue = masterChecking.DeleteProduct(Convert.ToInt32(WhichType));
                if (checkinvalue > 0)
                {


                    string[] filePath = oDBEngine.GetFieldValue1("Master_sProducts", "sProduct_ImagePath", "sProducts_ID=" + WhichType, 1);
                    if (filePath[0] != "")
                    {
                        if ((System.IO.File.Exists(Server.MapPath(filePath[0]))))
                        {
                            System.IO.File.Delete(Server.MapPath(filePath[0]));
                        }
                    }

                    oGenericMethod.Delete_Table("tbl_master_product_packingDetails", "packing_sProductId=" + WhichType + "");
                    deletecnt = oGenericMethod.Delete_Table("Master_sProducts", "sProducts_ID=" + WhichType + "");
                    if (deletecnt > 0)
                    {
                        deletecnt = oGenericMethod.Delete_Table("tbl_master_ProdComponent", "Product_id=" + WhichType + "");
                        cityGrid.JSProperties["cpDelete"] = "Success";
                        RefereshApplicationProductData();
                       // BindGrid();
                    }
                    else
                        cityGrid.JSProperties["cpDelete"] = "Fail";

                }
                else
                {
                    if (checkinvalue == -2)
                    {
                        cityGrid.JSProperties["cpDelete"] = "Fail";
                        cityGrid.JSProperties["cpErrormsg"] = "Transaction exists. Cannot delete.";
                    }
                    else if (checkinvalue == -3)
                    {
                        string UsedProductList = "";
                        DataTable dt = oDBEngine.GetDataTable("select (select sproducts_name from Master_sProducts where sProducts_ID=h.Product_id) as Product_idName,Product_id, (select sproducts_name from Master_sProducts where sProducts_ID=h.Component_prodId) as Component_prodIdName,Component_prodId from tbl_master_ProdComponent h  where h.Component_prodId=" + WhichType + " or Product_id=" + WhichType + "");
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToString(dr["Component_prodId"]).Trim() == WhichType)
                                {
                                    UsedProductList += ", " + Convert.ToString(dr["Product_idName"]);
                                }
                                else
                                {
                                    UsedProductList += ", " + Convert.ToString(dr["Component_prodIdName"]);
                                }
                            }
                            cityGrid.JSProperties["cpDelete"] = "Fail";
                            cityGrid.JSProperties["cpErrormsg"] = "This Product associated with " + UsedProductList.TrimStart(',') + ". \n Cannot delete.";
                        }
                    }
                    else
                    {
                        cityGrid.JSProperties["cpDelete"] = "Fail";
                        cityGrid.JSProperties["cpErrormsg"] = "Product is in use. Cannot delete.";
                    }
                }
            }
            if (WhichCall == "Edit" || WhichCall == "Active")
            {
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();
                Session["KeyVal_InternalID"] = "ProductMaster" + Convert.ToString(WhichType).Trim();
                DataTable dtEdit = oStore_MasterBL.GetProductDetails(Convert.ToInt32(WhichType));

             

                cityGrid.JSProperties["cpMainAccountInUse"] = oStore_MasterBL.getUsedMainAccountByProductId(Convert.ToInt32(WhichType));
                int CONSITEMINSTKVAL = 0;
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string sProducts_ID = Convert.ToString(dtEdit.Rows[0]["sProducts_ID"]);
                    string sProducts_Code = Convert.ToString(dtEdit.Rows[0]["sProducts_Code"]);
                    string sProducts_Name = Convert.ToString(dtEdit.Rows[0]["sProducts_Name"]);
                    string sProducts_Description = Convert.ToString(dtEdit.Rows[0]["sProducts_Description"]);
                    string sProducts_Type = (Convert.ToString(dtEdit.Rows[0]["sProducts_Type"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_Type"]));
                    string sProducts_TypeFull = Convert.ToString(dtEdit.Rows[0]["sProducts_TypeFull"]);
                    string ProductClass_Code = (Convert.ToString(dtEdit.Rows[0]["ProductClass_Code"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["ProductClass_Code"]));
                    string sProducts_GlobalCode = Convert.ToString(dtEdit.Rows[0]["sProducts_GlobalCode"]);
                    string sProducts_TradingLot = Convert.ToString(dtEdit.Rows[0]["sProducts_TradingLot"]);
                    string sProducts_TradingLotUnit = (Convert.ToString(dtEdit.Rows[0]["sProducts_TradingLotUnit"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_TradingLotUnit"]));
                    string sProducts_QuoteCurrency = (Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteCurrency"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteCurrency"]));
                    string sProducts_QuoteLot = Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteLot"]);
                    string sProducts_QuoteLotUnit = (Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteLotUnit"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteLotUnit"]));
                    string sProducts_DeliveryLot = Convert.ToString(dtEdit.Rows[0]["sProducts_DeliveryLot"]);
                    string sProducts_DeliveryLotUnit = (Convert.ToString(dtEdit.Rows[0]["sProducts_DeliveryLotUnit"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_DeliveryLotUnit"]));
                    string sProducts_Color = Convert.ToString(dtEdit.Rows[0]["sProducts_Color"]);
                    string sProducts_Size = Convert.ToString(dtEdit.Rows[0]["sProducts_Size"]);
                    string sProducts_CreateUser = Convert.ToString(dtEdit.Rows[0]["sProducts_CreateUser"]);
                    string sProducts_CreateTime = Convert.ToString(dtEdit.Rows[0]["sProducts_CreateTime"]);
                    string sProducts_ModifyUser = Convert.ToString(dtEdit.Rows[0]["sProducts_ModifyUser"]);
                    string sProducts_ModifyTime = Convert.ToString(dtEdit.Rows[0]["sProducts_ModifyTime"]);
                    //Rev Rajdip
                    if (dtEdit.Rows[0]["CONSITEMINSTKVAL"].ToString() == "True")
                    {
                        CONSITEMINSTKVAL = 1;
                    }
                    else
                    {
                        CONSITEMINSTKVAL = 0;
                    }
                    string ProductApplication = Convert.ToString(dtEdit.Rows[0]["ProductApplication"]);
                    string ProductNature = Convert.ToString(dtEdit.Rows[0]["ProductNature"]);

                    string ProductDimension=string.Empty;
                    string ProductPedestalNo=string.Empty;
                    string ProductCatNo=string.Empty;
                    string ProductWarranty=string.Empty;
                    if(Convert.ToString(dtEdit.Rows[0]["ProductDimension"])!="" && Convert.ToString(dtEdit.Rows[0]["ProductDimension"])!=null)
                    {
                        ProductDimension = Convert.ToString(dtEdit.Rows[0]["ProductDimension"]);
                    }
                    if (Convert.ToString(dtEdit.Rows[0]["ProductPedestalNo"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductPedestalNo"]) != null)
                    {
                        ProductPedestalNo = Convert.ToString(dtEdit.Rows[0]["ProductPedestalNo"]);
                    }
                    if (Convert.ToString(dtEdit.Rows[0]["ProductCatNo"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductCatNo"]) != null)
                    {
                        ProductCatNo = Convert.ToString(dtEdit.Rows[0]["ProductCatNo"]);
                    }
                    if (Convert.ToString(dtEdit.Rows[0]["ProductWarranty"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductWarranty"]) != null)
                    {
                        ProductWarranty = Convert.ToString(dtEdit.Rows[0]["ProductWarranty"]);
                    }
                    
                     //End Rev Rajdip
                    //.................Code  Added By Sam on 25102016....................................................
                    string sProducts_SizeApplicable = Convert.ToString(dtEdit.Rows[0]["sProducts_SizeApplicable"]);
                    string sProducts_ColorApplicable = Convert.ToString(dtEdit.Rows[0]["sProducts_ColorApplicable"]);
                    //.................Code Above Added By Sam on 25102016....................................................

                    string Is_ComponentsMandatory = Convert.ToString(dtEdit.Rows[0]["Is_ComponentsMandatory"]); //Surojit 11-02-2019

                    //............Code Added by Debjyoti 30-12-2016
                    string sProducts_barCodeType = Convert.ToString(dtEdit.Rows[0]["sProducts_barCodeType"]);
                    string sProducts_barCode = Convert.ToString(dtEdit.Rows[0]["sProducts_barCode"]);

                    //--------------Code added by Debjyoti 04-01-2017
                    string sProduct_IsInventory = Convert.ToString(dtEdit.Rows[0]["sProduct_IsInventory"]);
                    string sFurtheranceToBusiness = Convert.ToString(dtEdit.Rows[0]["FurtheranceToBusiness"]);//Subhabrata
                    string stkValuation = Convert.ToString(dtEdit.Rows[0]["sProduct_Stockvaluation"]);
                    string sProduct_SalePrice = Convert.ToString(dtEdit.Rows[0]["sProduct_SalePrice"]);
                    string sProduct_MinSalePrice = Convert.ToString(dtEdit.Rows[0]["sProduct_MinSalePrice"]);
                    string sProduct_PurPrice = Convert.ToString(dtEdit.Rows[0]["sProduct_PurPrice"]);
                    //rev srijeeta
                    string sProduct_packageqty = Convert.ToString(dtEdit.Rows[0]["sProduct_packageqty"]);
                    //ed of rev srijeeta
                    string sProduct_MRP = Convert.ToString(dtEdit.Rows[0]["sProduct_MRP"]);
                    string sProduct_StockUOM = (Convert.ToString(dtEdit.Rows[0]["sProduct_StockUOM"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProduct_StockUOM"]));
                    string sProduct_MinLvl = (Convert.ToString(dtEdit.Rows[0]["sProduct_MinLvl"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProduct_MinLvl"]));
                    string sProduct_reOrderLvl = (Convert.ToString(dtEdit.Rows[0]["sProduct_reOrderLvl"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProduct_reOrderLvl"]));
                    string sProduct_NegativeStock = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProduct_NegativeStock"]) == "" ? "W" : dtEdit.Rows[0]["sProduct_NegativeStock"]);
                    string sProduct_TaxSchemeSale = Convert.ToString(dtEdit.Rows[0]["sProduct_TaxSchemeSale"]);
                    string sProduct_TaxSchemePur = Convert.ToString(dtEdit.Rows[0]["sProduct_TaxSchemePur"]);
                    string sProduct_TaxScheme = Convert.ToString(dtEdit.Rows[0]["sProduct_TaxScheme"]);
                    string sProduct_AutoApply = Convert.ToString(dtEdit.Rows[0]["sProduct_AutoApply"]);
                    string sProduct_ImagePath = Convert.ToString(dtEdit.Rows[0]["sProduct_ImagePath"]);
                    string sproductcomponent = Convert.ToString(dtEdit.Rows[0]["ProductComponent"]).Trim();
                    sproductcomponent = sproductcomponent.TrimStart(',');
                    string sProduct_Status = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProduct_Status"]) == "" ? "A" : dtEdit.Rows[0]["sProduct_Status"]);
                    //Packing details
                    string sProduct_quantity = Convert.ToString(dtEdit.Rows[0]["sProduct_quantity"]);
                    string packing_quantity = Convert.ToString(dtEdit.Rows[0]["packing_quantity"]);
                    string packing_saleUOM = Convert.ToString(dtEdit.Rows[0]["packing_saleUOM"]);
                    string MaxLvl = Convert.ToString(dtEdit.Rows[0]["MaxLvl"]);

                    string isOverideConvertion = Convert.ToString(dtEdit.Rows[0]["isOverideConvertion"]); //Surojit 08-02-2019
                    //string isConvertionOverideVisible = Convert.ToString(dtEdit.Rows[0]["isConvertionOverideVisible"]); //Surojit 08-02-2019
                    string isConvertionOverideVisible = objmaster.GetSettings("ConvertionOverideVisible"); //Surojit 14-02-2019
                    string isProductMasterComponentMandatoryVisible = objmaster.GetSettings("ProductComponentMandatoryInAllCompany"); //Surojit 14-02-2019

                    string isComponentService = Convert.ToString(dtEdit.Rows[0]["isComponentService"]);//Tanmoy 24-04-2020

                    string sProduct_IsReplaceable = Convert.ToString(dtEdit.Rows[0]["sProduct_IsReplaceable"]);

                    
                    if (string.IsNullOrEmpty(sproductcomponent))
                    {
                        sproductcomponent = "N";
                    }
                    string sProducts_HsnCode = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProducts_HsnCode"]) == "0" ? "" : dtEdit.Rows[0]["sProducts_HsnCode"]);
                    string sProducts_serviceTax = Convert.ToString(dtEdit.Rows[0]["sProducts_serviceTax"]);
                    string sProducts_isInstall = Convert.ToString(dtEdit.Rows[0]["sProducts_isInstall"]);
                    string sProducts_Brand = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProducts_Brand"]) == "0" ? "" : dtEdit.Rows[0]["sProducts_Brand"]);
                    string sProduct_IsCapitalGoods = Convert.ToString(dtEdit.Rows[0]["sProduct_IsCapitalGoods"]);
                    string Is_ServiceItem = Convert.ToString(dtEdit.Rows[0]["Is_ServiceItem"]);
                    string TdsTcs = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["TDSTCS_ID"]) == "0" ? "" : dtEdit.Rows[0]["TDSTCS_ID"]);
                    string sProducts_IsOldUnit = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProducts_IsOldUnit"]) == "0" ? "" : dtEdit.Rows[0]["sProducts_IsOldUnit"]);
                    string sInv_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sInv_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["sInv_MainAccount"]);
                    string sRet_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sRet_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["sRet_MainAccount"]);
                    string pInv_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pInv_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["pInv_MainAccount"]);
                    string pRet_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pRet_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["pRet_MainAccount"]);
                    string sInv_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sInv_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["sInv_MainAccount_name"]);
                    string sRet_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sRet_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["sRet_MainAccount_name"]);
                    string pInv_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pInv_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["pInv_MainAccount_name"]);
                    string pRet_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pRet_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["pRet_MainAccount_name"]);
                    string MASIExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MASIExists"]) == "" ? "0" : dtEdit.Rows[0]["MASIExists"]);
                    string MASRExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MASRExists"]) == "" ? "0" : dtEdit.Rows[0]["MASRExists"]);
                    string MAPIExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MAPIExists"]) == "" ? "0" : dtEdit.Rows[0]["MAPIExists"]);
                    string MAPRExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MAPRExists"]) == "" ? "0" : dtEdit.Rows[0]["MAPRExists"]);

                    /* code added by Arindam*/
                    string reorder_qty = (Convert.ToString(dtEdit.Rows[0]["Reorder_Quantity"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["Reorder_Quantity"]));
                    string LENGTH = Convert.ToString(dtEdit.Rows[0]["LENGTH"]);
                    string WIDTH = Convert.ToString(dtEdit.Rows[0]["WIDTH"]);
                    string THICKNESS = Convert.ToString(dtEdit.Rows[0]["THICKNESS"]);
                    string SIZE_UOM = Convert.ToString(dtEdit.Rows[0]["SIZE_UOM"]);
                    string SIZE_APPLICABLE_ON = Convert.ToString(dtEdit.Rows[0]["SIZE_APPLICABLE_ON"]);
                    string SERIES = Convert.ToString(dtEdit.Rows[0]["SERIES"]);
                    string FINISH = Convert.ToString(dtEdit.Rows[0]["FINISH"]);
                    string LEADTIME = Convert.ToString(dtEdit.Rows[0]["LEADTIME"]);
                    string COVERAGE = Convert.ToString(dtEdit.Rows[0]["COVERAGE"]);
                    string COVERAGE_UOM = Convert.ToString(dtEdit.Rows[0]["COVERAGE_UOM"]);
                    string VOLUME = Convert.ToString(dtEdit.Rows[0]["VOLUME"]);
                    string VOLUME_UOM = Convert.ToString(dtEdit.Rows[0]["VOLUME_UOM"]);
                    string WEIGHT = Convert.ToString(dtEdit.Rows[0]["WEIGHT"]);
                    string PRINT_NAME = Convert.ToString(dtEdit.Rows[0]["PRINT_NAME"]);
                    string subcat = Convert.ToString(dtEdit.Rows[0]["SUBCATEGORY"]);
                    string ClassName = Convert.ToString(dtEdit.Rows[0]["ClassName"]);
                    string HSNCode = Convert.ToString(dtEdit.Rows[0]["HSNCode"]);


                    string DesignNo = Convert.ToString(dtEdit.Rows[0]["DesignNo"]);
                    string RevisionNo = Convert.ToString(dtEdit.Rows[0]["RevisionNo"]);
                    string sProducts_ItemType = Convert.ToString(dtEdit.Rows[0]["sProducts_ItemType"]);
                    int Application_Area = Convert.ToInt32(dtEdit.Rows[0]["Application_Area"]);
                    string Movement = Convert.ToString(dtEdit.Rows[0]["Movement"]);
                    string sProduct_Cost = Convert.ToString(dtEdit.Rows[0]["sProduct_Cost"]);
                    DataSet dtVendors = GetVendorsByProductID(Convert.ToInt32(WhichType));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVendors.Tables[0].Rows[0]["VendorIDs"])))
                    {
                        btnVendors.Text = Convert.ToString(dtVendors.Tables[0].Rows[0]["Vendors"]);
                        hdVendorsID.Value = Convert.ToString(dtVendors.Tables[0].Rows[0]["VendorIDs"]);



                        btnVendors.DataBind();

                        btnVendors.ClientEnabled = true;

                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dtVendors.Tables[1].Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dtVendors.Tables[1].Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsonProducts.InnerText = serializer.Serialize(rows);

                    var jsonVendors = jsonProducts.InnerText;

                    string sVendors_name = Convert.ToString(btnVendors.Text);
                    string sVendors_IDs = Convert.ToString(hdVendorsID.Value);
                    cityGrid.JSProperties["cpEdit"] = sProducts_ID + "~"//0
                                                    + sProducts_Code + "~"//1
                                                    + sProducts_Name + "~"//2
                                                    + sProducts_Description + "~"//3
                                                    + sProducts_Type + "~"//4
                                                    + sProducts_TypeFull + "~"//5
                                                    + ProductClass_Code + "~"//6
                                                    + sProducts_GlobalCode + "~"//7
                                                    + sProducts_TradingLot + "~"//8
                                                    + sProducts_TradingLotUnit + "~"//9
                                                    + sProducts_QuoteCurrency + "~"//10
                                                    + sProducts_QuoteLot + "~"//11
                                                    + sProducts_QuoteLotUnit + "~"//12
                                                    + sProducts_DeliveryLot + "~"//13
                                                    + sProducts_DeliveryLotUnit + "~"//14
                                                    + sProducts_Color + "~"//15
                                                    + sProducts_Size + "~"//16
                        //.................Code  Added By Sam on 25102016....................................................
                        ////+ WhichType;
                                                    + WhichType + "~"//17
                                                    + sProducts_SizeApplicable + "~"//18
                                                    + sProducts_ColorApplicable + "~"//19
                        //.................Code Above Added By Sam on 25102016....................................................

                    //------------------------Code added by Debjyoti 30-12-2016
                        // + sProducts_ColorApplicable;
                                                    + sProducts_barCodeType + "~"//20
                                                    + sProducts_barCode + "~"//21
                        //------------------------Code added by Debjyoti 30-12-2016
                        // + sProducts_barCode;
                                                    + sProduct_IsInventory + "~"//22
                                                    + stkValuation + "~"//23
                                                    + sProduct_SalePrice + "~"//24
                                                    + sProduct_MinSalePrice + "~"//25
                                                    + sProduct_PurPrice + "~"//26
                                                    + sProduct_MRP + "~"//27
                                                    + sProduct_StockUOM + "~"//28
                                                    + sProduct_MinLvl + "~"//29
                                                    + sProduct_reOrderLvl + "~"//30
                                                    + sProduct_NegativeStock + "~"//31
                                                    + sProduct_TaxSchemeSale + "~"//32
                                                    + sProduct_TaxSchemePur + "~"//33
                                                    + sProduct_TaxScheme + "~"//34
                                                    + sProduct_AutoApply + "~"//35
                                                    + sProduct_ImagePath + "~"//36
                                                    + sproductcomponent + "~"//37
                                                    + sProduct_Status + "~"//38
                                                    + sProducts_HsnCode + "~"//39
                                                    + sProducts_serviceTax + "~"//40
                                                    + sProduct_quantity + "~"//41
                                                    + packing_quantity + "~"//42
                                                    + packing_saleUOM + "~"//43

                                                    + sProducts_isInstall + "~"//44
                                                    + sProducts_Brand + "~"//45
                                                    + sProduct_IsCapitalGoods + "~"//46
                                                    + TdsTcs + "~"//47
                                                    + sProducts_IsOldUnit + "~"//48
                                                    + sInv_MainAccount + "~"//49
                                                    + sRet_MainAccount + "~"//50
                                                    + pInv_MainAccount + "~"//51
                                                    + pRet_MainAccount + "~"//52
                                                    + sFurtheranceToBusiness + "~"//53
                                                    + Is_ServiceItem + "~"//54
                                                    + sInv_MainAccount_name + "~"//55
                                                    + sRet_MainAccount_name + "~"//56
                                                    + pInv_MainAccount_name + "~"//57
                                                    + pRet_MainAccount_name + "~"//58
                                                    + MASIExist + "~"//59
                                                    + MASRExist + "~"//60
                                                    + MAPIExist + "~"//61
                                                    + MAPRExist + "~"//62
                                                    + reorder_qty + "~"//63
                                                    + MaxLvl + "~"//64
                                                    + isOverideConvertion + "~" //Surojit 08-02-2019//65
                                                    + isConvertionOverideVisible + "~" //Surojit 08-02-2019//66
                                                    + Is_ComponentsMandatory + "~"//Surojit 11-02-2019//67
                                                    + isProductMasterComponentMandatoryVisible + "~"//Surojit 14-02-2019//68
                                                    + LENGTH + "~"//69
                                                    + WIDTH + "~"//70
                                                    + THICKNESS + "~"//71
                                                    + SIZE_UOM + "~"//72
                                                    + SIZE_APPLICABLE_ON + "~"//73
                                                    + SERIES + "~"//74
                                                    + FINISH + "~"//75
                                                    + LEADTIME + "~"//76
                                                    + COVERAGE + "~"//77
                                                    + COVERAGE_UOM + "~"//78
                                                    + VOLUME + "~"//79
                                                    + VOLUME_UOM + "~"//80
                                                    + WEIGHT + "~"//81
                                                    + PRINT_NAME + "~"//82
                                                    + subcat + "~" + ClassName + "~" + HSNCode//85
                        //Rev Rajdip
                                                    + "~" + CONSITEMINSTKVAL//86
                                                    + "~" + ProductApplication//87
                                                    + "~" + ProductNature//88

                                                     + "~" + ProductDimension//89
                                                    + "~" + ProductPedestalNo//90
                                                    + "~" + ProductCatNo//91
                                                    + "~" + ProductWarranty//92
                        //End Rev rajdip
                        //Rev Tanmoy
                                                    + "~" + isComponentService//93
                        //End Rev Tanmoy
                                                    + "~" + sProduct_IsReplaceable//94
                                                    + "~" + DesignNo//95
                                                    + "~" + RevisionNo//96
                                                    + "~" + sProducts_ItemType//97
                                                    + "~" + Application_Area//98
                                                    + "~" + Movement//99
                                                    + "~" + sProduct_Cost  //100
                        //Rev Bapi         
                                                    + "~" + sVendors_name   //101
                                                    + "~" + sVendors_IDs    //102
                                                    + "~" + jsonVendors //103
                        //103

                                //End Rev Bapi

                    //101
                                                //rev srijeeta
                                               + "~" + sProduct_packageqty;//104
                                                //end of rev srijeeta

               

                }
            }
            //Rev Rajdip to copy
            if (WhichCall == "Copy" || WhichCall == "Active")
            {
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();
                Session["KeyVal_InternalID"] = "ProductMaster" + Convert.ToString(WhichType).Trim();
                DataTable dtEdit = oStore_MasterBL.GetProductDetails(Convert.ToInt32(WhichType));

                cityGrid.JSProperties["cpMainAccountInUse"] = oStore_MasterBL.getUsedMainAccountByProductId(Convert.ToInt32(WhichType));

                int CONSITEMINSTKVAL = 0;
                if (dtEdit.Rows.Count > 0 && dtEdit != null)
                {
                    string sProducts_ID = Convert.ToString(dtEdit.Rows[0]["sProducts_ID"]);
                    string sProducts_Code = Convert.ToString(dtEdit.Rows[0]["sProducts_Code"]);
                    string sProducts_Name = Convert.ToString(dtEdit.Rows[0]["sProducts_Name"]);
                    string sProducts_Description = Convert.ToString(dtEdit.Rows[0]["sProducts_Description"]);
                    string sProducts_Type = (Convert.ToString(dtEdit.Rows[0]["sProducts_Type"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_Type"]));
                    string sProducts_TypeFull = Convert.ToString(dtEdit.Rows[0]["sProducts_TypeFull"]);
                    string ProductClass_Code = (Convert.ToString(dtEdit.Rows[0]["ProductClass_Code"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["ProductClass_Code"]));
                    string sProducts_GlobalCode = Convert.ToString(dtEdit.Rows[0]["sProducts_GlobalCode"]);
                    string sProducts_TradingLot = Convert.ToString(dtEdit.Rows[0]["sProducts_TradingLot"]);
                    string sProducts_TradingLotUnit = (Convert.ToString(dtEdit.Rows[0]["sProducts_TradingLotUnit"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_TradingLotUnit"]));
                    string sProducts_QuoteCurrency = (Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteCurrency"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteCurrency"]));
                    string sProducts_QuoteLot = Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteLot"]);
                    string sProducts_QuoteLotUnit = (Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteLotUnit"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_QuoteLotUnit"]));
                    string sProducts_DeliveryLot = Convert.ToString(dtEdit.Rows[0]["sProducts_DeliveryLot"]);
                    string sProducts_DeliveryLotUnit = (Convert.ToString(dtEdit.Rows[0]["sProducts_DeliveryLotUnit"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProducts_DeliveryLotUnit"]));
                    string sProducts_Color = Convert.ToString(dtEdit.Rows[0]["sProducts_Color"]);
                    string sProducts_Size = Convert.ToString(dtEdit.Rows[0]["sProducts_Size"]);
                    string sProducts_CreateUser = Convert.ToString(dtEdit.Rows[0]["sProducts_CreateUser"]);
                    string sProducts_CreateTime = Convert.ToString(dtEdit.Rows[0]["sProducts_CreateTime"]);
                    string sProducts_ModifyUser = Convert.ToString(dtEdit.Rows[0]["sProducts_ModifyUser"]);
                    string sProducts_ModifyTime = Convert.ToString(dtEdit.Rows[0]["sProducts_ModifyTime"]);
                    //.................Code  Added By Sam on 25102016....................................................
                    string sProducts_SizeApplicable = Convert.ToString(dtEdit.Rows[0]["sProducts_SizeApplicable"]);
                    string sProducts_ColorApplicable = Convert.ToString(dtEdit.Rows[0]["sProducts_ColorApplicable"]);
                    //.................Code Above Added By Sam on 25102016....................................................

                    string Is_ComponentsMandatory = Convert.ToString(dtEdit.Rows[0]["Is_ComponentsMandatory"]); //Surojit 11-02-2019

                    //............Code Added by Debjyoti 30-12-2016
                    string sProducts_barCodeType = Convert.ToString(dtEdit.Rows[0]["sProducts_barCodeType"]);
                    string sProducts_barCode = Convert.ToString(dtEdit.Rows[0]["sProducts_barCode"]);

                    //--------------Code added by Debjyoti 04-01-2017
                    string sProduct_IsInventory = Convert.ToString(dtEdit.Rows[0]["sProduct_IsInventory"]);
                    string sFurtheranceToBusiness = Convert.ToString(dtEdit.Rows[0]["FurtheranceToBusiness"]);//Subhabrata
                    string stkValuation = Convert.ToString(dtEdit.Rows[0]["sProduct_Stockvaluation"]);
                    string sProduct_SalePrice = Convert.ToString(dtEdit.Rows[0]["sProduct_SalePrice"]);
                    string sProduct_MinSalePrice = Convert.ToString(dtEdit.Rows[0]["sProduct_MinSalePrice"]);
                    string sProduct_PurPrice = Convert.ToString(dtEdit.Rows[0]["sProduct_PurPrice"]);
                    //rev srijeeta
                    string sProduct_packageqty = Convert.ToString(dtEdit.Rows[0]["sProduct_packageqty"]);
                    //end of rev srijeeta
                    string sProduct_MRP = Convert.ToString(dtEdit.Rows[0]["sProduct_MRP"]);
                    string sProduct_StockUOM = (Convert.ToString(dtEdit.Rows[0]["sProduct_StockUOM"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProduct_StockUOM"]));
                    string sProduct_MinLvl = (Convert.ToString(dtEdit.Rows[0]["sProduct_MinLvl"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProduct_MinLvl"]));
                    string sProduct_reOrderLvl = (Convert.ToString(dtEdit.Rows[0]["sProduct_reOrderLvl"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["sProduct_reOrderLvl"]));
                    string sProduct_NegativeStock = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProduct_NegativeStock"]) == "" ? "W" : dtEdit.Rows[0]["sProduct_NegativeStock"]);
                    string sProduct_TaxSchemeSale = Convert.ToString(dtEdit.Rows[0]["sProduct_TaxSchemeSale"]);
                    string sProduct_TaxSchemePur = Convert.ToString(dtEdit.Rows[0]["sProduct_TaxSchemePur"]);
                    string sProduct_TaxScheme = Convert.ToString(dtEdit.Rows[0]["sProduct_TaxScheme"]);
                    string sProduct_AutoApply = Convert.ToString(dtEdit.Rows[0]["sProduct_AutoApply"]);
                    string sProduct_ImagePath = Convert.ToString(dtEdit.Rows[0]["sProduct_ImagePath"]);
                    string sproductcomponent = Convert.ToString(dtEdit.Rows[0]["ProductComponent"]).Trim();
                    sproductcomponent = sproductcomponent.TrimStart(',');
                    string sProduct_Status = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProduct_Status"]) == "" ? "A" : dtEdit.Rows[0]["sProduct_Status"]);
                    //Packing details
                    string sProduct_quantity = Convert.ToString(dtEdit.Rows[0]["sProduct_quantity"]);
                    string packing_quantity = Convert.ToString(dtEdit.Rows[0]["packing_quantity"]);
                    string packing_saleUOM = Convert.ToString(dtEdit.Rows[0]["packing_saleUOM"]);
                    string MaxLvl = Convert.ToString(dtEdit.Rows[0]["MaxLvl"]);

                    string isOverideConvertion = Convert.ToString(dtEdit.Rows[0]["isOverideConvertion"]); //Surojit 08-02-2019
                    //string isConvertionOverideVisible = Convert.ToString(dtEdit.Rows[0]["isConvertionOverideVisible"]); //Surojit 08-02-2019
                    string isConvertionOverideVisible = objmaster.GetSettings("ConvertionOverideVisible"); //Surojit 14-02-2019
                    string isProductMasterComponentMandatoryVisible = objmaster.GetSettings("ProductComponentMandatoryInAllCompany"); //Surojit 14-02-2019

                    string isComponentService = Convert.ToString(dtEdit.Rows[0]["isComponentService"]);//Tanmoy 24-04-2020
                    string sProduct_IsReplaceable = Convert.ToString(dtEdit.Rows[0]["sProduct_IsReplaceable"]);
                    string ProductDimension = string.Empty;
                    string ProductPedestalNo = string.Empty;
                    string ProductCatNo = string.Empty;
                    string ProductWarranty = string.Empty;
                    if (Convert.ToString(dtEdit.Rows[0]["ProductDimension"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductDimension"]) != null)
                    {
                        ProductDimension = Convert.ToString(dtEdit.Rows[0]["ProductDimension"]);
                    }
                    if (Convert.ToString(dtEdit.Rows[0]["ProductPedestalNo"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductPedestalNo"]) != null)
                    {
                        ProductPedestalNo = Convert.ToString(dtEdit.Rows[0]["ProductPedestalNo"]);
                    }
                    if (Convert.ToString(dtEdit.Rows[0]["ProductCatNo"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductCatNo"]) != null)
                    {
                        ProductCatNo = Convert.ToString(dtEdit.Rows[0]["ProductCatNo"]);
                    }
                    if (Convert.ToString(dtEdit.Rows[0]["ProductWarranty"]) != "" && Convert.ToString(dtEdit.Rows[0]["ProductWarranty"]) != null)
                    {
                        ProductWarranty = Convert.ToString(dtEdit.Rows[0]["ProductWarranty"]);
                    }

                    if (dtEdit.Rows[0]["CONSITEMINSTKVAL"].ToString() == "True")
                    {
                        CONSITEMINSTKVAL = 1;
                    }
                    else
                    {
                        CONSITEMINSTKVAL = 0;
                    }


                    //End of packing details

                    //if (!string.IsNullOrEmpty(sproductcomponent))
                    //{

                    //    //sproductcomponent = "N";

                    //    var data = sproductcomponent.Split(',').ToList(); ;
                    //    List<string> ComponentList =data;
                    //    foreach (object Pobj in ComponentList)
                    //    {
                    //        GridLookup.GridView.Selection.SelectRowByKey(Pobj);
                    //    }
                    //}
                    if (string.IsNullOrEmpty(sproductcomponent))
                    {

                        sproductcomponent = "N";
                    }

                    string sProducts_HsnCode = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProducts_HsnCode"]) == "0" ? "" : dtEdit.Rows[0]["sProducts_HsnCode"]);
                    string sProducts_serviceTax = Convert.ToString(dtEdit.Rows[0]["sProducts_serviceTax"]);

                    string sProducts_isInstall = Convert.ToString(dtEdit.Rows[0]["sProducts_isInstall"]);
                    string sProducts_Brand = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProducts_Brand"]) == "0" ? "" : dtEdit.Rows[0]["sProducts_Brand"]);

                    string sProduct_IsCapitalGoods = Convert.ToString(dtEdit.Rows[0]["sProduct_IsCapitalGoods"]);
                    string Is_ServiceItem = Convert.ToString(dtEdit.Rows[0]["Is_ServiceItem"]);

                    string TdsTcs = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["TDSTCS_ID"]) == "0" ? "" : dtEdit.Rows[0]["TDSTCS_ID"]);
                    string sProducts_IsOldUnit = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sProducts_IsOldUnit"]) == "0" ? "" : dtEdit.Rows[0]["sProducts_IsOldUnit"]);

                    string sInv_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sInv_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["sInv_MainAccount"]);
                    string sRet_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sRet_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["sRet_MainAccount"]);
                    string pInv_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pInv_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["pInv_MainAccount"]);
                    string pRet_MainAccount = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pRet_MainAccount"]) == "0" ? "" : dtEdit.Rows[0]["pRet_MainAccount"]);


                    string sInv_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sInv_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["sInv_MainAccount_name"]);
                    string sRet_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["sRet_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["sRet_MainAccount_name"]);
                    string pInv_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pInv_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["pInv_MainAccount_name"]);
                    string pRet_MainAccount_name = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["pRet_MainAccount_name"]) == "" ? "" : dtEdit.Rows[0]["pRet_MainAccount_name"]);


                    string MASIExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MASIExists"]) == "" ? "0" : dtEdit.Rows[0]["MASIExists"]);
                    string MASRExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MASRExists"]) == "" ? "0" : dtEdit.Rows[0]["MASRExists"]);
                    string MAPIExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MAPIExists"]) == "" ? "0" : dtEdit.Rows[0]["MAPIExists"]);
                    string MAPRExist = Convert.ToString(Convert.ToString(dtEdit.Rows[0]["MAPRExists"]) == "" ? "0" : dtEdit.Rows[0]["MAPRExists"]);

                    /* code added by Arindam*/
                    string reorder_qty = (Convert.ToString(dtEdit.Rows[0]["Reorder_Quantity"]) == "0" ? "" : Convert.ToString(dtEdit.Rows[0]["Reorder_Quantity"]));
                    /* code added by Arindam*/



                    string LENGTH = Convert.ToString(dtEdit.Rows[0]["LENGTH"]);
                    string WIDTH = Convert.ToString(dtEdit.Rows[0]["WIDTH"]);
                    string THICKNESS = Convert.ToString(dtEdit.Rows[0]["THICKNESS"]);
                    string SIZE_UOM = Convert.ToString(dtEdit.Rows[0]["SIZE_UOM"]);

                    string SIZE_APPLICABLE_ON = Convert.ToString(dtEdit.Rows[0]["SIZE_APPLICABLE_ON"]);
                    string SERIES = Convert.ToString(dtEdit.Rows[0]["SERIES"]);
                    string FINISH = Convert.ToString(dtEdit.Rows[0]["FINISH"]);
                    string LEADTIME = Convert.ToString(dtEdit.Rows[0]["LEADTIME"]);

                    string COVERAGE = Convert.ToString(dtEdit.Rows[0]["COVERAGE"]);
                    string COVERAGE_UOM = Convert.ToString(dtEdit.Rows[0]["COVERAGE_UOM"]);
                    string VOLUME = Convert.ToString(dtEdit.Rows[0]["VOLUME"]);
                    string VOLUME_UOM = Convert.ToString(dtEdit.Rows[0]["VOLUME_UOM"]);
                    string WEIGHT = Convert.ToString(dtEdit.Rows[0]["WEIGHT"]);
                    string PRINT_NAME = Convert.ToString(dtEdit.Rows[0]["PRINT_NAME"]);
                    string subcat = Convert.ToString(dtEdit.Rows[0]["SUBCATEGORY"]);
                    string ClassName = Convert.ToString(dtEdit.Rows[0]["ClassName"]);
                    string HSNCode = Convert.ToString(dtEdit.Rows[0]["HSNCode"]);
                    string DesignNo = Convert.ToString(dtEdit.Rows[0]["DesignNo"]);
                    string RevisionNo = Convert.ToString(dtEdit.Rows[0]["RevisionNo"]);
                    string sProducts_ItemType = Convert.ToString(dtEdit.Rows[0]["sProducts_ItemType"]);
                    string ProductApplication = Convert.ToString(dtEdit.Rows[0]["ProductApplication"]);
                    string ProductNature = Convert.ToString(dtEdit.Rows[0]["ProductNature"]);
                    int Application_Area = Convert.ToInt32(dtEdit.Rows[0]["Application_Area"]);
                    string Movement = Convert.ToString(dtEdit.Rows[0]["Movement"]);
                    string sProduct_Cost = Convert.ToString(dtEdit.Rows[0]["sProduct_Cost"]);
                    DataSet dtVendors = GetVendorsByProductID(Convert.ToInt32(WhichType));
                    if (!string.IsNullOrEmpty(Convert.ToString(dtVendors.Tables[0].Rows[0]["VendorIDs"])))
                    {
                        btnVendors.Text = Convert.ToString(dtVendors.Tables[0].Rows[0]["Vendors"]);
                        hdVendorsID.Value = Convert.ToString(dtVendors.Tables[0].Rows[0]["VendorIDs"]);



                        btnVendors.DataBind();

                        btnVendors.ClientEnabled = true;

                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dtVendors.Tables[1].Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dtVendors.Tables[1].Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsonProducts.InnerText = serializer.Serialize(rows);

                    var jsonVendors = jsonProducts.InnerText;

                    string sVendors_name = Convert.ToString(btnVendors.Text);
                    string sVendors_IDs = Convert.ToString(hdVendorsID.Value);


                    cityGrid.JSProperties["cpCopy"] = sProducts_ID + "~"//0
                                                    + sProducts_Code + "~"//1
                                                    + sProducts_Name + "~"//2
                                                    + sProducts_Description + "~"//3
                                                    + sProducts_Type + "~"//4
                                                    + sProducts_TypeFull + "~"//5
                                                    + ProductClass_Code + "~"//6
                                                    + sProducts_GlobalCode + "~"//7
                                                    + sProducts_TradingLot + "~"//8
                                                    + sProducts_TradingLotUnit + "~"//9
                                                    + sProducts_QuoteCurrency + "~"//10
                                                    + sProducts_QuoteLot + "~"//11
                                                    + sProducts_QuoteLotUnit + "~"//12
                                                    + sProducts_DeliveryLot + "~"//13
                                                    + sProducts_DeliveryLotUnit + "~"//14
                                                    + sProducts_Color + "~"//15
                                                    + sProducts_Size + "~"//16
                        //.................Code  Added By Sam on 25102016....................................................
                        ////+ WhichType;
                                                    + WhichType + "~"//17
                                                    + sProducts_SizeApplicable + "~"//18
                                                    + sProducts_ColorApplicable + "~"//19
                        //.................Code Above Added By Sam on 25102016....................................................

                    //------------------------Code added by Debjyoti 30-12-2016
                        // + sProducts_ColorApplicable;
                                                    + sProducts_barCodeType + "~"//20
                                                    + sProducts_barCode + "~"//21
                        //------------------------Code added by Debjyoti 30-12-2016
                        // + sProducts_barCode;
                                                    + sProduct_IsInventory + "~"//22
                                                    + stkValuation + "~"//23
                                                    + sProduct_SalePrice + "~"//24
                                                    + sProduct_MinSalePrice + "~"//25
                                                    + sProduct_PurPrice + "~"//26
                                                 
                                                    + sProduct_MRP + "~"//27
                                                    + sProduct_StockUOM + "~"//28
                                                    + sProduct_MinLvl + "~"//29
                                                    + sProduct_reOrderLvl + "~"//30
                                                    + sProduct_NegativeStock + "~"//31
                                                    + sProduct_TaxSchemeSale + "~"//32
                                                    + sProduct_TaxSchemePur + "~"//33
                                                    + sProduct_TaxScheme + "~"//34
                                                    + sProduct_AutoApply + "~"//35
                                                    + sProduct_ImagePath + "~"//36
                                                    + sproductcomponent + "~"//37
                                                    + sProduct_Status + "~"//38
                                                    + sProducts_HsnCode + "~"//39
                                                    + sProducts_serviceTax + "~"//40
                                                    + sProduct_quantity + "~"//41
                                                    + packing_quantity + "~"//42
                                                    + packing_saleUOM + "~"//43

                                                    + sProducts_isInstall + "~"//44
                                                    + sProducts_Brand + "~"//45
                                                    + sProduct_IsCapitalGoods + "~"//46
                                                    + TdsTcs + "~"//47
                                                    + sProducts_IsOldUnit + "~"//48
                                                    + sInv_MainAccount + "~"//49
                                                    + sRet_MainAccount + "~"//50
                                                    + pInv_MainAccount + "~"//51
                                                    + pRet_MainAccount + "~"//52
                                                    + sFurtheranceToBusiness + "~"//53
                                                    + Is_ServiceItem + "~"//54
                                                    + sInv_MainAccount_name + "~"//55
                                                    + sRet_MainAccount_name + "~"//56
                                                    + pInv_MainAccount_name + "~"//57
                                                    + pRet_MainAccount_name + "~"//58
                                                    + MASIExist + "~"//59
                                                    + MASRExist + "~"//60
                                                    + MAPIExist + "~"//61
                                                    + MAPRExist + "~"//62
                                                    + reorder_qty + "~"//63
                                                   + MaxLvl + "~"//64
                                                    + isOverideConvertion + "~" //Surojit 08-02-2019//65
                                                    + isConvertionOverideVisible + "~" //Surojit 08-02-2019//66
                                                    + Is_ComponentsMandatory + "~"//Surojit 11-02-2019//67
                                                    + isProductMasterComponentMandatoryVisible + "~"//Surojit 14-02-2019//68
                                                    + LENGTH + "~"//69
                                                    + WIDTH + "~"//70
                                                    + THICKNESS + "~"//71
                                                    + SIZE_UOM + "~"//72
                                                    + SIZE_APPLICABLE_ON + "~"//73
                                                    + SERIES + "~"//74
                                                    + FINISH + "~"//75
                                                    + LEADTIME + "~"//76
                                                    + COVERAGE + "~"//77
                                                    + COVERAGE_UOM + "~"//78
                                                    + VOLUME + "~"//79
                                                    + VOLUME_UOM + "~"//80
                                                    + WEIGHT + "~"//81
                                                    + PRINT_NAME + "~"//82
                                                    + subcat + "~" + ClassName + "~" + HSNCode//85
                                                    + "~" + CONSITEMINSTKVAL//86
                                                    + "~" + ProductApplication + "~" + ProductNature//88

                                                    + "~" + ProductDimension//89
                                                    + "~" + ProductPedestalNo//90
                                                    + "~" + ProductCatNo//91
                                                    + "~" + ProductWarranty//92
                                                   
                                                    //Rev Tanmoy
                                                    + "~" + isComponentService//93
                                                    //End Rev Tanmoy
                                                    + "~" + sProduct_IsReplaceable//94
                                                    + "~" + DesignNo//95
                                                    + "~" + RevisionNo//96
                                                    + "~" + sProducts_ItemType//97

                                                    + "~" + Application_Area//98
                                                    + "~" + Movement//99
                                                    + "~" + sProduct_Cost  //100
                               
                                                    + "~" + sVendors_name   //101
                                                    + "~" + sVendors_IDs    //102
                                                    + "~" + jsonVendors //103                  
                   
                                                    + "~" + sProduct_packageqty;//104
                    
                }
            }
            //End Rev Rajdip to copy
        }
        public void RefereshApplicationProductData()
        {
            //DataTable dt = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("PRC_POS_PRODUCTLIST");
            //dt = proc.GetTable();
            //Application.Remove("POSPRODUCTLISTDATA");
            //Application.Add("POSPRODUCTLISTDATA", dt);
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

        [WebMethod]
        public static bool CheckUniqueCode(string MarketsCode)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                DataTable dtCmb = new DataTable();
                dtCmb = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Markets] WHERE [Markets_Code] = " + "'" + MarketsCode + "'");
                int cnt = dtCmb.Rows.Count;
                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }

        protected void ASPxCallback1_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxCallbackPanel sendrPanel = sender as ASPxCallbackPanel;
            FileUpload fp = (FileUpload)sendrPanel.FindControl("FileUpload1");
            //   fp.SaveAs(Server.MapPath("~/OMS/") + fp.PostedFile..FileName);
        }
        protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            ASPxUploadControl uploader = sender as ASPxUploadControl;
            string fileName = uploader.FileName;
            string name = fileName.Substring(0, fileName.IndexOf('.'));
            string exten = fileName.Substring(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.'));


            string ProductFilePath = "/CommonFolderErpCRM/ProductImages/" + name + Guid.NewGuid() + exten;
            uploader.SaveAs(Server.MapPath(ProductFilePath));
            e.CallbackData = ProductFilePath;


        }

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string componet = Convert.ToString(e.Parameter);
            ASPxCallbackPanel cbPanl = source as ASPxCallbackPanel;
            ASPxGridLookup LookUp = (ASPxGridLookup)cbPanl.FindControl("GridLookup");

            string[] eachComponet = componet.Split(',');
            LookUp.GridView.Selection.UnselectAll();
            LookUp.GridView.Selection.BeginSelection();

            foreach (string val in eachComponet)
            {
                LookUp.GridView.Selection.SelectRowByKey(val);
            }
            LookUp.GridView.Selection.EndSelection();
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='Prd' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "sProducts_ID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            ProcedureExecute proc = new ProcedureExecute("PRC_ALLMASTERPAGELISTING");
            proc.AddVarcharPara("@WHICHMODULE", 100, "PRODUCTS");
            proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.RunActionQuery();

           


                   
                    var q = from d in dc.ALLMASTERPAGELISTINGs
                            where Convert.ToString(d.USERID) == Userid && d.REPORTTYPE == "PRODUCTS"
                            orderby d.sProducts_ID descending 
                            select d;

                    e.QueryableSource = q;
            
             
        }
        //chinmoy comment 22-07-2019
        //protected void SetHSnPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string ClassId = e.Parameter;
        //    DataTable dt = oDBEngine.GetDataTable("select isnull(ProductClass_HSNCode,'') ProductClass_HSNCode  from Master_ProductClass where ProductClass_ID=" + ClassId);
        //    if (dt.Rows.Count > 0)
        //    {



        //        if (Convert.ToString(dt.Rows[0]["ProductClass_HSNCode"]) != "")
        //        {
        //            HsnLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(dt.Rows[0]["ProductClass_HSNCode"]));
        //            SetHSnPanel.JSProperties["cpHsnCode"] = Convert.ToString(dt.Rows[0]["ProductClass_HSNCode"]);
        //            HsnLookUp.ClientEnabled = false;
        //        }
        //        else
        //        {
        //            HsnLookUp.GridView.Selection.SelectRowByKey("");
        //            HsnLookUp.ClientEnabled = true;
        //        }
        //    }

        //}

        //End

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetMainAccount(string SearchKey)
        {
            List<MainAccount> listMainAccount = new List<MainAccount>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable MainAccount = oDBEngine.GetDataTable("Select top 10 * from (select '' MainAccount_AccountCode,'--Select--' MainAccount_Name union all  select MainAccount_AccountCode,MainAccount_Name from Master_MainAccount where MainAccount_BankCashType not in ('Cash','Bank') and " +
                                                               "MainAccount_AccountCode not in (select distinct SubAccount_MainAcReferenceID from Master_SubAccount where SubAccount_MainAcReferenceID is not null) " +
                                                               "and MainAccount_AccountCode not like 'SYSTM%' and MainAccount_AccountCode like '%" + SearchKey + "%' or MainAccount_Name like '%" + SearchKey + "%' ) TblMA  order by Len(MainAccount_AccountCode) ");


                listMainAccount = (from DataRow dr in MainAccount.Rows
                                   select new MainAccount()
                                   {
                                       MainAccount_Name = Convert.ToString(dr["MainAccount_Name"]),
                                       MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]),

                                   }).ToList();
            }

            return listMainAccount;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static  object GetClassDetails(string SearchKey)
        {
            List<ClassModel> listClass = new List<ClassModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable classes = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CLASSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
               
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(classes);

                cmd.Dispose();
                con.Dispose();

                listClass = (from DataRow dr in classes.Rows
                             select new ClassModel()
                             {
                                 id = dr["ID"].ToString(),
                                 Name = dr["Name"].ToString(),
                             }).ToList();
            }

            return listClass;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetHSNDetails(string SearchKey)
        {
            List<HSNModel> listMainAccount = new List<HSNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable MainAccount = oDBEngine.GetDataTable("select top 10  Code , isnull(Code,'') HSNCode,isnull(Description,'') HSNDesc from tbl_HSN_Master where Code like '%" + SearchKey + "%' or Description like '%" + SearchKey + "%'   order by Description");


                listMainAccount = (from DataRow dr in MainAccount.Rows
                                   select new HSNModel()
                                   {
                                       HSNId = Convert.ToInt32(dr["Code"]),
                                       id = Convert.ToString(dr["HSNCode"]),
                                       Name = Convert.ToString(dr["HSNDesc"]),

                                   }).ToList();
            }

            return listMainAccount;
        }
       

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetVendorsList(string SearchKey)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' ) as tbl " +
                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModel()
                           {
                               ID = dr["cnt_internalid"].ToString(),
                               NAME = dr["Name"].ToString(),

                           }).ToList();
            }

        

            return listVen;
        }

        #region Model Populate

        protected void Model_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindModelGrid")
            {
                DataTable ModelTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                ModelTable = oDBEngine.GetDataTable("select ModelID, ModelDesc from master_model order by ModelDesc");

                if (ModelTable.Rows.Count > 0)
                {
                    Session["ModelData"] = ModelTable;
                    lookup_Model.DataSource = ModelTable;
                    lookup_Model.DataBind();
                }
                else
                {
                    Session["ModelData"] = ModelTable;
                    lookup_Model.DataSource = null;
                    lookup_Model.DataBind();
                }
            }
            else if (e.Parameter.Split('~')[0] == "SetModelGrid")
            {
                BindLookUp();
               // string userid = Convert.ToString(HttpContext.Current.Session["userid"]);
                DataTable Modeldt = new DataTable();
                string ProdID = e.Parameter.Split('~')[1];

                Modeldt = oDBEngine.GetDataTable("select Model_id from SRV_ProductModelMap where Product_id=" + ProdID + "");
                if (Modeldt != null && Modeldt.Rows.Count > 0)
                {
                    lookup_Model.GridView.Selection.CancelSelection();
                    lookup_Model.GridView.Selection.CancelSelection();
                    foreach (DataRow item in Modeldt.Rows)
                    {
                        lookup_Model.GridView.Selection.SelectRowByKey(Convert.ToInt32(item["Model_id"].ToString()));
                    }
                }
            }
        }

        protected void lookup_Model_DataBinding(object sender, EventArgs e)
        {
            if (Session["ModelData"] != null)
            {
                lookup_Model.DataSource = (DataTable)Session["ModelData"];
            }
        }

        protected void BindLookUp()
        {
            DataTable ModelTable = oDBEngine.GetDataTable("select ModelID, ModelDesc from master_model order by ModelDesc");
            lookup_Model.GridView.Selection.CancelSelection();

            lookup_Model.GridView.Selection.CancelSelection();
            lookup_Model.DataSource = ModelTable;
            lookup_Model.DataBind();

            Session["ModelData"] = ModelTable;
        }

        #endregion

        protected void AvailableStockgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Session["AvailableStockPos"] = GetProductStockBranchWise(Convert.ToString(HDSelectedProduct.Value));
            //rev Pratik
            AvailableStockgrid.KeyFieldName = "BRANCH_ID";
            //End of rev Pratik
            AvailableStockgrid.DataBind();
        }

        public DataTable GetProductStockBranchWise(string ProdId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_GetAllProductsAvailableStock");
            proc.AddVarcharPara("@companyId", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@finYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));           
            proc.AddVarcharPara("@productId", 10, ProdId);
            ds = proc.GetTable();
            return ds;
        }


        //Rev Bapi
        public DataSet GetVendorsByProductID(int ProductID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProductVendorsMapping");
            proc.AddPara("@ProductID", ProductID);
            ds = proc.GetDataSet();
            return ds;
        }

        //End rev Bapi

        protected void AvailableStockgrid_DataBinding(object sender, EventArgs e)
        {
            DataTable availablestock = (DataTable)Session["AvailableStockPos"];
            if (availablestock != null)
            {
                AvailableStockgrid.DataSource = availablestock;

            }
        }

        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            Decimal available = Convert.ToDecimal(e.GetValue("Available"));
            if (available < 0)
                e.Row.ForeColor = Color.Red;
                
            else if (available > 0)
                e.Row.ForeColor = Color.Blue;
            else
                e.Row.ForeColor = Color.Gray;
        }
        //Rev work start 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
        protected void lnlDownloaderexcel_Click(object sender, EventArgs e)
        {

            string strFileName = "BreezeERP_Product_Import.xlsx";
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + Convert.ToString(ConfigurationManager.AppSettings["SaveFile"]) + strFileName);

            Response.ContentType = "application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=BreezeERP_Product_Import.xlsx");
            Response.TransmitFile(strPath);
            Response.End();
        }
        protected void BtnSaveexcel_Click1(object sender, EventArgs e)
        {
            string fName = string.Empty;
            Boolean HasLog = false;
            if (OFDBankSelect.FileContent.Length != 0)
            {
                path = String.Empty;
                path1 = String.Empty;
                FileName = String.Empty;
                s = String.Empty;
                time = String.Empty;
                cannotParse = String.Empty;
                string strmodule = "InsertTradeData";

                BusinessLogicLayer.TransctionDescription td = new BusinessLogicLayer.TransctionDescription();

                FilePath = Path.GetFullPath(OFDBankSelect.PostedFile.FileName);
                FileName = Path.GetFileName(FilePath);
                string fileExtension = Path.GetExtension(FileName);

                if (fileExtension.ToUpper() != ".XLS" && fileExtension.ToUpper() != ".XLSX")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Uploaded file format not supported by the system');</script>");
                    return;
                }

                if (fileExtension.Equals(".xlsx"))
                {
                    fName = FileName.Replace(".xlsx", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                }

                else if (fileExtension.Equals(".xls"))
                {
                    fName = FileName.Replace(".xls", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                }

                else if (fileExtension.Equals(".csv"))
                {
                    fName = FileName.Replace(".csv", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                }

                Session["FileName"] = fName;

                String UploadPath = Server.MapPath((Convert.ToString(ConfigurationManager.AppSettings["SaveCSV"]) + Session["FileName"].ToString()));
                OFDBankSelect.PostedFile.SaveAs(UploadPath);

                ClearArray();

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                try
                {
                    HttpPostedFile file = OFDBankSelect.PostedFile;
                    String extension = Path.GetExtension(FileName);
                    HasLog = Import_To_Grid(UploadPath, extension, file);
                }
                catch (Exception ex)
                {
                    HasLog = false;
                }

                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Import Process Successfully Completed!'); ShowLogData('" + HasLog + "');</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>jAlert('Selected File Cannot Be Blank');</script>");
            }
        }
        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }
        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
        public static int? GetColumnIndexFromName(string columnName)
        {
            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }
        public Boolean Import_To_Grid(string FilePath, string Extension, HttpPostedFile file)
        {
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            Boolean Success = false;
            Boolean HasLog = false;
            int loopcounter = 1;

            if (file.FileName.Trim() != "")
            {

                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();

                    using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(FilePath, false))
                    {

                        Sheet sheet = spreadSheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                        Worksheet worksheet = (spreadSheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;
                        IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                        foreach (Row row in rows)
                        {
                            if (row.RowIndex.Value == 1)
                            {
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    if (cell.CellValue != null)
                                    {
                                        dt.Columns.Add(GetValue(spreadSheetDocument, cell));
                                    }
                                }
                            }
                            else
                            {
                                DataRow tempRow = dt.NewRow();
                                int columnIndex = 0;
                                foreach (Cell cell in row.Descendants<Cell>())
                                {
                                    // Gets the column index of the cell with data
                                    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                    cellColumnIndex--; //zero based index
                                    if (columnIndex < cellColumnIndex)
                                    {
                                        do
                                        {
                                            tempRow[columnIndex] = ""; //Insert blank data here;
                                            columnIndex++;
                                        }
                                        while (columnIndex < cellColumnIndex);
                                    }
                                    try
                                    {
                                        tempRow[columnIndex] = GetValue(spreadSheetDocument, cell);
                                    }
                                    catch
                                    {
                                        tempRow[columnIndex] = "";
                                    }

                                    columnIndex++;
                                }
                                dt.Rows.Add(tempRow);
                            }
                        }
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string ProductCode = string.Empty;
                        //Boolean isInventory = false;
                        //Boolean IsServiceItem = false;
                        int isInventory = 0;
                        int IsServiceItem = 0;
                        string PRODUCTDELIVERYLOTUNIT = string.Empty;
                        string SPRODUCT_STOCKUOM = string.Empty;
                        string PRODUCTTRADINGLOTUNIT = string.Empty;
                        string PACKING_SALEUOM = string.Empty;

                        string brand = string.Empty;
                        string PRODUCTCLASSCODE = string.Empty;
                        string SINV_MAINACCOUNT = string.Empty;
                        string SRET_MAINACCOUNT = string.Empty;
                        string PINV_MAINACCOUNT = string.Empty;
                        string PRET_MAINACCOUNT = string.Empty;
                        string color = string.Empty;
                        string size = string.Empty;
                        string prodseries = string.Empty;
                        string prodsurface = string.Empty;
                        string prodcat = string.Empty;
                        string prodUOM = string.Empty;

                        int NumberSchemaId = 0;
                        string ISACTIVE = string.Empty;
                        int SCHEMA_TYPE = 0;
                        bool CheckUniqueCode = false;
                        Boolean FURTHERANCETOBUSINESS = false;
                        string TechValue = string.Empty;

                        foreach (DataRow row in dt.Rows)
                        {
                            loopcounter++;
                            try
                            {
                                /*Purchase UOM Checking start*/
                                DataTable dtPurUOMDt = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_Name='" + Convert.ToString(row["Purchase UOM*"]) + "'");
                                if (dtPurUOMDt.Rows.Count > 0)
                                {
                                    PRODUCTDELIVERYLOTUNIT = dtPurUOMDt.Rows[0]["UOM_ID"].ToString();
                                }
                               /*Purchase UOM Checking close*/

                                /*Stock UOM Checking start*/
                                DataTable dtStockUOMDt = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_Name='" + Convert.ToString(row["Stock UOM*"]) + "'");
                                if (dtStockUOMDt.Rows.Count > 0)
                                {
                                    SPRODUCT_STOCKUOM = dtStockUOMDt.Rows[0]["UOM_ID"].ToString();
                                }
                                /*Stock UOM Checking close*/

                                /*Sale UOM Checking start*/
                                DataTable dtSaleUOMDt = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_Name='" + Convert.ToString(row["Sale UOM*"]) + "'");
                                if (dtSaleUOMDt.Rows.Count > 0)
                                {
                                    PRODUCTTRADINGLOTUNIT = dtSaleUOMDt.Rows[0]["UOM_ID"].ToString();
                                }
                                /*Sale UOM Checking close*/

                                /*Packing UOM Checking start*/
                                DataTable dtPackUOMDt = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_Name='" + Convert.ToString(row["Alt. UOM"]) + "'");
                                if (dtPackUOMDt.Rows.Count > 0)
                                {
                                    PACKING_SALEUOM = dtPackUOMDt.Rows[0]["UOM_ID"].ToString();
                                }
                                /*Packing UOM Checking close*/

                                /*brand Checking start*/
                                DataTable dtbrand = oDBEngine.GetDataTable("select Brand_Id from tbl_master_brand where Brand_Name='" + Convert.ToString(row["Brand"]) + "'");
                                if (dtbrand.Rows.Count > 0)
                                {
                                    brand = dtbrand.Rows[0]["Brand_Id"].ToString();
                                }
                                /*brand UOM Checking close*/

                                /*product class Checking start*/
                                //DataTable dtProdClass = oDBEngine.GetDataTable("select ProductClass_ID from Master_ProductClass where ProductClass_Code=='" + Convert.ToString(row["Class"]) + "'");
                                DataTable dtProdClass = oDBEngine.GetDataTable("select ProductClass_ID from Master_ProductClass where ProductClass_Code='" + Convert.ToString(row["Group Code"]) + "'");
                                if (dtProdClass.Rows.Count > 0)
                                {
                                    PRODUCTCLASSCODE = dtProdClass.Rows[0]["ProductClass_ID"].ToString();
                                }
                                /*product class Checking close*/
                                /*Sale Ledger start*/
                                DataTable dtsaleledger = oDBEngine.GetDataTable("select MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType not in ('Cash','Bank') and "+
                               " MainAccount_AccountCode not in (select distinct SubAccount_MainAcReferenceID from Master_SubAccount where SubAccount_MainAcReferenceID is not null)" +
                               " and MainAccount_AccountCode not like 'SYSTM%' and MainAccount_Name='" + Convert.ToString(row["Sale Ledger"]) + "'");
                                if(dtsaleledger.Rows.Count>0)
                                {
                                    SINV_MAINACCOUNT = dtsaleledger.Rows[0]["MainAccount_AccountCode"].ToString();
                                }
                                /*Sale Ledger close*/
                                /*Sale Ret. Ledger start*/
                                DataTable dtsaleleRetdger = oDBEngine.GetDataTable("select MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType not in ('Cash','Bank') and "+
                               " MainAccount_AccountCode not in (select distinct SubAccount_MainAcReferenceID from Master_SubAccount where SubAccount_MainAcReferenceID is not null)" +
                               " and MainAccount_AccountCode not like 'SYSTM%' and MainAccount_Name='" + Convert.ToString(row["Sale Ret. Ledger"]) + "'");
                                if(dtsaleledger.Rows.Count>0)
                                {
                                    SRET_MAINACCOUNT = dtsaleleRetdger.Rows[0]["MainAccount_AccountCode"].ToString();
                                }
                                /*Sale Ret. Ledger close*/
                                DataTable dtPurdger = oDBEngine.GetDataTable("select MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType not in ('Cash','Bank') and " +
                                " MainAccount_AccountCode not in (select distinct SubAccount_MainAcReferenceID from Master_SubAccount where SubAccount_MainAcReferenceID is not null)" +
                                " and MainAccount_AccountCode not like 'SYSTM%' and MainAccount_Name='" + Convert.ToString(row["Pur. Ledger"]) + "'");
                                if (dtsaleledger.Rows.Count > 0)
                                {
                                    PINV_MAINACCOUNT = dtPurdger.Rows[0]["MainAccount_AccountCode"].ToString();
                                }

                                DataTable dtPurRetdger = oDBEngine.GetDataTable("select MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType not in ('Cash','Bank') and " +
                                " MainAccount_AccountCode not in (select distinct SubAccount_MainAcReferenceID from Master_SubAccount where SubAccount_MainAcReferenceID is not null)" +
                                " and MainAccount_AccountCode not like 'SYSTM%' and MainAccount_Name='" + Convert.ToString(row["Pur. Ret. Ledger"]) + "'");
                                if (dtPurRetdger.Rows.Count > 0)
                                {
                                    PRET_MAINACCOUNT = dtPurRetdger.Rows[0]["MainAccount_AccountCode"].ToString();
                                }
                                DataTable dtcolor = oDBEngine.GetDataTable("select Color_ID from Master_Color where Color_Code='" + Convert.ToString(row["Color Code"]) + "'");
                                if (dtcolor.Rows.Count > 0)
                                {
                                    color = dtcolor.Rows[0]["Color_ID"].ToString();
                                }
                                DataTable dtsize = oDBEngine.GetDataTable("select Size_ID from Master_Size where Size_Name='" + Convert.ToString(row["Size Name"]) + "'");
                                if (dtsize.Rows.Count > 0)
                                {
                                    size = dtsize.Rows[0]["Size_ID"].ToString();
                                }
                                DataTable dtprodseries = oDBEngine.GetDataTable("select ProductSeries_ID FROM Master_ProductSeries where ProductSeries_Name='" + Convert.ToString(row["Product Series"]) + "'");
                                if (dtprodseries.Rows.Count > 0)
                                {
                                    prodseries = dtprodseries.Rows[0]["ProductSeries_ID"].ToString();
                                }
                                DataTable dtprodsurface = oDBEngine.GetDataTable("select ProductSurface_ID FROM Master_ProductSurface where ProductSurface_Name='" + Convert.ToString(row["Surface"]) + "'");
                                if (dtprodseries.Rows.Count > 0)
                                {
                                    prodsurface = dtprodseries.Rows[0]["ProductSurface_ID"].ToString();
                                }
                                DataTable dtprodcat = oDBEngine.GetDataTable("select ProductSubcategory_ID FROM Master_ProductSubcategory where ProductSubcategory_Name='" + Convert.ToString(row["Sub-Category"]) + "'");
                                if (dtprodcat.Rows.Count > 0)
                                {
                                    prodcat = dtprodcat.Rows[0]["ProductSubcategory_ID"].ToString();
                                }                               
                                DataTable dtUOM = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM where UOM_Name='" + Convert.ToString(row["UOM"]) + "'");
                                if (dtUOM.Rows.Count > 0)
                                {
                                    prodUOM = dtUOM.Rows[0]["UOM_ID"].ToString();
                                }

                                CommonBL ComBL = new CommonBL();
                                string UniqueAutoNumberProductMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberProductMaster");
                                if (UniqueAutoNumberProductMaster == "Yes")
                                {
                                    DataTable NumberSchemeDT = oDBEngine.GetDataTable("SELECT ID,ISNULL(ISACTIVE,0)ISACTIVE,SCHEMA_TYPE FROM TBL_MASTER_IDSCHEMA WHERE TYPE_ID=132 AND  ISNULL(ISACTIVE,0)=1 AND SCHEMANAME='" + Convert.ToString(row["Numering Scheme"]) + "'");
                                    if (NumberSchemeDT.Rows.Count > 0)
                                    {

                                        ISACTIVE = NumberSchemeDT.Rows[0]["ISACTIVE"].ToString();
                                        SCHEMA_TYPE = Convert.ToInt32(NumberSchemeDT.Rows[0]["SCHEMA_TYPE"].ToString());
                                        if (SCHEMA_TYPE == 1)
                                        {
                                            NumberSchemaId = Convert.ToInt32(NumberSchemeDT.Rows[0]["ID"].ToString());
                                            ProductCode = "Auto";
                                        }
                                        else
                                        {                                           
                                            NumberSchemaId = Convert.ToInt32(NumberSchemeDT.Rows[0]["ID"].ToString());
                                            ProductCode = Convert.ToString(row["Product Code*"]);
                                        }
                                    }
                                }
                                else
                                {
                                    NumberSchemaId = 0;
                                    ProductCode = Convert.ToString(row["Product Code*"]);
                                }

                                //ProductCode = Convert.ToString(row["Product Code*"]);
                                string ProductName = Convert.ToString(row["Product Name*"]);
                                string AddlDesc = Convert.ToString(row["Addl Description"]);
                                string AltName = Convert.ToString(row["Alternate Name"]);
                                string Inventory = Convert.ToString(row["Inventory* "]);
                                string NonInvt = Convert.ToString(row["Non-Invt.*"]);
                                string Service = Convert.ToString(row["Service*"]);

                                string PurchaseUOM = PRODUCTDELIVERYLOTUNIT;// Convert.ToString(row["Purchase UOM*"]);
                                string StockUOM = SPRODUCT_STOCKUOM;// Convert.ToString(row["Stock UOM*"]);
                                string SaleUOM = PRODUCTTRADINGLOTUNIT;// Convert.ToString(row["Sale UOM*"]);
                                string SaleRate = Convert.ToString(row["Sale Rate"]);

                                string BuyRate = Convert.ToString(row["Buy Rate"]);
                                string ConvMainQty = Convert.ToString(row["Conv. Main Qty"]);
                                string AltQty = Convert.ToString(row["Alt. Qty"]);
                                string AltUOM = PACKING_SALEUOM; //Convert.ToString(row["Alt. UOM"]);
                                string Brand = brand;//Convert.ToString(row["Brand"]);
                                string Class = PRODUCTCLASSCODE;// Convert.ToString(row["Class"]);
                                string HSN = Convert.ToString(row["HSN/SAC"]);

                                string FurtherBuis = Convert.ToString(row["Furtherance to Business"]);
                                string ValTech = Convert.ToString(row["Val Tech*"]);
                                string MinLvl = Convert.ToString(row["Min Level"]);
                                string MaxLvl = Convert.ToString(row["Max Level"]);
                                string RecordLvl = Convert.ToString(row["Reorder Level"]);
                                string ReorderQty = Convert.ToString(row["Reorder Qty"]);
                                string SaleLedger = SINV_MAINACCOUNT;// Convert.ToString(row["Sale Ledger"]);

                                string SaleRetLedger = SRET_MAINACCOUNT;// Convert.ToString(row["Sale Ret. Ledger"]);
                                string PurLedger = PINV_MAINACCOUNT; //Convert.ToString(row["Pur. Ledger"]);
                                string PurRetLedger = PRET_MAINACCOUNT;// Convert.ToString(row["Pur. Ret. Ledger"]);

                                string ColorCode = color;
                                string SizeCode = size;
                                string ProductSeries = prodseries;
                                string Surface = prodsurface;
                                string LeadTime = Convert.ToString(row["Lead Time"]);
                                string Weight = Convert.ToString(row["Weight"]);
                                string SubCategory = prodcat;
                                string Length = Convert.ToString(row["Length"]);
                                string Width = Convert.ToString(row["Width"]);
                                string Thickness = Convert.ToString(row["Thickness"]);
                                string UOM = prodUOM;
                                string UserId = Convert.ToString(HttpContext.Current.Session["userid"]);
                                string Year = Convert.ToString(Session["LastFinYear"]);
                                

                                if(Inventory=="Yes")
                                {
                                    isInventory = 1;                                    
                                }
                                else if (Inventory == "")
                                {
                                    isInventory = 2;
                                }
                                else
                                {
                                    isInventory = 0;
                                }
                                if(Service=="Yes")
                                {
                                    IsServiceItem = 1;
                                }
                                else if(Service=="")
                                {
                                    IsServiceItem = 2;
                                }
                                else
                                {
                                    IsServiceItem = 0;
                                }
                                if(FurtherBuis=="Yes")
                                {
                                    FURTHERANCETOBUSINESS = true;
                                }
                                else
                                {
                                    FURTHERANCETOBUSINESS = false;
                                }
                                if(ValTech=="FIFO")
                                {
                                    TechValue = "F";
                                }
                                else if(ValTech=="LIFO")
                                {
                                    TechValue = "L";
                                }
                                else
                                {
                                    TechValue = "A";
                                }

                                DataSet dt2 = oStore_MasterBL.InsertProductDataFromExcel(ProductCode, ProductName, AddlDesc, AltName, isInventory, IsServiceItem, PurchaseUOM,
                                   StockUOM, SaleUOM, SaleRate, BuyRate, ConvMainQty, AltQty, AltUOM, Brand, Class, HSN, FURTHERANCETOBUSINESS, TechValue, MinLvl, MaxLvl, RecordLvl, ReorderQty, SaleLedger,
                               SaleRetLedger, PurLedger, PurRetLedger, ColorCode, SizeCode, ProductSeries, Surface, LeadTime, Weight, SubCategory, Length, Width, Thickness, UOM, UserId, Year, NumberSchemaId);

                                if (dt2 != null && dt2.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row2 in dt2.Tables[0].Rows)
                                    {
                                        Success = Convert.ToBoolean(row2["Success"]);
                                        HasLog = Convert.ToBoolean(row2["HasLog"]);
                                    }
                                }

                                if (!HasLog)
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    int loginsert = oStore_MasterBL.InsertProductImportLOg(ProductCode, loopcounter, ProductName, UserId, Session["FileName"].ToString(), description, "Failed");
                                }

                                else
                                {
                                    string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                    int loginsert = oStore_MasterBL.InsertProductImportLOg(ProductCode, loopcounter, ProductName, UserId, Session["FileName"].ToString(), description, "Success");
                                }

                            }
                            catch (Exception ex)
                            {
                                Success = false;
                                HasLog = false;
                                // string description = Convert.ToString(dt2.Tables[0].Rows[0]["MSG"]);
                                int loginsert = oStore_MasterBL.InsertProductImportLOg(ProductCode, loopcounter, "", "", Session["FileName"].ToString(), ex.Message.ToString(), "Failed");
                            }

                        }
                    }

                }
                else
                {

                }
            }
            return HasLog;
        }
        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            Store_MasterBL oStore_MasterBL = new Store_MasterBL();
            string fileName = Convert.ToString(Session["FileName"]);
            DataSet dt2 = oStore_MasterBL.GetproductLog(fileName);
            GvJvSearch.DataSource = dt2.Tables[0];
        }
        //Rev work close 10.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP
    }

    class MainAccount
    {
        public string MainAccount_AccountCode { get; set; }
        public string MainAccount_Name { get; set; }
    }

    public class ClassModel
    {
        public string id { get; set; }
        public string Name { get; set; }
    }

    public class HSNModel
    {
        public int HSNId { get; set; }
        public string id { get; set; }
        public string Name { get; set; }
    }



















    //using System;
    //using System.Web;
    //using DevExpress.Web;
    //using BusinessLogicLayer;
    //using System.Data;
    //using System.Web.UI;
    //////using DevExpress.Web.ASPxClasses;
    //using System.Web.Services;
    //using System.Text;

    //public partial class management_master_Store_sMarkets : System.Web.UI.Page
    //{
    //    public string pageAccess = "";
    //  //  DBEngine oDBEngine = new DBEngine(string.Empty);
    //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
    //    BusinessLogicLayer.GenericMethod oGenericMethod;
    //    protected void Page_PreInit(object sender, EventArgs e)
    //    {
    //        if (!IsPostBack)
    //        {
    //            //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
    //            string sPath = HttpContext.Current.Request.Url.ToString();
    //            oDBEngine.Call_CheckPageaccessebility(sPath);
    //        }
    //    }
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
    //    }
    //    protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
    //        switch (Filter)
    //        {
    //            case 1:
    //                exporter.WritePdfToResponse();
    //                break;
    //            case 2:
    //                exporter.WriteXlsToResponse();
    //                break;
    //            case 3:
    //                exporter.WriteRtfToResponse();
    //                break;
    //            case 4:
    //                exporter.WriteCsvToResponse();
    //                break;
    //        }
    //    }
    //    protected void marketsGrid_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
    //    {
    //        if (e.RowType == GridViewRowType.Data)
    //        {
    //            int commandColumnIndex = -1;
    //            for (int i = 0; i < marketsGrid.Columns.Count; i++)
    //                if (marketsGrid.Columns[i] is GridViewCommandColumn)
    //                {
    //                    commandColumnIndex = i;
    //                    break;
    //                }
    //            if (commandColumnIndex == -1)
    //                return;
    //            //____One colum has been hided so index of command column will be leass by 1 
    //            commandColumnIndex = commandColumnIndex - 5;
    //            DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
    //            for (int i = 0; i < cell.Controls.Count; i++)
    //            {
    //                DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
    //                if (button == null) return;
    //                DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

    //                if (hyperlink.Text == "Delete")
    //                {
    //                    if (Session["PageAccess"].ToString() == "DelAdd" || Session["PageAccess"].ToString() == "Delete" || Session["PageAccess"].ToString() == "All")
    //                    {
    //                        hyperlink.Enabled = true;
    //                        continue;
    //                    }
    //                    else
    //                    {
    //                        hyperlink.Enabled = false;
    //                        continue;
    //                    }
    //                }


    //            }

    //        }

    //    }
    //    protected void marketsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    //    {
    //        if (!marketsGrid.IsNewRowEditing)
    //        {
    //            ASPxGridViewTemplateReplacement RT = marketsGrid.FindEditFormTemplateControl("UpdateButton") as ASPxGridViewTemplateReplacement;
    //            if (Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "Modify" || Session["PageAccess"].ToString().Trim() == "All")
    //                RT.Visible = true;
    //            else
    //                RT.Visible = false;
    //        }

    //    }
    //    protected void marketsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    //    {
    //        if (e.Parameters == "s")
    //            marketsGrid.Settings.ShowFilterRow = true;

    //        if (e.Parameters == "All")
    //        {
    //            marketsGrid.FilterExpression = string.Empty;
    //        }
    //    }

    //     [WebMethod]

    //    public static string GetStateListBycountryId(string countryid)
    //    {
    //        StringBuilder strStates = new StringBuilder();
    //        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
    //        try
    //        {
    //            DataTable dtCmb = new DataTable();
    //            dtCmb = oGenericMethod.GetDataTable("select [State_ID],[State_Name] from Master_States ms  inner join tbl_master_country tmc on tmc.cou_id=ms.[State_CountryID]  where tmc.[cou_country]=LTRIM (RTRIM (" + countryid + ")) order by State_Name");
    //            int cnt = dtCmb.Rows.Count;
    //            if (dtCmb.Rows.Count>0)
    //            {
    //                int i = 0;
    //                strStates.Append("[");
    //                foreach (DataRow dr in dtCmb.Rows)
    //                {
    //                    if (i == cnt - 1)
    //                    {
    //                        strStates.Append("{");
    //                        strStates.Append("\"statename\":\"" + dr["State_Name"] + "\",");
    //                        strStates.Append("\"ID\":\"" + dr["State_ID"] + "\"");
    //                        strStates.Append("}");
    //                    }
    //                    else
    //                    {
    //                        strStates.Append("{");
    //                        strStates.Append("\"statename\":\"" + dr["State_Name"] + "\",");
    //                        strStates.Append("\"ID\":\"" + dr["State_ID"] + "\"");
    //                        strStates.Append("},");
    //                    }
    //                    i++;
    //                }
    //            }
    //            strStates.Append("]");
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //        finally
    //        {
    //        }

    //        return strStates.ToString();
    //    }
    //}
}