<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/Vendor.Master" AutoEventWireup="true" CodeBehind="VendorPurchaseOrder.aspx.cs" 
    Inherits="ERP.OMS.Management.Vendor.VendorPurchaseOrder" EnableEventValidation="false" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 5px;
            padding-bottom: 5px;
        }

        .voucherno {
            position: absolute;
            right: -3px;
            top: 22px;
        }
        .POIndentReq{
             position: absolute;
            right: -3px;
            top: 22px;
        }
        .POVendor {
            position: absolute;
            right: 2px;
            top: 22px;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #grid_DXStatus span > a {
            display: none;
        }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .absolute, #grid_DXMainTable .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .col-md-3 > label, .col-md-3 > span {
            margin-top: 0px;
            display: inline-block;
        }

        #grid_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }

        #aspxGridTax_DXStatus {
            display: none !important;
        }

        .mTop {
            margin-top: 10px;
        }
        .mandt {
            position: absolute;
            right: -17px;
            top: 6px;
        }
    </style>
    <%-- ------Subhra Address and Billing Sectin Start-----25-01-2017---------%>
    <script type="text/javascript">

        function BackClick() {
            var keyOpening = document.getElementById('hdnOpening').value;
            if (keyOpening != '') {
                var url = 'VendorDashboard.aspx?op=' + 'yes';
            }
            else {
                var url = 'VendorDashboard.aspx';
            }
            window.location.href = url;
        }
        function FinYearCheckOnPageLoad() {
            var SelectedDate = new Date(cPLQuoteDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);

            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);

            var SelectedDateNumericValue = SelectedDateValue.getTime();
            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();
            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                //                   alert('Between');
            }
            else {
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    cPLQuoteDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    cPLQuoteDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function TDateChange(e) {
            var SelectedDate = new Date(cPLQuoteDate.GetDate());
            var monthnumber = SelectedDate.getMonth();
            var monthday = SelectedDate.getDate();
            var year = SelectedDate.getYear();

            var SelectedDateValue = new Date(year, monthnumber, monthday);
            ///Checking of Transaction Date For MaxLockDate
            var MaxLockDate = new Date('<%=Session["LCKBNK"]%>');
            monthnumber = MaxLockDate.getMonth();
            monthday = MaxLockDate.getDate();
            year = MaxLockDate.getYear();
            var MaxLockDateNumeric = new Date(year, monthnumber, monthday).getTime();

            if (SelectedDateValue <= MaxLockDateNumeric) {
                jAlert('This Entry Date has been Locked.');
                MaxLockDate.setDate(MaxLockDate.getDate() + 1);
                cPLQuoteDate.SetDate(MaxLockDate);
                return;
            }
            ///End Checking of Transaction Date For MaxLockDate

            ///Date Should Between Current Fin Year StartDate and EndDate
            var FYS = "<%=Session["FinYearStart"]%>";
            var FYE = "<%=Session["FinYearEnd"]%>";
            var LFY = "<%=Session["LastFinYear"]%>";
            var FinYearStartDate = new Date(FYS);
            var FinYearEndDate = new Date(FYE);
            var LastFinYearDate = new Date(LFY);

            monthnumber = FinYearStartDate.getMonth();
            monthday = FinYearStartDate.getDate();
            year = FinYearStartDate.getYear();
            var FinYearStartDateValue = new Date(year, monthnumber, monthday);


            monthnumber = FinYearEndDate.getMonth();
            monthday = FinYearEndDate.getDate();
            year = FinYearEndDate.getYear();
            var FinYearEndDateValue = new Date(year, monthnumber, monthday);


            var SelectedDateNumericValue = SelectedDateValue.getTime();

            var FinYearStartDateNumericValue = FinYearStartDateValue.getTime();

            var FinYearEndDatNumbericValue = FinYearEndDateValue.getTime();

            var keyOpening = document.getElementById('hdnOpening').value;

            if (keyOpening != '') {
                if (SelectedDateNumericValue <= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                    //GetIndentREquiNo();
                }
                else {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                        cPLQuoteDate.SetDate(new Date(FinYearStartDate));
                    }
                    if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                        cPLQuoteDate.SetDate(new Date(FinYearEndDate));
                    }
                }
            }
            else {
                if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {
                    GetIndentREquiNo();
                }
                else {
                    jAlert('Enter Date Is Outside Of Financial Year !!');
                    if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                        cPLQuoteDate.SetDate(new Date(FinYearStartDate));
                    }
                    if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                        cPLQuoteDate.SetDate(new Date(FinYearEndDate));
                    }
                }
                ///End OF Date Should Between Current Fin Year StartDate and EndDate
            }

        }
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('Billing/Shipping').SetEnabled(true);
            }
        }

        function CopyBillingAddresstoShipping(obj) {
            var chkbill = obj.GetChecked();
            if (chkbill == true) {
                $('#DivShipping').hide();
            }
            else {
                $('#DivShipping').show();
            }

            //  cComponentPanel.PerformCallback('Edit~BillingAddresstoShipping');
        }
        function CopyShippingAddresstoBilling(obj) {
            var chkship = obj.GetChecked();
            if (chkship == true) {
                $('#DivBilling').hide();
            }
            else {
                $('#DivBilling').show();
            }
            //cComponentPanel.PerformCallback('Edit~ShippingAddresstoBilling');
        }



        function btnSave_QuoteAddress() {

            checking = true;
            var chkbilling = cchkBilling.GetChecked();
            var chkShipping = cchkShipping.GetChecked();

            if (chkbilling == false && chkShipping == false) {
                // Billing Start
                if (ctxtAddress1.GetText() == '' || ctxtAddress1.GetText() == null) {
                    $('#badd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#badd1').attr('style', 'display:none'); }

                if (CmbCountry.GetValue() == '' || CmbCountry.GetValue() == null || CmbCountry.GetValue() == 'select') {
                    $('#bcountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcountry').attr('style', 'display:none'); }


                // State

                if (CmbState.GetValue() == '' || CmbState.GetValue() == null || CmbState.GetValue() == 'select') {
                    $('#bstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bstate').attr('style', 'display:none'); }

                // city
                if (CmbCity.GetValue() == '' || CmbCity.GetValue() == null || CmbCity.GetValue() == 'select') {
                    $('#bcity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcity').attr('style', 'display:none'); }

                // pin

                if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
                    $('#bpin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bpin').attr('style', 'display:none'); }
                // Billing End

                // Shipping Start

                if (ctxtsAddress1.GetText() == '' || ctxtsAddress1.GetText() == null) {
                    $('#sadd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sadd1').attr('style', 'display:none'); }

                if (CmbCountry1.GetValue() == '' || CmbCountry1.GetValue() == null || CmbCountry1.GetValue() == 'select') {
                    $('#scountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scountry').attr('style', 'display:none'); }


                // State

                if (CmbState1.GetValue() == '' || CmbState1.GetValue() == null || CmbState1.GetValue() == 'select') {
                    $('#sstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sstate').attr('style', 'display:none'); }

                // city
                if (CmbCity1.GetValue() == '' || CmbCity1.GetValue() == null || CmbCity1.GetValue() == 'select') {
                    $('#scity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scity').attr('style', 'display:none'); }

                // pin

                if (CmbPin1.GetValue() == '' || CmbPin1.GetValue() == null || CmbPin1.GetValue() == 'select') {
                    $('#spin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#spin').attr('style', 'display:none'); }

                // Shipping End

            }


            else if (chkbilling == true && chkShipping == false) {
                // Billing Start
                if (ctxtAddress1.GetText() == '' || ctxtAddress1.GetText() == null) {
                    $('#badd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#badd1').attr('style', 'display:none'); }

                if (CmbCountry.GetValue() == '' || CmbCountry.GetValue() == null || CmbCountry.GetValue() == 'select') {
                    $('#bcountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcountry').attr('style', 'display:none'); }


                // State

                if (CmbState.GetValue() == '' || CmbState.GetValue() == null || CmbState.GetValue() == 'select') {
                    $('#bstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bstate').attr('style', 'display:none'); }

                // city
                if (CmbCity.GetValue() == '' || CmbCity.GetValue() == null || CmbCity.GetValue() == 'select') {
                    $('#bcity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bcity').attr('style', 'display:none'); }

                // pin

                if (CmbPin.GetValue() == '' || CmbPin.GetValue() == null || CmbPin.GetValue() == 'select') {
                    $('#bpin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#bpin').attr('style', 'display:none'); }
                // Billing End
            }

            else if (chkbilling == false && chkShipping == true) {
                // Shipping Start

                if (ctxtsAddress1.GetText() == '' || ctxtsAddress1.GetText() == null) {
                    $('#sadd1').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sadd1').attr('style', 'display:none'); }

                if (CmbCountry1.GetValue() == '' || CmbCountry1.GetValue() == null || CmbCountry1.GetValue() == 'select') {
                    $('#scountry').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scountry').attr('style', 'display:none'); }


                // State

                if (CmbState1.GetValue() == '' || CmbState1.GetValue() == null || CmbState1.GetValue() == 'select') {
                    $('#sstate').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#sstate').attr('style', 'display:none'); }

                // city
                if (CmbCity1.GetValue() == '' || CmbCity1.GetValue() == null || CmbCity1.GetValue() == 'select') {
                    $('#scity').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#scity').attr('style', 'display:none'); }

                // pin

                if (CmbPin1.GetValue() == '' || CmbPin1.GetValue() == null || CmbPin1.GetValue() == 'select') {
                    $('#spin').attr('style', 'display:block');
                    checking = false;
                    //return false;
                }
                else { $('#spin').attr('style', 'display:none'); }

                // Shipping End

            }
            if (checking == true) {

                var custID = GetObjectID('hdnCustomerId').value;
                var chkbilling = cchkBilling.GetChecked();
                //var chkbilling = cchkBilling.GetChecked;
                var chkShipping = cchkShipping.GetChecked();
                //var chkShipping = cchkShipping.GetChecked;
                if ((chkbilling == false) && (chkShipping == false)) {
                    cComponentPanel.PerformCallback('save~1');
                }
                else if ((chkbilling == true) && (chkShipping == false)) {
                    cComponentPanel.PerformCallback('save~2');
                }
                else if ((chkbilling == false) && (chkShipping == true)) {
                    cComponentPanel.PerformCallback('save~3');
                }
                //GetObjectID('hdnAddressDtl').value = '1';
                page.SetActiveTabIndex(0);
                gridLookup.Focus();
            }
            else {
                page.SetActiveTabIndex(1);
            }
        }


        //function ClosebillingLookup() {
        //    billingLookup.ConfirmCurrentSelection();
        //    billingLookup.HideDropDown();
        //    billingLookup.Focus();
        //}
        //function CloseshippingLookup() {
        //    shippingLookup.ConfirmCurrentSelection();
        //    shippingLookup.HideDropDown();
        //    shippingLookup.Focus();
        //}

        //Subhra-----23-01-2017-------
        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;


        function OnCountryChanged(cmbCountry) {

            CmbState.PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnCountryChanged1(cmbCountry1) {
            CmbState1.PerformCallback(cmbCountry1.GetValue().toString());
        }

        function OnStateChanged(cmbState) {

            CmbCity.PerformCallback(cmbState.GetValue().toString());
        }
        function OnStateChanged1(cmbState1) {
            CmbCity1.PerformCallback(cmbState1.GetValue().toString());
        }

        function OnCityChanged(abc) {

            CmbPin.PerformCallback(abc.GetValue().toString());
            CmbArea.PerformCallback(abc.GetValue().toString());
        }
        function OnCityChanged1(abc) {
            CmbPin1.PerformCallback(abc.GetValue().toString());
            CmbArea1.PerformCallback(abc.GetValue().toString());


        }
        //----------------------------------
        function OnChildCall(CmbCity) {

            OnCityChanged(CmbCity.GetValue());
            OnCityChanged(CmbCity1.GetValue());
        }
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = CmbCity.GetValue();
            var cityname = CmbCity.GetText();
            var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            popupan.SetContentUrl(URL);
            popupan.Show();
        }

        function openAreaPageShip() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = CmbCity1.GetValue();
            var cityname = CmbCity1.GetText();
            var URL = '../Master/AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            popupan.SetContentUrl(URL);
            popupan.Show();
        }



        function fn_PopOpen() {
            CmbCountry.SetSelectedIndex(-1);
            CmbCountry1.SetSelectedIndex(-1);
            CmbState.SetSelectedIndex(-1);
            CmbState1.SetSelectedIndex(-1);
            CmbCity.SetSelectedIndex(-1);
            CmbCity1.SetSelectedIndex(-1);
            CmbPin.SetSelectedIndex(-1);
            CmbPin1.SetSelectedIndex(-1);
            CmbArea.SetSelectedIndex(-1);
            CmbArea1.SetSelectedIndex(-1);
            cComponentPanel.PerformCallback('Edit~1');
            //cComponentPanel.PerformCallback('Edit~1' + GetObjectID('hdnAddressDtl').value); 
        }

        function cmbstate_endcallback(s, e) {
            if (Billing_state != 0) {
                s.SetValue(Billing_state);
            }
            CmbCity.PerformCallback(CmbState.GetValue());
            //Billing_state = 0;
        }
        function cmbshipstate_endcallback(s, e) {
            if (Shipping_state != 0) {
                s.SetValue(Shipping_state);
            }
            CmbCity1.PerformCallback(CmbState1.GetValue());
            Shipping_state = 0;
        }

        function cmbcity_endcallback(s, e) {
            if (Billing_city != 0) {
                s.SetValue(Billing_city);
            }
            CmbPin.PerformCallback(CmbCity.GetValue());
            CmbArea.PerformCallback(CmbCity.GetValue());
            Billing_city = 0;

        }
        function cmbshipcity_endcallback(s, e) {
            if (Shipping_city != 0) {
                s.SetValue(Shipping_city);
            }
            CmbPin1.PerformCallback(CmbCity1.GetValue());
            CmbArea1.PerformCallback(CmbCity1.GetValue());
            Shipping_city = 0;

        }

        function cmbPin_endcallback(s, e) {
            if (Billing_pin != 0) {
                s.SetValue(Billing_pin);
            }
            if (Billing_pin != null || Billing_pin != '' || Billing_pin != '0') {
                cchkBilling.SetEnabled(true);
            }
            Billing_pin = 0;
        }
        function cmbshipPin_endcallback(s, e) {
            if (Shipping_pin != 0) {
                s.SetValue(Shipping_pin);
            }
            if (Shipping_pin != null || Shipping_pin != '' || Shipping_pin != '0') {
                cchkShipping.SetEnabled(true);
            }
            Shipping_pin = 0;
        }

        function cmbArea_endcallback(s, e) {
            if (billing_area != 0) {
                s.SetValue(billing_area);
            }
            billing_area = 0;
        }

        function cmbshipArea_endcallback(s, e) {
            if (Shipping_area != 0) {
                s.SetValue(Shipping_area);
            }
            Shipping_area = 0;
        }

        //function Popup_SalesQuote_EndCallBack() {
        //    if (Popup_SalesQuote.cpshow != null) {


        //        CmbAddressType.SetText(Popup_SalesQuote.cpshow.split('~')[0]);
        //        ctxtAddress1.SetText(Popup_SalesQuote.cpshow.split('~')[1]);
        //        ctxtAddress2.SetText(Popup_SalesQuote.cpshow.split('~')[2]);
        //        ctxtAddress3.SetText(Popup_SalesQuote.cpshow.split('~')[3]);
        //        ctxtlandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
        //        CmbCountry.SetValue(Popup_SalesQuote.cpshow.split('~')[5]);
        //        Billing_state = Popup_SalesQuote.cpshow.split('~')[6];
        //        Billing_city = Popup_SalesQuote.cpshow.split('~')[7];
        //        Billing_pin = Popup_SalesQuote.cpshow.split('~')[8];
        //        billing_area = Popup_SalesQuote.cpshow.split('~')[9];
        //        CmbState.PerformCallback(CmbCountry.GetValue());
        //    }

        //    if (Popup_SalesQuote.cpshowShip != null) {


        //        CmbAddressType1.SetText(Popup_SalesQuote.cpshowShip.split('~')[0]);
        //        ctxtsAddress1.SetText(Popup_SalesQuote.cpshowShip.split('~')[1]);
        //        ctxtsAddress2.SetText(Popup_SalesQuote.cpshowShip.split('~')[2]);
        //        ctxtsAddress3.SetText(Popup_SalesQuote.cpshowShip.split('~')[3]);
        //        ctxtslandmark.SetText(Popup_SalesQuote.cpshow.split('~')[4]);
        //        CmbCountry1.SetValue(Popup_SalesQuote.cpshowShip.split('~')[5]);
        //        Shipping_state = Popup_SalesQuote.cpshowShip.split('~')[6];
        //        Shipping_city = Popup_SalesQuote.cpshowShip.split('~')[7];
        //        Shipping_pin = Popup_SalesQuote.cpshowShip.split('~')[8];
        //        Shipping_area = Popup_SalesQuote.cpshowShip.split('~')[9];
        //        CmbState1.PerformCallback(CmbCountry1.GetValue());
        //    }

        //}
        function Panel_endcallback() {

            var billingStatus = null;
            var shippingStatus = null;
            if (cComponentPanel.cpshow != null) {


                //CmbAddressType.SetText(cComponentPanel.cpshow.split('~')[0]);
                ctxtAddress1.SetText(cComponentPanel.cpshow.split('~')[1]);
                ctxtAddress2.SetText(cComponentPanel.cpshow.split('~')[2]);
                ctxtAddress3.SetText(cComponentPanel.cpshow.split('~')[3]);
                ctxtlandmark.SetText(cComponentPanel.cpshow.split('~')[4]);
                var bcon = cComponentPanel.cpshow.split('~')[5];
                if (bcon == '') {
                    CmbCountry.SetSelectedIndex(-1);
                }
                else {
                    CmbCountry.SetValue(cComponentPanel.cpshow.split('~')[5]);
                }

                var bsta = cComponentPanel.cpshow.split('~')[6];
                if (bsta == '') {
                    CmbState.SetSelectedIndex(-1);
                    Billing_state = 0;
                }
                else {
                    Billing_state = cComponentPanel.cpshow.split('~')[6];
                }
                var bcity = cComponentPanel.cpshow.split('~')[7];
                if (bcity == '') {
                    CmbCity.SetSelectedIndex(-1);
                    Billing_city = 0;
                }
                else {
                    Billing_city = cComponentPanel.cpshow.split('~')[7];
                }

                var bpin = cComponentPanel.cpshow.split('~')[8];
                if (bpin == '') {
                    CmbPin.SetSelectedIndex(-1);
                    Billing_pin = 0;
                }
                else {
                    Billing_pin = cComponentPanel.cpshow.split('~')[8];
                }

                var barea = cComponentPanel.cpshow.split('~')[9];
                if (barea == '') {
                    CmbArea.SetSelectedIndex(-1);
                    billing_area = 0;
                }
                else {
                    billing_area = cComponentPanel.cpshow.split('~')[9];
                }
                //CmbCountry.SetValue(cComponentPanel.cpshow.split('~')[5]);
                //Billing_state = cComponentPanel.cpshow.split('~')[6];
                //Billing_city = cComponentPanel.cpshow.split('~')[7];
                //Billing_pin = cComponentPanel.cpshow.split('~')[8];
                //billing_area = cComponentPanel.cpshow.split('~')[9];
                billingStatus = cComponentPanel.cpshow.split('~')[10];
                var countryid = CmbCountry.GetValue()
                if (countryid != null && countryid != '' && countryid != '0') {
                    CmbState.PerformCallback(countryid);
                }
            }

            if (cComponentPanel.cpshowShip != null) {

                //CmbAddressType1.SetText(cComponentPanel.cpshowShip.split('~')[0]);
                ctxtsAddress1.SetText(cComponentPanel.cpshowShip.split('~')[1]);

                ctxtsAddress2.SetText(cComponentPanel.cpshowShip.split('~')[2]);
                ctxtsAddress3.SetText(cComponentPanel.cpshowShip.split('~')[3]);
                ctxtslandmark.SetText(cComponentPanel.cpshowShip.split('~')[4]);
                var scon = cComponentPanel.cpshowShip.split('~')[5];
                if (scon == '') {
                    CmbCountry1.SetSelectedIndex(-1);
                }
                else {
                    CmbCountry1.SetValue(cComponentPanel.cpshowShip.split('~')[5]);
                }
                var ssta = cComponentPanel.cpshowShip.split('~')[6];
                if (ssta == '') {
                    CmbState1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_state = cComponentPanel.cpshowShip.split('~')[6];
                }
                var scity = cComponentPanel.cpshowShip.split('~')[7];
                if (scity == '') {
                    CmbCity1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_city = cComponentPanel.cpshowShip.split('~')[7];
                }

                var spin = cComponentPanel.cpshowShip.split('~')[8];
                if (spin == '') {
                    CmbPin1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_pin = cComponentPanel.cpshowShip.split('~')[8];
                }

                var sarea = cComponentPanel.cpshowShip.split('~')[9];
                if (sarea == '') {
                    CmbArea1.SetSelectedIndex(-1);
                }
                else {
                    Shipping_area = cComponentPanel.cpshowShip.split('~')[9];
                }
                //CmbCountry1.SetValue(cComponentPanel.cpshowShip.split('~')[5]);
                //Shipping_state = 
                //Shipping_city = cComponentPanel.cpshowShip.split('~')[7];
                //Shipping_pin = cComponentPanel.cpshowShip.split('~')[8];
                //Shipping_area = cComponentPanel.cpshowShip.split('~')[9];
                shippingStatus = cComponentPanel.cpshowShip.split('~')[10];
                var countryid1 = CmbCountry1.GetValue()
                if (countryid1 != null && countryid1 != '' && countryid1 != '0') {
                    CmbState1.PerformCallback(countryid1);
                }
                //CmbState1.PerformCallback(CmbCountry1.GetValue());
            }
            if (billingStatus == 'Y' && shippingStatus == 'N') {
                cchkBilling.SetEnabled(true);
                cchkShipping.SetEnabled(false);
            }
            else if (billingStatus == 'N' && shippingStatus == 'Y') {
                cchkBilling.SetEnabled(false);
                cchkShipping.SetEnabled(true);

            }
            else if (billingStatus == 'Y' && shippingStatus == 'Y') {
                cchkBilling.SetEnabled(false);
                cchkShipping.SetEnabled(false);

            }
            else if (billingStatus == 'N' && shippingStatus == 'N') {
                cchkBilling.SetEnabled(false);
                cchkShipping.SetEnabled(false);
            }
            if (grid.cpView == "1") {
                ctxtAddress1.SetEnabled(false);
                ctxtAddress2.SetEnabled(false);
                ctxtAddress3.SetEnabled(false);
                ctxtlandmark.SetEnabled(false);
                CmbCountry.SetEnabled(false);
                CmbState.SetEnabled(false);
                CmbCity.SetEnabled(false);
                CmbPin.SetEnabled(false);
                CmbArea.SetEnabled(false);
                ctxtsAddress1.SetEnabled(false);
                ctxtsAddress2.SetEnabled(false);
                ctxtsAddress3.SetEnabled(false);
                ctxtslandmark.SetEnabled(false);
                CmbCountry1.SetEnabled(false);
                CmbState1.SetEnabled(false);
                CmbCity1.SetEnabled(false);
                CmbPin1.SetEnabled(false);
                CmbArea1.SetEnabled(false);
                cchkShipping.SetVisible(false);
                cchkBilling.SetVisible(false);
                cButton1.SetVisible(false);
                cbtnSave_citys.SetVisible(false);
            }
        }

        function disp_prompt(name) {

            if (name == "tab0") {
                gridLookup.Focus();
                //alert(name);
                //document.location.href = "SalesQuotation.aspx?";
            }
            if (name == "tab1") {
                var custID = GetObjectID('hdnCustomerId').value;
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);
                    return;
                }
                else {
                    page.SetActiveTabIndex(1);
                    fn_PopOpen();
                }
            }
        }

    </script>
    <%-- ------Subhra Address and Billing Section End-----25-01-2017---------%>
    <script type="text/javascript">
        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() + 100 ; i++) {
                grid.DeleteRow(frontRow);
                grid.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }

        }
        function onBranchItems() {
            GetIndentReqNoOnLoad();

            grid.batchEditApi.StartEdit(-1, 1);
            var accountingDataMin = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();
            console.log(accountingDataMin);

            grid.batchEditApi.StartEdit(0, 1);
            var accountingDataplus = grid.GetEditor('ProductName').GetValue();
            console.log(accountingDataplus);
            grid.batchEditApi.EndEdit();

            if (accountingDataMin != null || accountingDataplus != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        cQuotationComponentPanel.PerformCallback('BindNullGrid');
                        //  gridquotationLookup.SetValue(0);

                    } else {

                    }
                });
            }
        }
        function disp_prompt(name) {

            if (name == "tab0") {
                gridLookup.Focus();
                //alert(name);
                //document.location.href = "SalesQuotation.aspx?";
            }
            if (name == "tab1") {
                var custID = GetObjectID('hdnCustomerId').value;
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);

                    return;
                }
                else {
                    page.SetActiveTabIndex(1);
                    fn_PopOpen();
                }
            }
        }


        var globalRowIndex;
        var rowEditCtrl;

        //............................Product Pazination..............
        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }
        function ctaxUpdatePanelEndCall(s, e) {
            if (ctaxUpdatePanel.cpstock != null) {
                divAvailableStk.style.display = "block";
                // divpopupAvailableStock.style.display = "block";

                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;

                ctaxUpdatePanel.cpstock = null;
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }

            return false;
        }
        function ProductsGotFocusFromID(s, e) {
            pageheaderContent.style.display = "block";
            divAvailableStk.style.display = "block";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                //divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }
        }
        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }
        }
        function ProductSelected(s, e) {

            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                return;
            }
            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("gvColProduct").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            //console.log(LookUpData);
            pageheaderContent.style.display = "block";
            divAvailableStk.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("gvColDiscription");
            var tbUOM = grid.GetEditor("gvColUOM");
            var tbSalePrice = grid.GetEditor("gvColStockPurchasePrice");

            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            if (strRate == 0) {
                strSalePrice = strSalePrice;
            }
            else {
                strSalePrice = strSalePrice / strRate;
            }

            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbSalePrice.SetValue(strSalePrice);

            grid.GetEditor("gvColQuantity").SetValue("0.00");
            grid.GetEditor("gvColDiscount").SetValue("0.00");
            grid.GetEditor("gvColAmount").SetValue("0.00");
            grid.GetEditor("gvColTaxAmount").SetValue("0.00");
            grid.GetEditor("gvColTotalAmountINR").SetValue("0.00");

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strDescription);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                // divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }
            //divPacking.style.display = "none";

            //lblbranchName lblProduct
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            grid.batchEditApi.StartEdit(globalRowIndex, 6);
        }
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }
        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cproductLookUp.Clear()) {
                    cProductpopUp.Show();
                    cproductLookUp.Focus();
                    cproductLookUp.ShowDropDown();
                }
            }
        }
        //..............End Product........................
        //.............Available Stock Div Show............................
        function ProductsGotFocus(s, e) {
            pageheaderContent.style.display = "block";
            divAvailableStk.style.display = "block";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            //var ProductID = (cCmbProduct.GetValue() != null) ? cCmbProduct.GetValue() : "0";
            //var strProductName = (cCmbProduct.GetText() != null) ? cCmbProduct.GetText() : "0";

            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                //  divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            $('#<%= lblProduct.ClientID %>').text(strProductName);
            $('#<%= lblbranchName.ClientID %>').text(strBranch);

            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }
        }
        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                divAvailableStk.style.display = "block";
                //divpopupAvailableStock.style.display = "block";

                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = cacpAvailableStock.cpstock;


                cCmbWarehouse.cpstock = null;
            }
        }
        //................Available Stock Div Show....................
        //Code for UDF Control 
        function OpenUdf(s, e) {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PO&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code
        //............Check Unique   Purchase Order................
        function txtBillNo_TextChanged() {

            var VoucherNo = document.getElementById("txtVoucherNo").value;

            $.ajax({
                type: "POST",
                url: "PurchaseOrder.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#MandatoryBillNo").show();

                        document.getElementById("txtVoucherNo").value = '';
                        document.getElementById("txtVoucherNo").focus();
                    }
                    else {
                        $("#MandatoryBillNo").hide();
                    }
                }
            });
        }
        //..................Rate........................
        function ReBindGrid_Currency() {
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'gvColProduct') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'gvColProduct')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'gvColProduct') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'gvColProduct')) : "";

                if (frontProduct != "" || backProduct != "") {
                    IsProduct = "Y";
                    break;
                }

                backRow--;
                frontRow++;
            }

            if (IsProduct == "Y") {
                $('#<%=hdfIsDelete.ClientID %>').val('D');
                grid.UpdateEdit();
                //   grid.PerformCallback('CurrencyChangeDisplay');
            }
            cddl_AmountAre.Focus();
        }
        //...............end.........................
        //...............PopulateVAT........................
        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            //deleteAllRows();

            if (key == 1) {

                grid.GetEditor('gvColTaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                //cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(-1);
                cbtn_SaveRecords.SetVisible(true);
                grid.GetEditor('gvColProduct').Focus();
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }

            }
            else if (key == 2) {
                grid.GetEditor('gvColTaxAmount').SetEnabled(true);

                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
                cbtn_SaveRecords.SetVisible(true);
            }
            else if (key == 3) {

                grid.GetEditor('gvColTaxAmount').SetEnabled(false);

                //cddlVatGstCst.PerformCallback('3');
                cddlVatGstCst.SetSelectedIndex(-1);
                cddlVatGstCst.SetEnabled(false);
                cbtn_SaveRecords.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }


            }

            //// below code will be executed onlyin View Mode --- Samrat Roy -- 04-05-2017
            if (getUrlVars().req == "V") {
                cbtn_SaveRecords.SetVisible(false);
            }
        }
        function Keypressevt() {

            if (event.keyCode == 13) {

                //run code for Ctrl+X -- ie, Save & Exit! 
                SaveWarehouse();
                return false;
            }
        }

        function SetFocusonDemand(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == '1' || key == '3') {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 3);
                }
            }
            else if (key == '2') {
                cddlVatGstCst.Focus();
            }

        }

        //.................End PopulateVAT...............
        //................Amount Calculation.........................
        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }
        function taxAmtButnClick1(s, e) {
            // console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }
        function taxAmtButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";

                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }
                        //  console.log("1");
                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = Math.round(QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                        clblTaxProdGrossAmt.SetText(Amount);
                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue()).toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = Amount;
                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue()).toFixed(2);

                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {
                            var discount = Math.round((Amount * grid.GetEditor('gvColDiscount').GetValue() / 100)).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }
                        //End Here 


                        //Checking is gstcstvat will be hidden or not
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            $('.gstGrossAmount').show();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
                            $('.gstNetAmount').show();
                            //Set Gross Amount with GstValue
                            //Get The rate of Gst
                            var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
                            if (gstRate) {
                                if (gstRate != 0) {
                                    var gstDis = (gstRate / 100) + 1;
                                    if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                        $('.gstNetAmount').hide();
                                        clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                        document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                        clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                        clblTaxableNet.SetText("");
                                    }
                                    else {
                                        $('.gstGrossAmount').hide();
                                        clblProdNetAmt.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2));
                                        document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2);
                                        clblGstForNet.SetText(Math.round(grid.GetEditor('gvColAmount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                        clblTaxableGross.SetText("");
                                    }
                                }

                            } else {
                                $('.gstGrossAmount').hide();
                                $('.gstNetAmount').hide();
                                clblTaxableGross.SetText("");
                                clblTaxableNet.SetText("");
                            }
                        }
                        else if (cddl_AmountAre.GetValue() == "1") {
                            $('.GstCstvatClass').show();
                            $('.gstGrossAmount').hide();
                            $('.gstNetAmount').hide();
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");


                            //Get Customer Shipping StateCode
                            var shippingStCode = '';
                            if (cchkBilling.GetValue()) {
                                shippingStCode = CmbState.GetText();
                            }
                            else {
                                shippingStCode = CmbState1.GetText();
                            }
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //Debjyoti 09032017
                            if (shippingStCode.trim() != '') {
                                for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                                    //Check if gstin is blank then delete all tax
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                            //if its state is union territories then only UTGST will apply
                                            if (shippingStCode == "4" || shippingStCode == "35" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "7" || shippingStCode == "31" || shippingStCode == "34") {
                                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                                    cmbCount--;
                                                }
                                            }
                                            else {
                                                if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                                    ccmbGstCstVat.RemoveItem(cmbCount);
                                                    cmbCount--;
                                                }
                                            }
                                        } else {
                                            if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVat.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                                ccmbGstCstVat.RemoveItem(cmbCount);
                                                cmbCount--;
                                            }
                                        }
                                    } else {
                                        //remove tax because GSTIN is not define
                                        ccmbGstCstVat.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            }

                        }
                        //End here

                        if (globalRowIndex > -1) {
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                        } else {

                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                            //Set default combo
                            cgridTax.cpComboCode = grid.GetEditor('gvColProduct').GetValue().split('||@||')[9];
                        }

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('gvColAmount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
            }
        }

        function SalePriceTextChange(s, e) {
            pageheaderContent.style.display = "block";
            divAvailableStk.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier 
                var strFactor = SpliteDetails[14]; //Packing_Factor 
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                //console.log("Rate"+strRate);
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();

                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
                var strSalePrice = SpliteDetails[6];// purchase Price
                //console.log("PurchasePrice"+strSalePrice);

                if (strRate == 0) {
                    strRate = 1;
                }
                if (strSalePrice == 0.00000) {
                    strSalePrice = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

                // alert("here" + Amount);
                $('#<%= lblbranchName.ClientID %>').text(strBranch);

                var IsPackingActive = SpliteDetails[13];//IsPackingActive
                var Packing_Factor = SpliteDetails[14];//Packing_Factor
                var Packing_UOM = SpliteDetails[15];//Packing_UOM
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                    // divPacking.style.display = "block";
                } else {
                    divPacking.style.display = "none";
                }



                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(Amount);

                var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                tbTotalAmount.SetValue(Amount);

                DiscountTextChange(s, e);
                //.........AvailableStock.............
                cacpAvailableStock.PerformCallback(strProductID);
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('gvColQuantity').SetValue('0');
                grid.GetEditor('gvColProduct').Focus();
            }
        }



        function QuantityTextChange(s, e) {

            pageheaderContent.style.display = "block";
            divAvailableStk.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                // console.log(SpliteDetails)
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier
                //console.log("Multiplier"+strMultiplier);
                var strFactor = SpliteDetails[14]; //Packing_Factor
                //console.log("Factor"+strFactor);
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                //console.log("Rate"+strRate);
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();

                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
                //var strSalePrice = SpliteDetails[6];// purchase Price
                var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                console.log("PurchasePrice" + strSalePrice);

                if (strRate == 0) {
                    strRate = 1;
                }
                if (strSalePrice == 0.00000) {
                    strSalePrice = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                console.log("StockQuantity" + StockQuantity);
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

                // alert("here" + Amount);
                $('#<%= lblbranchName.ClientID %>').text(strBranch);

                var IsPackingActive = SpliteDetails[13];//IsPackingActive
                var Packing_Factor = SpliteDetails[14];//Packing_Factor
                var Packing_UOM = SpliteDetails[15];//Packing_UOM
                var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

                if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                // divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }



            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(Amount);

            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(Amount);

            DiscountTextChange(s, e);
                //.........AvailableStock.............
            cacpAvailableStock.PerformCallback(strProductID);
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('gvColQuantity').SetValue('0');
            grid.GetEditor('gvColProduct').Focus();
        }
    }
    function DiscountTextChange(s, e) {
        //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";

        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        var ProductID = grid.GetEditor('gvColProduct').GetValue();
        if (ProductID != null) {
            var SpliteDetails = ProductID.split("||@||");
            var strFactor = SpliteDetails[14];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            if (strSalePrice == '0') {
                strSalePrice = SpliteDetails[6];
            }
            if (strRate == 0) {
                strRate = 1;
            }
            var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
            //var Amount = grid.GetEditor("gvColAmount");

            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("gvColAmount");
            tbAmount.SetValue(amountAfterDiscount);

            var IsPackingActive = SpliteDetails[13];
            var Packing_Factor = SpliteDetails[14];
            var Packing_UOM = SpliteDetails[15];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y" && (parseFloat(Packing_Factor * QuantityValue) > 0)) {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                    // divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
            tbTotalAmount.SetValue(amountAfterDiscount);
        }
        else {
            jAlert('Select a product first.');
            grid.GetEditor('gvColDiscount').SetValue('0');
            grid.GetEditor('gvColProduct').Focus();
        }
            //Debjyoti 
        grid.GetEditor('gvColTaxAmount').SetValue(0);
        ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());

    }

    //......................Amount Calculation End.......................
    /*........................Tax Start...........................*/
    var taxAmountGlobalCharges;
    var chargejsonTax;
    var taxAmountGlobal;
    var GlobalCurChargeTaxAmt;
    var ChargegstcstvatGlobalName;
    var GlobalCurTaxAmt = 0;
    var rowEditCtrl;
    var globalRowIndex;
    var globalTaxRowIndex;
    var gstcstvatGlobalName;
    var taxJson;
    function Save_TaxClick() {
        if (gridTax.GetVisibleRowsOnPage() > 0) {
            gridTax.UpdateEdit();
        }
        else {
            gridTax.PerformCallback('SaveGst');
        }
        cPopup_Taxes.Hide();
    }
    //Set Running Total for Charges And Tax 
    function SetChargesRunningTotal() {
        var runningTot = parseFloat(ctxtProductNetAmount.GetValue());

        for (var i = 0; i < chargejsonTax.length; i++) {
            gridTax.batchEditApi.StartEdit(i, 3);
            if (chargejsonTax[i].applicableOn == "R") {
                gridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                var totLength = gridTax.GetEditor("TaxName").GetText().length;
                var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
                var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
                var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
                var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
                var GlobalTaxAmt = 0;

                var Percentage = gridTax.GetEditor("Percentage").GetText();
                var totLength = gridTax.GetEditor("TaxName").GetText().length;
                var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
                Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

                if (sign == '(+)') {
                    GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                    gridTax.GetEditor("Amount").SetValue(Sum);
                    ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                    ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                    //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
                    GlobalTaxAmt = 0;
                }
                else {
                    GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                    gridTax.GetEditor("Amount").SetValue(Sum);
                    ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                    ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                    //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                    GlobalTaxAmt = 0;
                }

                SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());


            }
            runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
            gridTax.batchEditApi.EndEdit();
        }
    }
    function GetPercentageData() {
        var Amount = ctxtProductAmount.GetValue();
        var GlobalTaxAmt = 0;
        var noofvisiblerows = gridTax.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i, cnt = 1;
        var sumAmount = 0, totalAmount = 0;
        for (i = 0 ; cnt <= noofvisiblerows ; i++) {
            var totLength = gridTax.batchEditApi.GetCellValue(i, 'TaxName').length;
            var sign = gridTax.batchEditApi.GetCellValue(i, 'TaxName').substring(totLength - 3);
            var DisAmount = (gridTax.batchEditApi.GetCellValue(i, 'Amount') != null) ? (gridTax.batchEditApi.GetCellValue(i, 'Amount')) : "0";

            if (sign == '(+)') {
                sumAmount = sumAmount + parseFloat(DisAmount);
            }
            else {
                sumAmount = sumAmount - parseFloat(DisAmount);
            }

            cnt++;
        }

        totalAmount = (parseFloat(Amount)) + (parseFloat(sumAmount));
        // ctxtTotalAmount.SetValue(totalAmount);
    }
    function Save_TaxesClick() {
        grid.batchEditApi.EndEdit();
        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i, cnt = 1;
        var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

        cnt = 1;
        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
            var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
            var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
            var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
            var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            sumAmount = sumAmount + parseFloat(Amount);
            sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
            sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
            sumNetAmount = sumNetAmount + parseFloat(NetAmount);

            cnt++;
        }

        if (sumAmount == 0 && sumTaxAmount == 0 && Discount == 0) {
            cnt = 1;
            for (i = 0 ; cnt <= noofvisiblerows ; i++) {
                var Amount = (grid.batchEditApi.GetCellValue(i, 'gvColAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColAmount')) : "0";
                var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTaxAmount')) : "0";
                var Discount = (grid.batchEditApi.GetCellValue(i, 'gvColDiscount') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColDiscount')) : "0";
                var NetAmount = (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR') != null) ? (grid.batchEditApi.GetCellValue(i, 'gvColTotalAmountINR')) : "0";
                var sumDiscountAmt = ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                sumAmount = sumAmount + parseFloat(Amount);
                sumTaxAmount = sumTaxAmount + parseFloat(TaxAmount);
                sumDiscount = sumDiscount + parseFloat(sumDiscountAmt);
                sumNetAmount = sumNetAmount + parseFloat(NetAmount);

                cnt++;
            }
        }

        //Debjyoti 
        document.getElementById('HdChargeProdAmt').value = sumAmount;
        document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;
        //End Here

        ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
        ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
        ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));
        ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
        clblChargesTaxableGross.SetText("");
        clblChargesTaxableNet.SetText("");

        //Checking is gstcstvat will be hidden or not
        if (cddl_AmountAre.GetValue() == "2") {

            $('.lblChargesGSTforGross').show();
            $('.lblChargesGSTforNet').show();

            //Set Gross Amount with GstValue
            //Get The rate of Gst
            var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);
            if (gstRate) {
                if (gstRate != 0) {
                    var gstDis = (gstRate / 100) + 1;
                    if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                        $('.lblChargesGSTforNet').hide();
                        ctxtProductAmount.SetText(Math.round(sumAmount / gstDis).toFixed(2));
                        document.getElementById('HdChargeProdAmt').value = Math.round(sumAmount / gstDis).toFixed(2);
                        clblChargesGSTforGross.SetText(Math.round(sumAmount - parseFloat(document.getElementById('HdChargeProdAmt').value)).toFixed(2));
                        clblChargesTaxableGross.SetText("(Taxable)");

                    }
                    else {
                        $('.lblChargesGSTforGross').hide();
                        ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                        document.getElementById('HdChargeProdNetAmt').value = Math.round(sumNetAmount / gstDis).toFixed(2);
                        clblChargesGSTforNet.SetText(Math.round(sumNetAmount - parseFloat(document.getElementById('HdChargeProdNetAmt').value)).toFixed(2));
                        clblChargesTaxableNet.SetText("(Taxable)");
                    }
                }

            } else {
                $('.lblChargesGSTforGross').hide();
                $('.lblChargesGSTforNet').hide();
            }
        }
        else if (cddl_AmountAre.GetValue() == "1") {
            $('.lblChargesGSTforGross').hide();
            $('.lblChargesGSTforNet').hide();
        }
        //End here





        //Set Total amount
        ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));

        gridTax.PerformCallback('Display');
        //Checking is gstcstvat will be hidden or not
        if (cddl_AmountAre.GetValue() == "2") {
            $('.chargeGstCstvatClass').hide();
        }
        else if (cddl_AmountAre.GetValue() == "1") {
            $('.chargeGstCstvatClass').show();
        }
        //End here
        $('.RecalculateCharge').hide();
        cPopup_Taxes.Show();
        gridTax.StartEditRow(0);
    }
    //function SetFocusonDemand(e) {
    //    var key = cddl_AmountAre.GetValue();
    //    if (key == '1' || key == '3') {
    //        if (grid.GetVisibleRowsOnPage() == 1) {
    //            grid.batchEditApi.StartEdit(-1, 2);
    //        }
    //    }
    //    else if (key == '2') {
    //        cddlVatGstCst.Focus();
    //    }

    //}
    function QuotationTaxAmountTextChange(s, e) {
        //var Amount = ctxtProductAmount.GetValue();
        var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
        var GlobalTaxAmt = 0;
        //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
        //var Percentage = s.GetText();
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        //Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

        if (sign == '(+)') {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            //gridTax.GetEditor("Amount").SetValue(Sum);
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + GlobalTaxAmt - taxAmountGlobalCharges);
            ctxtTotalAmount.SetText(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
            GlobalTaxAmt = 0;
            SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
        }
        else {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            //gridTax.GetEditor("Amount").SetValue(Sum);
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
            ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
            GlobalTaxAmt = 0;
            SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
        }

        RecalCulateTaxTotalAmountCharges();
    }
    function OnTaxEndCallback(s, e) {
        GetPercentageData();
        $('.gridTaxClass').show();
        if (gridTax.GetVisibleRowsOnPage() == 0) {
            $('.gridTaxClass').hide();
            ccmbGstCstVatcharge.Focus();
        }
        else {
            gridTax.StartEditRow(0);
        }
        //check Json data
        if (gridTax.cpJsonChargeData) {
            if (gridTax.cpJsonChargeData != "") {
                chargejsonTax = JSON.parse(gridTax.cpJsonChargeData);
                gridTax.cpJsonChargeData = null;
            }
        }

        //Set Total Charges And total Amount
        if (gridTax.cpTotalCharges) {
            if (gridTax.cpTotalCharges != "") {
                ctxtQuoteTaxTotalAmt.SetValue(gridTax.cpTotalCharges);
                ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
                gridTax.cpTotalCharges = null;
            }
        }


        SetChargesRunningTotal();
        ShowTaxPopUp("IN");
    }
    function QuotationTaxAmountGotFocus(s, e) {
        taxAmountGlobalCharges = parseFloat(s.GetValue());
    }
    function PercentageTextChange(s, e) {
        //var Amount = ctxtProductAmount.GetValue();
        var Amount = gridTax.GetEditor("calCulatedOn").GetValue();
        var GlobalTaxAmt = 0;
        //var Percentage = (gridTax.GetEditor('Percentage').GetValue() != null) ? gridTax.GetEditor('Percentage').GetValue() : "0";
        var Percentage = s.GetText();
        var totLength = gridTax.GetEditor("TaxName").GetText().length;
        var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        Sum = ((parseFloat(Amount) * parseFloat(Percentage)) / 100);

        if (sign == '(+)') {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            gridTax.GetEditor("Amount").SetValue(Sum);
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
            ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) + parseFloat(Sum) - GlobalTaxAmt);
            GlobalTaxAmt = 0;
        }
        else {
            GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
            gridTax.GetEditor("Amount").SetValue(Sum);
            ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
            ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
            //ctxtTotalAmount.SetText(parseFloat(ctxtTotalAmount.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
            GlobalTaxAmt = 0;
        }

        SetOtherChargeTaxValueOnRespectiveRow(0, Sum, gridTax.GetEditor("TaxName").GetText());
        SetChargesRunningTotal();

        RecalCulateTaxTotalAmountCharges();
    }
    function RecalCulateTaxTotalAmountCharges() {
        var totalTaxAmount = 0;
        for (var i = 0; i < chargejsonTax.length; i++) {
            gridTax.batchEditApi.StartEdit(i, 3);
            var totLength = gridTax.GetEditor("TaxName").GetText().length;
            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                totalTaxAmount = totalTaxAmount + parseFloat(gridTax.GetEditor("Amount").GetValue());
            } else {
                totalTaxAmount = totalTaxAmount - parseFloat(gridTax.GetEditor("Amount").GetValue());
            }

            gridTax.batchEditApi.EndEdit();
        }

        totalTaxAmount = totalTaxAmount + parseFloat(ctxtGstCstVatCharge.GetValue());

        ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
        ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
    }

    function ShowTaxPopUp(type) {
        if (type == "IY") {
            $('#ContentErrorMsg').hide();
            $('#content-6').show();


            if (ccmbGstCstVat.GetItemCount() <= 1) {
                $('.InlineTaxClass').hide();
            } else {
                $('.InlineTaxClass').show();
            }
            if (cgridTax.GetVisibleRowsOnPage() < 1) {
                $('.cgridTaxClass').hide();

            } else {
                $('.cgridTaxClass').show();
            }

            if (ccmbGstCstVat.GetItemCount() <= 1 && cgridTax.GetVisibleRowsOnPage() < 1) {
                $('#ContentErrorMsg').show();
                $('#content-6').hide();
            }
        }
        if (type == "IN") {
            $('#ErrorMsgCharges').hide();
            $('#content-5').show();

            if (ccmbGstCstVatcharge.GetItemCount() <= 1) {
                $('.chargesDDownTaxClass').hide();
            } else {
                $('.chargesDDownTaxClass').show();
            }
            if (gridTax.GetVisibleRowsOnPage() < 1) {
                $('.gridTaxClass').hide();

            } else {
                $('.gridTaxClass').show();
            }

            if (ccmbGstCstVatcharge.GetItemCount() <= 1 && gridTax.GetVisibleRowsOnPage() < 1) {
                $('#ErrorMsgCharges').show();
                $('#content-5').hide();
            }
        }
    }
    function gridFocusedRowChanged(s, e) {
        globalRowIndex = e.visibleIndex;
    }
    function TaxAmountKeyDown(s, e) {

        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
    }


    function taxAmountGotFocus(s, e) {
        taxAmountGlobal = parseFloat(s.GetValue());
    }

    function taxAmountLostFocus(s, e) {
        var finalTaxAmt = parseFloat(s.GetValue());
        var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        if (sign == '(+)') {
            ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
        } else {
            ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
        }


        SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        //Set Running Total
        SetRunningTotal();

        RecalCulateTaxTotalAmountInline();
    }

    function cmbGstCstVatChange(s, e) {

        SetOtherTaxValueOnRespectiveRow(0, 0, gstcstvatGlobalName);
        $('.RecalculateInline').hide();
        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        if (s.GetValue().split('~')[2] == 'G') {
            ProdAmt = parseFloat(clblTaxProdGrossAmt.GetValue());
        }
        else if (s.GetValue().split('~')[2] == 'N') {
            ProdAmt = parseFloat(clblProdNetAmt.GetValue());
        }
        else if (s.GetValue().split('~')[2] == 'O') {
            //Check for Other Dependecy
            $('.RecalculateInline').show();
            ProdAmt = 0;
            var taxdependentName = s.GetValue().split('~')[3];
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                var gridTaxName = cgridTax.GetEditor("Taxes_Name").GetText();
                gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                if (gridTaxName == taxdependentName) {
                    ProdAmt = cgridTax.GetEditor("Amount").GetValue();
                }
            }
        }
        else if (s.GetValue().split('~')[2] == 'R') {
            ProdAmt = GetTotalRunningAmount();
            $('.RecalculateInline').show();
        }

        GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());

        var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVat.GetValue().split('~')[1]) / 100;
        ctxtGstCstVat.SetValue(calculatedValue);

        var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
        ctxtTaxTotAmt.SetValue(Math.round(totAmt + calculatedValue - GlobalCurTaxAmt));

        //tax others
        SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
        gstcstvatGlobalName = ccmbGstCstVat.GetText();
    }


    //for tax and charges

    function ChargecmbGstCstVatChange(s, e) {

        SetOtherChargeTaxValueOnRespectiveRow(0, 0, ChargegstcstvatGlobalName);
        $('.RecalculateCharge').hide();
        var ProdAmt = parseFloat(ctxtProductAmount.GetValue());

        //Set ProductAmount
        if (s.GetValue().split('~')[2] == 'G') {
            ProdAmt = parseFloat(ctxtProductAmount.GetValue());
        }
        else if (s.GetValue().split('~')[2] == 'N') {
            ProdAmt = parseFloat(clblProdNetAmt.GetValue());
        }
        else if (s.GetValue().split('~')[2] == 'O') {
            //Check for Other Dependecy
            $('.RecalculateCharge').show();
            ProdAmt = 0;
            var taxdependentName = s.GetValue().split('~')[3];
            for (var i = 0; i < taxJson.length; i++) {
                gridTax.batchEditApi.StartEdit(i, 3);
                var gridTaxName = gridTax.GetEditor("TaxName").GetText();
                gridTaxName = gridTaxName.substring(0, gridTaxName.length - 3).trim();
                if (gridTaxName == taxdependentName) {
                    ProdAmt = gridTax.GetEditor("Amount").GetValue();
                }
            }
        }
        else if (s.GetValue().split('~')[2] == 'R') {
            $('.RecalculateCharge').show();
            ProdAmt = GetChargesTotalRunningAmount();
        }


        GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());

        var calculatedValue = parseFloat(ProdAmt * ccmbGstCstVatcharge.GetValue().split('~')[1]) / 100;
        ctxtGstCstVatCharge.SetValue(calculatedValue);

        var totAmt = parseFloat(ctxtQuoteTaxTotalAmt.GetText());
        ctxtQuoteTaxTotalAmt.SetValue(totAmt + calculatedValue - GlobalCurChargeTaxAmt);

        //tax others
        SetOtherChargeTaxValueOnRespectiveRow(0, calculatedValue, ctxtGstCstVatCharge.GetText());
        ChargegstcstvatGlobalName = ctxtGstCstVatCharge.GetText();

        //set Total Amount
        ctxtTotalAmount.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) + parseFloat(ctxtProductNetAmount.GetValue()));
    }




    function GetChargesTotalRunningAmount() {
        var runningTot = parseFloat(ctxtProductNetAmount.GetValue());
        for (var i = 0; i < chargejsonTax.length; i++) {
            gridTax.batchEditApi.StartEdit(i, 3);
            runningTot = runningTot + parseFloat(gridTax.GetEditor("Amount").GetValue());
            gridTax.batchEditApi.EndEdit();
        }

        return runningTot;
    }

    function chargeCmbtaxClick(s, e) {
        GlobalCurChargeTaxAmt = parseFloat(ctxtGstCstVatCharge.GetText());
        ChargegstcstvatGlobalName = s.GetText();
    }




    function GetVisibleIndex(s, e) {
        globalRowIndex = e.visibleIndex;
    }
    function GetTaxVisibleIndex(s, e) {
        globalTaxRowIndex = e.visibleIndex;
    }
    function cmbtaxCodeindexChange(s, e) {
        if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

            var taxValue = s.GetValue();

            if (taxValue == null) {
                taxValue = 0;
                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(0);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
            }


            var isValid = taxValue.indexOf('~');
            if (isValid != -1) {
                var rate = parseFloat(taxValue.split('~')[1]);
                var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            }
            else {
                s.SetText("");
            }

        } else {
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

            if (s.GetValue() == null) {
                s.SetValue(0);
            }

            if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                GlobalCurTaxAmt = 0;
            } else {
                s.SetText("");
            }
        }

    }

    function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
        for (var i = 0; i < taxJson.length; i++) {
            if (taxJson[i].applicableBy == name) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                cgridTax.GetEditor('calCulatedOn').SetValue(amt);

                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                var s = cgridTax.GetEditor("TaxField");
                if (sign == '(+)') {
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                }
                else {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                    GlobalCurTaxAmt = 0;
                }




            }
        }
        //return;
        cgridTax.batchEditApi.EndEdit();

    }



    function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
        name = name.substring(0, name.length - 3).trim();
        for (var i = 0; i < chargejsonTax.length; i++) {
            if (chargejsonTax[i].applicableBy == name) {
                gridTax.batchEditApi.StartEdit(i, 3);
                gridTax.GetEditor('calCulatedOn').SetValue(amt);

                var totLength = gridTax.GetEditor("TaxName").GetText().length;
                var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
                var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
                var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
                var s = gridTax.GetEditor("Percentage");
                if (sign == '(+)') {
                    GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                    gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                }
                else {

                    GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                    gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                    GlobalCurTaxAmt = 0;
                }




            }
        }
        //return;
        gridTax.batchEditApi.EndEdit();
    }



    function txtPercentageLostFocus(s, e) {

        //var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        if (s.GetText().trim() != '') {

            if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
                //Checking Add or less
                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                if (sign == '(+)') {
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                    GlobalCurTaxAmt = 0;
                }
                else {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                    ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                    GlobalCurTaxAmt = 0;
                }
                SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

                //Call for Running Total
                SetRunningTotal();

            } else {
                s.SetText("");
            }
        }

        RecalCulateTaxTotalAmountInline();
    }

    function RecalCulateTaxTotalAmountInline() {
        var totalInlineTaxAmount = 0;
        for (var i = 0; i < taxJson.length; i++) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
            } else {
                totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
            }

            cgridTax.batchEditApi.EndEdit();
        }

        totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());

        ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
    }


    function SetRunningTotal() {
        var runningTot = parseFloat(clblProdNetAmt.GetValue());
        for (var i = 0; i < taxJson.length; i++) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            if (taxJson[i].applicableOn == "R") {
                cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                var thisRunningAmt = 0;
                if (sign == '(+)') {
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                    GlobalCurTaxAmt = 0;
                }
                else {

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                    GlobalCurTaxAmt = 0;
                }
                SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            }
            runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.batchEditApi.EndEdit();
        }
    }

    function GetTotalRunningAmount() {
        var runningTot = parseFloat(clblProdNetAmt.GetValue());
        for (var i = 0; i < taxJson.length; i++) {
            cgridTax.batchEditApi.StartEdit(i, 3);
            runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
            cgridTax.batchEditApi.EndEdit();
        }

        return runningTot;
    }




    function CmbtaxClick(s, e) {
        GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
        gstcstvatGlobalName = s.GetText();
    }


    function txtTax_TextChanged(s, i, e) {
        cgridTax.batchEditApi.StartEdit(i, 2);
        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
    }



    function BatchUpdate() {

        //cgridTax.batchEditApi.StartEdit(0, 1);

        //if (cgridTax.GetEditor("TaxField").GetText().indexOf('.') == -1) {
        //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '.00');
        //} else {
        //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '0');
        //}
        if (cgridTax.GetVisibleRowsOnPage() > 0) {
            cgridTax.UpdateEdit();
        }
        else {
            cgridTax.PerformCallback('SaveGST');
        }
        return false;
    }


    function cgridTax_EndCallBack(s, e) {
        //cgridTax.batchEditApi.StartEdit(0, 1);
        $('.cgridTaxClass').show();

        cgridTax.StartEditRow(0);


        //check Json data
        if (cgridTax.cpJsonData) {
            if (cgridTax.cpJsonData != "") {
                taxJson = JSON.parse(cgridTax.cpJsonData);
                cgridTax.cpJsonData = null;
            }
        }
        //End Here

        if (cgridTax.cpComboCode) {
            if (cgridTax.cpComboCode != "") {
                if (cddl_AmountAre.GetValue() == "1") {
                    var selectedIndex;
                    for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
                        if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
                            selectedIndex = i;
                        }
                    }
                    if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
                        ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
                    }
                    cmbGstCstVatChange(ccmbGstCstVat);
                    cgridTax.cpComboCode = null;
                }
            }
        }

        if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
            ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
            var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
            var ddValue = parseFloat(ctxtGstCstVat.GetValue());
            ctxtTaxTotAmt.SetValue(Math.round(gridValue + ddValue));
            cgridTax.cpUpdated = "";
        }

        else {
            var totAmt = ctxtTaxTotAmt.GetValue();
            cgridTax.CancelEdit();
            caspxTaxpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex, 13);
            grid.GetEditor("gvColTaxAmount").SetValue(totAmt);
            grid.GetEditor("gvColTotalAmountINR").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()));

        }

        if (cgridTax.GetVisibleRowsOnPage() == 0) {
            $('.cgridTaxClass').hide();
            ccmbGstCstVat.Focus();
        }
        //Debjyoti Check where any Gst Present or not
        // If Not then hide the hole section

        SetRunningTotal();
        ShowTaxPopUp("IY");
    }

    function recalculateTax() {
        cmbGstCstVatChange(ccmbGstCstVat);
    }
    function recalculateTaxCharge() {
        ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
    }




    /*............................End Tax...........................................*/




    function PerformCallToGridBind() {

        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
        $('#hdnPageStatus').val('Quoteupdate');
        cProductsPopup.Hide();
        return false;
    }

    function componentEndCallBack(s, e) {
        if (cQuotationComponentPanel.cpNullGrid != null) {
            deleteAllRows();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }

            grid.GetEditor('ProductName').SetEnabled(true);
            cPLQADate.SetText('');
        }
        else {
            gridquotationLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {

                OnAddNewClick();
                grid.GetEditor('ProductName').SetEnabled(true);
                cPLQADate.SetText('');
            }
        }

    }
    function CloseGridQuotationLookup() {
        gridquotationLookup.ConfirmCurrentSelection();
        gridquotationLookup.HideDropDown();
        gridquotationLookup.Focus();
    }

    function QuotationNumberChanged() {

        document.getElementById('hdfTagMendatory').value = 'No';

        $("#MandatorysIndentReq").hide();
        var quote_Id = gridquotationLookup.GetValue();
        if (quote_Id != null) {
            var arr = quote_Id.split(',');
            if (arr.length > 1) {
                cPLQADate.SetText('Multiple Select Indent Dates');
            }
            else {
                if (arr.length == 1) {
                    cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);
                }
                else {
                    cPLQADate.SetText('');
                }
            }
        }
        else { cPLQADate.SetText(''); }

        if (quote_Id != null) {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            cProductsPopup.Show();
        }
        else {
            cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
            cProductsPopup.Show();
        }
        //txt_OANumber.Focus();           
    }
    function SetDifference1() {
        var diff = CheckDifferenceOfFromDateWithTodate();
    }
    function CheckDifferenceOfFromDateWithTodate() {
        var startDate = new Date();
        var endDate = new Date();
        var difference = -1;
        startDate = cPLSalesOrderDate.GetDate();
        if (startDate != null) {
            endDate = cExpiryDate.GetDate();
            var startTime = startDate.getTime();
            var endTime = endDate.getTime();
            difference = (startTime - endTime) / 86400000;

        }
        return difference;

    }
    function SetDifference() {
        var diff = CheckDifference();
    }
    function CheckDifference() {
        var startDate = new Date();
        var endDate = new Date();
        var difference = -1;
        startDate = cPLSalesOrderDate.GetDate();
        if (startDate != null) {
            endDate = cExpiryDate.GetDate();
            var startTime = startDate.getTime();
            var endTime = endDate.getTime();
            difference = (endTime - startTime) / 86400000;

        }
        return difference;

    }
    //.................WareHouse.......


    //...............end..........................
    //...............Addeess Part.......

    //function ClosebillingLookup() {
    //    billingLookup.ConfirmCurrentSelection();
    //    billingLookup.HideDropDown();
    //    billingLookup.Focus();
    //}
    //function CloseshippingLookup() {
    //    shippingLookup.ConfirmCurrentSelection();
    //    shippingLookup.HideDropDown();
    //    shippingLookup.Focus();
    //}

    //.......end........
    //function GetVisibleIndex(s, e) {
    //    globalRowIndex = e.visibleIndex;
    //}
    function BtnVisible() {
        document.getElementById('btnSaveExit').style.display = 'none'
        document.getElementById('btnnew').style.display = 'none'
        document.getElementById('tagged').style.display = 'block'

    }

    function OnEndCallback(s, e) {
        if (grid.cpBtnVisible != null && grid.cpBtnVisible != "") {
            grid.cpBtnVisible = null;
            BtnVisible();
        }
        var value = document.getElementById('hdnRefreshType').value;
        var pageStatus = document.getElementById('hdnPageStatus').value;


        if (grid.cpSaveSuccessOrFail == "outrange") {
            grid.batchEditApi.StartEdit(0, 2);
            jAlert('Can Not Add More Purchase Order Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
            //OnAddNewClick();
        }
        else if (grid.cpSaveSuccessOrFail == "duplicate") {
            grid.batchEditApi.StartEdit(0, 2);
            OnAddNewClick();
            jAlert('Can Not Save as Duplicate Quotation Numbe No. Found');
            //if (grid.GetVisibleRowsOnPage() == 0) {
            //    OnAddNewClick();
            //}
        }
        else if (grid.cpSaveSuccessOrFail == "UdfMandetory") {
            grid.batchEditApi.StartEdit(0, 2);
            OnAddNewClick();
            jAlert('UDF is set as Mandatory.Please enter values.', 'Alert', function () { OpenUdf(); });

        }
        else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
            OnAddNewClick();
            jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });

            grid.cpSaveSuccessOrFail = null;
        }
        else if (grid.cpSaveSuccessOrFail == "errorInsert") {
            //grid.batchEditApi.StartEdit(0, 2);
            OnAddNewClick();
            jAlert('Please try after sometime.');
            grid.cpSaveSuccessOrFail = null;
            //if (grid.GetVisibleRowsOnPage() == 0) {
            //    OnAddNewClick();
            //}
        }
        else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
            OnAddNewClick();
            grid.cpSaveSuccessOrFail = null;
            jAlert('Cannot save. Entered quantity must be greater then ZERO(0).');
        }
        else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
            //grid.batchEditApi.StartEdit(0, 2);
            OnAddNewClick();
            jAlert('Cannot Save. Selected Purchase Indent(s) in this document do not exist.');
        }
        else {

            var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
            var Order_Msg = "Purchase Order No. " + PurchaseOrder_Number + " saved.";
            if (value == "E") {
                if (grid.cpApproverStatus == "approve") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                else if (grid.cpApproverStatus == "rejected") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                // window.location.assign("PurchaseOrderList.aspx");
                if (PurchaseOrder_Number != "") {
                    jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {
                        if (r == true) {
                            grid.cpSalesOrderNo = null;
                            window.location.assign("VendorDashboard.aspx");
                        }
                    });

                }
                else {
                    window.location.assign("VendorDashboard.aspx");
                }
            }
            else if (value == "N") {
                if (grid.cpApproverStatus == "approve") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                else if (grid.cpApproverStatus == "rejected") {
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                }
                if (PurchaseOrder_Number != "") {
                    jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                        grid.cpSalesOrderNo = null;
                        if (r == true) {
                            window.location.assign("PurchaseOrder.aspx?key=ADD");
                        }
                    });


                }
                else {
                    window.location.assign("PurchaseOrder.aspx?key=ADD");

                }
            }
            else {
                if (pageStatus == "first") {
                    if (grid.GetVisibleRowsOnPage() == 0) {
                        OnAddNewClick();
                    }
                    grid.batchEditApi.EndEdit();
                    FinYearCheckOnPageLoad();
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                    GetIndentReqNoOnLoad();

                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    if ($("#ddl_Currency").val() == basedCurrency[0]) {
                        ctxtRate.SetEnabled(false);
                    }
                }
                   <%-- else if (pageStatus == "update") {

                        OnAddNewClick();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }--%>
                else if (pageStatus == "Quoteupdate") {
                    //OnAddGridNewClick();
                    grid.StartEditRow(0);
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    if ($("#ddl_Currency").val() == basedCurrency[0]) {
                        ctxtRate.SetEnabled(false);
                    }
                    // GetIndentReqNoOnLoad();
                }
                else if (pageStatus == "delete") {
                    // grid.StartEditRow(0);
                    OnAddNewClick();
                    $('#<%=hdnPageStatus.ClientID %>').val('');
                }
    }
}

    if (gridquotationLookup.GetValue() != null) {
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('gvColDiscription').SetEnabled(false);
        grid.StartEditRow(0);
        $('#<%=hdnPageStatus.ClientID %>').val('');
    }
    else {
        grid.GetEditor('ProductName').SetEnabled(true);
        grid.GetEditor('gvColDiscription').SetEnabled(true);
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
        }
    }
    cProductsPopup.Hide();
}

