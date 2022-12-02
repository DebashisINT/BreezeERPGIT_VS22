<%@ Page Title="Project Indent/Requisition" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PmsProjectIndent.aspx.cs" Inherits="ERP.OMS.Management.Activities.PmsProjectIndent" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/UOMConversion.ascx" TagPrefix="uc3" TagName="UOMConversionControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <script>
        $(function () {
            $('#UOMModal').on('hide.bs.modal', function () {

                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 6);
            });
        });

    </script>
    <%--Use for set focus on UOM after press ok on UOM--%>
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>
        var isFirstTime = true;
        function AllControlInitilize() {
            ///  document.getElementById('AddButton').style.display = 'inline-block';
            if (isFirstTime) {

                if (localStorage.getItem('PurIndentFromDate')) {
                    var fromdatearray = localStorage.getItem('PurIndentFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('PurIndentToDate')) {
                    var todatearray = localStorage.getItem('PurIndentToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('PurIndentBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PurIndentBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('PurIndentBranch'));
                    }

                }
                //updateGridByDate();

                isFirstTime = false;
            }
        }
        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {

                localStorage.setItem("PurIndentFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PurIndentToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("PurIndentBranch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                CgvPurchaseIndent.Refresh();

                $("#drdExport").val(0);
            }
        }

        //Rev Subhra 0019337 23-01-2019
        //function onPrintJv(id) {
        //    //window.location.href = "../../reports/XtraReports/Viewer/PurchaseIndentReportViewer.aspx?id=" + id;
        //    window.open("../../reports/XtraReports/Viewer/PurchaseIndentReportViewer.aspx?id=" + id, '_blank')
        //}

        var PIndentId = 0;
        function onPrintJv(id) {
            PIndentId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function cSelectPanelEndCall(s, e) {
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'PINDENT';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PIndentId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        //End of Rev Subhra 0019337 23-01-2019

        //This function is called to show the Status of All Sales Order Created By Login User Start
        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        // function above  End

        //This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }
        // function above  End


        // Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
            //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
            //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

        }
        function OnGetApprovedRowValues(obj) {
            uri = "PmsProjectIndent.aspx?key=" + obj + "&status=2";
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Approved

        // Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetRejectedQuoteId(s, e, itemIndex) {

            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "PmsProjectIndent.aspx?key=" + obj + "&status=3";
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "PmsProjectIndent.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }

            // function above  End 

            // To Hide the Popup and Refresh the Data in Pending Waiting Grid 

            $(document).ready(function () {
                $('#ApprovalCross').click(function () {

                    //window.popup.Hide();
                    //cgridPendingApproval.Refresh();
                    window.parent.popup.Hide();
                    window.parent.cgridPendingApproval.PerformCallback();
                    window.location.href = 'PmsProjectIndent.aspx'
                })
            });

            // Basic Setting for Approval in Edit Mode this page has the List and Detial Part so to call it
            function StartingSetupForApproval(indentid) {
                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
                $('#<%=hdn_Mode.ClientID %>').val('Edit'); //Edit
                //VisibleIndexE = e.visibleIndex;
                $('#<%= lblHeading.ClientID %>').text("Modify Project Indent/Requisition");
                document.getElementById("divfromTo").style.display = 'none';
                document.getElementById("divfromTo").style.display = 'none';
                document.getElementById('DivEntry').style.display = 'block';
                document.getElementById('DivEdit').style.display = 'none';
                document.getElementById('btnAddNew').style.display = 'none';
                //btncross.style.display = "block";
                InsgridBatch.PerformCallback("ApprovalEdit~" + indentid);
                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
            }

            // function above  End 

            //Added By Subhra For UOM 28/02/2019
            function QuantityProductsGotFocus(s, e) {
                var ProductID = InsgridBatch.GetEditor('gvColProduct').GetValue();

                var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
                var strProductName = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
                var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";

                var SpliteDetails = ProductID.split("||@||");
                var strProductID = SpliteDetails[0];
                var strDescription = SpliteDetails[1];
                var strUOM = SpliteDetails[2];
                var strUOMstk = SpliteDetails[4];


                strProductName = strDescription;

                var isOverideConvertion = SpliteDetails[12];
                var packing_saleUOM = SpliteDetails[11];
                var sProduct_SaleUom = SpliteDetails[10];
                var sProduct_quantity = SpliteDetails[9];
                var packing_quantity = SpliteDetails[8];

                var slno = (InsgridBatch.GetEditor('SrlNo').GetText() != null) ? InsgridBatch.GetEditor('SrlNo').GetText() : "0";

                //var Indent_Num = (grid.GetEditor('Indent_Num').GetText() != null) ? grid.GetEditor('Indent_Num').GetText() : "0";

                var ConvertionOverideVisible = $('#hdnConvertionOverideVisible').val();
                var ShowUOMConversionInEntry = $('#hdnShowUOMConversionInEntry').val();
                var type = 'add';
                var gridprodqty = parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetText()).toFixed(5);
                var gridPackingQty = '';
                var IsInventory = '';


                //var gridPackingQty = grid.GetEditor('QuoteDetails_PackingQty').GetText();
                //if (SpliteDetails.length == 14) {
                //    if (SpliteDetails[13] == "1") {
                //        IsInventory = 'Yes';
                //    }
                //}



                if (SpliteDetails.length > 13) {
                    if (SpliteDetails[13] == "1") {
                        IsInventory = 'Yes';
                        type = 'add';
                        var actionQry = '';
                        var IndentDetailsid = InsgridBatch.GetEditor('gvColIndentDetailsId').GetValue();
                        if (isTaggingUOM != "") {
                            actionQry = isTaggingUOM;
                        }
                        else {
                            actionQry = 'GetPurchaseIndentQty';
                        }

                        $.ajax({
                            type: "POST",
                            url: "Services/Master.asmx/GetMultiUOMDetails",
                            data: JSON.stringify({ orderid: IndentDetailsid, action: actionQry, module: 'PurchaseIndent', strKey: '' }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {

                                gridPackingQty = msg.d;
                                if (gridPackingQty != "" && gridPackingQty != null) {
                                    type = 'edit';
                                }

                                if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
                                    ShowUOM(type, "Purchase", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                                }

                            }
                        });
                    }
                }
                else {
                    if (ShowUOMConversionInEntry == "1" && SpliteDetails.length > 1) {
                        ShowUOM(type, "PurchaseIndent", 0, sProduct_quantity, sProduct_SaleUom, packing_quantity, packing_saleUOM, strProductID, slno, isOverideConvertion, gridprodqty, gridPackingQty);
                    }
                }
            }
            var issavePacking = 0;
            function SetDataToGrid(Quantity, packing, PackingUom, PackingSelectUom, productid, slno) {
                issavePacking = 1;
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
                InsgridBatch.GetEditor('gvColQuantity').SetValue(Quantity);
                <%--Use for set focus on UOM after press ok on UOM--%>

                setTimeout(function () {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 7);
                }, 400)
            <%--Use for set focus on UOM after press ok on UOM--%>
            }
        function SetFoucs() {

        }
        //End

    </script>

    <%-- Code Added By Sandip For Approval Detail Section End--%>
    <style>
        .voucherno {
            position: absolute;
            right: 0px;
            top: 34px;
        }

        .brnchreq label {
            font-size: 13px;
            font-weight: 300;
        }


        /*.dxgv {
            display: none;
        }*/

        .dxgv.dx-al, .dxgv.dx-ar, .dx-nowrap.dxgv, .gridcellleft.dxgv, .dxgv.dx-ac, .dxgvCommandColumn_PlasticBlue.dxgv.dx-ac {
            display: table-cell !important;
        }

        /*#gridBatch_DXMainTable tr td:first-child {
            display: table-cell !important;
        }*/

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        #gridBatch_DXStatus span > a {
            display: none;
        }

        #gridBatch_DXEditingErrorRow-1 {
            display: none;
        }

        /*#Grid_PurchaseIndent_DXMainTable .dxgv {
            display: table-cell !important;
        }*/

        #Grid_PurchaseIndent_DXFilterRow .dxgv {
            display: table-cell !important;
        }

        .padTabtype2 > tbody > tr > td {
            padding-right: 15px;
        }

        padTabtype2 > tbody > tr > td:last-child {
            padding-right: 0px;
        }
    </style>
    <script>
        var globalRowIndex;
        var chkAccount = 0;
        function OnMoreInfoClick(keyValue) {
            var URL = "PurchaseOrder.aspx?key=" + keyValue + "&req=V&status=PO&type=PO";
            capcPoDetails.SetContentUrl(URL);
            capcPoDetails.Show();

            document.getElementById('PODetailsCross').style.display = 'Block';
            document.getElementById('ApprovalCross').style.display = 'none';

        }
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var keyVal = document.getElementById('Keyval_internalId').value;
                var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=PI&&KeyVal_InternalID=' + keyVal;
                cUDFpopup.SetContentUrl(url);
                cUDFpopup.Show();
            }
            return true;
        }
        function acbpCrpUdfEndCall(s, e) {
            if (cacbpCrpUdf.cpUDFPI) {
                if (cacbpCrpUdf.cpUDFPI == "true") {
                    AddNewRow();
                    InsgridBatch.UpdateEdit();
                    cacbpCrpUdf.cpUDFPI = null;
                }
                else {
                    jAlert('UDF is set as Mandatory. Please enter values.', 'Alert', function () {
                        OpenUdf();
                    });
                    cacbpCrpUdf.cpUDFPI = null;
                }
            }
        }
        // End Udf Code
        function PageLoad() {
            FinYearCheckOnPageLoad();
        }
        function FinYearCheckOnPageLoad() {
            var SelectedDate = new Date(ctDate.GetDate());
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
                //                   alert('Between');
            }
            else {
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    ctDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    ctDate.SetDate(new Date(FinYearEndDate));
                }
            }
        }
        function TDateChange() {
            var SelectedDate = new Date(ctDate.GetDate());
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
                ctDate.SetDate(MaxLockDate);
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
            if (SelectedDateNumericValue >= FinYearStartDateNumericValue && SelectedDateNumericValue <= FinYearEndDatNumbericValue) {

            }
            else {
                jAlert('Enter Date Is Outside Of Financial Year !!');
                if (SelectedDateNumericValue < FinYearStartDateNumericValue) {
                    ctDate.SetDate(new Date(FinYearStartDate));
                }
                if (SelectedDateNumericValue > FinYearEndDatNumbericValue) {
                    ctDate.SetDate(new Date(FinYearEndDate));
                }
            }
            ///End OF Date Should Between Current Fin Year StartDate and EndDate
        }
        function BtnVisible() {
            document.getElementById('btnSaveExit').style.display = 'none'
            document.getElementById('btnnew').style.display = 'none'
            document.getElementById('tagged').style.display = 'block'

        }
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function InstrumentDateChange() {

            var ExpectedDeliveryDate = new Date(InsgridBatch.GetEditor('ExpectedDeliveryDate').GetValue());
            var requisitionDate = new Date(ctDate.GetValue());
            if (ExpectedDeliveryDate.format('yyyy-MM-dd') != requisitionDate.format('yyyy-MM-dd'))
                if (ExpectedDeliveryDate < requisitionDate) {
                    jAlert('Expected Delivery date must be same or later to Requisition Date. Cannot Proceed.', 'Alert', function () {

                        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 9);
                        InsgridBatch.GetEditor('ExpectedDeliveryDate').SetValue(null);
                    });

                }
        }
        //...................Shortcut keys.................
        var isCtrl = false;
        document.onkeydown = function (e) {
            if (event.keyCode == 78 && event.altKey == true) {
                //run code for Ctrl+S -- ie, save!       

            }
            else if (event.keyCode == 88 && event.altKey == true) {
                //run code for Ctrl+X -- ie, Save & Exit! 
                document.getElementById('btnSaveExit').click();
                return false;
            }
            else if (event.keyCode == 73 && event.altKey == true) {
                //(event.keyCode == 120 || )
                //run code for Ctrl+A -- ie, Add New
                if (document.getElementById('DivEntry').style.display != 'block') {
                    AddButtonClick();
                }
            }
            else if (event.keyCode == 85 && event.altKey == true) {
                OpenUdf();
            }

        }
        //...................end............................
        function ShowMsgLastCall() {
            if (CgvPurchaseIndent.cpDelete != null) {
                jAlert(CgvPurchaseIndent.cpDelete)
                //CgvPurchaseIndent.PerformCallback();
                CgvPurchaseIndent.Refresh();
                CgvPurchaseIndent.cpDelete = null
            }
        }
        function viewOnly() {

            if ($('#<%=hdn_Mode.ClientID %>').val().toUpperCase() == 'VIEW') {
                $('#DivEntry').find('input, textarea, button, select').attr('disabled', 'disabled');

                InsgridBatch.SetEnabled(false);
                ctDate.SetEnabled(false);
                cbtnnew.SetVisible(false);
                cbtnSaveExit.SetVisible(false);
            }
        }
        //Rev Debashis
        function zoompmspurchaseindent(keyid, docno) {
            document.getElementById("divfromTo").style.display = 'none';
            $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
            $('#<%= lblHeading.ClientID %>').text("View Project Indent/Requisition");
            document.getElementById('DivEntry').style.display = 'block';
            document.getElementById('DivEdit').style.display = 'none';
            document.getElementById('btnAddNew').style.display = 'none';
            $('#<%=hdn_Mode.ClientID %>').val('View');
            InsgridBatch.PerformCallback("View~" + keyid);
            chkAccount = 1;
            document.getElementById('divNumberingScheme').style.display = 'none';
        }
        //End of Rev Debashis
        function CustomButtonClick(s, e) {
            if (e.buttonID == 'CustomBtnPO') {
                VisibleIndexE = e.visibleIndex;
                CgvPurchaseIndent.GetRowKey(VisibleIndexE);
                capcPurchaseOrderList.Show();
                cgridPOLIst.PerformCallback("BindPOLIst~" + CgvPurchaseIndent.GetRowKey(VisibleIndexE));
            }
            if (e.buttonID == 'CustomBtnEdit') {
                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit
                $('#<%=hdn_Mode.ClientID %>').val('Edit'); //Edit
                VisibleIndexE = e.visibleIndex;
                //Sandip Section For Approval Section Detail Start
                var key = s.GetRowKey(e.visibleIndex);
                $('#<%=hdngridkeyval.ClientID %>').val(key);
                //Sandip Section For Approval Section Detail End
                $('#<%= lblHeading.ClientID %>').text("Modify Project Indent/Requisition");
                document.getElementById("divfromTo").style.display = 'none';
                document.getElementById('DivEntry').style.display = 'block';
                document.getElementById('DivEdit').style.display = 'none';
                document.getElementById('btnAddNew').style.display = 'none';
                btncross.style.display = "block";
                InsgridBatch.PerformCallback("Edit~" + VisibleIndexE);
                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
                // clookup_Project.gridView.Refresh();
                //    clookup_Project.gridView.Refresh();
            }
            if (e.buttonID == 'CustomBtnView') {
                $('#<%=hdnEditClick.ClientID %>').val('T'); //Edit

                VisibleIndexE = e.visibleIndex;
                document.getElementById("divfromTo").style.display = 'none';
                $('#<%= lblHeading.ClientID %>').text("View Project Indent/Requisition");
                document.getElementById('DivEntry').style.display = 'block';
                //document.getElementById('btnAddNew').style.display = 'none';
                document.getElementById('DivEdit').style.display = 'none';
                document.getElementById('btnAddNew').style.display = 'none';

                btncross.style.display = "block";
                $('#<%=hdn_Mode.ClientID %>').val('View');
                InsgridBatch.PerformCallback("View~" + VisibleIndexE);
                //LoadingPanel.Show();
                chkAccount = 1;
                document.getElementById('divNumberingScheme').style.display = 'none';
                // clookup_Project.gridView.Refresh();
                //   clookup_Project.gridView.Refresh();
            }
            if (e.buttonID == 'CustomBtnDelete') {

                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        VisibleIndexE = e.visibleIndex;
                        CgvPurchaseIndent.PerformCallback("Delete~" + VisibleIndexE);

                    }
                    else {
                        return false;
                    }
                });
            }
            else if (e.buttonID == 'CustomBtnPrint') {
                //var keyValueindex = s.GetRowKey(e.visibleIndex);
                //onPrintJv(keyValueindex); 
                //Rev Subhra 0019337 23-01-2019
                //jAlert('The Document Design is under Development...');
                var keyValueindex = s.GetRowKey(e.visibleIndex);
                onPrintJv(keyValueindex);
                //End of Rev Subhra 0019337 23-01-2019
            }
            //Mantis Issue 25065
            else if (e.buttonID == 'CustomBtnAttachment') {
                var keyValueindex = s.GetRowKey(e.visibleIndex);
                var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + keyValueindex + '&type=ProjectIndentRequisition';
                window.location.href = URL;
            }
            //End of Mantis Issue 25065
        }

        function deleteAllRows() {
            var frontRow = 0;
            var backRow = -1;
            for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() + 100 ; i++) {
                InsgridBatch.DeleteRow(frontRow);
                InsgridBatch.DeleteRow(backRow);
                backRow--;
                frontRow++;
            }

        }
        function AutoCalValue(s, e) {
            var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
            var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
            InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);

        }
        function AutoCalValueBtRate(s, e) {
            var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
            var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
            InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);
        }
        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtVoucherNo").value;

            $.ajax({
                type: "POST",
                url: "PmsProjectIndent.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
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
        function AddNewRow() {
            InsgridBatch.AddNewRow();
            var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            var tbQuotation = InsgridBatch.GetEditor("SrlNo");
            tbQuotation.SetValue(noofvisiblerows);
        }

        function ProjectEndCallback(s, e) {
            debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'PmsProjectIndent.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }


        var TotDocuments;
        var Indent_TagFroms = "";
        function OnEndCallback(s, e) {
            if (InsgridBatch.cpAddNewRow != null && InsgridBatch.cpAddNewRow != "") {
                InsgridBatch.cpAddNewRow = null;
                AddNewRow();
            }
            if (InsgridBatch.cpBtnVisible != null && InsgridBatch.cpBtnVisible != "") {
                InsgridBatch.cpBtnVisible = null;
                BtnVisible();
            }
            if (InsgridBatch.cpEdit != null) {

                var Indent_RequisitionNumber = InsgridBatch.cpEdit.split('~')[0];
                var Indent_RequisitionDate = InsgridBatch.cpEdit.split('~')[1];
                var Indent_BranchIdFor = InsgridBatch.cpEdit.split('~')[2];
                var Indent_Purpose = InsgridBatch.cpEdit.split('~')[3];
                var Indent_CurrencyId = InsgridBatch.cpEdit.split('~')[4];
                var Indent_ExchangeRtae = InsgridBatch.cpEdit.split('~')[5];
                var Indent_ID = InsgridBatch.cpEdit.split('~')[6];
                var Indent_ProjID = InsgridBatch.cpEdit.split('~')[7];
                var Indent_TagFrom = InsgridBatch.cpEdit.split('~')[8];
                var Indent_TagID = InsgridBatch.cpEdit.split('~')[9];

                var Estimate_isLastEntry = InsgridBatch.cpEdit.split('~')[10];

                Indent_TagFroms = Indent_TagFrom;
                $('#txtVoucherNo').val(Indent_RequisitionNumber);
                document.getElementById('Keyval_internalId').value = "PurchaseIndent" + Indent_ID;

                var Transdt = new Date(Indent_RequisitionDate);
                ctDate.SetDate(Transdt);

                $("#txtVoucherNo").attr("disabled", "disabled");
                $("#ddlBranch").attr("disabled", "disabled");
                $("#ddlHierarchy").attr("disabled", "disabled");
                document.getElementById('hdnEditIndentID').value = Indent_ID;
                ctxtMemoPurpose.SetValue(Indent_Purpose);
                cCmbCurrency.SetValue(Indent_CurrencyId);
                document.getElementById('ddlBranch').value = Indent_BranchIdFor;
                ctxtRate.SetValue(Indent_ExchangeRtae);
                //clookup_Project.gridView.Refresh();


                if (Indent_TagFrom != "") {
                    $('input:radio[name="ctl00$ContentPlaceHolder1$<%=rdl_SaleInvoice.ClientID %>"][value="' + Indent_TagFrom + '"]').attr('checked', true);
                    //selectValue();

                    // $('#rdl_SaleInvoice').('disabled', 'disabled');

            }
            var TotDocument = Indent_TagID.split(',');

            TotDocuments = TotDocument;
            setTimeout(function () {
                //    var TotDocument = Indent_TagID.split(',');
                //    if (TotDocument.length > 0) {
                //        for (var i = 0; i < TotDocument.length; i++) {
                //            if (TotDocument[i] != "") {
                //                gridquotationLookup.gridView.SelectItemsByKey(TotDocument[i]);
                //                gridquotationLookup.gridView.SelectItemsByKey(TotDocument);
                //            }
                //        }
                //    }
                //    else {
                //        gridquotationLookup.gridView.SelectItemsByKey(Indent_TagID);
                //        gridquotationLookup.gridView.SelectItemsByKey(TotDocument);
                //    }

                //  clookup_Project.gridView.Refresh();
                if ($("#hdnProjectSelectInEntryModule").val() == "1")
                    clookup_Project.gridView.SelectItemsByKey(Indent_ProjID);

                if (TotDocuments != null && TotDocuments != "") {
                    clookup_Project.SetEnabled(false);
                }

                gridquotationLookup.SetEnabled(false);
            }, 2000);


                // gridquotationLookup.gridView.SelectItemsByKey(TotDocument);

            selectValue();


            InsgridBatch.batchEditApi.StartEdit(-1, 1);
            if ($('#<%=hdnEditClick.ClientID %>').val() == 'T') {
                InsgridBatch.AddNewRow();
                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                $('#<%=hdnEditClick.ClientID %>').val("");
                }
                //Approval Detail Start
                if (InsgridBatch.cpApproval != null) {
                    if (InsgridBatch.cpApproval == 'A') {
                        $('#lbl_quotestatusmsg').css('display', 'inline-block');
                        $('#lbl_quotestatusmsg').text('Document already approved.');
                        $('#btnnew').css('display', 'none');
                        $('#btnSaveExit').css('display', 'none');
                    }
                    else if (InsgridBatch.cpApproval == 'R') {
                        $('#lbl_quotestatusmsg').css('display', 'inline-block');
                        $('#lbl_quotestatusmsg').text('Document already rejected.');
                        $('#btnnew').css('display', 'none');
                        $('#btnSaveExit').css('display', 'none');
                    }
                    else {
                        $('#lbl_quotestatusmsg').css('display', 'none');
                        $('#btnnew').css('display', 'block');
                        $('#btnSaveExit').css('display', 'block');
                    }
                }
                //Approval Detail End



                // 

                if (Estimate_isLastEntry=="False") {
                    cbtnSaveExit.SetVisible(false);
                    document.getElementById('Estimatetagged').style.display = 'block'
                }
            }
            if (InsgridBatch.cpSaveSuccessOrFail == "nullQuantity") {
                AddNewRow();
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Cannot save. Entered quantity must be greater then ZERO(0).');
                cLoadingPanelCRP.Hide();
            }
                //if (InsgridBatch.cpSaveSuccessOrFail == "ExceedQuantity") {
                //    AddNewRow();
                //    InsgridBatch.cpSaveSuccessOrFail = null;
                //    jAlert('Quantity can not be less than tagged quantity.');
                //    cLoadingPanelCRP.Hide();
                //}
                //else if (InsgridBatch.cpSaveSuccessOrFail == "duplicateProduct") {
                //    AddNewRow();
                //    InsgridBatch.cpSaveSuccessOrFail = null;
                //    jAlert('Can not Add Duplicate Product in the Purchase Indent.');
                //    InsgridBatch.cpSaveSuccessOrFail = '';
                //    cLoadingPanelCRP.Hide();
                //}
            else if (InsgridBatch.cpSaveSuccessOrFail == "outrange") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Can Not Add More Purchase Indent Number as Purchase Indent Scheme Exausted.<br />Update The Scheme and Try Again');
                cLoadingPanelCRP.Hide();
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "duplicate") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Can Not Save as Duplicate Purchase Indent Number.');
                cLoadingPanelCRP.Hide();
            }

            else if (InsgridBatch.cpSaveSuccessOrFail == "ExceedQuantity") {
                cLoadingPanelCRP.Hide();
                //InsgridBatch.batchEditApi.StartEdit(0, 2);
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Tagged product quantity exceeded.Update The quantity and Try Again.');

            }


            else if (InsgridBatch.cpSaveSuccessOrFail == "errorInsert") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Please try after sometime.');
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "ProjectError") {
                InsgridBatch.batchEditApi.StartEdit(0, 2);
                jAlert('Please select project.');
            }
            else if (InsgridBatch.cpSaveSuccessOrFail == "transactionbeingused") {
                InsgridBatch.cpSaveSuccessOrFail = null;
                jAlert('Transaction exist. cannot be processed.');
                cLoadingPanelCRP.Hide();
            }
            else {
                if (InsgridBatch.cpVouvherNo != null) {
                    var JV_Number = InsgridBatch.cpVouvherNo;
                    var value = document.getElementById('hdnRefreshType').value;
                    var JV_Msg = "Purchase Indent Requisition No. " + JV_Number + " generated.";
                    var strSchemaType = document.getElementById('hdnSchemaType').value;

                    if (value == "E") {
                        if (JV_Number != "") {
                            if (strSchemaType == '1') {
                                jAlert(JV_Msg, 'Alert Dialog: [PurchaseIndent]', function (r) {
                                    if (r == true) {
                                        InsgridBatch.cpVouvherNo = null;
                                        window.location.assign("PmsProjectIndent.aspx");
                                    }
                                });
                            }
                            else {
                                window.location.assign("PmsProjectIndent.aspx");
                            }
                        }
                        else {
                            window.location.assign("PmsProjectIndent.aspx");
                        }
                    }
                    else if (value == "S") {
                        if (JV_Number != "") {
                            if (strSchemaType == '1') {
                                jAlert(JV_Msg, 'Alert Dialog: [PurchaseIndent]', function (r) {
                                    if (r == true) {
                                        InsgridBatch.cpVouvherNo = null;
                                    }
                                });
                            }
                        }
                    }
                }
                if ($('#<%=hdnSaveNew.ClientID %>').val() == "Save_Exit") {

                    if (InsgridBatch.cpExitNew == "YES") {
                        <%--Code Added By Sandip For Approval Detail Section Start--%>
                        if (InsgridBatch.cpApproverStatus == "approve") {
                            window.parent.popup.Hide();
                            window.parent.cgridPendingApproval.PerformCallback();
                        }
                        <%--Code Above Added By Sandip For Approval Detail Section End--%>
                        deleteAllRows();
                    }

                }
                if ($('#<%=hdnSaveNew.ClientID %>').val() == "Save_New") {
                    ctxtMemoPurpose.SetValue("");
                    $("#divNumberingScheme").show();
                    var Campany_ID = '<%=Session["LastCompany"]%>';
                    var LocalCurrency = '<%=Session["LocalCurrency"]%>';
                    var basedCurrency = LocalCurrency.split("~");
                    cCmbCurrency.SetValue(basedCurrency[0]);
                    ctxtRate.SetValue("");
                    ctxtRate.SetEnabled(false);
                    $('#<%=lblHeading.ClientID %>').text("");
                    $('#<%=lblHeading.ClientID %>').text("Add Project Indent/Requisition");
                    deleteAllRows();
                    InsgridBatch.AddNewRow();
                    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                    tbQuotation.SetValue(noofvisiblerows);
                    $('#<%=hdn_Mode.ClientID %>').val('Entry');
                    if (document.getElementById('txtVoucherNo').value == "Auto") {
                        document.getElementById('txtVoucherNo').value = "Auto";
                    }
                    else {
                        document.getElementById('txtVoucherNo').value = "";
                        $('#txtVoucherNo').focus();
                    }

                    $('#txtMemoPurpose_I').focus();
                    cLoadingPanelCRP.Hide();
                }
            }
    if (InsgridBatch.cpView == "1") {
        viewOnly();
    }
}
function OnCustomButtonClick(s, e) {
    if (e.buttonID == 'CustomDelete') {
        if (InsgridBatch.GetVisibleRowsOnPage() > 1) {
            InsgridBatch.batchEditApi.EndEdit();
            InsgridBatch.DeleteRow(e.visibleIndex);
            $('#<%=hdfIsDelete.ClientID %>').val('D');
            InsgridBatch.UpdateEdit();
            InsgridBatch.PerformCallback('Display');
            InsgridBatch.batchEditApi.StartEdit(-1, 2);
            InsgridBatch.batchEditApi.StartEdit(0, 2);
        }
    }
    if (e.buttonID == 'CustomAddNewRow') {
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 2);
        var Product = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "";
        var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0.0";
        var componentType = gridquotationLookup.GetValue();
        if (componentType != null && componentType != '') {
            // cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
            //cgridproducts.PerformCallback('BindProductsDetails');
            //  if (Indent_TagFroms == "") {
            //   cProductsPopup.Show();
            // }
            // CloseGridQuotationLookup();
            QuotationNumberChanged();
        }
        else {
            if (Product != "" && Quantity != "0.0") {
                InsgridBatch.AddNewRow();
                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
                setTimeout(function () {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);
                }, 500);
                return false;
            }
        }
    }
    if (e.buttonID == "addlDesc") {
        var index = e.visibleIndex;
        InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 4);
        cPopup_InlineRemarks.Show();
        $("#txtInlineRemarks").val('');
        var SrlNo = (InsgridBatch.GetEditor('SrlNo').GetValue() != null) ? InsgridBatch.GetEditor('SrlNo').GetValue() : "";
        var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "";
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
}
//....Tab Index Change From Rate to Grid First Column......
$(document).ready(function () {
    $('#txtMemoPurpose_I').blur(function () {
        if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
            InsgridBatch.batchEditApi.StartEdit(-1, 2);
        }
    })
});
//.....end..........

