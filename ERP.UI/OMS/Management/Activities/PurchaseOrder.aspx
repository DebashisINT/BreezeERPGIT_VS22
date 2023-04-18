<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-04-2023        2.0.37           Pallab              25834: Add Purchase Order module design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="PurchaseOrder" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs"
    Inherits="ERP.OMS.Management.Activities.PurchaseOrder" EnableEventValidation="false" %>

<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
    <script src="JS/SearchPopupDatatable.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    <script src="JS/JSPurchaseOrder.js?v=4.10"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelPurchase.js"></script>
    <style type="text/css">
        .HeaderStyle {
            background-color: #180771d9;
            color: #f5f5f5;
        }

        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        #gridTax_DXStatus {
            display: none;
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

        .POIndentReq {
            position: absolute;
            right: -3px;
            top: 22px;
        }

        .POVendor {
            position: absolute;
            right: 2px;
            top: 22px;
        }

        .PODate {
            position: absolute;
            right: 2px;
            top: 22px;
        }

        .PODueDate {
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

        /*#grid_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }*/

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

        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .eqTble > tbody > tr > td {
            padding: 0 7px;
            vertical-align: top;
        }
          .mlableWh{
            padding-top: 22px;
            display:inline-block
        }
        .mlableWh>input +span {
            white-space: nowrap;
        }
    </style>




    <%-- ------Subhra Address and Billing Sectin Start-----25-01-2017---------%>
    <script type="text/javascript">
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';

        //$(document).ready(function () {
        //    SetRunningTotal();
        //})


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
                    // GetIndentREquiNo();
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
            if (ctxtRevisionDate.GetDate() < cPLQuoteDate.GetDate()) {
                ctxtRevisionDate.Clear();
            }

        }


        //Rev Bapi
        $(document).ready(function () {

            $("#UOMQuantity").on('blur', function () {
                var currentObj = $(this);
                var currentVal = currentObj.val();
                if (!isNaN(currentVal)) {
                    var updatedVal = parseFloat(currentVal).toFixed(4);
                    currentObj.val(updatedVal);
                }
                else {
                    currentObj.val("");
                }
            })


        })
        //End Rev Bapi

    </script>
    <%-- ------Subhra Address and Billing Section End-----25-01-2017---------%>
    <script type="text/javascript">
        //function cmbtaxCodeindexChange(s, e) {
        //    if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

        //        var taxValue = s.GetValue();

        //        if (taxValue == null) {
        //            taxValue = 0;
        //            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //            cgridTax.GetEditor("Amount").SetValue(0);
        //            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
        //        }


        //        var isValid = taxValue.indexOf('~');
        //        if (isValid != -1) {
        //            var rate = parseFloat(taxValue.split('~')[1]);
        //            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

        //            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


        //            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
        //            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
        //            GlobalCurTaxAmt = 0;
        //        }
        //        else {
        //            s.SetText("");
        //        }

        //    } else {
        //        var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

        //        if (s.GetValue() == null) {
        //            s.SetValue(0);
        //        }

        //        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {

        //            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

        //            ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
        //            GlobalCurTaxAmt = 0;
        //        } else {
        //            s.SetText("");
        //        }
        //    }

        //}

        //function SetOtherTaxValueOnRespectiveRow(idx, amt, name) {
        //    for (var i = 0; i < taxJson.length; i++) {
        //        if (taxJson[i].applicableBy == name) {
        //            cgridTax.batchEditApi.StartEdit(i, 3);
        //            cgridTax.GetEditor('calCulatedOn').SetValue(amt);

        //            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
        //            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        //            var s = cgridTax.GetEditor("TaxField");
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
        //                GlobalCurTaxAmt = 0;
        //            }




        //        }
        //    }
        //    //return;
        //    cgridTax.batchEditApi.EndEdit();

        //}



        //function SetOtherChargeTaxValueOnRespectiveRow(idx, amt, name) {
        //    name = name.substring(0, name.length - 3).trim();
        //    for (var i = 0; i < chargejsonTax.length; i++) {
        //        if (chargejsonTax[i].applicableBy == name) {
        //            gridTax.batchEditApi.StartEdit(i, 3);
        //            gridTax.GetEditor('calCulatedOn').SetValue(amt);

        //            var totLength = gridTax.GetEditor("TaxName").GetText().length;
        //            var taxNameWithSign = gridTax.GetEditor("Percentage").GetText();
        //            var sign = gridTax.GetEditor("TaxName").GetText().substring(totLength - 3);
        //            var ProdAmt = parseFloat(gridTax.GetEditor("calCulatedOn").GetValue());
        //            var s = gridTax.GetEditor("Percentage");
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //                gridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
        //                gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
        //                GlobalCurTaxAmt = 0;
        //            }




        //        }
        //    }
        //    //return;
        //    gridTax.batchEditApi.EndEdit();
        //}
        //function txtPercentageLostFocus(s, e) {
        //    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        //    if (s.GetText().trim() != '') {
        //        if (!isNaN(parseFloat(ProdAmt * s.GetText()) / 100)) {
        //            //Checking Add or less
        //            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
        //            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
        //                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
        //                //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
        //                GlobalCurTaxAmt = 0;
        //            }
        //            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));

        //            //Call for Running Total
        //            SetRunningTotal();

        //        } else {
        //            s.SetText("");
        //        }
        //    }
        //    RecalCulateTaxTotalAmountInline();
        //}
        //function RecalCulateTaxTotalAmountInline() {
        //    var totalInlineTaxAmount = 0;
        //    for (var i = 0; i < taxJson.length; i++) {
        //        cgridTax.batchEditApi.StartEdit(i, 3);
        //        var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //        var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //        if (sign == '(+)') {
        //            totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //        } else {
        //            totalInlineTaxAmount = totalInlineTaxAmount - parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //        }
        //        cgridTax.batchEditApi.EndEdit();
        //    }
        //    totalInlineTaxAmount = totalInlineTaxAmount + parseFloat(ctxtGstCstVat.GetValue());
        //    //ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
        //    ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);
        //}


        //function SetRunningTotal() {
        //    var runningTot = parseFloat(clblProdNetAmt.GetValue());
        //    for (var i = 0; i < taxJson.length; i++) {
        //        cgridTax.batchEditApi.StartEdit(i, 3);
        //        if (taxJson[i].applicableOn == "R") {
        //            cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
        //            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
        //            var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
        //            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
        //            var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
        //            var thisRunningAmt = 0;
        //            if (sign == '(+)') {
        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
        //                GlobalCurTaxAmt = 0;
        //            }
        //            else {

        //                GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //                cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

        //                ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
        //                GlobalCurTaxAmt = 0;
        //            }
        //            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
        //        }
        //        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //        cgridTax.batchEditApi.EndEdit();
        //    }
        //}

        //function GetTotalRunningAmount() {
        //    var runningTot = parseFloat(clblProdNetAmt.GetValue());
        //    for (var i = 0; i < taxJson.length; i++) {
        //        cgridTax.batchEditApi.StartEdit(i, 3);
        //        runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
        //        cgridTax.batchEditApi.EndEdit();
        //    }

        //    return runningTot;
        //}




        //function CmbtaxClick(s, e) {
        //    GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
        //    gstcstvatGlobalName = s.GetText();
        //}


        //function txtTax_TextChanged(s, i, e) {
        //    cgridTax.batchEditApi.StartEdit(i, 2);
        //    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
        //    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
        //}



        //function BatchUpdate() {
        //    var _SrlNo = document.getElementById('HdSerialNo').value;
        //    if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
        //        var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "Y" }
        //        TaxOfProduct.push(ProductTaxes)
        //    }
        //    else {
        //        $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "Y"; });
        //    }

        //    if (cgridTax.GetVisibleRowsOnPage() > 0) {
        //        cgridTax.UpdateEdit();
        //    }
        //    else {
        //        cgridTax.PerformCallback('SaveGST');
        //    }
        //    return false;
        //}


        //function cgridTax_EndCallBack(s, e) {
        //    //cgridTax.batchEditApi.StartEdit(0, 1);
        //    $('.cgridTaxClass').show();
        //    cgridTax.StartEditRow(0);
        //    //check Json data
        //    if (cgridTax.cpJsonData) {
        //        if (cgridTax.cpJsonData != "") {
        //            taxJson = JSON.parse(cgridTax.cpJsonData);
        //            cgridTax.cpJsonData = null;
        //        }
        //    }
        //    //End Here
        //    if (cgridTax.cpComboCode) {
        //        if (cgridTax.cpComboCode != "") {
        //            if (cddl_AmountAre.GetValue() == "1") {
        //                var selectedIndex;
        //                for (var i = 0; i < ccmbGstCstVat.GetItemCount() ; i++) {
        //                    if (ccmbGstCstVat.GetItem(i).value.split('~')[0] == cgridTax.cpComboCode) {
        //                        selectedIndex = i;
        //                    }
        //                }
        //                if (selectedIndex && ccmbGstCstVat.GetItem(selectedIndex) != null) {
        //                    ccmbGstCstVat.SetValue(ccmbGstCstVat.GetItem(selectedIndex).value);
        //                }
        //                cmbGstCstVatChange(ccmbGstCstVat);
        //                cgridTax.cpComboCode = null;
        //            }
        //        }
        //    }
        //    if (cgridTax.cpUpdated.split('~')[0] == 'ok') {
        //        ctxtTaxTotAmt.SetValue(Math.round(cgridTax.cpUpdated.split('~')[1]));
        //        var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]).toFixed(2);
        //        var ddValue = parseFloat(ctxtGstCstVat.GetValue()).toFixed(2);

        //        ctxtTaxTotAmt.SetValue(parseFloat(gridValue) + parseFloat(ddValue));
        //        cgridTax.cpUpdated = "";
        //    }
        //    else {
        //        var totAmt = ctxtTaxTotAmt.GetValue();
        //        cgridTax.CancelEdit();
        //        caspxTaxpopUp.Hide();
        //        grid.batchEditApi.StartEdit(globalRowIndex, 13);
        //        grid.GetEditor("gvColTaxAmount").SetValue(totAmt);
        //        if (cddl_AmountAre.GetValue() == "2") {
        //            var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue());
        //            var totalRoundOffAmount = Math.round(totalNetAmount);

        //            grid.GetEditor("gvColTotalAmountINR").SetValue(totalRoundOffAmount);
        //            grid.GetEditor("gvColAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("gvColAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
        //        }
        //        else {
        //            grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()), 2));
        //        }
        //    }
        //    if (cgridTax.GetVisibleRowsOnPage() == 0) {
        //        $('.cgridTaxClass').hide();
        //        ccmbGstCstVat.Focus();
        //    }
        //    //Debjyoti Check where any Gst Present or not
        //    // If Not then hide the hole section
        //    SetRunningTotal();
        //    ShowTaxPopUp("IY");
        //}

        //function recalculateTax() {
        //    cmbGstCstVatChange(ccmbGstCstVat);
        //}
        //function recalculateTaxCharge() {
        //    ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        //}




        /*............................End Tax...........................................*/


        //Rev Sayantani
        //function BindOrderProjectdata(OrderId,TagDocType)
        //{
        //   // debugger;
        //    var OtherDetail = {};

        //    OtherDetail.OrderId = OrderId;
        //    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
        //    if (checked=="Indent") {

        //        OtherDetail.TagDocType = "POIN";
        //    }
        //    else if(checked=="Quotation")
        //    {
        //        OtherDetail.TagDocType = "PurchaQuote";
        //    }        

        //    if ((OrderId != null) && (OrderId != "")) {

        //        $.ajax({
        //            type: "POST",
        //            url: "PurchaseOrder.aspx/SetProjectCode",
        //            data: JSON.stringify(OtherDetail),
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            success: function (msg) {
        //                var  Code = msg.d;

        //                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
        //                clookup_Project.SetEnabled(false);
        //            }
        //        });
        //    }
        //}
        // End of Rev Sayantani

        //function PerformCallToGridBind() {            
        //    var OrderTaggingData = cgridproducts.GetSelectedKeysOnPage();
        //    cPurchaseOrderPosGst.SetEnabled(false);
        //    if(OrderTaggingData==0){ 
        //        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        //        cProductsPopup.Hide();
        //    }
        //    else{
        //        grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
        //        // cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
        //        $('#hdnPageStatus').val('Quoteupdate');               
        //        var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();               
        //        if (quote_Id.length > 0) {
        //            var ComponentDetails = _ComponentDetails.split("~");
        //            cgridproducts.cpComponentDetails = null;
        //            var ComponentNumber = ComponentDetails[0];
        //            var ComponentDate = ComponentDetails[1];        
        //            ctaggingList.SetValue(ComponentNumber);
        //            cPLQADate.SetValue(ComponentDate);
        //            cPLQuoteDate.SetEnabled(false);
        //        }
        //        if (quote_Id.length > 0) {
        //            BindOrderProjectdata(quote_Id[0],$("#hdnTagDocType").val());
        //        }
        //        cProductsPopup.Hide();               
        //    }
        //}
        //function componentEndCallBack(s, e) {            
        //    if (cQuotationComponentPanel.cpNullGrid != null) {
        //        deleteAllRows();
        //        if (grid.GetVisibleRowsOnPage() == 0) {
        //            OnAddNewClick();
        //        }
        //        grid.GetEditor('ProductName').SetEnabled(true);
        //        cPLQADate.SetText('');
        //    }
        //    else {
        //        gridquotationLookup.gridView.Refresh();
        //        if (grid.GetVisibleRowsOnPage() == 0) {
        //            OnAddNewClick();
        //            grid.GetEditor('ProductName').SetEnabled(true);
        //            cPLQADate.SetText('');
        //        }
        //    }
        //}
        //function CloseGridQuotationLookup() {
        //    gridquotationLookup.ConfirmCurrentSelection();
        //    gridquotationLookup.HideDropDown();
        //    gridquotationLookup.Focus();
        //}
        //var SimilarProjectStatus = "0";

        //function SimilarProjetcheck(quote_Id, Doctype) {
        //    $.ajax({
        //        type: "POST",
        //        url: "PurchaseOrder.aspx/DocWiseSimilarProjectCheck",
        //        data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        async: false,
        //        success: function (msg) {
        //            SimilarProjectStatus = msg.d;
        //            debugger;
        //            if (SimilarProjectStatus != "1") {
        //                cPLQADate.SetText("");
        //                jAlert("Please select document with same project code to proceed.");

        //                return false;

        //            }
        //        }
        //    });
        //}
        //function QuotationNumberChanged() {

        //    document.getElementById('hdfTagMendatory').value = 'No';
        //    $("#MandatorysIndentReq").hide();            
        //    var OrderData = ctaggingGrid.GetSelectedKeysOnPage();


        //    var quotetag_Id = ctaggingGrid.GetSelectedKeysOnPage();

        //    if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {
        //        var Doctype = $("#rdl_Salesquotation").find(":checked").val();
        //        var quote_Id = "";
        //        // otherDets.quote_Id = quote_Id;
        //        for (var i = 0; i < quotetag_Id.length; i++) {
        //            if (quote_Id == "") {
        //                quote_Id = quotetag_Id[i];
        //            }
        //            else {
        //                quote_Id += ',' + quotetag_Id[i];
        //            }
        //        }

        //        SimilarProjetcheck(quote_Id, Doctype);
        //    }


        //    if (SimilarProjectStatus != "-1") {
        //        if (OrderData == 0) {
        //            cgridproducts.PerformCallback('BindProductsDetails');
        //            cpopup_taggingGrid.Hide();
        //            cProductsPopup.Show();
        //        }
        //        else {
        //            cgridproducts.PerformCallback('BindProductsDetails');
        //            cpopup_taggingGrid.Hide();
        //            cProductsPopup.Show();
        //        }
        //    }
        //}
        //function SetDifference1() {
        //    var diff = CheckDifferenceOfFromDateWithTodate();
        //}
        //function CheckDifferenceOfFromDateWithTodate() {
        //    var startDate = new Date();
        //    var endDate = new Date();
        //    var difference = -1;
        //    startDate = cPLSalesOrderDate.GetDate();
        //    if (startDate != null) {
        //        endDate = cExpiryDate.GetDate();
        //        var startTime = startDate.getTime();
        //        var endTime = endDate.getTime();
        //        difference = (startTime - endTime) / 86400000;

        //    }
        //    return difference;
        //}
        //function SetDifference() {
        //    var diff = CheckDifference();
        //}
        //function CheckDifference() {
        //    var startDate = new Date();
        //    var endDate = new Date();
        //    var difference = -1;
        //    startDate = cPLSalesOrderDate.GetDate();
        //    if (startDate != null) {
        //        endDate = cExpiryDate.GetDate();
        //        var startTime = startDate.getTime();
        //        var endTime = endDate.getTime();
        //        difference = (endTime - startTime) / 86400000;
        //    }
        //    return difference;

        //}
        //.................WareHouse.......        
        //function BtnVisible() {
        //    document.getElementById('btnSaveExit').style.display = 'none'
        //    document.getElementById('btn_SaveRecords').style.display = 'none'
        //    document.getElementById('tagged').style.display = 'block'
        //}

        function OnEndCallback(s, e) {
            if (grid.cpBtnVisible != null && grid.cpBtnVisible != "") {
                grid.cpBtnVisible = null;
                BtnVisible();
            }
            if (grid.cpComponent) {
                if (grid.cpComponent == 'true') {
                    grid.cpComponent = null;
                    OnAddNewClick();
                }
            }
            if (grid.cpBindNullGrid) {
                if (grid.cpBindNullGrid == 'Y') {
                    grid.cpBindNullGrid = null;
                    ctaggingList.SetValue("");
                    cPLQADate.SetValue("");
                    ctaggingList.SetEnabled(true);
                }
            }
            LoadingPanel.Hide();
            var value = document.getElementById('hdnRefreshType').value;
            var pageStatus = document.getElementById('hdnPageStatus').value;
            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Purchase Order Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
                //OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                OnAddNewClick();
                jAlert('Can Not Save as Duplicate Purchase Order No. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "BillingShippingNotLoaded") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                OnAddNewClick();
                jAlert('Billing Shipping is not yet loaded.Please wait.');
            }
            else if (grid.cpSaveSuccessOrFail == "ExceedQuantity") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                OnAddNewClick();
                jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');
            }

            else if (grid.cpSaveSuccessOrFail == "MINExceedQuantity") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                OnAddNewClick();
                jAlert('Tagged product quantity can not reduce.Update The quantity and Try Again.');
            }
            else if (grid.cpSaveSuccessOrFail == "checkAcurateTaxAmount") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Check GST Calculated for Item ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
                grid.cpSaveSuccessOrFail = '';
                grid.cpSerialNo = '';
                grid.cpProductName = '';
            }
            else if (grid.cpSaveSuccessOrFail == "UdfMandetory") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                OnAddNewClick();
                jAlert('UDF is set as Mandatory.Please enter values.', 'Alert', function () { OpenUdf(); });
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });

            }
            else if (grid.cpSaveSuccessOrFail == "TCMandatory") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });

            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert('Please try after sometime.');
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert('Please select project.');
            }
            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Please add Alt. Qty for SL No. " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "VendorAddressProblem") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert('You must enter the default Billing/Shipping Address for selected Vendor to proceed further.');
            }
            else if (grid.cpSaveSuccessOrFail == "VendorAddressSuccess") {
                GetPurchaseForGstValue();
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                var SrlNo = grid.cpProductSrlIDCheck;
                grid.cpSaveSuccessOrFail = null;
                grid.cpProductSrlIDCheck = null;
                OnAddNewClick();
                var msg = "Make sure product quantity are equal <br /> with warehouse quantity for SL No. " + SrlNo;
                jAlert(msg);
            }
            else if (grid.cpSaveSuccessOrFail == "nullWarehouse") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();

                jAlert('Cannot save. Stock details is mandatory.');
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                // Mantis Issue 24429
                grid.batchEditApi.StartEdit(0, 2);
                // End of Mantis Issue 24429
                grid.cpSaveSuccessOrFail = null;
                // Mantis Issue 24429
                //OnAddNewClick();
                // End of Mantis Issue 24429

                jAlert('Cannot save. Entered quantity must be greater then ZERO(0).');
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();

                jAlert('Can not Add Duplicate Product in the Purchase Order.');
            }
            else if (grid.cpSaveSuccessOrFail == "Ponotexist") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert('Cannot Save. Selected Purchase Indent(s) in this document do not exist.');
            }
                // Mantis Issue 25105
            else if (grid.cpOrderRunningBalance != null && grid.cpOrderRunningBalance != "") {
                var RunningBalance = grid.cpOrderRunningBalance;
                var RunningSpliteDetails = RunningBalance.split("~");
                grid.cpOrderRunningBalance = "";
                var SUM_Amount = RunningSpliteDetails[0];
                var SUM_TotalAmount = RunningSpliteDetails[1];
                var SUM_ProductQuantity = parseFloat(RunningSpliteDetails[2]).toFixed(2);

                cbnrLblTaxableAmtval.SetText(DecimalRoundoff(SUM_Amount, 2));
                cbnrLblTaxAmtval.SetText(DecimalRoundoff(0, 2));
                cbnrLblTotalQty.SetText(DecimalRoundoff(SUM_ProductQuantity, 4));
                //var totamt = totalAmount + totaltxAmount;
                cbnrlblAmountWithTaxValue.SetText(SUM_TotalAmount);
                cbnrLblInvValue.SetText(SUM_TotalAmount);
            }
                // End of Mantis Issue 25105
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
                    if (PurchaseOrder_Number != "" && PurchaseOrder_Number != null) {
                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {
                            if (r == true) {
                                grid.cpPurchaseOrderNo = null;
                                var newPorderId = grid.cpPurchaseOrderID;
                                //var reportName = "PO-Default~D";
                                //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Porder&id=' + newPorderId, '_blank')
                                grid.cpPurchaseOrderID = null;
                                window.location.assign("PurchaseOrderList.aspx");
                            }
                        });
                    }
                    else {
                        if (pageStatus != "delete") {
                            window.location.assign("PurchaseOrderList.aspx");
                        }
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
                    if (PurchaseOrder_Number != "" && PurchaseOrder_Number != null) {
                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("PurchaseOrder.aspx?key=ADD");
                            }
                        });
                    }
                    else {
                        if (pageStatus != "delete") {
                            window.location.assign("PurchaseOrder.aspx?key=ADD");
                        }

                    }
                }
                else {
                    if (pageStatus == "first") {
                        if (grid.GetVisibleRowsOnPage() == 0) {
                            // OnAddNewClick();
                        }
                        grid.batchEditApi.EndEdit();
                        FinYearCheckOnPageLoad();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        //GetIndentReqNoOnLoad();

                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxtRate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "Quoteupdate") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxtRate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "delete") {
                        OnAddNewClick();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                }
            }

    if (ctaggingList.GetValue() != null && ctaggingList.GetValue() != "") {
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

//function GridCallBack() {
//    //page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
//    grid.PerformCallback('Display');
//}


//function AutoPopulateMultiUOM() {

//    var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
//    var ProductID = Productdetails.split("||@||")[0];
//    var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";

//    $.ajax({
//        type: "POST",
//        url: "PurchaseOrder.aspx/AutoPopulateAltQuantity",
//        data: JSON.stringify({ ProductID: ProductID }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {

//            if (msg.d.length != 0) {
//                var packingQuantity = msg.d[0].packing_quantity;
//                var sProduct_quantity = msg.d[0].sProduct_quantity;
//                var AltUOMId = msg.d[0].AltUOMId;
//            }
//            else {
//                var packingQuantity = 0;
//                var sProduct_quantity = 0;
//                var AltUOMId = 0;
//            }
//            var uomfactor = 0
//            if (sProduct_quantity != 0 && packingQuantity != 0) {
//                uomfactor = parseFloat(packingQuantity / sProduct_quantity);
//                $('#hddnuomFactor').val(parseFloat(packingQuantity / sProduct_quantity));
//            }
//            else {
//                $('#hddnuomFactor').val(0);
//            }

//            var uomfac_Qty_to_stock = $('#hddnuomFactor').val();
//            var Qty = QuantityValue;
//            var calcQuantity = parseFloat(Qty * uomfac_Qty_to_stock);
//            if ($("#hdnPageStatusForMultiUOM").val() == "Quoteupdate") {               
//                ccmbSecondUOM.SetValue('');              
//                cAltUOMQuantity.SetValue("0.0000");
//            }
//            else {               
//                if (AltUOMId == 0) {
//                    ccmbSecondUOM.SetValue('');
//                }
//                else {
//                    ccmbSecondUOM.SetValue(AltUOMId);
//                }
//                cAltUOMQuantity.SetValue(calcQuantity);
//            }

//        }
//    });
//}



        <%--function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();      
        
        var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColTotalAmountINR');
        var oldAmountWithTaxValue= parseFloat(cbnrlblAmountWithTaxValue.GetValue());
        var AfterdeleteoldAmountWithTaxValue=oldAmountWithTaxValue-parseFloat(totalNetAmount);
        cbnrlblAmountWithTaxValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
        //cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));

        var RowQuantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'gvColQuantity');
        var totalquantity = parseFloat(cbnrLblTotalQty.GetValue());
        var updatedquantity=(parseFloat(totalquantity)-parseFloat(RowQuantity));
        //cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));

        var rowTaxAmount = grid.batchEditApi.GetCellValue(e.visibleIndex,'gvColTaxAmount');
        var totaltaxamt=parseFloat(cbnrLblTaxAmtval.GetValue());
        var updatedtaxamt=parseFloat(totaltaxamt)-parseFloat(rowTaxAmount);
        //cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));

        var rowAmount = grid.batchEditApi.GetCellValue(e.visibleIndex,'gvColAmount');
        var TotalAmt=parseFloat(cbnrLblTaxableAmtval.GetValue());
        var updatedAmt=parseFloat(TotalAmt)-parseFloat(rowAmount);
        //cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

        if (ctaggingList.GetValue() != null) {         
            jAlert('Cannot Delete using this button as the Purchase Indent is linked with this Purchase Order.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }
        var noofvisiblerows = grid.GetVisibleRowsOnPage();       
        if (noofvisiblerows != "1" && ctaggingList.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);

            cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
            cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
            cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
            cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));

            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display');
            $('#<%=hdnPageStatus.ClientID %>').val('delete');           
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {

        if (ctaggingList.GetValue() == null) {
            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
            var Product = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";
            var SpliteDetails = Product.split("||@||");
            var IsComponentProduct = SpliteDetails[16];
            var ComponentProduct = SpliteDetails[17];
            if (IsComponentProduct == "Y") {
                var messege = "Selected product is defined with components.<br/> Would you like to proceed with components (" + ComponentProduct + ") ?";
                jConfirm(messege, 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.batchEditApi.StartEdit(globalRowIndex);
                        var IsComponentProduct = grid.GetEditor("IsComponentProduct");
                        IsComponentProduct.SetValue("Y");
                        $('#<%=hdfIsDelete.ClientID %>').val('C');
                        grid.UpdateEdit();
                        grid.PerformCallback('Display~fromComponent');
                        
                    }
                    else {
                        OnAddNewClick();                       
                    }
                });
            }
            else if (Product != "") {
                OnAddNewClick();
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 500);
                return false;
            }
            else {                
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 3);
                }, 50);
                return false;
               
            }
        }
        else {
            QuotationNumberChanged();
        }      
    }


    else if (e.buttonID == 'CustomMultiUOM') {
            
        var index = e.visibleIndex;
        grid.batchEditApi.StartEdit(e.visibleIndex);
        var Productdetails = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
        var ProductID = Productdetails.split("||@||")[0];
        var UOMName = grid.GetEditor("gvColUOM").GetValue();
        var quantity = grid.GetEditor("gvColQuantity").GetValue();
        var DetailsId = grid.GetEditor('DetailsId').GetText();
        var StockUOM = Productdetails.split("||@||")[5];
        if (StockUOM == "") {
            StockUOM="0";
        }
        //$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
        cAltUOMQuantity.SetValue("0.0000");
        if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {

            if (StockUOM == "0") {
                jAlert("Main Unit Not Defined.");
            }

            else
            {
                if ($("#hddnMultiUOMSelection").val() == "1") {
                    ccmbUOM.SetEnabled(false);
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex);
                    //grid.batchEditApi.StartEdit(globalRowIndex);
                    var Qnty = grid.GetEditor("gvColQuantity").GetValue();
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var UomId = grid.GetEditor('gvColProduct').GetText().split("||@||")[3];
                    ccmbUOM.SetValue(UomId);
                    $("#UOMQuantity").val(Qnty);
                    cPopup_MultiUOM.Show();
                    cgrid_MultiUOM.cpDuplicateAltUOM = "";
                    AutoPopulateMultiUOM();
                    //chinmoy change start
                    cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);
                    //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + ProductID);
                }     //End
            }
        }
        else {
            return;
        }
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
                            url: 'PurchaseOrder.aspx/getProductType',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{Products_ID:\"" + strProductID + "\"}",
                            success: function (type) {
                                Ptype = type.d;
                                $('#<%=hdfProductType.ClientID %>').val(Ptype);
                        ctxtqnty.SetText("0.0");
                        ctxtqnty.SetEnabled(true);
                        if (Ptype == "") {
                            jAlert("No Warehouse or Batch or Serial is actived !.");
                        } else {
                            if (Ptype == "W") {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");                               
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
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);
                            }
                            else if (Ptype == "WB") {
                                $('#<%=hdnisserial.ClientID %>').val("false");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");                               
                            }
                            else if (Ptype == "WS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("false");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);                                
                            }
                            else if (Ptype == "WBS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("true");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);
                             
                            }
                            else if (Ptype == "BS") {
                                $('#<%=hdnisserial.ClientID %>').val("true");
                                $('#<%=hdnisbatch.ClientID %>').val("true");
                                $('#<%=hdniswarehouse.ClientID %>').val("false");
                                ctxtqnty.SetText(QuantityValue);
                                ctxtqnty.SetEnabled(false);
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

                            //ctxtqnty.SetText("0.0");
                            //ctxtqnty.SetEnabled(true);

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
                            //ctxtqnty.SetValue("0.0");
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
}--%>


        //function SetArrForUOM(){
        //    if (aarr.length == 0) {
        //        for(var i = -500; i < 500;i++)
        //        {
        //            if(grid.GetRow(i) != null){

        //                var ProductID = (grid.batchEditApi.GetCellValue(i,'gvColProduct') != null) ? grid.batchEditApi.GetCellValue(i,'gvColProduct') : "0";
        //                if(ProductID!="0"){
        //                    var Indent_Num= (grid.GetEditor('Indent_Num').GetText() != null) ? grid.GetEditor('Indent_Num').GetText() : "";
        //                    var actionQry = '';
        //                    //if($("#hdAddOrEdit").val() == "Edit"){

        //                    if (Indent_Num != "0" && Indent_Num != "") {
        //                        actionQry = 'PurchaseOrderIndent';
        //                    }
        //                    else{
        //                        actionQry = 'PurchaseOrderByProductID';
        //                    }


        //                    var SpliteDetails = ProductID.split("||@||");
        //                    var strProductID = SpliteDetails[0];

        //                    var orderid = grid.GetRowKey(i);
        //                    if(orderid!="" && orderid!=null)
        //                    {
        //                        orderid = (orderid.split("Q~"));
        //                    }
        //                    else
        //                    {
        //                        orderid=0;
        //                    }
        //                    var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
        //                    var Quantity = grid.batchEditApi.GetCellValue(i,'gvColQuantity');
        //                    if($("#hddnMultiUOMSelection").val()=="0")
        //                    {
        //                        $.ajax({
        //                            type: "POST",
        //                            url: "Services/Master.asmx/GetMultiUOMDetails",
        //                            data: JSON.stringify({orderid: orderid,action:actionQry,module:'PurchaseOrder',strKey :Indent_Num}),
        //                            contentType: "application/json; charset=utf-8",
        //                            dataType: "json",
        //                            async: false,
        //                            success: function (msg) {

        //                                gridPackingQty = msg.d;

        //                                if(msg.d != ""){
        //                                    var packing = SpliteDetails[19];
        //                                    var PackingUom = SpliteDetails[23];
        //                                    var PackingSelectUom = SpliteDetails[24];
        //                                    var arrobj = {};
        //                                    arrobj.productid = strProductID;
        //                                    arrobj.slno = slnoget;
        //                                    arrobj.Quantity = Quantity;
        //                                    arrobj.packing = gridPackingQty;
        //                                    arrobj.PackingUom = PackingUom;
        //                                    arrobj.PackingSelectUom = PackingSelectUom;

        //                                    aarr.push(arrobj);
        //                                    //alert();
        //                                }
        //                            }
        //                        });
        //                    }
        //                }
        //            }
        //        }

        //    }
        //}

        <%--function Save_ButtonClick() {
    LoadingPanel.Show();
    flag = true;

    var revdate=ctxtRevisionDate.GetText();	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate")	
    {	
        if(ctxtRevisionNo.GetText()=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionNo.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate")	
    {	
        if(revdate=="01-01-0100"||revdate==null||revdate=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionDate.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate")	
    {	
        var detRev={};	
        detRev.RevNo=ctxtRevisionNo.GetText();	
        detRev.Order=$("#hdnEditOrderId").val();	
           	
        $.ajax({	
            type: "POST",	
            url: "PurchaseOrder.aspx/Duplicaterevnumbercheck",	
            data: JSON.stringify(detRev),	
            contentType: "application/json; charset=utf-8",	
            dataType: "json",	
            async:false,	
            success: function (msg) {	
                var duplicateRevCheck=msg.d;	
                if (duplicateRevCheck==1)	
                {	
                    flag = false;	
                    LoadingPanel.Hide();	
                    jAlert("Please Enter a valid Revision No");	
                    //alert("Please Enter a valid Revision No");	
                    //  LoadingPanel.Hide();	
                    //$("#txtRevisionNo").val("");	
                    ctxtRevisionNo.SetFocus();	
                }	
            }	
        });	
    }


    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();
    var Podt = cPLQuoteDate.GetValue();

    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        //flag = false;
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();
    if ($('#ddlInventory').val() == 'Y') {
        if (TagMendatory == 'Yes') {
            LoadingPanel.Hide();
            $("#MandatorysIndentReq").show();

            return false;
        }
    }
    var PoDuedt = cdt_PODue.GetValue();
    if (PoDuedt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDueDate").show();
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
        SetArrForUOM(); //Surojit For UOM EDIT

        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var IsInventory=$("#ddlInventory").val();
                console.log("1"+IsInventory);
                if(IsInventory!='N')
                {            
                    if (issavePacking == 1) {
                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                                    grid.batchEditApi.EndEdit();
                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                                    OnAddNewClick();

                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else {

                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            $('#<%=hdnRefreshType.ClientID %>').val('N');
                            grid.batchEditApi.EndEdit();
                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                            OnAddNewClick();

                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }
                    }
                    else {

                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    $('#<%=hdnRefreshType.ClientID %>').val('N');
                                    grid.batchEditApi.EndEdit();
                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                                    OnAddNewClick();

                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else{

                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            $('#<%=hdnRefreshType.ClientID %>').val('N');
                            grid.batchEditApi.EndEdit();
                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                            OnAddNewClick();

                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }
                    }
                }
                else
                {
                    var VendorId = $('#hdnCustomerId').val();
                    $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                    grid.batchEditApi.EndEdit();
                    OnAddNewClick();
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    grid.UpdateEdit();
                }
            
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }

        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }            
}--%>
        <%--function SaveExit_ButtonClick() {
    LoadingPanel.Show();
    flag = true;

    var revdate=ctxtRevisionDate.GetText();	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate")	
    {	
        if(ctxtRevisionNo.GetText()=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionNo.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate")	
    {	
        if(revdate=="01-01-0100"||revdate==null||revdate=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Revision Details.");	
            ctxtRevisionDate.SetFocus();	
            return false;	
        }	
    }	
    if($("#hdnApproveStatus").val()==1 && $("#hdnEditPageStatus").val() == "Quoteupdate")	
    {	
        var detRev={};	
        detRev.RevNo=ctxtRevisionNo.GetText();	
        detRev.Order=$("#hdnEditOrderId").val();	
           	
        $.ajax({	
            type: "POST",	
            url: "PurchaseOrder.aspx/Duplicaterevnumbercheck",	
            data: JSON.stringify(detRev),	
            contentType: "application/json; charset=utf-8",	
            dataType: "json",	
            async:false,	
            success: function (msg) {	
                var duplicateRevCheck=msg.d;	
                if (duplicateRevCheck==1)	
                {	
                    flag = false;	
                    LoadingPanel.Hide();	
                    jAlert("Please Enter a valid Revision No");	
                    //alert("Please Enter a valid Revision No");	
                    //  LoadingPanel.Hide();	
                    //$("#txtRevisionNo").val("");	
                    ctxtRevisionNo.SetFocus();	
                }	
            }	
        });	
    }

    var txtPurchaseNo = $("#txtVoucherNo").val();
    var ddl_Vendor = $("#ddl_Vendor").val();          
    if (txtPurchaseNo == null || txtPurchaseNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        return false;
    }
    var Podt = cPLQuoteDate.GetValue();
    if (Podt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDate").show();
        return false;
    }
    var customerId = GetObjectID('hdnCustomerId').value
    if (customerId == '' || customerId == null) {
        LoadingPanel.Hide();
        $('#MandatorysVendor').attr('style', 'display:block');
        return false;
    }
    else {
        $('#MandatorysVendor').attr('style', 'display:none');
    }
    var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();
    if ($('#ddlInventory').val() == 'Y') {
        if (TagMendatory == 'Yes') {
            LoadingPanel.Hide();
            $("#MandatorysIndentReq").show();
            return false;
        }
    }
    var PoDuedt = cdt_PODue.GetValue();
    if (PoDuedt == null) {
        LoadingPanel.Hide();
        $("#MandatoryDueDate").show();
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

        SetArrForUOM(); //Surojit For UOM EDIT

        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
                var IsInventory=$("#ddlInventory").val();
                console.log("2"+IsInventory);
                if(IsInventory!='N')
                {   
                    if (issavePacking == 1) {
                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                                    grid.batchEditApi.EndEdit();
                                    OnAddNewClick();
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else {

                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                            grid.batchEditApi.EndEdit();
                            OnAddNewClick();
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }

                    }
                    else {


                        if (aarr.length > 0) {
                            $.ajax({
                                type: "POST",
                                url: "PurchaseOrder.aspx/SetSessionPacking",
                                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    var VendorId = $('#hdnCustomerId').val();
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                                    grid.batchEditApi.EndEdit();
                                    OnAddNewClick();
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    grid.UpdateEdit();
                                }
                            });
                        }
                        else{

                            // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            var VendorId = $('#hdnCustomerId').val();
                            $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                            grid.batchEditApi.EndEdit();
                            OnAddNewClick();
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            grid.UpdateEdit();
                        }
                    }
                }
                else
                {
                    var VendorId = $('#hdnCustomerId').val();
                    $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);
                     $('#<%=hdnRefreshType.ClientID %>').val('E');
                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
                    grid.batchEditApi.EndEdit();
                    OnAddNewClick();
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    grid.UpdateEdit();
                }
            }
            else {
                LoadingPanel.Hide();
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }

        }
        else {
            LoadingPanel.Hide();
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        }
    }
}--%>

        //Approve and Reject Button Action Tanmoy	
        //function Reject_ButtonClick()	
        //{	
        //    if($("#hdnProjectApproval").val()=="ProjApprove")	
        //    {	
        //        if($("#txtAppRejRemarks").val()=="")	
        //        {	
        //            jAlert("Please Enter Reject Remarks.")	
        //            $("#txtAppRejRemarks").focus();	
        //            return false;	
        //        }	
        //    }	
        //    var otherdet={};	
        //    otherdet.ApproveRemarks=$("#txtAppRejRemarks").val();	
        //    otherdet.ApproveRejStatus=2;	
        //    otherdet.OrderId= $("#hdnEditOrderId").val();	
        //    $.ajax({	
        //        type: "POST",	
        //        url: "PurchaseOrder.aspx/SetApproveReject",	
        //        data: JSON.stringify(otherdet),	
        //        contentType: "application/json; charset=utf-8",	
        //        dataType: "json",	
        //        success: function (msg) {	
        //            var value=msg.d;	
        //            if (value=="1")	
        //            {	
        //                jAlert("Order Rejected.");	
        //                window.location.href="PurchaseOrderList.aspx";	
        //            }	
        //        }	
        //    });	
        //}	
<%--function Approve_ButtonClick()	
{	
    LoadingPanel.Show();	
    flag = true;	
    if($("#hdnProjectApproval").val()=="ProjApprove")	
    {	
        if($("#txtAppRejRemarks").val()=="")	
        {	
            flag = false;	
            LoadingPanel.Hide();	
            jAlert("Please Enter Approval Remarks.")	
            $("#txtAppRejRemarks").focus();	
            return false;	
        }	
    }	
    $("#hdnApproveStatus").val(1);	
    var txtPurchaseNo = $("#txtVoucherNo").val();	
    var ddl_Vendor = $("#ddl_Vendor").val();          	
    if (txtPurchaseNo == null || txtPurchaseNo == "") {	
        flag = false;	
        LoadingPanel.Hide();	
        $("#MandatoryBillNo").show();	
        return false;	
    }	
    var Podt = cPLQuoteDate.GetValue();	
    if (Podt == null) {	
        LoadingPanel.Hide();	
        $("#MandatoryDate").show();	
        return false;	
    }	
    var customerId = GetObjectID('hdnCustomerId').value	
    if (customerId == '' || customerId == null) {	
        LoadingPanel.Hide();	
        $('#MandatorysVendor').attr('style', 'display:block');	
        return false;	
    }	
    else {	
        $('#MandatorysVendor').attr('style', 'display:none');	
    }	
    var TagMendatory = document.getElementById('hdfTagMendatory').value;// $('#hdfTagMendatory').val();	
    if ($('#ddlInventory').val() == 'Y') {	
        if (TagMendatory == 'Yes') {	
            LoadingPanel.Hide();	
            $("#MandatorysIndentReq").show();	
            return false;	
        }	
    }	
    var PoDuedt = cdt_PODue.GetValue();	
    if (PoDuedt == null) {	
        LoadingPanel.Hide();	
        $("#MandatoryDueDate").show();	
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
        SetArrForUOM(); //Surojit For UOM EDIT	
        if (grid.GetVisibleRowsOnPage() > 0) {	
            if (IsType == "Y") {	
                if (issavePacking == 1) {	
                    if (aarr.length > 0) {	
                        $.ajax({	
                            type: "POST",	
                            url: "PurchaseOrder.aspx/SetSessionPacking",	
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",	
                            contentType: "application/json; charset=utf-8",	
                            dataType: "json",	
                            success: function (msg) {	
                                // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                                var VendorId = $('#hdnCustomerId').val();	
                                $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);	
                                        $('#<%=hdnRefreshType.ClientID %>').val('E');	
                                        $('#<%=hdfIsDelete.ClientID %>').val('I');	
                                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());	
                                        grid.batchEditApi.EndEdit();	
                                        OnAddNewClick();	
                                        var JsonProductList = JSON.stringify(TaxOfProduct);	
                                        GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                                        grid.UpdateEdit();	
                                    }	
                                });	
                            }	
                            else {	
                                // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                                var VendorId = $('#hdnCustomerId').val();	
                                $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);	
                        $('#<%=hdnRefreshType.ClientID %>').val('E');	
                                $('#<%=hdfIsDelete.ClientID %>').val('I');	
                                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());	
                                grid.batchEditApi.EndEdit();	
                                OnAddNewClick();	
                                var JsonProductList = JSON.stringify(TaxOfProduct);	
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                                grid.UpdateEdit();	
                            }	
                        }	
                        else {	
                            if (aarr.length > 0) {	
                                $.ajax({	
                                    type: "POST",	
                                    url: "PurchaseOrder.aspx/SetSessionPacking",	
                                    data: "{'list':'" + JSON.stringify(aarr) + "'}",	
                                    contentType: "application/json; charset=utf-8",	
                                    dataType: "json",	
                                    success: function (msg) {	
                                        // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                                        var VendorId = $('#hdnCustomerId').val();	
                                        $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);	
                                $('#<%=hdnRefreshType.ClientID %>').val('E');	
                                $('#<%=hdfIsDelete.ClientID %>').val('I');	
                                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());	
                                grid.batchEditApi.EndEdit();	
                                OnAddNewClick();	
                                var JsonProductList = JSON.stringify(TaxOfProduct);	
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                                grid.UpdateEdit();	
                            }	
                        });	
                    }	
                    else{	
                        // var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";	
                        var VendorId = $('#hdnCustomerId').val();	
                        $('#<%=hdfLookupCustomer.ClientID %>').val(VendorId);	
                                $('#<%=hdnRefreshType.ClientID %>').val('E');	
                        $('#<%=hdfIsDelete.ClientID %>').val('I');	
                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());	
                        grid.batchEditApi.EndEdit();	
                        OnAddNewClick();	
                        var JsonProductList = JSON.stringify(TaxOfProduct);	
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;	
                        grid.UpdateEdit();	
                    }	
                }	
            }	
            else {	
                LoadingPanel.Hide();	
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');	
            }	
        }	
        else {	
            LoadingPanel.Hide();	
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');	
        }	
    }	
}	--%>
        //Approve and Reject Button Action Tanmoy
        //function OnAddNewClick() {   
        //    grid.AddNewRow();
        //    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        //    var i;
        //    var cnt = 1;
        //    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        //        var tbQuotation = grid.GetEditor("SrlNo");
        //        tbQuotation.SetValue(cnt);


        //        cnt++;
        //    }

        //}
        //function ProductsCombo_SelectedIndexChanged(s, e) {

        //    var tbDescription = grid.GetEditor("gvColDiscription");
        //    var tbUOM = grid.GetEditor("gvColUOM");
        //    var tbStockUOM = grid.GetEditor("gvColStockUOM");
        //    var tbPurchasePrice = grid.GetEditor("gvColStockPurchasePrice");

        //    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
        //    // var ProductID = s.GetValue();
        //    var SpliteDetails = ProductID.split("||@||");
        //    var strProductID = SpliteDetails[0];
        //    var strDescription = SpliteDetails[1];
        //    var strUOM = SpliteDetails[2];
        //    var strStockUOM = SpliteDetails[4];
        //    var strPurchasePrice = SpliteDetails[6];
        //    var strStockId = SpliteDetails[10];
        //    tbDescription.SetValue(strDescription);
        //    tbUOM.SetValue(strUOM);
        //    tbStockUOM.SetValue(strStockUOM);
        //    tbPurchasePrice.SetValue(strPurchasePrice);
        //    if (ProductID != "0") {
        //        cacpAvailableStock.PerformCallback(strProductID);
        //    }

        //}
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
//function ddl_AmountAre_valueChange() {
//    var key = $("#ddl_AmountAre").val();
//    if (key == 1) {
//        // grid.GetEditor('TaxAmount').SetEnabled(true);
//        cddlVatGstCst.SetEnabled(false);
//        cddlVatGstCst.PerformCallback('1');
//    }
//    else if (key == 2) {
//        // grid.GetEditor('TaxAmount').SetEnabled(true);
//        cddlVatGstCst.SetEnabled(true);
//        cddlVatGstCst.PerformCallback('2');

