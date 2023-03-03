<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                03-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="GSTR_SaleRegister.aspx.cs"
    Inherits="Reports.Reports.GridReports.GSTR_SaleRegister" %>

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
            bottom: 200px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/
    </style>
    <style>
        .tablpad > tbody > tr > td {
            padding: 0 10px;
        }
    </style>
    <script type="text/javascript">

        //function fn_OpenDetails(keyValue) {
        //    cCallbackPanelSale.PerformCallback('Edit~' + keyValue);
        //}

        function Tabchange() {
            $("#drdExport").val(0);
        }

        $(function () {
            BindBranches(null);

            function OnWaitingGridKeyPress(e) {

                if (e.code == "Enter") {

                }
            }
        });

        function BindBranches(noteTilte) {
            var lBox = $('select[id$=ListBoxBranches]');
            var listItems = [];
            var selectedNoteId = '';
            if (noteTilte) {

                selectedNoteId = noteTilte;
            }
            lBox.empty();


            $.ajax({
                type: "POST",
                url: 'GstrReport.aspx/GetBranchesList',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ NoteId: selectedNoteId }),
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;

                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }



                        $(lBox).append(listItems.join(''));
                        ListActivityType();

                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });
        }

        function ListActivityType() {

            $('#ListBoxBranches').chosen();
            $('#ListBoxBranches').fadeIn();

            var config = {
                '.chsnProduct': {},
                '.chsnProduct-deselect': { allow_single_deselect: true },
                '.chsnProduct-no-single': { disable_search_threshold: 10 },
                '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsnProduct-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })
        });
    </script>
    <script type="text/javascript">
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }
        function CallbackPanelSaleEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanelSale.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function CallbackPanelSaleRetEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanelSaleRet.cpBranchNames
            //End of Subhra
            Gridreturn.Refresh();
        }
        function CallbackPanelCustDebitNoteEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanelCustDebitNote.cpBranchNames
            //End of Subhra
            cgrid_debitNote.Refresh();
        }
        function CallbackPanelCustCreditNoteEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanelCustCreditNote.cpBranchNames
            //End of Subhra
            cgrid_creditNote.Refresh();
        }
        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            //var data = "OnDateChanged";
            
            var v = $("#ddlgstn").val();

            var activeTab = page.GetActiveTab();
            if (activeTab.name == 'sales') {
                $("#hfIsSaleGSTRegFilter").val("Y");
                //cCallbackPanelSale.PerformCallback('ListData~' + v);
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanelSale.PerformCallback('ListData~' + v);
                }
            }

            else if (activeTab.name == 'Return') {
                $("#hfIsSaleRetGSTRegFilter").val("Y");
                //Gridreturn.PerformCallback('ListData~' + v);
                //cCallbackPanelSaleRet.PerformCallback('ListData~' + v);
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanelSaleRet.PerformCallback('ListData~' + v);
                }
            }

            else if (activeTab.name == 'debitNote') {
                $("#hfIsCustDBNoteGSTRegFilter").val("Y");
                //cgrid_debitNote.PerformCallback('ListData~' + v);
                //cCallbackPanelCustDebitNote.PerformCallback('ListData~' + v);
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanelCustDebitNote.PerformCallback('ListData~' + v);
                }
            }

            else if (activeTab.name == 'creditNote') {
                $("#hfIsCustCRNoteGSTRegFilter").val("Y");
                //cgrid_creditNote.PerformCallback('ListData~' + v);
                //cCallbackPanelCustCreditNote.PerformCallback('ListData~' + v);
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanelCustCreditNote.PerformCallback('ListData~' + v);
                }
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

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function OpenBillDetails(branch) {

            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;
        }

        function popupHide(s, e) {
            cpopupApproval.Hide();
        }

        function OpenPOSDetails(invoice, type) {
            if (type == 'POS') {
                window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1', '_blank')
            }
            else if (type == 'SI') {
                window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type, '_blank')
            }
        }

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

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

        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
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
            bottom: 6px;
            right: 19px;
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
            top: 26px;
            right: 13px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut , #RcmGrid
        {
            max-width: 97% !important;
            /*width: 99% !important;*/
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
                Gridreturn.SetWidth(cntWidth);
                cgrid_debitNote.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
                Gridreturn.SetWidth(cntWidth);
                cgrid_debitNote.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                    Gridreturn.SetWidth(cntWidth);
                    cgrid_debitNote.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                    Gridreturn.SetWidth(cntWidth);
                    cgrid_debitNote.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Sales GSTR</h3>
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
                    <%--Rev Subhra 20-12-2018   0017670--%>
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
        <div>
            <%--Rev 1.0 : "simple-select" class add--%>
            <div class="col-md-2 simple-select">
                <label>
                    <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"></asp:Label></label>
                <asp:DropDownList ID="ddlgstn" runat="server" Width="100%"></asp:DropDownList>
            </div>
            <div class="col-md-2 branch-selection-box">
                <label>
                    <asp:Label ID="lblbranch" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <panelcollection>
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
                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </panelcollection>
                </dxe:ASPxCallbackPanel>
            </div>
            <%--Rev 1.0 : "simple-select" class add--%>
            <div class="col-md-2 simple-select">
                <label>
                    <asp:Label ID="lbl" runat="Server" Text="Inventory : " CssClass="mylabel1"></asp:Label></label>
                <asp:DropDownList ID="ddlinventory" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                    <asp:ListItem Text="Inventory" Value="1"></asp:ListItem>
                    <asp:ListItem Text="NonInventory" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </div>
            
            <div class="col-md-2">
                <label>From date</label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
            </div>
            <div class="col-md-2">
                <label>
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
            </div>


            <div class="col-md-2 custom-padd">

                <label></label>
                <div class="clear"></div>
                <div class="clear"></div>
                <asp:CheckBox runat="server" ID="chkwithouttax" Text="Include No Tax" />
            </div>

            <div class="clear"></div>


            <div class="col-md-2" style="padding-top: 10px;">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                    OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>
        <table class="pull-left tablpad">
            <tr>
                <td style="width: 254px; display: none">
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                </td>
                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear"></div>
        <table class="tablpad">
            <tr>
                <td></td>
                <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    </div>
                </td>
                <td></td>
                <td style="padding-left: 10px; padding-top: 3px"></td>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
                        Font-Size="12px" Width="100%">
                        <tabpages>
                            <dxe:TabPage Name="sales" Text="Sales">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div onkeypress="OnWaitingGridKeyPress(event)">
                                            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Datarepared" KeyFieldName="SEQ"
                                                 DataSourceID="GenerateSaleEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                                                OnCustomSummaryCalculate="ASPxGridView1_CustomSummaryCalculate"
                                                ClientSideEvents-BeginCallback="Callback_EndCallback"
                                                Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                                                <Columns>
                                                    <%-- OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding"--%>

                                                    <dxe:GridViewDataTextColumn FieldName="BRANCHNAME" Caption="Branch Name" VisibleIndex="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_DATE" Caption="Doc Date" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Doc Number" VisibleIndex="2" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Party" VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>


                                                     <dxe:GridViewDataTextColumn FieldName="PARTYTYPE" Caption="Customer Type" VisibleIndex="4" >
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="STATE" Caption="State" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="QTY" Caption="QTY" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="INVOICEDETAILS_PRODUCTDESCRIPTION" Caption="Product" VisibleIndex="8">
                                                    </dxe:GridViewDataTextColumn>

                                                      <dxe:GridViewDataTextColumn FieldName="SYSLEDGR" Caption="Ledger Description" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="INVNONINV" Caption="Inventory/Non Inventory item" VisibleIndex="10">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SPRODUCTS_HSNCODE" Caption="HSN/SAC" VisibleIndex="11">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TAXAMT" Caption="Taxable Value" CellStyle-HorizontalAlign="Right" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRATE2" Caption="CGST%" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRATE" Caption="CGST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRATE2" Caption="SGST%" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRATE" Caption="SGST" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRATE2" Caption="IGST%" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRATE" Caption="IGST" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    
                                                     <dxe:GridViewDataTextColumn FieldName="GLOBALTAX" Caption="Other Charges(Line)" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="NET" Caption="Net" CellStyle-HorizontalAlign="Right" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="OTHERTAX" Caption="Other tax(Global)" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="OLDUNITFET" Caption="Old Unit" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TOTALNET" Caption="Total" Width="120px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="NET" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CGSTRATE" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTRATE" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTRATE" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="TAXAMT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="QTY" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="OTHERTAX" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="GLOBALTAX" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="TOTALNET" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="OLDUNITFET" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="DOCUMENT_NO" SummaryType="Custom" DisplayFormat="Count" />
                                                </TotalSummary>
                                            </dxe:ASPxGridView>
                                            <dx:LinqServerModeDataSource ID="GenerateSaleEntityServerModeDataSource" runat="server" OnSelecting="GenerateSaleEntityServerModeDataSource_Selecting"
                                ContextTypeName="ReportSourceDataContext" TableName="SALESPURCHASEGSTREGISTER_REPORT"></dx:LinqServerModeDataSource>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Return" Text="Return">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div onkeypress="OnWaitingGridKeyPress(event)">
                                            <dxe:ASPxGridView runat="server" ID="ShowGrid2" ClientInstanceName="Gridreturn" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="ShowGrid2_SummaryDisplayText" KeyFieldName="SEQ"
                                                OnCustomSummaryCalculate="ASPxGridView2_CustomSummaryCalculate"
                                                Settings-HorizontalScrollBarMode="Visible"
                                                DataSourceID="GenerateSalesReturnEntityServerModeDataSource" ClientSideEvents-BeginCallback="Callback_EndCallback">
                                                <Columns>
                                                    <%-- OnCustomCallback="Grid2_CustomCallback" OnDataBinding="grid2_DataBinding"--%>

                                                    <dxe:GridViewDataTextColumn FieldName="BRANCHNAME" Caption="Branch Name" VisibleIndex="0">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_DATE" Caption="Doc Date" VisibleIndex="1" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Doc Number" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Party" VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="PARTYTYPE" Caption="Customer Type" VisibleIndex="4" >
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="STATE" Caption="State" VisibleIndex="6">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="QTY" Caption="QTY" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="INVOICEDETAILS_PRODUCTDESCRIPTION" Caption="Product" VisibleIndex="8">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SYSLEDGR" Caption="Ledger Description" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="INVNONINV" Caption="Inventory/Non Inventory item" VisibleIndex="10">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SPRODUCTS_HSNCODE" Caption="HSN/SAC" VisibleIndex="11">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="INVOICE_NUMBER" Caption="Against Inv  No" VisibleIndex="12">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="INVOICE_DATE" Caption="Inv Date" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TAXAMT" Caption="Taxable Value" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRATE2" Caption="CGST%" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRATE" Caption="CGST" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRATE2" Caption="SGST%" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRATE" Caption="SGST" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRATE2" Caption="IGST%" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRATE" Caption="IGST" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="GLOBALTAX" Caption="Other Charges(Line)" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="NET" Caption="Net" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="OTHERTAX" Caption="Other tax(Global)" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="OLDUNITFET" Caption="Old Unit" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TOTALNET" Width="120px" Caption="Total" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="NET" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CGSTRATE" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTRATE" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTRATE" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="TAXAMT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="QTY" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="OTHERTAX" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="TOTALNET" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="OLDUNITFET" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="DOCUMENT_NO" SummaryType="Custom" DisplayFormat="Count" />
                                                </TotalSummary>
                                            </dxe:ASPxGridView>
                                              <dx:LinqServerModeDataSource ID="GenerateSalesReturnEntityServerModeDataSource" runat="server" OnSelecting="GenerateSalesReturnEntityServerModeDataSource_Selecting"
                                                 ContextTypeName="ReportSourceDataContext" TableName="SALESPURCHASEGSTREGISTER_REPORT"></dx:LinqServerModeDataSource>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="debitNote" Text="Debit Note">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div onkeypress="OnWaitingGridKeyPress(event)">
                                            <dxe:ASPxGridView runat="server" ID="grid_debitNote" ClientInstanceName="cgrid_debitNote" Width="100%" KeyFieldName="SEQ"
                                                EnableRowsCache="false" Settings-HorizontalScrollBarMode="Visible"
                                                DataSourceID="GenerateCustDebitNoteEntityServerModeDataSource" ClientSideEvents-BeginCallback="Callback_EndCallback"
                                                OnSummaryDisplayText="grid_debitNote_SummaryDisplayText">
                                                <Columns>
                                                   <%-- OnCustomCallback="grid_debitNote_CustomCallback" OnDataBinding="grid_debitNote_DataBinding" --%>

                                                    <dxe:GridViewDataTextColumn FieldName="BRANCHNAME" Caption="Branch Name" VisibleIndex="0" Width="180px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_DATE" Caption="Doc Date" VisibleIndex="1" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Doc Number" VisibleIndex="2" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="3" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="STATE" Caption="State" VisibleIndex="4" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="NOTETYPE" Caption="Note Type" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Customer" VisibleIndex="6" Width="180px">
                                                      </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn FieldName="PARTYTYPE" Caption="Customer Type" VisibleIndex="7" >
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_NAME" Caption="MainAccount" VisibleIndex="8" Width="180px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SPRODUCTS_HSNCODE" Caption="HSN/SAC" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TAXAMT" Caption="Taxable Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>




                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRATE" Caption="CGST%" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTAMOUNT" Caption="CGST" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRATE" Caption="SGST%" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="SGSTAMOUNT" Caption="SGST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRATE" Caption="IGST%" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTAMOUNT" Caption="IGST" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="UTGSTRATE" Caption="UTGST%" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UTGSTAMOUNT" Caption="UTGST" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="OTHERTAX" Caption="Other Amount" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="NET" Caption="Net Amount" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="TAXAMT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="UTGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="OTHERTAX" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="NET" SummaryType="Sum" />
                                                </TotalSummary>
                                            </dxe:ASPxGridView>
                                            <dx:LinqServerModeDataSource ID="GenerateCustDebitNoteEntityServerModeDataSource" runat="server" OnSelecting="GenerateCustDebitNoteEntityServerModeDataSource_Selecting"
                                                 ContextTypeName="ReportSourceDataContext" TableName="SALESPURCHASEGSTREGISTER_REPORT"></dx:LinqServerModeDataSource>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="creditNote" Text="Credit Note">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div onkeypress="OnWaitingGridKeyPress(event)">

                                            <dxe:ASPxGridView runat="server" ID="grid_creditNote" ClientInstanceName="cgrid_creditNote" Width="100%"
                                                EnableRowsCache="false" Settings-HorizontalScrollBarMode="Visible"
                                                DataSourceID="GenerateCustCreditNoteEntityServerModeDataSource" ClientSideEvents-BeginCallback="Callback_EndCallback"
                                                OnSummaryDisplayText="grid_creditNote_SummaryDisplayText">
                                             
                                                 <Columns>

                                                     <%--OnCustomCallback="grid_creditNote_CustomCallback" OnDataBinding="grid_creditNote_DataBinding"--%>


                                                    <dxe:GridViewDataTextColumn FieldName="BRANCHNAME" Caption="Branch Name" VisibleIndex="0" Width="180px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_DATE" Caption="Doc Date" VisibleIndex="1" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Doc Number" VisibleIndex="2" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN" Caption="GSTIN" VisibleIndex="3" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="STATE" Caption="State" VisibleIndex="4" Width="120px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="NOTETYPE" Caption="Note Type" VisibleIndex="5">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Customer" VisibleIndex="6" Width="180px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="PARTYTYPE" Caption="Customer Type" VisibleIndex="7" >
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT_NAME" Caption="MainAccount" VisibleIndex="8" Width="180px">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="SPRODUCTS_HSNCODE" Caption="HSN/SAC" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn FieldName="TAXAMT" Caption="Taxable Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRATE" Caption="CGST%" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTAMOUNT" Caption="CGST" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRATE" Caption="SGST%" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="SGSTAMOUNT" Caption="SGST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRATE" Caption="IGST%" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTAMOUNT" Caption="IGST" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UTGSTRATE" Caption="UTGST%" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UTGSTAMOUNT" Caption="UTGST" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="OTHERTAX" Caption="Other Amount" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="NET" Caption="Net Amount" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="0.00">
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
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="TAXAMT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="CGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="UTGSTAMOUNT" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="OTHERTAX" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="NET" SummaryType="Sum" />
                                                </TotalSummary>
                                            </dxe:ASPxGridView>
                                             <dx:LinqServerModeDataSource ID="GenerateCustCreditNoteEntityServerModeDataSource" runat="server" OnSelecting="GenerateCustCreditNoteEntityServerModeDataSource_Selecting"
                                                 ContextTypeName="ReportSourceDataContext" TableName="SALESPURCHASEGSTREGISTER_REPORT"></dx:LinqServerModeDataSource>
                                        </div>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                        </tabpages>
                        <clientsideevents activetabchanged="Tabchange" />
                    </dxe:ASPxPageControl>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelSale" ClientInstanceName="cCallbackPanelSale" OnCallback="CallbackPanelSale_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsSaleGSTRegFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelSaleEndCall" />
</dxe:ASPxCallbackPanel>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelSaleRet" ClientInstanceName="cCallbackPanelSaleRet" OnCallback="CallbackPanelSaleRet_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsSaleRetGSTRegFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelSaleRetEndCall" />
</dxe:ASPxCallbackPanel>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelCustDebitNote" ClientInstanceName="cCallbackPanelCustDebitNote" OnCallback="CallbackPanelCustDebitNote_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsCustDBNoteGSTRegFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelCustDebitNoteEndCall" />
</dxe:ASPxCallbackPanel>

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelCustCreditNote" ClientInstanceName="cCallbackPanelCustCreditNote" OnCallback="CallbackPanelCustCreditNote_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsCustCRNoteGSTRegFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelCustCreditNoteEndCall" />
</dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