function SetArrForUOM() {
    if (aarr.length == 0) {
        for (var i = -500; i < 500; i++) {
            if (InsgridBatch.GetRow(i) != null) {

                var ProductID = (InsgridBatch.batchEditApi.GetCellValue(i, 'gvColProduct') != null) ? InsgridBatch.batchEditApi.GetCellValue(i, 'gvColProduct') : "0";
                if (ProductID != "0") {
                    //var Indent_Num = (InsgridBatch.GetEditor('Indent_Num').GetText() != null) ? InsgridBatch.GetEditor('Indent_Num').GetText() : "";
                    var actionQry = 'GetPurchaseIndentQty';
                    //if ($("#hdAddOrEdit").val() == "Edit") {

                    var SpliteDetails = ProductID.split("||@||");
                    var strProductID = SpliteDetails[0];
                    var orderid = InsgridBatch.batchEditApi.GetCellValue(i, 'gvColIndentDetailsId');//InsgridBatch.GetRowKey(i);
                    var slnoget = InsgridBatch.batchEditApi.GetCellValue(i, 'SrlNo');
                    var Quantity = InsgridBatch.batchEditApi.GetCellValue(i, 'gvColQuantity');
                    $.ajax({
                        type: "POST",
                        url: "Services/Master.asmx/GetMultiUOMDetails",
                        data: JSON.stringify({ orderid: orderid, action: actionQry, module: 'PurchaseIndent', strKey: '' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {

                            gridPackingQty = msg.d;

                            if (msg.d != "") {
                                var packing = SpliteDetails[8];
                                var PackingUom = SpliteDetails[11];
                                var PackingSelectUom = SpliteDetails[10];
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
                    //}
                }
            }
        }

    }
}


function Save_ButtonClick() {
    cLoadingPanelCRP.Show();
    $('#<%=hdnSaveNew.ClientID %>').val("Save_New");
    $('#<%=hdfIsDelete.ClientID %>').val('I');
    $('#<%=hdnRefreshType.ClientID %>').val('S');
    if (document.getElementById('<%= txtVoucherNo.ClientID %>').value == "") {
        $("#MandatoryBillNo").show();
        cLoadingPanelCRP.Hide();
        return false;
    }

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        cLoadingPanelCRP.Hide();
        jAlert("Please Select Project.");
        return false;
    }

    var IsType = "";
    var frontRow = 0;
    var backRow = -1;
    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = "";
        var backProduct = "";
        frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    SetArrForUOM(); //Surojit For UOM EDIT

    //Rev Subhra 01-03-2019
    if (issavePacking == 1) {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PmsProjectIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                }
            });
        }
    }
    else {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PmsProjectIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                }
            });
        }
    }
    //End of Rev
    if (InsgridBatch.GetVisibleRowsOnPage() > 0) {
        if (IsType == "Y") {
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            cLoadingPanelCRP.Hide();
        }
    }
    else {
        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        cLoadingPanelCRP.Hide();
    }
    chkAccount = 0;
    return false;
}

