<%--==========================================================Revision History ============================================================================================   
 1.0   Priti     V2.0.36    18-01-2023     	0025318: Views to be converted to Procedures in the Listing Page of Transaction / Return-Sales / Rate Difference Entry Customer
 2.0   Priti     V2.0.36    17-02-2023         After Listing view upgradation delete data show in listing issue solved.
 3.0   Pallab    V2.0.37    13-04-2023     	25818: Rate Difference Entry Customer page design modification
========================================== End Revision History =======================================================================================================--%>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RateDifferenceEntryCustomerList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Activities.RateDifferenceEntryCustomerList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .fwidth {
            width: 350px !important;
        }

        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
    </style>

    <%--Subhra--%>
    <script>
        var ReturnId = 0;
        function onPrintJv(id) {
            
            //ReturnId = id;
            //cSelectPanel.cpSuccess = "";
            //cDocumentsPopup.Show();
            //cCmbDesignName.SetSelectedIndex(0);
            //cSelectPanel.PerformCallback('Bindalldesignes');
            //$('#btnOK').focus();

            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                $.ajax({
                    type: "POST",
                    url: "RateDifferenceEntryCustomerList.aspx/Prc_EInvoiceChecking_details",
                    data: "{'returnid':'" + id + "','mode':'Print'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var status = msg.d;
                        if (status == "Yes") {
                            ReturnId = id;
                            cSelectPanel.cpSuccess = "";
                            cDocumentsPopup.Show();
                            cCmbDesignName.SetSelectedIndex(0);
                            cSelectPanel.PerformCallback('Bindalldesignes');
                            $('#btnOK').focus();
                        }
                        else {
                            jAlert("IRN generation is still pending. Cannot take print.");
                        }
                    }
                });
            }
            else {
                ReturnId = id;
                cSelectPanel.cpSuccess = "";
                cDocumentsPopup.Show();
                cCmbDesignName.SetSelectedIndex(0);
                cSelectPanel.PerformCallback('Bindalldesignes');
                $('#btnOK').focus();
            }
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'RateDiff_Entry_Cust';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + ReturnId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        var isFirstTime = true;

        function AllControlInitilize() {
            if (isFirstTime) {
                if (localStorage.getItem('ReturnList_FromDate')) {
                    var fromdatearray = localStorage.getItem('ReturnList_FromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ReturnList_ToDate')) {
                    var todatearray = localStorage.getItem('ReturnList_ToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }

                if (localStorage.getItem('ReturnList_Branch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ReturnList_Branch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('ReturnList_Branch'));
                    }

                }

                //if ($("#LoadGridData").val() == "ok")
                //    updateGridByDate();
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
                localStorage.setItem("ReturnList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ReturnList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ReturnList_Branch", ccmbBranchfilter.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
              
                //REV 1.0
                 // cGrdSalesReturn.Refresh();
                $("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                  //END REV 1.0

                //  cGrdSalesReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
            }
        }
        //REV 1.0
        function CallbackPanelEndCall(s, e) {
            cGrdSalesReturn.Refresh();
        }
        //END REV 1.0

    </script>
    <%--Subhra--%>
    <script>
        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=RateDifferenceEntryCustomer';
            window.location.href = URL;
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add

                StopDefaultAction(e);
                OnAddButtonClick();
            }

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnAddButtonClick() {
            var url = 'RateDifferenceEntryCustomer.aspx?key=' + 'ADD';
            window.location.href = url;
        }

        function OnMoreInfoClick(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                $.ajax({
                    type: "POST",
                    url: "RateDifferenceEntryCustomerList.aspx/Prc_EInvoiceChecking_details",
                    data: "{'returnid':'" + keyValue + "','mode':'Edit'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var status = msg.d;
                        if (status == "Yes") {
                            if (ActiveUser != null) {
                                $.ajax({
                                    type: "POST",
                                    url: "RateDifferenceEntryCustomerList.aspx/GetReturnIdIsExistPosAdjustment",
                                    data: "{'keyValue':'" + keyValue + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,
                                    success: function (msg) {
                                        var status = msg.d;
                                        if (status == "1") {
                                            jAlert('Already Adjusted.You can only view !', 'Confirmation Dialog', function (r) {
                                                if (r == true) {
                                                    var url = 'RateDifferenceEntryCustomer.aspx?key=' + keyValue + '&req=A' + '&type=RDEC';
                                                    window.location.href = url;
                                                }
                                            });
                                        }
                                        else {
                                            $.ajax({
                                                type: "POST",
                                                url: "RateDifferenceEntryCustomerList.aspx/GetEditablePermission",
                                                data: "{'ActiveUser':'" + ActiveUser + "'}",
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                success: function (msg) {
                                                    var status = msg.d;
                                                    var url = 'RateDifferenceEntryCustomer.aspx?key=' + keyValue + '&Permission=' + status + '&type=USR';
                                                    window.location.href = url;
                                                }
                                            });
                                        }


                                    }
                                });
                            }
                        }
                        else {
                            jAlert("IRN generated can not edit.");
                        }
                    }
                });
            }
            else {
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "RateDifferenceEntryCustomerList.aspx/GetReturnIdIsExistPosAdjustment",
                        data: "{'keyValue':'" + keyValue + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "1") {
                                jAlert('Already Adjusted.You can only view !', 'Confirmation Dialog', function (r) {
                                    if (r == true) {
                                        var url = 'RateDifferenceEntryCustomer.aspx?key=' + keyValue + '&req=A' + '&type=RDEC';
                                        window.location.href = url;
                                    }
                                });
                            }
                            else {
                                $.ajax({
                                    type: "POST",
                                    url: "RateDifferenceEntryCustomerList.aspx/GetEditablePermission",
                                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var status = msg.d;
                                        var url = 'RateDifferenceEntryCustomer.aspx?key=' + keyValue + '&Permission=' + status + '&type=USR';
                                        window.location.href = url;
                                    }
                                });
                            }


                        }
                    });
                }
            }
        }

            ////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
            ////Add an another param to define request type 
            function OnViewClick(keyValue) {
                var url = 'RateDifferenceEntryCustomer.aspx?key=' + keyValue + '&req=V' + '&type=RDEC';
                window.location.href = url;
            }

            function OnClickDelete(keyValue) {


                var ActiveEInvoice = $('#hdnActiveEInvoice').val();
                if (ActiveEInvoice == "1") {
                    $.ajax({
                        type: "POST",
                        url: "RateDifferenceEntryCustomerList.aspx/Prc_EInvoiceChecking_details",
                        data: "{'returnid':'" + keyValue + "','mode':'Delete'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var status = msg.d;
                            if (status == "Yes") {
                                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                                    if (r == true) {
                                        cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
                                    }
                                });
                            }
                            else {
                                jAlert("IRN generated can not delete.");
                            }
                        }
                    });
                }
                else {
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
                        }
                    });
                }
                //jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                //    if (r == true) {
                //        cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
                //    }
                //});
            }
            function OnEndCallback(s, e) {

                if (cGrdSalesReturn.cpDelete != null) {
                    jAlert(cGrdSalesReturn.cpDelete);

                    cGrdSalesReturn.cpDelete = null;
                   
                    /* Rev 2.0*/
                      cGrdSalesReturn.Refresh();
                    //updateGridByDate();
                    /* Rev 2.0 End*/
                }
            }
            //function OnClickDelete(keyValue) {
            //    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            //        if (r == true) {
            //            cGrdQuotation.PerformCallback('Delete~' + keyValue);
            //        }
            //    });
            //}

            function OnTransferClick(keyValue) {
                cPopup_BranchTransfer.Show();

                cBranchTransferCallBackPanel.PerformCallback('Edit~' + keyValue);
            }


            function SaveButtonClick() {
                var branch = $("#ddlBranch").val();
                if (branch == "") {
                    $("#MandatoryBranch").show();
                    return false;
                }
                //var CashBank = cddlCashBank.GetValue();
                //if (CashBank == null) {
                //    $("#MandatoryCashBank").show();
                //    return false;
                //}
                var EditID = document.getElementById('hdnEditID').value;
                //var branch = $("#ddlBranch").val();
                cBranchTransferCallBackPanel.PerformCallback('Save~' + EditID + '~' + branch);
            }
            function BranchTransferEndCallBack() {
                if (cBranchTransferCallBackPanel.cpEdit != null) {
                    document.getElementById('hdnEditID').value = cBranchTransferCallBackPanel.cpEdit;
                    cBranchTransferCallBackPanel.cpEdit = null;
                }
                if (cBranchTransferCallBackPanel.cpTransfer == "YES") {
                    cPopup_BranchTransfer.Hide();
                    window.location.href = 'RateDifferenceEntryCustomerList.aspx';
                    cBranchTransferCallBackPanel.cpTransfer = null;
                }
                if (cBranchTransferCallBackPanel.cpBtnVisible != null && cBranchTransferCallBackPanel.cpBtnVisible != "") {
                    cBranchTransferCallBackPanel.cpBtnVisible = null;
                    document.getElementById('btnSaveNew').style.display = 'none'
                    document.getElementById('tagged').style.display = 'block';
                }
            }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdSalesReturn.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdSalesReturn.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdSalesReturn.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdSalesReturn.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <style>
        /*Rev 3.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
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
            right: 10px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #ASPxDateEditFrom , #ASPxDateEditTo
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #ASPxDateEditFrom_B-1 , #ASPxDateEditTo_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img , #ASPxDateEditTo_B-1 #ASPxDateEditTo_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GrdSalesReturn
        {
            max-width: 98% !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .mb-10{
            margin-bottom: 10px !important;
        }

        /*.btn
        {
            padding: 5px 10px;
        }*/


        /*Rev end 3.0*/

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 3.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Rate Difference Entry Customer</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span><u>A</u>dd New</span> </a><%} %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>


            <table class="padTabtype2 pull-right" id="gridFilter">
                <tr>
                    <td>
                        <label>From Date</label></td>
                    <%--Rev 3.0: "for-cust-icon" class add --%>
                    <td class="for-cust-icon">
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                        <%--Rev 3.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 3.0--%>
                    </td>
                    <td>
                        <label>To Date</label>
                    </td>
                    <%--Rev 3.0: "for-cust-icon" class add --%>
                    <td class="for-cust-icon">
                        <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                        <%--Rev 3.0--%>
                        <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                        <%--Rev end 3.0--%>
                    </td>
                    <td>Unit</td>
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
    </div>
        <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdSalesReturn" ClientInstanceName="cGrdSalesReturn" runat="server" KeyFieldName="SrlNo"
            AutoGenerateColumns="False" Width="100%" SettingsBehavior-AllowFocusedRow="true"
            OnCustomCallback="GrdSalesReturn_CustomCallback"
            Setting-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="SalesReturnNo"
                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Return_Date" Caption="Posting Date" SortOrder="Descending" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <EditFormSettings Visible="True"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
                <%--<dxe:GridViewDataTextColumn Caption="Date" FieldName="Return_Date"
                    VisibleIndex="1">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn Caption="Invoice Number(s)" FieldName="Invoice"
                    VisibleIndex="2" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Date(s)" FieldName="InvoiceDate"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" Width="160px"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="160px"
                    VisibleIndex="4">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="5">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch" Width="170px"
                    VisibleIndex="6">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Assigned To Unit" FieldName="branchToBranch" Width="130px" Visible="false">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN ?" FieldName="IsIRN" VisibleIndex="8" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN" FieldName="IRN" VisibleIndex="9" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack No" FieldName="AckNo" VisibleIndex="10" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack Date" FieldName="AckDt" VisibleIndex="11" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="Return_CreateUser" Width="130px"
                    VisibleIndex="12">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Last Update On" FieldName="Return_CreateDateTime" Width="130px"
                    VisibleIndex="13">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="Return_ModifyUser" Width="130px"
                    VisibleIndex="14">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="15" Width="188px">
                    <DataItemTemplate>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Return_Id")%>')" class="pad" title="Edit">
                            <img src="../../../assests/images/info.png" /></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Return_Id")%>')" class="pad" title="Delete">
                            <img src="../../../assests/images/Delete.png" /></a><%} %>
                        <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                        <%--   <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status">
                            <img src="../../../assests/images/verified.png" /></a><%} %>--%>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Return_Id")%>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a><%} %>

                        <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>


                        <%--   <a href="javascript:void(0);" class="pad" title="Branch Assignment" id="a_Assignment" onclick="OnTransferClick('<%# Container.KeyValue %>')"  >
                            <span class="fa fa-truck out"></span>
                        </a> 
                        --%>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="OnEndCallback" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_RateDifferenceEntryCustomerList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdSalesReturn" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>



    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_BranchTransfer" runat="server" ClientInstanceName="cPopup_BranchTransfer"
            Width="450px" HeaderText="Branch Assignment" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="200px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchTransferCallBackPanel" ClientInstanceName="cBranchTransferCallBackPanel" OnCallback="BranchTransferCallBackPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div style="background: #f5f4f3; padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="Top clearfix">
                                        <div class="col-md-6">
                                            <label>Assigned To Unit<span style="color: red">*</span></label>
                                            <div>
                                                <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch"
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                </asp:DropDownList>
                                                <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none"
                                                    title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-6" id="tdCashBankLabel">
                                            <label>Cash/Bank <span style="color: red">*</span></label>
                                            <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>--%>
                                        <div class="clear"></div>
                                        <div class="col-md-12 lblmTop8">
                                            <label>Remarks </label>
                                            <div>
                                                <asp:TextBox ID="txtNarration" runat="server" MaxLength="500"
                                                    TextMode="MultiLine"
                                                    Width="100%" Height="80px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <table style="width: 100%;">
                                    <tr>
                                        <td style="padding: 5px 0;">
                                            <div class="text-center">
                                                <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                    AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & C&#818;lose"
                                                    UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                </dxe:ASPxButton>
                                                <b><span id="tagged" style="display: none; color: red">Advance Already Adjusted. Cannot Modify data</span></b>
                                            </div>

                                        </td>
                                    </tr>
                                </table>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="BranchTransferEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>

    </div>
    <div id="DivDataSource">
        <asp:SqlDataSource ID="dsBranch" runat="server"
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
    </div>
    <asp:HiddenField ID="hdnEditID" runat="server" />
    <%--SUBHRA--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
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
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
         <asp:HiddenField ID="hdnActiveEInvoice" runat="server" />
    </div>
        <%-- REV 1.0--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
    <%--END REV 1.0--%>

</asp:Content>

