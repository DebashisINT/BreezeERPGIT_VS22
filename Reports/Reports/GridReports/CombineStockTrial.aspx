<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CombineStockTrial.aspx.cs" Inherits="Reports.Reports.GridReports.CombineStockTrial" %>

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
                }
                else {
                    ctxtClass.SetText('');
                    GetObjectID('hdnClassId').value = '';
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
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopup2ndLevel.GetVisible() == true && cpopup3rdLevel.GetVisible() == false) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
            else if (event.keyCode == 27 && cpopup3rdLevel.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide3();
            }
        }


        function ClearGridLookup() {
            var grid = gridcustomerLookup.GetGridView();
            grid.UnselectRows();
        }

        function OnWaitingGridKeyPress(e) {
            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                $("#hfIsStockTrialL2Filter").val("Y");
                var index = Grid.GetFocusedRowIndex();
                //$("#hfProductID").val(Grid.GetRowKey(index));
                //cShowGridDetails2Level.PerformCallback(Grid.GetRowKey(index));
                //cCallbackPanelL2.PerformCallback(Grid.GetRowKey(index));
                $("#hfProductID").val(Grid.GetRow(index).children[0].innerHTML);
                cCallbackPanelL2.PerformCallback(Grid.GetRow(index).children[0].innerHTML);
                cpopup2ndLevel.Show();
            }

        }

        function OnWaitingGridKeyPress2(e) {
            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                $("#hfIsStockTrialL3Filter").val("Y");
                var index = cShowGridDetails2Level.GetFocusedRowIndex();
                var productID = cShowGridDetails2Level.GetRow(index).children[1].innerHTML;
                var branchID = cShowGridDetails2Level.GetRow(index).children[3].innerHTML;

                $("#hfProductID2").val(productID);
                $("#hfBranchID2").val(branchID);

                //cShowGridDetails3Level.PerformCallback(productID + "~" + branchID);
                cCallbackPanelL3.PerformCallback(productID + "~" + branchID);
                cpopup3rdLevel.Show();


                //      cShowGridDetails2Level.GetRowValues(cShowGridDetails2Level.GetFocusedRowIndex(), 'sProducts_ID;sProducts_Code;branch_id', OnGetRowValuesLvl2);
            }
        }


        function OnWaitingGridKeyPress3(e) {
            //if (e.code == "Enter" || e.code == "NumpadEnter") {
            if (e.key == "Enter" || e.key == "NumpadEnter") {
                /// var index = cShowGridDetails2Level.GetFocusedRowIndex();
                //var productID = cShowGridDetails2Level.GetRow(index).children[1].innerHTML;
                //var branchID = cShowGridDetails2Level.GetRow(index).children[3].innerHTML;
                //$("#hfProductID2").val(productID);
                //$("#hfBranchID2").val(branchID);
                //  OpenDetailsDocuments
                cShowGridDetails3Level.GetRowValues(cShowGridDetails3Level.GetFocusedRowIndex(), 'Doc_ID;Trans_Type', OnGetRowValuesLvl3);
            }
        }



        //function OnGetRowValuesLvl2(values) {
        //    //  alert(values[0]);

        //    $("#hfProductID2").val(values[0]);
        //    $("#hfBranchID2").val(values[2]);

        //    cShowGridDetails3Level.PerformCallback(values[0] + "~" + values[2]);
        //    cpopup3rdLevel.Show();

        //    // OpenAnalysisDetails(values[0], values[1])
        //}



        function EndShowGridDetails2Level() {
            cShowGridDetails2Level.Focus();
            //ctxtProductCode2ndLevel.SetText(cShowGridDetails2Level.cpProductCode);
            //ctxtProductDesc2ndLevel.SetText(cShowGridDetails2Level.cpProductDesc);
            //$("#lblFromDate2ndLevel")[0].innerHTML = "From " + cShowGridDetails2Level.cpFromDate;
            //$("#lblToDate2ndLevel")[0].innerHTML = " To " + cShowGridDetails2Level.cpToDate;

            //cShowGridDetails2Level.cpProductCode = null;
            //cShowGridDetails2Level.cpProductDesc = null;
            //cShowGridDetails2Level.cpFromDate = null;
            //cShowGridDetails2Level.cpToDate = null;

            ctxtProductCode2ndLevel.SetText(cCallbackPanelL2.cpProductCode);
            ctxtProductDesc2ndLevel.SetText(cCallbackPanelL2.cpProductDesc);
            $("#lblFromDate2ndLevel")[0].innerHTML = "From " + cCallbackPanelL2.cpFromDate;
            $("#lblToDate2ndLevel")[0].innerHTML = " To " + cCallbackPanelL2.cpToDate;
        }

        function EndShowGridDetails3Level() {
            cShowGridDetails3Level.Focus();
            ctxtProductCode3rdLevel.SetText(ctxtProductCode2ndLevel.GetText());
            ctxtProductDesc3rdLevel.SetText(ctxtProductDesc2ndLevel.GetText());
            $("#lblFromDate3rdLevel")[0].innerHTML = $("#lblFromDate2ndLevel")[0].innerHTML;
            $("#lblToDate3rdLevel")[0].innerHTML = $("#lblToDate2ndLevel")[0].innerHTML;
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
            else if (TransType == 'SISRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Doc_ID + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SISRN') {
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

            popupdocument.SetContentUrl(url);
            popupdocument.Show();

        }



        function OnGetRowValuesLvl3(values) {
            var docid = values[0];
            var TransType = values[1];
            //  alert(docid + '' + TransType);
            // alert(type);
            if (TransType == 'PB') {
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'PR') {
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SC') {
                url = '/OMS/Management/Activities/SalesChallanAdd.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'GRN') {
                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SISRM') {
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SISRN') {
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RFS') {
                url = '/OMS/Management/Activities/ReceiveFromServiceCenter.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'BI') {
                url = '/OMS/Management/Activities/BranchTransferIn.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RSI') {

            }
            else if (TransType == 'ITS') {
                url = '/OMS/Management/Activities/IssueToServiceCenter.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'BO') {
                url = '/OMS/Management/Activities/BranchTransferOut.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'RSO') {
            }
            else if (TransType == 'CRI') {
                url = '/OMS/Management/Activities/IssuetoCustomerReturn.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }
            else if (TransType == 'SAIN' || TransType == 'SAOUT') {
                url = '/OMS/Management/Activities/StockAdjustmentAdd.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + TransType;
            }

            console.log(url);
            popupdocument.SetContentUrl(url);
            popupdocument.Show();

        }

        function DocumentAfterHide(s, e) {
            popupdocument.Hide();
        }

        function closePopup(s, e) {
            e.cancel = false;
            Grid.Focus();
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $("#hfProductID").val('');
        }

        function closePopup3(s, e) {
            e.cancel = false;

            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $("#ddlExport3").val(0);
            $("#hfProductID2").val('');
            $("#hfBranchID2").val('');
            cShowGridDetails2Level.Focus();
        }

        function popupHide(s, e) {
            cpopup2ndLevel.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $("#hfProductID").val('');
        }

        function popupHide3(s, e) {
            cpopup3rdLevel.Hide();
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $("#ddlExport3").val(0);
            $("#hfProductID2").val('');
            $("#hfBranchID2").val('');
            cShowGridDetails2Level.Focus();
        }

    </script>

    <script type="text/javascript">


        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";

            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            $("#hfIsStockTrialL1Filter").val("Y");
            var data = "OnDateChanged";

            data += '~' + cxdeToDate.GetDate();

            //Grid.PerformCallback(data);
            LoadingPanel.Show();
            cCallbackPanelL1.PerformCallback(data);

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

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
            Grid.Focus();
            Grid.SetFocusedRowIndex(0);
            LoadingPanel.Hide();
        }

        function CallbackPanelL1EndCall(s, e) {
            Grid.Refresh();
        }

        function CallbackPanelL2EndCall(s, e) {
            cShowGridDetails2Level.Refresh();
        }

        function CallbackPanelL3EndCall(s, e) {
            cShowGridDetails3Level.Refresh();
        }

        //function _selectAll_Class() {
        //    clookupClass.gridView.SelectRows();
        //    document.getElementById("hflookupClassAllFlag").value = "ALL";
        //}
        //function _unselectAll_Class() {
        //    clookupClass.gridView.UnselectRows();
        //    document.getElementById("hflookupClassAllFlag").value = "";
        //}
        //function _CloseLookup_Class() {
        //    clookupClass.ConfirmCurrentSelection();
        //    clookupClass.HideDropDown();
        //    clookupClass.Focus();
        //}

        //function _selectAll_Brand() {
        //    clookupBrand.gridView.SelectRows();
        //    document.getElementById("hflookupBrandAllFlag").value = "ALL";
        //}
        //function _unselectAll_Brand() {
        //    clookupBrand.gridView.UnselectRows();
        //    document.getElementById("hflookupBrandAllFlag").value = "";
        //}
        //function _CloseLookup_Brand() {
        //    clookupBrand.ConfirmCurrentSelection();
        //    clookupBrand.HideDropDown();
        //    clookupBrand.Focus();
        //}

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
            bottom: 7px;
            right: 4px;
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

        .panel-title h3
        {
            padding-top: 0px;
        }
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Combined Stock Trial</h3>
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="form_main">

            <table class="pull-left">
                <tr>
                    <%--<td style="">
                        <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label1" runat="Server" Text="Class : " CssClass="mylabel1"
                                Width="92px"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 205px">
                        <asp:HiddenField ID="hflookupClassAllFlag" runat="server" Value="" />
                        <dxe:ASPxGridLookup ID="lookupClass" ClientInstanceName="clookupClass" SelectionMode="Multiple" runat="server"
                            OnDataBinding="lookupClass_DataBinding" KeyFieldName="ProductClass_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                <dxe:GridViewDataColumn FieldName="ProductClass_Name" Visible="true" VisibleIndex="1" Caption="Class Name" Settings-AutoFilterCondition="Contains">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="ProductClass_ID" Visible="true" VisibleIndex="2" Caption="Class ID" Settings-AutoFilterCondition="Contains" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <div class="hide">
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll_Class" UseSubmitBehavior="False"/>
                                                    </div>
                                                    <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll_Class" UseSubmitBehavior="False"/>                                                
                                                    <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup_Class" UseSubmitBehavior="False" />
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
                    </td>--%>


                    <%--<td style="padding-left: 15px">
                        <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label2" runat="Server" Text="Brand : " CssClass="mylabel1"
                                Width="110px"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 205px">
                        <asp:HiddenField ID="hflookupBrandAllFlag" runat="server" Value="" />
                        <dxe:ASPxGridLookup ID="lookupBrand" ClientInstanceName="clookupBrand" SelectionMode="Multiple" runat="server"
                            OnDataBinding="lookupBrand_DataBinding" KeyFieldName="Brand_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                            <Columns>
                                <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                <dxe:GridViewDataColumn FieldName="Brand_Name" Visible="true" VisibleIndex="1" Caption="Brand Name" Settings-AutoFilterCondition="Contains">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                                <dxe:GridViewDataColumn FieldName="Brand_Id" Visible="true" VisibleIndex="1" Caption="Brand ID" Settings-AutoFilterCondition="Contains" Width="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataColumn>
                            </Columns>
                            <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <div class="hide">
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll_Brand" UseSubmitBehavior="False"/>
                                                    </div>
                                                    <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll_Brand" UseSubmitBehavior="False"/>                                                
                                                    <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup_Brand" UseSubmitBehavior="False" />
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
                    </td>--%>

                     <td style="padding-left: 1px; width: 100px">
                        <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                            <asp:Label ID="Label1" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                        </div>
                    </td>
                    <td>
                        
                    <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                
                    </td>

                     <td style="padding-left: 15px; width: 100px">
                        <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                            <asp:Label ID="Label2" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label>
                        </div>
                    </td>
                    <td>
                       <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                    </td>

                    <td style="padding-left: 15px">


                        <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label8" runat="Server" Text="Transaction Type: " CssClass="mylabel1" Width="150px"></asp:Label>
                        </div>

                    </td>

                    <td style="width: 114px" class="simple-select">
                        <asp:DropDownList ID="ddlisdocument" runat="server" Width="100%">
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Sales" Value="Sales"></asp:ListItem>
                            <asp:ListItem Text="Purchases" Value="Purchases"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                </tr>

                <tr>
                    <td style="padding-top: 15px;">
                        <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="lblToDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                                Width="92px"></asp:Label>
                        </div>
                    </td>
                    <td class="for-cust-icon" style="padding-top: 15px;">
                        <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="205px" ClientInstanceName="cxdeFromDate">
                            <ButtonStyle Width="13px"></ButtonStyle>
                        </dxe:ASPxDateEdit>
                        <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                    </td>
                    <td style="padding-left: 15px; padding-top: 15px;">
                        <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label4" runat="Server" Text="To Date : " CssClass="mylabel1"
                                Width="92px"></asp:Label>
                        </div>
                    </td>
                    <td class="for-cust-icon" style="padding-top: 15px;">
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="205px" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px"></ButtonStyle>
                        </dxe:ASPxDateEdit>
                        <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                    </td>
                    <td style="padding-left: 15px; padding-top: 15px;" colspan="3">
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>

                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div class="pull-right">
            </div>
            <table class="TableMain100">
                <tr>
                    <td>
                        <div onkeypress="OnWaitingGridKeyPress(event)">
                            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="true" KeyboardSupport="true"
                                OnDataBound="Showgrid_DataBound" KeyFieldName="SLNO" DataSourceID="GenerateEntityServerModeDataSourceLevel1">
                                <%--OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid2_DataBinding" KeyFieldName="sProducts_ID" --%>
                                <%--<columns>
                                    <dxe:GridViewDataTextColumn FieldName="SL" Caption="Sl No" Width="10%" VisibleIndex="2" />
                                    <dxe:GridViewDataTextColumn FieldName="Brand_Name" Caption="Category" Width="50%" VisibleIndex="3" />
                                    <dxe:GridViewDataTextColumn FieldName="ProductClass_Description" Caption="Class" Width="50%" VisibleIndex="2" />
                                    <dxe:GridViewDataTextColumn FieldName="sProducts_Description" Caption="Particulars" Width="50%" VisibleIndex="3" />
                                    <dxe:GridViewDataTextColumn FieldName="IN_QTY_OP" Caption="Opening" Width="25%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" />
                                    <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Received" Width="20%" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" />
                                    <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Issue" Width="25%" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" />
                                    <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Closing" Width="25%" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" />
                                    <dxe:GridViewDataTextColumn FieldName="UNIT" Caption="Units" Width="25%" VisibleIndex="8" />
                                    <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Caption="Product ID" Width="0%" visible="false">
                                    </dxe:GridViewDataTextColumn>
                                </columns>--%>
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
                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <TotalSummary>
                                    <%--<dxe:ASPxSummaryItem FieldName="Over 120 Days" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="Within 120 Days" SummaryType="Sum" />
                                    <dxe:ASPxSummaryItem FieldName="Total" SummaryType="Sum" />--%>
                                </TotalSummary>
                                <ClientSideEvents EndCallback="Callback2_EndCallback" />
                            </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSourceLevel1" runat="server" OnSelecting="GenerateEntityServerModeDataSourceLevel1_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="COMBINDSTOCKTRIALLEVEL1_REPORT" ></dx:LinqServerModeDataSource>
                        </div>
                    </td>
                </tr>
            </table>
         </div>
     </div>

    <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup2ndLevel"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Combined Stock Trial 2nd Level" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closePopup(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <input id="hfProductID" type="hidden" />
                <div class="col-md-12">
                    <div class="row clearfix">
                        <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                            <tr>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label5" runat="Server" Text="Product Code : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtProductCode2ndLevel" ClientInstanceName="ctxtProductCode2ndLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>
                            <tr>
                                <td style="padding-top: 10px">
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label3" runat="Server" Text="Product Name : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td style="padding-top: 10px">
                                    <dxe:ASPxTextBox ID="txtProductDesc2ndLevel" ClientInstanceName="ctxtProductDesc2ndLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>

                            <tr>
                                <td></td>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblFromDate2ndLevel" runat="Server" CssClass="mylabel1"></asp:Label>
                                    </label>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="lblToDate2ndLevel" runat="Server" CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>

                            </tr>
                        </table>
                        <div class="pull-right" style="padding-top: 26px;">
                            <span style="padding-right: 10px; display: inline-block">Press Esc to Close</span>
                            <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddldetails_SelectedIndexChanged">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLSX</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div onkeypress="OnWaitingGridKeyPress2(event)">
                    <dxe:ASPxGridView runat="server" ID="ShowGridDetails2Level" ClientInstanceName="cShowGridDetails2Level" Width="100%" EnableRowsCache="false" AutoGenerateColumns="true"
                        OnDataBound="ShowGridDetails2Level_DataBound" KeyboardSupport="true" KeyFieldName="SL" DataSourceID="GenerateEntityServerModeDataSourceLevel2"
                        OnSummaryDisplayText="ShowGridDetails2Level_SummaryDisplayText">
                        <%--OnCustomCallback="ShowGridDetails2Level_CustomCallback" OnDataBinding="ShowGridDetails2Level_DataBinding" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto"--%>
                        <%-- <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" Width="19%" VisibleIndex="0" >
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="50%" VisibleIndex="1">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Opening" Caption="Opening" Width="9%" VisibleIndex="2">
                            
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Received" Caption="Received" Width="9%" VisibleIndex="3">
                            
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Issue" Caption="Issue" Width="9%" VisibleIndex="4">
                        
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Closing" Caption="Closing" Width="9%" VisibleIndex="5">
                               
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Caption="Product ID"   Visible="false">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn FieldName="branch_id" Caption="Branch ID"   Visible="false">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="sProducts_Code" Caption="Code"  Visible="false">
                            </dxe:GridViewDataTextColumn>

                        </Columns>--%>
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
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                         <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="Opening" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Received" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Issue" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Sale_Purchase" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="SPReturn" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Closing" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                        </TotalSummary>
                        <ClientSideEvents EndCallback="EndShowGridDetails2Level" />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSourceLevel2" runat="server" OnSelecting="GenerateEntityServerModeDataSourceLevel2_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="COMBINDSTOCKTRIALLEVEL2_REPORT" ></dx:LinqServerModeDataSource>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>

        <dxe:ASPxPopupControl ID="popup3" runat="server" ClientInstanceName="cpopup3rdLevel"
            Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Combined Stock Trial 3rd Level" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ClientSideEvents Closing="function(s, e) {
	        closePopup3(s, e);}" />
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <input id="hfProductID2" type="hidden" />
                    <input id="hfBranchID3" type="hidden" />
                    <div class="col-md-12">
                        <div class="row clearfix">
                            <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                                <tr>
                                    <td>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="Label6" runat="Server" Text="Product Code : " CssClass="mylabel1"></asp:Label>
                                        </label>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtProductCode3rdLevel" ClientInstanceName="ctxtProductCode3rdLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="padding-top: 10px">
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="Label7" runat="Server" Text="Product Name : " CssClass="mylabel1"></asp:Label>
                                        </label>
                                    </td>
                                    <td style="padding-top: 10px">
                                        <dxe:ASPxTextBox ID="txtProductDesc3rdLevel" ClientInstanceName="ctxtProductDesc3rdLevel" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblFromDate3rdLevel" runat="Server" CssClass="mylabel1"></asp:Label>
                                        </label>
                                        <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                            <asp:Label ID="lblToDate3rdLevel" runat="Server" CssClass="mylabel1"></asp:Label>
                                        </label>
                                    </td>

                                </tr>
                            </table>
                            <div class="pull-right" style="padding-top: 26px;">
                                <span style="padding-right: 10px; display: inline-block">Press Esc to Close</span>
                                <asp:DropDownList ID="ddlExport3" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddlExport3_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix" onkeypress="OnWaitingGridKeyPress3(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGridDetails3Level" ClientInstanceName="cShowGridDetails3Level" KeyFieldName="SEQ" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            KeyboardSupport="true" Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSourceLevel3" OnSummaryDisplayText="ShowGridDetails3Level_SummaryDisplayText">
                            <%--OnCustomCallback="ShowGridDetails3Level_CustomCallback" OnDataBinding="ShowGridDetails3Level_DataBinding"--%>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="Unit" Width="200" VisibleIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Document_Date" Caption="Date" Width="90" VisibleIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="400" VisibleIndex="2">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Trans_Type" Caption="Type" Width="200" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Document_No" Caption="Voucher No" Width="150" VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenDetailsDocuments('<%#Eval("Doc_ID") %>','<%#Eval("Trans_Type") %>')" class="pad">
                                            <%#Eval("Document_No")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Received" Caption="Received" Width="100" VisibleIndex="5">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Issue" Caption="Issue" Width="100" VisibleIndex="6">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" Width="100" VisibleIndex="7">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Value" Caption="Value" Width="100" VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closing" Caption="Closing" Width="100" VisibleIndex="9">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Reference" Caption="Reference" Width="200" VisibleIndex="10">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Doc_ID" Caption="Doc_ID" Width="100" VisibleIndex="11" Visible="false">
                                </dxe:GridViewDataTextColumn>

                                <%--<dxe:GridViewDataTextColumn FieldName="sProducts_ID" Caption="Reference" Width="19%" VisibleIndex="10">
                            </dxe:GridViewDataTextColumn>--%>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <TotalSummary>
                                <%-- <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="CLOSE_QTY" SummaryType="Sum" />--%>
                            </TotalSummary>
                            <ClientSideEvents EndCallback="EndShowGridDetails3Level" />
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSourceLevel3" runat="server" OnSelecting="GenerateEntityServerModeDataSourceLevel3_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="COMBINDSTOCKTRIALLEVEL3_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>

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

        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>

        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
            Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>

            <ClientSideEvents CloseUp="DocumentAfterHide" />
        </dxe:ASPxPopupControl>

        <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelL1" ClientInstanceName="cCallbackPanelL1" OnCallback="CallbackPanelL1_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsStockTrialL1Filter" runat="server" />
                </dxe:PanelContent>
            </PanelCollection>
              <ClientSideEvents EndCallback="CallbackPanelL1EndCall" />
        </dxe:ASPxCallbackPanel>

         <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelL2" ClientInstanceName="cCallbackPanelL2" OnCallback="CallbackPanelL2_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsStockTrialL2Filter" runat="server" />
                </dxe:PanelContent>
            </PanelCollection>
              <ClientSideEvents EndCallback="CallbackPanelL2EndCall" />
        </dxe:ASPxCallbackPanel>

         <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelL3" ClientInstanceName="cCallbackPanelL3" OnCallback="CallbackPanelL3_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                <asp:HiddenField ID="hfIsStockTrialL3Filter" runat="server" />
                </dxe:PanelContent>
            </PanelCollection>
              <ClientSideEvents EndCallback="CallbackPanelL3EndCall" />
        </dxe:ASPxCallbackPanel>
        <%--<dxe:ASPxGridViewExporter ID="exporterDetails3rdLevel" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>--%>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton" Modal="True" >     
    </dxe:ASPxLoadingPanel>
</asp:Content>