function SaveExitButtonClick() {
    debugger;
    cLoadingPanelCRP.Show();
    $('#<%=hdnSaveNew.ClientID %>').val("Save_Exit");
    var a = $('#<%=hdnEditIndentID.ClientID %>').val();
    $('#<%=hdnRefreshType.ClientID %>').val('E');
    $('#<%=hdfIsDelete.ClientID %>').val('I');
    if (document.getElementById('<%= txtVoucherNo.ClientID %>').value == "") {
        $("#MandatoryBillNo").show();
        cLoadingPanelCRP.Hide();
        return false;
    }

    var ProjectCode = clookup_Project.GetText();
    if ($("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
        cLoadingPanelCRP.Hide();
        jAlert("Please Select Project.");
        return false;
    }


    var IsType = "";
    var frontRow = 0;
    var backRow = -1;

    for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
        var frontProduct = "";
        var backProduct = "";
        frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
        backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";

        if (frontProduct != "" || backProduct != "") {
            IsType = "Y";
            break;
        }
        backRow--;
        frontRow++;
    }
    SetArrForUOM(); //Surojit For UOM EDIT

    //Rev Subhra 01-03-2019
    if (issavePacking == 1) {
        if (aarr.length > 0) {
            $.ajax({
                type: "POST",
                url: "PmsProjectIndent.aspx/SetSessionPacking",
                data: "{'list':'" + JSON.stringify(aarr) + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                }
            });
        }
    }
    //End of Rev
    if (InsgridBatch.GetVisibleRowsOnPage() > 0) {
        if (IsType == "Y") {
            cacbpCrpUdf.PerformCallback();
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            cLoadingPanelCRP.Hide();
        }
    }
    else {
        jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
        cLoadingPanelCRP.Hide();
    }
    //cbtnSaveExit.SetEnabled(false);
    chkAccount = 0;
    cLoadingPanelCRP.Hide();
    return false;
}

