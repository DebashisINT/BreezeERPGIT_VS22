using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxClasses;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web.ASPxGridView;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using System.Configuration;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;

namespace ERP.OMS.Managemnent.Master
{
    public partial class management_Master_TaxSchemeAddEdit : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.TaxSchemeBl oTaxSchemeBl = new TaxSchemeBl();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        CommonBL cbl = new CommonBL();
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
            //   bindLookUP();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string SystemTaxComponentScheme = cbl.GetSystemSettingsResult("SystemTaxComponentScheme");
            if (!String.IsNullOrEmpty(SystemTaxComponentScheme))
            {
                if (SystemTaxComponentScheme == "Yes")
                {
                    hdnSystemTaxComponentScheme.Value = "1";                   

                }
                else if (SystemTaxComponentScheme.ToUpper().Trim() == "NO")
                {
                    hdnSystemTaxComponentScheme.Value = "0";
                    
                }
            }
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
                txtTaxRates_DateTo.Value = DateTime.Now;
                hfRoundOffList.Value = GetTaxComponentListRoundingOff();
                Session["exportval"] = null;

                if (Request.QueryString["id"] != null)
                {
                    hdStatus.Value = Request.QueryString["id"];
                    if (Convert.ToString(hdStatus.Value) != "ADD")
                    {
                        setUpdateData(Convert.ToString(hdStatus.Value));
                        bindLookUP(Request.QueryString["id"]);
                        bindLedgerLookUP(Convert.ToString(hdStatus.Value));
                        CmbTaxRates_TaxCode.ClientEnabled = false;
                        if (oTaxSchemeBl.ChekTaxSchemeinUse(Convert.ToInt32(Request.QueryString["id"])) == 1)
                        {
                            HdSchemeInUse.Value = "Yes";
                        }
                        else
                        {
                            HdSchemeInUse.Value = "No";
                        }
                    }
                    else
                    {
                        txtTaxRates_DateTo.Date = DateTime.Today;
                        txtTaxRates_DateFrom.Date = DateTime.Today;
                        HdSchemeInUse.Value = "No";
                        //  bindLookUP(null);
                    }
                }




            }



        }

        public void bindLookUP(string code)
        {
            DataTable productTable;


            string[] taxconfigDetails = oDBEngine.GetFieldValue1("Config_TaxRates", "TaxRates_TaxCode,TaxRatesSchemeName", "TaxRates_ID=" + code, 2);
            //productTable = oDBEngine.GetDataTable("select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts where sProducts_ID not in (select prodId from tbl_trans_ProductTaxRate where TaxRates_TaxCode<>" + taxconfigDetails[0] + " and TaxRatesSchemeName<>'" + taxconfigDetails[1] + "' )");
            //GridLookup.DataSource = productTable;
            //GridLookup.DataBind();
            productTable = oTaxSchemeBl.GetProductListForScheme(Convert.ToInt32(code));
            GridLookup.GridView.Selection.CancelSelection();
            GridLookup.DataSource = productTable;
            Session["ProductTaxData"] = productTable;
            GridLookup.DataBind();
           // updateGridLookUP(taxconfigDetails[0], taxconfigDetails[1]);
            updateGridLookUPbyHSN(code);


            
        }


        public void bindLedgerLookUP(string TaxCofingID)
        {
            DataTable ledgerTable;

            //string[] taxconfigDetails = oDBEngine.GetFieldValue1("Config_TaxRates", "TaxRates_TaxCode,TaxRatesSchemeName", "TaxRates_ID=" + TaxCofingID, 2);
            //productTable = oDBEngine.GetDataTable("select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts where sProducts_ID not in (select prodId from tbl_trans_ProductTaxRate where TaxRates_TaxCode<>" + taxconfigDetails[0] + " and TaxRatesSchemeName<>'" + taxconfigDetails[1] + "' )");
            //GridLookup.DataSource = productTable;
            //GridLookup.DataBind();
            ledgerTable = oTaxSchemeBl.GetLedgerForScheme(0);
            GridLookUpLedger.GridView.Selection.CancelSelection();
            GridLookUpLedger.DataSource = ledgerTable;
            GridLookUpLedger.DataBind();
            UpdateGridLookUPLedger(TaxCofingID);


            Session["LedgerTaxData"] = ledgerTable;
        }

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string taxId = e.Parameter;
            #region ###### Product ####

            if (e.Parameter.Split('~')[0] == "selectAll")
            {
                if (e.Parameter.Split('~')[1] == "true")
                {
                    for (int i = 0; i < GridLookup.GridView.VisibleRowCount; i++)
                    {
                        GridLookup.GridView.Selection.SelectRow(i);
                    }
                }
                else
                {
                    for (int i = 0; i < GridLookup.GridView.VisibleRowCount; i++)
                    {
                        GridLookup.GridView.Selection.UnselectRow(i);
                    }
                }
            }
            else
            {
                DataTable newRecordDt = new DataTable();
                newRecordDt = oTaxSchemeBl.GetProductList(Convert.ToInt32(taxId));
                GridLookup.GridView.Selection.CancelSelection();
                GridLookup.DataSource = newRecordDt;
                Session["ProductTaxData"] = newRecordDt;
                GridLookup.DataBind();
               
            }

            #endregion

            #region #### Ledger ####

            if (e.Parameter.Split('~')[0] == "selectAll")
            {
                if (e.Parameter.Split('~')[1] == "true")
                {
                    for (int i = 0; i < GridLookUpLedger.GridView.VisibleRowCount; i++)
                    {
                        GridLookUpLedger.GridView.Selection.SelectRow(i);
                    }
                }
                else
                {
                    for (int i = 0; i < GridLookUpLedger.GridView.VisibleRowCount; i++)
                    {
                        GridLookUpLedger.GridView.Selection.UnselectRow(i);
                    }
                }
            }
            else
            {
                DataTable newRecordDt = new DataTable();
                newRecordDt = oTaxSchemeBl.GetLedgerForScheme(Convert.ToInt32(taxId));
                GridLookUpLedger.GridView.Selection.CancelSelection();
                GridLookUpLedger.DataSource = newRecordDt;
                GridLookUpLedger.DataBind();
                Session["LedgerTaxData"] = newRecordDt;
            }

            #endregion
        }

        //protected void LedgerComponent_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string taxId = e.Parameter;
        //    #region #### Ledger ####

        //    if (e.Parameter.Split('~')[0] == "selectAll")
        //    {
        //        if (e.Parameter.Split('~')[1] == "true")
        //        {
        //            for (int i = 0; i < GridLookUpLedger.GridView.VisibleRowCount; i++)
        //            {
        //                GridLookUpLedger.GridView.Selection.SelectRow(i);
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < GridLookUpLedger.GridView.VisibleRowCount; i++)
        //            {
        //                GridLookUpLedger.GridView.Selection.UnselectRow(i);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        DataTable newRecordDt = new DataTable();
        //        newRecordDt = oTaxSchemeBl.GetLedgerForScheme(Convert.ToInt32(taxId));
        //        GridLookUpLedger.GridView.Selection.CancelSelection();
        //        GridLookUpLedger.DataSource = newRecordDt;
        //        GridLookUpLedger.DataBind();
        //        Session["LedgerTaxData"] = newRecordDt;
        //    }

        //    #endregion
        //}

        public void UpdateGridLookUPLedger(string taxConfigID)
        {
            DataTable exsistingtable = oDBEngine.GetDataTable("select MainAccount_ReferenceID from tbl_trans_LedgerTaxRate where TaxRates_ID=" + taxConfigID);

            // foreach (object Pobj in GridLookup.GridView.GetSelectedFieldValues("sProducts_ID"))
            //{
            //    DataRow[] checkRecord = exsistingtable.Select("prodId=" + Pobj);
            //    if (checkRecord.Length > 0)
            //    {
            //        GridLookup.GridView.Selection.SelectRowByKey(Pobj);
            //    }
            //}

            foreach (DataRow dr in exsistingtable.Rows)
            {
                GridLookUpLedger.GridView.Selection.SelectRowByKey(dr["MainAccount_ReferenceID"]);
            }

        }


        public void updateGridLookUPbyHSN(string taxRatesId )
        {
            DataTable exsistingtable = oDBEngine.GetDataTable("select HsnCode  from tbl_trans_HSNTaxrate where TaxRates_ID ='"+taxRatesId.Trim() +"'");

           
            foreach (DataRow dr in exsistingtable.Rows)
            {
                GridLookup.GridView.Selection.SelectRowByKey(dr["HsnCode"]);
            }

        }

        public void updateGridLookUP(string code, string name)
        {
            DataTable exsistingtable = oDBEngine.GetDataTable("select prodId from tbl_trans_ProductTaxRate where TaxRates_TaxCode=" + code + " and TaxRatesSchemeName='" + name + "'");

            // foreach (object Pobj in GridLookup.GridView.GetSelectedFieldValues("sProducts_ID"))
            //{
            //    DataRow[] checkRecord = exsistingtable.Select("prodId=" + Pobj);
            //    if (checkRecord.Length > 0)
            //    {
            //        GridLookup.GridView.Selection.SelectRowByKey(Pobj);
            //    }
            //}

            foreach (DataRow dr in exsistingtable.Rows)
            {
                GridLookup.GridView.Selection.SelectRowByKey(dr["prodId"]);
            }

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProductTaxData"] != null)
            {
                GridLookup.DataSource = (DataTable)Session["ProductTaxData"];
            }


        }

        protected void gridLookupLedger_DataBinding(object sender, EventArgs e)
        {
            if (Session["LedgerTaxData"] != null)
            {
                GridLookUpLedger.DataSource = (DataTable)Session["LedgerTaxData"];
            }


        }

        protected void btnSaveTaxScheme_Click(object sender, EventArgs e)
        {
            DataTable tbl_IDList = new DataTable();
            tbl_IDList.Columns.Add("ID", typeof(string));

            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            int cityID = 0;
            string strSlabCode = "0";
            int insertcount = 0;
            int updtcnt = 0;
            if (Convert.ToString(hdStatus.Value) == "ADD")
            {
                int TaxRates_TaxCode = Convert.ToInt32(Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value));

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

                string IsCess = "0";
                if (chkCompensation.Checked)
                {
                    IsCess = "1";
                }
                //-----------End

                string RoundingOff = string.Empty;
                try
                {
                    if (hfRoundOffCheck.Value == "1")
                    {
                        RoundingOff = ddlRoundingOff.SelectedValue;
                    }
                }
                catch (Exception ex)
                {
                    RoundingOff = string.Empty;
                }

                if (TaxRates_TaxCode != 0)
                {

                    string sTaxRates_MainAccount = Convert.ToString(hndTaxRates_MainAccount_hidden.Value).Trim();
                    string sTaxRates_ReverseChargeMainAccount = Convert.ToString(hndTaxRates_reverseChargeMainAccount_hidden.Value).Trim();
                    string sTaxRates_SubAccount = "NULL";
                    if (hndTaxRates_SubAccount_hidden.Value != null && hndTaxRates_SubAccount_hidden.Value != "")
                    {
                        sTaxRates_SubAccount = "'" + Convert.ToString(hndTaxRates_SubAccount_hidden.Value).Trim() + "'";
                    }

                    string TaxRates_MinAmount = txtTaxRates_MinAmount.Text == "" ? "0.0" : txtTaxRates_MinAmount.Text;

                    insertcount = oGenericMethod.Insert_Table("Config_TaxRates", "TaxRates_TaxCode,TaxRates_ProductClass,TaxRates_Country,TaxRates_State,TaxRates_City,TaxRates_DateFrom,TaxRates_RateOrSlab,TaxRates_Rate,TaxRates_MinAmount,TaxRates_SlabCode,TaxRates_SurchargeApplicable,TaxRates_SurchargeCriteria,TaxRates_SurchargeAbove,TaxRates_SurchargeOn,[Taxes-SurchargeRate],TaxRates_CreateUser,TaxRates_CreateTime,TaxRates_MainAccount,TaxRates_SubAccount,TaxRatesSchemeName,Exempted,TaxRates_Sequence,TaxRates_DateTo,TaxRates_GSTtype,TaxRates_IsCess,TaxRates_RoundingOff,TaxRates_ReverseChargeMainAccount",
                            "'" + Convert.ToString(CmbTaxRates_TaxCode.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbState.SelectedItem.Value).Trim() + "','" + Convert.ToString(cityID).Trim() +
                            "','" + Convert.ToString(txtTaxRates_DateFrom.Value).Trim() + "','" + Convert.ToString(CmbTaxRates_RateOrSlab.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxRates_Rate.Text).Trim() + "','"
                            + Convert.ToDecimal(TaxRates_MinAmount) + "','"
                            + Convert.ToString(strSlabCode).Trim() + "','" + Convert.ToString(CmbTaxRates_SurchargeApplicable.SelectedItem.Value).Trim() + "','" + Convert.ToString(CmbtxtTaxRates_SurchargeCriteria.SelectedItem.Value).Trim() + "','" + Convert.ToString(txtTaxRates_SurchargeAbove.Text).Trim() + "','" + Convert.ToString(CmbTaxRates_SurchargeOn.Value == "0" ? 0 : CmbTaxRates_SurchargeOn.Value).Trim() + "','" + Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "',getdate(),'" + sTaxRates_MainAccount.Trim() + "'," + sTaxRates_SubAccount.Trim() + ",'" + txt_schemename.Text + "'," + rdpExempted.SelectedItem.Value + "," + txtSequenceNo.Text.Trim() +
                           ",'" + Convert.ToString(txtTaxRates_DateTo.Value).Trim() + "','" + Convert.ToString(cmbGSTType.Value) + "'," + IsCess + ",'" + RoundingOff + "','" + sTaxRates_ReverseChargeMainAccount.Trim() + "'");

                    string listOfProduct = "";
                    List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("Code");
                    foreach (object Pobj in ComponentList)
                    {
                       
                        listOfProduct += "," + Pobj;
                        tbl_IDList.Rows.Add(Convert.ToString(Pobj));
                    }
                    listOfProduct = listOfProduct.TrimStart(',');

                    //string HsnCodeList = "";
                    //List<object> ComponentListHSN = GridLookup.GridView.GetSelectedFieldValues("sProducts_HsnCode");
                    //List<object> ComponentListSAC = GridLookup.GridView.GetSelectedFieldValues("sProducts_serviceTaxCode");
                    //foreach (object obj in ComponentListHSN)
                    //{
                    //    if (!HsnCodeList.Contains(obj.ToString()))
                    //        HsnCodeList += "," + obj;
                    //}
                    //foreach (object obj in ComponentListSAC)
                    //{
                    //    if (obj != null && obj.ToString() != "" && !HsnCodeList.Contains(obj.ToString()))
                    //        HsnCodeList += "," + obj;
                    //}
                    //HsnCodeList = HsnCodeList.TrimStart(',');
                    int TaxRates_ID = oTaxSchemeBl.GetTaxRates_ID(TaxRates_TaxCode, txt_schemename.Text);
                    if (listOfProduct.Trim() != "")
                    {
                        oTaxSchemeBl.InsertProdTaxWise(Convert.ToInt32(CmbTaxRates_TaxCode.SelectedItem.Value), txt_schemename.Text, listOfProduct, tbl_IDList, TaxRates_ID);
                    }
                    //if (HsnCodeList.Trim() != "")
                    //{
                    //    int TaxRates_ID = oTaxSchemeBl.GetTaxRates_ID(TaxRates_TaxCode, txt_schemename.Text);

                    //    if (TaxRates_ID > 0)
                    //    {
                    //        oTaxSchemeBl.InsertProductWiseHSN(TaxRates_TaxCode, txt_schemename.Text, HsnCodeList, TaxRates_ID);


                            #region ####### Ledger Mapping ######

                            ////// Ledger Adding 
                            string listOfLedger = "";
                            List<object> LedgerComponentList = GridLookUpLedger.GridView.GetSelectedFieldValues("MainAccount_ReferenceID");
                            foreach (object Pobj in LedgerComponentList)
                            {
                                listOfLedger += "," + Pobj;
                            }
                            listOfLedger = listOfLedger.TrimStart(',');

                            if (listOfLedger.Trim() != "")
                            {
                                oTaxSchemeBl.InsertLedgerTaxWise(Convert.ToInt64(TaxRates_ID), listOfLedger);
                            }

                            ///// HSN Mapping
                            string HsnSacCodeList = "";
                            List<object> ComponentListHSNSAC = GridLookUpLedger.GridView.GetSelectedFieldValues("HSNSACCode");

                            foreach (object obj in ComponentListHSNSAC)
                            {
                                if (!HsnSacCodeList.Contains(obj.ToString()))
                                    HsnSacCodeList += "," + obj;
                            }

                            HsnSacCodeList = HsnSacCodeList.TrimStart(',');

                            oTaxSchemeBl.InsertHSNSACMappedWithLedger(TaxRates_TaxCode, txt_schemename.Text, HsnSacCodeList, TaxRates_ID);

                            #endregion

                    //    }

                    //}

                    Response.Redirect("/OMS/management/store/Settings_Options/Config_TaxLevies.aspx");
                }
            }
            else
            {

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

                if (Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() == "")
                {
                    txtTaxes_SurchargeRate.Text = "0";
                }

                if (txtTaxRates_SurchargeAbove.Text.Trim() == "")
                    txtTaxRates_SurchargeAbove.Text = "0";

                string sTaxRates_MainAccount = Convert.ToString(hndTaxRates_MainAccount_hidden.Value).Trim();
                string sTaxRates_ReverseChargeMainAccount = Convert.ToString(hndTaxRates_reverseChargeMainAccount_hidden.Value).Trim();
                string sTaxRates_SubAccount = "NULL";
                if (hndTaxRates_SubAccount_hidden.Value != null && hndTaxRates_SubAccount_hidden.Value != "")
                {
                    sTaxRates_SubAccount = Convert.ToString(hndTaxRates_SubAccount_hidden.Value).Trim();
                }
                string minamt = Convert.ToString(txtTaxRates_MinAmount.Text).Trim();
                if (minamt == "")
                    minamt = "0";
                //if (stateID != 0)  //comment by sanjib due to logic changed.
                //{
                string isCess = "0";
                if (chkCompensation.Checked)
                {
                    isCess = "1";
                }

                if (txtTaxRates_DateFrom.Value != null && txtTaxRates_DateTo.Value != null)
                {
                    /*Code  Added  By Sudip on 14122016 for Edit function*/

                    updtcnt = oGenericMethod.Update_Table("Config_TaxRates", "TaxRates_MainAccount='" + sTaxRates_MainAccount + "',TaxRates_SubAccount=" + sTaxRates_SubAccount + ",TaxRates_ReverseChargeMainAccount='" + sTaxRates_ReverseChargeMainAccount + "',Exempted=" + rdpExempted.SelectedItem.Value + ",TaxRatesSchemeName='" + txt_schemename.Text + "',TaxRates_TaxCode ='" + Convert.ToString(hdComponentName.Value).Trim() + "', TaxRates_ProductClass ='" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value) + "', TaxRates_DateFrom ='" + Convert.ToString(txtTaxRates_DateFrom.Value).Trim() + "', TaxRates_RateOrSlab  ='" + Convert.ToString(CmbTaxRates_RateOrSlab.SelectedItem.Value).Trim() + "', TaxRates_Rate  ='" + Convert.ToDecimal(Convert.ToString(txtTaxRates_Rate.Text)) + "', TaxRates_MinAmount  ='" + minamt + "', TaxRates_SlabCode  ='" + Convert.ToString(strSlabCode).Trim() + "', TaxRates_SurchargeApplicable ='" + Convert.ToString(CmbTaxRates_SurchargeApplicable.SelectedItem.Value).Trim() + "', TaxRates_SurchargeCriteria  ='" + Convert.ToString(strSurchargeCriteria).Trim() + "', TaxRates_SurchargeAbove  ='" + Convert.ToString(txtTaxRates_SurchargeAbove.Text).Trim() + "', TaxRates_SurchargeOn  ='" + Convert.ToString(CmbTaxRates_SurchargeOn.Value == "0" ? 0 : CmbTaxRates_SurchargeOn.Value).Trim() + "', [Taxes-SurchargeRate]  ='" + Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() + "', TaxRates_State ='" + Convert.ToString(CmbState.SelectedItem == null ? 0 : CmbState.SelectedItem.Value).Trim() + "', TaxRates_City ='" + Convert.ToString(cityID).Trim() + "', TaxRates_Country ='" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() + "',TaxRates_ModifyUser='" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "', TaxRates_ModifyTime=getdate(),TaxRates_Sequence=" + txtSequenceNo.Text.Trim() + ", TaxRates_DateTo ='" + Convert.ToString(txtTaxRates_DateTo.Value).Trim() + "',TaxRates_GSTtype='" + Convert.ToString(cmbGSTType.Value) + "',TaxRates_IsCess=" + isCess, "TaxRates_ID=" + Convert.ToString(hdStatus.Value));

                }

                //............Code Commented By 17-07-2017.................Priti
                //if (txtTaxRates_DateTo.Value != null)  // date from is not required in update statement due to non-editable field
                //{
                //    /*Code  Added  By Sudip on 14122016 for Edit function*/

                //    updtcnt = oGenericMethod.Update_Table("Config_TaxRates", "TaxRates_MainAccount='" + sTaxRates_MainAccount + "',TaxRates_SubAccount=" + sTaxRates_SubAccount + ",TaxRates_ReverseChargeMainAccount='" + sTaxRates_ReverseChargeMainAccount + "',Exempted=" + rdpExempted.SelectedItem.Value + ",TaxRatesSchemeName='" + txt_schemename.Text + "',TaxRates_TaxCode ='" + Convert.ToString(hdComponentName.Value).Trim() + "', TaxRates_ProductClass ='" + Convert.ToString(CmbTaxRates_ProductClass.SelectedItem.Value) + "', TaxRates_RateOrSlab  ='" + Convert.ToString(CmbTaxRates_RateOrSlab.SelectedItem.Value).Trim() + "', TaxRates_Rate  ='" + Convert.ToDecimal(Convert.ToString(txtTaxRates_Rate.Text)) + "', TaxRates_MinAmount  ='" + minamt + "', TaxRates_SlabCode  ='" + Convert.ToString(strSlabCode).Trim() + "', TaxRates_SurchargeApplicable ='" + Convert.ToString(CmbTaxRates_SurchargeApplicable.SelectedItem.Value).Trim() + "', TaxRates_SurchargeCriteria  ='" + Convert.ToString(strSurchargeCriteria).Trim() + "', TaxRates_SurchargeAbove  ='" + Convert.ToString(txtTaxRates_SurchargeAbove.Text).Trim() + "', TaxRates_SurchargeOn  ='" + Convert.ToString(CmbTaxRates_SurchargeOn.Value == "0" ? 0 : CmbTaxRates_SurchargeOn.Value).Trim() + "', [Taxes-SurchargeRate]  ='" + Convert.ToString(txtTaxes_SurchargeRate.Text).Trim() + "', TaxRates_State ='" + Convert.ToString(CmbState.SelectedItem == null ? 0 : CmbState.SelectedItem.Value).Trim() + "', TaxRates_City ='" + Convert.ToString(cityID).Trim() + "', TaxRates_Country ='" + Convert.ToString(CmbCountryName.SelectedItem.Value).Trim() + "',TaxRates_ModifyUser='" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "', TaxRates_ModifyTime=getdate(),TaxRates_Sequence=" + txtSequenceNo.Text.Trim() + ", TaxRates_DateTo ='" + Convert.ToString(txtTaxRates_DateTo.Value).Trim() + "',TaxRates_GSTtype='" + Convert.ToString(cmbGSTType.Value) + "',TaxRates_IsCess=" + isCess, "TaxRates_ID=" + Convert.ToString(hdStatus.Value));

                //}

                string listOfProduct = "";
                List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("Code");
                foreach (object Pobj in ComponentList)
                {
                    listOfProduct += "," + Pobj;
                    tbl_IDList.Rows.Add(Convert.ToString(Pobj));
                }
                listOfProduct = listOfProduct.TrimStart(',');

                string HsnCodeList = "";
                //List<object> ComponentListHSN = GridLookup.GridView.GetSelectedFieldValues("sProducts_HsnCode");
                //List<object> ComponentListSAC = GridLookup.GridView.GetSelectedFieldValues("sProducts_serviceTaxCode");
                //foreach (object obj in ComponentListHSN)
                //{
                //    if (!HsnCodeList.Contains(obj.ToString()))
                //        HsnCodeList += "," + obj;
                //}
                //foreach (object obj in ComponentListSAC)
                //{
                //    if (obj != null && obj.ToString() != "" && !HsnCodeList.Contains(obj.ToString()))
                //        HsnCodeList += "," + obj;
                //}
                //HsnCodeList = HsnCodeList.TrimStart(',');
                int TaxRates_ID = oTaxSchemeBl.GetTaxRates_ID(Convert.ToInt32(hdComponentName.Value), txt_schemename.Text);
                oTaxSchemeBl.UpdateProdTaxWise(Convert.ToInt32(hdComponentName.Value), txt_schemename.Text, listOfProduct, tbl_IDList, TaxRates_ID);

                

                if (TaxRates_ID > 0)
                {
                   // oTaxSchemeBl.InsertProductWiseHSN(Convert.ToInt32(hdComponentName.Value), txt_schemename.Text, HsnCodeList, TaxRates_ID);

                    #region ####### Ledger Mapping ######

                    ////// Ledger Adding 
                    string listOfLedger = "";
                    List<object> LedgerComponentList = GridLookUpLedger.GridView.GetSelectedFieldValues("MainAccount_ReferenceID");
                    foreach (object Pobj in LedgerComponentList)
                    {
                        listOfLedger += "," + Pobj;
                    }
                    listOfLedger = listOfLedger.TrimStart(',');

                    oTaxSchemeBl.UpdateLedgerTaxWise(Convert.ToInt64(TaxRates_ID), listOfLedger);

                    ///// HSN Mapping
                    string HsnSacCodeList = "";
                    List<object> ComponentListHSNSAC = GridLookUpLedger.GridView.GetSelectedFieldValues("HSNSACCode");

                    foreach (object obj in ComponentListHSNSAC)
                    {
                        if (!HsnSacCodeList.Contains(obj.ToString()))
                            HsnSacCodeList += "," + obj;
                    }

                    HsnSacCodeList = HsnSacCodeList.TrimStart(',');

                    oTaxSchemeBl.InsertHSNSACMappedWithLedger(Convert.ToInt32(hdComponentName.Value), txt_schemename.Text, HsnSacCodeList, TaxRates_ID);

                    #endregion
                }


                Response.Redirect("/OMS/management/store/Settings_Options/Config_TaxLevies.aspx");


            }
            setUpdateData(Convert.ToString(hdStatus.Value));
            oTaxSchemeBl.GetSchemeMaxupdateDate();
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
            //dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Name + case Taxes_ApplicableFor when 'B' then ' (Both)' when 'S' then ' (Sale)' Else ' (Purchase)' end as name from Master_Taxes order By Taxes_Code");
            dtCmb = oGenericMethod.GetDataTable("select Taxes_ID as id,Taxes_Name + case Taxes_ApplicableFor when 'B' then ' (Both)' when 'S' then ' (Sale)' Else ' (Purchase)' end + case TaxTypeCode when 'G' then ' (GST)' when 'C' then ' (CST)' when 'V' then ' (VAT)' when 'O' then ' (Others)' when 'IGST' then '(IGST)' when 'CGST' then '(CGST)' when 'SGST' then '(SGST)' when 'UTGST' then '(UTGST)' end as name from Master_Taxes order By Taxes_Code");
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

        public void setUpdateData(string id)
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
                                                          ,isnull([TaxRates_DateTo],getdate())[TaxRates_DateTo]
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
                                                          ,[TaxRates_ReverseChargeMainAccount]
                                                          ,[TaxRates_SubAccount]
                                                          ,[TaxRates_CreateUser]
                                                          ,[TaxRates_CreateTime]
                                                          ,[TaxRates_ModifyUser]
                                                          ,[TaxRates_ModifyTime]
                                                          ,TaxRatesSchemeName
                                                            ,case when Exempted=1 then 0 else 1 end as Exempted
                                                          ,TaxRates_Sequence,isnull(TaxRates_GSTtype,'1')  TaxRates_GSTtype,
                                                          ISNULL(TaxRates_IsCess,0)TaxRates_IsCess
                                                          ,mt.TaxTypeCode
                                                          ,mt.Taxes_ApplicableOn
                                                          ,ctr.TaxRates_RoundingOff
                                                      FROM [dbo].[Config_TaxRates] ctr
                                                      
                                                         left JOIN tbl_master_country tmcoun ON ctr.TaxRates_Country = tmcoun.cou_id 
                                                         left join tbl_master_state tms on  ctr.TaxRates_State=tms.id
                                                         left join tbl_master_city tmcity on ctr.TaxRates_City=tmcity.city_id 
                                                         left join Master_Taxes mt on ctr.TaxRates_TaxCode=mt.Taxes_ID 
                                                         left join Master_ProductClass mpc on ctr.TaxRates_ProductClass=mpc.ProductClass_ID  
                                                      Where TaxRates_ID=" + id + "");

            if (dtEdit.Rows.Count > 0 && dtEdit != null)
            {
                string TaxRates_TaxCode = Convert.ToString(dtEdit.Rows[0]["TaxRates_TaxCode"]);
                string TaxRates_ProductClass = Convert.ToString(dtEdit.Rows[0]["TaxRates_ProductClass"]);
                string TaxRates_DateFrom = Convert.ToDateTime(Convert.ToString(dtEdit.Rows[0]["TaxRates_DateFrom"])).Date.ToString("yyyy-MM-dd");
                string TaxRates_DateTo = Convert.ToDateTime(Convert.ToString(dtEdit.Rows[0]["TaxRates_DateTo"])).Date.ToString("yyyy-MM-dd");
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
                string TaxRates_ReverseChargeMainAccount = Convert.ToString(dtEdit.Rows[0]["TaxRates_ReverseChargeMainAccount"]);
                string TaxRates_SubAccount = Convert.ToString(dtEdit.Rows[0]["TaxRates_SubAccount"]);

                string TaxRates_State = Convert.ToString(dtEdit.Rows[0]["TaxRates_State"]);
                string TaxRates_City = Convert.ToString(dtEdit.Rows[0]["TaxRates_City"]);
                string TaxRates_Country = Convert.ToString(dtEdit.Rows[0]["TaxRates_Country"]);
                string TaxRatesSchemeName = Convert.ToString(dtEdit.Rows[0]["TaxRatesSchemeName"]);
                string Exempted = Convert.ToString(dtEdit.Rows[0]["Exempted"]);
                string TaxRates_Sequence = Convert.ToString(dtEdit.Rows[0]["TaxRates_Sequence"]);
                string TaxRates_GSTtype = Convert.ToString(dtEdit.Rows[0]["TaxRates_GSTtype"]);
                // string TaxRates_DateTo = Convert.ToDateTime(Convert.ToString(dtEdit.Rows[0]["TaxRates_DateTo"])).Date.ToString("dd-MM-yyyy");
                //  string TaxRates_TaxSlab_Code = dtEdit.Rows[0]["TaxRates_TaxSlab_Code"].ToString();

                BindCity(Convert.ToInt32(Convert.ToString(TaxRates_State)));
                BindCmbTaxSlab_Code();
                lstTaxRates_MainAccount.SelectedValue = Convert.ToString(dtEdit.Rows[0]["TaxRates_MainAccount"]);
                lstTaxRates_reverseChargeMainAccount.SelectedValue = Convert.ToString(dtEdit.Rows[0]["TaxRates_ReverseChargeMainAccount"]);
                lstTaxRates_SubAccount.SelectedValue = Convert.ToString(dtEdit.Rows[0]["TaxRates_SubAccount"]);

                string TaxRates_IsCess = Convert.ToString(dtEdit.Rows[0]["TaxRates_IsCess"]);
                string TaxCodeType = Convert.ToString(dtEdit.Rows[0]["TaxTypeCode"]);
                string TaxApplicableOn = Convert.ToString(dtEdit.Rows[0]["Taxes_ApplicableOn"]);
                string TaxRates_RoundingOff = Convert.ToString(dtEdit.Rows[0]["TaxRates_RoundingOff"]);
                hftaxCodeName.Value = TaxRatesSchemeName;

                //CmbTaxRates_TaxCode.Value = TaxRates_TaxCode;
                //CmbTaxRates_ProductClass.Value = TaxRates_ProductClass;
                //txtTaxRates_DateFrom.Date = Convert.ToDateTime(TaxRates_DateFrom);


                hdEditedValue.Value = TaxRates_TaxCode + "~" + TaxRates_ProductClass + "~" + TaxRates_DateFrom + "~"
                    + TaxRates_DateTo + "~" + TaxRates_RateOrSlab + "~" + TaxRates_Rate + "~" + TaxRates_MinAmount + "~" + TaxRates_SlabCode + "~"
                    + TaxRates_SurchargeApplicable + "~" + TaxRates_SurchargeCriteria + "~" + TaxRates_SurchargeAbove + "~" + TaxRates_SurchargeOn + "~" +
                   Taxes_SurchargeRate + "~" + TaxRates_State + "~" + TaxRates_City + "~" + TaxRates_Country + "~" + TaxRates_MainAccount + "~" + TaxRates_SubAccount
                   + "~" + id + "~" + TaxRatesSchemeName + "~" + Exempted + "~" + TaxRates_Sequence + "~" + TaxRates_GSTtype + "~" + TaxRates_IsCess + "~" +
                   TaxRates_ReverseChargeMainAccount + "~" + TaxCodeType.Trim() + "~" + TaxApplicableOn + "~" + TaxRates_RoundingOff;
            }

        }

        [WebMethod]
        public static List<string> GetMainAccountList(string reqStr, string SysTaxCompScheme)
        {
            
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");

            if (SysTaxCompScheme == "1")
            {
                DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", null);
            }
            else
            {
                DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_AccountCode not like 'SYS%'");
            }

        //    DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_AccountCode not like 'SYS%'");

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


        [WebMethod]
        public static bool CheckUniqueName(string ComponentId)
        {
            DataTable dt = new DataTable();
            bool IsPresent = true;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            if (ComponentId != "")
            {
                dt = oGeneric.GetDataTable("select 1 from Config_TaxRates  where TaxRatesSchemeName='" + ComponentId + "'");

                if (dt.Rows.Count > 0)
                {
                    IsPresent = false;

                }
            }


            return IsPresent;
        }

        private string GetTaxComponentListRoundingOff()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("SELECT mt.Taxes_ID,mt.Taxes_ApplicableFor,mt.Taxes_ApplicableOn,mt.TaxTypeCode,ISNULL(ct.TaxRates_RoundingOff,'')as TaxRates_RoundingOff FROM  [Config_TaxRates] ct right join [Master_Taxes] mt on mt.Taxes_ID=ct.TaxRates_TaxCode Where mt.TaxTypeCode = 'O' and mt.Taxes_ApplicableOn = 'A'");

            return ConvertDataTabletoJSON(dtCmb);

        }

        public string ConvertDataTabletoJSON(DataTable dt)
        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

    }
}
