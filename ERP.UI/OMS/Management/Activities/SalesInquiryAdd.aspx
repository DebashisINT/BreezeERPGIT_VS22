<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                05-04-2023        2.0.37           Pallab              25846: Add Sales Inquiry module design modification
2.0                21-06-2023        2.0.38           Sanchita            Some of the issues are there in Sales Invoice regarding 
                                                                          Multi UOM in EVAC - FOR ALL SALES MODULES. Refer : 26403
3.0   Pallab        V2.0.38    16-06-2023     "Multi UOM Details" popup parameter alignment issue fix . Mantis : 26331
4.0                04-10-2023        2.0.40         Sanchita             0026868 : Few Fields required in the Quotation Entry Module for the Purpose of Quotation Print from ERP
                                                                          New button "Other Condiion" to show instead of "Terms & Condition" Button 
                                                                          if the settings "Show Other Condition" is set as "Yes"
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SalesInquiryAdd.aspx.cs" EnableEventValidation="false" Inherits="ERP.OMS.Management.Activities.SalesInquiryAdd" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>
<%--Rev 4.0--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/uctrlOtherCondition.ascx" TagPrefix="uc4" TagName="uctrlOtherCondition" %>
<%--End of Rev 4.0--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" rel="stylesheet" />
<script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>
<script src="JS/SearchPopupDatatable.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    <link href="CSS/SalesInquiryAdd.css" rel="stylesheet" />
  
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=1.0.0" type="text/javascript"></script>
    <script src="JS/SalesInquiry.js?v=4.26"></script>
    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
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
        function OnCustomButtonClick(s, e) {
           // debugger;
            if (e.buttonID == 'CustomDelete') {

                var SrlNo = grid.batchEditApi.GetCellValue(e.visibleIndex, 'SrlNo');
                grid.batchEditApi.EndEdit();

                var totalNetAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TotalAmount');
                var oldAmountWithTaxValue = parseFloat(cbnrlblAmountWithTaxValue.GetValue());
                var AfterdeleteoldAmountWithTaxValue = oldAmountWithTaxValue - parseFloat(totalNetAmount);
                cbnrlblAmountWithTaxValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
                //cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));

                var RowQuantity = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Quantity');
                var totalquantity = parseFloat(cbnrLblTotalQty.GetValue());
                var updatedquantity = (parseFloat(totalquantity) - parseFloat(RowQuantity));
                //cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));

                var rowTaxAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'TaxAmount');
                var totaltaxamt = parseFloat(cbnrLblTaxAmtval.GetValue());
                var updatedtaxamt = parseFloat(totaltaxamt) - parseFloat(rowTaxAmount);
                //cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));

                var rowAmount = grid.batchEditApi.GetCellValue(e.visibleIndex, 'Amount');
                var TotalAmt = parseFloat(cbnrLblTaxableAmtval.GetValue());
                var updatedAmt = parseFloat(TotalAmt) - parseFloat(rowAmount);
                //cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));
                cbnrLblInvValue.SetText(parseFloat(AfterdeleteoldAmountWithTaxValue).toFixed(2));
                cbnrLblTotalQty.SetText(DecimalRoundoff(updatedquantity, 4));
                cbnrLblTaxAmtval.SetText(DecimalRoundoff(updatedtaxamt, 4));
                cbnrLblTaxableAmtval.SetText(DecimalRoundoff(updatedAmt, 4));


                $('#hdnRefreshType').val('');
                $('#hdnDeleteSrlNo').val(SrlNo);
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                if (noofvisiblerows != "1") {
                    grid.DeleteRow(e.visibleIndex);

                    $('#hdfIsDelete').val('D');


                    grid.UpdateEdit();
                    grid.PerformCallback('Display');

                    $('#hdnPageStatus').val('delete');
                    //grid.batchEditApi.StartEdit(-1, 2);
                    //grid.batchEditApi.StartEdit(0, 2);
                }
            }
            else if (e.buttonID == 'AddNew') {
                var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                if (ProductIDValue != "") {
                    OnAddNewClick();
                }
                else {
                    grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                }


                //var index = e.visibleIndex;
                //grid.batchEditApi.StartEdit(e.visibleIndex, 6);
                //var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                //var ProductID = Productdetails.split("||@||")[0];
                //var UOMName = grid.GetEditor("UOM").GetValue();
                //var quantity = grid.GetEditor("Quantity").GetValue();
                //var StockUOM = Productdetails.split("||@||")[5];
                //if (StockUOM == "") {
                //    StockUOM = "0";al
                //}
                ////$("#AltUOMQuantity").val(parseFloat(0).toFixed(4));
                //cAltUOMQuantity.SetValue("0.0000");
                //if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
                //    if (StockUOM == "0") {
                //        jAlert("Main Unit Not Defined.");
                //    }
                //    else {
                //        if ($("#hddnMultiUOMSelection").val() == "1") {
                //            ccmbUOM.SetEnabled(false);
                //            var index = e.visibleIndex;
                //            grid.batchEditApi.StartEdit(e.visibleIndex, 6);
                //            //grid.batchEditApi.StartEdit(globalRowIndex);
                //            var Qnty = grid.GetEditor("Quantity").GetValue();
                //            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                //            var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[3];
                //            ccmbUOM.SetValue(UomId);
                //            $("#UOMQuantity").val(Qnty);
                //            cPopup_MultiUOM.Show();
                //            cgrid_MultiUOM.cpDuplicateAltUOM = "";
                //            AutoPopulateMultiUOM();

                //            cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo);
                //        }
                //    }
                //}
                //else {
                //    return;
                //}
            }

            else if (e.buttonID == "CustomInlineRemarks") {

                var index = e.visibleIndex;
                grid.batchEditApi.StartEdit(e.visibleIndex, 5);
                cPopup_InlineRemarks.Show();

                $("#txtInlineRemarks").val('');

                var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                if (ProductID != "") {
                    // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
                    ccallback_InlineRemarks.PerformCallback('DisplayRemarks' + '~' + SrlNo + '~' + '0');

                }
                else {
                    $("#txtInlineRemarks").val('');
                }
                //$("#txtInlineRemarks").focus();
                document.getElementById("txtInlineRemarks").focus();
            }
            else if (e.buttonID == 'CustomMultiUOM') {
                //debugger;
                var index = e.visibleIndex;
               grid.batchEditApi.StartEdit(e.visibleIndex, 6);
                var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                var ProductID = Productdetails.split("||@||")[0];
                hdProductID.value = ProductID;

                var UOMName = grid.GetEditor("UOM").GetValue();
                var quantity = grid.GetEditor("Quantity").GetValue();
                //var DetailsId = grid.GetEditor('DetailsId').GetText();
                var StockUOM = Productdetails.split("||@||")[5];
                if (StockUOM == "") {
                    StockUOM = "0";
                }
                cAltUOMQuantity.SetValue("0.0000");
                ///rev bapi
              //  if ((ProductID != "") && (UOMName != "") && (quantity != "0.0000")) {
                if ((ProductID != "") && (UOMName != "")) {
                    //end rev bapi
                    if (StockUOM == "0") {
                        jAlert("Main Unit Not Defined.");
                    }
                    else {
                        if ($("#hddnMultiUOMSelection").val() == "1") {
                            ccmbUOM.SetEnabled(false);
                            var index = e.visibleIndex;
                          //  grid.batchEditApi.StartEdit(e.visibleIndex, 6);
                            var Qnty = grid.GetEditor("Quantity").GetValue();
                            var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                            var UomId = grid.GetEditor('ProductID').GetText().split("||@||")[3];
                            ccmbUOM.SetValue(UomId);
                            // Mantis Issue 24428
                            //$("#UOMQuantity").val(Qnty);
                            $("#UOMQuantity").val("0.0000");
                            ccmbBaseRate.SetValue(0);
                            cAltUOMQuantity.SetValue(0);
                            ccmbAltRate.SetValue(0);
                            ccmbSecondUOM.SetValue("");
                            $("#chkUpdateRow").prop('checked', false);
                            $("#chkUpdateRow").removeAttr("checked");
                            // End of Mantis Issue 24428
                            // Rev 2.0
                            document.getElementById('lblInfoMsg').innerHTML = "";
                            cbtn_SaveRecords_N.SetVisible(false);
                            cbtn_SaveRecords_p.SetVisible(false);
                            // End of Rev 2.0
                            cPopup_MultiUOM.Show();
                            cgrid_MultiUOM.cpDuplicateAltUOM = "";
                            AutoPopulateMultiUOM();
                            //cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo + '~' + DetailsId);      
                            cgrid_MultiUOM.PerformCallback('MultiUOMDisPlay~' + SrlNo);
                        }
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
                var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

                if (inventoryType == "C" || inventoryType == "Y") {
                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
                    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

                    $("#spnCmbWarehouse").hide();
                    $("#spnCmbBatch").hide();
                    $("#spncheckComboBox").hide();
                    $("#spntxtQuantity").hide();

                    if (ProductID != "" && parseFloat(QuantityValue) != 0) {
                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var strDescription = SpliteDetails[1];
                        var strUOM = SpliteDetails[2];
                        var strStkUOM = SpliteDetails[4];
                        var strMultiplier = SpliteDetails[7];
                        var strProductName = strDescription;
                        //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                        var StkQuantityValue = QuantityValue * strMultiplier;
                        var Ptype = SpliteDetails[14];
                        $('#hdfProductType').val(Ptype);

                        document.getElementById("lblProductName").innerHTML = strProductName;
                        document.getElementById("txt_SalesAmount").innerHTML = QuantityValue;
                        document.getElementById("txt_SalesUOM").innerHTML = strUOM;
                        document.getElementById("txt_StockAmount").innerHTML = StkQuantityValue;
                        document.getElementById("txt_StockUOM").innerHTML = strStkUOM;

                        $('#hdfProductID').val(strProductID);
                        $('#hdfProductSerialID').val(SrlNo);
                        $('#hdfProductSerialID').val(SrlNo);
                        $('#hdnProductQuantity').val(QuantityValue);
                        cacpAvailableStock.PerformCallback(strProductID);

                        if (Ptype == "W") {
                            div_Warehouse.style.display = 'block';
                            div_Batch.style.display = 'none';
                            div_Serial.style.display = 'none';
                            div_Quantity.style.display = 'block';
                            cCmbWarehouse.PerformCallback('BindWarehouse');
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else if (Ptype == "B") {
                            div_Warehouse.style.display = 'none';
                            div_Batch.style.display = 'block';
                            div_Serial.style.display = 'none';
                            div_Quantity.style.display = 'block';
                            cCmbBatch.PerformCallback('BindBatch~' + "0");
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else if (Ptype == "S") {
                            div_Warehouse.style.display = 'none';
                            div_Batch.style.display = 'none';
                            div_Serial.style.display = 'block';
                            div_Quantity.style.display = 'none';
                            checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else if (Ptype == "WB") {
                            div_Warehouse.style.display = 'block';
                            div_Batch.style.display = 'block';
                            div_Serial.style.display = 'none';
                            div_Quantity.style.display = 'block';
                            cCmbWarehouse.PerformCallback('BindWarehouse');
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else if (Ptype == "WS") {
                            div_Warehouse.style.display = 'block';
                            div_Batch.style.display = 'none';
                            div_Serial.style.display = 'block';
                            div_Quantity.style.display = 'none';
                            cCmbWarehouse.PerformCallback('BindWarehouse');
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else if (Ptype == "WBS") {
                            div_Warehouse.style.display = 'block';
                            div_Batch.style.display = 'block';
                            div_Serial.style.display = 'block';
                            div_Quantity.style.display = 'none';
                            cCmbWarehouse.PerformCallback('BindWarehouse');
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else if (Ptype == "BS") {
                            div_Warehouse.style.display = 'none';
                            div_Batch.style.display = 'block';
                            div_Serial.style.display = 'block';
                            div_Quantity.style.display = 'none';
                            cCmbBatch.PerformCallback('BindBatch~' + "0");
                            cGrdWarehouse.PerformCallback('Display~' + SrlNo);

                            SelectedWarehouseID = "0";
                            cPopup_Warehouse.Show();
                        }
                        else {
                            //div_Warehouse.style.display = 'none';
                            //div_Batch.style.display = 'none';
                            //div_Serial.style.display = 'none';
                            //div_Quantity.style.display = 'none';

                            //$.confirm({
                            //    title: 'Confirm!',
                            //    type: 'blue',
                            //    content: 'No Warehouse or Batch or Serial is actived !',

                            //    buttons: {
                            //        formSubmit: {
                            //            text: 'Ok',
                            //            btnClass: 'btn-blue',
                            //            keys: ['esc'],
                            //            action: function () {
                            //                grid.batchEditApi.StartEdit(index, 5);
                            //            }
                            //        },
                            //    },
                            //});

                            jAlert("No Warehouse or Batch or Serial is actived !");
                        }
                    }
                    else if (ProductID != "" && parseFloat(QuantityValue) == 0) {
                        //$.confirm({
                        //    title: '',
                        //    type: 'blue',
                        //    content: 'Please enter Quantity !',

                        //    buttons: {
                        //        formSubmit: {
                        //            text: 'Ok',
                        //            btnClass: 'btn-blue',
                        //            keys: ['esc'],
                        //            action: function () {
                        //                grid.batchEditApi.StartEdit(index, 5);
                        //            }
                        //        },
                        //    },
                        //});

                        jAlert("Please enter Quantity !");
                    }
                }
                else {
                    jAlert("You have selected Non-Inventory Item, so You cannot updated Stock.");
                }
            }
        }


        // Mantis Issue 24428 
        function CalcBaseQty() {
             //debugger;

            //var PackingQtyAlt = Productdetails.split("||@||")[20];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
            //var PackingQty = Productdetails.split("||@||")[22];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
           // var PackingSaleUOM = Productdetails.split("||@||")[25];  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)

            // Rev 2.0
            LoadingPanelMultiUOM.Show();
            document.getElementById('lblInfoMsg').innerHTML = "";
            // End of Rev 2.0

            var Productdetails = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var PackingQtyAlt = 0;
            var PackingQty = 0;
            var PackingSaleUOM = 0;


            var ProductID = Productdetails.split("||@||")[0];


            $.ajax({
                type: "POST",
                url: "PurchaseIndent.aspx/AutoPopulateAltQuantity",
                data: JSON.stringify({ ProductID: ProductID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                // Rev 2.0
                async: false,
                // End of Rev 2.0
                success: function (msg) {

                    if (msg.d.length != 0) {
                        PackingQtyAlt = msg.d[0].packing_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                        PackingQty = msg.d[0].sProduct_quantity;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                        PackingSaleUOM = msg.d[0].AltUOMId;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
                    }
                    else {
                        PackingQtyAlt = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_quantity)
                        PackingQty = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.sProduct_quantity)
                        PackingSaleUOM = 0;  // Alternate UOM selected from Product Master (tbl_master_product_packingDetails.packing_saleUOM)
                    }

                    if (PackingQtyAlt == "") {
                        PackingQtyAlt = 0
                    }
                    if (PackingQty == "") {
                        PackingQty = 0
                    }

                    // if Base UOM of product is not same as the Alternate UOM selected from Product Master, then Calculation of Base Quantity will not happen
                    if (ccmbSecondUOM.GetValue() != PackingSaleUOM) {
                        PackingQtyAlt = 0;
                        PackingQty = 0;
                    }

                    var BaseQty = 0
                    if (PackingQtyAlt > 0) {
                        var ConvFact = PackingQty / PackingQtyAlt;
                        var altQty = cAltUOMQuantity.GetValue();

                        if (ConvFact > 0) {
                            var BaseQty = (altQty * ConvFact).toFixed(4);
                            $("#UOMQuantity").val(BaseQty);
                            // Rev 2.0
                            CalcBaseRate();
                            // End of Rev 2.0
                        }
                    }
                    else {
                        $("#UOMQuantity").val("0.0000");
                        // Rev 2.0
                        document.getElementById('lblInfoMsg').innerHTML = "Base Quantity will not get auto calculated since no UOM Conversion details given for the selected Alt. UOM for Product : " + grid.GetEditor('Description').GetText();
                        // End of Rev 2.0
                    }
                }
            });
            // End of Rev 2.0
            LoadingPanelMultiUOM.Hide();
            // End of Rev 2.0
          
        }

        function CalcBaseRate() {
            //debugger;
            var altQty = cAltUOMQuantity.GetValue();
            var altRate = ccmbAltRate.GetValue();
            var baseQty = $("#UOMQuantity").val();


            if (baseQty > 0) {
                var BaseRate = (altQty * altRate) / baseQty;
                ccmbBaseRate.SetValue(BaseRate);
            }
        }
        function setTwoNumberDecimal(event) {
            this.value = parseFloat(this.value).toFixed(2);
        }
        // End of Mantis Issue 24428 


        $(document).ready(function () {

            //Added by :Subhabrata on 03-07-2017
            var hddnCRmVal = $("#<%=hddnCustIdFromCRM.ClientID%>").val();
            var CustId = $("#<%=hdnCustomerId.ClientID%>").val();
            if (hddnCRmVal == "1") {
                SetDefaultBillingShippingAddress(CustId);
                //LoadCustomerAddress(CustId, $('#ddl_Branch').val(), 'QO');
            }
            //End

            var schemaid = $('#ddl_numberingScheme').val();
            if (schemaid != null) {
                if (schemaid == '') {
                    ctxt_PLQuoteNo.SetEnabled(false);
                }
            }
            $('#ddl_numberingScheme').change(function () {
                var NoSchemeTypedtl = $(this).val();
                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];

                var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

                //Cut Off  Valid from To Date Sudip

                var fromdate = NoSchemeTypedtl.toString().split('~')[5];
                var todate = NoSchemeTypedtl.toString().split('~')[6];
                // alert(fromdate + '   ' + todate);
                var dt = new Date();
                tstartdate.SetDate(dt);


                if (dt < new Date(fromdate)) {
                    tstartdate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    tstartdate.SetDate(new Date(todate));
                }
                tstartdate.SetMinDate(new Date(fromdate));
                tstartdate.SetMaxDate(new Date(todate));


                //Cut Off  Valid from To Date Sudip

                if (branchID != "") document.getElementById('ddl_Branch').value = branchID;

                //ctxt_PLQuoteNo.SetMaxLength(quotelength);
                if (NoSchemeType == '1') {
                    ctxt_PLQuoteNo.SetText('Auto');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(false);

                    tstartdate.Focus();
                }
                else if (NoSchemeType == '0') {
                    ctxt_PLQuoteNo.SetEnabled(true);
                    ctxt_PLQuoteNo.GetInputElement().maxLength = quotelength;
                    //ctxt_PLQuoteNo.SetClientEnabled(true);
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.Focus();

                }
                else {
                    ctxt_PLQuoteNo.SetText('');
                    ctxt_PLQuoteNo.SetEnabled(false);
                    //ctxt_PLQuoteNo.SetClientEnabled(true);
                }
            });

            $('#ddl_Currency').change(function () {
                PreviousCurrency = ctxt_Rate.GetValue();
                var CurrencyId = $(this).val();
                var ActiveCurrency = '<%=Session["LocalCurrency"]%>'
                var Currency = ActiveCurrency.toString().split('~')[0];
                if (CurrencyId == Currency) {
                    ctxt_Rate.SetValue("0.00");
                    ctxt_Rate.SetEnabled(false);
                }
                else if (Currency != CurrencyId) {
                    if (ActiveCurrency != null) {
                        if (CurrencyId != '0') {
                            $.ajax({
                                type: "POST",
                                url: "SalesQuotation.aspx/GetCurrentConvertedRate",
                                data: "{'CurrencyId':'" + CurrencyId + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    var currentRate = msg.d;
                                    if (currentRate != null) {
                                        //$('#txt_Rate').text(currentRate);
                                        ctxt_Rate.SetValue(currentRate);
                                        ctxt_Rate.SetEnabled(true);
                                    }
                                    else {
                                        ctxt_Rate.SetValue('1');
                                        ctxt_Rate.SetEnabled(true);
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
                    ctxt_Rate.SetEnabled(true);
                    ReBindGrid_Currency();
                }
            });
        });

    </script>

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

        select#ddlInventory
        {
            -webkit-appearance: auto;
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

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PlQuoteExpiry
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PlQuoteExpiry_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PlQuoteExpiry_B-1 #dt_PlQuoteExpiry_B-1Img
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
            top: 34px;
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

        .mtc-10
        {
            margin-top: 10px;
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

        .col-sm-3{
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

        #txtQuantity_ET
        {
            height: 30px;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            #Popup_MultiUOM_PW-1 , #Popup_Warehouse_PW-1 , #Popup_Taxes_PW-1 , #aspxTaxpopUp_PW-1 , #Popup_InlineRemarks_PW-1
            {
                position:fixed !important;
                left: 15% !important;
                top: 60px !important;
            }
        }

        /*Rev 3.0*/
        .mlableWh
        {
            width: 120px !important;
            padding-top: 25px !important;
        }

        .mlableWh .dxeBase_PlasticBlue , .mlableWh label
        {
            line-height: 13px !important;
        }
        /*End of Rev 3.0*/

        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Subhra Section Start--%>
    <%-- CloseAction="CloseButton"--%>
    <%--Subhra Section End--%>

    <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_wareHouse"
        Width="500px" HeaderText="Add/Modify products" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="row">
                    <div class="col-md-8">
                        <div class="col-md-6">
                            <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                <%--Code--%>
                                        Short Name (Unique)
                                       <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%><span style="color: red;"> *</span>

                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="txt_Selectedproduct" MaxLength="80" ClientInstanceName="ctxtPro_Code" TabIndex="1"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                Available Stock=
                                        <%--<asp:Label ID="LblName" runat="server" Text="Name" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content" style="">
                                <asp:Label ID="lbl_AvailableStock" runat="server" Text="" CssClass="newLbl"></asp:Label>
                            </div>
                        </div>
                        <div class="clear"></div>





                        <div class="col-md-12">
                            <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                <%--Code--%>
                                        Sale Qty
                                       <%-- <asp:Label ID="LblCode" runat="server" Text="Short Name (Unique)" CssClass="newLbl"></asp:Label>--%><span style="color: red;"> *</span>

                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="Popup_txt_SaleQty" MaxLength="80" ClientInstanceName="ctxtPro_Code" TabIndex="1"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="Popup_txt_SaleUom" MaxLength="80" ClientInstanceName="ctxtPro_Code" TabIndex="1"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="cityDiv" style="height: auto; margin-bottom: 5px;">
                                Stock Qty 
                                        <%--<asp:Label ID="LblName" runat="server" Text="Name" CssClass="newLbl"></asp:Label>--%>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="ASPxTextBox1" MaxLength="80" ClientInstanceName="ctxtPro_Code" TabIndex="1"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>
                            <div class="Left_Content">
                                <dxe:ASPxTextBox ID="ASPxTextBox2" MaxLength="80" ClientInstanceName="ctxtPro_Code" TabIndex="1"
                                    runat="server" Width="100%">
                                </dxe:ASPxTextBox>
                            </div>

                        </div>

                    </div>

                </div>



                <%-- </div>--%>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
        </h3>
        <div id="pageheaderContent" class="pull-right wrapHolder content horizontal-images" style="display: none;">
            <div class="Top clearfix">
                <ul>
                    <li>
                        <div class="lblHolder" id="divAvailableStk">
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
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href="SalesInquiry.aspx"><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn" visible="false"><a href="SalesInquiry.aspx"><i class="fa fa-times"></i></a></div>

        <div id="Cross_CloseWindow" runat="server" class="crossBtn"><a href="" onclick=""><i class="fa fa-times"></i></a></div>
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
                                        <div class="col-md-4 hide">
                                            <asp:RadioButtonList ID="Opttype" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rbl">
                                                <asp:ListItem Value="P" Selected="True">Proforma</asp:ListItem>
                                                <asp:ListItem Value="Q">Quotation</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server" Width="100%" TabIndex="1" onchange="ddlInventory_OnChange()">
                                                <asp:ListItem Text="Inventory Item" Value="Y" />
                                                <asp:ListItem Text="Non-Inventory Item" Value="N" />
                                                <asp:ListItem Text="Capital Goods" Value="C" />
                                                <asp:ListItem Text="All Item" Value="B" />
                                            </asp:DropDownList>
                                        </div>
                                        <%--Rev 1.0: "simple-select" class add --%>
                                        <div class="col-md-3  simple-select" id="divScheme" runat="server">
                                            <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="160px" runat="server" Text="Numbering Scheme">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No." Width="120px">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxTextBox ID="txt_PLQuoteNo" runat="server" ClientInstanceName="ctxt_PLQuoteNo" TabIndex="2" Width="100%">
                                                <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                            </dxe:ASPxTextBox>
                                            <span id="MandatorysQuoteno" style="display: none" class="validclass">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                            </span>
                                            <span id="duplicateQuoteno" style="display: none" class="validclass">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Duplicate number">
                                            </span>
                                        </div>
                                        <%--Rev 1.0: "for-cust-icon" class add --%>
                                        <div class="col-md-3 for-cust-icon">
                                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date" Width="120px" CssClass="inline">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" UseMaskBehavior="True">
                                                <ClientSideEvents DateChanged="Startdate" />
                                            </dxe:ASPxDateEdit>
                                            <span id="MandatorysDate" style="display: none" class="validclass">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            <%--Rev 1.0--%>
                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                            <%--Rev end 1.0--%>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Expiry" CssClass="inline"
                                                Width="61px">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxDateEdit ID="dt_PlQuoteExpiry" runat="server" Date="" Width="100%" EditFormatString="dd-MM-yyyy" ClientInstanceName="tenddate" TabIndex="4">
                                                <ClientSideEvents DateChanged="Enddate" />
                                            </dxe:ASPxDateEdit>
                                            <span id="MandatoryEDate" style="display: none" class="validclass">
                                                <img id="2gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            <span id="MandatoryEgSDate" style="display: none" class="validclass">
                                                <img id="2gridHistory_DXPEForm_efnew_DXEFL_DXEditor12_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc"
                                                    title="Expiry Date must be greater than or equal to Proformat Date."></span>
                                            <%--Rev 1.0--%>
                                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                            <%--Rev end 1.0--%>
                                        </div>
                                        <%--Rev 1.0: "simple-select" class add --%>
                                        <div class="col-md-3 simple-select">
                                            <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                                            </dxe:ASPxLabel>
                                            <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="5">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                                            </dxe:ASPxLabel>

                                            <a href="#" onclick="AddcustomerClick()" style="left: -12px; top: 20px;">
                                                <% if (PiQuotationRights.CanAdd)
                                                   { %>
                                                <i id="openlink" runat="server" class="fa fa-plus-circle" aria-hidden="true"></i>

                                                <% 
                                                   } 
                                                %>
                                            </a>
                                            <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" TabIndex="6" Width="100%">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                            <%--<dxe:ASPxCallbackPanel runat="server" ID="lookup_CustomerControlPanelMain" ClientInstanceName="clookup_CustomerControlPanelMain" OnCallback="lookup_CustomerControlPanelMain_Callback">
                                                <PanelCollection>
                                                    <dxe:PanelContent runat="server">
                                                        <dxe:ASPxGridLookup ID="lookup_Customer" runat="server" TabIndex="6" ClientInstanceName="gridLookup"
                                                            KeyFieldName="cnt_internalid" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <dxe:GridViewDataColumn FieldName="shortname" Visible="true" VisibleIndex="0" Caption="Short Name" Width="200px" Settings-AutoFilterCondition="Contains" />
                                                                <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                                    // <Settings AllowAutoFilter="False"></Settings>//
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="2" Caption="Type" Settings-AutoFilterCondition="Contains" Width="200px">
                                                                   //<Settings AllowAutoFilter="False"></Settings>//
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="cnt_internalid" Visible="false" VisibleIndex="3" Settings-AllowAutoFilter="False" Width="200px">
                                                                    <Settings AllowAutoFilter="False"></Settings>
                                                                </dxe:GridViewDataColumn>
                                                            </Columns>
                                                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                                <Templates>
                                                                    <StatusBar>
                                                                        <table class="OptionsTable" style="float: right">
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="False" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </StatusBar>
                                                                </Templates>
                                                                <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                                                <SettingsLoadingPanel Text="Please Wait..." />
                                                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                            </GridViewProperties>
                                                            <ClientSideEvents TextChanged="function(s, e) { GetContactPerson(e)}" />
                                                            <ClearButton DisplayMode="Auto">
                                                            </ClearButton>
                                                        </dxe:ASPxGridLookup>
                                                    </dxe:PanelContent>
                                                </PanelCollection>
                                            </dxe:ASPxCallbackPanel>--%>

                                            <span id="MandatorysCustomer" style="display: none" class="validclass">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                        </div>
                                        <div class="col-md-3">

                                            <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" TabIndex="7" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                            </dxe:ASPxComboBox>
                                            <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                            </dxe:ASPxLabel>
                                            <a href="#" style="left: -12px; top: 20px;"><%--onclick="AddcustomerClick()"--%>

                                                <i id="I1" runat="server" class="fa fa-trash" aria-hidden="true" onclick="Deletesalesman()"></i>


                                            </a>

                                            <%--  <asp:DropDownList ID="ddl_SalesAgent" runat="server" Width="100%" TabIndex="8">
                                            </asp:DropDownList>--%>
                                            <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesManAgent" TabIndex="8" Width="100%">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){SalesManButnClick();}" KeyDown="SalesManbtnKeyDown" />

                                            </dxe:ASPxButtonEdit>
                                        </div>
                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Ref. Inquiry">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxTextBox ID="txt_Refference" runat="server" ClientInstanceName="ctxt_Refference" TabIndex="9" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row">
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="col-md-6 lblmTop8 simple-select">
                                                    <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                                    </dxe:ASPxLabel>
                                                    <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="10">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-6 lblmTop8">
                                                    <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                                    </dxe:ASPxLabel>
                                                    <dxe:ASPxTextBox ID="txt_Rate" ClientInstanceName="ctxt_Rate" runat="server" TabIndex="11" Width="100%">
                                                        <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                                                        <ClientSideEvents LostFocus="ReBindGrid_CurrencyByRate" GotFocus="GetPreviousCurrency" />
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                            </dxe:ASPxLabel>

                                            <%--<asp:DropDownList ID="ddl_AmountAre" runat="server" TabIndex="12" Width="100%">
                                            </asp:DropDownList>--%>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" TabIndex="12" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                                <ClientSideEvents LostFocus="function(s, e) { SetFocusonDemand(e)}" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <div class="col-md-3 hide">
                                            <dxe:ASPxLabel ID="lblVatGstCst" runat="server" Text="Select GST">
                                            </dxe:ASPxLabel>
                                            <dxe:ASPxComboBox ID="ddl_VatGstCst" runat="server" ClientInstanceName="cddlVatGstCst" OnCallback="ddl_VatGstCst_Callback" SelectedIndex="0" TabIndex="13" Width="100%">
                                                <ClientSideEvents EndCallback="Onddl_VatGstCstEndCallback" />
                                            </dxe:ASPxComboBox>
                                            <span id="Mandatorytaxcode" style="display: none" class="validclass">
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                        </div>
                                        <div style="clear: both;"></div>

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
                                        <div class="col-md-2 lblmTop8">
                                            <label id="lblProject" runat="server">Project</label>
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
                                                    <dxe:GridViewDataColumn FieldName="Hierarchy_ID" Visible="true" VisibleIndex="5" Caption="Hierarchy_ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookup_Project_LostFocus" ValueChanged="ProjectValueChange" />

                                                <ClearButton DisplayMode="Always">
                                                </ClearButton>
                                            </dxe:ASPxGridLookup>
                                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                        </div>

                                        <div class="col-md-4 lblmTop8">
                                            <label>
                                                <dxe:ASPxLabel ID="lblHierarchy" runat="server" Text="Hierarchy">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <div>
                                                <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblRearks" runat="server" Text="Remarks"></asp:Label>

                                            <asp:TextBox ID="txtProjRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="3" Columns="10" Height="50px">
                                                    
                                            </asp:TextBox>

                                        </div>


                                        <div class="col-md-6" id="dvAppRejRemarks" style="display: none" runat="server">
                                            <asp:Label ID="lblAppRejRemarks" runat="server" Text="Approve/Reject Remarks"></asp:Label>

                                            <asp:TextBox ID="txtAppRejRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="2" Columns="8" Height="50px"></asp:TextBox>

                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-12">

                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>
                                            <%--<HeaderTemplate>
                                <img src="../../../assests/images/Add.png" />
                            </HeaderTemplate>--%>



                                            <dxe:ASPxGridView runat="server" KeyFieldName="QuotationID"
                                                OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" ID="grid"
                                                Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                                OnBatchUpdate="grid_BatchUpdate"
                                                OnCustomCallback="grid_CustomCallback"
                                                OnDataBinding="grid_DataBinding"
                                                OnCellEditorInitialize="grid_CellEditorInitialize"
                                                OnRowInserting="Grid_RowInserting"
                                                OnRowUpdating="Grid_RowUpdating"
                                                OnRowDeleting="Grid_RowDeleting"
                                                SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
                                                ViewStateMode="Disabled">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Columns>
                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" " HeaderStyle-HorizontalAlign="Center">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="#" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="1" Width="15%">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName" DropDownWidth="300">
                                                            <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                        <CellStyle Wrap="True"></CellStyle>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                    <%--CallbackPageSize="10" OnItemsRequestedByFilterCondition="ASPxComboBox_OnItemsRequestedByFilterCondition_SQL"
                                                            OnItemRequestedByValue="ASPxComboBox_OnItemRequestedByValue_SQL" EnableCallbackMode="true"--%>
                                                    <%-- <dxe:GridViewDataComboBoxColumn FieldName="ProductID" Caption="Product" VisibleIndex="1" Width="15%">
                                                        <PropertiesComboBox TextField="ProductName" ValueField="ProductID">
                                                        </PropertiesComboBox>
                                                        <EditItemTemplate>
                                                            <dxe:ASPxComboBox runat="server" Width="100%" EnableIncrementalFiltering="true" TextField="ProductName" ValueField="ProductID"
                                                                ClearButton-DisplayMode="Always" ID="CmbProduct" ClientInstanceName="cCmbProduct" EnableCallbackMode="true" 
                                                                AllowMouseWheel="false" OnInit="CmbProduct_Init">
                                                                <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                            </dxe:ASPxComboBox>
                                                        </EditItemTemplate>
                                                    </dxe:GridViewDataComboBoxColumn>--%>

                                                    <%--Batch Product Popup Start--%>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="2" Width="14%">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" GotFocus="ProductsGotFocusFromID" />
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                        </PropertiesButtonEdit>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="20" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                        <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>



                                                    <%--Batch Product Popup End--%>

                                                    <%--<dxe:GridViewDataTextColumn FieldName="QuoteDetails_PackingQty" Caption="Packing" VisibleIndex="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                         <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <%--       <dxe:GridViewDataTextColumn FieldName="SelectUOM" Caption="Select UOM" VisibleIndex="0" Visible="false" Width="18%">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="3" ReadOnly="True" Width="18%">
                                                        <CellStyle Wrap="True"></CellStyle>
                                                        <%--<PropertiesTextEdit>
                                                            <ClientSideEvents GotFocus="ProductsGotFocus" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>--%>
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewCommandColumn Caption="Addl Desc." Width="70" VisibleIndex="4">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomInlineRemarks" Image-ToolTip="Remarks" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                                <Image ToolTip="Addl Desc." Url="/assests/images/more.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" VisibleIndex="5" Width="6%" PropertiesTextEdit-MaxLength="14" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityLostFocus" TextChanged="QuantityTextChange" />
                                                            <ClientSideEvents />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM" VisibleIndex="6" ReadOnly="true" Width="6%">
                                                        <PropertiesTextEdit>
                                                            <ClientSideEvents LostFocus="UomLostFocus" />
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn VisibleIndex="7" Caption="Multi UOM" Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomMultiUOM" Image-Url="/assests/images/MultiUomIcon.png" Image-ToolTip="Multi UOM">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>

                                                      <%--  Manis 24428--%> 
                                                     <dxe:GridViewDataTextColumn Caption="Multi Qty" CellStyle-HorizontalAlign="Right" FieldName="Order_AltQuantity" PropertiesTextEdit-MaxLength="14" VisibleIndex="8" Width="5%" ReadOnly="true">
                                                        <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                            <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     
                                                    <dxe:GridViewDataTextColumn Caption="Multi Unit" FieldName="Order_AltUOM" ReadOnly="true" VisibleIndex="9" Width="5%" >
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                     <%--  Manis End 24428--%> 

                                                    <%--Caption="Warehouse"--%>


                                                    <dxe:GridViewCommandColumn VisibleIndex="10" Caption="Stk Details" Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Warehouse">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>






                                                    <dxe:GridViewDataTextColumn FieldName="StockQuantity" Caption="Stock Qty" VisibleIndex="11" Visible="false">
                                                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="StockUOM" Caption="Stock UOM" VisibleIndex="12" ReadOnly="true" Visible="false">
                                                        <PropertiesTextEdit>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SalePrice" Caption="Price" VisibleIndex="13" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents TextChanged="SalePriceTextChange" LostFocus="spLostFocus" GotFocus="ProductsGotFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%-- <dxe:GridViewDataTextColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="10" Width="6%">
                                                    <PropertiesTextEdit DisplayFormatString="0.00" MaxLength="6"></PropertiesTextEdit>
                                                    <PropertiesTextEdit>
                                                        <MaskSettings Mask="<0..999>.<00..99>" AllowMouseWheel="false"  />
                                                        <ClientSideEvents LostFocus="DiscountTextChange" />
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>--%>
                                                    <dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="14" Width="5%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6" Style-HorizontalAlign="Right">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" />
                                                        </PropertiesSpinEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataSpinEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Amount" VisibleIndex="15" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <ClientSideEvents LostFocus="ProductAmountTextChange" GotFocus="AmountTextFocus" />
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="TaxAmount" Caption="Tax Amount" VisibleIndex="11" Width="6%">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>  Caption="TaxAmount"--%>

                                                    <%--   <dxe:GridViewCommandColumn VisibleIndex="6" Caption="Tax" Width="6%">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text='<%#Eval("TaxAmount") %>' ID="btnTaxAmount"   >
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>--%>

                                                    <dxe:GridViewDataButtonEditColumn FieldName="TaxAmount" Caption="Charges" VisibleIndex="16" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesButtonEdit>
                                                            <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                            <%--LostFocus="Taxlostfocus"--%>
                                                            <Buttons>
                                                                <dxe:EditButton Text="..." Width="20px">
                                                                </dxe:EditButton>
                                                            </Buttons>
                                                            <%--<MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                                        </PropertiesButtonEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataButtonEditColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TotalAmount" Caption="Net Amount" VisibleIndex="17" Width="6%" ReadOnly="true" HeaderStyle-HorizontalAlign="Right">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                                        <PropertiesTextEdit>
                                                            <ClientSideEvents GotFocus="TotalAmountgotfocus" />
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                        </PropertiesTextEdit>
                                                        <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="ProjectRemarks" Width="150" PropertiesTextEdit-MaxLength="5000" VisibleIndex="18">
                                                        <PropertiesTextEdit Style-HorizontalAlign="Left">
                                                            <Style HorizontalAlign="Left">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="19" Caption=" ">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dxe:GridViewCommandColumn>
                                                </Columns>
                                                <%--BatchEditEndEditing="OnBatchEditEndEditing"--%>
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                                </SettingsEditing>
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
                                        </div>
                                        <%-- Rev Rajdip --%>
                                        <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 15px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0; margin-top: 15px;">
                                            <ul>
                                                <li class="clsbnrLblTotalQty">
                                                    <div class="horizontallblHolder">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Total Quantity" ClientInstanceName="cbnrLblTotalQty" />
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
                                        <div class="col-md-12" id="divSubmitButton">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <%--Rev 2.0--%>
                                            <%--<dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>--%>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords_N" runat="server" AutoPostBack="False" Text="S&#818;ave & New" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords_p" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--End of Rev 2.0--%>
                                            <%--   <asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>
                                            <%--  Text="T&#818;axes"--%>
                                            <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                            </dxe:ASPxButton>
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
                                            <%--  For Approve /Reject  Tanmoy--%>
                                            <%--   <span id="dvApprove" style="display: none">
                                                <dxe:ASPxButton ID="btn_Approve" ClientInstanceName="cbtn_Approve" CssClass="btn btn-success" runat="server" AutoPostBack="False" Text="Approve" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Approve_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>
                                            <span id="dvReject" style="display: none">
                                                <dxe:ASPxButton ID="btn_Reject" ClientInstanceName="cbtn_Reject" runat="server" CssClass="btn btn-danger" AutoPostBack="False" Text="Reject" UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {Reject_ButtonClick();}" />
                                                </dxe:ASPxButton>
                                            </span>--%>
                                            <%--  For Approve /Reject  Tanmoy--%>


                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />

                                            <asp:HiddenField ID="hfControlData" runat="server" />

                                            <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                                            <%--Rev 4.0--%>
                                            <uc4:uctrlOtherCondition runat="server" ID="uctrlOtherCondition" />
                                            <%--End of Rev 4.0--%>

                                            <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SINQ" />
                                            <asp:HiddenField runat="server" ID="hdBasketId" />
                                            <asp:HiddenField runat="server" ID="sessionBranch" />
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                            <asp:HiddenField ID="hdnUomqnty" runat="server" />
                                               <asp:HiddenField ID="hdProductID" runat="server" />
                                            <%--Rev 4.0--%>
                                            <asp:HiddenField runat="server" ID="hfOtherConditionData" />
                                            <asp:HiddenField runat="server" ID="hfOtherConditionDocType" Value="SINQ" />
                                            <%--End of Rev 4.0--%>
                                            <%--<asp:HiddenField ID="hdnCustomerId" runat="server" />--%>
                                            <%-- onclick=""--%>
                                            <%--<a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary"><span>[A]ttachment(s)</span></a>--%>
                                            <%--<dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="[A]ttachment(s)" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                        </dxe:ASPxButton>--%>
                                            <%--<a class="btn btn-primary" href="javascript:void(0);" onclick="fn_PopOpen()"><span>[B]illing/Shipping</span> </a>--%>
                                        </div>
                                    </div>

                                </dxe:ContentControl>
                            </ContentCollection>
                            <%--test generel--%>
                        </dxe:TabPage>
                        <%--<dxe:TabPage Name="[A]ttachment(s)" Visible="false" Text="[A]ttachment(s)">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">
                                </dxe:ContentControl>
                            </ContentCollection>
                        </dxe:TabPage>--%>
                        <dxe:TabPage Name="[B]illing/Shipping" Text="Billing/Shipping">
                            <ContentCollection>
                                <dxe:ContentControl runat="server">

                                    <ucBS:Sales_BillingShipping runat="server" ID="BillingShippingControl" />
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
            <%--SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">--%>
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
            <%--Customer Popup--%>
            <%--    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
                Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>--%>
            <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="680px"
                Width="1100px" HeaderText="Add New Customer" Modal="true" AllowResize="false" ResizingMode="Postponed">

                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <%--Sudip--%>
            <div class="PopUpArea">
                <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
                <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
                <%--ChargesTax--%>
                <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
                    Width="900px" Height="300px" HeaderText="Quotation Taxes" PopupHorizontalAlign="WindowCenter"
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
                                        <dxe:ASPxButton ID="btn_SaveTax" ClientInstanceName="cbtn_SaveTax" runat="server" AccessKey="X" AutoPostBack="False" Text="Ok" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="ASPxButton5" ClientInstanceName="cbtn_tax_cancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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
                                <div class="clear"></div>
                                <div class="col-sm-12" style="padding-top: 8px;">
                                    <span id="chargesRoundOf"></span>
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
                                                Batch
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
                                                    <ClientSideEvents TextChanged="txtserialTextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3" id="div_Quantity">
                                            <div style="margin-bottom: 2px;">
                                                Quantity
                                            </div>
                                            <div class="Left_Content" style="">
                                                <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" DisplayFormatString="0.0000" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="30px">
                                                    <MaskSettings Mask="<0..999999999999>.<0..9999>" IncludeLiterals="DecimalSymbol" />
                                                    <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 14px">
                                                <dxe:ASPxButton ID="btnWarehouse" ClientInstanceName="cbtnWarehouse" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
                                                    <ClientSideEvents Click="function(s, e) { if(!document.getElementById('myCheck').checked)  {SaveWarehouse();}}" />
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
                                            <dxe:GridViewDataTextColumn Caption="Batch Number" FieldName="BatchNo"
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
                                                        <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="Delete">
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
                <asp:HiddenField ID="hdnIsFromActivity" runat="server" />
                <asp:HiddenField runat="server" ID="hddnCustIdFromCRM" />
                <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
                <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
                <asp:HiddenField runat="server" ID="hdnEditId" />
                <%--Subhra--%>
                <asp:HiddenField ID="hdntab2" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />
                <asp:HiddenField runat="server" ID="uniqueId" />
                <asp:HiddenField runat="server" ID="hdAddOrEdit" />
                 <%--Rev 2.0--%>
                <dxe:ASPxLoadingPanel ID="LoadingPanelMultiUOM" runat="server" ClientInstanceName="LoadingPanelMultiUOM" ContainerElementID="divMultiUOM"
                    Modal="True">
                </dxe:ASPxLoadingPanel>
                 <%--End of Rev 2.0--%>

                <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                    <ClientSideEvents ControlsInitialized="AllControlInitilize" />
                </dxe:ASPxGlobalEvents>
            </div>

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

            <%--End Sudip--%>

            <asp:HiddenField ID="hdnCustomerId" runat="server" />
            <asp:HiddenField ID="hdnAddressDtl" runat="server" />
            <%--Debu Section--%>

            <%--Batch Product Popup Start--%>

            <%--<dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
                Width="800" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
                <HeaderTemplate>
                    <span>Select Product</span>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <label><strong>Search By Product Name</strong></label>
                        <span style="color: red;">[Press ESC key to Cancel]</span>
                        <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp" 
                            IncrementalFilteringMode="Contains"
                            KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" 
                            ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                            <Columns>
                                <dxe:GridViewDataColumn FieldName="Products_Description" Caption="Name" Width="240">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="80">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="100">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="220">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="140">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120" Visible="false">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                   //<dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />//
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

            <asp:SqlDataSource runat="server" ID="ProductDataSource"
                SelectCommand="prc_SalesCRM_Details" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetails" />
                    <asp:ControlParameter DefaultValue="Y" Name="InventoryType" ControlID="ctl00$ContentPlaceHolder1$ASPxPageControl1$ddlInventory" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>

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
                                        <ClientSideEvents EndCallback="cgridTax_EndCallBack " RowClick="GetTaxVisibleIndex" />

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
                                        <input type="button" onclick="BatchUpdate()" value="Ok" class="btn btn-primary mTop" />
                                        <input type="button" onclick="cgridTax.CancelEdit(); caspxTaxpopUp.Hide(); return false;" value="Cancel" class="btn btn-danger mTop" />
                                        <span id="taxroundedOf"></span>
                                    </div>
                                    <table class="pull-right">
                                        <tr>
                                            <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="txtTaxTotAmt" MaxLength="80" ClientInstanceName="ctxtTaxTotAmt" Text="0.00" ReadOnly="true"
                                                    runat="server" Width="100%" CssClass="pull-left mTop">
                                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
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

                <ClientSideEvents CloseUp="udfAfterHide" />
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
        <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    </div>
    </div>
    <div>
        <asp:HiddenField runat="server" ID="HDItemLevelTaxDetails" />
        <asp:HiddenField runat="server" ID="HDHSNCodewisetaxSchemid" />
        <asp:HiddenField runat="server" ID="HDBranchWiseStateTax" />
        <asp:HiddenField runat="server" ID="HDStateCodeWiseStateIDTax" />
    </div>

    <!--Abhisek -->
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
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Name </th>
                                <th>Product Description </th>
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
                    <input type="text" onkeydown="SalesMankeydown(event)" id="txtSalesManSearch" autofocus width="100%" placeholder="Search By Salesman/Agent Name" />

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
    <%--SalesMan/Agent--%>
    <asp:HiddenField ID="hdnGuid" runat="server" />


    <dxe:ASPxPopupControl ID="Popup_MultiUOM" runat="server" ClientInstanceName="cPopup_MultiUOM"
        Width="900px" HeaderText="Multi UOM Details" PopupHorizontalAlign="WindowCenter"
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
                                <td>
                                    <div>
                                        <div style="margin-bottom: 5px;">
                                            <div>
                                                <label>Base Quantity</label>
                                            </div>
                                            <div>
                                                <%--Rev Sanchita--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal" />--%>
                                                <%--Rev 2.0--%>
                                                <%--<input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal"  onchange="CalcBaseRate();" placeholder="0.0000"  />--%>
                                                <input type="text" id="UOMQuantity" style="text-align: right;" maxlength="18"  class="allownumericwithdecimal"  onfocusout="CalcBaseRate();" placeholder="0.0000"  />
                                                <%--End of Rev 2.0--%>
                                                <%--End of Rev Sanchita--%>
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
                                  <%--Mantis Issue 24428--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Base Rate </label>
                                        </div>
                                        <div>
                                            <%--Rev 2.0--%>
                                            <%--<dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>--%>
                                            <dxe:ASPxTextBox ID="cmbBaseRate" runat="server" Width="80px" ClientInstanceName="ccmbBaseRate" DisplayFormatString="0.00" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..99&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right" ReadOnly="true" ></dxe:ASPxTextBox>
                                            <%--End of Rev 2.0--%>
                                        </div>
                                    </div>
                                </td>
                                <%--End of Mantis Issue 24428--%>
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
                                                <ClientSideEvents TextChanged="function(s,e) { PopulateMultiUomAltQuantity();setTwoNumberDecimal(this);}" />
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
                                                  <%--Mantis Issue 24428--%>
                                                <%--Rev 2.0--%>
                                                <%--<ClientSideEvents TextChanged="function(s,e) { CalcBaseQty();}" />--%>
                                                <ClientSideEvents LostFocus="function(s,e) { CalcBaseQty();}" />
                                                <%--End of Rev 2.0--%>
                                                <%--End of Mantis Issue 24428--%>
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                </td>

                                   <%--Mantis Issue 24428--%>
                                 <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            <label>Alt Rate </label>
                                        </div>
                                        <div>
                                            <%--Rev 2.0--%>
                                            <%--<dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.000" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..999&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents TextChanged="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxTextBox ID="cmbAltRate" Width="80px" runat="server" ClientInstanceName="ccmbAltRate" DisplayFormatString="0.00" MaskSettings-Mask="&lt;0..99999999&gt;.&lt;00..99&gt;" FocusedStyle-HorizontalAlign="Right" HorizontalAlign="Right"  >
                                                <ClientSideEvents LostFocus="function(s,e) { CalcBaseRate();}" />
                                            </dxe:ASPxTextBox>
                                            <%--End of Rev 2.0--%>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div style="margin-bottom: 5px;">
                                        <div>
                                            
                                        </div>
                                        <%--Rev 2.0 [ class="mlableWh" added] --%>
                                        <div class="mlableWh" >
                                            <%--Rev Sanchita--%>
                                            <%--<label class="checkbox-inline mlableWh">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                              
                                            </label>--%>
                                            <%--Rev 2.0 [ class="mlableWh" removed --%>
                                            <label class="checkbox-inline ">
                                                <input type="checkbox" id="chkUpdateRow"  />
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Update Row">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>
                                            <%--End of Rev Sanchita--%>
                                        </div>
                                    </div>

                                    
                                </td>
                                <%--End of Mantis Issue 24428--%>
                                <%--Rev 3.0--%>
                                </tr>
                                <tr>
                                <%--End of Rev 3.0--%>
                                    <td style="padding-top: 14px;">
                                        <dxe:ASPxButton ID="btnMUltiUOM" ClientInstanceName="cbtnMUltiUOM" Width="50px" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-primary">
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


                                   <dxe:GridViewDataTextColumn Caption="MultiUOMSR No" 
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="left" Width="0px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="0" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM"
                                    VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>

                                  <%--Mantis Issue 24428 --%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="BaseRate"
                                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>



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

                                   <%--Mantis Issue 24428--%>
                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="AltRate"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Update Row" FieldName="UpdateRow"
                                    VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24428 --%>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" Width="80px" Caption="Action">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="Delete_MultiUom('<%# Container.KeyValue %>','<%#Eval("SrlNo") %>')" title="Delete">
                                            <img src="/assests/images/crs.png" /></a>

                                          <%--Mantis Issue 24428 --%>

                                           <a href="javascript:void(0);" onclick="Edit_MultiUom('<%# Container.KeyValue %>','<%#Eval("MultiUOMSR") %>')" title="Edit">
                                            <img src="/assests/images/Edit.png" /></a>
                                          <%--End of Mantis Issue 24428 --%>
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
                            <dxe:ASPxButton ID="ASPxButton7" ClientInstanceName="cbtnfinalUomSave" Width="50px" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function(s, e) {FinalMultiUOM();}" />
                            </dxe:ASPxButton>
                            <%--Rev 2.0--%>
                            <label id="lblInfoMsg" style="font-weight:bold; color:red; " > </label>
                            <%--End of Rev 2.0--%>
                        </div>
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
    <asp:HiddenField ID="hdnQuantitySL" runat="server" />
    <asp:HiddenField ID="hdnDeleteRowCheck" runat="server" />
    <asp:HiddenField runat="server" ID="hdnPageStatForApprove" />
    <asp:HiddenField runat="server" ID="hdnApproveStatus" />
    <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
    <asp:HiddenField runat="server" ID="hdnApprovalReqInq" />
    <asp:HiddenField runat="server" ID="hdnEntityType" />
    <asp:HiddenField runat="server" ID="hdnQty" />
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
    <asp:HiddenField ID="hdngotfocusQty" runat="server" />
    <asp:HiddenField runat="server" ID="ProductMinPrice" />
    <asp:HiddenField runat="server" ID="ProductMaxPrice" />
    <asp:HiddenField runat="server" ID="hdnRateType" />
</asp:Content>
