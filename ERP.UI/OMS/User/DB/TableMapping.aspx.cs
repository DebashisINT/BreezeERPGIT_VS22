using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.User.DB
{

    public partial class DP_User_DB_Cdsl_TableMapping_New : System.Web.UI.Page//, ICallbackEventHandler
    {
        BusinessLogicLayer.GenericMethod Gm;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = null;
                Gm = new BusinessLogicLayer.GenericMethod();
                Gm.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                Gm.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                CbpContainer.JSProperties["cpResult"] = null;
                Gm = new BusinessLogicLayer.GenericMethod();
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetGeographicalData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Geographical Code") + "')");

                txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetAddressProofData("AddressProof_CVLID") + "')");
                txtKRA.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRA_MappingStaticData("AddressProof", 1) + "')");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoadFunction", "<script>PageLoad();</script>");
        }

        protected void CbpContainer_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CbpContainer.JSProperties["cpResult"] = null;
            string[] strSplit = e.Parameter.Split('^');
            string WhichCall = strSplit[0];

            #region Fetch From CDSL Master Tables
            Gm = new BusinessLogicLayer.GenericMethod();
            if (WhichCall == "Geographical")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetGeographicalData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Geographical Code") + "')");
            }
            if (WhichCall == "AnnualIncome")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetAnnualIncomeData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Annual Income Code") + "')");
            }
            if (WhichCall == "Nationality")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetNationalityData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Nationality Code") + "')");
            }
            if (WhichCall == "Education")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetEducationData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Education / Degree") + "')");
            }
            if (WhichCall == "LegalStatus")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetLegalStatusData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("BO Status") + "')");
            }
            if (WhichCall == "Language")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetLanguageData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Language Code") + "')");
            }
            if (WhichCall == "PanExemption")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCdslPanExemptionData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("PAN Exemption Code") + "')");
            }
            if (WhichCall == "CdslOccupation")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCdslOccupationData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Occupation Code") + "')");
            }
            if (WhichCall == "Currency")
            {
                txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCurrencyData() + "')");
                txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Account Currency") + "')");
            }
            #endregion

            #region Automatic Fill From Master_CdslStaticDataCode Table
            if (WhichCall == "AutoFillStaticCDSL")
            {
                CbpContainer.JSProperties["cpAutoFillCDSL"] = null;
                CbpContainer.JSProperties["cpAutoFillCDSLData"] = null;
                string selectedCDSLTab = e.Parameter.Split('^')[1];
                int oMappedStaticCDSLID = Convert.ToInt32(e.Parameter.Split('^')[2].ToString());
                Gm = new BusinessLogicLayer.GenericMethod();
                if (selectedCDSLTab == "Geographical")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetGeographicalData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Geographical Code") + "')");
                }
                if (selectedCDSLTab == "AnnualIncome")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetAnnualIncomeData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Annual Income Code") + "')");
                }
                if (selectedCDSLTab == "Nationality")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetNationalityData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Nationality Code") + "')");
                }
                if (selectedCDSLTab == "Education")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetEducationData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Education / Degree") + "')");
                }
                if (selectedCDSLTab == "LegalStatus")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetLegalStatusData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("BO Status") + "')");
                }
                if (selectedCDSLTab == "Language")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetLanguageData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Language Code") + "')");
                }
                if (selectedCDSLTab == "PanExemption")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCdslPanExemptionData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("PAN Exemption Code") + "')");
                }
                if (selectedCDSLTab == "CdslOccupation")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCdslOccupationData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Occupation Code") + "')");
                }
                if (selectedCDSLTab == "Currency")
                {
                    txtpkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCurrencyData() + "')");
                    txtcdsl.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCDSL_MappingStaticData("Account Currency") + "')");
                }
                Gm = new BusinessLogicLayer.GenericMethod();
                DataTable dtStaticCDSL = Gm.GetDataTable("Select * from Master_CdslStaticDataCode where CdslStaticData_ID=" + oMappedStaticCDSLID);
                if (dtStaticCDSL.Rows.Count > 0)
                {
                    CbpContainer.JSProperties["cpAutoFillCDSL"] = "T";
                    CbpContainer.JSProperties["cpAutoFillCDSLData"] = selectedCDSLTab + "^" + dtStaticCDSL.Rows[0]["CdslStaticData_ID"].ToString() + "~" + dtStaticCDSL.Rows[0]["CdslStaticData_Description"].ToString() + "~" + dtStaticCDSL.Rows[0]["CdslStaticData_TypeCode"].ToString() + "~" + dtStaticCDSL.Rows[0]["CdslStaticData_SubTypeCode"].ToString();
                }
                else
                    CbpContainer.JSProperties["cpAutoFillCDSL"] = "F";
            }
            #endregion

            #region Update CDSL Master Tables
            if (WhichCall == "update")
            {
                string internaltableid = strSplit[1];
                string cdslstaticsymbol = strSplit[2];
                string tabcontent = strSplit[3];

                //Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~Master_Geographical~Geographical_CdslID|More than one fields (example)");
                Gm = new BusinessLogicLayer.GenericMethod();
                if (tabcontent == "Geographical")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~Master_Geographical~Geographical_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~Geographical_ID='" + internaltableid + "'");
                if (tabcontent == "AnnualIncome")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~Master_AnnualIncome~AnnualIncome_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~AnnualIncome_ID='" + internaltableid + "'");
                if (tabcontent == "Nationality")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~Master_Nationality~Nationality_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~Nationality_ID='" + internaltableid + "'");
                if (tabcontent == "Education")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~tbl_master_education~edu_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~edu_ID='" + internaltableid + "'");
                if (tabcontent == "LegalStatus")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~tbl_master_legalStatus~lgl_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~lgl_ID='" + internaltableid + "'");
                if (tabcontent == "Language")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~master_language~Langauge_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~Language_ID='" + internaltableid + "'");
                if (tabcontent == "PanExemption")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~Master_PANExemptCategory~PanExemptCategory_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~PanExemptCategory_ID='" + internaltableid + "'");
                if (tabcontent == "CdslOccupation")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~tbl_master_profession~PRO_CdslID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~pro_id='" + internaltableid + "'");
                if (tabcontent == "Currency")
                    Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~Master_Currency~Currency_CDSLID~'" + Convert.ToInt32(cdslstaticsymbol.ToString().Trim()) + "'~Currency_ID='" + internaltableid + "'");

                CbpContainer.JSProperties["cpResult"] = tabcontent + "~Successfully Update";
            }
            #endregion
        }

        protected void CbpKRAContainer_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CbpKRAContainer.JSProperties["cpUpdate"] = null;
            string WhichCall = e.Parameter.Split('^')[0];

            #region Fetch From KRA Master Tables
            if (WhichCall == "FetchKRA")
            {
                Gm = new BusinessLogicLayer.GenericMethod();
                string CallBy = e.Parameter.Split('^')[1];
                int oCmbKra = Convert.ToInt32(e.Parameter.Split('^')[2].ToString());
                string oMapField = string.Empty;
                if (CallBy == "AddressProof")
                {
                    if (oCmbKra == 1)
                        oMapField = "AddressProof_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "AddressProof_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "AddressProof_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetAddressProofData(oMapField) + "')");
                }
                else if (CallBy == "IdentityProof")
                {
                    if (oCmbKra == 1)
                        oMapField = "IdentityProof_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "IdentityProof_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "IdentityProof_DotExId";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetIdentityProofData(oMapField) + "')");
                }
                else if (CallBy == "PanExemptCategory")
                {
                    if (oCmbKra == 1)
                        oMapField = "PanExemptCategory_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "PanExemptCategory_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "PanExemptCategory_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetPanExemptCategoryData(oMapField) + "')");
                }
                else if (CallBy == "IndividualStatus")
                {
                    if (oCmbKra == 1)
                        oMapField = "lgl_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "lgl_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "lgl_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetIndividualStatusData(oMapField) + "')");
                }
                else if (CallBy == "NRIStatusProof")
                {
                    if (oCmbKra == 1)
                        oMapField = "NRIStatusProof_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "NRIStatusProof_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "NRIStatusProof_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetNRIStatusProofData(oMapField) + "')");
                }
                else if (CallBy == "NonIndividualStatus")
                {
                    if (oCmbKra == 1)
                        oMapField = "NonIndividualStatus_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "NonIndividualStatus_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "NonIndividualStatus_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetNonIndividualStatusData(oMapField) + "')");
                }
                else if (CallBy == "Occupation")
                {
                    if (oCmbKra == 1)
                        oMapField = "PRO_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "PRO_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "PRO_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetOccupationData(oMapField) + "')");
                }
                else if (CallBy == "PoliticalConnection")
                {
                    if (oCmbKra == 1)
                        oMapField = "PoliticalConnection_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "PoliticalConnection_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "PoliticalConnection_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetPoliticalConnectionData(oMapField) + "')");
                }
                else if (CallBy == "MaritalStatus")
                {
                    if (oCmbKra == 1)
                        oMapField = "mts_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "mts_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "mts_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetMaritalStatusData(oMapField) + "')");
                }
                else if (CallBy == "State")
                {
                    if (oCmbKra == 1)
                        oMapField = "State_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "State_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "State_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetStateData(oMapField) + "')");
                }
                else if (CallBy == "Country")
                {
                    if (oCmbKra == 1)
                        oMapField = "Country_CvlID";
                    else if (oCmbKra == 2)
                        oMapField = "Country_NdmlID";
                    else if (oCmbKra == 3)
                        oMapField = "Country_DotExID";

                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCountryData(oMapField) + "')");
                }
                else if (CallBy == "ContactType")
                {
                    if (oCmbKra == 1)
                        oMapField = "ContactType_CVLID";
                    else if (oCmbKra == 2)
                    {
                        CallBy = "Intermediary Role";
                        oMapField = "ContactType_NDMLId";
                    }
                    else if (oCmbKra == 3)
                    {
                        CallBy = "Intermediary Type";
                        oMapField = "ContactType_DotExID";
                    }
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetContactTypeData(oMapField) + "')");
                }
                else if (CallBy == "KRANationality")
                {
                    if (oCmbKra == 1)
                        oMapField = "Nationality_CVLid";
                    else if (oCmbKra == 2)
                        oMapField = "Nationality_NDMLid";
                    else if (oCmbKra == 3)
                        oMapField = "Nationality_DotExid";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRANationalityData(oMapField) + "')");
                }
                else if (CallBy == "KRAAnnualIncome")
                {
                    if (oCmbKra == 1)
                        oMapField = "AnnualIncome_CVLID";
                    else if (oCmbKra == 2)
                        oMapField = "AnnualIncome_NDMLId";
                    else if (oCmbKra == 3)
                        oMapField = "AnnualIncome_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRAAnnualIncomeData(oMapField) + "')");
                }
                if (CallBy == "KRANationality") CallBy = "Nationality";
                if (CallBy == "KRAAnnualIncome") CallBy = "AnnualIncome";
                txtKRA.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRA_MappingStaticData(CallBy, oCmbKra) + "')");
            }
            #endregion

            #region Automatically Fill From Config_KRAStaticData Table
            if (WhichCall == "AutoFillStaticKRA")
            {
                CbpKRAContainer.JSProperties["cpAutoFillKRA"] = null;
                CbpKRAContainer.JSProperties["cpAutoFillKRAData"] = null;
                string selectedKRATab = e.Parameter.Split('^')[1];
                int selectedKRAComboID = Convert.ToInt32(e.Parameter.Split('^')[2].ToString());
                int oMappedStaticKRAID = Convert.ToInt32(e.Parameter.Split('^')[3].ToString());

                string oSelectedMapField = null;
                Gm = new BusinessLogicLayer.GenericMethod();
                if (selectedKRATab == "AddressProof")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "AddressProof_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "AddressProof_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "AddressProof_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetAddressProofData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "IdentityProof")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "IdentityProof_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "IdentityProof_NDMLID";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "IdentityProof_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetIdentityProofData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "PanExemptCategory")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "PanExemptCategory_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "PanExemptCategory_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "PanExemptCategory_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetPanExemptCategoryData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "IndividualStatus")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "lgl_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "lgl_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "lgl_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetIndividualStatusData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "NRIStatusProof")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "NRIStatusProof_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "NRIStatusProof_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "NRIStatusProof_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetNRIStatusProofData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "NonIndividualStatus")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "NonIndividualStatus_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "NonIndividualStatus_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "NonIndividualStatus_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetNonIndividualStatusData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "Occupation")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "PRO_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "PRO_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "PRO_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetOccupationData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "PoliticalConnection")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "PoliticalConnection_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "PoliticalConnection_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "PoliticalConnection_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetPoliticalConnectionData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "MaritalStatus")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "mts_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "mts_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "mts_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetMaritalStatusData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "State")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "State_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "State_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "State_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetStateData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "Country")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "Country_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "Country_NdmlID";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "Country_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCountryData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "ContactType")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "ContactType_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "ContactType_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "ContactType_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetContactTypeData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "KRANationality")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "Nationality_CVLid";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "Nationality_NDMLid";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "Nationality_DotExid";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRANationalityData(oSelectedMapField) + "')");
                }
                else if (selectedKRATab == "KRAAnnualIncome")
                {
                    if (selectedKRAComboID == 1)
                        oSelectedMapField = "AnnualIncome_CVLID";
                    else if (selectedKRAComboID == 2)
                        oSelectedMapField = "AnnualIncome_NDMLId";
                    else if (selectedKRAComboID == 3)
                        oSelectedMapField = "AnnualIncome_DotExID";
                    txtKRApkcg.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRAAnnualIncomeData(oSelectedMapField) + "')");
                }
                if (selectedKRATab == "KRANationality") selectedKRATab = "Nationality";
                if (selectedKRATab == "KRAAnnualIncome") selectedKRATab = "AnnualIncome";
                txtKRA.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetKRA_MappingStaticData(selectedKRATab, selectedKRAComboID) + "')");

                DataTable dtStaticKRA = Gm.GetDataTable("Select * from Config_KRAStaticData where KRAStaticData_ID=" + oMappedStaticKRAID);
                if (dtStaticKRA.Rows.Count > 0)
                {
                    if (e.Parameter.Split('^')[1] == "KRA" + selectedKRATab)
                        selectedKRATab = e.Parameter.Split('^')[1];
                    CbpKRAContainer.JSProperties["cpAutoFillKRA"] = "T";
                    CbpKRAContainer.JSProperties["cpAutoFillKRAData"] = selectedKRATab + "^" + selectedKRAComboID + "^" + dtStaticKRA.Rows[0]["KRAStaticData_ID"].ToString() + "~" + dtStaticKRA.Rows[0]["KRAStaticData_Value"].ToString() + "~" + dtStaticKRA.Rows[0]["KRAStaticData_Code"].ToString() + "~" + dtStaticKRA.Rows[0]["KRAStaticData_SubCode"].ToString();
                }
                else
                    CbpKRAContainer.JSProperties["cpAutoFillKRA"] = "F";
            }
            #endregion

            #region Update KRA Master Tables
            if (WhichCall == "UpdateKRA")
            {
                string UpdateBy = e.Parameter.Split('^')[1];
                int selCmbKra = Convert.ToInt32(e.Parameter.Split('^')[2].ToString());
                string MasterMapID = e.Parameter.Split('^')[3];
                int KRAMapID = Convert.ToInt32(e.Parameter.Split('^')[4].ToString());

                string oTableName = string.Empty;
                string oFieldName = string.Empty;
                string oWhere = string.Empty;
                int oValue = KRAMapID;

                DataTable dtUpdateResult;
                if (UpdateBy == "AddressProof")
                {
                    oTableName = "Master_AddressProof";
                    oWhere = "AddressProof_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "AddressProof_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "AddressProof_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "AddressProof_DotExID";

                }
                else if (UpdateBy == "IdentityProof")
                {
                    oTableName = "Master_IdentityProof";
                    oWhere = "IdentityProof_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "IdentityProof_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "IdentityProof_NDMLID";
                    else if (selCmbKra == 3)
                        oFieldName = "IdentityProof_DotExId";
                }
                else if (UpdateBy == "PanExemptCategory")
                {
                    oTableName = "Master_PanExemptCategory";
                    oWhere = "PanExemptCategory_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "PanExemptCategory_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "PanExemptCategory_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "PanExemptCategory_DotExID";
                }
                else if (UpdateBy == "IndividualStatus")
                {
                    oTableName = "tbl_master_legalStatus";
                    oWhere = "lgl_id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "lgl_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "lgl_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "lgl_DotExID";
                }
                else if (UpdateBy == "NRIStatusProof")
                {
                    oTableName = "Master_NRIStatusProof";
                    oWhere = "NRIStatusProof_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "NRIStatusProof_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "NRIStatusProof_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "NRIStatusProof_DotExID";
                }
                else if (UpdateBy == "NonIndividualStatus")
                {
                    oTableName = "Master_NonIndividualStatus";
                    oWhere = "NonIndividualStatus_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "NonIndividualStatus_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "NonIndividualStatus_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "NonIndividualStatus_DotExID";
                }
                else if (UpdateBy == "Occupation")
                {
                    oTableName = "tbl_master_profession";
                    oWhere = "pro_id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "PRO_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "PRO_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "PRO_DotExID";
                }
                else if (UpdateBy == "PoliticalConnection")
                {
                    oTableName = "Master_PoliticalConnection";
                    oWhere = "PoliticalConnection_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "PoliticalConnection_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "PoliticalConnection_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "PoliticalConnection_DotExID";
                }
                else if (UpdateBy == "MaritalStatus")
                {
                    oTableName = "tbl_master_maritalstatus";
                    oWhere = "mts_id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "mts_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "mts_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "mts_DotExID";
                }
                else if (UpdateBy == "State")
                {
                    oTableName = "tbl_master_state";
                    oWhere = "id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "State_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "State_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "State_DotExID";
                }
                else if (UpdateBy == "Country")
                {
                    oTableName = "tbl_master_country";
                    oWhere = "cou_id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "Country_CvlID";
                    else if (selCmbKra == 2)
                        oFieldName = "Country_NdmlID";
                    else if (selCmbKra == 3)
                        oFieldName = "Country_DotExID";
                }
                else if (UpdateBy == "ContactType")
                {
                    oTableName = "tbl_master_contacttype";
                    oWhere = "cnttpy_id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "ContactType_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "ContactType_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "ContactType_DotExID";
                }
                else if (UpdateBy == "KRANationality")
                {
                    oTableName = "Master_Nationality";
                    oWhere = "Nationality_id='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "Nationality_CVLid";
                    else if (selCmbKra == 2)
                        oFieldName = "Nationality_NDMLid";
                    else if (selCmbKra == 3)
                        oFieldName = "Nationality_DotExid";
                }
                else if (UpdateBy == "KRAAnnualIncome")
                {
                    oTableName = "Master_AnnualIncome";
                    oWhere = "AnnualIncome_ID='" + MasterMapID + "'";

                    if (selCmbKra == 1)
                        oFieldName = "AnnualIncome_CVLID";
                    else if (selCmbKra == 2)
                        oFieldName = "AnnualIncome_NDMLId";
                    else if (selCmbKra == 3)
                        oFieldName = "AnnualIncome_DotExID";
                }
                dtUpdateResult = UpdateRecord(oTableName, oFieldName, oValue, oWhere);
                CbpKRAContainer.JSProperties["cpUpdate"] = UpdateBy + "~Successfully Update";
            }
            #endregion
        }

        protected DataTable UpdateRecord(string tableName, string fieldName, int? fieldValue, string where)
        {
            DataTable dtResult = null;
            Gm = new BusinessLogicLayer.GenericMethod();
            if (fieldValue != null)
            {
                dtResult = Gm.CallGeneric_StoreProcedure("UpdateTableWhere", "Update~" + tableName + "~" + fieldName + "~'" + fieldValue + "'~" + where + "");
            }
            return dtResult;
        }
    }
}