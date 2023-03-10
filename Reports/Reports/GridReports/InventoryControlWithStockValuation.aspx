<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="InventoryControlWithStockValuation.aspx.cs" Inherits="Reports.Reports.GridReports.InventoryControlWithStockValuation" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
     <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" integrity="sha384-mzrmE5qonljUremFsqc01SB46JvROS7bZs3IO2EmfFsd15uHvIt+Y8vEf7N7fWAU" crossorigin="anonymous">
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
        .inlinecheckbox>input, .inlinecheckbox label 
        {
            display:inline-block !important;
        }
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/ 
    </style>

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
            else if (ArrName == 'BrandSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#BrandModel').modal('hide');
                    ctxtBrandName.SetText(Name);
                    GetObjectID('hdnBranndId').value = key;
                }
                else {
                    ctxtBrandName.SetText('');
                    GetObjectID('hdnBranndId').value = '';
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

   <%-- For Brand multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#BrandModel').on('shown.bs.modal', function () {
                 $('#txtBrandSearch').focus();
             })

         })
         var BrandArr = new Array();
         $(document).ready(function () {
             var BrandObj = new Object();
             BrandObj.Name = "BrandSource";
             BrandObj.ArraySource = BrandArr;
             arrMultiPopup.push(BrandObj);
         })
         function BrandButnClick(s, e) {
             $('#BrandModel').modal('show');
         }

         function Brand_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#BrandModel').modal('show');
             }
         }

         function Brandkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtBrandSearch").val()) == "" || $.trim($("#txtBrandSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtBrandSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Name");

                 if ($("#txtBrandSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetBrand", OtherDetails, "BrandTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "BrandSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtBrandSearch').focus();
             else
                 $('#txtBrandSearch').focus();
         }
   </script>
      <%-- For Brand multiselection--%>

    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }

        $(function () {
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        });

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function AvgMaxLvlCallback_EndCallback() {
            $("#drdMaxLvl").val(0);
        }
        function BelowMaxLvlCallback_EndCallback() {
            $("#drdBelowMaxLvl").val(0);
        }
        function BelowMinLvlCallback_EndCallback() {
            $("#drdBelowMinLvl").val(0);
        }
       <%--Rev Subhra  21-12-2018--%>
        function ShowReorderLvl_EndCallback() {
            $("#drdReorderMaxLvl").val(0);
        }
        //End Rev
        
       function OnEndCallback(s, e) {
           $("#spntotalproduct").text(Grid.cpCountMaxQty);
           $("#spntotalvalue").text(Grid.cpTotalValue);
           $("#spnAMaxlvl").text(Grid.cpCountAboveMaxlvl);
           $("#spnBMinlvl").text(Grid.cpCountBelowMinQty);
           $("#spnBMaxlvl").text(Grid.cpCountAboveMinBelowMaxlvl);
           <%--Rev Subhra  21-12-2018--%>
           $("#spnReordlvl").text(Grid.cpReorderLevel);
           //End Rev
           if (Grid.cpBaseCurrency=='INR')
           {
               spncurrency.innerHTML = "(<i class='fa  fa-rupee-sign'></i>)"
           }
           else {
               spncurrency.innerHTML = Grid.cpBaseCurrencySymboll;
           }
           
       }
        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 24-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function CallbackPanelDetEndCall(s, e) {
            cgridValuationdetails.Refresh();                                                                                                                                                              
        }

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }


    </script>


    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupApproval.GetVisible() == true && cpopupAboveMaxLevel.GetVisible() == false && cpopupBelowMaxLevel.GetVisible() == false && cpopupBelowMinLevel.GetVisible() == false) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
            else if (event.keyCode == 27 && cpopupApproval.GetVisible() == false && cpopupAboveMaxLevel.GetVisible() == true && cpopupBelowMaxLevel.GetVisible() == false && cpopupBelowMinLevel.GetVisible() == false) {
                popupHideAboveMaxLvl();
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
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsInventoryFilter").val("Y");
            $("#hfIsInventoryAvgMaxlvlFilter").val("Y");
            $("#hfIsInventoryBelowMaxlvlFilter").val("Y");
            $("#hfIsInventoryBlwMinlvlFilter").val("Y");
             <%--Rev Subhra  21-12-2018--%>
            $("#hfIsInventoryReorderlvlFilter").val("Y");
            //End Rev
            
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data);
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function btn_AMaxLevelsClick(e) {
            e.preventDefault;
            cShowAMaxLvl.Refresh();
            cpopupAboveMaxLevel.Show();
        }
        function btn_BMinLevelsClick(e) {
            e.preventDefault;
            cShowBelowMinLvl.Refresh();
            cpopupBelowMinLevel.Show();
        }
        function btn_BMaxLevelsClick(e) {
            e.preventDefault;
            cShowBelowMaxLvl.Refresh();
            cpopupBelowMaxLevel.Show();
        }
        <%--Rev Subhra  21-12-2018--%>
        function btn_ReordlvlClick(e) {
            e.preventDefault;
            cShowReorderLvl.Refresh();
            cpopupReorderLevel.Show();
        }
        //End of Rev
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

        function OpenBillDetails(branch, prodid) {
            $("#hfIsInventoryDetFilter").val("Y");
            cCallbackPanelDetail.PerformCallback('BndPopupgrid~' + branch + '~' + prodid);
            cpopupApproval.Show();
            return true;
        }

        function popupHide(s, e) {
            cpopupApproval.Hide();
        }
        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

        });

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
        .blocklableTbl>tbody>tr>td>label {
            display:block;
            
            margin-bottom:4px;
        }
        .blocklableTbl>tbody>tr>td>label>span {
            font-weight:600;
        }
        .blocklableTbl>tbody>tr>td>select {
            margin:0 !important;
        }
        .blocklableTbl>tbody>tr>td:not(:first-child){
            padding-left:25px;
        }
        .btnTD {
                min-width: 200px;
                padding-top: 19px;
        }
        .btnTD>.btn {
            margin:0 !important;
        }
        .badge {
            display: inline-block;
            min-width: 10px;
            padding: 5px 8px;
            font-size: 12px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #929ef0;
            border-radius: 25px;
        }
        .btn .badge {
            background:#fff;
            color:#333;
            margin-left:4px;
            font-size: 14px;
            line-height: 11px;
        }
        .btn-badge-color1:hover, .btn-badge-color2:hover, .btn-badge-color3:hover,.btn-badge-color4:hover,.btn-badge-color5:hover,.btn-badge-color6:hover,
        .btn-badge-color1:focus, .btn-badge-color2:focus, .btn-badge-color3:focus,.btn-badge-color4:focus,.btn-badge-color5:focus,.btn-badge-color6:focus {
            color:#fff;
            opacity:0.8;
        }
        .btn-badge-color1 {
            /*background:chocolate;
            color:#fff;*/
            background:#d9c015;
            color:#fff;
        }
        .btn-badge-color2 {
            background:#30cb57;
            color:#fff;
        }
        .btn-badge-color3 {
            background:#ff6a00;
            color:#fff;
        }
        .btn-badge-color4 {
            background:#7a6fd6;
            color:#fff;
        }
        .btn-badge-color5 {
            background:#6272e2;
            color:#fff;
        }
         .btn-badge-color6 {
            background:#3366FF;
            color:#fff;
        }
        .nocursor{
            cursor:default;
        }
        .auto-style1 {
            height: 145px;
        }
        .m0{
            margin-top:0 !important;
        }
        .mBot0{
            margin-bottom:0 !important;
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
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut  
        /*#B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM*/
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

        /*.padTopbutton {
    padding-top: 27px;
}*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <style>
        .btx-dexwraper{
            margin-bottom: 9px;
        }
        .btx-dexwraper>button {
            border-radius: 20px;
            margin-right: 8px;
            display: inline-table;
            padding: 6px 6px 4px 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
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
                  <%--Rev Subhra 24-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--End of Rev--%>
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left blocklableTbl">       
            <tr>
                
                <td class="simple-select" style="width: 150px">
                    <label><asp:Label ID="Label4" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            ></asp:Label></label>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>

                <td class="branch-selection-box">
                    <label><asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            ></asp:Label></label>
                    <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <%--<div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll"  UseSubmitBehavior="False" />
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll"  UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupbranch" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>

                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </td>

                
                <td>
                        <label> <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label></label>
                        <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                
                </td>


                <td >
                    <label><asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                            ></asp:Label></label>
                     <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                </td>
            

                <td>
                    <label> <asp:Label ID="Label6" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label></label>
                   <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                </td>

                
                <td class="hide">
                    <label><asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            ></asp:Label></label>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                
                <td class="for-cust-icon">
                    <label>  <asp:Label ID="lblToDate" runat="Server" Text="As On Date : " CssClass="mylabel1"
                            ></asp:Label></label>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td style="width:250px;padding-top: 20px;" class="inlinecheckbox">
                    <%--<asp:CheckBox runat="server" ID="chkCmpStkWthPrvsMnths" Checked="false" Text="Consider last two months" />--%>
                    <dxe:ASPxCheckBox runat="server" ID="chkCmpStkWthPrvsMnths" Checked="false" Text="Consider last two months">
                    </dxe:ASPxCheckBox>
                </td>
            </tr>
           
            <tr>
                <td style="width:180px;padding-top: 8px;" class="inlinecheckbox">
                    <%--<asp:CheckBox runat="server" ID="chkConsLandCost" Checked="false" Text="Consider Landed Cost" />--%>
                    <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" Text="Consider Landed Cost">
                    </dxe:ASPxCheckBox>
                </td>
                <td style="width:50px;padding-top: 8px;" class="inlinecheckbox" colspan="2">
                    <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" Text="Consider Overhead Cost">
                    </dxe:ASPxCheckBox>
                </td>
                <td style="padding:15px 0 10px 25px;">                    
                        <button id="btnShow" class="btn btn-success btn-radius mBot0" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                     <% if (rights.CanExport)
                           { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary m0 btn-radius"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                        <% } %>
                   
                </td>
            </tr>
            <tr>
                <td colspan="8" class="pTop15" style="padding-top:8px;border-top: 1px dashed #e2e2e2;">
                    

                    <div class="btx-dexwraper">
                    <%-- <% } %>--%>
                    <button id="btntotalproduct" class="btn btn-badge-color4 nocursor" type="button">Total Products :<span class="badge" id="spntotalproduct">0</span></button>
                    <button id="btntotalvalue" class="btn btn-badge-color5 nocursor" type="button">Total Value :<span id="spncurrency"></span><span class="badge" id="spntotalvalue">0.00</span></button>

                    <%-- <button id="btntotalproduct" class="btn btn-badge-color1" type="button">Total Products : 0</button>
                     <button id="btntotalvalue" class="btn btn-badge-color1" type="button">Total Value : 0.00</button>--%>

                    <%--Rev 22-01-2019--%>
                    <%-- <button id="btnAMaxlvl" class="btn btn-badge-color1" type="button" onclick="btn_AMaxLevelsClick(this);">Above Max Level <span class="badge" id="spnAMaxlvl">0</span></button>
                     <button id="btnBMinlvl" class="btn btn-badge-color3" type="button" onclick="btn_BMinLevelsClick(this);">Below Min Level <span class="badge" id="spnBMinlvl">0</span></button>
                     <button id="btnBMaxlvl" class="btn btn-badge-color2" type="button" onclick="btn_BMaxLevelsClick(this);">Below Max & Above Min Level <span class="badge" id="spnBMaxlvl">0</span></button>
                     <button id="btnReordlvl" class="btn btn-badge-color6" type="button" onclick="btn_ReordlvlClick(this);">Reorder Level <span class="badge" id="spnReordlvl">0</span></button>--%>

                     <button id="btnAMaxlvl" class="btn btn-badge-color1" type="button" onclick="btn_AMaxLevelsClick(this);">Above Max <span class="badge" id="spnAMaxlvl">0</span></button>
                     <button id="btnBMinlvl" class="btn btn-badge-color3" type="button" onclick="btn_BMinLevelsClick(this);">Below Min <span class="badge" id="spnBMinlvl">0</span></button>
                     <button id="btnBMaxlvl" class="btn btn-badge-color2" type="button" onclick="btn_BMaxLevelsClick(this);">Below Max & Above Min <span class="badge" id="spnBMaxlvl">0</span></button>
                     <button id="btnReordlvl" class="btn btn-badge-color6" type="button" onclick="btn_ReordlvlClick(this);">Reorder <span class="badge" id="spnReordlvl">0</span></button>

                    <%--End of Rev 22-01-2019--%>
                    </div>
                </td>
            </tr>
            </table>
       
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2" class="auto-style1">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" DataSourceID="GenerateEntityServerModeDataSource" 
                           Settings-HorizontalScrollBarMode="Visible" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared" >
                             <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                              <Columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Width="360px" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PRODUCTS_DESCRIPTION" Width="360px" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("BRANCH_ID") %>','<%#Eval("PRODUCTS_ID") %>')" class="pad">
                                            <%#Eval("PRODUCTS_DESCRIPTION")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="140px" Caption="Class" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="120px" Caption="Brand" VisibleIndex="4" />
                              
                                 <dxe:GridViewDataTextColumn FieldName="MINLEVEL" Width="100px" Caption="Min. Level" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="MAXLEVEL" Width="100px" Caption="Max. Level" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--Rev Subhra  21-12-2018--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERLEVEL" Width="100px" Caption="Reorder Level" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                 </dxe:GridViewDataTextColumn>
                                <%--End of Rev--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERQTY" Width="100px" Caption="Reorder Qty." VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <%--Rev Subhra  21-01-2019--%>
                                <dxe:GridViewDataTextColumn FieldName="PREVIOUSOFPREVIOUSMONTH_QTY" Width="120px" Caption="PREVIOUSOFPREVIOUSMONTH_QTY" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PREVIOUSMONTH_QTY" Width="120px" Caption="PREVIOUSMONTH_QTY" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <%--End of Rev Subhra  21-01-2019--%>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="120px" Caption="Stock In Hand" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="130px" Caption="Total Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                               <%--Rev Subhra  21-01-2019--%>
                                <dxe:ASPxSummaryItem FieldName="PREVIOUSOFPREVIOUSMONTH_QTY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="PREVIOUSMONTH_QTY" SummaryType="Sum" />
                               <%--End of Rev Subhra  21-01-2019--%>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>

                             <ClientSideEvents EndCallback="OnEndCallback" />
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INVENTORYCONTROLWITHSTOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Inventory Control - Detailed" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                 <% if (rights.CanExport)
                     { %>
                <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                    <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">


                        <dxe:ASPxGridView ID="grivaluation" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="cgridValuationdetails" KeyFieldName="SLNO"
                            DataSourceID="GenerateEntityServerDetailsModeDataSource"  OnSummaryDisplayText="ShowGrid1_SummaryDisplayText" >
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Document Date" FieldName="DOCUMENT_DATE" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                                    VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Document No" FieldName="DOCUMENT_NO"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="TRANS_TYPE"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="PRODUCTS_DESCRIPTION"
                                    VisibleIndex="3" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="100" Caption="Class" VisibleIndex="4" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="100" Caption="Brand" VisibleIndex="5" />

                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Available Stock" Width="12%" VisibleIndex="6" >
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_PRICE_OP" Caption="Rate" Width="12%" VisibleIndex="7">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Caption="Value" Width="12%" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
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
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerDetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerDetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INVENTORYCONTROLWITHSTOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>


    
    <%--------------------------Above Max Level--------------------------%>

     <dxe:ASPxPopupControl ID="popupAboveMaxLevel" runat="server" ClientInstanceName="cpopupAboveMaxLevel" 
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Above Max Level" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad" CloseOnEscape="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                     { %>
                <asp:DropDownList ID="drdMaxLvl" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="drdMaxLvl_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                    <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">

                         <dxe:ASPxGridView runat="server" ID="ShowAMaxLvl" ClientInstanceName="cShowAMaxLvl" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            OnSummaryDisplayText="ShowAMaxLvl_SummaryDisplayText" ClientSideEvents-BeginCallback="AvgMaxLvlCallback_EndCallback" DataSourceID="GenerateAvgMaxLvlEntityServerModeDataSource" 
                           Settings-HorizontalScrollBarMode="Visible" >
                              <Columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Width="360px" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PRODUCTS_DESCRIPTION" Width="360px" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("BRANCH_ID") %>','<%#Eval("PRODUCTS_ID") %>')" class="pad">
                                            <%#Eval("PRODUCTS_DESCRIPTION")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="140px" Caption="Class" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="120px" Caption="Brand" VisibleIndex="4" />
                              
                                 <dxe:GridViewDataTextColumn FieldName="MINLEVEL" Width="100px" Caption="Min. Level" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="MAXLEVEL" Width="100px" Caption="Max. Level" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <%--Rev Subhra  21-12-2018--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERLEVEL" Width="100px" Caption="Reorder Level" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                 </dxe:GridViewDataTextColumn>
                                <%--End of Rev--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERQTY" Width="100px" Caption="Reorder Qty." VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="120px" Caption="Stock In Hand" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="100px" Caption="Total Value" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateAvgMaxLvlEntityServerModeDataSource" runat="server" OnSelecting="GenerateAvgMaxLvlEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INVENTORYCONTROLWITHSTOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hfIsInventoryAvgMaxlvlFilter" runat="server" />
  <%--------------------------Above Max Level--------------------------%>

    <%--------------------------Below Max Level $ Above Min Level--------------------------%>

     <dxe:ASPxPopupControl ID="popupBelowMaxLevel" runat="server" ClientInstanceName="cpopupBelowMaxLevel"
        Width="1500px" Height="600px" ScrollBars="Vertical" HeaderText="
Below Max & Above Min Level" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton" CloseOnEscape="True"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                     { %>
                <asp:DropDownList ID="drdBelowMaxLvl" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="drdBelowMaxLvl_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                    <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">

                         <dxe:ASPxGridView runat="server" ID="ShowBelowMaxLvl" ClientInstanceName="cShowBelowMaxLvl" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            OnSummaryDisplayText="ShowBelowMaxLvl_SummaryDisplayText" ClientSideEvents-BeginCallback="BelowMaxLvlCallback_EndCallback" DataSourceID="GenerateBelowMaxLvlEntityServerModeDataSource" 
                           Settings-HorizontalScrollBarMode="Visible" >
                              <Columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Width="360px" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PRODUCTS_DESCRIPTION" Width="360px" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("BRANCH_ID") %>','<%#Eval("PRODUCTS_ID") %>')" class="pad">
                                            <%#Eval("PRODUCTS_DESCRIPTION")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="140px" Caption="Class" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="120px" Caption="Brand" VisibleIndex="4" />
                              
                                 <dxe:GridViewDataTextColumn FieldName="MINLEVEL" Width="100px" Caption="Min. Level" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="MAXLEVEL" Width="100px" Caption="Max. Level" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                   <%--Rev Subhra  21-12-2018--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERLEVEL" Width="100px" Caption="Reorder Level" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                 </dxe:GridViewDataTextColumn>
                                <%--End of Rev--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERQTY" Width="100px" Caption="Reorder Qty." VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="120px" Caption="Stock In Hand" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="100px" Caption="Total Value" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateBelowMaxLvlEntityServerModeDataSource" runat="server" OnSelecting="GenerateBelowMaxLvlEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INVENTORYCONTROLWITHSTOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hfIsInventoryBelowMaxlvlFilter" runat="server" />
  <%--------------------------Below Max Level--------------------------%>

     <%--------------------------Below Min Level--------------------------%>

     <dxe:ASPxPopupControl ID="popupBelowMinLevel" runat="server" ClientInstanceName="cpopupBelowMinLevel"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Below Min Level" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton" CloseOnEscape="True"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                     { %>
                <asp:DropDownList ID="drdBelowMinLvl" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="drdBelowMinLvl_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                    <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">

                         <dxe:ASPxGridView runat="server" ID="ShowBelowMinLvl" ClientInstanceName="cShowBelowMinLvl" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            OnSummaryDisplayText="ShowBelowMinLvl_SummaryDisplayText" ClientSideEvents-BeginCallback="BelowMinLvlCallback_EndCallback" DataSourceID="GenerateBelowMinLvlEntityServerModeDataSource" 
                           Settings-HorizontalScrollBarMode="Visible" >
                              <Columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Width="360px" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PRODUCTS_DESCRIPTION" Width="360px" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("BRANCH_ID") %>','<%#Eval("PRODUCTS_ID") %>')" class="pad">
                                            <%#Eval("PRODUCTS_DESCRIPTION")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="140px" Caption="Class" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="120px" Caption="Brand" VisibleIndex="4" />
                              
                                 <dxe:GridViewDataTextColumn FieldName="MINLEVEL" Width="100px" Caption="Min. Level" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="MAXLEVEL" Width="100px" Caption="Max. Level" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <%--Rev Subhra  21-12-2018--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERLEVEL" Width="100px" Caption="Reorder Level" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                 </dxe:GridViewDataTextColumn>
                                <%--End of Rev--%>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERQTY" Width="100px" Caption="Reorder Qty." VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="120px" Caption="Stock In Hand" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="100px" Caption="Total Value" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateBelowMinLvlEntityServerModeDataSource" runat="server" OnSelecting="GenerateBelowMinLvlEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INVENTORYCONTROLWITHSTOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hfIsInventoryBlwMinlvlFilter" runat="server" />
  <%--------------------------Below Min Level--------------------------%>


      <%--------------------------Reorder level--------------------------%>

     <dxe:ASPxPopupControl ID="popupReorderLevel" runat="server" ClientInstanceName="cpopupReorderLevel"
        Width="1500px" Height="600px" ScrollBars="Vertical" HeaderText="Reorder Level" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton" CloseOnEscape="True"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                     { %>
                <asp:DropDownList ID="drdReorderMaxLvl" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="drdReorderMaxLvl_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                    <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">

                         <dxe:ASPxGridView runat="server" ID="ShowReorderLvl" ClientInstanceName="cShowReorderLvl" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            OnSummaryDisplayText="ShowReorderLvl_SummaryDisplayText" ClientSideEvents-BeginCallback="ShowReorderLvl_EndCallback" DataSourceID="GenerateReorderLvlEntityServerModeDataSource" 
                           Settings-HorizontalScrollBarMode="Visible" >
                              <Columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Width="360px" Caption="Unit" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PRODUCTS_DESCRIPTION" Width="360px" Caption="Product Name">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenBillDetails('<%#Eval("BRANCH_ID") %>','<%#Eval("PRODUCTS_ID") %>')" class="pad">
                                            <%#Eval("PRODUCTS_DESCRIPTION")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTCLASS_DESCRIPTION" Width="140px" Caption="Class" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="BRAND_NAME" Width="120px" Caption="Brand" VisibleIndex="4" />
                              
                                 <dxe:GridViewDataTextColumn FieldName="MINLEVEL" Width="100px" Caption="Min. Level" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="MAXLEVEL" Width="100px" Caption="Max. Level" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="REORDERLEVEL" Width="100px" Caption="Reorder Level" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                 </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="REORDERQTY" Width="100px" Caption="Reorder Qty." VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Width="120px" Caption="Stock In Hand" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL_OP" Width="100px" Caption="Total Value" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                     <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY_OP" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL_OP" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateReorderLvlEntityServerModeDataSource" runat="server" OnSelecting="GenerateReorderLvlEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INVENTORYCONTROLWITHSTOCKVALUATION_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hfIsInventoryReorderlvlFilter" runat="server" />
  <%--------------------------Reorder level--------------------------%>


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
  
     <!--Brand Modal -->
    <div class="modal fade" id="BrandModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Brand Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Brandkeydown(event)" id="txtBrandSearch" width="100%" placeholder="Search By Brand Name" />
                    <div id="BrandTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('BrandSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('BrandSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnBranndId" runat="server" />
    <!--Brand Modal -->


<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsInventoryFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetail" ClientInstanceName="cCallbackPanelDetail" OnCallback="CallbackPanelDetail_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsInventoryDetFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelDetEndCall" />
</dxe:ASPxCallbackPanel>

</asp:Content>

