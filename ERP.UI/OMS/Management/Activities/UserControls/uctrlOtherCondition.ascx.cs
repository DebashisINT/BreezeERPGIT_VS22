/*************************************************************************************************************************************
 * Written by Sanchita on 28-09-2023 for V2.0.40
 * Few Fields required in the Quotation Entry Module for the Purpose of Quotation Print from ERP
 * New button "Other Condiion" to show instead of "Terms & Condition" Button if the settings "Show Other Condition" is set as "Yes"
 * Mantis: 26868
 * ***********************************************************************************************************************************/
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
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    #region Show T&C
                    //string Variable_Name = string.Empty;
                    //if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "")
                    //{
                    //    string Type = Convert.ToString(Request.QueryString["type"]);
                    //    Variable_Name = "Show_OC_" + Type;
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        HiddenField ctl = (HiddenField)this.Parent.FindControl("hfOtherConditionDocType");
                    //        string DocType = ctl.Value;
                    //        Variable_Name = "Show_OC_" + DocType;
                    //    }
                    //    catch (Exception ex) { Variable_Name = "Show_OC_QO"; }
                    //}

                    DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Other_Condition' AND IsActive=1");

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
                    #endregion

                    #region Bind controll
                    if (Request.QueryString["key"] != null && Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        string docid = Convert.ToString(Request.QueryString["key"]);

                        if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "")
                        {
                            string Type = Convert.ToString(Request.QueryString["type"]);
                            BindOC(docid, Type); //bind existing data
                            //SHowHidePanelVendorWise(Convert.ToInt32(docid), Type);
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

        private void DisableControls(bool Isvisible)
        {
            btnOCsave.Visible = Isvisible;

            txtPriceBasis.Enabled = Isvisible;
            txtLoadingCharges.Enabled = Isvisible;
            txtDetentionCharges.Enabled = Isvisible;
            txtDeliveryPeriod.Enabled = Isvisible;
            txtInspection.Enabled = Isvisible;
            txtPaymentTermsOther.Enabled = Isvisible;
            dtOfferValidUpto.ClientEnabled = Isvisible;
            txtQuantityTol.Enabled = Isvisible;
            txtDimensionalTol.Enabled = Isvisible;
            txtThicknessTol.Enabled = Isvisible;
            txtWarranty.Enabled = Isvisible;
            txtDeviation.Enabled = Isvisible;
            txtLDClause.Enabled = Isvisible;
            txtInterestClause.Enabled = Isvisible;
            txtPriceEscalationClause.Enabled = Isvisible;
            txtInternalCoating.Enabled = Isvisible;
            txtExternalCoating.Enabled = Isvisible;
            txtSpecialNote.Enabled = Isvisible;
            
        }

        protected void callBackuserControlPanelMainOC_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] Data = e.Parameter.Split('~');
                #region TC Tagging Process
                if (Data != null && Data.Length == 3 && Data[0] == "OCtagging") // Tagging Property
                {
                    string DocId = Data[1];
                    string DocType = Data[2];

                    BindOC(DocId, DocType);

                }
                #endregion
                
            }
            catch (Exception ex) { }
        }

        public void BindOC(string docid, string Type)
        {
            try
            {
                DataTable DT = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_OtherConditionAddEdit");
                proc.AddVarcharPara("@Action", 500, "GetTOCTagDetail");
                proc.AddVarcharPara("@DocId", 500, docid);
                proc.AddVarcharPara("@DocType", 500, Type);
                DT = proc.GetTable();

                if (DT != null && DT.Rows.Count > 0)
                {

                    txtPriceBasis.Text = Convert.ToString(DT.Rows[0]["PriceBasis"]);
                    txtLoadingCharges.Text = Convert.ToString(DT.Rows[0]["LoadingCharges"]);
                    txtDetentionCharges.Text = Convert.ToString(DT.Rows[0]["DetentionCharges"]);
                    txtDeliveryPeriod.Text = Convert.ToString(DT.Rows[0]["DeliveryPeriod"]);
                    txtInspection.Text = Convert.ToString(DT.Rows[0]["Inspection"]);
                    txtPaymentTermsOther.Text = Convert.ToString(DT.Rows[0]["PaymentTerms"]);

                    if (DT.Rows[0]["OfferValidUpto"] != null && DT.Rows[0]["OfferValidUpto"].ToString() != "")
                    {
                        dtOfferValidUpto.Value = Convert.ToDateTime(DT.Rows[0]["OfferValidUpto"]);
                    }

                    txtQuantityTol.Text = Convert.ToString(DT.Rows[0]["QuantityTol"]);
                    txtDimensionalTol.Text = Convert.ToString(DT.Rows[0]["DimensionalTol"]);
                    txtThicknessTol.Text = Convert.ToString(DT.Rows[0]["ThicknessTol"]);
                    txtWarranty.Text = Convert.ToString(DT.Rows[0]["Warranty"]);
                    txtDeviation.Text = Convert.ToString(DT.Rows[0]["Deviation"]);
                    txtLDClause.Text = Convert.ToString(DT.Rows[0]["LDClause"]);
                    txtInterestClause.Text = Convert.ToString(DT.Rows[0]["InterestClause"]);
                    txtPriceEscalationClause.Text = Convert.ToString(DT.Rows[0]["PriceEscalationClause"]);
                    txtInternalCoating.Text = Convert.ToString(DT.Rows[0]["InternalCoating"]);
                    txtExternalCoating.Text = Convert.ToString(DT.Rows[0]["ExternalCoating"]);
                    txtSpecialNote.Text = Convert.ToString(DT.Rows[0]["SpecialNote"]);

                }
                else
                {
                    txtPriceBasis.Text = "";
                    txtLoadingCharges.Text = "";
                    txtDetentionCharges.Text = "";
                    txtDeliveryPeriod.Text = "";
                    txtInspection.Text = "";
                    txtPaymentTermsOther.Text = "";
                    dtOfferValidUpto.Value = "";
                    txtQuantityTol.Text = "";
                    txtDimensionalTol.Text = "";
                    txtThicknessTol.Text = "";
                    txtWarranty.Text = "";
                    txtDeviation.Text = "";
                    txtLDClause.Text = "";
                    txtInterestClause.Text = "";
                    txtPriceEscalationClause.Text = "";
                    txtInternalCoating.Text = "";
                    txtExternalCoating.Text = "";
                    txtSpecialNote.Text = "";
                }
            }
            catch (Exception ex) { }
        }

        public void SaveOC(string OtherConditionData, string DocId, string DocType)
        {
            try
            {
                string doctype = OtherConditionData.Split('|')[0];
                if (doctype == "@")
                {
                    doctype = "";
                }

                string PriceBasis = OtherConditionData.Split('|')[1];
                if (PriceBasis == "@")
                {
                    PriceBasis = "";
                }
                string LoadingCharges = OtherConditionData.Split('|')[2];
                if (LoadingCharges == "@")
                {
                    LoadingCharges = null;
                }
                string DetentionCharges = OtherConditionData.Split('|')[3];
                if (DetentionCharges == "@")
                {
                    DetentionCharges = null;
                }
                string DeliveryPeriod = OtherConditionData.Split('|')[4];
                if (DeliveryPeriod == "@")
                {
                    DeliveryPeriod = "";
                }
                string Inspection = OtherConditionData.Split('|')[5];
                if (Inspection == "@")
                {
                    Inspection = "";
                }
                string PaymentTerms = OtherConditionData.Split('|')[6];
                if (PaymentTerms == "@")
                {
                    PaymentTerms = "";
                }

                string OfferValidUpto = OtherConditionData.Split('|')[7];
                if (OfferValidUpto != "@")
                {
                    string DD = OfferValidUpto.Substring(0, 2);
                    string MM = OfferValidUpto.Substring(3, 2);
                    string YYYY = OfferValidUpto.Substring(6, 4);
                    string Date = YYYY + '-' + MM + '-' + DD;

                    OfferValidUpto = Date;
                }
                if (OfferValidUpto == "@")
                {
                    OfferValidUpto = null;
                }

                string QuantityTol = OtherConditionData.Split('|')[8];
                if (QuantityTol == "@")
                {
                    QuantityTol = null;
                }
                string DimensionalTol = OtherConditionData.Split('|')[9];
                if (DimensionalTol == "@")
                {
                    DimensionalTol = "";
                }
                string ThicknessTol = OtherConditionData.Split('|')[10];
                if (ThicknessTol == "@")
                {
                    ThicknessTol = null;
                }
                string Warranty = OtherConditionData.Split('|')[11];
                if (Warranty == "@")
                {
                    Warranty = null;
                }
                string Deviation = OtherConditionData.Split('|')[12];
                if (Deviation == "@")
                {
                    Deviation = null;
                }
                string LDClause = OtherConditionData.Split('|')[13];
                if (LDClause == "@")
                {
                    LDClause = "";
                }
                string InterestClause = OtherConditionData.Split('|')[14];
                if (InterestClause == "@")
                {
                    InterestClause = null;
                }
                string PriceEscalationClause = OtherConditionData.Split('|')[15];
                if (PriceEscalationClause == "@")
                {
                    PriceEscalationClause = "";
                }
                string InternalCoating = OtherConditionData.Split('|')[16];
                if (InternalCoating == "@")
                {
                    InternalCoating = null;
                }
                string ExternalCoating = OtherConditionData.Split('|')[17];
                if (ExternalCoating == "@")
                {
                    ExternalCoating = "";
                }
                string SpecialNote = OtherConditionData.Split('|')[18];
                if (SpecialNote == "@")
                {
                    SpecialNote = null;
                }
                
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_OtherConditionAddEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ADDEditOtherCondition");
                cmd.Parameters.AddWithValue("@DocId", DocId);
                cmd.Parameters.AddWithValue("@DocType", DocType);
                cmd.Parameters.AddWithValue("@PriceBasis", PriceBasis);
                cmd.Parameters.AddWithValue("@LoadingCharges", LoadingCharges);
                cmd.Parameters.AddWithValue("@DetentionCharges", DetentionCharges);
                cmd.Parameters.AddWithValue("@DeliveryPeriod", DeliveryPeriod);
                cmd.Parameters.AddWithValue("@Inspection", Inspection);
                cmd.Parameters.AddWithValue("@PaymentTerms", PaymentTerms);
                cmd.Parameters.AddWithValue("@OfferValidUpto", OfferValidUpto);
                cmd.Parameters.AddWithValue("@QuantityTol", QuantityTol);
                cmd.Parameters.AddWithValue("@DimensionalTol", DimensionalTol);
                cmd.Parameters.AddWithValue("@ThicknessTol", ThicknessTol);
                cmd.Parameters.AddWithValue("@Warranty", Warranty);
                cmd.Parameters.AddWithValue("@Deviation", Deviation);
                cmd.Parameters.AddWithValue("@LDClause", LDClause);
                cmd.Parameters.AddWithValue("@InterestClause", InterestClause);
                cmd.Parameters.AddWithValue("@PriceEscalationClause", PriceEscalationClause);
                cmd.Parameters.AddWithValue("@InternalCoating", InternalCoating);
                cmd.Parameters.AddWithValue("@ExternalCoating", ExternalCoating);
                cmd.Parameters.AddWithValue("@SpecialNote", SpecialNote);

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                Int64 ReturnValue = Convert.ToInt64(output.Value);

            }
            catch (Exception ex) { }
        }
    }
}