//    }
//    else if (key == 3) {
//        //  grid.GetEditor('TaxAmount').SetEnabled(false);
//        cddlVatGstCst.SetEnabled(false);
//        cddlVatGstCst.PerformCallback('3');

//    }
//}

//function GetIndentREquiNo(e) {

//    var PODate = new Date();
//    PODate = cPLQuoteDate.GetValueString();
//    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

//    grid.batchEditApi.StartEdit(-1, 1);
//    var accountingDataMin = grid.GetEditor('ProductName').GetValue();
//    grid.batchEditApi.EndEdit();

//    grid.batchEditApi.StartEdit(0, 1);
//    var accountingDataplus = grid.GetEditor('ProductName').GetValue();

//    grid.batchEditApi.EndEdit();

//    if (accountingDataMin != null || accountingDataplus != null) {
//        jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

//            if (r == true) {
//                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
//                grid.PerformCallback('GridBlank');
//            }
//        });
//        //onBranchItems();
//    }
//}

//function GetIndentReqNoOnLoad() {

//    var PODate = new Date();
//    PODate = cPLQuoteDate.GetValueString();
//    cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

//}
//function GetContactPersonPhone(e) {
//    var key = cContactPerson.GetValue();
//    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
//}

//function ShowIndntRequisition() {

//}
        <%--function cmbContactPersonEndCall(s, e) {         
}
function acpContactPersonPhoneEndCall(s, e) {
    if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
        pageheaderContent.style.display = "block";
        $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
        document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
        cacpContactPersonPhone.cpPhone = null;

    }
}--%>
        <%--$(document).ready(function () {  
    var key = GetObjectID('hdnCustomerId').value;
    if (key != null && key != "") {
        if ($("#btn_TermsCondition").is(":visible")) {
            callTCspecefiFields_PO(key);
        }
    }
    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
    if (checked=="Indent" || checked=="Quotation") {
        ctaggingList.SetEnabled(true);
    }
    else
    {
        ctaggingList.SetEnabled(false);
    }

    if ($('#hdnPageStatus').val() == "Quoteupdate")
    {
        cPurchaseOrderPosGst.SetEnabled(false);
        PopulatePurchasePosGst();

        if($("#hdnApproveStatus").val()==1)	
        {	
            document.getElementById("dvRevisionDate").style.display="block";	
            document.getElementById("dvRevision").style.display="block";	
        }
    }

    if(($("#hdnEditPageStatus").val()=="Quoteupdate")&&($("#hdnApproveStatus").val()==1)&&($("#hdnProjectApproval").val()=="ProjApprove"))	
    {	
        document.getElementById("dvRevisionDate").style.display="block";	
        document.getElementById("dvRevision").style.display="block";	
        document.getElementById("dvAppRejRemarks").style.display="block";	
        document.getElementById("dvReject").style.display="none";	
        document.getElementById("dvApprove").style.display="none";	
    }	
    if(($("#hdnEditPageStatus").val()=="Quoteupdate")&&($("#hdnApproveStatus").val()==2)&&($("#hdnProjectApproval").val()=="ProjApprove"))	
    {	
        document.getElementById("dvAppRejRemarks").style.display="block";	
        document.getElementById("dvReject").style.display="none";	
        document.getElementById("dvApprove").style.display="inline-block";	
    }	
    if(($("#hdnEditPageStatus").val()=="Quoteupdate")&&($("#hdnApproveStatus").val()==0)&&($("#hdnProjectApproval").val()=="ProjApprove"))	
    {	
        document.getElementById("dvAppRejRemarks").style.display="block";	
        document.getElementById("dvReject").style.display="inline-block";	
        document.getElementById("dvApprove").style.display="inline-block";	
    }

    LoadCustomerBillingShippingAddress(GetObjectID('hdnCustomerId').value);
    LoadBranchAddressInEditMode($('#ddl_Branch').val());

   
    $('#ProductModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })
    
    var schemaid = $('#ddl_numberingScheme').val();
    if (schemaid != null) {
        if (schemaid == '0') {
            document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
        }
    }
   
    $('#ApprovalCross').click(function () {
        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.Refresh();
    })
   

    if($('#Keyval_internalId').val()=="Add" && $('#ddl_numberingScheme').val() != "0"){
        CmbScheme_ValueChange();
    }


});--%>
        <%--function CmbScheme_ValueChange() {
            var val = $("#ddl_numberingScheme").val();
            ctxtVendorName.SetText("");
            GetObjectID('hdnCustomerId').value = "";
            page.tabs[1].SetEnabled(false);
            var schemetypeValue = val;
            var schemetype = schemetypeValue.toString().split('~')[1];
            var schemelength = schemetypeValue.toString().split('~')[2];
            var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
            //Rev Debashis
            var fromdate = (schemetypeValue.toString().split('~')[4] != null) ? schemetypeValue.toString().split('~')[4] : "";
            var todate = (schemetypeValue.toString().split('~')[5] != null) ? schemetypeValue.toString().split('~')[5] : "";
            var dt = new Date();
            cPLQuoteDate.SetDate(dt);
            if (dt < new Date(fromdate)) {
                cPLQuoteDate.SetDate(new Date(fromdate));
            }
            if (dt > new Date(todate)) {
                cPLQuoteDate.SetDate(new Date(todate));
            }
            cPLQuoteDate.SetMinDate(new Date(fromdate));
            cPLQuoteDate.SetMaxDate(new Date(todate));
            //End of Rev Debashis
            $("#hdnTCBranchId").val(branchID);
            if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
            document.getElementById('<%= ddl_Branch.ClientID %>').disabled = true;
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
            else {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            }
            //Chinmoy added this line
            PosGstId = "";
            cPurchaseOrderPosGst.SetValue(PosGstId);
            SetPurchaseBillingShippingAddress( $('#ddl_Branch').val());
            

            //if ($("#hdnProjectSelectInEntryModule").val() == "1")
            //    clookup_Project.gridView.Refresh();
        }--%>
        //function IndentRequisitionNo_ValueChange() {

        //    var val = $("#ddl_IndentRequisitionNo").val();
        //    if (val != 0) {
        //        $.ajax({
        //            type: "POST",
        //            url: 'PurchaseOrder.aspx/getIndentRequisitionDate',
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            data: "{IndentRequisitionNo:\"" + val + "\"}",
        //            success: function (type) {

        //                var Transdt = new Date(type.d);
        //                cIndentRequisDate.SetDate(Transdt);

        //            }
        //        });
        //    }
        //    else {
        //        cIndentRequisDate.SetVal("");
        //    }

        //}



        //function SetDifference() {
        //    var diff = CheckDifference();
        //    if (diff > 0) {
        //        clientResult.SetText(diff.toString());
        //    }

        //}

        //function CheckDifference() {
        //    var startDate = new Date();
        //    var endDate = new Date();
        //    var difference = -1;
        //    startDate = cPLQuoteDate.GetDate();
        //    if (startDate != null) {
        //        endDate = cExpiryDate.GetDate();
        //        var startTime = startDate.getTime();
        //        var endTime = endDate.getTime();
        //        difference = (endTime - startTime) / 86400000;

        //    }
        //    return difference;

        //}


    </script>
    <%--Mantis Issue 25152--%>
    <script type="text/javascript">
        $(document).ready(function () {
            //// Mantis Issue 25235
           
            //if ($('#hdnSettings').val() == "No" && $('#hdnADDEditMode').val() == "Edit") {
            //    $("#divIsDirector").addClass('hide');
            //    //$("#onSmsClickJv").removeClass('hide');
            //}
            //else if ($('#hdnSettings').val() == "Yes" && $('#hdnADDEditMode').val() == "Edit") {
            //    $("#divIsDirector").addClass('hide');
            //    //$("#onSmsClickJv").removeClass('hide');
            //}
            //else {
                
            //    $("#divIsDirector").removeClass('hide');
            //    BindModalEmployee();
            //    //$("#onSmsClickJv").addClass('hide');
            //}
            //    // End of Mantis Issue 25235
        })
    </script>
    <%--End of Mantis Issue 25152--%>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

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
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dt_OADate , #dt_PODue
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dt_OADate_B-1 , #dt_PODue_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dt_OADate_B-1 #dt_OADate_B-1Img ,
        #dt_PODue_B-1 #dt_PODue_B-1Img
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
            top: 26px;
            right: 13px;
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        
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

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }*/

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
                margin-top: 3px;
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

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        #txtProdSearch
        {
            margin-bottom: 10px;
        }

        select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        #massrecdt
        {
            width: 100%;
        }

        .col-md-2 , .col-md-4{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        .typeNotification span
        {
             color: #ffffff !important;
        }

        #rdl_Salesquotation
        {
            margin-top: 8px;
            line-height: 20px;
        }

        #ASPxLabel8
        {
            line-height: 16px;
        }

        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 0 !important;
        }
        .mCustomScrollBox
        {
            overflow: inherit;
        }

        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <script src="JS/SearchPopup.js"></script>--%>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Add Purchase Order"></asp:Label>

                </span>
            </h3>
            <div id="pageheaderContent" class="scrollHorizontal pull-right reverse wrapHolder content horizontal-images" style="display: none; width: 836px" runat="server">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
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

                            <div class="lblHolder" id="divOutstanding" style="display: none;" runat="server">
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
                                        <td>Selected Unit</td>
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

            <%-- region Sandip Section For Approval Section Start--%>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>

            <div id="divcross" runat="server" class="crossBtn">
                <a href="PurchaseOrderList.aspx"><i class="fa fa-times"></i></a>
            </div>

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
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%" onchange="ddlInventory_OnChange()">
                                            <asp:ListItem Text="Both" Value="B" />
                                            <asp:ListItem Text="Inventory Item" Value="Y" />
                                            <asp:ListItem Text="Non-Inventory Item" Value="N" />
                                            <asp:ListItem Text="Capital Goods" Value="C" />
                                            <asp:ListItem Text="Service Item" Value="S" />
                                        </asp:DropDownList>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 simple-select" runat="server" id="divNumberingScheme">
                                        <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="160px" runat="server" Text="Numbering Scheme">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%"
                                            DataTextField="SchemaName" DataValueField="Id" onchange="CmbScheme_ValueChange()">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                                        </asp:TextBox>
                                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                            ClientInstanceName="cPLQuoteDate" Width="100%" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents DateChanged="function(s, e) { TDateChange(e)}" GotFocus="function(s,e){cPLQuoteDate.ShowDropDown();}" />
                                        </dxe:ASPxDateEdit>
                                        <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        <%--Rev 1.0--%>
                                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                        <%--Rev end 1.0--%>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-4 simple-select">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%"
                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" onchange="onBranchItems()">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="col-md-2" id="DivForUnit" runat="server">
                                        <label>For Unit</label>
                                        <div>
                                            <asp:DropDownList ID="ddlForBranch" runat="server" 
                                                DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <a href="#" onclick="AddVendorClick()" style="left: -12px; top: 20px; font-size: 16px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                        <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>

                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                        <span id="MandatorysVendor" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8" id="indentRequisition" runat="server">
                                        <asp:RadioButtonList ID="rdl_Salesquotation" runat="server" RepeatDirection="Horizontal" onchange="return selectValueForRadioBtn();" Width="85%">
                                            <asp:ListItem Text="Indent" Value="Indent"></asp:ListItem>
                                            <asp:ListItem Text="Quotation" Value="Quotation"></asp:ListItem>
                                        </asp:RadioButtonList>

                                        <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server" ReadOnly="true" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                                        </dxe:ASPxButtonEdit>
                                        <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
                                            HeaderText="Select Documents" PopupHorizontalAlign="WindowCenter"
                                            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
                                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                                            ContentStyle-CssClass="pad">
                                            <ContentStyle VerticalAlign="Top" CssClass="pad">
                                            </ContentStyle>
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                    <div style="padding: 7px 0;">
                                                        <input type="button" value="Select All Products" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="De-select All Products" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>
                                                    </div>
                                                    <div>
                                                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="Indent_Id"
                                                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords"
                                                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                                                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                                                            <SettingsBehavior AllowDragDrop="False"></SettingsBehavior>
                                                            <SettingsPager Visible="false"></SettingsPager>
                                                            <Columns>
                                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                                                                <dxe:GridViewDataTextColumn FieldName="Indent_RequisitionNumber" Caption="Document Number" Width="150" VisibleIndex="1">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Unit" Width="100" VisibleIndex="2">
                                                                </dxe:GridViewDataTextColumn>
                                                                <dxe:GridViewDataTextColumn FieldName="Indent_RequisitionDate" Caption="Date" Width="150" VisibleIndex="3">
                                                                </dxe:GridViewDataTextColumn>

                                                            </Columns>
                                                            <SettingsDataSecurity AllowEdit="true" />
                                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                        </dxe:ASPxGridView>
                                                    </div>
                                                    <div class="text-center">
                                                        <dxe:ASPxButton ID="btnTaggingSave" ClientInstanceName="cbtnTaggingSave" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                                            <ClientSideEvents Click="function(s, e) {QuotationNumberChanged();}" />
                                                        </dxe:ASPxButton>
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                        </dxe:ASPxPopupControl>

                                        <span id="MandatorysIndentReq" class="POIndentReq  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
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
                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Document Number">
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="gvColQuantity" Width="70" VisibleIndex="6">
                                                                <PropertiesTextEdit>
                                                                    <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                </PropertiesTextEdit>
                                                            </dxe:GridViewDataTextColumn>

                                                        </Columns>

                                                        <SettingsDataSecurity AllowEdit="true" />
                                                        <ClientSideEvents EndCallback="gridProducts_EndCallback" />
                                                    </dxe:ASPxGridView>
                                                    <div class="text-center pTop10">
                                                        <%-- <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />--%>

                                                        <dxe:ASPxButton ID="Button2" ClientInstanceName="cButton2" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                                        </dxe:ASPxButton>
                                                    </div>
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                        </dxe:ASPxPopupControl>


                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Indent/Quotation Date">
                                        </dxe:ASPxLabel>
                                        <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                            <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Indent/Quotation Dates" Style="display: none"></asp:Label>
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
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" ClientInstanceName="ctxt_Refference">
                                        </dxe:ASPxTextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <label id="lblProject" runat="server">Project</label>
                                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="ProjectServerModeDataSource"
                                            KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False">
                                            <Columns>
                                                <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                                <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                </dxe:GridViewDataColumn>
                                            </Columns>
                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                <Templates>
                                                    <StatusBar>
                                                        <table class="OptionsTable" style="float: right">
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </StatusBar>
                                                </Templates>
                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                            </GridViewProperties>
                                            <ClientSideEvents GotFocus="Project_gotFocus" LostFocus="Project_LostFocus" ValueChanged="ProjectValueChange" />
                                            <ClearButton DisplayMode="Always">
                                            </ClearButton>
                                        </dxe:ASPxGridLookup>
                                        <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                                            ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                                    </div>



                                    <div style="clear: both"></div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select">
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
                                        <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate">
                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="ReBindGrid_Currency" />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <%--Rev 1.0: "simple-select" class add --%>
                                    <div class="col-md-2 lblmTop8 simple-select">
                                        <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%" Native="true">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-3 lblmTop8 hide" style="margin-bottom: 15px">
                                        <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" Width="100%" OnCallback="ddl_VatGstCst_Callback">
                                            <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                    <div class="col-md-6 lblmTop8 ">
                                        <div class="row">
                                            <div class="col-md-4 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Credit Days">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txtCreditDays" ClientInstanceName="ctxtCreditDays" runat="server" Width="100%">
                                                    <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents TextChanged="CreditDays_TextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxDateEdit ID="dt_PODue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy"
                                                    UseMaskBehavior="True" ClientInstanceName="cdt_PODue"
                                                    Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents GotFocus="function(s,e){cdt_PODue.ShowDropDown();}" LostFocus="function(s, e) { SetFocusonGrid(e)}" />
                                                </dxe:ASPxDateEdit>
                                                <span id="MandatoryDueDate" class="PODueDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <%--Rev 1.0--%>
                                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                <%--Rev end 1.0--%>
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Place of Supply[GST]">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                                <dxe:ASPxComboBox ID="PurchaseOrderPosGst" ClientInstanceName="cPurchaseOrderPosGst" runat="server" ValueType="System.String" Width="100%">

                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePurchasePosGst(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-4 lblmTop8">
                                        <label>
                                            <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                            </dxe:ASPxLabel>
                                        </label>
                                        <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                    <%-- Add revision no and date Tanmoy--%>
                                    <div class="col-md-2 lblmTop8" id="dvRevision" style="display: none">
                                        <label>
                                            <dxe:ASPxLabel ID="lblRevisionNo" runat="server" Text="Revision No." Width="120px" CssClass="inline">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </label>
                                        <dxe:ASPxTextBox ID="txtRevisionNo" runat="server" Width="100%" MaxLength="50" ClientInstanceName="ctxtRevisionNo">
                                            <%-- <ClientSideEvents LostFocus="Revision_LostFocus" />--%>
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8" id="dvRevisionDate" style="display: none">
                                        <label>
                                            <dxe:ASPxLabel ID="lblRevisionDate" runat="server" Text="Revision Date" Width="120px" CssClass="inline">
                                            </dxe:ASPxLabel>
                                            <span style="color: red;">*</span>
                                        </label>
                                        <dxe:ASPxDateEdit ID="txtRevisionDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="ctxtRevisionDate" Width="100%">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents GotFocus="function(s,e){ctxtRevisionDate.ShowDropDown();}" />
                                        </dxe:ASPxDateEdit>
                                    </div>
                                    <%-- Add revision no and date Tanmoy--%>
                                    <%--Add approve remarks Tanmoy --%>
                                    <div class="clear"></div>
                                    <div class="col-md-6" id="dvAppRejRemarks" style="display: none">
                                        <label>
                                            <asp:Label ID="lblAppRejRemarks" runat="server" Text="Approve/Reject Remarks"></asp:Label>
                                            <span style="color: red;">*</span>
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txtAppRejRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="2" Columns="8" Height="50px"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--Add approve remarks Tanmoy --%>
                                    <div style="clear: both;"></div>
                                    <div class="col-md-12">
                                        <div class="makeFullscreen ">
                                            <span class="fullScreenTitle">Add Purchase Order</span>
                                            <span class="makeFullscreen-icon half hovered " data-instance="grid" title="Maximize Grid"><i class="fa fa-expand"></i></span>
                                            <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="OrderDetails_Id" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                                OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                                OnHtmlRowPrepared="grid_HtmlRowPrepared"
                                                Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150" RowHeight="2" SettingsPager-Mode="ShowAllRecords">
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
                                                    <dxe:GridViewDataTextColumn Caption="Document" FieldName="Indent_Num" ReadOnly="True" Width="6%" VisibleIndex="2">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="3" Width="14%" ReadOnly="True">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" LostFocus="ProductsGotFocus" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>

                                                    <%--Batch Product Popup End--%>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColDiscription" Caption="Description" VisibleIndex="4" Width="18%" ReadOnly="True">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn Caption="Addl Desc." Width="70" VisibleIndex="5">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="addDescRemarks" Image-ToolTip="Remarks" Image-Url="/assests/images/MultiUomIcon.png" Text=" ">
                                                                <Image ToolTip="Addl Desc." Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="gvColQuantity" Caption="Quantity" VisibleIndex="6" Width="6%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityProductsGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColUOM" Caption="UOM" VisibleIndex="7" Width="7%" ReadOnly="True">
                                                        <PropertiesTextEdit>
                                                            <ClientSideEvents GotFocus="uomGotFocus" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="4%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                     <%--Mantis Issue 24429--%>
                                                    <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="PO_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="7%" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents LostFocus="QuantityTextChange" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     
                                                    <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="PO_AltUOM" ReadOnly="true" VisibleIndex="10" Width="7%" >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--VisibleIndex changed for below columns--%>
                                                    <%--End of Mantis Issue 24429--%>


                                                    <dxe:GridViewCommandColumn Width="7%" VisibleIndex="11" Caption="Stk Details">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePrice" Caption="Price" VisibleIndex="12" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="PurchasePriceTextChange" GotFocus="PurchasePriceTextFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColDiscount" Caption="Disc(%)" VisibleIndex="13" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="DiscountValueChange" GotFocus="DiscountTextFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColAmount" Caption="Amount" VisibleIndex="14" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <ClientSideEvents LostFocus="gvColAmountlostfocus" />
                                                            <%-- REV RAJDIP FOR RUNNING TOTAL --%>
                                                            <%--GotFocus="gvColAmountgotfocus"--%>
                                                            <%-- END REV RAJDIP --%>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="gvColTaxAmount" Caption="Charges" VisibleIndex="15" Width="6%" ReadOnly="True"
                                                        HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <%--LostFocus="Taxlostfocus"--%>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColTotalAmountINR" Caption="Net Amount" VisibleIndex="16" Width="8%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                            <%-- Rev Rajdip --%>
                                                            <ClientSideEvents GotFocus="TotalAmountgotfocus" />
                                                            <%--End Rev Rajdip --%>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="PurchaseOrder_InlineRemarks" Width="150" VisibleIndex="17" PropertiesTextEdit-MaxLength="5000">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">
                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="7%" VisibleIndex="18" Caption="Add New">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Quotation No" FieldName="Indent" Width="0" VisibleIndex="19">
                                                        <PropertiesTextEdit>
                                                            <NullTextStyle></NullTextStyle>
                                                            <ReadOnlyStyle></ReadOnlyStyle>
                                                            <Style></Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle />
                                                        <CellStyle>
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" Width="0" VisibleIndex="20">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" Width="0" VisibleIndex="21">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColStockQty" Caption="Stock Qty" Width="0" VisibleIndex="22">
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                            <NullTextStyle></NullTextStyle>
                                                            <ReadOnlyStyle></ReadOnlyStyle>
                                                            <Style></Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle />
                                                        <CellStyle>
                                                        </CellStyle>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="gvColStockUOM" Caption="Stk UOM"
                                                        Width="0" VisibleIndex="23">
                                                        <PropertiesTextEdit>
                                                            <NullTextStyle></NullTextStyle>
                                                            <ReadOnlyStyle></ReadOnlyStyle>
                                                            <Style></Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle />
                                                        <CellStyle></CellStyle>

                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DetailsId" Caption="Details ID" VisibleIndex="24" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="TagDocDetailsId" Caption="Details ID" VisibleIndex="25" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="26" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                </Columns>
                                                <%--      Init="OnInit"BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing"
                                                    CustomButtonClick="OnCustomButtonClick" EndCallback="OnEndCallback" --%>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                            </dxe:ASPxGridView>
                                        </div>
                                    </div>
                                    <%-- Rev Rajdip --%>
                                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                        <ul>
                                            <li class="clsbnrLblTotalQty">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="0.00" ClientInstanceName="cbnrLblTotalQty" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblTaxableAmt">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTaxableAmt" runat="server" Text="Taxable Amt" ClientInstanceName="cbnrLblTaxableAmt" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTaxableAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxableAmtval" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblTaxAmt">
                                                <div class="horizontallblHolder" id="">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax & Others Amt" ClientInstanceName="cbnrLblTaxAmt" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblTaxAmtval" runat="server" Text="0.00" ClientInstanceName="cbnrLblTaxAmtval" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblAmtWithTax" runat="server" id="oldUnitBanerLbl">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblAmtWithTax" runat="server" Text="Amount" ClientInstanceName="cbnrLblAmtWithTax" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrlblAmountWithTaxValue" runat="server" Text="0.00" ClientInstanceName="cbnrlblAmountWithTaxValue" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblInvVal" id="otherChargesId">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="cbnrOtherCharges" runat="server" Text="Additional Amt" ClientInstanceName="cbnrOtherCharges" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrOtherChargesvalue" runat="server" Text="0.00" ClientInstanceName="cbnrOtherChargesvalue" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>

                                            <li class="clsbnrLblLessOldVal" style="display: none;">
                                                <div class="horizontallblHolder" id="">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblLessOldVal" runat="server" Text="Less Old Unit Value" ClientInstanceName="cbnrLblLessOldVal" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblLessOldMainVal" runat="server" Text=" 0.00" ClientInstanceName="cbnrLblLessOldMainVal"></dxe:ASPxLabel>
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblLessAdvance" id="idclsbnrLblLessAdvance" runat="server" style="display: none;">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblLessAdvance" runat="server" Text="Advance Adjusted" ClientInstanceName="cbnrLblLessAdvance" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblLessAdvanceValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblLessAdvanceValue" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>


                                            <li class="clsbnrLblInvVal">
                                                <div class="horizontallblHolder">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Net Amt" ClientInstanceName="cbnrLblInvVal" />
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxLabel ID="bnrLblInvValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblInvValue" />
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>




                                            <li class="clsbnrLblInvVal" style="display: none;">
                                                <div class="horizontallblHolder" style="border-color: #f14327;">
                                                    <table>
                                                        <tbody>
                                                            <tr>
                                                                <td style="background: #f14327;">
                                                                    <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                </td>
                                                                <td>
                                                                    <strong>
                                                                        <dxe:ASPxLabel ID="lblRunningBalanceCapsul" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                    </strong>
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </li>
                                            <li class="clsbnrLblInvVal">
                                                <div runat="server" id="divSendSMS">

                                                    <strong>

                                                        <%-- <input type="checkbox" name="chksendSMS" id="chksendSMS" onclick="SendSMSChk()" />&nbsp;Send SMS--%>
                                                        <asp:HiddenField ID="hdnSendSMS" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnCustMobile" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnsendsmsSettings" runat="server" />
                                                    </strong>

                                                </div>
                                            </li>

                                        </ul>

                                    </div>
                                    <%-- End Rev Rajdip --%>
                                    <div style="clear: both;"></div>
                                    <%--Mantis Issue 25152--%>
                                    <%--Mantis Issue 25235 [ runat="server" added ]--%>
                                    <div class="" id="divIsDirector" runat="server">
                                        <div style="padding-top: 10px;">
                                            <div class="typeHeader col-md-12">Approval Details</div>
                                            <%--<div class="col-md-3">
                                                <label>Approval Action <span style="color: red;">*</span></label>
                                                <div id="tdddlApprovalAction">
                                                    <select id="ddlApprovalAction" class="form-control">
                                                        <option value="0">Select</option>
                                                        <option value="1">Approve</option>
                                                        <option value="2">Reject</option>
                                                        <option value="3">Hold</option>
                                                    </select>
                                                </div>
                                            </div>--%>
                                            <div class="col-md-4">
                                                <div class="checkbox">
                                                    <label class="red">
                                                        <input type="checkbox" id="chkDirectorApprovalRequired" onchange="chkDirectorApprovalRequired_change();" />
                                                        Is Director Approval Required?</label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label id="divEmployee" class="hide">
                                                    <div>Employee</div>
                                                </label>
                                                <div class="hide" id="divEmployeeIn">
                                                    <div class="dropDev">
                                                        <dxe:ASPxComboBox ID="dddlApprovalEmployee" runat="server" ClientInstanceName="cdddlApprovalEmployee" Width="100%">
                                                        </dxe:ASPxComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--<div class="col-md-6">
                                                <label>Remarks <span style="color: red;">*</span></label>
                                                <div>
                                
                                                    <input type="text" class="form-control" id="txtApprovalRemarks" />
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                <%--End of Mantis Issue 25152--%>
                                    <div class="col-md-12 pdTop15">
                                        <div class="pull-left">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New"
                                                CssClass="btn btn-success" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtn_SaveRecordExits" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btn_SaveRecordTaxs" ClientInstanceName="cbtn_SaveRecordTaxs" runat="server" AutoPostBack="False" Text="T&#818;axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>

                                            <%-- Add Approve and Reject Button Tanmoy --%>
                                            <span id="dvApprove" style="display: none">
                                                <dxe:ASPxButton ID="btn_Approve" ClientInstanceName="cbtn_Approve" CssClass="btn btn-success" runat="server" AutoPostBack="False" Text="Approve" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Approve_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>
                                            <span id="dvReject" style="display: none">
                                                <dxe:ASPxButton ID="btn_Reject" ClientInstanceName="cbtn_Reject" runat="server" CssClass="btn btn-danger" AutoPostBack="False" Text="Reject" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Reject_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>
                                            <%-- Add Approve and Reject Button Tanmoy --%>

                                            <b><span id="tagged" style="display: none; color: red" runat="server">This Purchase Order is tagged in other modules. Cannot Modify data except UDF</span></b>
                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />

                                            <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PO" />
                                            <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
                                        </div>

                                        <div class="pull-left" style="padding: 15px;">
                                            <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" />
                                        </div>
                                    </div>
                                </div>
                            </dxe:ContentControl>
                        </ContentCollection>
                    </dxe:TabPage>
                    <dxe:TabPage Name="Billing/Shipping" Text="Our Billing/Shipping">
                        <ContentCollection>
                            <dxe:ContentControl runat="server">

                                <ucBS:Purchase_BillingShipping runat="server" ID="Purchase_BillingShipping" />

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
        <!--Product Modal -->
        <div class="modal fade" id="ProductModel" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Product Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name Or Product Code" />

                        <div id="ProductTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">Id</th>
                                    <th>Product Code</th>
                                    <th>Product Name</th>
                                    <th>Inventory</th>
                                    <th>HSN/SAC</th>
                                    <th>Class</th>
                                    <th>Brand</th>
                                    <%--<th>Installation Reqd.</th>--%>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">

                        <% if (rightsProd.CanAdd)
                           { %>
                        <button type="button" class="btn btn-success btn-radius" onclick="fn_PopOpen();">
                            <span class="btn-icon"><i class="fa fa-plus"></i></span>
                            Add New
                        </button>
                        <% } %>

                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>
        <!--Customer Modal -->
        <div class="modal fade" id="CustModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Vendor Search</h4>
                    </div>
                    <div class="modal-body">
                        <input type="text" onkeydown="VendorModekkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />

                        <div id="CustomerTable">
                            <table border='1' width="100%" class="dynamicPopupTbl">
                                <tr class="HeaderStyle">
                                    <th class="hide">id</th>
                                    <th>Vendor Name</th>
                                    <th>Unique Id</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>
        <%--InlineTax--%>
        <%--Mantis Issue 25152--%>
        <div class="modal fade pmsModal w30" id="assignEmployee" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    
                    <h5 class="modal-title">Select Employee</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    
                </div>
                <div class="modal-body">
                    <div class="row ">
                        
                        <div class="col-md-12 mTop5">
                            <label class="deep">Employee </label>
                            <div class="fullWidth">
                                 <select class="form-control" id="ddl_DirEmployee" style="width:200px" >
                                    <option value="0">--Select--</option>
                                </select>
                                <%--<input type="hidden" id="hdDbName" runat="server" />--%>
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" id="divsave">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                   
                    <button type="button" class="btn btn-success" onclick="PhoneNoSend();">Confirm</button>
                    
                </div>
            </div>
        </div>
    </div>
       <%-- End of Mantis Issue 25152--%>
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

                        <div class="col-sm-2 gstNetAmount hide">
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
                                            <td>Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tax Code/Charges Not defined.
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
                                    <MaskSettings Mask="<0..999999999999999999>.<0..99>;" AllowMouseWheel="false" />
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
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
                                        <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                            <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Left">
                                                <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;000..999&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
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
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                    <dxe:ASPxButton ID="Button1" ClientInstanceName="cButton1" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return BatchUpdate();}" />
                                    </dxe:ASPxButton>
                                    <%--<asp:Button ID="Button3" runat="server" Text="Cancel" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />--%>
                                    <dxe:ASPxButton ID="Button3" ClientInstanceName="cButton3" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger mTop" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;}" />
                                    </dxe:ASPxButton>
                                </div>
                                <table class="pull-right">
                                    <tr>
                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                runat="server" Width="100%" CssClass="pull-left mTop">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                            <td>Tax Code/Charges Not Defined.
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
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                            <MaskSettings Mask="<0..999999999999999999>.<00..999>" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="PercentageTextChange" />
                                            <ClientSideEvents />
                                        </PropertiesTextEdit>
                                        <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                </dxe:ASPxButton>
                                <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="False">
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
                        <div id="content-6" class="pull-right wrapHolder reverse content horizontal-images" style="width: 100%; margin-right: 0px; height: auto;">
                            <ul>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tr>
                                                <td>Selected Unit</td>
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
                                                    <asp:Label ID="lblAvailableStkunit" runat="server"></asp:Label>
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
                                                    <asp:Label ID="lblopeningstockUnit" runat="server"></asp:Label>
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
                                            <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange(s)}" EndCallback="function(s,e){endcallcmware(s)}"></ClientSideEvents>

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
                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                            <ClientSideEvents TextChanged="function(s,e){changedqnty(s)}" LostFocus="function(s,e){Setenterfocuse(s)}" KeyPress="function(s, e) {Keypressevt();}" />
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
                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
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
                                    <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {SaveWarehouse();}" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtnrefreshWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Clear Entries" CssClass="btn btn-primary pull-left" UseSubmitBehavior="False">
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
                                    <dxe:GridViewDataTextColumn Caption="Barcode" FieldName="Barcode" Width="0"
                                        VisibleIndex="7">
                                        <Settings ShowInFilterControl="False" />
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
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="S&#818;ave & Exit" AccessKey="S" CssClass="btn btn-primary" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveWarehouseAll();}" />
                            </dxe:ASPxButton>


                        </div>
                    </div>
                    <%--  </div>--%>
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
            <%--Mantis Issue 25152--%>
            <asp:HiddenField  ID="hdDbName" runat="server" />
            <%--End of Mantis Issue 25152--%>
        </div>

        <%--   Warehouse End    --%>

        <%-- HiddenField --%>
        <div>
            <asp:HiddenField runat="server" ID="hdnJsonProductTax" />
            <asp:HiddenField ID="hfControlData" runat="server" />
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
            <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
            <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
            <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
            <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
            <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
            <asp:HiddenField ID="hdnTagDocType" runat="server" />

            <asp:HiddenField ID="hdnPageStatusForMultiUOM" runat="server" />

            <asp:HiddenField ID="hdnProjectApproval" runat="server" />
            <asp:HiddenField ID="hdnApproveStatus" runat="server" />
            <asp:HiddenField ID="hdnEditPageStatus" runat="server" />
            <asp:HiddenField runat="server" ID="hdnEditOrderId" />
            <%--Mantis Issue 24920--%>
            <asp:HiddenField ID="hdnIsCopy" runat="server" />
            <%--End of Mantis Issue 24920--%>
            <%--Mantis Issue 25152--%>
            <asp:HiddenField ID="hdnEmployee" runat="server" />
            <asp:HiddenField ID="hdnSettings" runat="server" />
            <asp:HiddenField ID="hdnOrderId" runat="server" />
            <asp:HiddenField ID="hdnAddEditCopy" runat="server" />
            <%--End of Mantis Issue 25152--%>
            <%--for Project  --%>
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

        <%-- <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
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
                            <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">

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
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                    </dxe:ASPxGridLookup>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
        </dxe:ASPxPopupControl>--%>

        <%--<asp:SqlDataSource runat="server" ID="ProductDataSource"
            SelectCommand="prc_PurchaseOrderDetailsList" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                <asp:ControlParameter DefaultValue="Y" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                <asp:SessionParameter Name="campany_Id" SessionField="LastCompany1" Type="String" />
                <asp:SessionParameter Type="String" Name="FinYear" SessionField="LastFinYear1" />
            </SelectParameters>
        </asp:SqlDataSource>--%>

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


        <%-- <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='17' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) and Branch=@userbranch and comapanyInt=@company)) as X Order By ID ASC">
            <SelectParameters>
                <asp:SessionParameter Name="userbranch" SessionField="userbranch" Type="string" />
                <asp:SessionParameter Name="company" SessionField="LastCompany1" Type="string" />
                <asp:SessionParameter Name="year" SessionField="LastFinYear1" Type="string" />

            </SelectParameters>
        </asp:SqlDataSource>--%>
        <%-- <asp:SqlDataSource ID="SqlIndentRequisitionNo" runat="server"
            SelectCommand="(Select '0' as Indent_Id,'Select' as Indent_RequisitionNumber) Union
            (select Indent_Id,Indent_RequisitionNumber from tbl_trans_Indent)"></asp:SqlDataSource>--%>
        <%--  <asp:SqlDataSource ID="Sqlvendor" runat="server"
            SelectCommand="select '0' as cnt_internalId,'Select' as Name 
            union select cnt_internalId,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name 
            from tbl_master_contact  where cnt_contacttype='DV'"></asp:SqlDataSource>--%>
        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>

        <%--   <asp:SqlDataSource ID="DS_Branch" runat="server"
            SelectCommand=""></asp:SqlDataSource>--%>
        <%-- <asp:SqlDataSource ID="DS_SalesAgent" runat="server"
            SelectCommand="select '0' as cnt_id,'Select' as Name
            union select cnt_id,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG'"></asp:SqlDataSource>--%>
        <%--  <asp:SqlDataSource ID="DS_AmountAre" runat="server"
            SelectCommand="select '0'as taxGrp_Id,'Select'as taxGrp_Description
            union select taxGrp_Id,taxGrp_Description from tbl_master_taxgrouptype order by taxGrp_Id"></asp:SqlDataSource>--%>


        <%--  <asp:SqlDataSource ID="CountrySelect" runat="server"
            SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>--%>
        <%--<asp:SqlDataSource ID="StateSelect" runat="server"
            SelectCommand="SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
            <SelectParameters>
                <asp:Parameter Name="State" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        <%-- <asp:SqlDataSource ID="SelectCity" runat="server"
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>--%>

        <%--   <asp:SqlDataSource ID="SelectArea" runat="server"
            SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
            <SelectParameters>
                <asp:Parameter Name="Area" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
        <%-- <asp:SqlDataSource ID="SelectPin" runat="server"
            SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>--%>
    </div>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dxe:ASPxLoadingPanel>


    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
        Width="1020px" HeaderText="Add New Vendor" Modal="true" AllowResize="true" ResizingMode="Postponed">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="1100px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closeMultiUOM(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Mantis Issue 24429--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" onchange="CalcBaseRate()" />
                                                <%--End of Rev Mantis Issue 24429--%>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="Left_Content" style="">
                                        <div>
                                            <label style="text-align: right;">Base UOM</label>
                                        </div>
                                        <div>
                                            <dxe:ASPxComboBox ID="cmbUOM" ClientInstanceName="ccmbUOM" runat="server" SelectedIndex="0" DataSourceID="UomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24429--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24429--%>
                                <td>
                                    <span style="font-size: 22px; padding-top: 15px; display: inline-block;">=</span>
                                </td>
                                <td>
                                    <div>
                                        <div>
                                            <label style="text-align: right;">Alt. UOM</label>
                                        </div>
                                        <div class="Left_Content" style="">
                                            <dxe:ASPxComboBox ID="cmbSecondUOM" ClientInstanceName="ccmbSecondUOM" runat="server" SelectedIndex="0" DataSourceID="AltUomSelect"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" ValueField="UOM_ID" TextField="UOM_Name">
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--  <input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                 <%--Mantis Issue 24429--%>
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Mantis Issue 24429--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24429--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <div>
                                           <%-- <label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>

                                             <label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24429--%>
                                <td style="padding-top: 14px;">
                                    <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="function(s, e) { SaveMultiUOM();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                 <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>

                                 <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>


                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="3" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24429--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24429--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>','<%#Eval("DetailsId") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>
                                        <%--Mantis Issue 24429 --%>

                                           <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                          <%--End of Mantis Issue 24429 --%>
                                    </DataItemTemplate>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="OnMultiUOMEndCallback" />
                            <SettingsPager Visible="false"></SettingsPager>
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clearfix">
                        <br />
                        <div style="align-content: center">
                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                        </div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="PosView" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cPosView" Height="650px"
        Width="1200px" HeaderText="Product" Modal="true">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>


    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Remarks" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <%--<ClientSideEvents Closing="function(s, e) {
	                   closeRemarks(s, e);}" />--%>
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>


                        <dxe:PopupControlContentControl runat="server">
                            <div>
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Additional Description"></asp:Label>

                                <asp:TextBox ID="txtInlineRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
                            </div>


                            <div class="clearfix">
                                <br />
                                <div style="align-content: center">
                                    <dxe:ASPxButton ID="btnSaveInlineRemarks" ClientInstanceName="cbtnSaveInlineRemarks" Width="50px" runat="server" AutoPostBack="false" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">

                                        <ClientSideEvents Click="function(s, e) {FinalRemarks();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>



                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="callback_InlineRemarks_EndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <asp:HiddenField ID="hdnQuantitySL" runat="server" />
    <asp:HiddenField ID="hdnForBranchTaggingPurchase" runat="server" />
    <asp:HiddenField ID="hddnDocumentIdTagged" runat="server" />
    <asp:HiddenField ID="hdnEntityType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnQty" />
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnApprovalsetting" runat="server" />
    <asp:HiddenField ID="hdnADDEditMode" runat="server" />
    <asp:HiddenField ID="HdnBackDatedEntryPurchaseGRN" runat="server" />
    <asp:HiddenField ID="hdnBackdateddate" runat="server" />
    <%--Mantis Issue 25235--%>
    <asp:HiddenField ID="hdnVendorRequiredInPurchaseIndent" runat="server" />
    <%--End of Mantis Issue 25235--%>
</asp:Content>
