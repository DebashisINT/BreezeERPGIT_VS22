<%--=======================================================Revision History=========================================================================
 1.0     Sanchita    V2.0.41  14-11-2023     	0026854: Data Freeze Required for Project Sale Invoice & Project Purchase Invoice
=========================================================End Revision History========================================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProjectIssueMaterialReturn.aspx.cs" Inherits="ERP.OMS.Management.Activities.ProjectIssueMaterialReturn" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%--<%@ Register Src="~/OMS/Management/Activities/UserControls/BillingShippingControl.ascx" TagPrefix="ucBS" TagName="BillingShippingControl" %>--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js?var=1.2" type="text/javascript"></script>--%>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=1.0.0" type="text/javascript"></script>
    <script src="JS/SearchPopup.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/ProjectIssueMaterialReturn.js?v=3.3"></script>
    
    <script>
        // Rev 1.0
        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cPLSalesChallanDate.GetDate()) && (cPLSalesChallanDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
            }
        }
        // End of Rev 1.0

        $(document).ready(function () {
            var setting = document.getElementById("hdnShowUOMConversionInEntry").value;
            if (setting == 1) {

                document.getElementById("_div_Uom").style.display = "block";
                document.getElementById("div_AltQuantity").style.display = "block";
            }
            else {
                document.getElementById("_div_Uom").style.display = "none";
                document.getElementById("div_AltQuantity").style.display = "none";

            }

        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var mode = $('#<%=hdnADDEditMode.ClientID %>').val();
            if (mode == 'Edit') {
                if ($("#hdnCustomerId").val() != "") {
                    var VendorID = $("#hdnCustomerId").val();
                    SetEntityType(VendorID);
                }
            }
        });
        var StateCodeList = [];
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        function GlobalBillingShippingEndCallBack() {
            //  alert('1111');
            if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
                cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                if (gridquotationLookup.GetValue() != null) {
                    // var key = ctxtCustName.GetValue();
                    var key = $('#<%=hdfLookupCustomer.ClientID %>').val();
                    if (key != null && key != '') {
                        cContactPerson.PerformCallback('BindContactPerson~' + key);
                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        if (key != null && key != '') {

                            // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');
                        }
                        //grid.PerformCallback('GridBlank');
                        deleteAllRows();
                        grid.AddNewRow();
                        grid.GetEditor('SrlNo').SetValue('1');
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "")
                    }

                }
                else {
                    // var key = ctxtCustName.GetValue();
                    var key = $('#<%=hdfLookupCustomer.ClientID %>').val();
                    GetObjectID('hdnCustomerId').value = key;
                    if (key != null && key != '') {
                        cContactPerson.PerformCallback('BindContactPerson~' + key);
                        // cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + '%');                      
                    }
                }
            }
        }


        function callback_InlineRemarks_EndCall(s, e) {

            if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
                $("#txtInlineRemarks").focus();
            }
            else {
                cPopup_InlineRemarks.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }

    </script>
    <script type="text/javascript">
        function grid_ProductsEndCallback(s, e) {
            // alert(cgridproducts.cpTaxTypeid + 'grid_ProductsEndCallback');
            cddl_AmountAre.SetValue(cgridproducts.cpTaxTypeid);
            cddlVatGstCst.PerformCallback('Tax-code' + '~' + '');//Tax_Code
            if (cgridproducts.cpTaxType == "refreshGrid") {
                jAlert("Selected invoice different Amounts");
                gridquotationLookup.SetText("");

            }
            oldadate = tstartdate.GetDate();
        }
    </script>
    <script>

        function acpContactPersonPhoneEndCall(s, e) {
            if (cacpContactPersonPhone.cpPhone != null && cacpContactPersonPhone.cpPhone != undefined) {
                pageheaderContent.style.display = "block";
                $("#<%=divContactPhone.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblContactPhone.ClientID %>').innerHTML = cacpContactPersonPhone.cpPhone;
                cacpContactPersonPhone.cpPhone = null;
            }
        }

        function componentEndCallBack(s, e) {
            LoadingPanel.Hide();
            gridquotationLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {

                /// OnAddNewClick();  kaushik 3/7/2017  avoid extra rows
            }

            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;
                var SpliteDetails = details.split("~");
                var Reference = SpliteDetails[0];
                var Currency_Id = (SpliteDetails[1] == "" || SpliteDetails[1] == null) ? "0" : SpliteDetails[1];
                var SalesmanId = (SpliteDetails[2] == "" || SpliteDetails[2] == null) ? "0" : SpliteDetails[2];
                var CurrencyRate = SpliteDetails[4];
                var Contact_person_id = SpliteDetails[5];
                var Tax_option = (SpliteDetails[6] == "" || SpliteDetails[6] == null) ? "1" : SpliteDetails[6];
                var Tax_Code = (SpliteDetails[7] == "" || SpliteDetails[7] == null) ? "0" : SpliteDetails[7];
                $('#<%=txt_Refference.ClientID %>').val(Reference);
                ctxt_Rate.SetValue(CurrencyRate);
                //alert(Tax_option + 'componentEndCallBack');
                cddl_AmountAre.SetValue(Tax_option); if (Tax_option == 1) {
                    grid.GetEditor('TaxAmount').SetEnabled(true);
                    cddlVatGstCst.SetEnabled(false);
                    cddlVatGstCst.SetSelectedIndex(0);
                    cbtn_SaveRecords.SetVisible(true);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }

                }
                else if (Tax_option == 2) {
                    grid.GetEditor('TaxAmount').SetEnabled(true);
                    cddlVatGstCst.SetEnabled(true);
                    cddlVatGstCst.PerformCallback('2');
                    cddlVatGstCst.Focus();
                    cbtn_SaveRecords.SetVisible(true);
                }
                else if (Tax_option == 3) {
                    grid.GetEditor('TaxAmount').SetEnabled(false);
                    cddlVatGstCst.SetSelectedIndex(0);
                    cddlVatGstCst.SetEnabled(false);
                    cbtn_SaveRecords.SetVisible(false);
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        grid.batchEditApi.StartEdit(-1, 2);
                    }
                }
                cddlVatGstCst.PerformCallback('Tax-code' + '~' + Tax_Code)
                document.getElementById('ddl_Currency').value = Currency_Id;
                document.getElementById('ddl_SalesAgent').value = SalesmanId;
                if (Contact_person_id != "0" && Contact_person_id != "")
                { cContactPerson.SetValue(Contact_person_id); }

            }

            gridquotationLookup.SetText("");
        }
        function componentDateEndCallBack() {
            var PlaceOfSupplyValue = $('#hdnPlaceOfSupply').val();
            if (PlaceOfSupplyValue != "") {
                cddlPosGstSReturn.Clear();
            }
            var SplitValue = PlaceOfSupplyValue.split("~");
            var PlaceText = SplitValue[0];
            var PlaceValue = SplitValue[1];
            // cddlPosGstReturnNormal.SetValue($('#hdnPlaceOfSupply').val());
            // cddlPosGstReturnNormal.items.AddItem($('#hdnPlaceOfSupply').val());
            cddlPosGstSReturn.AddItem(PlaceText, PlaceValue);
            cddlPosGstSReturn.SetText(PlaceText);
        }






        function PerformCallToGridBind() {
            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            $('#hdnPageStatus').val('Invoiceupdate');
            cProductsPopup.Hide();
            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
                callTransporterControl(quote_Id[0], 'MIR');
            }

            cddlPosGstSReturn.SetEnabled(false);
            var Invoice_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if (Invoice_Id.length > 0) {
                BindOrderProjectdata(Invoice_Id[0], 'MI');
            }



            return false;
        }


        //.............Available Stock Div Show............................


        function acpAvailableStockEndCall(s, e) {

            if (cacpAvailableStock.cpstock != null) {
                divAvailableStk.style.display = "block";
                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                cCmbWarehouse.cpstock = null;
            }
        }

        //................Available Stock Div Show....................







        //transporter


        function onBranchItems() {

            grid.batchEditApi.StartEdit(-1, 1);
            var accountingDataMin = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();
            grid.batchEditApi.StartEdit(0, 1);
            var accountingDataplus = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();

            if (accountingDataMin != null || accountingDataplus != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');
                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                        //  var key = ctxtCustName.GetValue();
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                        if (key != null && key != '') {
                            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                        }
                        // grid.PerformCallback('GridBlank');
                        deleteAllRows();
                        grid.AddNewRow();
                        grid.GetEditor('SrlNo').SetValue('1');
                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter();
                        }
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "")
                        $('#<%=txt_InvoiceDate.ClientID %>').val('');


                    } else {

                    }
                });
            }
            else {
                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                page.SetActiveTabIndex(0);
                $('.dxeErrorCellSys').addClass('abc');
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                //  var key = ctxtCustName.GetValue();
                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                deleteTax('DeleteAllTax', "", "")
                page.SetActiveTabIndex(0);

            }
        }
    </script>


    <script type="text/javascript">

        var taxJson;
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
                ctxtTaxTotAmt.SetValue(cgridTax.cpUpdated.split('~')[1]);
                var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]);
                var ddValue = parseFloat(ctxtGstCstVat.GetValue());
                ctxtTaxTotAmt.SetValue(gridValue + ddValue);
                cgridTax.cpUpdated = "";
            }
            else {
                //var totAmt = ctxtTaxTotAmt.GetValue();
                //cgridTax.CancelEdit();
                //caspxTaxpopUp.Hide();
                //grid.batchEditApi.StartEdit(globalRowIndex, 16);             
                //grid.GetEditor("TaxAmount").SetValue(totAmt);

                //if (cddl_AmountAre.GetValue() == "1") {
                //    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
                //}
                //else if (cddl_AmountAre.GetValue() == "2") {
                //    grid.GetEditor("TotalAmount").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()), 2));
                //}

                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
                grid.GetEditor("TaxAmount").SetValue(totAmt);

                var totalGst = 0;
                var GSTType = 'G';

                if (cgridTax.cpTotalGST != null) {

                    if (cgridTax.cpGSTType != null) {
                        GSTType = cgridTax.cpGSTType;
                        cgridTax.cpGSTType = null;
                    }

                    totalGst = parseFloat(cgridTax.cpTotalGST);
                    var qty = grid.GetEditor("Quantity").GetValue();
                    var price = grid.GetEditor("SalePrice").GetValue();
                    var Discount = grid.GetEditor("Discount").GetValue();

                    var finalAmt = qty * price;


                    //var totalNetAmount = (DecimalRoundoff(totAmt, 2) + DecimalRoundoff(grid.GetEditor("Amount").GetValue(), 2)).toFixed(2);
                    //var totalRoundOffAmount = totalNetAmount;
                    //grid.GetEditor("TotalAmount").SetValue(totalRoundOffAmount);

                    //if (cddl_AmountAre.GetValue() == "2") {
                    //    grid.GetEditor("Amount").SetValue(parseFloat(grid.GetEditor("Amount").GetValue()) + (totalRoundOffAmount - totalNetAmount));
                    //    var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
                    //    var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);
                    //    cbnrlblAmountWithTaxValue.SetValue(parseFloat(Math.round(Math.abs(finalAmount) * 100) / 100).toFixed(2));
                    //}



                    //if (GSTType=="G")
                    //    grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - totalGst), 2));
                    //else if (GSTType == "N") {
                    if (cddl_AmountAre.GetValue() == "2") {
                        grid.GetEditor("Amount").SetValue(DecimalRoundoff((finalAmt - (finalAmt * (Discount / 100)) - totalGst), 2));
                    }
                    //}
                    cgridTax.cpTotalGST = null;

                }

                var totalNetAmount = DecimalRoundoff((parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue())), 2);
                grid.GetEditor("TotalAmount").SetValue(totalNetAmount);



                var finalNetAmount = parseFloat(grid.GetEditor("TotalAmount").GetValue());
                //var finalAmount = parseFloat(cbnrlblAmountWithTaxValue.GetValue()) + (finalNetAmount - globalNetAmount);



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









    </script>

    <script type="text/javascript">

        function GetContactPerson(e) {
            var startDate = new Date();

            startDate = tstartdate.GetDate().format('yyyy/MM/dd');

            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        gridquotationLookup.gridView.UnselectRows();

                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                        //  var key = ctxtCustName.GetValue();
                        if (key != null && key != '') {
                            $('#<%=txt_InvoiceDate.ClientID %>').val('');
                            $('.dxeErrorCellSys').addClass('abc');

                            var startDate = new Date();
                            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                            if (key != null && key != '') {
                                //cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                            }

                            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                    clearTransporter();
                                }
                            }

                            $('#<%=txt_InvoiceDate.ClientID %>').val('');

                            // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                            // SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                            //GetPurchaseForGstValue();
                            GetObjectID('hdnCustomerId').value = key;
                            page.tabs[0].SetEnabled(true);
                            page.tabs[1].SetEnabled(true);

                            //###### END : Samrat Roy : END ########## 

                        }

                    } else {

                        ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                        var Customerid = $('#<%=hdnCustomerId.ClientID %>').val();
                        ctxtCustName.PerformCallback(Customerid);
                        // gridLookup.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                    }

                });
            }
            else {
                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                //  var key = ctxtCustName.GetValue();
                if (key != null && key != '') {
                    $('#<%=txt_InvoiceDate.ClientID %>').val('');

                    //###### Added By : Samrat Roy ##########

                    // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SR');
                    //SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                    // GetPurchaseForGstValue();

                    GetObjectID('hdnCustomerId').value = key;
                    page.tabs[0].SetEnabled(true);
                    page.tabs[1].SetEnabled(true);

                    //###### END : Samrat Roy : END ########## 
                    $('.dxeErrorCellSys').addClass('abc');
                    GetObjectID('hdnCustomerId').value = key;
                    GetObjectID('hdnAddressDtl').value = '0';
                }

            }

        }

        $(document).ready(function () {
            $('#ddl_Currency').change(function () {

                var CurrencyId = $(this).val();
                var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                var basedCurrency = LocalCurrency.split("~")[0];
                if ($("#ddl_Currency").val() == basedCurrency) {
                    ctxt_Rate.SetValue("");
                    ctxt_Rate.SetEnabled(false);
                }
                else {
                    if (basedCurrency != CurrencyId) {
                        if (LocalCurrency != null) {
                            if (CurrencyId != '0') {
                                $.ajax({
                                    type: "POST",
                                    url: "ProjectIssueMaterialReturn.aspx/GetCurrentConvertedRate",
                                    data: "{'CurrencyId':'" + CurrencyId + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var currentRate = msg.d;
                                        if (currentRate != null) {

                                            ctxt_Rate.SetValue(currentRate);
                                        }
                                        else {
                                            ctxt_Rate.SetValue('1');
                                        }
                                        ReBindGrid_Currency();
                                    }
                                });
                            }
                            else {
                                ctxt_Rate.SetValue("1");
                                ReBindGrid_Currency();
                            }
                        }
                    }
                    else {
                        ctxt_Rate.SetValue("1");
                        ReBindGrid_Currency();
                    }
                    ctxt_Rate.SetEnabled(true);
                }
            });
        });

    </script>

    <script>
        var IsProduct = "";
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;

        function GridCallBack() {

            $('#ddl_numberingScheme').focus();
            // grid.PerformCallback('Display');
        }


        function cmbContactPersonEndCall(s, e) {

            LoadingPanel.Hide();

            if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);
                cdt_SaleInvoiceDue.SetDate(myDate);
                cContactPerson.cpDueDate = null;
            }

            if (cContactPerson.cpGSTN != null && cContactPerson.cpGSTN != undefined) {

                $("#<%=divGSTN.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = cContactPerson.cpGSTN;
                cContactPerson.cpGSTN = null;
            }

            if (cContactPerson.cpOutstanding != null && cContactPerson.cpOutstanding != undefined) {

                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:block');

                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:block');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = cContactPerson.cpOutstanding;

                cContactPerson.cpOutstanding = null;
            }
            else {
                $("#<%=pageheaderContent.ClientID%>").attr('style', 'display:none');
                $("#<%=divOutstanding.ClientID%>").attr('style', 'display:none');
                document.getElementById('<%=lblOutstanding.ClientID %>').innerHTML = '';
            }


        }

        function OnEndCallback(s, e) {
            var value = document.getElementById('hdnRefreshType').value;

            //Debjyoti Check grid needs to be refreshed or not
            if ($('#<%=HdUpdateMainGrid.ClientID %>').val() == 'True') {
                $('#<%=HdUpdateMainGrid.ClientID %>').val('False');
                grid.PerformCallback('DateChangeDisplay');
            }

            //if (grid.cpRemoveProductInvoice) {
            //    if (grid.cpRemoveProductInvoice == "valid") {
            //        OnAddNewClick();
            //        grid.cpRemoveProductInvoice = null;
            //    }
            //}
            //else { grid.GetEditor('Product').SetEnabled(true); }  //when invoice is not select
            if (grid.cpSaveSuccessOrFail == "outrange") {
                LoadingPanel.Hide();
                jAlert('Can Not Add More Material Issue Number as Material Issue Scheme Exausted.<br />Update The Scheme and Try Again');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                LoadingPanel.Hide();
                OnAddNewClick();
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.cpSaveSuccessOrFail = null;
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                LoadingPanel.Hide();
                jAlert('Can Not Save as Duplicate Material Issue Number No. Found');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "quantityTagged") {
                LoadingPanel.Hide();
                jAlert(' Quantity of selected products cannot be less than Ordered Quantity.');
                OnAddNewClick();
            }
                //else if (grid.cpSaveSuccessOrFail == "EmptyPlaceOfsupply") {
                //    LoadingPanel.Hide();
                //    jAlert('Please enter valid Place Of Supply.');
                //    OnAddNewClick();
                //}
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                LoadingPanel.Hide();
                jAlert('Please try again later.');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "nullAmount") {
                LoadingPanel.Hide();
                jAlert('total amount cant not be zero(0).');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                LoadingPanel.Hide();
                jAlert('Please fill Quantity');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {
                LoadingPanel.Hide();
                jAlert('Can not Duplicate Product in the Material Received List.');
                OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                LoadingPanel.Hide();
                var SrlNo = grid.cpProductSrlIDCheck;
                var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
                jAlert(msg);
                OnAddNewClick();
            }


            else if (grid.cpSaveSuccessOrFail == "checkAddress") {
                LoadingPanel.Hide();
                jAlert('Enter Billing Shipping address to save this document.');
                //OnAddNewClick();
                grid.AddNewRow();
                grid.cpSaveSuccessOrFail = null;
            }
            else {
                var SalesReturn_Number = grid.cpQuotationNo;
                var SalesReturn_Msg = "Material Received No. " + SalesReturn_Number + " saved.";

                if (value == "E") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else {
                        if (SalesReturn_Number != "") {

                            jAlert(SalesReturn_Msg, 'Alert Dialog: [Material Received]', function (r) {
                                LoadingPanel.Hide();
                                grid.cpQuotationNo = null;
                                if (r == true) {
                                    window.location.assign("ProjectIssueMaterialReturnList.aspx");
                                }
                            });


                        }
                        else {

                            window.location.assign("ProjectIssueMaterialReturnList.aspx");
                        }
                    }

                }
                else if (value == "N") {
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    else {
                        if (SalesReturn_Number != "") {

                            jAlert(SalesReturn_Msg, 'Alert Dialog: [Material Received]', function (r) {
                                LoadingPanel.Hide();
                                grid.cpQuotationNo = null;
                                if (r == true) {

                                    window.location.assign("ProjectIssueMaterialReturn.aspx?key=ADD");
                                }
                            });

                        }
                        else {

                            window.location.assign("ProjectIssueMaterialReturn.aspx?key=ADD");
                        }
                    }
                }
                else {
                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {

                        grid.batchEditApi.EndEdit();

                        // above part has been commented by sam on 04032017 due to set focus from server side start

                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxt_Rate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "update") {

                        grid.batchEditApi.StartEdit(0, 1)
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                        var basedCurrency = LocalCurrency.split("~");
                        if ($("#ddl_Currency").val() == basedCurrency[0]) {
                            ctxt_Rate.SetEnabled(false);
                        }
                    }
                    else if (pageStatus == "Invoiceupdate") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "delete") {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else {
                        grid.StartEditRow(0);

                    }
    }
}

    if (grid.cpGridBlank == "1") {


        gridquotationLookup.gridView.Refresh();

        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        grid.GetEditor('Product').SetEnabled(true);
        grid.cpGridBlank = null;
    }
    else {
        if (gridquotationLookup.GetValue() != null) {
            //grid.GetEditor('Product').SetEnabled(false);


        }
        else {

            //grid.GetEditor('Product').SetEnabled(true);
        }
    }

    cProductsPopup.Hide();

}



