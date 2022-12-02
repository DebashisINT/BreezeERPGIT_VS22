using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities.UserControls
{
    public partial class Import_BillingShippingControl : System.Web.UI.UserControl
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        CommonBL objCommonBL = new CommonBL();

        string shippingStateCode = string.Empty;
        static bool OurBSFlag = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string docID = Request.QueryString["key"];
                if (docID != "ADD" && !(string.IsNullOrEmpty(docID)))
                {
                    string docType = Request.QueryString["type"];
                    string[] listData = new string[] { "EditDetails", docID, docType };
                    //DataSet dsAddressDetails = objCommonBL.GetBillingShippingAddressDetails(docID, docType);
                    //HiddenField hfBillingStateID = (HiddenField)Page.FindControl("hfBTaxStateID");
                    //HiddenField hfShippingStateID = (HiddenField)Page.FindControl("hfSTaxStateID");


                    ////bsBCmbState.Value=ds
                    // AddressDetailsPopulate(listData);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearLookUpSession();

                AllPinCodesBind();

                CustomerBind();
            }
        }

        #region ######## CallBack Details ###########
        protected void bsComponentPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] listData = e.Parameter.Split('~');
                if (listData.Count() > 0) // Callback Parameter Present --- listData has values of ModeOfTrans, DocID, DocType
                {
                    #region ######## Comments Code 31/08/2017 ################
                    //DataTable dtCustomerDetails = new DataTable();
                    //if (listData[0] != "EditDetails")
                    //{
                    //    dtCustomerDetails = objCommonBL.GetCustomerNameByCustID(listData[1]);
                    //}
                    //else
                    //{
                    //    dtCustomerDetails = objCommonBL.GetCustomerNameByCustID(string.Empty, listData[1], listData[2]);
                    //}

                    //bsBHeaderText.Value = "Billing Address";
                    //bsSHeaderText.Value = "Shipping Address";
                    //if (dtCustomerDetails != null)
                    //{
                    //    if (dtCustomerDetails.Rows.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            if (listData[0] == "CustAddDetails" || listData[0] == "EditDetails")
                    //            {
                    //                bshfBHeaderText.Value = ("Billing Address") + " (" + Convert.ToString(dtCustomerDetails.Rows[0][0]) + ")";

                    //            }
                    //            bsBHeaderText.Value = bshfBHeaderText.Value;
                    //            bsSHeaderText.Value = "Shipping Address";
                    //            if (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)))
                    //            {
                    //                bsSHeaderText.Value = bshfBHeaderText.Value;
                    //            }
                    //            else
                    //            {
                    //                bsSHeaderText.Value = ("Shipping Address") + " (" + Convert.ToString(bsSCustomer.Text) + ")";
                    //            }
                    //        }
                    //        catch { }
                    //    }
                    //}



                    //if (!string.IsNullOrEmpty(listData[1]) && listData[0] != "EditDetails")
                    //{
                    //    if (listData[0] != "ShippingCustomer")
                    //    {
                    //        string branchID = string.Empty;
                    //        try { branchID = listData[2]; }
                    //        catch { };
                    //        if (listData[0] == "CustAddDetails")
                    //        {
                    //            Session.Remove("sessBillingAddress");
                    //            Session.Remove("sessShippingAddress");
                    //        }
                    //        BindLookupGrid(listData[1], false, branchID);
                    //    }
                    //}
                    #endregion

                       switch (listData[0])
                    {
                        case "EditDetails":
                            bsSChkShiipingToBilling.Checked = false;
                            bsBChkBillingToShipping.Checked = false;
                            SetEditAddressDetails(listData[1], listData[2]);
                            bsComponentPanel.JSProperties["cpDocType"] = listData[2];
                            bsComponentPanel.JSProperties["cpGlobalBillingShippingEndCallBack_Edit"] = "1";
                            break;
                        case "CustAddDetails":
                            bsSChkShiipingToBilling.Checked = false;
                            bsBChkBillingToShipping.Checked = false;
                            shippingAddress.Value = string.Empty;
                            billingAddress.Value = string.Empty;
                            bsSCustomer.Value = string.Empty;
                            SetNewAddressDetails(listData[1], listData[2], listData[3]);
                            bsComponentPanel.JSProperties["cpBSFocus"] = "CUSTLOAD";
                            bsComponentPanel.JSProperties["cpDocType"] = listData[3];
                            bsComponentPanel.JSProperties["cpGlobalBillingShippingEndCallBack_Edit"] = "0";
                            break;
                        case "DocTagging":
                            bsSChkShiipingToBilling.Checked = false;
                            bsBChkBillingToShipping.Checked = false;
                            SetEditAddressDetails(listData[1], listData[2]);
                            bsComponentPanel.JSProperties["cpDocType"] = listData[2];
                            break;

                        case "DocTaggingTPB":
                            bsSChkShiipingToBilling.Checked = false;
                            bsBChkBillingToShipping.Checked = false;
                            SetEditAddressDetails(listData[1], listData[2]);
                            bsComponentPanel.JSProperties["cpDocType"] = listData[2];
                            break;
                    }
                }

                //if (OurBSFlag == true)
                //{
                //    bsBHeaderText.Value = "Our Billing Address";
                //    bsSHeaderText.Value = "Our Shipping Address";
                //}
            }
            catch (Exception ex) { }

        }

        protected void BillingComponent_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] listData = e.Parameter.Split('~');
                if (listData.Count() > 0) // Callback Parameter Present --- listData has values of ModeOfTrans, DocID, DocType
                {
                    switch (listData[0])
                    {
                        #region ##### Billing Lookup ########
                        case "BillingLookup":
                            NewAddressDetailsPopulateForLookup(listData, listData[4]);
                            BillingComponent.JSProperties["cpBSBillingLookup"] = "1";
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void ShippingComponent_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] listData = e.Parameter.Split('~');
                if (listData.Count() > 0) // Callback Parameter Present --- listData has values of ModeOfTrans, DocID, DocType
                {
                    switch (listData[0])
                    {
                        #region ##### Shipping Lookup ########
                        case "ShippingLookup":
                            NewAddressDetailsPopulateForLookup(listData, listData[4]);
                            ShippingComponent.JSProperties["cpBSShippingLookup"] = "1";
                            break;
                        #endregion

                        #region ####### Ship to Party Lookup Fill #######
                        case "ShippingCustomer":
                            BindLookupGrid(Convert.ToString(listData[1]), true);
                            //CustomerAddressDetailsPopulate(Convert.ToString(listData[1]), "", "SO"); //// Doctype used SO becuase it always search customer address
                            bsSCustomer.Value = listData[1];
                            shippingAddress.Value = string.Empty;
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void BillingPinChangeComponent_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] listData = e.Parameter.Split('~');
                if (listData.Count() > 0) // Callback Parameter Present --- listData has values of ModeOfTrans, DocID, DocType
                {
                    switch (listData[0])
                    {
                        #region ##### PinChangeEvent #########
                        case "PinChangeEvent":
                            HiddenField ctl = (HiddenField)this.Parent.FindControl("hfTermsConditionDocType");
                            string docType = ctl.Value;
                            string pinId = listData[1];

                            DataSet dtDetails = NewDDLBinding(pinId, docType, listData[3], listData[4]);
                            switch (listData[2])
                            {
                                case "Billing":
                                    bsBTxtAddress1.Text = bsBTxtAddress1.Text;
                                    string pinValue = Convert.ToString(bsBCmbPin.Value);

                                    NewAreaBind(dtDetails.Tables[3], "Billing");

                                    if (dtDetails.Tables[5] != null && dtDetails.Tables[5].Rows.Count > 0)
                                    {
                                        bsBTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(dtDetails.Tables[5].Rows[0][0])) ? string.Empty : Convert.ToString(dtDetails.Tables[5].Rows[0][0]).Substring(0, 2);
                                        bsBTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(dtDetails.Tables[5].Rows[0][0])) ? string.Empty : Convert.ToString(dtDetails.Tables[5].Rows[0][0]).Substring(2, 10);
                                        bsBTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(dtDetails.Tables[5].Rows[0][0])) ? string.Empty : Convert.ToString(dtDetails.Tables[5].Rows[0][0]).Substring(12, 3);
                                    }

                                    bsBCmbCountry.Text = Convert.ToString(dtDetails.Tables[0].Rows[0]["cou_country"]);
                                    bsBCmbCountryHF.Value = Convert.ToString(dtDetails.Tables[0].Rows[0]["cou_id"]);

                                    bsBCmbState.Text = Convert.ToString(dtDetails.Tables[1].Rows[0]["state_name"]);
                                    bsBCmbStateHF.Value = Convert.ToString(dtDetails.Tables[1].Rows[0]["id"]);

                                    bsBCmbCity.Text = Convert.ToString(dtDetails.Tables[2].Rows[0]["city_name"]);
                                    bsBCmbCityHF.Value = Convert.ToString(dtDetails.Tables[2].Rows[0]["city_id"]);

                                    bsBCmbPin.Value = pinValue;
                                    bsBCmbArea.Value = "0";
                                    BillingPinChangeComponent.JSProperties["cpPinChange"] = "PinChangeEvent";
                                    break;
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void ShippingPinChangeComponent_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] listData = e.Parameter.Split('~');
                if (listData.Count() > 0) // Callback Parameter Present --- listData has values of ModeOfTrans, DocID, DocType
                {
                    switch (listData[0])
                    {
                        #region ##### PinChangeEvent #########
                        case "PinChangeEvent":
                            string pinId = listData[1];
                            HiddenField ctl = (HiddenField)this.Parent.FindControl("hfTermsConditionDocType");
                            string docType = ctl.Value;

                            DataSet dtDetails = NewDDLBinding(pinId, docType, (listData[3] == "undefined") ? "0" : listData[3], (listData[4] == "undefined") ? "0" : listData[4]);
                            switch (listData[2])
                            {
                                case "Shipping":
                                    bsSTxtAddress1.Text = bsSTxtAddress1.Text;
                                    string pinValue = Convert.ToString(bsSCmbPin.Value);

                                    NewAreaBind(dtDetails.Tables[3], "Shipping");

                                    if (dtDetails.Tables[5] != null && dtDetails.Tables[5].Rows.Count > 0)
                                    {
                                        bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(dtDetails.Tables[5].Rows[0][0])) ? string.Empty : Convert.ToString(dtDetails.Tables[5].Rows[0][0]).Substring(0, 2);
                                        bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(dtDetails.Tables[5].Rows[0][0])) ? string.Empty : Convert.ToString(dtDetails.Tables[5].Rows[0][0]).Substring(2, 10);
                                        bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(dtDetails.Tables[5].Rows[0][0])) ? string.Empty : Convert.ToString(dtDetails.Tables[5].Rows[0][0]).Substring(12, 3);
                                    }

                                    bsSCmbCountry.Text = Convert.ToString(dtDetails.Tables[0].Rows[0]["cou_country"]);
                                    bsSCmbCountryHF.Value = Convert.ToString(dtDetails.Tables[0].Rows[0]["cou_id"]);

                                    bsSCmbState.Text = Convert.ToString(dtDetails.Tables[1].Rows[0]["state_name"]);
                                    bsSCmbStateHF.Value = Convert.ToString(dtDetails.Tables[1].Rows[0]["id"]);
                                    ucBShfSStateCode.Value = Convert.ToString(dtDetails.Tables[1].Rows[0]["StateCode"]);

                                    bsSCmbCity.Text = Convert.ToString(dtDetails.Tables[2].Rows[0]["city_name"]);

                                    bsSCmbCityHF.Value = Convert.ToString(dtDetails.Tables[2].Rows[0]["city_id"]);

                                    bsSCmbPin.Value = pinValue;
                                    bsSCmbArea.Value = "0";
                                    ShippingPinChangeComponent.JSProperties["cpPinChange"] = "PinChangeEvent";
                                    break;
                            }
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void bsBCmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            PincodeBind(e.Parameter);
        }

        protected void bsSCmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                CommonBL objCommonBL = new CommonBL();

                DataSet dsState = objCommonBL.GetPinByCity(e.Parameter);

                bsSCmbPin.ValueField = "pin_id";
                bsSCmbPin.TextField = "pin_code";
                bsSCmbPin.DataSource = dsState.Tables[0];
                bsSCmbPin.DataBind();
            }
            bsSCmbPin.Items.Insert(0, new ListEditItem("", ""));

        }

        protected void bsBCmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            AreaBind(e.Parameter);
        }

        protected void bsSCmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                CommonBL objCommonBL = new CommonBL();

                DataSet dsState = objCommonBL.GetAreaByCity(e.Parameter);

                bsSCmbArea.ValueField = "area_id";
                bsSCmbArea.TextField = "area_name";
                bsSCmbArea.DataSource = dsState.Tables[0];
                bsSCmbArea.DataBind();
            }
            bsSCmbArea.Items.Insert(0, new ListEditItem("", ""));
        }

        #endregion

        #region ###Ship To Party Lookup Grid Binding #########
        private void BindLookupGrid(string customerID, bool shippingCustLookupFlag = false, string branchID = "", string shipToParty = "")
        {
            HiddenField ctl = (HiddenField)this.Parent.FindControl("hfTermsConditionDocType");
            string docType = ctl.Value;

            if (customerID == "null") { customerID = string.Empty; bsSCustomer.Value = customerID; }

            DataSet dsAllAddressDetails = objCommonBL.GetAllBillingShippingCustomerAddressDetails(customerID, branchID, docType);
            if (dsAllAddressDetails.Tables.Count > 1)  //// For Shipping Grid Lookup...
            {
                Session["sessShippingAddress"] = dsAllAddressDetails.Tables[1];
                shippingAddress.DataSource = dsAllAddressDetails.Tables[1];
                shippingAddress.DataBind();

                bsSTxtAddress1.Value = "";
                bsSTxtAddress2.Value = "";
                bsSTxtAddress3.Value = "";
                bsSTxtLandmark.Value = "";
                bsSCmbCountry.Value = "";
                bsSCmbState.Value = "";
                ucBShfSStateCode.Value = "";
                bsSCmbCity.Value = "";
                bsSCmbPin.Value = "";
                bsSCmbArea.Value = "";
                bsSTxtGSTIN1.Value = "";
                bsSTxtGSTIN2.Value = "";
                bsSTxtGSTIN3.Value = "";
                bsBChkBillingToShipping.Checked = false;
                bsSChkShiipingToBilling.Checked = false;

                AllPinCodesBind();

            }
        }
        #endregion

        #region #### Billing/Shipping Data Process ####
        /// Method Name : SaveBillingShippingControlData()
        /// This method has to call from the page where user need to get the billing/shipping control data
        /// <returns>DataTable</returns>
        public DataTable SaveBillingShippingControlData()
        {
            DataTable dtBillingShippingAddressData = new DataTable();

            dtBillingShippingAddressData.Columns.Add("AddressType", typeof(string));
            dtBillingShippingAddressData.Columns.Add("Address1", typeof(string));
            dtBillingShippingAddressData.Columns.Add("Address2", typeof(string));
            dtBillingShippingAddressData.Columns.Add("Address3", typeof(string));
            dtBillingShippingAddressData.Columns.Add("LandMark", typeof(string));
            dtBillingShippingAddressData.Columns.Add("CountryID", typeof(string));
            dtBillingShippingAddressData.Columns.Add("StateID", typeof(string));
            dtBillingShippingAddressData.Columns.Add("CityID", typeof(string));
            dtBillingShippingAddressData.Columns.Add("Pincode", typeof(string));
            dtBillingShippingAddressData.Columns.Add("Area", typeof(string));
            dtBillingShippingAddressData.Columns.Add("GSTIN", typeof(string));
            dtBillingShippingAddressData.Columns.Add("ShipToParty", typeof(string));

            DataRow newDataRow;
            if (!string.IsNullOrEmpty(Convert.ToString(bsBTxtAddress1.Value).Trim()))
            {
                newDataRow = dtBillingShippingAddressData.NewRow();
                newDataRow["AddressType"] = "Billing";
                newDataRow["Address1"] = Convert.ToString(bsBTxtAddress1.Value).Trim();
                newDataRow["Address2"] = Convert.ToString(bsBTxtAddress2.Value).Trim();
                newDataRow["Address3"] = Convert.ToString(bsBTxtAddress3.Value).Trim();
                newDataRow["LandMark"] = Convert.ToString(bsBTxtLandmark.Value).Trim();
                newDataRow["CountryID"] = Convert.ToString(bsBCmbCountryHF.Value).Trim();
                newDataRow["StateID"] = Convert.ToString(bsBCmbStateHF.Value).Trim();
                newDataRow["CityID"] = Convert.ToString(bsBCmbCityHF.Value).Trim();
                newDataRow["Pincode"] = Convert.ToString(bsBCmbPin.Value).Trim();
                newDataRow["Area"] = Convert.ToString(bsBCmbArea.Value).Trim();
                newDataRow["GSTIN"] = Convert.ToString(bsBTxtGSTIN1.Value).Trim() + Convert.ToString(bsBTxtGSTIN2.Value).Trim() + Convert.ToString(bsBTxtGSTIN3.Value).Trim();
                newDataRow["ShipToParty"] = string.Empty;
                dtBillingShippingAddressData.Rows.Add(newDataRow);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(bsSTxtAddress1.Value).Trim()))
            {
                newDataRow = dtBillingShippingAddressData.NewRow();
                newDataRow["AddressType"] = "Shipping";
                newDataRow["Address1"] = Convert.ToString(bsSTxtAddress1.Value).Trim();
                newDataRow["Address2"] = Convert.ToString(bsSTxtAddress2.Value).Trim();
                newDataRow["Address3"] = Convert.ToString(bsSTxtAddress3.Value).Trim();
                newDataRow["LandMark"] = Convert.ToString(bsSTxtLandmark.Value).Trim();
                newDataRow["CountryID"] = Convert.ToString(bsSCmbCountryHF.Value).Trim();
                newDataRow["StateID"] = Convert.ToString(bsSCmbStateHF.Value).Trim();
                newDataRow["CityID"] = Convert.ToString(bsSCmbCityHF.Value).Trim();
                newDataRow["Pincode"] = Convert.ToString(bsSCmbPin.Value).Trim();
                newDataRow["Area"] = Convert.ToString(bsSCmbArea.Value).Trim();
                newDataRow["GSTIN"] = Convert.ToString(bsSTxtGSTIN1.Value).Trim() + Convert.ToString(bsSTxtGSTIN2.Value).Trim() + Convert.ToString(bsSTxtGSTIN3.Value).Trim();
                newDataRow["ShipToParty"] = Convert.ToString(bsSCustomer.Value);
                dtBillingShippingAddressData.Rows.Add(newDataRow);
            }
            ClearLookUpSession();
            return dtBillingShippingAddressData;
        }
        #endregion

        #region ######## Ship to Party Binding ############
        private void CustomerBind()
        {
            if (hfBSShipToPartyVisibility.Value.ToUpper() == "YES")
            {
                DataSet dstDT = objCommonBL.GetBillingShippingAllCustomer();
                if (dstDT != null)
                {
                    if (dstDT.Tables[0] != null && dstDT.Tables[0].Rows.Count > 0)
                    {
                        bsSCustomer.TextField = "cnt_name";
                        bsSCustomer.ValueField = "cnt_internalId";
                        bsSCustomer.DataSource = dstDT.Tables[0];
                        bsSCustomer.DataBind();
                    }
                }
            }
        }
        #endregion


        #region ### Populate GSTIN ######

        private string PopulateGSTIN(string custID, string stateID, string branchID, string docType, string reqType = "")
        {
            string gstin = string.Empty;
            if (bsBCmbCountry.Text.ToUpper() == "INDIA")
            {
                if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                   || docType == "PRI" || docType == "SR"
                   || docType == "CR" || docType == "SRN" || docType == "SRM")
                {
                    if (reqType == "Billing")
                    {
                        custID = Convert.ToString(Session["LastCompany"]);
                    }
                    else if (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value))) ////checking for request comes for Customer or Our Branch/Company
                    {
                        custID = Convert.ToString(Session["LastCompany"]);
                    }
                    else
                    {
                        custID = Convert.ToString(bsSCustomer.Value);
                        docType = "SO";  //// DocType make forcefully "SO" because requset comes from our shipping for customer
                    }
                }
                DataSet dst = objCommonBL.GetGSTINBillingShipping(custID, stateID, branchID, docType);

                if (dst.Tables[0].Rows.Count > 0)
                {
                    gstin = Convert.ToString(dst.Tables[0].Rows[0][0]);
                }
            }
            return gstin;
        }

        #endregion

        #region ####### Addition Function ##########
        public string GetShippingStateCode(string reqType)
        {
            return ucBShfSStateCode.Value;
            //if (reqType.ToUpper() != "ADD")
            //    return ucBShfSStateCode.Value;
            //else
            //    return bsSCmbState.Text;
            //return ucBShfSStateCode.Value;
        }

        public string GetShippingStateCode()
        {
            return Convert.ToString(bsSCmbStateHF.Value);
        }
          public string GetBillingStateCode()
        {
            return Convert.ToString(bsBCmbState.Value);
        }
        

        public string GetGSTINCode(string reqType)
        {
            string returnVal = string.Empty;
            switch (reqType.Trim().ToUpper())
            {
                case "BILLING":
                    returnVal = Convert.ToString(bsBTxtGSTIN1.Value).Trim() + Convert.ToString(bsBTxtGSTIN2.Value).Trim() + Convert.ToString(bsBTxtGSTIN3.Value).Trim();
                    break;
                case "SHIPPING":
                    returnVal = Convert.ToString(bsSTxtGSTIN1.Value).Trim() + Convert.ToString(bsSTxtGSTIN2.Value).Trim() + Convert.ToString(bsSTxtGSTIN3.Value).Trim();
                    break;
            }
            return returnVal;
        }

        protected void BillingAddress_DataBinding(object sender, EventArgs e)
        {
            if (Session["sessBillingAddress"] != null)
            {
                billingAddress.DataSource = (DataTable)Session["sessBillingAddress"];
            }
            if (Session["sessShippingAddress"] != null)
            {
                shippingAddress.DataSource = (DataTable)Session["sessShippingAddress"];

            }
        }

        protected void ShippingAddress_DataBinding(object sender, EventArgs e)
        {
            if (Session["sessShippingAddress"] != null)
            {
                shippingAddress.DataSource = (DataTable)Session["sessShippingAddress"];
            }
        }

        private void ClearLookUpSession()
        {
            if (Session["sessBillingAddress"] != null)
            {
                Session.Remove("sessBillingAddress");
            }

            if (Session["sessShippingAddress"] != null)
            {
                Session.Remove("sessShippingAddress");
            }
        }

        private void ClearJSProperties()
        {
            bsComponentPanel.JSProperties["cpBSCopy"] = 0;
            bsComponentPanel.JSProperties["cpDocType"] = string.Empty;
            bsComponentPanel.JSProperties["cpBSFocus"] = string.Empty;
            bsComponentPanel.JSProperties["cpBSBillingLookup"] = 0;
            bsComponentPanel.JSProperties["cpBSShippingLookup"] = 0;
        }
        #endregion





        private void PincodeBind(string cityID, string reqType = "")
        {
            if (cityID != "")
            {
                DataSet dsState = objCommonBL.GetPinByCity(cityID);
                if (reqType == "Billing")
                {
                    bsBCmbPin.ValueField = "pin_id";
                    bsBCmbPin.TextField = "pin_code";
                    bsBCmbPin.DataSource = dsState.Tables[0];
                    bsBCmbPin.DataBind();
                    bsBCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                }
                else if (reqType == "Shipping")
                {
                    bsSCmbPin.ValueField = "pin_id";
                    bsSCmbPin.TextField = "pin_code";
                    bsSCmbPin.DataSource = dsState.Tables[0];
                    bsSCmbPin.DataBind();
                    bsSCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                }
                else
                {
                    bsBCmbPin.ValueField = "pin_id";
                    bsBCmbPin.TextField = "pin_code";
                    bsBCmbPin.DataSource = dsState.Tables[0];
                    bsBCmbPin.DataBind();

                    bsSCmbPin.ValueField = "pin_id";
                    bsSCmbPin.TextField = "pin_code";
                    bsSCmbPin.DataSource = dsState.Tables[0];
                    bsSCmbPin.DataBind();
                    bsBCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                    bsSCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                }

            }

        }

        private void AreaBind(string cityID, string reqType = "")
        {
            if (cityID != "")
            {
                DataSet dsState = objCommonBL.GetAreaByCity(cityID);
                if (reqType == "Billing")
                {
                    bsBCmbArea.ValueField = "area_id";
                    bsBCmbArea.TextField = "area_name";
                    bsBCmbArea.DataSource = dsState.Tables[0];
                    bsBCmbArea.DataBind();
                    bsBCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                }
                else if (reqType == "Shipping")
                {
                    bsSCmbArea.ValueField = "area_id";
                    bsSCmbArea.TextField = "area_name";
                    bsSCmbArea.DataSource = dsState.Tables[0];
                    bsSCmbArea.DataBind();
                    bsSCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                }
                else
                {
                    bsBCmbArea.ValueField = "area_id";
                    bsBCmbArea.TextField = "area_name";
                    bsBCmbArea.DataSource = dsState.Tables[0];
                    bsBCmbArea.DataBind();

                    bsSCmbArea.ValueField = "area_id";
                    bsSCmbArea.TextField = "area_name";
                    bsSCmbArea.DataSource = dsState.Tables[0];
                    bsSCmbArea.DataBind();
                    bsBCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                    bsSCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                }

            }


        }

        #region ##### New Billing/Shipping ########

        private void AllPinCodesBind()
        {
            DataSet dstDT = objCommonBL.GetBillingShippingAllPinCode();

            hfBSShipToPartyVisibility.Value = Convert.ToString(dstDT.Tables[1].Rows[0]["Variable_Value"]);

            string IsVisible = Convert.ToString(dstDT.Tables[2].Rows[0]["Variable_Value"]).Trim();
            if (IsVisible == "Yes")
            {
                hfBSAlertFlag.Value = "1";
            }
            else
            {
                hfBSAlertFlag.Value = "0";
            }

            if (dstDT != null && dstDT.Tables[0].Rows.Count > 0)
            {
                bsBCmbPin.TextField = "pin_code";
                bsBCmbPin.ValueField = "pin_id";
                bsBCmbPin.DataSource = dstDT.Tables[0];
                bsBCmbPin.DataBind();

                bsSCmbPin.TextField = "pin_code";
                bsSCmbPin.ValueField = "pin_id";
                bsSCmbPin.DataSource = dstDT.Tables[0];
                bsSCmbPin.DataBind();

                bsBCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                bsBCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                bsBCmbPin.Value = "0";

                bsSCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                bsSCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                bsSCmbPin.Value = "0";
            }
        }
        private void SetNewAddressDetails(string CustVenID, string branchID = "", string docType = "")
        {
            #region ######  Vendor GSTIN ########
            OurBSFlag = true;
            hfVendorGSTIN.Value = objCommonBL.GetVendorGSTIN(CustVenID, branchID);
            #endregion

            if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                  || docType == "PRI" || docType == "SR"
                  || docType == "CR" || docType == "SRN" || docType == "SRM")
            {
                CustVenID = Convert.ToString(Session["LastCompany"]);
                if (!string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)))
                {
                    docType = "SO";  //// DocType make forcefully "SO" because requset comes from our shipping for customer
                }

                bsBHeaderText.Text = "Our Billing Address";
                bsSHeaderText.Text = "Our Shipping Address";
            }
            else
            {
                bsBHeaderText.Text = "Billing Address";
                bsSHeaderText.Text = "Shipping Address";
            }

            DataSet DS_AllDetails = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetBillingShipping_ResultSet");
            proc.AddNVarcharPara("@CustomerID", 20, CustVenID);
            proc.AddNVarcharPara("@BranchID", 20, branchID);
            proc.AddNVarcharPara("@DocType", -1, docType);
            proc.AddNVarcharPara("@Action", -1, "GetBillingShippingDetails");
            DS_AllDetails = proc.GetDataSet();

            #region ### Bind DDl ###

            NewAreaBind(DS_AllDetails.Tables[8], "Billing");
            NewAreaBind(DS_AllDetails.Tables[9], "Shipping");

            #endregion


            if (DS_AllDetails != null && DS_AllDetails.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["add_addressType"]).ToUpper() == "BILLING" && DS_AllDetails.Tables[0].Rows.Count == 1)
                {
                    #region #### Set Billing Section ####
                    bsBTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][2]);
                    bsBTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][3]);
                    bsBTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][4]);
                    bsBTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][5]);

                    bsBCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["cou_country"]);
                    bsBCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][6]);

                    bsBCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                    bsBCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][7]);

                    ucBShfSStateCodeText.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);

                    bsBCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["city_name"]);
                    bsBCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][8]);

                    bsBCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][9]);
                    bsBCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][10]);

                    if (DS_AllDetails.Tables[11] != null && DS_AllDetails.Tables[11].Rows.Count > 0)
                    {
                        bsBTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(0, 2);
                        bsBTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(2, 10);
                        bsBTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(12, 3);
                    }

                    #endregion
                }
                else if (Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["add_addressType"]).ToUpper() == "SHIPPING" && DS_AllDetails.Tables[0].Rows.Count == 1)
                {
                    #region #### Set Shipping Section ####

                    try { shippingAddress.GridView.Selection.SelectRowByKey(Convert.ToString(DS_AllDetails.Tables[0].Rows[0][12])); }
                    catch { }
                    bsSTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][2]);
                    bsSTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][3]);
                    bsSTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][4]);
                    bsSTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][5]);

                    bsSCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["cou_country"]);
                    bsSCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][6]);

                    bsSCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                    bsSCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][7]);

                    bsSCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["city_name"]);
                    bsSCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][8]);

                    ucBShfSStateCode.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][11]);
                    ucBShfSStateCodeText.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                 
                    bsSCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][9]);
                    bsSCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][10]);

                    #region ####### Ship To Party Name Visibilty in Party To Ship Lookup ########
                    //bsSCustomer.Value = CustVenID; // Set Default Customer Name as regular process
                    //if (!string.IsNullOrEmpty(shipToParty))
                    //{
                    //    custID = shipToParty;
                    //    bsSCustomer.Value = custID;
                    //}
                    //else
                    //{
                    //    if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                    //       || docType == "PRI" || docType == "SR"
                    //       || docType == "CR" || docType == "SRN" || docType == "SRM")
                    //    {
                    //        bsSCustomer.Value = string.Empty;
                    //    }
                    //}
                    #endregion

                    if (DS_AllDetails.Tables[11] != null && DS_AllDetails.Tables[11].Rows.Count > 0)
                    {
                        if (DS_AllDetails.Tables[11].Rows.Count > 1)
                        {
                            bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(0, 2);
                            bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(2, 10);
                            bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(12, 3);
                        }
                        else
                        {
                            bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(0, 2);
                            bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(2, 10);
                            bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(12, 3);
                        }
                    }


                    if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                       || docType == "PRI" || docType == "SR"
                       || docType == "CR" || docType == "SRN" || docType == "SRM")
                    {
                        bsSCustomer.Value = string.Empty;
                    }
                    //}
                    #endregion
                }
                else
                {
                    #region #### Set Billing Section ####
                    bsBTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][2]);
                    bsBTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][3]);
                    bsBTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][4]);
                    bsBTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][5]);


                    bsBCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["cou_country"]);
                    bsBCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][6]);

                    bsBCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                    bsBCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][7]);

                    bsBCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["city_name"]);
                    bsBCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][8]);

                    bsBCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][9]);
                    bsBCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][10]);

                    if (DS_AllDetails.Tables[11] != null && DS_AllDetails.Tables[11].Rows.Count > 0)
                    {
                        bsBTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(0, 2);
                        bsBTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(2, 10);
                        bsBTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(12, 3);
                    }

                    #endregion
                    #region #### Set Shipping Section ####

                    try { shippingAddress.GridView.Selection.SelectRowByKey(Convert.ToString(DS_AllDetails.Tables[0].Rows[1][12])); }
                    catch { }
                    bsSTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][2]);
                    bsSTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][3]);
                    bsSTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][4]);
                    bsSTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][5]);

                    bsSCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["cou_country"]);
                    bsSCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][6]);

                    bsSCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                    bsSCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][7]);

                    bsSCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["city_name"]);
                    bsSCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][8]);

                    ucBShfSStateCode.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][11]);
                    ucBShfSStateCodeText.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);

                    bsSCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][9]);
                    bsSCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][10]);

                    if (DS_AllDetails.Tables[11] != null && DS_AllDetails.Tables[11].Rows.Count > 0)
                    {
                        if (DS_AllDetails.Tables[11].Rows.Count > 1)
                        {
                            bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(0, 2);
                            bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(2, 10);
                            bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(12, 3);
                        }
                        else
                        {
                            bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(0, 2);
                            bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(2, 10);
                            bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[0][0]).Substring(12, 3);
                        }
                    }


                    //if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                    //   || docType == "PRI" || docType == "SR"
                    //   || docType == "CR" || docType == "SRN" || docType == "SRM")
                    //{
                    //    bsSCustomer.Value = string.Empty;
                    //}

                    if (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)))
                    {
                        bsSCustomer.Value = string.Empty;
                    }


                    //}
                    #endregion
                }

                #region ########## Biiling To Shipping / Shipping To Billing ############
                if (DS_AllDetails.Tables[0].Rows.Count == 1 && Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["add_addressType"]).ToUpper() == "BILLING")
                {
                    #region ########## Biiling To Shipping
                    bsSTxtAddress1.Value = bsBTxtAddress1.Value;
                    bsSTxtAddress2.Value = bsBTxtAddress2.Value;
                    bsSTxtAddress3.Value = bsBTxtAddress3.Value;
                    bsSTxtLandmark.Value = bsBTxtLandmark.Value;

                    bsSCmbCountry.Text = bsBCmbCountry.Text;
                    bsSCmbCountryHF.Value = Convert.ToString(bsBCmbCountryHF.Value);

                    bsSCmbState.Text = bsBCmbState.Text;
                    bsSCmbStateHF.Value = Convert.ToString(bsBCmbStateHF.Value);
                    ucBShfSStateCode.Value = "(State Code:" + bsBCmbState.Text.Substring(bsBCmbState.Text.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "") + ")";
                    ucBShfSStateCodeText.Value = bsBCmbState.Text;

                    bsSCmbCity.Text = bsBCmbCity.Text;
                    bsSCmbCityHF.Value = Convert.ToString(bsBCmbCityHF.Value);


                    bsSCmbPin.Items.Insert(0, new ListEditItem(bsBCmbPin.Text, Convert.ToString(bsBCmbPin.Value)));
                    bsSCmbPin.Value = Convert.ToString(bsBCmbPin.Value);
                    bsSCmbPin.Text = bsBCmbPin.Text;
                    bsSCmbArea.Items.Insert(0, new ListEditItem(bsBCmbArea.Text, Convert.ToString(bsBCmbArea.Value)));
                    bsSCmbArea.Value = Convert.ToString(bsBCmbArea.Value);
                    bsSCmbArea.Text = bsBCmbArea.Text;

                    bsSTxtGSTIN1.Value = bsBTxtGSTIN1.Value;
                    bsSTxtGSTIN2.Value = bsBTxtGSTIN2.Value;
                    bsSTxtGSTIN3.Value = bsBTxtGSTIN3.Value;
                    #endregion
                }
                else if (DS_AllDetails.Tables[0].Rows.Count == 1 && Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["add_addressType"]).ToUpper() == "SHIPPING")
                {
                    #region Shipping To Billing ############
                    bsBTxtAddress1.Value = bsSTxtAddress1.Value;
                    bsBTxtAddress2.Value = bsSTxtAddress2.Value;
                    bsBTxtAddress3.Value = bsSTxtAddress3.Value;
                    bsBTxtLandmark.Value = bsSTxtLandmark.Value;

                    bsBCmbCountry.Text = bsSCmbCountry.Text;
                    bsBCmbCountryHF.Value = Convert.ToString(bsSCmbCountryHF.Value);

                    bsBCmbState.Text = bsSCmbState.Text;
                    bsBCmbStateHF.Value = Convert.ToString(bsSCmbCountryHF.Value);
                    ucBShfSStateCode.Value = "(State Code:" + bsSCmbState.Text.Substring(bsSCmbState.Text.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "") + ")";
                    ucBShfSStateCodeText.Value = bsSCmbState.Text;

                    bsBCmbCity.Text = bsSCmbCity.Text;
                    bsBCmbCityHF.Value = Convert.ToString(bsSCmbCountryHF.Value);

                    bsBCmbPin.Items.Insert(0, new ListEditItem(bsSCmbPin.Text, Convert.ToString(bsSCmbPin.Value)));
                    bsBCmbPin.Value = Convert.ToString(bsSCmbPin.Value);
                    bsBCmbPin.Text = bsSCmbPin.Text;
                    bsBCmbArea.Items.Insert(0, new ListEditItem(bsSCmbArea.Text, Convert.ToString(bsSCmbArea.Value)));
                    bsBCmbArea.Value = Convert.ToString(bsSCmbArea.Value);
                    bsBCmbArea.Text = bsSCmbArea.Text;

                    bsBTxtGSTIN1.Value = bsSTxtGSTIN1.Value;
                    bsBTxtGSTIN2.Value = bsSTxtGSTIN2.Value;
                    bsBTxtGSTIN3.Value = bsSTxtGSTIN3.Value;
                    #endregion
                }
                #endregion
            }

            #region #### Set Lookup Grid For All Address Details ######

            #region ##### Set Billing Look up Grid #########
            Session["sessBillingAddress"] = DS_AllDetails.Tables[10];
            billingAddress.DataSource = DS_AllDetails.Tables[10];
            billingAddress.DataBind();
            #endregion

            #region ##### Set Shipping Look up Grid #########
            Session["sessShippingAddress"] = DS_AllDetails.Tables[10];
            shippingAddress.DataSource = DS_AllDetails.Tables[10];
            shippingAddress.DataBind();
            #endregion

            #endregion

        }

        private void SetEditAddressDetails(string DocID, string docType)
        {
            if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                  || docType == "PRI" || docType == "SR"
                  || docType == "CR" || docType == "SRN" || docType == "SRM")
            {
                bsBHeaderText.Text = "Our Billing Address";
                bsSHeaderText.Text = "Our Shipping Address";
            }
            else
            {
                bsBHeaderText.Text = "Billing Address";
                bsSHeaderText.Text = "Shipping Address";
            }

            DataSet DS_AllDetails = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBillingShippingAddressDetails_ResultSet");
            proc.AddNVarcharPara("@DocID", 20, DocID);
            proc.AddNVarcharPara("@DocType", 50, docType);
            DS_AllDetails = proc.GetDataSet();


            #region ### Bind DDl ###

            NewAreaBind(DS_AllDetails.Tables[9], "Billing");
            NewAreaBind(DS_AllDetails.Tables[10], "Shipping");

            //CustomerBind();

            #endregion


            if (DS_AllDetails != null && DS_AllDetails.Tables[0].Rows.Count > 0)
            {
                #region #### Set Billing Section ####
                bsBTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][2]);
                bsBTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][3]);
                bsBTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][4]);
                bsBTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][5]);

                bsBCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["cou_country"]);
                bsBCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][6]);

                bsBCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                bsBCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][7]);

                bsBCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["city_name"]);
                bsBCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][8]);

                bsBCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][9]);
                bsBCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][10]);

                bsBTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[0][13])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[0].Rows[0][13]).Substring(0, 2);
                bsBTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[0][13])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[0].Rows[0][13]).Substring(2, 10);
                bsBTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[0][13])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[0].Rows[0][13]).Substring(12, 3);

                #endregion

                #region #### Set Shipping Section ####
                if (DS_AllDetails.Tables[0].Rows.Count > 1)
                {

                    try { shippingAddress.GridView.Selection.SelectRowByKey(Convert.ToString(DS_AllDetails.Tables[0].Rows[1][12])); }
                    catch { }
                    bsSTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][2]);
                    bsSTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][3]);
                    bsSTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][4]);
                    bsSTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][5]);


                    bsSCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["cou_country"]);
                    bsSCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][6]);

                    bsSCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["StateCode"]);
                    bsSCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][7]);

                    ucBShfSStateCode.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][11]);
                    ucBShfSStateCodeText.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["StateCode"]);

                    bsSCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[1]["city_name"]);
                    bsSCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][8]);


                    bsSCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][9]);
                    bsSCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][10]);
                    if (hfBSShipToPartyVisibility.Value.ToUpper() == "YES")
                        bsSCustomer.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[1][12]);

                    bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[1][13])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[0].Rows[1][13]).Substring(0, 2);
                    bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[1][13])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[0].Rows[1][13]).Substring(2, 10);
                    bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[1][13])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[0].Rows[1][13]).Substring(12, 3);

                    #region ####### Ship To Party Name Visibilty in Party To Ship Lookup ########
                    //bsSCustomer.Value = CustVenID; // Set Default Customer Name as regular process
                    //if (!string.IsNullOrEmpty(shipToParty))
                    //{
                    //    custID = shipToParty;
                    //    bsSCustomer.Value = custID;
                    //}
                    //else
                    //{
                    //    if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                    //       || docType == "PRI" || docType == "SR"
                    //       || docType == "CR" || docType == "SRN" || docType == "SRM")
                    //    {
                    //        bsSCustomer.Value = string.Empty;
                    //    }
                    //}
                    #endregion


                    if (DS_AllDetails.Tables[11] != null && DS_AllDetails.Tables[11].Rows.Count > 0)
                    {
                        if (DS_AllDetails.Tables[11].Rows.Count > 1)
                        {
                            bsSTxtGSTIN1.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(0, 2);
                            bsSTxtGSTIN2.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(2, 10);
                            bsSTxtGSTIN3.Value = string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1])) ? string.Empty : Convert.ToString(DS_AllDetails.Tables[11].Rows[1][1]).Substring(12, 3);
                        }
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)))
                    {
                        bsSCustomer.Value = string.Empty;
                    }
                }
                else
                {
                    bsSTxtAddress1.Value = bsBTxtAddress1.Value;
                    bsSTxtAddress2.Value = bsBTxtAddress2.Value;
                    bsSTxtAddress3.Value = bsBTxtAddress3.Value;
                    bsSTxtLandmark.Value = bsBTxtLandmark.Value;

                    bsSCmbCountryHF.Value = Convert.ToString(bsBCmbCountryHF.Value);
                    bsSCmbCountry.Text = bsBCmbCountry.Text;
                    bsSCmbStateHF.Value = Convert.ToString(bsBCmbStateHF.Value);

                    bsSCmbState.Text = bsBCmbState.Text;

                    ucBShfSStateCode.Value = "(State Code:" + bsBCmbState.Text.Substring(bsBCmbState.Text.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "") + ")";
                    ucBShfSStateCodeText.Value = bsBCmbState.Text;

                    bsSCmbCityHF.Value = Convert.ToString(bsBCmbCityHF.Value);
                    bsSCmbCity.Text = bsBCmbCity.Text;

                    bsSCmbPin.Value = bsBCmbPin.Value;
                    bsSCmbPin.Text = bsBCmbPin.Text;
                    bsSCmbArea.Value = bsBCmbArea.Value;
                    bsSCmbArea.Text = bsBCmbArea.Text;

                    bsSTxtGSTIN1.Value = bsBTxtGSTIN1.Value;
                    bsSTxtGSTIN2.Value = bsBTxtGSTIN2.Value;
                    bsSTxtGSTIN3.Value = bsBTxtGSTIN3.Value;
                }
                #endregion
                if (DS_AllDetails.Tables.Count > 1)
                {
                    ucBShfBranchID.Value = Convert.ToString(DS_AllDetails.Tables[1].Rows[0][2]);
                    ucBShfCustID.Value = Convert.ToString(DS_AllDetails.Tables[1].Rows[0][3]);
                }
            }

            #region #### Set Lookup Grid For All Address Details ######
            if (DS_AllDetails.Tables[12] != null && DS_AllDetails.Tables[12].Rows.Count > 0)
            {
                DataTable dtLookUp = (DataTable)DS_AllDetails.Tables[12];
                dtLookUp = dtLookUp.AsEnumerable().Where(x => x.Field<string>("ShipToParty") == "0").CopyToDataTable();
                #region ##### Set Billing Look up Grid #########
                Session["sessBillingAddress"] = dtLookUp;
                billingAddress.DataSource = dtLookUp;
                billingAddress.DataBind();
                #endregion

                dtLookUp.Clear();
                dtLookUp = (DataTable)DS_AllDetails.Tables[12];

                #region ##### Set Shipping Look up Grid #########

                if (DS_AllDetails.Tables[12] != null && DS_AllDetails.Tables[12].Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[0].Rows[1][12])))
                    {
                        //// Do not load Ship to party address
                        DataView dvdtLookup = new DataView(dtLookUp);
                        dvdtLookup.RowFilter = "ShipToParty=0";
                        dtLookUp = dvdtLookup.ToTable();
                        //dtLookUp = dtLookUp.AsEnumerable().Where(x => x.Field<string>("ShipToParty") == "0").CopyToDataTable();
                    }
                    else
                    {
                        //// Load Ship to party address
                        DataView dvdtLookup = new DataView(dtLookUp);
                        dvdtLookup.RowFilter = "ShipToParty=1";
                        dtLookUp = dvdtLookup.ToTable();
                        //dtLookUp = dtLookUp.AsEnumerable().Where(x => x.Field<string>("ShipToParty") == "1").CopyToDataTable();
                    }
                }

                Session["sessShippingAddress"] = dtLookUp;
                shippingAddress.DataSource = dtLookUp;
                shippingAddress.DataBind();

                #endregion
            }
            #endregion
        }

        private void NewPincodeBind(DataTable DtPinZip, string reqType = "")
        {
            if (DtPinZip != null && DtPinZip.Rows.Count > 0)
            {
                if (reqType == "Billing")
                {
                    bsBCmbPin.ValueField = "pin_id";
                    bsBCmbPin.TextField = "pin_code";
                    bsBCmbPin.DataSource = DtPinZip;
                    bsBCmbPin.DataBind();
                    bsBCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                }
                else if (reqType == "Shipping")
                {
                    bsSCmbPin.ValueField = "pin_id";
                    bsSCmbPin.TextField = "pin_code";
                    bsSCmbPin.DataSource = DtPinZip;
                    bsSCmbPin.DataBind();
                    bsSCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                }
                else
                {
                    bsBCmbPin.ValueField = "pin_id";
                    bsBCmbPin.TextField = "pin_code";
                    bsBCmbPin.DataSource = DtPinZip;
                    bsBCmbPin.DataBind();

                    bsSCmbPin.ValueField = "pin_id";
                    bsSCmbPin.TextField = "pin_code";
                    bsSCmbPin.DataSource = DtPinZip;
                    bsSCmbPin.DataBind();
                    bsBCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                    bsSCmbPin.Items.Insert(0, new ListEditItem("", "0"));
                }

            }

        }

        private void NewAreaBind(DataTable DtArea, string reqType = "")
        {
            if (DtArea != null && DtArea.Rows.Count > 0)
            {
                if (reqType == "Billing")
                {
                    bsBCmbArea.ValueField = "area_id";
                    bsBCmbArea.TextField = "area_name";
                    bsBCmbArea.DataSource = DtArea;
                    bsBCmbArea.DataBind();
                    bsBCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                }
                else if (reqType == "Shipping")
                {
                    bsSCmbArea.ValueField = "area_id";
                    bsSCmbArea.TextField = "area_name";
                    bsSCmbArea.DataSource = DtArea;
                    bsSCmbArea.DataBind();
                    bsSCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                }
                else
                {
                    bsBCmbArea.ValueField = "area_id";
                    bsBCmbArea.TextField = "area_name";
                    bsBCmbArea.DataSource = DtArea;
                    bsBCmbArea.DataBind();

                    bsSCmbArea.ValueField = "area_id";
                    bsSCmbArea.TextField = "area_name";
                    bsSCmbArea.DataSource = DtArea;
                    bsSCmbArea.DataBind();
                    bsBCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                    bsSCmbArea.Items.Insert(0, new ListEditItem("", "0"));
                }

            }

        }

        private DataSet NewDDLBinding(string PinID, string docType = "", string BranchID = "0", string CustVenID = "")
        {
            if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                 || docType == "PRI" || docType == "SR" || docType == "CBE"
                 || docType == "CR" || docType == "SRN" || docType == "SRM")
            {
                CustVenID = Convert.ToString(Session["LastCompany"]);
            }
            DataSet DS_AllDetails = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetBillingShipping_ResultSet");
            proc.AddNVarcharPara("@PinId", 20, PinID);
            proc.AddNVarcharPara("@BranchID", 20, BranchID);
            proc.AddNVarcharPara("@DocType", -1, docType);
            proc.AddNVarcharPara("@CustomerID", 20, CustVenID);
            proc.AddNVarcharPara("@Action", -1, "PinChangeEvent");
            DS_AllDetails = proc.GetDataSet();

            return DS_AllDetails;
        }

        private void NewAddressDetailsPopulateForLookup(string[] listData, string docType = "")
        {
            DataSet DS_AllDetails = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BillingShipping_GetBillingShipping_ResultSet");
            proc.AddNVarcharPara("@AddressID", 20, listData[3]);
            proc.AddNVarcharPara("@BranchID", 20, (listData[2] == "undefined") ? "0" : listData[2]);
            proc.AddNVarcharPara("@DocType", -1, (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)) ? docType : "SO"));
            proc.AddNVarcharPara("@CustomerID", -1, (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)) ? (listData[3] == "undefined") ? "0" : listData[3] : Convert.ToString(bsSCustomer.Value)));
            proc.AddNVarcharPara("@Action", -1, "GetBillingShippingDetailsByAddressID");
            DS_AllDetails = proc.GetDataSet();


            string BillingOrShipping = (listData[0] == "BillingLookup") ? "Billing" : "Shipping";

            #region ### Bind DDl ###
            NewAreaBind(DS_AllDetails.Tables[4], BillingOrShipping);
            NewPincodeBind(DS_AllDetails.Tables[5], BillingOrShipping);
            #endregion

            if (DS_AllDetails != null && DS_AllDetails.Tables[0].Rows.Count > 0)
            {
                switch (BillingOrShipping)
                {
                    case "Billing":
                        #region #### Set Billing Section ####
                        if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                               || docType == "PRI" || docType == "SR" || docType == "PR"
                               || docType == "CR" || docType == "SRN" || docType == "SRM")
                        {
                            bsBHeaderText.Text = "Our Billing Address";
                        }
                        try { billingAddress.GridView.Selection.SelectRowByKey(Convert.ToString(DS_AllDetails.Tables[0].Rows[0][0])); }
                        catch { }
                        bsBTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][2]);
                        bsBTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][3]);
                        bsBTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][4]);
                        bsBTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][5]);

                        bsBCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["cou_country"]);
                        bsBCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][6]);

                        bsBCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                        bsBCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][7]);

                        bsBCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["city_name"]);
                        bsBCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][8]);

                        bsBCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][9]);
                        bsBCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][10]);

                        #endregion
                        break;
                    case "Shipping":
                        #region #### Set Shipping Section ####

                        if (docType == "PC" || docType == "PB" || docType == "TPB" || docType == "PO" || docType == "VPR" || docType == "VN"
                               || docType == "PRI" || docType == "SR" || docType == "PR"
                               || docType == "CR" || docType == "SRN" || docType == "SRM")
                        {
                            bsSHeaderText.Text = "Our Shipping Address";
                        }

                        try { shippingAddress.GridView.Selection.SelectRowByKey(Convert.ToString(DS_AllDetails.Tables[0].Rows[0][0])); }
                        catch { }
                        bsSTxtAddress1.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][2]);
                        bsSTxtAddress2.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][3]);
                        bsSTxtAddress3.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][4]);
                        bsSTxtLandmark.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][5]);

                        bsSCmbCountry.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["cou_country"]);
                        bsSCmbCountryHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][6]);

                        ucBShfSStateCode.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][11]);
                        ucBShfSStateCodeText.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);

                        bsSCmbState.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["state"]) + " " + Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["StateCode"]);
                        bsSCmbStateHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][7]);


                        bsSCmbCity.Text = Convert.ToString(DS_AllDetails.Tables[0].Rows[0]["city_name"]);
                        bsSCmbCityHF.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][8]);

                        bsSCmbPin.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][9]);
                        bsSCmbArea.Value = Convert.ToString(DS_AllDetails.Tables[0].Rows[0][10]);

                        if (!string.IsNullOrEmpty(Convert.ToString(DS_AllDetails.Tables[6].Rows[0][0])))
                        {
                            bsSTxtGSTIN1.Value = Convert.ToString(DS_AllDetails.Tables[6].Rows[0][0]).Substring(0, 2);
                            bsSTxtGSTIN2.Value = Convert.ToString(DS_AllDetails.Tables[6].Rows[0][0]).Substring(2, 10);
                            bsSTxtGSTIN3.Value = Convert.ToString(DS_AllDetails.Tables[6].Rows[0][0]).Substring(12, 3);
                        }

                        if (string.IsNullOrEmpty(Convert.ToString(bsSCustomer.Value)))
                        {
                            bsSCustomer.Value = string.Empty;
                        }
                        #endregion
                        break;
                }
            }
        }

        #endregion

    }
}