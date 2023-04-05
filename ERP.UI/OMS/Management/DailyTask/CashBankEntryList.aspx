<%--=======================================================Revision History=========================================================================    
    1.0     Priti    V2.0.36   02-02-2023     0025263: Listing view upgradation required of Cash/Bank Voucher of Accounts & Finance
    2.0     Priti    V2.0.36   17-02-2023      After Listing view upgradation delete data show in listing issue solved.
    3.0     Priti    V2.0.38   03-04-2023      REv 2.0 Revert work from Page to procedure 
    4.0     Pallab   V2.0.38   04-04-2023      Transactions pages design modification 
=========================================================End Revision History========================================================================--%>


<%@ Page Title="Cash/Bank Voucher List" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CashBankEntryList.aspx.cs" Inherits="ERP.OMS.Management.DailyTask.CashBankEntryList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var isFirstTime = true;
        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };
            e.returnValue = false;
            e.stopPropagation();
        }
        document.onkeydown = function (e) {
            if ((event.keyCode == 86) && event.altKey == true) { //run code for alt+A -- ie, Add New  
                StopDefaultAction(e);
                AddButtonClick();

            }
            else  if ((event.keyCode == 67) && event.altKey == true) { //run code for alt+A -- ie, Add New  
                StopDefaultAction(e);
                AddButtonClickTDS();
            }

            else  if ((event.keyCode == 84) && event.altKey == true) { //run code for alt+A -- ie, Add New  
                StopDefaultAction(e);
                AddButtonClickTDSVoucher();
            }

        }
        function onPrintJv(id) {

            RecPayId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }
        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        function cSelectPanelEndCall(s, e) {

            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'CBVUCHR';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
            }
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        function AllControlInitilizeList() {
            if (isFirstTime) {

                if (localStorage.getItem('FromDateCashBank')) {
                    var fromdatearray = localStorage.getItem('FromDateCashBank').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateCashBank')) {
                    var todatearray = localStorage.getItem('ToDateCashBank').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchCashBank')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchCashBank'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('BranchCashBank'));
                    }

                }
                //updateGridByDate();
                isFirstTime = false;
            }
        }
        function AddButtonClick() {
            var url = 'CashBankEntry.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        /*Start of ADD for Copy Button on 03.03.2022*/
        /* Mantis Issue 24725 03.03.2022*/
        function OnCopyInfoClick(keyValue, ValueDate, IsTDS, IsTDSvoucher) {            
            //var url = 'CashBankEntry.aspx?key=' + 'ADD'+'&keyValue=' + keyValue + '&type=Copy';
            debugger;
            if(IsTDSvoucher!='True')
            {
                if (IsTDS == "True") {
                    jAlert("Voucher is TDS Type.Cannot Copy");
                }
                else {
                    var url = 'CashBankEntry.aspx?key=' + keyValue + '&type=Copy';
                    window.location.href = url;
                }
            }
            else {
                var url = 'CashBankEntry.aspx?key=' + keyValue + '&type=Copy';
                window.location.href = url;
            }
        }
        /* End of Mantis Issue 24725 03.03.2022 */
        /*Close of ADD for Copy Button on 03.03.2022*/

        function AddButtonClickTDS() {
            var url = 'CashBankEntry.aspx?key=' + 'ADD&IsTDS=Y';
            window.location.href = url;
            
        }

        function AddButtonClickTDSVoucher() {
            var url = 'CashBankEntryTDS.aspx?key=' + 'ADD';
            window.location.href = url;
        }

        function OnMoreInfoClick(keyValue, ValueDate, IsTDS,IsTDSvoucher) {
            if (IsTDSvoucher != 'True') {
                if (IsTDS == "True") {

                    jAlert("Voucher is TDS Type.Cannot Edit");
                }


                else if (ValueDate != "") {
                    jAlert("Voucher is Reconciled.Cannot Edit");
                }
                else {
                    var url = 'CashBankEntry.aspx?key=' + keyValue + '&type=CBE';
                    window.location.href = url;
                }
            }
            else {
                var url = 'CashBankEntryTDS.aspx?key=' + keyValue + '&type=CBE';
                window.location.href = url;
            }
           
        }
        
        function OnViewClick(keyValue, IsTDS, IsTDSvoucher) {

            if (IsTDSvoucher != 'True') {
                if (IsTDS == "True") {
                    var url = 'CashBankEntry.aspx?key=' + keyValue + '&req=V' + '&type=CBE&IsTDS=Y';
                }
                else {
                    var url = 'CashBankEntry.aspx?key=' + keyValue + '&req=V' + '&type=CBE';
                }
            }
            else {
                var url = 'CashBankEntryTDS.aspx?key=' + keyValue + '&req=V' + '&type=CBE';

            }
            
            window.location.href = url;
        }
        function GvCBSearch_EndCallBack() {
            if (cGvCBSearch.cpDelete != null) {
                // cGvCBSearch.PerformCallback();
                jAlert(cGvCBSearch.cpDelete);
                cGvCBSearch.cpDelete = null;
               // Rev 2.0
                cGvCBSearch.Refresh();
                // Rev 3.0
                //updateGridByDate();
                // Rev 3.0 End
                // Rev 2.0 End
            }
        }
        function OnClickDelete(keyValue, ValueDate) {
            if (ValueDate != "") {
                jAlert("Voucher is Reconciled.Cannot Delete");
            }
            else {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        cGvCBSearch.PerformCallback("Delete~" + keyValue);
                    }
                });
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
                localStorage.setItem("FromDateCashBank", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateCashBank", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchCashBank", ccmbBranchfilter.GetValue());
                //cGvCBSearch.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");

                //rev 1.0
                // cGvCBSearch.Refresh();
                $("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                 //end rev 1.0

                
                $("#drdExport").val(0);

            }
        }
        //rev 1.0
        function CallbackPanelEndCall(s, e) {
            cGvCBSearch.Refresh();
        }
        //end rev 1.0
        function gridRowclick(s, e) {
            //alert('hi');          
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
    <style>
        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGvCBSearch.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGvCBSearch.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGvCBSearch.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGvCBSearch.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <style>
        /*Rev 4.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
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
            right: 11px;
            z-index: 0;
            cursor: pointer;
        }

        .calendar-icon-2 {
            position: absolute;
            bottom: 7px;
            right: 4px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #FormDate , #toDate , #massrecdt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #FormDate_B-1 , #toDate_B-1 , #massrecdt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #massrecdt_B-1 #massrecdt_B-1Img
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
            top: 31px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus ,
        #GvCBSearch
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
        .mt-30{
            margin-top: 30px;
        }

        .makeFullscreen
        {
                z-index: 0;
        }

        .panel-fullscreen
        {
                z-index: 99 !important;
        }
        #massrecdt
        {
            width: 100%;
        }

        .mb-10{
            margin-bottom: 10px;
        }
        /*Rev end 4.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 4.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title  pull-left">
            <h3 class="">
                <asp:Label ID="lblHeading" runat="server" Text="Cash/Bank Voucher List"></asp:Label>                
            </h3>
        </div>
        <table class="padTabtype2 pull-right mTop5" id="gridFilter">
                    <tr>
                        <td>
                            <label>From Date</label></td>
                        <%--Rev 4.0: "for-cust-icon" class add --%>
                        <td class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                 DisplayFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 4.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 4.0--%>
                        </td>
                        <td>
                            <label>To Date</label>
                        </td>
                        <%--Rev 4.0: "for-cust-icon" class add --%>
                        <td class="for-cust-icon">
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" 
                                ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                            <%--Rev 4.0--%>
                            <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                            <%--Rev end 4.0--%>
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
        <div class="form_main rgth pull-left full">
        <div class="clearfix mb-10" id="btnAddNew">
            <div style="padding-right: 5px;">
                <span id="divAddButton">
                    <% if (rights.CanAdd)
                       { %>
                    <a id="AddButton" href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>V</u>oucher</span> </a>
                    <a id="AddButtonTDSbill" href="javascript:void(0);" onclick="AddButtonClickTDSVoucher()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>T</u>DS Voucher</span> </a>
                    <a id="AddButtonTDS" href="javascript:void(0);" onclick="AddButtonClickTDS()" class="btn btn-success "><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>TDS <u>C</u>hallan</span> </a>
                    

                    <% } %>
                </span>
                <span id="divExportto">
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdCashBank" runat="server" CssClass="btn btn-primary expad" OnSelectedIndexChanged="drdCashBankExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                        <asp:ListItem Value="4">XLSX</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>

                    <%-- <% if (rights.CanAdd)
                           { %>

                        <a id="AddButton" style="display: none;" href="javascript:void(0);" onclick="AddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>


                        <% } %>
                    </span>
                    <span id="divExportto">
                        <% if (rights.CanExport)
                           { %>

                        <asp:DropDownList ID="drdCashBank" runat="server" CssClass="btn btn-sm btn-primary expad" OnSelectedIndexChanged="drdCashBankExport_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>

                        <% } %>--%>
                </span>
                
            </div>

        </div>

          <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
          <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

        <div id="DivEdit" class="relative">
            <dxe:ASPxGridView ID="GvCBSearch" ClientInstanceName="cGvCBSearch" runat="server" AutoGenerateColumns="False"
                KeyFieldName="CBID" Width="100%" SettingsBehavior-AllowFocusedRow="true"
                OnCustomCallback="GvCBSearch_CustomCallback"
                OnSummaryDisplayText="GvCBSearch_SummaryDisplayText"
                Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="240" Settings-VerticalScrollBarMode="Auto"
                DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <SettingsBehavior ColumnResizeMode="NextColumn" />
                <Columns>
                    <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="CBID" Caption="CBID" SortOrder="Descending">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CashBank_TransactionType" Caption="Type" FixedStyle="Left">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                  <%--  0024170   <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>  uncomment--%>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TransactionDate" FixedStyle="Left"
                        Caption="Posting Date">
                         <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit> 
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="VoucherNumber" FixedStyle="Left"
                        Caption="Document No." Width="140px">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Voucher_Type" FixedStyle="Left"
                        Caption="Voucher Type" Width="140px">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="CashBank_Currency" Settings-AllowAutoFilter="False"
                        Caption="Currency" Width="6%">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="CashBankID" Settings-AllowAutoFilter="True"
                        Caption="Bank/Cash">
                        <CellStyle CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Amount" HeaderStyle-HorizontalAlign="Right"
                        Caption="Amount">
                        <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Total_taxable_amount" HeaderStyle-HorizontalAlign="Right" Settings-AllowAutoFilter="False"
                        Caption="Taxable Amount">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Total_CGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="CGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Total_SGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="SGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Total_UTGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="UTGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="Total_IGST" Settings-AllowAutoFilter="False" HeaderStyle-HorizontalAlign="Right"
                        Caption="IGST">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="EnteredBranchName" 
                        Caption="Entered Unit">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="ForBranchName" 
                        Caption="For Unit">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                   
                    <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="CashBank_PaidTo" 
                        Caption="Received From/Paid To">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                     <%-- Rev Rajdip --%>
                     <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="Paymentrequisition_Number" 
                        Caption="Ref. Cash/Fund Req. No.">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <%--End Rev Rajdip --%>
                    <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="CashBank_CreateUser" 
                        Caption="Entered By">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="CashBank_CreateDateTime" Settings-AllowAutoFilter="False"
                        Caption="Last Update On">
                        <CellStyle HorizontalAlign="Right" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="CashBank_ModifyUser" Settings-AllowAutoFilter="False"
                        Caption="Updated By">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="0" FieldName="IsTDS"></dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Width="0" FieldName="IsTDSVoucher"></dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="Proj_Name"  Settings-AllowAutoFilter="True"
                        Caption="Project Name">
                        <CellStyle Wrap="true" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="true" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="20" Width="0">
                        <DataItemTemplate>
                            <div class='floatedBtnArea'>
                                <% if (rights.CanView)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>','<%#Eval("IsTDS")%>','<%#Eval("IsTDSVoucher")%>')" class="" title="">
                                    <span class='ico ColorThree'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                <% } %>
                                <% if (rights.CanEdit)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>','<%#Eval("ValueDate")%>','<%#Eval("IsTDS")%>','<%#Eval("IsTDSVoucher")%>')" class="" title="" style='<%#Eval("Editlock")%>'>
                                    <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                <% } %>
                              <%-- <% if (rights.CanAdd)
                                   { %>--%>
                                <a href="javascript:void(0);" onclick="OnCopyInfoClick('<%# Container.KeyValue %>','<%#Eval("ValueDate")%>','<%#Eval("IsTDS")%>','<%#Eval("IsTDSVoucher")%>')" class="" title="">
                                    <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                               <%--<% } %>   --%>                            
                                <% if (rights.CanDelete)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("IBRef")%>','<%#Eval("ValueDate")%>')" class="" title="" style='<%#Eval("Deletelock")%>'>
                                    <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                <% } %>
                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a>
                                <%} %>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="CBID">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="IBRef">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="ExchangeSegmentID">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="MaxLockDate">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="ValueDate">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="BankStatementDate">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_taxable_amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                </TotalSummary>
                <Settings ShowGroupPanel="True" HorizontalScrollBarMode="Visible" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" ShowFooter="true"
                    ShowGroupFooter="VisibleIfExpanded" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <ClientSideEvents RowClick="gridRowclick" />
                <%-- <SettingsSearchPanel Visible="True" />--%>
                <ClientSideEvents EndCallback="function(s, e) {GvCBSearch_EndCallBack();}" />
            </dxe:ASPxGridView>

        </div>
    </div>
    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_CashBankList" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilizeList" />
    </dxe:ASPxGlobalEvents>
    <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfBranchID" runat="server" />
   

     <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
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
    </div>
    <%--DEBASHIS--%>

<asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
<asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
<asp:HiddenField ID="hdnLockToDatedelete" runat="server" />


    <%--  REV 1.0--%>
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