function GridCallBack() {
    //page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    grid.PerformCallback('Display');
}
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
        if (gridquotationLookup.GetValue() != null) {
            //jAlert();
            jAlert('Cannot Delete using this button as the Purchase Indent is linked with this Purchase Order.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }
        var noofvisiblerows = grid.GetVisibleRowsOnPage();
        //if (noofvisiblerows != "1" && lookup_quotation.GetValue() == null) {
        if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');
            $('#<%=hdnPageStatus.ClientID %>').val('delete');
            // grid.batchEditApi.StartEdit(-1, 2);
            // grid.batchEditApi.StartEdit(0, 2);
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {

        if (gridquotationLookup.GetValue() == null) {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            var Product = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";

            if (Product != "") {

                OnAddNewClick();


                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 500);
                return false;
            }
            else {
                // grid.batchEditApi.StartEdit(e.visibleIndex, 3);
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 50);
                return false;
                //grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }
        }
        else {
            QuotationNumberChanged();
        }
        //var key = s.GetRowKey(e.visibleIndex);
        //if (key == 9) {
        //    cbtn_SaveRecords.Focus();
        //}



    }
    else if (e.buttonID == 'CustomWarehouse') {
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(index, 2)
        Warehouseindex = index;

        var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
        if (QuantityValue == "0.0") {
            jAlert("Quantity should not be zero !.");
        } else {

            $("#spnCmbWarehouse").hide();
            $("#spnCmbBatch").hide();
            $("#spncheckComboBox").hide();
            $("#spntxtQuantity").hide();

            if (ProductID != "") {
                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strStkUOM = SpliteDetails[4];
                var strMultiplier = SpliteDetails[7];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "";
                var StkQuantityValue = QuantityValue * strMultiplier;

                $('#<%=hdfProductIDPC.ClientID %>').val(strProductID);
                $('#<%=hdfProductType.ClientID %>').val("");
                $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                var Ptype = "";

                $('#<%=hdnisserial.ClientID %>').val("");
                $('#<%=hdnisbatch.ClientID %>').val("");
                $('#<%=hdniswarehouse.ClientID %>').val("");
                document.getElementById('<%=lblAvailableStkunit.ClientID %>').innerHTML = strUOM;
                document.getElementById('<%=lblopeningstockUnit.ClientID %>').innerHTML = strUOM;
                $.ajax({
                    type: "POST",
                    url: 'PurchaseChallan.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + strProductID + "\"}",
                    success: function (type) {
                        Ptype = type.d;
                        $('#<%=hdfProductType.ClientID %>').val(Ptype);
                        //alert(Ptype);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            if (Ptype == "W") {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                        $('#<%=hdnisbatch.ClientID %>').val("false");
                                        $('#<%=hdniswarehouse.ClientID %>').val("true");
                                        //cCmbWarehouse.PerformCallback('BindWarehouse');

                                    }

                                    else if (Ptype == "B") {
                                        $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");

                            }
                            else if (Ptype == "S") {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");

                            }
                            else if (Ptype == "WB") {

                                $('#<%=hdnisserial.ClientID %>').val("false");
                            $('#<%=hdnisbatch.ClientID %>').val("true");
                            $('#<%=hdniswarehouse.ClientID %>').val("true");
                            //cCmbWarehouse.PerformCallback('BindWarehouse');
                            //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                        }
                        else if (Ptype == "WS") {
                            $('#<%=hdnisserial.ClientID %>').val("true");
                            $('#<%=hdnisbatch.ClientID %>').val("false");
                            $('#<%=hdniswarehouse.ClientID %>').val("true");
                            //cCmbWarehouse.PerformCallback('BindWarehouse');
                            //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                        }
                        else if (Ptype == "WBS") {
                            $('#<%=hdnisserial.ClientID %>').val("true");
                            $('#<%=hdnisbatch.ClientID %>').val("true");
                            $('#<%=hdniswarehouse.ClientID %>').val("true");
                            //cCmbWarehouse.PerformCallback('BindWarehouse');
                            //cGrdWarehousePC.PerformCallback('Display~' + SrlNo);
                        }
                        else if (Ptype == "BS") {
                            $('#<%=hdnisserial.ClientID %>').val("true");
                            $('#<%=hdnisbatch.ClientID %>').val("true");
                            $('#<%=hdniswarehouse.ClientID %>').val("false");


                        }
                        else {
                            $('#<%=hdnisserial.ClientID %>').val("false");
                            $('#<%=hdnisbatch.ClientID %>').val("false");
                            $('#<%=hdniswarehouse.ClientID %>').val("false");
                        }

    $("#RequiredFieldValidatortxtbatch").css("display", "none");
    $("#RequiredFieldValidatortxtserial").css("display", "none");
    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");

    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");

    $(".blockone").css("display", "none");
    $(".blocktwo").css("display", "none");
    $(".blockthree").css("display", "none");

    ctxtqnty.SetText("0.0");
    ctxtqnty.SetEnabled(true);

    ctxtbatchqnty.SetText("0.0");
    ctxtserial.SetText("");
    ctxtbatchqnty.SetText("");

    ctxtbatch.SetEnabled(true);
    cCmbWarehouse.SetEnabled(true);

    $('#<%=hdnoutstock.ClientID %>').val("0");
    $('#<%=hdnisedited.ClientID %>').val("false");
                                    $('#<%=hdnisoldupdate.ClientID %>').val("false");
                                    $('#<%=hdnisnewupdate.ClientID %>').val("false");

                                    $('#<%=hdnisolddeleted.ClientID %>').val("false");

                                    $('#<%=hdntotalqntyPC.ClientID %>').val(0);
                                    $('#<%=hdnoldrowcount.ClientID %>').val(0);
                                    $('#<%=hdndeleteqnity.ClientID %>').val(0);
                                    $('#<%=hidencountforserial.ClientID %>').val("1");

                                    $('#<%=hdfstockidPC.ClientID %>').val(0);
                                    $('#<%=hdfopeningstockPC.ClientID %>').val(0);
                                    $('#<%=oldopeningqntity.ClientID %>').val(0);
                                    $('#<%=hdnnewenterqntity.ClientID %>').val(0);

                                    $('#<%=hdnenterdopenqnty.ClientID %>').val(0);
                                    $('#<%=hdbranchIDPC.ClientID %>').val(0);

                                    $('#<%=hdnisviewqntityhas.ClientID %>').val("false");


                                    $('#<%=hdndefaultID.ClientID %>').val("");
                                    $('#<%=hdnbatchchanged.ClientID %>').val("0");
                                    $('#<%=hdnrate.ClientID %>').val("0");
                                    $('#<%=hdnvalue.ClientID %>').val("0");
                                    $('#<%=hdnstrUOM.ClientID %>').val(strUOM);

                                    var branchid = $("#ddl_Branch option:selected").val();

                                    $('#<%=hdnisreduing.ClientID %>').val("false");

                            var productid = strProductID ? strProductID : "";
                            var StkQuantityValue = QuantityValue ? QuantityValue : "0.0000";

                            var stockids = SpliteDetails[10];
                            var checkstockdecimalornot = StkQuantityValue.toString().split(".")[1]

                            $('#<%=hdnpcslno.ClientID %>').val(SrlNo);

                                    // var ProductName = SpliteDetails[12];
                            var ProductName = SpliteDetails[1];
                            var ratevalue = "0";
                            var rate = "0";

                            var branchid = $('#<%=ddl_Branch.ClientID %>').val();


                        var BranchNames = $("#ddl_Branch option:selected").text();

                        var strProductID = productid;
                        var strDescription = "";
                        var strUOM = (strUOM != null) ? strUOM : "0";
                        var strProductName = ProductName;

                        document.getElementById('<%=lblbranchName.ClientID %>').innerHTML = BranchNames;
                        var availablestock = SpliteDetails[12];
                        $('#<%=hdndefaultID.ClientID %>').val("0");

                        $('#<%=hdfstockidPC.ClientID %>').val(stockids);
                                    var calculateopein = Number(StkQuantityValue) - Number(availablestock);
                                    var oldopeing = 0;
                                    var oldqnt = Number(oldopeing);

                                    $('#<%=hdfopeningstockPC.ClientID %>').val(QuantityValue);
                            $('#<%=oldopeningqntity.ClientID %>').val(0);
                                    $('#<%=hdnnewenterqntity.ClientID %>').val(QuantityValue);
                                    $('#<%=hdnenterdopenqnty.ClientID %>').val(0);
                                    $('#<%=hdbranchIDPC.ClientID %>').val(branchid);
                                    $('#<%=hdnselectedbranch.ClientID %>').val(branchid);

                                    $('#<%=hdnrate.ClientID %>').val(rate);
                                    $('#<%=hdnvalue.ClientID %>').val(ratevalue);

                                    var dtd = (Number(StkQuantityValue)).toFixed(4);


                                    $("#lblopeningstock").text(dtd);
                                    ctxtmkgdate.SetDate = null;
                                    txtexpirdate.SetDate = null;
                                    ctxtserial.SetValue("");
                                    ctxtbatch.SetValue("");
                                    ctxtqnty.SetValue("0.0");
                                    ctxtbatchqnty.SetValue("0.0");

                                    var hv = $('#hdnselectedbranch').val();

                                    var iswarehousactive = $('#hdniswarehouse').val();
                                    var isactivebatch = $('#hdnisbatch').val();
                                    var isactiveserial = $('#hdnisserial').val();


                                    cCmbWarehouse.PerformCallback('BindWarehouse');

                                    if (iswarehousactive == "true") {

                                        cCmbWarehouse.SetVisible(true);
                                        cCmbWarehouse.SetSelectedIndex(1);
                                        cCmbWarehouse.Focus();
                                        ctxtqnty.SetVisible(true);
                                        $('#<%=hdniswarehouse.ClientID %>').val("true");

                                $(".blockone").css("display", "block");

                            } else {
                                cCmbWarehouse.SetVisible(false);
                                ctxtqnty.SetVisible(false);
                                $('#<%=hdniswarehouse.ClientID %>').val("false");
                                cCmbWarehouse.SetSelectedIndex(-1);
                                $(".blockone").css("display", "none");

                            }

                            if (isactivebatch == "true") {

                                ctxtbatch.SetVisible(true);
                                ctxtmkgdate.SetVisible(true);
                                ctxtexpirdate.SetVisible(true);
                                $('#<%=hdnisbatch.ClientID %>').val("true");

                            $(".blocktwo").css("display", "block");

                        } else {
                            ctxtbatch.SetVisible(false);
                            ctxtmkgdate.SetVisible(false);
                            ctxtexpirdate.SetVisible(false);
                            $('#<%=hdnisbatch.ClientID %>').val("false");

                            $(".blocktwo").css("display", "none");

                        }
                        if (isactiveserial == "true") {
                            ctxtserial.SetVisible(true);
                            $('#<%=hdnisserial.ClientID %>').val("true");


                            $(".blockthree").css("display", "block");
                        } else {
                            ctxtserial.SetVisible(false);
                            $('#<%=hdnisserial.ClientID %>').val("false");


                            $(".blockthree").css("display", "none");
                        }

                        if (iswarehousactive == "false" && isactivebatch == "true") {
                            ctxtbatchqnty.SetVisible(true);

                            $(".blocktwoqntity").css("display", "block");
                        } else {
                            ctxtbatchqnty.SetVisible(false);
                            $(".blocktwoqntity").css("display", "none");
                        }

                        if (iswarehousactive == "false" && isactivebatch == "true") {
                            ctxtbatch.Focus();
                        } else {
                            cCmbWarehouse.Focus();
                        }

                        cbtnWarehouse.SetVisible(true);
                        cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

                        cPopup_WarehousePC.Show();
                    }
                    }
                });
        }





    }
}


}
function Save_ButtonClick() {
    flag = true;
    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    var Podt = cPLQuoteDate.GetValue();

    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        //flag = false;
        $("#MandatoryBillNo").show();
        return false;
    }
    if (Podt == null) {
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {

        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();

    if (TagMendatory == 'Yes') {
        $("#MandatorysIndentReq").show();

        return false;
    }
    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";


        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }

    if (flag != false) {
        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                $('#<%=hdfIsDelete.ClientID %>').val('I');
                $('#<%=hdnRefreshType.ClientID %>').val('N');
                grid.batchEditApi.EndEdit();
                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                grid.UpdateEdit();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }

        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
            <%--if (flag != false) {
                $('#<%=hdfIsDelete.ClientID %>').val('I');
                grid.batchEditApi.EndEdit();
                grid.UpdateEdit();
            }--%>
}
        function SaveExit_ButtonClick() {
            flag = true;
            var txtPurchaseNo = $("#txtVoucherNo").val();
            var ddl_Vendor = $("#ddl_Vendor").val();
            // alert(txtPurchaseNo);
            if (txtPurchaseNo == null || txtPurchaseNo == "") {
                flag = false;
                $("#MandatoryBillNo").show();
                return false;
            }
            var Podt = cPLQuoteDate.GetValue();
            if (Podt == null) {
                $("#MandatoryDate").show();
                return false;
            }
            var customerId = GetObjectID('hdnCustomerId').value
            if (customerId == '' || customerId == null) {

                $('#MandatorysVendor').attr('style', 'display:block');
                return false;
            }
            else {
                $('#MandatorysVendor').attr('style', 'display:none');
            }
            var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();

            if (TagMendatory == 'Yes') {
                $("#MandatorysIndentReq").show();
                return false;
            }
            var IsType = "";
            var frontRow = 0;
            var backRow = -1;

            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
                if (frontProduct != "" || backProduct != "") {
                    IsType = "Y";
                    break;
                }
                backRow--;
                frontRow++;
            }
            if (flag != false) {
                if (grid.GetVisibleRowsOnPage() > 0) {
                    if (IsType == "Y") {
                        var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        $('#<%=hdnRefreshType.ClientID %>').val('E');
                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                        grid.batchEditApi.EndEdit();
                        grid.UpdateEdit();
                    }
                    else {
                        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
                    }

                }
                else {
                    jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
                }
            }
        }
        function OnAddNewClick() {

            grid.AddNewRow();
            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var i;
            var cnt = 1;
            for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(cnt);


                cnt++;
            }
            //var tbQuotation = grid.GetEditor("SrlNo");
            //tbQuotation.SetValue(noofvisiblerows);

        }
        function ProductsCombo_SelectedIndexChanged(s, e) {

            var tbDescription = grid.GetEditor("gvColDiscription");
            var tbUOM = grid.GetEditor("gvColUOM");
            var tbStockUOM = grid.GetEditor("gvColStockUOM");
            var tbPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");

            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            // var ProductID = s.GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStockUOM = SpliteDetails[4];
            var strPurchasePrice = SpliteDetails[6];
            var strStockId = SpliteDetails[10];
            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbStockUOM.SetValue(strStockUOM);
            tbPurchasePrice.SetValue(strPurchasePrice);
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }

        }
        function ddl_Currency_Rate_Change() {
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = $("#ddl_Currency").val();


            if ($("#ddl_Currency").val() == basedCurrency[0]) {
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "PurchaseOrder.aspx/GetRate",
                    data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        ctxtRate.SetValue(data);
                    }
                });
                ctxtRate.SetEnabled(true);
            }
        }
        function ddl_AmountAre_valueChange() {
            var key = $("#ddl_AmountAre").val();
            if (key == 1) {
                // grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.PerformCallback('1');
            }
            else if (key == 2) {
                // grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');

            }
            else if (key == 3) {
                //  grid.GetEditor('TaxAmount').SetEnabled(false);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.PerformCallback('3');

            }
        }

        function GetIndentREquiNo(e) {

            var PODate = new Date();
            PODate = cPLQuoteDate.GetValueString();
            cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

            grid.batchEditApi.StartEdit(-1, 1);
            var accountingDataMin = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();
            console.log(accountingDataMin);

            grid.batchEditApi.StartEdit(0, 1);
            var accountingDataplus = grid.GetEditor('ProductName').GetValue();
            console.log(accountingDataplus);
            grid.batchEditApi.EndEdit();

            if (accountingDataMin != null || accountingDataplus != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        grid.PerformCallback('GridBlank');
                    }
                });
                //onBranchItems();
            }
        }

        function GetIndentReqNoOnLoad() {

            var PODate = new Date();
            PODate = cPLQuoteDate.GetValueString();
            cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

        }
        function GetContactPersonPhone(e) {
            var key = cContactPerson.GetValue();
            cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
        }
        function GetContactPerson(e) {
            //  GetIndentReqNoOnLoad();
            var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
            if (key != null && key != '') {
                cchkBilling.SetChecked(false);
                cchkShipping.SetChecked(false);
                cContactPerson.PerformCallback('BindContactPerson~' + key);
                page.GetTabByName('Billing/Shipping').SetEnabled(true);
                jConfirm('Wish to View/Enter Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(1);
                        $('.dxeErrorCellSys').addClass('abc');
                    }
                    else {
                        cContactPerson.Focus();
                    }
                });
                GetObjectID('hdnCustomerId').value = key;



            }

        }
        function ShowIndntRequisition() {

        }
        function cmbContactPersonEndCall(s, e) {

            //if (cContactPerson.cpDueDate != null) {
            //    var DeuDate = cContactPerson.cpDueDate;
            //    var myDate = new Date(DeuDate);

            //    cdt_SaleInvoiceDue.SetDate(myDate);
            //    cContactPerson.cpDueDate = null;
            //}

            if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {

                pageheaderContent.style.display = "block";

                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblTotalPayable.ClientID %>').innerHTML = cContactPerson.cpOutstanding;
                cContactPerson.cpOutstanding = null;
            }
            else {
                pageheaderContent.style.display = "none";

                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblTotalPayable.ClientID %>').innerHTML = '';
            }
            if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {

                $("#<%=divGSTIN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN;
                cContactPerson.cpGSTN = null;
            }
        }
        function acpContactPersonPhoneEndCall(s, e) {
            if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
                cacpContactPersonPhone.cpPhone = null;

            }
        }
        $(document).ready(function () {
            var schemaid = $('#ddl_numberingScheme').val();
            // alert(schemaid);
            if (schemaid != null) {
                if (schemaid == '0') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                }
            }
             <%-- region Sandip Section For Approval Section Start--%>
            $('#ApprovalCross').click(function () {

                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh()();
            })
            <%-- endregion Sandip Section For Approval Dtl Section End--%>
        });
        function CmbScheme_ValueChange() {

            // GetIndentReqNoOnLoad();
            var val = $("#ddl_numberingScheme").val();

            $.ajax({
                type: "POST",
                url: 'PurchaseOrder.aspx/getSchemeType',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{sel_scheme_id:\"" + val + "\"}",
                success: function (type) {

                    var schemetypeValue = type.d;
                    var schemetype = schemetypeValue.toString().split('~')[0];
                    var schemelength = schemetypeValue.toString().split('~')[1];
                    $('#txtVoucherNo').attr('maxLength', schemelength);
                    if (schemetype == '0') {

                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                        $("#txtVoucherNo").focus();

                    }
                    else if (schemetype == '1') {

                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                        cPLQuoteDate.Focus();
                        $("#MandatoryBillNo").hide();

                    }
                    else if (schemetype == '2') {

                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
                    }
                    else if (schemetype == 'n') {
                        document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                        document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                    }
                }
            });
}
function IndentRequisitionNo_ValueChange() {

    var val = $("#ddl_IndentRequisitionNo").val();
    if (val != 0) {
        $.ajax({
            type: "POST",
            url: 'PurchaseOrder.aspx/getIndentRequisitionDate',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{IndentRequisitionNo:\"" + val + "\"}",
            success: function (type) {

                var Transdt = new Date(type.d);
                cIndentRequisDate.SetDate(Transdt);

            }
        });
    }
    else {
        cIndentRequisDate.SetVal("");
    }

}


function CloseGridLookup() {
    gridLookup.ConfirmCurrentSelection();
    gridLookup.HideDropDown();
    gridLookup.Focus();
}



function SetDifference() {
    var diff = CheckDifference();
    if (diff > 0) {
        clientResult.SetText(diff.toString());
    }

}

function CheckDifference() {
    var startDate = new Date();
    var endDate = new Date();
    var difference = -1;
    startDate = cPLQuoteDate.GetDate();
    if (startDate != null) {
        endDate = cExpiryDate.GetDate();
        var startTime = startDate.getTime();
        var endTime = endDate.getTime();
        difference = (endTime - startTime) / 86400000;

    }
    return difference;

}


    </script>
     <%--   Warehouse  Script   --%>
      <script type="text/javascript">

          function Keypressevt() {

              if (event.keyCode == 13) {

                  //run code for Ctrl+X -- ie, Save & Exit! 
                  SaveWarehouse();
                  return false;
              }
          }

          function getUrlVars() {
              var vars = [], hash;
              var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
              for (var i = 0; i < hashes.length; i++) {
                  hash = hashes[i].split('=');
                  vars.push(hash[0]);
                  vars[hash[0]] = hash[1];
              }
              return vars;
          }

          $(document).ready(function () {

              var isCtrl = false;
              document.onkeydown = function (e) {
                  if (event.keyCode == 83 && event.altKey == true) {
                      if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                          SaveVehicleControlData();
                      }
                  }
                  if (event.keyCode == 67 && event.altKey == true && getUrlVars().req != "V") {
                      modalShowHide(0);
                  }
                  if (event.keyCode == 82 && event.altKey == true && getUrlVars().req != "V") {
                      modalShowHide(1);
                      $('body').on('shown.bs.modal', '#exampleModal', function () {
                          $('input:visible:enabled:first', this).focus();
                      })
                  }

                  if (event.keyCode == 78 && event.altKey == true && getUrlVars().req != "V") {
                      Save_ButtonClick();
                  }
                  else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") {
                      SaveExit_ButtonClick();
                  }
                  else if (event.keyCode == 85 && event.altKey == true) {
                      OpenUdf();
                  }
                  else if (event.keyCode == 84 && event.altKey == true) {
                      Save_TaxesClick();
                  }
                  if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                      //  StopDefaultAction(e);
                      alert();

                      btnSave_QuoteAddress();
                      // document.getElementById('Button3').click();

                      // return false;
                  }

              }
          });
          function StopDefaultAction(e) {
              if (e.preventDefault) { e.preventDefault() }
              else { e.stop() };

              e.returnValue = false;
              e.stopPropagation();
          }

          function DeleteWarehousebatchserial(SrlNo, BatchWarehouseID, viewQuantity, Quantity, WarehouseID, BatchNo) {
              //alert(viewQuantity);
              var IsSerial = $('#hdnisserial').val();
              if (IsSerial == "true" && viewQuantity != "1.0000" && viewQuantity != "1.0" && viewQuantity != "") {
                  jAlert("Cannot Proceed. You have to delete subsequent data first before delete this data.");
              } else {
                  if (BatchWarehouseID == "" || BatchWarehouseID == "0") {

                      $('#<%=hdnisolddeleted.ClientID %>').val("false");
                      if (SrlNo != "") {


                          cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
                      }

                  } else {

                      $('#<%=hdnisolddeleted.ClientID %>').val("true");
                      if (SrlNo != "") {

                          cGrdWarehousePC.PerformCallback('Delete~' + SrlNo + '~' + viewQuantity + '~' + Quantity + '~' + WarehouseID + '~' + BatchNo);
                      }
                  }
              }



          }

          function Setenterfocuse(s) {

           <%-- var Isbatch = $('#hdnisbatch').val();
            var IsSerial = $('#hdnisserial').val();
            //alert(Isbatch);
            if (Isbatch == "true") {
                ctxtbatch.Focus();
                document.getElementById("<%=txtbatch.ClientID%>").focus();
            } else if (IsSerial == "true") {
                ctxtserial.Focus();
            }--%>
          }

          function UpdateWarehousebatchserial(SrlNo, WarehouseID, BatchNo, SerialNo, isnew, viewQuantity, Quantity) {

              var Isbatch = $('#hdnisbatch').val();

              if (isnew == "old" || isnew == "Updated") {

                  $('#<%=hdnisoldupdate.ClientID %>').val("true");
                  $('#<%=hdncurrentslno.ClientID %>').val("");
                  cCmbWarehouse.SetValue(WarehouseID);
                  if (Quantity != null && Quantity != "" && Isbatch != "true") {
                      ctxtqnty.SetText(Quantity);
                  } else {
                      ctxtqnty.SetText(viewQuantity);
                  }
                  var IsSerial = $('#hdnisserial').val();

                  if (IsSerial == "true") {

                      if (viewQuantity == "") {
                          ctxtbatch.SetEnabled(false);
                          cCmbWarehouse.SetEnabled(false);
                          ctxtqnty.SetEnabled(false);
                          ctxtserial.Focus();
                      } else {
                          ctxtbatch.SetEnabled(true);
                          cCmbWarehouse.SetEnabled(true);
                          ctxtqnty.SetEnabled(true);
                          ctxtserial.Focus();
                      }

                  }
                  else {
                      ctxtbatch.SetEnabled(true);
                      cCmbWarehouse.SetEnabled(true);
                      ctxtqnty.SetEnabled(true);
                      ctxtbatch.Focus();
                  }
                  // ctxtqnty.SetEnabled(false);

                  ctxtbatchqnty.SetText(Quantity);
                  //ctxtbatchqnty.SetEnabled(false);
                  ctxtbatch.SetText(BatchNo);
                  ctxtserial.SetText(SerialNo);

                  if (viewQuantity == "") {
                      ctxtbatch.SetEnabled(false);
                      cCmbWarehouse.SetEnabled(false);
                      $('#<%=hdnisviewqntityhas.ClientID %>').val("true");
                  } else {
                      ctxtbatch.SetEnabled(true);
                      cCmbWarehouse.SetEnabled(true);
                      $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                  }

                  var hdniswarehouse = $('#hdniswarehouse').val();


                  if (hdniswarehouse != "true" && Isbatch == "true") {
                      ctxtbatchqnty.SetText(viewQuantity);
                      ctxtbatchqnty.Focus();

                  } else {
                      ctxtqnty.Focus();
                  }
                  $('#<%=hdncurrentslno.ClientID %>').val(SrlNo);

              } else {

                  $('#<%=hdnisoldupdate.ClientID %>').val("false");

                  ctxtqnty.SetText("0.0");
                  ctxtqnty.SetEnabled(true);

                  ctxtbatchqnty.SetText("0.0");
                  ctxtserial.SetText("");
                  ctxtbatchqnty.SetText("");
                  $('#<%=hdncurrentslno.ClientID %>').val("");

                  $('#<%=hdnisnewupdate.ClientID %>').val("true");
                  $('#<%=hdncurrentslno.ClientID %>').val("");
                  cCmbWarehouse.SetValue(WarehouseID);
                  if (Quantity != null && Quantity != "" && Isbatch != "true") {
                      ctxtqnty.SetText(Quantity);
                  } else {
                      ctxtqnty.SetText(viewQuantity);
                  }
                  var IsSerial = $('#hdnisserial').val();
                  if (IsSerial == "true") {

                      if (viewQuantity == "") {
                          ctxtbatch.SetEnabled(false);
                          cCmbWarehouse.SetEnabled(false);
                          ctxtqnty.SetEnabled(false);
                          $('#<%=hdnisviewqntityhas.ClientID %>').val("true");
                          ctxtserial.Focus();
                      } else {
                          ctxtbatch.SetEnabled(true);
                          cCmbWarehouse.SetEnabled(true);
                          ctxtqnty.SetEnabled(true);
                          $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                          ctxtserial.Focus();
                      }

                  } else {
                      ctxtbatch.SetEnabled(true);
                      cCmbWarehouse.SetEnabled(true);
                      ctxtqnty.SetEnabled(true);
                      ctxtbatch.Focus();
                  }
                  // ctxtqnty.SetEnabled(false);

                  ctxtbatchqnty.SetText(Quantity);
                  //ctxtbatchqnty.SetEnabled(false);
                  ctxtbatch.SetText(BatchNo);
                  ctxtserial.SetText(SerialNo);

                  if (viewQuantity == "") {
                      ctxtbatch.SetEnabled(false);
                      cCmbWarehouse.SetEnabled(false);
                  } else {
                      ctxtbatch.SetEnabled(true);
                      cCmbWarehouse.SetEnabled(true);
                  }

                  var hdniswarehouse = $('#hdniswarehouse').val();


                  if (hdniswarehouse != "true" && Isbatch == "true") {
                      ctxtbatchqnty.SetText(viewQuantity);
                  } else {
                      ctxtqnty.Focus();
                  }

                  $('#<%=hdncurrentslno.ClientID %>').val(SrlNo);

                  //jAlert("Sorry, This is new entry you can not update. please click on 'Clear Entries' and Add again.");
              }
          }

          function changedqnty(s) {

              var qnty = s.GetText();
              var sum = $('#hdntotalqntyPC').val();

              sum = Number(Number(sum) + Number(qnty));
              //alert(sum);
              $('#<%=hdntotalqntyPC.ClientID %>').val(sum);
           <%-- document.getElementById("<%=txtbatch.ClientID%>").focus();
            var Isbatch = $('#hdnisbatch').val();
            var IsSerial = $('#hdnisserial').val();
            //alert(Isbatch);
            if (Isbatch == "true") {
                ctxtbatch.Focus();
                document.getElementById("<%=txtbatch.ClientID%>").focus();
            } else if (IsSerial == "true") {
                ctxtserial.Focus();
            }--%>
          }

          function endcallcmware(s) {

              if (cCmbWarehouse.cpstock != null) {

                  var ddd = cCmbWarehouse.cpstock + " " + $('#hdnstrUOM').val();
                  document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ddd;
                  cCmbWarehouse.cpstock = null;
              }
          }
          function changedqntybatch(s) {

              var qnty = s.GetText();
              var sum = $('#hdntotalqntyPC').val();
              sum = Number(Number(sum) + Number(qnty));
              //alert(sum);
              $('#<%=hdntotalqntyPC.ClientID %>').val(sum);

              //var Isbatch = $('#hdnisbatch').val();
              //var IsSerial = $('#hdnisserial').val();
              ////alert(Isbatch);
              //if (IsSerial == "true") {
              //    ctxtserial.Focus();
              //}

          }
          function chnagedbtach(s) {

              $('#<%=hdnoldbatchno.ClientID %>').val(s.GetText());
              $('#<%=hidencountforserial.ClientID %>').val(1);

              var sum = $('#hdnbatchchanged').val();
              sum = Number(Number(sum) + Number(1));

              $('#<%=hdnbatchchanged.ClientID %>').val(sum);
              //ctxtqnty.SetValue("0.0");
              //ctxtbatchqnty.SetValue("0.0");
              //ctxtmkgdate.SetDate = null;
              //txtexpirdate.SetDate = null;
              //ASPx.CalClearClick('txtmkgdate_DDD_C');
              //ASPx.CalClearClick('txtexpirdate_DDD_C');
              ctxtexpirdate.SetText("");
              ctxtmkgdate.SetText("");
          }

          function CmbWarehouse_ValueChange(s) {

              var ISupdate = $('#hdnisoldupdate').val();
              var isnewupdate = $('#hdnisnewupdate').val();

              $('#<%=hdnoldwarehousname.ClientID %>').val(s.GetText());

              if (ISupdate == "true" || isnewupdate == "true") {


              } else {
        <%--$('#<%=hidencountforserial.ClientID %>').val(1);
           
            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            $('#<%=hidencountforserial.ClientID %>').val("1");--%>

                  ctxtserial.SetValue("");
                  //ctxtbatch.SetValue("");
                  //ctxtmkgdate.SetDate = null;
                  //txtexpirdate.SetDate = null;

                  ctxtbatch.SetEnabled(true);
                  ctxtexpirdate.SetEnabled(true);
                  ctxtmkgdate.SetEnabled(true);

                  //ctxtqnty.SetValue("0.0");
                  //ctxtbatchqnty.SetValue("0.0");

                  //cCmbWarehouse.PerformCallback('Bindstock');
                  //ASPx.CalClearClick('txtmkgdate_DDD_C');
                  //ASPx.CalClearClick('txtexpirdate_DDD_C');
                  //ctxtexpirdate.SetText("");
                  //ctxtmkgdate.SetText("");
              }


          }

          function Clraear() {
              ctxtbatch.SetValue("");

              ASPx.CalClearClick('txtmkgdate_DDD_C');
              ASPx.CalClearClick('txtexpirdate_DDD_C');
              $('#<%=hdnisoldupdate.ClientID %>').val("false");
             //ctxtmkgdate.SetDate = null;
             //txtexpirdate.SetDate = null;

             //ctxtexpirdate.SetText("");
             //ctxtmkgdate.SetText("");

             //ctxtmkgdate.CalClearClick('txtmkgdate_DDD_C');
             //ctxtexpirdate.CalClearClick('txtexpirdate_DDD_C');
             ctxtserial.SetValue("");
             ctxtqnty.SetValue("0.0000");
             ctxtbatchqnty.SetValue("0.0000");
             $('#<%=hdntotalqntyPC.ClientID %>').val(0);
             $('#<%=hidencountforserial.ClientID %>').val(1);
             $('#<%=hdnbatchchanged.ClientID %>').val("0");
             var strProductID = $('#hdfProductIDPC').val();
             var stockids = $('#hdfstockidPC').val();
             var branchid = $('#hdbranchIDPC').val();
             var strProductName = $('#lblProductName').text();
             $('#<%=hdnisnewupdate.ClientID %>').val("false");
             ctxtbatch.SetEnabled(true);
             ctxtexpirdate.SetEnabled(true);
             ctxtmkgdate.SetEnabled(true);
             ctxtbatch.SetEnabled(true);
             cCmbWarehouse.SetEnabled(true);
             $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
            $('#<%=hdnisolddeleted.ClientID %>').val("false");
             ctxtqnty.SetEnabled(true);

             var existingqntity = $('#hdfopeningstockPC').val();
             var totaldeleteqnt = $('#hdndeleteqnity').val();

             var addqntity = Number(existingqntity) + Number(totaldeleteqnt);

             $('#<%=hdndeleteqnity.ClientID %>').val(0);
           <%-- $('#<%=hdfopeningstockPC.ClientID %>').val(addqntity);--%>



             cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

         }

         function SaveWarehouse() {


             //alert(ISupdate);
           <%-- var openqnty = Number($('#hdfopeningstockPC').val());
            var totalqnty = Number($('#hdntotalqntyPC').val());
            if (totalqnty > openqnty) {

                var qnty = Number(ctxtqnty.GetText());
                var againcal = Number(totalqnty - qnty);

                $('#<%=hdntotalqntyPC.ClientID %>').val(againcal);

                var totalqntys = Number($('#hdntotalqntyPC').val());
                ctxtqnty.SetText("0.0");
                ctxtbatchqnty.SetText("0.0");
                //alert(totalqntys);
                jAlert("Please make sure Opening quantity should not be greater and less than total INput quantity.");--%>

             //}
             //else {

             var WarehouseID = cCmbWarehouse.GetValue();
             var WarehouseName = cCmbWarehouse.GetText();

             var qnty = ctxtqnty.GetText();
             var IsSerial = $('#hdnisserial').val();
             //alert(qnty);

             if (qnty == "0.0000") {
                 qnty = ctxtbatchqnty.GetText();
             }

             if (Number(qnty) % 1 != 0 && IsSerial == "true") {
                 jAlert("Serial number is activated, Quantity should not contain decimals. ");
                 return;
             }

             //alert(qnty);
             var BatchName = ctxtbatch.GetText();
             var SerialName = ctxtserial.GetText();
             var Isbatch = $('#hdnisbatch').val();

             var enterdqntity = $('#hdfopeningstockPC').val();

             var hdniswarehouse = $('#hdniswarehouse').val();

             var ISupdate = $('#hdnisoldupdate').val();
             var isnewupdate = $('#hdnisnewupdate').val();
             //alert(Isbatch + "/" + IsSerial);
             //alert(hdniswarehouse+"/"+WarehouseID);
             if (Isbatch == "true" && hdniswarehouse == "false") {
                 qnty = ctxtbatchqnty.GetText();
             }

             if (ISupdate == "true") {

                 if (hdniswarehouse == "true" && WarehouseID == null) {

                     $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
                 }
                 else {
                     $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                 }
                 if (qnty == "0.0") {

                     if (Isbatch != "false" || hdniswarehouse != "false") {
                         $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                         $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                         //jAlert("Quantity should not be 0.0");
                     } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                         qnty = "0.00"
                         $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                         $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                     }
                 } else {

                     qnty = "0.00"
                     $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                     $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                 }

                 if (Isbatch == "true" && BatchName == "") {

                     $("#RequiredFieldValidatortxtbatch").css("display", "block");
                     ctxtbatch.Focus();
                 } else {
                     $("#RequiredFieldValidatortxtbatch").css("display", "none");
                 }
                 if (IsSerial == "true" && SerialName == "") {
                     $("#RequiredFieldValidatortxtserial").css("display", "block");
                     ctxtserial.Focus();

                 } else {
                     $("#RequiredFieldValidatortxtserial").css("display", "none");
                 }
                 var slno = $('#hdncurrentslno').val();



                 if (slno != "") {

                     cGrdWarehousePC.PerformCallback('Updatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

               <%--     $('#<%=hdnisoldupdate.ClientID %>').val("false");
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);--%>
                    return false;
                }


            } else if (isnewupdate == "true") {
                if (hdniswarehouse == "true" && WarehouseID == null) {

                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");
                }
                else {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                }
                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                } else {

                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }

                if (Isbatch == "true" && BatchName == "") {

                    $("#RequiredFieldValidatortxtbatch").css("display", "block");
                    ctxtbatch.Focus();
                }
                else {
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                }
                if (IsSerial == "true" && SerialName == "") {


                    $("#RequiredFieldValidatortxtserial").css("display", "block");
                    ctxtserial.Focus();

                } else {
                    $("#RequiredFieldValidatortxtserial").css("display", "none");
                }
                var slno = $('#hdncurrentslno').val();

                if (slno != "") {

                    cGrdWarehousePC.PerformCallback('NewUpdatedata~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + slno + '~' + qnty);

                    $('#<%=hdnisviewqntityhas.ClientID %>').val("false");
                    $('#<%=hdnisnewupdate.ClientID %>').val("false");
                    ctxtbatch.SetEnabled(true);
                    cCmbWarehouse.SetEnabled(true);
                    ctxtqnty.SetEnabled(true);
                    ctxtqnty.SetText("0.0");
                    ctxtbatch.SetText("");
                    return false;
                }

            }
            else {

                var hdnisediteds = $('#hdnisedited').val();

                if (hdniswarehouse == "true" && WarehouseID == null) {

                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "block");

                    return;
                } else {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                }
                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                } else {

                    qnty = "0.00"
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                }
                if (Isbatch == "true" && BatchName == "") {

                    $("#RequiredFieldValidatortxtbatch").css("display", "block");
                    ctxtbatch.Focus();
                    return;

                } else {
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                }
                if (IsSerial == "true" && SerialName == "") {


                    $("#RequiredFieldValidatortxtserial").css("display", "block");
                    ctxtserial.Focus();
                    return;

                } else {
                    $("#RequiredFieldValidatortxtserial").css("display", "none");
                }
                if (Isbatch == "true" && hdniswarehouse == "false") {

                    qnty = ctxtbatchqnty.GetText();

                    if (qnty == "0.0000") {
                        //alert("Enter" + ctxtbatchqnty.GetText());

                        ctxtbatchqnty.Focus();
                    }
                }

                if (qnty == "0.0") {

                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");
                        //jAlert("Quantity should not be 0.0");
                    } else if (Isbatch == "false" && hdniswarehouse == "false" && IsSerial == "true") {
                        qnty = "0.00"
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "none");
                    }
                }
                else if (((hdniswarehouse == "true" && WarehouseID != null) || hdniswarehouse == "false") && ((Isbatch == "true" && BatchName != "") || Isbatch == "false") && ((IsSerial == "true" && SerialName != "") || IsSerial == "false") && qnty != "0.0") {
                    $("#RequiredFieldValidatorCmbWarehouse").css("display", "none");
                    $("#RequiredFieldValidatortxtbatch").css("display", "none");
                    $("#RequiredFieldValidatortxtserial").css("display", "none");

                    $("#RequiredFieldValidatortxtwareqntity").removeAttr("style");
                    $("#RequiredFieldValidatortxtbatchqntity").removeAttr("style");
                    $("#RequiredFieldValidatortxtbatchqntity").css("display", "none");
                    $("#RequiredFieldValidatortxtwareqntity").css("display", "none");


                    if (Isbatch == "true" && hdniswarehouse == "false") {

                        qnty = ctxtbatchqnty.GetText();

                        if (qnty = "0.0000") {
                            ctxtbatchqnty.Focus();
                        }
                    }


                    var oldenterqntity = $('#hdnenterdopenqnty').val();
                    var enterdqntityss = $('#hdnnewenterqntity').val();
                    var deletedquantity = $('#hdndeleteqnity').val();
                    //alert(deletedquantity);
                    // alert(enterdqntityss + "|" + oldenterqntity);
                    //if (Number(enterdqntityss) < Number(oldenterqntity)) {
                    //    qnty = "0.00";
                    //    jConfirm('You have entered Quantity less than Opening Quantity. Do you want to clear all existing entries.?', 'Confirmation Dialog', function (r) {
                    //        if (r == true) {

                    //            cGrdWarehousePC.PerformCallback('Delete~' + WarehouseID);
                    //            var strProductID = $('#hdfProductIDPC').val();
                    //            var stockids = $('#hdfstockidPC').val();
                    //            var branchid = $('#hdbranchIDPC').val();
                    //            var strProductName = $('#lblProductName').text();
                    //            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);
                    //        }
                    //    });


                    //}




                    if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                        qnty = "0.00";
                        jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");


                    }
                    else {


                        cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);

                        //ctxtserial.SetValue("");

                        //ASPx.CalClearClick('txtmkgdate_DDD_C');
                        //ASPx.CalClearClick('txtexpirdate_DDD_C');

                        cCmbWarehouse.Focus();
                    }
                }
                //}

                //ctxtexpirdate.SetText("");
                //ctxtmkgdate.SetText("");
                return false;
            }
    }
    function SaveWarehouseAll() {
        //var openqnty = Number($('#hdfopeningstockPC').val());
        //var totalqnty = Number($('#hdntotalqntyPC').val());
        // if (totalqnty != openqnty) {

        //jAlert("Please make sure Opening quantity should not be greater and less than total INput quantity.");
        //} else {

        cGrdWarehousePC.PerformCallback('Saveall~');


        //}

    }

    function cGrdWarehousePCShowError(obj) {

        if (cGrdWarehousePC.cpdeletedata != null) {
            var existingqntity = $('#hdfopeningstockPC').val();
            var totaldeleteqnt = $('#hdndeleteqnity').val();

            var addqntity = Number(cGrdWarehousePC.cpdeletedata) + Number(existingqntity);
            var adddeleteqnty = Number(cGrdWarehousePC.cpdeletedata) + Number(totaldeleteqnt);

            $('#<%=hdndeleteqnity.ClientID %>').val(adddeleteqnty);
            <%--$('#<%=hdfopeningstockPC.ClientID %>').val(addqntity);--%>
            cGrdWarehousePC.cpdeletedata = null;
        }

        if (cGrdWarehousePC.cpdeletedatasubsequent != null) {
            jAlert(cGrdWarehousePC.cpdeletedatasubsequent);
            cGrdWarehousePC.cpdeletedatasubsequent = null;
        }
        if (cGrdWarehousePC.cpbatchinsertmssg != null) {
            ctxtbatch.SetText("");

            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            cGrdWarehousePC.cpbatchinsertmssg = null;
        }
        if (cGrdWarehousePC.cpupdateexistingdata != null) {

            $('#<%=hdnisedited.ClientID %>').val("true");
            cGrdWarehousePC.cpupdateexistingdata = null;
        }
        if (cGrdWarehousePC.cpupdatenewdata != null) {

            $('#<%=hdnisedited.ClientID %>').val("true");

            cGrdWarehousePC.cpupdateexistingdata = null;
        }

        if (cGrdWarehousePC.cpupdatemssgserialsetdisblebatch != null) {
            ctxtbatch.SetEnabled(false);
            ctxtexpirdate.SetEnabled(false);
            ctxtmkgdate.SetEnabled(false);
            cGrdWarehousePC.cpupdatemssgserialsetdisblebatch = null;
        }
        if (cGrdWarehousePC.cpupdatemssgserialsetenablebatch != null) {
            ctxtbatch.SetEnabled(true);
            ctxtexpirdate.SetEnabled(true);
            ctxtmkgdate.SetEnabled(true);
            $('#<%=hidencountforserial.ClientID %>').val(1);

            $('#<%=hdnbatchchanged.ClientID %>').val("0");
            $('#<%=hidencountforserial.ClientID %>').val("1");
            ctxtqnty.SetValue("0.0000");
            ctxtbatchqnty.SetValue("0.0000");
            ctxtbatch.SetText("");
            cGrdWarehousePC.cpupdatemssgserialsetenablebatch = null;
        }


        if (cGrdWarehousePC.cpproductname != null) {
            document.getElementById('<%=lblpro.ClientID %>').innerHTML = cGrdWarehousePC.cpproductname;
            cGrdWarehousePC.cpproductname = null;
        }

          <%--  if (cGrdWarehousePC.cpbranchqntity != null) {

                var qnty = cGrdWarehousePC.cpbranchqntity;
                var sum = $('#hdfopeningstockPC').val();
                sum = Number(Number(sum) + Number(qnty));
               
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = "0";
                cGrdWarehousePC.cpbranchqntity = null;
            }--%>

        if (cGrdWarehousePC.cpupdatemssg != null) {
            if (cGrdWarehousePC.cpupdatemssg == "Saved Successfully.") {
                $('#<%=hdntotalqntyPC.ClientID %>').val("0");
                $('#<%=hdnbatchchanged.ClientID %>').val("0");
                $('#<%=hidencountforserial.ClientID %>').val("1");
                ctxtqnty.SetValue("0.0000");
                ctxtbatchqnty.SetValue("0.0000");

                parent.cPopup_WarehousePC.Hide();
                var hdnselectedbranch = $('#hdnselectedbranch').val();
                grid.batchEditApi.StartEdit(globalRowIndex, 9);
                //cOpeningGrid.Enable = false;
                // parent.cOpeningGrid.PerformCallback("branchwise~" + hdnselectedbranch);
            } else {
                jAlert(cGrdWarehousePC.cpupdatemssg);
            }

            cGrdWarehousePC.cpupdatemssg = null;


        }
        if (cGrdWarehousePC.cpupdatemssgserial != null) {
            jAlert(cGrdWarehousePC.cpupdatemssgserial);
            cGrdWarehousePC.cpupdatemssgserial = null;
        }

        if (cGrdWarehousePC.cpinsertmssg != null) {
            $('#<%=hidencountforserial.ClientID %>').val(2);
            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehousePC.cpinsertmssg = null;
        }
        if (cGrdWarehousePC.cpinsertmssgserial != null) {

            ctxtserial.SetValue("");
            ctxtserial.Focus();
            cGrdWarehousePC.cpinsertmssgserial = null;
        }


    }
    function Onddl_VatGstCstEndCallback(s, e) {
        if (s.GetItemCount() == 1) {
            cddlVatGstCst.SetEnabled(false);
        }
    }
    </script>
    <script>

        document.onkeydown = function (e) {
            if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                //  StopDefaultAction(e);
                alert();

                btnSave_QuoteAddress();
                // document.getElementById('Button3').click();

                // return false;
            }

            if (event.keyCode == 88 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);
                alert('');

                page.SetActiveTabIndex(0);
                gridLookup.Focus();
                // document.getElementById('Button3').click();

                // return false;
            }
        }

    </script>

     <%--   Warehouse Script End    --%>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <span class="">
                <asp:Label ID="lblHeading" runat="server" Text="Add Purchase Order"></asp:Label>

                </span>
            </h3>
               <div id="pageheaderContent" class="pull-right reverse wrapHolder content horizontal-images" style="display: none;" runat="server">
            <div class="Top clearfix">
                <ul>
                    <li>                        
                            <div class="lblHolder" id="divContactPhone"  style="display: none;"  runat="server">
                                <table>
                                    <tr>
                                        <td>Contact Person's Phone</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                       
                    </li>
                    <li>
                        
                            <div class="lblHolder" id="divOutstanding"  style="display: none;"  runat="server">
                                <table>
                                    <tr>
                                        <td>Total Payable(Dues)</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTotalPayable" runat="server" Text="0.0" CssClass="classout"></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                       
                    </li>
                    <li>
                        <div class="lblHolder" id="divAvailableStk" style="display: none;">
                            <table>
                                <tr>
                                    <td>Available Stock</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAvailableStkPro" runat="server" Text="0.0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                            <table>
                                <tr>
                                        <td>GST Registed?</td>
                                    </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divPacking" style="display: none;">
                            <table>
                                <tr>
                                    <td>Packing Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
                <ul style="display: none;">
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Branch</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label13" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Product</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProduct" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Stock Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                        <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
           <%-- <div id="btncross" class="crossBtn"  margin-left: 50px;"><a href="PurchaseOrderList.aspx"><i class="fa fa-times"></i></a></div>--%>
            <%-- region Sandip Section For Approval Section Start--%>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
       
            <div id="divcross" runat="server" class="crossBtn">
                 <a href="VendorDashboard.aspx"><i class="fa fa-times"></i></a></div>
                <%-- <a href="javascript:void(0);" onclick="BackClick()"><i class="fa fa-times"></i></a></div>--%>
            <%-- endregion Sandip Section For Approval Dtl Section End--%>
             </div>

    </div>
            <div class="form_main">      
             <div class="row">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                         <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="row">
                                        <div class="col-md-3" runat="server" id="divNumberingScheme" >

                                            <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" 
                                                DataTextField="SchemaName" DataValueField="Id" onchange="CmbScheme_ValueChange()">
                                            </asp:DropDownList>
                                        </div>         
          
                                        <div class="col-md-3">

                                            <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Purchase Number" >
                                            </dxe:ASPxLabel><span style="color: red;">*</span>
                                            <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                                            </asp:TextBox>
                                            <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                            <%-- <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" TabIndex="2" Width="100%">
                                            </dxe:ASPxTextBox>--%>
                                        </div>
                                        <div class="col-md-2">
                                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date">
                                            </dxe:ASPxLabel><span style="color: red;">*</span>

                                            <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPLQuoteDate"  Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                  <ClientSideEvents DateChanged="function(s, e) { TDateChange(e)}" GotFocus="function(s,e){cPLQuoteDate.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>
                                            <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                        </div>
                                        <div class="col-md-2">
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                                </dxe:ASPxLabel><span style="color: red;">*</span>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" DataSourceID="DS_Branch"
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" onchange="onBranchItems()" >
                                                </asp:DropDownList>
                                            </div>
                                        
                                    
                                    <div class="col-md-2">

                                            <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                            </dxe:ASPxLabel><span style="color: red;">*</span>
                                           <%-- <asp:DropDownList ID="ddl_Vendor" runat="server" Width="100%"  DataSourceID="Sqlvendor"
                                                DataTextField="Name" DataValueField="cnt_internalId" onchange="ddl_Vendor_ValueChange()">
                                            </asp:DropDownList>--%>
                                                <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup"
                                                                        KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">

                                                                        <Columns>


                                                                           <%-- <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="150" Settings-AutoFilterCondition="Contains" />--%>
                                                                            <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="0" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                            </dxe:GridViewDataColumn>
                                                                             <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="1" Caption="Short Name" Width="100" Settings-AutoFilterCondition="Contains">
                                                                            </dxe:GridViewDataColumn>
                                                                            <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="2" Settings-AllowAutoFilter="False" Width="150">
                                                                                <Settings AllowAutoFilter="False"></Settings>
                                                                            </dxe:GridViewDataColumn>
                                                                        </Columns>
                                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                                            <Templates>
                                                                                <StatusBar>
                                                                                    <table class="OptionsTable" style="float: right">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <%--<dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior ="False"/>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </StatusBar>
                                                                            </Templates>

                                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                            <%-- <Settings ShowFilterRow="True" ShowStatusBar="Visible" />--%>

                                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                                                        </GridViewProperties>
                                                                        <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                                                        <ClearButton DisplayMode="Auto">
                                                                        </ClearButton>
                                                                    </dxe:ASPxGridLookup>
                                                <span id="MandatorysVendor" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory">
                                              
                                                </span>
                                        </div>
                                        <div style="clear: both"></div>           
                                        <div class="col-md-3">

                                            <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback"  Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                               <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}"  />
                                            </dxe:ASPxComboBox>
                                            <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                        </div>
                                     <div class="col-md-3">
                                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" DataSourceID="DS_SalesAgent" DataTextField="Name" DataValueField="cnt_id" >
                                            </asp:DropDownList>
                                        </div>
                                    
                                    <div class="col-md-2" id="indentRequisition" runat="server" >
                                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Indent/Requisition">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                 <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server"  ClientInstanceName="gridquotationLookup" 
                                                        OnDataBinding="lookup_quotation_DataBinding"
                                                        KeyFieldName="Indent_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />


                                                            <dxe:GridViewDataColumn FieldName="Indent_RequisitionNumber" Visible="true" VisibleIndex="1"  Caption="Indent/Requisition Number"  Settings-AutoFilterCondition="Contains" Width="250"/>
                                                            <dxe:GridViewDataColumn FieldName="Branch" Visible="true" VisibleIndex="2" Caption="Branch" Width="150" Settings-AutoFilterCondition="Contains" /> 
                                                            <dxe:GridViewDataColumn FieldName="Indent_RequisitionDate" Visible="true" VisibleIndex="3"  Caption="Indent/Requisition Date"  Settings-AutoFilterCondition="Contains" Width="250" />
                                                           
                                                        </Columns>
                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                            <Templates>
                                                                <StatusBar>
                                                                    <table class="OptionsTable" style="float: right">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior ="False"  />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </StatusBar>
                                                            </Templates>
                                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                                            <SettingsPager Mode="ShowAllRecords">
                                                            </SettingsPager>
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                        </GridViewProperties>
                                                        <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}"  />
                                                    </dxe:ASPxGridLookup>

                                                </dxe:PanelContent>
                                            </PanelCollection>
                                                 <ClientSideEvents EndCallback="componentEndCallBack" />
                                            </dxe:ASPxCallbackPanel>
                                        <span id="MandatorysIndentReq" class="POIndentReq  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory">
                                              
                                                </span>
                                           <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
                                            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                                            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                                            <HeaderTemplate>
                                                <strong><span style="color: #fff">Select Products</span></strong>
                                                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                                                    <ClientSideEvents Click="function(s, e){ 
                                                                                        cProductsPopup.Hide();
                                                                                    }" />
                                                </dxe:ASPxImage>
                                            </HeaderTemplate>
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                    <div style="padding: 7px 0;">
                                                        <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                                                    </div>
                                                    <dxe:ASPxGridView runat="server" KeyFieldName="QuoteDetails_Id" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                        Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                                                        OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                                                      
                                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                                        <SettingsPager Visible="false"></SettingsPager>
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
                                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="45" ReadOnly="true" Caption="Sl. No.">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="gvColProduct" ReadOnly="true" Caption="Product" Width="0">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="gvColDiscription" Width="200" ReadOnly="true" Caption="Product Description">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Indent No">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="gvColQuantity" Width="70" VisibleIndex="6">
                                                                <PropertiesTextEdit>                                                               
                                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>

                                                        </Columns>
                                                    
                                                        <SettingsDataSecurity AllowEdit="true" />
                                                       
                                                    </dxe:ASPxGridView>
                                                    <div class="text-center pTop10">
                                                       <%-- <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />--%>

                                                        <dxe:ASPxButton ID="Button2" ClientInstanceName="cButton2" runat="server"  AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                                        </dxe:ASPxButton>
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                            </dxe:ASPxPopupControl>
                                            <%--<asp:DropDownList ID="ddl_IndentRequisitionNo" runat="server" Width="100%"  DataSourceID="SqlIndentRequisitionNo" onchange="IndentRequisitionNo_ValueChange()"
                                                DataTextField="Indent_RequisitionNumber" DataValueField="Indent_Id">
                                            </asp:DropDownList>--%>

                                          
                                        </div>
                                        <div class="col-md-2">
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Indent/Requisition Date">
                                            </dxe:ASPxLabel>
                                            <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                          <%--      <dxe:ASPxDateEdit ID="txtDateIndentRequis" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" 
                                              ClientInstanceName="cIndentRequisDate"
                                                    Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>                                                 
                                                </dxe:ASPxDateEdit>--%>
                                                 <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Indent Dates" Style="display: none"></asp:Label>

                                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxTextBox ID="dt_Quotation" runat="server" TabIndex="9" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                                    </dxe:ASPxTextBox>

                                                    <dxe:ASPxDateEdit ID="txtDateIndentRequis" runat="server" Enabled="false" Visible="false" EditFormat="Custom" ClientInstanceName="cIndentRequisDate" TabIndex="13" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" ErrorText="Expiry date can not be shorter than Pl/Indent date.">
                                                            <RequiredField IsRequired="true" />
                                                        </ValidationSettings>

                                                        <ClientSideEvents DateChanged="function(s,e){SetDifference1();}"
                                                            Validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />
                                                    </dxe:ASPxDateEdit>
                                                </dxe:PanelContent>
                                            </PanelCollection>

                                        </dxe:ASPxCallbackPanel>
                                                 
                                            </div>
                                        </div>             
                                                  

                                        
                                        <div class="col-md-2">
                                            <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference" >
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxTextBox ID="txt_Refference" runat="server"  Width="100%" ClientInstanceName="ctxt_Refference">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    <div style="clear: both"></div> 
                                        
                                        
                                        
                                        <div class="col-md-1 lblmTop8">

                                            <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" 
                                                DataSourceID="SqlCurrency" DataValueField="Currency_ID"
                                                DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                            </asp:DropDownList>


                                        </div>
                                        <div class="col-md-2 lblmTop8">

                                            <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                            </dxe:ASPxLabel>


                                            <dxe:ASPxTextBox ID="txt_Rate" runat="server"  Width="100%" ClientInstanceName="ctxtRate">
                                                 <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                 <ClientSideEvents LostFocus="ReBindGrid_Currency" />
                                            </dxe:ASPxTextBox>

                                        </div>
                                    
                                        <div class="col-md-3 lblmTop8">

                                            <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                            </dxe:ASPxLabel>

                                            <%--<asp:DropDownList ID="ddl_AmountAre" runat="server"  Width="100%" DataSourceID="DS_AmountAre"
                                             DataTextField="taxGrp_Description"  DataValueField="taxGrp_Id"    onchange="ddl_AmountAre_valueChange()">
                                            </asp:DropDownList>--%>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="12" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <div class="col-md-3 lblmTop8" style="margin-bottom:15px">

                                            <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%" OnCallback="ddl_VatGstCst_Callback">
                                                  <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-12">         
                

                                            <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="OrderDetails_Id" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" 
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                                 OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                               Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150" RowHeight="2">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0"
                                                         Caption="">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                     <%--Batch Product Popup Start--%>
                                                     <dxe:GridViewDataTextColumn Caption="Indent" FieldName="Indent_Num" ReadOnly="True" Width="6%" VisibleIndex="2">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="14%">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" LostFocus="ProductsGotFocus" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Batch Product Popup End--%>
                                                   <%-- <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="gvColProduct" VisibleIndex="2" Width="15%">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName"  EnableCallbackMode="true" CallbackPageSize="100">
                                                             <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                          <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColDiscription" Caption="Description" VisibleIndex="4"  Width="18%"  >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                         <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColQuantity" Caption="Quantity" VisibleIndex="5" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                             <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                              <ClientSideEvents LostFocus="QuantityTextChange"  />

                                                        </PropertiesTextEdit>
                                                           <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColUOM" Caption="UOM(Purc.)" VisibleIndex="6" Width="7%"  >
                                                        <PropertiesTextEdit>
                                                              <ClientSideEvents LostFocus="QuantityTextChange"  />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn Width="7%" VisibleIndex="6" Caption="Stk Details" >
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                     
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePrice" Caption="Purc.Price" VisibleIndex="7" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                               <ClientSideEvents LostFocus="QuantityTextChange"  />
                                                        </PropertiesTextEdit>
                                                          <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColDiscount" Caption="Disc(%)" VisibleIndex="8" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                               <ClientSideEvents LostFocus="DiscountTextChange"/>
                                                        </PropertiesTextEdit>
                                                          <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColAmount" Caption="Amount" VisibleIndex="9" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                          <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                   <%-- <dxe:GridViewDataTextColumn FieldName="gvColTaxAmount" Caption="Tax Amount" VisibleIndex="12" Width="6%">
                                                        <PropertiesTextEdit>
                                                              <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    
                                                    <dxe:GridViewDataButtonEditColumn FieldName="gvColTaxAmount" Caption="Charges" VisibleIndex="10" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColTotalAmountINR" Caption="Net Amount" VisibleIndex="11" Width="8%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                          <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="7%" VisibleIndex="12" Caption="Add New">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png" >
                                                                
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                            
                                                        </CustomButtons>
                                                         
                                                    </dxe:GridViewCommandColumn>
                                            <dxe:GridViewDataTextColumn Caption="Quotation No" FieldName="Indent" Width="0"  VisibleIndex="13">
                                                <PropertiesTextEdit >
                                                    <NullTextStyle ></NullTextStyle>
                                                    <ReadOnlyStyle ></ReadOnlyStyle>
                                                    <Style></Style>
                                                </PropertiesTextEdit>
                                                <HeaderStyle  />
                                                <CellStyle >
                                                </CellStyle>
                                            </dxe:GridViewDataTextColumn> 
                                                     <dxe:GridViewDataTextColumn FieldName="gvColStockQty" Caption="Stock Qty"   Width="0" >
                                                        <PropertiesTextEdit >
                                                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                            <NullTextStyle ></NullTextStyle>
                                                        <ReadOnlyStyle ></ReadOnlyStyle>
                                                        <Style ></Style>
                                                        </PropertiesTextEdit>
                                                         <HeaderStyle  />
                                                    <CellStyle >
                                                    </CellStyle>
                                                          <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                   <dxe:GridViewDataTextColumn FieldName="gvColStockUOM" Caption="Stk UOM"  
                                                       width="0">
                                                        <PropertiesTextEdit >
                                                             <NullTextStyle ></NullTextStyle>
                                                             <ReadOnlyStyle ></ReadOnlyStyle>
                                                             <Style ></Style>
                                                        </PropertiesTextEdit>
                                                       <HeaderStyle  />
                                                        <CellStyle > </CellStyle>
                                                   
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                                    CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                                               <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                            </dxe:ASPxGridView>
                                        </div>
                                        <div style="clear: both;"></div>
            
                                        <div class="col-md-12 pdTop15" >
                                             <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server"  AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtn_SaveRecordExits" runat="server"  AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server"  AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecordTaxs" runat="server"  AutoPostBack="False" Text="T&#818;axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <b><span id="tagged" style="display:none;color: red">This Purchase Order is tagged in other modules. Cannot Modify data except UDF</span></b>
                                             <uc1:VehicleDetailsControl runat="server" id="VehicleDetailsControl" />
                                        </div>
                                    </div>
                                     </dxe:ContentControl>
                            </ContentCollection>                        
                        </dxe:TabPage>
                         <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                        Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                        <ContentCollection>
                                            <dxe:PopupControlContentControl runat="server">
                                            </dxe:PopupControlContentControl>
                                        </ContentCollection>
                                        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                    </dxe:ASPxPopupControl>

                                    <%--Subhra Changes-----------01-02-2017--%>
                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="ComponentPanel_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                    <div class="row">
                                                        <div class="col-md-5 mbot5" id="DivBilling">
                                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">
                                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Our Billing Address</h5>
                                                                <div style="padding-right: 8px">
                                                                   <%-- <div class="col-md-4" style="height: auto; display:none;">
                                                                      
                                                                        <asp:Label ID="LblType" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8" style="display:none;">

                                                                    
                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxGridLookup ID="billingAddress" runat="server" TabIndex="5" ClientInstanceName="billingLookup"
                                                                                KeyFieldName="add_id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" SelectionMode="Single">
                                                                                <Columns>
                                                                                     
                                                                                    <dxe:GridViewDataColumn FieldName="address" Visible="true" VisibleIndex="1" Caption="Address" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="City_Name" Visible="true" VisibleIndex="2" Caption="City" Settings-AllowAutoFilter="False" Width="100">

                                                                                        
                                                                                    </dxe:GridViewDataColumn>
                                                                                    <dxe:GridViewDataColumn FieldName="State" Visible="true" VisibleIndex="3" Caption="State" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="pin_code" Visible="true" VisibleIndex="4" Caption="Zip" Width="80" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="Country_Name" Visible="true" VisibleIndex="5" Caption="Country" Width="100" Settings-AutoFilterCondition="Contains" />

                                                                                </Columns>
                                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                                    <Templates>
                                                                                        <StatusBar>
                                                                                            <table class="OptionsTable" style="float: right">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                    
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </StatusBar>
                                                                                    </Templates>

                                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                                    <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                                </GridViewProperties>
                                                                                <ClientSideEvents TextChanged="function(s, e) { GetBillingAddressDetailByAddressId(e)}" />
                                                                                <ClearButton DisplayMode="Auto">
                                                                                </ClearButton>
                                                                            </dxe:ASPxGridLookup>
                                                                           
                                                                        </div>
                                                                    </div>--%>


                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">                                                                       
                                                                        Address1:
                                                                       <span style="color: red;"> *</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress1" MaxLength="80" ClientInstanceName="ctxtAddress1" TabIndex="2"
                                                                                runat="server" Width="100%">
                                                                               
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="badd1" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                            Address2:
                                                                           

                                                                    </div>
                                                                    <%--Start of Address2 --%>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress2" MaxLength="80" ClientInstanceName="ctxtAddress2" TabIndex="3"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                   
                                                                        Address3:
                                                                     
                                                                    </div>
                                                                    <%--Start of Address3 --%>

                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtAddress3" MaxLength="80" ClientInstanceName="ctxtAddress3" TabIndex="4"
                                                                                runat="server" Width="100%">
                                                                            
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <%--Start of Landmark --%>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        <%--Code--%>
                                                                            Landmark:
                                                                             

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtlandmark" MaxLength="80" ClientInstanceName="ctxtlandmark" TabIndex="5"
                                                                                runat="server" Width="100%">
                                                                               
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of Country--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label2" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCountry" ClientInstanceName="CmbCountry" runat="server" TabIndex="6" ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                                                >
                                                                                <%--<ClearButton DisplayMode="Always"></ClearButton>--%>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                                
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bcountry" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of State--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label4" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="CmbState" runat="server" TabIndex="7"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"
                                                                                 OnCallback="cmbState_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }" EndCallback="cmbstate_endcallback"></ClientSideEvents>
                                                                             
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bstate" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--start of City/district.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label6" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCity" ClientInstanceName="CmbCity" runat="server" TabIndex="8"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True"
                                                                                EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="cmbCity_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }" EndCallback="cmbcity_endcallback"></ClientSideEvents>
                                                                                
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bcity" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--start of Pin/Zip.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label8" runat="server" Text="Pin/Zip:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbPin" ClientInstanceName="CmbPin" runat="server" TabIndex="9"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="cmbPin_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbPin_endcallback"></ClientSideEvents>
                                                                                
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="bpin" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>

                                                                        </div>
                                                                    </div>
                                                                    <%--start of Area--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label10" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxComboBox ID="CmbArea" ClientInstanceName="CmbArea" runat="server" TabIndex="10"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0" OnCallback="cmbArea_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbArea_endcallback"></ClientSideEvents>
                                                                            </dxe:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="col-md-12" style="height: auto;">
                                                                        <dxe:ASPxCheckBox ID="chkBilling" runat="server" ClientInstanceName="cchkBilling" Text="Shipping to be in the same location of Billing. ">
                                                                            <ClientSideEvents CheckedChanged="function(s,e){CopyBillingAddresstoShipping(s);}"></ClientSideEvents>
                                                                        </dxe:ASPxCheckBox>
                                                                    </div>

                                                                   
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="col-md-5 mbot5" id="DivShipping">
                                                            <div class="clearfix" style="background: #fff; border: 1px solid #ccc; padding: 0px 0 10px 0;">

                                                                <h5 class="headText" style="margin: 0; padding: 10px 15px 10px; background: #ececec; margin-bottom: 10px">Our Shipping Address</h5>
                                                                <div style="padding-right: 8px">
                                                                    <%--<div class="col-md-4" style="height: auto; display:none;">
                                                                        
                                                                        <asp:Label ID="Label1" runat="server" Text="Select Address:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8" style="display:none;">


                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxGridLookup ID="shippingAddress" runat="server" TabIndex="5" ClientInstanceName="shippingLookup"
                                                                                KeyFieldName="add_id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" SelectionMode="Single">
                                                                                <Columns>
                                                                                    
                                                                                    <dxe:GridViewDataColumn FieldName="address" Visible="true" VisibleIndex="1" Caption="Address" Width="250" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="City_Name" Visible="true" VisibleIndex="2" Caption="City" Settings-AllowAutoFilter="False" Width="100">

                                                                                         
                                                                                    </dxe:GridViewDataColumn>
                                                                                    <dxe:GridViewDataColumn FieldName="State" Visible="true" VisibleIndex="3" Caption="State" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="pin_code" Visible="true" VisibleIndex="4" Caption="Zip" Width="80" Settings-AutoFilterCondition="Contains" />
                                                                                    <dxe:GridViewDataColumn FieldName="Country_Name" Visible="true" VisibleIndex="5" Caption="Country" Width="100" Settings-AutoFilterCondition="Contains" />

                                                                                </Columns>
                                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                                    <Templates>
                                                                                        <StatusBar>
                                                                                            <table class="OptionsTable" style="float: right">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                         
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </StatusBar>
                                                                                    </Templates>

                                                                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                                    <Settings ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                                                </GridViewProperties>
                                                                                <ClientSideEvents TextChanged="function(s, e) { GetShippingAddressDetailByAddressId(e)}" />
                                                                                <ClearButton DisplayMode="Auto">
                                                                                </ClearButton>
                                                                            </dxe:ASPxGridLookup>
                                                                             
                                                                        </div>
                                                                    </div>--%>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address1: <span style="color: red;">*</span>

                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxTextBox ID="txtsAddress1" MaxLength="80" ClientInstanceName="ctxtsAddress1" TabIndex="12"
                                                                                runat="server" Width="100%">
                                                                                <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxTextBox>
                                                                            <span id="sadd1" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address2:
                                                                           
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtsAddress2" MaxLength="80" ClientInstanceName="ctxtsAddress2" TabIndex="13"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Address3: 
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtsAddress3" MaxLength="80" ClientInstanceName="ctxtsAddress3" TabIndex="14"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto; margin-bottom: 5px;">
                                                                        Landmark: 
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxTextBox ID="txtslandmark" MaxLength="80" ClientInstanceName="ctxtslandmark" TabIndex="15"
                                                                                runat="server" Width="100%">
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4" style="height: auto;">

                                                                        <asp:Label ID="Label3" runat="server" Text="Country:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCountry1" ClientInstanceName="CmbCountry1" runat="server" TabIndex="16"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                                                                                SelectedIndex="0" >
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged1(s); }"></ClientSideEvents>
                                                                                <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="scountry" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of Country--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label5" runat="server" Text="State:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbState1" ClientInstanceName="CmbState1" runat="server" TabIndex="17"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True"
                                                                                SelectedIndex="0"  OnCallback="cmbState1_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged1(s); }" EndCallback="cmbshipstate_endcallback"></ClientSideEvents>
                                                                                <%--<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="sstate" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of State--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label7" runat="server" Text="City/District:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbCity1" ClientInstanceName="CmbCity1" runat="server" TabIndex="18"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="cmbCity1_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged1(s); }" EndCallback="cmbshipcity_endcallback"></ClientSideEvents>
                                                                                <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="scity" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of City/District--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label9" runat="server" Text="Pin/Zip:" CssClass="newLbl"></asp:Label><span style="color: red;">*</span>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content relative">
                                                                            <dxe:ASPxComboBox ID="CmbPin1" ClientInstanceName="CmbPin1" runat="server" TabIndex="19"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="cmbPin_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbshipPin_endcallback"></ClientSideEvents>
                                                                                <%--<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="Address" ErrorTextPosition="Right" ErrorImage-ToolTip="Mandatory" SetFocusOnError="True">
                                                                                    <ErrorImage ToolTip="Mandatory"></ErrorImage>

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>--%>
                                                                            </dxe:ASPxComboBox>
                                                                            <span id="spin" style="display: none" class="mandt">
                                                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI1" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                                            </span>
                                                                        </div>
                                                                    </div>
                                                                    <%--End of Pin/Zip.--%>
                                                                    <div class="col-md-4" style="height: auto;">
                                                                        <%--Type--%>
                                                                        <asp:Label ID="Label11" runat="server" Text="Area:" CssClass="newLbl"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-8">

                                                                        <div class="Left_Content">
                                                                            <dxe:ASPxComboBox ID="CmbArea1" ClientInstanceName="CmbArea1" runat="server" TabIndex="20"
                                                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0"  OnCallback="cmbArea1_OnCallback">
                                                                                <ClearButton DisplayMode="Always"></ClearButton>
                                                                                <ClientSideEvents EndCallback="cmbshipArea_endcallback"></ClientSideEvents>
                                                                            </dxe:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="col-md-12" style="height: auto;">
                                                                        <dxe:ASPxCheckBox ID="chkShipping" runat="server" ClientInstanceName="cchkShipping" Text="Billing to be in the same location of Shipping">
                                                                            <ClientSideEvents CheckedChanged="function(s,e){CopyShippingAddresstoBilling(s);}"></ClientSideEvents>
                                                                        </dxe:ASPxCheckBox>
                                                                    </div>
                                                                    <%--<div class="col-md-offset-4 col-md-8">
                                                                        <a href="#" onclick="javascript:openAreaPageShip();"><span class="Ecoheadtxt" style="color: Blue">
                                                                            <strong>Add New Area</strong></span></a>
                                                                    </div>--%>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--End of Address Type--%>




                                                    <%--End of Area--%>


                                                    <div class="clear"></div>
                                                    <div class="col-md-12 pdLeft0" style="padding-top: 10px">
                                                        <%--   <button class="btn btn-primary">OK</button> ValidationGroup="Address"--%>

                                                        <dxe:ASPxButton ID="btnSave_citys" CausesValidation="true" ClientInstanceName="cbtnSave_citys" runat="server" UseSubmitBehavior ="False"
                                                            AutoPostBack="False" Text="OK" CssClass="btn btn-primary" TabIndex="26">
                                                            <ClientSideEvents Click="function (s, e) {btnSave_QuoteAddress();}" />
                                                        </dxe:ASPxButton>

                                                    </div>
                                                </div>
                                            </dxe:PanelContent>
                                        </PanelCollection>
                                    <%--    <ClientSideEvents EndCallback="Panel_endcallback" />--%>
                                    </dxe:ASPxCallbackPanel>

                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>
                        
                    </TabPages>
                    <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>
                </dxe:ASPxPageControl>
            </div>
             <%--InlineTax--%>

            <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
                Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <HeaderTemplate>
                    <span style="color: #fff"><strong>Select Tax</strong></span>
                    <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <asp:HiddenField runat="server" ID="setCurrentProdCode" />
                        <asp:HiddenField runat="server" ID="HdSerialNo" />
                        <asp:HiddenField runat="server" ID="HdProdGrossAmt" />
                        <asp:HiddenField runat="server" ID="HdProdNetAmt" />
                        <asp:HiddenField ID="hdnPageStatus1" runat="server" />
                        <div id="content-6">
                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblTaxProdGrossAmt" runat="server" Text="" ClientInstanceName="clblTaxProdGrossAmt"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="col-sm-3 gstGrossAmount">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>GST</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblGstForGross" runat="server" Text="" ClientInstanceName="clblGstForGross"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Discount</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblTaxDiscount" runat="server" Text="" ClientInstanceName="clblTaxDiscount"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Net Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblProdNetAmt" runat="server" Text="" ClientInstanceName="clblProdNetAmt"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <div class="col-sm-2 gstNetAmount">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>GST</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxLabel ID="lblGstForNet" runat="server" Text="" ClientInstanceName="clblGstForNet"></dxe:ASPxLabel>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>

                        <%--Error Message--%>
                          <div id="ContentErrorMsg">
                            <div class="col-sm-8">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    Status
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tax Code/Charges Not defined.
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                              </div>





                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2"></td>
                            </tr>

                            <tr>
                                <td colspan="2"></td>
                            </tr>


                            <tr style="display: none">
                                <td><span><strong>Product Basic Amount</strong></span></td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" TabIndex="1" ReadOnly="true"
                                        runat="server" Width="50%">
                                        <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>

                            <tr class="cgridTaxClass">
                                <td colspan="3">
                                    <dxe:ASPxGridView runat="server" OnBatchUpdate="taxgrid_BatchUpdate" KeyFieldName="Taxes_ID" ClientInstanceName="cgridTax" ID="aspxGridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridTax_CustomCallback"
                                        Settings-ShowFooter="false" AutoGenerateColumns="False" OnCellEditorInitialize="aspxGridTax_CellEditorInitialize" OnHtmlRowCreated="aspxGridTax_HtmlRowCreated"
                                        OnRowInserting="taxgrid_RowInserting" OnRowUpdating="taxgrid_RowUpdating" OnRowDeleting="taxgrid_RowDeleting">
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Taxes_Name" ReadOnly="true" Caption="Tax Component ID">
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="taxCodeName" ReadOnly="true" Caption="Tax Component Name">
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                            <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>
                                        <ClientSideEvents EndCallback=" cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

                                    </dxe:ASPxGridView>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table class="InlineTaxClass">
                                        <tr class="GstCstvatClass" style="">
                                            <td style="padding-top: 10px; padding-bottom: 15px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; padding-bottom: 15px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1" TabIndex="2"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                    ClearButton-DisplayMode="Always" OnCallback="cmbGstCstVat_Callback">

                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>

                                                    <ClientSideEvents SelectedIndexChanged="cmbGstCstVatChange"
                                                        GotFocus="CmbtaxClick" />
                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; padding-bottom: 15px; padding-right: 25px">
                                                <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" TabIndex="3" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>


                                            </td>
                                            <td>
                                                <input type="button" onclick="recalculateTax()" class="btn btn-info btn-small RecalculateInline" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="pull-left">
                                        <%--<asp:Button ID="Button1" runat="server" Text="Ok" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />--%>
                                        <dxe:ASPxButton ID="Button1" ClientInstanceName="cButton1" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                <ClientSideEvents Click="function(s, e) {return BatchUpdate();}" />
                                            </dxe:ASPxButton>
                                         <%--<asp:Button ID="Button3" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />--%>
                                          <dxe:ASPxButton ID="Button3" ClientInstanceName="cButton3" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                                <ClientSideEvents Click="function(s, e) {cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;}" />
                                            </dxe:ASPxButton>
                                    </div>
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                    runat="server" Width="100%" CssClass="pull-left mTop">
                                                    <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" /> --%>
                                                </dxe:ASPxTextBox>

                                            </td>
                                        </tr>
                                    </table>


                                    <div class="clear"></div>
                                </td>
                            </tr>

                        </table>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                 <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
            </dxe:ASPxCallbackPanel>
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Purchase order Taxes" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="col-md-12  wrapHolder content horizontal-images" style="width: 100%; margin-right: 0;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Gross Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductAmount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforGross">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforGross" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforGross">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Discount</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductDiscount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductDiscount">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Total Charges</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductTaxAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductTaxAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Net Amount Total
                                                                <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li class="lblChargesGSTforNet">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>GST</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblChargesGSTforNet" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesGSTforNet">
                                                                </dxe:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                   <div class="clear">
                                </div>
                                <%--Error Msg--%>

                                <div class="col-md-8" id="ErrorMsgCharges">
                                <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Status
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                               Tax Code/Charges Not Defined.
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>

                                </div>

                                <div class="clear"> 
                                </div>
                                <div class="col-md-12 gridTaxClass" style="">
                                    <dxe:ASPxGridView runat="server" KeyFieldName="TaxID" ClientInstanceName="gridTax" ID="gridTax"
                                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                        Settings-ShowFooter="false" OnCustomCallback="gridTax_CustomCallback" OnBatchUpdate="gridTax_BatchUpdate"
                                        OnRowInserting="gridTax_RowInserting" OnRowUpdating="gridTax_RowUpdating" OnRowDeleting="gridTax_RowDeleting"
                                        OnDataBinding="gridTax_DataBinding">
                                        <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto"></Settings>
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <Columns>
                                            <dxe:GridViewDataTextColumn FieldName="TaxName" Caption="Tax" VisibleIndex="0" Width="40%" ReadOnly="true">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="calCulatedOn" Caption="Calculated On" VisibleIndex="0" Width="20%" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn FieldName="Percentage" Caption="Percentage" VisibleIndex="1" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange"  />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="QuotationTaxAmountTextChange" GotFocus="QuotationTaxAmountGotFocus" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnTaxEndCallback" />
                                        <SettingsDataSecurity AllowEdit="true" />
                                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                        </SettingsEditing>
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="col-md-12">
                                    <table style="" class="chargesDDownTaxClass">
                                        <tr class="chargeGstCstvatClass">
                                            <td style="padding-top: 10px; padding-right: 25px"><span><strong>GST</strong></span></td>
                                            <td style="padding-top: 10px; width: 200px;">
                                                <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0" TabIndex="2"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" TextFormatString="{0}"
                                                    OnCallback="cmbGstCstVatcharge_Callback">
                                                    <Columns>
                                                        <dxe:ListBoxColumn FieldName="Taxes_Name" Caption="Tax Component ID" Width="250" />
                                                        <dxe:ListBoxColumn FieldName="TaxCodeName" Caption="Tax Component Name" Width="250" />

                                                    </Columns>
                                                    <ClientSideEvents SelectedIndexChanged="ChargecmbGstCstVatChange"
                                                        GotFocus="chargeCmbtaxClick" />

                                                </dxe:ASPxComboBox>



                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px; width: 200px;">
                                                <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" TabIndex="3" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="<-999999999..999999999>.<0..99>" AllowMouseWheel="false" />
                                                </dxe:ASPxTextBox>

                                            </td>
                                            <td style="padding-left: 15px; padding-top: 10px">
                                                <input type="button" onclick="recalculateTaxCharge()" class="btn btn-info btn-small RecalculateCharge" value="Recalculate GST" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                    <br />
                                </div>



                                <div class="col-sm-3">
                                    <div>
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior ="False">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior ="False">
                                            <ClientSideEvents Click="function(s, e) {cPopup_Taxes.Hide();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>

                                <div class="col-sm-9">
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-right: 30px"><strong>Total Charges</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                        <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                                <div class="col-sm-2" style="padding-top: 8px;">
                                    <span></span>
                                </div>
                                <div class="col-sm-4">
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
             <%--   Inline Tax End    --%>  
            
            <%--   Warehouse     --%>
           <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="cPopup_WarehousePC"
        Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div id="content-6" class="pull-right wrapHolder reverse content horizontal-images" style="width: 100%; margin-right: 0px;height:auto;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tr>
                                                        <td>Selected Branch</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblbranchName" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tr>
                                                        <td>Selected Product</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblpro" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tr>
                                                        <td>Available Stock</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                                                         <asp:Label ID="lblAvailableStkunit" runat="server" ></asp:Label>
                                                            </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tr>
                                                        <td>Entered Stock</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblopeningstock" runat="server" Text="0.0000"></asp:Label>
                                                            <asp:Label ID="lblopeningstockUnit" runat="server" ></asp:Label>
                                                            </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                    
                    </div>

                    <div class="clear">
                        <br />
                    </div>
                    <div class="clearfix">
                        <div class="row manAb">
                            <div class="blockone">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatorCmbWarehousetxt">Warehouse</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <%-- <dxe:ASPxTextBox ID="txtwarehousname" runat="server" Width="80%" ClientInstanceName="ctxtwarehousname" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                            TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                            <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange(s)}" EndCallback="function(s,e){endcallcmware(s)}" ></ClientSideEvents>

                                        </dxe:ASPxComboBox>
                                        <span id="RequiredFieldValidatorCmbWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatorCmbWarehouseQuantity">Quantity</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="txtqnty" runat="server" Width="100%" ClientInstanceName="ctxtqnty" HorizontalAlign="Left" Font-Size="12px">
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqnty(s)}" LostFocus="function(s,e){Setenterfocuse(s)}" KeyPress="function(s, e) {Keypressevt();}"/>
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtwareqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="blocktwo">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxt">Batch</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <dxe:ASPxTextBox ID="txtbatch" runat="server" Width="100%" ClientInstanceName="ctxtbatch" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                            <ClientSideEvents TextChanged="function(s,e){chnagedbtach(s)}" KeyPress="function(s, e) {Keypressevt();}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtbatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                                <div class="col-md-3 blocktwoqntity">
                                    <div>
                                        <span id="RequiredFieldValidatorbatchQuantity">Quantity</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <dxe:ASPxTextBox ID="batchqnty" runat="server" Width="100%" ClientInstanceName="ctxtbatchqnty" HorizontalAlign="Left" Font-Size="12px">
                                            <MaskSettings Mask="<0..999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqntybatch(s)}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtbatchqntity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>

                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxtmkgdate">Manufacture Date</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%--<dxe:ASPxTextBox ID="txtmkgdate" runat="server" Width="80%" ClientInstanceName="ctxtmkgdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxDateEdit ID="txtmkgdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtmkgdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtbatchtxtexpdate">Expiry Date</span>
                                    </div>
                                    <div class="Left_Content" style="">
                                        <%-- <dxe:ASPxTextBox ID="txtexpirdate" runat="server" Width="80%" ClientInstanceName="ctxtexpirdate" HorizontalAlign="Left" Font-Size="12px">
                                    </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxDateEdit ID="txtexpirdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxtexpirdate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>

                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="blockthree">
                                <div class="col-md-3">
                                    <div>
                                        <span id="RequiredFieldValidatortxtserialtxt">Serial No</span>
                                    </div>
                                    <div class="Left_Content relative" style="">
                                        <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="100%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                        <ClientSideEvents KeyPress="function(s, e) {Keypressevt();}" />
                                        </dxe:ASPxTextBox>
                                        <span id="RequiredFieldValidatortxtserial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div>
                                </div>
                                <div class=" clearfix" style="padding-top: 11px;">
                                    <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left" UseSubmitBehavior ="False">
                                        <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left"  UseSubmitBehavior ="False">
                                        <ClientSideEvents Click="function(s, e) {Clraear();}" />
                                    </dxe:ASPxButton>

                                </div>
                            </div>

                        </div>
                        <br />


                        <div class="clearfix">
                            <dxe:ASPxGridView ID="GrdWarehousePC" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False" SettingsBehavior-AllowSort="false"
                                Width="100%" ClientInstanceName="cGrdWarehousePC" OnCustomCallback="GrdWarehousePC_CustomCallback" OnDataBinding="GrdWarehousePC_DataBinding">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="viewWarehouseName"
                                        VisibleIndex="0">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="viewBatchNo"
                                        VisibleIndex="2">
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataDateColumn Caption="Manufacture Date" FieldName="viewMFGDate"
                                        VisibleIndex="2">
                                        <Settings AllowHeaderFilter="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                    </dxe:GridViewDataDateColumn>

                                    <dxe:GridViewDataDateColumn Caption="Expiry Date" FieldName="viewExpiryDate"
                                        VisibleIndex="2">
                                        <Settings AllowHeaderFilter="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>
                                    </dxe:GridViewDataDateColumn>
                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="viewQuantity"
                                        VisibleIndex="3">
                                        <Settings ShowInFilterControl="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                        VisibleIndex="5">
                                        <Settings ShowInFilterControl="False" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="viewSerialNo"
                                        VisibleIndex="4">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Action" FieldName="SrlNo" CellStyle-VerticalAlign="Middle" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Settings-ShowFilterRowMenu="False" Settings-AllowHeaderFilter="False" Settings-AllowAutoFilter="False" Width="100px">
                                        <EditFormSettings Visible="False" />
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="UpdateWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>','<%#Eval("SerialNo")%>','<%#Eval("isnew")%>','<%#Eval("viewQuantity")%>','<%#Eval("Quantity")%>')" title="update Details" class="pad">
                                                <img src="../../../assests/images/Edit.png" />
                                            </a>
                                            <a href="javascript:void(0);" onclick="DeleteWarehousebatchserial(<%#Eval("SrlNo")%>,'<%#Eval("BatchWarehouseID")%>','<%#Eval("viewQuantity")%>',<%#Eval("Quantity")%>,'<%#Eval("WarehouseID")%>','<%#Eval("BatchNo")%>')" title="delete Details" class="pad">
                                                <img src="../../../assests/images/crs.png" />
                                            </a>
                                        </DataItemTemplate>
                                    </dxe:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents EndCallback="function(s,e) { cGrdWarehousePCShowError(s.cpInsertError);}" />
                                <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>--ShowFilterRow="true" ShowFilterRowMenu="true" --%>
                                <SettingsPager Mode="ShowAllRecords" />
                                <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" VerticalScrollBarMode="Visible" VerticalScrollableHeight="190" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                            </dxe:ASPxGridView>
                        </div>
                        <br />
                        <div class="Center_Content" style="">
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary" UseSubmitBehavior ="False">
                                <ClientSideEvents Click="function(s, e) {SaveWarehouseAll();}" />
                            </dxe:ASPxButton>


                        </div>
                    </div>
                    
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
            <div id="hdnFieldWareHouse">
            <asp:HiddenField ID="hdfProductIDPC" runat="server" />
            <asp:HiddenField ID="hdfstockidPC" runat="server" />
            <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
            <asp:HiddenField ID="hdbranchIDPC" runat="server" />
            <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />

            <asp:HiddenField ID="hdnProductQuantity" runat="server" />

            <asp:HiddenField ID="hdniswarehouse" runat="server" />
            <asp:HiddenField ID="hdnisbatch" runat="server" />
            <asp:HiddenField ID="hdnisserial" runat="server" />
            <asp:HiddenField ID="hdndefaultID" runat="server" />

            <asp:HiddenField ID="hdnoldrowcount" runat="server" Value="0" />

            <asp:HiddenField ID="hdntotalqntyPC" runat="server" Value="0" />

            <asp:HiddenField ID="hdnoldwarehousname" runat="server" />
            <asp:HiddenField ID="hdnoldbatchno" runat="server" />
            <asp:HiddenField ID="hidencountforserial" runat="server" />
            <asp:HiddenField ID="hdnbatchchanged" runat="server" Value="0" />

            <asp:HiddenField ID="hdnrate" runat="server" Value="0" />
            <asp:HiddenField ID="hdnvalue" runat="server" Value="0" />

            <asp:HiddenField ID="oldhdnoldwarehousname" runat="server" Value="0" />

            <asp:HiddenField ID="oldhidencountforserial" runat="server" Value="0" />
            <asp:HiddenField ID="oldhdnbatchchanged" runat="server" Value="0" />
            <asp:HiddenField ID="hdnstrUOM" runat="server" />
            <asp:HiddenField ID="hdnenterdopenqnty" runat="server" />
            <asp:HiddenField ID="hdnnewenterqntity" runat="server" />

            <asp:HiddenField ID="hdnisoldupdate" runat="server" />
            <asp:HiddenField ID="hdncurrentslno" runat="server" />
            <asp:HiddenField ID="oldopeningqntity" runat="server" Value="0" />
            <asp:HiddenField ID="hdnisedited" runat="server" />

            <asp:HiddenField ID="hdnisnewupdate" runat="server" />

            <asp:HiddenField ID="hdnisviewqntityhas" runat="server" />
            <asp:HiddenField ID="hdndeleteqnity" runat="server" Value="0" />
            <asp:HiddenField ID="hdnisolddeleted" runat="server" Value="false" />

            <asp:HiddenField ID="hdnisreduing" runat="server" Value="false" />
            <asp:HiddenField ID="hdnoutstock" runat="server" Value="0" />

                <asp:HiddenField ID="hdnpcslno" runat="server" Value="0" />
            </div>
           
           <%--   Warehouse End    --%>  
 
         <%-- HiddenField --%>
            <div>
                <asp:HiddenField ID="hfControlData" runat="server"/>
                 <asp:HiddenField ID="hdfTagMendatory" runat="server" />

                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                 <asp:HiddenField ID="hdnOpening" runat="server" />
            </div>
         <%-- HiddenField End--%>  
         <%--UDF--%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" 
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
           <%-- <HeaderTemplate>
                <span>UDF</span>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png"  Cursor="pointer" cssClass="popUpHeader" >
                    <ClientSideEvents Click="function(s, e){ 
                        popup.Hide();
                    }" />
            </dxe:ASPxImage>
            </HeaderTemplate>--%>
            <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
            </ContentCollection>
            </dxe:ASPxPopupControl>
            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--End UDF--%> 
         <%--Batch Product Popup Start--%>

            <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Product Name</strong></label>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                            <Columns>
                                  <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220"> 
                                 <Settings AutoFilterCondition="Contains" /> 
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60" >
                                     <Settings AutoFilterCondition="Contains" /> 
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80" >
                                     <Settings AutoFilterCondition="Contains" /> 
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200" >
                                     <Settings AutoFilterCondition="Contains" /> 
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100" >
                                     <Settings AutoFilterCondition="Contains" /> 
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120" >
                                     <Settings AutoFilterCondition="Contains" /> 
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            </GridViewProperties>
                        </dxe:ASPxGridLookup>

                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_PurchaseOrderDetailsList" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                     <asp:sessionparameter name="campany_Id" sessionfield="LastCompany1" type="String" />
                    <asp:sessionparameter Type="String" name="FinYear" sessionfield="LastFinYear1"  />
                </SelectParameters>
            </asp:SqlDataSource>

            <%--Batch Product Popup End--%> 
        <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
       </dxe:ASPxCallbackPanel>
        <dxe:ASPxCallbackPanel runat="server" ID="acpContactPersonPhone" ClientInstanceName="cacpContactPersonPhone" OnCallback="acpContactPersonPhone_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acpContactPersonPhoneEndCall" />
       </dxe:ASPxCallbackPanel>
        
        <%--<asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='17')) as X Order By ID ASC"></asp:SqlDataSource>--%>
                <asp:SqlDataSource ID="SqlSchematype" runat="server" 
           SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='17' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) and Branch=@userbranch and comapanyInt=@company)) as X Order By ID ASC">
       <SelectParameters>
           <asp:sessionparameter name="userbranch" sessionfield="userbranch" type="string" />
              <asp:sessionparameter name="company" sessionfield="LastCompany1" type="string" />
              <asp:sessionparameter name="year" sessionfield="LastFinYear1" type="string" />
           
       </SelectParameters>
   </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlIndentRequisitionNo" runat="server" 
            SelectCommand="(Select '0' as Indent_Id,'Select' as Indent_RequisitionNumber) Union
            (select Indent_Id,Indent_RequisitionNumber from tbl_trans_Indent)"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Sqlvendor" runat="server" 
            SelectCommand="select '0' as cnt_internalId,'Select' as Name 
            union select cnt_internalId,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name 
            from tbl_master_contact  where cnt_contacttype='DV'"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrency" runat="server" 
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
         <%--<asp:SqlDataSource ID="DS_Branch" runat="server"
            SelectCommand="Select * From (select '0' as branch_id,'Select' as branch_description 
            union select branch_id,branch_description from tbl_master_branch)tbl "></asp:SqlDataSource>--%>
                <asp:SqlDataSource ID="DS_Branch" runat="server" 
            SelectCommand=""></asp:SqlDataSource>
         <asp:SqlDataSource ID="DS_SalesAgent" runat="server" 
            SelectCommand="select '0' as cnt_id,'Select' as Name
            union select cnt_id,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG'"></asp:SqlDataSource>
           <asp:SqlDataSource ID="DS_AmountAre" runat="server"
            SelectCommand="select '0'as taxGrp_Id,'Select'as taxGrp_Description
            union select taxGrp_Id,taxGrp_Description from tbl_master_taxgrouptype order by taxGrp_Id"></asp:SqlDataSource>


         <asp:SqlDataSource ID="CountrySelect" runat="server" 
                SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
            <asp:SqlDataSource ID="StateSelect" runat="server" 
                SelectCommand="SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
                <SelectParameters>
                    <asp:Parameter Name="State" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SelectCity" runat="server" 
                SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SelectArea" runat="server" 
                SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
                <SelectParameters>
                    <asp:Parameter Name="Area" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SelectPin" runat="server" 
                SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
                <SelectParameters>
                    <asp:Parameter Name="City" Type="string" />
                </SelectParameters>
            </asp:SqlDataSource>
    
    </div>
</asp:Content>
