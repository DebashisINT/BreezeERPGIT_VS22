<%--/*********************************************************************************************************
 * Rev 1.0      Sanchita      V2.0.37               Tolerance feature required in Sales Order Module 
 *                                                  Refer: 25223  -- WORK REVERTED
   Rev 2.0      Pallab        V2.0.37               Add Sales Order page design modification
                                                    Refer: 25813
   Rev 3.0      Sanchita      V2.0.39   28/06/2023  Some of the issues are there in Sales Invoice regarding 
                                                    Multi UOM in EVAC - FOR ALL SALES ORDER. Refer: 26453
   Rev 4.0      Priti         V2.0.39   18/07/2023  Sales Ordedr product deletion issue solved
   Rev 5.0      Sanchita      V2.0.40   04/10/2023  0026868 : Few Fields required in the Quotation Entry Module for the Purpose of Quotation Print from ERP
                                                    New button "Other Condiion" to show instead of "Terms & Condition" Button 
                                                    if the settings "Show Other Condition" is set as "Yes"
   Rev 6.0      Sanchita      V2.0.40   06-10-2023  New Fields required in Sales Quotation - RFQ Number, RFQ Date, Project/Site
                                                    Mantis : 26871
 **********************************************************************************************************/--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrderAdd.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.SalesOrderAdd" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/OtherTermsAndCondition.ascx" TagPrefix="ucOTC" TagName="OtherTermsAndCondition" %>
