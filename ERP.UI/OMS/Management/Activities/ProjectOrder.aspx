<%--/*************************************************************************************************************************************************
 *  Rev 1.0     Sanchita    V2.0.40     28-09-2023      Data Freeze Required for Project Sale Invoice & Project Purchase Invoice. Mantis:26854
 *************************************************************************************************************************************************/--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ProjectOrder.aspx.cs" Inherits="ERP.OMS.Management.Activities.ProjectOrder" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%--For Terms & Condition start --%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/TermsConditionsControl.ascx" TagPrefix="uc2" TagName="TermsConditionsControl" %>
<%--End--%>
<%@ Register Src="~/OMS/Management/Activities/UserControls/ProjectTermsConditions.ascx" TagPrefix="ucProject" TagName="ProjectTermsConditionsControl" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../../Tax%20Details/Js/TaxDetailsItemlevelNew.js?v=1.0.0" type="text/javascript"></script>

    <style>
        #lookup_Project .dxeButtonEditClearButton_PlasticBlue {
            display: table-cell;
        }

        #grid_DXMainTable > tbody > tr > td:nth-child(18) {
            display: none;
        }
    </style>
    <style type="text/css">
        #openlink {
            font-size: 18px;
        }
        /*BillingGstDiv*/
        .inline {
            display: inline !important;
        }


        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 1.5;
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

        #DivBilling [class^="col-md"], #DivShipping [class^="col-md"] {
            padding-top: 2px;
            padding-bottom: 2px;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .validclass {
            position: absolute;
            right: -4px;
            top: 23px;
        }

        .valid2 {
            position: absolute;
            right: -4px;
            top: 25px;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }

        #grid_DXMainTable > tbody > tr > td:nth-child(21) {
            display: none !important;
        }

        .dynamicPopupTbl > thead > tr > th {
            font-size: 12px;
            padding: 5px 6px;
        }
    </style>
    <script>
        var taxSchemeUpdatedDate = '<%=Convert.ToString(Cache["SchemeMaxDate"])%>';
        $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {
                grid.batchEditApi.StartEdit(globalRowIndex, 8);
            });
        });

    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script type="text/javascript">
        var canCallBack = true;
        var TaggingCall = false;

        // Rev 1.0
        function SetLostFocusonDemand(e) {
            if ((new Date($("#hdnLockFromDate").val()) <= cPLSalesOrderDate.GetDate()) && (cPLSalesOrderDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
                jAlert("DATA is Freezed between  " + $("#hdnDatafrom").val() + " to " + $("#hdnDatato").val());
            }
        }
        // End of Rev 1.0

        //Subhabrata
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function SalesManButnClick(s, e) {
            $('#SalesManModel').modal('show');
            $("#txtSalesManSearch").focus();
        }

        function SalesManbtnKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#SalesManModel').modal('show');
                $("#txtSalesManSearch").focus();
            }
        }

        function RateKeydown(s,event){
            if (event.htmlEvent.altKey ==true && event.htmlEvent.keyCode == 82) {
                var Product="0";
                var ProductID = grid.GetEditor('ProductID').GetText();
                if (ProductID != null && ProductID!="") {
                    var SpliteDetails = ProductID.split("||@||");
                    Product=SpliteDetails[0];
                    $.ajax({
                        type: "POST",
                        url: "ProjectOrder.aspx/GetLastRates",
                        data: JSON.stringify({ ProductID: Product }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async:false,
                        success: function (msg) {
                           
                            var lst=msg.d;
                            var str="";
                            if(lst.length>0)
                            {
                                for(var i=0;i<lst.length;i++){
                                    str+="<tr>";
                                    str+="<td>"+lst[i].Order_Number+"</td>";
                                    str+="<td>"+lst[i].Order_Date+"</td>";
                                    str+="<td>"+lst[i].cnt_firstName+"</td>";
                                    str+="<td>"+lst[i].OrderDetails_SalePrice+"</td>";
                                    str+="</tr>";

                                }
                            }
                            else{
                                str+="<tr>";
                                str+="<td colspan='4' class='text-center'>No Sale Rate Found.</td>";
                                str+="</tr>";
                            }
                            
                            $("#tbodyRate").html('');
                            $("#tbodyRate").html(str);
                            $("#LastRateModal").modal('show');
                        }
                    });
                }
                else{
                    jAlert('Please select a valid product to get sales rate.','Alert');
                }

            }
        }
        
        function AllControlInitilize() {
           
    
            if (canCallBack) {

                grid.AddNewRow();
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                grid.batchEditApi.EndEdit();
                $('#ddlInventory').focus();
                canCallBack = false;
                //Edited By Chinmoy 
                //start
                //$('#openlink').hide();
                PopulatePosGst();
                LoadtBillingShippingCustomerAddress($('#hdnCustomerId').val());
                LoadtBillingShippingShipTopartyAddress();
                //End
                var pageStatus = document.getElementById('hdnPageStatus').value;
                if ($('#hdnPageStatus').val() == "update")
                {
                    cddl_PosGstSalesOrder.SetEnabled(false);
                    AllowAddressShipToPartyState = false;
                    // BillShipAddressVisible();
                    
                }
                if($("#hdBasketId").val()!="")
                {
                    var NoSchemeTypedtl = $("#ddl_numberingScheme").val();
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

                        var hddnCRmVal = $("#<%=hddnCustIdFromCRM.ClientID%>").val();
                        if (hddnCRmVal == "1") {
                            page.SetActiveTabIndex(1);
                            page.tabs[0].SetEnabled(false);
                        }

                    

                        //   document.getElementById('<%= txt_SlOrderNo.ClientID %>').disabled = true;
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
                        document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();

                    }

                    //Added On 09-01-2018
                if (grid.GetEditor('ProductID').GetText() != "") {
                    //ccmbGstCstVat.PerformCallback();
                    //ccmbGstCstVatcharge.PerformCallback();
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                }
                    //End

                $("#<%=ddl_Branch.ClientID%>").val(BranchId);
                $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);

                    //                    grid.AddNewRow();
                    PopulateGSTCSTVAT();
                }

            }
        }


        function SalesMankeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSalesManSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {

                    callonServer("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "OnFocus");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[salesmanIndex=0]"))
                    $("input[salesmanIndex=0]").focus();
            } 
            else if (e.code == "Escape") {
                ctxtSalesManAgent.Focus();
            }
        }

        function OnFocus(Id, Name) {
           
            $("#<%=hdnSalesManAgentId.ClientID%>").val(Id);

            ctxtCreditDays.Focus();
            ctxtSalesManAgent.SetText(Name);
            $('#SalesManModel').modal('hide');
        }

        function Customerkeydown(e) {
            
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();
            OtherDetails.CustomerIds=$("#<%=hddnCustomers.ClientID%>").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != null && $("#txtCustSearch").val() != "") {
                    gridquotationLookup.SetEnabled(false);
                    $('input[type=radio]').prop('checked', false);
                    callonServer("Services/Master.asmx/GetCustomerSaleOrder", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "GetContactPersonOnJSON");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
            else if (e.code == "Escape") {
                ctxtCustName.Focus();
            }
        }

        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
                $("#txtCustSearch").focus();
            }
        }

        function ValueSelected(e, indexName) {
            
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;

                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex") {
                        SetProduct(Id, name);
                    }
                    else if (indexName == "salesmanIndex") {
                        OnFocus(Id, name);
                    }

                    else if (indexName == "customerIndex") {
                        $('#CustModel').modal('hide');
                        GetContactPersonOnJSON(Id, name);
                    }


                        //Start:Chinmoy 25-05-2018
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                    else if (indexName == "customeraddressIndex") {
                        SetCustomeraddress(Id, name);
                    }
                    //End

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
                    else if (indexName == "salesmanIndex")
                        ctxtCreditDays.Focus();
                        //Start Chinmoy 25-05-2018
                    else if (indexName == "BillingAreaIndex")
                        $('#txtbillingArea').focus();
                    else if (indexName == "ShippingAreaIndex")
                        $('#txtshippingArea').focus();
                    else if (indexName == "customeraddressIndex")
                        ('#txtshippingShipToParty').focus();
                        //End
                    else
                        $('#txtCustSearch').focus();
                }
            }

        }

        //Start Chinmoy 25-05-2018
        function SetCustomer(Id, Name) {
            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);

                GetObjectID('hdnCustomerId').value = Id;
                GetObjectID('hdnAddressDtl').value = '0';
                SetEntityType(Id);    
               
               

                var CustomerID=Id;

                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/GetCustomerReletedData",
                    data: JSON.stringify({ CustomerID: CustomerID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async:false,
                    success: function (msg) {
                        var strStatus = data.toString().split('~')[0];
                        if (strStatus == "D")
                        {
                                
                            jAlert('You have selected a "Dormant" Customer. Please change the Status of this Customer to "Active" to proceed further.');
                            ctxtCustName.SetText("");            
                            GetObjectID('hdnCustomerId').value = "";
                            cddl_AmountAre.SetValue("1");
                            cddl_AmountAre.SetEnabled(true);
                            ctxtCustName.Focus();
                        }
                        GetContactPerson(Id);
                    }
                });

                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                $('.dxeErrorCellSys').addClass('abc');
                $('.crossBtn').hide();
                page.GetTabByName('General').SetEnabled(false);
                $('#CustModel').modal('hide');                
                var startDate = new Date();
                startDate = tstartdate.GetValueString();


            }
        }
        //End
       

       
        function selectValue() {
            //
            var IsInventory=$("#<%=ddlInventory.ClientID%>").val();

            var checked = $('#rdl_Salesquotation').attr('checked', true);
            if (checked) {
                //$(this).attr('checked', false);
                gridquotationLookup.SetEnabled(true);
            }
            else {
                $(this).attr('checked', true);
            }
            var startDate = new Date();
            startDate = cPLSalesOrderDate.GetValueString();
            var key = $("#<%=hdnCustomerId.ClientID%>").val();
            var type = ($("[id$='rdl_Salesquotation']").find(":checked").val() != null) ? $("[id$='rdl_Salesquotation']").find(":checked").val() : "";

            //if (type == "QN") {
            //    clbl_Quotation_Date.SetText('PI/Quotation Date');
            //}
            //else if (type == "SINQ") {
            //    clbl_Quotation_Date.SetText('Sales Inquiry Date');
            //}

            if (key == null || key == "") {
                jAlert("Customer required !", 'Alert Dialog: [Quoation]', function (r) {
                    if (r == true) {
                        ctxtCustName.Focus();
                        gridquotationLookup.SetEnabled(false);
                        $('input[type=radio]').prop('checked', false);
                    }
                });

                return;

            }
            TaggingCall=true;
            if (key != null && key != '' && type != "") {
                cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate  + '~' + '%'+'~'+ type);
            }
            



            //var componentType = gridSalesOrderLookup.GetGridView().GetRowKey(gridSalesOrderLookup.GetGridView().GetFocusedRowIndex());
            //if (componentType != null && componentType != '') {
            //    grid.PerformCallback('GridBlank');
            //}
        }


        function GlobalBillingShippingEndCallBack() {
            if (cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit == "0") {
                cbsComponentPanel.cpGlobalBillingShippingEndCallBack_Edit = "0";
                var startDate = new Date();
               
                startDate = cPLSalesOrderDate.GetValueString();
                if (gridquotationLookup.GetValue() != null) {
                   
                    var key = $("#<%=hdnCustomerId.ClientID%>").val();
                    if (key != null && key != '') {
                        //cContactPerson.PerformCallback('BindContactPerson~' + key); //ON 08-01-2018 sUBHABRATA
                       

                       
                       

                    }
                }
                else {
                    
                    var key = $("#<%=hdnCustomerId.ClientID%>").val();
                    if (key != null && key != '') {
                        
                        //cContactPerson.PerformCallback('BindContactPerson~' + key);//sUBHABRATA ON 08-01-2018
                        //SetFocusAfterBillingShipping();

                       

                       
                        
                    }
                }
                $('#CustModel').modal('hide');
                //ctxtCustName.Focus();
            }
        }

        function SetDataSourceOnComboBox(ControlObject, Source){
            ControlObject.ClearItems();
            for(var count=0;count<Source.length;count ++){
                ControlObject.AddItem(Source[count].Name , Source[count].Id);
            } 
            ControlObject.SetSelectedIndex(0);
        }
    

        function SetFocusAfterBillingShipping()
        {
            setTimeout(function () {
                cContactPerson.Focus(); 
            }, 200);  
        }

    </script>
    <script>
        var   statusValueforRejectApproval;

        function ProductPriceCalculate() {
            if ((grid.GetEditor('SalePrice').GetValue() == null || grid.GetEditor('SalePrice').GetValue() == 0) && (grid.GetEditor('Discount').GetValue() == null || grid.GetEditor('Discount').GetValue() == 0))
            {
                var _saleprice = 0;
                var _Qty = grid.GetEditor('Quantity').GetValue();
                var _Amount = grid.GetEditor('Amount').GetValue();
                _saleprice = (_Amount / _Qty);
                grid.GetEditor('SalePrice').SetValue(_saleprice);
            }
        }
        function ProductAmountTextChange(s, e) {
            ProductPriceCalculate();
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
        
            if (document.getElementById('hdnPageStatus').value == "update")
            {
                //Rev Rajdip
                // clookup_Project.SetEnabled(false);
                //End Rev Rajdip



            }

            if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1")
            {

                document.getElementById("dvReject").style.display="inline-block";
                document.getElementById("dvApprove").style.display="inline-block";
                document.getElementById("dvAppRejRemarks").style.display="block";
            }
           
            

            if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1")
            {
                var det={};
                det.OrderId=$("#hdnEditOrderId").val();

                if($("#hdnEditOrderId").val()!="")
                {
                    $.ajax({
                        type: "POST",
                        url: "ProjectOrder.aspx/GetApproveRejectStatus",
                        data: JSON.stringify(det),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        //async:false,
                        success: function (msg) {
                       
                            var   statusValueforApproval = msg.d;
                            if ($("#hdnRevisionRequiredEveryAfterApproval").val()=="Yes") {
                                document.getElementById("dvAppRejRemarks").style.display="block";
                                document.getElementById("dvReject").style.display="inline-block";
                                document.getElementById("dvApprove").style.display="inline-block";
                            }
                            else {
                                if (statusValueforApproval == 1) {
                             
                                    document.getElementById("dvRevisionDate").style.display="block";
                                    document.getElementById("dvRevision").style.display="block";
                                    document.getElementById("dvAppRejRemarks").style.display="block";
                                    document.getElementById("dvReject").style.display="none";
                                    document.getElementById("dvApprove").style.display="none";
                                }
                                else if(statusValueforApproval == 2)
                                {
                                    document.getElementById("dvAppRejRemarks").style.display="block";
                                    document.getElementById("dvReject").style.display="none";
                                    document.getElementById("dvApprove").style.display="inline-block";
                                }
                                else if(statusValueforApproval == 0)
                                {   document.getElementById("dvAppRejRemarks").style.display="block";
                                    document.getElementById("dvReject").style.display="inline-block";
                                    document.getElementById("dvApprove").style.display="inline-block";
                                }
                            }                            
                        }
                    });
                }
            }


            if($("#hdnPageStatus").val()=="update" && $("#hdnApprovalReqInq").val() == "1")
            {
                var detApp={};
                detApp.OrderId=$("#hdnEditOrderId").val();

                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/GetApproveRejectStatus",
                    data: JSON.stringify(detApp),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //async:false,
                    success: function (msg) {
                       
                        statusValueforRejectApproval = msg.d;

                        if(statusValueforRejectApproval==1)
                        {
                            document.getElementById("dvRevisionDate").style.display="block";
                            document.getElementById("dvRevision").style.display="block";
                            //ctxtRevisionDate.SetMinDate(cPLSalesOrderDate.GetDate());
                            //ctxtRevisionDate.SetMinDate(new Date(cPLQuoteDate.GetDate().toDateString()))
                        }
                    }
                });
            }

            if ($("#hdnisFirstApprove").val() == 'Yes' && $("#hdnRevisionRequiredEveryAfterApproval").val() == "Yes" && ($("#hdnPageStatForApprove").val()=="PO")) {

                document.getElementById("dvRevisionDate").style.display="block";
                document.getElementById("dvRevision").style.display="block";
            }


            $('#idOutstanding').on("click",function(){
               
                $("#<%=drdExport.ClientID%>").val('0');
                cOutstandingPopup.Show();
                var CustomerId=$("#<%=hdnCustomerId.ClientID%>").val();
                var BranchId=$("#<%=ddl_Branch.ClientID%>").val();
                $("#<%=hddnBranchId.ClientID%>").val(BranchId);
                var AsOnDate=cPLSalesOrderDate.GetDate().format('yyyy-MM-dd');
                $("#<%=hddnAsOnDate.ClientID%>").val(AsOnDate);
                $("#<%=hddnOutStandingBlock.ClientID%>").val('1');
                //Clear Row
                var rw=$("[id$='CustomerOutstanding_DXMainTable']").find("tr")
                for(var RowClount=0; RowClount <rw.length;RowClount++){
                    rw[RowClount].remove();
                }



                //cCustomerOutstanding.Refresh();
                
                //cCustomerOutstanding.PerformCallback('BindOutStanding~' + CustomerId + '~' + BranchId + '~' + AsOnDate);
                var CheckUniqueCode=false;
                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/GetCustomerOutStanding",
                    data: JSON.stringify({strAsOnDate: AsOnDate, strCustomerId: CustomerId,BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //async:false,
                    success: function (msg) {
                       
                        CheckUniqueCode = msg.d;
                        if (CheckUniqueCode == true) {
                            cCustomerOutstanding.Refresh();
                            
                        }
                    }
                });


                //cCustomerOutstanding.Refresh();
                //cOutstandingPopup.Show();

            });

            document.onkeyup = function (e) {
                if (event.keyCode == 27) { //run code for alt+N -- ie, Save & New  
                    cOutstandingPopup.Hide();
                }
                else if (event.keyCode == 27) { //run code for alt+N -- ie, Save & New  
                    cOutstandingPopup.Hide();
                }
            }


            ////document.onkeydown = function (e) {
            ////    if (event.keyCode == 83 && event.altKey == true && getUrlVars().req != "V") { //run code for Alt + n -- ie, Save & New  
            ////        StopDefaultAction(e);
            ////        Save_ButtonClick();
            ////    }
            ////    else if (event.keyCode == 88 && event.altKey == true && getUrlVars().req != "V") { //run code for Ctrl+X -- ie, Save & Exit!     
            ////        StopDefaultAction(e);
            ////        SaveExit_ButtonClick();
            ////    }
                
            ////}

            document.onkeydown = function (e) {
                if (event.altKey == true) {
                    switch (event.keyCode) {
                        case 83:
                            if (($("#exampleModal").data('bs.modal') || {}).isShown) {
                                SaveVehicleControlData();
                            }
                            break;
                        case 80:
                            //if (($("#ProjectTermsConditionseModal").data('bs.modal') || {}).) {
                            //TermsConditionsSave();
                            $('#ProjectTermsConditionseModal').modal({
                                show: 'true'
                            
                            });
                            // cdtDefectPerid.ShowDropDown();
                            //  $("#ProjectTermsConditionseModal").show();
                            //}
                            break;
                        case 67:
                            modalShowHide(0);
                            break;
                        case 82:
                            modalShowHide(1);
                            $('body').on('shown.bs.modal', '#exampleModal', function () {
                                $('input:visible:enabled:first', this).focus();
                            })
                            break;
                        case 78:
                            StopDefaultAction(e);
                            if (getUrlVars().req != "V") {
                                Save_ButtonClick();
                            }
                            break;
                        case 88:
                            //
                            StopDefaultAction(e);
                            if (getUrlVars().req != "V") {
                                SaveExit_ButtonClick();
                            }
                            break;
                        case 120:
                            StopDefaultAction(e);
                            if (getUrlVars().req != "V") {
                                SaveExit_ButtonClick();
                            }
                            break;
                        case 84:
                            StopDefaultAction(e);
                            Save_TaxesClick();
                            break;
                        case 85:
                            OpenUdf();
                            break;
                        case 69:
                            if (($("#TermsConditionseModal").data('bs.modal') || {}).isShown) {
                                StopDefaultAction(e);
                                SaveTermsConditionData();
                            }
                            break;
                        case 76:
                            StopDefaultAction(e);
                            calcelbuttonclick();
                            break;
                        case 77:
                            $('#TermsConditionseModal').modal({
                                show: 'true'
                            });
                            break;
                        case 79:
                            if (page.GetActiveTabIndex() == 1) {
                                fnSaveBillingShipping();
                            }
                            break;
                    }
                }
            }
        });
        function modalShowHide(param) {

            switch (param) {
                case 0:
                    $('#exampleModal').modal('toggle');
                    break;
                case 1:
                    $('#exampleModal').modal({
                        show: 'true'
                    });
                    break;
            }

        }

        function CustomerCallBackPanelEndCallBack() {
            //GetContactPerson();
        }


        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function recalculateTax() {
            cmbGstCstVatChange(ccmbGstCstVat);
        }
        function recalculateTaxCharge() {
            ChargecmbGstCstVatChange(ccmbGstCstVatcharge);
        }

        var taxAmountGlobalCharges;
        function QuotationTaxAmountGotFocus(s, e) {
            taxAmountGlobalCharges = parseFloat(s.GetValue());
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
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                //gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - GlobalTaxAmt + taxAmountGlobalCharges);
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
                SetOtherChargeTaxValueOnRespectiveRow(0, s.GetValue(), gridTax.GetEditor("TaxName").GetText());
            }

            RecalCulateTaxTotalAmountCharges();

        }



    </script>
    <script>
        var Pre_TotalAmt = "0";
        
        function DiscountGotFocus(s, e) {
            var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
            Pre_TotalAmt = _Amount;
        }

       


        function QuantityGotFocus(s, e) {
       
            var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
            Pre_TotalAmt = _Amount;

            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strProductName = SpliteDetails[1];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];

            
            var isOverideConvertion = SpliteDetails[26];
            var packing_saleUOM = SpliteDetails[25];
            var sProduct_SaleUom = SpliteDetails[24];
            var sProduct_quantity = SpliteDetails[22];
            var packing_quantity = SpliteDetails[20];

            var slno= (grid.GetEditor('SrlNo').GetText() != null) ? grid.GetEditor('SrlNo').GetText() : "0";

            var QuotationNum= (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";

            var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
            var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
            var type = 'add';
            var gridprodqty = parseFloat(grid.GetEditor('Quantity').GetText()).toFixed(4);
            var gridPackingQty = '';
            //Rev Subhra 02-05-2019
            if (SpliteDetails.length > 27 ) {
                if (SpliteDetails[27] == "1") {
                    IsInventory = 'Yes';
                }
            }
           
         

            //if (SpliteDetails.length > 27 ) {
            //    if (SpliteDetails[27] == "1") {
            //        IsInventory = 'Yes';
                    
            //        type = 'edit';

            //        if (QuotationNum!="0" && QuotationNum!="") {
                        
            //            $.ajax({
            //                type: "POST",
            //                url: "Services/Master.asmx/GetMultiUOMDetails",
            //                data: JSON.stringify({orderid: strProductID,action:'SalesQuotationPackingQty',module:'SalesOrder',strKey : QuotationNum}),
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                success: function (msg) {
                                
            //                    gridPackingQty = msg.d;

            //                    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
            //                        ShowUOM(type, "SalesOrder", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            //                    }

            //                }
            //            });


            //        }
            //        else{
                       
            //            var orderid = grid.GetRowKey(globalRowIndex);
            //            $.ajax({
            //                type: "POST",
            //                url: "Services/Master.asmx/GetMultiUOMDetails",
            //                data: JSON.stringify({orderid: orderid,action:'SalesOrderPackingQty',module:'SalesOrder',strKey :''}),
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                success: function (msg) {
                                
            //                    gridPackingQty = msg.d;

            //                    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
            //                        ShowUOM(type, "SalesOrder", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            //                    }

            //                }
            //            });
            //        }
            //    }
            //}
            //else{

            //    if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
            //        ShowUOM(type, "SalesOrder", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
            //    }
            //}

           


            if (QuotationNum!="0" && QuotationNum!="") {
                        
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({orderid: strProductID,action:'SalesQuotationPackingQty',module:'SalesOrder',strKey : QuotationNum}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                                
                        gridPackingQty = msg.d;

                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }

                    }
                });


            }
            else if($("#hdAddOrEdit").val() == "Edit"){
                       
                var orderid = grid.GetRowKey(globalRowIndex);
                $.ajax({
                    type: "POST",
                    url: "Services/Master.asmx/GetMultiUOMDetails",
                    data: JSON.stringify({orderid: orderid,action:'SalesOrderPackingQty',module:'SalesOrder',strKey :''}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                                
                        gridPackingQty = msg.d;

                        if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                            ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                        }

                    }
                });
            }
                //$("#hdBasketId").val()!="" ||("#hdBasketId").val()!=null ||
            else if (SpliteDetails.length > 27 && ( $("#hdBasketId").val()!="" )) {
                if (SpliteDetails[27] == "1") {
                    IsInventory = 'Yes';

                    // type = 'edit';

                    if (SpliteDetails[28] != '') {
                        if (parseFloat(SpliteDetails[28]) > 0) {
                            gridPackingQty = SpliteDetails[28];
                            if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                                ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                            }
                        }
                    }
                }
            }



            else{
                if (ShowUOMConversionInEntry == "1" && IsInventory == 'Yes' && SpliteDetails.length > 1) {
                    ShowUOM(type, "Sales", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                }
            }
            //End of Rev

        } 

        function SalesPriceGotFocus(s, e) {
            //
            var _Amount = (grid.GetEditor('Amount').GetText() != null) ? grid.GetEditor('Amount').GetText() : "0";
            Pre_TotalAmt = _Amount;
        }

        var issavePacking = 0;

        function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
          
            issavePacking = 1;
            grid.batchEditApi.StartEdit(globalRowIndex);
            var Bal_Qty=grid.GetEditor('BalQty').GetValue();      
           
            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            //if(quote_Id.length==0)
            //{
            if(parseFloat(Quantity)<parseFloat(Bal_Qty))
            {
                jAlert('Quantity can not be less than tagged quantity.','Alert',function(){
                    grid.batchEditApi.StartEdit(globalRowIndex, 6);
                });
            }
            else
            {
                grid.GetEditor('Quantity').SetValue(Quantity);
                <%--Use for set focus on UOM after press ok on UOM--%> 
                setTimeout(function () {
                    grid.batchEditApi.StartEdit(globalRowIndex, 8);
                }, 600);
                <%--Use for set focus on UOM after press ok on UOM--%> 
            }
            // }
            //else if(($("#hdAddOrEdit").val()=="Add")&&(quote_Id.length!=0))
            //{
            //    if(parseFloat(Quantity)>parseFloat(Bal_Qty))
            //    {
            //        jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.','Alert',function(){
            //            grid.batchEditApi.StartEdit(globalRowIndex, 7);
            //        })
            //    }
            //    else
            //    {
               
            //        setTimeout(function () {
            //            grid.batchEditApi.StartEdit(globalRowIndex, 8);
            //        }, 600)
                
            //    }
            //}
        }

        function SetFoucs() {
            //
            grid.batchEditApi.StartEdit(globalRowIndex, 8);
        }


        $(function () {
            $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
                //this.value = this.value.replace(/[^0-9\.]/g,'');
                $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
                if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            });
        });

        function ProductsGotFocusFromID(s, e) {

            //pageheaderContent.style.display = "block";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strProductName = SpliteDetails[1];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y") {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

            if (ProductID != "") {

                var SalesOrder_ID = ctxt_SlOrderNo.GetValue();
                var pageStatus = document.getElementById('hddnActionFieldOnStockBlock').value;
                if (pageStatus == 'Add') {

                    //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);
                }
                else if (pageStatus == 'Edit') {
                    //cacpAvailableStock.toback('GetAvailableStockOnOrder' + '~' + strProductID + '~' + SalesOrder_ID + '~' + QuantityValue);
                }

            }
        }

        function cmbContactPersonEndCall(s, e) {
            if (cContactPerson.cpDueDate != null) {
                var DeuDate = cContactPerson.cpDueDate;
                var myDate = new Date(DeuDate);

                var invoiceDate = new Date();
                var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

                ctxtCreditDays.SetValue(0);

                cdt_SaleInvoiceDue.SetDate(myDate);
                cContactPerson.cpDueDate = null;
            }

            if (cContactPerson.cpTotalDue != null) {
                var TotalDue = cContactPerson.cpTotalDue;
                document.getElementById('<%=lblTotalDues.ClientID %>').innerHTML = TotalDue;
                //pageheaderContent.style.display = "block";
                divDues.style.display = "block";
                cContactPerson.cpTotalDue = null;
            }
        }

        function CreditDays_TextChanged(s, e) {
            //
            var CreditDays = ctxtCreditDays.GetValue();

            //var today = new Date();
            var today = cPLSalesOrderDate.GetDate();
            //var newdate = new Date();
            var newdate = cPLSalesOrderDate.GetDate();
            newdate.setDate(today.getDate() + Math.round(CreditDays));

            cdt_SaleInvoiceDue.SetDate(newdate);
            cdt_SaleInvoiceDue.SetEnabled(false);
        }

        function ProductsGotFocus(s, e) {

            //pageheaderContent.style.display = "block";
            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";

            var ddlbranch = $("[id*=ddl_Branch]");
            var strBranch = ddlbranch.find("option:selected").text();

            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            strProductName = strDescription;
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];
            var IsPackingActive = SpliteDetails[10];
            var Packing_Factor = SpliteDetails[11];
            var Packing_UOM = SpliteDetails[12];
            var PackingValue = (Packing_Factor * QuantityValue) + " " + Packing_UOM;

            if (IsPackingActive == "Y") {
                $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                divPacking.style.display = "block";
            } else {
                divPacking.style.display = "none";
            }

            $('#<%= lblStkQty.ClientID %>').text(QuantityValue);
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

            //if (ProductID != "0") {
            //    cacpAvailableStock.PerformCallback(strProductID);
            //}
        }

        //function isInventoryChanged(s, e) {
        //    //
        //    var IsInventoryValue = ccmbIsInventory.GetValue();
        //    cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);
        //    cproductLookUp.gridView.Refresh();
        //}


    </script>
    <script type="text/javascript">
        <%--kaushik Section--%>
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

     

     

        function SalePriceTextChange(s, e) {
            // 
            //pageheaderContent.style.display = "block";
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var Saleprice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetText();
            var SpliteDetails = ProductID.split("||@||");
            var strMultiplier = SpliteDetails[7];
            var strFactor = SpliteDetails[8];
            var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
            //var strRate = "1";
            var strStkUOM = SpliteDetails[4];
            //var strSalePrice = SpliteDetails[6];

            if (strRate == 0) {
                strRate = 1;
            }
            if ($("#hdnRateType").val() == "2") {
                if ($("#ProductMinPrice").val() <= Saleprice && $("#ProductMaxPrice").val() >= Saleprice) {

                }
                else {
                    jAlert("Product Min price :" + $("#ProductMinPrice").val() + " and Max price :" + $("#ProductMaxPrice").val(), "Alert", function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 9);
                        return;
                    });
                }
            }

            var StockQuantity = strMultiplier * QuantityValue;
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

            var Amount = QuantityValue * strFactor * (Saleprice / strRate);
            var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

            var tbAmount = grid.GetEditor("Amount");
            tbAmount.SetValue(amountAfterDiscount);

            var TotaAmountRes = '';
            TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

            var tbTotalAmount = grid.GetEditor("TotalAmount");
            //tbTotalAmount.SetValue(amountAfterDiscount);
            tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));




            //Debjyoti section GST
            var ShippingStateCode = $("#bsSCmbStateHF").val();
            var TaxType = "";
            if (cddl_AmountAre.GetValue() == "1") {
                TaxType = "E";
            }
            else if (cddl_AmountAre.GetValue() == "2") {
                TaxType = "I";
            }

            var CompareStateCode;
            if (cddl_PosGstSalesOrder.GetValue()== "S") {
                CompareStateCode = GeteShippingStateCode();
            }
            else {
                CompareStateCode = GetBillingStateCode();
            }



            //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
            //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());

            caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
               SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesOrderDate.GetDate(), QuantityValue);
            //END



            //var tbAmount = grid.GetEditor("Amount");
            //tbAmount.SetValue(Amount); 
            //var tbTotalAmount = grid.GetEditor("TotalAmount");
            //tbTotalAmount.SetValue(Amount); 
            DiscountTextChange(s, e);
        }

        function ctaxUpdatePanelEndCall(s, e) {
            //
            //grid.batchEditApi.StartEdit(globalRowIndex, 6);
            if (ctaxUpdatePanel.cpstock != null) { 
                LoadingPanel.Hide();
                ctaxUpdatePanel.cpstock = null;
            }
        }

        function ctaxUpdatePanelEndCall2(s, e) {
            //
            if (ctaxUpdatePanel2.cpstock != null) {
                //grid.batchEditApi.StartEdit(globalRowIndex, 6);
                LoadingPanel.Hide();
                ctaxUpdatePanel2.cpstock = null;
            }
        }


        function DiscountTextChange(s, e) {
            //
            var IsDiscountVal = '';
            //var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
            var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
            var strProductID = '';
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetText();
            IsDiscountVal = $("#<%=IsDiscountPercentage.ClientID%>").val();
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                strProductID = SpliteDetails[0];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                if (strRate == 0) {
                    strRate = 1;
                }
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

                //Subhabrata on 04-12-2017
                var amountAfterDiscount = "0";
                var ResultAmountAfterDiscount = "0";
                if (IsDiscountVal == "Y") {
                    if (parseFloat(Discount) > 100) {
                        Discount = "0";

                        var tb_Discount = grid.GetEditor("Discount");
                        tb_Discount.SetValue(Discount);
                    }
                    //ResultAmountAfterDiscount = Math.round(Amount).toFixed(2) + ((Math.round(Discount).toFixed(2) * Math.round(Amount).toFixed(2)) / 100);
                    //parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                    ResultAmountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                    amountAfterDiscount = (ResultAmountAfterDiscount).toFixed(2);
                    //alert(amountAfterDiscount);
                }
                else {
                    ResultAmountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                    amountAfterDiscount = ResultAmountAfterDiscount.toFixed(2);
                }

                //End
                // var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(amountAfterDiscount);

                var TotaAmountRes = '';
                TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(amountAfterDiscount + (TotaAmountRes * 1));
                //tbTotalAmount.SetValue(amountAfterDiscount);


                //Debjyoti section GST
                var ShippingStateCode = $("#bsSCmbStateHF").val();
                var TaxType = "";
                if (cddl_AmountAre.GetValue() == "1") {
                    TaxType = "E";
                }
                else if (cddl_AmountAre.GetValue() == "2") {
                    TaxType = "I";
                }

                var CompareStateCode;
                if (cddl_PosGstSalesOrder.GetValue()== "S") {
                    CompareStateCode = GeteShippingStateCode();
                }
                else {
                    CompareStateCode = GetBillingStateCode();
                }
                //Added on 09-01-2018
                var _SrlNo = grid.GetEditor("SrlNo").GetValue();
                if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                    var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
                    TaxOfProduct.push(ProductTaxes);
                }
                else {
                    $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
                }
                //End

                //caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
                //    SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val());

                caluculateAndSetGST(grid.GetEditor("Amount"), grid.GetEditor("TaxAmount"), grid.GetEditor("TotalAmount"),
                   SpliteDetails[19], Amount, amountAfterDiscount, TaxType, CompareStateCode, $('#ddl_Branch').val(), $("#hdnEntityType").val(), cPLSalesOrderDate.GetDate(), QuantityValue);

                if (parseFloat(Amount) != parseFloat(Pre_TotalAmt)) {
                    ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue()+ '~' + strProductID);
                }
                //END
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Discount').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }         

        }
         <%--kaushik Section--%>
        //Ware house KAUSHIK     27-2-2017

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

                SelectWarehouse = strWarehouse;
                SelectBatch = strBatchID;
                SelectSerial = strSrlID;

                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + strWarehouse);
                checkListBox.PerformCallback('EditSerial~' + strWarehouse + '~' + strBatchID + '~' + strSrlID);

                cCmbWarehouse.SetValue(strWarehouse);
                ctxtQuantity.SetValue(strQuantity);
            }
        }

        function acpAvailableStockEndCall(s, e) {

            if (cacpAvailableStock.cpstock != null) {
                var AvlStk = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                document.getElementById('<%=lblAvailableStk.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableSStk.ClientID %>').innerHTML = AvlStk;
                document.getElementById('<%=lblAvailableStock.ClientID %>').innerHTML = cacpAvailableStock.cpstock;
                document.getElementById('<%=lblAvailableStockUOM.ClientID %>').innerHTML = document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                cCmbWarehouse.cpstock = null;

                grid.batchEditApi.StartEdit(globalRowIndex, 6);
                return;
            }

        }

        function CmbWarehouseEndCallback(s, e) {
            if (SelectWarehouse != "0") {
                cCmbWarehouse.SetValue(SelectWarehouse);
                SelectWarehouse = "0";
            }
            else {
                cCmbWarehouse.SetEnabled(true);
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
                //checkListBox.SetValue(SelectWarehouse);
                SelectSerial = "0";
                cCmbBatch.SetEnabled(false);
                cCmbWarehouse.SetEnabled(false);
            }
        }
        //Ware house KAUSHIK     27-2-2017

        //TAXES KAUSHIK     27-2-2017


        function acbpCrpUdfEndCall(s, e) {
            debugger;
            if (gridquotationLookup.GetValue() != null) {
                        
                grid.AddNewRow();
            }
            if (cacbpCrpUdf.cpUDF) {

                if (cacbpCrpUdf.cpUDF == "false") {
                    jAlert("UDF is set as Mandatory. Please enter values.", "Alert", function () { OpenUdf(); });
                    LoadingPanel.Hide();
                    if (gridquotationLookup.GetValue() != null) {
                        grid.AddNewRow();
                    }
                    cacbpCrpUdf.cpUDF = null;

                }
                else if (cacbpCrpUdf.cpTransport == "false") {
                    jAlert("Transporter is set as Mandatory. Please enter values.", "Alert", function () { $("#exampleModal").modal('show'); });
                    LoadingPanel.Hide();
                    //grid.AddNewRow();
                    if (gridquotationLookup.GetValue() != null) {
                        grid.AddNewRow();
                    }
                    cacbpCrpUdf.cpTransport = null;
                }
                else if (cacbpCrpUdf.cpTC == "false") {

                    jAlert("Terms and Condition is set as Mandatory. Please enter values.", "Alert", function () { $("#TermsConditionseModal").modal('show'); });
                    LoadingPanel.Hide();
                    //grid.AddNewRow();
                    if (gridquotationLookup.GetValue() != null) {
                        
                        grid.AddNewRow();
                    }
                    cacbpCrpUdf.cpTC = null;
                }
                else {
                    grid.UpdateEdit();
                }
            }
        }


        function Save_TaxesClick() {
            grid.batchEditApi.EndEdit();
            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var i, cnt = 1;
            var sumAmount = 0, sumTaxAmount = 0, sumDiscount = 0, sumNetAmount = 0, sumDiscountAmt = 0;

            cnt = 1;
            for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
                var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
                var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
                var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
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
                    var Amount = (grid.batchEditApi.GetCellValue(i, 'Amount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Amount')) : "0";
                    var TaxAmount = (grid.batchEditApi.GetCellValue(i, 'TaxAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TaxAmount')) : "0";
                    var Discount = (grid.batchEditApi.GetCellValue(i, 'Discount') != null) ? (grid.batchEditApi.GetCellValue(i, 'Discount')) : "0";
                    var NetAmount = (grid.batchEditApi.GetCellValue(i, 'TotalAmount') != null) ? (grid.batchEditApi.GetCellValue(i, 'TotalAmount')) : "0";
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





                //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
                //// Details: Billing/Shipping user control integration 

                //Old Code
                //Get Customer Shipping StateCode
                //var shippingStCode = '';
                //if (cchkBilling.GetValue()) {
                //    shippingStCode = CmbState.GetText();
                //}
                //else {
                //    shippingStCode = CmbState1.GetText();
                //}

                //New Copde
                var shippingStCode = '';
                shippingStCode = ctxtshippingState.GetText();
                if (shippingStCode.trim() != "") {
                    shippingStCode = shippingStCode;
                }

                //// Date: 30-05-2017    Author: Kallol Samanta  [END] 





                shippingStCode = shippingStCode;

                //Debjyoti 09032017
                if (shippingStCode.trim() != '') {
                    for (var cmbCount = 1; cmbCount < ccmbGstCstVatcharge.GetItemCount() ; cmbCount++) {
                        //Check if gstin is blank then delete all tax
                        if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] != "") {
                            if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {
                                if (shippingStCode == "4" || shippingStCode == "26" || shippingStCode == "25" || shippingStCode == "35" || shippingStCode == "31" || shippingStCode == "34") {
                                    if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S') {
                                        ccmbGstCstVatcharge.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                                else {
                                    if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'I' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                        ccmbGstCstVatcharge.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            } else {
                                if (ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'S' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'C' || ccmbGstCstVatcharge.GetItem(cmbCount).value.split('~')[4] == 'U') {
                                    ccmbGstCstVatcharge.RemoveItem(cmbCount);
                                    cmbCount--;
                                }
                            }
                        } else {
                            //remove tax because GSTIN is not define
                            ccmbGstCstVatcharge.RemoveItem(cmbCount);
                            cmbCount--;
                        }
                    }
                }





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
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
                GlobalTaxAmt = 0;
            }
            else {
                GlobalTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                gridTax.GetEditor("Amount").SetValue(Sum);
                ctxtQuoteTaxTotalAmt.SetValue(parseFloat(ctxtQuoteTaxTotalAmt.GetValue()) - parseFloat(Sum) + GlobalTaxAmt);
                //ctxtTotalAmount.SetValue(parseFloat(Amount) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue())); 
                ctxtTotalAmount.SetValue(parseFloat(ctxtProductNetAmount.GetValue()) + parseFloat(ctxtQuoteTaxTotalAmt.GetValue()));
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






        ////////////

        var AmountOldValue;
        var AmountNewValue;

        function AmountTextChange(s, e) {
            AmountLostFocus(s, e);
            var RecieveValue = (grid.GetEditor('Amount').GetValue() != null) ? parseFloat(grid.GetEditor('Amount').GetValue()) : "0";
        }

        function AmountLostFocus(s, e) {
            AmountNewValue = s.GetText();
            var indx = AmountNewValue.indexOf(',');

            if (indx != -1) {
                AmountNewValue = AmountNewValue.replace(/,/g, '');
            }
            if (AmountOldValue != AmountNewValue) {
                changeReciptTotalSummary();
            }
        }

        function AmountGotFocus(s, e) {
            AmountOldValue = s.GetText();
            var indx = AmountOldValue.indexOf(',');
            if (indx != -1) {
                AmountOldValue = AmountOldValue.replace(/,/g, '');
            }
        }

        function changeReciptTotalSummary() {
            var newDif = AmountOldValue - AmountNewValue;
            var CurrentSum = ctxtSumTotal.GetText();
            var indx = CurrentSum.indexOf(',');
            if (indx != -1) {
                CurrentSum = CurrentSum.replace(/,/g, '');
            }

            ctxtSumTotal.SetValue(parseFloat(CurrentSum - newDif));
        }


        //taxEs KAUSHIK 27-2-2017
        function CmbWarehouse_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS" || type == "WB") {
                cCmbBatch.PerformCallback('BindBatch~' + WarehouseID);
            }
            else if (type == "WS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + "0");
            }
        }
        function CmbBatch_ValueChange() {
            var WarehouseID = cCmbWarehouse.GetValue();
            var BatchID = cCmbBatch.GetValue();
            var type = document.getElementById('hdfProductType').value;

            if (type == "WBS") {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            }
            else if (type == "BS") {
                checkListBox.PerformCallback('BindSerial~' + "0" + '~' + BatchID);
            }
        }
        //tab start
        function disp_prompt(name) {
            //
            if (name == "tab0") {
                //gridLookup.Focus();
                ctxtCustName.Focus();
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                $("#divcross").show();
                //alert(name);
                //document.location.href = "SalesQuotation.aspx?";
            }
            if (name == "tab1") {
                $("#divcross").hide();
                var custID = GetObjectID('hdnCustomerId').value;
                page.GetTabByName('General').SetEnabled(false);
                if (custID == null && custID == '') {
                    jAlert('Please select a customer');
                    page.SetActiveTabIndex(0);

                    return;
                }
                else {
                    //
                    page.SetActiveTabIndex(1);
                    //fn_PopOpen();
                }
            }
            
        }
        //tab end


        <%--kaushik 24-2-2017--%>

        $(document).ready(function () {
            //
            //cProductsPopup.Hide();
            if (GetObjectID('hdnCustomerId').value == null || GetObjectID('hdnCustomerId').value == '') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(false);
            }

            <%--  $('#<%=rdl_Salesquotation.ClientID%>').click(function () {
                var checked = $(this).attr('checked', true);
                if (checked) {
                    $(this).attr('checked', false);
                }
                else {
                    $(this).attr('checked', true);
                }
            });--%>

            <%--$('#<%=rdl_Salesquotation.ClientID%>').off('click').on('click', function () {
                if ($(this).data('checked')) {
                    $(this).removeAttr('checked');
                    $(this).data('checked', false);
                } else {
                    $(this).data('checked', true);
                }
            });--%>

            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustSearch').focus();
            })

            

            $('#LastRateModal').on('shown.bs.modal', function () {
                $('#btnCloseRate').focus();
            })
            $('#LastRateModal').on('hide.bs.modal', function () {

                grid.batchEditApi.StartEdit(globalRowIndex,9);
            })




            $('#ProductModel').on('shown.bs.modal', function () {
                $('#txtProdSearch').focus();
            })

            //region Sandip Section For Approval Section Start
            $('#ApprovalCross').click(function () {
                window.parent.popup.Hide();
                window.parent.cgridPendingApproval.Refresh();
            })
            //endregion Sandip Section For Approval Dtl Section End

            ////
            //var IsInventoryValue = ccmbIsInventory.GetValue();
            //cIsInventory.PerformCallback('BindSession' + '~' + IsInventoryValue);
        })

        function GetBillingAddressDetailByAddressId(e) {
            var addresskey = billingLookup.GetGridView().GetRowKey(billingLookup.GetGridView().GetFocusedRowIndex());
            if (addresskey != null && addresskey != '') {

                cComponentPanel.PerformCallback('BlookupEdit~' + addresskey);
            }
        }

        function GetShippingAddressDetailByAddressId(e) {

            var saddresskey = shippingLookup.GetGridView().GetRowKey(shippingLookup.GetGridView().GetFocusedRowIndex());
            if (saddresskey != null && saddresskey != '') {

                cComponentPanel.PerformCallback('SlookupEdit~' + saddresskey);
            }
        }

        <%--kaushik 24-2-2017--%>
        function UniqueCodeCheck() {

            var SchemeVal = $('#<%=ddl_numberingScheme.ClientID %> option:selected').val();
            if (SchemeVal == "") {
                alert('Please Select Numbering Scheme');
                ctxt_SlOrderNo.SetValue('');
                ctxt_SlOrderNo.Focus();
            }
            else {
                var OrderNo = ctxt_SlOrderNo.GetText();
                if (OrderNo != '') {

                    var SchemaLength = GetObjectID('hdnSchemaLength').value;
                    var x = parseInt(SchemaLength);
                    var y = parseInt(OrderNo.length);

                    if (y > x) {
                        //alert('Sales Order No length cannot be more than ' + x);
                        //jAlert('Please enter unique Sales Order No');
                        //ctxt_SlOrderNo.SetValue('');
                        //ctxt_SlOrderNo.Focus();

                    }
                    else {
                        var CheckUniqueCode = false;
                        $.ajax({
                            type: "POST",
                            url: "ProjectOrder.aspx/CheckUniqueCode",
                            data: JSON.stringify({ OrderNo: OrderNo }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                CheckUniqueCode = msg.d;
                                if (CheckUniqueCode == true) {
                                    alert('Please enter unique Project Sales Order No');
                                    //jAlert('Please enter unique Sales Order No');
                                    ctxt_SlOrderNo.SetValue('');
                                    ctxt_SlOrderNo.Focus();
                                }
                                else {
                                    $('#MandatorySlOrderNo').attr('style', 'display:none');
                                    var hddnCRmVal = $("#<%=hddnCustIdFromCRM.ClientID%>").val();
                                    if (hddnCRmVal == "1") {
                                        page.SetActiveTabIndex(1);
                                        page.tabs[0].SetEnabled(false);
                                    }
                                }
                            }

                        });
                    }
                }
            }
        }

        function ValidfromCheck(s,e)
        {
            cdtProjValidUpto.SetMinDate(cdtProjValidFrom.GetDate());
            if(cdtProjValidUpto.GetDate()<cdtProjValidFrom.GetDate())
            {
                cdtProjValidUpto.Clear();
            }
        }

        function DateCheck(s,e) {
            var full_url = document.URL;
            var url_array = full_url.split('key=');
            var id = url_array[1];

            

            var proddata=grid.GetRow(-1).children[3].innerText;

            if(id=="ADD" && proddata.trim()!="")
            {
                if (gridquotationLookup.GetValue() != null) {
                    jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {

                            page.SetActiveTabIndex(0);
                            $('.dxeErrorCellSys').addClass('abc');


                            var startDate = cPLSalesOrderDate.GetValueString();
                            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            //var key = gridLookup.GetValue();subhabrata on 02-01-2018
                            var key = $("#<%=hdnCustomerId.ClientID%>").val();

                            if (key != null && key != '') {
                                cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                            }
                            grid.PerformCallback('GridBlank');

                            ccmbGstCstVat.PerformCallback();
                            ccmbGstCstVatcharge.PerformCallback();
                            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                        }
                    });
                }
                else {
                    jConfirm('Documents in the grid are to be automatically blank. Confirm ?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            var startDate = cPLSalesOrderDate.GetValueString();
                            page.SetActiveTabIndex(0);
                            $('.dxeErrorCellSys').addClass('abc');
                            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + key + '~' + startDate + '~' + '@');

                            //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                            //var key = gridLookup.GetValue();
                            var key = $("#<%=hdnCustomerId.ClientID%>").val();

                            if (key != null && key != '') {
                                cQuotationComponentPanel.PerformCallback('BindQuotationGrid' + '~' + key + '~' + startDate + '~' + 'DateCheck');

                            }
                            grid.PerformCallback('GridBlank');
                            ccmbGstCstVat.PerformCallback();
                            ccmbGstCstVatcharge.PerformCallback();
                            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                            page.SetActiveTabIndex(0);
                        }
                        else
                        {
                            
                            cPLSalesOrderDate.SetDate(cPLSalesOrderDate.lastChangedValue);  
                        }
                    });

                }
            }

            cdtProjValidFrom.SetMinDate(cPLSalesOrderDate.GetDate());
            //if(cdtProjValidUpto.GetDate()<cdtProjValidFrom.GetDate())
            //{
            //    cdtProjValidUpto.Clear();
            //}
            if(cdtProjValidFrom.GetDate()<cPLSalesOrderDate.GetDate())
            {
                cdtProjValidFrom.Clear();
            }

            //ctxtRevisionDate.SetMinDate(cPLSalesOrderDate.GetDate());
            //ctxtRevisionDate.SetMinDate(new Date(cPLQuoteDate.GetDate().toDateString()))
           
            if(ctxtRevisionDate.GetDate()<cPLSalesOrderDate.GetDate())
            {
                ctxtRevisionDate.Clear();
            }

            ctxtRevisionDate.SetMinDate(cPLSalesOrderDate.GetDate());
           
        }
        var SimilarProjectStatus="0";
        function CloseGridQuotationLookup() {
            //
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            //gridquotationLookup.Focus();
            txt_OANumber.focus();
            if(gridquotationLookup.GetValue()==null)
            {
                $('input[type=radio]').prop('checked', false);
                grid.PerformCallback('GridBlank');
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                gridquotationLookup.SetEnabled(false);
            }


            var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
           
            debugger;
            if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val()=="1")
            {
               
                var quote_Id = "";
                // otherDets.quote_Id = quote_Id;
                for (var i = 0; i < quotetag_Id.length; i++) {
                    if (quote_Id == "") {
                        quote_Id = quotetag_Id[i];
                    }
                    else {
                        quote_Id += ',' + quotetag_Id[i];
                    }
                }
                var Doctype=$("#rdl_Salesquotation").find(":checked").val();
                debugger;
                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/DocWiseSimilarProjectCheck",
                    data: JSON.stringify({quote_Id:quote_Id,Doctype:Doctype}),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        SimilarProjectStatus = msg.d;
                        debugger;
                        if (SimilarProjectStatus != "1")
                        {
                            cPLQADate.SetText("");
                            jAlert("Unable to procceed. Project are for the selected Document(s) are different.");
                           
                            return false;

                        }
                    }
                });
            }


        }

        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }

        function cmbtaxCodeindexChange(s, e) {
            if (cgridTax.GetEditor("Taxes_Name").GetText() == "GST/CST/VAT") {

                var taxValue = s.GetValue();

                if (taxValue == null) {
                    taxValue = 0;
                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                    cgridTax.GetEditor("Amount").SetValue(0);
                    //ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetText((ctxtTaxTotAmt.GetValue() * 1).toFixed(2) - (GlobalCurTaxAmt * 1).toFixed(2));
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    cgridTax.GetEditor("Amount").SetValue(((ProdAmt * rate) / 100).toFixed(2));
                    //ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetText(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * rate) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
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
                    //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
                    cgridTax.GetEditor("Amount").SetValue(((ProdAmt * s.GetText()) / 100).toFixed(2));

                    //ctxtTaxTotAmt.SetText(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetText(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
                }
            }

        }

        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
        }


        function txtTax_TextChanged(s, i, e) {


            cgridTax.batchEditApi.StartEdit(i, 2);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            //cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
            cgridTax.GetEditor("Amount").SetValue(((ProdAmt * s.GetText()) / 100).toFixed(2));



        }


        function taxAmtButnClick1(s, e) {
            //console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }

        function BatchUpdate() {

            cgridTax.UpdateEdit();

            return false;
        }




        $(document).ready(function () {
            //
            ctxtRate.SetValue("");
            ctxtRate.SetEnabled(false);
            ctxt_SlOrderNo.SetEnabled(false);
            gridquotationLookup.SetEnabled(false);

            PopulateLoadGSTCSTVAT();
        });
    </script>

    <%--Debu Section End--%>



    <script type="text/javascript">

        //window.onload = function () {
        //    // grid.AddNewRow();
        //     
        //    OnAddNewClick();
        //};
        function ParentCustomerOnClose(newCustId,customerName,Unique) {
            //clookup_CustomerControlPanelMain1.PerformCallback(newCustId);
            // ctxtCustName.SetText(customerName);
           
            GetObjectID('hdnCustomerId').value = newCustId;
           
            AspxDirectAddCustPopup.Hide();
            ctxtShipToPartyShippingAdd.SetText('');
            if (newCustId != "") {
                ctxtCustName.SetText(customerName);
                GetContactPersonOnJSON(newCustId, customerName);
            }
            //gridLookup.gridView.Refresh();
            //gridLookup.Focus();
        }
        function AddcustomerClick() {
            var isLighterPage = $("#hidIsLigherContactPage").val();
       
            if (isLighterPage == 1) {
                var url = '/OMS/management/Master/customerPopup.html?var=1.1.3.7';
                AspxDirectAddCustPopup.SetContentUrl(url);
                //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
                AspxDirectAddCustPopup.RefreshContentUrl();
                AspxDirectAddCustPopup.Show();
            }
            else
            {
                var url = '/OMS/management/Master/Customer_general.aspx';
                //window.location.href = url;
                AspxDirectAddCustPopup.SetContentUrl(url);
                AspxDirectAddCustPopup.Show();
            }
        }


        //Start Chinmoy 25-05-2018
        function AfterSaveBillingShipiing(validate) {
            
            GetPosForGstValue();
            if (validate) {
                page.SetActiveTabIndex(0);
                page.tabs[0].SetEnabled(true);
                $("#divcross").show();

                var ShippingAddress=ctxtsAddress1.GetValue();

                var IsDistanceCalculate = document.getElementById('hdnIsDistanceCalculate').value;
                //if(IsDistanceCalculate=='Y')
                //GetLocation(ShippingAddress);

            }
            else {
                page.SetActiveTabIndex(1);
                page.tabs[0].SetEnabled(false);
                $("#divcross").hide();
            }
        }

        //End
        function GetPosForGstValue()
        {
           
            cddl_PosGstSalesOrder.ClearItems();
            if(cddl_PosGstSalesOrder.GetItemCount()==0)
            {
                cddl_PosGstSalesOrder.AddItem(GetBillingStateName() + '[Billing]', "B");
                cddl_PosGstSalesOrder.AddItem(GetShippingStateName() + '[Shipping]', "S");
                
            }
            
            else  if(cddl_PosGstSalesOrder.GetItemCount()>2)
            {
                cddl_PosGstSalesOrder.ClearItems();
                //cddl_PosGstSalesOrder.RemoveItem(0);
                //cddl_PosGstSalesOrder.RemoveItem(0);
            }

            if(PosGstId=="" || PosGstId==null)
            {
                cddl_PosGstSalesOrder.SetValue("B");
            }
            else
            {
                cddl_PosGstSalesOrder.SetValue(PosGstId);
            }
        }




        function CloseGridLookup() {
            gridLookup.ConfirmCurrentSelection();
            gridLookup.HideDropDown();
            gridLookup.Focus();
            gridquotationLookup.SetEnabled(true);
        }
        //function GetSalesManAgent(e) {
        //    var SalesManComboBox = cSalesManComboBox.GetText();
        //    if (!cSalesManComboBox.FindItemByText(SalesManComboBox)) {
        //        cSalesManComboBox.SetValue("");
        //        cSalesManComboBox.Focus();
        //        jAlert("Entered Salesman/Agent not Exists.");
        //        return;
        //    }
        //} Subhabrata on 03-01-2018


        function GetContactPersonOnJSON(id, Name) {

            var CustomerID=id;

            $.ajax({
                type: "POST",
                url: "ProjectOrder.aspx/GetCustomerReletedData",
                data: JSON.stringify({ CustomerID: CustomerID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async:false,
                success: function (msg) {
                    var data = msg.d;         
                    var strStatus = data.toString().split('~')[0];
                    if (strStatus == "D")
                    {                                
                        jAlert('You have selected a "Dormant" Customer. Please change the Status of this Customer to "Active" to proceed further.');
                        ctxtCustName.SetText("");            
                        GetObjectID('hdnCustomerId').value = "";
                        cddl_AmountAre.SetValue("1");
                        cddl_AmountAre.SetEnabled(true);
                        ctxtCustName.Focus();
                        $('#CustModel').modal('hide');
                    }
                    else
                    {
                        AllowAddressShipToPartyState=true;
                        //LoadingPanel.Show();
                        var IsContactperson=true;
                        var startDate = new Date();
                        startDate = cPLSalesOrderDate.GetValueString();
                        var OutStandingAmount='';          

                        $('#CustModel').modal('hide');
                        // Added  Chinmoy 25-05-2018
                        $('#openlink').show();
                        cddl_PosGstSalesOrder.ClearItems();
                        cddl_PosGstSalesOrder.SetEnabled(true);
                        SetDefaultBillingShippingAddress(id);
                        //End
                        if (gridquotationLookup.GetValue() != null) {
                            jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                                if (r == true) {
                                    //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                                    //var key = gridLookup.GetValue();
                                    var key = id;
                                    ctxtCustName.SetText(Name);
                                    GetObjectID('hdnCustomerId').value = key;
                                    pageheaderContent.style.display = "block";
                                    if (key != null && key != '') {
                                        $.ajax({
                                            type: "POST",
                                            url: "ProjectOrder.aspx/GetContactPersonafterBillingShipping",
                                            data: JSON.stringify({ Key: key }),
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            success: function (r) {
                                                var contactPersonJsonObject=r.d;
                                                //cContactPerson.SetValue(contactPerson);
                                                IsContactperson=false;
                                                SetDataSourceOnComboBox(cContactPerson,contactPersonJsonObject);
                                                SetFocusAfterBillingShipping();                            
                                            }
                                        });
                                        if(IsContactperson)
                                        {
                                            SetFocusAfterBillingShipping();
                                        }
                                        page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                                        //New Code
                                        //Edited Chinmoy Below Line
                                        SetDefaultBillingShippingAddress(key);
                                        // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                                        //GetObjectID('hdnCustomerId').value = key;
                                        if ($('#hfBSAlertFlag').val() == "1") {
                                            jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                                                if (r == true) {
                                                    page.SetActiveTabIndex(1);
                                                    //cbtnSave_SalesBillingShiping.Focus();
                                                    page.tabs[0].SetEnabled(false);
                                                    $("#divcross").hide();
                                                }
                                            });
                                        }
                                        else {
                                            page.SetActiveTabIndex(1);
                                            cbtnSave_SalesBillingShiping.Focus();
                                            page.tabs[0].SetEnabled(false);
                                            $("#divcross").hide();
                                        }

                                        if (grid.GetVisibleRowsOnPage() == 0) {
                                            OnAddNewClick();
                                        }

                                        GetObjectID('hdnCustomerId').value = key;
                                        GetObjectID('hdnAddressDtl').value = '0';
                                        <%--$("#<%=ddl_SalesAgent.ClientID%>").focus();--%>//subhabrata on 12-12-2017

                                        if (grid.GetEditor('ProductID').GetText() != "") {
                                            grid.PerformCallback('GridBlank');
                                            ccmbGstCstVat.PerformCallback();
                                            ccmbGstCstVatcharge.PerformCallback();
                                            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                                        }

                                    }
                                }
                            });
                        }
                        else {
                            var key = id;
                            GetObjectID('hdnCustomerId').value = key;
                            pageheaderContent.style.display = "block";
                            ctxtCustName.SetText(Name);
                            if (key != null && key != '') {

                                $.ajax({
                                    type: "POST",
                                    url: "ProjectOrder.aspx/GetContactPersonafterBillingShipping",
                                    data: JSON.stringify({ Key: key }),
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    // async:false,
                                    success: function (r) {
                                        var contactPersonJsonObject=r.d;
                                        //cContactPerson.SetValue(contactPerson);
                                        SetDataSourceOnComboBox(cContactPerson,contactPersonJsonObject);
                                        IsContactperson=false;
                                        //SetFocusAfterBillingShipping();                            
                                    }

                                });

                                if(IsContactperson)
                                {
                                    SetFocusAfterBillingShipping();
                                }
                                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
                                //New Code
                                PosGstId = "";
                                cddl_PosGstSalesOrder.SetValue(PosGstId);
                                SetDefaultBillingShippingAddress(key);
                                //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                                //GetObjectID('hdnCustomerId').value = key;
                                if ($('#hfBSAlertFlag').val() == "1") {
                                    jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {
                                            page.SetActiveTabIndex(1);
                                            // cbsSave_BillingShipping.Focus();
                                            //page.tabs[0].SetEnabled(false);
                                            $("#divcross").hide();
                                        }
                                    });
                                }
                                else {
                                    page.SetActiveTabIndex(1);
                                    // cbsSave_BillingShipping.Focus();
                                    //cContactPerson.Focus();
                                    page.tabs[0].SetEnabled(false);
                                    $("#divcross").hide();
                                }
                                startDate = cPLSalesOrderDate.GetValueString();
                                GetObjectID('hdnCustomerId').value = key;
                                //alert(GetObjectID('hdnCustomerId').value);
                                GetObjectID('hdnAddressDtl').value = '0';                   
                                if (grid.GetEditor('ProductID').GetText() != "") {
                                    grid.PerformCallback('GridBlank');
                                    ccmbGstCstVat.PerformCallback();
                                    ccmbGstCstVatcharge.PerformCallback();
                                    ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                                }
                            }
                        }
                        var CustomerId=$("#<%=hdnCustomerId.ClientID%>").val();
                        var BranchId=$("#<%=ddl_Branch.ClientID%>").val();
                        var AsOnDate=cPLSalesOrderDate.GetDate().format('yyyy-MM-dd');

                        //Ajax Started
                        $.ajax({
                            type: "POST",
                            url: "ProjectOrder.aspx/GetCustomerOutStandingAmount",
                            data: JSON.stringify({strAsOnDate: AsOnDate, strCustomerId: CustomerId,BranchId: BranchId }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            // async:false,
                            success: function (msg) {
                                
                                LoadingPanel.Hide();
                                OutStandingAmount = msg.d;
                                if(OutStandingAmount==="")
                                {
                                    $('#<%=lblOutstanding.ClientID %>').text('0.00');
                                }
                                else
                                {
                                    $('#<%=lblOutstanding.ClientID %>').text(OutStandingAmount);
                                }
                    
                            }
                        });

                        //End
                    }
                }
            });
            clookup_Project.gridView.Refresh();
        }

        <%--function GetLocation(address) {
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    var latitude = results[0].geometry.location.lat();
                    var longitude = results[0].geometry.location.lng();
                    var BranchId=$("#<%=ddl_Branch.ClientID%>").val();
                    
                    //alert("Latitude: " + latitude + "\nLongitude: " + longitude);

                    $.ajax({
                        type: "POST",
                        url: "SalesOrderAdd.aspx/GetBranchAddress",
                        data: JSON.stringify({BranchId: BranchId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        // async:false,
                        success: function (msg) {
                            var branchAddress=msg.d;

                            geocoder.geocode({ 'address': branchAddress }, function (_results, _status) {
                                if (_status == google.maps.GeocoderStatus.OK) {
                                    var Branchlatitude = _results[0].geometry.location.lat();
                                    var Branchlongitude = _results[0].geometry.location.lng();

                                    //var loc1=new google.maps.LatLng(latitude,longitude);
                                    //var loc2=new google.maps.LatLng(Branchlatitude,Branchlongitude);

                                    //alert("Latitude: " + Branchlatitude + "\nLongitude: " + Branchlongitude);
                                    var distance=distances(latitude, longitude,Branchlatitude,Branchlongitude);
                                    distance=parseFloat(distance).toFixed(4)

                                    //var dist = loc2.distanceFrom(loc1);
                                    //alert("Distance: " + distance);
                                    //alert("Distance: " + dist);
                                    ctxtFreight.SetValue(distance*2);
                                } 
                            });



                        }
                    });
                } 
            });
        };

        function distances(lat1, lon1, lat2, lon2) {
            var R = 3963.189;
            
            if(deg2rad(lon2)>=deg2rad(lon1)) delta_lon = deg2rad(lon2) - deg2rad(lon1);
            else delta_lon = deg2rad(lon1) - deg2rad(lon2);
 
            // Find the Great Circle distance
            if(delta_lon>0) distance = Math.acos(Math.sin(deg2rad(lat1)) * Math.sin(deg2rad(lat2)) + Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) * Math.cos(delta_lon)) * 3963.189;//miles
            else distance=0;

            distance=(parseFloat(distance).toFixed(4)) * 1.60934;//KM
            return distance;
        }
 
        function deg2rad(val) {
            var pi = Math.PI;
            var de_ra = ((eval(val))*(pi/180));
            return de_ra;
        }

        google.maps.LatLng.prototype.distanceFrom = function(latlng) {
            var lat = [this.lat(), latlng.lat()]
            var lng = [this.lng(), latlng.lng()]
            var R = 6378137;
            var dLat = (lat[1]-lat[0]) * Math.PI / 180;
            var dLng = (lng[1]-lng[0]) * Math.PI / 180;
            var a = Math.sin(dLat/2) * Math.sin(dLat/2) +
            Math.cos(lat[0] * Math.PI / 180 ) * Math.cos(lat[1] * Math.PI / 180 ) *
            Math.sin(dLng/2) * Math.sin(dLng/2);
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
            var d = R * c;
            return Math.round(d);
        }--%>

        function GetContactPerson(e) {
            //
            var CustomerComboBox = gridLookup.GetText();
            if (!gridLookup.FindItemByText(CustomerComboBox)) {
                gridLookup.SetValue("");
                gridLookup.Focus();
                jAlert("Customer not Exists.");
                return;
            }
            var startDate = new Date();
            startDate = cPLSalesOrderDate.GetValueString();
            if (gridquotationLookup.GetValue() != null) {
                jConfirm('Documents tagged are to be automatically De-selected. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                        var key = gridLookup.GetValue();
                        GetObjectID('hdnCustomerId').value = key;
                        if (key != null && key != '') {







                            //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
                            //// Details: Billing/Shipping user control integration 

                            //Old Code
                            //cchkBilling.SetChecked(false);
                            //cchkShipping.SetChecked(false);
                            //cContactPerson.PerformCallback('BindContactPerson~' + key);
                            page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

                            //New Code
                            // Edited Chinmoy 25-05-2018
                            SetDefaultBillingShippingAddress(key);
                            // LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                            //GetObjectID('hdnCustomerId').value = key;
                            if ($('#hfBSAlertFlag').val() == "1") {
                                jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                                    if (r == true) {
                                        page.SetActiveTabIndex(1);
                                        cbtnSave_SalesBillingShiping.Focus();
                                        page.tabs[0].SetEnabled(false);
                                        $("#divcross").hide();
                                    }
                                });
                            }
                            else {
                                page.SetActiveTabIndex(1);
                                cbtnSave_SalesBillingShiping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            }

                            if (grid.GetVisibleRowsOnPage() == 0) {
                                OnAddNewClick();
                            }

                            GetObjectID('hdnCustomerId').value = key;

                            GetObjectID('hdnAddressDtl').value = '0';
                            <%--$("#<%=ddl_SalesAgent.ClientID%>").focus();--%>//subhabrata on 12-12-2017

                            //});

                            //document.getElementById('popup_ok').focus();
                        }
                    }
                });
            }
            else {
                //var key = gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex());
                var key = gridLookup.GetValue();
                GetObjectID('hdnCustomerId').value = key;

                if (key != null && key != '') {

                    page.GetTabByName('[B]illing/Shipping').SetEnabled(true);

                    //New Code
                    //Edited Chinmoy Below Code
                    SetDefaultBillingShippingAddress(key);
                    //LoadCustomerAddress(key, $('#ddl_Branch').val(), 'SO');   //For SalesOrder => SO 
                    //GetObjectID('hdnCustomerId').value = key;
                    if ($('#hfBSAlertFlag').val() == "1") {
                        jConfirm('Wish to View/Select Billing and Shipping details?', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                page.SetActiveTabIndex(1);
                                cbtnSave_SalesBillingShiping.Focus();
                                page.tabs[0].SetEnabled(false);
                                $("#divcross").hide();
                            }
                        });
                    }
                    else {
                        page.SetActiveTabIndex(1);
                        cbtnSave_SalesBillingShiping.Focus();
                        //cContactPerson.Focus();
                        page.tabs[0].SetEnabled(false);
                        $("#divcross").hide();
                    }

                    startDate = cPLSalesOrderDate.GetValueString();



                    GetObjectID('hdnCustomerId').value = key;
                    //alert(GetObjectID('hdnCustomerId').value);
                    GetObjectID('hdnAddressDtl').value = '0';


                }
            }
            //grid.GetEditor('Quotation_Num').SetEnabled(true);
            //grid.GetEditor('ProductName').SetEnabled(true);
            //grid.GetEditor('Description').SetEnabled(true);




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

        $(document).ready(function () {

            var mode = $('#hdAddOrEdit').val();
            if (mode == 'Edit') {
                if ($("#hdAddOrEdit").val() != "") {
                    var CustomerId = $("#hdnCustomerId").val();
                    SetEntityType(CustomerId);
                }
            }
            //
            //$('#ddl_numberingScheme').focus();

            $('#divCrossActivity').on("click", function () {
               
                var url = '';
                url = "../CRMPhoneCallWithFrame.aspx?TransSale=" + <%=Session["salesid"]%> + "&Assigned=" + <%=Session["AssignedById"]%> + "&type=" + <%=Session["type"]%> + "&Cid=" + <%=Session["CusId"]%> + "&Pid=1";
                window.location.href = url;
            });


            var IsEditMode = '<%= Session["ActionType"]%>';
            <%--$("#<%=rdl_Salesquotation.ClientID%>").attr('disabled', false);--%>
            if (IsEditMode.trim() != 'Add') {

                page.SetActiveTabIndex(0);
                page.tabs[1].SetEnabled(false);
                $("#openlink").css("display","none");
            }
            var hddnCRmVal = $("#<%=hddnCustIdFromCRM.ClientID%>").val();
            var CustId = $("#<%=hdnCustomerId.ClientID%>").val();
            
            if (hddnCRmVal == "1") {
                

                page.SetActiveTabIndex(0);
                page.tabs[1].SetEnabled(false);

                pageheaderContent.style.display = "block";
                //Chinmoy Edited on 25-05-2018
                SetDefaultBillingShippingAddress(CustId);
                //LoadCustomerAddress(CustId, $('#ddl_Branch').val(), 'SO');

                var BranchId=$("#<%=ddl_Branch.ClientID%>").val();
                var AsOnDate=cPLSalesOrderDate.GetDate().format('yyyy-MM-dd');
                ctxtCustName.SetEnabled(true);
                //Ajax Started
                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/GetCustomerOutStandingAmount",
                    data: JSON.stringify({strAsOnDate: AsOnDate, strCustomerId: CustId,BranchId: BranchId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    // async:false,
                    success: function (msg) {
                        
                        LoadingPanel.Hide();
                        OutStandingAmount = msg.d;
                        if(OutStandingAmount==="")
                        {
                            $('#<%=lblOutstanding.ClientID %>').text('0.00');
                        }
                        else
                        {
                            $('#<%=lblOutstanding.ClientID %>').text(OutStandingAmount);
                        }
                    
                    }
                });

                //End

            }


            $('#ddl_Branch').change(function () {
                clookup_Project.gridView.Refresh();
            });


            //$('#dt_PLSales').change(function () {

            //    var posDate = cPLSalesOrderDate.GetText();
            //    var dt = new Date();
            //    cdtProjValidFrom.SetMinDate(new Date(posDate));

            //});


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

                    var hddnCRmVal = $("#<%=hddnCustIdFromCRM.ClientID%>").val();
                    if (hddnCRmVal == "1") {
                        page.SetActiveTabIndex(1);
                        page.tabs[0].SetEnabled(false);
                    }

                    

                    //   document.getElementById('<%= txt_SlOrderNo.ClientID %>').disabled = true;
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
                    document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();

                }

                //Added On 09-01-2018
                if (grid.GetEditor('ProductID').GetText() != "") {
                    //ccmbGstCstVat.PerformCallback();
                    //ccmbGstCstVatcharge.PerformCallback();
                    //ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                }
                //End

                $("#<%=ddl_Branch.ClientID%>").val(BranchId);
                $("#<%=ddl_Branch.ClientID%>").prop("disabled", true);
                //gridLookup.SetText('');

                clookup_Project.gridView.Refresh();
            });

            <%--$("#<%=ddl_SalesAgent.ClientID%>").change(function () {

                $("#<%=ddl_Branch.ClientID%>").focus();
            });--%>



            $('#ddl_Currency').change(function () {

                var CurrencyId = $(this).val();
                var ActiveCurrency = '<%=Session["ActiveCurrency"]%>'
                var Currency = ActiveCurrency.toString().split('~')[0];
                if (Currency != CurrencyId) {
                    if (ActiveCurrency != null) {
                        if (CurrencyId != '0') {
                            $.ajax({
                                type: "POST",
                                url: "ProjectOrder.aspx/GetCurrentConvertedRate",
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
        //cddl_PosGstSalesOrder.AddItem(   GetShippingStateName() + '[Shipping]', "S");
        //cddl_PosGstSalesOrder.AddItem( GetBillingStateName() + '[Billing]', "B");
        //cddl_PosGstSalesOrder.SetValue("S");
        var PosGstId="";
        function PopulatePosGst(e)
        {
            
            PosGstId=cddl_PosGstSalesOrder.GetValue();
            if(PosGstId=="S")
            {
                cddl_PosGstSalesOrder.SetValue("S");  
            }
            else if(PosGstId=="B")
            {
                cddl_PosGstSalesOrder.SetValue("B"); 
            }
        }



        function PopulateGSTCSTVAT(e) {
            var key = cddl_AmountAre.GetValue();
            if (key == 1) {
                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(false);
                // cddlVatGstCst.PerformCallback('1');
                cddlVatGstCst.SetSelectedIndex(0);
            }
            else if (key == 2) {
                grid.GetEditor('TaxAmount').SetEnabled(true);
                cddlVatGstCst.SetEnabled(true);
                cddlVatGstCst.PerformCallback('2');
                cddlVatGstCst.Focus();
            }
            else if (key == 3) {
                grid.GetEditor('TaxAmount').SetEnabled(false);
                cddlVatGstCst.SetEnabled(false);
                cddlVatGstCst.PerformCallback('3');

            }

        }

        function Onddl_VatGstCstEndCallback(s, e) {
            if (s.GetItemCount() == 1) {
                cddlVatGstCst.SetEnabled(false);
            }
        }

        function PopulateLoadGSTCSTVAT() {
            cddlVatGstCst.SetEnabled(false);
        }



        function showQuotationDocument() {
            var URL = "Contact_Document.aspx?requesttype=" + Quotation + "";
            window.location.href = URL;
        }


        // Popup Section

        function ShowCustom() {

            cPopup_wareHouse.Show();


        }

        // Popup Section End

    </script>

    <%--Sudip--%>
    <script>
        var currentEditableVisibleIndex;
        var preventEndEditOnLostFocus = false;
        var lastProductID;
        var setValueFlag;

        //function ProductsCombo_SelectedIndexChanged(s, e) {
        //    
        //    var tbDescription = grid.GetEditor("Description");
        //    var tbUOM = grid.GetEditor("UOM");
        //    var tbStkUOM = grid.GetEditor("StockUOM");
        //    var tbSalePrice = grid.GetEditor("SalePrice");
        //    var tbStockQuantity = grid.GetEditor("StockQuantity");

        //    var ProductID = s.GetValue();
        //    var SpliteDetails = ProductID.split("||@||");
        //    var strProductID = SpliteDetails[0];
        //    var strDescription = SpliteDetails[1];
        //    var strUOM = SpliteDetails[2];
        //    var strStkUOM = SpliteDetails[4];
        //    var strSalePrice = SpliteDetails[6];

        //    tbDescription.SetValue(strDescription);
        //    tbUOM.SetValue(strUOM);
        //    tbStkUOM.SetValue(strStkUOM);
        //    tbSalePrice.SetValue(strSalePrice);
        //    tbStockQuantity.SetValue("0");
        //}

        function ProductsCombo_SelectedIndexChanged(s, e) {
            //pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = s.GetValue();
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

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
            divPacking.style.display = "none";
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");

            //Debjyoti
            //Subhabrata commented on 09-01-2018
            //ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            //End

            //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);
        }
        //function OnEndCallback(s, e) {
        //    if (grid.cpSaveSuccessOrFail == "outrange") {
        //        jAlert('Can Not Add More Sales Order Exausted.<br />Update The Scheme and Try Again');
        //        gridTax.batchEditApi.StartEdit(-1, 2);
        //    }
        //    else if (grid.cpSaveSuccessOrFail == "duplicate") {
        //        jAlert('Can Not Save as Duplicate Journal Voucher No. Found');
        //        gridTax.batchEditApi.StartEdit(-1, 2);
        //    }
        //    else if (grid.cpSaveSuccessOrFail == "errorInsert") {
        //        grid.batchEditApi.StartEdit(0, 2);
        //        jAlert('Please try again late.');
        //    }
        //}

        function OnEndCallbackOutstanding(s,e)
        {
            //if(cCustomerOutstanding.cpOutStanding=="OutStanding")
            //{
            //    cCustomerOutstanding.cpOutStanding=null;
            //    cCustomerOutstanding.Refresh();
            //    cOutstandingPopup.Show();
            //}
            
        }

        function OnEndCallback(s, e) {
          
            if(grid.cpLoadAddressFromQuote){            
                if(grid.cpLoadAddressFromQuote!="")
                {
                    cQuotationBillingShipping.PerformCallback('BindAddress~'+grid.cpLoadAddressFromQuote);
                    grid.cpLoadAddressFromQuote=null;
                }                  
            }
            LoadingPanel.Hide();

            var value = document.getElementById('hdnRefreshType').value;
            var IsFromActivity = document.getElementById('hdnIsFromActivity').value;
            if (grid.cpSaveSuccessOrFail == "outrange") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Quotation Number as Quotation Scheme Exausted.<br />Update The Scheme and Try Again');
            }
            // Rev 1.0
            else if (grid.cpSaveSuccessOrFail == "AddLock") {
                OnAddNewClick();
                jAlert('DATA is Freezed between ' + grid.cpAddLockStatus);
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                LoadingPanel.Hide();
            }
            // End of Rev 1.0
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
                //Mantis Issue number 0018841 End
            else if (grid.cpSaveSuccessOrFail == "duplicateProduct") {

                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Product. Found');
            }
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
                jAlert('Please select Project.');
            }
            else if (grid.cpSaveSuccessOrFail == "ExceedQuantity") {
                grid.batchEditApi.StartEdit(0, 2);
                grid.cpSaveSuccessOrFail = null;
                jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');
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
                jAlert('No SalesMan/Agent selected.Please Select.');
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
                var Order_Msg = "Project Sales Order No. " + SalesOrder_Number + " saved.";              
                if (IsFromActivity == "Y" && value == "E") {
                    $('#<%=hdnRefreshType.ClientID %>').val('');
                    if (SalesOrder_Number != "") {                     

                       

                        jAlert(Order_Msg, 'Alert Dialog: [ProjectOrder]', function (r) {
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

                                if ($("#<%=hddnSaveOrExitButton.ClientID%>").val() == 'Save_Exit') {
                                    var DocumentNo = grid.cpDocumentNo;
                                    jConfirm('Do you want to print ?', 'Confirmation Dialog', function (r) {
                                        if (r == true) {
                                            
                                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=SO-Default~D&modulename=Sorder&id=" + DocumentNo, '_blank');
                                            grid.cpDocumentNo=null;
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
                    $('#<%=hdnRefreshType.ClientID %>').val('');
                    //#region Sandip Section For Approval Section Start
                    if (grid.cpApproverStatus == "approve") {
                        window.parent.popup.Hide();
                        window.parent.cgridPendingApproval.PerformCallback();
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
                                grid.cpSalesOrderNo = null;
                            
                                window.location.assign("projectOrderList.aspx");
                                

                            }
                        });

                    }
                    else {
                        window.location.assign("projectOrderList.aspx");
                    }
                }
                else if (value == "N") {
                    // window.location.assign("SalesOrderAdd.aspx?key=ADD");


                    if (SalesOrder_Number != "") {
                        jAlert(Order_Msg, 'Alert Dialog: [ProjectOrder]', function (r) {
                            //jAlert(Order_Msg);
                            grid.cpSalesOrderNo = null;
                            if (r == true) {
                                
                                window.location.assign("ProjectOrder.aspx?key=ADD");
                            }
                        });

                    }
                    else {
                        window.location.assign("ProjectOrder.aspx?key=ADD");
                    }
                }
                else {

                    var pageStatus = document.getElementById('hdnPageStatus').value;
                    if (pageStatus == "first") {
                        //OnAddNewClick();

                        grid.batchEditApi.EndEdit();

                        //$('#ddl_numberingScheme').focus();

                        $('#ddlInventory').focus();
                        //document.getElementById("<%=ddl_numberingScheme.ClientID%>").focus();
                        document.getElementById("<%=ddlInventory.ClientID%>").focus();
                        $('#<%=hdnPageStatus.ClientID %>').val('');
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
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "Quoteupdate") {
                        //OnAddGridNewClick();
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                    }
                    else if (pageStatus == "delete") {
                        $('#<%=hdnPageStatus.ClientID %>').val('');
                        //grid.batchEditApi.StartEdit(0, 2);
                        OnAddNewClick();
                    }
                    else {
                        grid.StartEditRow(0);
                        $('#<%=hdnPageStatus.ClientID %>').val('');
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

}


function SetArrForUOM(){
    if (aarr.length == 0) {
        for(var i = -500; i < 500;i++)
        {
            if(grid.GetRow(i) != null){
               
                var ProductID = (grid.batchEditApi.GetCellValue(i,'ProductID') != null) ? grid.batchEditApi.GetCellValue(i,'ProductID') : "0";
                if(ProductID!="0"){
                    var QuotationNum= (grid.GetEditor('Quotation_Num').GetText() != null) ? grid.GetEditor('Quotation_Num').GetText() : "0";
                    if($("#hdAddOrEdit").val() == "Edit"){
                        var SpliteDetails = ProductID.split("||@||");
                        var strProductID = SpliteDetails[0];
                        var orderid = grid.GetRowKey(i);
                        var slnoget = grid.batchEditApi.GetCellValue(i,'SrlNo');
                        var Quantity = grid.batchEditApi.GetCellValue(i,'Quantity');
                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({orderid: orderid,action:'SalesOrderPackingQty',module:'SalesOrder',strKey :''}),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                               
                                gridPackingQty = msg.d;

                                if(msg.d != ""){
                                    var packing = SpliteDetails[20];
                                    var PackingUom = SpliteDetails[24];
                                    var PackingSelectUom = SpliteDetails[25];
                                    var arrobj = {};
                                    arrobj.productid = strProductID;
                                    arrobj.slno = slnoget;
                                    arrobj.Quantity = Quantity;
                                    arrobj.packing = gridPackingQty;
                                    arrobj.PackingUom = PackingUom;
                                    arrobj.PackingSelectUom = PackingSelectUom;

                                    aarr.push(arrobj);
                                    //alert();
                                }
                            }
                        });
                    }
                }
            }
        }
        
    }
}

function Save_ButtonClick() {
    grid.AddNewRow();
    var flag = true;
    LoadingPanel.Show();
    var revdate=ctxtRevisionDate.GetText();
    var  RevisionDate=new Date(revdate);
    var OrderNo = ctxt_SlOrderNo.GetText();
    var slsdate = cPLSalesOrderDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var salesorderDate = new Date(slsdate);
    var IsTaggingMandatory=$("#<%=hdnConfigValueForTagging.ClientID%>").val();
    var quotationDate = "";
    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);
    }
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
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
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
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


    var ProjectCode = clookup_Project.GetText();
    if ( $("#hdnProjectMandatory").val()=="1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        
        flag = false;
    }


    if (customerid == null || customerid == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }    
    if (slsdate == null || slsdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlDate').attr('style', 'display:block');
    }
    else {
        $('#MandatorySlDate').attr('style', 'display:none');
        if (qudate != null && qudate != '') {
            var qd = qudate.split('-');
            quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);
            if (quotationDate > salesorderDate) {
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }
    if (OrderNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }
    if(IsTaggingMandatory.trim()=="Y")
    {
        flag = false;
        LoadingPanel.Hide();
        jAlert("Tagging is mandatory.Please select quotationDate");
        gridquotationLookup.Focus();
    }
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
    {    
        var detRev={};
        detRev.RevNo=ctxtRevisionNo.GetText();
        detRev.Order=$("#hdnEditOrderId").val();       
        $.ajax({
            type: "POST",
            url: "ProjectOrder.aspx/Duplicaterevnumbercheck",
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
                    ctxtRevisionNo.SetFocus();         
                }
            }
        });
        //End Rev Rajdip
        //}
    }
    if (flag) {       
        SetArrForUOM(); //For UOM Conversion Surojit
        if (gridquotationLookup.GetValue() == null) {
            grid.AddNewRow();
            var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }
        if (grid.GetVisibleRowsOnPage() > 0) {
            var IsInventory=$("#ddlInventory").val();
            console.log("1"+IsInventory);
            if(IsInventory!='N')
            {            
                if (issavePacking == 1) {
                    if (aarr.length > 0) {
                        console.log("2"+IsInventory);
                        $.ajax({
                            type: "POST",
                            url: "ProjectOrder.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {                                
                                var customerval = $("#<%=hdnCustomerId.ClientID%>").val() != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);                                
                                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());                               
                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;                             

                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                grid.batchEditApi.EndEdit();                               
                                cacbpCrpUdf.PerformCallback();
                                $('#<%=hdnRefreshType.ClientID %>').val('N');
                            }
                        });
                    }
                    else{                      
                        var customerval = $("#<%=hdnCustomerId.ClientID%>").val() != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);                  

                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());                       
                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;                     

                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();                     
                        cacbpCrpUdf.PerformCallback();
                        $('#<%=hdnRefreshType.ClientID %>').val('N');
                    }
                }
                else{

                    if (aarr.length > 0) {
                        console.log("3"+IsInventory);
                        $.ajax({
                            type: "POST",
                            url: "ProjectOrder.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {                                
                                var customerval = $("#<%=hdnCustomerId.ClientID%>").val() != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                                $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);                            

                                $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());                              
                                var JsonProductList = JSON.stringify(TaxOfProduct);
                                GetObjectID('hdnJsonProductTax').value = JsonProductList;                                
                                $('#<%=hdfIsDelete.ClientID %>').val('I');
                                grid.batchEditApi.EndEdit();                             
                                cacbpCrpUdf.PerformCallback();
                                $('#<%=hdnRefreshType.ClientID %>').val('N');
                            }
                        });
                    }
                    else{                       
                        var customerval = $("#<%=hdnCustomerId.ClientID%>").val() != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                        $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);
                        // Custom Control Data Bind
                        $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());                       
                        var JsonProductList = JSON.stringify(TaxOfProduct);
                        GetObjectID('hdnJsonProductTax').value = JsonProductList;                   

                        $('#<%=hdfIsDelete.ClientID %>').val('I');
                        grid.batchEditApi.EndEdit();                       
                        cacbpCrpUdf.PerformCallback();
                        $('#<%=hdnRefreshType.ClientID %>').val('N');
                    }
                }
            }           
        }
        else {
            LoadingPanel.Hide();
            jAlert('You must enter proper details before save');
        }
    }    
}
function Reject_ButtonClick()
{

    if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1")
    {

        if($("#txtAppRejRemarks").val()=="")
        {
                   
            jAlert("Please Enter Reject Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }

    }


    var otherdet={};
    otherdet.ApproveRemarks=$("#txtAppRejRemarks").val();
    otherdet.ApproveRejStatus=2;
    otherdet.OrderId= $("#hdnEditOrderId").val();

    $.ajax({
        type: "POST",
        url: "ProjectOrder.aspx/SetApproveReject",
        data: JSON.stringify(otherdet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var value=msg.d;
            if (value=="1")
            {
                jAlert("Order Rejected.");
                window.location.href="projectOrderList.aspx";
            }
        }
               
    });

}




function Approve_ButtonClick()
{
    var revdate=ctxtRevisionDate.GetText();
    var  RevisionDate=new Date(revdate);
    var flag = true;
    if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
    {
        if($("#txtAppRejRemarks").val()=="")
        {
            flag = false;
            jAlert("Please Enter Approval Remarks.")
            $("#txtAppRejRemarks").focus();
            return false;
        }
    }
    else if($("#hdnPageStatForApprove").val()=="PO" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "Yes"){
        if(ctxtRevisionNo.GetText()=="" && $("#hdnisFirstApprove").val() == 'Yes')
        {          
            flag = false;
            LoadingPanel.Hide();
            jAlert("Please Enter Revision Details.");
            ctxtRevisionNo.SetFocus();
            return false;
        }

        if ($("#hdnisFirstApprove").val() == 'Yes') {
            if(revdate=="01-01-0100"||revdate==null||revdate=="")
            {           
                flag = false;
                LoadingPanel.Hide();
                jAlert("Please Enter Revision Details.");
                ctxtRevisionDate.SetFocus();
                return false;           
            }
        }
    }

    if($("#hdnisFirstApprove").val() == 'Yes' && $('#hdnRevisionRequiredEveryAfterApproval').val() == "Yes")
    {
        var detRev={};
        detRev.RevNo=ctxtRevisionNo.GetText();
        detRev.Order=$("#hdnEditOrderId").val();       
        $.ajax({
            type: "POST",
            url: "ProjectOrder.aspx/Duplicaterevnumbercheck",
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
                    ctxtRevisionNo.SetFocus();    
                    return false; 
                    return;
                }
            }
        });
    }

    grid.AddNewRow();
          
    //alert(1);
    $("#hdnIsfromApproval").val("Yes");
    $("#hdnApproveStatus").val(1);

    var IsProductwithtaggedDoc=false;
    LoadingPanel.Show();
    $("#<%=hddnSaveOrExitButton.ClientID%>").val('Save_Exit');
    var OrderNo = ctxt_SlOrderNo.GetText();
    var slsdate = cPLSalesOrderDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var IsTaggingMandatory=$("#<%=hdnConfigValueForTagging.ClientID%>").val();
    var salesorderDate = new Date(slsdate);
    var quotationDate = "";

    //var StateCode=GeteShippingStateCode();

    //alert(StateCode);
            
    
    

    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }
    if (customerid == null || customerid == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }




    if(IsTaggingMandatory.trim()=="Y")
    {
        if(gridquotationLookup.GetValue() == null)
        {

            flag = false;
            //gridquotationLookup.SetEnabled(true);
            //$('input[type=radio]').prop('checked', true);
            LoadingPanel.Hide();
            jAlert("Tagging is mandatory.Please select quotation number.");
            //gridquotationLookup.Focus();
        }
       
    }

    if (slsdate == null || slsdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlDate').attr('style', 'display:block');
    }
    else {
        $('#MandatorySlDate').attr('style', 'display:none');
        if (qudate != null && qudate != '') {
            var qd = qudate.split('-');
            quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

            if (quotationDate > salesorderDate) {

                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }

    }

    if (OrderNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }




    if (flag) {

        SetArrForUOM(); //For UOM Conversion Surojit

        if (issavePacking == 1) {
            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        if (gridquotationLookup.GetValue() == null) {
                            //grid.AddNewRow();
                            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                            //var tbQuotation = grid.GetEditor("SrlNo");
                            //tbQuotation.SetValue(noofvisiblerows);
                        }
                        if (grid.GetVisibleRowsOnPage() > 0) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                            var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                            // Custom Control Data Bind

                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                            //HiddenTax :On 09-01-2018
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            //END


                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            //alert(2);
                            cacbpCrpUdf.PerformCallback();
                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                                }
                                else {
                                    LoadingPanel.Hide();
                                    jAlert('You must enter proper details before save');
                                }
                    }
                
                
                });
                    }
                    else{
                        if (gridquotationLookup.GetValue() == null) {
                            //grid.AddNewRow();
                            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                            //var tbQuotation = grid.GetEditor("SrlNo");
                            //tbQuotation.SetValue(noofvisiblerows);
                        }
                        if (grid.GetVisibleRowsOnPage() > 0) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                            var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                    // Custom Control Data Bind

                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                    //HiddenTax :On 09-01-2018
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    //END


                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    //alert(2);
                    cacbpCrpUdf.PerformCallback();
                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
        }
        else{


            if (aarr.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/SetSessionPacking",
                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        if (gridquotationLookup.GetValue() == null) {
                            //grid.AddNewRow();
                            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                            //var tbQuotation = grid.GetEditor("SrlNo");
                            //tbQuotation.SetValue(noofvisiblerows);
                        }
                        if (grid.GetVisibleRowsOnPage() > 0) {
                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                            //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                            var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                            // Custom Control Data Bind

                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                            //HiddenTax :On 09-01-2018
                            var JsonProductList = JSON.stringify(TaxOfProduct);
                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                            //END


                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                            grid.batchEditApi.EndEdit();
                            //grid.UpdateEdit();
                            //alert(2);
                            cacbpCrpUdf.PerformCallback();
                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                        }
                        else {
                            LoadingPanel.Hide();
                            jAlert('You must enter proper details before save');
                        }
                    }
                
                
                });
            }
            else{


                if (gridquotationLookup.GetValue() == null) {
                    //grid.AddNewRow();
                    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    //var tbQuotation = grid.GetEditor("SrlNo");
                    //tbQuotation.SetValue(noofvisiblerows);
                }
                if (grid.GetVisibleRowsOnPage() > 0) {
                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                    //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                    var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                    // Custom Control Data Bind

                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                    //HiddenTax :On 09-01-2018
                    var JsonProductList = JSON.stringify(TaxOfProduct);
                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                    //END


                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                    grid.batchEditApi.EndEdit();
                    //grid.UpdateEdit();
                    //alert(2);
                    cacbpCrpUdf.PerformCallback();
                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                }
                else {
                    LoadingPanel.Hide();
                    jAlert('You must enter proper details before save');
                }
            }
        }

       
    }
    else {
        LoadingPanel.Hide();
    }

}



