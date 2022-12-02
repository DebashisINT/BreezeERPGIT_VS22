<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ServiceManagement.STBManagement.Approval.index" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>
    <script src="/assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>

    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/dataTables.buttons.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.flash.min.js"></script>
    <script src="/assests/pluggins/DataTable/JSZip-2.5.0/jszip.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/pdfmake.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/vfs_fonts.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.html5.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.print.min.js"></script>

    <style>
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

    <style>
        
        @media only screen and (min-width: 620px) {
            .circulerUl {
                display: inline-block;
                list-style-type: none;
                margin: 0;
                padding: 0;
            }

                .circulerUl > li {
                    display: inline-block;
                    margin-right: 15px;
                    cursor: pointer;
                    color: #4a60ff;
                }

                    .circulerUl > li.ds {
                        opacity: 0.4;
                    }

                    .circulerUl > li .crcl {
                        width: 80px;
                        height: 80px;
                        text-align: center;
                        border: 5px solid #4a60ff;
                        border-radius: 50%;
                        line-height: 70px;
                        font-size: 20px;
                        color: #4a60ff;
                        margin: 0 auto;
                        position: relative;
                    }

                        .circulerUl > li .crcl.red {
                            border: 5px solid #e81b1b !important;
                            color: #e81b1b !important;
                        }

                    .circulerUl > li.activeCrcl {
                        opacity: 1;
                    }

                    .circulerUl > li .crcl .icn {
                        position: absolute;
                        bottom: 2px;
                        left: 27px;
                        visibility: hidden;
                    }

                    .circulerUl > li.activeCrcl .crcl .icn {
                        visibility: visible;
                    }

                    .circulerUl > li .crcl + div {
                        font-size: 14px;
                    }

            .colRed {
                color: red !important;
            }

            #divListData .dt-buttons {
                display: none;
            }

            .closeApprove {
                float: right;
                margin-right: 7px;
            }
        }
    </style>

      <style>
        .mtop8 {
            margin-top: 8px;
        }

        .ptTbl > tbody > tr > td {
            padding-right: 10px;
            padding-bottom: 8px;
        }

        .headerPy {
            background: #66b1c7;
            /* display: inline-block; */
            padding: 4px 10px;
            /* transform: translate(-4px); */
            border-radius: 5px 5px 0 0;
            /* border: 1px solid #858eb7; */
            font-weight: 500;
            color: #f1f1f1;
            margin-top: 2px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                }
            });

            $('#dataTable2').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                }
            });

            $('#filterToggle').hide();

            $('.togglerSlide').click(function () {
                $('#filterToggle').slideDown({
                    duration: 200,
                    easing: "easeOutQuad"
                });
                $(this).hide()
            });
            $('.togglerSlidecut').click(function () {
                $('#filterToggle').slideUp({
                    duration: 200,
                    easing: "easeOutQuad"
                });
                $('.togglerSlide').show();
            });

            $('[data-toggle="tooltip"]').tooltip();

        });
    </script>

    <script>
        function TotalTotal() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('ALL');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d1').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalMRCancelation() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('MRCancelation');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d2').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalWRCancelation() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('WRCancelation');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d3').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalHolds() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterByStatus").val('Hold');
                $("#hdnFilterby").val('STBRequisition');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d4').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalSTBRequisition() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('STBRequisition');
                $("#hdnFilterByStatus").val('Open');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d5').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalInventoryCancelation() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('STBRequisition');
                $("#hdnFilterByStatus").val('Inventory Cancellation');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d6').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function DirectorApproval() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('STBRequisition');
                $("#hdnFilterByStatus").val('Director Approval');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d7').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalInventoryCancelation() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('STBRequisition');
                $("#hdnFilterByStatus").val('Inventory Cancellation');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d6').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function TotalReturnRequisition() {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnFilterby").val('STBReturnRequisition');
                $("#hdnFilterByStatus").val('Return Requisition');
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d8').addClass('activeCrcl');

                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function gridRowclick(s, e) {
            $('#GrdReceiptChallan').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                $.each(lists, function (index, value) {
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function ClickCancel(val) {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnmodule_ID").val(val);
                $("#CancelDocumentPopUp").modal('toggle');
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function ClickOnApprove(val) {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnmodule_ID").val(val);
                $("#ApproveDocumentpop").modal('toggle');
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function ClickOnApproveM(val) {
            
           
        }

        function CancelDocument() {
            var Remarks = $("#txtCancelDocumentRemarks").val();
            var module_id = $("#hdnmodule_ID").val();
            $.ajax({
                type: "POST",
                url: "index.aspx/CancelDocument",
                data: JSON.stringify({ Document_ID: module_id, Remarks: Remarks }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        if (response.d == "Updated Successfully.") {
                            jAlert("Updated Successfully.", "Alert", function () {

                                $("#txtCancelDocumentRemarks").val("");
                                $("#hdnmodule_ID").val("");
                                $("#CancelDocumentPopUp").toggle();
                                ButtonCount();
                                cgridStatus.Refresh();
                            });
                        }
                        else if (response.d == "Logout") {
                            location.href = "../../OMS/SignOff.aspx";
                        }
                        else {
                            jAlert(response.d);
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function ApproveDocument() {
            var Remarks = $("#txtApproveDocumentRemarks").val();
            var module_id = $("#hdnmodule_ID").val();
            $.ajax({
                type: "POST",
                url: "index.aspx/ApproveDocument",
                data: JSON.stringify({ Document_ID: module_id, Remarks: Remarks }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        if (response.d == "Updated Successfully.") {
                            jAlert("Updated Successfully.", "Alert", function () {

                                $("#txtApproveDocumentRemarks").val("");
                                $("#hdnmodule_ID").val("");
                                $("#ApproveDocumentpop").toggle();
                                ButtonCount();
                                cgridStatus.Refresh();
                            });
                        }
                        else if (response.d == "Logout") {
                            location.href = "../../OMS/SignOff.aspx";
                        }
                        else {
                            jAlert(response.d);
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function gridRowclick(s, e) {
            //alert('hi');
            $('#gridStatus').find('tr').removeClass('rowActive');
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
                        //console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function ButtonCount() {
            $.ajax({
                type: "POST",
                url: "index.aspx/CountButton",
                data: JSON.stringify({ UserType: '' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $("#divTotal").text(msg.d.Total);
                    $("#divMRCancelation").text(msg.d.MRCancelation);
                    $("#divWRCancelation").text(msg.d.WRCancelation);
                    $("#divHolds").text(msg.d.Holds);
                    $("#divSTBRequisition").text(msg.d.STBRequisition);
                    $("#divInventoryCancelation").text(msg.d.InventoryCancelation);
                    $("#divDirectorApproval").text(msg.d.DirectorApproval);
                }
            });
        }
    </script>

    <script>
        function ClickOnApproval(val, CancelStatus) {
            var windowSize = $(window).width();
            if (windowSize < 580) {
                //Mobile
                $.ajax({
                    type: "POST",
                    url: "index.aspx/ClickOnApproveM",
                    data: JSON.stringify({ ID: val.split(',')[1] }),
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d) {
                            if (response.d != "") {
                                var url = "";
                                if (CancelStatus == "Director Approval") {
                                    url = "/STBManagement/reqApprovalM/reqApproval.aspx?id=" + val.split(',')[1] + "&AU=" + response.d.split('~')[0] + "&UniqueKey=" + response.d.split('~')[1];
                                } else {
                                    if (val.split(',')[0] == "RQ") {
                                        if (CancelStatus == 'Inventory Cancellation') {
                                            url = "/STBManagement/StbReqMMM/StbReqMMM.aspx?id=" + val.split(',')[1] + "&AU=" + response.d.split('~')[0] + "&UniqueKey=" + response.d.split('~')[1] + "&Inv=y";
                                        }
                                        else {
                                            url = "/STBManagement/StbReqMMM/StbReqMMM.aspx?id=" + val.split(',')[1] + "&AU=" + response.d.split('~')[0] + "&UniqueKey=" + response.d.split('~')[1];
                                        }                                       
                                    }
                                    else {
                                        url = "/STBManagement/RetReqM/RetReqM.aspx?id=" + val.split(',')[1] + "&AU=" + response.d.split('~')[0] + "&UniqueKey=" + response.d.split('~')[1];
                                    }
                                }
                                //alert(url);
                                $("#approveMobile").modal("show");
                                $("#mApproveIframe").attr("src", url);

                            }
                            else if (response.d == "Logout") {
                                location.href = "../../OMS/SignOff.aspx";
                            }
                            else {
                                jAlert(response.d);
                                return
                            }
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            } else {
                //desktop
                WorkingRoster();
                if (rosterstatus) {
                    if (CancelStatus == "Inventory Cancellation") {
                        if (val.split(',')[0] == "RQ") {
                            uri = "../Requisition/STBRequisitionAdd.aspx?id=" + val.split(',')[1] + "&Key=edit&status=1";
                        }
                        else {
                            uri = "../ReturnRequisition/ReturnRequisition.aspx?id=" + val.split(',')[1] + "&Key=edit&status=1";
                        }
                        popup.SetContentUrl(uri);
                        popup.Show();
                    }
                    else {
                        if (val.split(',')[0] == "RQ") {
                            uri = "../Requisition/STBRequisitionAdd.aspx?id=" + val.split(',')[1] + "&Key=edit&status=2";
                        }
                        else {
                            uri = "../ReturnRequisition/ReturnRequisition.aspx?id=" + val.split(',')[1] + "&Key=edit&status=2";
                        }
                        popup.SetContentUrl(uri);
                        popup.Show();
                    }
                }
                else {
                    $("#divPopHead").removeClass('hide');
                }
            }           
        }

        function closeUserApproval() {
            popup.Hide();
        }
    </script>

    <script>
        var rosterstatus = false;
        function WorkingRoster() {
            $.ajax({
                type: "POST",
                url: 'index.aspx/CheckWorkingRoster',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ module_ID: '4' }),
                success: function (response) {
                    if (response.d.split('~')[0] == "true") {
                        rosterstatus = true;
                    }
                    else if (response.d.split('~')[0] == "false") {
                        rosterstatus = false;
                        $("#spnbegin").text(response.d.split('~')[1]);
                        $("#spnEnd").text(response.d.split('~')[2]);
                    }
                },
            });
        }

        function WorkingRosterClick() {
            $("#divPopHead").addClass('hide');
        }
    </script>
    <style>
        /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .popBox {
            width: 670px;
            background: #fff;
            padding: 35px;
            text-align: center;
            min-height: 350px;
            display: flex;
            align-items: center;
            flex-direction: column;
            justify-content: center;
            background: #fff url("/assests/images/popupBack.png") no-repeat top left;
            box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        }

            .popBox h1, .popBox p {
                font-family: 'Poppins', sans-serif !important;
                margin-bottom: 20px !important;
            }

            .popBox p {
                font-size: 15px;
            }

        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

            .btn-sign:hover {
                background: #2e71e1;
                color: #fff;
            }

        .MobiledivReceiptChallanDtls {
            display: none;
        }

        @media screen and (max-width: 580px) {
            #onlyMonitor, .onlyDesktop {
                display: none;
            }
            .onlyMobile {
                display:block;
            }
            .MobiledivReceiptChallanDtls {
                display: block;
                border: 1px solid #3db191;
                border-radius: 5px;
                padding: 10px 0;
            }
        }
    </style>

    <script>

        function MoneyReceiptView(values, module) {
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "index.aspx/ReceptDetails",
                    data: JSON.stringify({ ReceiptID: values, module: module }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        $("#txtDocumentNumber").val(msg.d.DocumentNumber);
                        $("#txtLocation").val(msg.d.Branch);
                        $("#txtDate").val(msg.d.DocumentDate);
                        $("#EntityCode").val(msg.d.EntityCode);
                        $("#NetworkName").val(msg.d.NetworkName);
                        $("#ContactPerson").val(msg.d.ContactPerson);
                        $("#ContactNumber").val(msg.d.ContactNo);
                        $("#txtType").val(msg.d.EntryType);

                        $("#hdnIsCancelReceipt").val(msg.d.IsCancel);

                        if ($("#hdnIsCancelReceipt").val() == "True") {

                            $("#DivReceiptCancelDetails").removeClass('hide');
                            $("#DivReceiptCancelData").removeClass('hide');
                            $("#txtReceiptCancellationReason").val(msg.d.ReasonForCancel);
                            $("#txtReceiptCancelRequestBY").val(msg.d.Cancel_Create_by);
                            $("#txtReceiptCancelRequestON").val(msg.d.Cancel_Create_date);
                        }
                        else {

                            $("#DivReceiptCancelDetails").addClass('hide');
                            $("#DivReceiptCancelData").addClass('hide');

                        }

                        var status = " <table id='dataTable2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Srl.No.</th><th>Payment Mode</th><th>Amount</th><th>Cheque No</th><th>Cheque Date</th><th>Ref No.</th><th>Bank Name</th><th>Branch Name</th><th>Remarks</th>";

                        status = status + " </tr></thead>";

                        status = status + " </table>";
                        $('#divReceiptChallanDtls').html(status);

                        console.log("dd", msg.d.DetailsList);

                        setTimeout(function () {
                            $('#dataTable2').DataTable({
                                scrollX: true,
                                fixedColumns: {
                                    rightColumns: 1
                                },
                                data: msg.d.DetailsList,
                                columns: [
                                   { 'data': 'Payment_Id' },
                                   { 'data': 'Payment_Mode' },
                                   { 'data': 'Payment_Amount' },
                                   { 'data': 'Cheque_No' },
                                   { 'data': 'Cheque_date' },
                                   { 'data': 'Ref_No' },
                                   { 'data': 'PaymentDetails_BankName' },
                                   { 'data': 'PaymentDetails_BranchName' },
                                   { 'data': 'Remarks' },

                                ],
                            });
                            $("#mPaymentMode").val(msg.d.DetailsList[0].Payment_Mode);
                            $("#mAmount").val(msg.d.DetailsList[0].Payment_Amount);
                            $("#mChecqueNo").val(msg.d.DetailsList[0].Cheque_No);
                            $("#mChecqueDate").val(msg.d.DetailsList[0].Cheque_date);
                            $("#mRefNo").val(msg.d.DetailsList[0].Ref_No);
                            $("#mBankName").val(msg.d.DetailsList[0].PaymentDetails_BankName);
                            $("#mBranchName").val(msg.d.DetailsList[0].PaymentDetails_BranchName);
                            $("#mRemarks").val(msg.d.DetailsList[0].Remarks);
                        }, 400);
                        $("#detailsModal").modal('show');
                    }
                });
            }, 1000);
        }

        function ClickOnViewDetails(values) {
            var id = values.split(',')[1];
            var module = values.split(',')[0];

            if (module == "WR") {
                WalletRechargeView(id, 'Normal');
            }
            else if (module == "MR") {
                MoneyReceiptView(id, 'Normal');
            }
            else if (module == "RQ") {
                STBRequisitionView(id, 'Normal');
            }
            else if (module == "RR") {
                STBReturnRequisitionDetails(id, 'Normal');
            }
        }


        function WalletRechargeView(values, module) {
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "index.aspx/WalletRechargeDetails",
                    data: JSON.stringify({ WalletID: values, module: module }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        $("#txtDocumentNumberWallet").val(msg.d.DocumentNumber);
                        $("#txtLocationWallet").val(msg.d.Branch);
                        $("#txtDateWallet").val(msg.d.DocumentDate);
                        $("#EntityCodeWallet").val(msg.d.EntityCode);
                        $("#NetworkNameWallet").val(msg.d.NetworkName);
                        $("#ContactPersonWallet").val(msg.d.ContactPerson);
                        $("#ContactNumberWallet").val(msg.d.ContactNo);
                        $("#txtTypeWallet").val(msg.d.EntryType);

                        $("#hdnIsCancelWallet").val(msg.d.IsCancel);

                        if ($("#hdnIsCancelWallet").val() == "True") {

                            $("#DivCancelDetails").removeClass('hide');
                            $("#DivCancelData").removeClass('hide');
                            $("#txtReasonCancel").val(msg.d.ReasonForCancel);
                            $("#txtReasonCancelMadeBy").val(msg.d.Cancel_Create_by);
                            $("#txtReasonCancelMadeOn").val(msg.d.Cancel_Create_date);
                        }
                        else {

                            $("#DivCancelDetails").addClass('hide');
                            $("#DivCancelData").addClass('hide');

                        }

                        var status = " <table id='dataTableWR' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Srl.No.</th><th>Payment Mode</th><th>Amount</th><th>Cheque No</th><th>Cheque Date</th><th>Ref No.</th><th>Bank Name</th><th>Branch Name</th><th>Remarks</th>";

                        status = status + " </tr></thead>";

                        status = status + " </table>";
                        $('#divWalletRechargeDtls').html(status);

                        setTimeout(function () {
                            console.log("msg.d.DetailsList", msg.d.DetailsList);
                           
                            $('#dataTableWR').DataTable({
                                scrollX: true,
                                fixedColumns: {
                                    rightColumns: 1
                                },
                                data: msg.d.DetailsList,
                                columns: [
                                   { 'data': 'Payment_Id' },
                                   { 'data': 'Payment_Mode' },
                                   { 'data': 'Payment_Amount' },
                                   { 'data': 'Cheque_No' },
                                   { 'data': 'Cheque_date' },
                                   { 'data': 'Ref_No' },
                                   { 'data': 'Payment_BankName' },
                                   { 'data': 'Payment_BranchName' },
                                   { 'data': 'Remarks' },

                                ],
                            });
                            var dataForM = msg.d.DetailsList;
                            var htMM = "";
                            for (i = 0; i < dataForM.length; i++) {
                                htMM += "<div class='segment'>";
                                htMM += "<div class='col-sm-12'><label>Payment Mode</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Payment_Mode + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Amount</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Payment_Amount + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Checque No</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Cheque_No + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Check Date</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Cheque_date + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Ref No</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Ref_No + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Bank Name</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Payment_BankName + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Branch Name</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Payment_BranchName + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Remarks</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Remarks + "' /></div></div>";
                                htMM += "</div>";
                            }
                            
                            $("#walleTViewPop").html(htMM);
                            
                        }, 400);
                        $("#detailsWalletModal").modal('show');
                    }
                });
            }, 1000);
        }

        function STBRequisitionView(values, module) {
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "index.aspx/STBRequisitionDetails",
                    data: JSON.stringify({ STBRequisitionID: values, module: module }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        $("#txtDocumentNumberSTBReq").val(msg.d.DocumentNumber);
                        $("#txtLocationSTBReq").val(msg.d.Location);
                        $("#txtDateSTBReq").val(msg.d.DocumentDate);
                        $("#txtRequisitionTypeSTBReq").val(msg.d.RequisitionType);
                        $("#txtRequisitionForSTBReq").val(msg.d.RequisitionFor);
                        $("#txtEntityCodeSTBReq").val(msg.d.EntityCode);
                        $("#txtNetworkNameSTBReq").val(msg.d.NetworkName);
                        $("#txtContactPersonSTBReq").val(msg.d.ContactPerson);
                        $("#txtContactNumberSTBReq").val(msg.d.ContactNo);
                        $("#txtDASSTBReq").val(msg.d.DAS);


                        $("#spnPayUsingAcountBalance").text(msg.d.PayUsingAcountBalance);
                        $("#spnNoPayment").text(msg.d.NoPayment);
                        $("#txtPaymentRelatedRemarks").val(msg.d.PaymentRelatedRemarksNotes);

                        $("#DivCancelDetails").addClass('hide');
                        $("#DivCancelData").addClass('hide');

                        var status = " <table id='dataTableReq' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Srl.No.</th><th>Model</th><th>Unit Price</th><th>Quantity</th><th>Amount</th><th>Remarks</th>";
                        status = status + " </tr></thead>";
                        status = status + " </table>";
                        $('#divSTBReqRechargeDtls').html(status);

                        setTimeout(function () {
                            $('#dataTableReq').DataTable({
                                scrollX: true,
                                fixedColumns: {
                                    rightColumns: 1
                                },
                                data: msg.d.DetailsList,
                                columns: [
                                   { 'data': 'SrlNo' },
                                   { 'data': 'Model' },
                                   { 'data': 'UnitPrice' },
                                   { 'data': 'Quantity' },
                                   { 'data': 'Amount' },
                                   { 'data': 'Remarks' },
                                ],
                            });
                        }, 400);

                        if (msg.d.RequisitionFor != "Fresh Issue") {
                            $('#divSTBReqRechargeDtls2').removeClass('hide');
                            var status = " <table id='dataTableReq2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                            status = status + " <thead><tr>";
                            status = status + " <th>Srl.No.</th><th>Model</th><th>Unit Price</th><th>Quantity</th><th>Amount</th><th>Remarks</th>";
                            status = status + " </tr></thead>";
                            status = status + " </table>";
                            $('#divSTBReqRechargeDtls2').html(status);

                            setTimeout(function () {
                                $('#dataTableReq2').DataTable({
                                    scrollX: true,
                                    fixedColumns: {
                                        rightColumns: 1
                                    },
                                    data: msg.d.DetailsList2,
                                    columns: [
                                       { 'data': 'SrlNo' },
                                       { 'data': 'Model' },
                                       { 'data': 'UnitPrice' },
                                       { 'data': 'Quantity' },
                                       { 'data': 'Amount' },
                                       { 'data': 'Remarks' },
                                    ],
                                });
                            }, 400);
                        }
                        else {
                            $('#divSTBReqRechargeDtls2').addClass('hide');
                        }

                        $("#detailsSTBReqModal").modal('show');
                    }
                });
            }, 1000);
        }

        function STBReturnRequisitionDetails(values, module) {
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "index.aspx/STBReturnRequisitionDetails",
                    data: JSON.stringify({ STBReturnRequisitionID: values, module: module }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        $("#txtDocumentNumberSTBReturn").val(msg.d.DocumentNumber);
                        $("#txtLocationSTBReturn").val(msg.d.Location);
                        $("#txtDateSTBReturn").val(msg.d.DocumentDate);
                        $("#txtRequisitionTypeSTBReturn").val(msg.d.RequisitionType);
                        $("#txtRequisitionForSTBReturn").val(msg.d.RequisitionFor);
                        $("#txtEntityCodeSTBReturn").val(msg.d.EntityCode);
                        $("#txtNetworkNameSTBReturn").val(msg.d.NetworkName);
                        $("#txtContactPersonSTBReturn").val(msg.d.ContactPerson);
                        $("#txtContactNumberSTBReturn").val(msg.d.ContactNo);
                        $("#txtDASSTBReturn").val(msg.d.DAS);


                        $("#spnPayUsingAcountBalanceReturn").text(msg.d.PayUsingAcountBalance);
                        $("#spnNoPaymentReturn").text(msg.d.NoPayment);
                        $("#txtRemarksReturn").val(msg.d.PaymentRelatedRemarksNotes);


                        var status = " <table id='dataTableReturn' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Srl.No.</th><th>Model</th><th>Unit Price</th><th>Quantity</th><th>Amount</th><th>Remarks</th>";
                        status = status + " </tr></thead>";
                        status = status + " </table>";
                        $('#divSTBReturnRechargeDtls').html(status);

                        setTimeout(function () {
                            $('#dataTableReturn').DataTable({
                                scrollX: true,
                                fixedColumns: {
                                    rightColumns: 1
                                },
                                data: msg.d.DetailsList,
                                columns: [
                                   { 'data': 'SrlNo' },
                                   { 'data': 'Model' },
                                   { 'data': 'UnitPrice' },
                                   { 'data': 'Quantity' },
                                   { 'data': 'Amount' },
                                   { 'data': 'Remarks' },
                                ],
                            });
                            var dataForM = msg.d.DetailsList;
                            var htMM = "";
                            for (i = 0; i < dataForM.length; i++) {
                                htMM += "<div class='segment'>";
                                htMM += "<div class='col-sm-12'><label>Model</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Model + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Unit Price</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].UnitPrice + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Quantity</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Quantity + "' /></div></div>";
                                htMM += "<div class='col-sm-12'><label>Amount</label><div><input type='text' class='form-control' disabled='disabled' value='" + dataForM[i].Amount + "' /></div></div>";
                               
                                htMM += "</div>";
                            }

                            $("#STBReturnRechargeM").html(htMM);
                        }, 400);

                        $("#detailsSTBReturnModal").modal('show');
                    }
                });
            }, 1000);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/warningAlert.png" class="mBot10" style="width: 70px;" />
            <h1 id="h1heading" class="red">Your Access is Denied</h1>
            <p id="pParagraph" class="red">
                You can access this section starting from <span id="spnbegin"></span>upto <span id="spnEnd"></span>
            </p>
            <button type="button" class="btn btn-sign" onclick="WorkingRosterClick()">OK</button>
        </div>
    </div>


    <span class="hddd font-pp">Approval</span>
    <div class="">
        <div class="clearfix font-pp">
            <div class="clearfix text-center ">
                <ul class="circulerUl">
                    <% if (rights.CanMRCancellation && rights.CanWRCancellation && rights.CanSTBRequisition && rights.CanHolds && rights.CanDirectorApproval && rights.CanInventoryCancellation)
                       { %>
                    <li id="d1">
                        <div class="crcl" onclick="TotalTotal();">
                            <div id="divTotal" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Total </div>
                    </li>
                    <% } %>
                    <% if (rights.CanMRCancellation)
                       { %>
                    <li id="d2">
                        <div class="crcl red" onclick="TotalMRCancelation();">
                            <div id="divMRCancelation" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div class="colRed">MR Cancel</div>
                    </li>
                    <% } %>
                    <% if (rights.CanWRCancellation)
                       { %>
                    <li id="d3">
                        <div class="crcl red" onclick="TotalWRCancelation();">
                            <div id="divWRCancelation" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div class="colRed">WR Cancel</div>
                    </li>
                    <% } %>
                    <% if (rights.CanSTBRequisition)
                       { %>
                    <li id="d5">
                        <div class="crcl" onclick="TotalSTBRequisition();">
                            <div id="divSTBRequisition" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>STB Requisition</div>
                    </li>
                    <% } %>
                    <% if (rights.CanHolds)
                       { %>
                    <li id="d4">
                        <div class="crcl" onclick="TotalHolds();">
                            <div id="divHolds" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Holds</div>
                    </li>
                    <% } %>
                    <% if (rights.CanDirectorApproval)
                       { %>
                    <li id="d7">
                        <div class="crcl" onclick="DirectorApproval();">
                            <div id="divDirectorApproval" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Dir. Approval</div>
                    </li>
                    <% } %>
                    <% if (rights.CanInventoryCancellation)
                       { %>
                    <li id="d6">
                        <div class="crcl red" onclick="TotalInventoryCancelation();">
                            <div id="divInventoryCancelation" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div class="colRed">Inv. Cancel</div>
                    </li>
                    <% } %>
                    <li id="d8">
                        <div class="crcl" onclick="TotalReturnRequisition();">
                            <div id="divReturnRequisition" runat="server">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Ret. Requisition</div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="clearfix">
            <div id="filterToggle" class=" boxModel clearfix mBot10 mTop5 font-pp" style="background: #f9f9f9">
                <span class="togglerSlidecut"><i class="fa fa-times-circle"></i></span>
                <div class="row">
                    <div class="col-md-2">
                        <label>From Date</label>
                        <div class="relative">
                            <div class="input-group date" data-provide="datepicker">
                                <input type="text" class="form-control" id="FromDate" style="height: 28px;" />
                                <div class="input-group-addon">
                                    <span class="fa fa-calendar-check-o"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>To Date</label>
                        <div class="relative">
                            <div class="input-group date" data-provide="datepicker">
                                <input type="text" class="form-control" id="ToDate" style="height: 28px;" />
                                <div class="input-group-addon">
                                    <span class="fa fa-calendar-check-o"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>Location</label>
                        <div>
                            <input type="text" class="form-control" />
                        </div>
                    </div>

                    <div class="col-md-2">
                        <label>Technician name</label>
                        <div>
                            <input type="text" class="form-control" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>&nbsp</label>
                        <div>
                            <button class="btn btn-success" type="button" onclick="SearchClick();" style="margin-top: -2px;">Search</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="mtop8" id="LodingId">
            <div class="clearfix">
                <div class="relative">
                    <span class="togglerSlide btn btn-warning hide" style="position: absolute; right: 99px; z-index: 10;" data-toggle="tooltip" data-placement="top" title="Filter"><i class="fa fa-filter"></i></span>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                        Style="position: absolute; right: 0;">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                    </asp:DropDownList>

                </div>
                <ul class="nav nav-tabs" id="myTab" role="tablist">
                    <li class="nav-item active" id="LiTotalJob">
                        <a class="nav-link " id="home-tab" data-toggle="tab" href="#home" role="tab" onclick="TotalJob();" aria-controls="home" aria-selected="true">Total</a>
                    </li>

                </ul>
                <div class="tab-content mBot10" id="myTabContent">
                    <div class="tab-pane fade in active" id="home" role="tabpanel" aria-labelledby="home-tab">
                        <div class="clearfix relative">
                            <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="cgridStatus" DataSourceID="EntityServerModeDataSource" KeyFieldName="ID" WIDTH="100%"
                                SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" runat="server"
                                Settings-VerticalScrollableHeight="200" Settings-HorizontalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto"
                                AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true">
                                <settingssearchpanel visible="True" delay="5000" />

                                <columns>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="DocumentNumber"  Width="150px"
                                Caption="Document No." >
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="DocDate" Width="100px"
                                Caption="Doc Date">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DocType" Width="150px"
                                Caption="Doc Type">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ApprovalType"
                                Caption="Approval Type" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="EntityCode" Width="150px"
                                Caption="Entity Code" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="NetworkName" Width="250px"
                                Caption="Network Name" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ContactPerson" Width="200px"
                                Caption="Contact Person" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ContactNo" Width="150px"
                                Caption="Contact No" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="branch_description" Width="200px"
                                Caption="Location" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Enteredby" Width="200px"
                                Caption="Entered by" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="EnteredOn" Width="150px"
                                Caption="Entered on" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="CancelStatus" Width="200px"
                                Caption="Status" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
  
                            <dxe:GridViewDataTextColumn Caption="Cancel Reason" FieldName="CancelReason" VisibleIndex="12" Width="200px">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="Hold Reason" FieldName="HoldReason" VisibleIndex="13" Width="200px">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="16" Width="1px">
                        <DataItemTemplate>
                           <div class="floatedIcons">
                            <div class='floatedBtnArea'>
                            <% if (rights.CanApproved)
                               { %>
                          <%--  <a href="javascript:void(0);" onclick="ClickCancel('<%# Container.KeyValue %>')" id="a_viewInvoice" class="" title="">
                                <span class='ico deleteColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Reject</span></a>--%>
                                <a href="javascript:void(0);" onclick="ClickCancel('<%# Container.KeyValue %>')" id="a_CANCELInvoice" class="pad" title="Reject" style='<%#Eval("MRWRApprove")%>'>
                                <span class='ico deleteColor'><i class='fa fa-close' aria-hidden='true'></i></span><span class='hidden-xs'>Reject</span></a>
                            <%} %>
                            <% if (rights.CanApproved)
                               { %>
                                <a href="javascript:void(0);" onclick="ClickOnApprove('<%# Container.KeyValue %>')" id="a_APPROVEInvoice" class="pad " title="Approve" style='<%#Eval("MRWRApprove")%>'>
                                    <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve</span></a>
                                
                            <%} %>
                                <% if (rights.CanApproved)
                                   { %>
                                 <a href="javascript:void(0);" onclick="ClickOnApproval('<%# Container.KeyValue %>','<%#Eval("CancelStatus")%>')" id="a_ApprovalSTBReq" class="pad" title="Approve" style='<%#Eval("STBReqApproval")%>'>
                                    <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approval</span></a>
                                <%--<a href="javascript:void(0);" onclick="ClickOnApproveM('<%# Container.KeyValue %>','<%#Eval("CancelStatus")%>')" id="a_ApprovalSTBReqM" class="pad onlyMobileLink" title="Approve" style='<%#Eval("STBReqApproval")%>'>
                                    <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approval</span></a>--%>
                                  <%} %>

                                <a href="javascript:void(0);" onclick="ClickOnViewDetails('<%# Container.KeyValue %>')" id="a_ViewDetails" class="pad" title="View Details" >
                                    <span class='ico editColor'><i class='fa fa-eye dat' aria-hidden='true'></i></span><span class='hidden-xs'>View Details</span></a>
                               </div>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>                     

                        </columns>
                                <clientsideevents rowclick="gridRowclick" />
                                <settingscontextmenu enabled="true"></settingscontextmenu>
                                <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>
                                <settingssearchpanel visible="True" />
                                <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />

                                <settingspager numericbuttoncount="1" pagesize="10" showseparators="false" mode="ShowPager">
                             <PageSizeItemSettings Visible="false" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="false">
                            </FirstPageButton>
                            <LastPageButton Visible="false">
                            </LastPageButton>
                        </settingspager>
                                <settingsbehavior columnresizemode="NextColumn" confirmdelete="True" />
                            </dxe:ASPxGridView>

                            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                ContextTypeName="ServicveManagementDataClassesDataContext" TableName="V_STB_ApprovalList" />
                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                            </dxe:ASPxGridViewExporter>
                        </div>
                    </div>
                    <div class="tab-pane fade in active" id="profile" role="tabpanel" aria-labelledby="home-tab"></div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfBranchID" runat="server" />
    <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
    <asp:HiddenField ID="hddnKeyValue" runat="server" />
    <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />

    <asp:HiddenField ID="hdnFilterby" runat="server" />

    <asp:HiddenField ID="hdnUserType" runat="server" />
    <asp:HiddenField ID="hdnSearchType" runat="server" />

    <asp:HiddenField ID="hdnmodule_ID" runat="server" />

    <asp:HiddenField ID="hdnFilterByStatus" runat="server" />

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <div class="modal fade pmsModal w30" id="CancelDocumentPopUp" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Cancel Document</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtCancelDocumentRemarks" maxlength="500" rows="5"></textarea>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success" onclick="CancelDocument();">Confirm</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w30" id="ApproveDocumentpop" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Approve Document</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtApproveDocumentRemarks" maxlength="500" rows="5"></textarea>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success" onclick="ApproveDocument();">Confirm</button>
                </div>
            </div>
        </div>
    </div>

    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ClientInstanceName="popup"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="630px"
        Width="1200px" HeaderText="STB Requisition" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <headertemplate>
                <span>Approval</span>
                <div class="closeApprove" onclick="closeUserApproval();"><i class="fa fa-close"></i></div>
            </headertemplate>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </contentcollection>
    </dxe:ASPxPopupControl>


    <div class="modal fade pmsModal w90" id="detailsModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">View Money Receipt</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-3">
                            <label>Document Number <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDocumentNumber" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Location <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtLocation" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Date <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDate" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Type</label>
                            <div>
                                <input type="text" id="txtType" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-3">
                            <label>Entity Code <span class="red">*</span></label>
                            <div>
                                <input type="text" id="EntityCode" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Network Name <span class="red">*</span></label>
                            <div>
                                <input type="text" id="NetworkName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Person <span class="red">*</span></label>
                            <div>
                                <input type="text" id="ContactPerson" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Number <span class="red">*</span></label>
                            <div>
                                <input type="text" id="ContactNumber" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="clearfix onlyMobileDiv" id="">
                        <div class="col-sm-12">
                            <label>Payment Mode <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mPaymentMode" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Amount <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mAmount" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Cheque No <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mChecqueNo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Cheque Date <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mChecqueDate" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Ref No <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mRefNo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Bank Name <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mBankName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Branch Name <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mBranchName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <label>Remarks <span class="red">*</span></label>
                            <div>
                                <input type="text" id="mRemarks" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row onlyMonitorDiv">
                        <div id="divReceiptChallanDtls" class="onlyMonitor">
                        </div>
                    </div>
                    <br />
                </div>
                <div class="headerPy hide" id="DivReceiptCancelDetails">Cancellation Details</div>
                <div id="DivReceiptCancelData" class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px">


                    <div class="row ">
                        <div class="col-md-6">
                            <label class="red">Reason For Cancellation Request:</label>
                            <div>
                                <input type="text" class="form-control" id="txtReceiptCancellationReason" maxlength="500" runat="server" disabled />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <label class="red">Cancellation Request Made By:</label>
                            <div>
                                <input type="text" class="form-control" id="txtReceiptCancelRequestBY" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="red">Cancellation Request Made On:</label>
                            <div>
                                <input type="text" class="form-control" id="txtReceiptCancelRequestON" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w90" id="detailsWalletModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleWalletModalLabel">View Wallet Recharge</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-3">
                            <label>Document Number <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDocumentNumberWallet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Location <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtLocationWallet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Date <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDateWallet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Type</label>
                            <div>
                                <input type="text" id="txtTypeWallet" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-3">
                            <label>Entity Code <span class="red">*</span></label>
                            <div>
                                <input type="text" id="EntityCodeWallet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Network Name <span class="red">*</span></label>
                            <div>
                                <input type="text" id="NetworkNameWallet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Person <span class="red">*</span></label>
                            <div>
                                <input type="text" id="ContactPersonWallet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Number <span class="red">*</span></label>
                            <div>
                                <input type="text" id="ContactNumberWallet" class="form-control" disabled />
                            </div>
                        </div>
                    </div>

                    <hr />
                    <div class="clearfix row onlyMobileDiv" id="walleTViewPop">
                        
                    </div>
                    <div class="clearfix onlyMonitorDiv">
                        <div id="divWalletRechargeDtls">
                        </div>
                    </div>
                    <br />
                </div>
                <div class="headerPy hide" id="DivCancelDetails">Cancellation Details</div>
                <div id="DivCancelData" class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px">


                    <div class="row ">
                        <div class="col-md-6">
                            <label class="red">Reason For Cancellation Request:</label>
                            <div>
                                <input type="text" class="form-control" id="txtReasonCancel" maxlength="500" runat="server" disabled />
                            </div>
                        </div>

                        <div class="col-md-3 trY">
                            <label class="red">Cancellation Request Made By:</label>
                            <div>
                                <input type="text" class="form-control" id="txtReasonCancelMadeBy" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                        <div class="col-md-3 trY">
                            <label class="red">Cancellation Request Made On:</label>
                            <div>
                                <input type="text" class="form-control" id="txtReasonCancelMadeOn" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w90" id="detailsSTBReqModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleSTBReqModalLabel">View STB Requisition</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Document Number </label>
                            <div>
                                <input type="text" id="txtDocumentNumberSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Location </label>
                            <div>
                                <input type="text" id="txtLocationSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Date </label>
                            <div>
                                <input type="text" id="txtDateSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Requisition Type</label>
                            <div>
                                <input type="text" id="txtRequisitionTypeSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Requisition For</label>
                            <div>
                                <input type="text" id="txtRequisitionForSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Entity Code </label>
                            <div>
                                <input type="text" id="txtEntityCodeSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Network Name </label>
                            <div>
                                <input type="text" id="txtNetworkNameSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Person </label>
                            <div>
                                <input type="text" id="txtContactPersonSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Number </label>
                            <div>
                                <input type="text" id="txtContactNumberSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>DAS </label>
                            <div>
                                <input type="text" id="txtDASSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="clearfix ">
                        <div id="divSTBReqRechargeDtls">
                        </div>
                    </div>
                    <br />
                    <div class="clearfix ">
                        <div id="divSTBReqRechargeDtls2">
                        </div>
                    </div>
                </div>
                <div class="headerPy" id="DivSTBReqCancelDetails">Payment Details</div>
                <div id="DivSTBReqCancelData" class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px">
                    <div class="row ">
                        <div class="col-md-3">
                            <label class="">Pay using on Acount Balance: </label>
                            <span id="spnPayUsingAcountBalance"></span>
                        </div>

                        <div class="col-md-2 trY">
                            <label class="">No Payment (Due): </label>
                            <span id="spnNoPayment"></span>
                        </div>
                        <div class="col-md-6 trY">
                            <label class="">Payment related remarks/notes:</label>
                            <div>
                                <input type="text" class="form-control" id="txtPaymentRelatedRemarks" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w90" id="detailsSTBReturnModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleSTBReturnModalLabel">View Return Requisition</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Document Number </label>
                            <div>
                                <input type="text" id="txtDocumentNumberSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Location </label>
                            <div>
                                <input type="text" id="txtLocationSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Date </label>
                            <div>
                                <input type="text" id="txtDateSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Requisition Type</label>
                            <div>
                                <input type="text" id="txtRequisitionTypeSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Requisition For</label>
                            <div>
                                <input type="text" id="txtRequisitionForSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Entity Code </label>
                            <div>
                                <input type="text" id="txtEntityCodeSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Network Name </label>
                            <div>
                                <input type="text" id="txtNetworkNameSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Person </label>
                            <div>
                                <input type="text" id="txtContactPersonSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Number </label>
                            <div>
                                <input type="text" id="txtContactNumberSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>DAS </label>
                            <div>
                                <input type="text" id="txtDASSTBReturn" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="clearfix onlyMobileDiv" id="STBReturnRechargeM"></div>
                    <div class="clearfix onlyMonitorDiv">
                        <div id="divSTBReturnRechargeDtls">
                        </div>
                    </div>
                    <br />
                </div>
                <div class="headerPy" id="DivSTBReturnCancelDetails">Payment Details</div>
                <div id="DivSTBReturnCancelData" class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px">
                    <div class="row ">
                        <div class="col-md-3">
                            <label class="">Pay using on Acount Balance: </label>
                            <span id="spnPayUsingAcountBalanceReturn"></span>
                        </div>

                        <div class="col-md-2 trY">
                            <label class="">No Payment (Due): </label>
                            <span id="spnNoPaymentReturn"></span>
                        </div>
                        <div class="col-md-6 trY">
                            <label class="">Payment related remarks/notes:</label>
                            <div>
                                <input type="text" class="form-control" id="txtRemarksReturn" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


<!-- Modal -->
<div id="approveMobile" class="modal fade pmsModal w90" role="dialog">
  <div class="modal-dialog">
    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Mobile Approve</h4>
      </div>
      <div class="modal-body">
        <iframe style="border: none;width: 100%;height: 71vh;" id="mApproveIframe" title="description"></iframe>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>


    <!-- Modal -->


</asp:Content>
