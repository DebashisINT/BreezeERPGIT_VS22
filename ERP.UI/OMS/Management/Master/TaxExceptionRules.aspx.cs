using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class TaxExceptionRules : System.Web.UI.Page
    {
        clsDropDownList OclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        TaxExceptionrulesBL obj = new TaxExceptionrulesBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString.AllKeys.Contains("Code"))
                {
                    txtHsnSacCode.Text = Request.QueryString["Code"];
                }
                if (Request.QueryString.AllKeys.Contains("Type"))
                {
                    hdnHSNSACType.Value = Request.QueryString["Type"];
                }

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


                if (Session["GrdTaxException"] == null)
                {
                    Session["GrdTaxException"] = CreateDatatable();
                }
                
                grid.DataBind();


            }
        }

        private DataTable CreateDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlrNo", typeof(string));
            dt.Columns.Add("Entitytype", typeof(string));
            dt.Columns.Add("BasedOn", typeof(string));
            dt.Columns.Add("Operator", typeof(string));
            dt.Columns.Add("Criteria", typeof(string));
            return dt;

        }

        protected void Grid_DataBinding(object sender, EventArgs e)
        {

            grid.DataSource = (DataTable)Session["GrdTaxException"];

            grid.DataSource = obj.GetDataforHSNSAC(txtHsnSacCode.Text, hdnHSNSACType.Value);

        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters;
            string Action = param.Split('~')[0];

            grid.JSProperties["cpOutput"] = null;



            if (Action == "Save")
            {
                string ActionType = hdnAction.Value;
                string EntityType = dllEntityType.Value;
                string BasedOn = dllBasedOn.Value;
                string operators = ddlOperator.Value;
                string criteria = txtVoucherAmount.Text;
                DateTime fromdate = dt_Fromdate.Date;
                DateTime todate = dt_ToDate.Date;
                string HSNSACcode = txtHsnSacCode.Text;
                string type = hdnHSNSACType.Value;
                string id = hdnID.Value;
                string output = obj.SaveException(ActionType,id, EntityType, BasedOn, operators, criteria,
                    fromdate, todate, HSNSACcode, type, cmbCGST.Value.ToString(), cmbSGST.Value.ToString(), cmbUTGST.Value, cmbIGST.Value,
                   cmbsaleCGST.Value, cmbSaleSGST.Value, cmbSaleUTGST.Value, cmbSaleIGST.Value);

                grid.DataBind();
                grid.JSProperties["cpOutput"] = output;
            }

            


        }

        [WebMethod]
        public static object EdtExceptionRule(string id)
        {
            EditDetails objEdit = new EditDetails();
            TaxExceptionrulesBL obj = new TaxExceptionrulesBL();
            DataTable dt = obj.GetEditData(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                objEdit.APPLICABLE_FROMDATE = Convert.ToDateTime(dt.Rows[0]["APPLICABLE_FROMDATE"]);
                objEdit.APPLICABLE_TODATE = Convert.ToDateTime(dt.Rows[0]["APPLICABLE_TODATE"]);
                objEdit.ENTITY_TYPE = Convert.ToString(dt.Rows[0]["ENTITY_TYPE"]);
                objEdit.BASED_ON = Convert.ToString(dt.Rows[0]["BASED_ON"]);
                objEdit.OPERATOR = Convert.ToString(dt.Rows[0]["OPERATOR"]);
                objEdit.CRITERIA = Convert.ToString(dt.Rows[0]["CRITERIA"]);
                objEdit.INPUT_CGST_TAXRATESID = Convert.ToString(dt.Rows[0]["INPUT_CGST_TAXRATESID"]);
                objEdit.INPUT_SGST_TAXRATESID = Convert.ToString(dt.Rows[0]["INPUT_SGST_TAXRATESID"]);
                objEdit.INPUT_UTGST_TAXRATESID = Convert.ToString(dt.Rows[0]["INPUT_UTGST_TAXRATESID"]);
                objEdit.INPUT_IGST_TAXRATESID = Convert.ToString(dt.Rows[0]["INPUT_IGST_TAXRATESID"]);
                objEdit.OUTPUT_CGST_TAXRATESID = Convert.ToString(dt.Rows[0]["OUTPUT_CGST_TAXRATESID"]);
                objEdit.OUTPUT_SGST_TAXRATESID = Convert.ToString(dt.Rows[0]["OUTPUT_SGST_TAXRATESID"]);
                objEdit.OUTPUT_UTGST_TAXRATESID = Convert.ToString(dt.Rows[0]["OUTPUT_UTGST_TAXRATESID"]);
                objEdit.OUTPUT_IGST_TAXRATESID = Convert.ToString(dt.Rows[0]["OUTPUT_IGST_TAXRATESID"]);



            }
            return objEdit;
        }

        [WebMethod]
        public static object DeleteExceptionRule(string id)
        {
            string output = "";
            TaxExceptionrulesBL obj = new TaxExceptionrulesBL();
            DataTable dt = obj.DeleteData(id);

            return output;
        }

    }


    public class EditDetails
    {
        public DateTime APPLICABLE_FROMDATE { get; set; }
        public DateTime APPLICABLE_TODATE { get; set; }
        public string ENTITY_TYPE { get; set; }
        public string BASED_ON { get; set; }
        public string OPERATOR { get; set; }
        public string CRITERIA { get; set; }
        public string INPUT_CGST_TAXRATESID { get; set; }
        public string INPUT_SGST_TAXRATESID { get; set; }
        public string INPUT_UTGST_TAXRATESID { get; set; }
        public string INPUT_IGST_TAXRATESID { get; set; }
        public string OUTPUT_CGST_TAXRATESID { get; set; }
        public string OUTPUT_SGST_TAXRATESID { get; set; }
        public string OUTPUT_UTGST_TAXRATESID { get; set; }
        public string OUTPUT_IGST_TAXRATESID { get; set; }
    }
}