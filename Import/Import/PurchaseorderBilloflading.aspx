<%@ Page Title="Billoflading" Language="C#" MasterPageFile="~/OMS/Masterpage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseorderBilloflading.aspx.cs" Inherits="Import.Import.Billoflading" %>


<%@ Register Src="~/Import/Import/OMS/Management/Activities/UserControls/Import_Purchase_BillingShipping.ascx" TagPrefix="ucBS" TagName="Purchase_BillingShipping" %>
<%@ Register Src="~/Import/Import/OMS/Management/Activities/UserControls/Import_VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Src="~/Import/Import/OMS/Management/Activities/UserControls/Import_TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="Import/CSS/SearchPopup.css" rel="stylesheet" />

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

        #grid_DXMainTable > tbody > tr > td:last-child,
        #grid_DXMainTable > tbody > tr > td:nth-child(18) {
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

        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }
    </style>

    <script>

        $(function(){

            $('#CustModel').on('hide.bs.modal', function () {
                ctxtVendorName.Focus();
                //grid.batchEditApi.StartEdit(globalRowIndex, 2);
            })

        });

        var _ComponentDetails;
        function gridProducts_EndCallback(s, e) {
            if (cgridproducts.cpComponentDetails) {
                _ComponentDetails=cgridproducts.cpComponentDetails;
                cgridproducts.cpComponentDetails = null;
            }
        }
        function taggingListKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function taggingListButnClick(s, e) {
            ctaggingGrid.PerformCallback('BindComponentGrid');
            cpopup_taggingGrid.Show();
        }
        function Tag_ChangeState(value) {
            ctaggingGrid.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }
    </script>

    <script type="text/javascript">
        var GlobalAllAddress = [];
        function VendorButnClick(s, e) {
            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Vendor Name</th><th>Unique Id</th></tr><table>";
            document.getElementById("CustomerTable").innerHTML = txt;
            setTimeout(function () { $("#txtCustSearch").focus(); }, 500);
            $('#txtCustSearch').val('');
            $('#CustModel').modal('show');
        }
        function VendorKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter"|| e.code == "NumpadEnter") {
                s.OnButtonClick(0);
            }
        }

        function VendorModekkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            OtherDetails.BranchID = $('#ddl_Branch').val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Vendor Name");
                HeaderCaption.push("Unique Id");
                if($("#txtCustSearch").val()!="")
                {
                    //callonServer("Services/Master.asmx/GetVendorWithOutBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                    callonServer("Services/Import_Master.asmx/GetVendorWithBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
               
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function SetCustomer(Id, Name) {
            if (Id) {
                $('#CustModel').modal('hide');
                ctxtVendorName.SetText(Name);
            
                GetObjectID('hdnCustomerId').value = Id;
                $('#MandatorysVendor').attr('style', 'display:none');
                var VendorId=Id;
                //GetContactPerson();
                $('#CustModel').modal('hide');
                cContactPerson.Focus();

                //changes 27-06-2018  Sudip  Pal


                $.ajax({
                    type: "POST",
                    url: "PurchaseOrderAcceptance.aspx/GetVendorReletedData",
                    data: JSON.stringify({ VendorId: VendorId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;                       
                        var strDueDate = data.toString().split('~')[0];
                        var strcountryId = data.toString().split('~')[1];
                        var strOutstanding = data.toString().split('~')[2];
                        var strGSTN = data.toString().split('~')[3];

                        if (strDueDate != null) {
                            var DeuDate = strDueDate;
                            var myDate = new Date(DeuDate);
                            var invoiceDate = new Date();
                            var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));
                            ctxtCreditDays.SetValue(datediff);
                            cdt_PODue.SetDate(myDate);                            
                        }
                        if (strcountryId != null && strcountryId != "") {
                            var CountryID = strcountryId;
                            if (CountryID != "1") {
                                cddl_AmountAre.SetValue("4");
                                cddl_AmountAre.SetEnabled(false);
                            }                           
                            else
                            {
                                cddl_AmountAre.SetValue("1");
                                cddl_AmountAre.SetEnabled(true);

                                if ($('#hfVendorGSTIN').val() == '')
                                {
                                    IfVendorGstInIsBlank();
                                }
                            }


                            if (CountryID == "0")
                            {
                                
                                jAlert('You must enter the default Billing/Shipping Address for selected Vendor to proceed further.');
                                ctxtVendorName.SetText("");            
                                GetObjectID('hdnCustomerId').value = "";
                                cddl_AmountAre.SetValue("1");
                                cddl_AmountAre.SetEnabled(true);
                                ctxtVendorName.Focus();
                            }
                        }
                        else
                        {
                            cddl_AmountAre.SetValue("1");
                            cddl_AmountAre.SetEnabled(true);
                        }

               <%--         if (strOutstanding != null) {
                            pageheaderContent.style.display = "block";
                            $("#<%=divOutstanding.ClientID%>").attr('style', 'display:block');
                            document.getElementById('<%=lblTotalPayable.ClientID %>').innerHTML = strOutstanding;                            
                        }
                        else {
                            pageheaderContent.style.display = "none";
                            $("#<%=divOutstanding.ClientID%>").attr('style', 'display:none');
                            document.getElementById('<%=lblTotalPayable.ClientID %>').innerHTML = '';
                        }
                        if (strGSTN != null) {
                            $("#<%=divGSTIN.ClientID%>").attr('style', 'display:block');
                            document.getElementById('<%=lblGSTIN.ClientID %>').innerHTML = strGSTN;                            
                        }--%>
                        
                    }
                });

                //changes 27-06-2018  Sudip  Pal

                GetContactPerson();
            }
        }


        function ValueSelected(e, indexName) {

            if (e.code == "Enter"|| e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex")
                        SetProduct(Id, name);
                        //Start:Chinmoy 
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                        //End
                    else
                        SetCustomer(Id, name);
                }
            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "ProdIndex")
                        $('#txtProdSearch').focus();
                        //Start Chinmoy 
                    else if (indexName == "BillingAreaIndex")
                        $('#txtbillingArea').focus();
                    else if (indexName == "ShippingAreaIndex")
                        $('#txtshippingArea').focus();
                        //End

                    else
                        $('#txtCustSearch').focus();
                }
            }
        }

        function IfVendorGstInIsBlank()
        {
           // cddl_AmountAre.SetValue("3");
            PopulateGSTCSTVAT();
           // cddl_AmountAre.SetEnabled(false);
        }


        function AfterSaveBillingShipiing(validate) {
            if (validate) {
                page.SetActiveTabIndex(0);
                page.tabs[0].SetEnabled(true);
                $("#divcross").show();
            }
            else {
                page.SetActiveTabIndex(1);
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }
        }

    </script>

    <script>
        function prodkeydown(e) {
            var inventoryType = (document.getElementById("ddlInventory").value != null) ? document.getElementById("ddlInventory").value : "";

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.InventoryType = inventoryType;
            // console.log(e.code);
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                // HeaderCaption.push("Product Description");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                // HeaderCaption.push("Installation Reqd.");

                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Import_Master.asmx/GetPurchaseProductForPO", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //  grid.StartEditRow(globalRowIndex);
                grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }
        }
    </script>

    <script type="text/javascript">
        function GlobalBillingShippingEndCallBack() {  
            
            var VendorId = $('#hdnCustomerId').val();         

            if (VendorId != null && VendorId != "") {              
                
                cContactPerson.PerformCallback('BindContactPerson~' + VendorId);
                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCspecefiFields_PO(VendorId);
                }
            }
            
        }
    </script>


    <%--    Changes 18-06-2018  Sudip Pal--%>
    <%--    <script src="../../Tax%20Details/Js/TaxDetailsItemlevel.js" type="text/javascript"></script>--%>


    <script src="Import/Js/Taxdetails.js" type="text/javascript"></script>

    <%--  Changes 18-06-2018  Sudip Pal--%>
    <%--------   Address and Billing Sectin Start   -----25-01-2017  ---------%>

    <script type="text/javascript">
        function CreditDays_TextChanged(s, e) {
            var CreditDays = ctxtCreditDays.GetValue();
            var today = new Date();
            var newdate = new Date();
            newdate.setDate(today.getDate() + Math.round(CreditDays));
            cdt_PODue.SetDate(newdate);
        }
        function BackClick() {
            var keyOpening = document.getElementById('hdnOpening').value;
            if (keyOpening != '') {
                var url = 'PurchaseOrder-Acceptancelist.aspx?op=' + 'yes';
            }
            else {
                var url = 'PurchaseOrder-Acceptancelist.aspx';
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

              
            //Changes 20-06-2018 Sudip Pal
            $("#ddl_Currency").val(1);
            $("#txt_Rate_I").val(0.0);
            //Changes 20-06-2018 Sudip Pal


       
       
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

    </script>

    <%-------   Address and Billing Section End     -----25-01-2017   ---------%>

    <script type="text/javascript">

        var globalRowIndex;
        var rowEditCtrl;
        var TaxOfProduct = [];

        var _GetQuantityValue = "0";
        var _GetPurchasePriceValue = "0";
        var _GetDiscountValue = "0";
        var _GetAmountValue = "0";

        function selectValue() {
            var checked = $('#rdl_IndentRequisition').attr('checked', true);
            if (checked) {
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', true);
            }         
            var type = ($("[id$='rdl_IndentRequisition']").find(":checked").val() != null) ? $("[id$='rdl_IndentRequisition']").find(":checked").val() : "";           
            if (type != "") {
                if ($('#ddlInventory').val() == 'Y') {
                    GetIndentReqNoOnLoad();
                }
            }
        }
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
            //GetIndentReqNoOnLoad();
            grid.batchEditApi.StartEdit(-1, 1);
            var accountingDataMin = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();
            grid.batchEditApi.StartEdit(0, 1);
            var accountingDataplus = grid.GetEditor('ProductName').GetValue();
            grid.batchEditApi.EndEdit();
            if (accountingDataMin != null || accountingDataplus != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        // cQuotationComponentPanel.PerformCallback('BindNullGrid');
                    }
                });
            }
        }
        
        //............................Product Pazination..............
        function ChangeState(value) {
            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }

        function ctaxUpdatePanelEndCall(s, e) {

            if (ctaxUpdatePanel.cpstock != null) {
                divAvailableStk.style.display = "block";
                var AvlStk = ctaxUpdatePanel.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStkPro.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = ctaxUpdatePanel.cpstock;
                ctaxUpdatePanel.cpstock = null;
                //grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }
            return false;
        }


        function ProductsGotFocusFromID(s, e) {
            pageheaderContent.style.display = "block";
            //  divAvailableStk.style.display = "block";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";

            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
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
    
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter"|| e.code == "NumpadEnter") {
                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Tab") {

            //    s.OnButtonClick(0);
            //}
        }

        function ProductButnClick(s, e) {
            var VendorID = GetObjectID('hdnCustomerId').value;
            if (VendorID != null && VendorID != "") {

                if(($("#txt_Rate_I").val() =='0.0' || $("#txt_Rate_I").val() =='0' ) && $("#ddl_Currency").val() !=1)
                {
                    jAlert("Currency Rate should not be 0");
                }
                else
                {
                    if (e.buttonIndex == 0) {
                        $('#txtProdSearch').val('');
                        $('#ProductModel').modal('show');
                        setTimeout(function () { $("#txtProdSearch").focus(); }, 500);   
                    }
                }

            }
            else {
                jAlert("Please Select a Vendor", "Alert", function () { ctxtVendorName.Focus(); });
            }
        } 


        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');
            var LookUpData = Id;
            var ProductCode = Name;


            if (!ProductCode) {
                LookUpData = null;
            }


            // cProductpopUp.Hide();
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("gvColProduct").SetText(LookUpData);
           

            pageheaderContent.style.display = "block";
            ///    divAvailableStk.style.display = "block";
            cddl_AmountAre.SetEnabled(false);
            ctxtVendorName.SetEnabled(false);    


            //Changes 20-06-2018 Sudip Pal
            $("#ddl_Currency").attr("disabled", true); 
            $("#txt_Rate_I").attr("disabled", true); 
            //Changes 20-06-2018 Sudip Pal


            if(document.getElementById("ddl_numberingScheme")!=null)
            {
                document.getElementById("ddl_numberingScheme").disabled = true;
            }
            
            document.getElementById("ddlInventory").disabled = true;

            var tbDescription = grid.GetEditor("gvColDiscription");
            var tbUOM = grid.GetEditor("gvColUOM");
            var tbSalePrice = grid.GetEditor("gvColStockPurchasePrice");
            ///Changes 13-06-2018 Sudip Pal

            var tbPriceforeign = grid.GetEditor("gvColStockPurchasePriceforeign");

            ///Changes 13-06-2018 Sudip Pal

            var ProductID = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var Product_Name = SpliteDetails[12];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            //Changes 13-06-2018  Sudip Pal
            var strSalePriceforeign='';

            strSalePriceforeign=SpliteDetails[6];
            //Changes 13-06-2018  Sudip Pal


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

            ///Changes 13-06-2018  Sudip Pal
            ///  tbSalePrice.SetValue(strSalePrice);

            tbPriceforeign.SetValue(strSalePrice);
            tbSalePrice.SetValue(strSalePriceforeign);

            ///Changes 13-06-2018  Sudip Pal

            grid.GetEditor("ProductName").SetText(Product_Name);
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
            } else {
                divPacking.style.display = "none";
            }

            document.getElementById("ddlInventory").disabled = true;
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            grid.batchEditApi.StartEdit(globalRowIndex, 6);

            var _SrlNo = grid.GetEditor("SrlNo").GetValue();
            if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
                TaxOfProduct.push(ProductTaxes);      
                
            }
            else {
                $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; }); 
            }
            SetFocusAfterProductSelect();
        }

        function SetFocusAfterProductSelect(){
            setTimeout(function () {           
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                return;
            }, 300);
        }


        //..............End Product........................
        //.............Available Stock Div Show............................
        function ProductsGotFocus(s, e) {
            pageheaderContent.style.display = "block";
            //   divAvailableStk.style.display = "block";
            var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "0";
            var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
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
                /// divAvailableStk.style.display = "block";
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
                url: "PurchaseOrder-Import.aspx/CheckUniqueName",
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

        function Currency_Change()
        {

            if(($("#txt_Rate_I").val() =='0.0' || $("#txt_Rate_I").val() =='0' ) && $("#ddl_Currency").val() !=1)
            {
              
                jAlert("Currency Rate should not be 0");
                ctxtRate.Focus();
            }
           
        }

        //..................Rate........................
        function ReBindGrid_Currency() {
            
           
         <%--   var frontRow = 0;
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
            cddl_AmountAre.Focus();--%>
            

        }
        //...............end.........................
        //...............PopulateVAT........................
        function PopulateGSTCSTVAT(e) {
          
            var key = cddl_AmountAre.GetValue();
            if (key == 1) {
                grid.GetEditor('gvColTaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                //cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(-1);
                cbtn_SaveRecords.SetVisible(true);

                //Changes 05-07-2018  Sudip Pal
                cbtn_SaveRecordTaxs.SetVisible(true);
                //Changes 05-07-2018  Sudip Pal


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
                //Changes 05-07-2018  Sudip Pal
                cbtn_SaveRecordTaxs.SetVisible(true);
                //Changes 05-07-2018  Sudip Pal

            }
            else if (key == 3) {
              
                grid.GetEditor('gvColTaxAmount').SetEnabled(false);
                //cddlVatGstCst.PerformCallback('3');
                cddlVatGstCst.SetSelectedIndex(-1);
                cddlVatGstCst.SetEnabled(false);
                // cbtn_SaveRecords.SetVisible(false);
                cbtn_SaveRecordTaxs.SetVisible(false);
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            }
            //// below code will be executed only in View Mode --- Samrat Roy -- 04-05-2017
            if (getUrlVars().req == "V") {
                cbtn_SaveRecords.SetVisible(false);
                cbtn_SaveRecordExits.SetVisible(false);
                //Changes 05-07-2018  Sudip Pal
                cbtn_SaveRecordTaxs.SetVisible(false);
                //Changes 05-07-2018  Sudip Pal


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
            ctxtCreditDays.Focus();
        }


        function SetFocusonGrid(e) {
            if (grid.GetVisibleRowsOnPage() == 1) {
                grid.batchEditApi.StartEdit(-1, 3);
            }
        }

        //.................End PopulateVAT..........................
        //................Amount Calculation.........................

        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }



        function taxAmtButnClick1(s, e) {
            rowEditCtrl = s;
        }

        function taxAmtButnClick(s, e) {
        
            $("#HDNbranch").val($("#ddl_Branch").val());
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('gvColProduct').GetValue() != null) ? grid.GetEditor('gvColProduct').GetValue() : "";

                    ///    alert(ProductID);
                   
                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();


                        //Changes 20-06-2018  Sudip  Pal
                        $("#span_foreign").html($("#ddl_Currency :selected").text());
                        //Changes 20-06-2018  Sudip  Pal
                        caspxTaxpopUp.Show();   
                        

                        var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        var strStkUOM = SpliteDetails[4];

                        ///  alert(SpliteDetails[0]+'|'+SpliteDetails[7]+'|'+SpliteDetails[4]+'|'+SpliteDetails[7]+'|'+SpliteDetails[8]);


                        var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }
                        var StockQuantity = strMultiplier * QuantityValue;                      
                        var Amount = (QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                        clblTaxProdGrossAmt.SetText(parseFloat(Amount).toFixed(2));
                        var strAmount = grid.GetEditor('gvColAmount').GetValue();                       
                        clblProdNetAmt.SetText(parseFloat(strAmount).toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = parseFloat(Amount).toFixed(2);                       
                        document.getElementById('HdProdNetAmt').value = parseFloat(strAmount).toFixed(2);                    
                        if (parseFloat(grid.GetEditor('gvColDiscount').GetValue()) > 0) {                           
                            var discount = (Amount * grid.GetEditor('gvColDiscount').GetValue() / 100).toFixed(2);
                            clblTaxDiscount.SetText(discount);
                        }
                        else {
                            clblTaxDiscount.SetText('0.00');
                        }                      
                        if (cddl_AmountAre.GetValue() == "2") {
                            $('.GstCstvatClass').hide();
                            $('.gstGrossAmount').show();
                            clblTaxableGross.SetText("(Taxable)");
                            clblTaxableNet.SetText("(Taxable)");
                            $('.gstNetAmount').show();                          
                            var gstRate = parseFloat(cddlVatGstCst.GetValue().split('~')[1]);


                      



                            if (gstRate) {
                                if (gstRate != 0) {
                                    var gstDis = (gstRate / 100) + 1;
                                    if (cddlVatGstCst.GetValue().split('~')[2] == "G") {
                                        $('.gstNetAmount').hide();                                      
                                        clblTaxProdGrossAmt.SetText((Amount / gstDis).toFixed(2));                                      
                                        document.getElementById('HdProdGrossAmt').value =(Amount / gstDis).toFixed(2);                                      
                                        clblGstForGross.SetText((Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                        clblTaxableNet.SetText("");
                                    }
                                    else {
                                        $('.gstGrossAmount').hide();                                    
                                        clblProdNetAmt.SetText((grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2));                                      
                                        document.getElementById('HdProdNetAmt').value = (grid.GetEditor('gvColAmount').GetValue() / gstDis).toFixed(2);                                      
                                        clblGstForNet.SetText((grid.GetEditor('gvColAmount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
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
                            var shippingStCode = '';
                            shippingStCode = ctxtshippingState.GetText();
                            shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();                         

                         
                            if (shippingStCode.trim() != '') {
                                for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {                                  
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {
                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {                                       
                                            if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
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
                                        ccmbGstCstVat.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            }
                        }
                        else {
                            clblTaxableGross.SetText("");
                            clblTaxableNet.SetText("");

                        }
                        var _SrlNo = document.getElementById('HdSerialNo').value;
                        var _IsEntry="";
                        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length > 0) {
                            _IsEntry=TaxOfProduct.find(o => o.SrlNo === _SrlNo).IsTaxEntry;
                        }
                     
                        //if (globalRowIndex > -1) {
                        if(_IsEntry=="N"){
                          
                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());                          
                            cgridTax.cpComboCode = grid.GetEditor('gvColProduct').GetValue().split('||@||')[9];
                            
                        } else {
                           
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());


                        }
                        ctxtprodBasicAmt.SetValue(grid.GetEditor('gvColAmount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
            }
        }


        function QuantityProductsGotFocus(s, e) {
            var ProductID = grid.GetEditor('gvColProduct').GetValue();

            if (ProductID != null) {            
        
                Pre_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
                Pre_TotalAmt_Forgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

                _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
                _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
                _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
            }           
        }

      
      

        
        function PurchasePriceTextFocus(s, e) {  


            Pre_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
            Pre_TotalAmt_Forgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

            _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
            _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";

        }

        function PurchasePriceTextFocusfrgn(s, e) {  


            Pre_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
            Pre_TotalAmt_Forgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

            _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
            _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";

        }


        function QuantityTextChange(s, e) {

            pageheaderContent.style.display = "block";
            //  divAvailableStk.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier
                //var strFactor = SpliteDetails[14]; //Packing_Factor
                var strFactor = SpliteDetails[8]; //Packing_Factor
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
                var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                if (strRate == 0) {
                    strRate = 1;
                }
                var Amount = (QuantityValue * strFactor * (strSalePrice / strRate)).toFixed(2);
                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(Amount);

                var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                tbTotalAmount.SetValue(Amount);



                $('#<%= lblbranchName.ClientID %>').text(strBranch);
                var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
                if (IsLinkedProduct != "Y") {
                    var tbAmount = grid.GetEditor("gvColAmount");
                    tbAmount.SetValue(Amount);
                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(Amount);


                    ///Changes 02-07-2018  Sudip Pal

                    // alert(parseFloat(QuantityValue) + parseFloat(_GetQuantityValue));
                
                    //if (parseFloat(QuantityValue) != parseFloat(_GetQuantityValue))
                    //{
                    DiscountTextChange(s, e);
                    //}

                    ///Changes 02-07-2018  Sudip Pal
                }

            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('gvColQuantity').SetValue('0');
                grid.GetEditor('gvColProduct').Focus();
            }
        }

        function PurchasePriceTextChange(s, e) {

            pageheaderContent.style.display = "block";
            // divAvailableStk.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier
                //var strFactor = SpliteDetails[14]; //Packing_Factor
                var strFactor = SpliteDetails[8]; //Packing_Factor
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name
                var strPurPrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                if (strRate == 0) {
                    strRate = 1;
                }
                var Amount = (QuantityValue * strFactor * (strPurPrice / strRate)).toFixed(2);
                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(Amount);
                var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                tbTotalAmount.SetValue(Amount);


               
                ////Changes 13-06-2018 Sudip Pal

                var tbTotalAmountfrgn = grid.GetEditor("gvColStockPurchasePriceforeign");
                tbTotalAmountfrgn.SetValue(strPurPrice/strRate);

                ////Changes 13-06-2018 Sudip Pal


                ////Changes 27-06-2018 Sudip Pal

                var Ammountbasecurr = grid.GetEditor("gvColAmountbase");
                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.00000')
                {
                    Ammountbasecurr.SetValue(Amount);

                }

                else
                {
                    Ammountbasecurr.SetValue(Amount*$("#txt_Rate_I").val());

                }

                ////Changes 27-06-2018 Sudip Pal


                $('#<%= lblbranchName.ClientID %>').text(strBranch);
                var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
                if (IsLinkedProduct != "Y") {
                    var tbAmount = grid.GetEditor("gvColAmount");
                    tbAmount.SetValue(Amount);
                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(Amount);
                    
                    ///Changes 02-07-2018  Sudip Pal
                    //if (parseFloat(_GetPurchasePriceValue) != parseFloat(strPurPrice)) {
                      
                    DiscountTextChange(s, e);
                    //}

                    ///Changes 02-07-2018  Sudip Pal
                }
            }

            else {
                jAlert('Select a product first.');
                grid.GetEditor('gvColQuantity').SetValue('0');
                grid.GetEditor('gvColProduct').Focus();
            }

           
        }


        function PurchasePriceTextChangefrgn(s, e)
        {
            pageheaderContent.style.display = "block";
            // divAvailableStk.style.display = "block";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];//Conversion_Multiplier
                //var strFactor = SpliteDetails[14]; //Packing_Factor
                var strFactor = SpliteDetails[8]; //Packing_Factor
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strProductID = SpliteDetails[0];
                var strProductName = (grid.GetEditor('gvColProduct').GetText() != null) ? grid.GetEditor('gvColProduct').GetText() : "0";
                var ddlbranch = $("[id*=ddl_Branch]");
                var strBranch = ddlbranch.find("option:selected").text();
                var strStkUOM = SpliteDetails[4];//Stk_UOM_Name


                var tbTotalAmountfrgnbase = grid.GetEditor("gvColStockPurchasePrice");
                var strPurPrice = (grid.GetEditor('gvColStockPurchasePriceforeign').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceforeign').GetValue() : "0";
            
               
                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.00000')
                {
                    tbTotalAmountfrgnbase.SetValue(strPurPrice*1);
                    strPurPrice=strPurPrice*1
                    // var strPurPrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                }
                else
                {
                    tbTotalAmountfrgnbase.SetValue(strPurPrice*strRate);
                    strPurPrice=strPurPrice*strRate
                }


                //alert(strPurPrice);

                var n=(grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";

                // alert(n);
                if (strRate == 0) {
                    strRate = 1;
                }

                var Amount = (QuantityValue * strFactor * (strPurPrice / strRate)).toFixed(2);
                var tbAmount = grid.GetEditor("gvColAmount");
                tbAmount.SetValue(Amount);
                var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                tbTotalAmount.SetValue(Amount);


               
                ////Changes 13-06-2018 Sudip Pal

                //var tbTotalAmountfrgn = grid.GetEditor("gvColStockPurchasePriceforeign");
                //tbTotalAmountfrgn.SetValue(strPurPrice/strRate);

                ////Changes 13-06-2018 Sudip Pal


                ////Changes 27-06-2018 Sudip Pal

                var Ammountbasecurr = grid.GetEditor("gvColAmountbase");
                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.00000')
                {
                    Ammountbasecurr.SetValue(Amount);

                }

                else
                {
                    Ammountbasecurr.SetValue(Amount*$("#txt_Rate_I").val());

                }

                ////Changes 27-06-2018 Sudip Pal


                $('#<%= lblbranchName.ClientID %>').text(strBranch);
                var IsLinkedProduct = (grid.GetEditor('IsLinkedProduct').GetText() != null) ? grid.GetEditor('IsLinkedProduct').GetText() : "";
                if (IsLinkedProduct != "Y") {
                    var tbAmount = grid.GetEditor("gvColAmount");
                    tbAmount.SetValue(Amount);
                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(Amount);
                    
                    ///Changes 02-07-2018  Sudip Pal
                    //if (parseFloat(_GetPurchasePriceValue) != parseFloat(strPurPrice)) {
                    // alert(Amount);
                    DiscountTextChange(s, e);
                    //}

                    ///Changes 02-07-2018  Sudip Pal
                }
            }

            else {
                jAlert('Select a product first.');
                grid.GetEditor('gvColQuantity').SetValue('0');
                grid.GetEditor('gvColProduct').Focus();
            }


        }




        function DiscountTextFocus() {       

            Pre_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
            Pre_TotalAmt_Forgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";


            _GetQuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            _GetPurchasePriceValue = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
            _GetDiscountValue = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
            _GetAmountValue = (grid.GetEditor('gvColAmount').GetValue() != null) ? grid.GetEditor('gvColAmount').GetValue() : "0";
        }
        function DiscountValueChange(s, e) {
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";

            if (ProductID != null) {
                if (parseFloat(Discount) != parseFloat(_GetDiscountValue)) {
                    DiscountTextChange(s, e);
                }
            }
            else {
                jAlert('Select a product first.');
            }
        }
        function DiscountTextChange(s, e) {
            
            var Discount = (grid.GetEditor('gvColDiscount').GetValue() != null) ? grid.GetEditor('gvColDiscount').GetValue() : "0";
            var QuantityValue = (grid.GetEditor('gvColQuantity').GetValue() != null) ? grid.GetEditor('gvColQuantity').GetValue() : "0";
            var ProductID = grid.GetEditor('gvColProduct').GetValue();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strSalePrice = (grid.GetEditor('gvColStockPurchasePrice').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePrice').GetValue() : "0";
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                var amountAfterDiscount = "";
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);
                Amount = Amount.toFixed(2);

                if (Discount != "0" && Discount != "0.00") {


                    /// alert(amountAfterDiscount);

                    amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                    amountAfterDiscount = amountAfterDiscount.toFixed(2);

                    var tbAmount = grid.GetEditor("gvColAmount");
                    tbAmount.SetValue(amountAfterDiscount);

                    var tbTotalAmount = grid.GetEditor("gvColTotalAmountINR");
                    tbTotalAmount.SetValue(amountAfterDiscount);


                   
                    ////Changes 13-06-2018 Sudip Pal

                    var tbTotalAmountfrgn = grid.GetEditor("gvColStockPurchasePriceforeign");
                    tbTotalAmountfrgn.SetValue(strSalePrice/strRate);

                    ////Changes 13-06-2018 Sudip Pal


                    ////Changes 14-06-2018 Sudip Pal
                    var Amountbasecurrency = (QuantityValue * strFactor * (strPurPrice)).toFixed(2);
                    var tbTotalAmountbasecurr = grid.GetEditor("gvColStockPurchasePriceNetamountbase");
                    tbTotalAmountbasecurr.SetValue(Amountbasecurrency);
                   
                    ////Changes 14-06-2018 Sudip Pal


                 
                    ////Changes 27-06-2018 Sudip Pal

                    var Ammountbasecurr = grid.GetEditor("gvColAmountbase");
                    if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.00000')
                    {
                        Ammountbasecurr.SetValue(Amount);

                    }

                    else
                    {
                        Ammountbasecurr.SetValue(Amount*$("#txt_Rate_I").val());

                    }

                    ////Changes 27-06-2018 Sudip Pal

                }
                else {
                    amountAfterDiscount = Amount;
                }

                var ShippingStateCode = $("#bsSCmbStateHF").val();

                var TaxType = "";
                if (cddl_AmountAre.GetValue() == "1") {
                    TaxType = "E";
                }
                else if (cddl_AmountAre.GetValue() == "2") {
                    TaxType = "I";
                }

                var _SrlNo = grid.GetEditor("SrlNo").GetValue();
                if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                    var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
                    TaxOfProduct.push(ProductTaxes);
                }
                else {
                    $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
                }

                ///Changes 19-06-2018 Sudip Pal

                //caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"), SpliteDetails[18], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P');
                caluculateAndSetGST(grid.GetEditor("gvColAmount"), grid.GetEditor("gvColTaxAmount"), grid.GetEditor("gvColTotalAmountINR"),  grid.GetEditor("gvColStockPurchasePriceNetamountbase"),SpliteDetails[18], Amount, amountAfterDiscount, TaxType, ShippingStateCode, $('#ddl_Branch').val(), 'P',$("#txt_Rate_I").val());

                Cur_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
                Cur_TotalAmt_frgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

                
                CalculateAmount();

                ///Changes 19-06-2018 Sudip Pal
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('gvColDiscount').SetValue('0');
                grid.GetEditor('gvColProduct').Focus();
            }
            ctaxUpdatePanel.PerformCallback('DelQtybySl~' + grid.GetEditor("SrlNo").GetValue());


        }

        function CalculateAmount() {
           
            var TotalAmount = (parseFloat((ctxttotamt.GetValue()).toString())).toFixed(2);

            var TotalAmountfrgn = (parseFloat((ctxttotamtfrgn.GetValue()).toString())).toFixed(2);

            var TotalAmountbase = (parseFloat((ctxtdocamt.GetValue()).toString())).toFixed(2);

           
            var Calculate_TotalAmount = (parseFloat(TotalAmount) + parseFloat(Cur_TotalAmt) - parseFloat(Pre_TotalAmt)).toFixed(2);
            var Calculate_TotalAmountfrgn = (parseFloat(TotalAmountfrgn) + parseFloat(Cur_TotalAmt_frgn) - parseFloat(Pre_TotalAmt_Forgn)).toFixed(2);

            ctxttotamtfrgn.SetValue(Calculate_TotalAmountfrgn);
            ctxttotamt.SetValue(Calculate_TotalAmount);
            ctxtdocamt.SetValue(Calculate_TotalAmount);





        }


        //function CalculateSummary(visibleIndex) {
        //    var s = 0;
        //    var tot = 0;
        //    //for (var index = 0 ; index < grid.GetVisibleRowsOnPage() ; index++) {
        //    //    alert( grid.GetVisibleRowsOnPage());
        //    alert(visibleIndex);
        //    s=(grid.batchEditApi.GetCellValue(visibleIndex, "gvColStockPurchasePriceNetamountbase"));
        //        alert(s);
        //    //}
        //}

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



            //Changes 22-06-2018 Sudip Pal
            if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
            {
                ctxtdocamt.SetValue(parseFloat(ctxtTotalAmount.GetValue())*1);
            }
            else
            {
                ctxtdocamt.SetValue(parseFloat(ctxtTotalAmount.GetValue())*$("#txt_Rate_I").val());

            }
            //Changes 22-06-2018 Sudip Pal

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


            document.getElementById('HdChargeProdAmt').value = sumAmount;


            //Changes  25-06-2018  Sudip Pal
            if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
            {
                document.getElementById('HdChargeProdNetAmt').value = $("#txt_totamont_I").val();
            }
            else
            {
                document.getElementById('HdChargeProdNetAmt').value = $("#txt_totamont_I").val()/$("#txt_Rate_I").val();
            }

            //document.getElementById('HdChargeProdNetAmt').value = sumNetAmount;

            //Changes  25-06-2018  Sudip Pal

         
            //End Here

            ctxtProductAmount.SetValue(Math.round(sumAmount).toFixed(2));
            ctxtProductTaxAmount.SetValue(Math.round(sumTaxAmount).toFixed(2));
            ctxtProductDiscount.SetValue(Math.round(sumDiscount).toFixed(2));

            //Changes  25-06-2018  Sudip Pal
            if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
            {
                ctxtProductNetAmount.SetValue( $("#txt_totamont_I").val());
            }
            else
            {
                ctxtProductNetAmount.SetValue( $("#txt_totamont_I").val()/$("#txt_Rate_I").val());
            }

            /// ctxtProductNetAmount.SetValue(Math.round(sumNetAmount).toFixed(2));
            //Changes  25-06-2018  Sudip Pal



          
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
                         


                            //Changes  25-06-2018  Sudip Pal
                            if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                            {
                                ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                            }
                            else
                            {
                                ctxtProductNetAmount.SetValue( $("#txt_totamont_I").val()/$("#txt_Rate_I").val());

                                // /Changes  25-06-2018  Sudip Pal
                            }

                            // ctxtProductNetAmount.SetText(Math.round(sumNetAmount / gstDis).toFixed(2));
                            //Changes  25-06-2018  Sudip Pal



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

            // ctxtQuoteTaxTotalAmt.SetValue(Math.round(totalTaxAmount));
            ctxtQuoteTaxTotalAmt.SetValue(totalTaxAmount);
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

                //Changes 19-06-2018  Sudip Pal
                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                {

                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                }
                else
                {
                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                }
                //Changes 19-06-2018  Sudip Pal

            }
            else {
                ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));

                //Changes 19-06-2018  Sudip Pal
                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                {

                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                }
                else
                {
                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                }
                //Changes 19-06-2018  Sudip Pal
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


            //Changes 19-06-2018  Sudip Pal
            if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
            {

                ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
            }
            else
            {
                ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
            }
            //Changes 19-06-2018  Sudip Pal


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

                    //Changes 19-06-2018  Sudip Pal
                    if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                    {

                        ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                    }
                    else
                    {
                        ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                    }
                    //Changes 19-06-2018  Sudip Pal
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);

                    //Changes 19-06-2018  Sudip Pal
                    if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                    {

                        ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                    }
                    else
                    {
                        ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                    }
                    //Changes 19-06-2018  Sudip Pal

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


                    //Changes 19-06-2018  Sudip Pal
                    if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                    {

                        ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()*1);
                    }
                    else
                    {
                        ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                    }
                    //Changes 19-06-2018  Sudip Pal


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

                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()*1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                        }
                        //Changes 19-06-2018  Sudip Pal

                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));


                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                        }
                        //Changes 19-06-2018  Sudip Pal

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
                        
                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                        }
                        //Changes 19-06-2018  Sudip Pal
                        
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                        }
                        //Changes 19-06-2018  Sudip Pal
                        
                        
                        GlobalCurTaxAmt = 0;
                    }




                }
            }
            //return;
            gridTax.batchEditApi.EndEdit();
        }
        function txtPercentageLostFocus(s, e) {
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
                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    
                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());
                        }
                        //Changes 19-06-2018  Sudip Pal
                        
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);
                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                      
                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());

                        }
                        
                        //Changes 19-06-2018  Sudip Pal

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
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
            //ctxtTaxTotAmt.SetValue(Math.round(totalInlineTaxAmount));
            ctxtTaxTotAmt.SetValue(totalInlineTaxAmount);

            //Changes 19-06-2018  Sudip Pal
            if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
            {

                ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
            }
            else
            {
                ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());

            }
         
            //Changes 19-06-2018  Sudip Pal
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
                       
                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());

                        }
                        
                        //Changes 19-06-2018  Sudip Pal

                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        
                        //Changes 19-06-2018  Sudip Pal
                        if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                        {

                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                        }
                        else
                        {
                            ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());

                        }

                
                        //Changes 19-06-2018  Sudip Pal
                        
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
            var _SrlNo = document.getElementById('HdSerialNo').value;
            if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "Y" }
                TaxOfProduct.push(ProductTaxes)
            }
            else {
                $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "Y"; });
            }

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

                //Changes 19-06-2018  Sudip Pal
                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                {

                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                }
                else
                {
                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());

                }

              
                //Changes 19-06-2018  Sudip Pal

                var gridValue = parseFloat(cgridTax.cpUpdated.split('~')[1]).toFixed(2);
                var ddValue = parseFloat(ctxtGstCstVat.GetValue()).toFixed(2);

                ctxtTaxTotAmt.SetValue(parseFloat(gridValue) + parseFloat(ddValue));

                //Changes 19-06-2018  Sudip Pal

                if ($("#txt_Rate_I").val() == '0' || $("#txt_Rate_I").val() == '0.0')
                {

                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* 1);
                }
                else
                {
                    ctxtTaxTotAmtInr.SetValue(ctxtTaxTotAmt.GetValue()* $("#txt_Rate_I").val());

                }

                //Changes 19-06-2018  Sudip Pal

                cgridTax.cpUpdated = "";
            }
            else {

                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
                grid.GetEditor("gvColTaxAmount").SetValue(totAmt);
                Pre_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
                Pre_TotalAmt_Forgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";
                if (cddl_AmountAre.GetValue() == "2") {

                    var totalNetAmount = parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue());
                    var totalRoundOffAmount = Math.round(totalNetAmount);

                    grid.GetEditor("gvColTotalAmountINR").SetValue(totalRoundOffAmount);
                    grid.GetEditor("gvColAmount").SetValue(DecimalRoundoff(parseFloat(grid.GetEditor("gvColAmount").GetValue()) + (totalRoundOffAmount - totalNetAmount), 2));
                    grid.GetEditor("gvColStockPurchasePriceNetamountbase").SetValue(DecimalRoundoff(parseFloat(StrRate) * parseFloat(grid.GetEditor("gvColTotalAmountINR").GetValue()), 2));

                    Cur_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
                    Cur_TotalAmt_frgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

                    CalculateAmount();  
                }
                else
                {
                    grid.GetEditor("gvColTotalAmountINR").SetValue(DecimalRoundoff(parseFloat(totAmt) + parseFloat(grid.GetEditor("gvColAmount").GetValue()), 2));
                    grid.GetEditor("gvColStockPurchasePriceNetamountbase").SetValue(DecimalRoundoff(parseFloat(StrRate) * parseFloat(grid.GetEditor("gvColTotalAmountINR").GetValue()), 2));

                    
                    Cur_TotalAmt = (grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() != null) ? grid.GetEditor('gvColStockPurchasePriceNetamountbase').GetValue() : "0";
                    Cur_TotalAmt_frgn = (grid.GetEditor('gvColTotalAmountINR').GetValue() != null) ? grid.GetEditor('gvColTotalAmountINR').GetValue() : "0";

                    CalculateAmount();          
                }
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
           
            var OrderTaggingData = cgridproducts.GetSelectedKeysOnPage();

            if(OrderTaggingData==0){ 
                grid.PerformCallback('BindGridOnQuotation' + '~' + '@'+'~'+$("#ddl_Currency :selected").text());
                cProductsPopup.Hide();
            }
            else{
                grid.PerformCallback('BindGridOnQuotation' + '~' + '@'+'~'+$("#ddl_Currency :selected").text());
                // cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
                $('#hdnPageStatus').val('Quoteupdate');
               
                var quote_Id = ctaggingGrid.GetSelectedKeysOnPage();
                if (quote_Id.length > 0) {
                    var ComponentDetails = _ComponentDetails.split("~");
                    cgridproducts.cpComponentDetails = null;

                    var ComponentNumber = ComponentDetails[0];
                    var ComponentDate = ComponentDetails[1];
        
                    var orderRefNumber = ComponentDetails[2];
                    var orderaccepRefDate = ComponentDetails[3];


                    ctaggingList.SetValue(ComponentNumber);
                    cPLQADate.SetValue(ComponentDate);
                    cPLQuoteDate.SetEnabled(false);

                    cPorderdate.SetValue(orderRefDate);
                    cPorderno.SetValue(orderRefNumber);

                }
                cProductsPopup.Hide();
            }
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

            //var quote_Id = gridquotationLookup.GetValue();
            //if (quote_Id != null) {
            //    var arr = quote_Id.split(',');
            //    if (arr.length > 1) {
            //        cPLQADate.SetText('Multiple Select Indent Dates');
            //    }
            //    else {
            //        if (arr.length == 1) {
            //            cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);
            //        }
            //        else {
            //            cPLQADate.SetText('');
            //        }
            //    }
            //}
            //else { cPLQADate.SetText(''); }
            var OrderData = ctaggingGrid.GetSelectedKeysOnPage();

            if(OrderData==0){
                cgridproducts.PerformCallback('BindProductsDetails');
                cpopup_taggingGrid.Hide();
                cProductsPopup.Show();
            }
            else{
                cgridproducts.PerformCallback('BindProductsDetails');
                cpopup_taggingGrid.Hide();
                cProductsPopup.Show();
            }

            //if (quote_Id != null) {
            //    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            //    cProductsPopup.Show();
            //}
            //else {
            //    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '$');
            //    cProductsPopup.Show();
            //}
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
            document.getElementById('btn_SaveRecords').style.display = 'none'
            document.getElementById('tagged').style.display = 'block'
        }

        function OnEndCallback(s, e) {

            
            if(grid.cpnull =="yes")
            {
                //alert(grid.cpDueDate);


                cddl_AmountAre.SetEnabled(false);
                ctxtVendorName.SetEnabled(false);    


                //Changes 20-06-2018 Sudip Pal
                $("#ddl_Currency").attr("disabled", true); 
                $("#txt_Rate_I").attr("disabled", true); 


                $("#ddl_Currency").val(grid.cpcurrencyId);
                cddl_AmountAre.SetValue(grid.cpTax_Option);
                ctxtRate.SetValue(grid.cpcurrencyrate);
                ctxtCreditDays.SetValue(grid.cpCreditDays);
                /// cdt_PODue.SetValue(grid.cpDueDate);
                var myDate = new Date(grid.cpDueDate);
                //cdt_PODue.setDate(myDate);


               

                ctxttotamtfrgn.SetValue(grid.cpTotalAmountfrgn);
                ctxttotamt.SetValue(grid.cpTotalAmountLocal);
                ctxtdocamt.SetValue(grid.cpTotaldocAmtLocal);

              
            }

            $("#lbltotalchargepordertax").html('['+$("#ddl_Currency :selected").text()+']');
            $("#lbltotalamountordertax").html('['+$("#ddl_Currency :selected").text()+']');
            $("#lbl_totalamtfrgn").html('Total Amount['+$("#ddl_Currency :selected").text()+']');
           
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
                grid.cpSaveSuccessOrFail=null;
                grid.batchEditApi.StartEdit(0, 2);
                jAlert('Can Not Add More Purchase Order Number as Purchase Order Scheme Exausted.<br />Update The Scheme and Try Again');
                //OnAddNewClick();
            }
            else if (grid.cpSaveSuccessOrFail == "duplicate") {
                grid.cpSaveSuccessOrFail=null;
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
            else if (grid.cpSaveSuccessOrFail == "UdfMandetory") {
                grid.cpSaveSuccessOrFail=null;
                grid.batchEditApi.StartEdit(0, 2);
                OnAddNewClick();
                jAlert('UDF is set as Mandatory.Please enter values.', 'Alert', function () { OpenUdf(); });
            }
            else if (grid.cpSaveSuccessOrFail == "transporteMandatory") {
                grid.cpSaveSuccessOrFail=null;
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
            else if (grid.cpSaveSuccessOrFail == "VendorAddressProblem") {
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
                jAlert('You must enter the default Billing/Shipping Address for selected Vendor to proceed further.');
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
                grid.cpSaveSuccessOrFail = null;
                OnAddNewClick();
               
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
            else {

                var PurchaseOrder_Number = grid.cpPurchaseOrderNo;
                var Order_Msg = "B/L No. " + PurchaseOrder_Number + " saved.";
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
                    if (PurchaseOrder_Number != "" && PurchaseOrder_Number!=null) {
                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                            if (r == true) {
                                grid.cpPurchaseOrderNo = null;
                                var newPorderId = grid.cpPurchaseOrderID;
                                //var reportName = "PO-Default~D";
                                //window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=Porder&id=' + newPorderId, '_blank')
                                grid.cpPurchaseOrderID = null;
                                window.location.assign("PurchaseorderBillofladingList.aspx");
                            }


                        });

                    }
                    else {
                        if (pageStatus != "delete") {                           
                            window.location.assign("PurchaseorderBillofladingList.aspx");
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
                    if (PurchaseOrder_Number != ""&& PurchaseOrder_Number!=null) {
                        jAlert(Order_Msg, 'Alert Dialog: [PurchaseOrder]', function (r) {

                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                window.location.assign("PurchaseOrderAcceptance.aspx?key=ADD");
                            }
                        });
                    }
                    else {
                        if (pageStatus != "delete") {                           
                            window.location.assign("PurchaseOrderAcceptance.aspx?key=ADD");
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

    if (ctaggingList.GetValue() != null && ctaggingList.GetValue()!="") {
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


            //ctxtRate.SetEnabled(false);
            //ctxtRate.SetEnabled(false);
            //$("#ddl_AmountAre").attr("disabled", "disabled");
            //$("#ddl_Currency").attr("disabled", "disabled");
}

function GridCallBack() {
    //page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
    ///  grid.PerformCallback('Display');
    grid.PerformCallback('Display~'+$("#ddl_Currency :selected").text());

}
function OnCustomButtonClick(s, e) {

    if (e.buttonID == 'CustomDelete') {
        grid.batchEditApi.EndEdit();
       
        if (ctaggingList.GetValue() != null) {         
            jAlert('Cannot Delete using this button as the Purchase Indent is linked with this Purchase Order.<br/>Click on Plus(+) sign to Add or Delete Product from last column.!', 'Alert Dialog', function (r) {

            });
        }
        var noofvisiblerows = grid.GetVisibleRowsOnPage();       
        if (noofvisiblerows != "1" && ctaggingList.GetValue() == null) {
            grid.DeleteRow(e.visibleIndex);
            $('#<%=hdfIsDelete.ClientID %>').val('D');
            grid.UpdateEdit();
            grid.PerformCallback('Display~'+$("#ddl_Currency :selected").text());
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
                    url: 'PurchaseOrder-Import.aspx/getProductType',
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
}
function Save_ButtonClick() {

    $("#HDNumberingschema").val($("#ddl_numberingScheme").val());
    $("#HDNbranch").val($("#ddl_Branch").val());
    $("#HDNtaxtypeamt").val(cddl_AmountAre.GetValue());

    ///  alert($("#HDNumberingschema").val());


    LoadingPanel.Show();
    flag = true;
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
        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
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
}
function SaveExit_ButtonClick() {

    
    $("#HDNumberingschema").val($("#ddl_numberingScheme").val());
    $("#HDNbranch").val($("#ddl_Branch").val());
    $("#HDNtaxtypeamt").val(cddl_AmountAre.GetValue());
    LoadingPanel.Show();


    flag = true;
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
        if (grid.GetVisibleRowsOnPage() > 0) {
            if (IsType == "Y") {
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


                //  alert(  $("#HDNumberingschema").val());
                grid.UpdateEdit();
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
}
function OnAddNewClick() {
    //if (gridquotationLookup.GetValue() == null) {
    //    grid.AddNewRow();
    //    var noofvisiblerows = grid.GetVisibleRowsOnPage(); 
    //    var i;
    //    var cnt = 1;
    //    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
    //        var tbQuotation = grid.GetEditor("SrlNo");
    //        tbQuotation.SetValue(cnt);


    //        cnt++;
    //    }
    //}
    //else {
    //    QuotationNumberChanged();
    //}
    grid.AddNewRow();
    var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var i;
    var cnt = 1;
    for (i = -1 ; cnt <= noofvisiblerows ; i--) {
        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);


        cnt++;
    }

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


 
    //Changes 20-06-2018 Sudip Pal
    grid.PerformCallback('GridForeignCurrency~'+$("#ddl_Currency :selected").text());

    $("#lbltotalchargepordertax").html('['+$("#ddl_Currency :selected").text()+']');
    $("#lbltotalamountordertax").html('['+$("#ddl_Currency :selected").text()+']');
    $("#lbl_totalamtfrgn").html('Total Amount['+$("#ddl_Currency :selected").text()+']');

    //Changes 20-06-2018 Sudip Pal


    if ($("#ddl_Currency").val() == basedCurrency[0]) {
        ctxtRate.SetValue("");
        ctxtRate.SetEnabled(false);
    }
    else {
        $.ajax({
            type: "POST",
            url: "PurchaseOrderAcceptance.aspx/GetRate",
            data: JSON.stringify({ Currency_ID: Currency_ID, Campany_ID: Campany_ID, basedCurrency: basedCurrency[0] ,Dateposting:cPLQuoteDate.GetDate()}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                var data = msg.d;
                if(data.Ismatched==3)
                {
                    jAlert('No rates found.');
                    ctxtRate.SetValue('');
                }
                else
                {


                    jConfirm('Would you like to proceed with last currency rate from Master? Click Ok  to proceed or Cancel to manually enter the currency rate.','',function(r){
                
                        if (r == true) {

                       
                            ctxtRate.SetValue(data.PurchaseRate);
                        

                        }
                        ctxtRate.Focus();
                        PopulateGSTCSTVAT();
                
                    })
                }

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
    // cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

    grid.batchEditApi.StartEdit(-1, 1);
    var accountingDataMin = grid.GetEditor('ProductName').GetValue();
    grid.batchEditApi.EndEdit();

    grid.batchEditApi.StartEdit(0, 1);
    var accountingDataplus = grid.GetEditor('ProductName').GetValue();

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
    ///   cQuotationComponentPanel.PerformCallback('BindIndentGrid' + '~' + PODate);

}

function GetContactPersonPhone(e) {
    var key = cContactPerson.GetValue();
    cacpContactPersonPhone.PerformCallback('ContactPersonPhone~' + key);
}
      
function GetContactPerson() {           
    var key = GetObjectID('hdnCustomerId').value;          
    if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '') {
        page.GetTabByName('Billing/Shipping').SetEnabled(true);
        /// LoadCustomerAddress(key, $('#ddl_Branch').val(), 'PO');              
        page.tabs[0].SetEnabled(true);
        page.tabs[1].SetEnabled(true);
        GetVendorGSTInFromBillShip(key);
    }
}

function ShowIndntRequisition() {

}

function cmbContactPersonEndCall(s, e) {

            <%--if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);

                var invoiceDate = new Date();
                var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

                ctxtCreditDays.SetValue(datediff);

                cdt_PODue.SetDate(myDate);
                cContactPerson.cpDueDate = null;
            }
            if (cContactPerson.cpVendorCountryID != null) {
                var CountryID = cContactPerson.cpVendorCountryID;
                if (CountryID != "1") {
                    cddl_AmountAre.SetValue("4");
                    cddl_AmountAre.SetEnabled(false);
                }
                else
                {
                    cddl_AmountAre.SetValue("1");
                    cddl_AmountAre.SetEnabled(true);
                }
            }

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
            }--%>
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

            //#### added by Sayan Dutta for TC Purchase Order Specefic Fields #######################
            var key = GetObjectID('hdnCustomerId').value;
            if (key != null && key != "") {
                if ($("#btn_TermsCondition").is(":visible")) {
                    callTCspecefiFields_PO(key);
                }
            }
            //#### End : added by Sayan Dutta for TC Purchase Order Specefic Fields : End #############
            $('#ProductModel').on('shown.bs.modal', function () {
                $('#txtProdSearch').focus();
            })
            //$('#ProductModel').on('hide.bs.modal', function () {
            //    grid.StartEditRow(globalRowIndex);
            //    grid.batchEditApi.StartEdit(globalRowIndex, 3);
            //})
            var schemaid = $('#ddl_numberingScheme').val();
            if (schemaid != null) {
                if (schemaid == '0') {
                    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                }
            }
             <%-- region Sandip Section For Approval Section Start--%>
            $('#ApprovalCross').click(function () {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh();
            })
            <%-- endregion Sandip Section For Approval Dtl Section End--%>
        });
        function CmbScheme_ValueChange() {
            var val = $("#ddl_numberingScheme").val();
            ctxtVendorName.SetText("");
            GetObjectID('hdnCustomerId').value = "";
            page.tabs[1].SetEnabled(false);

            var schemetypeValue = val;
            var schemetype = schemetypeValue.toString().split('~')[1];
            var schemelength = schemetypeValue.toString().split('~')[2];
            var branchID = (schemetypeValue.toString().split('~')[3] != null) ? schemetypeValue.toString().split('~')[3] : "";
            $("#hdnTCBranchId").val(branchID);
            if (branchID != "") document.getElementById('ddl_Branch').value = branchID;
            document.getElementById('<%= ddl_Branch.ClientID %>').disabled = true;
            $('#txtVoucherNo').attr('maxLength', schemelength);


            var fromdate = schemetypeValue.toString().split('~')[4];
            var todate = schemetypeValue.toString().split('~')[5];

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
    SetPurchaseBillingShippingAddress( $('#ddl_Branch').val());
            //GetIndentReqNoOnLoad();
}
function IndentRequisitionNo_ValueChange() {

    var val = $("#ddl_IndentRequisitionNo").val();
    if (val != 0) {
        $.ajax({
            type: "POST",
            url: 'PurchaseOrder-Import.aspx/getIndentRequisitionDate',
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
            var urlKeys = getUrlVars();
            if (urlKeys.key != 'ADD') {
                if(cddl_AmountAre.GetValue()=="3")
                {                    
                    grid.GetEditor('gvColTaxAmount').SetEnabled(false);
                }
            }
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
                if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") {

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
                else if (event.keyCode == 79 && event.altKey == true) { //run code for Ctrl+O -- ie, Billing/Shipping Samrat!     
                    StopDefaultAction(e);
                    if (page.GetActiveTabIndex() == 1) {
                        fnSaveBillingShipping();
                    }
                }
                else if (event.keyCode == 77 && event.altKey == true) {
                    $('#TermsConditionseModal').modal({
                        show: 'true'
                    });
                }
                else if (event.keyCode == 69 && event.altKey == true) {
                    if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                        StopDefaultAction(e);
                        SaveTermsConditionData();
                    }
                }
                else if (event.keyCode == 76 && event.altKey == true) {
                    StopDefaultAction(e);
                    calcelbuttonclick();
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

        function DataPopulatedWarehouseGrid()
        {
            var WarehouseID = cCmbWarehouse.GetValue();
            var WarehouseName = cCmbWarehouse.GetText();
            var qnty = ctxtqnty.GetText();
            var IsSerial = $('#hdnisserial').val();     
            if (qnty == "0.0000") {
                qnty = ctxtbatchqnty.GetText();
            }
            if (Number(qnty) % 1 != 0 && IsSerial == "true") {
                jAlert("Serial number is activated, Quantity should not contain decimals. ");
                return;
            }           
            var BatchName = ctxtbatch.GetText();
            var SerialName = ctxtserial.GetText();
            var Isbatch = $('#hdnisbatch').val();
            var enterdqntity = $('#hdfopeningstockPC').val();
            var hdniswarehouse = $('#hdniswarehouse').val();
            var ISupdate = $('#hdnisoldupdate').val();
            var isnewupdate = $('#hdnisnewupdate').val();          
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
                        ctxtbatchqnty.Focus();
                    }
                }
                if (qnty == "0.0") {
                    if (Isbatch != "false" || hdniswarehouse != "false") {
                        $("#RequiredFieldValidatortxtbatchqntity").css("display", "block");
                        $("#RequiredFieldValidatortxtwareqntity").css("display", "block");                         
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
                    if (Number(qnty) > (Number(enterdqntity) + Number(deletedquantity)) && hdnisediteds == "false") {
                        qnty = "0.00";
                        jAlert("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                    }
                    else {
                        cGrdWarehousePC.PerformCallback('Display~' + WarehouseID + '~' + WarehouseName + '~' + BatchName + '~' + SerialName + '~' + qnty);                         
                        cCmbWarehouse.Focus();
                    }
                }                 
                return false;
            }
    }

    function SaveWarehouse() {            
        var IsSerial = $('#hdnisserial').val(); 
        if (IsSerial == "true") 
        {
            var SerialNo = ctxtserial.GetText();
            var ProductID = $('#hdfProductIDPC').val(); 
            var Branch = $('#ddl_Branch').val();
            var objectToPass = {}
            objectToPass.SerialNo = SerialNo;
            objectToPass.ProductID = ProductID;
            objectToPass.BranchID = Branch;
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/CheckDuplicateSerial",
                data: JSON.stringify(objectToPass),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if(msg.d==0){
                        DataPopulatedWarehouseGrid();
                    }
                    else{
                        ctxtserial.SetText("");
                        jAlert("Duplicate Serial No. entered. Cannot proceed.", "Alert", function () { ctxtserial.Focus(); });        
                    }
                }
            });
        }
        else
        {
            DataPopulatedWarehouseGrid();
        }
    }

      

    function SaveWarehouseAll() {         
        cGrdWarehousePC.PerformCallback('Saveall~');
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
    function ddlInventory_OnChange() {

        if ($('#ddlInventory').val() == 'Y') {
            ctaggingList.SetEnabled(true);
            //gridquotationLookup.SetEnabled(true);
            //document.getElementById('indentRequisition').style.display = 'block'
        }
        else {
            ctaggingList.SetEnabled(false);
            //gridquotationLookup.SetEnabled(false);
            //document.getElementById('indentRequisition').style.display = 'none'
        }
        //cproductLookUp.GetGridView().Refresh();
        var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Unique Id</th><th>Vendor Name</th></tr><table>";
        document.getElementById("CustomerTable").innerHTML = txt;
        
        //var _txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th><th>Installation Reqd.</th></tr><table>";
        var _txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Product Code</th><th>Product Name</th><th>Inventory</th><th>HSN/SAC</th><th>Class</th><th>Brand</th></tr><table>";
        document.getElementById("ProductTable").innerHTML = _txt
    }
    </script>

    <%--   Warehouse Script End    --%>
    <%-- New Billing/Shipping Section --%>

    <script>
        $(document).ready(function () {
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('Billing/Shipping').SetEnabled(false);
            }          
        })
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }
        function disp_prompt(name) {
            if (name == "tab0") {
                //gridLookup.Focus();               
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
                }
            }
        }

    </script>


    <%--New Billing/Shipping Section End--%>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="Import/JS/SearchPopup.js"></script>

    <div class="panel-heading">

        <div class="panel-title clearfix">

            <h3 class="pull-left">
                <span class="">
                    <asp:Label ID="lblHeading" runat="server" Text="Add Bill of Lading(B/L)"></asp:Label>
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

                                            <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label>

                                        </td>

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
                                            <asp:Label ID="lblTotalPayable" runat="server" Text="0.0" CssClass="classout"></asp:Label>
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
                <a href="PurchaseorderBillofladingList.aspx"><i class="fa fa-times"></i></a>
            </div>

            <%-- endregion Sandip Section For Approval Dtl Section End--%>
        </div>

    </div>

    <div class="form_main">
        <div class="row">

            <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">

                <tabpages>

                    <dxe:TabPage Name="General" Text="General">

                        <ContentCollection>

                            <dxe:ContentControl runat="server">

                                <div class="row">
                                    <div class="col-md-2">

                                        <dxe:ASPxLabel ID="lbl_Inventory" runat="server" Text="Inventory Item?">
                                        </dxe:ASPxLabel>

                                        <asp:DropDownList ID="ddlInventory" CssClass="backSelect" runat="server"  TabIndex="0" Width="100%" onchange="ddlInventory_OnChange()">
                                          
                                        <asp:ListItem Text="Inventory Item" Value="Y" />
                                         
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-2" runat="server" id="divNumberingScheme">
                                        <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%" TabIndex="1"
                                            DataTextField="SchemaName" DataValueField="Id" onchange="CmbScheme_ValueChange()">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()" TabIndex="2">                             
                                        </asp:TextBox>
                                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>

                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date">
                                        </dxe:ASPxLabel>

                                        <span style="color: red;">*</span>
                                        
                                         <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" TabIndex="3"
                                            ClientInstanceName="cPLQuoteDate" Width="100%" UseMaskBehavior="True">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                            <ClientSideEvents DateChanged="function(s, e) { TDateChange(e)}" GotFocus="function(s,e){cPLQuoteDate.ShowDropDown();}" />
                                        </dxe:ASPxDateEdit>

                                        <span id="MandatoryDate" class="PODate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>


                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Unit">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" DataSourceID="DS_Branch" 
                                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" onchange="onBranchItems()">
                                        </asp:DropDownList>
                                    </div>
                                    
                                    <div class="col-md-2">
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Vendor">
                                        </dxe:ASPxLabel>
                                        <span style="color: red;">*</span>
                                        <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%" TabIndex="4">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>

                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                        
                                        <span id="MandatorysVendor" class="POVendor  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                    </div>
                                  <div style="clear: both"></div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" TabIndex="5" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px" ClientSideEvents-EndCallback="cmbContactPersonEndCall">
                                          
                                        </dxe:ASPxComboBox>
                                    </div>
                                      
                                    <div class="col-md-2 lblmTop8" id="indentRequisition" runat="server">
                                         <dxe:ASPxLabel ID="lbl_IndentRequisition" runat="server" Text="Ref O/A No">
                                        </dxe:ASPxLabel>

                                        <dxe:ASPxButtonEdit ID="taggingList" ClientInstanceName="ctaggingList" runat="server"  Width="100%" TabIndex="6">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="taggingListButnClick" KeyDown="taggingListKeyDown" />
                                        </dxe:ASPxButtonEdit>

                                        <dxe:ASPxPopupControl ID="popup_taggingGrid" runat="server" ClientInstanceName="cpopup_taggingGrid"
                                            HeaderText="Select O/A Number" PopupHorizontalAlign="WindowCenter"
                                            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Height="400px" Width="850px"
                                            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                                            ContentStyle-CssClass="pad">
                                            <ContentStyle VerticalAlign="Top" CssClass="pad">
                                            </ContentStyle>
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">

                                                    <div style="padding: 7px 0;">
                                                      <%--  <input type="button" value="Select All Products" onclick="Tag_ChangeState('SelectAll')" class="btn btn-primary" ></input>
                                                        <input type="button" value="De-select All Products" onclick="Tag_ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                                                        <input type="button" value="Revert" onclick="Tag_ChangeState('Revart')" class="btn btn-primary"></input>--%>
                                                    </div>

                                                    <div>

                                                        <dxe:ASPxGridView ID="taggingGrid" ClientInstanceName="ctaggingGrid" runat="server" KeyFieldName="PurchaseOrder_Id"
                                                            Width="100%" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" 
                                                            Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                                                            OnCustomCallback="taggingGrid_CustomCallback" OnDataBinding="taggingGrid_DataBinding">
                                                            <SettingsBehavior AllowDragDrop="False" AllowSelectSingleRowOnly="true"></SettingsBehavior>
                                                            <SettingsPager Visible="false"></SettingsPager>
                                                         
                                                            <Columns>


                                                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />

                                                                  <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Unit" Width="100" VisibleIndex="1">
                                                                </dxe:GridViewDataTextColumn>

                                                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Number" Caption="Order Acceptance Number" Width="150" VisibleIndex="2">
                                                                </dxe:GridViewDataTextColumn>

                                                              

                                                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Date_RequisitionDate" Caption="Order Acceptance Date" Width="150" VisibleIndex="3">
                                                                </dxe:GridViewDataTextColumn>

                                                                  <dxe:GridViewDataTextColumn FieldName="RefOrder" Caption="Ref Purchase Order No" Width="150" VisibleIndex="4">
                                                                </dxe:GridViewDataTextColumn>


                                                                <dxe:GridViewDataTextColumn FieldName="PurchaseOrder_Date" Caption="Ref Purchase Order Date" Width="150" VisibleIndex="5">
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

                                                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Quotation_Num" Width="90" ReadOnly="true" Caption="Indent No">
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
                                        <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Ref O/A Date" TabIndex="7">
                                        </dxe:ASPxLabel>
                                        <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
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


                                    <div class="col-md-2 lblmTop8">

                                                <dxe:ASPxLabel ID="lbl_purchaseorderno" runat="server" Text="Ref Purchase Order No.">
                                                </dxe:ASPxLabel>


                                         <dxe:ASPxTextBox ID="txt_orderno" runat="server" TabIndex="9" Width="100%" ClientEnabled="false" ClientInstanceName="cPorderno">
                                                        </dxe:ASPxTextBox>


                                            </div>


                                    <div class="col-md-2 lblmTop8">

                                                <dxe:ASPxLabel ID="lbl_purchaseoredrdate" runat="server" Text="Ref Purchase Order Date">
                                                </dxe:ASPxLabel>

                               
                                            <dxe:ASPxTextBox ID="txt_orderdate" runat="server" TabIndex="10" Width="100%" ClientEnabled="false" ClientInstanceName="cPorderdate">
                                                        </dxe:ASPxTextBox>

                                        </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Vessel/Voyage No">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txtvessalno" runat="server" Width="100%" ClientInstanceName="ctxt_vessaleNo" TabIndex="11">
                                        </dxe:ASPxTextBox>
                                    </div>
                                         <div class="clear"></div>
                                                <div class="col-md-2 lblmTop8">

                                        <dxe:ASPxLabel ID="lbl_container" runat="server" Text="Container No.">
                                        </dxe:ASPxLabel>

                                           <dxe:ASPxTextBox ID="txt_container" ClientInstanceName="ctxtContainer" runat="server" TabIndex="12" Width="100%">
                                           </dxe:ASPxTextBox>

                                    </div>

                                   

                                  <div class="col-md-2 lblmTop8">

                                        <dxe:ASPxLabel ID="lbl_portofloadng" runat="server" Text="Port Of Loading">
                                        </dxe:ASPxLabel>
                                       
                                        <asp:DropDownList ID="ddlportloading" runat="server" Width="100%" TabIndex="13"
                                            DataSourceID="portofloadingdispatch" DataValueField="Port_Id"
                                            DataTextField="Port_Description" >
                                        </asp:DropDownList>

                                    </div>
                                  
                                   <div class="col-md-2 lblmTop8">

                                        <dxe:ASPxLabel ID="lbl_dispatch" runat="server" Text="Port Of Dispatch">
                                        </dxe:ASPxLabel>

                                        <asp:DropDownList ID="ddlportdistach" runat="server" Width="100%" TabIndex="14"
                                            DataSourceID="portofloadingdispatch" DataValueField="Port_Id"
                                            DataTextField="Port_Description" >
                                        </asp:DropDownList>

                                    </div>


                        

                                   <div class="col-md-2 lblmTop8">

                                        <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Tracking">
                                        </dxe:ASPxLabel>



                                       <input type="url" name="url" id="txttracking" runat="server" style="width:100%"  tabIndex="15" placeholder="https://example.com" pattern="https://.*" size="20" required />

                                    </div>

                                     <div class="col-md-4 lblmTop8 ">
                                        <div class="row">
                                            <div class="col-md-6 lblmTop8">
                                                <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Credit Days" TabIndex="16">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxTextBox ID="txtCreditDays" ClientInstanceName="ctxtCreditDays" runat="server" Width="100%" TabIndex="16">
                                                    <MaskSettings Mask="<0..999999999>" AllowMouseWheel="false" />
                                                    <ClientSideEvents TextChanged="CreditDays_TextChanged" />
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div class="col-md-6 lblmTop8">
                                                <dxe:ASPxLabel ID="lbl_DueDate" runat="server" Text="Due Date">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxDateEdit ID="dt_PODue" TabIndex="17" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy"
                                                    UseMaskBehavior="True" ClientInstanceName="cdt_PODue"
                                                    Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <ClientSideEvents GotFocus="function(s,e){cdt_PODue.ShowDropDown();}" LostFocus="function(s, e) { SetFocusonGrid(e)}" />
                                                </dxe:ASPxDateEdit>

                                                <span id="MandatoryDueDate" class="PODueDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                            </div>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                       
                                    


                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Currency" runat="server" Text="Currency">
                                        </dxe:ASPxLabel>
                                        <asp:DropDownList ID="ddl_Currency" runat="server" Width="100%" TabIndex="18"
                                            DataSourceID="SqlCurrency" DataValueField="Currency_ID"
                                            DataTextField="Currency_AlphaCode" onchange="ddl_Currency_Rate_Change()">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Rate" runat="server" Text="Rate">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Rate" runat="server" Width="100%" ClientInstanceName="ctxtRate" TabIndex="19">
                                            <MaskSettings Mask="<0..999999999999999999>.<00000..99999>" AllowMouseWheel="false" />
                                            <ClientSideEvents LostFocus="ReBindGrid_Currency"  />
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" TabIndex="20" ClientIDMode="Static" ClientInstanceName="cddl_AmountAre" Width="100%" Native="true">
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
                                 

                                    <div class="col-md-2 lblmTop8">
                                        <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                        </dxe:ASPxLabel>
                                        <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" ClientInstanceName="ctxt_Refference" TabIndex="21">
                                        </dxe:ASPxTextBox>
                                    </div>



                                    <div style="clear: both;"></div>
                                    <div class="col-md-12">
                                        <dxe:ASPxGridView runat="server" OnBatchUpdate="grid_BatchUpdate" KeyFieldName="OrderDetails_Id" ClientInstanceName="grid" ID="grid"
                                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                                            OnCellEditorInitialize="grid_CellEditorInitialize"    
                                            Settings-ShowFooter="false" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding"
                                            OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting"
                                            OnHtmlRowPrepared="grid_HtmlRowPrepared" TabIndex="22"
                                            Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="150" RowHeight="2" SettingsPager-Mode="ShowAllRecords">
                                            <SettingsPager Visible="false"></SettingsPager>

                                            <Columns>

                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="0"  VisibleIndex="0"
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

                                                <dxe:GridViewDataTextColumn Caption="Indent" FieldName="Indent_Num" ReadOnly="True" Width="6%" VisibleIndex="2" visible="false" >
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


                                                <dxe:GridViewDataTextColumn FieldName="gvColProduct" Caption="hidden Field Id" VisibleIndex="19" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <%--Batch Product Popup End--%>

                                                <dxe:GridViewDataTextColumn FieldName="gvColDiscription" Caption="Description" VisibleIndex="4" Width="18%" ReadOnly="True">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="True"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="gvColQuantity" Caption="Quantity" VisibleIndex="5" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />
                                               <%--         <ClientSideEvents LostFocus="QuantityTextChange" GotFocus="QuantityProductsGotFocus" />--%>
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="gvColUOM" Caption="UOM" VisibleIndex="6" Width="5%" ReadOnly="True">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewCommandColumn Width="7%" VisibleIndex="7" Caption="Stk Details" visible="false">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomWarehouse" Image-Url="/assests/images/warehouse.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>

                                                <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePrice" Caption="Price" VisibleIndex="9" Width="9%" HeaderStyle-HorizontalAlign="Right" >
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;000..999&gt;" AllowMouseWheel="false" />
                                                        <ClientSideEvents LostFocus="PurchasePriceTextChange" GotFocus="PurchasePriceTextFocus" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePriceforeign" Caption="Price" VisibleIndex="8" Width="9%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.000">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;000..999&gt;" AllowMouseWheel="false" />
                                                   <ClientSideEvents LostFocus="PurchasePriceTextChangefrgn" GotFocus="PurchasePriceTextFocusfrgn" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="gvColDiscount" Caption="Disc(%)" VisibleIndex="10" Width="6%" HeaderStyle-HorizontalAlign="Right" >
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                        <ClientSideEvents LostFocus="DiscountValueChange" GotFocus="DiscountTextFocus" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>



                                                <dxe:GridViewDataTextColumn FieldName="gvColAmount" Caption="Amount" VisibleIndex="11" Width="6%" HeaderStyle-HorizontalAlign="Right" readonly="true">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                               

                                                <dxe:GridViewDataTextColumn FieldName="gvColAmountbase" Caption="Amount" VisibleIndex="12" Width="6%" HeaderStyle-HorizontalAlign="Right" readonly="true">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>



                                                <dxe:GridViewDataButtonEditColumn FieldName="gvColTaxAmount" Caption="Charges" VisibleIndex="13" Width="6%" ReadOnly="True"
                                                    HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesButtonEdit Style-HorizontalAlign="Right">
                                                        <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataButtonEditColumn>


                                                <dxe:GridViewDataTextColumn FieldName="gvColTotalAmountINR" Caption="Net Amt" VisibleIndex="14" Width="8%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                               <%--  Changes 19-06-2018 Sudip pal--%>

                                               <dxe:GridViewDataTextColumn FieldName="gvColStockPurchasePriceNetamountbase" Caption="Net Amount" VisibleIndex="15" Width="8%" HeaderStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                    </PropertiesTextEdit>
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <%--  Changes 19-06-2018 Sudip pal--%>


                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" visible="false" ShowNewButtonInHeader="false" Width="7%" VisibleIndex="16" Caption="Add New">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>


                                                <dxe:GridViewDataTextColumn Caption="Quotation No" FieldName="Indent" Width="0" VisibleIndex="17">
                                                    <PropertiesTextEdit>
                                                        <NullTextStyle></NullTextStyle>
                                                        <ReadOnlyStyle></ReadOnlyStyle>
                                                        <Style></Style>
                                                    </PropertiesTextEdit>
                                                    <HeaderStyle />
                                                    <CellStyle>
                                                    </CellStyle>
                                                </dxe:GridViewDataTextColumn>


                                                <dxe:GridViewDataTextColumn FieldName="IsComponentProduct" Caption="IsComponentProduct" Width="0">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="IsLinkedProduct" Caption="IsLinkedProduct" Width="0">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="gvColStockQty" Caption="Stock Qty" Width="0">
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
                                                    Width="0">
                                                    <PropertiesTextEdit>
                                                        <NullTextStyle></NullTextStyle>
                                                        <ReadOnlyStyle></ReadOnlyStyle>
                                                        <Style></Style>
                                                    </PropertiesTextEdit>
                                                    <HeaderStyle />
                                                    <CellStyle></CellStyle>

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

                                    <div class="col-md-12 pdTop15">
                                        <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                        <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" TabIndex="24" runat="server" AutoPostBack="False" Text="S&#818;ave & New" 
                                            CssClass="btn btn-primary"  UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtn_SaveRecordExits" runat="server" TabIndex="25" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" visible="false" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                        </dxe:ASPxButton>
                                        <dxe:ASPxButton ID="btn_SaveRecordTaxs" ClientInstanceName="cbtn_SaveRecordTaxs"  TabIndex="26" runat="server" AutoPostBack="False" Text="T&#818;axes" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                        </dxe:ASPxButton>
                                        <b><span id="tagged" style="display: none; color: red" runat="server">This Purchase Order is tagged in other modules. Cannot Modify data except UDF</span></b>
                                        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />

                                        <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" TabIndex="28" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                        <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="PO" />

                                        <%--Changes 19-06-2018 Sudip pal--%>

                                        <div style="display:none;">
                                        <asp:CheckBox ID="chkmail" runat="server" Text="Send Mail" Visible="false" />
                                        </div>

                                        <%--Changes 19-06-2018 Sudip pal--%>

                                        <div style="float:right">

                                            <table>
                                                <tr>

                                                   <td style="padding-right:15px">

                                                        <dxe:ASPxLabel ID="lbl_totalamtfrgn" runat="server" >
                                                        </dxe:ASPxLabel> : 

                                                    </td>

                                                    <td>

                                                        <dxe:ASPxTextBox ID="txt_totamont_foreign" runat="server" Width="120px" ClientInstanceName="ctxttotamtfrgn" readonly="true">
                                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                            <ClientSideEvents />
                                                        </dxe:ASPxTextBox>

                                                    </td>
                                                    
                                                    <td style="padding-right:15px">
                                                        &nbsp;&nbsp;
                                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" >
                                                        </dxe:ASPxLabel> : 

                                                    </td>

                                                    <td>

                                                        <dxe:ASPxTextBox ID="txt_totamont" runat="server" Width="120px" ClientInstanceName="ctxttotamt" readonly="true">
                                                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                                            <%--<ClientSideEvents LostFocus="ReBindGrid_Currency" />--%>
                                                        </dxe:ASPxTextBox>

                                                    </td>


                                                </tr>
                                            </table>
                                            
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

                </tabpages>

                <clientsideevents activetabchanged="function(s, e) {
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


	                                            }">

                </clientsideevents>

            </dxe:ASPxPageControl>
        </div>

        <div class="clearfix">
            <table class="pull-right mTop5">
                <tr>
                    <td style="padding-right: 15px">

                        <dxe:ASPxLabel ID="ASPxLabel3" runat="server">
                        </dxe:ASPxLabel>

                    </td>
                    <td>

                        <dxe:ASPxTextBox ID="txt_docnetamt" runat="server" Width="120px" ClientInstanceName="ctxtdocamt">
                            <masksettings mask="&lt;0..999999999&gt;.&lt;00..99&gt;" allowmousewheel="false" />
                            <%-- <clientsideevents lostfocus="ReBindGrid_Currency" />--%>
                        </dxe:ASPxTextBox>

                    </td>
                </tr>
            </table>
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

        <dxe:ASPxPopupControl ID="aspxTaxpopUp" runat="server" ClientInstanceName="caspxTaxpopUp"
            Width="850px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">

            <headertemplate>
                <span style="color: #fff"><strong>Select Tax</strong></span>
                <dxe:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cgridTax.CancelEdit();
                                                            caspxTaxpopUp.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </headertemplate>

            <contentcollection>
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
                            <div class="lblHolder" style="display:none;">
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

                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="calCulatedOn" ReadOnly="true" Caption="Calculated On" width="0">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                     <%--   Changes 18-06-2018 Sudip Pal--%>

                                         <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="calCulatedOnInr" ReadOnly="true" Caption="Calculated On">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <%--   Changes 18-06-2018 Sudip Pal--%>
                  
                                        <dxe:GridViewDataTextColumn Caption="Percentage" FieldName="TaxField" VisibleIndex="5">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <ClientSideEvents LostFocus="txtPercentageLostFocus" GotFocus="CmbtaxClick" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Amount" Caption="Amount" ReadOnly="true" width="0">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>


                                            <%--   Changes 18-06-2018 Sudip Pal  --%>

                                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="AmountInr" Caption="Amount" ReadOnly="true">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left">
                                                <ClientSideEvents LostFocus="taxAmountLostFocus" GotFocus="taxAmountGotFocus" />
                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <CellStyle Wrap="False" HorizontalAlign="Left"></CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                            <%--   Changes 18-06-2018 Sudip Pal  --%>


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
                                <table class="InlineTaxClass" style="display:none;">
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
                                <div class="pull-left" style="margin-top: 20px;">
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

                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges(<label id="lblbasecurrency" runat="server"></label>)</strong></td>

                                      <%--  Changes  19-06-2018 Sudip Pal--%>

                                        <td>

                                                <dxe:ASPxTextBox ID="txtTaxTotAmtInr" MaxLength="80" ClientInstanceName="ctxtTaxTotAmtInr" Text="0.00" ReadOnly="true"
                                                runat="server" Width="100%" CssClass="pull-left mTop">

                                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                                          
                                                </dxe:ASPxTextBox>

                                        </td>
                                    </tr><tr>
                                        <%--  Changes  19-06-2018 Sudip Pal--%>

                                        <td style="padding-top: 10px; padding-right: 5px"><strong>Total Charges</strong> <span id="span_foreign"></span> </td>

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
            </contentcollection>
            <contentstyle verticalalign="Top" cssclass="pad"></contentstyle>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
            <panelcollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </panelcollection>
            <clientsideevents endcallback="ctaxUpdatePanelEndCall" />
        </dxe:ASPxCallbackPanel>
        <asp:HiddenField ID="HdChargeProdAmt" runat="server" />
        <asp:HiddenField ID="HdChargeProdNetAmt" runat="server" />
        <%--ChargesTax--%>

        <dxe:ASPxPopupControl ID="Popup_Taxes" runat="server" ClientInstanceName="cPopup_Taxes"
            Width="900px" Height="300px" HeaderText="Purchase order Taxes" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
            <contentcollection>
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
                                        <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            <MaskSettings Mask="<0..999999999999999999>.<0..99>" AllowMouseWheel="false" />
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
                                    <td style="padding-right: 30px"><strong>Total Charges</strong><label id="lbltotalchargepordertax" runat="server"></label></td>
                                    <td>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtQuoteTaxTotalAmt" runat="server" Width="100%" ClientInstanceName="ctxtQuoteTaxTotalAmt" Text="0.00" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                <MaskSettings Mask="<-999999999..99999999999999999999>.<0..99>" AllowMouseWheel="false" />
                                                <%-- <MaskSettings Mask="<0..999999999999g>.<0..99g>" />--%>
                                            </dxe:ASPxTextBox>
                                        </div>

                                    </td>
                                    <td style="padding-right: 30px; padding-left: 5px"><strong>Total Amount</strong><label id="lbltotalamountordertax" runat="server"></label></td>
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
            </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>
        <%--   Inline Tax End    --%>

        <%--   Warehouse     --%>
        <dxe:ASPxPopupControl ID="ASPxPopupControl3" runat="server" ClientInstanceName="cPopup_WarehousePC"
            Width="900px" HeaderText="Stock Details" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <contentstyle verticalalign="Top" cssclass="pad">
            </contentstyle>
            <contentcollection>
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
            </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
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

            <%--   Changes 14-06-2018 Sudip Pal--%>
            <asp:HiddenField runat="server" ID="HDNumberingschema" />
            <asp:HiddenField runat="server" ID="HDNbranch" />
            <asp:HiddenField runat="server" ID="HDNtaxtypeamt" />


            <%--   Changes 14-06-2018 Sudip Pal--%>
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
            <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </contentcollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--End UDF--%>
        <%--Batch Product Popup Start--%>





        <%--Batch Product Popup End--%>
        <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
            <panelcollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </panelcollection>
            <clientsideevents endcallback="acpAvailableStockEndCall" />
        </dxe:ASPxCallbackPanel>
        <dxe:ASPxCallbackPanel runat="server" ID="acpContactPersonPhone" ClientInstanceName="cacpContactPersonPhone" OnCallback="acpContactPersonPhone_Callback">
            <panelcollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </panelcollection>
            <clientsideevents endcallback="acpContactPersonPhoneEndCall" />
        </dxe:ASPxCallbackPanel>


        <asp:SqlDataSource ID="SqlSchematype" runat="server" 
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='17' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code=@year) and Branch=@userbranch and comapanyInt=@company)) as X Order By ID ASC">
            <SelectParameters>
                <asp:SessionParameter Name="userbranch" SessionField="userbranch" Type="string" />
                <asp:SessionParameter Name="company" SessionField="LastCompany1" Type="string" />
                <asp:SessionParameter Name="year" SessionField="LastFinYear1" Type="string" />

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

       <asp:SqlDataSource ID="portofloadingdispatch" runat="server"
            SelectCommand="select Port_Id,Port_Description from tbl_master_portcode Order By Port_Description"></asp:SqlDataSource>


        <asp:SqlDataSource ID="DS_Branch" runat="server" 
            SelectCommand=""></asp:SqlDataSource>
        <%-- <asp:SqlDataSource ID="DS_SalesAgent" runat="server" ConnectionString="<%$ ConnectionStrings:crmConnectionString %>"
            SelectCommand="select '0' as cnt_id,'Select' as Name
            union select cnt_id,isnull(cnt_firstName,'')+isnull(cnt_middleName,'')+isnull(cnt_lastName,'') Name from tbl_master_contact  where Substring(cnt_internalId,1,2)='AG'"></asp:SqlDataSource>--%>
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

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True">
    </dxe:ASPxLoadingPanel>


</asp:Content>




