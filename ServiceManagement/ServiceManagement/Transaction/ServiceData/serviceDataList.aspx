<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="serviceDataList.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.ServiceData.serviceDataList" %>

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

    <script src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>

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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 2
                }
            });
            $('.datepicker').datepicker({
                format: 'mm/dd/yyyy'
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
            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy'
            }).datepicker('update', new Date());
        });
    </script>
    <style>
        .circulerUl {
            display: inline-block;
            list-style-type: none;
            margin: 0;
            padding: 0;
            margin-top: -27px;
            margin-bottom: -27px;
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

                .circulerUl > li:nth-child(2) .crcl {
                    border: 5px solid #39a961;
                    color: #1b773c;
                }

                .circulerUl > li:nth-child(2) {
                    color: #1b773c;
                }

                .circulerUl > li:nth-child(3) .crcl {
                    border: 5px solid #e81b1b;
                    color: #e81b1b;
                }

                .circulerUl > li:nth-child(3) {
                    color: #e81b1b;
                }

        #divListData .dt-buttons {
            display: none;
        }
        #FnAcceptRejectIframe {
            width:100%;
            border:none;
            min-height:350px
        }
    </style>

    <script>

        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "serviceDataList.aspx/CountServiceEntry",
                data: JSON.stringify({ UserType: $("#hdnUserType").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $("#divTotalAssigned").text(msg.d.TOTAL);
                    $("#divServiceEntered").text(msg.d.Assigned);
                    $("#divServicePending").text(msg.d.Unassigned);
                    //Mantis Issue 24665
                    $("#divRepairPending").text(msg.d.RepairPending);
                    //End of Mantis Issue 24665
                }
            });
            var returnvalue = true;
        });

        function SearchClick() {
            LoadingPanel.Show();
            var brnch = "";//gridbranchLookup.gridView.GetSelectedKeysOnPage();
            var branchss = "";
            //for (var i = 0; i < brnch.length; i++) {
            //    branchss += ',' + brnch[i];
            //}

            var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
            var Technician = "";
            //for (var i = 0; i < Tech.length; i++) {
            //    if (Technician == "") {
            //        Technician = Tech[i];
            //    }
            //    else {
            //        Technician += ',' + Tech[i];
            //    }
            //}

            // if ($("#hdnUserType").val() == "Technician") {
            var SearchType = "";
            if ($("#hdnSearchType").val() == "") {
                SearchType = "TotalAssigned"
            }
            else {
                SearchType = $("#hdnSearchType").val();
            }


            gridbranchLookup.gridView.GetSelectedFieldValues("ID", function (val) {
                brnch = val

                for (var i = 0; i < brnch.length; i++) {
                    if (branchss == "") {
                        branchss = brnch[i];
                    }
                    else {
                        branchss += ',' + brnch[i];
                    }
                }
                gridTechnicianLookup.gridView.GetSelectedFieldValues("cnt_internalId", function (val) {
                    Tech = val

                    for (var i = 0; i < Tech.length; i++) {
                        if (Technician == "") {
                            Technician = Tech[i];
                        }
                        else {
                            Technician += ',' + Tech[i];
                        }
                    }

                    var Apply = {
                        FromDate: $("#FromDate").val(),
                        ToDate: $("#ToDate").val(),
                        Branch: branchss,//$("#ddlBranch").val(),
                        SearchType: SearchType,
                        Technician_ID: Technician//$("#ddlTechnician").val()
                    }

                    $.ajax({
                        type: "POST",
                        url: "serviceDataList.aspx/TotalSearchJob",
                        data: JSON.stringify({ model: Apply }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            $("#divTotalAssigned").text(msg.d.TOTAL);
                            $("#divServiceEntered").text(msg.d.Assigned);
                            $("#divServicePending").text(msg.d.Unassigned);
                            //Mantis Issue 24665
                            $("#divRepairPending").text(msg.d.RepairPending);
                            //End of Mantis Issue 24665

                            var status = "";
                            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                            status = status + " <thead><tr>";
                            status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th>";
                            status = status + " <th>Contact Person</th><th>Technician </th><th>Location </th><th>Received by</th>";
                            //Rev work start 22.06.2022  mantise no:0024978
                            //status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Serv. Entered By</th>";
                            status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Repairing Status</th><th>Repaired On</th><th>Serv. Entered By</th>";
                            //Rev work close 22.06.2022 mantise no:0024978
                            status = status + " <th>Serv. Entered On</th><th>Status</th><th>Action</th></tr></thead>";

                            status = status + " </table>";

                            $('#divListData').html(status);

                            $('#dataTable').DataTable({
                                scrollX: true,
                                fixedColumns: {
                                    rightColumns: 2
                                },
                                data: msg.d.DetailsList,
                                columns: [
                                   { 'data': 'ReceiptChallan' },
                                   { 'data': 'Type' },
                                   { 'data': 'EntityCode' },
                                   { 'data': 'NetworkName' },
                                   { 'data': 'ContactPerson' },
                                   { 'data': 'Technician' },
                                   { 'data': 'Location' },
                                   { 'data': 'Receivedby' },
                                   { 'data': 'Receivedon' },
                                   { 'data': 'Assignedby' },
                                   { 'data': 'Assignedon' },
                                   //Rev work start 22.06.2022  mantise no:0024978
                                   { 'data': 'RepairStatus' },
                                   { 'data': 'Repair_date' },
                                   //Rev work close 22.06.2022 mantise no:0024978
                                   { 'data': 'ServEnteredBy' },
                                   { 'data': 'ServEnteredOn' },
                                   { 'data': 'Status' },
                                   { 'data': 'Action' },
                                ],
                                dom: 'Bfrtip',
                                buttons: [
                                    {
                                        extend: 'excel',
                                        title: null,
                                        filename: 'Service Entry',
                                        text: 'Save as Excel',
                                        customize: function (xlsx) {
                                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                            $('row:first c', sheet).attr('s', '42');
                                        },

                                        exportOptions: {
                                        }
                                    }
                                ],
                                error: function (error) {
                                    alert(error);
                                    LoadingPanel.Hide();
                                }
                            });

                            LoadingPanel.Hide();
                        }
                    });
                });
            });
            // }
        }

        function TotalAssigned() {
            LoadingPanel.Show();
            $("#hdnSearchType").val("TotalAssigned");
            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d1').addClass('activeCrcl');

            $.ajax({
                type: "POST",
                url: "serviceDataList.aspx/TotalServiceEntry",
                data: JSON.stringify({ model: $("#hdnUserType").val(), SearchType: "TotalAssigned" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#divTotalAssigned").text(msg.d.TOTAL);
                    $("#divServiceEntered").text(msg.d.Assigned);
                    $("#divServicePending").text(msg.d.Unassigned);
                    //Mantis Issue 24665
                    $("#divRepairPending").text(msg.d.RepairPending);
                    //End of Mantis Issue 24665

                    var status = "";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th>";
                    status = status + " <th>Contact Person</th><th>Technician </th><th>Location </th><th>Received by</th>";
                    status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Serv. Entered By</th>";
                    status = status + " <th>Serv. Entered On</th><th>Status</th><th>Action</th></tr></thead>";

                    status = status + " </table>";

                    $('#divListData').html(status);
                    
                    $('#dataTable').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 2
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'ReceiptChallan' },
                           { 'data': 'Type' },
                           { 'data': 'EntityCode' },
                           { 'data': 'NetworkName' },
                           { 'data': 'ContactPerson' },
                           { 'data': 'Technician' },
                           { 'data': 'Location' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Assignedby' },
                           { 'data': 'Assignedon' },
                           { 'data': 'ServEnteredBy' },
                           { 'data': 'ServEnteredOn' },
                           { 'data': 'Status' },
                           { 'data': 'Action' },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Service Entry',
                                text: 'Save as Excel',
                                customize: function (xlsx) {
                                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                    $('row:first c', sheet).attr('s', '42');
                                },

                                exportOptions: {
                                }
                            }
                        ],
                        error: function (error) {
                            alert(error);
                            LoadingPanel.Hide();
                        }
                    });
                    LoadingPanel.Hide();
                }
            });
        }

        function TotalEntered() {
            LoadingPanel.Show();
            $("#hdnSearchType").val("TotalEntered");
            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d2').addClass('activeCrcl');

            $.ajax({
                type: "POST",
                url: "serviceDataList.aspx/TotalServiceEntry",
                data: JSON.stringify({ model: $("#hdnUserType").val(), SearchType: "TotalEntered" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#divTotalAssigned").text(msg.d.TOTAL);
                    $("#divServiceEntered").text(msg.d.Assigned);
                    $("#divServicePending").text(msg.d.Unassigned);
                    //Mantis Issue 24665
                    $("#divRepairPending").text(msg.d.RepairPending);
                    //End of Mantis Issue 24665

                    var status = "";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th>";
                    status = status + " <th>Contact Person</th><th>Technician </th><th>Location </th><th>Received by</th>";
                    status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Serv. Entered By</th>";
                    status = status + " <th>Serv. Entered On</th><th>Status</th><th>Action</th></tr></thead>";

                    status = status + " </table>";

                    $('#divListData').html(status);

                    $('#dataTable').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 2
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'ReceiptChallan' },
                           { 'data': 'Type' },
                           { 'data': 'EntityCode' },
                           { 'data': 'NetworkName' },
                           { 'data': 'ContactPerson' },
                           { 'data': 'Technician' },
                           { 'data': 'Location' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Assignedby' },
                           { 'data': 'Assignedon' },
                           { 'data': 'ServEnteredBy' },
                           { 'data': 'ServEnteredOn' },
                           { 'data': 'Status' },
                           { 'data': 'Action' },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Service Entry',
                                text: 'Save as Excel',
                                customize: function (xlsx) {
                                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                    $('row:first c', sheet).attr('s', '42');
                                },

                                exportOptions: {
                                }
                            }
                        ],
                        error: function (error) {
                            alert(error);
                            LoadingPanel.Hide();
                        }
                    });
                    LoadingPanel.Hide();
                }
            });
        }

        function TotalUnassigned() {
            LoadingPanel.Show();
            $("#hdnSearchType").val("TotalUnassigned");
            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d3').addClass('activeCrcl');

            $.ajax({
                type: "POST",
                url: "serviceDataList.aspx/TotalServiceEntry",
                data: JSON.stringify({ model: $("#hdnUserType").val(), SearchType: "TotalUnassigned" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#divTotalAssigned").text(msg.d.TOTAL);
                    $("#divServiceEntered").text(msg.d.Assigned);
                    $("#divServicePending").text(msg.d.Unassigned);
                    //Mantis Issue 24665
                    $("#divRepairPending").text(msg.d.RepairPending);
                    //End of Mantis Issue 24665

                    var status = "";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th>";
                    status = status + " <th>Contact Person</th><th>Technician </th><th>Location </th><th>Received by</th>";
                    //Mantis Issue 24840
                    //status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Serv. Entered By</th>";
                    status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Repairing Status</th><th>Repaired On</th><th>Serv. Entered By</th>";
                    status = status + " <th>Serv. Entered On</th><th>Status</th><th>Action</th></tr></thead>";
                    //End of Mantis Issue 24840
                    status = status + " </table>";

                    $('#divListData').html(status);

                    $('#dataTable').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 2
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'ReceiptChallan' },
                           { 'data': 'Type' },
                           { 'data': 'EntityCode' },
                           { 'data': 'NetworkName' },
                           { 'data': 'ContactPerson' },
                           { 'data': 'Technician' },
                           { 'data': 'Location' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Assignedby' },
                           { 'data': 'Assignedon' },
                           //Mantis Issue 24840
                           { 'data': 'RepairStatus' },
                           { 'data': 'Repair_date' },
                           //End of Mantis Issue 24840
                           { 'data': 'ServEnteredBy' },
                           { 'data': 'ServEnteredOn' },
                           { 'data': 'Status' },
                           
                           { 'data': 'Action' },

                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Service Entry',
                                text: 'Save as Excel',
                                customize: function (xlsx) {
                                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                    $('row:first c', sheet).attr('s', '42');
                                },

                                exportOptions: {
                                }
                            }
                        ],
                        error: function (error) {
                            alert(error);
                            LoadingPanel.Hide();
                        }
                    });
                    LoadingPanel.Hide();
                }
            });
        }
        //Mantis Issue 24665
        
        function TotalRepairingPending() {
            LoadingPanel.Show();
            $("#hdnSearchType").val("TotalRepairingPending");
            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d4').addClass('activeCrcl');

            $.ajax({
                type: "POST",
                url: "serviceDataList.aspx/TotalServiceEntry",
                data: JSON.stringify({ model: $("#hdnUserType").val(), SearchType: "TotalRepairingPending" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#divTotalAssigned").text(msg.d.TOTAL);
                    $("#divServiceEntered").text(msg.d.Assigned);
                    $("#divServicePending").text(msg.d.Unassigned);
                    //Mantis Issue 24665
                    $("#divRepairPending").text(msg.d.RepairPending);
                    //End of Mantis Issue 24665

                    var status = "";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th>";
                    status = status + " <th>Contact Person</th><th>Technician </th><th>Location </th><th>Received by</th>";
                    status = status + " <th>Received on</th><th>Assigned by</th><th>Assigned on</th><th>Serv. Entered By</th>";
                    status = status + " <th>Serv. Entered On</th><th>Status</th><th>Action</th></tr></thead>";

                    status = status + " </table>";

                    $('#divListData').html(status);
                    console.log('d', msg.d.DetailsList)
                    $('#dataTable').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 2
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'ReceiptChallan' },
                           { 'data': 'Type' },
                           { 'data': 'EntityCode' },
                           { 'data': 'NetworkName' },
                           { 'data': 'ContactPerson' },
                           { 'data': 'Technician' },
                           { 'data': 'Location' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Assignedby' },
                           { 'data': 'Assignedon' },
                           { 'data': 'ServEnteredBy' },
                           { 'data': 'ServEnteredOn' },
                           { 'data': 'Status' },
                           { 'data': 'Action' },

                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Service Entry',
                                text: 'Save as Excel',
                                customize: function (xlsx) {
                                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                    $('row:first c', sheet).attr('s', '42');
                                },

                                exportOptions: {
                                }
                            }
                        ],
                        error: function (error) {
                            alert(error);
                            LoadingPanel.Hide();
                        }
                    });
                    LoadingPanel.Hide();
                }
            });
        }
        //End of Mantis Issue 24665

        function AssignServiceEntry(values) {
            location.href = "serviceDataEntry.aspx?key=Add&id=" + values;
        }

        function Edit(values) {
            checkWarranty(values);
            if (returnvalue) {
                location.href = "serviceDataEntry.aspx?key=Edit&id=" + values;
            }
            else {
                jAlert("Warranty Date is updated. Cannot Edit.");
                returnvalue = true;
            }
        }

        function View(values) {
            location.href = "serviceDataEntry.aspx?key=View&id=" + values;
        }

        function checkWarranty(values) {
            returnvalue = true;
            $.ajax({
                type: "POST",
                url: "serviceDataList.aspx/CheckTagInWarranty",
                data: JSON.stringify({ ReceptID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "true") {
                        returnvalue = false;
                    }
                    else {
                        returnvalue = true;
                    }
                },
                error: function (response) {
                    console.log(response);
                    returnvalue = false;
                }
            });
        }

        function Delete(values) {

            jConfirm('Confirm Delete?', 'Alert', function (r) {
                if (r) {
                    checkWarranty(values);;
                    if (returnvalue) {
                        $.ajax({
                            type: "POST",
                            url: "serviceDataList.aspx/DeleteServiceEntry",
                            data: JSON.stringify({ ReceptID: values }),
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                console.log(response);
                                if (response.d) {
                                    if (response.d.split('~')[1] == "Success") {
                                        jAlert(response.d.split('~')[0], "Alert", function () {
                                            TotalEntered();
                                        });
                                    }
                                    else {
                                        jAlert(response.d.split('~')[0]);
                                        return
                                    }
                                }
                            },
                            error: function (response) {
                                console.log(response);
                            }
                        });
                    }
                    else {
                        jAlert("Warranty Date is updated. Cannot Delete.");
                        returnvalue = true;
                    }
                }
            });
        }

        $(function () {
            cBranchPanel.PerformCallback('BindComponentGrid' + '~' + "All");
            cTechnicianPanel.PerformCallback('BindTechnicianGrid' + '~' + "All");
        });

        function selectAllBranch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAllBranch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function CloseBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAllTechnician() {
            gridTechnicianLookup.gridView.SelectRows();
        }
        function unselectAllTechnician() {
            gridTechnicianLookup.gridView.UnselectRows();
        }
        function CloseTechnicianLookup() {
            gridTechnicianLookup.ConfirmCurrentSelection();
            gridTechnicianLookup.HideDropDown();
            gridTechnicianLookup.Focus();
        }

        function SendSMS(val) {
            jConfirm('Confirm Send SMS?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "serviceDataList.aspx/SendSMS",
                        data: JSON.stringify({ ReceiptChallan_ID: val }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // console.log(response);
                            if (response.d) {

                            }
                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                }
            });
        }
        //Mantis Issue 24665
        function SendTechSMS(val) {
            jConfirm('Confirm Send SMS?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "serviceDataList.aspx/SendTechnicianSMS",
                        data: JSON.stringify({ ReceiptChallan_ID: val }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // console.log(response);
                            if (response.d) {

                            }
                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                }
            });
        }
        function UnAssignJob(values) {
            var recept_id = values;
            jConfirm('Confirm Unassign?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "serviceDataList.aspx/UnAssign",
                        data: JSON.stringify({ ReceiptChallan_ID: values }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            //console.log(response);
                            if (response.d) {
                                if (response.d == "true") {
                                    jAlert("Unassign successfully.", "Alert", function () {
                                        //TotalJob();
                                        window.location.reload();
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
            });
            
        }
        function FnAcceptReject(values) {
            var TechId = 0;
            //Mantis Issue 25172
            //var UniqueKey = "GTPL_SRV";
            var UniqueKey = $("#hdnDbName").val();
            //End of Mantis Issue 25172
            var pageUrl = "TechnicianAssign.aspx?id=" + values + "&&AU=" + TechId + "&&UniqueKey=" + UniqueKey + "&&ispopup=true";
            $("#FnAcceptReject").modal('show');
            $("#FnAcceptRejectIframe").attr('src', pageUrl)
            
        }
        //End of Mantis Issue 24665

        function ExportChange() {
            if ($("#drdExport").val() == "1") {
                $('#dataTable').DataTable().button('.buttons-pdf').trigger();
            }
            if ($("#drdExport").val() == "2") {
                $('#dataTable').DataTable().button('.buttons-excel').trigger();
            }
            if ($("#drdExport").val() == "4") {
                $('#dataTable').DataTable().button('.buttons-csv').trigger();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="modal fade pmsModal w40" id="FnAcceptReject" tabindex="-1" role="dialog" aria-labelledby="FnAcceptRejectLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Repairing Action</h5>
           <%-- <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>--%>
          </div>
          <div class="modal-body">
            <iframe id="FnAcceptRejectIframe"></iframe>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="TotalRepairingPending();">Close</button>
          </div>
        </div>
      </div>
    </div>


    <span class="hddd font-pp">Service Entry</span>
    <div class="clearfix font-pp">
        <div class="clearfix text-center ">
            <ul class="circulerUl">
                <li id="d1">
                    <%--Mantis Issue 24893--%>
                    <%--<div class="crcl" onclick="TotalAssigned();">
                        <div id="divTotalAssigned">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Total Assigned</div>--%>
                    <% if (rights.CanTotalAssigned)
                      { %>
                    <div class="crcl" onclick="TotalAssigned();">
                        <div id="divTotalAssigned">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Total Assigned</div>
                    <% } %>
                </li>
                <%--End of Mantis Issue 24893--%>
                <%--Mantis Issue 25172--%>
                <li id="d4">
                    <%--Mantis Issue 24893--%>
                    <%--<div class="crcl" onclick="TotalRepairingPending();">
                        <div id="divRepairPending">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Repairing Pending</div>--%>
                    <% if (rights.CanRepairingPending)
                      { %>
                    <div class="crcl" onclick="TotalRepairingPending();">
                        <div id="divRepairPending">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Repairing Pending</div>
                    <% } %>
                    <%--End of Mantis Issue 24893--%>
                </li>
                <%--End of Mantis Issue 25172--%>
                <li id="d2">
                    <%--Mantis Issue 24893--%>
                    <%--<div class="crcl" onclick="TotalEntered();">
                        <div id="divServiceEntered">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Service Entered</div>--%>
                    <% if (rights.CanServiceEntered)
                      { %>
                    <div class="crcl" onclick="TotalEntered();">
                        <div id="divServiceEntered">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Service Entered</div>
                    <% } %>
                    <%--End of Mantis Issue 24893--%>
                </li>
                <li id="d3">
                    <%--Mantis Issue 24893--%>
                    <%--<div class="crcl" onclick="TotalUnassigned();">
                        <div id="divServicePending">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Service Pending</div>--%>
                    <% if (rights.CanServicePending)
                      { %>
                    <div class="crcl" onclick="TotalUnassigned();">
                        <div id="divServicePending">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div>Service Pending</div>
                    <% } %>
                    <%--End of Mantis Issue 24893--%>
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
                    <div class="relative">
                        <%-- <asp:DropDownList ID="ddlBranch" runat="server" CssClass="js-example-basic-single" DataTextField="branch_description" DataValueField="branch_id" Width="100%">
                        </asp:DropDownList>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="BranchPanel" ClientInstanceName="cBranchPanel" OnCallback="Componentbranch_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                OnDataBinding="lookup_branch_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllBranch" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllBranch" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseBranchLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>

                <div class="col-md-2">
                    <label>Technician name</label>
                    <div>
                        <%-- <asp:DropDownList ID="ddlTechnician" runat="server" CssClass="js-example-basic-single" DataTextField="cnt_firstName" DataValueField="cnt_id" Width="100%">
                        </asp:DropDownList>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="TechnicianPanel" ClientInstanceName="cTechnicianPanel" OnCallback="Technician_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Technician" SelectionMode="Multiple" runat="server" ClientInstanceName="gridTechnicianLookup"
                                OnDataBinding="lookup_Technician_DataBinding"
                                KeyFieldName="cnt_internalId" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="cnt_firstName" Visible="true" VisibleIndex="1" width="200px" Caption="Technician(s)" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton12" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllTechnician" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton13" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllTechnician" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton14" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseTechnicianLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
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
    <div class="" id="LodingId">
        <div class="clearfix">
            <div class="relative">
                <span class="togglerSlide btn btn-warning" style="position: absolute; right: 335px; z-index: 10;" data-toggle="tooltip" data-placement="top" title="Filter"><i class="fa fa-filter"></i></span>
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" onchange="ExportChange();" Style="position: absolute; right: 240px; z-index: 10;">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                </asp:DropDownList>
                <% } %>
                <div id="divListData">
                    <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                        <thead>
                            <tr>
                                <th>Receipt Challan</th>
                                <th>Type</th>
                                <th>Entity Code </th>
                                <th>Network Name</th>
                                <th>Contact Person</th>
                                <th>Technician </th>
                                <th>Location </th>
                                <th>Received by</th>
                                <th>Received on</th>

                                <th>Assigned by</th>
                                <th>Assigned on</th>
                                <th>Serv. Entered By</th>
                                <th>Serv. Entered On</th>
                                <th>Status</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                       
                    </table>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnUserType" runat="server" />
    <asp:HiddenField ID="hdnSearchType" runat="server" />
    <%--Mantis Issue 25172--%>
    <asp:HiddenField ID="hdnDbName" runat="server" />
    <%--End of Mantis Issue 25172--%>

     <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
         </dxe:ASPxLoadingPanel>
</asp:Content>
