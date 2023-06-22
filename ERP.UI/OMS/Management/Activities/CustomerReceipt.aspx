<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                28-04-2023        2.0.37           Pallab              25967: Add Customer Receipt module design modification & check in small device
2.0                20-06-2023        2.0.38           Pallab              26399: Add Customer Receipt module all bootstrap modal outside click event disable
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerReceipt.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerReceipt" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>--%>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/ucPaymentDetails.ascx" TagPrefix="uc1" TagName="ucPaymentDetails" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/Sales_BillingShipping.ascx" TagPrefix="ucBS" TagName="Sales_BillingShipping" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/CustomerReceipt.js?v2.0"></script>
    <link href="CSS/CustomerReceiptPayment.css?v2.0" rel="stylesheet" />
    <script src="UserControls/Js/ucPaymentDetails.js"></script>
    <script src="JS/SearchPopup.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMPopUp.js"></script>
    <link href="CSS/PosSalesInvoice.css" rel="stylesheet" />
    
    <style>
        table#floatedTbl > tbody {
            display:table-header-group !important;
        }
        table#floatedTbl > tbody>tr {
                display: table-row !important;
        }
    </style>

    <script>

        function AddcustomerClick() {
            var isLighterPage = $("#hidIsLigherContactPage").val();
            if (isLighterPage == 1) {
                var url = '/OMS/management/Master/customerPopup.html?var=1.1.3.5';
                AspxDirectAddCustPopup.SetContentUrl(url);
                //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
                AspxDirectAddCustPopup.RefreshContentUrl();
                AspxDirectAddCustPopup.Show();
            }
            else {
                var url = '/OMS/management/Master/Contact_general.aspx?id=' + 'ADD';
                window.location.href = url;
            }
        }

    </script>

    <%-- Project Script start --%>
    <script>


        function cProject_GotFocus(s, e) {
            var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
            $("#hdnEditProjId").val(ProjectId);
            // clookup_Project.ShowDropDown();

        }



        function lookup_ProjectCodeKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProjectCodePopup.Hide();
                //var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
                //if (vouchertype == 'P') {
                //    //grid.batchEditApi.StartEdit(globalRowIndex, 5);
                //}
                //else {
                //    // grid.batchEditApi.StartEdit(globalRowIndex, 4);
                //}
            }
        }

        //chinmoy added for inline project code 10-12-2019
        function ProjectCodeButnClick(s, e) {
            if (e.buttonIndex == 0) {
                if ($("#hdnAllowProjectInDetailsLevel").val() != "0") {
                    clookupPopup_ProjectCode.Clear();
                    var Type = (grid.GetEditor('Type').GetValue() != null) ? grid.GetEditor('Type').GetValue() : "0";
                    var InvoiceNo = (grid.GetEditor('DocumentNo').GetValue() != null) ? grid.GetEditor('DocumentNo').GetValue() : "0";
                    if (clookupPopup_ProjectCode.Clear()) {
                        cProjectCodePopup.Show();
                        clookupPopup_ProjectCode.Focus();
                    }

                    var id = (clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()));
                    if (((Type == "On Account") || (Type == "Advance")) && (id != null)) {

                        cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + id);
                    }
                    else {
                        cProjectCodeCallback.PerformCallback('Type~' + Type + "~" + InvoiceNo);
                    }
                }
            }
        }


        function Project_Code_LostFocus(s, e) {
            grid.batchEditApi.StartEdit(globalRowIndex, 5);
            setTimeout(function () {
                grid.batchEditApi.EndEdit();
                grid.batchEditApi.StartEdit(globalRowIndex, 6);
            }, 200);
        }


        function ProjectCodeKeyDown(s, e) {


            if (e.htmlEvent.key == "Enter" || e.htmlEvent.key == "NumpadEnter") {


                s.OnButtonClick(0);
            }
        }

        //End


        function ProjectCodeCallback_endcallback() {

            clookupPopup_ProjectCode.ShowDropDown();;
            clookupPopup_ProjectCode.Focus();
            clookupPopup_ProjectCode.Clear()

        }
        function ProjectCodeinlineSelected(s, e) {
            if ($("#hdnProjectSelectInEntryModule").val() == "1") {
                if ((clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex()) != null)) {
                    LoadDocument();
                }
            }
        }


        function ProjectCodeSelected(s, e) {
            debugger;
            if (clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex() == -1) {
                cProjectCodePopup.Hide();
                //var vouchertype = $("#ComboVoucherType").val(); //cComboVoucherType.GetValue();
                //if (vouchertype == 'P') {
                //    grid.batchEditApi.StartEdit(globalRowIndex, 5);
                //}
                //else {
                //    grid.batchEditApi.StartEdit(globalRowIndex, 4);
                //}
                return;
            }
            var ProjectInlineLookUpData = clookupPopup_ProjectCode.GetGridView().GetRowKey(clookupPopup_ProjectCode.GetGridView().GetFocusedRowIndex());
            var ProjectInlinedata = ProjectInlineLookUpData.split('~')[0];
            grid.batchEditApi.StartEdit(globalRowIndex);
            var ProjectCode = clookupPopup_ProjectCode.GetValue();
            cProjectCodePopup.Hide();

            grid.GetEditor("Project_Code").SetText(ProjectCode);
            grid.GetEditor("ProjectId").SetText(ProjectInlinedata);

        }



        function ProjectListKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function ProjectListButnClick(s, e) {
            //ctaggingGrid.PerformCallback('BindComponentGrid');
            clookup_Project.ShowDropDown();
        }

        //Hierarchy Start Tanmoy
        function clookup_Project_LostFocus() {
         
            
            var gridVal = "";
            if (grid.GetVisibleRowsOnPage() > 0) {
                if ($("#hdAddEdit").val() == "Add") {
                    grid.batchEditApi.StartEdit(-1);
                    gridVal = grid.GetEditor("Type").GetValue();
                    grid.batchEditApi.EndEdit();
                }
                else {
                    grid.batchEditApi.StartEdit(0);
                    gridVal = grid.GetEditor("Type").GetValue();
                    grid.batchEditApi.EndEdit();
                }
            }
            //&& Gotprojid != ""
            var Gotprojid = $("#hdnEditProjId").val();
            if (grid.GetVisibleRowsOnPage() > 0 && gridVal != "" && gridVal != null) {
                debugger;
                jConfirm('Project Change will  blank  the grid. Confirm ?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        LoadDocument();
                        grid.PerformCallback("GridBlank");

                    }
                    else {
                        clookup_Project.gridView.SelectItemsByKey($("#hdnEditProjId").val())
                    }
                });
            }


            var projID = clookup_Project.GetValue();
            if (projID == null || projID == "") {
                $("#ddlHierarchy").val(0);
            }

            //var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'SalesChallanAdd.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });



        }

        function ProjectValueChange(s, e) {
            //debugger;
            var projID = clookup_Project.GetValue();

            $.ajax({
                type: "POST",
                url: 'SalesChallanAdd.aspx/getHierarchyID',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ProjID: projID }),
                success: function (msg) {
                    var data = msg.d;
                    $("#ddlHierarchy").val(data);
                }
            });
        }
        //Hierarchy End Tanmoy
    </script>
    <%-- Project Script End --%>
    <style>
        #lookup_Project .dxeButtonEditClearButton_PlasticBlue {
            display: table-cell;
        }
    </style>

    <%--<style>
        /*Rev 1.0*/

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
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img
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
            line-height: 18px;
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

        /*input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }*/
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
        #Grid_CustomerReceiptPayment
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

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        #txtProdSearch
        {
            margin-bottom: 10px;
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

        #massrecdt
        {
            width: 100%;
        }

        .col-sm-3 , .col-md-3 , .col-md-2{
            margin-bottom: 10px;
        }

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        .typeNotification span
        {
             color: #ffffff !important;
        }

        #rdl_Salesquotation
        {
            margin-top: 8px;
    line-height: 20px;
        }

        #ASPxLabel8
        {
            line-height: 16px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

        #OFDBankSelect
        {
            height: 30px;
            border-radius: 4px;
        }

        .mt-28{
            margin-top: 28px;
        }

        .mb-10{
            margin-bottom: 10px;
        }

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0;
        }*/

        #CallbackPanel_LPV
        {
            top: 450px !important;
        }

        /*.GridViewArea
        {
            z-index: 0;
        }*/

        select.btn
        {
            height: 34px !important;
        }

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }

        .lblmBot4 > span, .lblmBot4 > label
        {
                margin-bottom: 0px !important;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0px !important;
        }

        .btn
        {
            padding: 5px 10px;
        }
        .mbot5 .col-md-8 {
    margin-bottom: 5px;
}
         #ApprovalCross
        {
            top: 6px !important;
            right: 6px !important;
        }

        /*Rev end 1.0*/
        </style>--%>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    

    <style>
            #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PLSales , #dt_SaleInvoiceDue , #dtPostingDate
            {
                position: relative;
                z-index: 1;
                background: transparent;
            }

            #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PLSales_B-1 , #dt_SaleInvoiceDue_B-1 , #dtPostingDate_B-1
            {
                background: transparent !important;
                border: none;
                width: 30px;
                padding: 10px !important;
            }

            #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
            #dt_PLSales_B-1 #dt_PLSales_B-1Img , #dt_SaleInvoiceDue_B-1 #dt_SaleInvoiceDue_B-1Img , #dtPostingDate_B-1 #dtPostingDate_B-1Img
            {
                display: none;
            }

        .calendar-icon
        {
                right: 18px !important;
        }

        /*select#ddlInventory
        {
            -webkit-appearance: auto;
        }*/

        .simple-select::after
        {
            top: 6px !important;
            right: -2px !important;
        }

        .col-sm-3 , .col-md-3 , .col-md-2{
            margin-bottom: 5px;
        }

        #rdl_Salesquotation
        {
            margin-top: 10px;
        }
        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }
        .lblmBot4 > span, .lblmBot4 > label
        {
                margin-bottom: 0px !important;
        }

        #drdTransCategory.aspNetDisabled {
    background: #f3f3f3 !important;
}

       /* #CustomerTableTbl.dynamicPopupTbl>tbody>tr>td
        {
            width: 33.33%;
        }*/

       .lblmTop8>span, .lblmTop8>label
        {
                margin-top: 0 !important;
        }

       input + label
       {
               margin-top: 3px;
               margin-right: 5px;
       }

            @media only screen and (max-width: 1380px) and (min-width: 1300px)
            {

                .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
                    padding-right: 10px;
                    padding-left: 10px;
                }

                /*.simple-select::after
                {
                    right: 8px !important;
                }*/
                .calendar-icon {
                    right: 13px !important;
                }

                input[type="radio"], input[type="checkbox"] {
                    margin-right: 0px;
                }
            }
        </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <label id="TxtHeaded">Add Customer Receipt</label>
        </h3>
        <div id="pageheaderContent" class=" pull-right wrapHolder content horizontal-images" style="width: auto !important" runat="server">
            <div class="Top clearfix">
                <ul>
                    <li>
                        <div class="lblHolder" id="divContactPhone" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>Contact Person's Phone</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblContactPhone" runat="server" Text="N/A" CssClass="classout"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="divGSTIN" style="display: none;" runat="server">
                            <table>
                                <tr>
                                    <td>GST Registed?</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGSTIN" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder" id="custBal" runat="server">
                            <table>
                                <tr>
                                    <td>Customer Balance</td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#" id="idOutstanding">
                                            <asp:Label ID="lblOutstanding" runat="server" ToolTip="Click here to show details."></asp:Label>

                                        </a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>


        <div id="ApprovalCross" runat="server" class="crossBtn"><a href="CustomerReceiptPaymentList.aspx"><i class="fa fa-times"></i></a></div>


        <%--        <div class="tab">
            <input type="button" class="tablinks" value="General" onclick="ShowHideTab(event, 'General'); return false" />
            <input type="button" class="tablinks" value="Billing/Shipping" onclick="ShowHideTab(event, 'BS'); return false" />
        </div>--%>

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
        <TabPages>
            <dxe:TabPage Name="General" Text="General">
                <ContentCollection>
                    <dxe:ContentControl runat="server">
                        <div id="General" class="tabcontent" style="display: block;">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="">
                                        <div style=" padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px;">
                                            <div class="col-md-2" id="divNumberingScheme" runat="server">
                                                <label style="">Numbering Scheme</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="CmbScheme" ClientInstanceName="cCmbScheme"
                                                        SelectedIndex="0" EnableCallbackMode="false"
                                                        TextField="SchemaName" ValueField="ID"
                                                        runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                        <ClientSideEvents SelectedIndexChanged="CmbScheme_ValueChange"></ClientSideEvents>
                                                    </dxe:ASPxComboBox>
                                                    <span id="MandatoryNumberingScheme" class="iconNumberScheme pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <label style="">Document No.</label>
                                                <div>

                                                    <dxe:ASPxTextBox runat="server" ID="txtVoucherNo" ClientInstanceName="ctxtVoucherNo" MaxLength="16" Text="Auto" ClientEnabled="false">
                                                    </dxe:ASPxTextBox>

                                                    <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; margin-top: -21px; margin-left: 169px; display: none"
                                                        title="Mandatory"></span>

                                                </div>
                                            </div>


                                            <div class="col-md-2">
                                                <label style="">Posting Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="dtTDate" runat="server" ClientInstanceName="cdtTDate" EditFormat="Custom" AllowNull="false"
                                                        Font-Size="12px" UseMaskBehavior="True" Width="100%" EditFormatString="dd-MM-yyyy" CssClass="pull-left">
                                                        <ButtonStyle Width="13px"></ButtonStyle>
                                                        <ClientSideEvents LostFocus="function(s, e) { SetLostFocusonDemand(e)}"></ClientSideEvents>
                                                    </dxe:ASPxDateEdit>
                                                    <span id="MandatoryTransDate" class="iconTransDate  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                    <%--Rev 1.0--%>
                                                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                                                    <%--Rev end 1.0--%>
                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>For Unit <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch"
                                                        Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="ddlBranch_Change" />
                                                    </dxe:ASPxComboBox>
                                                    <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>


                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8 relative">
                                                <label class="btn-block" style="margin-bottom: 0;">
                                                    <%--<dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">--%>
                                                    <%--</dxe:ASPxLabel>--%>

                                                    <asp:RadioButtonList ID="rdl_Contact" runat="server" RepeatDirection="Horizontal" onchange="return selectContactValue();" CssClass="pull-left">

                                                        <asp:ListItem Text="Customer" Value="CL" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Vendor" Value="DV" Enabled="false"></asp:ListItem>
                                                    </asp:RadioButtonList>

                                                    <% if (rights.CanAdd && hdAddEdit.Value != "Edit")
                                                       { %>
                                                    <a href="#" onclick="AddcustomerClick()" style="position: absolute; top: 2px; margin-left: 8px; font-size: 16px;"><i id="openlink" class="fa fa-plus-circle" aria-hidden="true"></i></a>
                                                    <% } %>

                                                    <span style="color: red">*</span>
                                                    <% if (false)
                                                       { %>
                                                    <i id="openlink" class="fa fa-plus-circle ml5" aria-hidden="true"></i>

                                                    <% 
                                                       } 
                                                    %>
                                                </label>



                                                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>
                                                <span id="MandatorysCustomer" class="iconCustomer pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>Contact Person </label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlContactPerson" runat="server" ClientInstanceName="cddlContactPerson" Width="100%">
                                                    </dxe:ASPxComboBox>


                                                </div>
                                            </div>
                                            <div class="clear"></div>

                                            <div class="col-md-2 lblmTop8" id="tdCashBankLabel">
                                                <label>Cash Bank <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientInstanceName="cddlCashBank" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="CashBank_SelectedIndexChanged" />
                                                    </dxe:ASPxComboBox>

                                                    <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                                                </div>
                                            </div>


                                            <div class="col-md-2 lblmTop8">
                                                <label>Currency <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="ddlCurrency" runat="server" ClientInstanceName="cddlCurrency" Width="100%">
                                                        <ClientSideEvents SelectedIndexChanged="Currency_Rate" />
                                                    </dxe:ASPxComboBox>


                                                </div>
                                            </div>

                                            <div class="col-md-2 lblmTop8">
                                                <label>Rate <span style="color: red">*</span></label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtRate" runat="server" ClientInstanceName="ctxtRate" Width="100%" ClientEnabled="false" Text="0.00">
                                                    </dxe:ASPxTextBox>


                                                </div>
                                            </div>


                                            <div id="multipleredio" class="col-md-2" runat="server">
                                                <div style="padding-top: 14px; margin-top: 10px">
                                                    <asp:RadioButtonList ID="rdl_MultipleType" runat="server" Width="160px" RepeatDirection="Horizontal" onchange="return selectValue();">
                                                        <asp:ListItem Text="Single" Value="S" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Multiple" Value="M"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>

                                            <div class="clear" id="ClearSingle" style="display: none; !important"></div>
                                            <div id="singletype" runat="server">

                                                <div class="col-md-2 lblmTop8">
                                                    <label style="">Instrument Type</label>
                                                    <div style="" class="simple-select">
                                                        <dxe:ASPxComboBox ID="cmbInstrumentType" runat="server" ClientInstanceName="cComboInstrumentTypee" Font-Size="12px"
                                                            ValueType="System.String" Width="100%" EnableIncrementalFiltering="True" Native="true">
                                                            <Items>

                                                                <dxe:ListEditItem Text="Cheque" Value="C" Selected />
                                                                <dxe:ListEditItem Text="Draft" Value="D" />
                                                                <dxe:ListEditItem Text="E.Transfer" Value="E" />
                                                                <dxe:ListEditItem Text="Cash" Value="CH" />
                                                                <dxe:ListEditItem Text="Card" Value="CRD" />
                                                                 <dxe:ListEditItem Text="Pay Order" Value="PO" />
                                                            </Items>
                                                            <ClientSideEvents SelectedIndexChanged="InstrumentTypeSelectedIndexChanged" />
                                                        </dxe:ASPxComboBox>

                                                    </div>
                                                </div>

                                                <div class="col-md-2 lblmTop8" id="divInstrumentNo" style="" runat="server">
                                                    <label id="" style="">Instrument No</label>
                                                    <div id="">
                                                        <dxe:ASPxTextBox runat="server" ID="txtInstNobth" ClientInstanceName="ctxtInstNobth" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                                <div class="col-md-2 lblmTop8" id="tdIDateDiv" style="" runat="server">
                                                    <label id="tdIDateLable" style="">Instrument Date</label>
                                                    <div id="tdIDateValue" style="">
                                                        <dxe:ASPxDateEdit ID="InstDate" runat="server" EditFormat="Custom" ClientInstanceName="cInstDate"
                                                            UseMaskBehavior="True" Font-Size="12px" Width="100%" EditFormatString="dd-MM-yyyy">
                                                            <ClientSideEvents GotFocus="PutDate" />
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3" id="divDrawnOn" style="" runat="server">
                                                    <label id="" style="">Drawn on ( Party banker’s name )</label>
                                                    <div id="">
                                                        <dxe:ASPxTextBox runat="server" ID="txtDrawnOn" ClientInstanceName="ctxtDrawnOn" Width="100%" MaxLength="30">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>

                                            </div>


                                            <div class="col-md-5 lblmTop8">
                                                <label>Narration </label>
                                                <div>
                                                    <asp:TextBox ID="txtNarration" runat="server" MaxLength="500"
                                                        TextMode="MultiLine"
                                                        Width="100%"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2 lblmTop8">
                                                <label>Voucher Amount <span style="color: red">*</span> </label>
                                                <div>
                                                    <dxe:ASPxTextBox ID="txtVoucherAmount" runat="server" ClientInstanceName="ctxtVoucherAmount" Width="100%" CssClass="pull-left">
                                                        <ClientSideEvents TextChanged="function(s, e) { GetInvoiceMsg(e)}" LostFocus="lostFocusVoucherAmount" />
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="MandatoryVoucherAmount" class="iconVoucherAmount pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                                </div>
                                            </div>

                                            <div class="clear"></div>
                                            <div id="dvGST">

                                                <div class="col-md-2" id="ProductGSTApplicableSection" runat="server" style="margin-top: 25px;">
                                                    <asp:CheckBox ID="CB_GSTApplicable" runat="server" Text="GST Applicable" TextAlign="Right" Checked="false" onclick="return CheckedChange();"></asp:CheckBox>
                                                </div>

                                                <div class="col-md-4">
                                                    <label>Select Product</label>
                                                    <dxe:ASPxButtonEdit ID="btnProduct" runat="server" ReadOnly="true" ClientInstanceName="cbtnProduct" Width="100%" ClientEnabled="false">
                                                        <Buttons>
                                                            <dxe:EditButton>
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){btnProduct_KeyDown(s,e);}" />
                                                    </dxe:ASPxButtonEdit>

                                                </div>

                                                <div class="col-md-2">
                                                    <%--lblmTop8--%>
                                                    <label id="lblProject" runat="server">Project</label>
                                                    <dxe:ASPxGridLookup ID="lookup_Project" runat="server" ClientInstanceName="clookup_Project" DataSourceID="LinqServerModeDataSourceProject"
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
                                                            <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="false" VisibleIndex="5" Caption="" Settings-AutoFilterCondition="Contains" Width="200px">
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
                                                        <ClientSideEvents  CloseUp="clookup_Project_LostFocus" GotFocus="cProject_GotFocus" />
                                                       <%-- <ClearButton DisplayMode="Always"></ClearButton>--%>
                                                    </dxe:ASPxGridLookup>
                                               <%--     ClientSideEvents-TextChanged="ProjectCodeinlineSelected"--%>
                                                <%--    GotFocus="function(s,e){clookup_Project.ShowDropDown();}"--%>
                                                   <%-- ValueChanged="ProjectValueChange" --%>
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceProject" runat="server" OnSelecting="LinqServerModeDataSourceProject_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext1" TableName="ProjectCodeBind" />

                                                </div>
                                                <div class="col-md-4">
                                                    <label id="lblHierarchy" runat="server">Hierarchy</label>
                                                    <asp:DropDownList ID="ddlHierarchy" runat="server" Width="100%" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Ref. Proforma </label>
                                                <%-- <div>
                                                    <dxe:ASPxComboBox ID="ddlProformaInvoice" runat="server" ClientInstanceName="cddlProformaInvoice" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>--%>
                                                <dxe:ASPxCallbackPanel runat="server" ID="CBPProformaInvoice" ClientInstanceName="cCBPProformaInvoice" OnCallback="CBPProformaInvoice_Callback">
                                                    <PanelCollection>
                                                        <dxe:PanelContent runat="server">
                                                            <dxe:ASPxGridLookup ID="lookup_ProformaInvoice" SelectionMode="Single" runat="server" ClientInstanceName="clookup_ProformaInvoice"
                                                                OnDataBinding="lookup_ProformaInvoice_DataBinding"
                                                                KeyFieldName="Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">

                                                                <Columns>
                                                                    <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />--%>

                                                                    <dxe:GridViewDataColumn FieldName="Quote_Number" Visible="true" VisibleIndex="1" Caption="Doc No." Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="Quote_Date" Visible="true" VisibleIndex="2" Caption="Doc Date" Width="150" Settings-AutoFilterCondition="Contains" >
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>                                                                   
                                                                    <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="4" Caption="Type" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                    <dxe:GridViewDataColumn FieldName="CreatedBy" Visible="true" VisibleIndex="5" Caption="Entered By" Width="150" Settings-AutoFilterCondition="Contains">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                    </dxe:GridViewDataColumn>
                                                                </Columns>
                                                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                                    <Templates>
                                                                        <StatusBar>
                                                                            <table class="OptionsTable" style="float: right">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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
                                                               <%-- <ClientSideEvents ValueChanged="function(s, e) { QuotationNumberChanged();}" />
                                                                <ClientSideEvents GotFocus="function(s,e){gridquotationLookup.ShowDropDown();}" />--%>
                                                            </dxe:ASPxGridLookup>

                                                        </dxe:PanelContent>
                                                    </PanelCollection>
                                                <%--    <ClientSideEvents EndCallback="componentEndCallBack" />--%>
                                                </dxe:ASPxCallbackPanel>

                                            </div>
                                                    <div class="clear"></div>
                                            <div class="col-md-2  " id="DivSegment1" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="Segment1">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment1" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment1ButnClick();}" KeyDown="function(s,e){Segment1_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="col-md-2  " id="DivSegment2" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel15" runat="server" Text="Segment2">
                                                </dxe:ASPxLabel>
                                                <dxe:ASPxButtonEdit ID="txtSegment2" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment2" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment2ButnClick();}" KeyDown="function(s,e){Segment2_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>
                                            </div>
                                            <div class="col-md-2  " id="DivSegment3" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="Segment3">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment3" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment3" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment3ButnClick();}" KeyDown="function(s,e){Segment3_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="col-md-2  " id="DivSegment4" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="Segment4">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment4" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment4" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment4ButnClick();}" KeyDown="function(s,e){Segment4_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="col-md-2  " id="DivSegment5" runat="server">
                                                <dxe:ASPxLabel ID="ASPxLabel18" runat="server" Text="Segment5">
                                                </dxe:ASPxLabel>

                                                <dxe:ASPxButtonEdit ID="txtSegment5" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment5" Width="100%" TabIndex="5">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){Segment5ButnClick();}" KeyDown="function(s,e){Segment5_KeyDown(s,e);}" />
                                                </dxe:ASPxButtonEdit>

                                            </div>
                                            <div class="clear"></div>
                                            <div class="clear"></div>

                                            <div id="Multipletype" style="display: none" class="bgrnd" runat="server">
                                                <uc1:ucPaymentDetails runat="server" ID="PaymentDetails" />
                                            </div>


                                            <div class="clear"></div>
                                        </div>
                                        <dxe:ASPxGridView runat="server" ClientInstanceName="grid" ID="grid"
                                            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                                            SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                                            OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback"
                                            OnRowInserting="Grid_RowInserting"
                                            OnRowUpdating="Grid_RowUpdating"
                                            OnRowDeleting="Grid_RowDeleting"
                                            OnBatchUpdate="grid_BatchUpdate"
                                            OnDataBinding="grid_DataBinding"
                                            KeyFieldName="ReceiptDetail_ID"
                                            Settings-VerticalScrollableHeight="170" CommandButtonInitialize="false" Settings-ShowStatusBar="Hidden">
                                            <SettingsPager Visible="false"></SettingsPager>
                                            <Styles>
                                                <StatusBar CssClass="statusBar">
                                                </StatusBar>
                                            </Styles>
                                            <Columns>
                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>
                                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl#" ReadOnly="true" VisibleIndex="1" Width="5%">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="Type" Caption="Type" VisibleIndex="2">

                                                    <PropertiesButtonEdit>

                                                        <ClientSideEvents ButtonClick="TypeButnClick" KeyDown="TypeKeyDown" />
                                                        <Buttons>

                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataButtonEditColumn ReadOnly="true" FieldName="DocumentNo" Caption="Document No." VisibleIndex="3">

                                                    <PropertiesButtonEdit>

                                                        <ClientSideEvents ButtonClick="DocumentNoButnClick" KeyDown="DocumentNoKeyDown" />
                                                        <Buttons>

                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Receipt" ReadOnly="false" FieldName="Receipt" Width="130">
                                                    <PropertiesTextEdit Style-HorizontalAlign="Right">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                                        <ClientSideEvents LostFocus="Receipt_LostFocus"></ClientSideEvents>
                                                        <ValidationSettings RequiredField-IsRequired="false" Display="None"></ValidationSettings>

                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <%--  Chinmoy Added new column Project Code Start 13-12-2019--%>

                                                <dxe:GridViewDataButtonEditColumn FieldName="Project_Code" Caption="Project Code" VisibleIndex="5" Width="14%">
                                                    <PropertiesButtonEdit>
                                                        <ClientSideEvents ButtonClick="ProjectCodeButnClick" KeyDown="ProjectCodeKeyDown" LostFocus="Project_Code_LostFocus" />
                                                        <Buttons>
                                                            <dxe:EditButton Text="..." Width="20px">
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                    </PropertiesButtonEdit>
                                                </dxe:GridViewDataButtonEditColumn>


                                                <%-- End--%>






                                                <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Remarks" ReadOnly="false" FieldName="Remarks" Width="200">
                                                    <PropertiesTextEdit>
                                                    </PropertiesTextEdit>
                                                    <CellStyle Wrap="False" HorizontalAlign="Left" CssClass="gridcellleft"></CellStyle>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn FieldName="ReceiptDetail_ID" Caption="Srl No" ReadOnly="true" Width="0">
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ProjectId" Caption="ProjectId" VisibleIndex="16" ReadOnly="True" Width="0"
                                                    EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide"
                                                    PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="IsOpening" Caption="hidden Field Id" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="ActualAmount" Caption="hidden Field Id" VisibleIndex="17" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="TypeId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="DocId" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn FieldName="UpdateEdit" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" Width="0">
                                                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="2.5%" VisibleIndex="6" Caption=" ">
                                                    <CustomButtons>
                                                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                                                        </dxe:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxe:GridViewCommandColumn>


                                            </Columns>



                                            <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="GetVisibleIndex" />
                                            <SettingsDataSecurity AllowEdit="true" />
                                            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                                            </SettingsEditing>
                                        </dxe:ASPxGridView>
                                        <div class="clear"></div>
                                        <div class="text-center">
                                            <table style="margin-left: 30%; margin-top: 10px">
                                                <tr>
                                                    <td style="padding-right: 50px"><b>Total Amount</b></td>
                                                    <td style="width: 203px;">
                                                        <dxe:ASPxTextBox ID="txtTotalAmount" runat="server" Width="105px" ClientInstanceName="c_txt_Debit" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">

                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr style="display: none">
                                                    <td style="padding-right: 30px">
                                                        <asp:Label ID="lbltaxAmountHeader" runat="server" Text="Total Taxable Amount" Font-Bold="true"></asp:Label></td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="txtTaxAmount" runat="server" Width="105px" ClientInstanceName="ctxtTaxAmount" HorizontalAlign="Right" Font-Size="12px" ReadOnly="true">
                                                            <MaskSettings Mask="&lt;0..999999999999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="content reverse horizontal-images clearfix" style="margin-top: 8px; width: 100%; margin-right: 0; padding: 8px; height: auto; border-top: 1px solid #ccc; border-bottom: 1px solid #ccc; border-radius: 0;">
                                                <ul>
                                                    <li class="clsbnrLblTaxableAmt" style="float: right;">
                                                        <div class="horizontallblHolder">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Running Balance" ClientInstanceName="cbnrLblInvVal" />
                                                                        </td>
                                                                        <td>
                                                                            <strong>
                                                                                <dxe:ASPxLabel ID="lblRunningBalanceCapsulCrp" runat="server" Text="0.00" ClientInstanceName="clblRunningBalanceCapsul" />
                                                                            </strong>
                                                                        </td>
                                                                    </tr>

                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div id="divSubmitButton" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="padding: 5px 0px;">
                                                        <span id="tdSaveButtonNew" runat="server">
                                                            <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                                AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="S&#818;ave & New"
                                                                UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {SaveButtonClickNew();}" />
                                                            </dxe:ASPxButton>
                                                        </span>
                                                        <span id="tdSaveButton" runat="server">
                                                            <dxe:ASPxButton ID="btnSaveRecords" ClientInstanceName="cbtnSaveRecords" runat="server"
                                                                AutoPostBack="false" CssClass="btn btn-success" TabIndex="0" Text="Save & Ex&#818;it"
                                                                UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                            </dxe:ASPxButton>
                                                        </span>
                                                        <dxe:ASPxButton ID="btnSaveUdf" ClientInstanceName="cbtn_SaveUdf" runat="server" AutoPostBack="False" Text="U&#818;DF" UseSubmitBehavior="False"
                                                            CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1">
                                                            <ClientSideEvents Click="function(s, e) {OpenUdf();}" />
                                                        </dxe:ASPxButton>
                                                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="T&#818;ax & Charges" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {Save_TaxesClick();}" />
                                                        </dxe:ASPxButton>
                                                        <dxe:ASPxButton ID="ASPxButton6" ClientInstanceName="cbtn_TCS" runat="server" AutoPostBack="False" Text="Add TC&#818;S" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                                            <ClientSideEvents Click="function(s, e) {ShowTCS();}" />
                                                        </dxe:ASPxButton>
                                                        <b><span id="tagged" runat="server" style="display: none; color: red">This Customer Receipt Payment is tagged in other modules. Cannot Modify data except UDF</span></b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>




                                    </div>
                                </div>
                            </div>

                        </div>
                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
            <dxe:TabPage Name="Billing/Shipping" Text="Billing/Shipping">
                <ContentCollection>
                    <dxe:ContentControl runat="server">


                        <ucBS:Sales_BillingShipping runat="server" ID="Sales_BillingShipping" />

                    </dxe:ContentControl>
                </ContentCollection>
            </dxe:TabPage>
        </TabPages>

        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab = page.GetActiveTab();
	                                            var Tab0 = page.GetTab(0);
	                                            var Tab1 = page.GetTab(1);
	                                            var Tab2 = page.GetTab(2);
                                                 
                                               if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                           else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }


	                                            }"></ClientSideEvents>

    </dxe:ASPxPageControl>

    </div>



    <%--Modal Section Start--%>

    <div class="modal fade" id="TypeModal" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="CloseSubModal();">&times;</button>
                    <h4 class="modal-title">Select Type</h4>
                </div>
                <div class="modal-body">
                    <div id="TypeTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Type</th>

                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseSubModal();">Close</button>
                </div>
            </div>

        </div>
    </div>


    <div class="modal fade" id="TaxModal" role="dialog" data-backdrop="static"
        data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Tax</h4>
                </div>
                <div class="modal-body">
                    <div id="TaxTable">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="CloseTaxModal();">Close</button>
                </div>
            </div>

        </div>
    </div>




    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="CustModel" role="dialog">--%>
    <div class="modal fade" id="CustModel" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search by Entity Name,Unique Id and Phone No." />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
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




    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="DocModel" role="dialog">--%>
    <div class="modal fade" id="DocModel" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title">Select Document</h4>
                </div>
                <div class="modal-body" style="overflow-y: auto;">
                    <input type="text" onkeydown="DocNewkeydown(event)" id="txtDocSearch" autofocus="autofocus" width="100%" placeholder="Search by Document Number" />

                    <div id="DocTable">
                        <table border="1" width="100%" class="table table-hover">
                            <tr class="HeaderStyle">
                                <th>Document Number</th>
                                <th>Document Date</th>
                                <th>Document Unit</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeDocModal();">Close</button>
                </div>
            </div>
        </div>
    </div>




    <%--Customer Popup--%>
    <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="750px"
        Width="1020px" HeaderText="Add New Customer" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <%-- <HeaderTemplate>
            <span>Add New Customer</span>
        </HeaderTemplate>--%>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--Tax PopUP Start--%>

    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="ProdModel" role="dialog">--%>
    <div class="modal fade" id="ProdModel" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
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

                            <tr class="HeaderStyle" style="font-size: small">
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
                    <button type="button" class="hide" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>





    <%--Modal Section End--%>

    <%--Chinmoy added inline Project code start 13-12-2019--%>

    <dxe:ASPxPopupControl ID="ProjectCodePopup" runat="server" ClientInstanceName="cProjectCodePopup"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="400"
        Width="700" HeaderText="Select Document Number" AllowResize="true" ResizingMode="Postponed" Modal="true">
        <%--  <headertemplate>
                <span>Select Document Number</span>
            </headertemplate>--%>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <label><strong>Search By Document Number</strong></label>
                <%--   <span style="color: red;">[Press ESC key to Cancel]</span>--%>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectCodeCallback" ClientInstanceName="cProjectCodeCallback"
                    OnCallback="ProjectCodeCallback_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookupPopup_ProjectCode" runat="server" ClientInstanceName="clookupPopup_ProjectCode" Width="800"
                                KeyFieldName="ProjectId" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProjectCodeSelected"
                                ClientSideEvents-KeyDown="lookup_ProjectCodeKeyDown" OnDataBinding="lookup_ProjectCode_DataBinding">
                                <Columns>

                                    <%--   <dxe:GridViewDataColumn FieldName="Proj_Id" Visible="true" VisibleIndex="0" Caption="Project id" Settings-AutoFilterCondition="Contains" Width="200px">
                                                    </dxe:GridViewDataColumn>--%>
                                    <dxe:GridViewDataColumn FieldName="ProjectCode" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
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
                                                    <td>
                                                        <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                            <%--   <dx:LinqServerModeDataSource ID="EntityServerModeDataProjectQuotation" runat="server" OnSelecting="EntityServerModeDataProjectQuotation_Selecting"
                                                ContextTypeName="ERPDataClassesDataContext" TableName="ProjectCodeBind" />--%>
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="ProjectCodeCallback_endcallback" />
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
    </dxe:ASPxPopupControl>



    <%--//End--%>





    <%--Customer balance--%>
    <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cOutstandingPopup"
        Width="1300px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Customer Outstanding</span></strong>

            <dxe:ASPxImage ID="ASPxImage2" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cOutstandingPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <dxe:ASPxGridView runat="server" KeyFieldName="SLNO" ClientInstanceName="cCustomerOutstanding" ID="CustomerOutstanding"
                    DataSourceID="EntityServerModeDataSource" OnSummaryDisplayText="ShowGridCustOut_SummaryDisplayText"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="cCustomerOutstanding_CustomCallback"
                    Settings-ShowFooter="true" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible"
                    OnHtmlFooterCellPrepared="ShowGridCustOut_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustOut_HtmlDataCellPrepared">

                    <SettingsBehavior AllowDragDrop="true" AllowSort="true"></SettingsBehavior>
                    <SettingsPager Visible="true"></SettingsPager>
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="PARTYNAME" GroupIndex="0"
                            VisibleIndex="0">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="55%" ReadOnly="true" Caption="UNIT">
                        </dxe:GridViewDataTextColumn>
                        <%--<dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PARTYNAME" Width="100" ReadOnly="true" Caption="Customer">
                                </dxe:GridViewDataTextColumn>--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" ReadOnly="true" Caption="Doc. Type" Width="100%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" ReadOnly="true" Caption="Opening?" Width="30%">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="DOC_NO" ReadOnly="true" Width="95%" Caption="Document No">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_DATE" Width="50%" ReadOnly="true" Caption="Document Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DUE_DATE" Width="50%" ReadOnly="true" Caption="Due Date">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOC_AMOUNT" ReadOnly="true" Caption="Document Amt." Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="BAL_AMOUNT" ReadOnly="true" Caption="Balance Amount" Width="50%">
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DAYS" Width="20%" ReadOnly="true" Caption="Days">
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" AllowSort="False" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" />
                    <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    </SettingsPager>
                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType" />
                        <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt"></dxe:ASPxSummaryItem>
                    </TotalSummary>

                    <SettingsDataSecurity AllowEdit="true" />

                </dxe:ASPxGridView>

                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                    ContextTypeName="ERPDataClassesDataContext" TableName="PARTYOUTSTANDINGDET_REPORT" />
                <div style="display: none">
                    <dxe:ASPxGridViewExporter ID="exporter1" GridViewID="CustomerOutstanding" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>

    <%--Hidden Feild Section--%>

    <asp:HiddenField runat="server" ID="hdnEnterBranch" />
    <asp:HiddenField runat="server" ID="hdnProductId" />
    <asp:HiddenField runat="server" ID="hdtHsnCode" />
    <asp:HiddenField runat="server" ID="hdnInstrumentType" />
    <asp:HiddenField runat="server" ID="hdnCustomerId" />
    <div runat="server" id="hdnCompany" class="hide"></div>
    <div runat="server" id="hdnLocalCurrency" class="hide"></div>
    <div runat="server" id="ReceiptPaymentId" class="hide"></div>
    <asp:HiddenField runat="server" ID="hdAddEdit" />
    <asp:HiddenField runat="server" ID="SysSetting" />
    <asp:HiddenField runat="server" ID="hdnRefreshType" />
    <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnHSNId" runat="server" />
    <div runat="server" id="jsonProducts" class="hide"></div>
    <asp:HiddenField runat="server" ID="IsUdfpresent" />
    <asp:HiddenField runat="server" ID="Keyval_internalId" />
    <asp:HiddenField runat="server" ID="DoEdit" />
    <asp:HiddenField runat="server" ID="hddnBranchId" />
    <asp:HiddenField runat="server" ID="hddnAsOnDate" />
    <asp:HiddenField runat="server" ID="hddnOutStandingBlock" />
    <asp:HiddenField runat="server" ID="ISAllowBackdatedEntry" />
    <asp:HiddenField runat="server" ID="warehousestrProductID" />
    <asp:HiddenField ID="hdnProjectSelectInEntryModule" runat="server" />
     <asp:HiddenField ID="hdnAllowProjectInDetailsLevel" runat="server" />
      <asp:HiddenField ID="hdnEditProjId" runat="server" />
     <%--Rev Work Date:-21.03.2022 -Copy Function add--%>
                    <asp:HiddenField ID="hrCopy" runat="server" />  <%--Copy Mode--%>
                     <%--Close of Rev Work Date:-21.03.2022 -Copy Function add--%>
    <%--for Project  --%>
    <%--End Hidden Feild--%>

    <%-- 20-05-2019 Surojit --%>
    <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    <%-- 20-05-2019 Surojit --%>

    <dxe:ASPxLoadingPanel ID="LoadingPanelCRP" runat="server" ClientInstanceName="cLoadingPanelCRP"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <asp:HiddenField ID="hdnProjectMandatory" runat="server" />

    <%--Rev 2.0--%>
    <%--<div id="tcsModal" class="modal fade" role="dialog">--%>
    <div id="tcsModal" class="modal fade" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">TCS Calculation</h4>
      </div>
      <div class="modal-body">
        <div class="row">
            <div class="col-md-3">
                <label>
                    TCS Section
                </label>
                <dxe:ASPxTextBox runat="server" ClientEnabled="false" ID="txtTCSSection" ClientInstanceName="ctxtTCSSection">

                </dxe:ASPxTextBox> 
            </div>
            <div class="col-md-3">
                <label>
                    TCS Applicable Amount
                </label>
                <dxe:ASPxTextBox runat="server" ClientEnabled="true" ID="txtTCSapplAmount" ClientInstanceName="ctxtTCSapplAmount">
                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                </dxe:ASPxTextBox> 
            </div>

            <div class="col-md-3">
                <label>
                    TCS Percentage
                </label>
                <dxe:ASPxTextBox runat="server" ClientEnabled="true" ID="txtTCSpercentage" ClientInstanceName="ctxtTCSpercentage">
                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" AllowMouseWheel="false" />
                </dxe:ASPxTextBox> 
            </div>

            <div class="col-md-3">
                <label>
                    TCS Amount
                </label>
                <dxe:ASPxTextBox runat="server" ClientEnabled="true" ID="txtTCSAmount" ClientInstanceName="ctxtTCSAmount">
                    <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                </dxe:ASPxTextBox> 
            </div>
            
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-success" data-dismiss="modal">Save</button>
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

