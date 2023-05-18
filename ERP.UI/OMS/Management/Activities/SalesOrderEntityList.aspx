<%--====================================================Revision History=========================================================================
 1.0   Priti    V2.0.36                   Change Approval Realted Dev Express Table Bind to HTML table 
 2.0   Pallab   V2.0.37    06-04-2023     25948: Sales Order module design modification
 3.0   Pallab   V2.0.38    11-05-2023     26100: sales order "Order waiting" button value batch design change
====================================================End Revision History=====================================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrderEntityList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.SalesOrderEntityList" EnableEventValidation="false" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   <%--REV 1.0 ADD DATATABLE SCRIPT FOR HTML TABLE--%>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>
    <%-- END DATATABLE SCRIPT FOR HTML TABLE--%>


    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.1.0/jquery-confirm.min.css">--%>
    <link href="../../../assests/css/custom/jquery.confirm.css" rel="stylesheet" />    
    <script src="JS/SalesOrderEntityList.js?v=2.1"></script>

     <%--REV 1.0 ADD DATATABLE STYLE--%>
    <style>
        .modal-dialog {
    width: 1000px !important;
}
        .fullWidth .select2-container {
            width: 100% !important;
        }

        .mBot10 {
            margin-bottom: 10px;
        }

        .font-pp {
            font-family: 'Poppins', sans-serif;
        }

        .pmsForm input[type="text"], .pmsForm input[type="password"], .pmsForm textarea, .pmsForm select {
            height: 29px;
        }

        .select2-container--default .select2-selection--single {
            background-color: #fff;
            border: 1px solid #d4d2d2;
            border-radius: 0;
            min-height: 28px;
        }

            .select2-container--default .select2-selection--single .select2-selection__rendered {
                line-height: 25px;
            }

            .select2-container--default .select2-selection--single .select2-selection__arrow {
                height: 28px;
            }

        .select2-container {
            max-width: 100% !important;
        }

        .hrBoder {
            border-color: #cecece;
        }

        .flex-wraper {
            display: flex;
            justify-content: center;
            align-items: center;
            margin-bottom: 5px;
            margin-top: 5px;
        }

            .flex-wraper > .flex-item {
                min-width: 200px;
                text-align: center;
                border: 1px solid #ccc;
                padding: 8px 10px;
            }

                .flex-wraper > .flex-item:not(:last-child) {
                    border-right: none;
                }

                .flex-wraper > .flex-item:first-child {
                    border-radius: 3px 0 0 3px;
                }

                .flex-wraper > .flex-item:last-child {
                    border-radius: 0px 3px 3px 0;
                }

                .flex-wraper > .flex-item .lb {
                    font-size: 13px;
                    font-weight: 500;
                }

                .flex-wraper > .flex-item .nm {
                    font-size: 18px;
                    font-weight: 600;
                    color: #565353;
                }

                .flex-wraper > .flex-item.active {
                    border-bottom: 5px solid #1555b5;
                }

        #filterToggle {
            padding: 10px 15px;
            position: relative;
            margin-top: 10px;
        }

        .togglerSlide {
        }

            .togglerSlide:hover {
                box-shadow: 0px 5px 10px rgba(0,0,0,0.13);
                cursor: pointer;
            }

        .togglerSlidecut {
            color: #e22222;
            font-size: 20px;
            position: absolute;
            right: 5px;
            top: 6px;
            cursor: pointer;
        }

        #myTab {
            border-bottom: 1px solid #24243e;
        }

            #myTab > .nav-item > a {
                font-family: 'Poppins', sans-serif !important;
                font-size: 13px;
            }

            #myTab > .nav-item.active > a, #myTab > .nav-item.active > a:hover {
                border: 1px solid #24243e;
                border-bottom: none;
                background: #0f0c29; /* fallback for old browsers */
                background: -webkit-linear-gradient(to right, #24243e, #302b63, #0f0c29); /* Chrome 10-25, Safari 5.1-6 */
                background: linear-gradient(to right, #24243e, #302b63, #0f0c29); /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
                color: #fff;
                min-width: 120px;
            }

            #myTab > .nav-item:hover > a {
                background: #11998e; /* fallback for old browsers */
                background: -webkit-linear-gradient(to right, #09b94b, #11998e); /* Chrome 10-25, Safari 5.1-6 */
                background: linear-gradient(to right, #09b94b, #11998e); /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
                border: 1px solid #11998e;
                color: #fff;
            }

        #myTabContent {
            padding: 20px;
            border: 1px solid #ccc;
            border-top: none;
            padding-top: 10px;
        }

        #dataTable > thead > tr > th, #dataTable2 > thead > tr > th,
        .dataTables_scrollHeadInner table > thead > tr > th, table.DTFC_Cloned > thead > tr > th {
            font-size: 14px !important;
            background: #5568f1;
            border-top: 1px solid #5568ef;
            color: #fff;
            font-weight: 400;
            border-bottom: 1px solid #1f32b5;
            padding: 10px 12px;
        }

        table.DTFC_Cloned.dataTable.no-footer {
            border-bottom: none;
        }

        #dataTable > tbody > tr > td, #dataTable2 > tbody > tr > td {
            font-size: 13px;
        }

        .badge {
            font-weight: 500;
            font-size: 10px;
        }

        .badge-danger {
            color: #fff;
            background-color: #dc3545;
        }

        .badge-info {
            color: #fff;
            background-color: #17a2b8;
        }

        .badge-success {
            color: #fff;
            background-color: #28a745;
        }

        .badge-warning {
            color: #212529;
            background-color: #ffc107;
        }

        .pmsModal .modal-header {
            background: #11998e; /* fallback for old browsers */
            background: -webkit-linear-gradient(to right, #1f5fbf, #11998e); /* Chrome 10-25, Safari 5.1-6 */
            background: linear-gradient(to right, #1f5fbf, #11998e) !important; /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
        }

        label.deep {
            font-weight: 500 !important;
        }


        .centerd {
            width: auto;
            margin: 0 auto;
            list-style-type: none;
            display: inline-block;
            border-radius: 15px;
            padding: 0;
            margin-top: -23px;
        }

            .centerd > li {
                position: relative;
                display: inline-block;
                padding: 10px 35px;
                padding-left: 55px;
                border-radius: 20px;
                cursor: pointer;
                background: #fff;
                box-shadow: 0px 2px 5px rgba(0,0,0,0.12);
            }

                .centerd > li:not(:last-child) {
                    margin-right: 20px;
                }

                .centerd > li.active {
                    background: #2cb9ae;
                    color: #fff;
                    -webkit-transform: scale(1.1);
                    -moz-transform: scale(1.1);
                    transform: scale(1.1);
                }

                .centerd > li .lb {
                    font-size: 13px;
                    font-weight: 500;
                }

                .centerd > li .nm {
                    font-size: 23px;
                    font-weight: 600;
                    color: #ffffff;
                }

        .tooltip {
            min-width: auto;
        }

        .mtop25N {
            margin-top: -23px;
        }

        .hddd {
            display: inline-block;
            padding: 10px 23px;
            background: #1c86c4;
            font-weight: 500;
            font-size: 15px;
            margin-top: -11px;
            border-radius: 0 0 8px 8px;
            color: #fff;
        }

        .setIcon {
            position: absolute;
            left: 13px;
            top: 20px;
            border: 1px dashed #fff;
            padding: 5px;
            font-size: 15px;
            min-width: 29px;
            border-radius: 9px;
        }

            .setIcon.green {
                border-color: #5bbd7e;
                color: #5bbd7e;
            }

            .setIcon.red {
                border-color: #e81b1b;
            }

        .centerd > li.active .setIcon {
            border: 2px dashed #fff;
        }

        .dataTables_wrapper input[type="search"] {
            height: 26px;
        }

        .dataTables_length select {
            margin-left: 10px;
            border-radius: 4px;
        }

        .actionInput i {
            font-size: 16px;
            cursor: pointer;
        }

            .actionInput i.assig {
                color: #0949ff;
                margin-right: 5px;
            }

            .actionInput i.det {
                color: #b30a9e;
            }

            .actionInput i[data-toggle="tooltip"] + .tooltip {
                visibility: visible;
            }

        .pmsForm label {
            font-size: 13px;
        }

        table.dataTable thead .sorting {
            background-image: none;
        }

        td.reWrap {
            white-space: normal !important;
            min-width: 150px !important;
        }
    </style>
      <%-- END DATATABLE STYLE--%>

    <link href="CSS/SalesOrderEntityList.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $("#btntransporter").hide();
            $(".dt - buttons").hide();
            ApprovalButtonVisible();
            ShowApprovalWaiting();
           
        });

        function ShowApprovalWaiting() {
            $.ajax({
                type: "POST",
                url: "SalesOrderEntityList.aspx/ButtonCountApprovalWaiting",
                //data: JSON.stringify({ FormDate: $("#hfFromDate").val(), toDate: $("#hfToDate").val(), Branch: $("#hfBranchID").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                // async: false,
                success: function (msg) {
                    //alert(msg.d);
                    var status = msg.d;
                    //if (msg.d.All != null) {
                    $("#lblWaiting").text(msg.d.ApprovalPending);
                    if (msg.d.OrderWaiting == null) {
                        $("#lblQuoteweatingCount").text("0");
                        $("#waitingOrderCount").val("0");
                    }
                    else {
                        $("#lblQuoteweatingCount").text(msg.d.OrderWaiting);
                        $("#waitingOrderCount").val(msg.d.OrderWaiting);

                    }

                }
            });
        }


        function ApprovalButtonVisible() {
            $.ajax({
                type: "POST",
                url: "SalesOrderEntityList.aspx/ApprovalButtonVisible",
                //data: JSON.stringify({ FormDate: $("#hfFromDate").val(), toDate: $("#hfToDate").val(), Branch: $("#hfBranchID").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //  async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (msg.d.spanStatus == "0") {
                        //$("#spanStatus").css("display", "none");
                        $("#spanStatus").hide();
                    }
                    else {
                        //$("#spanStatus").css("display", "block");
                        $("#spanStatus").show();
                    }

                    if (msg.d.divPendingWaiting == "0") {
                        //$("#divPendingWaiting").css("display", "none");
                        $("#divPendingWaiting").hide();
                    }
                    else {
                        //  $("#divPendingWaiting").css("display", "block");
                        $("#divPendingWaiting").show();
                    }

                }
            });
        }

        function OnApproveClick(keyValue, visibleIndex) {

            if ($("#hdnIsMultiuserApprovalRequired").val() == "Yes") {
                $.ajax({
                    type: "POST",
                    url: "SalesOrderEntityList.aspx/SalesOrderApproval",
                    data: JSON.stringify({ keyValue: keyValue }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        if (msg != null && msg == true) {
                            //document.getElementById("forfeitTable2").style.display = "block";
                            //document.getElementById("forfeitTable2").style.display = "block";
                            cGrdOrder.SetFocusedRowIndex(visibleIndex);
                            var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
                            //if (IsBalMapQtyExists.trim() != "0") {


                            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[12].innerText;

                            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                                var ActiveUser = '<%=Session["userid"]%>'
                                if (ActiveUser != null) {
                                    $.ajax({
                                        type: "POST",
                                        url: "SalesOrderList.aspx/GetEditablePermission",
                                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,//Added By:Subhabrata
                                        success: function (msg) {
                                            //debugger;
                                            var status = msg.d;
                                            var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO' + '&type1=PO';
                                            window.location.href = url;
                                        }
                                    });
                                }
                            }
                            else {
                                jAlert("Project Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
                            }
                        }
                        else {
                            jAlert('You dont have permission.');
                            return false;
                        }
                    }
                });
            }
            else {
                //document.getElementById("forfeitTable2").style.display = "block";
                //document.getElementById("forfeitTable2").style.display = "block";
                cGrdOrder.SetFocusedRowIndex(visibleIndex);
                var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
                //if (IsBalMapQtyExists.trim() != "0") {

                var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[12].innerText;




                if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                    var ActiveUser = '<%=Session["userid"]%>'
                    if (ActiveUser != null) {
                        $.ajax({
                            type: "POST",
                            url: "SalesOrderList.aspx/GetEditablePermission",
                            data: "{'ActiveUser':'" + ActiveUser + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,//Added By:Subhabrata
                            success: function (msg) {
                                //debugger;
                                var status = msg.d;
                                var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO' + '&type1=PO';
                                window.location.href = url;
                            }
                        });
                    }
                }
                else {
                    jAlert("Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
                }

            }
        }
        function OnProductDeliveryScheduleClick(keyValue, visibleIndex) {
            var url = 'OrderDeliveryScheduleList.aspx?key=' + keyValue + '&type=SO';
            window.location.href = url;
        }

        function OnMoreInfoClick(keyValue, visibleIndex) {
            //  debugger;
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
            var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            //if (IsBalMapQtyExists.trim() != "0") {


            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[12].innerText;

            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesOrderList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,//Added By:Subhabrata
                        success: function (msg) {
                            //debugger;
                            var status = msg.d;
                            var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO';
                            window.location.href = url;
                        }
                    });
                }
            }
            else {
                jAlert("Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
            }
        }

        //Rev Pratik Copy
        function OnCopyClick(keyValue, visibleIndex) {
            //  debugger;
            cGrdOrder.SetFocusedRowIndex(visibleIndex);
            var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[17].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            //if (IsBalMapQtyExists.trim() != "0") {


            var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[12].innerText;

            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesOrderList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,//Added By:Subhabrata
                        success: function (msg) {
                            //debugger;
                            var status = msg.d;
                            var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=COPY';
                            window.location.href = url;
                        }
                    });
                }
            }
            else {
                jAlert("Sales Order is " + IsCancelFlag.trim() + ".Edit is not allowed.");
            }
        }
        //End of rev Pratik Copy

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

    </script>

    <script>
        function UpdateTransporter(values) {
            $("#hdnOrderID").val(values);
            callTransporterControl(values, "SO");
            $("#exampleModal").modal('show');
        }

        function InsertTransporterControlDetails(data) {
            var orderid = $("#hdnOrderID").val();
            $.ajax({
                type: "POST",
                url: "SalesOrderEntityList.aspx/InsertTransporterControlDetails",
                data: "{'id':'" + orderid + "','hfControlData':'" + data + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {

                }
            });
        }
    </script>
    <%--rev Pallab--%>
    <style>
        #popupApproval_PW-1 {
            left: -40px !important;
        }
        #PopupUserWiseQuotation_PW-1 {
            top: 15px !important;
            left: -15px !important;
        }
        .dt-buttons {
            display: none;
        }
        /*#popupApproval_PW-1
        {

        }*/
    </style>
    <%--rev end Pallab--%>

    <style>
        /*Rev 2.0*/

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

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PlQuoteExpiry
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PlQuoteExpiry_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PlQuoteExpiry_B-1 #dt_PlQuoteExpiry_B-1Img
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
            top: 34px;
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

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }

        select
        {
            margin-bottom: 0;
        }*/

        .form-control
        {
            background-color: transparent;
        }

        /*#massrecdt
        {
            width: 100%;
        }

        .col-sm-3 , .col-md-3{
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

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }

        #CallbackPanel_LPV
        {
            top: 420px !important;
        }

        #lblWaiting
        {
            background: #f8666f;
            padding: 1px 5px;
            border-radius: 2px;
            margin-left: 4px;
        }

        /*Rev end 2.0*/

        /*Rev 3.0*/
        .typeNotification
        {
            position: initial !important;
            display: inherit !important;
            width: auto !important;
            line-height: 19px !important;
            background: #683ad5 !important;
            padding: 1px 5px !important;
            border-radius: 2px !important;
            margin-left: 4px !important;
            font-size: 14px !important;
            height: 20px;
        }

        .btn.typeNotificationBtn
        {
            padding-right: 10px !important;
                height: 30px;
        }

        .btn.typeNotificationBtn #lblQuoteweatingCount
        {
                line-height: 18px;
        }

        .hight-30
        {
                height: 30px;
        }

        /*Rev end 3.0*/
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
    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Sales Order</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <%--Rev 2.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 2.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 2.0--%>
                </td>
                <td>
                    <label>To </label>
                </td>
                <%--Rev 2.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 2.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 2.0--%>
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
            <% if (rights.CanAdd)
                { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success hight-30"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>New</span> </a>
            <% } %>

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
            <a href="javascript:void(0);" onclick="OpenWaitingOrder()" class="btn btn-warning  typeNotificationBtn"><span><u>O</u>rder Waiting </span>
                <span class="typeNotification">
                    <dxe:ASPxLabel runat="server" Text="" ID="lblQuoteweatingCount" ClientInstanceName="ClblQuoteweatingCount"></dxe:ASPxLabel>
                </span>
            </a>
            <%--Sandip Section for Approval Section in Design Start --%>

            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                    <span>My Sales Order Status</span>                                  
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
        <div class="GridViewArea relative">
        <div class="makeFullscreen ">
            <span class="fullScreenTitle">Sales Order</span>
            <span class="makeFullscreen-icon half hovered " data-instance="cGrdOrder" title="Maximize Grid" id="expandCgvPurchaseOrder"><i class="fa fa-expand"></i></span>
            <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="False" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
                SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback"
                SettingsBehavior-AllowFocusedRow="true" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared"
                HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
                <SettingsSearchPanel Visible="True" Delay="5000" />
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
                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                        VisibleIndex="3" Width="300px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BranchName"
                        VisibleIndex="4" Width="250px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name"
                        VisibleIndex="5" Width="250px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                        VisibleIndex="6" Width="100" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <%--<dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                    VisibleIndex="6"  Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="Challan_Date"
                    VisibleIndex="7"  Width="150" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>--%>
                    <%--<dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos"
                    VisibleIndex="6" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime"
                    VisibleIndex="7" Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                    <%--   <dxe:GridViewDataTextColumn FieldName="VehicleNos" Caption="Vehicle No" VisibleIndex="7" Width="150px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("MultipleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("VehicleNos")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>
                        </a>
                        <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("SingleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("VehicleNos")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                  
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn FieldName="VehicleOutDateTime" Caption="Vehicle Date" VisibleIndex="8" Width="120px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("MultipleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text='<%# Eval("VehicleOutDateTime")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("SingleStatusV")%>'>
                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text='<%# Eval("VehicleOutDateTime")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn FieldName="InvoiceNo" Caption="Invoice Details" VisibleIndex="7" Width="150px">
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("MultipleStatus")%>'>
                                <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='See Invoice'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>
                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceNo") %>')" style='<%#Eval("SingleStatus")%>'>
                                <%-- <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("InvoiceNo")%>'
                                ToolTip="Invoice Number">
                            </dxe:ASPxLabel>--%>
                                <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='See Invoice' ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>
                            <a href="javascript:void(0);" style='<%#Eval("NOInvoice")%>'>
                                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='No Invoice'
                                    ToolTip="Invoice Number">
                                </dxe:ASPxLabel>
                            </a>
                            <%--  <asp:Label ID="lblSN" runat="server" Text='<%# Eval("InvoiceNo") %>' style='<%#Eval("SingleStatus")%>'></asp:Label>--%>
                        </DataItemTemplate>
                        <EditFormSettings Visible="False" />
                        <CellStyle Wrap="False" CssClass="text-center">
                        </CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <%-- <HeaderTemplate>
                        Status
                    </HeaderTemplate>--%>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AllowAutoFilter="False" />
                        <HeaderStyle Wrap="False" CssClass="text-center" />
                    </dxe:GridViewDataTextColumn>
                    <%--   <dxe:GridViewDataTextColumn FieldName="InvoiceDate" Caption="Invoice Date" VisibleIndex="10" Width="80px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("MultipleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("InvoiceDate")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                        <a href="javascript:void(0);" onclick="OnAddEditDateClick(this,'Show~'+'<%# Eval("Order_Id") %>'+'~'+'<%# Eval("InvoiceDate") %>')" style='<%#Eval("SingleStatus")%>'>
                            <dxe:ASPxLabel ID="ASPxTextBox3" runat="server" Text='<%# Eval("InvoiceDate")%>'
                                ToolTip="Invoice Date">
                            </dxe:ASPxLabel>
                        </a>
                    </DataItemTemplate>
                    <EditFormSettings Visible="False" />
                    <CellStyle Wrap="False" CssClass="text-center">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AllowAutoFilter="False" />
                    <HeaderStyle Wrap="False" CssClass="text-center" />
                </dxe:GridViewDataTextColumn>--%>
                    <%--  <dxe:GridViewDataTextColumn Caption="Invoice Net Amount" FieldName="InvoiceNetAmount"
                    VisibleIndex="11" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>--%>
                    <%-- <dxe:GridViewDataTextColumn Caption="Invoice Amount Received" FieldName="InvoiceAmountReceived"
                    VisibleIndex="12" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Invoice Balance Amount" FieldName="InvoiceBalanceAmount"
                    VisibleIndex="13" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PosState"
                        VisibleIndex="8" Width="150">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                        VisibleIndex="9" Width="80">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Last Modified On" FieldName="LastModifiedOn"
                        VisibleIndex="10" Width="110">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy"
                        VisibleIndex="11" Width="100">
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

                    <dxe:GridViewDataTextColumn Caption="Bal Quantity" FieldName="BalQty"
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
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="IsCancel" FieldName="IsCancel" Width="0"
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
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Revision No." FieldName="SO_RevisionNo" Width="120"
                        VisibleIndex="24">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Revision date" FieldName="SO_RevisionDate"
                        VisibleIndex="25" Width="100">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="SO_ApproveStatus" Width="150"
                        VisibleIndex="26">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Party Order No" FieldName="Order_OANumber"
                        VisibleIndex="27" Width="140px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Party order date" FieldName="Order_OADate"
                        VisibleIndex="28" Width="150px" Settings-ShowFilterRowMenu="True" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <%--  End of Rev Sayantani --%>
                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="27" Width="0">
                        <DataItemTemplate>
                            <div class="floatedIcons">
                                <div class='floatedBtnArea'>
                                    <% if (rights.CanView)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Order_Id")%>')" class="" title="">
                                        <span class='ico ColorSix'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                                    <% } %>
                                    <% if (rights.CanEdit)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("SOLastEntryStaus")%>'>

                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Modify</span></a>  <% } %>
                                    <%-- rev Pratik Copy--%>
                                    <% if (rights.CanAdd)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnCopyClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">

                                        <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>  <% } %>
                                    <%-- End of rev Pratik Copy--%>

                                    <% if (rights.CanApproved && isApprove)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnApproveClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">

                                        <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve/Reject</span>

                                    </a><% } %>

                                    <% if (rights.CanDelete)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("SOLastEntryStaus")%>'>
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <% } %>

                                    <% if (rights.CanCancel)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnCancelClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("SOLastEntryStaus")%>'>

                                        <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel Order</span>

                                    </a><% } %>

                                    <% if (rights.CanClose)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnClosedClick('<%#Eval("Order_Id")%>',<%# Container.VisibleIndex %>)" class="" title="" style='<%#Eval("SOLastEntryStaus")%>'>

                                        <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Closed Order</span>

                                    </a><% } %>

                                    <a href="javascript:void(0);" onclick="OnClickCopy('<%#Eval("Order_Id")%>')" class="" title=" " style="display: none">
                                        <span class='ico ColorFour'><i class='fa fa-files-o'></i></span><span class='hidden-xs'>Copy</span>
                                    </a>
                                    <a href="javascript:void(0);" onclick="OnClickStatus('<%#Eval("Order_Id")%>')" class="" title="" style="display: none">
                                        <span class='ico ColorFive'><i class='fa fa-check'></i></span><span class='hidden-xs'>Status</span></a>
                                    <% if (rights.CanView)
                                        { %>
                                    <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Order_Id")%>')" class="" title="">
                                        <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                                    </a>
                                    <% } %>
                                    <% if (rights.CanPrint)
                                        { %>
                                    <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Order_Id")%>')" class="" title="">
                                        <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                    </a><%} %>

                                    <% if (rights.CanUpdateTransporter)
                                        { %>
                                    <a href="javascript:void(0);" onclick="UpdateTransporter('<%#Eval("Order_Id")%>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Update Transporter</span></a>
                                    <% } %>

                                    <% if (rights.CanClose)
                                        { %>
                                    <% if (ShowProductWiseClose.ToUpper() == "1")
                                        {%>
                                    <a href="javascript:void(0);" onclick="OnProductWiseClosedClick('<%#Eval("Order_Id")%>','<%# Container.VisibleIndex %>','<%#Eval("OrderNo") %>')" class="" title="">
                                        <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Product Wise Closed</span></a>
                                    <% } %>
                                    <% } %>
                                    <% if (rights.CanAdd)
                                        { %>
                                    <% if (ShowDeliverySchedule.ToUpper() == "1")
                                        {%>
                                    <a href="javascript:void(0);" onclick="OnProductDeliveryScheduleClick('<%#Eval("Order_Id")%>','<%# Container.VisibleIndex %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Delivery Schedule</span></a>
                                    <% } %>
                                    <% } %>
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
                <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="true" Version="3.3" />
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
                ContextTypeName="ERPDataClassesDataContext" TableName="v_GetSalesOrderEntityList" />

            <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
        </div>
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <%--DEBASHIS--%>
    <div class="PopUpArea">

        <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
            Width="400px" HeaderText="Reason For Cancel" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width: 94%">

                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>


        <dxe:ASPxPopupControl ID="Popup_Closed" runat="server" ClientInstanceName="cPopup_Closed"
            Width="400px" HeaderText="Reason For Close" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">
                        <table style="width: 94%">
                            <tr>
                                <td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosed"></dxe:ASPxMemo>
                                    <span id="MandatoryRemarksFeedback1" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSave" class="btn btn-primary" onclick="CallClosed_save()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnClosedCancel" class="btn btn-danger" onclick="CancelClosed_save()" type="button" value="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />


        </dxe:ASPxPopupControl>


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
    <%-- Sandip Approval Dtl Section Start--%>

    <div class="PopUpArea">

        <dxe:ASPxPopupControl ID="popup_OrderWait" runat="server" Width="1200"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="Cpopup_OrderWait"
            HeaderText="Order Waiting" AllowResize="false" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <div onkeypress="OnWaitingGridKeyPress(event)">

                        <dxe:ASPxGridView ID="watingOrdergrid" runat="server" KeyFieldName="SBMain_Id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="CwatingOrdergrid" OnCustomCallback="watingOrdergrid_CustomCallback" KeyboardSupport="true"
                            SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="watingOrdergrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
                            Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="320">
                            <Columns>
                                <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />--%>

                                <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="Salesman"
                                    VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                                    VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Name"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Product List" FieldName="ProductList"
                                    VisibleIndex="1" FixedStyle="Left" Width="180px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn Caption="Billing Amount" FieldName="finalAmount"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Billing Date" FieldName="Billingdate"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Payment Type" FieldName="Paymenttype"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="60">
                                    <DataItemTemplate>
                                        <% if (rights.CanDelete)
                                            { %>
                                        <a href="javascript:void(0);" onclick="RemoveQuote('<%# Container.KeyValue %>')" class="pad" title="Remove">
                                            <%--   <img src="../../../assests/images/Delete.png" />--%>
                                            <i class="fa fa-close" aria-hidden="true" id="CloseRemoveWattingBtn" style="font-size: 19px; color: #f35248;"></i>
                                        </a>
                                        <%} %>
                                    </DataItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>

                            </Columns>

                            <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingOrdergridEndCallback" />

                            <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <SettingsSearchPanel Visible="True" />
                            <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>
                    </div>


                    <dxe:ASPxButton ID="OrderWattingOk" runat="server" AutoPostBack="false" Text="O&#818;k" CssClass="btn btn-primary okClass"
                        ClientSideEvents-Click="OrderWattingOkClick" UseSubmitBehavior="False" />
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

    </div>
    <dxe:ASPxTimer runat="server" Interval="3600000" ClientInstanceName="Timer1">
    <ClientSideEvents Tick="timerTick" />
    </dxe:ASPxTimer>
    <asp:HiddenField ID="waitingOrderCount" runat="server" />
    <div class="PopUpArea">
       
      


     <div class="modal fade" id="popupApprovalModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" id="ModuleSegment5Header">Pending Approvals</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto; height: 380px;">
                        <div id="divListData"  >
                            <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Document No.</th>
                                        <th>Party Name</th>
                                        <th>Posting Date</th>
                                        <th>Unit</th>
                                        <th>Entered By</th>
                                        <th>Approved</th>
                                        <th>Rejected </th>

                                    </tr>
                                </thead>

                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>



      <%--  <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrder" runat="server" OnSelecting="EntityServerModeDataSalesOrder_Selecting"
            ContextTypeName="ERPDataClassesDataContext" />
        <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
            Width="1000px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">                           
                                <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cgridPendingApproval"
                                    OnCustomCallback="gridPendingApproval_CustomCallback"
                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="350"
                                    DataSourceID="EntityServerModeDataSalesOrder">
                                    <Columns>

                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number" Width="200px"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Name" FieldName="customer" Width="200px"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreateDate" Width="100px"
                                            VisibleIndex="1" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description" Width="200px"
                                            VisibleIndex="2" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby" Width="100px"
                                            VisibleIndex="3" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved" Width="100px">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                        </dxe:GridViewDataCheckColumn>

                                        <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected" Width="100px">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                        </dxe:GridViewDataCheckColumn>
                                    </Columns>
                                    <SettingsBehavior AllowFocusedRow="true" />
                                    <SettingsPager NumericButtonCount="20" PageSize="50" ShowSeparators="True">
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
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>--%>




        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ClientInstanceName="popup"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span>
                <div class="closeApprove" onclick="closeUserApproval();"><i class="fa fa-close"></i></div>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

    <%--    REV 1.0 Start--%>
            <div class="modal fade" id="UserWiseApprovalModel" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">User Wise Sales Order Status</h4>
                    </div>
                    <div class="modal-body">
                        <div id="divListUserWiseData">
                            <table id="UserWisedataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Branch</th>
                                        <th>Sale Order No.</th>
                                        <th>Posting Date</th>
                                        <th>Date</th>
                                        <th>Customer</th>
                                        <th>Approval User</th>
                                        <th>User Level </th>
                                        <th>Status</th>
                                    </tr>
                                </thead>

                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    <%--    REV 1.0 End--%>

       <%--    REV 1.0 Start--%>
       <%-- <dx:LinqServerModeDataSource ID="EntityServerModeDataSalesOrderUserWise" runat="server" OnSelecting="EntityServerModeDataSalesOrderUserWise_Selecting"
        ContextTypeName="ERPDataClassesDataContext" />
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Sales Order Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">

                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback"
                                DataSourceID="EntityServerModeDataSalesOrderUserWise">
                                <Columns>
                                    <%--OnDataBinding="gridUserWiseQuotation_DataBinding" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged"
                                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="Branch"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Sale Order No." FieldName="number"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="OrderedDate"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="false" />
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="4" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="5" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="true" />
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
        </dxe:ASPxPopupControl>--%>
       <%--    REV 1.0 End--%>




    </div>
    <%-- Sandip Approval Dtl Section End--%>

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
popup.Hide();
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
popup.Hide();
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
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hdnOrderID" runat="server" />
        <asp:HiddenField ID="hFilterType" runat="server" />
        <asp:HiddenField ID="hdnIsMultiuserApprovalRequired" runat="server" />
        <asp:HiddenField ID="hdnIsFilter" runat="server" />

    </div>
    <dxe:ASPxPopupControl ID="PopupProductwiseClose" runat="server" ClientInstanceName="cPopupProductwiseClose"
        Width="900px" HeaderText="Product wise - Close" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="row">
                    <div class="col-md-4">
                        <dxe:ASPxLabel ID="lbl_PIQuoteNo" runat="server" Text="Document No.">
                        </dxe:ASPxLabel>
                        <asp:TextBox ID="txtVoucherNo" runat="server" Width="100%" MaxLength="50" ReadOnly="true">                             
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="gridProductwiseClose" runat="server" KeyFieldName="OrderDetails_Id" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="cgridProductwiseClose" OnCustomCallback="gridProductwiseClose_CustomCallback"
                            OnRowInserting="gridProductwiseClose_RowInserting" OnRowUpdating="gridProductwiseClose_RowUpdating" OnRowDeleting="gridProductwiseClose_RowDeleting" Settings-VerticalScrollableHeight="400" Settings-VerticalScrollBarMode="Visible">
                            <Columns>
                                <dxe:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="60" Caption=" " />
                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="ProductName"
                                    VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description"
                                    VisibleIndex="2">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                                    VisibleIndex="3">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Balance Quantity" FieldName="BalQuantity"
                                    VisibleIndex="4">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="OrderDetails_OrderId" ReadOnly="true" Caption="OrderDetails_OrderId" Width="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="OrderDetails_ProductId" ReadOnly="true" Caption="OrderDetails_ProductId" Width="0">
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

                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <ClientSideEvents EndCallback="OnProductCloseEndCall" />
                        </dxe:ASPxGridView>
                    </div>
                    <div class="clear"></div>
                    <div class="text-center pTop10">
                        <dxe:ASPxButton ID="btnCloseProduct" ClientInstanceName="cbtnCloseProduct" runat="server" AutoPostBack="False" Text="Close Product" CssClass="btn btn-primary" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {return CloseProduct();}" />
                        </dxe:ASPxButton>
                    </div>



                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>