var chargejsonTax;
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



    </script>


    <script type="text/javascript">
        //warehouse

        function CmbWarehouseEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouse.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouse.SetEnabled(true);
            }

            // Changes
            if (cCmbWarehouse.cpwarehouseid == "Y") {
                cCmbWarehouse.cpwarehouseid = null;

                var WarehouseID = cCmbWarehouse.GetValue();
                var type = document.getElementById('hdfProductType').value;

                if (type == "WBS" || type == "WB") {
                    cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
                }
                else if (type == "WS") {
                    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
                }
            }
        }

        function CmbBatchEndCall(s, e) {
            if (SelectBatch != "0") {
                cCmbBatch.SetValue(SelectBatch);
                SelectBatch = "0";
            }
            else {
                cCmbBatch.SetEnabled(true);
            }
        }

        function listBoxEndCall(s, e) {
            if (SelectSerial != "0") {
                var values = [SelectSerial];
                checkListBox.SelectValues(values);
                UpdateSelectAllItemState();
                UpdateText();
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
            }
        }


        function OnWarehouseEndCallback(s, e) {
            var Ptype = document.getElementById('hdfProductType').value;

            if (cGrdWarehouse.cpIsSave == "Y") {
                cPopup_Warehouse.Hide();
                grid.batchEditApi.StartEdit(Warehouseindex, 8);
            }
            else if (cGrdWarehouse.cpIsSave == "N") {
                jAlert('Sales Quantity must be equal to Warehouse Quantity.');
            }
            else {
                if (document.getElementById("myCheck").checked == true) {
                    if (IsPostBack == "N") {
                        checkListBox.PerformCallback('BindSerial~' + PBWarehouseID + '~' + PBBatchID);

                        IsPostBack = "";
                        PBWarehouseID = "";
                        PBBatchID = "";
                    }

                    if (Ptype == "W" || Ptype == "WB") {
                        cCmbWarehouse.Focus();
                    }
                    else if (Ptype == "B") {
                        cCmbBatch.Focus();
                    }
                    else {
                        ctxtserial.Focus();
                    }
                }
                else {
                    if (Ptype == "W" || Ptype == "WB" || Ptype == "WS" || Ptype == "WBS") {
                        cCmbWarehouse.Focus();
                    }
                    else if (Ptype == "B" || Ptype == "BS") {
                        cCmbBatch.Focus();
                    }
                    else if (Ptype == "S") {
                        checkComboBox.Focus();
                    }
                }
            }
            //Rev Subhra 15-05-2019
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
            //End of Rev Subhra 15-05-2019
        }

        var SelectWarehouse = "0";
        var SelectBatch = "0";
        var SelectSerial = "0";
        var SelectedWarehouseID = "0";


        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpEdit != null) {
                var strWarehouse = cCallbackPanel.cpEdit.split('~')[0];
                var strBatchID = cCallbackPanel.cpEdit.split('~')[1];
                var strSrlID = cCallbackPanel.cpEdit.split('~')[2];
                var strQuantity = cCallbackPanel.cpEdit.split('~')[3];
                var AltQty = cCallbackPanel.cpEdit.split('~')[4];
                var AltUOM = cCallbackPanel.cpEdit.split('~')[5];
                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
                if ($("#hdnShowUOMConversionInEntry").val() == "1") {
                    CtxtPacking.SetValue(AltQty);
                    ccmbPackingUom1.SetValue(AltUOM);
                  }
                }
        }
    </script>

    <style>
        .dxgvcontrol_plasticblue td.dxgvbatcheditmodifiedcell_plasticblue {
            background: #fff !important;
        }

        .popover {
            z-index: 999999;
            max-width: 350px;
        }

            .popover .popover-title {
                margin-top: 0 !important;
                background: #465b9d;
                color: #fff;
            }

        .pdleft15 {
            padding-left: 15px;
        }

        .mtop {
            margin-top: 10px;
        }

        .mleft {
            margin-left: 15px;
        }

        .popover .popover-content {
            min-height: 60px;
        }


        #grid_dxstatus {
            display: none;
        }

        #aspxgridtax_dxstatus {
            display: none;
        }

        #gridtax_dxstatus {
            display: none;
        }

        .hidecell {
            display: none;
        }

        .pullleftclass {
            position: absolute;
            right: -3px;
            top: 24px;
        }

        #mycheck {
            transform: translatey(2px);
            -webkit-transform: translatey(2px);
            -moz-transform: translatey(2px);
            margin-right: 5px;
        }
        /*/*#grid_dxmaintable>tbody>tr> td:last-child {
    display: none !important;
}*/
    
        .dxeerrorframesys.dxeerrorcellsys {
            position: absolute;
        }

        .dxebuttoneditclearbutton_plasticblue {
            display: none;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .validclass {
            position: absolute;
            right: -4px;
            top: 20px;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }

        #txtproductamount, #txtproducttaxamount, #txtproductdiscount {
            font-weight: bold;
        }


        .crossbtn {
            cursor: pointer;
        }

        #txttaxtotamt input, #txtprodbasicamt input, #txtgstcstvat input {
            text-align: right;
        }

        #grid .dxgvhsdc > div, #grid .dxgvcsd {
            width: 100% !important;
        }

        #grid_dxmaintable > tbody > tr > td:nth-child(24) {
            display: none;
        }
    .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
    background: #FFFFFF;
}
        .inline {
            display: inline !important;
            
        }

        #lookup_project .dxebuttoneditclearbutton_plasticblue {
            display: table-cell;
        }

        .dxtclite_plasticblue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }


        .fa.fa-exclamation-circle:before {
            font-family: fontawesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtcreditlimit_ec {
            position: absolute;
        }

        #grid_dxstatus span > a {
            display: none;
        }

        #aspxgridtax_dxeditingerrorrow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #rdl_saleinvoice {
            margin-top: 3px;
        }

            #rdl_saleinvoice > tbody > tr > td {
                padding-right: 5px;
            }

        #grid_dxmaintable > tbody > tr > td:last-child,
        #grid_dxmaintable > tbody > tr > td:last-child > div,
        #grid_dxmaintable > tbody > tr > td:nth-child(20) {
            display: none !important;
        }

        .classout {
            text-transform: none !important;
        }

        .dynamicpopuptbl > tbody > tr > td input { 
            border: none !important;
            cursor: pointer;
            background: transparent !important;
            height: 14px;
            margin-bottom: 0;
            font: 12px tahoma, geneva, sans-serif;
            width: 400px;
        }
    
        .col-md-2 > label, .col-md-2 > span,
        .col-md-1 > label, .col-md-1 > span {
            margin-top: 8px;
            display: inline-block;
        }
        #grid_DXStatus {
            display:none
        }
    </style>
    <script>
        function DateCheck(s, e) {

            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        page.SetActiveTabIndex(0);
                        $('.dxeErrorCellSys').addClass('abc');
                        var startDate = new Date();
                        startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                        cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                        if (key != null && key != '') {
                            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                        }
                        // grid.PerformCallback('GridBlank');
                        deleteAllRows();
                        grid.AddNewRow();
                        grid.GetEditor('SrlNo').SetValue('1');
                        if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                            clearTransporter();
                        }
                        ccmbGstCstVat.PerformCallback();
                        ccmbGstCstVatcharge.PerformCallback();
                        //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        deleteTax('DeleteAllTax', "", "")
                        $('#<%=txt_InvoiceDate.ClientID %>').val('');
                    }
                    else {
                        tstartdate.SetDate(oldadate);
                    }
                });
            }
            else {

                var startDate = new Date();
                startDate = tstartdate.GetDate().format('yyyy/MM/dd');
                page.SetActiveTabIndex(0);
                $('.dxeErrorCellSys').addClass('abc');
                cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');
                //  var key = ctxtCustName.GetValue();
                var key = $('#<%=hdnCustomerId.ClientID %>').val();
                if (key != null && key != '') {
                    cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                }
                ccmbGstCstVat.PerformCallback();
                ccmbGstCstVatcharge.PerformCallback();
                //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                deleteTax('DeleteAllTax', "", "")
                page.SetActiveTabIndex(0);

            }
        }
    </script>

    
    <%--Batch Product Popup End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
        </h3>

        <div id="pageheaderContent" class="pull-right wrapHolder content horizontal-images" style="display: none;" runat="server">
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
                                    <td>Receivable</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOutstanding" runat="server" Text="0.0" CssClass="classout"></asp:Label>
                                    </td>
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

                    <li>
                        <div class="lblHolder" id="divGSTN" style="display: none;" runat="server">
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
                                        <asp:Label ID="lblbranchName" runat="server"></asp:Label>
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
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="ProjectIssueMaterialReturnList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
    <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="">
                                        <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix col-md-12">
                                            <div class="col-md-2" id="divScheme" runat="server">

                                                <asp:Label ID="lbl_NumberingScheme" runat="server" Text="Numbering Scheme"></asp:Label>
                                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1" onchange="CmbScheme_ValueChange()">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_SaleInvoiceNo" runat="server" Text="Document No."></asp:Label><span style="color: red">*</span>

                                                <asp:TextBox ID="txt_PLQuoteNo" runat="server" TabIndex="2" Width="100%"></asp:TextBox>

                                                <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                                </span>
                                                <span id="duplicateQuoteno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                                </span>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_SaleInvoiceDt" runat="server" Text="Posting Date"></asp:Label>
                                                <span style="color: red">*</span>
                                                <%--Rev 1.0 [ LostFocus="function(s, e) { SetLostFocusonDemand(e)}" added ]--%>
                                                <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%">
                                                    <ClientSideEvents DateChanged="DateCheck" Init="dateInit" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                                    <ClientSideEvents GotFocus="function(s,e){tstartdate.ShowDropDown();}"></ClientSideEvents>
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_Branch" runat="server" Text="Unit"></asp:Label>
                                                <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="4" onchange="onBranchItems()">
                                                </asp:DropDownList>
                                            </div>
                                            <%-- kaushik 20-10-2017--%>
                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_Customer" runat="server" Text="Customer"></asp:Label>
                                                <span style="color: red">*</span>
                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>
                                                <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                            </div>
                                            <%-- kaushik 20-10-2017--%>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_ContactPerson" runat="server" Text="Contact Person"></asp:Label>
                                                <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" ClientSideEvents-EndCallback="cmbContactPersonEndCall" TabIndex="6" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">

                                                    <ClientSideEvents GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />

                                                    <ClientSideEvents TextChanged="function(s, e) { GetContactPersonPhone(e)}" />
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div style="clear: both"></div>
                                            <div class="col-md-2">

                                                <asp:Label ID="ASPxLabel3" runat="server" Text="Salesman/Agents"></asp:Label>
                                                <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="7">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_Refference" runat="server" Text="Reference"></asp:Label>
                                                <asp:TextBox ID="txt_Refference" runat="server" TabIndex="8" Width="100%"></asp:TextBox>

                                            </div>

                                            <div class="col-md-2">
                                                <asp:RadioButtonList ID="rdl_SalesInvoice" runat="server" TabIndex="9" RepeatDirection="Horizontal" onchange="return selectValue();" Width="120px">
                                                    <asp:ListItem Text="Material Issue" Value="SIN"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%--   <asp:Label ID="lbl_invoice_No" runat="server" Text="Sale Invoice"></asp:Label>--%>

                                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="10" ClientInstanceName="gridquotationLookup"
                                                                OnDataBinding="lookup_quotation_DataBinding"
                                                                KeyFieldName="ComponentID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                                <Columns>
                                                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Number" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Date" Width="100" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="CustomerName" Visible="true" VisibleIndex="3" Caption="Customer Name" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="branch" Visible="true" VisibleIndex="4" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                    <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="5" Caption="Reference" Width="80" Settings-AutoFilterCondition="Contains" />

                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>

                                                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="false" />
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
                                                                <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />
                                                            </dxe:ASPxGridLookup>
                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                    <ClientSideEvents EndCallback="componentEndCallBack" />
                                                </dxe:ASPxCallbackPanel>

                                                <span id="MandatorysSCno" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor21_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
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
                                                            <dxe:ASPxGridView runat="server" KeyFieldName="ComponentDetailsID" ClientInstanceName="cgridproducts" ID="grid_Products"
                                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                                                SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                                                                Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                                                                OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating"
                                                                OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300"
                                                                Settings-VerticalScrollBarMode="Visible">

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
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Sales Invoice No">
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="gvColQuantity" Width="0" VisibleIndex="6">
                                                                        <PropertiesTextEdit>
                                                                            <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>

                                                                <SettingsDataSecurity AllowEdit="true" />
                                                                <ClientSideEvents EndCallback="grid_ProductsEndCallback" />
                                                            </dxe:ASPxGridView>
                                                            <div class="text-center" style="padding-top: 8px;">


                                                                <dxe:ASPxButton ID="Button13" ClientInstanceName="cbtn_Button13" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                                    <ClientSideEvents Click="function(s, e) {PerformCallToGridBind();}" />
                                                                </dxe:ASPxButton>


                                                            </div>
                                                        </dxe:PopupControlContentControl>
                                                    </ContentCollection>
                                                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                                                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                                                </dxe:ASPxPopupControl>
                                            </div>
                                            <div class="col-md-2">

                                                <asp:Label ID="lbl_InvoiceNO" runat="server" Text="Material Issue Date"></asp:Label>

                                                <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                                                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                        <PanelCollection>
                                                            <dxe:PanelContent runat="server">
                                                                <asp:HiddenField ID="hdnPlaceOfSupply" runat="server" />
                                                                <asp:TextBox ID="txt_InvoiceDate" runat="server" TabIndex="8" Width="100%" Enabled="false"></asp:TextBox>

                                                            </dxe:PanelContent>
                                                        </PanelCollection>
                                                        <ClientSideEvents EndCallback="componentDateEndCallBack" />
                                                    </dxe:ASPxCallbackPanel>
                                                </div>

                                            </div>

                                            <div class="col-md-2" style="display: none">
                                                <asp:Label ID="lbl_DueDate" runat="server" Text="Due Date"></asp:Label>
                                                <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_SaleInvoiceDue" TabIndex="12" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </div>

                                            <div class="col-md-1">

                                                <asp:Label ID="lbl_Currency" runat="server" Text="Currency"></asp:Label>

                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="13">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">

                                                <asp:Label ID="lbl_Rate" runat="server" Text="Exch Rate"></asp:Label>

                                                <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="14" Width="100%" Height="28px">
                                                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                    <%-- <ClientSideEvents LostFocus="ReBindGrid_Currency" GotFocus="function(s,e){ctxt_Rate.ShowDropDown();}" />--%>
                                                    <ClientSideEvents LostFocus="ReBindGrid_Currency" />
                                                </dxe:ASPxTextBox>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:Label ID="lbl_AmountAre" runat="server" Text="Amounts are"></asp:Label>
                                                <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="15" Width="100%">
                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                    <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" GotFocus="function(s,e){cddl_AmountAre.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                            </div>

                                            <div class="clear"></div>
                                            <div class="col-md-6">
                                                <asp:Label ID="ASPxLabel4" runat="server" Text="Reason For Received"></asp:Label>
                                                <span style="color: red">*</span>
                                                <asp:TextBox ID="txtReasonforChange" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" onblur="return blurOut()"></asp:TextBox>

                                                <span id="MandatoryReasonforChange" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Place Of Supply[GST]">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                                <dxe:ASPxComboBox ID="ddlPosGstSReturn" runat="server" ClientInstanceName="cddlPosGstSReturn" Width="100%" ValueField="System.String">
                                                    <%-- <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateSReturnPosGst(e)}" />--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-2 hide">
                                                <asp:Label ID="lblVatGstCst" runat="server" Text="Select GST"></asp:Label>
                                                <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" TabIndex="17" Width="100%">
                                                    <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" GotFocus="function(s,e){cddlVatGstCst.ShowDropDown();}" />
                                                </dxe:ASPxComboBox>
                                                <span id="Mandatorytaxcode" style="display: none" class="validclass">
                                                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            </div>
                                            <div class="col-md-2">
                                                <%--<label id="lblProject" runat="server">Project</label>--%>
                                                <asp:Label ID="lblProject" runat="server" Text="Project"></asp:Label>
                                                <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesReturn"
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
                                                    <ClientSideEvents GotFocus="clookup_project_GotFocus" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                                                    <ClearButton DisplayMode="Always">
                                                    </ClearButton>
                                                </dxe:ASPxGridLookup>
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesReturn" runat="server" OnSelecting="EntityServerModeDataSalesReturn_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                            </div>
                                            <div class="col-md-4 lblmTop8">
                                                <%-- <label id="Label1" runat="server">Hierarchy</label>--%>
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="">
                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>
                                            <dxe:ASPxGridView runat="server" KeyFieldName="QuotationID" OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="170"
                                                Settings-HorizontalScrollBarMode="Visible">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="50px" VisibleIndex="0" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="50px">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Batch Product Popup Start--%>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="Product" Caption="Product" VisibleIndex="2" Width="200px" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductDisButnClick" KeyDown="ProductDisKeyDown" GotFocus="PsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Received" VisibleIndex="3" Width="200px" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>


                                                    <dxe:GridViewCommandColumn VisibleIndex="4" Caption="Addl. Desc." Width="100px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Additional Description">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="5" Width="100px" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="6" ReadOnly="true" Width="100px">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Caption="Warehouse"--%>
                                                    <dxe:GridViewCommandColumn VisibleIndex="7" Caption="Stk Details" Width="100px">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="8" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="SalePriceTextChange" GotFocus="SalePriceGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="9" Width="80px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" GotFocus="DiscountGotChange" />
                                                        </PropertiesSpinEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataSpinEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="10" ReadOnly="true" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..9999999999999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="AmtTextChange" GotFocus="AmtGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" ReadOnly="true" VisibleIndex="11" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>

                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="12" ReadOnly="true" Width="100px" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="13" Width="200px" ReadOnly="false">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">

                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="200px" VisibleIndex="14" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ComponentID" Caption="Invoice ID" ReadOnly="True" Width="0" VisibleIndex="15">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Batch Product Popup End--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" ReadOnly="True" VisibleIndex="16" Width="0" CellStyle-CssClass="hide">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="StockQuantity" VisibleIndex="17" Caption="Stock Qty" Width="0" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="18" ReadOnly="true" Width="0" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="TotalQty" Caption="Total Qty" VisibleIndex="19" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="BalanceQty" Caption="Balance Qty" VisibleIndex="20" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" VisibleIndex="21" ReadOnly="True" Width="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ComponentNumber" ReadOnly="true" VisibleIndex="22" Caption="Number" Width="0">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="IsInventory" Caption="hidden Field Id" ReadOnly="True" Width="0" VisibleIndex="24" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ProductDisID" Caption="hidden Field Id" ReadOnly="True" Width="0" VisibleIndex="25" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" ReadOnly="True" Width="0" VisibleIndex="30" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>

                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <br />


                                        <%--<div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                            <ul>
                                                <li class="clsbnrLblTotalQty">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
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
                                                                        <dxe:ASPxLabel ID="bnrLblTaxAmt" runat="server" Text="Tax Amt" ClientInstanceName="cbnrLblTaxAmt" />
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
                                                                        <dxe:ASPxLabel ID="bnrLblAmtWithTax" runat="server" Text="Amount With Tax" ClientInstanceName="cbnrLblAmtWithTax" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrlblAmountWithTaxValue" runat="server" Text="0.00" ClientInstanceName="cbnrlblAmountWithTaxValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>
                                                <li class="clsbnrLblLessOldVal">
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
                                                <li class="clsbnrLblLessAdvance" id="idclsbnrLblLessAdvance" runat="server">
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

                                                <li class="clsbnrLblInvVal" id="otherChargesId">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="cbnrOtherCharges" runat="server" Text="Other Charges" ClientInstanceName="cbnrOtherCharges" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrOtherChargesvalue" runat="server" Text="0.00" ClientInstanceName="cbnrOtherChargesvalue" />
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
                                                                        <dxe:ASPxLabel ID="bnrLblInvVal" runat="server" Text="Invoice Value" ClientInstanceName="cbnrLblInvVal" />
                                                                    </td>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="bnrLblInvValue" runat="server" Text="0.00" ClientInstanceName="cbnrLblInvValue" />
                                                                    </td>
                                                                </tr>

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </li>

                                            </ul>

                                        </div>--%>




                                        <div class="" id="divSubmitButton" runat="server">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>

                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>

                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <%--  Text="T&#818;axes"--%>
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />

                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />

                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SR" />
                                            <%--   <uc1:VehicleDetailsControl runat="server" id="VehicleDetailsControl" />--%>
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
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
            <asp:SqlDataSource ID="sqltaxDataSource" runat="server"
                SelectCommand="select Taxes_ID,Taxes_Name from dbo.Master_Taxes"></asp:SqlDataSource>

            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <%--Sudip--%>
            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Taxes" PopupHorizontalAlign="WindowCenter"
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
                                                                <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableGross"></dxe:ASPxLabel>
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
                                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="ASPxLabel" ClientInstanceName="clblChargesTaxableNet"></dxe:ASPxLabel>
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

                                <div class="col-md-8" id="ErrorMsgCharges" style="display: none;">
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
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
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
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" UseSubmitBehavior="false" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_tax_cancel" UseSubmitBehavior="false" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
                <dxe:ASPxPopupControl ID="Popup_Warehouse" runat="server" ClientInstanceName="cPopup_Warehouse"
                    Width="900px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ClientSideEvents Closing="function(s, e) {
	closeWarehouse(s, e);}" />
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <div class="Top clearfix">
                                <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Selected Product</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblProductName" runat="server"></asp:Label></td>
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
                                                            <td>Entered Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_SalesAmount" runat="server"></asp:Label>
                                                                <asp:Label ID="txt_SalesUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder" id="divpopupAvailableStock" style="display: none;">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Available Stock</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAvailableStock" runat="server"></asp:Label>
                                                                <asp:Label ID="lblAvailableStockUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li style="display: none;">
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Stock Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="txt_StockAmount" runat="server"></asp:Label>
                                                                <asp:Label ID="txt_StockUOM" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>

                                    </ul>
                                </div>

                                <div class="clear">
                                    <br />
                                </div>
                                <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div>
                                        <div class="col-md-3" id="div_Warehouse">
                                            <div style="margin-bottom: 5px;">
                                                Warehouse
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                                                    TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%" OnCallback="CmbWarehouse_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbWarehouse_ValueChange()}" EndCallback="CmbWarehouseEndCallback"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbWarehouse" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Batch">
                                            <div style="margin-bottom: 5px;">
                                                Batch/Lot
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="CmbBatch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBatch"
                                                    TextField="BatchName" ValueField="BatchID" runat="server" Width="100%" OnCallback="CmbBatch_Callback">
                                                    <ClientSideEvents ValueChanged="function(s,e){CmbBatch_ValueChange()}" EndCallback="CmbBatchEndCall"></ClientSideEvents>
                                                </dxe:ASPxComboBox>
                                                <span id="spnCmbBatch" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-4" id="div_Serial">
                                            <div style="margin-bottom: 5px;">
                                                Serial No &nbsp;&nbsp; (
                                                <input type="checkbox" id="myCheck" name="BarCode" onchange="AutoCalculateMandateOnChange(this)">Barcode )
                                            </div>
                                            <div class="" id="divMultipleCombo">

                                                <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="85%" CssClass="pull-left" runat="server" AnimationType="None">
                                                    <DropDownWindowStyle BackColor="#EDEDED" />
                                                    <DropDownWindowTemplate>
                                                        <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn" OnCallback="CmbSerial_Callback"
                                                            runat="server">
                                                            <Border BorderStyle="None" />
                                                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                                            <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" EndCallback="listBoxEndCall" />
                                                        </dxe:ASPxListBox>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="padding: 4px">
                                                                    <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                        <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DropDownWindowTemplate>
                                                    <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" GotFocus="function(s, e){ s.ShowDropDown(); }" />
                                                </dxe:ASPxDropDownEdit>
                                                <span id="spncheckComboBox" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                <div class="pull-left">
                                                    <i class="fa fa-commenting" id="abpl" aria-hidden="true" style="font-size: 16px; cursor: pointer; margin: 3px 0 0 5px;" title="Serial No " data-container="body" data-toggle="popover" data-placement="right" data-content=""></i>
                                                </div>
                                            </div>
                                            <div class="" id="divSingleCombo" style="display: none;">
                                                <dxe:ASPxTextBox ID="txtserial" runat="server" Width="85%" ClientInstanceName="ctxtserial" HorizontalAlign="Left" Font-Size="12px" MaxLength="49">
                                                    <ClientSideEvents LostFocus="txtserialTextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Quantity">
                                            <div style="margin-bottom: 2px;">
                                                Quantity
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <%-- <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />--%>
                                                    <ClientSideEvents TextChanged="function(s,e) { ChangePackingByQuantityinjs();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>


                                        <div class="col-md-3" id="div_AltQuantity">
                                            <div style="margin-bottom: 5px;">
                                                Alt. Qty
                                            </div>
                                            <div class="Left_Content" style="">
                                                <%--                  Rev Rajdip                         <dxe:ASPxTextBox ID="txtPacking"  runat="server" ClientSideEvents-GotFocus="QuantityGotFocus" ClientInstanceName="CtxtPacking"   HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">End Rev Rajdip--%>
                                                <dxe:ASPxTextBox ID="txtPacking" runat="server" ClientInstanceName="CtxtPacking" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                    <%--ClientSideEvents-GotFocus="QuantityGotFocus"--%>
                                                    <ValidationSettings Display="None"></ValidationSettings>
                                                    <ClientSideEvents TextChanged="function(s,e) { ChangeQuantityByPacking1();}" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="_div_Uom">
                                            <div style="margin-bottom: 5px;">
                                                Alt. UOM
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxComboBox ID="cmbPackingUom1" ClientInstanceName="ccmbPackingUom1" runat="server" SelectedIndex="0"
                                                    ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                    <%--EnableIncrementalFiltering="False"--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 14px">
                                                <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function(s, e) {if(!document.getElementById('myCheck').checked) SaveWarehouse();}" />
                                                </dxe:ASPxButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                    <dxe:ASPxGridView ID="GrdWarehouse" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
                                        Width="100%" ClientInstanceName="cGrdWarehouse" OnCustomCallback="GrdWarehouse_CustomCallback" OnDataBinding="GrdWarehouse_DataBinding"
                                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                                        <Columns>
                                            <dxe:GridViewDataTextColumn Caption="Warehouse Name" FieldName="WarehouseName"
                                                VisibleIndex="0">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Available Quantity" FieldName="AvailableQty" Visible="false"
                                                VisibleIndex="1">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="SalesQuantity"
                                                VisibleIndex="2">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Conversion Foctor" FieldName="ConversionMultiplier" Visible="false"
                                                VisibleIndex="3">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Stock Quantity" FieldName="StkQuantity" Visible="false"
                                                VisibleIndex="4">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Balance Stock" FieldName="BalancrStk" Visible="false"
                                                VisibleIndex="5">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Batch/Lot Number" FieldName="BatchNo"
                                                VisibleIndex="6">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Mfg Date" FieldName="MfgDate"
                                                VisibleIndex="7">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Expiry Date" FieldName="ExpiryDate"
                                                VisibleIndex="8">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNo"
                                                VisibleIndex="9">
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px">
                                                <DataItemTemplate>
                                                    <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Edit">
                                                        <img src="../../../assests/images/Edit.png" /></a>
                                                    &nbsp;
                                                        <a href="javascript:void(0);" id="ADelete" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
                                                            <img src="/assests/images/crs.png" /></a>
                                                </DataItemTemplate>
                                            </dxe:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents EndCallback="OnWarehouseEndCallback" />
                                        <SettingsPager Visible="false"></SettingsPager>
                                        <SettingsLoadingPanel Text="Please Wait..." />
                                    </dxe:ASPxGridView>
                                </div>
                                <div class="clearfix">
                                    <br />
                                    <div style="align-content: center">
                                        <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
            </div>
            <div>
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
                <%----hidden --%>

                <asp:HiddenField ID="hdfProductIDPC" runat="server" />
                <asp:HiddenField ID="hdfstockidPC" runat="server" />
                <asp:HiddenField ID="hdfopeningstockPC" runat="server" />
                <asp:HiddenField ID="hdbranchIDPC" runat="server" />
                <asp:HiddenField ID="hdnselectedbranch" runat="server" Value="0" />
                <asp:HiddenField ID="hdfComponentID" runat="server" />
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
                <%---- hidden--%>
                <asp:HiddenField ID="HdUpdateMainGrid" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hdnDeleteSrlNo" runat="server" />
                <%--Subhra--%>
                <asp:HiddenField ID="hdnInnumber" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />
                <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
                <%-- Surojit 12-03-2019 --%>
                <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
                <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
                <%-- Surojit 12-03-2019 --%>
                <%-- Surojit 20-05-2019 --%>
                <asp:HiddenField runat="server" ID="hdnPostingDateDisable" />
                <%-- Surojit 20-05-2019 --%>
                <asp:HiddenField runat="server" ID="hdnEntityType" />
                <asp:HiddenField runat="server" ID="hdnQty" />
                <asp:HiddenField ID="hdnADDEditMode" runat="server" />
                <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
                <%--Rev 1.0--%>
                <asp:HiddenField ID="hdnLockFromDate" runat="server" />
                <asp:HiddenField ID="hdnLockToDate" runat="server" />
                <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
                <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
                <asp:HiddenField ID="hdnValAfterLock" runat="server" />
                <asp:HiddenField ID="hdnValAfterLockMSG" runat="server" />
                <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
                <asp:HiddenField ID="hdnLockToDateedit" runat="server" /> 
                <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
                <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
                <%--End of Rev 1.0--%>
            </div>

            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>
            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>


            <%--End Sudip--%>

            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            <%--Debu Section--%>

            <%--Batch Product Popup Start--%>

            <dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name (4 Char)</strong></label>


                        <dxe:ASPxComboBox ID="productLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                            ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductLookUp" Width="92%"
                            OnItemsRequestedByFilterCondition="productLookUp_ItemsRequestedByFilterCondition"
                            OnItemRequestedByValue="productLookUp_ItemRequestedByValue" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True" FilterMinLength="4">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="ProductSelected" KeyDown="ProductlookUpKeyDown" GotFocus="function(s,e){cproductLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>


                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>

            <dxe:ASPxPopupControl ID="ProductpopUpdis" runat="server" ClientInstanceName="cProductpopUpdis"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By product Name (4 Char)</strong></label>


                        <dxe:ASPxComboBox ID="productDisLookUp" runat="server" EnableCallbackMode="true" CallbackPageSize="5"
                            ValueType="System.String" ValueField="Products_ID" ClientInstanceName="cproductDisLookUp" Width="92%"
                            OnItemsRequestedByFilterCondition="productDisLookUp_ItemsRequestedByFilterCondition"
                            OnItemRequestedByValue="productDisLookUp_ItemRequestedByValue" TextFormatString="{0}"
                            DropDownStyle="DropDown" DropDownRows="5" ItemStyle-Wrap="True" FilterMinLength="4">
                            <Columns>
                                <dxe:ListBoxColumn FieldName="Products_Name" Caption="Name" Width="400px" />
                                <dxe:ListBoxColumn FieldName="IsInventory" Caption="Inventory" Width="90px" />
                                <dxe:ListBoxColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100px" />
                                <dxe:ListBoxColumn FieldName="ClassCode" Caption="Class" Width="100px" />
                                <dxe:ListBoxColumn FieldName="BrandName" Caption="Brand" Width="100px" />
                                <dxe:ListBoxColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="100px" />
                            </Columns>
                            <ClientSideEvents ValueChanged="ProductDisSelected" KeyDown="ProductlookUpdisKeyDown" GotFocus="function(s,e){cproductDisLookUp.ShowDropDown();}" />

                        </dxe:ASPxComboBox>


                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>


            <%--Batch Product Popup End--%>


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
                        <div id="content-6">
                            <div class="col-sm-3">
                                <div class="lblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Gross Amount
                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableGross"></dxe:ASPxLabel>
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
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="(Taxable)" ClientInstanceName="clblTaxableNet"></dxe:ASPxLabel>
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
                        <div id="ContentErrorMsg" style="display: none;">
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
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>

                                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Amount" Caption="Amount" ReadOnly="true">
                                                <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                    <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                        <asp:Button ID="Button1" runat="server" Text="Ok" UseSubmitBehavior="false" TabIndex="5" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" />
                                        <asp:Button ID="Button2" runat="server" Text="Cancel" UseSubmitBehavior="false" TabIndex="5" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" />
                                    </div>
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                    runat="server" Width="100%" CssClass="pull-left mTop">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

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

            <%--debjyoti 22-12-2016--%>
            <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>

            <asp:HiddenField runat="server" ID="IsUdfpresent" />
            <asp:HiddenField runat="server" ID="Keyval_internalId" />
            <%--End debjyoti 22-12-2016--%>
            <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <%--Debu Section End--%>
        </asp:Panel>
    </div>


    <script type="text/javascript">

        function Keypressevt() {

            if (event.keyCode == 13) {

                //run code for Ctrl+X -- ie, Save & Exit! 
                SaveWarehouse();
                return false;
            }
        }

        function Setenterfocuse(s) {
        }
        function changedqnty(s) {
            var qnty = s.GetText();
            var sum = $('#hdntotalqntyPC').val();

            sum = Number(Number(sum) + Number(qnty));
            //alert(sum);
            $('#<%=hdntotalqntyPC.ClientID %>').val(sum);

        }


        function Clraear() {
            ctxtbatch.SetValue("");
            ASPx.CalClearClick('txtmkgdate_DDD_C');
            ASPx.CalClearClick('txtexpirdate_DDD_C');
            $('#<%=hdnisoldupdate.ClientID %>').val("false");
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

            cGrdWarehousePC.PerformCallback('checkdataexist~' + strProductID + '~' + stockids + '~' + branchid + '~' + strProductName);

        }

        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SR&&KeyVal_InternalID=' + keyVal;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }

        function acbpCrpUdfEndCall(s, e) {
            if (cacbpCrpUdf.cpUDF) {
                if (cacbpCrpUdf.cpUDF == "true" && cacbpCrpUdf.cpTransport == "true") {
                    grid.UpdateEdit();
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                }
                else if (cacbpCrpUdf.cpUDF == "false") {
                    LoadingPanel.Hide();
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                }
                else {
                    LoadingPanel.Hide();
                    jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                    cacbpCrpUdf.cpUDF = null;
                    cacbpCrpUdf.cpTransport = null;
                }
            }
        }

    </script>


    <script>

        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
            setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

        }
        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
                setTimeout(function () { $("#txtCustSearch").focus(); }, 500);

            }
        }

        function Customerkeydown(e) {
            var OtherDetail = {}
            OtherDetail.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");

                callonServer("Services/Master.asmx/GetCustomer", OtherDetail, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function SetCustomer(Id, Name) {


            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);

                GetObjectID('hdnCustomerId').value = Id;
                GetObjectID('hdfLookupCustomer').value = Id;
                SetEntityType(Id);
                var OtherDetail = {};
                OtherDetail.CustomerID = Id;
                $.ajax({
                    type: "POST",
                    url: "ProjectIssueMaterialReturn.aspx/GetCustomerStateCode",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        StateCodeList = msg.d;
                    },
                    error: function (msg) {
                        jAlert('Please try again later');
                    }
                });



                $('.crossBtn').hide();
                //   page.GetTabByName('General').SetEnabled(false);
                $('#CustModel').modal('hide');
            }
            //PosGstId = "";
            // cddlPosGstSReturn.SetValue(PosGstId);
            var startDate = new Date();
            startDate = tstartdate.GetDate().format('yyyy/MM/dd');
            var branchid = $('#ddl_Branch').val();

            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {

                    if (r == true) {
                        gridquotationLookup.gridView.UnselectRows();
                        // var key = cCustomerComboBox.GetValue();
                        var key = $('#<%=hdnCustomerId.ClientID %>').val();
                        if (key != null && key != '') {

                            //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'CR');
                            SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                            //  GetPurchaseForGstValue();
                            GetObjectID('hdnCustomerId').value = key;
                            page.tabs[0].SetEnabled(true);
                            page.tabs[1].SetEnabled(true);


                            $('.dxeErrorCellSys').addClass('abc');
                            var startDate = new Date();
                            startDate = tstartdate.GetDate().format('yyyy/MM/dd');

                            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                                clearTransporter();
                            }


                            $('#<%=txt_InvoiceDate.ClientID %>').val('');
                            //  });

                            //   document.getElementById('popup_ok').focus();
                        }

                    } else {
                        // gridLookup.SetValue($('#<%=hdnCustomerId.ClientID %>').val());

                        ctxtCustName.SetValue($('#<%=hdnCustomerId.ClientID %>').val());
                        var customerid = $('#<%=hdnCustomerId.ClientID %>').val();
                        ctxtCustName.PerformCallback(customerid)
                    }
                });
            }
            else {
                var key = ctxtCustName.GetValue();

                if (key != null && key != '') {


                    $('#<%=txt_InvoiceDate.ClientID %>').val('');

                    SetPurchaseBillingShippingAddress($('#ddl_Branch').val());
                    setTimeout(function () {
                        //  GetPurchaseForGstValue();
                    }, 500);


                    GetObjectID('hdnCustomerId').value = key;
                    page.tabs[0].SetEnabled(true);
                    page.tabs[1].SetEnabled(true);


                    $('.dxeErrorCellSys').addClass('abc');

                    GetObjectID('hdnAddressDtl').value = '0';
                }

            }
            GlobalBillingShipping();



            page.SetActiveTabIndex(1);

            cContactPerson.Focus();
            clookup_Project.gridView.Refresh();


        }

        function blurOut() {
            grid.batchEditApi.StartEdit(0, 2);
        }

    </script>


    <script>
        function prodkeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");


                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetSalesReturnProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }


        function prodDiskeydown(e) {


            //Both-->B;Inventory Item-->Y;Capital Goods-->C
            // var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdDisSearch").val();
            //  OtherDetails.InventoryType = inventoryType;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");


                if ($("#txtProdDisSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetSalesReturnProduct", OtherDetails, "ProductDisTable", HeaderCaption, "ProdDisIndex", "SetDisProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdDisIndex=0]"))
                    $("input[ProdDisIndex=0]").focus();
            }
        }
    </script>

    <div style="display: none">
        <dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tenddate" TabIndex="4">
            <ClientSideEvents DateChanged="Enddate" />
        </dxe:ASPxDateEdit>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:SqlDataSource runat="server" ID="dsCustomer"
        SelectCommand="prc_CRMSalesReturn_Details" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Type="String" Name="Action" DefaultValue="PopulateCustomerDetail" />
        </SelectParameters>
    </asp:SqlDataSource>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmit1Button"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <%--  <asp:SqlDataSource ID="CustomerDataSource" runat="server"  />--%>


    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
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
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

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
    <!--Product Modal -->
    <asp:HiddenField ID="hdnbeforedate" runat="server" />
    <!--Product Modal Dis-->
    <div class="modal fade" id="ProductDisModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodDiskeydown(event)" id="txtProdDisSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductDisTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>

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
    <!--Product Modal Dis-->

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
                                <asp:Label ID="lblInlineRemarks" runat="server" Text="Remarks"></asp:Label>

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


</asp:Content>
