<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                10-04-2023        V2.0.37           Pallab              25959: Invoice cum Challan with SO module design modification
2.0                28/11/2023        V2.0.41          Priti               0027028: Customer code column is required in the listing module of Sales entry
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="InvoiceCumChallanSO.aspx.cs" Inherits="ERP.OMS.Management.Activities.InvoiceCumChallanSO" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.1.0/jquery-confirm.min.css">--%>
    <link href="../../../assests/css/custom/jquery.confirm.css" rel="stylesheet" />
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script src="JS/InvoiceCumChallanSO.js?v=3.1"></script>
    <link href="CSS/SalesOrderEntityList.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        $(document).ready(function () {
            $("#expandCgvPurchaseOrder").click(function (e) {
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


                    CgvPurchaseOrder.SetHeight(browserHeight - 150);
                    CgvPurchaseOrder.SetWidth(cntWidth);
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


                    CgvPurchaseOrder.SetHeight(450);

                    var cntWidth = $this.parent('.makeFullscreen').width();
                    CgvPurchaseOrder.SetWidth(cntWidth);

                }

            });
        });

        function MakeInvoice(keyValue, RegisterCust, visibleIndex) {
            //  debugger;

            var status=true;
             $.ajax({
                type: "POST",
                url: "InvoiceCumChallanSO.aspx/GetSalesOrderStatusInvoice",
                data: "{'OrderId':'" + keyValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    if( msg.d=="Yes"){
                    status=false;
                        jAlert("Invoice alredy gelerated.");
                        status=false;
                       return;
                    }
                   
                }
            });

            if (status) {
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
           // var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            //if (IsBalMapQtyExists.trim() != "0") {
            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[12].innerText;
            if (($("#hdnSalesinvSt").val() == "1"))//($("#hdnActinveInvst").val() == "1") || 
            {
                if (RegisterCust == "R") {
                   

                    if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                        var ActiveUser = '<%=Session["userid"]%>'
                    if (ActiveUser != null) {
                        $.ajax({
                            type: "POST",
                            url: "InvoiceCumChallanSO.aspx/GetEditablePermission",
                            data: "{'ActiveUser':'" + ActiveUser + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,//Added By:Subhabrata
                            success: function (msg) {
                                //debugger;
                                var status = msg.d;
                                var url = 'InvoiceDeliveryChallan.aspx?SalesOrderId=' + keyValue + '&Permission=' + status + '&type=SO' + '&Isformorder=Y';
                                window.location.href = url;
                            }
                        });
                    }
                  }
                else {
                    jAlert("Sales Order is " + IsCancelFlag.trim() + ".make in invoice is not allowed.");
                   }
                }
                else
                {
                    jAlert("Must select registered Customers to proceed with Einvoice.");
                }
            }
            else
            {
               

                if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                    var ActiveUser = '<%=Session["userid"]%>'
                        if (ActiveUser != null) {
                            $.ajax({
                                type: "POST",
                                url: "InvoiceCumChallanSO.aspx/GetEditablePermission",
                                data: "{'ActiveUser':'" + ActiveUser + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,//Added By:Subhabrata
                                success: function (msg) {
                                    //debugger;
                                    var status = msg.d;
                                    var url = 'InvoiceDeliveryChallan.aspx?SalesOrderId=' + keyValue + '&Permission=' + status + '&type=SO' + '&Isformorder=Y';
                                    window.location.href = url;
                                }
                            });
                        }
                    }
                    else {
                        jAlert("Sales Order is " + IsCancelFlag.trim() + ".make in invoice is not allowed.");
                    }
            }
            }
          }
        function ReadyToInvoice(id) {
            $("#btnFeedbackSaves").removeClass("hide");
            $("#hdnOrderID").val(id);
            cReadyToInvoicePopup.Show();
        }

        function CancelReadyToInvoice() {
            $("#hdnOrderID").val('');
            ctxtRemarks.SetValue();
            cReadyToInvoicePopup.Hide();
        }

        function ReadyToInvoice_save() {
            var Remarks = ctxtRemarks.GetText();
            var Stage = ccmbStage.GetValue();
            var Orderid = $("#hdnOrderID").val();

            if ($("#hdnOrderID").val() == '') {
                jAlert("Please select Order.");
                return;
            }

            if (Stage == 0) {
                jAlert("Please select stage.");
                return;
                ccmbStage.Focus();
            }

            if (Remarks == '') {
                jAlert("Please enter remarks.");
                return;
                ctxtRemarks.Focus();
            }

            $.ajax({
                type: "POST",
                url: "InvoiceCumChallanSO.aspx/InsertUpdateReadyToInvoice",
                data: JSON.stringify({ Orderid: Orderid, Stage: Stage, Remarks: Remarks }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == 'OK') {
                        jAlert("Saved successfully.");
                        $("#hdnOrderID").val('');
                        ctxtRemarks.SetValue();
                        ccmbStage.SetValue(0);
                        cReadyToInvoicePopup.Hide();
                        $.ajax({
                            type: "POST",
                            url: "InvoiceCumChallanSO.aspx/ButtonCountShow",
                            data: JSON.stringify({ FormDate: $("#hfFromDate").val(), toDate: $("#hfToDate").val(), Branch: $("#hfBranchID").val() }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                //alert(msg.d);
                                var status = msg.d;
                                //if (msg.d.All != null) {
                                    $("#spnInvoiceGeneratedlvl").html(msg.d.InvoiceGenerated);
                                    $("#spnPendingforInvoicelvl").html(msg.d.PendingforInvoice);
                                    $("#spnAlllvl").html(msg.d.All);
                                //}
                                //else {
                                //     $("#spnInvoiceGeneratedlvl").html(0);
                                //    $("#spnPendingforInvoicelvl").html(0);
                                //    $("#spnAlllvl").html(0);
                                //}
                            }
                        });
                        cGrdOrder.Refresh();
                    }
                    else {
                        jAlert("Please try again.");
                    }
                }
            });
        }

        function OnViewReadyforInvoice(Orderid) {

            $.ajax({
                type: "POST",
                url: "ApproveSaleasOrder.aspx/viewReadyToInvoice",
                data: JSON.stringify({ Orderid: Orderid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    //alert(msg.d);
                    var status = msg.d;
                    if (msg.d.Remarks != null) {
                        $("#hdnOrderID").val('');
                        ctxtRemarks.SetValue(msg.d.Remarks);
                        ccmbStage.SetValue(msg.d.Status);
                        cReadyToInvoicePopup.Show();
                        $("#btnFeedbackSaves").addClass("hide");
                    }
                }
            });
        }

    </script>

    <style>
        .plhead a {
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        .plhead a.collapsed .fa-minus-circle {
            display: none;
        }

        .blocklableTbl > tbody > tr > td > label {
            display: block;
            margin-bottom: 4px;
        }

            .blocklableTbl > tbody > tr > td > label > span {
                font-weight: 600;
            }

        .blocklableTbl > tbody > tr > td > select {
            margin: 0 !important;
        }

        .blocklableTbl > tbody > tr > td:not(:first-child) {
            padding-left: 25px;
        }

        .btnTD {
            min-width: 200px;
            padding-top: 19px;
        }

            .btnTD > .btn {
                margin: 0 !important;
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
            border-radius: 14px;
        }

        .btn .badge {
            background: #fff;
            color: #333;
            margin-left: 4px;
            font-size: 14px;
        }

        .btn-badge-color1:hover, .btn-badge-color2:hover, .btn-badge-color3:hover, .btn-badge-color4:hover, .btn-badge-color5:hover, .btn-badge-color6:hover,
        .btn-badge-color1:focus, .btn-badge-color2:focus, .btn-badge-color3:focus, .btn-badge-color4:focus, .btn-badge-color5:focus, .btn-badge-color6:focus {
            color: #fff;
            opacity: 0.8;
        }

        .btn-badge-color1 {
            /*background:chocolate;
            color:#fff;*/
            background: #d9c015;
            color: #fff;
        }

        .btn-badge-color2 {
            background: #30cb57;
            color: #fff;
        }

        .btn-badge-color3 {
            background: #ff6a00;
            color: #fff;
        }

        .btn-badge-color4 {
            background: #7a6fd6;
            color: #fff;
        }

        .btn-badge-color5 {
            background: #6272e2;
            color: #fff;
        }

        .btn-badge-color6 {
            background: #3366FF;
            color: #fff;
        }

        .nocursor {
            cursor: default;
        }

        .auto-style1 {
            height: 145px;
        }

        .m0 {
            margin-top: 0 !important;
        }

        .mBot0 {
            margin-bottom: 0 !important;
        }
    </style>

    <style>
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        
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

        /*.col-sm-3 , .col-md-3{
            margin-bottom: 10px;
        }*/

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

        /*Rev end 1.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
        Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">
                            <table width="100%">
                                <tr>
                                    <td style="padding-right: 20px">
                                        <label style="margin-bottom: 5px">Proforma</label>
                                    </td>
                                    <td>
                                        <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                    <td style="padding-right: 20px; padding-left: 8px">
                                        <label style="margin-bottom: 5px">Customer</label>
                                    </td>
                                    <td>
                                        <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td style="width: 70px; padding: 13px 0;">Status </td>
                            <td>
                                <asp:RadioButtonList ID="rbl_OrderStatus" runat="server" Width="250px" CssClass="mTop5" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Accepted" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                    </table>
                </div>
                <div class="clear"></div>
                <div class="col-md-12">

                    <div class="" style="margin-bottom: 5px;">
                        Reason 
                    </div>

                    <div>
                        <dxe:ASPxMemo ID="txt_OrderRemarks" runat="server" ClientInstanceName="cOrderRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                    </div>
                </div>

                <div class="col-md-12" style="padding-top: 10px;">
                    <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                        AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                    </dxe:ASPxButton>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Invoice cum Challan with SO</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>
                    <label>To </label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
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
        <div class="form_main">
        <div class="clearfix">
            <%-- <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>New</span> </a>
            <% } %>--%>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>

            <%--<button id="btnPrinted" class="btn btn-badge-color1 btn-radius" type="button" onclick="btn_AMaxLevelsClick(this);">Printed <span class="badge" id="spnPrintedlvl">0</span></button>--%>
            <button id="btnPending" data-toggle="tooltip" title="Click to see the pending for invoice details" class="btn btn-badge-color3" type="button" onclick="btn_PendingforInvoiceClick(this);">Pending for Invoice <span class="badge" id="spnPendingforInvoicelvl">0</span></button>
            <button id="btnReadyforInvoice" data-toggle="tooltip" title="Click to see the invoice generated details" class="btn btn-badge-color2" type="button" onclick="btn_InvoiceGeneratedClick(this);">Invoice generated <span class="badge" id="spnInvoiceGeneratedlvl">0</span></button>            
            <button id="btnAll" data-toggle="tooltip" title="Click to see the all details" class="btn btn-badge-color6" type="button" onclick="btn_AllClick(this);">All <span class="badge" id="spnAlllvl">0</span></button>

           
           
        </div>
    </div>
        <div class="GridViewArea relative">
        <div class="makeFullscreen ">
            <span class="fullScreenTitle">Invoice cum Challan with SO</span>
            <span class="makeFullscreen-icon half hovered " data-instance="cGrdOrder" title="Maximize Grid" id="expandCgvPurchaseOrder"><i class="fa fa-expand"></i></span>
            <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="False" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
                SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback"
                SettingsBehavior-AllowFocusedRow="true" 
                HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
                <SettingsSearchPanel Visible="True" Delay="5000" />
               <%-- OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared"--%>
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="OrderNo"
                        VisibleIndex="1" FixedStyle="Left" Width="140px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Order_Date"
                        VisibleIndex="2" Width="150px" Settings-ShowFilterRowMenu="True" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                     <%--Rev  2.0--%>
                      <dxe:GridViewDataTextColumn Caption="Customer Code" FieldName="CustomerCode" Width="160px"
                      VisibleIndex="3">
                      <CellStyle CssClass="gridcellleft" Wrap="true">
                      </CellStyle>
                      <Settings AutoFilterCondition="Contains" />
                     </dxe:GridViewDataTextColumn>
                    <%-- Rev  2.0 End--%>
                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                        VisibleIndex="4" Width="300px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BranchName"
                        VisibleIndex="5" Width="250px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name"
                        VisibleIndex="6" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                        VisibleIndex="7" Width="100" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="InvoiceNo" Caption="Invoice Details" VisibleIndex="8" Width="150px">
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("MultipleStatus")%>'>
                                <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='See Invoice'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>
                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("SingleStatus")%>'>

                                <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='See Invoice' ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>
                            <a href="javascript:void(0);" style='<%#Eval("NOInvoice")%>'>
                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='No Invoice'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>

                        </DataItemTemplate>
                        <EditFormSettings Visible="False" />
                        <CellStyle Wrap="False" CssClass="text-center">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AllowAutoFilter="False" />
                        <HeaderStyle Wrap="False" CssClass="text-center" />
                    </dxe:GridViewDataTextColumn>

                  <%--  <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PosState"
                        VisibleIndex="8" Width="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                        VisibleIndex="9" Width="80">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Modified On" FieldName="LastModifiedOn"
                        VisibleIndex="10" Width="70">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy"
                        VisibleIndex="11" Width="80">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Created From" FieldName="CreatedFrom"
                        VisibleIndex="12" Width="120">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="Doc_status"
                        VisibleIndex="13" Width="120">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                  <%--  <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="BalQty"
                        VisibleIndex="20" Width="0">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" Width="0"
                        VisibleIndex="21">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>--%>
                  <%--  <dxe:GridViewDataTextColumn Caption="IsCancel" FieldName="IsCancel" Width="0"
                        VisibleIndex="22">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="IsClosed" FieldName="IsClosed" Width="0"
                        VisibleIndex="23">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn Caption="Revision No." FieldName="SO_RevisionNo" Width="120"
                        VisibleIndex="14">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Revision date" FieldName="SO_RevisionDate"
                        VisibleIndex="15" Width="100">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="SO_ApproveStatus" Width="150"
                        VisibleIndex="16">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ReadyforInvoice" Caption="Stage" VisibleIndex="17" Width="150px">
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="OnViewReadyforInvoice('<%# Eval("Order_Id") %>')">
                                <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("ReadyforInvoice")%>'
                                    ToolTip="Click to see details">
                                </dxe:ASPxLabel>
                            </a>

                        </DataItemTemplate>
                        <EditFormSettings Visible="true" />
                        <CellStyle Wrap="False" CssClass="text-center">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AllowAutoFilter="False" />
                        <HeaderStyle Wrap="False" CssClass="text-center" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                        <DataItemTemplate>
                            <div class="floatedIcons">
                                <div class='floatedBtnArea'>
                                    <% if (rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Order_Id")%>')" class="" title="">
                                        <span class='ico ColorSix'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                    <% } %>
                                    <%-- <% if (rights.CanReadyToInvoice)
                                       { %>
                                    <a href="javascript:void(0);" onclick="ReadyToInvoice('<%#Eval("Order_Id")%>')" class="" title="" style='<%#Eval("ReadyToInvoice")%>'>
                                        <span class='ico editColor'><i class='fa fa-eye'></i></span><span class='hidden-xs'>Ready To Invoice</span></a>
                                    <% } %>--%>
                                    <% if (rights.CanMakeInvoice)
                                       { %>
                                    <a href="javascript:void(0);" onclick="MakeInvoice('<%#Eval("Order_Id")%>','<%#Eval("RegisterCustomer")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("MakeInvoice")%>'>
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Make Invoice</span></a>
                                    <% } %>
                                    <% if (rights.CanPrint)
                                       { %>
                                    <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Order_Id")%>')" class="" title="">
                                        <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                    </a><%} %>
                                </div>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <%-- --Rev Sayantani--%>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                <SettingsCookies Enabled="true" StorePaging="true" Version="2.5" />
                <%--  StoreColumnsVisiblePosition="true" --%>
                <%-- -- End of Rev Sayantani --%>
                <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <%--<SettingsSearchPanel Visible="True" />--%>
                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="false" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                </TotalSummary>
            </dxe:ASPxGridView>
            <asp:HiddenField ID="hiddenedit" runat="server" />
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="InvoiceCumChallanWithSOEntityList" />
        </div>
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

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
  

    <%--Kaushik--%>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxInvoiceDocumentsPopup" runat="server" ClientInstanceName="cInvoiceDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <dxe:ASPxCallbackPanel runat="server" ID="SelectInvoicePanel" ClientInstanceName="cInvoiceSelectPanel" OnCallback="SelectInvoicePanel_Callback" ClientSideEvents-EndCallback="cInvoiceSelectPanelEndCall">
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
                                <dxe:ASPxCheckBox ID="selectOfficecopy" Text="Extra/Office Copy" runat="server" ToolTip="Select Office Copy"
                                    ClientInstanceName="CselectOfficecopy">
                                </dxe:ASPxCheckBox>

                                <dxe:ASPxComboBox ID="CmbInvoiceDesignName" ClientInstanceName="cInvoiceCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>

                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnInvoiceOK" ClientInstanceName="cInvoicebtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformInvoiceCallToGridBind();}" />
                                    </dxe:ASPxButton>

                                </div>




                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>



                </dxe:PopupControlContentControl>

            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--Kaushik--%>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>


    <dxe:ASPxPopupControl ID="InvoiceNumberpopup" ClientInstanceName="cInvoiceNumberpopup" runat="server"
        AllowDragging="False" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Invoice Number Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="850px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="InvoiceNumberpanel" runat="server" Width="650px" ClientInstanceName="popInvoiceNumberPanel"
                    OnCallback="InvoiceNumberpanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdInvoiceNumber" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbInvoiceNumber" OnDataBinding="InvoiceNumberpanel_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Number" FieldName="Invoice_Number" HeaderStyle-CssClass="text-left"
                                            VisibleIndex="0" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-left" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="1" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Net Amt" FieldName="NetAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="2" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Amt Received" FieldName="AmountReceived" HeaderStyle-CssClass="right"
                                            VisibleIndex="3" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Balance Amt" FieldName="BalanceAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="4" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos" HeaderStyle-CssClass="right"
                                            VisibleIndex="5" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime" HeaderStyle-CssClass="right"
                                            VisibleIndex="6" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" HeaderStyle-CssClass="right"
                                            VisibleIndex="7" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn Caption="Print" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="8" Width="240px">
                                            <DataItemTemplate>

                                                <a href="javascript:void(0);" onclick="onPrintSi('<%#Eval("Invoice_Id")%>','<%#Eval("Type")%>')" class="pad" title="print" style='<%#Eval("PrintButton")%>'>
                                                    <img src="../../../assests/images/Print.png" />
                                                </a>
                                            </DataItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <HeaderTemplate><span>Print</span></HeaderTemplate>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
cInvoiceNumberpopup.Hide();
}" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="InvoiceDatepopup" ClientInstanceName="cInvoiceDatepopup" runat="server"
        AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Invoice Date Detail"
        EnableHotTrack="False" BackColor="#DDECFE" Width="850px" CloseAction="CloseButton">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <dxe:ASPxCallbackPanel ID="InvoiceDatepanel" runat="server" Width="650px" ClientInstanceName="popInvoiceDatePanel"
                    OnCallback="InvoiceDatepanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <div>
                                <dxe:ASPxGridView ID="grdInvoiceDate" runat="server" KeyFieldName="id" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cpbInvoiceDate" OnDataBinding="InvoiceDatepanel_DataBinding">
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Number" FieldName="Invoice_Number" HeaderStyle-CssClass="text-left"
                                            VisibleIndex="0" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-left" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" HeaderStyle-CssClass="text-center"
                                            VisibleIndex="1" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Net Amt" FieldName="NetAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="2" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Amt Received" FieldName="AmountReceived" HeaderStyle-CssClass="right"
                                            VisibleIndex="3" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Invoice Balance Amt" FieldName="BalanceAmount" HeaderStyle-CssClass="right"
                                            VisibleIndex="4" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos" HeaderStyle-CssClass="right"
                                            VisibleIndex="5" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime" HeaderStyle-CssClass="right"
                                            VisibleIndex="6" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" HeaderStyle-CssClass="right"
                                            VisibleIndex="7" FixedStyle="Left" Width="100px">
                                            <CellStyle CssClass="text-center" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Print" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="8" Width="240px">
                                            <DataItemTemplate>
                                                <a href="javascript:void(0);" onclick="onPrintSi('<%#Eval("Invoice_Id")%>','<%#Eval("Type")%>')" class="pad" title="print">
                                                    <img src="../../../assests/images/Print.png" />
                                                </a>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:ASPxGridView>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left">
            <Paddings PaddingRight="6px" />
        </HeaderStyle>
        <SizeGripImage Height="16px" Width="16px" />
        <CloseButtonImage Height="12px" Width="13px" />
        <ClientSideEvents CloseButtonClick="function(s, e) {
cInvoiceDatepopup.Hide();
}" />
    </dxe:ASPxPopupControl>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
        <asp:HiddenField ID="hddnKeyValue" runat="server" />
        <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />
        <asp:HiddenField ID="hddnPrintButton" runat="server" />
        <asp:HiddenField ID="hFilterType" runat="server" />
    </div>



    <%--Tanmoy--%>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cReadyToInvoicePopup"
            Width="500px" HeaderText="Update Stage" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <table style="width: 94%">
                            <tr>
                                <td>Stage<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxComboBox ID="cmbStage" runat="server" ClientInstanceName="ccmbStage" Width="100%">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Remarks<span style="color: red">*</span></td>
                                <td class="relative" colspan="2" style="padding-top: 10px;">
                                    <dxe:ASPxMemo ID="txtRemarks" runat="server" Width="100%" Height="50px" ClientInstanceName="ctxtRemarks"></dxe:ASPxMemo>
                                    <%--   <span id="MandatoryRemarksFeedbacks" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EIs" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSaves" class="btn btn-primary" onclick="ReadyToInvoice_save()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnFeedbackCancels" class="btn btn-danger" onclick="CancelReadyToInvoice()" type="button" value="Cancel" />

                                    <asp:HiddenField ID="hdnOrderID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--Tanmoy--%>
    <asp:HiddenField ID="hdnActinveInvst" runat="server" />
    <asp:HiddenField ID="hdnSalesinvSt" runat="server" />

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
</asp:Content>
