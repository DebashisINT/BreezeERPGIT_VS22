<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                03-04-2023        2.0.37           Pallab              25768: CRM pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AssignedSalesmanDetails.aspx.cs" Inherits="Reports.Reports.GridReports.AssignedSalesmanDetails" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

         .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
            }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>
    <script type="text/javascript">
        function ClickVIewInfo(keyValue) {
            CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
            var url = '/OMS/Management/Master/View/ViewCustomer.html?id=' + keyValue;

            CAspxDirectCustomerViewPopup.SetContentUrl(url);

            CAspxDirectCustomerViewPopup.RefreshContentUrl();
            CAspxDirectCustomerViewPopup.Show();
        }

        function ClickVIewPInfo(keyValue) {
            CAspxDirectProductViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectProductViewPopup.SetHeight(window.innerHeight - 70);
            var url = '/OMS/Management/Master/View/ViewProduct.html?id=' + keyValue;

            CAspxDirectProductViewPopup.SetContentUrl(url);

            CAspxDirectProductViewPopup.RefreshContentUrl();
            CAspxDirectProductViewPopup.Show();
        }
    </script>
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ClassSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ClassModel').modal('hide');
                    ctxtClass.SetText(Name);
                    GetObjectID('hdnClassId').value = key;


                    ctxtProdName.SetText('');
                    $('#txtProdSearch').val('')

                    var OtherDetailsProd = {}
                    OtherDetailsProd.SearchKey = 'undefined text';
                    OtherDetailsProd.ClassID = '';
                    var HeaderCaption = [];
                    HeaderCaption.push("Code");
                    HeaderCaption.push("Name");
                    HeaderCaption.push("Hsn");

                    callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

                }
                else {
                    ctxtClass.SetText('');
                    GetObjectID('hdnClassId').value = '';
                }
            }
            else if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdncWiseProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }
            else if (ArrName == 'CustomerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustModel').modal('hide');
                    ctxtCustName.SetText(Name);
                    GetObjectID('hdnCustomerId').value = key;
                }
                else {
                    ctxtCustName.SetText('');
                    GetObjectID('hdnCustomerId').value = '';
                }
            }

        }

    </script>
   <%-- For multiselection when click on ok button--%>

   <%-- For Class multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ClassModel').on('shown.bs.modal', function () {
                 $('#txtClassSearch').focus();
             })

         })
         var ClassArr = new Array();
         $(document).ready(function () {
             var ClassObj = new Object();
             ClassObj.Name = "ClassSource";
             ClassObj.ArraySource = ClassArr;
             arrMultiPopup.push(ClassObj);
         })
         function ClassButnClick(s, e) {
             $('#ClassModel').modal('show');
         }

         function Class_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ClassModel').modal('show');
             }
         }

         function Classkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtClassSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Class Name");


                 if ($("#txtClassSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtClassSearch').focus();
             else
                 $('#txtClassSearch').focus();
         }
   </script>
 <%-- For Class multiselection--%>

    <%-- For Product multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ProdModel').on('shown.bs.modal', function () {
                 $('#txtProdSearch').focus();
             })

         })
         var ProdArr = new Array();
         $(document).ready(function () {
             var ProdObj = new Object();
             ProdObj.Name = "ProductSource";
             ProdObj.ArraySource = ProdArr;
             arrMultiPopup.push(ProdObj);
         })
         function ProductButnClick(s, e) {
             $('#ProdModel').modal('show');
         }

         function Product_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ProdModel').modal('show');
             }
         }

         function Productkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtProdSearch").val();
             OtherDetails.ClassID = hdnClassId.value;

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Code");
                 HeaderCaption.push("Name");
                 HeaderCaption.push("Hsn");


                 if ($("#txtProdSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtProdSearch').focus();
             else
                 $('#txtProdSearch').focus();
         }
   </script>
      <%-- For Product multiselection--%>

   <%-- For Customer multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#CustModel').on('shown.bs.modal', function () {
                 $('#txtCustSearch').focus();
             })

         })
         var CustArr = new Array();
         $(document).ready(function () {
             var CustObj = new Object();
             CustObj.Name = "CustomerSource";
             CustObj.ArraySource = CustArr;
             arrMultiPopup.push(CustObj);
         })
         function CustomerButnClick(s, e) {
             $('#CustModel').modal('show');
         }

         function Customer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#CustModel').modal('show');
             }
         }

         function Customerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtCustSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Customer Name");
                 HeaderCaption.push("Unique ID");
                 HeaderCaption.push("Alternate No.");
                 HeaderCaption.push("Address");


                 if ($("#txtCustSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtCustSearch').focus();
             else
                 $('#txtCustSearch').focus();
         }
   </script>
      <%-- For Customer multiselection--%>
    <script type="text/javascript">
        //function monthDiff(start, end) {

        //    //var diff = (dt2.getTime() - dt1.getTime()) / 1000;
        //    //diff /= (60 * 60 * 24 * 7 * 4);
        //    //return Math.abs(Math.round(diff));

        //    dt1 = new Date(cxdeFromDate.GetValue());
        //    dt2 = new Date(cxdeToDate.GetValue());


        //    if (dt1 != "" && dt2 != "") {

        //        var From_date = dt1;
        //        var To_date = dt2;
        //        var diff_date = To_date - From_date;

        //        var years = Math.floor(diff_date / 31536000000);
        //        var months = Math.floor((diff_date % 31536000000) / 2628000000);
        //        var days = Math.floor(((diff_date % 31536000000) % 2628000000) / 86400000);
        //        //$("#Result").html(years + " year(s) " + months + " month(s) " + days + " and day(s)");
        //        //alert( years+" year(s) "+months+" month(s) "+days+" and day(s)");
        //        if (months >= 3)
        //        {
        //            return false;
        //        }
        //        else 
        //        {
        //            return true;
        //        }
               
        //    }
        //    else {
        //        alert("Please select dates");
        //        return false;
        //    }
        //}
       
        </script>
    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }


        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
            Grid.Refresh();
        }
    </script>


    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupApproval.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            cpopupApproval.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }


        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsAssignSalesFilter").val("Y");
            $("#drdExport").val(0);

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            
          
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            //if (ToDate < FromDate)
            if (cxdeToDate.GetDate() < cxdeFromDate.GetValue()) 
            {
                jAlert("From date can not be greater than To date");
            }
            else
            {
                cCallbackPanel.PerformCallback(data);
            }
            
          

            //alert(monthDiff(cxdeFromDate.GetValue(), cxdeToDate.GetValue()));
            dt1 = new Date(cxdeFromDate.GetValue());
            dt2 = new Date(cxdeToDate.GetValue());
            ////var MonthDiff = monthDiff(dt1, dt2);
            //var MonthDiff = monthDiff();
            //if (MonthDiff==false) {
            //    jAlert("Please Select Date range within 3 month");
            //}
            //else {
            //    cCallbackPanel.PerformCallback(data);
            //}

            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function CloseLookup() {
            clookupClass.ConfirmCurrentSelection();
            clookupClass.HideDropDown();
            clookupClass.Focus();
        }

        function _CloseLookup() {
            clookupBrand.ConfirmCurrentSelection();
            clookupBrand.HideDropDown();
            clookupBrand.Focus();
        }

        function popupHide(s, e) {

            cpopupApproval.Hide();
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }

       
    </script>

    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
        
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }
        .pdr20 {
            padding-right:14px;
        }
        .verticaltTBL>tbody>tr>td{
            padding-right:15px;
            padding-bottom:5px;
        }
    </style>

    <style>
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

        select
        {
            height: 30px !important;
            border-radius: 4px;
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
            bottom: 12px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        {
            max-width: 98% !important;
        }*/

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

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }
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
        <%--<div class="panel-title">
            <h3>Stock Valuation</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left verticaltTBL">
            <tr>
                  <%--Rev 1.0 : "for-cust-icon" class add--%> 
                <td class="for-cust-icon">
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                            <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                               ></asp:Label>
                    </div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate" TabIndex="1">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <%--Rev 1.0 : "for-cust-icon" class add--%>
                <td class="for-cust-icon">
                     <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            ></asp:Label>
                    </div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate" TabIndex="2">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>

                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Customer: " CssClass="mylabel1"
                            ></asp:Label>
                    </div>
                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="3">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                </td>

                <td>
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                </div>      
                <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="4">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                
                </td>

                <td >
                     <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                            ></asp:Label>
                     </div>
                     <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </td>
             
                <td style="padding-left: 10px; width:180px; padding-top: 17px" colspan="2">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
                           { %>--%>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}" TabIndex="6">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <%-- <% } %>--%>
                </td>

            </tr>
            


            </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div>
                         <dxe:ASPxGridView ID="ShowGrid" runat="server" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="Grid" KeyFieldName="SLNO"
                            DataSourceID="GenerateEntityServerModeDataSource"  OnSummaryDisplayText="ShowGrid_SummaryDisplayText" TabIndex="7" >
                            <Columns>
                               <%-- <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH_DESCRIPTION"
                                    VisibleIndex="1" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>--%>
                                  <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="SALESMAN"
                                    VisibleIndex="1" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Product Class" FieldName="PRODUCTCLASSNAME"
                                    VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Product" FieldName="PRODUCTNAME"  VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                     <HeaderStyle HorizontalAlign="Left" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="ClickVIewPInfo('<%#Eval("PROD_ID") %>')" class="pad">
                                            <%#Eval("PRODUCTNAME")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CUSTOMER" Width="250px" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="ClickVIewInfo('<%#Eval("CUSTOMER_CODE") %>')" class="pad">
                                            <%#Eval("CUSTOMER")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Assigned On" Width="10%" FieldName="ASSIGNED_DATE" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                                    VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                              
                                <dxe:GridViewDataTextColumn Caption="Contact Person" FieldName="CONTACTPERSON"
                                    VisibleIndex="6" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Contact No." FieldName="CONTACTNO"
                                    VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Status" FieldName="ACTIVITY_STATUS" width="10%"
                                    VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                               

                            </Columns>


                            <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" />
                            <SettingsEditing Mode="Inline">
                            </SettingsEditing>
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />






                             <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings   ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" /> 
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>



                            <TotalSummary>

                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="ASSIGNED_SALESMAN_REPORT" ></dx:LinqServerModeDataSource>


                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>


    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>





    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>
       <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnClassId" runat="server" />
    <!--Class Modal -->

     <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Code</th>
                                 <th>Name</th>
                                <th>Hsn</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
         <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    
    <!--Product Modal -->
  
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
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" width="100%" placeholder="Search By Customer Name or Unique ID" />
                    <div id="CustomerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Customer Name</th>
                                 <th>Unique ID</th>
                                <th>Alternate No.</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('CustomerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->

<asp:HiddenField ID="hdnCustomerId" runat="server" />

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsAssignSalesFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

<dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Customer View" Modal="true" AllowResize="false">
         
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
</dxe:ASPxPopupControl>


<dxe:ASPxPopupControl ID="AspxDirectProductViewPopup" runat="server"
    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectProductViewPopup" Height="650px"
    Width="1020px" HeaderText="Product View" Modal="true" AllowResize="false">
         
    <ContentCollection>
        <dxe:PopupControlContentControl runat="server">
        </dxe:PopupControlContentControl>
    </ContentCollection>
</dxe:ASPxPopupControl>

</asp:Content>