function AddBatchNew(s, e) {
    InsgridBatch.batchEditApi.EndEdit();
    var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
    if (keyCode === 13) {
        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var i;
        var cnt = 2;

        InsgridBatch.AddNewRow();
        if (noofvisiblerows == "0") {
            InsgridBatch.AddNewRow();
        }
        InsgridBatch.SetFocusedRowIndex();

        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            cnt++;
        }

        var tbQuotation = grid.GetEditor("SrlNo");
        tbQuotation.SetValue(cnt);
    }
}

function ProductsComboGotFocusChange(s, e) {
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");
    var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
    var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";

    // var ProductID = s.GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strRate = SpliteDetails[6];
    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tdRate.SetValue(strRate);
    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LastFinYear = '<%=Session["LastFinYear"]%>';
    var BranchFor = $("#ddlBranch").val();
    if (ProductID != "0" && ProductID != "") {
        $.ajax({
            type: "POST",
            url: 'BranchRequisition.aspx/getAvilableStock',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
            success: function (msg) {
                var data = msg.d;
                document.getElementById("pageheaderContent").style.display = 'block';
                // document.getElementById("B_AvailableStock").Value = data;
                var AvailableStock = data + " " + strUOM;
                $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);

            }
        });
    }
}
function ProductsCombo_SelectedIndexChanged(s, e) {
    var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
    var tbUOM = InsgridBatch.GetEditor("gvColUOM");
    var tdRate = InsgridBatch.GetEditor("gvColRate");

    var ProductID = s.GetValue();
    var SpliteDetails = ProductID.split("||@||");
    var strProductID = SpliteDetails[0];
    var strDescription = SpliteDetails[1];
    var strUOM = SpliteDetails[2];
    var strRate = SpliteDetails[6];
    chkAccount = 1;
    tbDescription.SetValue(strDescription);
    tbUOM.SetValue(strUOM);
    tdRate.SetValue(strRate);
    var Campany_ID = '<%=Session["LastCompany"]%>';
    var LastFinYear = '<%=Session["LastFinYear"]%>';
    var BranchFor = $("#ddlBranch").val();
    $.ajax({
        type: "POST",
        url: 'PmsProjectIndent.aspx/getAvilableStock',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ Campany_ID: Campany_ID, ProductID: strProductID, LastFinYear: LastFinYear, BranchFor: BranchFor }),
        success: function (msg) {
            var data = msg.d;
            document.getElementById("pageheaderContent").style.display = 'block';
            var AvailableStock = data + " " + strUOM;
            // document.getElementById("B_AvailableStock").Value = data;
            $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);

                }
            });

        }
        function AddButtonClick() {
            $('#<%=hdn_Mode.ClientID %>').val('Entry'); //Entry
    $('#<%=Keyval_internalId.ClientID %>').val('Add');
    cCmbScheme.SetValue("0");
    ctxtRate.SetEnabled(false);
    document.getElementById('DivEntry').style.display = 'block';
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';
    document.getElementById("divfromTo").style.display = 'none';
    btncross.style.display = "block";
    //btnheadlist.style.display = "none";
    //document.getElementById('drdCashBank').style.display = 'none';
    document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
    document.getElementById('<%= txtRate.ClientID %>').disabled = true;
    document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
    $('#<%=lblHeading.ClientID %>').text("");
    $('#<%=lblHeading.ClientID %>').text("Add Project Indent/Requisition");
    deleteAllRows();
    InsgridBatch.AddNewRow();
    var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
    var tbQuotation = InsgridBatch.GetEditor("SrlNo");
    tbQuotation.SetValue(noofvisiblerows);
    cCmbScheme.Focus();

}
function CmbScheme_ValueChange() {
    var val = cCmbScheme.GetValue();
    $.ajax({
        type: "POST",
        url: 'PmsProjectIndent.aspx/getSchemeType',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{sel_scheme_id:\"" + val + "\"}",
        success: function (type) {
            var schemetypeValue = type.d;
            var schemetype = schemetypeValue.toString().split('~')[0];
            var schemelength = schemetypeValue.toString().split('~')[1];
            $('#txtVoucherNo').attr('maxLength', schemelength);
            var branchID = schemetypeValue.toString().split('~')[2];

            var fromdate = schemetypeValue.toString().split('~')[3];
            var todate = schemetypeValue.toString().split('~')[4];

            var dt = new Date();

            ctDate.SetDate(dt);

            if (dt < new Date(fromdate)) {
                ctDate.SetDate(new Date(fromdate));
            }

            if (dt > new Date(todate)) {
                ctDate.SetDate(new Date(todate));
            }




            ctDate.SetMinDate(new Date(fromdate));
            ctDate.SetMaxDate(new Date(todate));


            document.getElementById('ddlBranch').value = branchID;
            document.getElementById('<%= ddlBranch.ClientID %>').disabled = true;
            if (schemetype == '0') {
                $('#<%=hdnSchemaType.ClientID %>').val('0');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = false;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
                $('#<%=txtVoucherNo.ClientID %>').focus();
            }
            else if (schemetype == '1') {
                $('#<%=hdnSchemaType.ClientID %>').val('1');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Auto";
                $("#MandatoryBillNo").hide();
                ctDate.Focus();
            }
            else if (schemetype == '2') {
                $('#<%=hdnSchemaType.ClientID %>').val('2');
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "Datewise";
            }
            else if (schemetype == 'n') {
                document.getElementById('<%= txtVoucherNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtVoucherNo.ClientID %>').value = "";
            }
        }
    });

    //clookup_Project.gridView.Refresh();
    clookup_Project.gridView.Refresh();
}
function Currency_Rate() {

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
            url: "PmsProjectIndent.aspx/GetRate",
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

    </script>
    <style>
        /*.absolute, #gridBatch_DXMainTable .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        #gridBatch_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }*/

        #rdl_SaleInvoice {
            margin-top: 3px;
        }

            #rdl_SaleInvoice > tbody > tr > td {
                padding-right: 5px;
            }
    </style>

    <%--Batch Product Popup Start--%>

    <script>
        var preColumn = '';
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function ProductsGotFocus(s, e) {
            document.getElementById("pageheaderContent").style.display = 'block';
            //document.getElementById("liToBranch").style.display = 'block';
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var tdRate = InsgridBatch.GetEditor("gvColRate");
            var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
            var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strUOMstk = SpliteDetails[4];
            chkAccount = 1;
            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);
            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }
        }

        function clookupProjectLostFocus(s, e) {
            if (InsgridBatch.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }

            var projID = clookup_Project.GetValue();
            if (projID == null && projID == "") {
                $("#ddlHierarchy").val(0);
            }
        }

        function ProductsGotFocusFromID(s, e) {
            pageheaderContent.style.display = "block";

            var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
            var strProductName = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
            var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var tdRate = InsgridBatch.GetEditor("gvColRate");
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strUOMstk = SpliteDetails[4];

            var strRate = SpliteDetails[6];
            chkAccount = 1;
            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);

            if (ProductID != "0") {
                cacpAvailableStock.PerformCallback(strProductID);
            }
        }
        function acpAvailableStockEndCall(s, e) {
            if (cacpAvailableStock.cpstock != null) {
                var AvailableStock = cacpAvailableStock.cpstock + " " + document.getElementById('<%=lblStkUOM.ClientID %>').innerHTML;
                $('#<%=B_AvailableStock.ClientID %>').text(AvailableStock);
                cacpAvailableStock.cpstock = null;
            }
            if (preColumn == "Product") {
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
                preColumn = '';
                return;
            }
        }
        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
            //if (e.htmlEvent.key == "Tab") {
            //    s.OnButtonClick(0);
            //}
        }
        function prodkeydown(e) {

            var OtherDetails = {}
            if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtProdSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetAllProductDetailsIndentRequisition", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }
        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {
                setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
                $('#txtProdSearch').val('');
                $('#ProductModel').modal('show');
            }
            //if (e.buttonIndex == 0) {

            //    if (cproductLookUp.Clear()) {
            //        cProductpopUp.Show();
            //        cproductLookUp.Focus();
            //        cproductLookUp.ShowDropDown();
            //    }
            //}
        }
        function ValueSelected(e, indexName) {
            if (e.code == "Enter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex") {
                        SetProduct(Id, name);
                    }

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
                    if (indexName == "ProdIndex") {
                        $('#txtProdSearch').focus();
                    }

                }
            }
        }
        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }
        function SetProduct(Id, Name) {
            $('#ProductModel').modal('hide');
            var LookUpData = Id;
            var ProductCode = Name;
            if (!ProductCode) {
                LookUpData = null;
            }
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
            InsgridBatch.GetEditor("gvColProduct").SetText(LookUpData);
            InsgridBatch.GetEditor("ProductName").SetText(ProductCode);
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var tdRate = InsgridBatch.GetEditor("gvColRate");
            var ProductID = LookUpData;
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strRate = SpliteDetails[6];
            chkAccount = 1;
            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tdRate.SetValue(strRate);
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LastFinYear = '<%=Session["LastFinYear"]%>';
            var BranchFor = $("#ddlBranch").val();
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);
            preColumn = "Product";
            cacpAvailableStock.PerformCallback(strProductID);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 4);
        }
        <%-- function ProductSelected(s, e) {
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }
            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            cProductpopUp.Hide();
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
            InsgridBatch.GetEditor("gvColProduct").SetText(LookUpData);
            InsgridBatch.GetEditor("ProductName").SetText(ProductCode);
            var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
            var tbUOM = InsgridBatch.GetEditor("gvColUOM");
            var tdRate = InsgridBatch.GetEditor("gvColRate");            
            var ProductID = LookUpData;
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];          
            var strRate = SpliteDetails[6];
            chkAccount = 1;
            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tdRate.SetValue(strRate);
            var Campany_ID = '<%=Session["LastCompany"]%>';
            var LastFinYear = '<%=Session["LastFinYear"]%>';
            var BranchFor = $("#ddlBranch").val();
            $('#<%= lblStkQty.ClientID %>').text("0.00");
            $('#<%= lblStkUOM.ClientID %>').text(strUOM);            
            preColumn = "Product";
            cacpAvailableStock.PerformCallback(strProductID);
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);           
        }--%>
    </script>

    <%--Batch Product Popup End--%>

    <%-- Project Script start --%>
    <script>
        function ProjectListKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function ProjectListButnClick(s, e) {
            //ctaggingGrid.PerformCallback('BindComponentGrid');
            clookup_Project.ShowDropDown();
        }

        function ddlBranch_SelectedIndexChanged() {
            //clookup_Project.gridView.Refresh();
            clookup_Project.gridView.Refresh();
        }

        function CmbScheme_LostFocus() {
            if ($("#hdnProjectSelectInEntryModule").val() == "1")
                clookup_Project.gridView.Refresh();
        }
    </script>
    <style>
        #gridSkillMasterList, #gridSkillMasterList > tbody > tr > td .dxgvCSD {
            width: 100% !important;
        }

        #gridBatch_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }
    </style>
    <script>
        $(document).ready(function () {
            //Toggle fullscreen expandEntryGrid
            $("#ddlHierarchy").attr("disabled", "disabled");
            $("#expandEntryGrid").click(function (e) {
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


                    InsgridBatch.SetHeight(browserHeight - 150);
                    InsgridBatch.SetWidth(cntWidth);
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

                    InsgridBatch.SetHeight(300);

                    var cntWidth = $this.parent('.makeFullscreen').width();
                    InsgridBatch.SetWidth(cntWidth);
                }

            });
            $("#expandCgvPurchaseIndent").click(function (e) {
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


                    CgvPurchaseIndent.SetHeight(browserHeight - 150);
                    CgvPurchaseIndent.SetWidth(cntWidth);
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


                    CgvPurchaseIndent.SetHeight(450);

                    var cntWidth = $this.parent('.makeFullscreen').width();
                    CgvPurchaseIndent.SetWidth(cntWidth);

                }

            });

        });
    </script>
    <%-- Project Script End --%>

    <script>
        function callback_InlineRemarks_EndCall(s, e) {
            if (ccallback_InlineRemarks.cpDisplayFocus == "DisplayRemarksFocus") {
                setTimeout(function () {
                    $("#txtInlineRemarks").focus();
                }, 500);
            }
            else {
                cPopup_InlineRemarks.Hide();
                //setTimeout(function () {
                //    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
                //}, 700);
            }
        }

        function FinalRemarks() {
            ccallback_InlineRemarks.PerformCallback('RemarksFinal' + '~' + InsgridBatch.GetEditor('SrlNo').GetValue() + '~' + $('#txtInlineRemarks').val());
            $("#txtInlineRemarks").val('');
            //setTimeout(function () {
            //    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            //}, 700);
        }

        function closeRemarks(s, e) {
            cPopup_InlineRemarks.Hide();
            setTimeout(function () {
                InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            }, 700);
            //e.cancel = false;
            //ccallback_InlineRemarks.PerformCallback('RemarksDelete'+'~'+InsgridBatch.GetEditor('SrlNo').GetValue()+'~'+$('#txtInlineRemarks').val());
            //cPopup_InlineRemarks.Hide();
            //e.cancel = false;
            // cPopup_InlineRemarks.Hide();
        }

        function PriceProductsGotFocus(s, e) {
        }

        function AmountProductsGotFocus(s, e) {
        }
    </script>

    <%--Tagging Start--%>

    <script>
        var SimilarProjectStatus = "0";
        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();

            var quotetag_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();

            //debugger;
            if (quotetag_Id.length > 0 && $("#hdnProjectSelectInEntryModule").val() == "1") {

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
                var Doctype = $("#rdl_SaleInvoice").find(":checked").val();
                //debugger;
                $.ajax({
                    type: "POST",
                    url: "PmsProjectIndent.aspx/DocWiseSimilarProjectCheck",
                    data: JSON.stringify({ quote_Id: quote_Id, Doctype: Doctype }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        SimilarProjectStatus = msg.d;
                        debugger;
                        if (SimilarProjectStatus != "1") {
                            ctxt_InvoiceDate.SetText("");
                            clookup_Project.Clear();
                            jAlert("Unable to procceed. Project are for the selected Document(s) are different.");

                            return false;

                        }
                    }
                });
            }

        }

        function LoadOldSelectedKeyvalue() {
            var x = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            var Ids = "";
            for (var i = 0; i < x.length; i++) {
                Ids = Ids + ',' + x[i];
            }
            document.getElementById('OldSelectedKeyvalue').value = Ids;
        }

        function BeginComponentCallback() {
        }

        var isTaggingUOM = "";

        function componentEndCallBack(s, e) {
            //gridquotationLookup.gridView.Refresh();
            if (InsgridBatch.GetVisibleRowsOnPage() == 0) {
                OnAddNewClick();
            }

            if (cQuotationComponentPanel.cpRebindGridQuote && cQuotationComponentPanel.cpRebindGridQuote != "") {
                ctxt_InvoiceDate.SetText(cQuotationComponentPanel.cpRebindGridQuote);
                cQuotationComponentPanel.cpRebindGridQuote = null;
            }


            if (cQuotationComponentPanel.cpDetails != null) {
                var details = cQuotationComponentPanel.cpDetails;
                cQuotationComponentPanel.cpDetails = null;


                var SpliteDetails = details.split("~");
                var Reference = SpliteDetails[0];
                var Currency_Id = SpliteDetails[1];
                var SalesmanId = SpliteDetails[2];
                var ExpiryDate = SpliteDetails[3];
                var CurrencyRate = SpliteDetails[4];
                var Type = SpliteDetails[5];
                var CreditDays = SpliteDetails[6];
                var DueDate = SpliteDetails[7];
                var SalesmanName = SpliteDetails[8];
                //if (Type == "BQ") {
                //    if (DueDate != "" && CreditDays != "") {
                //        ctxtCreditDays.SetValue(CreditDays);

                //        var Due_Date = new Date(DueDate);
                //        cdt_SaleInvoiceDue.SetDate(Due_Date);
                //    }
                //}

                //  ctxt_Refference.SetValue(Reference);
                // ctxt_Rate.SetValue(CurrencyRate);
                document.getElementById('ddl_Currency').value = Currency_Id;
                //document.getElementById('ddl_SalesAgent').value = SalesmanId;
                $("#<%=hdnSalesManAgentId.ClientID%>").val(SalesmanId);
                // ctxtSalesManAgent.SetValue(SalesmanName);
                if (ExpiryDate != "") {
                    var myDate = new Date(ExpiryDate);
                    var invoiceDate = new Date();
                    var datediff = Math.round((myDate - invoiceDate) / (1000 * 60 * 60 * 24));

                    //ctxtCreditDays.SetValue(datediff);
                    //cdt_SaleInvoiceDue.SetDate(myDate);
                }
            }

            //if (TotDocuments != null && TotDocuments != "") {
            //    gridquotationLookup.gridView.SelectItemsByKey(TotDocuments);
            //}

        }

        function selectValue() {
            var startDate = new Date();

            deleteAllRows();
            deleteAllRows();
            AddNewRow();

            startDate = ctDate.GetValueString();
            //  var key = "CL20000001";//GetObjectID('hdnCustomerId').value;
            var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

            if (type == "ES") {
                clbl_InvoiceNO.SetText('Estimate Date');
                isTaggingUOM = "GetPurchaseIndentEstimateQty";
            }
            else if (type == "BQ") {
                clbl_InvoiceNO.SetText('BOQ Date');
                isTaggingUOM = "GetPurchaseIndentBOQQty";
            }
            else if (type == "SC") {
                clbl_InvoiceNO.SetText('Sales Challan Date');
            }


            //  if (Indent_TagFroms == "") {

            cQuotationComponentPanel.PerformCallback('DateCheckOnChanged' + '~' + startDate + '~' + '@');

            cQuotationComponentPanel.PerformCallback('BindComponentGrid' + '~' + startDate + '~' + 'DateCheck' + '~' + type);// + '~' + inventory + '~' + isinventory
            //   }

            //Chinmoy added below line
            // cddl_PosGst.SetEnabled(false);

            var componentType = gridquotationLookup.GetValue();//gridquotationLookup.GetGridView().GetRowKey(gridquotationLookup.GetGridView().GetFocusedRowIndex());
            if (componentType != null && componentType != '') {
                //InsgridBatch.PerformCallback('GridBlank');
                deleteAllRows();

                AddNewRow();
                AddNewRow();
                //  cddl_PosGst.SetEnabled(true);
            }

            //clookup_Project.gridView.Reference();
            clookup_Project.gridView.SelectItemsByKey(0);
            clookup_Project.SetEnabled(true);
        }

        function OnAddNewClick() {
            if (gridquotationLookup.GetValue() == null) {
                InsgridBatch.AddNewRow();

                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);
            }
            else {
                QuotationNumberChanged();
                InsgridBatch.StartEditRow(0);
            }
        }


        function QuotationNumberChanged() {
            if (SimilarProjectStatus != "-1") {
                var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();//gridquotationLookup.GetValue();
                quote_Id = quote_Id.join();

                var type = ($("[id$='rdl_SaleInvoice']").find(":checked").val() != null) ? $("[id$='rdl_SaleInvoice']").find(":checked").val() : "";

                if (quote_Id != null) {
                    var arr = quote_Id.split(',');

                    if (arr.length > 1) {
                        if (type == "ES") {
                            ctxt_InvoiceDate.SetText('Multiple Select Estimate Dates');
                            isTaggingUOM = "GetPurchaseIndentEstimateQty";
                        }
                        else if (type == "BQ") {
                            ctxt_InvoiceDate.SetText('Multiple Select BOQ Dates');
                            isTaggingUOM = "GetPurchaseIndentBOQQty";
                        }
                        else if (type == "SC") {
                            ctxt_InvoiceDate.SetText('Multiple Select Challan Dates');
                        }
                    }
                    else {
                        if (arr.length == 1) {
                            cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id + '~' + type);
                        }
                        else {
                            ctxt_InvoiceDate.SetText('');
                        }
                    }
                }
                else { ctxt_InvoiceDate.SetText(''); }

                if (quote_Id != null) {
                    cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                    if ($("#hdn_Mode").val() == "Edit") {
                        cProductsPopup.Show();
                    }
                    else {
                        if (Indent_TagFroms == "") {
                            cProductsPopup.Show();
                        }
                    }
                }
            }
        }

        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }

        function PerformCallToProdGridBind() {

            InsgridBatch.PerformCallback('BindGridOnQuotation' + '~' + '@');
            cQuotationComponentPanel.PerformCallback('BindComponentGridOnSelection');
            $('#hdnPageStatus').val('Quoteupdate');
            cProductsPopup.Hide();


            AllowAddressShipToPartyState = false;
            //#### added by Samrat Roy for Transporter Control #############

            var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            if ("<%=Convert.ToString(Session["TransporterVisibilty"])%>" == "Yes") {
                //callTransporterControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            if (quote_Id.length > 0) {
                //Chinmoy added Below line
                // GetDocumentAddress(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
                //BSDocTagging(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            if (quote_Id.length > 0) {
                // BindOrderProjectdata(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            //#### end : Samrat Roy for Transporter Control :end #############

            //#### added by Sayan Dutta for TC Control #############
            if ($("#btn_TermsCondition").is(":visible")) {
                // callTCControl(quote_Id[0], $("#rdl_SaleInvoice").find(":checked").val());
            }
            //Chinmoy added below line
            // cddl_PosGst.SetEnabled(false);
            //#### End : added by Sayan Dutta for TC Control : End #############

            return false;
        }

        function gridProducts_EndCallback(s, e) {
            if (cgridproducts.cpComponentDetails) {
                _ComponentDetails = cgridproducts.cpComponentDetails;
                cgridproducts.cpComponentDetails = null;

                // clookup_Project.gridView.Refresh();
                var _cpProjectID = _ComponentDetails;
                clookup_Project.gridView.SelectItemsByKey(_cpProjectID);
                if (_cpProjectID > 0) {
                    //clookup_Project.gridView.SetEnabled=false;
                    clookup_Project.SetEnabled(false);
                }
                else {
                    clookup_Project.SetEnabled(true);
                }
            }
        }
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                CgvPurchaseIndent.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                CgvPurchaseIndent.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    CgvPurchaseIndent.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    CgvPurchaseIndent.SetWidth(cntWidth);
                }

            });
        });

    </script>

    <%--Tagging End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left"><span class="">
                <asp:Label ID="lblHeading" runat="server" Text="Project Indent/Requisition"></asp:Label></span>

            </h3>
            <div id="pageheaderContent" class="pull-right wrapHolder reverse content horizontal-images" style="display: none; width: 617px;">
                <div class="Top clearfix">
                    <ul>
                        <li>
                            <div class="lblHolder">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>Available Stock </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <%-- <b style="text-align: left" id="B_ImgSymbolBankBal" runat="server"></b>--%>
                                                    <b style="text-align: center" id="B_AvailableStock" runat="server">0.0</b>
                                                </div>

                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </li>
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
            <%--Abhisek--%>
            <div id="divfromTo">
                <table class="padTabtype2 pull-right  brnchreq" style="margin-top: 7px">
                    <tr>
                        <td>
                            <label>From Date</label></td>
                        <td>
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <label>To Date</label>
                        </td>
                        <td>
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <label>Unit</label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                        </td>
                    </tr>
                </table>
            </div>
            <%--Sandip Section for Show Hide Cross Button based on Condition Either for Normal Add/Edit or Approval Section--%>
            <div id="PODetailsCross" runat="server" class="crossBtn" style="display: none"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="ApprovalCross" runat="server" class="crossBtn"><a><i class="fa fa-times"></i></a></div>
            <div id="btncross" runat="server" class="crossBtn" style="display: none; margin-left: 50px;"><a href="PmsProjectIndent.aspx"><i class="fa fa-times"></i></a></div>
            <%--Sandip Section for Show Hide Cross Button based on Condition Either for Normal Add/Edit or Approval Section--%>
        </div>

    </div>
    <div class="form_main">
        <div class="clearfix" id="btnAddNew">
            <div style="float: left; padding-right: 5px;">
                <% if (rights.CanAdd)
                   { %>
                <a href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success btn-radius">
                    <span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>I</u>ndent/Requisition</span> </a>
                <% } %>
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary btn-radius" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>
                <%--Sandip Section for Approval Section in Design Start --%>

                <span id="spanStatus" runat="server">
                    <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                        <span>My Purchase Indent Status</span>
                        <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                    </a>
                </span>
                <span id="divPendingWaiting" runat="server">
                    <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary">
                        <span>Approval Waiting</span>
                        <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                    </a>
                    <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

                </span>

                <%--Sandip Section for Approval Section in Design End --%>
            </div>

        </div>
        <div id="DivEntry" style="display: none">
            <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 15px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                <div class="col-md-3" id="divNumberingScheme">
                    <label style="">Numbering Scheme</label>
                    <div>
                        <dxe:ASPxComboBox ID="CmbScheme" EnableIncrementalFiltering="true" ClientInstanceName="cCmbScheme"
                            TextField="SchemaName" ValueField="ID" IncrementalFilteringMode="Contains"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            <ClientSideEvents ValueChanged="function(s,e){CmbScheme_ValueChange()}" GotFocus="function(s,e){cCmbScheme.ShowDropDown();}" LostFocus="CmbScheme_LostFocus"></ClientSideEvents>
                        </dxe:ASPxComboBox>
                        <%--<span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>--%>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Document No.<span style="color: red;">*</span></label>
                    <div>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" onchange="txtBillNo_TextChanged()">                             
                        </asp:TextBox>
                        <%-- <asp:TextBox ID="txtIndentId" runat="server" >                             
                        </asp:TextBox>--%>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                    </div>
                </div>
                <div class="col-md-3 lblmTop9">
                    <label>Posting Date<span style="color: red;">*</span></label>
                    <div>
                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" ClientInstanceName="ctDate" DisplayFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy">
                            <%--  <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />--%>
                            <ClientSideEvents DateChanged="function(s,e){TDateChange();}" GotFocus="function(s,e){ctDate.ShowDropDown();}"></ClientSideEvents>
                            <ValidationSettings RequiredField-IsRequired="true" ErrorFrameStyle-CssClass="absolute"></ValidationSettings>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3">

                    <label>For Unit</label>
                    <div>
                        <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onchange="ddlBranch_SelectedIndexChanged()"
                            DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="clear: both"></div>
                <div class="col-md-4">
                    <label style="margin-bottom: 5px; display: inline-block">Purpose</label>
                    <div>
                        <dxe:ASPxMemo ID="txtMemoPurpose" ClientInstanceName="ctxtMemoPurpose" runat="server" Height="50px" Width="100%" Font-Names="Arial"></dxe:ASPxMemo>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="rdl_SaleInvoice" runat="server" RepeatDirection="Horizontal" onchange="return selectValue();">
                        <asp:ListItem Text="Estimate  " Value="ES"></asp:ListItem>
                        <asp:ListItem Text="BOQ" Value="BQ"></asp:ListItem>
                        <%--<asp:ListItem Text="Challan" Value="SC"></asp:ListItem>--%>
                    </asp:RadioButtonList>
                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cQuotationComponentPanel" OnCallback="ComponentQuotation_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <asp:HiddenField runat="server" ID="OldSelectedKeyvalue" />
                                <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_quotation_DataBinding"
                                    KeyFieldName="Details_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="ComponentNumber" Visible="true" VisibleIndex="1" Caption="Number" Width="150" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="ComponentDate" Visible="true" VisibleIndex="2" Caption="Date" Width="100" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="REV_No" Visible="true" VisibleIndex="3" Caption="Rev. No." Width="100" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="4" Caption="Project" Width="150" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <%--<dxe:GridViewDataColumn FieldName="ReferenceName" Visible="true" VisibleIndex="5" Caption="Reference" Width="150" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>--%>
                                        <dxe:GridViewDataColumn FieldName="BranchName" Visible="true" VisibleIndex="5" Caption="Unit" Width="150" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                    <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" GotFocus="function(s,e){gridquotationLookup.ShowDropDown();  }" DropDown="LoadOldSelectedKeyvalue" />
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="componentEndCallBack" BeginCallback="BeginComponentCallback" />
                    </dxe:ASPxCallbackPanel>
                </div>

                <div class="col-md-2 lblmTop8 hidden">
                    <dxe:ASPxLabel ID="lbl_InvoiceNO" ClientInstanceName="clbl_InvoiceNO" runat="server" Text="Posting Date">
                    </dxe:ASPxLabel>
                    <div style="width: 100%; height: 23px; border: 1px solid #e6e6e6;">
                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentDatePanel" ClientInstanceName="cComponentDatePanel" OnCallback="ComponentDatePanel_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <dxe:ASPxTextBox ID="txt_InvoiceDate" ClientInstanceName="ctxt_InvoiceDate" runat="server" Width="100%" ClientEnabled="false">
                                    </dxe:ASPxTextBox>
                                </dxe:PanelContent>
                            </PanelCollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
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
                        <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" LostFocus="clookupProjectLostFocus" ValueChanged="ProjectEndCallback" />

                    </dxe:ASPxGridLookup>
                    <dx:LinqServerModeDataSource ID="ProjectServerModeDataSource" runat="server" OnSelecting="ProjectServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />

                </div>
                <div style="clear: both"></div>

                <div class="col-md-4">
                    <label id="lblHierarchy" runat="server">Hierarchy </label>
                    <div>
                        <asp:DropDownList ID="ddlHierarchy" runat="server" DataSourceID="dsHierarchy"
                            DataTextField="HIERARCHY_NAME" DataValueField="Hierarchy_ID" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="clear: both"></div>
                <div>
                    <br />
                </div>
                <div style="display: none">
                    <div class="col-md-1">
                        <label>Currency:  </label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbCurrency" EnableIncrementalFiltering="True" ClientInstanceName="cCmbCurrency"
                                TextField="Currency_AlphaCode" ValueField="Currency_ID" DataSourceID="SqlCurrency"
                                runat="server" ValueType="System.String" EnableSynchronization="True" Width="100%" CssClass="pull-left">
                                <ClientSideEvents ValueChanged="function(s,e){Currency_Rate()}"></ClientSideEvents>
                            </dxe:ASPxComboBox>

                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>Rate:  </label>
                        <div>
                            <dxe:ASPxTextBox runat="server" ID="txtRate" ClientInstanceName="ctxtRate" Width="100%" CssClass="pull-left">
                                <MaskSettings Mask="<0..9999>.<0..99999>" IncludeLiterals="DecimalSymbol" />

                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <br />
            </div>
            <div>
                <div class="makeFullscreen ">
                    <span class="fullScreenTitle">Project Indent/Requisition</span>
                    <span class="makeFullscreen-icon half hovered " data-instance="InsgridBatch" title="Maximize Grid" id="expandEntryGrid"><i class="fa fa-expand"></i></span>
                    <dxe:ASPxGridView runat="server" ClientInstanceName="InsgridBatch" ID="gridBatch" KeyFieldName="PurchaseIndentID"
                        OnBatchUpdate="gridBatch_BatchUpdate"
                        OnCellEditorInitialize="gridBatch_CellEditorInitialize"
                        OnDataBinding="gridBatch_DataBinding"
                        Width="100%" Settings-ShowFooter="false" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false"
                        OnCustomCallback="gridBatch_CustomCallback"
                        OnRowInserting="Grid_RowInserting" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="300" SettingsPager-Mode="ShowAllRecords"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting">
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2%" VisibleIndex="0" Caption="#" HeaderStyle-HorizontalAlign="Center">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" VisibleIndex="1" Width="2%">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataComboBoxColumn Caption="Product" FieldName="gvColProduct" VisibleIndex="2">
                            <PropertiesComboBox ValueField="ProductID" ClientInstanceName="ProductID" TextField="ProductName">
                                <ClientSideEvents SelectedIndexChanged="ProductsCombo_SelectedIndexChanged" GotFocus="ProductsComboGotFocusChange" />
                            </PropertiesComboBox>
                        </dxe:GridViewDataComboBoxColumn>--%>

                            <%--Batch Product Popup Start--%>

                            <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product Name" VisibleIndex="2" Width="10%">
                                <PropertiesButtonEdit>
                                    <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="ProductKeyDown" LostFocus="ProductsGotFocus" GotFocus="ProductsGotFocusFromID" />
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

                            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Description" FieldName="gvColDiscription" Width="10%">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="true" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="4" Caption="Addl. Desc." Width="6%">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="addlDesc" Image-Url="/assests/images/more.png" Image-ToolTip="Additional Description">
                                        <Image ToolTip="Warehouse" Url="/assests/images/more.png">
                                        </Image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Quantity" FieldName="gvColQuantity" Width="6%" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit FocusedStyle-HorizontalAlign="Right" Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                    <%--For quantity user control Subhra 17-02-2019--%>
                                    <%-- <ClientSideEvents LostFocus="AutoCalValue" />--%>
                                    <ClientSideEvents LostFocus="AutoCalValue" GotFocus="QuantityProductsGotFocus" />
                                    <%--For quantity user control--%>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Price" FieldName="gvColPrice" Width="110" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit FocusedStyle-HorizontalAlign="Right" Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                    <ClientSideEvents LostFocus="AutoCalValue" GotFocus="PriceProductsGotFocus" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Amt." FieldName="gvColAmt" Width="110" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit FocusedStyle-HorizontalAlign="Right" Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" />
                                    <ClientSideEvents LostFocus="AutoCalValue" GotFocus="AmountProductsGotFocus" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn Caption="Rate" FieldName="gvColRate" Width="6%" VisibleIndex="6" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="AutoCalValueBtRate" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Value" FieldName="gvColValue" Width="8%" VisibleIndex="7" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="UOM(Pur.)" FieldName="gvColUOM" Width="7%" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit>
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataDateColumn VisibleIndex="9" Caption="Expected Delivery Date" FieldName="ExpectedDeliveryDate" Width="9%">
                                <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                    <ClientSideEvents DateChanged="function(s,e){InstrumentDateChange();}"></ClientSideEvents>
                                </PropertiesDateEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataDateColumn>

                            <dxe:GridViewDataTextColumn FieldName="Remarks" Caption="Remarks" VisibleIndex="10" Width="9%" ReadOnly="false">
                                <PropertiesTextEdit Style-HorizontalAlign="Left">
                                    <Style HorizontalAlign="Left"></Style>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="11" Caption="Action" HeaderStyle-HorizontalAlign="Center">
                                <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomAddNewRow" Image-Url="/assests/images/add.png">
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxe:GridViewCommandColumn>
                            <%--  <dxe:GridViewDataTextColumn Caption="Rate" FieldName="gvColRate" Width="0" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    <ClientSideEvents LostFocus="AutoCalValueBtRate" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Value" FieldName="gvColValue" Width="0" HeaderStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit Style-HorizontalAlign="Right">
                                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                </PropertiesTextEdit>
                                <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn FieldName="gvColIndentDetailsId" Caption="hidden Field Id" EditFormCaptionStyle-CssClass="hide" CellStyle-CssClass="hide" VisibleIndex="12" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="gvProdDetailsId" Caption="hidden Field Id" VisibleIndex="14" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="gvDetailsId" Caption="hidden Field Id" VisibleIndex="15" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="gvPRODUCTFROM" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex"
                            BatchEditStartEditing="gridFocusedRowChanged" />
                        <SettingsDataSecurity AllowEdit="true" />
                        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Row" />
                        </SettingsEditing>

                        <Styles>
                            <StatusBar CssClass="statusBar hide">
                            </StatusBar>
                        </Styles>
                    </dxe:ASPxGridView>
                </div>
                <div>
                    <br />
                </div>
                <table style="float: left;">
                    <tr>
                        <td colspan="3">

                            <%--<asp:Button ID="btnnew" CssClass="btn btn-primary" runat="server" Text="S&#818;ave & New" OnClientClick="return  Save_ButtonClick();"   />--%>
                            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                            <dxe:ASPxButton ID="btnnew" ClientInstanceName="cbtnnew" runat="server" AutoPostBack="False" Text="Save & N&#818;ew" CssClass="btn btn-primary hide" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                            </dxe:ASPxButton>


                            <%--<asp:Button ID="btnSaveExit" CssClass="btn btn-primary" runat="server" Text="Save & Ex&#818;it" OnClientClick="return  SaveExitButtonClick();" />--%>

                            <dxe:ASPxButton ID="btnSaveExit" ClientInstanceName="cbtnSaveExit" runat="server" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {SaveExitButtonClick();}" />
                            </dxe:ASPxButton>


                            <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                            </dxe:ASPxButton>
                            <uc3:UOMConversionControl runat="server" ID="UOMConversionControl" />
                        </td>
                    </tr>
                    <tr><b><span id="tagged" style="display: none; color: red">Tagged in Purchase Order. Cannot Modify</span></b></tr>
                    <tr><b><span id="Estimatetagged" style="display: none; color: red">Tagged Estimate/BOQ Revised. Cannot Edit.</span></b></tr>
                </table>
            </div>
        </div>
        <div id="DivEdit">
            <%--<dxe:ASPxGridView ID="Grid_PurchaseIndent" runat="server" AutoGenerateColumns="False" OnCustomCallback="Grid_PurchaseIndent_CustomCallback"
                ClientInstanceName="CgvPurchaseIndent" KeyFieldName="Indent_Id" Width="100%" OnCustomButtonInitialize="Grid_PurchaseIndent_CustomButtonInitialize"
                SettingsBehavior-AllowFocusedRow="true" SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
                SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true">--%>
            <div class="makeFullscreen ">
                <span class="fullScreenTitle">Project Indent/Requisition List</span>
                <span class="makeFullscreen-icon half hovered " data-instance="CgvPurchaseIndent" title="Maximize Grid" id="expandCgvPurchaseIndent"><i class="fa fa-expand"></i></span>
                <dxe:ASPxGridView ID="Grid_PurchaseIndent" runat="server" AutoGenerateColumns="False" OnCustomCallback="Grid_PurchaseIndent_CustomCallback"
                    ClientInstanceName="CgvPurchaseIndent" KeyFieldName="Indent_Id" Width="100%" OnCustomButtonInitialize="Grid_PurchaseIndent_CustomButtonInitialize"
                    SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource" Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto"
                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control"
                    Settings-HorizontalScrollBarMode="Auto">
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <%-- SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
                SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" --%>
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <ClientSideEvents CustomButtonClick="CustomButtonClick" />
                    <Columns>
                        <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                            <EditFormSettings Visible="True" />
                            <EditItemTemplate>
                                <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                            </EditItemTemplate>
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataCheckColumn>
                        <dxe:GridViewDataTextColumn FieldName="Indent_Id" Visible="false" SortOrder="Descending">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Document No." FieldName="Indent_RequisitionNumber" Width="100px">
                            <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Posting Date" FieldName="Indent_RequisitionDate" Width="100px">
                            <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"
                            DisplayFormatInEditMode="True"></PropertiesTextEdit>--%>
                            <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="From Unit" FieldName="Indent_branch" Width="150px">
                            <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Total Amount" FieldName="ValueInBaseCurrency" Width="150">
                            <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Purchase Order No." FieldName="PurchaseOrder_Number" Width="140px">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Purchase Order Date" FieldName="PurchaseOrder_Date" Width="120px">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Purpose" FieldName="Indent_Purpose" Width="200px">
                            <CellStyle CssClass="gridcellleft"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Doc No." FieldName="Order_Numbers" Width="130px">
                            <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Doc Date" FieldName="Order_Dates" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Project Name" FieldName="Proj_Name" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Entered By" FieldName="EnteredBy" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Last Update On" FieldName="LastUpdateOn" Width="100px">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="Updated By" FieldName="UpdatedBy" Width="150px">
                            <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="22" Width="180px" Caption="Actions" ButtonType="Image" HeaderStyle-HorizontalAlign="Center">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPO" Text="PO Details" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/document.png" ToolTip="PO Details"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnView" Text="View" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/viewIcon.png" ToolTip="View"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" Text="Edit" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Edit.png" ToolTip="Edit"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" Text="Delete" Styles-Style-CssClass="pad">
                                    <Image Url="/assests/images/Delete.png" ToolTip="Delete"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnPrint" Text="Print">
                                    <Image Url="/assests/images/Print.png" ToolTip="Print"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <%--Mantis Issue 25065--%>
                                <dxe:GridViewCommandColumnCustomButton ID="CustomBtnAttachment" Text="Add/View Attachment">
                                    <Image Url="/assests/images/upload.png" ToolTip="Add/View Attachment"></Image>
                                </dxe:GridViewCommandColumnCustomButton>
                                <%--End of Mantis Issue 25065--%>
                            </CustomButtons>
                        </dxe:GridViewCommandColumn>

                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
                    <SettingsBehavior AllowFocusedRow="false" ConfirmDelete="True" />
                    <Styles>
                        <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                        <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                        <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                        <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                        <Footer CssClass="gridfooter"></Footer>
                    </Styles>
                    <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                </dxe:ASPxGridView>
                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="V_ProjectPurchaseIndentList" />
                <asp:HiddenField ID="hfIsFilter" runat="server" />
                <asp:HiddenField ID="hfFromDate" runat="server" />
                <asp:HiddenField ID="hfToDate" runat="server" />
                <asp:HiddenField ID="hfBranchID" runat="server" />

                <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
                    <ClientSideEvents ControlsInitialized="AllControlInitilize" />
                </dxe:ASPxGlobalEvents>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                </dxe:ASPxGridViewExporter>
            </div>
        </div>
        <asp:HiddenField ID="hdnRefreshType" runat="server" />
        <asp:HiddenField ID="hdnEditIndentID" runat="server" />
        <asp:HiddenField ID="hdfIsDelete" runat="server" />
        <asp:HiddenField ID="hdnSchemaType" runat="server" />
        <asp:HiddenField ID="hdnCurrenctId" runat="server" />
        <asp:HiddenField ID="hdnSaveNew" runat="server" />
        <asp:HiddenField ID="hdnEditClick" runat="server" />
        <asp:HiddenField ID="hdn_Mode" runat="server" />
        <asp:HiddenField ID="hdnPIID" runat="server" />
        <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />



        <asp:SqlDataSource ID="dsBranch" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>

        <asp:SqlDataSource ID="dsHierarchy" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlSchematype" runat="server"
            SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='15')) as X Order By ID ASC"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrency" runat="server"
            SelectCommand="select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlCurrencyBind" runat="server"></asp:SqlDataSource>

        <%--Rev Subhra 0019337 23-01-2019--%>
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <ClientSideEvents EndCallback="cSelectPanelEndCall"></ClientSideEvents>
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <%--End of Rev Subhra 0019337 23-01-2019--%>

        <%-- Sandip Approval Dtl Section Start--%>
        <asp:HiddenField ID="hdngridkeyval" runat="server" />
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
                Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="row">
                            <div class="col-md-12">
                                <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" OnPageIndexChanged="gridPendingApproval_PageIndexChanged"
                                    Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Purchase Indent No." FieldName="Number"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreateDate"
                                            VisibleIndex="1" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
                                            VisibleIndex="2" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                            VisibleIndex="3" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                    <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                        </dxe:GridViewDataCheckColumn>

                                        <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                    <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                        </dxe:GridViewDataCheckColumn>
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" />
                                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline">
                                    </SettingsEditing>
                                    <SettingsSearchPanel Visible="True" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                    <ClientSideEvents EndCallback="OnApprovalEndCall" />
                                </dxe:ASPxGridView>
                            </div>
                            <div class="clear"></div>


                            <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
                <HeaderTemplate>
                    <span>User Approval</span>
                </HeaderTemplate>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
            <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
                Width="900px" HeaderText="User Wise Purchase Indent Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="number"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="createddate"
                                            VisibleIndex="1" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                            VisibleIndex="2" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                            VisibleIndex="3" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                            VisibleIndex="4" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Approved On" FieldName="ApprovedOn"
                                            VisibleIndex="5" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" />
                                    <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline">
                                    </SettingsEditing>
                                    <SettingsSearchPanel Visible="True" />
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />

                                </dxe:ASPxGridView>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
            </dxe:ASPxPopupControl>
        </div>

        <%-- Sandip Approval Dtl Section End--%>

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
                            <dxe:GridViewDataColumn FieldName="ProductsName" Caption="Name" Width="220">
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
        </dxe:ASPxPopupControl>

        <asp:SqlDataSource runat="server" ID="ProductDataSource" 
            SelectCommand="prc_PurchaseIndentDetailsList" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter Type="String" Name="Action" DefaultValue="ProductDetailsPurchaseIndent" />
            </SelectParameters>
        </asp:SqlDataSource>--%>


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
        <%--Batch Product Popup End--%>
        <%--UDF Popup --%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cUDFpopup" Height="630px"
            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="Keyval_internalId" />
        <%--UDF Popup End--%>
        <dxe:ASPxCallbackPanel runat="server" ID="acbpCrpUdf" ClientInstanceName="cacbpCrpUdf" OnCallback="acbpCrpUdf_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acbpCrpUdfEndCall" />
        </dxe:ASPxCallbackPanel>

        <dxe:ASPxCallbackPanel runat="server" ID="acpAvailableStock" ClientInstanceName="cacpAvailableStock" OnCallback="acpAvailableStock_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="acpAvailableStockEndCall" />
        </dxe:ASPxCallbackPanel>
        <%--PurchaseOrderList Popup --%>

        <dxe:ASPxPopupControl ID="apcPurchaseOrderList" runat="server" ClientInstanceName="capcPurchaseOrderList"
            Width="900px" HeaderText="" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPOLIst" runat="server" KeyFieldName="PurchaseOrder_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPOLIst" OnCustomCallback="gridPOLIst_CustomCallback" OnDataBinding="gridPOLIst_DataBinding">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Purchase Order Number" FieldName="PurchaseOrder_Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="PurchaseOrder Date" FieldName="PurchaseOrder_Date"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="ApprovalStatus"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>


                                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="15%">
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Details">
                                                <img src="../../../assests/images/viewIcon.png" />
                                            </a>
                                        </DataItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>


                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                <%--  <ClientSideEvents EndCallback="OnApprovalEndCall" />--%>
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="apcPoDetails" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="capcPoDetails" Height="630px"
            Width="1200px" HeaderText="Purchase Order Details" Modal="true" AllowResize="true" ResizingMode="Postponed">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP" ContainerElementID="divSubmitButton"
            Modal="True">
        </dxe:ASPxLoadingPanel>
        <%--PurchaseOrderList Popup END --%>
    </div>
    <asp:HiddenField runat="server" ID="hdnConvertionOverideVisible" />
    <asp:HiddenField runat="server" ID="hdnShowUOMConversionInEntry" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
    <%--for Project  --%>



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

                                <asp:TextBox ID="txtInlineRemarks" runat="server" ValidateRequestMode="Disabled" TabIndex="16" Width="100%" TextMode="MultiLine" Rows="5" Columns="10" Height="50px" MaxLength="5000"></asp:TextBox>
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
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                    Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                    <SettingsPager Visible="false"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SrlNo" Width="40" ReadOnly="true" Caption="Sl. No.">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ComponentNumber" Width="120" ReadOnly="true" Caption="Number">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductID" ReadOnly="true" Caption="Product" Width="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="Quantity" Width="70" VisibleIndex="6">
                            <PropertiesTextEdit>
                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ComponentID" ReadOnly="true" Caption="ComponentID" Width="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ESTIMATE_DETAILSID" ReadOnly="true" Caption="ESTIMATE_DETAILSID" Width="0">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsDataSecurity AllowEdit="true" />
                    <ClientSideEvents EndCallback="gridProducts_EndCallback" />
                </dxe:ASPxGridView>
                <div class="text-center">
                    <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToProdGridBind();" UseSubmitBehavior="false" />
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />
</asp:Content>
