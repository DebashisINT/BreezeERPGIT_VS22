using System;
using System.Web;
//using DevExpress.Web;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Collections.Generic;
using System.Data;
using BusinessLogicLayer;
using System.Collections;
using System.Web.UI;
using System.Web.Services;
using DataAccessLayer;
namespace ERP.OMS.Management.Master
{
    public partial class HSN : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.UdfGroupMasterBL udfBl = new BusinessLogicLayer.UdfGroupMasterBL();

        clsDropDownList OclsDropDownList = new clsDropDownList();

        string[] lengthIndex;
        string RemarksId;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
                //debjyoti
                Session["exportval"] = null;
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HSN.aspx");
            //SqlDataSource1.ConnectionString = ConfigurationManager.AppSettings["DBConnectionDefault"]; MULTI
           
            if (!IsPostBack)
            {

                string[,] list1 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('P','B') and TaxTypeCode='CGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list1, cmbCGST, false);

                string[,] list2 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('P','B') and TaxTypeCode='SGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list2, cmbSGST, false);

                string[,] list3 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('P','B') and TaxTypeCode='IGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list3, cmbIGST, false);
               

                string[,] list4 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('P','B') and TaxTypeCode='UTGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list4, cmbUTGST, false);


                string[,] list5 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('S','B') and TaxTypeCode='CGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list5, cmbsaleCGST, false);               

                string[,] list6 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('S','B') and TaxTypeCode='SGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list6, cmbSaleSGST, false);

                string[,] list7 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('S','B') and TaxTypeCode='IGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list7, cmbSaleIGST, false);

                string[,] list8 = oDBEngine.GetFieldValue("Config_TaxRates", "TaxRates_Id,TaxRatesSchemeName", "Taxrates_TaxCode in (select Taxes_Id from Master_Taxes where Taxes_Applicablefor in ('S','B') and TaxTypeCode='UTGST')", 2, "TaxRates_Id");
                OclsDropDownList.AddDataToDropDownListToAspx(list8, cmbSaleUTGST, false);

                string[,] list9 = oDBEngine.GetFieldValue("Master_HsnSacType", "HsnSacTypeId,HsnSacType", null, 2);
                OclsDropDownList.AddDataToDropDownListToAspx(list9, cmbSacType, false);

                cmbCGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbSGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbIGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbUTGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbsaleCGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbSaleSGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbSaleIGST.Items.Insert(0, new ListEditItem("--Select--", "0"));
                cmbSaleUTGST.Items.Insert(0, new ListEditItem("--Select--", "0"));

                cmbCGST.SelectedIndex = 0;
                cmbIGST.SelectedIndex = 0;
                cmbSGST.SelectedIndex = 0;
                cmbUTGST.SelectedIndex = 0;
                cmbsaleCGST.SelectedIndex = 0;
                cmbSaleIGST.SelectedIndex = 0;
                cmbSaleSGST.SelectedIndex = 0;
                cmbSaleUTGST.SelectedIndex = 0;        
            }

        }

        protected void gridudfGroup_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                int commandColumnIndex = -1;
                for (int i = 0; i < gridudfGroup.Columns.Count; i++)
                    if (gridudfGroup.Columns[i] is GridViewCommandColumn)
                    {
                        commandColumnIndex = i;
                        break;
                    }
                if (commandColumnIndex == -1)
                {
                    return;
                }
                //____One colum has been hided so index of command column will be leass by 1 
                commandColumnIndex = commandColumnIndex;
                DevExpress.Web.Rendering.GridViewTableCommandCell cell = e.Row.Cells[commandColumnIndex] as DevExpress.Web.Rendering.GridViewTableCommandCell;
                for (int i = 0; i < cell.Controls.Count; i++)
                {
                    DevExpress.Web.Rendering.GridCommandButtonsCell button = cell.Controls[i] as DevExpress.Web.Rendering.GridCommandButtonsCell;
                    if (button == null) return;
                    DevExpress.Web.Internal.InternalHyperLink hyperlink = button.Controls[0] as DevExpress.Web.Internal.InternalHyperLink;

                    if (hyperlink.Text == "Delete")
                    {
                        if (Session["PageAccess"] == "DelAdd" || Session["PageAccess"] == "Delete" || Session["PageAccess"] == "All")
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

        protected void gridudfGroup_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                gridudfGroup.JSProperties["cpHide"] = null;
                gridudfGroup.JSProperties["cpMsg"] = null;
                gridudfGroup.JSProperties["cpEditJson"] = null;
                gridudfGroup.JSProperties["cpBeforeTaxMappingJson"] = null;
                ProductComponentBL pbl = new ProductComponentBL();

                string[] lengthIndex;
                lengthIndex = e.Parameters.Split('~');

                if (lengthIndex[0].ToString() == "SAVE_NEW")
                {                   
                    int retData = pbl.InsertHSN(txtCode.Text.Trim(), txtDescription.Text.Trim(),"HSN");
                    if (retData==1)
                    {
                        gridudfGroup.JSProperties["cpHide"] = "Y";
                        gridudfGroup.JSProperties["cpMsg"] = "HSN saved successfully";
                    }
                    else if (retData == 999)
                    {
                        gridudfGroup.JSProperties["cpHide"] = "N";
                        gridudfGroup.JSProperties["cpMsg"] = "HSN Code already exists";
                    }
                }
                else if (lengthIndex[0].ToString() == "BEFORE_EDIT")
                {
                    string[,] Field_Value;
                    Field_Value = oDBEngine.GetFieldValue("tbl_HSN_Master", "Code,[Description] ", "Code='" + lengthIndex[1].ToString() + "'", 2);

                    gridudfGroup.JSProperties["cpEditJson"] = @"{""Code"":" + @"""" + Field_Value[0, 0].ToString() +
                                                           @""",""Description"":""" + Field_Value[0, 1].ToString() +                                                          
                                                           @"""}";
                }
                else if (lengthIndex[0].ToString() == "EDIT")
                {
                    int retData = pbl.UpdateHSN(txtCode.Text.Trim(), txtDescription.Text.Trim(), "UpdateHSN");
                   
                    if (retData == 1)
                    {
                        gridudfGroup.JSProperties["cpHide"] = "Y";
                        gridudfGroup.JSProperties["cpMsg"] = "HSN updated successfully";
                    }                  
                }
                else if (lengthIndex[0].ToString() == "Delete")
                {
                    string HSNCode = lengthIndex[1].ToString();                   
                    int retData = pbl.DeleteHSN(HSNCode, "DeleteHSN");
                    if (retData ==1)
                    {
                        gridudfGroup.JSProperties["cpMsg"] = "HSN deleted successfully.";
                    }
                    else if (retData == 999)
                    {
                        gridudfGroup.JSProperties["cpMsg"] = "HSN used in other modules. Cannot Delete.";
                    }

                }
                else if (lengthIndex[0].ToString() == "BeforeTaxMapping")
                {                  
                    int hsnPurCGST = 0; int hsnPurSGST = 0;  int hsnPurIGST = 0; int hsnPurUTGST = 0;
                    int hsnSaleCGST = 0; int hsnSaleSGST = 0; int hsnSaleIGST = 0; int hsnSaleUTGST = 0; int type = 0;
                    TaxSchemeBl tbl = new TaxSchemeBl();
                    DataTable dt = tbl.GetHSNTaxrates(lengthIndex[1].ToString());
                    DataTable dt1 = tbl.GetHSNSacType(lengthIndex[1].ToString(), "Hsn");
                    if (dt.Rows.Count > 0)
                    {
                        DataRow[] PurCGST = dt.Select("(Taxes_ApplicableFor = 'P' or Taxes_ApplicableFor='B')  AND TaxTypeCode='CGST'");
                        DataRow[] PurSGST = dt.Select("(Taxes_ApplicableFor = 'P' or Taxes_ApplicableFor='B')  AND TaxTypeCode='SGST'");
                        DataRow[] PurIGST = dt.Select("(Taxes_ApplicableFor = 'P' or Taxes_ApplicableFor='B')  AND TaxTypeCode='IGST'");                        
                        DataRow[] PurUTGST = dt.Select("(Taxes_ApplicableFor = 'P' or Taxes_ApplicableFor='B')  AND TaxTypeCode='UTGST'");

                        DataRow[] SalesCGST = dt.Select("(Taxes_ApplicableFor = 'S' or Taxes_ApplicableFor='B')  AND TaxTypeCode='CGST'");
                        DataRow[] SalesSGST = dt.Select("(Taxes_ApplicableFor = 'S' or Taxes_ApplicableFor='B')  AND TaxTypeCode='SGST'");
                        DataRow[] SalesIGST = dt.Select("(Taxes_ApplicableFor = 'S' or Taxes_ApplicableFor='B')  AND TaxTypeCode='IGST'");                        
                        DataRow[] SalesUTGST = dt.Select("(Taxes_ApplicableFor = 'S' or Taxes_ApplicableFor='B')  AND TaxTypeCode='UTGST'");

                        if (PurCGST.Length > 0)
                            hsnPurCGST = Convert.ToInt32(PurCGST[0]["TaxRates_ID"]);
                        if (PurSGST.Length > 0)
                            hsnPurSGST = Convert.ToInt32(PurSGST[0]["TaxRates_ID"]);
                        if (PurIGST.Length > 0)
                            hsnPurIGST = Convert.ToInt32(PurIGST[0]["TaxRates_ID"]);                        
                        if (PurUTGST.Length > 0)
                            hsnPurUTGST = Convert.ToInt32(PurUTGST[0]["TaxRates_ID"]);

                        if (SalesCGST.Length > 0)
                            hsnSaleCGST = Convert.ToInt32(SalesCGST[0]["TaxRates_ID"]);
                        if (SalesSGST.Length > 0)
                            hsnSaleSGST = Convert.ToInt32(SalesSGST[0]["TaxRates_ID"]);
                        if (SalesIGST.Length > 0)
                            hsnSaleIGST = Convert.ToInt32(SalesIGST[0]["TaxRates_ID"]);                        
                        if (SalesUTGST.Length > 0)
                            hsnSaleUTGST = Convert.ToInt32(SalesUTGST[0]["TaxRates_ID"]);

                    }

                    if (dt1.Rows.Count>0)
                    {
                        if(Convert.ToBoolean(dt1.Rows[0]["Exempted"].ToString())==true)
                        {
                            type = 2;
                        }
                        else if(Convert.ToBoolean(dt1.Rows[0]["NilRated"].ToString())==true)
                        {
                            type = 3;
                        }

                        else if (Convert.ToBoolean(dt1.Rows[0]["NonGst"].ToString())==true)
                        {
                            type = 4;
                        }
                        else
                        {
                            type = 1;
                        }
                    }


                    gridudfGroup.JSProperties["cpBeforeTaxMappingJson"] = @"{""HsnCode"":" + @"""" + lengthIndex[1].ToString() +
                        @""",""hsnPurCGST"":""" + hsnPurCGST.ToString() +                       
                        @""",""hsnPurSGST"":""" + hsnPurSGST +
                         @""",""hsnPurIGST"":""" + hsnPurIGST +
                        @""",""hsnPurUTGST"":""" + hsnPurUTGST +
                        @""",""hsnSaleCGST"":""" + hsnSaleCGST +                     
                        @""",""hsnSaleSGST"":""" + hsnSaleSGST +
                        @""",""hsnSaleIGST"":""" + hsnSaleIGST +
                        @""",""hsnSaleUTGST"":""" + hsnSaleUTGST +
                        @""",""hsnType"":""" + type +
                        @"""}";

                    //gridudfGroup.JSProperties["cpBeforeTaxMappingJson"] = @"{""HsnCode"":" + @"""" + lengthIndex[1].ToString() +
                    //                                                    @"""}";
                }
                else if (lengthIndex[0].ToString() == "SaveTaxMap")
                {
                    string hsncode = txtHsnCode1.Text;
                    int PurchaseCGST_TaxRatesId =Convert.ToInt32(cmbCGST.SelectedItem.Value);
                    int PurchaseIGST_TaxRatesId = Convert.ToInt32(cmbIGST .SelectedItem.Value);
                    int PurchaseSGST_TaxRatesId = Convert.ToInt32(cmbSGST.SelectedItem.Value);
                    int PurchaseUTGST_TaxRatesId = Convert.ToInt32(cmbUTGST.SelectedItem.Value);
                    int SaleCGST_TaxRatesId = Convert.ToInt32(cmbsaleCGST.SelectedItem.Value);
                    int SaleIGST_TaxRatesId = Convert.ToInt32(cmbSaleIGST.SelectedItem.Value);
                    int SaleSGST_TaxRatesId = Convert.ToInt32(cmbSaleSGST.SelectedItem.Value);
                    int SaleUTGST_TaxRatesId = Convert.ToInt32(cmbSaleUTGST.SelectedItem.Value);
                    int SacType = Convert.ToInt32(cmbSacType.SelectedItem.Value);

                    TaxSchemeBl tbl=new TaxSchemeBl();
                    bool result=tbl.SetHsnTaxRate(hsncode,PurchaseCGST_TaxRatesId,PurchaseIGST_TaxRatesId,PurchaseSGST_TaxRatesId,PurchaseUTGST_TaxRatesId,
                        SaleCGST_TaxRatesId, SaleIGST_TaxRatesId, SaleSGST_TaxRatesId, SaleUTGST_TaxRatesId, SacType,"Hsn");
                    
                    //int retData = pbl.InsertHSN(txtCode.Text.Trim(), txtDescription.Text.Trim(), "HSN");
                    //if (retData == 1)
                    //{
                        TaxSchemeBl oTaxSchemeBl = new TaxSchemeBl();
                        oTaxSchemeBl.GetSchemeMaxupdateDate();

                        gridudfGroup.JSProperties["cpHide"] = "Y";
                        gridudfGroup.JSProperties["cpMsg"] = "HSN Tax rate saved successfully";
                    //}
                    //else if (retData == 999)
                    //{
                    //    gridudfGroup.JSProperties["cpHide"] = "N";
                    //    gridudfGroup.JSProperties["cpMsg"] = "HSN Code already exists";
                    //}
                }





                gridudfGroup.DataBind();
                gridudfGroup.Settings.ShowFilterRow = true;

            }
            catch (Exception ex)
            {

            }
        }
        protected void gridudfGroup_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "All";
        }

        private void DataBinderSegmentSpecific()
        {

            SqlDataSource1.SelectCommand = "select HSN_Id, Code, [Description] from tbl_HSN_Master";

            gridudfGroup.DataBind();


        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //if (Filter != 0)
            //{
            //    if (Session["exportval"] == null)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //    else if (Convert.ToInt32(Session["exportval"]) != Filter)
            //    {
            //        Session["exportval"] = Filter;
            //        bindexport(Filter);
            //    }
            //}





        }

        public void bindexport(int Filter)
        {
            gridudfGroup.Columns[4].Visible = false;


            //exporter.FileName = "UDF Group Master";
            //exporter.ReportHeader = "UDF Group Master";
            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";

            //switch (Filter)
            //{
            //    case 1:
            //        exporter.WritePdfToResponse();
            //        break;
            //    case 2:
            //        exporter.WriteXlsToResponse();
            //        break;
            //    case 3:
            //        exporter.WriteRtfToResponse();
            //        break;
            //    case 4:
            //        exporter.WriteCsvToResponse();
            //        break;
            //}
        }

        [WebMethod]
        public static string ChecktaxRateForHSN(string PurchaseCGST, string PurchaseSGST, string PurchaseIGST, string SaleCGST, string SaleSGST, string SaleIGST)
        {
            DataTable dt = new DataTable();
            string Result = "";

            ProcedureExecute proc = new ProcedureExecute("Prc_HSNTaxRateCheck");
            proc.AddIntegerPara("@InputCGST", Convert.ToInt32(PurchaseCGST));
            proc.AddIntegerPara("@InputSGST", Convert.ToInt32(PurchaseSGST));
            proc.AddIntegerPara("@InputIGST", Convert.ToInt32(PurchaseIGST));
            proc.AddIntegerPara("@OutputCGST", Convert.ToInt32(SaleCGST));
            proc.AddIntegerPara("@OutputSGST", Convert.ToInt32(SaleSGST));
            proc.AddIntegerPara("@OutputIGST", Convert.ToInt32(SaleIGST));
            dt = proc.GetTable();

            if(dt !=null && dt.Rows.Count>0)
            {
                Result = Convert.ToString(dt.Rows[0]["CheckValue"]);
            }
            return Result;

        }


    }
}