<%--Rev 5.0--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/uctrlOtherCondition.ascx" TagPrefix="uc4" TagName="uctrlOtherCondition" %>
<%--End of Rev 5.0--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
<script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
<script src="JS/SearchPopupDatatable.js"></script>
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <link href="CSS/salesorderAdd.css" rel="stylesheet" />
    <script src="JS/SalesOrderAdd.js?v=6.5"></script>
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=2.1" type="text/javascript"></script>

    <%--<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>    --%>
    <%--<script type="text/javascript" src="http://maps.google.com/maps/api/js?key=AIzaSyBTuRBvZK4ebtCQAjltccZ7UFO_Luldnms&sensor=false&libraries=places&language=en-AU"></script>--%>
    <style>
        #grid_DXMainTable > tbody > tr > td:nth-child(22) {
            display: none;
        }
    </style>
    <script>
        $(document).ready(function () {
            //
            //$('#ddl_numberingScheme').focus();

            $('#divCrossActivity').on("click", function () {

                var url = '';
                url = "../CRMPhoneCallWithFrame.aspx?TransSale=" + <%=Session["salesid"]%> + "&Assigned=" + <%=Session["AssignedById"]%> + "&type=" + <%=Session["type"]%> + "&Cid=" + <%=Session["CusId"]%> + "&Pid=1";
                window.location.href = url;
            });


            var IsEditMode = '<%= Session["ActionType"]%>';

            if (IsEditMode.trim() != 'Add') {

                page.SetActiveTabIndex(0);
              //  page.tabs[1].SetEnabled(false);
                $("#openlink").css("display", "none");
            }
            var hddnCRmVal = $("#hddnCustIdFromCRM").val();
            var CustId = $("#hdnCustomerId").val();

            if (hddnCRmVal == "1") {


                page.SetActiveTabIndex(0);
                page.tabs[1].SetEnabled(false);

                pageheaderContent.style.display = "block";
                //Chinmoy Edited on 25-05-2018
                SetDefaultBillingShippingAddress(CustId);
                //LoadCustomerAddress(CustId, $('#ddl_Branch').val(), 'SO');

                var BranchId = $("#ddl_Branch").val();
                var AsOnDate = cPLSalesOrderDate.GetDate().format('yyyy-MM-dd');
                ctxtCustName.SetEnabled(true);
                //Ajax Started
                $.ajax({
                    type: "POST",
                    url: "SalesOrderAdd.aspx/GetCustomerOutStandingAmount",
                    data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustId, BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    // async:false,
                    success: function (msg) {

                        LoadingPanel.Hide();
                        OutStandingAmount = msg.d;
                        if (OutStandingAmount === "") {
                            $('#lblOutstanding').text('0.00');
                        }
                        else {
                            $('#lblOutstanding').text(OutStandingAmount);
                        }

                    }
                });

                //End

            }


            $('#ddl_Branch').change(function () {
                //clookup_Project.gridView.Refresh();
            });


            $('#ddl_numberingScheme').change(function () {
                //

                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];
                var BranchId = NoSchemeTypedtl.toString().split('~')[3];

                var fromdate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];

                var dt = new Date();

                cPLSalesOrderDate.SetDate(dt);

                if (dt < new Date(fromdate)) {
                    cPLSalesOrderDate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    cPLSalesOrderDate.SetDate(new Date(todate));
                }




                cPLSalesOrderDate.SetMinDate(new Date(fromdate));
                cPLSalesOrderDate.SetMaxDate(new Date(todate));



                if (NoSchemeType == '1') {
                    ctxt_SlOrderNo.SetText('Auto');
                    ctxt_SlOrderNo.SetEnabled(false);

                    var hddnCRmVal = $("#hddnCustIdFromCRM").val();
                    if (hddnCRmVal == "1") {
                        page.SetActiveTabIndex(1);
                        page.tabs[0].SetEnabled(false);
                    }



                    //   document.getElementById('txt_SlOrderNo').disabled = true;
                    cPLSalesOrderDate.Focus();
                }
                else if (NoSchemeType == '0') {
                    ctxt_SlOrderNo.SetText('');
                    ctxt_SlOrderNo.SetEnabled(true);
                    ctxt_SlOrderNo.GetInputElement().maxLength = quotelength;
                    ctxt_SlOrderNo.Focus();

                }
                else {
                    ctxt_SlOrderNo.SetText('');
                    ctxt_SlOrderNo.SetEnabled(false);
                    document.getElementById("ddl_numberingScheme").focus();

                }

                //Added On 09-01-2018
                if (grid.GetEditor('ProductID').GetText() != "") {
                    //ccmbGstCstVat.PerformCallback();
                    //ccmbGstCstVatcharge.PerformCallback();
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                }
                //End

                // Mantis Issue 25378
                if (ctxt_SlOrderNo.GetText() == "Auto") {
                    cPLSalesOrderDate.SetEnabled(false);
                }
                else {
                    cPLSalesOrderDate.SetEnabled(true);
                }
                // End of Mantis Issue 25378


                $("#ddl_Branch").val(BranchId);
                $("#ddl_Branch").prop("disabled", true);
                //gridLookup.SetText('');

                //clookup_Project.gridView.Refresh();
            });





            $('#ddl_Currency').change(function () {

                var CurrencyId = $(this).val();
                var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
                var Currency = ActiveCurrency.toString().split('~')[0];
                if (Currency != CurrencyId) {
                    if (ActiveCurrency != null) {
                        if (CurrencyId != '0') {
                            $.ajax({
                                type: "POST",
                                url: "SalesOrderAdd.aspx/GetCurrentConvertedRate",
                                data: "{'CurrencyId':'" + CurrencyId + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    var currentRate = msg.d;
                                    if (currentRate != null) {
                                        //$('#txt_Rate').text(currentRate);
                                        ctxtRate.SetValue(currentRate);
                                    }
                                    else {
                                        ctxt_Rate.SetValue('1');
                                    }
                                    ReBindGrid_Currency();
                                }
                            });
                        }
                        else {
                            ctxtRate.SetValue("1");
                            ReBindGrid_Currency();
                        }
                    }
                }
                else {
                    ctxtRate.SetValue("1");
                    ReBindGrid_Currency();
                }
            });
        });

        function OnEndCallback(s, e) {
            if (grid.cpLoadAddressFromQuote) {
                if (grid.cpLoadAddressFromQuote != "") {
                    cQuotationBillingShipping.PerformCallback('BindAddress~' + grid.cpLoadAddressFromQuote);
                    grid.cpLoadAddressFromQuote = null;
                }
            }
            LoadingPanel.Hide();


            var value = document.getElementById('hdnRefreshType').value;
            var IsFromActivity = document.getElementById('hdnIsFromActivity').value;
            //Rev work start 24.06.2022 mantise no:0024987 
            var seting = document.getElementById('hdnCoordinate').value;
            //Rev work close 24.06.2022 mantise no:0024987
            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                OnAddNewClick();
                jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                LoadingPanel.Hide();
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Cannot proceed. Please remove duplicate product from multiple line to save this entry.');
            }
            else if (grid.cpSaveSuccessOrFail == "checkAcurateTaxAmount") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Check GST Calculated for Item ' + grid.cpProductName + ' at line ' + grid.cpSerialNo);
                grid.cpSaveSuccessOrFail = '';
                grid.cpSerialNo = '';
                grid.cpProductName = '';
            }
            else if (grid.cpSaveSuccessOrFail == "BillingShippingNull") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert("Billing & Shipping is mandatory, please enter Billing & Shipping address and proceed");
            }
            else if (grid.cpSaveSuccessOrFail == "udfNotSaved") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () { OpenUdf(); });
            }
            else if (grid.cpIsDocIdExists == "NotExistsDocId") {
                grid.cpIsDocIdExists = null;
                jAlert('Tag Quotation not Exists');
            }
                //Mantis Issue number 0018841 start
            else if (grid.DocNumberExist == "DocNumberExist") {
                grid.DocNumberExist = null;
                jAlert('Document number already exists');
            }
            else if (grid.cpSaveSuccessOrFail == "ExceedQuantity") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');
            }
            else if (grid.cpSaveSuccessOrFail == "OverRatedTaggedQuantity") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Alredy Tagged with greater product quantity.Update The quantity and Try Again.');
            }
                //Mantis Issue number 0018841 End
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {

                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Product. Found');
            }
            else if (grid.cpSaveSuccessOrFail == "nullQuantity") {
                grid.batchEditApi.StartEdit(0, 2);
                ///LoadingPanel.Hide();
                jAlert('Please fill Quantity');
                grid.cpSaveSuccessOrFail = null;
                //OnAddNewClick();

            }

            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData") {

                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Please add Alt. Qty for SL No. " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                grid.cpSaveSuccessOrFail = "";
                grid.cpSaveSuccessOrFail = null;
                jAlert(msg);
                OnAddNewClick();
            }

            // Rev 3.0
            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData_QtyMismatch") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Please check Multi UOM details for SL No. not matching with outer grid " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            else if (grid.cpSaveSuccessOrFail == "checkMultiUOMData_NotFound") {
                OnAddNewClick();
                grid.cpSaveSuccessOrFail = null;
                var SrlNo = grid.cpcheckMultiUOMData;
                var msg = "Multi UOM details not given for SL No. " + SrlNo;
                grid.cpcheckMultiUOMData = null;
                jAlert(msg);
                grid.cpSaveSuccessOrFail = '';
            }
            // End of Rev 3.0

            else if (grid.cpProductNotExists == "Select Product First") {
                grid.batchEditApi.StartEdit(0, 1);
                if (grid.GetVisibleRowsOnPage() == 0) {
                    OnAddNewClick();
                    grid.GetEditor('ProductID').Focus();
                }
                jAlert('Select Product First');
                grid.cpProductNotExists = null;
            }
            else if (grid.cpSaveSuccessOrFail == "IsAvailableStock") {
                grid.cpSaveSuccessOrFail = null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Save as Available StockFor the particular Item is Zero(0).');
            }
            else if (grid.cpSaveSuccessOrFail == "checkWarehouse") {
                var SrlNo = grid.cpProductSrlIDCheck;
                grid.cpSaveSuccessOrFail = null;
                var msg = "Product Sales Quantity must be equal to Warehouse Quantity for SL No. " + SrlNo;
                OnAddNewClick();
                jAlert(msg);
            }
            else if (grid.cpSaveSuccessOrFail == "BlankCustomerNotSaved") {
                grid.cpSaveSuccessOrFail = null;
                var msg = "Customer can not be blank.";
                OnAddNewClick();
                jAlert(msg);
            }
            else if (grid.cpSaveSuccessOrFail == "errorInsert") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please try after sometime.');
            }
            else if (grid.cpSaveSuccessOrFail == "EmptyProject") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Please select project.');
            }
            else if (grid.cpSaveSuccessOrFail == "CrediDaysZero") {
                AddNewRow();
                grid.cpSaveSuccessOrFail = null;
                jAlert('Credit Days must be greater than Zero(0).');
            }
            else if (grid.cpSaveSuccessOrFail == "SalesManAgentMandatoryCheck") {
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.cpSaveSuccessOrFail = null;
                //Rev work start 24.06.2022 mantise no:0024987   
                //jAlert('No SalesMan/Agent selected.Please Select.');
                if (seting == "NO") {
                    jAlert('No SalesMan/Agent selected.Please Select.');
                }
                else {
                    jAlert("Coordinator is set as Mandatory. Please select a Coordinator.");
                }
                //Rev work close 24.06.2022 mantise no:0024987 
            }
            else if (grid.cpSaveSuccessOrFail == "MoreThanStock") {
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.cpSaveSuccessOrFail = null;
                var msg = "Product entered quantity more than stock quantity.Can not proceed.";
                jAlert(msg);
            }
            else if (grid.cpSaveSuccessOrFail == "ZeroStock") {
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                jAlert('Insufficient Avaialble Stock.Cannot proceed');
                grid.cpProductZeroStock = null;
            }
            else if (grid.cpPartyOrderDate == "PartyOrderDateMisMatch") {
                grid.cpPartyOrderDate = null;
                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                jAlert('Cannot proceed. Party Order Date must be less than Sale Order date. ');
            }
            else if (grid.cpSaveSuccessOrFail == "Dormant_Customer") {
                //grid.AddNewRow();
                //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                //var tbQuotation = grid.GetEditor("SrlNo");
                //tbQuotation.SetValue(noofvisiblerows);
                grid.cpSaveSuccessOrFail = null;
                jAlert('You have selected a "Dormant" Customer. Please change the Status of this Customer to "Active" to proceed further. ');
            }

            else if (grid.cpDormantCustomer == "DormantCustomer") {
                grid.cpDormantCustomer = null;
                //grid.AddNewRow();
                //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                //var tbQuotation = grid.GetEditor("SrlNo");
                //tbQuotation.SetValue(noofvisiblerows);
                ctxtCustName.Focus();
                jAlert('You have selected a "Dormant" Customer. Please change the Status of this Customer to "Active" to proceed further. ');
            }
            else {
                var SalesOrder_Number = grid.cpSalesOrderNo;
                var Order_Msg = "Sales Order No. " + SalesOrder_Number + " saved.";
                if (IsFromActivity == "Y" && value == "E") {
                    $('#hdnRefreshType').val('');
                    if (SalesOrder_Number != "") {



                        jAlert(Order_Msg, 'Alert Dialog: [SalesOrder]', function (r) {
                            if (r == true) {
                                if (grid.cpCRMSavedORNot == "crmOrderSaved") {//Subhabrata on 02-01-2018
                                    parent.EnabledSaveBtn();

                                    grid.cpCRMSavedORNot = null;
                            <%-- var url = '';
                            url = "../CRMPhoneCallWithFrame.aspx?TransSale=" + <%=Session["salesid"]%> + "&Assigned=" + <%=Session["AssignedById"]%> + "&type=" + <%=Session["type"]%> + "&Cid=" + <%=Session["CusId"]%> + "&Pid=1";
                            window.location.href = url;--%>
                                }
                                // }
                                //});

                                if ($("#hddnSaveOrExitButton").val() == 'Save_Exit') {
                                    var DocumentNo = grid.cpDocumentNo;
                                    jConfirm('Do you want to print ?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {

                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SO-Default~D&modulename=Sorder&id=" + DocumentNo, '_blank');
                                            grid.cpDocumentNo = null;
                                        }
                                    });

                                }
                            }
                        });
                        grid.cpSalesOrderNo = null;


                    }
                    else {
                        self.close();
                    }
                }
                else if (value == "E") {
                    $('#hdnRefreshType').val('');
                    //#region Sandip Section For Approval Section Start
                    if (grid.cpApproverStatus == "approve") {
                       // window.parent.PendingApproval();
                        window.parent.PendingApproval();
                        window.parent.popup.Hide();
                       
                        //window.onunload = function (e) {
                        //    /*opener.PendingApproval();*/
                        //    window.parent.PendingApproval();
                        //    window.parent.popup.Hide();
                        //}; 
                       
                    }
                    else if (grid.cpApproverStatus == "rejected") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
                    }
                    //#endregion Sandip Section For Approval Dtl Section End
                    //window.location.assign("SalesOrderList.aspx");
                    if (SalesOrder_Number != "") {

                        //jAlert(Order_Msg);
                        jAlert(Order_Msg, 'Alert Dialog: [SalesOrder]', function (r) {
                            //

                            if (r == true) {
                                //
                                grid.cpSalesOrderNo = null;                              window.location.assign("SalesOrderEntityList.aspx");


                            }
                        });

                    }
                    else {
                        window.location.assign("SalesOrderEntityList.aspx");
                    }
                }
                else if (value == "N") {
                    // window.location.assign("SalesOrderAdd.aspx?key=ADD");


                    if (SalesOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [SalesOrder]', function (r) {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("SalesOrderAdd.aspx?key=ADD");
                            }
                        });

                    }
                    else {
                        window.location.assign("SalesOrderAdd.aspx?key=ADD");
                    }
                }
                else {

                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {
                        //OnAddNewClick();

                        grid.batchEditApi.EndEdit();

                        //$('#ddl_numberingScheme').focus();

                        $('#ddlInventory').focus();
                        //document.getElementById("ddl_numberingScheme").focus();
                        document.getElementById("ddlInventory").focus();
                        $('#hdnPageStatus').val('');
                    }
                    else if (pageStatus == "update") {


                        //var i;
                        //var cnt = 1;
                        //var noofvisiblerows = grid.GetVisibleRowsOnPage();
                        //for (i = 1 ; i <= noofvisiblerows ; i++) {
                        //    var tbQuotation = grid.GetEditor("SrlNo");
                        //    tbQuotation.SetValue(i);
                        //    grid.StartEditRow(i);

                        //}
                        //OnAddNewClick();

                        grid.AddNewRow();
                        //grid.StartEditRow(0);
                        var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                        var tbQuotation = grid.GetEditor("SrlNo");
                        tbQuotation.SetValue(noofvisiblerows);

                        //grid.StartEditRow(0);
                        $('#hdnPageStatus').val('');
                    }
                    else if (pageStatus == "Quoteupdate") {
                        //OnAddGridNewClick();
                        grid.StartEditRow(0);
                        $('#hdnPageStatus').val('');
                    }
                    else if (pageStatus == "delete") {
                        $('#hdnPageStatus').val('');
                        //grid.batchEditApi.StartEdit(0, 2);
                        //Rev 4.0
                        //OnAddNewClick();
                         //Rev 4.0 End
                    }
                    else {
                        grid.StartEditRow(0);
                        $('#hdnPageStatus').val('');
                    }

                }

            }
    if (gridquotationLookup.GetValue() != null) {
        grid.GetEditor('Quotation_Num').SetEnabled(false);
        grid.GetEditor('ProductName').SetEnabled(false);
        grid.GetEditor('Description').SetEnabled(false);
    }
    else {
        if (grid.GetVisibleRowsOnPage() == 0) {
            OnAddNewClick();
            grid.GetEditor('Quotation_Num').SetEnabled(true);
            grid.GetEditor('ProductName').SetEnabled(true);
            grid.GetEditor('Description').SetEnabled(true);
        }

    }
    for (var i = 0; i < grid.GetVisibleRowsOnPage() ; i++) {
        grid.batchEditApi.StartEdit(i);
        //grid.batchEditApi.StartEdit(i, 6);
    }

    var key = cddl_AmountAre.GetValue();
    if (key == 3) {
        grid.GetEditor('TaxAmount').SetEnabled(false);
    }
    cProductsPopup.Hide();
            //Rev Rajdip For Running Total
    if (grid.cpRunningTotal != null && grid.cpRunningTotal != "" && grid.cpRunningTotal != "undefined") {
        var strRunnging = grid.cpRunningTotal;
        var TotalQty = strRunnging.split("~")[0].toString();
        var Amount = strRunnging.split("~")[1].toString();
        var TaxAmount = strRunnging.split("~")[2].toString();
        var AmountWithTaxValue = strRunnging.split("~")[3].toString();
        var TotalAmt = strRunnging.split("~")[4].toString();
        cbnrLblTotalQty.SetText(TotalQty);
        cbnrLblTaxableAmtval.SetText(Amount);
        cbnrLblTaxAmtval.SetText(TaxAmount);
        cbnrlblAmountWithTaxValue.SetText(AmountWithTaxValue);
        cbnrLblInvValue.SetText(TotalAmt);
    }
            //End Rev Rajdip For Running Total

}

function BindOrderProjectdata(OrderId, TagDocType) {
    // debugger;	
    var OtherDetail = {};

    OtherDetail.OrderId = OrderId;
    var checked = $("[id$='rdl_Salesquotation']").find(":checked").val();
    if (checked == "QN") {

        OtherDetail.TagDocType = "QN";
    }
    else if (checked == "SINQ") {
        OtherDetail.TagDocType = "SINQ";
    }


    if ((OrderId != null) && (OrderId != "")) {
        $.ajax({
            type: "POST",
            url: "SalesOrderAdd.aspx/SetProjectCode",
            data: JSON.stringify(OtherDetail),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var Code = msg.d;

                clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                clookup_Project.SetEnabled(false);


                if ($("[id$='rdl_Salesquotation']").find(":checked").val() == "SINQ" && msg.d.length > 0) {
                    if (msg.d[0].ProjectCode == "") {
                        clookup_Project.SetEnabled(true);
                    }
                    else if (msg.d[0].ProjectCode == null) {
                        clookup_Project.SetEnabled(true);
                    }
                    else if (msg.d[0].ProjectCode == undefined) {
                        clookup_Project.SetEnabled(true);
                    }
                    else {
                        clookup_Project.SetEnabled(false);
                    }
                }


            }
        });


        var projID = clookup_Project.GetValue();

        $.ajax({
            type: "POST",
            url: 'SalesOrderAdd.aspx/getHierarchyID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ProjID: projID }),
            success: function (msg) {
                var data = msg.d;
                $("#ddlHierarchy").val(data);
            }
        });

    }
}


