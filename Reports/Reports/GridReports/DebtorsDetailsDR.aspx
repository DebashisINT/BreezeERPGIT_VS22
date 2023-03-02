<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                02-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DebtorsDetailsDR.aspx.cs" Inherits="Reports.Reports.GridReports.DebtorsDetailsDR" %>

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
            bottom: 180px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'CustomerSource') {
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
            else if (ArrName == 'FinancerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#FinancerModel').modal('hide');
                    ctxtFinancerName.SetText(Name);
                    GetObjectID('hdnFinancerId').value = key;
                }
                else {
                    ctxtFinancerName.SetText('');
                    GetObjectID('hdnFinancerId').value = '';
                }
            }
        }
    </script>
    <%-- For multiselection when click on ok button--%>

    <%-- For Customer multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#CustModel').on('shown.bs.modal', function () {
                 $('#txtCustSearch').focus();
             })
         });

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

    <%-- For Financer multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#FinancerModel').on('shown.bs.modal', function () {
                 $('#txtFinancerSearch').focus();
             })
         })
         var FinancerArr = new Array();
         $(document).ready(function () {
             var FinancerObj = new Object();
             FinancerObj.Name = "FinancerSource";
             FinancerObj.ArraySource = FinancerArr;
             arrMultiPopup.push(FinancerObj);
         })
         function FinancerButnClick(s, e) {
             $('#FinancerModel').modal('show');
         }

         function Financer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#FinancerModel').modal('show');
             }
             ctxtFinancerName.Focus();
         }

         function Financerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtFinancerSearch").val()) == "" || $.trim($("#txtFinancerSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtFinancerSearch").val();
             // OtherDetails.BranchID = $('#ddl_Branch').val();
             OtherDetails.BranchID = hdnSelectedBranches.value;

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Financer Name");

                 if ($("#txtFinancerSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetOnlyFinancer", OtherDetails, "FinancerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "FinancerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtFinancerSearch').focus();
             else
                 $('#txtFinancerSearch').focus();
         }
    </script>
    <%-- For Financer multiselection--%>

    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridcustomerLookup.GetGridView();
            grid.UnselectRows();
        }

        function BindOtherDetails(e) {
            var Branchids = gridbranchLookup.gridView.GetSelectedKeysOnPage();
            gridfinancerLookup.gridView.SetFocusedRowIndex(-1);
            gridcustomerLookup.gridView.SetFocusedRowIndex(-1);

            cCustomerCallbackPanel.PerformCallback(Branchids.join(','));
            cFinancierCallbackPanel.PerformCallback(Branchids.join(','));
        }

    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#hdnSelectedBranches").val('');
        })

        function BranchValuewisefinancer() {
            gridbranchLookup.GetGridView().GetSelectedFieldValues("branch_id", GotSelectedValues);
        }
        function GotSelectedValues(selectedValues) {
            if (selectedValues.length == 0) {
            }
            else {
                values = "";
                for (i = 0; i < selectedValues.length; i++) {
                    if (values == "") {
                        values = selectedValues[i] + "";
                    }
                    else {
                        values = values + ',' + selectedValues[i] + "";
                    }
                }
                $("#hdnSelectedBranches").val(values);
            }
        }

    </script>

    <script type="text/javascript">
        //function cxdeToDate_OnChaged(s, e) {
        //    var data = "OnDateChanged";
        //    data += '~' + cxdeToDate.GetDate();
        //    //CallServer(data, "");
        //    // Grid.PerformCallback('');
        //}

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsDebtorsDetDRFilter").val("Y");
            data += '~' + cxdeToDate.GetDate();

            //Grid.PerformCallback(data);

            //cCallbackPanel.PerformCallback(data);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data);
            }

            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As On: " + ToDate;
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
           <%--Rev Subhra 17-12-2018   0017670--%>
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

        //function BudgetAfterHide(s, e) {
        //    popupbudget.Hide();
        //}

        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
        }


        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function CloseGridLookupBranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        //function selectAll_customer() {
            //gridcustomerLookup.gridView.SelectRows();
        //}
        //function unselectAll_customer() {
        //    gridcustomerLookup.gridView.UnselectRows();
        //}
        //function CloseGridLookupCustomer() {
        //    gridcustomerLookup.ConfirmCurrentSelection();
        //    gridcustomerLookup.HideDropDown();
        //    gridcustomerLookup.Focus();
        //}

        //function selectAll_financier() {
        //    gridfinancerLookup.gridView.SelectRows();
        //}
        //function unselectAll_financier() {
        //    gridfinancerLookup.gridView.UnselectRows();
        //}
        //function CloseGridLookupFinancier() {
        //    gridfinancerLookup.ConfirmCurrentSelection();
        //    gridfinancerLookup.HideDropDown();
        //    gridfinancerLookup.Focus();
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
            bottom: 12px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut , .TableMain100 #ShowGridVendAgeingWithDaysInterval
        {
            max-width: 98% !important;
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

        .pTop10 {
    padding-top: 20px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Debtors Details Debit</h3>
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
                  <%--Rev Subhra 11-12-2018   0017670--%>
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
        <table class="pull-left">
            <tr>
                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td style="width: 254px">
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                        OnDataBinding="lookup_branch_DataBinding"
                        KeyFieldName="branch_id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                            <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>

                            <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" UseSubmitBehavior="False" />
                                                <%--</div>--%>
                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" UseSubmitBehavior="False" />                                                
                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupBranch" UseSubmitBehavior="False" />
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
                        <ClientSideEvents TextChanged="function(s, e) { BindOtherDetails(e)}" />
                        <ClientSideEvents ValueChanged="BranchValuewisefinancer" />
                    </dxe:ASPxGridLookup>
                </td>


               <%-- <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Customer : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </div>
                </td>--%>
                <%--<td style="width: 254px">
                    <dxe:ASPxCallbackPanel runat="server" ID="CustomerCallbackPanel" ClientInstanceName="cCustomerCallbackPanel" OnCallback="CustomerCallbackPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <asp:ListBox ID="ListBox1" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                                <dxe:ASPxGridLookup ID="lookup_customer" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridcustomerLookup"
                                    OnDataBinding="lookup_customer_DataBinding" OnCallback="lookup_customer_Callback"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Contact" Visible="true" VisibleIndex="2" Caption="Contact No." Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Alternate" Visible="true" VisibleIndex="3" Caption="Alternate No." Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Addresscus" Visible="true" VisibleIndex="4" Caption="Address" Width="180" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_customer" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_customer" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupCustomer" UseSubmitBehavior="False" />
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
                </td>--%>
                <td style="padding-left: 15px">
                    <dxe:ASPxLabel ID="lbl_Customer" style="color: #b5285f; font-weight: bold;" runat="server" Text="Customer :">
                    </dxe:ASPxLabel>
                </td>
                <td style="width: 254px">
                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                    <span id="MandatorysCustomer" style="display: none" class="validclass">
                        <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </td>
               <%-- <td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Financer : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>--%>
                <%--<td style="width: 254px">
                    <dxe:ASPxCallbackPanel runat="server" ID="FinancierCallbackPanel" ClientInstanceName="cFinancierCallbackPanel" OnCallback="FinancierCallbackPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>

                                <dxe:ASPxGridLookup ID="gridfinancerLookup" SelectionMode="Multiple" runat="server" ClientInstanceName="gridfinancerLookup"
                                    OnDataBinding="lookup_financer_DataBinding" OnCallback="gridfinancerLookup_Callback"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Financer Name" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton2_finance" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_financier" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1_finance" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_financier" />
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupFinancier" UseSubmitBehavior="False" />
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
                </td>--%>
                <td style="padding-left: 15px; padding-right: 10px;">
                    <dxe:ASPxLabel ID="lbl_Financer" style="color: #b5285f;font-weight: bold;" runat="server" Text="Financer :">
                    </dxe:ASPxLabel>
                </td>
                <td style="width: 254px">
                    <dxe:ASPxButtonEdit ID="txtFinancerName" runat="server" ReadOnly="true" ClientInstanceName="ctxtFinancerName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){FinancerButnClick();}" KeyDown="function(s,e){Financer_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </td>
            </tr>

            <tr>
                <td style="padding-top: 10px;">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="As On Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <%--Rev 1.0 : "for-cust-icon" class add--%>
                <td class="for-cust-icon" style="padding-top: 10px;">
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
                </td>
                <td style="padding-left: 15px; padding-top: 15px;">
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
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" ClientSideEvents-BeginCallback="Callback2_EndCallback" OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
                            Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="270" Settings-VerticalScrollBarMode="Auto">
                            <%--OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid2_DataBinding"--%>
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit and Warehouse Details" Width="50%" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="Over_120_Days" Caption="Over 120 days" Width="25%" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Within_120_Days" Caption="Within 120 days" Width="20%" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="Total" Caption="Total" Width="25%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" />
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" AllowSort="False" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="Over_120_Days" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Within_120_Days" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="DEBTORSDETAILS_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

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

     <!--Financer Modal -->
    <div class="modal fade" id="FinancerModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Financer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Financerkeydown(event)" id="txtFinancerSearch" width="100%" placeholder="Search By Financer Name" />
                    <div id="FinancerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Financer Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('FinancerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('FinancerSource')">OK</button>
                </div>
            </div>
        </div>
    </div>
    <!--Financer Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdnFinancerId" runat="server" />
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <%--<ClientSideEvents CloseUp="BudgetAfterHide" />--%>
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsDebtorsDetDRFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