function SaveExit_ButtonClick() {
    
    var flag = true;    
    var IsProductwithtaggedDoc=false;
    LoadingPanel.Show();   
    var revdate=ctxtRevisionDate.GetText();
    var  RevisionDate=new Date(revdate);
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
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
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
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
            
    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        LoadingPanel.Hide();
        jAlert("Please Select Project.");
        flag = false;
    }

    //End Rev Rajdip

    $("#<%=hddnSaveOrExitButton.ClientID%>").val('Save_Exit');
    var OrderNo = ctxt_SlOrderNo.GetText();
    var slsdate = cPLSalesOrderDate.GetValue();
    var qudate = cPLQADate.GetText();
    var customerid = GetObjectID('hdnCustomerId').value;
    var IsTaggingMandatory=$("#<%=hdnConfigValueForTagging.ClientID%>").val();
    var salesorderDate = new Date(slsdate);
    var quotationDate = ""; 
    
    if (qudate != null && qudate != '') {
        var qd = qudate.split('-');
        LoadingPanel.Hide();
        quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);

    }
    if (customerid == null || customerid == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorysCustomer').attr('style', 'display:block');
    }
    else {
        $('#MandatorysCustomer').attr('style', 'display:none');
    }
    if(IsTaggingMandatory.trim()=="Y")
    {
        if(gridquotationLookup.GetValue() == null)
        {
            flag = false;
            //gridquotationLookup.SetEnabled(true);
            //$('input[type=radio]').prop('checked', true);
            LoadingPanel.Hide();
            jAlert("Tagging is mandatory.Please select quotation number.");
            //gridquotationLookup.Focus();
        }       
    }
    if (slsdate == null || slsdate == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlDate').attr('style', 'display:block');
    }
    else {
        $('#MandatorySlDate').attr('style', 'display:none');
        if (qudate != null && qudate != '') {
            var qd = qudate.split('-');
            quotationDate = new Date(qd[1] + '-' + qd[0] + '-' + qd[2]);
            if (quotationDate > salesorderDate) {
                flag = false;
                $('#MandatoryEgSDate').attr('style', 'display:block');
            }
            else { $('#MandatoryEgSDate').attr('style', 'display:none'); }
        }
    }
    if (OrderNo == "") {
        flag = false;
        LoadingPanel.Hide();
        $('#MandatorySlOrderNo').attr('style', 'display:block');
    }
    else { $('#MandatorySlOrderNo').attr('style', 'display:none'); }
    grid.AddNewRow();
    //Rev Rajdip
    if($("#hdnApproveStatus").val()==1 && $("#hdnPageStatus").val() == "update" && $("#hdnApprovalReqInq").val() == "1" && $('#hdnRevisionRequiredEveryAfterApproval').val() == "No")
    {        
        //if(ctxtRevisionNo.GetText()=="")
        //{
        var detRev={};
        detRev.RevNo=ctxtRevisionNo.GetText();
        detRev.Order=$("#hdnEditOrderId").val();          
   
        $.ajax({
            type: "POST",
            url: "ProjectOrder.aspx/Duplicaterevnumbercheck",
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
        // }
    }
    if (flag) {        
        SetArrForUOM(); //For UOM Conversion Surojit
        var IsInventory=$("#ddlInventory").val();
        
        if (getUrlVars().InvType != "N") {
            { 
                if (issavePacking == 1) {
                    console.log("1"+IsInventory);
                    if (aarr.length > 0) {
                        $.ajax({
                            type: "POST",
                            url: "ProjectOrder.aspx/SetSessionPacking",
                            data: "{'list':'" + JSON.stringify(aarr) + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {

                                if (gridquotationLookup.GetValue() == null) {
                                    //grid.AddNewRow();
                                    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                                    //var tbQuotation = grid.GetEditor("SrlNo");
                                    //tbQuotation.SetValue(noofvisiblerows);
                                }
                                if (grid.GetVisibleRowsOnPage() > 0) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                                    var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                                    // Custom Control Data Bind

                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                                    //HiddenTax :On 09-01-2018
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    //END


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                            grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    //alert(2);
                                            cacbpCrpUdf.PerformCallback();
                                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                                        }
                                        else {
                                            LoadingPanel.Hide();
                                            jAlert('You must enter proper details before save');
                                        }
                            }
                
                
                        });
                            }
                            else{
                                if (gridquotationLookup.GetValue() == null) {
                                    //grid.AddNewRow();
                                    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                                    //var tbQuotation = grid.GetEditor("SrlNo");
                                    //tbQuotation.SetValue(noofvisiblerows);
                                }
                                if (grid.GetVisibleRowsOnPage() > 0) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                                    var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                                    // Custom Control Data Bind

                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                                    //HiddenTax :On 09-01-2018
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    //END


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    //alert(2);
                                    cacbpCrpUdf.PerformCallback();
                                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                                }
                                else {
                                    LoadingPanel.Hide();
                                    jAlert('You must enter proper details before save');
                                }
                            }
                        }
                        else{

                            console.log("2"+IsInventory);
                            if (aarr.length > 0) {
                                $.ajax({
                                    type: "POST",
                                    url: "ProjectOrder.aspx/SetSessionPacking",
                                    data: "{'list':'" + JSON.stringify(aarr) + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {

                                        if (gridquotationLookup.GetValue() == null) {
                                            //grid.AddNewRow();
                                            //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                                            //var tbQuotation = grid.GetEditor("SrlNo");
                                            //tbQuotation.SetValue(noofvisiblerows);
                                        }
                                        if (grid.GetVisibleRowsOnPage() > 0) {
                                            //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                            //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                                            var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                                            $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                                            // Custom Control Data Bind

                                            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                                            //HiddenTax :On 09-01-2018
                                            var JsonProductList = JSON.stringify(TaxOfProduct);
                                            GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                            //END


                                            $('#<%=hdfIsDelete.ClientID %>').val('I');
                                            grid.batchEditApi.EndEdit();
                                            //grid.UpdateEdit();
                                            //alert(2);
                                            cacbpCrpUdf.PerformCallback();
                                            $('#<%=hdnRefreshType.ClientID %>').val('E');
                                        }
                                        else {
                                            LoadingPanel.Hide();
                                            jAlert('You must enter proper details before save');
                                        }
                                    }
                
                
                                });
                            }
                            else{


                                if (gridquotationLookup.GetValue() == null) {
                                    //grid.AddNewRow();
                                    //var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                                    //var tbQuotation = grid.GetEditor("SrlNo");
                                    //tbQuotation.SetValue(noofvisiblerows);
                                }
                                if (grid.GetVisibleRowsOnPage() > 0) {
                                    //var customerval = (gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) != null) ? gridLookup.GetGridView().GetRowKey(gridLookup.GetGridView().GetFocusedRowIndex()) : "";
                                    //var customerval = (gridLookup.GetValue() != null) ? gridLookup.GetValue() : "";
                                    var customerval = ($("#<%=hdnCustomerId.ClientID%>").val()) != null ? $("#<%=hdnCustomerId.ClientID%>").val() : "";
                                    $('#<%=hdfLookupCustomer.ClientID %>').val(customerval);

                                    // Custom Control Data Bind

                                    $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());

                                    //HiddenTax :On 09-01-2018
                                    var JsonProductList = JSON.stringify(TaxOfProduct);
                                    GetObjectID('hdnJsonProductTax').value = JsonProductList;
                                    //END


                                    $('#<%=hdfIsDelete.ClientID %>').val('I');
                                    grid.batchEditApi.EndEdit();
                                    //grid.UpdateEdit();
                                    //alert(2);
                                    cacbpCrpUdf.PerformCallback();
                                    $('#<%=hdnRefreshType.ClientID %>').val('E');
                                }
                                else {
                                    LoadingPanel.Hide();
                                    jAlert('You must enter proper details before save');
                                }
                            }
                        }

                    }
                }
        // return flag;
            }
        }

        function QuantityTextChange(s, e) {   
               
            var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
            var ProductID = grid.GetEditor('ProductID').GetText();
            var Id = grid.GetEditor('Quotation_No').GetValue();
              
            if (ProductID != null) {
                var SpliteDetails = ProductID.split("||@||");
                var strMultiplier = SpliteDetails[7];
                var strFactor = SpliteDetails[8];
                var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                //var strRate = "1";
                var strStkUOM = SpliteDetails[4];
                //var strSalePrice = SpliteDetails[6];
                var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "0";
                if (strSalePrice == '0') {
                    strSalePrice = SpliteDetails[6];
                }
                grid.batchEditApi.StartEdit(globalRowIndex);
                var Bal_Qty=grid.GetEditor('BalQty').GetValue();      
           
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
                //if(quote_Id.length==0)
                //{
                if(parseFloat(QuantityValue)<parseFloat(Bal_Qty))
                {
                    jAlert('Quantity can not be less than tagged quantity.','Alert',function(){
                           
                        grid.batchEditApi.StartEdit(globalRowIndex, 6);
                        grid.GetEditor('Quantity').SetValue(Bal_Qty);
                    });
                }
                else
                {
                    grid.GetEditor('Quantity').SetValue(QuantityValue);
                    <%--Use for set focus on UOM after press ok on UOM--%> 
                    setTimeout(function () {
                        grid.batchEditApi.StartEdit(globalRowIndex, 8);
                    }, 600);
                    <%--Use for set focus on UOM after press ok on UOM--%> 
                }




                if (strRate == 0) {
                    strRate = 1;
                }

                var StockQuantity = strMultiplier * QuantityValue;
                var Amount = QuantityValue * strFactor * (strSalePrice / strRate);

                $('#<%= lblStkQty.ClientID %>').text(StockQuantity);
                $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);

                //var tbStockQuantity = grid.GetEditor("StockQuantity");
                //tbStockQuantity.SetValue(StockQuantity);

                //Subhabrata added on 06-03-2017
                var IsPackingActive = SpliteDetails[10];
                var Packing_Factor = SpliteDetails[11];
                var Packing_UOM = SpliteDetails[12];
                var PackingValue = (parseFloat((Packing_Factor * QuantityValue).toString())).toFixed(4) + " " + Packing_UOM;

                if (IsPackingActive == "Y") {
                    $('#<%= lblPackingStk.ClientID %>').text(PackingValue);
                    //console.log('jhsdfafa');
                    //divPacking.style.display = "block";
                    $('#divPacking').css({ 'display': 'block' });
                } else {
                    divPacking.style.display = "none";
                }//END


                var tbAmount = grid.GetEditor("Amount");
                tbAmount.SetValue(Amount);

                var TotaAmountRes = '';
                TotaAmountRes = grid.GetEditor('TaxAmount').GetValue();

                var tbTotalAmount = grid.GetEditor("TotalAmount");
                tbTotalAmount.SetValue(Amount + (TotaAmountRes * 1));

                DiscountTextChange(s, e);
            }
            else {
                jAlert('Select a product first.');
                grid.GetEditor('Quantity').SetValue('0');
                grid.GetEditor('ProductID').Focus();
            }
        }

        //function DiscountTextChange(s, e) {
        //    var Amount = (grid.GetEditor('Amount').GetValue() != null) ? grid.GetEditor('Amount').GetValue() : "0";
        //    var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";

        //    var amountAfterDiscount = parseFloat(Amount) - ((parseFloat(Discount) * parseFloat(Amount)) / 100);

        //    var tbAmount = grid.GetEditor("Amount");
        //    tbAmount.SetValue(amountAfterDiscount);

        //    var tbTotalAmount = grid.GetEditor("TotalAmount");
        //    tbTotalAmount.SetValue(amountAfterDiscount);
        //}
        function AddBatchNew(s, e) {

            grid.batchEditApi.EndEdit();
            var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
            if (keyCode === 13) {
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var i;
                var cnt = 2;

                grid.AddNewRow();
                if (noofvisiblerows == "0") {
                    grid.AddNewRow();
                }
                grid.SetFocusedRowIndex();

                for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                    cnt++;
                }

                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(cnt);
            }
        }

        function AddNewRow() {
            grid.AddNewRow();
            var noofvisiblerows = grid.GetVisibleRowsOnPage();
            var tbQuotation = grid.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }

        function OnAddNewClick() {


            if (gridquotationLookup.GetValue() == null) {
                grid.AddNewRow();
                //grid.StartEditRow(0);
                var noofvisiblerows = grid.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = grid.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                //var i;
                //var cnt = 1;
                //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
                //    var tbQuotation = grid.GetEditor("SrlNo");
                //    tbQuotation.SetValue(cnt);


                //    cnt++;
                //}
            }
            else {
                QuotationNumberChanged();
            }
            // $('#ddl_numberingScheme').focus();
        }

        function OnAddGridNewClick() {


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

        function Save_TaxClick() {
            gridTax.UpdateEdit();
            cPopup_Taxes.Hide();
        }

        var Warehouseindex;
        function OnCustomButtonClick(s, e) {

            if (e.buttonID == 'CustomDelete') {
                grid.batchEditApi.EndEdit();
                //
                var noofvisiblerows = grid.GetVisibleRowsOnPage();
                $('#<%=hdnRefreshType.ClientID %>').val('');
                if (gridquotationLookup.GetValue() != null) {
                    //jAlert();
                    jAlert('Cannot Delete using this button as the Proforma is linked with this Sale Order.<br /> Click on Plus(+) sign to Add or Delete Product from last column !', 'Alert Dialog: [Delete Challan Products]', function (r) {

                    });
                }
                if (noofvisiblerows != "1" && gridquotationLookup.GetValue() == null) {
                    grid.DeleteRow(e.visibleIndex);

                    $('#<%=hdfIsDelete.ClientID %>').val('D');
                        grid.UpdateEdit();
                        grid.PerformCallback('Display');

                        grid.batchEditApi.StartEdit(-1, 2);
                        grid.batchEditApi.StartEdit(0, 2);

                        $('#<%=hdnPageStatus.ClientID %>').val('delete');
                    }
                }


                else if(e.buttonID=="CustomInlineRemarks")
                {

                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(e.visibleIndex,6);
                    cPopup_InlineRemarks.Show();

                    $("#txtInlineRemarks").val('');

                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    if (ProductID != "") {
                        // ccallback_InlineRemarks.PerformCallback('BindRemarks'+'~' + '0'+'~'+'0');
                        ccallback_InlineRemarks.PerformCallback('DisplayRemarks'+'~' + SrlNo+'~'+'0');
        
                    }
                    else
                    {
                        $("#txtInlineRemarks").val('');
                    }
                    //$("#txtInlineRemarks").focus();
                    document.getElementById("txtInlineRemarks").focus();
                }

                else if (e.buttonID == 'AddNew') {

                    if (gridquotationLookup.GetValue() == null) {
                        var ProductIDValue = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                        if (ProductIDValue != "") {
                            OnAddNewClick();
                        }
                        else {

                            grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                        }
                    }
                    else {
                        OnAddNewClick();
                    }

                }
                else if (e.buttonID == 'CustomWarehouse') {
                    //
                    var index = e.visibleIndex;
                    grid.batchEditApi.StartEdit(index, 2)
                    Warehouseindex = index;

                    var SrlNo = (grid.GetEditor('SrlNo').GetValue() != null) ? grid.GetEditor('SrlNo').GetValue() : "";
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                    var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                    //var StkQuantityValue = (grid.GetEditor('StockQuantity').GetValue() != null) ? grid.GetEditor('StockQuantity').GetValue() : "0";

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
                        //var strProductName = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";
                        var strProductName = SpliteDetails[1];
                        var StkQuantityValue = QuantityValue * strMultiplier;

                        document.getElementById('<%=lblProductName.ClientID %>').innerHTML = strProductName;
                        document.getElementById('<%=txt_SalesAmount.ClientID %>').innerHTML = QuantityValue;
                        document.getElementById('<%=txt_SalesUOM.ClientID %>').innerHTML = strUOM;
                        document.getElementById('<%=txt_StockAmount.ClientID %>').innerHTML = StkQuantityValue;

                        $('#<%=hdfProductID.ClientID %>').val(strProductID);
                        $('#<%=hdfProductType.ClientID %>').val("");
                        $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                        $('#<%=hdfProductSerialID.ClientID %>').val(SrlNo);
                        $('#<%=hdnProductQuantity.ClientID %>').val(QuantityValue);
                        var Ptype = "";


                        $.ajax({
                            type: "POST",
                            url: 'SalesQuotationList.aspx/getProductType',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{Products_ID:\"" + strProductID + "\"}",
                            async: false,
                            success: function (type) {

                                //$.ajax({
                                //    type: "POST",
                                //    url: 'SalesOrderAdd.aspx/GetIsMandatory',
                                //    contentType: "application/json; charset=utf-8",
                                //    dataType: "json",
                                //    data: "{Products_ID:\"" + strProductID + "\"}",
                                //    async:false,
                                //    success: function (type1) {
                                //        var IsMandatory = type1.d;
                                //    }
                                //});

                                Ptype = type.d;
                                $('#<%=hdfProductType.ClientID %>').val(Ptype);

                                if (Ptype == "W") {
                                    div_Warehouse.style.display = 'block';
                                    div_Batch.style.display = 'none';
                                    div_Serial.style.display = 'none';
                                    div_Quantity.style.display = 'block';
                                    cCmbWarehouse.PerformCallback('BindWarehouse');
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "block");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else if (Ptype == "B") {
                                    div_Warehouse.style.display = 'none';
                                    div_Batch.style.display = 'block';
                                    div_Serial.style.display = 'none';
                                    div_Quantity.style.display = 'block';
                                    cCmbBatch.PerformCallback('BindBatch~' + "0");
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "block");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else if (Ptype == "S") {
                                    div_Warehouse.style.display = 'none';
                                    div_Batch.style.display = 'none';
                                    div_Serial.style.display = 'block';
                                    div_Quantity.style.display = 'none';
                                    checkListBox.PerformCallback('BindSerial~' + "0" + '~' + "0");
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "none");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else if (Ptype == "WB") {
                                    div_Warehouse.style.display = 'block';
                                    div_Batch.style.display = 'block';
                                    div_Serial.style.display = 'none';
                                    div_Quantity.style.display = 'block';
                                    cCmbWarehouse.PerformCallback('BindWarehouse');
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "block");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else if (Ptype == "WS") {
                                    div_Warehouse.style.display = 'block';
                                    div_Batch.style.display = 'none';
                                    div_Serial.style.display = 'block';
                                    div_Quantity.style.display = 'none';
                                    cCmbWarehouse.PerformCallback('BindWarehouse');
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "none");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else if (Ptype == "WBS") {
                                    div_Warehouse.style.display = 'block';
                                    div_Batch.style.display = 'block';
                                    div_Serial.style.display = 'block';
                                    div_Quantity.style.display = 'none';
                                    cCmbWarehouse.PerformCallback('BindWarehouse');
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "none");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else if (Ptype == "BS") {
                                    div_Warehouse.style.display = 'none';
                                    div_Batch.style.display = 'block';
                                    div_Serial.style.display = 'block';
                                    div_Quantity.style.display = 'none';
                                    cCmbBatch.PerformCallback('BindBatch~' + "0");
                                    cGrdWarehouse.PerformCallback('Display~' + SrlNo);
                                    $("#ADelete").css("display", "none");//Subhabrata
                                    cPopup_Warehouse.Show();
                                }
                                else {
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
                        });

                    }
                }
}
function FinalWarehouse() {
    cGrdWarehouse.PerformCallback('WarehouseFinal');
    //Rev Subhra 15-05-2019


    setTimeout(function () {
        grid.batchEditApi.StartEdit(globalRowIndex, 10);
    }, 600);
    //grid.batchEditApi.StartEdit(globalRowIndex, 10);
    //End of Rev Subhra 15-05-2019
}



function callback_InlineRemarks_EndCall(s,e)
{
   
    if(ccallback_InlineRemarks.cpDisplayFocus=="DisplayRemarksFocus")
    {
        $("#txtInlineRemarks").focus();
    }
    else
    {
        cPopup_InlineRemarks.Hide();
        grid.batchEditApi.StartEdit(globalRowIndex, 7);
    }
}


function FinalRemarks() {


    ccallback_InlineRemarks.PerformCallback('RemarksFinal'+'~'+grid.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    $("#txtInlineRemarks").val('');
    
   
}


function closeWarehouse(s, e) {
    e.cancel = false;
    cGrdWarehouse.PerformCallback('WarehouseDelete');
    $('#abpl').popover('hide');//Subhabrata
}

function closeRemarks(s,e)
{
   
    cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    //ccallback_InlineRemarks.PerformCallback('RemarksDelete'+'~'+grid.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
    //cPopup_InlineRemarks.Hide();
    //e.cancel = false;
    // cPopup_InlineRemarks.Hide();
}


function OnWarehouseEndCallback(s, e) {
    var Ptype = document.getElementById('hdfProductType').value;

    if (cGrdWarehouse.cpIsSave == "Y") {
        cPopup_Warehouse.Hide();
        grid.batchEditApi.StartEdit(Warehouseindex, 10);
    }
    else if (cGrdWarehouse.cpIsSave == "N") {
        jAlert('Sales Quantity must be equal to Warehouse.');
    }
    else {
        if (document.getElementById("myCheck").checked == true) {
            if (Ptype == "W" || Ptype == "WB") {
                cCmbWarehouse.SetFocus();
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
                cCmbWarehouse.SetFocus();
            }
            else if (Ptype == "B" || Ptype == "BS") {
                cCmbBatch.Focus();
            }
            else if (Ptype == "S") {
                checkComboBox.Focus();
            }
        }
    }
    //grid.batchEditApi.StartEdit(Warehouseindex, 9);

    //if(cGrdWarehouse.cpWarehouseFinalOK=="WarehouseFinalOK")
    //{
    //    grid.batchEditApi.StartEdit(globalRowIndex, 11);
    //}


}


function SaveWarehouse() {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var WarehouseName = cCmbWarehouse.GetText();
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";
    var BatchName = cCmbBatch.GetText();
    var SerialID = "";
    var SerialName = "";
    var Qty = ctxtQuantity.GetValue();

    var items = checkListBox.GetSelectedItems();
    var vals = [];
    var texts = [];

    for (var i = 0; i < items.length; i++) {
        if (items[i].index != 0) {
            if (i == 0) {
                SerialID = items[i].value;
                SerialName = items[i].text;
            }
            else {
                if (SerialID == "" && SerialID == "") {
                    SerialID = items[i].value;
                    SerialName = items[i].text;
                }
                else {
                    SerialID = SerialID + '||@||' + items[i].value;
                    SerialName = SerialName + '||@||' + items[i].text;
                }
            }
            //texts.push(items[i].text);
            //vals.push(items[i].value);
        }
    }

    //WarehouseID, BatchID, SerialID, Qty=0.0
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    var Ptype = document.getElementById('hdfProductType').value;
    if ((Ptype == "W" && WarehouseID == "0") || (Ptype == "WB" && WarehouseID == "0") || (Ptype == "WS" && WarehouseID == "0") || (Ptype == "WBS" && WarehouseID == "0")) {
        $("#spnCmbWarehouse").show();
    }
    else if ((Ptype == "B" && BatchID == "0") || (Ptype == "WB" && BatchID == "0") || (Ptype == "WBS" && BatchID == "0") || (Ptype == "BS" && BatchID == "0")) {
        $("#spnCmbBatch").show();
    }
    else if ((Ptype == "W" && Qty == "0.0") || (Ptype == "B" && Qty == "0.0") || (Ptype == "WB" && Qty == "0.0")) {
        $("#spntxtQuantity").show();
    }
    else if ((Ptype == "S" && SerialID == "") || (Ptype == "WS" && SerialID == "") || (Ptype == "WBS" && SerialID == "") || (Ptype == "BS" && SerialID == "")) {
        $("#spncheckComboBox").show();
    }
    else {
        if (document.getElementById("myCheck").checked == true && SelectedWarehouseID == "0") {
            if (Ptype == "W" || Ptype == "WB" || Ptype == "B") {
                cCmbWarehouse.PerformCallback('BindWarehouse');
                cCmbBatch.PerformCallback('BindBatch~' + "");
                checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
                ctxtQuantity.SetValue("0");
            }
            else {
                checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
            }
        }
        else {
            cCmbWarehouse.PerformCallback('BindWarehouse');
            cCmbBatch.PerformCallback('BindBatch~' + "");
            checkListBox.PerformCallback('BindSerial~' + "" + '~' + "");
            ctxtQuantity.SetValue("0");
        }
        UpdateText();
        cGrdWarehouse.PerformCallback('SaveDisplay~' + WarehouseID + '~' + WarehouseName + '~' + BatchID + '~' + BatchName + '~' + SerialID + '~' + SerialName + '~' + Qty + '~' + SelectedWarehouseID);
        SelectedWarehouseID = "0";
    }
}



function txtserialTextChanged() {
    checkListBox.UnselectAll();

    var SerialNo = (ctxtserial.GetValue != null) ? (ctxtserial.GetValue()) : "0";
    ctxtserial.SetValue("");
    var texts = [SerialNo];
    var values = GetValuesByTexts(texts);
    checkListBox.SelectValues(values);
    UpdateSelectAllItemState();
    UpdateText(); // for remove non-existing texts
    SaveWarehouse();
}

function AutoCalculateMandateOnChange(element) {
    $("#spnCmbWarehouse").hide();
    $("#spnCmbBatch").hide();
    $("#spncheckComboBox").hide();
    $("#spntxtQuantity").hide();

    if (document.getElementById("myCheck").checked == true) {
        divSingleCombo.style.display = "block";
        divMultipleCombo.style.display = "none";

        checkComboBox.Focus();
    }
    else {
        divSingleCombo.style.display = "none";
        divMultipleCombo.style.display = "block";

        ctxtserial.Focus();
    }
}


function fn_Deletecity(keyValue) {
    var WarehouseID = (cCmbWarehouse.GetValue() != null) ? cCmbWarehouse.GetValue() : "0";
    var BatchID = (cCmbBatch.GetValue() != null) ? cCmbBatch.GetValue() : "0";

    cGrdWarehouse.PerformCallback('Delete~' + keyValue);
    checkListBox.PerformCallback('BindSerial~' + WarehouseID + '~' + BatchID);
}
function fn_Edit(keyValue) {
    //cGrdWarehouse.PerformCallback('EditWarehouse~' + keyValue);
    SelectedWarehouseID = keyValue;
    cCallbackPanel.PerformCallback('EditWarehouse~' + keyValue);
}
            <%-- kaushik 20-2-2017 --%>

        $(document).ready(function () {
            //
            $('#ddl_VatGstCst_I').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            });
            
            $('#txtProjRemarks').blur(function () {
                if (grid.GetVisibleRowsOnPage() == 1) {
                    grid.batchEditApi.StartEdit(-1, 2);
                }
            });

            $('#ddl_AmountAre_I').blur(function () {


                var key = cddl_AmountAre.GetValue();

                if (key == 1 || key == 3) {
                    if (grid.GetVisibleRowsOnPage() == 1) {
                        if($("#hdnProjectSelectInEntryModule").val()=="1")
                        {
                            clookup_Project.SetFocus(); 
                        }
                        else
                        {
                            cdtProjValidFrom.SetFocus();
                            //grid.batchEditApi.StartEdit(-1, 2);
                        }
                        
                    }

                }
            });

        });
        
        <%-- kaushik 20-2-2017 --%>
    </script>
    <script>

        function Revision_LostFocus()
        {
            var detRev={};
            detRev.RevNo=ctxtRevisionNo.GetText();
            detRev.Order=$("#hdnEditOrderId").val();
           
            if(ctxtRevisionNo.GetText()!="" && $("#hdnEditOrderId").val()!="")
            {
                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/RevisionNumberCheck",
                    data: JSON.stringify(detRev),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async:false,
                    success: function (msg) {

                        var duplicateRevCheck=msg.d;
                        if (duplicateRevCheck==1)
                        {
                            jAlert("");
                            $("#txtRevisionNo").val("");
                            ctxtRevisionNo.SetFocus();
                        }

                    }
                });
            }
        }




        function clookup_Project_LostFocus()
        {
            cdtProjValidFrom.SetFocus();
            //grid.batchEditApi.StartEdit(-1, 2);
        }

        function cPLSalesOrderDate_LostFocus()
        {
           
            debugger;
            cdtProjValidFrom.SetMinDate(cPLSalesOrderDate.GetDate());
            // var  PostingDate=cPLSalesOrderDate.GetDate();

            // var validfromDate=cdtProjValidFrom.GetDate();
            // var validUptoDate=cdtProjValidUpto.GetDate();
            // cdtProjValidUpto.SetMinDate(cdtProjValidFrom.GetDate());
            //(PostingDate<validUptoDate)||

            //if((PostingDate!="01-01-0100")&&(validUptoDate!="01-01-0100"))
            //{
            //    if((validfromDate>validUptoDate))
            //   {
            //    cdtProjValidUpto.Clear();
            //    //cdtProjValidUpto.SetFocus();
            //    jAlert("Valid From Date should be Equal or greater than Valid To Date.",'Alert',function(){
            //    cdtProjValidUpto.SetFocus();
            //    });
            //    return false;
            //   }
            //}
            // $("#txtProjRemarks").focus();
           
        }


        function cdtProjValidFrom_LostFocus()
        {
          
            debugger;
           
            //var  PostingDate=cPLSalesOrderDate.GetDate();
            //alert(new Date(PostingDate));
            //var validfromDate=cdtProjValidFrom.GetDate();
            //cdtProjValidFrom.SetMinDate(PostingDate);

            cdtProjValidUpto.SetMinDate(cdtProjValidFrom.GetDate());

            //Rev Rajdip
            //var  PostingDate=cPLSalesOrderDate.GetDate();

            //var validfromDate=cdtProjValidFrom.GetDate();
            //var validUptoDate=cdtProjValidUpto.GetDate();
            //if((PostingDate!="01-01-0100")&&(validfromDate!="01-01-0100"))
            //{
            
            //    if(validfromDate<PostingDate)
            //    {
            //    cdtProjValidFrom.Clear();
            //    //cdtProjValidFrom.SetFocus();
            //    jAlert("Valid From Date should be Equal or greater than Document Date.",'Alert',function(){
            //    cdtProjValidFrom.SetFocus();
            //    });
            //    return false;

            //    }
             
            //}
               
             
            //cdtProjValidFrom.SetMinDate(new Date(PostingDate));
            //END Rev Rajdip

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
        document.onkeydown = function (e) {
            if (event.keyCode == 86 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
                StopDefaultAction(e);


                page.SetActiveTabIndex(0);
                //gridLookup.Focus();
                ctxtCustName.Focus();
                // document.getElementById('Button3').click();

                // return false;
            }
        }

    </script>
    <script type="text/javascript">
        // <![CDATA[
        var textSeparator = ";";
        var selectedChkValue = "";

        function OnListBoxSelectionChanged(listBox, args) {
            if (args.index == 0)
                args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
            UpdateSelectAllItemState();
            UpdateText();

            var selectedItems = checkListBox.GetSelectedItems();
            var val = GetSelectedItemsText(selectedItems);
            var strWarehouse = cCmbWarehouse.GetValue();
            var strBatchID = cCmbBatch.GetValue();
            var ProducttId = $("#hdfProductID").val();
            //$.ajax({
            //    type: "POST",
            //    url: "SalesOrderAdd.aspx/GetSerialId",
            //    data: JSON.stringify({
            //        "id": val,
            //        "wareHouseStr": strWarehouse,
            //        "BatchID": strBatchID,
            //        "ProducttId": ProducttId
            //    }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,//Added By:Subhabrata
            //    success: function (msg) {

            //        var type = msg.d;
            //        if (type == "1") {

            //            return true;
            //        }
            //        else if (type == "0") {
            //            alert("Serial No can be Stock out based on FIFO process.Select the Serial No. shown from Oldest to Newest sequence to proceed");
            //            //listBox.UnselectAll();

            //            var indices = [];
            //            //Added By:Subhabrata
            //            if ((selectedItems.length * 1) == 1) {
            //                indices.push(listBox.GetItem(args.index));
            //                listBox.UnselectIndices(indices[0].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }
            //            if (((args.index) * 1) <= (selectedItems.length * 1)) {
            //                for (var i = ((args.index) * 1) ; i <= ((selectedItems.length * 1) + 1) ; i++) {
            //                    indices.push(listBox.GetItem(i));

            //                }
            //            }
            //            else {
            //                indices.push(listBox.GetItem(args.index));
            //                listBox.UnselectIndices(indices[0].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }

            //            for (var j = 0; j < indices.length   ; j++) {
            //                listBox.UnselectIndices(indices[j].text);
            //                UpdateSelectAllItemState();
            //                UpdateText();
            //            }



            //        }
            //    }
            //});


        }
        function UpdateSelectAllItemState() {
            IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
        }
        function IsAllSelected() {
            var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
            return checkListBox.GetSelectedItems().length == selectedDataItemCount;
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            selectedChkValue = GetSelectedItemsText(selectedItems);
            //checkComboBox.SetText(GetSelectedItemsText(selectedItems));
            checkComboBox.SetText(selectedItems.length + " Items");

            var val = GetSelectedItemsText(selectedItems);
            $("#abpl").attr('data-content', val);
        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            //var texts = dropDown.GetText().split(textSeparator);
            var texts = selectedChkValue.split(textSeparator);
            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            UpdateSelectAllItemState();
            UpdateText(); // for remove non-existing texts
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index != 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
        $(function () {
            $('[data-toggle="popover"]').popover();
        })
        // ]]>
    </script>







    <%-- Date: 30-05-2017    Author: Kallol Samanta  [START] --%>
    <%-- Details: Billing/Shipping user control integration --%>

    <%-- ------Subhra Address and Billing Sectin Start-----25-01-2017---------%>
    <%--

    <script type="text/javascript">


        //Subhra-----23-01-2017-------
        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;
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

        //kaushik-----24-02-2017-------
        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
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
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
            //cComponentPanel.PerformCallback('Edit~BillingAddresstoShipping');
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
                GetObjectID('hdnAddressDtl').value = '1';
                page.SetActiveTabIndex(0);
                gridLookup.Focus();
            }
            else {
                page.SetActiveTabIndex(1);
            }
        }
        function ClosebillingLookup() {
            billingLookup.ConfirmCurrentSelection();
            billingLookup.HideDropDown();
            billingLookup.Focus();
        }
        function CloseshippingLookup() {
            shippingLookup.ConfirmCurrentSelection();
            shippingLookup.HideDropDown();
            shippingLookup.Focus();
        }


        var Billing_state;
        var Billing_city;
        var Billing_pin;
        var billing_area;

        var Shipping_state;
        var Shipping_city;
        var Shipping_pin;
        var Shipping_area;


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
                }
                else {
                    Billing_state = cComponentPanel.cpshow.split('~')[6];
                }
                var bcity = cComponentPanel.cpshow.split('~')[7];
                if (bcity == '') {
                    CmbCity.SetSelectedIndex(-1);
                }
                else {
                    Billing_city = cComponentPanel.cpshow.split('~')[7];
                }

                var bpin = cComponentPanel.cpshow.split('~')[8];
                if (bpin == '') {
                    CmbPin.SetSelectedIndex(-1);
                }
                else {
                    Billing_pin = cComponentPanel.cpshow.split('~')[8];
                }

                var barea = cComponentPanel.cpshow.split('~')[9];
                if (barea == '') {
                    CmbArea.SetSelectedIndex(-1);
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
        }
        //kaushik-----24-02-2017-------
        function OnCountryChanged(cmbCountry) {
            CmbState.PerformCallback(cmbCountry.GetValue().toString());
        }
        function OnCountryChanged1(cmbCountry1) {
            CmbState1.PerformCallback(cmbCountry1.GetValue().toString());
        }
        function OnStateChanged(cmbState) {
            CmbCity.PerformCallback(cmbState.GetValue().toString());
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        }
        function OnStateChanged1(cmbState1) {
            CmbCity1.PerformCallback(cmbState1.GetValue().toString());
            ctaxUpdatePanel.PerformCallback('DeleteAllTax');
        }

        function OnCityChanged(abc) {
            CmbPin.PerformCallback(abc.GetValue().toString());
            CmbArea.PerformCallback(abc.GetValue().toString());
        }
        function OnCityChanged1(abc) {
            CmbPin1.PerformCallback(abc.GetValue().toString());
            CmbArea1.PerformCallback(abc.GetValue().toString());


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

        function Popup_SalesQuote_EndCallBack() {
            if (Popup_SalesQuote.cpshow != null) {


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

            if (Popup_SalesQuote.cpshowShip != null) {


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


        }
    </script>
    --%>
    <%-- ------Subhra Address and Billing Section End-----25-01-2017------- --%>

    <%-- Date: 30-05-2017    Author: Kallol Samanta  [END] --%>





    <style>
        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        /*#grid_DXEditingErrorRow-1 {
            display: none;
        }*/

        /*#grid_DXStatus span > a {
            display: none;
        }

        #gridTax_DXStatus span > a {
            display: none;
        }*/

        #grid_DXStatus {
            display: none;
        }

        #aspxGridTax_DXStatus {
            display: none;
        }

        #gridTax_DXStatus {
            display: none;
        }

        .hideCell {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#btnAdd").bind("click", function () {
                $("#SerialContainer").empty();
                $("#BatchContainer").empty();
                $("#SerialContainer").append("<div><span>Serial Number</span><div />");
                $("#BatchContainer").append("<div><span>Batch Number</span><div />");

                var count = ctxtQuantity_Warehouse.GetValue();


                for (var i = 1; i <= count; i++) {
                    var div = $("<div />");
                    div.html(GetDynamicSerial(""));
                    $("#SerialContainer").append(div);

                    var div1 = $("<div />");
                    div1.html(GetDynamicBatch(""));
                    $("#BatchContainer").append(div1);
                }
            });
        });

        function GetDynamicSerial(value) {
            return '<input name = "SerialContainer" type="text" value = "' + value + '" />'
        }

        function GetDynamicBatch(value) {
            return '<input name = "BatchContainer" type="text" value = "' + value + '" />'
        }


    <%--    function Currency_Rate() {

            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LocalCurrency = '<%=Session["LocalCurrency"]%>';
            var basedCurrency = LocalCurrency.split("~");
            var Currency_ID = cCmbCurrency.GetValue();
            $('#<%=hdnCurrenctId.ClientID %>').val(Currency_ID);


            if (cCmbCurrency.GetText().trim() == basedCurrency[1]) {
                ctxtRate.SetValue("");
                ctxtRate.SetEnabled(false);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "ContraVoucher.aspx/GetRate",
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
        }--%>

        function QuotationNumberChanged() {
            //
            //gridquotationLookup.GetValue()
            //grid.PerformCallback('BindGridOnQuotation' + '~' + cddl_Quotatione.GetValue() + '~' + ctxt_SlOrderNo.GetValue());
            //var quote_Id = gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if ( SimilarProjectStatus != "-1" )
            {
                var quote_Id = gridquotationLookup.GetValue();

                if (quote_Id != null) {
                    var arr = quote_Id.split(',');

                    if (arr.length > 1) {

                        var Doctype=$("#rdl_Salesquotation").find(":checked").val();
                        if(Doctype=="QN")
                            cPLQADate.SetText('Multiple Select Quotation Dates');
                        else if(Doctype=="SINQ")
                            cPLQADate.SetText('Multiple Select Inquiry Dates');
                    }
                    else {
                        if (arr.length == 1) {
                            cComponentDatePanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);


                        }
                        else {
                            cPLQADate.SetText('');

                        }
                    }
                    //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                    //cProductsPopup.Show();

                }
                else { cPLQADate.SetText(''); }

                if (quote_Id != null) {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                    cProductsPopup.Show();
                }

                txt_OANumber.focus();
                <%--if (quote_Id != null) {
                var arr = quote_Id.split(',');

                if (arr.length > 1) {
                    cPLQADate.SetText('Multiple Select Quotation Dates');

                }
                else {
                    if (arr.length == 1) {
                        cComponentPanel.PerformCallback('BindQuotationDate' + '~' + quote_Id);


                    }
                    else {
                        cPLQADate.SetText('');

                    }
                }
            }
            else { cPLQADate.SetText(''); }
            if (quote_Id != null) {
                if ($("<%=hddnOrderNumber.ClientID%>").val() == undefined || IsNaN($("<%=hddnOrderNumber.ClientID%>").val())) {
                    grid.PerformCallback('BindGridOnQuotation' + '~' + quote_Id + '~' + ctxt_SlOrderNo.GetValue());
                }
                else {
                    grid.PerformCallback('BindGridOnQuotation' + '~' + quote_Id + '~' + $("<%=hddnc.ClientID%>").val());
                }

            }
            else {
                if ($("<%=hddnOrderNumber.ClientID%>").val() == undefined || IsNaN($("<%=hddnOrderNumber.ClientID%>").val())) {
                    grid.PerformCallback('BindGridOnQuotation' + '~' + '$' + '~' + ctxt_SlOrderNo.GetValue());
                }
                else {
                    grid.PerformCallback('BindGridOnQuotation' + '~' + '$' + '~' + $("<%=hddnOrderNumber.ClientID%>").val());
                }
            }--%>
            }
        }
        <%--kaushik--%>
        function GridCallBack() {
            
            //grid.PerformCallback('Display');
            $('#ddlInventory').focus();
        }
        <%--kaushik--%>

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }


        function BindOrderProjectdata(OrderId, TagDocType) {
            debugger;
            var OtherDetail = {};

            OtherDetail.OrderId = OrderId;
            OtherDetail.TagDocType = TagDocType;


            if ((OrderId != null) && (OrderId != "")) {

                $.ajax({
                    type: "POST",
                    url: "ProjectOrder.aspx/SetProjectCode",
                    data: JSON.stringify(OtherDetail),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async:false,
                    success: function (msg) {
                        var Code = msg.d;
                       
                        clookup_Project.gridView.SelectItemsByKey(Code[0].ProjectId);
                        clookup_Project.SetEnabled(false);

                        if($("[id$='rdl_Salesquotation']").find(":checked").val()=="SINQ" && msg.d.length>0)
                        {
                            if(msg.d[0].ProjectCode=="")
                            {
                                clookup_Project.SetEnabled(true);
                            }
                            else if(msg.d[0].ProjectCode==null)
                            {
                                clookup_Project.SetEnabled(true);
                            }
                            else if(msg.d[0].ProjectCode==undefined)
                            {
                                clookup_Project.SetEnabled(true);
                            }
                            else
                            {
                                clookup_Project.SetEnabled(false);
                            }
                        }

                       
                    }
                });

                var projID = clookup_Project.GetValue();

                $.ajax({
                    type: "POST",
                    url: 'ProjectOrder.aspx/getHierarchyID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ ProjID: projID }),
                    async:false,
                    success: function (msg) {
                        var data = msg.d;
                        $("#ddlHierarchy").val(data);
                    }
                });
            }
        }



        function PerformCallToGridBind() {

            //
            if(gridquotationLookup.GetValue()!=null)
            {
                grid.PerformCallback('BindGridOnQuotation' + '~' + '@');
                cQuotationComponentPanel.PerformCallback('BindQuotationGridOnSelection');
                $('#hdnPageStatus').val('Quoteupdate');
                cProductsPopup.Hide();
                AllowAddressShipToPartyState = false;
                //#### added by Samrat Roy for Transporter Control #############
                //
                //var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
                //callTransporterControl(quote_Id[0], "QO");

                //#### added by Samrat Roy for Transporter Control #############
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
                if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                    callTransporterControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                }
                

                if (quote_Id.length > 0) {
                    BindOrderProjectdata(quote_Id[0], $("#rdl_Salesquotation").find(":checked").val());
                }
                ///// #### End : Samrat Roy for Transporter Control : End #############
                //grid.Refresh();

                return false;
            }
            else{
                grid.PerformCallback('GridBlank');
                ctaxUpdatePanel.PerformCallback('DeleteAllTax');
                $('input[type=radio]').prop('checked', false);
                gridquotationLookup.SetEnabled(false);
                return false;
            }
            
        }


        function componentEndCallBack(s, e) {
            gridquotationLookup.gridView.Refresh();
            if (grid.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();

            }
            if(TaggingCall)
            {
                gridquotationLookup.Focus();
                TaggingCall=false;
            }
            else
            {
                ctxt_OANumber.Focus();
            }

            //  cQuotationBillingShipping.PerformCallback();
        }
        //Code for UDF Control  kaushik 24-2-2017
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                // var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '';
                var keyval = $('#<%=hdnmodeId.ClientID %>').val();
                //  alert(keyval);
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=SO&&KeyVal_InternalID=' + keyval;
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code  kaushik 24-2-2017
    </script>
    <%--End Sudip--%>

    <%--Debu Section--%>
    <script type="text/javascript">

        function SetRunningTotal() {
            //

            var runningTot = parseFloat(clblProdNetAmt.GetValue());
            for (var i = 0; i < taxJson.length; i++) {
                cgridTax.batchEditApi.StartEdit(i, 3);
                var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                if (taxJson[i].applicableOn == "R") {
                    cgridTax.GetEditor("calCulatedOn").SetValue(runningTot);
                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();

                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var thisRunningAmt = 0;
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100);

                        //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) - GlobalCurTaxAmt);
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));

                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * cgridTax.GetEditor("TaxField").GetValue()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(cgridTax.GetEditor("TaxField").GetValue())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (cgridTax.GetEditor("TaxField").GetValue() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                        GlobalCurTaxAmt = 0;
                    }
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), ProdAmt, sign);
                }
                if (sign == '(+)') {
                    runningTot = runningTot + parseFloat(cgridTax.GetEditor("Amount").GetValue());
                }
                else {
                    runningTot = runningTot - parseFloat(cgridTax.GetEditor("Amount").GetValue());
                }

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


        function gridFocusedRowChanged(s, e) {
            
            globalRowIndex = e.visibleIndex;
        }
        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }


        var taxAmountGlobal;
        function taxAmountGotFocus(s, e) {
            taxAmountGlobal = parseFloat(s.GetValue());
        }
        function taxAmountLostFocus(s, e) {
            //alert('hi');
            var finalTaxAmt = parseFloat(s.GetValue());
            var totAmt = parseFloat(ctxtTaxTotAmt.GetText());
            var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
            var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
            if (sign == '(+)') {
                //ctxtTaxTotAmt.SetValue(Math.round(totAmt + finalTaxAmt - taxAmountGlobal));
                ctxtTaxTotAmt.SetValue(((totAmt * 1) + (finalTaxAmt * 1) - (taxAmountGlobal * 1)).toFixed(2));
            } else {
                //ctxtTaxTotAmt.SetValue(Math.round(totAmt + (finalTaxAmt * -1) - (taxAmountGlobal * -1)));
                ctxtTaxTotAmt.SetValue(((totAmt * 1) + (finalTaxAmt * -1) - (taxAmountGlobal * -1)).toFixed(2));
            }


            //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
            SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);
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
            ctxtTaxTotAmt.SetValue(((totAmt * 1) + (calculatedValue * 1) - (GlobalCurTaxAmt * 1)).toFixed(2));

            //tax others
            SetOtherTaxValueOnRespectiveRow(0, calculatedValue, ccmbGstCstVat.GetText());
            gstcstvatGlobalName = ccmbGstCstVat.GetText();
        }


        //for tax and charges
        var GlobalCurChargeTaxAmt;
        var ChargegstcstvatGlobalName;
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



        var GlobalCurTaxAmt = 0;
        var rowEditCtrl;
        var globalRowIndex;
        var globalTaxRowIndex;
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
                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) - (GlobalCurTaxAmt * 1)).toFixed(2));
                }


                var isValid = taxValue.indexOf('~');
                if (isValid != -1) {
                    var rate = parseFloat(taxValue.split('~')[1]);
                    var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());

                    GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());


                    cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * rate) / 100);
                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * rate) / 100) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * rate) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
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

                    //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                    ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                    GlobalCurTaxAmt = 0;
                } else {
                    s.SetText("");
                }
            }

        }

        function SetOtherTaxValueOnRespectiveRow(idx, amt, name, runninTot, signCal) {
            for (var i = 0; i < taxJson.length; i++) {
                if (taxJson[i].applicableBy == name) {
                    cgridTax.batchEditApi.StartEdit(i, 3);
                    var totCal = 0;
                    if (signCal == '(+)') {
                        totCal = parseFloat(parseFloat(amt) + parseFloat(runninTot));
                    }
                    else {
                        totCal = parseFloat(parseFloat(runninTot) - parseFloat(amt));
                    }
                    cgridTax.GetEditor('calCulatedOn').SetValue(totCal);

                    var totLength = cgridTax.GetEditor("Taxes_Name").GetText().length;
                    var taxNameWithSign = cgridTax.GetEditor("TaxField").GetText();
                    var sign = cgridTax.GetEditor("Taxes_Name").GetText().substring(totLength - 3);
                    var ProdAmt = parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue());
                    var s = cgridTax.GetEditor("TaxField");
                    if (sign == '(+)') {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);

                        //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue()) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                        GlobalCurTaxAmt = 0;
                    }
                    else {
                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
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

                        //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt);
                        ctxtTaxTotAmt.SetValue((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(gridTax.GetEditor("Amount").GetValue());
                        gridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1));
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
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

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + (parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) - GlobalCurTaxAmt));
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + ((ProdAmt * (s.GetText() * 1)) / 100) - (GlobalCurTaxAmt * 1)).toFixed(2));
                        GlobalCurTaxAmt = 0;
                    }
                    else {

                        GlobalCurTaxAmt = parseFloat(cgridTax.GetEditor("Amount").GetValue());
                        cgridTax.GetEditor("Amount").SetValue((parseFloat(ProdAmt * s.GetText()) / 100) * -1);

                        //ctxtTaxTotAmt.SetValue(Math.round(parseFloat(ctxtTaxTotAmt.GetValue()) + ((parseFloat(ProdAmt * parseFloat(s.GetText())) / 100) * -1) - (GlobalCurTaxAmt * -1)));
                        ctxtTaxTotAmt.SetValue(((ctxtTaxTotAmt.GetValue() * 1) + (((ProdAmt * (s.GetText() * 1)) / 100) * -1) - (GlobalCurTaxAmt * -1)).toFixed(2));
                        GlobalCurTaxAmt = 0;
                    }
                    //SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''));
                    SetOtherTaxValueOnRespectiveRow(0, cgridTax.GetEditor("Amount").GetValue(), cgridTax.GetEditor("Taxes_Name").GetText().replace('(+)', '').replace('(-)', ''), parseFloat(cgridTax.GetEditor("calCulatedOn").GetValue()), sign);
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

            ctxtTaxTotAmt.SetValue((totalInlineTaxAmount * 1).toFixed(2));
        }


        var gstcstvatGlobalName;
        function CmbtaxClick(s, e) {
            GlobalCurTaxAmt = parseFloat(ctxtGstCstVat.GetText());
            gstcstvatGlobalName = s.GetText();
        }


        function txtTax_TextChanged(s, i, e) {
            cgridTax.batchEditApi.StartEdit(i, 2);
            var ProdAmt = parseFloat(ctxtprodBasicAmt.GetValue());
            cgridTax.GetEditor("Amount").SetValue(parseFloat(ProdAmt * s.GetText()) / 100);
        }

        function TaxAmountKeyDown(s, e) {

            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function taxAmtButnClick(s, e) {
            //
            if (e.buttonIndex == 0) {

                if (cddl_AmountAre.GetValue() != null) {
                    var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "";

                    if (ProductID.trim() != "") {

                        document.getElementById('setCurrentProdCode').value = ProductID.split('||')[0];
                        document.getElementById('HdSerialNo').value = grid.GetEditor('SrlNo').GetText() == "" ? "1" : grid.GetEditor('SrlNo').GetText();
                        ctxtTaxTotAmt.SetValue(0);
                        ccmbGstCstVat.SetSelectedIndex(0);
                        $('.RecalculateInline').hide();
                        caspxTaxpopUp.Show();
                        //Set Product Gross Amount and Net Amount

                        var QuantityValue = (grid.GetEditor('Quantity').GetValue() != null) ? grid.GetEditor('Quantity').GetValue() : "0";
                        var Discount = (grid.GetEditor('Discount').GetValue() != null) ? grid.GetEditor('Discount').GetValue() : "0";
                        var SpliteDetails = ProductID.split("||@||");
                        var strMultiplier = SpliteDetails[7];
                        var strFactor = SpliteDetails[8];
                        var strRate = (ctxtRate.GetValue() != null && ctxtRate.GetValue() != "0") ? ctxtRate.GetValue() : "1";
                        //var strRate = "1";
                        var strStkUOM = SpliteDetails[4];
                        // var strSalePrice = SpliteDetails[6];
                        var strSalePrice = (grid.GetEditor('SalePrice').GetValue() != null) ? grid.GetEditor('SalePrice').GetValue() : "";
                        if (strRate == 0) {
                            strRate = 1;
                        }
                        document.getElementById('hdnQty').value = grid.GetEditor('Quantity').GetText();                   


                        var StockQuantity = strMultiplier * QuantityValue;
                        var Amount = parseFloat((Math.round((QuantityValue * strFactor * (strSalePrice / strRate)) * 100).toFixed(2)) / 100);

                        var IsDiscountPercentage = document.getElementById('IsDiscountPercentage').value;
                        var amountAfterDiscount = "0";
                        var ResultamountAfterDiscount = "0";
                        if (IsDiscountPercentage == "Y") {
                            ResultamountAfterDiscount = parseFloat(Amount) + ((parseFloat(Discount) * parseFloat(Amount)) / 100);
                            amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                        }
                        else {
                            ResultamountAfterDiscount = parseFloat(Amount) + parseFloat(Discount);
                            amountAfterDiscount = ResultamountAfterDiscount.toFixed(2);
                        }

                        //var _GrossAmt = parseFloat(amountAfterDiscount);
                        var _GrossAmt = (Amount * 1);
                        //var _NetAmt = parseFloat(grid.GetEditor('Amount').GetValue());
                        var _NetAmt = (amountAfterDiscount * 1);

                        clblTaxProdGrossAmt.SetText(_GrossAmt.toFixed(2));
                        clblProdNetAmt.SetText(_NetAmt.toFixed(2));
                        document.getElementById('HdProdGrossAmt').value = _GrossAmt;
                        document.getElementById('HdProdNetAmt').value = _NetAmt;


                        //End Here

                        //Set Discount Here
                        if (parseFloat(grid.GetEditor('Discount').GetValue()) > 0) {
                            //var discount = Math.round((Amount * grid.GetEditor('Discount').GetValue() / 100)).toFixed(2);
                            //var discount = (Amount * grid.GetEditor('Discount').GetValue() / 100);
                            //clblTaxDiscount.SetText(discount);
                            var DiscountAfter = parseFloat(grid.GetEditor('Discount').GetValue());
                            var discount = DiscountAfter.toFixed(2); //Math.abs(parseFloat(Amount) - parseFloat(_GrossAmt));
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
                                        //clblTaxProdGrossAmt.SetText(Math.round(Amount / gstDis).toFixed(2));
                                        clblTaxProdGrossAmt.SetText(parseFloat(Amount / gstDis));
                                        //document.getElementById('HdProdGrossAmt').value = Math.round(Amount / gstDis).toFixed(2);
                                        document.getElementById('HdProdGrossAmt').value = parseFloat(Amount / gstDis);
                                        //clblGstForGross.SetText(Math.round(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)).toFixed(2));
                                        clblGstForGross.SetText(parseFloat(Amount - parseFloat(document.getElementById('HdProdGrossAmt').value)));
                                        clblTaxableNet.SetText("");
                                    }
                                    else {
                                        $('.gstGrossAmount').hide();
                                        //clblProdNetAmt.SetText(Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2));
                                        clblProdNetAmt.SetText(parseFloat(grid.GetEditor('Amount').GetValue() / gstDis));
                                        //document.getElementById('HdProdNetAmt').value = Math.round(grid.GetEditor('Amount').GetValue() / gstDis).toFixed(2);
                                        document.getElementById('HdProdNetAmt').value = parseFloat(grid.GetEditor('Amount').GetValue() / gstDis);
                                        //clblGstForNet.SetText(Math.round(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)).toFixed(2));
                                        clblGstForNet.SetText(parseFloat(grid.GetEditor('Amount').GetValue() - parseFloat(document.getElementById('HdProdNetAmt').value)));
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

                            //###### Added By : Samrat Roy ##########
                            //Get Customer Shipping StateCode
                            var shippingStCode = '';
                            
                            if (cddl_PosGstSalesOrder.GetValue() == "S") {
                                shippingStCode = GeteShippingStateCode();
                            }
                            else {
                                shippingStCode = GetBillingStateCode();
                            }
                            shippingStCode = shippingStCode;

                            //// ###########  Old Code #####################
                            //////Get Customer Shipping StateCode
                            ////var shippingStCode = '';
                            ////if (cchkBilling.GetValue()) {
                            ////    shippingStCode = CmbState.GetText();
                            ////}
                            ////else {
                            ////    shippingStCode = CmbState1.GetText();
                            ////}
                            ////shippingStCode = shippingStCode.substring(shippingStCode.lastIndexOf('(')).replace('(State Code:', '').replace(')', '').trim();

                            //###### END : Samrat Roy : END ########## 


                            //Debjyoti 09032017
                            if (shippingStCode.trim() != '') {
                                for (var cmbCount = 1; cmbCount < ccmbGstCstVat.GetItemCount() ; cmbCount++) {
                                    //Check if gstin is blank then delete all tax
                                    if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] != "") {

                                        if (ccmbGstCstVat.GetItem(cmbCount).value.split('~')[5] == shippingStCode) {

                                            //if its state is union territories then only UTGST will apply
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
                                        //remove tax because GSTIN is not define
                                        ccmbGstCstVat.RemoveItem(cmbCount);
                                        cmbCount--;
                                    }
                                }
                            }


                        }
                        //End here

                        //Start 09-01-2018
                        //if (globalRowIndex > -1) {
                        //    cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                        //} else {

                        //    cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                        //    //Set default combo
                        //    cgridTax.cpComboCode = grid.GetEditor('ProductID').GetText().split('||@||')[9];
                        //}
                        //End

                        //Subhabrata on 09-01-2018
                        var _SrlNo = document.getElementById('HdSerialNo').value;
                        var _IsEntry="";
                        if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length > 0) {
                            _IsEntry=TaxOfProduct.find(o => o.SrlNo === _SrlNo).IsTaxEntry;
                        }

                        if(_IsEntry=="N"){
                            cgridTax.PerformCallback('New~' + cddl_AmountAre.GetValue());
                            //Set default combo
                            cgridTax.cpComboCode = grid.GetEditor('ProductID').GetText().split('||@||')[9];
                        }
                        else{
                            cgridTax.PerformCallback(grid.keys[globalRowIndex] + '~' + cddl_AmountAre.GetValue());
                        }
                        //End

                        ctxtprodBasicAmt.SetValue(grid.GetEditor('Amount').GetValue());
                    } else {
                        grid.batchEditApi.StartEdit(globalRowIndex, 13);
                    }
                }
            }
        }

        function taxAmtButnClick1(s, e) {
            //console.log(grid.GetFocusedRowIndex());
            rowEditCtrl = s;
        }

        function BatchUpdate() {

            //cgridTax.batchEditApi.StartEdit(0, 1);

            //if (cgridTax.GetEditor("TaxField").GetText().indexOf('.') == -1) {
            //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '.00');
            //} else {
            //    cgridTax.GetEditor("TaxField").SetText(cgridTax.GetEditor("TaxField").GetText().trim() + '0');
            //}

            //on 09-01-2018
            var _SrlNo = document.getElementById('HdSerialNo').value;
            if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "Y" }
                TaxOfProduct.push(ProductTaxes)
            }
            else {
                $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "Y"; });
            }
            //END

            cgridTax.UpdateEdit();
            return false;
        }

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
                //ctxtTaxTotAmt.SetValue(gridValue + ddValue);
                ctxtTaxTotAmt.SetValue(((gridValue * 1) + (ddValue * 1)).toFixed(2));
                cgridTax.cpUpdated = "";
            }

            else {
                var totAmt = ctxtTaxTotAmt.GetValue();
                cgridTax.CancelEdit();
                caspxTaxpopUp.Hide();
                grid.batchEditApi.StartEdit(globalRowIndex, 13);
                grid.GetEditor("TaxAmount").SetValue(totAmt);
                grid.GetEditor("TotalAmount").SetValue(parseFloat(totAmt) + parseFloat(grid.GetEditor("Amount").GetValue()));

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

        //kaushik 27-2-2017 grid bind with respect to currency changes
        function ReBindGrid_Currency() {
            var frontRow = 0;
            var backRow = -1;
            var IsProduct = "";
            for (var i = 0; i <= grid.GetVisibleRowsOnPage() ; i++) {
                var frontProduct = (grid.batchEditApi.GetCellValue(backRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(backRow, 'ProductID')) : "";
                var backProduct = (grid.batchEditApi.GetCellValue(frontRow, 'ProductID') != null) ? (grid.batchEditApi.GetCellValue(frontRow, 'ProductID')) : "";

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
                grid.PerformCallback('CurrencyChangeDisplay');
            }
        }

        //kaushik 27-2-2017
    </script>
    <%--Debu Section End--%>

    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .dxeButtonEditClearButton_PlasticBlue {
            display: none;
        }

        #txt_Rate {
            min-height: 24px;
        }

        .col-md-3 > label {
            margin-bottom: 3px;
            margin-top: 0;
            display: block;
        }

        .mTop {
            margin-top: 10px;
            padding: 5px 20px;
        }
    </style>


    <%--Batch Product Popup Start--%>

    <script>
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
            if (e.htmlEvent.key == "Tab") {

                s.OnButtonClick(0);
            }
        }
        var strProductName;
        var TaxOfProduct = [];
        function ProductButnClick(s, e) {
            
            if (e.buttonIndex == 0) {
                //var ProductID = grid.GetEditor('ProductID').GetText();
                //var SpliteDetails = ProductID.split("||@||");
                //strProductName = grid.GetEditor('ProductName').GetText();
                //if (cproductLookUp.Clear()) {

                //    cProductpopUp.Show();
                //    cproductLookUp.Focus();
                //    cproductLookUp.ShowDropDown();
                //}

                $('#txtProdSearch').val('');
                $('#ProductModel').modal('show');
            }
        }

        function prodkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.InventoryType = $("#<%=ddlInventory.ClientID%>").val();
            OtherDetails.ProductIds= $("#<%=hdnnproductIds.ClientID%>").val();
            
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Description");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("IsInventory");
                //HeaderCaption.push("IsInstallation");
                HeaderCaption.push("Class");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetSalesOrderProductList", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
            else if (e.code == "Escape") {
                //grid.GetEditor("ProductName").Focus();
                grid.batchEditApi.StartEdit(globalRowIndex, 3);
            }

        }

        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                //cProductpopUp.Hide();//Subhabrata on 08-01-2018
                grid.batchEditApi.StartEdit(globalRowIndex, 6);

            }
        }

        function ddlInventory_OnChange() {
            
            //cproductLookUp.GetGridView().Refresh();
            $('input[type=radio]').prop('checked', false);
            gridquotationLookup.SetEnabled(false);
            
        }

        var IsInventory = '';

        function SetProduct(Id, Name,e) {
            
            //Subhra 28-03-2019 (Commented Because it is not used anywhere and for this,page getting error)
            //IsInventory = e.parentElement.children[4].innerText;
            //Subhra 28-03-2019
            var LookUpData = Id;
            var ProductCode = Name;
            if (!ProductCode) {
                LookUpData = null;
            }
            $('#ProductModel').modal('hide');

            //cProductpopUp.Hide();Subhabrata on 08-01-2018
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            //console.log(LookUpData);

            //pageheaderContent.style.display = "block";
            // $('#openlink').hide();
            cddl_AmountAre.SetEnabled(false);
            cddl_PosGstSalesOrder.SetEnabled(false);
            // BillShipAddressVisible();
            //Chinmoy added this variable
            AllowAddressShipToPartyState = false;

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

            GetSalesRateSchemePrice($("#hdnCustomerId").val(), strProductID, "0");

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
            divPacking.style.display = "none";
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            //grid.batchEditApi.StartEdit(globalRowIndex, 6);
            
            

            //Debjyoti
            if (grid.GetEditor("ProductID").GetValue() != "" && grid.GetEditor("ProductID").GetValue() != null) {

                ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);
            }

            var _SrlNo = grid.GetEditor("SrlNo").GetValue();
            if (TaxOfProduct.filter(function (e) { return e.SrlNo == _SrlNo; }).length == 0) {
                var ProductTaxes = { SrlNo: _SrlNo, IsTaxEntry: "N" }
                TaxOfProduct.push(ProductTaxes);
                grid.batchEditApi.StartEdit(globalRowIndex,5); 
                SetFocusAfterProductSelect();
            }
            else {
                $.grep(TaxOfProduct, function (e) { if (e.SrlNo == _SrlNo) e.IsTaxEntry = "N"; });
                grid.batchEditApi.StartEdit(globalRowIndex, 5); 
                SetFocusAfterProductSelect();
            }


            //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);

            setTimeout(function () {
                if ($("#ProductMinPrice").val() != "") {
                    grid.GetEditor("SalePrice").SetValue($("#ProductMinPrice").val());
                }
            }, 200); 
           
        }


        function SetFocusAfterProductSelect()
        {
            setTimeout(function () {
                grid.batchEditApi.StartEdit(globalRowIndex,5); 
            }, 200);  
        }


        function ProductSelected(s, e) {
            //
            //LoadingPanel.Show();
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                //cProductpopUp.Hide();//Subhabrata on 08-01-2018
                grid.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }

            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }


            //cProductpopUp.Hide();//Subhabrata on 08-01-2018
            grid.batchEditApi.StartEdit(globalRowIndex);
            grid.GetEditor("ProductID").SetText(LookUpData);
            grid.GetEditor("ProductName").SetText(ProductCode);
            //console.log(LookUpData);

            //pageheaderContent.style.display = "block";
            cddl_AmountAre.SetEnabled(false);

            var tbDescription = grid.GetEditor("Description");
            var tbUOM = grid.GetEditor("UOM");
            var tbSalePrice = grid.GetEditor("SalePrice");
            //var tbStkUOM = grid.GetEditor("StockUOM");
            //var tbStockQuantity = grid.GetEditor("StockQuantity");

            var ProductID = (grid.GetEditor('ProductID').GetText() != null) ? grid.GetEditor('ProductID').GetText() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strSalePrice = SpliteDetails[6];

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
            divPacking.style.display = "none";
            grid.GetEditor("Quantity").SetValue("0.00");
            grid.GetEditor("Discount").SetValue("0.00");
            grid.GetEditor("Amount").SetValue("0.00");
            grid.GetEditor("TaxAmount").SetValue("0.00");
            grid.GetEditor("TotalAmount").SetValue("0.00");

            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strStkUOM);
            //tbStkUOM.SetValue(strStkUOM);
            //tbStockQuantity.SetValue("0");
            //Debjyoti
            ctaxUpdatePanel.PerformCallback('DelProdbySl~' + grid.GetEditor("SrlNo").GetValue() + '~' + strProductID);

            //cacpAvailableStock.PerformCallback('MainAviableStockBind' + '~' + strProductID);

            grid.batchEditApi.StartEdit(globalRowIndex, 6);
        }
        $(document).ready(function () {
            $("#Cross_CloseWindow a").click(function (e) {
                e.preventDefault();
                window.close();
            });
        });
       

    </script>

    <%--Batch Product Popup End--%>





    <%-- Date: 30-05-2017    Author: Kallol Samanta  [START] --%>
    <%-- Details: Billing/Shipping user control integration --%>
    <script lang="Javascript" type="text/javascript">

        function SettingTabStatus() {
            if (GetObjectID('hdnCustomerId').value != null && GetObjectID('hdnCustomerId').value != '' && GetObjectID('hdnCustomerId').value != '0') {
                page.GetTabByName('[B]illing/Shipping').SetEnabled(true);
            }
        }

        //function disp_prompt(name) {
        //    if (name == "tab0") {
        //        //gridLookup.Focus();
        //        ctxtCustName.Focus();
        //    }
        //    if (name == "tab1") {
        //        var custID = GetObjectID('hdnCustomerId').value;
        //        if (custID == null && custID == '') {
        //            jAlert('Please select a customer');
        //            page.SetActiveTabIndex(0);
        //            return;
        //        }
        //        else {
        //            page.SetActiveTabIndex(1);
        //        }
        //    }
        //}
        $(document).ready(function () {
            //Toggle fullscreen expandEntryGrid
            $("#expandgrid").click(function (e) {
                e.preventDefault();
                var $this = $(this);
                if ($this.children('i').hasClass('fa-expand')) {
                    $this.removeClass('hovered half').addClass('full');
                    $this.attr('title', 'Minimize Grid');
                    $this.children('i').removeClass('fa-expand');
                    $this.children('i').addClass('fa-arrows-alt');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
                    var cntWidth = $(this).parent('.makeFullscreen').width();
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    grid.SetHeight(browserHeight - 150);
                    grid.SetWidth(cntWidth);
                }
                else if ($this.children('i').hasClass('fa-arrows-alt')) {
                    $this.children('i').removeClass('fa-arrows-alt');
                    $this.removeClass('full').addClass('hovered half');
                    $this.attr('title', 'Maximize Grid');
                    $this.children('i').addClass('fa-expand');
                    var gridId = $(this).attr('data-instance');
                    $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
                    var browserHeight = document.documentElement.clientHeight;
                    var browserWidth = document.documentElement.clientWidth;
                    grid.SetHeight(300);
                    var cntWidth = $this.parent('.makeFullscreen').width();
                    grid.SetWidth(cntWidth);
                }
            });
        });


        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'ProjectOrder.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });

            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if(quote_Id.length==0)
            {
                $.ajax({
                    type: "POST",
                    url: 'ProjectOrder.aspx/GetTermsConditionsDataFromProjectCode',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ ProjectCode: projID }),
                    success: function (msg) {
                        debugger;

                        cdtDefectPerid.Clear();
                        ctxtDefectPerid.SetText('');
                        ctxtLiquiDamage.SetText('');
                        cdtLiqDmgAppliDt.Clear();
                        ctxtPaymentTerms.SetText('');
                        ctxtOrderType.SetText('');
                        ctxtNatureWork.SetText('');


                        var data = msg.d;
                        CallTaggedOrderTermsCondition(data,"Project");
                    }
                });
            }



        }

        function clookupProjectLostFocus(s, e) {
            var projID = clookup_Project.GetValue();
            if (projID == null && projID=="") {
                $("#ddlHierarchy").val(0);
            }
        }
    </script>
    <style>
        #grid {
            max-width: 100% !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
        function SetEntityType(Id) {
            $.ajax({
                type: "POST",
                url: "SalesQuotation.aspx/GetEntityType",
                data: JSON.stringify({ Id: Id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("#hdnEntityType").val(r.d);
                }
            });
        }
    </script>

    <%-- Date: 30-05-2017    Author: Kallol Samanta  [END] --%>

    <script>
        function GetSalesRateSchemePrice(CustomerID, ProductID, SalesPrice) {

            var date = new Date;
            var seconds = date.getSeconds();
            var minutes = date.getMinutes();
            var hour = date.getHours();

            var times = hour + ':' + minutes;

            var sdate = cPLSalesOrderDate.GetValue();
            var startDate = new Date(sdate);
            var OtherDetails = {}
            OtherDetails.CustomerID = CustomerID;
            OtherDetails.ProductID = ProductID;
            OtherDetails.PostingDate = startDate;//+ ' ' + times;
            $.ajax({
                type: "POST",
                url: "Services/Master.asmx/GetSalesRateSchemePrice",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    console.log(returnObject);
                    if (returnObject.length>0) {
                        $("#ProductMinPrice").val(returnObject[0].MinSalePrice);
                        $("#ProductMaxPrice").val(returnObject[0].MaxSalePrice);
                        $("#hdnRateType").val(returnObject[0].RateType);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Subhra Section Start--%>

    <script src="JS/SearchPopup.js"></script>

    <%--Subhra Section End--%>


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
        <div id="divcross" runat="server" class="crossBtn"><a href="projectOrderList.aspx"><i class="fa fa-times"></i></a></div>
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

                                        <div class="col-md-2" id="ddl_Num" runat="server">

                                            <label>
                                                <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
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
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>

                                        </div>

                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Posting Date" Width="120px" CssClass="inline">
                                                </dxe:ASPxLabel>
                                                <span style="color: red">*</span>
                                            </label>
                                            <%--Rev 1.0 [ LostFocus="function(s, e) { SetLostFocusonDemand(e)}" added ] --%>
                                            <dxe:ASPxDateEdit ID="dt_PLSales" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientInstanceName="cPLSalesOrderDate" Width="100%">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents DateChanged="DateCheck" />
                                                <ClientSideEvents GotFocus="function(s,e){cPLSalesOrderDate.ShowDropDown();}" LostFocus="function(s, e) { SetLostFocusonDemand(e)}" />
                                            </dxe:ASPxDateEdit>

                                            <span id="MandatorySlDate" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor211_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                            <span id="MandatoryEgSDate" style="display: none" class="validclass">
                                                <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor2114_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Project Order date must not be prior date than quotation date"></span>


                                        </div>
                                        <div class="col-md-2">
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

                                                        <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">

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
                                                <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>


                                        </div>
                                        <div class="clear"></div>

                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_ContactPerson" runat="server" Text="Contact Person" CssClass="inline">
                                                </dxe:ASPxLabel>
                                            </label>
                                            <dxe:ASPxComboBox ID="cmbContactPerson" runat="server" OnCallback="cmbContactPerson_Callback" Width="100%" ClientInstanceName="cContactPerson" Font-Size="12px">
                                                <ClientSideEvents EndCallback="cmbContactPersonEndCall" GotFocus="function(s,e){cContactPerson.ShowDropDown();}" />
                                            </dxe:ASPxComboBox>
                                            <%--<asp:DropDownList ID="ddl_ContactPerson" runat="server" TabIndex="6" Width="100%"></asp:DropDownList>--%>
                                        </div>
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Salesman/Agents">
                                                </dxe:ASPxLabel>
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


                                        <div class="col-md-2">
                                            <%--<label>
                                                <dxe:ASPxLabel ID="lbl_quotation_No" runat="server" Text="Quotation Number" Width="120px">
                                                </dxe:ASPxLabel>
                                            </label>--%>
                                            <%--  <dxe:ASPxComboBox ID="ddl_Quotation" runat="server" ClientInstanceName="cddl_Quotatione" TabIndex="12" Width="100%">
                        <clientsideevents selectedindexchanged="function(s, e) { QuotationNumberChanged();}" />
                    </dxe:ASPxComboBox>--%>
                                            <asp:RadioButtonList ID="rdl_Salesquotation" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();" Width="85%">
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


                                                                <dxe:GridViewDataColumn FieldName="Quote_Number" Visible="true" VisibleIndex="1" Caption="Quotation Number" Width="150" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Quote_Date" Visible="true" VisibleIndex="2" Caption="Quotation Date" Width="150" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="name" Visible="true" VisibleIndex="3" Caption="Customer" Width="150" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Branch" Visible="true" VisibleIndex="4" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dxe:GridViewDataColumn>
                                                                <dxe:GridViewDataColumn FieldName="Reference" Visible="true" VisibleIndex="5" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
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
                                        <div class="col-md-2">
                                            <label>
                                                <dxe:ASPxLabel ID="lbl_Quotation_Date" runat="server" ClientInstanceName="clbl_Quotation_Date" Text="Quot./Inq. Date" Width="120px">
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

                                        <div class="col-md-2">
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
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_Refference" runat="server" Text="Reference">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxTextBox ID="txt_Refference" runat="server" Width="100%" MaxLength="50">
                                            </dxe:ASPxTextBox>
                                        </div>



                                        <div class="col-md-1">
                                            <label style="margin: 3px 0; display: block">Currency:  </label>
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
                                            <label style="margin: 3px 0; display: block">Exch. Rate:  </label>
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
                                            <span style="margin: 3px 0; display: block">
                                                <dxe:ASPxLabel ID="lbl_AmountAre" runat="server" Text="Amounts are">
                                                </dxe:ASPxLabel>
                                            </span>
                                            <dxe:ASPxComboBox ID="ddl_AmountAre" runat="server" ClientInstanceName="cddl_AmountAre" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulateGSTCSTVAT(e)}" />
                                            </dxe:ASPxComboBox>

                                        </div>
                                        <div class="col-md-2 lblmTop8" id="divposGst">
                                            <dxe:ASPxLabel ID="lbl_PosForGst" runat="server" Text="Place Of Supply [GST]">
                                            </dxe:ASPxLabel>
                                            <span style="color: red">*</span>
                                            <dxe:ASPxComboBox ID="ddl_PosGstSalesOrder" runat="server" ValueType="System.String" Width="100%" ClientInstanceName="cddl_PosGstSalesOrder">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { PopulatePosGst(e)}" />
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="clear"></div>


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
                                                <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" ValueChanged="ProjectValueChange" LostFocus="clookupProjectLostFocus" />

                                                <%--  <ClearButton DisplayMode="Always">
                                                </ClearButton>--%>
                                            </dxe:ASPxGridLookup>
                                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />
                                        </div>

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
                                        <div class="col-md-2">
                                            <label class="checkbox-inline" style="margin-top: 22px;">
                                                <asp:CheckBox ID="chkSendMail" runat="server"></asp:CheckBox>
                                                <span style="margin: 0px 0; display: block">
                                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Send Email">
                                                    </dxe:ASPxLabel>
                                                </span>
                                            </label>


                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblRearks" runat="server" Text="Remarks"></asp:Label>

                                            <asp:TextBox ID="txtProjRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="3" Columns="10" Height="50px">
                                                    
                                            </asp:TextBox>

                                        </div>


                                        <div class="col-md-6" id="dvAppRejRemarks" style="display: none" runat="server">
                                            <asp:Label ID="lblAppRejRemarks" runat="server" Text="Approve/Reject Remarks"></asp:Label>

                                            <asp:TextBox ID="txtAppRejRemarks" runat="server" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="2" Columns="8" Height="50px"></asp:TextBox>

                                        </div>
                                        <div class="clear"></div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblHierarchy" runat="server" Text="Hierarchy"></asp:Label>

                                            <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%">
                                            </asp:DropDownList>

                                        </div>
                                        <div style="clear: both;"></div>
                                        <div class="col-md-12">

                                            <div style="display: none;">
                                                <a href="javascript:void(0);" onclick="OnAddNewClick()" class="btn btn-primary"><span>Add New</span> </a>
                                            </div>
                                            <div>
                                                <br />
                                            </div>

                                            <div class="makeFullscreen ">
                                                <span class="fullScreenTitle">Add Project Sales Order</span>
                                                <span class="makeFullscreen-icon half hovered " data-instance="grid" title="Maximize Grid" id="expandgrid">
                                                    <i class="fa fa-expand"></i>
                                                </span>
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
                                                    OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" Settings-HorizontalScrollBarMode="Visible"
                                                    Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" SettingsPager-Mode="ShowAllRecords" SettingsBehavior-ColumnResizeMode="Control">
                                                    <SettingsPager Visible="false"></SettingsPager>
                                                    <Settings VerticalScrollBarMode="Auto" />
                                                    <SettingsBehavior AllowDragDrop="False" AllowSort="False" />
                                                    <Columns>
                                                        <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%">
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
                                                        <dxe:GridViewDataTextColumn Caption="Sl" FieldName="SrlNo" ReadOnly="true" Width="4%">
                                                            <PropertiesTextEdit>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Quot./Inq." FieldName="Quotation_Num" ReadOnly="True" Width="110px">
                                                            <PropertiesTextEdit>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>
                                                        <%--Batch Product Popup Start--%>
                                                        <dxe:GridViewDataButtonEditColumn Caption="Product" FieldName="ProductName" Width="200px">
                                                            <PropertiesButtonEdit>
                                                                <ClientSideEvents ButtonClick="ProductButnClick" GotFocus="ProductsGotFocusFromID" KeyDown="ProductKeyDown" />
                                                                <Buttons>
                                                                    <dxe:EditButton Text="..." Width="20px">
                                                                    </dxe:EditButton>
                                                                </Buttons>
                                                            </PropertiesButtonEdit>
                                                        </dxe:GridViewDataButtonEditColumn>



                                                        <%--Batch Product Popup End--%><%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="ProductID" VisibleIndex="3" Width="150">
                                                        <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName">
                                                            <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsGotFocus" />
                                                        </PropertiesComboBox>
                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                        <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" ReadOnly="True" Width="250px">
                                                            <PropertiesTextEdit>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewCommandColumn Caption="Addl Desc." Width="70">
                                                            <CustomButtons>
                                                                <dxe:GridViewCommandColumnCustomButton ID="CustomInlineRemarks" Image-ToolTip="Remarks" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                                    <Image ToolTip="Addl Desc." Url="/assests/images/more.png">
                                                                    </Image>
                                                                </dxe:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                        </dxe:GridViewCommandColumn>




                                                        <dxe:GridViewDataTextColumn Caption="Quantity" CellStyle-HorizontalAlign="Right" FieldName="Quantity" PropertiesTextEdit-MaxLength="14" Width="70">
                                                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                                                <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                                <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                                <Style HorizontalAlign="Right">
                                                            </Style>
                                                            </PropertiesTextEdit>
                                                            <PropertiesTextEdit>
                                                                <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                                                <ClientSideEvents GotFocus="QuantityGotFocus" LostFocus="QuantityTextChange" />
                                                                <ClientSideEvents />
                                                            </PropertiesTextEdit>
                                                            <CellStyle HorizontalAlign="Right">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="UOM(Sale)" FieldName="UOM" ReadOnly="true" Width="80">
                                                            <PropertiesTextEdit>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>
                                                        <%--Caption="Warehouse"--%>
                                                        <dxe:GridViewCommandColumn Caption="Stk Details" Width="80">
                                                            <CustomButtons>
                                                                <dxe:GridViewCommandColumnCustomButton ID="CustomWarehouse" Image-ToolTip="Warehouse" Image-Url="/assests/images/warehouse.png" Text=" ">
                                                                    <Image ToolTip="Warehouse" Url="/assests/images/warehouse.png">
                                                                    </Image>
                                                                </dxe:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Sale Price" CellStyle-HorizontalAlign="Right" FieldName="SalePrice" Width="100">
                                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                                <ClientSideEvents GotFocus="SalesPriceGotFocus" LostFocus="SalePriceTextChange" KeyDown="RateKeydown" />
                                                                <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                <Style HorizontalAlign="Right">
                                                            </Style>
                                                            </PropertiesTextEdit>
                                                            <PropertiesTextEdit>
                                                                <ClientSideEvents GotFocus="SalesPriceGotFocus" LostFocus="SalePriceTextChange" />
                                                                <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                            </PropertiesTextEdit>
                                                            <CellStyle HorizontalAlign="Right">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                        <%--<dxe:GridViewDataSpinEditColumn FieldName="Discount" Caption="Disc(%)" VisibleIndex="9" Width="50">
                                                        <PropertiesSpinEdit MinValue="0" MaxValue="100" AllowMouseWheel="false" DisplayFormatString="0.00" MaxLength="6">
                                                            <SpinButtons ShowIncrementButtons="false"></SpinButtons>
                                                            <ClientSideEvents LostFocus="DiscountTextChange" />
                                                        </PropertiesSpinEdit>
                                                    </dxe:GridViewDataSpinEditColumn>--%>
                                                        <dxe:GridViewDataTextColumn Caption="Discount" FieldName="Discount" HeaderStyle-HorizontalAlign="Right" Width="10%">
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
                                                        <dxe:GridViewDataTextColumn Caption="Amount" CellStyle-HorizontalAlign="Right" FieldName="Amount" Width="100">
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
                                                        <dxe:GridViewDataButtonEditColumn Caption="Charges" FieldName="TaxAmount" Width="75">
                                                            <PropertiesButtonEdit>
                                                                <ClientSideEvents ButtonClick="taxAmtButnClick" GotFocus="taxAmtButnClick1" KeyDown="TaxAmountKeyDown" />
                                                                <Buttons>
                                                                    <dxe:EditButton Text="..." Width="20px">
                                                                    </dxe:EditButton>
                                                                </Buttons>
                                                            </PropertiesButtonEdit>
                                                        </dxe:GridViewDataButtonEditColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Net Amount" CellStyle-HorizontalAlign="Right" FieldName="TotalAmount" ReadOnly="true" Width="80">
                                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                                <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                <Style HorizontalAlign="Right">
                                                            </Style>
                                                            </PropertiesTextEdit>
                                                            <PropertiesTextEdit>
                                                                <MaskSettings AllowMouseWheel="false" Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                                <%--<ClientSideEvents KeyDown="AddBatchNew"></ClientSideEvents>--%>
                                                            </PropertiesTextEdit>
                                                            <CellStyle HorizontalAlign="Right">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>







                                                        <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="ProjectRemarks" Width="150" PropertiesTextEdit-MaxLength="5000">
                                                            <PropertiesTextEdit Style-HorizontalAlign="Left">
                                                                <Style HorizontalAlign="Left">
                                                            </Style>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="40">
                                                            <CustomButtons>
                                                                <dxe:GridViewCommandColumnCustomButton ID="AddNew" Image-Url="/assests/images/add.png" Text=" ">
                                                                    <Image Url="/assests/images/add.png">
                                                                    </Image>
                                                                </dxe:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Quotation No" CellStyle-CssClass="hide" EditCellStyle-CssClass="hide" EditFormCaptionStyle-CssClass="hide" FieldName="Quotation_No" FilterCellStyle-CssClass="hide" FooterCellStyle-CssClass="hide" HeaderStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" Width="0">
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
                                                        <dxe:GridViewDataTextColumn Caption="Stock Qty" FieldName="StockQuantity" Width="0">
                                                            <PropertiesTextEdit DisplayFormatString="0.00">
                                                            </PropertiesTextEdit>
                                                            <PropertiesTextEdit>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Stock UOM" FieldName="StockUOM" ReadOnly="true" Width="0">
                                                            <PropertiesTextEdit>
                                                            </PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="BalQty" FieldName="BalQty" Width="0">
                                                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="hidden Field Id" EditCellStyle-CssClass="hide" FieldName="ProductID" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="hide" ReadOnly="True" VisibleIndex="22" Width="0">
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
                                        <div style="clear: both;"></div>
                                        <br />
                                        <div class="col-md-12">
                                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
                                            </dxe:ASPxButton>
                                            <%--<asp:Button ID="ASPxButton2" runat="server" Text="UDF" CssClass="btn btn-primary" OnClientClick="if(OpenUdf()){ return false;}" />--%>
                                            <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="U&#818;DF" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {if(OpenUdf()){ return false}}" />
                                            </dxe:ASPxButton>


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
                                            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
                                            <uc2:TermsConditionsControl runat="server" ID="TermsConditionsControl" />
                                            <div id="dvSOTC" runat="server" style="display: inline-block">
                                                <ucProject:ProjectTermsConditionsControl runat="server" ID="ProjectTermsConditionsControl" />
                                            </div>
                                            <asp:HiddenField runat="server" ID="hdnProjectSOTC" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionData" />
                                            <asp:HiddenField runat="server" ID="hfTermsConditionDocType" Value="SO" />
                                            <asp:HiddenField runat="server" ID="hdBasketId" />
                                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />

                                            <%-- onclick=""--%>
                                            <a href="javascript:void(0);" id="btnAddNew" runat="server" class="btn btn-primary" style="display: none"><span><u>A</u>ttachment(s)</span></a>


                                            <asp:HiddenField ID="hfControlData" runat="server" />


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
            <asp:SqlDataSource ID="CustomerDataSource" runat="server" />
            <asp:SqlDataSource ID="SalesManDataSource" runat="server" />
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
                            <h4 class="modal-title">Salesman/Agent Search</h4>
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
                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                            Settings-ShowFooter="false" AutoGenerateColumns="False" OnHtmlRowCreated="aspxGridProduct_HtmlRowCreated"
                            OnRowInserting="Productgrid_RowInserting" OnRowUpdating="Productgrid_RowUpdating" OnRowDeleting="Productgrid_RowDeleting" OnDataBinding="grid_Products_DataBinding"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                            <%-- <Settings VerticalScrollableHeight="450" VerticalScrollBarMode="Auto"></Settings>--%>
                            <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                            <SettingsPager Visible="false"></SettingsPager>
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " />
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
                                                <dxe:ASPxTextBox ID="txtQuantity" runat="server" ClientInstanceName="ctxtQuantity" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                    <MaskSettings Mask="<0..999999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                                    <ClientSideEvents TextChanged="function(s, e) {SaveWarehouse();}" />
                                                </dxe:ASPxTextBox>
                                                <span id="spntxtQuantity" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div>
                                            </div>
                                            <div class="Left_Content" style="padding-top: 14px">
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

                <%--   <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <panelcollection>
                    <dxe:PanelContent runat="server">
                    </dxe:PanelContent>
                </panelcollection>
                <clientsideevents endcallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>--%>
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
                <asp:HiddenField runat="server" ID="hdnApproveStatus" />
                <asp:HiddenField runat="server" ID="hdnEditOrderId" />
                <asp:HiddenField runat="server" ID="hdnProjectSelectInEntryModule" />
                <asp:HiddenField runat="server" ID="hdnPageStatForApprove" />
                <asp:HiddenField runat="server" ID="hdnQty" />
                <asp:HiddenField runat="server" ID="hdnEntityType" />
                <%--kaushik 24-2-2017--%>

                <asp:HiddenField ID="hdnRevisionRequiredEveryAfterApproval" runat="server" />
                <asp:HiddenField runat="server" ID="hdnisFirstApprove" />
                 <asp:HiddenField ID="hdnIsfromApproval" runat="server" />
                <%--Rev 1.0--%>
                <asp:HiddenField ID="hdnLockFromDate" runat="server" />
                <asp:HiddenField ID="hdnLockToDate" runat="server" />
                <asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
                <asp:HiddenField ID="hdnLockToDateCon" runat="server" />
                <asp:HiddenField ID="hdnDatafrom" runat="server" />
                <asp:HiddenField ID="hdnDatato" runat="server" />
                <%--End of Rev 1.0--%>
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
        <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel" ClientInstanceName="ctaxUpdatePanel" OnCallback="taxUpdatePanel_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall" />
        </dxe:ASPxCallbackPanel>

        <dxe:ASPxCallbackPanel runat="server" ID="taxUpdatePanel2" ClientInstanceName="ctaxUpdatePanel2" OnCallback="taxUpdatePanel_Callback1">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="ctaxUpdatePanelEndCall2" />
        </dxe:ASPxCallbackPanel>

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

    <div>
        <asp:HiddenField runat="server" ID="hdnIsDistanceCalculate" />

        <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
        <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    </div>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="grid"
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



    <%--  <asp:SqlDataSource ID="sqlQuationList" runat="server"  
        SelectCommand="prc_GetQuotationOnSalesOrder" 
        SelectCommandType="StoredProcedure" 
       >     
      <SelectParameters>
           <asp:Parameter Name="Status" Type="String"   />
          </SelectParameters>
    </asp:SqlDataSource>--%>

    <%--  <asp:SqlDataSource ID="sqlQuationList" runat="server" 
        SelectCommand="select ttq.Quote_Id,ttq.Quote_Number,IsNull(CONVERT(VARCHAR(10), ttq.Quote_Date, 103),'') as Quote_Date	 ,case when( tmc.cnt_middleName is null  or tmc.cnt_middleName='') then isnull(tmc.cnt_firstName,'')+' ' +isnull(tmc.cnt_LastName,'')+' ' else   isnull(tmc.cnt_firstName,'')+' '+ isnull(tmc.cnt_middleName,'')+' ' +isnull(tmc.cnt_LastName,'')+' ' end as name from tbl_trans_Quotation  ttq left join tbl_master_contact tmc on ttq.Customer_Id=tmc.cnt_internalId where ttq.Quote_Number is not null and ttq.Quote_Number <>' '"></asp:SqlDataSource>--%>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
    <asp:HiddenField ID="hdnApprovalReqInq" runat="server" />
    <asp:HiddenField runat="server" ID="ProductMinPrice" />
    <asp:HiddenField runat="server" ID="ProductMaxPrice" />
    <asp:HiddenField runat="server" ID="hdnRateType" />
</asp:Content>

