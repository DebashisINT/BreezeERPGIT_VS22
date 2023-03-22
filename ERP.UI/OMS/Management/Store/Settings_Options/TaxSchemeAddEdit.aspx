<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                22-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Tax Scheme" Language="C#" AutoEventWireup="true" Inherits="ERP.OMS.Managemnent.Master.management_Master_TaxSchemeAddEdit" CodeBehind="TaxSchemeAddEdit.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <%--<script type="text/javascript" src="/assests/js/ajaxList_inner.js"></script>--%>

    <%--<script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>--%>

    <script language="javascript" type="text/javascript">

        function chkCompensationCheckedChange() {

            var newComponenet = cCmbTaxRates_TaxCode.GetText();
            var indx = newComponenet.lastIndexOf("(");
            var type = newComponenet.substring(indx);
            console.log(type);
            if (type == "(Others)") {
                gridLookup.SetEnabled(cchkCompensation.GetValue());
                cComponentPanel.SetEnabled(cchkCompensation.GetValue());
                gridLookup.Clear();

                gridLookup.SetEnabled(cchkCompensation.GetValue());
                cComponentPanel.SetEnabled(cchkCompensation.GetValue());
                gridLookup.Clear();
            }
        }


        function LookupGotFocus() {
            gridLookup.ShowDropDown();
        }

        function LedgerLookupGotFocus() {
            gridLookupLedger.ShowDropDown();
        }

        function disableControlForOthersType(val) {

            cCmbTaxRates_ProductClass.SetSelectedIndex(0)
            cCmbTaxRates_ProductClass.SetEnabled(val);

            cCmbCountryName.SetSelectedIndex(0);
            cCmbCountryName.SetEnabled(val);

            cCmbState.SetSelectedIndex(0);
            cCmbState.SetEnabled(val);

            cCmbCity.SetSelectedIndex(0);
            cCmbCity.SetEnabled(val);

            //  ctxtTaxRates_DateFrom.SetDate(new Date());
            //    ctxtTaxRates_DateFrom.SetEnabled(val);

            ctxtTaxRates_MinAmount.SetValue(0);
            ctxtTaxRates_MinAmount.SetEnabled(val);

            cCmbTaxRates_SurchargeApplicable.SetSelectedIndex(2);
            CmbTaxRates_SurchargeApplicable_ValueChange();
            cCmbTaxRates_SurchargeApplicable.SetEnabled(val);

            crdpExempted.SetSelectedIndex(1);
            crdpExempted.SetEnabled(val);
            gridLookup.Clear();

            gridLookupLedger.Clear();

            cComponentPanel.SetEnabled(val);
            //cLedgerComponentPanel.SetEnabled(val);

            ccmbGSTType.SetEnabled(val);
            ccmbGSTType.SetValue('1');

            gridLookup.SetEnabled(val);
            gridLookupLedger.SetEnabled(val);

        }

        function selectAll() {
            // cComponentPanel.PerformCallback('selectAll~true');
            gridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            //cComponentPanel.PerformCallback('selectAll~false');
            gridLookup.gridView.UnselectRows();
        }
        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            // gridLookup.Focus();
        }

        function GridLedgerselectAll() {
            gridLookupLedger.gridView.SelectRows();
        }
        function GridLedgerunselectAll() {
            gridLookupLedger.gridView.UnselectRows();
        }
        function GridLedgerCloseGridLookup() {
            gridLookupLedger.ConfirmCurrentSelection();
            gridLookupLedger.HideDropDown();
        }


        function disableOnlyGSTtype() {
            var newComponenet = cCmbTaxRates_TaxCode.GetText();
            var indx = newComponenet.lastIndexOf("(");
            var type = newComponenet.substring(indx);
            ccmbGSTType.SetEnabled(false);


            if (type == "(Others)") {
                ccmbGSTType.SetValue('1');
            }
            else {
                if (type == "(GST)") {
                    $('.clsGstType').show();
                } else {
                    ccmbGSTType.SetValue('1');
                    $('.clsGstType').hide();
                }
            }
        }


        function ComponentNameChange(s, e) {

            var newComponenet = s.GetText();
            var indx = newComponenet.lastIndexOf("(");
            var type = newComponenet.substring(indx);
            cchkCompensation.SetChecked(false);
            ccmbGSTType.SetEnabled(false);
            $("#divRoundingOff").hide();
            $("#hfRoundOffCheck").val("0");
            if (type == "(Others)") {

                disableControlForOthersType(false);
                ccmbGSTType.SetValue('1');
                gridLookup.gridView.UnselectRows();
                cchkCompensation.SetEnabled(true);

                //----- Added By : Samrat Roy 22/06/2017 , purpose : to check Rounding Off Visible & Rounding Disable -----
                debugger;
                $("#ddlRoundingOff").prop('disabled', false);
                var roundingOffVisible = false, roundingoffChecking = false;
                var array = JSON.parse($("#hfRoundOffList").val());

                $.each(array, function () {
                    if (this.Taxes_ID == cCmbTaxRates_TaxCode.GetValue() && this.Taxes_ApplicableOn == "A") { roundingOffVisible = true; $("#hfRoundOffCheck").val("1"); }
                    switch (newComponenet.substring(newComponenet.indexOf("(") + 1, newComponenet.indexOf(")"))) {
                        case "Sale":
                            if (this.Taxes_ApplicableFor == "S" && this.TaxRates_RoundingOff != "") { roundingoffChecking = true; $("#hfRoundOffCheck").val("1"); }
                            break;
                        case "Purchase":
                            if (this.Taxes_ApplicableFor == "P" && this.TaxRates_RoundingOff != "") { roundingoffChecking = true; $("#hfRoundOffCheck").val("1"); }
                            break;
                        case "Both":
                            if ((this.Taxes_ApplicableFor == "S" && this.TaxRates_RoundingOff != "") || (this.Taxes_ApplicableFor == "P" && this.TaxRates_RoundingOff != "")) { roundingoffChecking = true; $("#hfRoundOffCheck").val("1"); }
                            break;
                    }
                });
                if (roundingOffVisible == true) {
                    $("#divRoundingOff").show();
                    if (roundingoffChecking == true) {
                        $("#hfRoundOffCheck").val("1");
                        $("#ddlRoundingOff").prop('disabled', true);
                    }
                }
                //----- END : Samrat Roy 22/06/2017 , purpose : to check Rounding Off Visible & Rounding Disable -----


            }
            else {
                cchkCompensation.SetEnabled(false);
                disableControlForOthersType(true);
                cComponentPanel.PerformCallback(s.GetValue());

                //cLedgerComponentPanel.PerformCallback(s.GetValue());

                if (type == "(GST)") {
                    //   ccmbGSTType.SetEnabled(true);
                    $('.clsGstType').show();
                } else {
                    ccmbGSTType.SetValue('1');
                    //ccmbGSTType.SetEnabled(false);
                    $('.clsGstType').hide();
                }


            }

        }

        function componentEndCallBack(s, e) {
            debugger;
            gridLookup.gridView.Refresh();
            gridLookupLedger.gridView.Refresh();

        }

        function LedgerComponentEndCallBack(s, e) {
            debugger;
            gridLookupLedger.gridView.Refresh();

        }

        function ComponentID_Check(s, e) {
            var compId = s.GetText().trim();
            $.ajax({
                type: "POST",
                url: "TaxSchemeAddEdit.aspx/CheckUniqueName",
                data: JSON.stringify({ ComponentId: compId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == false) {
                        jAlert("Please enter unique name");
                        ctxt_schemename.SetText('');
                        ctxt_schemename.Focus();
                        return false;
                    }
                }

            });
        }



        function AllControlInitilize() {
            if (document.getElementById('hdStatus').value != 'ADD') {
                SetEditedData();
            }
            else {
                fn_PopOpen();
            }
        }

        function setEditableIfInUse(state) {
            cCmbTaxRates_ProductClass.SetEnabled(state);
            cCmbCountryName.SetEnabled(state);
            cCmbState.SetEnabled(state);
            cCmbCity.SetEnabled(state);
            ctxtTaxRates_DateFrom.SetEnabled(state);
            ctxtTaxRates_MinAmount.SetEnabled(state);
            cCmbTaxRates_SurchargeApplicable.SetEnabled(state);
            cCmbtxtTaxRates_SurchargeCriteria.SetEnabled(state);
            ctxtTaxRates_SurchargeAbove.SetEnabled(state);
            cCmbTaxRates_SurchargeOn.SetEnabled(state);
            ctxtTaxes_SurchargeRate.SetEnabled(state);
            crdpExempted.SetEnabled(state);
            $('#lstTaxRates_MainAccount').prop('disabled', true).trigger("chosen:updated");
            $('#lstTaxRates_reverseChargeMainAccount').prop('disabled', true).trigger("chosen:updated");
            $('#lstTaxRates_SubAccount').prop('disabled', true).trigger("chosen:updated");
        }


        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>

    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>

    <script type="text/javascript">
        function visibleValidation() {
            $("#MandatoryTaxCode").hide();
            $("#MandatoryState").hide();

            $("#MandatoryCountry").hide()

            $("#MandatoryCity").hide();
            $("#MandatoryDateFrom").hide();
            $("#MandatoryDateTo").hide();
            $("#MandatoryBigDate").hide();
            $("#MandatoryRateOrSlab").hide();
            $("#MandatoryRate").hide();
            $("#MandatorySlabCode").hide();
            $("#MandatorySurchargeApplicable").hide();
            $("#MandatorySurchargeCriteria").hide();
            $("#MandatorySurchargeAbove").hide();
            $("#MandatorySurchargeOn").hide();
            $("#MandatorySurchargeRate").hide();
            $("#MandatorylstTaxRates_MainAccount").hide();
            $("#MandatorylstTaxRates_reverseChargeMainAccount").hide();
            $("#Mandatorytxt_schemename").hide();
        }
        function fn_PopOpen() {
            if (!isSet) {
                visibleValidation();
                document.getElementById('hiddenedit').value = "";

                var val = cCmbTaxRates_SurchargeApplicable.GetValue();
                if (val == 'Y') {

                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
                }
                else {
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
                }

                var val1 = cCmbTaxRates_RateOrSlab.GetValue();

                if (val1 == 'R') {
                    ctxtTaxRates_Rate.SetText('');
                    ctxtTaxRates_MinAmount.SetText('');
                    cCmbTaxSlab_Code.SetSelectedIndex(0);

                    document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

                }
                else {
                    ctxtTaxRates_Rate.SetText('');
                    ctxtTaxRates_MinAmount.SetText('');
                    cCmbTaxSlab_Code.SetSelectedIndex(0);

                    document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
                }

                ctxt_schemename.Focus();
                cCmbCountryName.SetSelectedIndex(0);
                cCmbTaxSlab_Code.SetSelectedIndex(0);
                cCmbState.SetSelectedIndex(0);
                cCmbCity.SetSelectedIndex(0);
                cCmbTaxRates_TaxCode.SetSelectedIndex(-1);
                cCmbTaxRates_ProductClass.SetSelectedIndex(0);
                ctxt_schemename.SetText('');

                var date = new Date();
                //  ctxtTaxRates_DateFrom.SetDate(date); 

                cCmbTaxRates_RateOrSlab.SetSelectedIndex(1);
                ctxtTaxRates_Rate.SetText('');
                ctxtTaxRates_MinAmount.SetText('');

                cCmbtxtTaxRates_SurchargeCriteria.SetSelectedIndex(0);
                ctxtTaxRates_SurchargeAbove.SetText('0');
                cCmbTaxRates_SurchargeOn.SetSelectedIndex(0);
                ctxtTaxes_SurchargeRate.SetText('');
                crdpExempted.SetSelectedIndex(1);
                // ChangeSource();
                cCmbTaxRates_SurchargeApplicable.SetSelectedIndex(2);
                CmbTaxRates_SurchargeApplicable_ValueChange();
                // cPopup_Empcitys.Show();
                isSet = true;
            }
        }
        function btnSave_citys() {

            //if (ctxtMarkets_Code.GetText() != '' && ctxtMarkets_Name.GetText() != '') {


            visibleValidation();

            var val = cCmbTaxRates_RateOrSlab.GetValue();



            if (ctxt_schemename.GetText() == '') {
                $("#Mandatorytxt_schemename").show();
                return false;

            }
            else if (cCmbTaxRates_TaxCode.GetValue() == '0' || cCmbTaxRates_TaxCode.GetValue() == null) {
                //alert('Please Enter TaxCode');
                $("#MandatoryTaxCode").show();
                return false;
            }


            else if (val == '0') {
                //alert('Please Enter Rate Or Slab');
                $("#MandatoryRateOrSlab").show();
                return false;
            }


                //Bellow line commented by debjyoti 
                //Reason : Rate should be allow zero 
                //else if (val == 'R' && ctxtTaxRates_Rate.GetText() == "0.0") {
                //    //alert('Please Enter TaxRates Rate');
                //    $("#MandatoryRate").show();
                //    return false;
                //}



            else if (cCmbTaxRates_SurchargeApplicable.GetValue() == '0') {
                //alert('Please Enter Surcharge Applicable');
                $("#MandatorySurchargeApplicable").show();
                return false;
            }
            else if (ctxtTaxRates_DateFrom.GetValue() == null) {
                //alert('Please Enter Surcharge Applicable');
                $("#MandatoryDateFrom").show();
                return false;
            }
            else if (ctxtTaxRates_DateTo.GetValue() == null) {
                $("#MandatoryDateTo").show();
                return false;
            }
            else if (val == 'R' && ctxtTaxRates_Rate.GetText() == "") {
                //alert('Please Enter TaxRates Rate');
                $("#MandatoryRate").show();
                return false;
            }

            var maincountval = document.getElementById("hndTaxRates_MainAccount_hidden").value;
            if (maincountval == null || maincountval == "" || maincountval == 0) {

                $("#MandatorylstTaxRates_MainAccount").show();
                return false;
            }
            //
            //var reverseChargeMainAccountval = document.getElementById("hndTaxRates_reverseChargeMainAccount_hidden").value;
            //if (reverseChargeMainAccountval == null || reverseChargeMainAccountval == "" || reverseChargeMainAccountval == 0) {

            //    $("#MandatorylstTaxRates_reverseChargeMainAccount").show();
            //    return false;
            //}

            if (document.getElementById('hiddenedit').value == '') {


            }
            else {
                //  grid.PerformCallback('updatecity~' + GetObjectID('hiddenedit').value);
            }

            if (!(ctxtTaxRates_DateTo.GetDate() > ctxtTaxRates_DateFrom.GetDate())) {

                $("#MandatoryBigDate").show();
                return false;
            }

            if (document.getElementById('HdSchemeInUse').value == 'Yes') {
                if (ctxtTaxRates_DateTo.GetDate().localeFormat('yyyy-MM-dd') != globalEditedToDate.localeFormat('yyyy-MM-dd')) {
                    if (!(ctxtTaxRates_DateTo.GetDate() > globalEditedToDate)) {
                        jAlert('Tax Component has been used. So, you must enter To Date later than ' + globalEditedToDate.format('dd-MM-yyyy') + '.', "Alert", function () {
                            ctxtTaxRates_DateTo.SetDate(globalEditedToDate);
                            ctxtTaxRates_DateTo.Focus();
                        });

                        return false;
                    }
                }
            }

            var newComponenet = cCmbTaxRates_TaxCode.GetText();
            var indx = newComponenet.lastIndexOf("(");
            var type = newComponenet.substring(indx);
            if (type == "(GST)") {
                if (ccmbGSTType.GetValue() == "1") {
                    jAlert("Select GST type to proceed.", "Alert", function () {
                        ccmbGSTType.Focus();
                    })
                    return false;
                }
            }


            document.getElementById("hdComponentName").value = cCmbTaxRates_TaxCode.GetValue();

            $('#hdnCityId').val(cCmbCity.GetValue());
            ccmbGSTType.SetEnabled(true);
            cCmbCountryName.SetEnabled(true);
            cCmbTaxRates_SurchargeApplicable.SetEnabled(true);
            cCmbState.SetEnabled(true);
            cCmbTaxRates_ProductClass.SetEnabled(true);
            ctxtTaxRates_DateFrom.SetEnabled(true);
            return true;
        }


        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
        }
        function fn_Editcity(keyValue) {
            visibleValidation();
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {

                    grid.PerformCallback('Delete~' + keyValue);
                }
            });

        }
        function grid_EndCallBack() {

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved Successfully');
                    cPopup_Empcitys.Hide();
                }
                else if (grid.cpinsert == 'Invalid') {
                    jAlert("Should be compulsorily greater than the datefrom value of the earlier instance of rate-configuration of this 'TaxCode+ProductClass+Country+State+City' combination \n 'Please Try Another date!!'");
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpEdit != null) {

                cCmbTaxRates_TaxCode.SetValue(grid.cpEdit.split('~')[0]);
                cCmbTaxRates_ProductClass.SetValue(grid.cpEdit.split('~')[1]);
                ctxtTaxRates_DateFrom.SetText(grid.cpEdit.split('~')[2]);
                cCmbTaxRates_RateOrSlab.SetValue(grid.cpEdit.split('~')[4]);
                ctxtTaxRates_Rate.SetText(grid.cpEdit.split('~')[5]);
                ctxtTaxRates_MinAmount.SetText(grid.cpEdit.split('~')[6]);
                cCmbTaxSlab_Code.SetValue(grid.cpEdit.split('~')[7]);
                cCmbTaxRates_SurchargeApplicable.SetValue(grid.cpEdit.split('~')[8]);
                cCmbtxtTaxRates_SurchargeCriteria.SetValue(grid.cpEdit.split('~')[9]);
                ctxtTaxRates_SurchargeAbove.SetText(grid.cpEdit.split('~')[10]);
                cCmbTaxRates_SurchargeOn.SetValue(grid.cpEdit.split('~')[11]);
                ctxtTaxes_SurchargeRate.SetText(grid.cpEdit.split('~')[12]);

                cCmbCountryName.SetValue(grid.cpEdit.split('~')[15]);
                cCmbState.SetValue(grid.cpEdit.split('~')[13]);
                cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());


                if (grid.cpEdit.split('~')[14] != "0") {

                    cCmbCity.SetValue(grid.cpEdit.split('~')[14]);
                } else {
                    cCmbCity.SetText("Any");
                }


                /*Code  Added  By Sudip on 14122016 for Edit function*/


                document.getElementById('hiddenedit').value = grid.cpEdit.split('~')[18];
                document.getElementById('hndTaxRates_MainAccount_hidden').value = grid.cpEdit.split('~')[16];
                document.getElementById('hndTaxRates_reverseChargeMainAccount_hidden').value = grid.cpEdit.split('~')[24];
                document.getElementById('hndTaxRates_SubAccount_hidden').value = grid.cpEdit.split('~')[17];

                ctxt_schemename.SetText(grid.cpEdit.split('~')[19]);


                if (typeof (grid.cpEdit.split('~')[20]) != 'undefined') {
                    crdpExempted.SetSelectedIndex(grid.cpEdit.split('~')[20]);
                } else {

                    crdpExempted.SetSelectedIndex(1);
                }
                ctxtSequenceNo.SetValue(grid.cpEdit.split('~')[21]);

                var valuenew = document.getElementById('hndTaxRates_MainAccount_hidden').value;
                var valuenew1 = document.getElementById('hndTaxRates_SubAccount_hidden').value;

                ChangeSource();
                ChangeSourceReverseChargeMainAccount();
                //changeFunc();
                //ChangeSubSource();

                if (valuenew == "NONE") {

                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                }
                if (valuenew == "") {

                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                }
                else {
                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                    ChangeSubSource();
                }

                ChangeSubSource();

                var val = cCmbTaxRates_SurchargeApplicable.GetValue();
                if (val == 'Y') {

                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
                }
                else {
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
                }

                var val1 = cCmbTaxRates_RateOrSlab.GetValue();

                if (val1 == 'R') {
                    document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

                }
                else {
                    document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
                }

                //.................code end.......

                cPopup_Empcitys.Show();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Updated Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    //$("#MandatoryState").show();
                    //$("#MandatoryCity").show();
                    //alert("Please Select proper country state and city");
                    return true;
                }
                else if (grid.cpUpdateValid == "TaxCodeInvalid") {
                    jAlert("Please Select proper TaxCode");
                    return false;
                }
                else if (grid.cpUpdateValid == "dateInvalid") {
                    $("#MandatoryDateFrom").show();
                    //alert("Please Select proper date");
                    return false;
                }
            }


            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success')
                    jAlert('Deleted Successfully');
                else
                    jAlert("Error on deletion\n'Please Try again!!'")
            }
            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Record already Exists');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
        }


        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
            cCmbCity.PerformCallback(0);
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
        function OnCmbStateName_ValueChange() {
            cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
        }
        function CmbCity_EndCallback() {
            cCmbCity.SetSelectedIndex(0);
            cCmbCity.Focus();
        }
        function CmbCity_ValuChanged(s, e) {
            $('#sa').val(s.GetValue());
        }
        function CmbTaxRates_RateOrSlab_ValueChange() {
            var val = cCmbTaxRates_RateOrSlab.GetValue();

            if (val == 'R') {
                ctxtTaxRates_Rate.SetText('');
                ctxtTaxRates_MinAmount.SetText('');
                //ctxtTaxRates_SlabCode.SetText('0');
                cCmbTaxSlab_Code.SetSelectedIndex(0);

                document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

            }
            else {
                ctxtTaxRates_Rate.SetText('0');
                ctxtTaxRates_MinAmount.SetText('0');
                //ctxtTaxRates_SlabCode.SetText('');
                cCmbTaxSlab_Code.SetSelectedIndex(0);

                document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
            }
        }
        function CmbTaxRates_SurchargeApplicable_ValueChange() {
            var val = cCmbTaxRates_SurchargeApplicable.GetValue();
            if (val == 'Y') {

                document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
            }
            else {
                ctxtTaxes_SurchargeRate.SetText('0');
                cCmbtxtTaxRates_SurchargeCriteria.SetSelectedIndex(0);
                ctxtTaxRates_SurchargeAbove.SetText('0');

                document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
            }
        }

        var isSet = false;
        var globalEditedToDate;
        function SetEditedData() {
            $("#ddlRoundingOff").attr('disabled', true);
            dataValue = document.getElementById('hdEditedValue').value;
            if (!isSet) {

                ctxt_schemename.enabled = false;

                cCmbTaxRates_TaxCode.SetValue(dataValue.split('~')[0]);
                disableOnlyGSTtype();
                // ComponentNameChange(cCmbTaxRates_TaxCode);
                //cCmbTaxRates_TaxCode.SetEnabled(false);
                cCmbTaxRates_ProductClass.SetValue(dataValue.split('~')[1]);
                //ctxtTaxRates_DateFrom.SetText(dataValue.split('~')[2]);
                // ctxtTaxRates_DateTo.SetText(dataValue.split('~')[3]);
                //Date tiem control accept only date object so changed the last 
                var dateFrom = new Date(dataValue.split('~')[2]);
                var dateTo = new Date(dataValue.split('~')[3]);
                ctxtTaxRates_DateTo.SetDate(dateTo);
                ctxtTaxRates_DateFrom.SetDate(dateFrom);
                //Gloabal Date so we can check in edit mode that user can not reduce the date
                globalEditedToDate = dateTo;

                cCmbTaxRates_RateOrSlab.SetValue(dataValue.split('~')[4]);
                ctxtTaxRates_Rate.SetText(dataValue.split('~')[5]);
                ctxtTaxRates_MinAmount.SetText(dataValue.split('~')[6]);
                cCmbTaxSlab_Code.SetValue(dataValue.split('~')[7]);
                cCmbTaxRates_SurchargeApplicable.SetValue(dataValue.split('~')[8]);
                cCmbtxtTaxRates_SurchargeCriteria.SetValue(dataValue.split('~')[9]);
                ctxtTaxRates_SurchargeAbove.SetText(dataValue.split('~')[10]);
                cCmbTaxRates_SurchargeOn.SetValue(dataValue.split('~')[11]);
                ctxtTaxes_SurchargeRate.SetText(dataValue.split('~')[12]);

                cCmbCountryName.SetValue(dataValue.split('~')[15]);
                cCmbState.SetValue(dataValue.split('~')[13]);
                //  cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());


                if (dataValue.split('~')[14] != "0") {

                    cCmbCity.SetValue(dataValue.split('~')[14]);
                } else {
                    cCmbCity.SetText("Any");
                }



                /*Code  Added  By Sudip on 14122016 for Edit function*/


                document.getElementById('hiddenedit').value = dataValue.split('~')[18];
                document.getElementById('hndTaxRates_MainAccount_hidden').value = dataValue.split('~')[16];
                document.getElementById('hndTaxRates_reverseChargeMainAccount_hidden').value = dataValue.split('~')[24];
                document.getElementById('hndTaxRates_SubAccount_hidden').value = dataValue.split('~')[17];
                ctxt_schemename.SetText(dataValue.split('~')[19]);


                if (typeof (dataValue.split('~')[20]) != 'undefined') {
                    crdpExempted.SetSelectedIndex(dataValue.split('~')[20]);
                } else {

                    crdpExempted.SetSelectedIndex(1);
                }

                ctxtSequenceNo.SetValue(dataValue.split('~')[21]);
                ccmbGSTType.SetValue(dataValue.split('~')[22]);


                console.log(dataValue.split('~')[23]);
                if (dataValue.split('~')[23] == "False") {
                    cchkCompensation.SetChecked(false);
                } else {
                    cchkCompensation.SetChecked(true);
                }



                var valuenew = document.getElementById('hndTaxRates_MainAccount_hidden').value;
                var valuenew1 = document.getElementById('hndTaxRates_SubAccount_hidden').value;

                ChangeSource();
                ChangeSourceReverseChargeMainAccount();
                //changeFunc();
                //ChangeSubSource();

                if (valuenew == "NONE") {

                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                }
                if (valuenew == "") {

                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                }
                else {
                    document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                    ChangeSubSource();
                }

                ChangeSubSource();

                var val = cCmbTaxRates_SurchargeApplicable.GetValue();
                if (val == 'Y') {

                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'block';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'block';
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'block';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'block';
                }
                else {
                    document.getElementById('divtxtTaxes_SurchargeRate').style.display = 'none';
                    document.getElementById('divCmbTaxRates_SurchargeOn').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeCriteria').style.display = 'none';
                    document.getElementById('divTaxRates_SurchargeAbove').style.display = 'none';
                }

                var val1 = cCmbTaxRates_RateOrSlab.GetValue();

                if (val1 == 'R') {
                    document.getElementById('divtxtTaxRates_Rate').style.display = 'block';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'block';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'none';

                }
                else {
                    document.getElementById('divtxtTaxRates_Rate').style.display = 'none';
                    document.getElementById('divtxtTaxRates_MinAmount').style.display = 'none';
                    document.getElementById('divtxtTaxRates_SlabCode').style.display = 'block';
                }

                // ComponentNameChange(cCmbTaxRates_TaxCode);
                //To Disable Extra Control
                var newComponenet = cCmbTaxRates_TaxCode.GetText();
                var indx = newComponenet.lastIndexOf("(");
                var type = newComponenet.substring(indx);
                console.log(type);
                if (type == "(Others)") {
                    disableControlForOthersType(false);
                }

                cCmbTaxRates_ProductClass.Focus();

                isSet = true;

                //Disable set of control if taxscheme in use
                if (document.getElementById('HdSchemeInUse').value == 'Yes') {
                    setEditableIfInUse(false);
                }
                chkCompensationCheckedChange();
                //Disabled Reverse Charge Posting Ledger forcefully
                debugger;
                if (document.getElementById('hndTaxRates_reverseChargeMainAccount_hidden').value == '') {
                    $("#lstTaxRates_reverseChargeMainAccount").attr("disabled", false);
                }
                //else {
                //    $("#lstTaxRates_reverseChargeMainAccount").attr("disabled", true);
                //}

                if (dataValue.split('~')[25] == "O" && dataValue.split('~')[26] == "A") {
                    $("#divRoundingOff").show();
                    $("#ddlRoundingOff").val(dataValue.split('~')[27]);

                    if (document.getElementById('hndTaxRates_reverseChargeMainAccount_hidden').value == '') {
                        $("#ddlRoundingOff").attr('disabled', false);
                    }
                }
            }
        }

    </script>
    <script type="text/javascript">
        /*Code  Added  By Sudip on 14122016 to use jquery Choosen*/

        $(document).ready(function () {
            ListBind();
            ChangeSource();
            ChangeSourceReverseChargeMainAccount();
            if (document.getElementById('hdStatus').value != 'ADD') {
                //  SetEditedData(document.getElementById('hdEditedValue').value);

                var newComponenet = $("#hftaxCodeName").val();
                var indx = newComponenet.lastIndexOf("(");
                var lastindx = newComponenet.lastIndexOf(")");
                var type = newComponenet.substring(indx, lastindx + 1);
                //if (type == "(Others)") {
                //    $("#divRoundingOff").show();
                //    $("#ddlRoundingOff").prop('disabled', true);
                //}
            }
            else {
                //  fn_PopOpen();
            }

        });
        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstTaxRates_MainAccount() {
            $('#lstTaxRates_MainAccount').fadeIn();
        }
        function Changeselectedvalue() {
            var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
            if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
                    if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                        lstTaxRates_MainAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
            }
        }
        function lstTaxRates_reverseChargeMainAccount() {
            $('#lstTaxRates_reverseChargeMainAccount').fadeIn();
        }
        function ChangeselectedvalueReverseChargeMainAccount() {
            var lstTaxRates_reverseChargeMainAccount = document.getElementById("lstTaxRates_reverseChargeMainAccount");
            if (document.getElementById("hndTaxRates_reverseChargeMainAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_reverseChargeMainAccount.options.length; i++) {
                    if (lstTaxRates_reverseChargeMainAccount.options[i].value == document.getElementById("hndTaxRates_reverseChargeMainAccount_hidden").value) {
                        lstTaxRates_reverseChargeMainAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_reverseChargeMainAccount').trigger("chosen:updated");
            }
        }
        function ChangeSource() {

            var SysTaxComponentScheme = $("#hdnSystemTaxComponentScheme").val();
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "TaxSchemeAddEdit.aspx/GetMainAccountList",
                data: JSON.stringify({ reqStr: fname, SysTaxCompScheme: SysTaxComponentScheme }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        lstTaxRates_MainAccount();
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                        Changeselectedvalue();
                    }
                    else {
                        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                    }
                }
            });
        }
        function ChangeSourceReverseChargeMainAccount() {
            var fname = "%";
            var lReportTo = $('select[id$=lstTaxRates_reverseChargeMainAccount]');
            lReportTo.empty();
            var SysTaxComponentScheme = $("#hdnSystemTaxComponentScheme").val();
            $.ajax({
                type: "POST",
                url: "TaxSchemeAddEdit.aspx/GetMainAccountList",
                data: JSON.stringify({ reqStr: fname, SysTaxCompScheme: SysTaxComponentScheme }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_reverseChargeMainAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        lstTaxRates_reverseChargeMainAccount();
                        $('#lstTaxRates_reverseChargeMainAccount').trigger("chosen:updated");
                        ChangeselectedvalueReverseChargeMainAccount();
                    }
                    else {
                        $('#lstTaxRates_reverseChargeMainAccount').trigger("chosen:updated");
                    }
                }
            });
        }
        function lstTaxRates_SubAccount() {
            $('#lstTaxRates_SubAccount').fadeIn();
        }
        function ChangeSubselectedvalue() {
            var lstTaxRates_SubAccount = document.getElementById("lstTaxRates_SubAccount");
            if (document.getElementById("hndTaxRates_SubAccount_hidden").value != '') {
                for (var i = 0; i < lstTaxRates_SubAccount.options.length; i++) {
                    if (lstTaxRates_SubAccount.options[i].value == document.getElementById("hndTaxRates_SubAccount_hidden").value) {
                        lstTaxRates_SubAccount.options[i].selected = true;
                    }
                }
                $('#lstTaxRates_SubAccount').trigger("chosen:updated");
            }
        }
        function ChangeSubSource() {
            var fname = "%";
            //var mainAccount = document.getElementById("lstTaxRates_MainAccount").value;;
            var mainAccount = document.getElementById("hndTaxRates_MainAccount_hidden").value;;
            var lReportTo = $('select[id$=lstTaxRates_SubAccount]');
            lReportTo.empty();

            $.ajax({
                type: "POST",
                url: "Config_TaxLevies.aspx/GetSubAccountList",
                data: JSON.stringify({ reqStr: fname, mainreqStr: mainAccount }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            $('#lstTaxRates_SubAccount').append($('<option>').text(name).val(id));
                        }

                        $(lReportTo).append(listItems.join(''));
                        ListBind();
                        lstTaxRates_SubAccount();
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                        ChangeSubselectedvalue();
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                    }
                    else {
                        $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                    }
                }
            });
        }
        function changeFunc() {
            var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
            document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;

            if (MainAccount_val2 == "NONE") {

                document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
            }
            if (MainAccount_val2 == "") {

                document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
            }
            else {
                document.getElementById('divtxtTaxRates_SubAccount').style.display = 'none';
                ChangeSubSource();
            }
        }
        function changeFuncreverseChargeMainAccount() {
            var reverseChargeMainAccount_val = document.getElementById("lstTaxRates_reverseChargeMainAccount").value;
            document.getElementById("hndTaxRates_reverseChargeMainAccount_hidden").value = reverseChargeMainAccount_val;
        }
        function changeSubFunc() {
            document.getElementById("hndTaxRates_SubAccount_hidden").value = document.getElementById("lstTaxRates_SubAccount").value;
        }
    </script>
    <style type="text/css">
        #lstTaxRates_MainAccount {
            width: 100%;
        }

        #lstTaxRates_MainAccount {
            display: none !important;
        }

        #lstTaxRates_MainAccount_chosen {
            width: 100% !important;
        }

        #lstTaxRates_reverseChargeMainAccount {
            width: 100%;
        }

        #lstTaxRates_reverseChargeMainAccount {
            display: none !important;
        }

        #lstTaxRates_reverseChargeMainAccount_chosen {
            width: 100% !important;
        }

        #lstTaxRates_SubAccount {
            width: 100%;
        }

        #lstTaxRates_SubAccount {
            display: none !important;
        }

        #lstTaxRates_SubAccount_chosen {
            width: 100% !important;
        }

        #txtTaxRates_Rate_EC, #txtTaxRates_SurchargeAbove_EC, #txtTaxes_SurchargeRate_EC, #txtTaxRates_MinAmount_EC {
            position: absolute;
        }
    </style>
    <style type="text/css">
        .cityDiv {
            height: 25px;
            width: 155px;
            float: left;
            margin-left: 70px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 400px;
            border: 2px;
        }



        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            padding-left: 15px;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
        /* Big box with list of options */ #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div {
                /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv {
                /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected {
                /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .pdl8 {
            padding-left: 8px;
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
            top: 9px;
            position: absolute;
        }

        .Left_Content {
            position: relative;
        }

        .col-md-6 {
            position: relative;
        }

        .pullleftClass {
            position: absolute;
            right: -17px;
            top: 5px;
        }

        #rdpExempted {
            width: 100%;
        }

            #rdpExempted table {
                width: auto !important;
            }

        .padBot5 {
            padding-top: 5px;
        }

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 6px;
            right: -2px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        #divNewTDS
        {
            margin-top: 15px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Add/Edit Tax Component Scheme</h3>
            <div class="crossBtn"><a href="Config_TaxLevies.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

        <div class="form_main">
        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
        <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
            <clientsideevents controlsinitialized="AllControlInitilize" />
        </dxe:ASPxGlobalEvents>



        <div class="row clearfix">
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    Tax Component ID<span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxTextBox ID="txt_schemename" ClientInstanceName="ctxt_schemename" runat="server"
                        Width="100%">

                        <clientsideevents textchanged="ComponentID_Check" />
                    </dxe:ASPxTextBox>

                    <span id="Mandatorytxt_schemename" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                </div>
            </div>

            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    <span>Tax Component Name</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbTaxRates_TaxCode" EnableIncrementalFiltering="true" ClientInstanceName="cCmbTaxRates_TaxCode"
                        runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" ForeColor="#000000">
                        <clientsideevents selectedindexchanged="ComponentNameChange" />
                    </dxe:ASPxComboBox>
                    <span id="MandatoryTaxCode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    Product Class
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbTaxRates_ProductClass" EnableIncrementalFiltering="true"
                        ClientInstanceName="cCmbTaxRates_ProductClass" runat="server" ValueType="System.String"
                        Width="100%" EnableSynchronization="True">
                    </dxe:ASPxComboBox>
                </div>
            </div>

            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    Country
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server"
                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        <clientsideevents valuechanged="function(s,e){OnCmbCountryName_ValueChange()}"></clientsideevents>
                    </dxe:ASPxComboBox>
                    <span id="MandatoryCountry" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div  class="clear"></div>
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    State
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="cCmbState" runat="server" ValueType="System.String"
                        Width="100%" EnableSynchronization="True" OnCallback="CmbState_Callback" EnableIncrementalFiltering="True">
                        <clientsideevents endcallback="CmbState_EndCallback" valuechanged="function(s,e){OnCmbStateName_ValueChange()}"></clientsideevents>
                    </dxe:ASPxComboBox>

                    <span id="MandatoryState" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    <span>City</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbCity" ClientInstanceName="cCmbCity" runat="server" ValueType="System.String"
                        Width="100%" EnableSynchronization="True" OnCallback="CmbCity_Callback" EnableIncrementalFiltering="True">
                        <clientsideevents valuechanged="CmbCity_ValuChanged"></clientsideevents>
                    </dxe:ASPxComboBox>

                    <span id="MandatoryCity" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    <span>Applicable From</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxDateEdit ID="txtTaxRates_DateFrom" runat="server" Width="100%" ClientInstanceName="ctxtTaxRates_DateFrom"
                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                    <span id="MandatoryDateFrom" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <%--To date--%>
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    <span>Applicable To</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxDateEdit ID="txtTaxRates_DateTo" runat="server" Width="100%" ClientInstanceName="ctxtTaxRates_DateTo"
                        EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                    <span id="MandatoryDateTo" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    <span id="MandatoryBigDate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Must Be Greater Than From Date"></span>
                </div>
            </div>

            <div class="clear"></div>
            <div class="col-md-3" style="display: none;">
                <div class="padBot5">
                    <span>Rate</span>  <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbTaxRates_RateOrSlab" EnableIncrementalFiltering="True" ClientInstanceName="cCmbTaxRates_RateOrSlab"
                        runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                        <clientsideevents valuechanged="function(s,e){CmbTaxRates_RateOrSlab_ValueChange()}"></clientsideevents>
                    </dxe:ASPxComboBox>
                    <span id="MandatoryRateOrSlab" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3" id="divtxtTaxRates_Rate" style="display: block">
                <div class="padBot5" style="display: block;">
                    <span>Rate</span> <%--<span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>--%>
                </div>
                <div/ class="Left_Content">
                    <dxe:ASPxTextBox ID="txtTaxRates_Rate" ClientInstanceName="ctxtTaxRates_Rate" runat="server"
                        Width="100%">
                        <masksettings mask="<0..99999g>.<00..999g>" />
                        <%-- <ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_Rate_I',event);}" />--%>
                    </dxe:ASPxTextBox>

                    <span id="MandatoryRate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div/>
            </div>

            <div class="col-md-3" id="divtxtTaxRates_MinAmount" style="display: block">
                <div class="padBot5" style="display: block;">
                    Min Amount
                </div>
                <div class="Left_Content">
                    <dxe:ASPxTextBox ID="txtTaxRates_MinAmount" ClientInstanceName="ctxtTaxRates_MinAmount"
                        runat="server" Width="100%">
                        <%--<ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_MinAmount_I',event);}" />--%>
                        <masksettings mask="<0..99999g>.<0..99g>" />
                    </dxe:ASPxTextBox>
                </div>
            </div>

            <div class="col-md-3" id="divtxtTaxRates_SlabCode" style="display: none">
                <div class="padBot5" style="display: block;">
                    Slab Code
                </div>
                <div class="Left_Content">
                    <%-- <dxe:ASPxTextBox ID="txtTaxRates_SlabCode" ClientInstanceName="ctxtTaxRates_SlabCode"
                                        runat="server" Width="100%">
                                    </dxe:ASPxTextBox>--%>
                    <dxe:ASPxComboBox ID="CmbTaxSlab_Code" ClientInstanceName="cCmbTaxSlab_Code" runat="server"
                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        <%-- <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>--%>
                    </dxe:ASPxComboBox>

                    <span id="MandatorySlabCode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    Surcharge Applicable <span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbTaxRates_SurchargeApplicable" EnableIncrementalFiltering="True"
                        ClientInstanceName="cCmbTaxRates_SurchargeApplicable" runat="server" ValueType="System.String"
                        Width="100%" EnableSynchronization="True">
                        <clientsideevents valuechanged="function(s,e){CmbTaxRates_SurchargeApplicable_ValueChange()}"></clientsideevents>
                    </dxe:ASPxComboBox>

                    <span id="MandatorySurchargeApplicable" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="col-md-3" id="divTaxRates_SurchargeCriteria">
                <div class="padBot5" style="display: block;">
                    Surcharge Criteria
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbtxtTaxRates_SurchargeCriteria" EnableIncrementalFiltering="True"
                        ClientInstanceName="cCmbtxtTaxRates_SurchargeCriteria" runat="server" ValueType="System.String"
                        Width="100%" EnableSynchronization="True">
                    </dxe:ASPxComboBox>
                    <%--<dxe:ASPxTextBox ID="txtTaxRates_SurchargeCriteria" ReadOnly="true" ClientInstanceName="ctxtTaxRates_SurchargeCriteria"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>--%>

                    <span id="MandatorySurchargeCriteria" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>

            <div class="col-md-3" id="divTaxRates_SurchargeAbove">
                <div class="padBot5" style="display: block;">
                    Surcharge Above
                </div>
                <div class="Left_Content">
                    <dxe:ASPxTextBox ID="txtTaxRates_SurchargeAbove" ClientInstanceName="ctxtTaxRates_SurchargeAbove"
                        runat="server" Width="100%">
                        <%--  <ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxRates_SurchargeAbove_I',event);}" />--%>
                        <masksettings mask="<0..99999g>.<0..99g>" />
                    </dxe:ASPxTextBox>

                    <span id="MandatorySurchargeAbove" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3" id="divCmbTaxRates_SurchargeOn">
                <div class="padBot5" style="display: block;">
                    Surcharge On
                </div>
                <div class="Left_Content">
                    <dxe:ASPxComboBox ID="CmbTaxRates_SurchargeOn" EnableIncrementalFiltering="True"
                        ClientInstanceName="cCmbTaxRates_SurchargeOn" runat="server" ValueType="System.String"
                        Width="100%" EnableSynchronization="True">
                    </dxe:ASPxComboBox>

                    <span id="MandatorySurchargeOn" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>

            <div class="col-md-3" id="divtxtTaxes_SurchargeRate">
                <div class="padBot5" style="display: block;">
                    Surcharge Rate
                </div>
                <div class="Left_Content">
                    <dxe:ASPxTextBox ID="txtTaxes_SurchargeRate" ClientInstanceName="ctxtTaxes_SurchargeRate"
                        runat="server" Width="100%">
                        <%--<ClientSideEvents KeyPress="function(s,e){ return CheckDecimal('Popup_Empcitys_txtTaxes_SurchargeRate_I',event);}" />--%>
                        <masksettings mask="<0..99999g>.<0..99g>" />
                    </dxe:ASPxTextBox>

                    <span id="MandatorySurchargeRate" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                </div>
            </div>

            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    <span>Main Account</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px;">*</span>
                </div>
                <div class="Left_Content">
                    <%--<asp:TextBox ID="txtTaxRates_MainAccount" Width="100%" runat="server" onkeyup="FunCallAjaxList(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_MainAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>
                    <%--onmouseout="onBlrTXT();"   --%>

                    <asp:ListBox ID="lstTaxRates_MainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFunc();"></asp:ListBox>
                    <span id="MandatorylstTaxRates_MainAccount" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    <asp:HiddenField ID="hndTaxRates_MainAccount_hidden" runat="server" />
                </div>
            </div>
            <div class="col-md-3" id="divtxtTaxRates_SubAccount" style="display: none;">
                <div class="padBot5" style="display: block; height: auto;">
                    Sub Account
                </div>
                <div class="Left_Content">
                    <%-- <asp:TextBox ID="txtTaxRates_SubAccount" Width="176px" runat="server" onkeyup="FunCallAjaxList2(this,event,'Digital')"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_SubAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>

                    <asp:ListBox ID="lstTaxRates_SubAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeSubFunc();"></asp:ListBox>
                    <asp:HiddenField ID="hndTaxRates_SubAccount_hidden" runat="server" />
                </div>
            </div>

            <div class="col-md-3">
                <div class="padBot5" style="display: block;">
                    <span>Reverse Charge Posting Ledger</span><span style="text-align: left; float: left; font-size: medium; color: Red; width: 8px; display: none;">*</span>
                </div>
                <div class="Left_Content">
                    <%--<asp:TextBox ID="txtTaxRates_MainAccount" Width="100%" runat="server" onkeyup="FunCallAjaxList(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_MainAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>
                    <%--onmouseout="onBlrTXT();"   --%>

                    <asp:ListBox ID="lstTaxRates_reverseChargeMainAccount" CssClass="chsn" runat="server" Font-Size="12px" Width="100%" data-placeholder="Select..." onchange="changeFuncreverseChargeMainAccount();"></asp:ListBox>
                    <span id="MandatorylstTaxRates_reverseChargeMainAccount" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    <asp:HiddenField ID="hndTaxRates_reverseChargeMainAccount_hidden" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-3" id="divtxtTaxRates_Exempted" style="padding-top: 4px">
                <div class="padBot5" style="display: block; height: auto;">
                    Exempted
                </div>
                <div class="Left_Content">
                    <%-- <asp:TextBox ID="txtTaxRates_SubAccount" Width="176px" runat="server" onkeyup="FunCallAjaxList2(this,event,'Digital')"></asp:TextBox>
                                <asp:TextBox ID="txtTaxRates_SubAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>--%>

                    <dxe:ASPxRadioButtonList ID="rdpExempted" runat="server" ClientInstanceName="crdpExempted" ValueType="System.String" RepeatDirection="Horizontal">
                        <items>
                                        <dxe:ListEditItem Text="Yes" Value="1"/>
                                         <dxe:ListEditItem Text="No" Value="0"/>
                                        
                                        
                                    </items>
                    </dxe:ASPxRadioButtonList>
                </div>
            </div>

            <%--Compensation--%>
            <div class="col-md-3" style="padding-top: 4px">
                <div class="padBot5" style="display: block; height: auto;">
                    Compensation Cess?
                </div>
                <dxe:ASPxCheckBox ID="chkCompensation" runat="server" ClientInstanceName="cchkCompensation">
                    <clientsideevents checkedchanged="chkCompensationCheckedChange" />
                </dxe:ASPxCheckBox>
            </div>
            <%--End Of Compensation--%>


            <%--Product SelectTion--%>

            <dxe:ASPxCallbackPanel runat="server" id="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">
                <panelcollection>
                        <dxe:PanelContent runat="server">
                            <div class="col-md-3" style="padding-top: 4px">
                                <div class="padBot5" style="display: block; height: auto;">
                                    Select HSN/SAC 
                                </div>
                                <dxe:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple"  ClientInstanceName="gridLookup"
                                    KeyFieldName="Code" Width="100%" TextFormatString="{0}" MultiTextSeparator=", "  OnDataBinding="grid_DataBinding">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                                                            
                                        </dxe:GridViewCommandColumn> 

                                            <dxe:GridViewDataColumn FieldName="Code" Caption="HSN / SAC" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="Description" Caption="Desctiprion" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                        <%--<dxe:GridViewDataColumn FieldName="sProducts_serviceTax" Caption="Services Accounting Code" Width="0">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="ProductClass_Name" Caption="Class" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Product Code" Width="150">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Product Name" Width="300">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>--%>

                                    </Columns>
                                    <GridViewProperties  Settings-VerticalScrollBarMode="Auto"   >
                                                                             
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                            
                                    </GridViewProperties>
                                    <ClientSideEvents GotFocus="LookupGotFocus" />
                                </dxe:ASPxGridLookup>
                              </div>
                            <div class="col-md-3 hide" style="padding-top: 4px">
                                <div class="padBot5" style="display: block; height: auto;">
                                    Select HSN/SAC Mapped With Ledger
                                </div>
                            <dxe:ASPxGridLookup ID="GridLookUpLedger" runat="server" SelectionMode="Multiple"  ClientInstanceName="gridLookupLedger"
                                KeyFieldName="MainAccount_ReferenceID" Width="100%" TextFormatString="{4}" MultiTextSeparator=", "  OnDataBinding="gridLookupLedger_DataBinding">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                                                            
                                    </dxe:GridViewCommandColumn> 

                                        <dxe:GridViewDataColumn FieldName="HSNSACCode" Caption="HSN/SAC Code" Width="150">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                      <dxe:GridViewDataColumn FieldName="HSN_SAC_Type" Caption="HSN/SAC Type" Width="150">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="HSNSACDescription" Caption="Description" Width="150">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="MainAccount_AccountType" Caption="Type" Width="150">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="MainAccount_Name" Caption="Name" Width="300">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties  Settings-VerticalScrollBarMode="Auto"   >
                                                                             
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="GridLedgerselectAll" />
                                                            <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="GridLedgerunselectAll" />
                                                            <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="GridLedgerCloseGridLookup" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                            
                                </GridViewProperties>
                                <ClientSideEvents GotFocus="LedgerLookupGotFocus" />
                            </dxe:ASPxGridLookup>
                               </div>
                        </dxe:PanelContent>
                    </panelcollection>
                <clientsideevents endcallback="componentEndCallBack" />
            </dxe:ASPxCallbackPanel>

            <%--Product SelectTion--%>


           <%--  <dxe:ASPxCallbackPanel runat="server" id="ASPxCallbackPanelLedger" ClientInstanceName="cLedgerComponentPanel" OnCallback="LedgerComponent_Callback">
                    <panelcollection>
                        <dxe:PanelContent runat="server">

                    </panelcollection>
                <clientsideevents endcallback="LedgerComponentEndCallBack" />
            </dxe:ASPxCallbackPanel>--%>
            <div style="clear: both"></div>

            <%--Sequence Number--%>
            <div class="col-md-3" style="padding-top: 4px">
                <div class="padBot5" style="display: block; height: auto;">
                    Sequence Number
                </div>
                <dxe:ASPxTextBox ID="txtSequenceNo" ClientInstanceName="ctxtSequenceNo" runat="server"
                    Width="100%">
                    <masksettings mask="<0..99999>" />
                </dxe:ASPxTextBox>
            </div>
            <%--End Of Sequence  Number--%>

            <%--GST Type Number--%>
            <div class="col-md-3 clsGstType" style="padding-top: 4px">
                <div class="padBot5" style="display: block; height: auto;">
                    GST Type
                </div>
                <dxe:ASPxComboBox ID="cmbGSTType" ClientInstanceName="ccmbGSTType" runat="server"
                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                    <items>
                        <dxe:ListEditItem Text="-Select-" Value="1" />
                        <dxe:ListEditItem Text="IGST" Value="I" />
                            <dxe:ListEditItem Text="SGST" Value="S" />
                            <dxe:ListEditItem Text="CGST" Value="C" />
                            <dxe:ListEditItem Text="UTGST" Value="U" />
                    </items>
                </dxe:ASPxComboBox>
            </div>
            <%--End Of GST Type Number--%>

            <%--Rounding Off--%>
            <div id="divRoundingOff" class="col-md-3" style="padding-top: 4px; display: none;" >
                <div class="padBot5"  height: auto;">
                    Rounding Off
                </div>
                <asp:DropDownList ID="ddlRoundingOff" CssClass="form-control" runat="server">
                    <asp:ListItem Value="R+">Rounding Off (+)</asp:ListItem>
                    <asp:ListItem Value="R-">Rounding Off (-)</asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hfRoundOffList" runat="server" />
                <asp:HiddenField ID="hfRoundOffCheck" runat="server" />
                <asp:HiddenField ID="hftaxCodeName" runat="server" />
            </div>
            <%--End Of Rounding Off--%>
        </div>

         

        <div class="ContentDiv clearfix">
            <div style="display: none">
                <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                    <h5>Static Code</h5>
                </div>
                <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray; float: left;">
                    Exchange
                </div>
                <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                    Value
                </div>
                <div class="ScrollDiv">
                    <div class="cityDiv" style="padding-top: 5px;">
                        NSE Code
                    </div>
                    <div style="padding-top: 5px;">
                        <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        BSE Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        MCX Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        MCXSX Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        NCDEX Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        CDSL Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                            runat="server">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        NSDL Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                            runat="server">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        NDML Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        CVL Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                    <br style="clear: both;" />
                    <div class="cityDiv">
                        DOTEX Code
                    </div>
                    <div>
                        <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                            CssClass="cityTextbox">
                        </dxe:ASPxTextBox>
                    </div>
                </div>
            </div>
            <div class="clearfix" style="padding-top: 15px">

                <dxe:ASPxButton ID="btnSaveTaxScheme" ClientInstanceName="cbtnSave_citys" runat="server" AutoPostBack="false" OnClick="btnSaveTaxScheme_Click"
                    Text="Save" CssClass="btn btn-primary">
                    <clientsideevents click="function (s, e) {e.processOnServer= btnSave_citys();}" />
                </dxe:ASPxButton>


                <%--<dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                              
                            </dxe:ASPxButton>--%>
            </div>
        </div>

        <%-- </div>--%>
    </div>
    </div>

    <asp:HiddenField runat="server" ID="hiddenedit" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdnCityId" runat="server" />
    <asp:HiddenField ID="hdStatus" runat="server" />
    <asp:HiddenField ID="hdEditedValue" runat="server" />
    <asp:HiddenField ID="hdComponentName" runat="server" />
    <asp:HiddenField ID="hdnSystemTaxComponentScheme" runat="server" />
    
    <asp:HiddenField ID="HdSchemeInUse" runat="server" />
</asp:Content>
