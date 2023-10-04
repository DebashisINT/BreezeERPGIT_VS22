//==================================================== Revision History ==================================================================================
// 1.0      Priti       V2.0.39     13-07-2023      0026528:Project Purchase Order Terms & Condition are becoming Blank if Something is Pasted in the Payment Terms Field
// 2.0      Sanchita    V2.0.40     04-10-2023      Few Fields required in the Quotation Entry Module for the Purpose of Quotation Print from ERP
//                                                  New button "Other Condiion" to show instead of "Terms & Condition" Button 
//                                                  if the settings "Show Other Condition" is set as "Yes". Mantis: 0026868
//====================================================End Revision History================================================================================

using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class TermsConditionsControl : System.Web.UI.UserControl
    {
        PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();
        //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Init(object sender, EventArgs e)
        {
            dsBankName.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                try
                {
                    #region Show T&C

                    txt_BEValue.Attributes.Add("onkeypress", "return isNumeric(event)");
                    string Variable_Name = string.Empty;
                    // Rev 2.0
                    string Called_From = string.Empty;
                    // End of Rev 2.0

                    if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "")
                    {
                        string Type = Convert.ToString(Request.QueryString["type"]);
                        Variable_Name = "Show_TC_" + Type;
                        // Rev 2.0
                        Called_From = Type;
                        // End of Rev 2.0
                    }
                    else
                    {
                        try
                        {
                            HiddenField ctl = (HiddenField)this.Parent.FindControl("hfTermsConditionDocType");
                            string DocType = ctl.Value;
                            Variable_Name = "Show_TC_" + DocType;
                            // Rev 2.0
                            Called_From = DocType;
                            // End of Rev 2.0
                        }
                        catch (Exception ex) { Variable_Name = "Show_TC_SO"; }
                    }

                    // Rev 2.0
                    DataTable DTOth = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Other_Condition' AND IsActive=1");

                    if ((Called_From == "SINQ" || Called_From == "QO" || Called_From == "SO" || Called_From == "SC" 
                    || Called_From == "SI") 
                        && DTOth != null && DTOth.Rows.Count > 0 && Convert.ToString(DTOth.Rows[0]["Variable_Value"]).Trim()=="Yes" )
                    {
                        this.Visible = false;
                    }
                    else
                    {
                        // End of Rev 2.0
                        DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='" + Variable_Name + "' AND IsActive=1");

                        if (DT != null && DT.Rows.Count > 0)
                        {
                            string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                            if (IsVisible == "Yes")
                            {
                                this.Visible = true;
                            }
                            else
                            {
                                this.Visible = false;
                            }
                        }
                        // Rev 2.0
                    }
                    // End of Rev 2.0
                    #endregion
                    #region Show & Hide purchase module specefic field
                    HiddenField hidden_DocType = (HiddenField)this.Parent.FindControl("hfTermsConditionDocType");
                    string DType = hidden_DocType.Value;
                    if (DType == "PO" || DType == "PC" || DType == "PB")
                    {
                        pnlpurchasemodulefields.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    {
                        pnlpurchasemodulefields.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    #endregion
                    #region Bind Transporter DropDown
                    DataTable DT_TR = GetTransporterBYLegalStatus(55);
                    if (DT_TR.Rows.Count > 0)
                    {
                        cmbtrnsprtrname.Items.Clear();
                        cmbtrnsprtrname.DataSource = DT_TR;
                        cmbtrnsprtrname.DataBind();
                        cmbtrnsprtrname.Items.Insert(0, new ListEditItem("--Select", 0));
                        cmbtrnsprtrname.SelectedIndex = 0;
                    }

                    #endregion
                    #region Bind Country DropDown
                    DataSet dstDT = GetCountry();
                    if (dstDT.Tables[0] != null && dstDT.Tables[0].Rows.Count > 0)
                    {
                        ddlCountryOfOrigin.TextField = "cou_country";
                        ddlCountryOfOrigin.ValueField = "cou_id";
                        ddlCountryOfOrigin.DataSource = dstDT.Tables[0];
                        ddlCountryOfOrigin.DataBind();
                    }
                    #endregion

                    // Code Added By Sam to Bind Port Detail on 05012018 Section Start
                    DataTable portdt = PopulatePortDetail();
                    if (portdt.Rows.Count > 0)
                    { 
                        ddl_PortOfShippment.Items.Clear();
                        ddl_PortOfShippment.TextField = "Port_Description";
                        ddl_PortOfShippment.ValueField = "Port_Code";
                        ddl_PortOfShippment.DataSource = portdt;
                        ddl_PortOfShippment.DataBind();

                        ddl_PortOfShippment.Items.Insert(0, new ListEditItem("--Select", 0));
                        ddl_PortOfShippment.SelectedIndex = 0;
                    }

                    
                    // Code Added By Sam to Bind Port Detail on 05012018 Section End
                    #region Bind controll
                    if (Request.QueryString["key"] != null && Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        string docid = Convert.ToString(Request.QueryString["key"]);

                        bool flag_Challan = false;
                        bool flag_Invoice = false;
                        bool flag_Approval = false;
                        bool Isvisible = false;

                        if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "")
                        {
                            string Type = Convert.ToString(Request.QueryString["type"]);
                            BindTC(docid, Type); //bind existing data
                            SHowHidePanelVendorWise(Convert.ToInt32(docid), Type);
                            switch (Type)
                            {
                                case "SO":
                                    flag_Challan = IsSalesOrderExistsInChallan(docid, "SO") == 1 ? false : true;
                                    flag_Invoice = IsSalesOrderExistsInInvoice(docid, "SO") == 1 ? false : true;
                                    Isvisible = (flag_Challan == false || flag_Invoice == false) ? false : true;
                                    DisableControls(Isvisible);
                                    break;

                                case "PO":
                                    flag_Challan = IsPurchaseOrderExistsInChallan(docid, "PO") == 1 ? false : true;
                                    flag_Invoice = IsPurchaseOrderExistsInInvoice(docid, "PO") == 1 ? false : true;
                                    Isvisible = (flag_Challan == false || flag_Invoice == false) ? false : true;
                                    DisableControls(Isvisible);
                                    break;

                                case "PB":
                                    //flag_Challan = IsPurchaseOrderExistsInChallan(docid, "PB") == 1 ? false : true;
                                    //flag_Invoice = IsPurchaseOrderExistsInInvoice(docid, "PB") == 1 ? false : true;
                                    //Isvisible = (flag_Challan == false || flag_Invoice == false) ? false : true;
                                    //DisableControls(Isvisible);
                                    break;

                                case "SI":
                                    //flag_Challan = IsSalesOrderExistsInChallan(docid, "SI") == 1 ? false : true;
                                    //flag_Invoice = IsSalesOrderExistsInInvoice(docid, "SI") == 1 ? false : true;
                                    //Isvisible = (flag_Challan == false || flag_Invoice == false) ? false : true;
                                    //DisableControls(Isvisible);
                                    break;

                                case "SC":
                                    //flag_Challan = IsSalesOrderExistsInChallan(docid, "SC") == 1 ? false : true;
                                    //flag_Invoice = IsSalesOrderExistsInInvoice(docid, "SC") == 1 ? false : true;
                                    //Isvisible = (flag_Challan == false || flag_Invoice == false) ? false : true;
                                    //DisableControls(Isvisible);
                                    break;

                                case "PC":
                                    //flag_Challan = IsPurchaseOrderExistsInChallan(docid, "PC") == 1 ? false : true;
                                    //flag_Invoice = IsPurchaseOrderExistsInInvoice(docid, "PC") == 1 ? false : true;
                                    //Isvisible = (flag_Challan == false || flag_Invoice == false) ? false : true;
                                    //DisableControls(Isvisible);
                                    break;
                            }
                        }
                        else
                        {
                            DisableControls(true);
                        }
                    }
                    #endregion
                }
                catch (Exception ex) { }
            }
        }

        private DataTable PopulatePortDetail()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
            proc.AddVarcharPara("@Action", 500, "PopulatePortDetail"); 
            dt = proc.GetTable(); 
            return dt;
        }
        private void DisableControls(bool Isvisible)
        {
            btnTCsave.Visible = Isvisible;

            dtDeliveryDate.ClientEnabled = Isvisible;
            txtDelremarks.Enabled = Isvisible;
            cmbInsuranceType.ClientEnabled = Isvisible;
            cmbFreightCharges.ClientEnabled = Isvisible;
            txtFreightRemarks.Enabled = Isvisible;
            txtPermitValue.ClientEnabled = Isvisible;
            txtRemarks.Enabled = Isvisible;
            cmbDelDetails.ClientEnabled = Isvisible;
            txtotherlocation.Enabled = Isvisible;
            cmbCertReq.ClientEnabled = Isvisible;
            cmbtrnsprtrname.ClientEnabled = Isvisible;
            cmbDiscntrcv.ClientEnabled = Isvisible;
            txtDiscntrcv.Enabled = Isvisible;
            cmbCommissionRcv.ClientEnabled = Isvisible;
            txtCommissionRcv.Enabled = Isvisible;
        }
        public void BindTC(string docid, string Type)
        {
            try
            {

                DataTable DT = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_TermsAndCondition");
                proc.AddVarcharPara("@Action", 500, "GetTCTagDetail");
                proc.AddVarcharPara("@DocId", 500, docid);
                proc.AddVarcharPara("@DocType", 500, Type);
                DT = proc.GetTable();




                // DataTable DT = objEngine.GetDataTable("tbl_trans_TermsAndConditions", " * ", " docid='" + prc_TermsAndCondition + "' and doctype = '" + Type + "'");
                if (DT != null && DT.Rows.Count > 0)
                {
                    if (DT.Rows[0]["DeliveryDate"] != null && DT.Rows[0]["DeliveryDate"].ToString() != "")
                    {
                        dtDeliveryDate.Value = Convert.ToDateTime(DT.Rows[0]["DeliveryDate"]);
                    }

                    txtDelremarks.Text = Convert.ToString(DT.Rows[0]["Delremarks"]);

                    if (DT.Rows[0]["insuranceType"] != null && DT.Rows[0]["insuranceType"].ToString() != "")
                    {
                        cmbInsuranceType.SelectedIndex = Convert.ToInt32(DT.Rows[0]["insuranceType"].ToString());
                    }

                    if (DT.Rows[0]["FreightCharges"] != null && DT.Rows[0]["FreightCharges"].ToString() != "")
                    {
                        cmbFreightCharges.SelectedIndex = Convert.ToInt32(DT.Rows[0]["FreightCharges"].ToString());
                    }

                    txtFreightRemarks.Text = Convert.ToString(DT.Rows[0]["FreightRemarks"]);

                    txtPermitValue.Text = Convert.ToString(DT.Rows[0]["PermitValue"]);

                    txtRemarks.Text = Convert.ToString(DT.Rows[0]["Remarks"]);

                    if (DT.Rows[0]["DelDetails"] != null && DT.Rows[0]["DelDetails"].ToString() != "")
                    {
                        cmbDelDetails.SelectedIndex = Convert.ToInt32(DT.Rows[0]["DelDetails"].ToString());
                    }

                    txtotherlocation.Text = Convert.ToString(DT.Rows[0]["otherlocation"]);

                    if (DT.Rows[0]["CertReq"] != null && DT.Rows[0]["CertReq"].ToString() != "")
                    {
                        cmbCertReq.SelectedIndex = Convert.ToInt32(DT.Rows[0]["CertReq"].ToString());
                    }

                    if (DT.Rows[0]["trnsprtrname"] != null && DT.Rows[0]["trnsprtrname"].ToString() != "")
                    {
                        cmbtrnsprtrname.Value = DT.Rows[0]["trnsprtrname"].ToString();
                    }

                    if (DT.Rows[0]["DelDetails"] != null && DT.Rows[0]["DelDetails"].ToString() != "" && Convert.ToInt32(DT.Rows[0]["DelDetails"]) == 3)
                    {
                        pnlotherlocation.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    {
                        pnlotherlocation.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }

                    if (DT.Rows[0]["trnsprtrname"] != null && DT.Rows[0]["trnsprtrname"].ToString() != "" && DT.Rows[0]["trnsprtrname"].ToString() != "0")
                    {
                        pnlTransporter.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    {
                        pnlTransporter.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }

                    if (DT.Rows[0]["Discntrcv"] != null && DT.Rows[0]["Discntrcv"].ToString() != "")
                    {
                        cmbDiscntrcv.SelectedIndex = Convert.ToInt32(DT.Rows[0]["Discntrcv"].ToString());
                    }
                    if (DT.Rows[0]["Discntrcv"] != null && DT.Rows[0]["Discntrcv"].ToString() != "" && Convert.ToInt32(DT.Rows[0]["Discntrcv"]) ==1)
                    {
                        pnlDiscntrcv.Style.Add(HtmlTextWriterStyle.Display, "block");
                        txtDiscntrcv.Text = Convert.ToString(DT.Rows[0]["Discntrcvdtls"]);
                    }
                    else
                    {
                        pnlDiscntrcv.Style.Add(HtmlTextWriterStyle.Display, "none");
                        txtDiscntrcv.Text = Convert.ToString(DT.Rows[0]["Discntrcvdtls"]);
                    }

                    if (DT.Rows[0]["CommissionRcv"] != null && DT.Rows[0]["CommissionRcv"].ToString() != "")
                    {
                        cmbCommissionRcv.SelectedIndex = Convert.ToInt32(DT.Rows[0]["CommissionRcv"].ToString());
                    }
                    if (DT.Rows[0]["CommissionRcv"] != null && DT.Rows[0]["CommissionRcv"].ToString() != "" && Convert.ToInt32(DT.Rows[0]["CommissionRcv"]) == 1)
                    {
                        pnlCommissionRcv.Style.Add(HtmlTextWriterStyle.Display, "block");
                        txtCommissionRcv.Text = Convert.ToString(DT.Rows[0]["CommissionRcvdtls"]);
                        txtCommissionRate.Text = Convert.ToString(DT.Rows[0]["CommissionRate"]);
                    }
                    else
                    {
                        pnlCommissionRcv.Style.Add(HtmlTextWriterStyle.Display, "none");
                        txtCommissionRcv.Text = "";
                        txtCommissionRate.Text = "";
                    }

                    //New PO fields
                    if (DT.Rows[0]["TypeOfImport"] != null && DT.Rows[0]["TypeOfImport"].ToString() != "")
                    {
                        ddlTypeOfImport.Value = DT.Rows[0]["TypeOfImport"].ToString();
                    }
                    txtPaymentTrmRemarks.Text = Convert.ToString(DT.Rows[0]["PaymentTrmRemarks"]);
                    txtPaymentTerms.Text = Convert.ToString(DT.Rows[0]["TC_PaymentTerms"]);
                    if (DT.Rows[0]["IncoDVTerms"] != null && DT.Rows[0]["IncoDVTerms"].ToString() != "")
                    {
                        ddlIncoDVTerms.Value = DT.Rows[0]["IncoDVTerms"].ToString();
                    }
                    txtIncoDVTermsRemarks.Text = Convert.ToString(DT.Rows[0]["IncoDVTermsRemarks"]);
                    txtShippmentSchedule.Text = Convert.ToString(DT.Rows[0]["ShippmentSchedule"]);

                   
                    // Code Added by Sam on 05012018 Section Start
                    //txtPortOfShippment.Text = Convert.ToString(DT.Rows[0]["PortOfShippment"]);
                      ddl_PortOfShippment.Value = Convert.ToString(DT.Rows[0]["PortOfShippment"]);
                      txt_BENumber.Text = Convert.ToString(DT.Rows[0]["BE_Number"]);
                      if (DT.Rows[0]["BE_Date"] != null && DT.Rows[0]["BE_Date"].ToString() != "")
                      {

                          dt_BEDate.Value = Convert.ToDateTime(DT.Rows[0]["BE_Date"]);
                      }
                      txt_BEValue.Text = Convert.ToString(DT.Rows[0]["BE_Value"]);
                    // Code Added by Sam on 05012018 Section End

                    txtPortOfDestination.Text = Convert.ToString(DT.Rows[0]["PortOfDestination"]);
                    if (DT.Rows[0]["PartialShippment"] != null && DT.Rows[0]["PartialShippment"].ToString() != "")
                    {
                        ddlPartialShippment.Value = DT.Rows[0]["PartialShippment"].ToString();
                    }
                    if (DT.Rows[0]["Transshipment"] != null && DT.Rows[0]["Transshipment"].ToString() != "")
                    {
                        ddlTransshipment.Value = DT.Rows[0]["Transshipment"].ToString();
                    }
                    txtPackingSpec.Text = Convert.ToString(DT.Rows[0]["PackingSpec"]);
                    if (DT.Rows[0]["ValidityOfOrderDate"] != null && DT.Rows[0]["ValidityOfOrderDate"].ToString() != "")
                    {
                        dtValidityOfOrder.Value = Convert.ToDateTime(DT.Rows[0]["ValidityOfOrderDate"]);
                    }
                    txtValidityOfOrderRemarks.Text = Convert.ToString(DT.Rows[0]["ValidityOfOrderRemarks"]);
                    if (DT.Rows[0]["CountryOfOrigin"] != null && DT.Rows[0]["CountryOfOrigin"].ToString() != "")
                    {
                        ddlCountryOfOrigin.Value = DT.Rows[0]["CountryOfOrigin"].ToString();
                    }
                    txtFreeDetentionPeriod.Text = Convert.ToString(DT.Rows[0]["FreeDetentionPeriod"]);
                    txtFreeDetentionPeriodRemark.Text = Convert.ToString(DT.Rows[0]["FreeDetentionPeriodRemark"]);

                    //Added for #16920
                    if (!string.IsNullOrEmpty(hdnTCBranchId.Value))
                    {

                        if (DT.Rows[0]["Bank_Branch"] != null && DT.Rows[0]["Bank_Branch"].ToString() != "")
                        {
                            txtBankBranchName.Text = Convert.ToString(DT.Rows[0]["Bank_Branch"]);
                        }
                        if (DT.Rows[0]["Bank_Address"] != null && DT.Rows[0]["Bank_Address"].ToString() != "")
                        {
                            txtBankBranchAddress.Text = Convert.ToString(DT.Rows[0]["Bank_Address"]);
                        }
                        if (DT.Rows[0]["Bank_landmark"] != null && DT.Rows[0]["Bank_landmark"].ToString() != "")
                        {
                            txtBankBranchLandmark.Text = Convert.ToString(DT.Rows[0]["Bank_landmark"]);
                        }
                        if (DT.Rows[0]["Bank_Pin"] != null && DT.Rows[0]["Bank_Pin"].ToString() != "")
                        {
                            txtBankBranchPin.Text = Convert.ToString(DT.Rows[0]["Bank_Pin"]);
                        }
                        if (DT.Rows[0]["Bank_SwiftCode"] != null && DT.Rows[0]["Bank_SwiftCode"].ToString() != "")
                        {
                            txtSwiftCode.Text = Convert.ToString(DT.Rows[0]["Bank_SwiftCode"]);
                        }

                        if (DT.Rows[0]["Bank_RTGSCode"] != null && DT.Rows[0]["Bank_RTGSCode"].ToString() != "")
                        {
                            txtRTGS.Text = Convert.ToString(DT.Rows[0]["Bank_RTGSCode"]);
                        }
                        if (DT.Rows[0]["Bank_IFSCCode"] != null && DT.Rows[0]["Bank_IFSCCode"].ToString() != "")
                        {
                            txtIFSC.Text = Convert.ToString(DT.Rows[0]["Bank_IFSCCode"]);
                        }
                        if (DT.Rows[0]["Bank_AcNo"] != null && DT.Rows[0]["Bank_AcNo"].ToString() != "")
                        {
                            txtAccountNumber.Text = Convert.ToString(DT.Rows[0]["Bank_AcNo"]);
                        }
                        if (DT.Rows[0]["Bank_Remarks"] != null && DT.Rows[0]["Bank_Remarks"].ToString() != "")
                        {
                            txtBankRemarks.Text = Convert.ToString(DT.Rows[0]["Bank_Remarks"]);
                        }
                        if (!string.IsNullOrEmpty(hdnTCBranchId.Value))
                        {

                            dsBankName.SelectParameters["BranchId"].DefaultValue = Convert.ToString(hdnTCBranchId.Value);
                            ddlBankName.DataBind();
                        }
                        if (DT.Rows[0]["Bank_Id"] != null && DT.Rows[0]["Bank_Id"].ToString() != "")
                        {
                            ddlBankName.Value = Convert.ToString(DT.Rows[0]["Bank_Id"]);
                        }                        
                    }
                    //Rev work start 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                    if (DT.Rows[0]["Project"] != null && DT.Rows[0]["Project"].ToString() != "")
                    {
                        txtProject.Text = Convert.ToString(DT.Rows[0]["Project"]);
                    }
                    //Rev work close 09.08.2022 mantise no:0025110: A Text Input field required in the Terms & Condition of the entry Module as "Project"
                }
                else
                {
                    dtDeliveryDate.Value = "";
                    txtDelremarks.Text = "";
                    cmbInsuranceType.SelectedIndex = -1;
                    cmbFreightCharges.SelectedIndex = -1;
                    txtFreightRemarks.Text = "";
                    txtPermitValue.Text = "";
                    txtRemarks.Text = "";
                    cmbDelDetails.SelectedIndex = -1;
                    txtotherlocation.Text = "";
                    cmbCertReq.SelectedIndex = -1;
                    cmbtrnsprtrname.SelectedIndex = -1;
                    cmbDiscntrcv.SelectedIndex = -1;
                    txtDiscntrcv.Text = "";
                    cmbCommissionRcv.SelectedIndex = -1;
                    txtCommissionRcv.Text = "";
                    pnlotherlocation.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pnlTransporter.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pnlDiscntrcv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pnlCommissionRcv.Style.Add(HtmlTextWriterStyle.Display, "none");

                    //New PO fields
                    ddlTypeOfImport.SelectedIndex = -1;
                    txtPaymentTrmRemarks.Text = "";
                    txtPaymentTerms.Text = "";
                    ddlIncoDVTerms.SelectedIndex = -1;
                    txtIncoDVTermsRemarks.Text = "";
                    txtShippmentSchedule.Text = "";

                    // Code Added by Sam on 05012018 Section Start
                    //txtPortOfShippment.Text = "";
                    ddl_PortOfShippment.Text = "";
                    // Code Added by Sam on 05012018 Section End

                    txtPortOfDestination.Text = "";
                    ddlPartialShippment.SelectedIndex = -1;
                    ddlTransshipment.SelectedIndex = -1;
                    txtPackingSpec.Text = "";
                    dtValidityOfOrder.Value = "";
                    txtValidityOfOrderRemarks.Text = "";
                    ddlCountryOfOrigin.SelectedIndex = -1;
                    txtFreeDetentionPeriod.Text = "";
                    txtFreeDetentionPeriodRemark.Text = "";
                }

                //if( hfTCspecefiFieldsVisibilityCheck.Value == "1")
                //{
                //    pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                //    pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                     
                //}
                //else
                //{
                //    pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                //    pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                    
                //}
            }
            catch (Exception ex) { }
        }
        public string GetControlValue(string controlID)
        {
            string returnVal = string.Empty;

            switch (controlID)
            {
                case "dtDeliveryDate":
                    returnVal = Convert.ToString(dtDeliveryDate.Value);
                    break;
                case "ddlTypeOfImport":
                    returnVal = Convert.ToString(ddlTypeOfImport.Value);
                    break; 
            }

            return returnVal;

        }
        public bool GetControlVisibility(string controlID)
        {
            bool returnVal = false;

            switch (controlID)
            {
                case "dtDeliveryDate":
                    returnVal = (hfTCspecefiFieldsVisibilityCheck.Value == "1") ? false : true;  //dtDeliveryDate.Visible;
                    break;
            }

            return returnVal;

        }
        public Int32 IsSalesOrderExistsInChallan(string DocId, string DocType)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("sp_Tagged_TC");
            proc.AddVarcharPara("@Action", 500, "IsSalesOrderExistsInChallan");
            proc.AddVarcharPara("@DocId", 500, Convert.ToString(DocId));
            proc.AddVarcharPara("@DocType", 500, Convert.ToString(DocType));

            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["OUTPUT"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }
        public Int32 IsSalesOrderExistsInInvoice(string DocId, string DocType)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("sp_Tagged_TC");
            proc.AddVarcharPara("@Action", 500, "IsSalesOrderExistsInInvoice");
            proc.AddVarcharPara("@DocId", 500, Convert.ToString(DocId));
            proc.AddVarcharPara("@DocType", 500, Convert.ToString(DocType));

            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["OUTPUT"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }
        public Int32 IsPurchaseOrderExistsInChallan(string DocId, string DocType)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("sp_Tagged_TC");
            proc.AddVarcharPara("@Action", 500, "IsPurchaseOrderExistsInChallan");
            proc.AddVarcharPara("@DocId", 500, Convert.ToString(DocId));
            proc.AddVarcharPara("@DocType", 500, Convert.ToString(DocType));

            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["OUTPUT"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }
        public Int32 IsPurchaseOrderExistsInInvoice(string DocId, string DocType)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("sp_Tagged_TC");
            proc.AddVarcharPara("@Action", 500, "IsPurchaseOrderExistsInInvoice");
            proc.AddVarcharPara("@DocId", 500, Convert.ToString(DocId));
            proc.AddVarcharPara("@DocType", 500, Convert.ToString(DocType));

            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["OUTPUT"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }
        public Int32 IsDocTypeApproved(string DocId, string DocType)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("sp_Tagged_TC");
            proc.AddVarcharPara("@Action", 500, "IsDocTypeApproved");
            proc.AddVarcharPara("@DocId", 500, Convert.ToString(DocId));
            proc.AddVarcharPara("@DocType", 500, Convert.ToString(DocType));

            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["OUTPUT"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }
        private DataTable GetTransporterBYLegalStatus(int lgl_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetTransporterBYLegalStatus");
            proc.AddIntegerPara("@lgl_id", lgl_id);
            dt = proc.GetTable();

            return dt;
        }
        public DataSet GetCountry(string countryID = null)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetAllCountry");
            proc.AddVarcharPara("@CountryID", 20, countryID);
            ds = proc.GetDataSet();
            return ds;
        }
        public void SaveTC(string TermsConditionData, string DocId, string DocType)
        {
            try
            {
                //REV 1.0
                string doctype = TermsConditionData.Split('|')[0];
                if(doctype=="@")
                {
                    doctype = "";
                }

                string DeliveryDate = TermsConditionData.Split('|')[1];                
                if (DeliveryDate != "@")
                {
                    string DD = DeliveryDate.Substring(0, 2);
                    string MM = DeliveryDate.Substring(3, 2);
                    string YYYY = DeliveryDate.Substring(6, 4);
                    string Date = YYYY + '-' + MM + '-' + DD;

                    DeliveryDate = Date;
                }
                if (DeliveryDate == "@")
                {
                    DeliveryDate =null;
                }
                string Delremarks=TermsConditionData.Split('|')[2];
                if (Delremarks == "@")
                {
                    Delremarks = "";
                }
                string insuranceType = TermsConditionData.Split('|')[3];
                if (insuranceType == "@")
                {
                    insuranceType = null;
                }
                string FreightCharges = TermsConditionData.Split('|')[4];
                if (FreightCharges == "@")
                {
                    FreightCharges = null;
                }
                string FreightRemarks = TermsConditionData.Split('|')[5];
                if (FreightRemarks == "@")
                {
                    FreightRemarks = "";
                }
                string PermitValue = TermsConditionData.Split('|')[6];
                if (PermitValue == "@")
                {
                    PermitValue = "";
                }
                string Remarks = TermsConditionData.Split('|')[7];
                if (Remarks == "@")
                {
                    Remarks = "";
                }
                string DelDetails = TermsConditionData.Split('|')[8];
                if (DelDetails == "@")
                {
                    DelDetails = null;
                }
                string otherlocation = TermsConditionData.Split('|')[9];
                if (otherlocation == "@")
                {
                    otherlocation = "";
                }
                string CertReq = TermsConditionData.Split('|')[10];
                if (CertReq == "@")
                {
                    CertReq = null;
                }
                string trnsprtrname = TermsConditionData.Split('|')[11];
                if (trnsprtrname == "@")
                {
                    trnsprtrname = null;
                }
                string Discntrcv = TermsConditionData.Split('|')[12];
                if (Discntrcv == "@")
                {
                    Discntrcv = null;
                }
                string Discntrcvdtls = TermsConditionData.Split('|')[13];
                if (Discntrcvdtls == "@")
                {
                    Discntrcvdtls = "";
                }
                string CommissionRcv = TermsConditionData.Split('|')[14];
                if (CommissionRcv == "@")
                {
                    CommissionRcv = null;
                }
                string CommissionRcvdtls = TermsConditionData.Split('|')[15];
                if (CommissionRcvdtls == "@")
                {
                    CommissionRcvdtls = "";
                }
                string TypeOfImport = TermsConditionData.Split('|')[16];
                if (TypeOfImport == "@")
                {
                    TypeOfImport = null;
                }
                string PaymentTrmRemarks = TermsConditionData.Split('|')[17];
                if (PaymentTrmRemarks == "@")
                {
                    PaymentTrmRemarks = "";
                }
                string IncoDVTerms = TermsConditionData.Split('|')[18];
                if (IncoDVTerms == "@")
                {
                    IncoDVTerms = null;
                }
                string IncoDVTermsRemarks = TermsConditionData.Split('|')[19];
                if (IncoDVTermsRemarks == "@")
                {
                    IncoDVTermsRemarks = "";
                }
                string ShippmentSchedule = TermsConditionData.Split('|')[20];
                if (ShippmentSchedule == "@")
                {
                    ShippmentSchedule = "";
                }
                string PortOfShippment = TermsConditionData.Split('|')[21];
                if (PortOfShippment == "@")
                {
                    PortOfShippment = "";
                }
                string PortOfDestination = TermsConditionData.Split('|')[22];
                if (PortOfDestination == "@")
                {
                    PortOfDestination = "";
                }
                string PartialShippment = TermsConditionData.Split('|')[23];
                if (PartialShippment == "@")
                {
                    PartialShippment = null;
                }
                string Transshipment = TermsConditionData.Split('|')[24];
                if (Transshipment == "@")
                {
                    Transshipment = null;
                }
                string PackingSpec = TermsConditionData.Split('|')[25];
                if (PackingSpec == "@")
                {
                    PackingSpec = "";
                }
                string ValidityOfOrderDate = TermsConditionData.Split('|')[26];
                if (ValidityOfOrderDate != "@")
                {
                    string DD = ValidityOfOrderDate.Substring(0, 2);
                    string MM = ValidityOfOrderDate.Substring(3, 2);
                    string YYYY = ValidityOfOrderDate.Substring(6, 4);
                    string Date = YYYY + '-' + MM + '-' + DD;
                    ValidityOfOrderDate = Date;
                }
                if (ValidityOfOrderDate == "@")
                {
                    ValidityOfOrderDate =null;
                }          
               
                
                string ValidityOfOrderRemarks = TermsConditionData.Split('|')[27];
                if (ValidityOfOrderRemarks == "@")
                {
                    ValidityOfOrderRemarks = "";
                }
                string CountryOfOrigin = TermsConditionData.Split('|')[28];
                if (CountryOfOrigin == "@")
                {
                    CountryOfOrigin = null;
                }
                string FreeDetentionPeriod = TermsConditionData.Split('|')[29];
                if (FreeDetentionPeriod == "@")
                {
                    FreeDetentionPeriod = "";
                }
                string FreeDetentionPeriodRemark = TermsConditionData.Split('|')[30];
                if (FreeDetentionPeriodRemark == "@")
                {
                    FreeDetentionPeriodRemark = "";
                }
                decimal CommissionRate = 0;
                string _CommissionRate = TermsConditionData.Split('|')[31];
                if (_CommissionRate == "@")
                {
                    CommissionRate = 0;
                }
                string BENumber = TermsConditionData.Split('|')[32];
                if (BENumber == "@")
                {
                    BENumber = "";
                }
                string BEDate = TermsConditionData.Split('|')[33];
                if (BEDate != "@")
                {
                    string DD = BEDate.Substring(0, 2);
                    string MM = BEDate.Substring(3, 2);
                    string YYYY = BEDate.Substring(6, 4);
                    string Date = YYYY + '-' + MM + '-' + DD;
                    BEDate = Date;
                }
                if (BEDate == "@")
                {
                    BEDate = null;
                }
                decimal BEValue = 0;
                string _BEValue = TermsConditionData.Split('|')[34];
                if (_BEValue == "@")
                {
                    BEValue = 0;
                }
                string Bank_Id = TermsConditionData.Split('|')[35];
                if (Bank_Id == "@")
                {
                    Bank_Id = "";
                }
                string Bank_Branch = TermsConditionData.Split('|')[36];
                if (Bank_Branch == "@")
                {
                    Bank_Branch = "";
                }
                string Bank_Address = TermsConditionData.Split('|')[37];
                if (Bank_Address == "@")
                {
                    Bank_Address = "";
                }
                string Bank_Landmark = TermsConditionData.Split('|')[38];
                if (Bank_Landmark == "@")
                {
                    Bank_Landmark = "";
                }
                string Bank_Pin = TermsConditionData.Split('|')[39];
                if (Bank_Pin == "@")
                {
                    Bank_Pin = "";
                }
                string Bank_AcNo = TermsConditionData.Split('|')[40];
                if (Bank_AcNo == "@")
                {
                    Bank_AcNo = "";
                }
                string Bank_SwiftCode = TermsConditionData.Split('|')[41];
                if (Bank_SwiftCode == "@")
                {
                    Bank_SwiftCode = "";
                }
                string Bank_RTGSCode = TermsConditionData.Split('|')[42];
                if (Bank_RTGSCode == "@")
                {
                    Bank_RTGSCode = "";
                }
                string Bank_IFSCCode = TermsConditionData.Split('|')[43];
                if (Bank_IFSCCode == "@")
                {
                    Bank_IFSCCode = "";
                }
                string Bank_Remarks = TermsConditionData.Split('|')[44];
                if (Bank_Remarks == "@")
                {
                    Bank_Remarks = "";
                }
                string TC_PaymentTerms = TermsConditionData.Split('|')[45];
                if (TC_PaymentTerms == "@")
                {
                    TC_PaymentTerms = "";
                }
                string Project = TermsConditionData.Split('|')[46];
                if (Project == "@")
                {
                    Project = "";
                }



                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Prc_TermsConditions", con);                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ADDEditTermsCondition");
                cmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
                cmd.Parameters.AddWithValue("@Delremarks", Delremarks);
                cmd.Parameters.AddWithValue("@insuranceType", insuranceType);
                cmd.Parameters.AddWithValue("@FreightCharges", FreightCharges);
                cmd.Parameters.AddWithValue("@FreightRemarks", FreightRemarks);
                cmd.Parameters.AddWithValue("@PermitValue", PermitValue);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@DelDetails", DelDetails);
                cmd.Parameters.AddWithValue("@otherlocation", otherlocation);
                cmd.Parameters.AddWithValue("@CertReq", CertReq);
                cmd.Parameters.AddWithValue("@trnsprtrname", trnsprtrname);
                cmd.Parameters.AddWithValue("@Discntrcv", Discntrcv);
                cmd.Parameters.AddWithValue("@Discntrcvdtls", Discntrcvdtls);
                cmd.Parameters.AddWithValue("@CommissionRcv", CommissionRcv);
                cmd.Parameters.AddWithValue("@CommissionRcvdtls", CommissionRcvdtls);
                cmd.Parameters.AddWithValue("@TypeOfImport", TypeOfImport);
                cmd.Parameters.AddWithValue("@PaymentTrmRemarks", PaymentTrmRemarks);
                cmd.Parameters.AddWithValue("@IncoDVTerms", IncoDVTerms);
                cmd.Parameters.AddWithValue("@IncoDVTermsRemarks", IncoDVTermsRemarks);
                cmd.Parameters.AddWithValue("@ShippmentSchedule", ShippmentSchedule);
                cmd.Parameters.AddWithValue("@PortOfShippment", PortOfShippment);
                cmd.Parameters.AddWithValue("@PortOfDestination", PortOfDestination);
                cmd.Parameters.AddWithValue("@PartialShippment", PartialShippment);
                cmd.Parameters.AddWithValue("@Transshipment", Transshipment);
                cmd.Parameters.AddWithValue("@PackingSpec", PackingSpec);
                cmd.Parameters.AddWithValue("@ValidityOfOrderDate", ValidityOfOrderDate);
                cmd.Parameters.AddWithValue("@ValidityOfOrderRemarks", ValidityOfOrderRemarks);
                cmd.Parameters.AddWithValue("@CountryOfOrigin", CountryOfOrigin);
                cmd.Parameters.AddWithValue("@FreeDetentionPeriod", FreeDetentionPeriod);
                cmd.Parameters.AddWithValue("@FreeDetentionPeriodRemark", FreeDetentionPeriodRemark);
                cmd.Parameters.AddWithValue("@CommissionRate", CommissionRate);
                cmd.Parameters.AddWithValue("@BENumber", BENumber);
                cmd.Parameters.AddWithValue("@BEDate", BEDate);
                cmd.Parameters.AddWithValue("@BEValue", BEValue);
                cmd.Parameters.AddWithValue("@Bank_Id", Bank_Id);
                cmd.Parameters.AddWithValue("@Bank_Branch", Bank_Branch);
                cmd.Parameters.AddWithValue("@Bank_Address", Bank_Address);
                cmd.Parameters.AddWithValue("@Bank_Landmark", Bank_Landmark);
                cmd.Parameters.AddWithValue("@Bank_Pin", Bank_Pin);
                cmd.Parameters.AddWithValue("@Bank_AcNo", Bank_AcNo);
                cmd.Parameters.AddWithValue("@Bank_SwiftCode", Bank_SwiftCode);
                cmd.Parameters.AddWithValue("@Bank_RTGSCode", Bank_RTGSCode);
                cmd.Parameters.AddWithValue("@Bank_IFSCCode", Bank_IFSCCode);
                cmd.Parameters.AddWithValue("@Bank_Remarks", Bank_Remarks);
                cmd.Parameters.AddWithValue("@TC_PaymentTerms", TC_PaymentTerms);
                cmd.Parameters.AddWithValue("@Project", Project);
                cmd.Parameters.AddWithValue("@DocId", DocId);
                cmd.Parameters.AddWithValue("@DocType", DocType);


                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                Int64 ReturnValue = Convert.ToInt64(output.Value);


                //ProcedureExecute proc = new ProcedureExecute("SP_TC_CRUD");
                //proc.AddVarcharPara("@TermsConditionData", 500, TermsConditionData);               
                //proc.AddVarcharPara("@DocId", 500, DocId);
                //proc.AddVarcharPara("@DocType", 500, DocType);
                //int _Ret = proc.RunActionQuery();

                //REV 1.0 END
            }
            catch (Exception ex) { }
        }
        public string GetCountryByVendorId(string cntId)
        {
            try
            {
                ProcedureExecute proc = new ProcedureExecute("SP_TC_CRUD");
                proc.AddVarcharPara("@Action", 500, "GetCountryByVendor");
                proc.AddVarcharPara("@cntId", 500, cntId);

                DataTable DT = proc.GetTable();
                if (DT != null && DT.Rows.Count > 0)
                {
                    return Convert.ToString(DT.Rows[0][0]).ToUpper();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex) { return ""; }
        }

        public void SHowHidePanelVendorWise(int DocId, string DocType)
        {

            DataTable dtImport = objPurchaseInvoice.SHowHidePanelVendorWise(DocId, DocType);
            if(dtImport.Rows.Count>0)
            {
                if (Convert.ToString(dtImport.Rows[0][0])!="" && Convert.ToString(dtImport.Rows[0][0])!="1")
                {
                    pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                    pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                else
                {
                    pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                }
                
            }
            else
            {
                pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
            }

            

        }
        protected void callBackuserControlPanelMainTC_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] Data = e.Parameter.Split('~');
                #region TC Tagging Process
                if (Data != null && Data.Length == 3 && Data[0] == "TCtagging") // Tagging Property
                {
                    string DocId = Data[1];
                    string DocType = Data[2];

                    BindTC(DocId, DocType);
                    if (DocType == "PO" || DocType == "PC" || DocType == "PI")
                    {
                        SHowHidePanelVendorWise(Convert.ToInt32(DocId), DocType);
                    }
                    
                }
                #endregion
                #region Show PO specefic fields
                if (Data != null && Data.Length == 2 && Data[0] == "TCspecefiFields_PO") // Show PO specefic fields
                {
                    if (GetCountryByVendorId(Data[1]) != "" && GetCountryByVendorId(Data[1]) != "INDIA") //
                    {
                        pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                        pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                        hfTCspecefiFieldsVisibilityCheck.Value = "1";
                    }
                    else
                    {
                        pnl_TCspecefiFields_PO.Style.Add(HtmlTextWriterStyle.Display, "none");
                        pnl_TCspecefiFields_Not_PO.Style.Add(HtmlTextWriterStyle.Display, "block");
                        hfTCspecefiFieldsVisibilityCheck.Value = "0";
                    }
                }




                if (!string.IsNullOrEmpty(hdnTCBranchId.Value))
                {
                    dsBankName.SelectParameters["BranchId"].DefaultValue = Convert.ToString(hdnTCBranchId.Value);
                    ddlBankName.DataBind();
                }
                #endregion
            }
            catch (Exception ex) { }
        }
          
        public string GetTermsConditionType()
        {
            // Here we are sending Branch StateCode instead of Shipping Statecode after discuss with
            // Pijush Da and Debjyoti on 14122017

            return hfTCspecefiFieldsVisibilityCheck.Value;
            //return ucBShfSStateCode.Value;

        }

        public void SetBranchId(string _BranchId){
            hdnTCBranchId.Value = _BranchId;
        }


    }
}