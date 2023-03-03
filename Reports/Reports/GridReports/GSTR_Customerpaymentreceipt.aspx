<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                03-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="GSTR_Customerpaymentreceipt.aspx.cs" 
    Inherits="Reports.Reports.GridReports.GSTR_Customerpaymentreceipt" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        } 
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 150px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/       
    </style>


    <script type="text/javascript">
        $(function () {
            $('body').on('change', '#ddlgstn', function () {
                if ($("#ddlgstn").val()) {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlgstn").val());
                }
                else {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
                }
            });
        });

        //function fn_OpenDetails(keyValue) {
        //    //cPopup_Empcitys.SetHeaderText('Modify Products');
        //    Grid.PerformCallback('Edit~' + keyValue);
        //    // document.getElementById('Keyval_internalId').value = 'ProductMaster' + keyValue;
        //}

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll() {
            cBranchGridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            cBranchGridLookup.gridView.UnselectRows();
        }

        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        //$(function () {
        //    BindBranches(null);
        //    // BindCustomerVendor();
        //    function OnWaitingGridKeyPress(e) {
        //        if (e.code == "Enter") {
        //            //var index = cwatingInvoicegrid.GetFocusedRowIndex();
        //            //var listKey = cwatingInvoicegrid.GetRowKey(index);
        //            //if (listKey) {
        //            //    if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
        //            //        var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
        //            //        LoadingPanel.Show();
        //            //        window.location.href = url;
        //            //    } else {
        //            //        ShowReceiptPayment();
        //            //    }
        //            //}
        //        }
        //    }
        //});



        $(function () {
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            //$("#drp_partytype").change(function () {
            //    var end = $("#drp_partytype").val();
            //    if (end == '1') {
            //        $("#Label3").text('Customer');
            //    }
            //    else if (end == '2') {
            //        $("#Label3").text('Vendor');
            //    }
            //    else if (end == '0') {
            //        $("#Label3").text('Customer/Vendor');
            //    }
            //    BindCustomerVendor(end);
            //});

            function OnWaitingGridKeyPress(e) {
                //alert('1Hi');
                if (e.code == "Enter") {
                    //alert('Hi');
                    //var index = cwatingInvoicegrid.GetFocusedRowIndex();
                    //var listKey = cwatingInvoicegrid.GetRowKey(index);
                    //if (listKey) {
                    //    if (cwatingInvoicegrid.GetRow(index).children[6].innerText != "Advance") {
                    //        var url = 'PosSalesInvoice.aspx?key=' + 'ADD&&BasketId=' + listKey;
                    //        LoadingPanel.Show();
                    //        window.location.href = url;
                    //    } else {
                    //        ShowReceiptPayment();
                    //    }
                    //}
                }
            }
        });

        //function BindBranches(noteTilte) {
        //    var lBox = $('select[id$=ListBoxBranches]');
        //    var listItems = [];
        //    var selectedNoteId = '';
        //    if (noteTilte) {
        //        selectedNoteId = noteTilte;
        //    }
        //    lBox.empty();
        //    $.ajax({
        //        type: "POST",
        //        url: 'GstrReport.aspx/GetBranchesList',
        //        contentType: "application/json; charset=utf-8",
        //        data: JSON.stringify({ NoteId: selectedNoteId }),
        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;
        //            if (list.length > 0) {
        //                for (var i = 0; i < list.length; i++) {
        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];
        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }
        //                $(lBox).append(listItems.join(''));
        //                ListActivityType();
        //                $('#ListBoxBranches').trigger("chosen:updated");
        //                $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxBranches').trigger("chosen:updated");
        //                $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");
        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });
        //}

        //function ListActivityType() {
        //    $('#ListBoxBranches').chosen();
        //    $('#ListBoxBranches').fadeIn();
        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}

        //function ListLedgerType() {
        //    $('#ListBoxLedgerType').chosen();
        //    $('#ListBoxLedgerType').fadeIn();
        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}
        //function ListCustomerVendor() {

        //    $('#ListBoxCustomerVendor').chosen();
        //    $('#ListBoxCustomerVendor').fadeIn();

        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryBranch').attr('style', 'display:none');
            })



            <%-- $("#ListBoxLedgerType").chosen().change(function () {
                 var Ids = $(this).val();

                 $('#<%=hdnSelectedLedger.ClientID %>').val(Ids);
                 $('#MandatoryLedgerType').attr('style', 'display:none');

             })--%>

            <%-- $("#ListBoxCustomerVendor").chosen().change(function () {
                 var Ids = $(this).val();

                 $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                 $('#MandatoryCustomerType').attr('style', 'display:none');

             })--%>

        })


        //function BindLedgerType(Ids) {
        //    var lBox = $('select[id$=ListBoxLedgerType]');
        //    var listItems = [];
        //    $.ajax({
        //        type: "POST",
        //        url: 'LedgerPostingReport.aspx/BindLedgerType',
        //        data: "{'Ids':'" + Ids + "'}",
        //        //  data: JSON.stringify({ Ids: Ids }),
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;
        //            if (list.length > 0) {
        //                for (var i = 0; i < list.length; i++) {
        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];
        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }
        //                $(lBox).append(listItems.join(''));
        //                ListLedgerType();
        //                $('#ListBoxLedgerType').trigger("chosen:updated");
        //                $('#ListBoxLedgerType').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxLedgerType').trigger("chosen:updated");
        //                $('#ListBoxLedgerType').prop('disabled', true).trigger("chosen:updated");
        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });
        //}


        //function BindCustomerVendor() {
        //    var lBox = $('select[id$=ListBoxCustomerVendor]');
        //    var listItems = [];
        //    $.ajax({
        //        type: "POST",
        //        url: 'LedgerPostingReport.aspx/BindCustomerVendor',
        //        //data: "{'Ids':'" + Ids + "'}",                   
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;
        //            if (list.length > 0) {
        //                for (var i = 0; i < list.length; i++) {
        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];
        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }
        //                $(lBox).append(listItems.join(''));
        //                ListCustomerVendor();
        //                $('#ListBoxCustomerVendor').trigger("chosen:updated");
        //                $('#ListBoxCustomerVendor').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxCustomerVendor').trigger("chosen:updated");
        //                $('#ListBoxCustomerVendor').prop('disabled', true).trigger("chosen:updated");
        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });
        //}

    </script>

    <script type="text/javascript">
        //function cxdeToDate_OnChaged(s, e) {
        //    var data = "OnDateChanged";
        //    data += '~' + cxdeFromDate.GetDate();
        //    data += '~' + cxdeToDate.GetDate();
        //    //CallServer(data, "");
        //    // Grid.PerformCallback('');
        //}

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            //var data = "OnDateChanged";
            var v = $("#ddlgstn").val();
            $("#hfIsGSTRCustRecPayRegFilter").val("Y");
            //data += '~' + cxdeFromDate.GetDate();
            //data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);
            //   Grid.PerformCallback('Te');

            //Grid.PerformCallback('ListData~' + v);
            //cCallbackPanel.PerformCallback('ListData~' + v);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback('ListData~' + v);
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

        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 18-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }

        //function OnContextMenuItemClick(sender, args) {
        //    if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
        //        args.processOnServer = true;
        //        args.usePostBack = true;
        //    } else if (args.item.name == "SumSelected")
        //        args.processOnServer = true;
        //}


        //function abc() {
        //     alert();
        //    $("#drdExport").val(0);
        //}

        //function OpenBillDetails(branch) {
        //    cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
        //    cpopupApproval.Show();
        //    return true;
        //}

        //function popupHide(s, e) {
        //    cpopupApproval.Hide();
        //}

        //function OpenPOSDetails(invoice, type) {
        //    // alert(type);
        //    if (type == 'POS') {
        //        window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1', '_blank')
        //    }
        //    else if (type == 'SI') {
        //        window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')
        //    }
        //}


        function Callback_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
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
            bottom: 8px;
            right: 6px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
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
            right: 8px;
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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut , #RcmGrid
        {
            max-width: 98% !important;
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
            <h3>GSTR Customer Payment Receipt</h3>
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
                   <%--Rev Subhra 18-12-2018   0017670--%>
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

    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <table class="pull-left">
            <tr>
                <td style="width: 254px; display: none">
                    <%--<asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:Hid--%>denField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                </td>
                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td class="simple-select">
                    <asp:DropDownList ID="ddlgstn" runat="server" Width="150px"></asp:DropDownList>
                </td>

                <%-- new added 18-09-2017 --%>
                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>

                <td style="width: 254px">

                    <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <span id="MandatoryBranch" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EII" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="HiddenField3" runat="server" />

                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="ASPxCallbackPanel1_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="branch_id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" />
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
                <%--end new added --%>

                <td>
                    <table>
                        <tr>
                            <td>
                                <div style="color: #b5285f; padding-left: 15px; font-weight: bold;" class="clsFrom">
                                    <asp:Label ID="Label1" runat="Server" Text="From Date : " CssClass="mylabel1"
                                        Width="92px"></asp:Label>
                                </div>
                            </td>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
                            </td>
                            <td style="padding-left: 15px">
                                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                                        Width="92px"></asp:Label>
                                </div>
                            </td>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                                <%--Rev 1.0--%>
                                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                <%--Rev end 1.0--%>
                            </td>

                            <%--<td style="padding-left: 70px; padding-top: 1px">--%>
                            <td style="padding-left: 20px; padding-top: 0px">
                                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                            </td>

                            <td style="padding-left: 10px; padding-top: 0px">
                                <%-- <% if (rights.CanExport)
                               { %>--%>
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="btn btn-sm btn-primary"
                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
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
                </td>
            </tr>
            <tr>
            </tr>
        </table>
        <div class="pull-right">
            <%-- <% if (rights.CanExport)
               { %>--%>
            <%--<asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLSX</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>--%>
            <%-- <% } %>--%>
        </div>
        <table class="TableMain100">

            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" KeyFieldName="SrlNo"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSource"
                            ClientSideEvents-BeginCallback="Callback_EndCallback" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
                            <%--OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding" OnDataBound="Showgrid_Datarepared" OnCustomSummaryCalculate="ASPxGridView1_CustomSummaryCalculate" --%>
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" VisibleIndex="0" Width="50px" FixedStyle="Left">
                                <CellStyle HorizontalAlign="Left"></CellStyle>
                                <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="branch_description" Caption="Unit" VisibleIndex="1" Width="140px" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_VoucherNumber" Caption="ARN No." VisibleIndex="1" Width="140px" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_TransactionDate" Caption="ARN Date" VisibleIndex="2" FixedStyle="Left" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Transaction_Type" Caption="Transaction Type" FixedStyle="Left" VisibleIndex="4" Width="135px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Party" Caption="Customer's Name" FixedStyle="Left" VisibleIndex="5" Width="200px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00" Width="140px">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="HSN" Caption="HSN" VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Taxable_Amount" Caption="Advanced Taxable Amount" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Total_CGST" Caption="CGST" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Total_SGST" Caption="SGST" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Total_UTGST" Caption="UTGST" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Total_IGST" Caption="IGST" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Voucher_Amount" Caption="Total Advance Amount" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Invoice_Number" Caption="Adjusted Invoice No." VisibleIndex="14">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Invoice_Date" Caption="Invoice Date" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Adjusted_Tax_Amount" Caption="Adjusted Taxable Amount" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Adjusted_CGST" Caption="Adjusted CGST" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Adjusted_SGST" Caption="Adjusted SGST" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Adjusted_UTGST" Caption="Adjusted UTGST" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Adjusted_IGST" Caption="Adjusted IGST" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Adjusted_Amount" Caption="Total Adjusted Amount" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <%--                                <dxe:GridViewDataTextColumn FieldName="Doc Type" Caption="Doc Type" VisibleIndex="3">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Transaction Type" Caption="Transaction Type" VisibleIndex="4">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="HSN" Caption="HSN" VisibleIndex="5">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="State" Caption="State" VisibleIndex="8">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Tax_Amount" Caption="Tax Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>--%>
                                
                                <dxe:GridViewDataTextColumn FieldName="State" Caption="State" VisibleIndex="22">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Balanced_Tax_Amount" Caption="Balanced Taxable Amount" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Balanced_CGST" Caption="Balanced CGST" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Balanced_SGST" Caption="Balanced SGST" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Balanced_UTGST" Caption="Balanced UTGST" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Balanced_IGST" Caption="Balanced IGST" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Balanced_Amount" Caption="Total Balanced Amount" VisibleIndex="28" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>
                            </Columns>

                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />

                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="Taxable_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Tax_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Voucher_Amount" SummaryType="Sum" />

                                <dxe:ASPxSummaryItem FieldName="Adjusted_Tax_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Adjusted_CGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Adjusted_SGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Adjusted_UTGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Adjusted_IGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Adjusted_Amount" SummaryType="Sum" />

                                <dxe:ASPxSummaryItem FieldName="Balanced_Tax_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Balanced_CGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Balanced_SGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Balanced_UTGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Balanced_IGST" SummaryType="Sum" />
                                <%--<dxe:ASPxSummaryItem FieldName="Balanced_Amount" SummaryType="custom" />--%>
                                <dxe:ASPxSummaryItem FieldName="Balanced_Amount" SummaryType="Sum" />

                                <dxe:ASPxSummaryItem FieldName="Invoice_Number" SummaryType="Custom" Tag="Item_InvoiceNumber" />
                                <dxe:ASPxSummaryItem FieldName="ReceiptPayment_VoucherNumber" SummaryType="Custom" Tag="Item_RecPayNumber" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="GSTRCUSTVENDRECPAYREGISTER_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="ShowGrid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsGSTRCustRecPayRegFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

