<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StockMovement.aspx.cs" Inherits="Reports.Reports.GridReports.StockMovement" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
        cursor:default !important;
        }
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

        #ListBoxProjects{
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
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
                    GetObjectID('hdnProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdnProductId').value = '';
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
             OtherDetails.ClassID = "";

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

    <%-- For Project multiselection Mantis : 0025145--%> 
    <script type="text/javascript">
    $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
    });

    $(document).ready(function () {
         var ProjectSelection = document.getElementById('hdnProjectSelection').value;
         if (ProjectSelection == "0") {
             $('#divProj').addClass('hidden');
         }
         else {
             $('#divProj').removeClass('hidden');
         }
     })
    </script>
    <%-- For Project multiselection Mantis : 0025145--%> 

    <script type="text/javascript">

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

        function CallbackPanelEndCall(s, e) {
            Grid.Refresh();
        }

        function CallbackPanelDetEndCall(s, e) {
            cGridStkDet.Refresh();
        }

        function OpenStockDetails(prodid) {
            $("#hfIsStockMoveDetailFilter").val("Y");
            if (CchkIsBranchWise.GetChecked() == false && CchkIsWHWise.GetChecked() == false) {
                jAlert('Please check Branch wise or Warehouse wise for generate the report.');
            }
            else {
                cCallbackPanelDetail.PerformCallback('BndPopupgrid~' + prodid);
                cpopupStockDetail.Show();
                return true;
            }
        }

        function popupHide(s, e) {
            cpopupStockDetail.Hide();
        }
    </script>

    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupStockDetail.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            cpopupStockDetail.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function btn_ShowRecordsClick(e) {
            //Rev Debashis 0025145
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            //End of Rev Debashis 0025145
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsStockMoveSummaryFilter").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            if (CchkIsBranchWise.GetChecked() == false && CchkIsWHWise.GetChecked() == false) {
                jAlert('Please check Branch wise or Warehouse wise for generate the report.');
            }
            else if (cxdeFromDate.GetValue() == null || cxdeToDate.GetValue() == null) {
                jAlert('Date can not be blank.');
            }
                //Rev Debashis 0025145
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data);
                }
            }
                //End of Rev Debashis 0025145
            else {
                cCallbackPanel.PerformCallback(data);
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
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

        //Rev Debashis 0025145
        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }
        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
        }
        //End of Rev Debashis 0025145

        function CheckIsBranchWise(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkIsWHWise.SetCheckState('UnChecked');
            }
            else {
                CchkIsWHWise.SetEnabled(true);
            }
        }

        function CheckIsWHWise(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkIsBranchWise.SetCheckState('UnChecked');
            }
            else {
                CchkIsBranchWise.SetEnabled(true);
            }
        }

        function OpenDetailsDocuments(Doc_ID, TransType) {
            if (TransType == 'PB') {
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SC') {
                url = '/OMS/Management/Activities/SalesChallanAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SD' || TransType == 'OD') {
                url = '/OMS/Management/Activities/CustomerDeliveryPendingOurDelv.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'OUR') {
                url = '/OMS/Management/Activities/OldUnitReceivedFromServiceCenter.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'GRN') {
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SRN') {
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RFS') {
                url = '/OMS/Management/Activities/ReceiveFromServiceCenter.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'BI') {
                url = '/OMS/Management/Activities/BranchTransferIn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RSI') {

            }
            else if (TransType == 'ITS') {
                url = '/OMS/Management/Activities/IssueToServiceCenter.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'BO') {
                url = '/OMS/Management/Activities/BranchTransferOut.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RSO') {
            }
            else if (TransType == 'CRI') {
                url = '/OMS/Management/Activities/IssuetoCustomerReturn.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SAIN' || TransType == 'SAOUT') {
                url = '/OMS/Management/Activities/StockAdjustmentAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'WHSTIN' || TransType == 'WHSTOUT') {
                url = '/OMS/Management/Activities/WarehousewiseStockTransferAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'WHSJIN' || TransType == 'WHSJOUT') {
                url = '/OMS/Management/Activities/WarehousewiseStockJournalAdd.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'StkReceipt') {
                url = '/Import/GoodsReceivedNoteAdd_Import.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }

            popupdocument.SetContentUrl(url);
            popupdocument.Show();

        }

        function DocumentAfterHide(s, e) {
            popupdocument.Hide();
        }

    </script>

    <style>
        .pl-10{
                padding-left: 10px;
        }
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
        .padTbl>tbody>tr>td {
            padding:8px 20px 8px 0;
            
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
            right: 20px;
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut  
        #B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM
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
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
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

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2">
                 <div style="color: #b5285f;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                 <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                     <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </div>
            </div>
            
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label7" runat="Server" Text="Valuation Technique : " CssClass="mylabel1"
                        ></asp:Label>
                </div>
                <%--Rev 1.0: "simple-select" class add --%>
                <div class="simple-select">
                    <asp:DropDownList ID="ddlValTech" runat="server" Width="100%">
                        <asp:ListItem Text="FIFO" Value="F"></asp:ListItem>
                        <asp:ListItem Text="Average" Value="A"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2">
                 <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="Label6" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="clear"></div>

            <div class="col-md-3 pl-10" style="margin-top:10px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkOWMSTVT" Checked="false" Text="Override Product Valuation Technique in Master" ClientInstanceName="CchkOWMSTVT">
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="margin-top:10px;color: #b5285f; font-weight: bold;">
                 <dxe:ASPxCheckBox runat="server" ID="chkConsLandCost" Checked="false" Text="Consider Landed Cost" ClientInstanceName="CchkConsLandCost">
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="margin-top:10px;color: #b5285f; font-weight: bold;">
                 <dxe:ASPxCheckBox runat="server" ID="chkConsOverHeadCost" Checked="false" Text="Consider Overhead Cost" ClientInstanceName="CchkConsOvrHeadCost">
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="margin-top:10px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkIsBranchWise" Checked="True" Text="Branch wise Stock Valuation" ClientInstanceName="CchkIsBranchWise">
                    <ClientSideEvents CheckedChanged="CheckIsBranchWise" />
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-3" style="margin-top:10px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkIsWHWise" Checked="false" Text="Warehouse wise Stock Valuation" ClientInstanceName="CchkIsWHWise">
                    <ClientSideEvents CheckedChanged="CheckIsWHWise" />
                </dxe:ASPxCheckBox>
            </div>

            <div class="clear"></div>
            <%--Rev Debashis 0025145--%>
            <div class="col-md-2 lblmTop8" style="" id="divProj">
                <div style="color: #b5285f; /*font-weight: bold;*/" class="clsTo">
                    <asp:Label ID="lblProj" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
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

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>
            <%--End of Rev Debashis 0025145--%>
           <div class="col-md-2  pb-2">
                <label style="margin-bottom: 0">&nbsp</label>
                    <div class="">
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                        <% if (rights.CanExport)
                            { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                    </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" DataSourceID="GenerateEntityServerModeDataSource"
                            OnHtmlFooterCellPrepared="ShowGrid_HtmlFooterCellPrepared" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="400">
                            <Columns>
                                 <dxe:GridViewDataTextColumn Caption="Product" Width="220px" FieldName="PRODDESC" VisibleIndex="1" FixedStyle="Left" HeaderStyle-CssClass="colDisable">
                                     <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                     <HeaderStyle HorizontalAlign="Center" />
                                     <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenStockDetails('<%#Eval("PRODID") %>')" class="pad">
                                            <%#Eval("PRODDESC")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Width="100px" Caption="Main UOM" VisibleIndex="2" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="ALTSTOCKUOM" Width="100px" Caption="Alt. UOM" VisibleIndex="3" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="CLASSDESC" Width="130px" Caption="Class" VisibleIndex="4" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="BRANDNAME" Width="130px" Caption="Brand" VisibleIndex="5" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewBandColumn Caption="Opening" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="OP_QTY" Caption="Main Qty." Width="100px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="OP_ALTQTY" Caption="Alt. Qty." Width="100px" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="OP_TOTAL" Caption="Amount" Width="100px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Inward" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewBandColumn Caption="Purchase" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Main Qty." Width="100px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL" Caption="Amount" Width="100px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                        <dxe:GridViewBandColumn Caption="Other Inwards" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="OTHIN_QTY" Caption="Main Qty." Width="100px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHALTIN_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHIN_TOTAL" Caption="Amount" Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Outward" VisibleIndex="17" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewBandColumn Caption="Sales" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Main Qty." Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ALTOUT_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OUT_TOTAL" Caption="Amount" Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                        <dxe:GridViewBandColumn Caption="Other Outwards" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="OTHOUT_QTY" Caption="Main Qty." Width="100px" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHALTOUT_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHOUT_TOTAL" Caption="Amount" Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>                                

                                <dxe:GridViewBandColumn Caption="Closing" VisibleIndex="24" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Main Qty." Width="100px" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="CLOSE_ALTQTY" Caption="Alt. Qty." Width="100px" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="CLOSE_VAL" Caption="Amount" Width="100px" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>
                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" AllowSort="False" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="false" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="BRANDNAME" SummaryType="Custom" Tag="Sum_Brand"/>
                                <dxe:ASPxSummaryItem FieldName="OP_QTY" SummaryType="Custom" Tag="Sum_OpQty"/>
                                <dxe:ASPxSummaryItem FieldName="OP_ALTQTY" SummaryType="Custom" Tag="Sum_OpAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OP_TOTAL" SummaryType="Custom" Tag="Sum_OpValue"/>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Custom" Tag="Sum_InQty"/>                                
                                <dxe:ASPxSummaryItem FieldName="ALTIN_QTY" SummaryType="Custom" Tag="Sum_InAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL" SummaryType="Custom" Tag="Sum_InVal"/>
                                <dxe:ASPxSummaryItem FieldName="OTHIN_QTY" SummaryType="Custom" Tag="Sum_OthInQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHALTIN_QTY" SummaryType="Custom" Tag="Sum_OthInAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHIN_TOTAL" SummaryType="Custom" Tag="Sum_OthInVal"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Custom" Tag="Sum_OutQty"/>
                                <dxe:ASPxSummaryItem FieldName="ALTOUT_QTY" SummaryType="Custom" Tag="Sum_OutAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_TOTAL" SummaryType="Custom" Tag="Sum_OutVal"/>
                                <dxe:ASPxSummaryItem FieldName="OTHOUT_QTY" SummaryType="Custom" Tag="Sum_OthOutQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHALTOUT_QTY" SummaryType="Custom" Tag="Sum_OthOutAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHOUT_TOTAL" SummaryType="Custom" Tag="Sum_OthOutVal"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_QTY" SummaryType="Custom" Tag="Sum_CloseQty"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_ALTQTY" SummaryType="Custom" Tag="Sum_CloseAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_VAL" SummaryType="Custom" Tag="Sum_CloseVal"/>
                            </TotalSummary>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="STOCKMOVEMENT_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="popupStockDetail" runat="server" ClientInstanceName="cpopupStockDetail"
        Width="1500px" Height="600px" ScrollBars="Vertical" HeaderText="Stock Movement - Detail" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                    { %> 
                        <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary"
                            AutoPostBack="true" OnSelectedIndexChanged="cmbExportDet_SelectedIndexChanged">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="GridStkDet" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="cGridStkDet" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerDetailsModeDataSource" OnSummaryDisplayText="ShowGrid1_SummaryDisplayText" OnHtmlDataCellPrepared="ShowGrid1_HtmlDataCellPrepared"
                            OnHtmlFooterCellPrepared="ShowGrid1_HtmlFooterCellPrepared" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH_DESCRIPTION" Width="150px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"> 
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Warehouse" FieldName="WHDESC" Width="220px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"> 
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <%--Rev Debashis 0025145--%>
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="PROJ_NAME" Width="200px" Caption="Project Name" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Rev Debashis 0025145--%>

                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="PRODDESC" Width="220px" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Date" FieldName="DOCUMENT_DATE" Width="100px" VisibleIndex="5" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. No" FieldName="DOCUMENT_NO" Width="130px" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                   <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenDetailsDocuments('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>')" class="pad">
                                            <%#Eval("DOCUMENT_NO")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Type" FieldName="TRANS_TYPE" Width="130px" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTY" Width="100px" Caption="Party" VisibleIndex="8" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="CLASSDESC" Width="130px" Caption="Class" VisibleIndex="9" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="BRANDNAME" Width="130px" Caption="Brand" VisibleIndex="10" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewBandColumn Caption="Opening" VisibleIndex="11" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="OP_QTY" Caption="Main Qty." Width="100px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="OP_ALTQTY" Caption="Alt. Qty." Width="100px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="OP_TOTAL" Caption="Amount" Width="100px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Inward" VisibleIndex="15" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewBandColumn Caption="Purchase" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Main Qty." Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ALTIN_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="IN_TOTAL" Caption="Amount" Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                        <dxe:GridViewBandColumn Caption="Other Inwards" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="OTHIN_QTY" Caption="Main Qty." Width="100px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHALTIN_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHIN_TOTAL" Caption="Amount" Width="100px" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Outward" VisibleIndex="22" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewBandColumn Caption="Sales" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Main Qty." Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ALTOUT_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OUT_TOTAL" Caption="Amount" Width="100px" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                        <dxe:GridViewBandColumn Caption="Other Outwards" HeaderStyle-CssClass="colDisable">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <dxe:GridViewDataTextColumn FieldName="OTHOUT_QTY" Caption="Main Qty." Width="100px" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHALTOUT_QTY" Caption="Alt. Qty." Width="100px" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="OTHOUT_TOTAL" Caption="Amount" Width="100px" VisibleIndex="28" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                                    HeaderStyle-CssClass="colDisable">                                        
                                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                </dxe:GridViewDataTextColumn>
                                            </Columns>
                                        </dxe:GridViewBandColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewDataTextColumn FieldName="DOC_RATE" Caption="Doc. Rate" Width="100px" VisibleIndex="29" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 
                                <dxe:GridViewDataTextColumn FieldName="RATE" Caption="Valuation Rate" Width="100px" VisibleIndex="30" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="Closing" VisibleIndex="31" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Main Qty." Width="100px" VisibleIndex="32" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                            HeaderStyle-CssClass="colDisable">
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="CLOSE_ALTQTY" Caption="Alt. Qty." Width="100px" VisibleIndex="33" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                            HeaderStyle-CssClass="colDisable">
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="CLOSE_VAL" Caption="Amount" Width="100px" VisibleIndex="34" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" 
                                            HeaderStyle-CssClass="colDisable">
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>
                            </Columns>

                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" AllowSort="False" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="false" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="BRANDNAME" SummaryType="Custom" Tag="Det_Brand"/>
                                <dxe:ASPxSummaryItem FieldName="OP_QTY" SummaryType="Custom" Tag="Det_OpQty"/>
                                <dxe:ASPxSummaryItem FieldName="OP_ALTQTY" SummaryType="Custom" Tag="Det_OpAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OP_TOTAL" SummaryType="Custom" Tag="Det_OpValue"/>
                                <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Custom" Tag="Det_InQty"/>                                
                                <dxe:ASPxSummaryItem FieldName="ALTIN_QTY" SummaryType="Custom" Tag="Det_InAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="IN_TOTAL" SummaryType="Custom" Tag="Det_InVal"/>
                                <dxe:ASPxSummaryItem FieldName="OTHIN_QTY" SummaryType="Custom" Tag="Det_OthInQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHALTIN_QTY" SummaryType="Custom" Tag="Det_OthInAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHIN_TOTAL" SummaryType="Custom" Tag="Det_OthInVal"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Custom" Tag="Det_OutQty"/>
                                <dxe:ASPxSummaryItem FieldName="ALTOUT_QTY" SummaryType="Custom" Tag="Det_OutAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OUT_TOTAL" SummaryType="Custom" Tag="Det_OutVal"/>
                                <dxe:ASPxSummaryItem FieldName="OTHOUT_QTY" SummaryType="Custom" Tag="Det_OthOutQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHALTOUT_QTY" SummaryType="Custom" Tag="Det_OthOutAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="OTHOUT_TOTAL" SummaryType="Custom" Tag="Det_OthOutVal"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_QTY" SummaryType="Custom" Tag="Det_CloseQty"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_ALTQTY" SummaryType="Custom" Tag="Det_CloseAltQty"/>
                                <dxe:ASPxSummaryItem FieldName="CLOSE_VAL" SummaryType="Custom" Tag="Det_CloseVal"/>
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerDetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerDetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="STOCKMOVEMENT_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="DocumentAfterHide" />
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
                                <th>HSN</th>
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
         <asp:HiddenField ID="hdnProductId" runat="server" />    
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
            <asp:HiddenField ID="hfIsStockMoveSummaryFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
          <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetail" ClientInstanceName="cCallbackPanelDetail" OnCallback="CallbackPanelDetail_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsStockMoveDetailFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelDetEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>