function PerformCallToGridBind() {
    if (validateOrderwithAmountAre()) {
        if (gridquotationLookup.GetValue() != null) {
            grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
            cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
            $('#hdnPageStatus').val('Quoteupdate');
            cProductsPopup.Hide();
            AllowAddressShipToPartyState = false;
            //#### added by Samrat Roy for Transporter Control #############
            //#### added by Samrat Roy for Transporter Control #############
            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            // Rev 5.0
            //if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                // End of Rev 5.0
                var type = "";

                if ($("#rdl_Salesquotation").find(":checked").val() == "QN") {
                    type = "QO";
                }
                else {
                    type = "SINQ";
                }
                callTransporterControl(quote_Id[0], type);
            // Rev 5.0
            //}
            // End of Rev 5.0
            ///// #### End : Samrat Roy for Transporter Control : End #############
            //grid.Refresh();
            clookup_Project.SetEnabled(false);
            if (quote_Id.length > 0) {
                BindOrderProjectdata(quote_Id[0], $("#hdnTagDocType").val());
            }


            //#### added by Sayan Dutta for TC Control #############
            // Rev 5.0
            //if ($("#btn_TermsCondition").is(":visible")) {
            //    callTCControl(quote_Id, 'QO');
            //}

            if ($("#btn_OtherCondition").is(":visible")) {
                callOCControl(quote_Id, type);
            }
            else {
                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCControl(quote_Id, type);
                }
            }
            // End of Rev 5.0
            
            return false;
        }
        else {
            grid.PerformCallback('GridBlank');
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            $('input[type=radio]').prop('checked', false);
            gridquotationLookup.SetEnabled(false);
            return false;
        }
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

    <%--<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.7/angular.js" >  </script>  
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.7/angular-route.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.7/angular-resource.js"></script>--%>



    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
    </script>
    <%--Use for focus on UOM after press ok on UOM--%>
    <style>
        .wrapHolder#pageheaderContent {
            width: 920px;
        }

        #DivCost {
            min-width: 100px;
        }
    </style>

    

    <%--Rev 2.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    

        <style>
            #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_OADate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

            #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_OADate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_OADate_B-1 #dt_OADate_B-1Img
        {
            display: none;
        }

        .calendar-icon
        {
                right: 18px !important;
        }

        select#ddlInventory
        {
            -webkit-appearance: auto;
        }

        .simple-select::after
        {
            top: 26px !important;
            right: 13px !important;
        }

        .col-sm-3 , .col-md-3 , .col-md-2{
            margin-bottom: 5px;
        }

        #rdl_Salesquotation
        {
            margin-top: 10px;
        }
        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        #CustomerTableTbl.dynamicPopupTbl>tbody>tr>td
        {
            width: 33.33%;
        }

            @media only screen and (max-width: 1380px) and (min-width: 1300px)
            {

                .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
                    padding-right: 10px;
                    padding-left: 10px;
                }

                .simple-select::after
                {
                    right: 8px !important;
                }
                .calendar-icon {
                    right: 13px !important;
                }

                input[type="radio"], input[type="checkbox"] {
                    margin-right: 0px;
                }
            }
        </style>
    <%--Rev end 2.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Subhra Section Start--%>

   <%-- <script src="JS/SearchPopup.js"></script>--%>

    <%--Subhra Section End--%>

    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix">

        <h3 class="pull-left">
            <%--<asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>--%>
            <label>
                <asp:Literal ID="ltrTitle" Text="" runat="server"></asp:Literal>
            </label>
        </h3>


        <div id="pageheaderContent" class="scrollHorizontal pull-right reverse wrapHolder content horizontal-images" style="display: none;">
            <div class="Top clearfix">
                <ul>

                    <li>
                        <div class="lblHolder" id="divPacking" style="display: none">
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
                        <div class="lblHolder" id="divCustomerOutstanding">
                            <table>
                                <tr>
                                    <td>Debtors Balance</td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#" id="idOutstanding">
                                            <asp:Label ID="lblOutstanding" runat="server" ToolTip="Click here to show details."></asp:Label></a>

                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivSell" style="display: none;">
                            <table>
                                <tr>
                                    <td>Sell @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSell" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivMRP" style="display: none;">
                            <table>
                                <tr>
                                    <td>MRP @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMRP" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivPurchase" style="display: none;">
                            <table>
                                <tr>
                                    <td>Purchase @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPurchase" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="DivCost" style="display: none;">
                            <table>
                                <tr>
                                    <td>Cost @</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCost" runat="server" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divDues" style="display: none;">
                            <table>
                                <tr>
                                    <td>Receivable(Dues)</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTotalDues" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" style="display: none;">
                            <table>
                                <tr>
                                    <td>Available Stock</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAvailableSStk" runat="server" Text="0.0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
                <ul style="display: none;">
                    <li>
                        <div class="lblHolder" style="display: none;">
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
        <div id="divcross" runat="server" class="crossBtn"><a href="SalesOrderEntityList.aspx" id="lnkBack" runat="server"><i class="fa fa-times"></i></a></div>
        <div id="divCrossActivity" runat="server" class="crossBtn"><i class="fa fa-times"></i></div>

        <div id="Cross_CloseWindow" runat="server" class="crossBtn "><a href=""><i class="fa fa-times"></i></a></div>
        <%--<div id="pageheaderContent" class="pull-right wrapHolder content horizontal-images" style="display: none;">
            <ul>
                <li>
                    <div class="lblHolder">
                        <table>
                            <tr>
                                <td>Stock Quantity</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </li>
                <li>
                    <div class="lblHolder">
                        mTop
                        <table>
                            <tr>
                                <td>Stock UOM</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </li>

            </ul>
        </div>--%>
        <%-- <div class="crossBtn" id="divcross1" runat="server"><a href="SalesOrderList.aspx"><i class="fa fa-times"></i></a></div>--%>
    </div>
        <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="row">

                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label>
                                                <%--Inventory Item--%>
                                                <asp:Label ID="Label12" runat="server" Text="Inventory Item?" CssClass="newLbl"></asp:Label>
                                            </label>
                                            <div class="Left_Content">
                                                <asp:DropDownList ID="ddlInventory" runat="server" Width="100%" CssClass="backSelect" onchange="ddlInventory_OnChange()">
                                                    <asp:ListItem Value="Y">Inventory Items</asp:ListItem>
                                                    <asp:ListItem Value="N">Non-Inventory Items</asp:ListItem>
                                                    <asp:ListItem Value="C">Capital Goods</asp:ListItem>
                                                    <asp:ListItem Value="B">All Items</asp:ListItem>
                                                </asp:DropDownList>
                                                <%--<dxe:ASPxCallbackPanel runat="server" ID="IsInventotry" ClientInstanceName="cIsInventory" OnCallback="ComponentIsInventory_Callback">
                                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                        <dxe:ASPxComboBox ID="cmbIsInventory" ClientInstanceName="ccmbIsInventory" runat="server" 
                                                            ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True" SelectedIndex="0">
                                                             <Items>
                                                                <dxe:ListEditItem Text="Yes" Value="1" />
                                                                <dxe:ListEditItem Text="No" Value="0" />
                                                            </Items>
                               
                                                            <ClientSideEvents SelectedIndexChanged="isInventoryChanged" />
                                                        </dxe:ASPxComboBox>
                                                 </dxe:PanelContent>
                                                            </PanelCollection>

                                                        </dxe:ASPxCallbackPanel>--%>
                                            </div>
                                        </div>
                                        <%--Rev 2.0: "simple-select" class add --%>
                                        <div class="col-md-2 simple-select" id="ddl_Num" runat="server">

                                            <label>
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="150px" runat="server" Text="Numbering Scheme">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                            </asp:DropDownList>


                                        </div>

                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_SlOrderNo" runat="server" Text="Document No" Width="">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>

                                            <dxe:ASPxTextBox ID="txt_SlOrderNo" runat="server" ClientInstanceName="ctxt_SlOrderNo" Width="100%" MaxLength="30">
                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="MandatorySlOrderNo" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>

                                        </div>
                                        <%--Rev 2.0: "for-cust-icon" class add --%>
                                        <div class="col-md-2 for-cust-icon">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date" Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_PLSales" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cPLSalesOrderDate" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents DateChanged="DateCheck" />
                                                <ClientSideEvents GotFocus="function(s,e){cPLSalesOrderDate.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>

                                            <span id="MandatorySlDate" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor211_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>
                                            <span id="MandatoryEgSDate" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2114_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Sales Order date must not be prior date than quotation date" /></span>

                                            <%--Rev 2.0--%>
                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                            <%--Rev end 2.0--%>
                                        </div>
                                        <%--Rev 2.0: "simple-select" class add --%>
                                        <div class="col-md-2 simple-select">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                                <% if (rights.CanAdd)
                                                   { %>
                                                <a href="#" onclick="AddcustomerClick()" style="left: -12px; top: 20px;"><i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                                <% } %>
                                            </label>
                                            <dxe:ASPxCallbackPanel runat="server" ID="lookup_CustomerControlPanelMain1" ClientInstanceName="clookup_CustomerControlPanelMain1" OnCallback="lookup_CustomerControlPanelMain_Callback1">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName">
                                                            <Buttons>
                                                                <dxe:EditButton>
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                                        </dxe:ASPxButtonEdit>

                                                        <%--<dxe:ASPxComboBox ID="CustomerComboBox" runat="server"  EnableCallbackMode="true" CallbackPageSize="15"
                                                        ValueType="System.String" ValueField="cnt_internalid" ClientInstanceName="gridLookup" Width="92%"
                                                        OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                                        OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" TextFormatString="{1} [{0}]"
                                                            DropDownStyle="DropDown">
                                                        <Columns>
                                                            <dxe:ListBoxColumn FieldName="uniquename"  Caption="Unique ID"  Width="200px"/>
                                                            <dxe:ListBoxColumn FieldName="Name" Caption="Name"  Width="200px"/>
                                                            <dxe:ListBoxColumn FieldName="Billing" Caption="Billing Address"  Width="300px" />
                                                        </Columns> 
                                                            <ClientSideEvents ValueChanged="function(s, e) { GetContactPerson(e)}" />
                                                </dxe:ASPxComboBox>--%>


                                                        <%--<dxe:ASPxGridLookup ID="lookup_Customer" runat="server" ClientInstanceName="gridLookup"
                                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">

                                                            <Columns>


                                                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Unique Id" Width="150" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="150" Settings-AutoFilterCondition="Contains">
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="150">
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="150">
                                                                    <Settings AllowAutoFilter="False"></Settings>
                                                                </dxe:GridViewDataColumn>
                                                            </Columns>
                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                                <Templates>
                                                                    <StatusBar>
                                                                        <table class="OptionsTable" style="float: right">
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="False" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </StatusBar>
                                                                </Templates>

                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

                                                                

                                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                                            </GridViewProperties>
                                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                                            <ClientSideEvents GotFocus="function(s,e){gridLookup.ShowDropDown();}" />
                                                            <ClearButton DisplayMode="Auto">
                                                            </ClearButton>
                                                        </dxe:ASPxGridLookup>--%>
                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="CustomerCallBackPanelEndCallBack" />
                                            </dxe:ASPxCallbackPanel>
                                            <span id="MandatorysCustomer" style="display: none;" class="valid2">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" /></span>


                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-2  lblmTop8" id="DivSegment1" runat="server">
                                            <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="Segment1">
                                            </dxe:ASPxLabel>

                                            <dxe:ASPxButtonEdit ID="txtSegment1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment1" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){Segment1ButnClick();}" KeyDown="function(s,e){Segment1_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>

                                        </div>
                                        <div class="col-md-2  lblmTop8" id="DivSegment2" runat="server">
                                            <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Segment2">
                                            </dxe:ASPxLabel>                                            
                                            <dxe:ASPxButtonEdit ID="txtSegment2" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment2" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){Segment2ButnClick();}" KeyDown="function(s,e){Segment2_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>                                            
                                        </div>
                                        <div class="col-md-2  lblmTop8" id="DivSegment3" runat="server">
                                            <dxe:ASPxLabel ID="ASPxLabel15" runat="server" Text="Segment3">
                                            </dxe:ASPxLabel>
                                           
                                            <dxe:ASPxButtonEdit ID="txtSegment3" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment3" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){Segment3ButnClick();}" KeyDown="function(s,e){Segment3_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                          
                                        </div>
                                        <div class="col-md-2  lblmTop8" id="DivSegment4" runat="server">
                                            <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Segment4">
                                            </dxe:ASPxLabel>
                                            
                                            <dxe:ASPxButtonEdit ID="txtSegment4" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment4" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){Segment4ButnClick();}" KeyDown="function(s,e){Segment4_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                           
                                        </div>
                                        <div class="col-md-2  lblmTop8" id="DivSegment5" runat="server">
                                            <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Segment5">
                                            </dxe:ASPxLabel>
                                           
                                            <dxe:ASPxButtonEdit ID="txtSegment5" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment5" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){Segment5ButnClick();}" KeyDown="function(s,e){Segment5_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                           
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                                <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                            </dxe:ASPxComboBox>
                                            <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
                                                <a href="#" style="left: -12px; top: 20px;"><%--onclick="AddcustomerClick()"--%>

                                                    <i id="I1" runat="server" class="fa fa-trash" aria-hidden="true" onclick="Deletesalesman()"></i></a>
                                            </label>
                                            <%-- <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%">
                                            </asp:DropDownList>--%>

                                            <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesManAgent" Width="100%">

                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>

                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){SalesManButnClick();}" KeyDown="SalesManbtnKeyDown" />

                                            </dxe:ASPxButtonEdit>

                                            <%--<dxe:ASPxComboBox ID="SalesManComboBox" runat="server"  EnableCallbackMode="true" CallbackPageSize="15"
                                                        ValueType="System.String" ValueField="cnt_id" ClientInstanceName="cSalesManComboBox" Width="92%"
                                                        OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedBySalesMenFilterCondition_SQL"
                                                        OnItemRequestedByValue="ASPxComboBox_OnItemRequestedBySalesMenValue_SQL" TextFormatString="{0}"
                                                            DropDownStyle="DropDown"  >
                                                        <Columns>
                                                            
                                                            <dxe:ListBoxColumn FieldName="Name" Caption="Name"  Width="200px"/>
                                                        </Columns> 
                                                    <ClientSideEvents TextChanged="function(s, e) { GetSalesManAgent(e)}" />
                                                </dxe:ASPxComboBox>--%>
                                        </div>

                                        <div class="col-md-2 lblmTop8">
                                            <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Credit Days">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxTextBox ID="txtCreditDays" ClientInstanceName="ctxtCreditDays" runat="server" Width="100%">
                                                <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                                                <ClientSideEvents TextChanged="CreditDays_TextChanged" />
                                            </dxe:ASPxTextBox>
                                        </div>

                                        <div class="col-md-2 lblmTop8">
                                            <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxDateEdit ID="dt_SaleInvoiceDue" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cdt_SaleInvoiceDue" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents GotFocus="function(s,e){cdt_SaleInvoiceDue.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>
                                        </div>


                                        <div class="col-md-2 lblmTop8">
                                            <%--<label>
                                                <dxe:ASPxLabel ID="lbl_quotation_No" runat="server" Text="Quotation Number" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>--%>
                                            <%--  <dxe:ASPxComboBox ID="ddl_Quotation" runat="server" ClientInstanceName="cddl_Quotatione" TabIndex="12" Width="100%">
                        <clientsideevents selectedindexchanged="function(s, e) { QuotationNumberChanged();}" />
                    </dxe:ASPxComboBox>--%>
                                            <asp:RadioButtonList ID="rdl_Salesquotation" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();" Width="90%">
                                                <asp:ListItem Text="Quotation" Value="QN"></asp:ListItem>
                                                <asp:ListItem Text="Inquiry" Value="SINQ"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                                            OnDataBinding="lookup_quotation_DataBinding"
                                                            KeyFieldName="Quote_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                            <Columns>
                                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />


                                                                <dxe:GridViewDataColumn FieldName="Quote_Number" Visible="true" VisibleIndex="1" Caption="Number" Width="135" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Quote_Date" Visible="true" VisibleIndex="2" Caption="Date" Width="90" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="name" Visible="true" VisibleIndex="3" Caption="Customer" Width="150" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="RevisionNo" Visible="true" VisibleIndex="4" Caption="Revision No." Width="120" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="RevDate" Visible="true" VisibleIndex="5" Caption="Revision Date" Width="90" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>

                                                                <dxe:GridViewDataColumn FieldName="Branch" Visible="true" VisibleIndex="6" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="7" Caption="Reference" Width="110" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                            </Columns>
                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                <Templates>
                                                                    <StatusBar>
                                                                        <table class="OptionsTable" style="float: right">
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                                            <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" />
                                                            <ClientSideEvents GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />
                                                        </dxe:ASPxGridLookup>

                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="componentEndCallBack" />
                                            </dxe:ASPxCallbackPanel>
                                            <%--  <asp:DropDownList ID="ddl_Quotation_No" runat="server" Width="100%" TabIndex="1" >
                    </asp:DropDownList>--%>
                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Quotation_Date" ClientInstanceName="clbl_Quotation_Date" runat="server" Text="Quot./Inq. Date" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>

                                            <asp:Label ID="lbl_MultipleDate" runat="server" Text="Multiple Select Quotation Dates" Style="display: none"></asp:Label>

                                            <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxTextBox ID="dt_Quotation" runat="server" Width="100%" ClientEnabled="false" ClientInstanceName="cPLQADate">
                                                        </dxe:ASPxTextBox>

                                                        <dxe:ASPxDateEdit ID="dt_PLQuotation" runat="server" Enabled="false" Visible="false" EditFormat="Custom" ClientInstanceName="cPLOADate" Width="100%">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" ErrorText="Expiry date can not be shorter than Pl/Quote date.">
                                                                <RequiredField IsRequired="true" />
                                                            </ValidationSettings>

                                                            <ClientSideEvents DateChanged="function(s,e){SetDifference1();}"
                                                                Validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />
                                                        </dxe:ASPxDateEdit>
                                                    </dxe:PanelContent>
                                                </PanelCollection>

                                            </dxe:ASPxCallbackPanel>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_OANumber" runat="server" Text="Party Order No." Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>

                                            <dxe:ASPxTextBox ID="txt_OANumber" runat="server" Width="100%" MaxLength="50" ClientInstanceName="ctxt_OANumber">
                                            </dxe:ASPxTextBox>
                                        </div>

                                        <%--Rev 2.0: "for-cust-icon" class add --%>
                                        <div class="col-md-2 for-cust-icon">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_OADate" runat="server" Text="Party Order Date" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>

                                            <dxe:ASPxDateEdit ID="dt_OADate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cPLOADate" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <%-- <validationsettings causesvalidation="True" errordisplaymode="ImageWithTooltip" errortextposition="Right" errortext="Expiry date can not be shorter than Pl/Quote date.">
                            <RequiredField IsRequired="true" />
                        </validationsettings>--%>

                                                <%-- <clientsideevents datechanged="function(s,e){SetDifference1();}"
                            validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />--%>
                                            </dxe:ASPxDateEdit>
                                            <%--Rev 2.0--%>
                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                            <%--Rev end 2.0--%>
                                        </div>
                                        <div class="col-md-2" style="display: none;">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Expiry" CssClass="inline"
                                                    Width="61px">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxDateEdit ID="dt_PlOrderExpiry" runat="server" Style="display: none;" ClientInstanceName="cExpiryDate" EditFormat="Custom" EditFormatString="dd-MM-yyyy" Width="100%">

                                                <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" ErrorText="Expiry date can not be shorter than Pl/Quote date.">
                                                    <RequiredField IsRequired="true" />
                                                </ValidationSettings>

                                                <ClientSideEvents DateChanged="function(s,e){SetDifference();}"
                                                    Validation="function(s,e){e.isValid = (CheckDifference()>=0)}" />
                                            </dxe:ASPxDateEdit>

                                        </div>






                                        <div class="col-md-2">
                                            <span style=" display: block">
                                                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <%--Rev work start  13.07.2022 mantise no:0025041: Reference column max limit will be 500 characters in Sales Order module--%>
                                            <%--<dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" MaxLength="50">--%>
                                            <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" MaxLength="500">
                                            <%--Rev work close  13.07.2022 mantise no:0025041: Reference column max limit will be 500 characters in Sales Order module--%>
                                            </dxe:ASPxTextBox>
                                        </div>


                                        <%--Rev 2.0: "simple-select" class add --%>
                                        <div class="col-md-1 simple-select">
                                            <label style=" display: block">Currency:  </label>
                                            <div>
                                                <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%"
                                                    DataSourceID="SqlCurrency" DataValueField="Currency_ID"
                                                    DataTextField="Currency_AlphaCode">
                                                </asp:DropDownList>
                                                <%-- <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                            DataSourceID="SqlCurrencyBind"
                            TextField="Currency_AlphaCode" ValueField="Currency_ID" SelectedIndex="0"
                            runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                            <clientsideevents valuechanged="function(s,e){Currency_Rate()}"></clientsideevents>
                        </dxe:ASPxComboBox>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <label style=" display: block">Exch. Rate:  </label>
                                            <div>
                                                <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate">
                                                </dxe:ASPxTextBox>
                                                <%-- <dxe:ASPxTextBox runat="server" ID="txt_Rate" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                            <masksettings mask="<0..9999>.<0..99999>" includeliterals="DecimalSymbol" />
                        </dxe:ASPxTextBox>--%>
                                            </div>
                                        </div>

                                        <%--  <div class="col-md-3">

                    <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                    </dxe:ASPxLabel>
                    <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="10">
                    </asp:DropDownList>


                </div>
                <div class="col-md-3">

                    <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Exchange Rate">
                    </dxe:ASPxLabel>


                    <dxe:ASPxTextBox ID="txt_Rate" runat="server" TabIndex="11" Width="100%" Enabled="false" Height="28px">
                    </dxe:ASPxTextBox>

                </div>--%>




                                        <div class="col-md-2">
                                            <span style=" display: block">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientInstanceName="cddl_AmountAre" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <%--<div class="col-md-2 lblmTop8" id="divposGst">--%>
                                        <div class="col-md-2" id="divposGst">
                                            <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                            </dxe:ASPxLabel>
                                            <span style="color: red">*</span>
                                            <dxe:ASPxComboBox ID="ddl_PosGstSalesOrder" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGstSalesOrder">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lblprojectValidfrom" runat="server" Text="Valid From" Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>

                                            </label>
                                            <dxe:ASPxDateEdit ID="dtProjValidFrom" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cdtProjValidFrom" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents DateChanged="ValidfromCheck" />
                                                <ClientSideEvents GotFocus="function(s,e){cdtProjValidFrom.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>

                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lblprojectValidUpto" runat="server" Text="Valid Up To" Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>

                                            </label>
                                            <dxe:ASPxDateEdit ID="dtProjValidUpto" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cdtProjValidUpto" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>

                                                <ClientSideEvents GotFocus="function(s,e){cdtProjValidUpto.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>

                                        </div>
                                        <div class="col-md-2 lblmTop8" id="dvRevision" style="display: none" runat="server">
                                            <label>
                                                <dxe:ASPxLabel ID="lblRevisionNo" runat="server" Text="Revision No." Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>

                                            </label>
                                            <dxe:ASPxTextBox ID="txtRevisionNo" runat="server" Width="100%" MaxLength="50" ClientInstanceName="ctxtRevisionNo">
                                                <%-- <ClientSideEvents LostFocus="Revision_LostFocus" />--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                        <div class="col-md-2 lblmTop8" id="dvRevisionDate" style="display: none" runat="server">
                                            <label>
                                                <dxe:ASPxLabel ID="lblRevisionDate" runat="server" Text="Revision Date" Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>

                                            </label>
                                            <dxe:ASPxDateEdit ID="txtRevisionDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="ctxtRevisionDate" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>

                                                <ClientSideEvents GotFocus="function(s,e){ctxtRevisionDate.ShowDropDown();}" />
                                            </dxe:ASPxDateEdit>
                                        </div>

                                        <div class="col-md-4" id="dvAppRejRemarks" style="display: none" runat="server">
                                            <asp:Label ID="lblAppRejRemarks" runat="server" Text="Approve/Reject Remarks"></asp:Label>

                                            <asp:TextBox ID="txtAppRejRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="2" Columns="8" Height="50px"></asp:TextBox>

                                        </div>

                                        <%--Rev 6.0--%>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-3" id="divRFQNumber" runat="server">
                                            <dxe:ASPxLabel ID="lblRFQNumber" runat="server" Text="RFQ Number">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxTextBox ID="txtRFQNumber" runat="server" ClientInstanceName="ctxtRFQNumber" Width="100%" PropertiesTextEdit-MaxLength="500" >
                                            </dxe:ASPxTextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-3 lblmTop8" id="divRFQDate" runat="server" >
                                                    <dxe:ASPxLabel ID="lblRFQDate" runat="server" Text="RFQ Date">
                                                    </dxe:ASPxLabel>
                                                    <dxe:ASPxDateEdit ID="dtRFQDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cdtRFQDate" Width="100%">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>

                                                        <ClientSideEvents GotFocus="function(s,e){cdtRFQDate.ShowDropDown();}" />
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                                <%--<div class="col-md-9 lblmTop8" id="divProjectSite" runat="server">--%>
                                                <div class="col-md-9" id="divProjectSite" runat="server">
                                                    <dxe:ASPxLabel ID="lblProjectSite" runat="server" Text="Project/Site">
                                                    </dxe:ASPxLabel>
                                                    <dxe:ASPxTextBox ID="txtProjectSite" runat="server" ClientInstanceName="ctxtProjectSite" Width="100%" PropertiesTextEdit-MaxLength="500">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--End of Rev 6.0--%>

                                        <div class="clear"></div>
                                        <div class="col-md-2">
                                            <label class="checkbox-inline" style="margin-top: 30px;">
                                                <asp:CheckBox ID="chkSendMail" runat="server"></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Send Email">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>


                                        </div>

                                        <div class="col-md-2 hide">
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" Width="100%">
                                                <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <div class="col-md-2 lblmTop8">
                                            <dxe:ASPxLabel ID="lblProject" runat="server" Text="Project"></dxe:ASPxLabel>
                                            <%--<label id="lblProject" runat="server">Project</label>--%>
                                            <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataSalesOrder"
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
                                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                        </div>
                                        <div class="col-md-4 lblmTop8">
                                            <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                        <%--Rev 1.0--%>
                                        <%--<div id="divQtyTolerance" runat="server" class="col-md-2 lblmTop8">
                                            <dxe:ASPxLabel ID="lblTolerance" runat="server" Text="Quantity Tolerance %">
                                            </dxe:ASPxLabel>
                                           <dxe:ASPxTextBox runat="server" ID="txtQtyTolerance" ClientInstanceName="ctxtQtyTolerance" Width="100%" CssClass="pull-left">
                                                <masksettings mask="<0..999>.<0..99>" includeliterals="DecimalSymbol" />
                                            </dxe:ASPxTextBox>
                                        </div>--%>
                                        <%--End of Rev 1.0--%>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-12">

                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>


                                            <dxe:ASPxGridView runat="server" KeyFieldName="OrderID"
                                                OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                OnHtmlRowCreated="grid_HtmlRowCreated"
                                                OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                                                Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords">
                                                <SettingsPager Visible="false"></SettingsPager>

                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False" />
                                                <Columns>
                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="0" Width="2%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomDelete" Image-Url="/assests/images/crs.png" Text=" ">
                                                                <Image Url="/assests/images/crs.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" ForeColor="White" Text=" ">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Sl" FieldName="SrlNo" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Quot./Inq." FieldName="Quotation_Num" ReadOnly="True" VisibleIndex="2" Width="5%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup Start--%>
                                                    <dxe:GridViewDataButtonEditColumn Caption="Product" FieldName="ProductName" VisibleIndex="3" Width="8%">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" GotFocus="ProductsGotFocusFromID" KeyDown="ProductKeyDown" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn Caption="hidden Field Id" EditCellStyle-CssClass="hide" FieldName="ProductID" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="hide" ReadOnly="True" VisibleIndex="23" Width="0">
                                                        <PropertiesTextEdit Height="15px">
                                                            <FocusedStyle CssClass="hide">
                                                            </FocusedStyle>
                                                            <Style CssClass="hide">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <EditCellStyle CssClass="hide">
                                                        </EditCellStyle>
                                                        <CellStyle CssClass="hide" Wrap="True">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--Batch Product Popup End--%><%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="3" Width="150">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName">
                                                            <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                    <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" ReadOnly="True" VisibleIndex="4" Width="11%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewCommandColumn Caption="Addl Desc." Width="4%" VisibleIndex="5">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomInlineRemarks" Image-ToolTip="Remarks" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                                <Image ToolTip="Addl Desc." Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>


                                                    <dxe:GridViewDataTextColumn Caption="Quantity" CellStyle-HorizontalAlign="Right" FieldName="Quantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="6" Width="5%">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="UOM(Sale)" FieldName="UOM" ReadOnly="true" VisibleIndex="7" Width="5%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewCommandColumn VisibleIndex="8" Caption="Multi UOM" Width="4%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <%--Mantis Issue 24425, 24428--%>
                                                    <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="9" Width="5%" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     
                                                    <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Order_AltUOM" ReadOnly="true" VisibleIndex="10" Width="5%" >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--VisibleIndex changed for below columns--%>
                                                    <%--End of Mantis Issue 24425, 24428--%>

                                                    <%--Caption="Warehouse"--%>
                                                    <dxe:GridViewCommandColumn Caption="Stk Details" VisibleIndex="11" Width="4%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomWarehouse" Image-ToolTip="Warehouse" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                                <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <%--Mantis Issue 25377 [ReadOnly="true" added ]--%>
                                                    <dxe:GridViewDataTextColumn Caption="Sale Price" CellStyle-HorizontalAlign="Right" FieldName="SalePrice" VisibleIndex="12" Width="7%" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="SalesPriceGotFocus" KeyDown="RateKeydown" TextChanged="SalePriceTextChange" LostFocus="spLostFocus" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn Caption="Add/Less Amt" FieldName="Discount" HeaderStyle-HorizontalAlign="Right" VisibleIndex="13" Width="4%">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" />
                                                            <ClientSideEvents GotFocus="DiscountGotFocus" LostFocus="DiscountTextChange" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Mantis Issue 25377 [ ReadOnly="true" added ]--%>
                                                    <dxe:GridViewDataTextColumn Caption="Amount" CellStyle-HorizontalAlign="Right" FieldName="Amount" VisibleIndex="14" Width="8%" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <ClientSideEvents LostFocus="ProductAmountTextChange" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataButtonEditColumn Caption="Charges" FieldName="TaxAmount" VisibleIndex="15" Width="7%" ReadOnly="true">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <%--LostFocus="Taxlostfocus"--%>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Net Amount" CellStyle-HorizontalAlign="Right" FieldName="TotalAmount" ReadOnly="true" VisibleIndex="16" Width="8%">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            <ClientSideEvents GotFocus="TotalAmountgotfocus" />
                                                            <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="ProjectRemarks" Width="12%" PropertiesTextEdit-MaxLength="5000" VisibleIndex="17">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">
                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="18" Width="2%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="AddNew" Image-Url="/assests/images/add.png" Text=" ">
                                                                <Image Url="/assests/images/add.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" EditCellStyle-CssClass="hide" EditFormCaptionStyle-CssClass="hide" FieldName="Quotation_No" FilterCellStyle-CssClass="hide" FooterCellStyle-CssClass="hide" HeaderStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" VisibleIndex="19" Width="0">
                                                        <PropertiesTextEdit NullTextStyle-CssClass="hide" ReadOnlyStyle-CssClass="hide" Style-CssClass="hide">
                                                            <NullTextStyle CssClass="hide">
                                                            </NullTextStyle>
                                                            <ReadOnlyStyle CssClass="hide">
                                                            </ReadOnlyStyle>
                                                            <Style CssClass="hide">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <EditCellStyle CssClass="hide">
                                                        </EditCellStyle>
                                                        <FilterCellStyle CssClass="hide">
                                                        </FilterCellStyle>
                                                        <EditFormCaptionStyle CssClass="hide">
                                                        </EditFormCaptionStyle>
                                                        <HeaderStyle CssClass="hide" />
                                                        <CellStyle CssClass="hide">
                                                        </CellStyle>
                                                        <FooterCellStyle CssClass="hide">
                                                        </FooterCellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Stock Qty" FieldName="StockQuantity" VisibleIndex="20" Width="0">
                                                        <PropertiesTextEdit DisplayFormatString="0.00">
                                                        </PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="Stock UOM" FieldName="StockUOM" ReadOnly="true" VisibleIndex="21" Width="0">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn Caption="QuoteDetailsId" FieldName="QuoteDetails_Id" ReadOnly="true" VisibleIndex="22" Width="0">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
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

                                            <%--<HeaderTemplate>
                                <img src="../../../assests/images/Add.png" />
                            </HeaderTemplate>--%>
                                            <%--<dxe:ASPxGridView runat="server" KeyFieldName="OrderID"
                        OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                        OnBatchUpdate="grid_BatchUpdate"
                        OnHtmlRowCreated="grid_HtmlRowCreated"
                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                        OnCustomCallback="grid_CustomCallback"
                        OnDataBinding="grid_DataBinding"
                        OnCellEditorInitialize="grid_CellEditorInitialize"
                        OnRowInserting="Grid_RowInserting"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                        >
                        <settingspager visible="false"></settingspager>
                        <settingsbehavior allowdragdrop="False" allowsort="False" />
                        <columns>
                            <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="0" Width="1%">
                                <custombuttons>
                                    <dxe:GridViewCommandColumnCustomButton ID="CustomDelete" Image-Url="/assests/images/crs.png" Text=" ">
                                        <image url="/assests/images/crs.png">
                                        </image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </custombuttons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Sl" FieldName="SrlNo" ReadOnly="true" VisibleIndex="1" Width="2%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="1" Width="10%">
                                <propertiescombobox clientinstancename="ProductID" textfield="ProductName" valuefield="ProductID">
                                    
                                    <clientsideevents selectedindexchanged="ProductsCombo_SelectedIndexChanged" />

                                </propertiescombobox>
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" ReadOnly="True" VisibleIndex="3">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" VisibleIndex="4" Width="6%">
                                <propertiestextedit>
                                    <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                    <clientsideevents lostfocus="QuantityTextChange" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="UOM(Sale)" FieldName="UOM" ReadOnly="true" VisibleIndex="5" Width="8%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn Caption="Warehouse" VisibleIndex="6" Width="1%">
                                <custombuttons>
                                    <dxe:GridViewCommandColumnCustomButton ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Text=" ">
                                        <image url="/assests/images/warehouse.png">
                                        </image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </custombuttons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Caption="Stock Qty" FieldName="StockQuantity" VisibleIndex="7" Width="6%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Stock UOM" FieldName="StockUOM" ReadOnly="true" VisibleIndex="8" Width="8%">
                                <propertiestextedit>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Sale Price" FieldName="SalePrice" ReadOnly="true" VisibleIndex="9" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Discount" FieldName="Discount" VisibleIndex="10" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" VisibleIndex="11" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                           
                            <dxe:GridViewDataButtonEditColumn Caption="TaxAmount" FieldName="TaxAmount" VisibleIndex="12" Width="6%">
                                <propertiesbuttonedit>
                                <clientsideevents buttonclick="taxAmtButnClick" gotfocus="taxAmtButnClick1" />
                                <buttons>
                                <dxe:EditButton Text="..." Width="20px">
                                        </dxe:EditButton>
                                </buttons>
                                </propertiesbuttonedit>
                            </dxe:GridViewDataButtonEditColumn>
                            <dxe:GridViewDataTextColumn Caption="Total Amount" FieldName="TotalAmount" VisibleIndex="13" Width="6%">
                                <propertiestextedit>
                                <masksettings allowmousewheel="false" mask="&lt;0..999999999999&gt;.&lt;0..99&gt;" />
                                <clientsideevents keydown="AddBatchNew"></clientsideevents>
                                </propertiestextedit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" FieldName="Quotation_No" HeaderStyle-CssClass="hide" VisibleIndex="14">
                                <propertiestextedit nulltextstyle-cssclass="hide" readonlystyle-cssclass="hide" style-cssclass="hide">
                                
                                <nulltextstyle cssclass="hide"></nulltextstyle>

                                <readonlystyle cssclass="hide"></readonlystyle>

                                    <style cssclass="hide"></style>

                                    </propertiestextedit>
                                <HeaderStyle CssClass="hide" />
                                <cellstyle cssclass="hide">
                                </cellstyle>
                            </dxe:GridViewDataTextColumn>
                        
                        </columns>
                     
                        <clientsideevents endcallback="OnEndCallback" custombuttonclick="OnCustomButtonClick" rowclick="GetVisibleIndex" />
                        <settingsdatasecurity allowedit="true" />
                        <settingsediting mode="Batch" newitemrowposition="Bottom">
                            <BatchEditSettings ShowConfirmOnLosingChanges="false"  EditMode="row" />
                        </settingsediting>
                    </dxe:ASPxGridView>--%>
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
                                                                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
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
                                                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
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
                                        <br />
                                        <div class="col-md-12" id="divSubmitButton" runat="server">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveNewRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btn_SaveExit" ClientInstanceName="cbtn_SaveExitRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--<asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>


                                            <span id="dvApprove" style="display: none" runat="server">
                                                <dxe:ASPxButton ID="btn_Approve" ClientInstanceName="cbtn_Approve" CssClass="btn btn-success" runat="server" AutoPostBack="False" Text="Approve" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Approve_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>
                                            <span id="dvReject" style="display: none" runat="server">
                                                <dxe:ASPxButton ID="btn_Reject" ClientInstanceName="cbtn_Reject" runat="server" CssClass="btn btn-danger" AutoPostBack="False" Text="Reject" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Reject_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>

                                            <%--Rev 3.0 [ClientInstanceName="cbtn_SaveRecords" changed to new specific name] --%>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecordsUDF" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>

                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecordsTax" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
                                            <a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary" style="display: none"><span><u>A</u>ttachment(s)</span></a>
                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                            <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                            <ucOTC:OtherTermsAndCondition runat="server" ID="OtherTermsAndCondition" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SO" />
                                            <asp:HiddenField runat="server" ID="hdBasketId" />
                                            <%--Rev 5.0--%>
                                            <uc4:uctrlOtherCondition runat="server" ID="uctrlOtherCondition" />
                                            <%--End of Rev 5.0--%>
                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />

                                            
                                            <asp:HiddenField runat="server" ID="hfOtherTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfOtherTermsConditionDocType" Value="SO" />
                                            
                                            <%--Rev 5.0--%>
                                            <asp:HiddenField runat="server" ID="hfOtherConditionData" />
                                            <asp:HiddenField runat="server" ID="hfOtherConditionDocType" Value="SO" />
                                            <%--End of Rev 5.0--%>

                                            <%-- onclick=""--%>


                                            <asp:HiddenField ID="hfControlData" runat="server" />
                                            <%--Rev 6.0--%>
                                            <asp:HiddenField runat="server" ID="hdnShowRFQ" />
                                            <asp:HiddenField runat="server" ID="hdnShowProject" />
                                            <%--End of Rev 6.0--%>


                                            <%--<dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>--%>
                                            <%--<a class="btn btn-primary" href="javascript:void(0);" onclick="fn_PopOpen()"><span><u>B</u>illing/Shipping</span> </a>--%>
                                        </div>
                                    </div>
                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>
                        <dxe:TabPage Name="[B]illing/Shipping" Text="Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">

                                    <%-- Date: 30-05-2017    Author: Kallol Samanta  [START] --%>
                                    <%-- Details: Billing/Shipping user control integration.   Old content deleted.--%>
                                    <dxe:ASPxCallbackPanel runat="server" ID="QuotationBillingShipping" ClientInstanceName="cQuotationBillingShipping" OnCallback="QuotationBillingShipping_Callback">
                                        <PanelCollection>
                                            <dxe:PanelContent runat="server">
                                                <ucBS:Sales_BillingShipping runat="server" ID="Sales_BillingShipping" />
                                            </dxe:PanelContent>
                                        </PanelCollection>
                                    </dxe:ASPxCallbackPanel>
                                    <%-- Date: 30-05-2017    Author: Kallol Samanta  [END] --%>
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


            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
            </dxe:ASPxPopupControl>
            <%--Customer Popup--%>
            <%--<dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
                Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <HeaderTemplate>
                    <span>Add Customer</span>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>--%>
            <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="650px"
                Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">

                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>

            <!--Customer Modal For sales Challan -->
            <div class="modal fade" id="CustModel" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Customer Search</h4>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />

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

            <%--SalesMan/Agent--%>
            <div class="modal fade" id="SalesManModel" role="dialog">
                <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <%--Rev work start 24.06.2022 mantise no:0024987 --%>
                            <%--<h4 class="modal-title">Salesman/Agent Search</h4>--%>
                            <h4 class="modal-title" runat="server" id="hs1">Salesman/Agent Search</h4>
                            <%--Rev work close 24.06.2022 mantise no:0024987--%>
                        </div>
                        <div class="modal-body">
                            <input type="text" onkeydown="SalesMankeydown(event)" id="txtSalesManSearch" autofocus width="100%" placeholder="Search By SalesMan/Agent Name" />

                            <div id="SalesManTable">
                                <table border='1' width="100%" class="dynamicPopupTbl">
                                    <tr class="HeaderStyle">
                                        <th class="hide">id</th>
                                        <th>Name</th>
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
                                        <th>Product Description</th>
                                        <th>HSN/SAC</th>
                                        <th>Inventory</th>
                                        <%-- <th>Brand</th>
                                        <th>Installation Required</th>--%>
                                        <th>Class</th>
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


            <%--Subhabrata Start Popup--%>
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
                        <dxe:ASPxGridView runat="server" KeyFieldName="Key_UniqueId" ClientInstanceName="cgridproducts" ID="grid_Products"
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback" OnDataBinding="grid_Products_DataBinding"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                            OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                            <%-- <Settings VerticalScrollableHeight="450" VerticalScrollBarMode="Auto"></Settings>--%>
                            <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " VisibleIndex="0" />
                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="45" ReadOnly="true" Caption="Sl. No.">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Product_Shortname" ReadOnly="true" Width="100" Caption="Product Name">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Width="200" ReadOnly="true" Caption="Product Description">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Quotation_No" ReadOnly="true" Caption="Quotation Id" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="QuoteDetails_Id" ReadOnly="true" Caption="Quotation_U" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="TaxAmountType" ReadOnly="true" Caption="TaxAmountType" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Quotation No">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                                    <Settings AutoFilterCondition="Contains" />
                                    <PropertiesTextEdit>
                                        <%--<ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />--%>
                                        <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                    </PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <%-- <ClientSideEvents EndCallback="OnEndProductCallback" />--%>
                            <%--  <SettingsPager Mode="ShowAllRecords"></SettingsPager>--%>
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsDataSecurity AllowEdit="true" />
                            <%--<SettingsEditing Mode="Batch">
                                            <BatchEditSettings EditMode="row" />
                                        </SettingsEditing>--%>
                            <%--<ClientSideEvents EndCallback=" cgridTax_EndCallBack " />--%>
                        </dxe:ASPxGridView>
                        <div class="text-center">
                            <asp:Button ID="Button2" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" UseSubmitBehavior="false" />
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <%-- End--%>
            <%--Outstanding Report--%>

            <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cOutstandingPopup"
                Width="1300px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <HeaderTemplate>
                    <strong><span style="color: #fff">Customer Outstanding</span></strong>

                    <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                        <ClientSideEvents Click="function(s, e){ 
                                                            cOutstandingPopup.Hide();
                                                        }" />
                    </dxe:ASPxImage>
                </HeaderTemplate>
                <ContentCollection>

                    <dxe:PopupControlContentControl runat="server">
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <dxe:ASPxGridView runat="server" KeyFieldName="SLNO" ClientInstanceName="cCustomerOutstanding" ID="CustomerOutstanding"
                            DataSourceID="EntityServerModeDataSource" OnSummaryDisplayText="ShowGridCustOut_SummaryDisplayText"
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cCustomerOutstanding_CustomCallback"
                            Settings-ShowFooter="true" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                            OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                            <SettingsBehavior AllowDragDrop="true" AllowSort="true"></SettingsBehavior>
                            <SettingsPager Visible="true"></SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="PARTYNAME" GroupIndex="0"
                                    VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="55%" ReadOnly="true" Caption="UNIT">
                                </dxe:GridViewDataTextColumn>
                                <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PARTYNAME" Width="100" ReadOnly="true" Caption="Customer">
                                </dxe:GridViewDataTextColumn>--%>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" ReadOnly="true" Caption="Doc. Type" Width="100%">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" ReadOnly="true" Caption="Opening?" Width="30%">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_NO" ReadOnly="true" Width="95%" Caption="Document No">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_DATE" Width="50%" ReadOnly="true" Caption="Document Date">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DUE_DATE" Width="50%" ReadOnly="true" Caption="Due Date">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOC_AMOUNT" ReadOnly="true" Caption="Document Amt." Width="50%">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BAL_AMOUNT" ReadOnly="true" Caption="Balance Amount" Width="50%">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DAYS" Width="20%" ReadOnly="true" Caption="Days">
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" AllowSort="False" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType" />
                                <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt"></dxe:ASPxSummaryItem>
                            </TotalSummary>

                            <SettingsDataSecurity AllowEdit="true" />
                            <ClientSideEvents EndCallback="OnEndCallbackOutstanding"></ClientSideEvents>
                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="PARTYOUTSTANDINGDET_REPORT" />
                        <div style="display: none">
                            <dxe:ASPxGridViewExporter ID="exporter1" GridViewID="CustomerOutstanding" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                            </dxe:ASPxGridViewExporter>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>

            <%--END--%>



            <%--Sudip--%>

            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <asp:HiddenField ID="hdnmodeId" runat="server" />

                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Order Taxes" PopupHorizontalAlign="WindowCenter"
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
                                                                <strong>
                                                                    <dxe:ASPxLabel ID="txtProductNetAmount" runat="server" Text="ASPxLabel" ClientInstanceName="ctxtProductNetAmount"></dxe:ASPxLabel>
                                                                </strong>
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
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    <ClientSideEvents LostFocus="PercentageTextChange" />
                                                    <ClientSideEvents />
                                                </PropertiesTextEdit>
                                                <CellStyle Wrap="False" HorizontalAlign="Right"></CellStyle>
                                            </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="2" Width="20%">
                                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                                <dxe:ASPxComboBox ID="cmbGstCstVatcharge" ClientInstanceName="ccmbGstCstVatcharge" runat="server" SelectedIndex="0"
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
                                                <dxe:ASPxTextBox ID="txtGstCstVatCharge" MaxLength="80" ClientInstanceName="ctxtGstCstVatCharge" ReadOnly="true" Text="0.00"
                                                    runat="server" Width="100%">
                                                    <MaskSettings Mask="&lt;-999999999..999999999g&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger" UseSubmitBehavior="false">
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
                                                        <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                    </dxe:ASPxTextBox>
                                                </div>

                                            </td>
                                            <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong></td>
                                            <td>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="100%" ClientInstanceName="ctxtTotalAmount" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                        <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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



                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
                <%-- kaushik 20-2-2017 --%>
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
                                <div id="content-6" class="pull-right wrapHolder reverse content horizontal-images" style="width: 100%; margin-right: 0px; height: auto;">
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
                                                    <tr>
                                                        <td>Available Stock</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                                                        </td>
                                                    </tr>
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
                                            <div class="lblHolder">
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
                                                <%--<dxe:ASPxComboBox ID="CmbSerial" EnableIncrementalFiltering="True" ClientInstanceName="cCmbSerial"
                                                    TextField="SerialName" ValueField="SerialID" runat="server" Width="100%" OnCallback="CmbSerial_Callback">
                                                </dxe:ASPxComboBox>--%>
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
                                                                    <dxe:ASPxButton ID="ASPxButton4" AutoPostBack="False" runat="server" Text="Close" Style="float: right" UseSubmitBehavior="false">
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
                                                <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="31px">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                                    <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 17px">
                                                <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
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
                                                    <a href="javascript:void(0);" onclick="fn_Edit('<%# Container.KeyValue %>')" title="Delete">
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
                                        <dxe:ASPxButton ID="btnWarehouseSave" ClientInstanceName="cbtnWarehouseSave" Width="50px" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="function(s, e) {FinalWarehouse();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <%-- kaushik 20-2-2017--%>
            </div>
            <div>
                <asp:HiddenField runat="server" ID="hddnSaveOrExitButton" />
                <asp:HiddenField ID="hddnDocumentIdTagged" runat="server" />
                <asp:HiddenField ID="hdnnproductIds" runat="server" />
                <asp:HiddenField ID="hdfIsDelete" runat="server" />
                <asp:HiddenField ID="hdfSerialDetails" runat="server" />
                <asp:HiddenField ID="hdfBatchDetails" runat="server" />
                <asp:HiddenField ID="hdfLookupCustomer" runat="server" />
                <asp:HiddenField ID="hddnOrderNumber" runat="server" />
                <asp:HiddenField ID="hdfProductID" runat="server" />
                <asp:HiddenField ID="hdfProductSerialID" runat="server" />
                <asp:HiddenField ID="hdnRefreshType" runat="server" />
                <asp:HiddenField ID="hdfProductType" runat="server" />
                <asp:HiddenField ID="hdnProductQuantity" runat="server" />
                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnCustomerId" runat="server" />
                <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />
                <asp:HiddenField ID="hdnAddressDtl" runat="server" />
                <asp:HiddenField ID="hdnPageStatus" runat="server" />
                <asp:HiddenField ID="hddnActionFieldOnStockBlock" runat="server" />
                <asp:HiddenField ID="hdnSchemaLength" runat="server" />
                <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
                <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
                <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
                <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
                <asp:HiddenField runat="server" ID="hddnCustIdFromCRM" />
                <asp:HiddenField runat="server" ID="hdAddOrEdit" />
                <asp:HiddenField runat="server" ID="hddnBranchId" />
                <asp:HiddenField runat="server" ID="hddnAsOnDate" />
                <%--kaushik 24-2-2017 --%>
                <asp:HiddenField runat="server" ID="IsUdfpresent" />
                <asp:HiddenField ID="hdnIsFromActivity" runat="server" />
                <asp:HiddenField ID="IsDiscountPercentage" runat="server" />
                <asp:HiddenField runat="server" ID="hdnJsonProductTax" />
                <asp:HiddenField runat="server" ID="hdnConfigValueForTagging" />
                <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
                <asp:HiddenField runat="server" ID="hddnCustomers" />
                <asp:HiddenField runat="server" ID="uniqueId" />
                <asp:HiddenField runat="server" ID="hdnSalesIrderId" />
                 <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />
                <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
                <%--kaushik 24-2-2017--%>
                <%--Rev 3.0--%>
                <dxe:ASPxLoadingPanel ID="LoadingPanelMultiUOM" runat="server" ClientInstanceName="LoadingPanelMultiUOM" ContainerElementID="divMultiUOM"
                    Modal="True">
                </dxe:ASPxLoadingPanel>
                 <%--End of Rev 3.0--%>
            </div>
            <%--End Sudip--%>

            <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                <ClientSideEvents ControlsInitialized="AllControlInitilize" />
            </dxe:ASPxGlobalEvents>

            <%--kaushik 28-2-2017--%>
            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
            </dxe:ASPxCallbackPanel>
            <%--kaushik 28-2-2017--%>


            <%--Batch Product Popup Start--%>

            <%--<dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="700" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <headertemplate>
                    <span>Select Product(s)</span>
                </headertemplate>


                <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Product Name</strong></label>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " 
                           ClientSideEvents-QueryCloseUp="ProductSelected"
                            ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown" >
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
                </contentcollection>
                <headerstyle backcolor="Blue" font-bold="True" forecolor="White" />
            </dxe:ASPxPopupControl>--%>

            <%-- <asp:SqlDataSource runat="server" ID="ProductDataSource" 
                SelectCommand="prc_SalesCRM_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetailsChallan" />
                   
                    <asp:ControlParameter Name="IsInventory" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>--%>

            <%--Batch Product Popup End--%>


            <%--Debu Section--%>

            <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
                Width="850px" HeaderText="Tax & Charges" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
                <HeaderTemplate>
                    <span style="color: #fff"><strong>Select Tax</strong></span>
                    <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
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
                        <div>
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
                                    <dxe:ASPxTextBox ID="txtprodBasicAmt" MaxLength="80" ClientInstanceName="ctxtprodBasicAmt" ReadOnly="true"
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
                                            <%--<dxe:GridViewDataComboBoxColumn Caption="percentage" FieldName="TaxField" VisibleIndex="2">
                                                <PropertiesComboBox ClientInstanceName="cTaxes_Name" ValueField="Taxes_ID" TextField="Taxes_Name" DropDownStyle="DropDown">
                                                    <ClientSideEvents SelectedIndexChanged="cmbtaxCodeindexChange"
                                                        GotFocus="CmbtaxClick" />
                                                </PropertiesComboBox>
                                            </dxe:GridViewDataComboBoxColumn>--%>
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
                                                <dxe:ASPxComboBox ID="cmbGstCstVat" ClientInstanceName="ccmbGstCstVat" runat="server" SelectedIndex="-1"
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
                                                <dxe:ASPxTextBox ID="txtGstCstVat" MaxLength="80" ClientInstanceName="ctxtGstCstVat" ReadOnly="true" Text="0.00"
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
                                        <asp:Button ID="Button1" runat="server" Text="Ok" CssClass="btn btn-primary mTop" OnClientClick="return BatchUpdate();" Width="85px" UseSubmitBehavior="false" />
                                        <asp:Button ID="Button3" runat="server" Text="Cancel" CssClass="btn btn-danger mTop" Width="85px" OnClientClick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" UseSubmitBehavior="false" />
                                    </div>
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                    runat="server" Width="100%" CssClass="pull-left mTop">
                                                    <MaskSettings Mask="&lt;-999999999..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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

            <%--debjyoti 22-12-2016--%>
        </asp:Panel>
        <%--End debjyoti 22-12-2016--%>




        <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
        </dxe:ASPxCallbackPanel>

        <%--Debu Section End--%>
        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>
        <asp:HiddenField ID="hdnCurrenctId" runat="server" />
        <%--kaushik 24-2-2017--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    </div>
    </div>
    <div>
        <asp:HiddenField runat="server" ID="hdnIsDistanceCalculate" />
        <asp:HiddenField runat="server" ID="hdnTransCategory" />
        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
        <asp:HiddenField runat="server" ID="hdnGuid" />
        <asp:HiddenField runat="server" ID="hdnPlaceShiptoParty" />
    </div>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <dxe:ASPxLoadingPanel ID="LoadingPanelSubmitButton" runat="server" ClientInstanceName="LoadingPanelSubmitButton" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <!-- Modal -->
    <div id="LastRateModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Last 5 Rates of <span id="spnProduct"></span></h4>
                </div>
                <div class="modal-body">
                    <div>
                        <input type="text" class="hide" />
                        <table class="dynamicPopupTbl" style="width: 100%;">
                            <thead>
                                <tr class="HeaderStyle">
                                    <th>Order Number</th>
                                    <th>Order Date</th>
                                    <th>Customer</th>
                                    <th>Rate</th>

                                </tr>
                            </thead>
                            <tbody id="tbodyRate">
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="btnCloseRate" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnAmountareTax" />

    <%--  <asp:SqlDataSource ID="sqlQuationList" runat="server"  
        SelectCommand="prc_GetQuotationOnSalesOrder" 
        SelectCommandType="StoredProcedure" 
       >     
      <SelectParameters>
           <asp:Parameter Name="Status" Type="String"   />
          </SelectParameters>SqlCurrencyBind
    </asp:SqlDataSource>--%>

    <%--  <asp:SqlDataSource ID="sqlQuationList" runat="server" 
        SelectCommand="select ttq.Quote_Id,ttq.Quote_Number,IsNull(CONVERT(VARCHAR(10), ttq.Quote_Date, 103),'') as Quote_Date	 ,case when( tmc.cnt_middleName is null  or tmc.cnt_middleName='') then isnull(tmc.cnt_firstName,'')+' ' +isnull(tmc.cnt_LastName,'')+' ' else   isnull(tmc.cnt_firstName,'')+' '+ isnull(tmc.cnt_middleName,'')+' ' +isnull(tmc.cnt_LastName,'')+' ' end as name from tbl_trans_Quotation  ttq left join tbl_master_contact tmc on ttq.Customer_Id=tmc.cnt_internalId where ttq.Quote_Number is not null and ttq.Quote_Number <>' '"></asp:SqlDataSource>--%>
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
                <%--Rev 2.0 [ id="divMultiUOM" added ] --%>
                <div class="Top clearfix" id="divMultiUOM">



                    <div class="clearfix col-md-12" style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;">

                        <table class="eqTble">
                            <tr>
                                <%--Mantis Issue 24425, 24428--%>
                                <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" width="0">
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24425, 24428--%>
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Mantis Issue 24913--%>
                                                <%--<input type="text" style="width:80px" id="UOMQuantity" style="text-align: right;" maxlength="18" readonly="true" class="allownumericwithdecimal" />--%>
                                                <%--Rev 3.0--%>
                                                <%--<input type="text" style="width:105px" id="UOMQuantity" style="text-align: right;" maxlength="18" class="allownumericwithdecimal" onchange="CalcBaseRate()" />--%>
                                                <input type="text" style="width:105px" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal"  onfocusout="CalcBaseRate();" placeholder="0.0000"  />
                                                <%--End of Rev 3.0--%>
                                                <%--End of Mantis Issue 24913--%>
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
                                <%--Mantis Issue 24397--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <%--Rev 3.0--%>
                                            <%--<dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>--%>
                                             <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.00" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..99&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                            <%--End of Rev 3.0--%>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24397--%>
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
                                            <label style="white-space:nowrap">Alt. Quantity </label>
                                        </div>
                                        <div>
                                            <%--<input type="text" id="AltUOMQuantity" style="text-align:right;"  maxlength="18" class="allownumericwithdecimal"/> --%>
                                            <dxe:ASPxTextBox Width="80px" ID="AltUOMQuantity" runat="server" ClientInstanceName="cAltUOMQuantity" DisplayFormatString="0.0000" MaskSettings-Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right">
                                                <%--Mantis Issue 24397--%>
                                                <%--Rev 3.0--%>
                                                <%--<ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />--%>
                                                 <ClientSideEvents LostFocus="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Rev 3.0--%>
                                                <%--End of Mantis Issue 24397--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>
                                <%--Mantis Issue 24397--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <%--Rev 3.0--%>
                                            <%--<dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.00" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..99&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents LostFocus="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                            <%--End of Rev 3.0--%>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <%--Rev 3.0 [ class="mlableWh" added] --%>
                                        <div class="mlableWh" >
                                            <%--<label class="checkbox-inline mlableWh">
                                                <asp:CheckBox ID="chkUpdateRow" Checked="false" runat="server" ></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>--%>

                                            <%--Rev 3.0 [ class="mlableWh" removed --%>
                                            <label class="checkbox-inline ">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24397--%>
                                <%--Rev 3.0--%>
                                </tr>
                                <tr>
                                <%--Rev 3.0--%>
                                    <td style="padding-top: 14px;">
                                        <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                            <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveMultiUOM();}}" />
                                        </dxe:ASPxButton>
                                    </td>
                               <%--Rev 3.0--%>
                               </tr>
                               <%--End of Rev 3.0--%>
                            </tr>
                        </table>

                    </div>
                    <div class="clearfix">
                        <dxe:ASPxGridView ID="grid_MultiUOM" runat="server" KeyFieldName="AltUomId;AltQuantity" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgrid_MultiUOM" OnCustomCallback="MultiUOM_CustomCallback" OnDataBinding="MultiUOM_DataBinding"
                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsBehavior-AllowSort="false">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 24397--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24397--%>

                                <dxe:GridViewDataTextColumn Caption="Alt. UOM" FieldName="AltUOM"
                                    VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Alt. Quantity" FieldName="AltQuantity"
                                    VisibleIndex="4" HeaderStyle-HorizontalAlign="Right">
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="" FieldName="UomId" Width="0px"
                                    VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="" FieldName="AltUomId" Width="0px"
                                    VisibleIndex="6">
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 24397--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24397--%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                        <%--Mantis Issue 24425, 24428--%>
                                        <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                        <%--End of Mantis Issue 24425, 24428--%>
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
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                            <%--Rev 3.0--%>
                            <label id="lblInfoMsg" style="font-weight:bold; color:red; " > </label>
                            <%--End of Rev 3.0--%>
                        </div>
                        <br />
                        <asp:Label ID="lblUOMmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <asp:SqlDataSource ID="UomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:SqlDataSource ID="AltUomSelect" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM "></asp:SqlDataSource>

    <asp:HiddenField ID="hddnuomFactor" runat="server" />
    <asp:HiddenField ID="hddnMultiUOMSelection" runat="server" />
    <%-- Rev Rajdip For Customer Mapping With Sales Man --%>
    <dxe:ASPxPopupControl ID="ASPxPopupSalesman" runat="server" ClientInstanceName="cPopup_salesman"
        Width="200px" HeaderText="SalesMan List" PopupHorizontalAlign="WindowCenter"
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
                                                <label>Salesman</label>
                                            </div>
                                            <div>
                                                <dxe:ASPxComboBox ID="ddlsalesmanmapped" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cddlsalesmanmapped" Font-Size="12px">
                                                    <%-- <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />--%>
                                                </dxe:ASPxComboBox>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <dxe:ASPxButton ID="btnselectsalesman" ClientInstanceName="cbtn_btnselectsalesman" runat="server" AutoPostBack="False" Text="Map Salesman" UseSubmitBehavior="false" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function(s, e) {Set_MappedSalesMan();}" />

                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%-- End Rev Rajdip --%>

    <dxe:ASPxCallbackPanel runat="server" ID="callback_InlineRemarks" ClientInstanceName="ccallback_InlineRemarks" OnCallback="callback_InlineRemarks_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
                <dxe:ASPxPopupControl ID="Popup_InlineRemarks" runat="server" ClientInstanceName="cPopup_InlineRemarks"
                    Width="900px" HeaderText="Additional Description" PopupHorizontalAlign="WindowCenter"
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
                                <asp:Label ID="lblInlineRemarks" runat="server"></asp:Label>

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


    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField runat="server" ID="hdnEntityType" />
    <asp:HiddenField runat="server" ID="hdnQty" />
    <asp:HiddenField ID="hdnApprovalReqInq" runat="server" />
    <asp:HiddenField runat="server" ID="hdnApproveStatus" />
    <asp:HiddenField runat="server" ID="hdnPageStatForApprove" />
    <asp:HiddenField runat="server" ID="hdnEditOrderId" />
    <asp:HiddenField runat="server" ID="hdnUpperApproveReject" />
    <asp:HiddenField runat="server" ID="ProductMinPrice" />
    <asp:HiddenField runat="server" ID="ProductMaxPrice" />
    <asp:HiddenField runat="server" ID="hdnRateType" />
    <asp:HiddenField runat="server" ID="hdnPricingDetail" />
    <asp:HiddenField runat="server" ID="hdnSalesRateBuyRateChecking" />
    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />
     <asp:HiddenField runat="server" ID="hdnShowDeliverySchedule" />
    <asp:HiddenField runat="server" ID="hdnValueSegment1" />
    <asp:HiddenField runat="server" ID="hdnValueSegment2" />
    <asp:HiddenField runat="server" ID="hdnValueSegment3" />
    <asp:HiddenField runat="server" ID="hdnValueSegment4" />
    <asp:HiddenField runat="server" ID="hdnValueSegment5" />
   <%-- Rev work start 28.06.2022 Mantise no:24949--%>
    <asp:HiddenField ID="hdnSettings" runat="server"></asp:HiddenField>
    <%-- Rev work close 28.06.2022 Mantise no:24949--%>
    <%--Rev work start 24.06.2022 mantise no:0024987--%> 
    <asp:HiddenField runat="server" ID="hdnCoordinate" />
    <%--Rev work start 24.06.2022 mantise no:0024987--%> 
     <div class="modal fade" id="Segment1Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment1header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment1keydown(event)" id="txtSegment1Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment1Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>      
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
      <div class="modal fade" id="Segment2Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment2Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment2Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>      
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
      <div class="modal fade" id="Segment3Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment3Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment3keydown(event)" id="txtSegment3Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment3Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th> 
                                <th>Name</th>                     
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
      <div class="modal fade" id="Segment4Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment4Header">Segment4 Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment4keydown(event)" id="txtSegment4Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment4Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th> 
                                <th>Name</th>                             
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
      <div class="modal fade" id="Segment5Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment5Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment5keydown(event)" id="txtSegment5Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment5Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>                                 
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
</asp:Content>