<asp:HiddenField ID="hdnLockFromDate" runat="server" />
<asp:HiddenField ID="hdnLockToDate" runat="server" />
<asp:HiddenField ID="hdnLockFromDateCon" runat="server" />
<asp:HiddenField ID="hdnLockToDateCon" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />

    <asp:HiddenField runat="server" ID="hdnValueSegment1" />
    <asp:HiddenField runat="server" ID="hdnValueSegment2" />
    <asp:HiddenField runat="server" ID="hdnValueSegment3" />
    <asp:HiddenField runat="server" ID="hdnValueSegment4" />
    <asp:HiddenField runat="server" ID="hdnValueSegment5" />
    <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="Segment1Model" role="dialog">--%>
    <div class="modal fade" id="Segment1Model" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment1header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment1keydown(event)" id="txtSegment1Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment1Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="Segment2Model" role="dialog">--%>
    <div class="modal fade" id="Segment2Model" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment2Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment2Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="Segment3Model" role="dialog">--%>
    <div class="modal fade" id="Segment3Model" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment3Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment3keydown(event)" id="txtSegment3Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment3Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <%--Rev 2.0--%>
    <%--<div class="modal fade" id="Segment4Model" role="dialog">--%>
    <div class="modal fade" id="Segment4Model" role="dialog" data-backdrop="static" data-keyboard="false">
        <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment4Header">Segment4 Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment4keydown(event)" id="txtSegment4Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment4Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    <%--Rev 2.0--%>
   <%-- <div class="modal fade" id="Segment5Model" role="dialog">--%>
    <div class="modal fade" id="Segment5Model" role="dialog" data-backdrop="static" data-keyboard="false">
    <%--Rev end 2.0--%>
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment5Header"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Segment5keydown(event)" id="txtSegment5Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />
                    <div id="Segment5Table">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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

</asp:Content>
