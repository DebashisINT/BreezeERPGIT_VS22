<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AssignedJob.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.AssignedJob" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />

    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>
    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/css/datepicker.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/js/bootstrap-datepicker.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.min.css" />
    <script src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/fixedcolumns/3.3.0/js/dataTables.fixedColumns.min.js"></script>--%>

    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/dataTables.buttons.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.flash.min.js"></script>
    <script src="/assests/pluggins/DataTable/JSZip-2.5.0/jszip.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/pdfmake.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/vfs_fonts.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.html5.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.print.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.js-example-basic-single').select2();

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
            $('#dataTable').DataTable({
                scrollX: true,
                ordering: false,
                sortable: false,
                fixedColumns: {
                    rightColumns: 2
                }
            });
            $('#dataTable2').DataTable({
                scrollX: true,
                ordering: false,
                sortable: false,
                fixedColumns: {
                    rightColumns: 2
                }
            });
            $('#dataTable3').DataTable({
            });
            $('[data-toggle="tooltip"]').tooltip();
            $(".date").datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-mm-yyyy'
            }).datepicker('update', new Date());

        });
    </script>

    <script>
        <%-- Code Added By Debashis Talukder For Document Printing End--%>
        var RCId = 0;
        function onPrintJv(id) {
            RCId = id;
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
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'ASSIGNJOB';
                window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RCId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        <%-- Code Added By Debashis Talukder For Document Printing End--%>

        $(document).ready(function () {

            if ($("#hdnUserType").val() == "Technician") {
                $("#LiTotalJob").addClass('hide');
                $("#home").addClass('hide');
                $("#spnMassAssign").addClass('hide');
                $("#TabMyJob").removeClass('hide');
                $("#home").removeClass('active');
                $("#LiTotalJob").removeClass('active');
                $("#profile").addClass('in active');
                $("#TabMyJob").addClass('in active');
                MyJob();
            }
            else {
                $("#LiTotalJob").removeClass('hide');
                $("#home").removeClass('hide');
                $("#spnMassAssign").removeClass('hide');
                $("#TabMyJob").removeClass('active');
                $("#LiTotalJob").addClass('active');
                $("#TabMyJob").addClass('hide');
                $("#home").addClass('in active');
                $("#profile").removeClass('in active');
            }

            $.ajax({
                type: "POST",
                url: "AssignedJob.aspx/CountJob",
                data: JSON.stringify({ UserType: $("#hdnUserType").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $("#divCountTotal").text(msg.d.TOTAL);
                    $("#divCountAssigned").text(msg.d.Assigned);
                    $("#divCountUnassigned").text(msg.d.Unassigned);
                }
            });

        });

        function MassAssign() {
            $.ajax({
                type: "POST",
                url: "AssignedJob.aspx/bindMassAssign",
                data: JSON.stringify({ ReceiptType: "" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    var status = "<table id='dataTable3' class='table table-striped table-bordered' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>#</th><th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Received on </th>";
                    status = status + " </tr></thead>";
                    status = status + ' <tbody>';
                    for (var i = 0; i < msg.d.length; i++) {
                        status = status + ' <tr>';
                        status = status + "<td><div class='form-check'><input class='form-check-input' type='checkbox' onclick='CheckChangesMas(" + msg.d[i].ReceiptChallan_ID + ")' value='' id='chk" + msg.d[i].ReceiptChallan_ID + "' />";
                        status = status + "<label class='form-check-label' for='defaultCheck1'></label></div></td>"
                        status = status + '<td>' + msg.d[i].DocumentNumber + '</td><td>' + msg.d[i].EntryType + '</td>';
                        status = status + '<td>' + msg.d[i].EntityCode + '</td><td>' + msg.d[i].NetworkName + '</td>';
                        status = status + '<td>' + msg.d[i].ContactPerson + '</td><td>' + msg.d[i].DocumentDate + '</td>';
                        status = status + '</tr>';
                    }
                    status = status + '</tbody> </table>';
                    $('#divMassAssign').html(status);

                }
            });
        }

        var MassAssing = [];

        function CheckChangesMas(values) {
            if ($('#chk' + values).is(':checked') == true) {
                MassAssing.push(values);
            }
            else {
                //  var itemtoRemove = "HTML";
                MassAssing.splice($.inArray(values, MassAssing), 1);
            }
            console.log(MassAssing);
        }

        function MassJobSave() {
            var Technician_ID = $("#ddlTechnicianMass").val();
            if (Technician_ID == 0) {
                jAlert("Please select Technician.");
                $("#ddlTechnicianMass").focus();

                return
            }
            var Apply = {
                RecptID: MassAssing,
                Technician_ID: Technician_ID
            }

            $.ajax({
                type: "POST",
                url: "AssignedJob.aspx/save",
                data: "{model:" + JSON.stringify(Apply) + "}",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Saved Successfully.", "Alert", function () {
                                window.location.href = "AssignedJob.aspx";
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
    </style>
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
            margin-bottom: 36px;
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

        #tabWraper {
            margin-top: -27px;
        }

        #myTab {
            border-bottom: 1px solid #24243e;
        }

            #myTab > .nav-item > a {
                font-family: 'Poppins', sans-serif !important;
                font-size: 13px;
                border: 1px solid #cec7c7;
                border-bottom: 1px solid #161336;
                background: #ffffff;
                background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
                background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
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

        .actionInput {
            text-align: center;
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

        #DivJobDetails .dt-buttons {
            display: none;
        }

        #DivMyJobDetails .dt-buttons {
            display: none;
        }
    </style>

    <script>
        function TotalJob() {
            LoadingPanel1.Show();
            var types = "TotalJob";
            var urls = "AssignedJob.aspx/bindTotalJobs";
            $("#hdnSearchType").val("TotalJob");

            if ($("#hdnUserType").val() == "Technician") {
                urls = "AssignedJob.aspx/BindMyJobs";
                types = "MyJobs";
                $("#hdnSearchType").val("MyJobs");
            }
            else {
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d1').addClass('activeCrcl');
                $('#DivSearchTechinician').removeClass('hide');
            }


            //    console.log(this);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: urls,
                    data: JSON.stringify({ Type: types }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        $("#divCountTotal").text(msg.d.TOTAL);
                        $("#divCountAssigned").text(msg.d.Assigned);
                        $("#divCountUnassigned").text(msg.d.Unassigned);

                        var status = "<div>";
                        status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>";
                        status = status + " <th>Technician </th><th>Location </th><th>Received by</th><th>Received on</th><th>Assigned by</th>";
                        status = status + " <th>Assigned on</th><th>Status</th><th>Action</th></tr></thead>";

                        //status = status + " <tbody>";
                        //for (var i = 0; i < msg.d.length; i++) {
                        //    status = status + "<tr>";
                        //    status = status + " <td>" + msg.d[i].ReceiptChallan + "</td>";
                        //    status = status + " <td>" + msg.d[i].Type + "</td><td>" + msg.d[i].EntityCode + "</td>";
                        //    status = status + " <td>" + msg.d[i].NetworkName + "</td><td>" + msg.d[i].ContactPerson + "</td>";
                        //    status = status + " <td>" + msg.d[i].Technician + "</td><td>" + msg.d[i].Location + "</td>";
                        //    status = status + " <td>" + msg.d[i].Receivedby + "</td><td>" + msg.d[i].Receivedon + "</td>";
                        //    status = status + " <td>" + msg.d[i].Assignedby + "</td><td>" + msg.d[i].Assignedon + "</td>";
                        //    if (msg.d[i].Status=="P") {
                        //        status = status + " <td><span class='badge badge-warning'>Pending</span></td>";
                        //    }
                        //    else if (msg.d[i].Status == "DU") {
                        //        status = status + " <td><span class='badge badge-danger'>Due</span></td>";
                        //    }
                        //    else if (msg.d[i].Status == "DN") {
                        //        status = status + "<td><span class='badge badge-success'>Done</span></td>";
                        //    }
                        //    else {
                        //        status = status + "<td>&nbsp;</td>";
                        //    }
                        //    status = status + " <td class='actionInput'> ";

                        //    if (msg.d[i].Status != "DU") {
                        //        status = status + " <span data-toggle='modal' data-target='#assignpop' onclick='AssignJob(" + msg.d[i].ReceiptChallan_ID + ")'><i class='fa fa-user-plus assig' data-toggle='tooltip' data-placement='left' title='Assign'></i></span>";
                        //    }

                        //    status = status + " <span data-toggle='modal' data-target='#viewDetails' onclick='ViewDetails(" + msg.d[i].ReceiptChallan_ID + ")'><i class='fa fa-eye det' data-toggle='tooltip' data-placement='left' title='Details'></i></span></td></tr>";
                        //}
                        //status = status + " </tbody>/";
                        status = status + " </table>";
                        status = status + "</div>";
                        $('#DivJobDetails').html(status);

                        //$('#dataTable').DataTable({
                        //    scrollX: true,
                        //    fixedColumns: {
                        //        rightColumns: 2
                        //    }
                        //});
                        //  alert(msg.d);
                        //  debugger;
                        $('#dataTable').DataTable({
                            scrollX: true,
                            ordering: false,
                            sortable: false,
                            //scrollY:        "400px",
                            //scrollCollapse: true,
                            //paging:         false,
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
                               { 'data': 'Status' },
                               { 'data': 'Action' },
                            ],
                            dom: 'Bfrtip',
                            buttons: [
                                {
                                    extend: 'excel',
                                    title: null,
                                    filename: 'Assign Job',
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
                                LoadingPanel1.Hide();
                            }
                        });
                        LoadingPanel1.Hide();
                    }
                });
            }, 1000);
        }

        function AssignedJob() {
            LoadingPanel1.Show();
            var types = "AssignedJob";
            var urls = "AssignedJob.aspx/bindTotalJobs";
            $("#hdnSearchType").val("AssignedJob");

            if ($("#hdnUserType").val() == "Technician") {
                urls = "AssignedJob.aspx/BindMyJobs";
                types = "MyJobs";
                $("#hdnSearchType").val("MyJobs");
            }

            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d2').addClass('activeCrcl');
            $('#DivSearchTechinician').removeClass('hide');


            $.ajax({
                type: "POST",
                url: urls,
                data: JSON.stringify({ Type: types }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#divCountTotal").text(msg.d.TOTAL);
                    $("#divCountAssigned").text(msg.d.Assigned);
                    $("#divCountUnassigned").text(msg.d.Unassigned);

                    var status = "<div>";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>";
                    status = status + " <th>Technician </th><th>Location </th><th>Received by</th><th>Received on</th><th>Assigned by</th>";
                    status = status + " <th>Assigned on</th><th>Status</th><th>Action</th></tr></thead>";

                    status = status + " </table>";
                    status = status + "</div>";
                    $('#DivJobDetails').html(status);

                    $('#dataTable').DataTable({
                        scrollX: true,
                        ordering: false,
                        sortable: false,
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
                           { 'data': 'Status' },
                           { 'data': 'Action' },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Assign Job',
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
                            LoadingPanel1.Hide();
                        }
                    });
                    LoadingPanel1.Hide();
                }
            });
        }

        function UnassignedJob() {
            LoadingPanel1.Show();
            var types = "UnassignedJob";
            var urls = "AssignedJob.aspx/BindUnassignedJob";
            $("#hdnSearchType").val("UnassignedJob");

            if ($("#hdnUserType").val() == "Technician") {
                urls = "AssignedJob.aspx/BindMyJobs";
                types = "MyJobs";
                $("#hdnSearchType").val("MyJobs");
            }
            else {
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d3').addClass('activeCrcl');
                $('#DivSearchTechinician').addClass('hide');
            }


            $.ajax({
                type: "POST",
                url: urls,
                data: JSON.stringify({ Type: types }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#divCountTotal").text(msg.d.TOTAL);
                    $("#divCountAssigned").text(msg.d.Assigned);
                    $("#divCountUnassigned").text(msg.d.Unassigned);

                    var status = "<div>";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>";
                    status = status + " <th>Location </th><th>Received by</th><th>Received on</th>";
                    status = status + " <th>Status</th><th>Action</th></tr></thead>";


                    status = status + " </table>";
                    status = status + "</div>";
                    $('#DivJobDetails').html(status);
                    //$('#dataTable').DataTable({
                    //    scrollX: true,
                    //    fixedColumns: {
                    //        rightColumns: 2
                    //    }
                    //});

                    $('#dataTable').DataTable({
                        scrollX: true,
                        ordering: false,
                        sortable: false,
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
                           { 'data': 'Location' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Status' },
                           { 'data': 'Action' },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Assign Job',
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
                            LoadingPanel1.Hide();
                        }
                    });
                    LoadingPanel1.Hide();
                }
            });
        }


        function ViewDetails(values) {
            setTimeout(function () {


                $.ajax({
                    type: "POST",
                    url: "AssignedJob.aspx/ReceptDetails",
                    data: JSON.stringify({ ReceiptID: values }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        $("#DivEntityCode").text(msg.d.EntityCode);
                        $("#DivNetworkName").text(msg.d.NetworkName);
                        $("#DivContactPerson").text(msg.d.ContactPerson);
                        $("#DivReceivedBy").text(msg.d.ReceivedBy);

                        var status = "<table id='dataTabl3' class='table table-striped table-bordered' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th></th><th>Serial Number</th><th>Model Number</th><th>Problem </th>";
                        status = status + " <th>AC Cord/Adaptor</th><th>Remote</th><th>Others</th></thead>";

                        //status = status + " <tbody>";
                        //for (var i = 0; i < msg.d.DetailsList.length; i++) {
                        //    status = status + "<tr>";
                        //    status = status + " <td>" + msg.d.DetailsList[i].SLNO + "</td><td>" + msg.d.DetailsList[i].DeviceNumber + "</td>";
                        //    status = status + " <td>" + msg.d.DetailsList[i].ModelNumber + "</td><td>" + msg.d.DetailsList[i].Problem + "</td>";
                        //    status = status + " <td>" + msg.d.DetailsList[i].CordAdaptor + "</td><td>" + msg.d.DetailsList[i].Remote + "</td>";
                        //    status = status + " <td>" + msg.d.DetailsList[i].Others + "</td>";

                        //}
                        //  status = status + " </tbody></table>";
                        status = status + " </table>";
                        $('#DivViewDetails').html(status);
                        //$('#viewDetails')
                        $('#dataTabl3').DataTable({
                            "scrollY": "200px",
                            "scrollCollapse": true,
                            "paging": false,
                            data: msg.d.DetailsList,
                            columns: [
                               { 'data': 'SLNO' },
                               { 'data': 'DeviceNumber' },
                               { 'data': 'ModelNumber' },
                               { 'data': 'Problem' },
                               { 'data': 'CordAdaptor' },
                               { 'data': 'Remote' },
                               { 'data': 'Others' },
                            ],
                        });
                    }
                });
            }, 1000);
        }

        function MyJob() {
            LoadingPanel1.Show();
            $("#hdnSearchType").val("MyJob");
            $('#DivSearchTechinician').removeClass('hide');

            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d2').addClass('activeCrcl');
            $('#DivSearchTechinician').removeClass('hide');

            setTimeout(function () {


                $.ajax({
                    type: "POST",
                    url: "AssignedJob.aspx/BindMyJobs",
                    data: JSON.stringify({ Type: "MyJobs" }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#dataTable2').DataTable().destroy();

                        $("#divCountTotal").text(msg.d.TOTAL);
                        $("#divCountAssigned").text(msg.d.Assigned);
                        $("#divCountUnassigned").text(msg.d.Unassigned);

                        var status = '<div>';
                        status = status + '<table id="dataTable2" class="table table-striped table-bordered display nowrap" style="width: 100%">';
                        status = status + ' <thead><tr>';
                        status = status + ' <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>';
                        status = status + ' <th>Technician </th><th>Location </th><th>Received by</th><th>Received on</th><th>Assigned by</th>';
                        status = status + ' <th>Assigned on</th><th>Status</th><th>Action</th></tr></thead>';

                        //status = status + ' <tbody>';
                        //for (var i = 0; i < msg.d.length; i++) {
                        //    status = status + '<tr>';
                        //    status = status + ' <td>' + msg.d[i].ReceiptChallan + '</td>';
                        //    status = status + ' <td>' + msg.d[i].Type + '</td><td>' + msg.d[i].EntityCode + '</td>';
                        //    status = status + ' <td>' + msg.d[i].NetworkName + '</td><td>' + msg.d[i].ContactPerson + '</td>';
                        //    status = status + ' <td>' + msg.d[i].Technician + '</td><td>' + msg.d[i].Location + '</td>';
                        //    status = status + ' <td>' + msg.d[i].Receivedby + '</td><td>' + msg.d[i].Receivedon + '</td>';
                        //    status = status + ' <td>' + msg.d[i].Assignedby + '</td><td>' + msg.d[i].Assignedon + '</td>';
                        //    if (msg.d[i].Status == "P") {
                        //        status = status + ' <td><span class="badge badge-warning">Pending</span></td>';
                        //    }
                        //    else if (msg.d[i].Status == "DU") {
                        //        status = status + ' <td><span class="badge badge-danger">Due</span></td>';
                        //    }
                        //    else if (msg.d[i].Status == "DN") {
                        //        status = status + '<td><span class="badge badge-success">Done</span></td>';
                        //    }
                        //    else {
                        //        status = status + '<td>&nbsp;</td>';
                        //    }
                        //    status = status + ' <td class="actionInput"> ';
                        //    status = status + ' <span data-toggle="modal" data-target="#assignpop"><i class="fa fa-file-o assig" data-toggle="tooltip" data-placement="left" title="Assign"></i></span>';
                        //    status = status + ' <span data-toggle="modal" data-target="#viewDetails" onclick="ViewDetails(' + msg.d[i].ReceiptChallan_ID + ')"><i class="fa fa-eye det" data-toggle="tooltip" data-placement="left" title="Details"></i></span></td></tr>';
                        //}
                        //  status = status + ' </tbody>';
                        status = status + ' </table>';
                        status = status + '</div>';
                        $('#DivMyJobDetails').html(status);
                        $('#dataTable2').DataTable({
                            scrollX: true,
                            ordering: false,
                            sortable: false,
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
                               { 'data': 'Status' },
                               { 'data': 'Action' },
                            ],
                            dom: 'Bfrtip',
                            buttons: [
                                {
                                    extend: 'excel',
                                    title: null,
                                    filename: 'Assign Job',
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
                                LoadingPanel1.Hide();
                            }
                        });
                        LoadingPanel1.Hide();
                    }
                });
            }, 1000);
        }

        function AssignJob(values) {
            $("#hdnReceipt_ID").val(values);
            $.ajax({
                type: "POST",
                url: "AssignedJob.aspx/SingleAssignTechnician",
                data: JSON.stringify({ ChallanID: values }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var modl = '<select id="ddlAssignTechnician" class="js-example-basic-single" style="width: 100%;" >';
                    for (var i = 0; i < msg.d.TecchnicianList.length; i++) {
                        modl = modl + '<option value=' + msg.d.TecchnicianList[i].cnt_id + '>' + msg.d.TecchnicianList[i].cnt_firstName + '</option>';
                    }
                    modl = modl + '</select>';

                    $("#DivTechnician").html(modl);
                },
                error: function (msg) {
                }
            });
        }

        function SingleJobSave() {
            LoadingPanel.Show();
            var Technician_ID = $("#ddlAssignTechnician").val();
            if (Technician_ID == 0) {
                jAlert("Please select Technician.");
                $("#ddlAssignTechnician").focus();
                LoadingPanel.Hide();
                return
            }

            var Remarks = $("#txtRemarks").val();
            var recept_id = $("#hdnReceipt_ID").val();
            $.ajax({
                type: "POST",
                url: "AssignedJob.aspx/SingleAssign",
                data: JSON.stringify({ Technician_ID: Technician_ID, ReceiptChallan_ID: recept_id, Remarks: Remarks }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d.split('~')[1] == "Sucess") {
                            jAlert(response.d.split('~')[0], "Alert", function () {
                                if ($("#hdnOnlinePrint").val() == "Yes") {
                                    window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=AssignJob~D&modulename=ASSIGNJOB&id=" + response.d.split('~')[2], '_blank')
                                }
                                $("#ddlAssignTechnician").val(0).trigger('change');
                                $("#txtRemarks").val("");
                                $("#hdnReceipt_ID").val("");
                                $("#assignpop").toggle();
                                AssignedJob();
                                LoadingPanel.Hide();
                            });
                        }
                        else if (response.d == "Logout") {
                            location.href = "../../OMS/SignOff.aspx";
                            LoadingPanel.Hide();
                        }
                        else {
                            jAlert(response.d.split('~')[0]);
                            LoadingPanel.Hide();
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                    LoadingPanel.Hide();
                }
            });
        }

        function SearchClick() {
            if ($("#hdnUserType").val() == "Technician") {
                SearchDataMyJob();
            }
            else if ($("#hdnSearchType").val() == "MyJob") {
                SearchDataMyJob();
            }
            else if ($("#hdnSearchType").val() == "TotalJob") {
                TotalSearchJob();
            }
            else if ($("#hdnSearchType").val() == "AssignedJob") {
                AssignedSearchJob();
            }
            else if ($("#hdnSearchType").val() == "UnassignedJob") {
                UnassignedSearchJob();
            }
        }

        function SearchDataMyJob() {
            LoadingPanel1.Show();
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
                        SearchType: $("#hdnSearchType").val(),
                        Technician_ID: Technician,//$("#ddlTechnician").val()
                    }

                    $.ajax({
                        type: "POST",
                        url: "AssignedJob.aspx/SearchDataMyJob",
                        data: "{model:" + JSON.stringify(Apply) + "}",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            $('#dataTable2').DataTable().destroy();

                            $("#divCountTotal").text(msg.d.TOTAL);
                            $("#divCountAssigned").text(msg.d.Assigned);
                            $("#divCountUnassigned").text(msg.d.Unassigned);

                            var status = '<div>';
                            status = status + '<table id="dataTable2" class="table table-striped table-bordered display nowrap" style="width: 100%">';
                            status = status + ' <thead><tr>';
                            status = status + ' <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>';
                            status = status + ' <th>Technician </th><th>Location </th><th>Received by</th><th>Received on</th><th>Assigned by</th>';
                            status = status + ' <th>Assigned on</th><th>Status</th><th>Action</th></tr></thead>';

                            status = status + ' </table>';
                            status = status + '</div>';
                            $('#DivMyJobDetails').html(status);
                            $('#dataTable2').DataTable({
                                scrollX: true,
                                ordering: false,
                                sortable: false,
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
                                   { 'data': 'Status' },
                                   { 'data': 'Action' },
                                ],
                                dom: 'Bfrtip',
                                buttons: [
                                    {
                                        extend: 'excel',
                                        title: null,
                                        filename: 'Assign Job',
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
                                    LoadingPanel1.Hide();
                                }
                            });
                            LoadingPanel1.Hide();
                        },
                        error: function (response) {
                            console.log(response);
                            LoadingPanel1.Hide();
                        }
                    });
                });
            });
        }

        function TotalSearchJob() {
            LoadingPanel1.Show();
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
                        SearchType: $("#hdnSearchType").val(),
                        Technician_ID: Technician,//$("#ddlTechnician").val()
                    }

                    setTimeout(function () {
                        $.ajax({
                            type: "POST",
                            url: "AssignedJob.aspx/TotalSearchJob",
                            data: JSON.stringify({ model: Apply }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {

                                $("#divCountTotal").text(msg.d.TOTAL);
                                $("#divCountAssigned").text(msg.d.Assigned);
                                $("#divCountUnassigned").text(msg.d.Unassigned);

                                var status = "<div>";
                                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                                status = status + " <thead><tr>";
                                status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>";
                                status = status + " <th>Technician </th><th>Location </th><th>Received by</th><th>Received on</th><th>Assigned by</th>";
                                status = status + " <th>Assigned on</th><th>Status</th><th>Action</th></tr></thead>";

                                status = status + " </table>";
                                status = status + "</div>";
                                $('#DivJobDetails').html(status);

                                $('#dataTable').DataTable({
                                    scrollX: true,
                                    ordering: false,
                                    sortable: false,
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
                                       { 'data': 'Status' },
                                       { 'data': 'Action' },
                                    ],
                                    dom: 'Bfrtip',
                                    buttons: [
                                        {
                                            extend: 'excel',
                                            title: null,
                                            filename: 'Assign Job',
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
                                        LoadingPanel1.Hide();
                                    }
                                });
                                LoadingPanel1.Hide();
                            }
                        });
                    }, 1000);
                });
            });
        }

        function AssignedSearchJob() {
            LoadingPanel1.Show();
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
                        SearchType: $("#hdnSearchType").val(),
                        Technician_ID: Technician,//$("#ddlTechnician").val()
                    }


                    $.ajax({
                        type: "POST",
                        url: "AssignedJob.aspx/TotalSearchJob",
                        data: JSON.stringify({ model: Apply }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            $("#divCountTotal").text(msg.d.TOTAL);
                            $("#divCountAssigned").text(msg.d.Assigned);
                            $("#divCountUnassigned").text(msg.d.Unassigned);

                            var status = "<div>";
                            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                            status = status + " <thead><tr>";
                            status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>";
                            status = status + " <th>Technician </th><th>Location </th><th>Received by</th><th>Received on</th><th>Assigned by</th>";
                            status = status + " <th>Assigned on</th><th>Status</th><th>Action</th></tr></thead>";

                            status = status + " </table>";
                            status = status + "</div>";
                            $('#DivJobDetails').html(status);

                            $('#dataTable').DataTable({
                                scrollX: true,
                                ordering: false,
                                sortable: false,
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
                                   { 'data': 'Status' },
                                   { 'data': 'Action' },
                                ],
                                dom: 'Bfrtip',
                                buttons: [
                                    {
                                        extend: 'excel',
                                        title: null,
                                        filename: 'Assign Job',
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
                                    LoadingPanel1.Hide();
                                }
                            });
                            LoadingPanel1.Hide();
                        }
                    });
                });
            });
        }

        function UnassignedSearchJob() {
            LoadingPanel1.Show();
            var brnch = "";//gridbranchLookup.gridView.GetSelectedKeysOnPage();
            var branchss = "";
            //for (var i = 0; i < brnch.length; i++) {
            //    branchss += ',' + brnch[i];
            //}

            var Tech = "";// gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
            var Technician = "";
            //for (var i = 0; i < Tech.length; i++) {
            //    if (Technician == "") {
            //        Technician = Tech[i];
            //    }
            //    else {
            //        Technician += ',' + Tech[i];
            //    }
            //}
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
                        SearchType: $("#hdnSearchType").val(),
                        Technician_ID: Technician,//$("#ddlTechnician").val()
                    }


                    $.ajax({
                        type: "POST",
                        url: "AssignedJob.aspx/BindUnassignedSearchJob",
                        data: JSON.stringify({ model: Apply }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {

                            $("#divCountTotal").text(msg.d.TOTAL);
                            $("#divCountAssigned").text(msg.d.Assigned);
                            $("#divCountUnassigned").text(msg.d.Unassigned);

                            var status = "<div>";
                            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                            status = status + " <thead><tr>";
                            status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th>";
                            status = status + " <th>Location </th><th>Received by</th><th>Received on</th>";
                            status = status + " <th>Status</th><th>Action</th></tr></thead>";

                            status = status + " </table>";
                            status = status + "</div>";
                            $('#DivJobDetails').html(status);

                            $('#dataTable').DataTable({
                                scrollX: true,
                                ordering: false,
                                sortable: false,
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
                                   { 'data': 'Location' },
                                   { 'data': 'Receivedby' },
                                   { 'data': 'Receivedon' },
                                   { 'data': 'Status' },
                                   { 'data': 'Action' },
                                ],
                                dom: 'Bfrtip',
                                buttons: [
                                    {
                                        extend: 'excel',
                                        title: null,
                                        filename: 'Assign Job',
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
                                    LoadingPanel1.Hide();
                                }
                            });
                            LoadingPanel1.Hide();
                        }
                    });
                });
            });
        }

        function UnAssignJob(values) {
            var recept_id = $("#hdnReceipt_ID").val();
            $.ajax({
                type: "POST",
                url: "AssignedJob.aspx/UnAssign",
                data: JSON.stringify({ ReceiptChallan_ID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Unassign successfully.", "Alert", function () {
                                TotalJob();
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

        //function ServiceEntry(values) {
        //    location.href = "../Transaction/servicedata/serviceDataEntry.aspx?id=" + values;
        //}
    </script>

    <script>
        function MethodAssignJob() {
            popup.Hide();
            MyJob();
        }

        function closeUserApproval() {
            popup.Hide();
        }

        function ServiceEntry(obj) {
            uri = "servicedata/serviceDataEntry.aspx?id=" + obj + "&status='yes'";
            popup.SetContentUrl(uri);
            popup.Show();
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
    <span class="hddd font-pp">Assign Job</span>
    <div class="" id="LodingId">
        <div class="clearfix font-pp">
            <div class="clearfix text-center ">
                <ul class="circulerUl">
                    <li id="d1">
                        <div class="crcl" onclick="TotalJob();">
                            <div id="divCountTotal">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Total</div>
                    </li>
                    <li id="d2">
                        <div class="crcl" onclick="AssignedJob();">
                            <div id="divCountAssigned">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Assigned</div>
                    </li>
                    <li id="d3">
                        <div class="crcl" id="" onclick="UnassignedJob();">
                            <div id="divCountUnassigned">0</div>
                            <i class="fa fa-check-circle icn"></i>
                        </div>
                        <div>Unassigned</div>
                    </li>
                </ul>
                <ul class="centerd hide">
                    <li class="active">
                        <span class="setIcon"><i class="fa fa-list-ol" aria-hidden="true"></i></span>
                        <div class="lb">Total</div>
                        <div class="nm">52</div>
                    </li>
                    <li>
                        <span class="setIcon green"><i class="fa fa-user" aria-hidden="true"></i></span>
                        <div class="lb">Assigned</div>
                        <div class="nm" style="color: #5bbd7e;">32</div>
                    </li>
                    <li>
                        <span class="setIcon red"><i class="fa fa-user-times" aria-hidden="true"></i></span>
                        <div class="lb">Unassigned</div>
                        <div class="nm" style="color: #e81b1b;">20</div>
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
                            <%-- <asp:DropDownList ID="ddlBranch" runat="server" CssClass="js-example-basic-single" DataTextField="branch_description" DataValueField="branch_id" Width="100%"
                                meta:resourcekey="ddlBranchResource1">
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

                    <div class="col-md-2" id="DivSearchTechinician">
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
        <div class="clearfix font-pp relative" id="tabWraper">
            <div class="pull-right">
                <span onclick="MassAssign();" id="spnMassAssign"><span class="btn btn-success" data-toggle="modal" data-target="#exampleModal"><i class="fa fa-users" aria-hidden="true"></i>Mass Assign</span></span>
                <span class="togglerSlide btn btn-warning" data-toggle="tooltip" data-placement="top" title="Filter"><i class="fa fa-filter"></i></span>
                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" onchange="ExportChange();">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                </asp:DropDownList>
                <%} %>
            </div>
            <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item active" id="LiTotalJob">
                    <a class="nav-link " id="home-tab" data-toggle="tab" href="#home" role="tab" onclick="TotalJob();" aria-controls="home" aria-selected="true">Total Jobs</a>
                </li>
                <li class="nav-item" id="TabMyJob">
                    <a class="nav-link" id="profile-tab" data-toggle="tab" href="#profile" role="tab" onclick="MyJob();" aria-controls="profile" aria-selected="false">My Jobs</a>
                </li>

            </ul>
            <div class="tab-content mBot10" id="myTabContent">
                <div class="tab-pane fade in active" id="home" role="tabpanel" aria-labelledby="home-tab">
                    <div id="DivJobDetails">
                        <%-- <table id="dataTabletotal" class="table table-striped table-bordered display nowrap" style="width: 100%">
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
                                    <th>Status</th>
                                    <th>Action</th>
                                </tr>
                            </thead>--%>
                        <%--<tbody>
                                <tr>
                                    <td>154610</td>

                                    <td>Technician name</td>
                                    <td>asfasf </td>
                                    <td>asfasf </td>
                                    <td>asfasf </td>
                                    <td>10.2.21</td>
                                    <td>asfasf </td>
                                    <td>asfasf </td>
                                    <td>10.2.21</td>
                                    <td>Assigned by</td>
                                    <td>Assigned on</td>

                                    <td><span class="badge badge-warning">Pending</span></td>
                                    <td class="actionInput">
                                        <span data-toggle="modal" data-target="#assignpop"><i class="fa fa-user-plus assig" data-toggle="tooltip" data-placement="left" title="Assign"></i></span>
                                        <span data-toggle="modal" data-target="#viewDetails" onclick="ViewDetails()"><i class="fa fa-eye det" data-toggle="tooltip" data-placement="left" title="Details"></i></span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>--%>
                    </div>
                </div>
                <div class="tab-pane fade" id="profile" role="tabpanel" aria-labelledby="profile-tab">
                    <div id="DivMyJobDetails">
                        <%--<table id="dataTable2" class="table table-striped table-bordered" style="width: 100%">
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
                                    <th>Status</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                   <td>154610</td>

                                    <td>Technician name</td>
                                    <td>asfasf </td>
                                    <td>asfasf </td>
                                    <td>asfasf </td>
                                    <td>10.2.21</td>
                                    <td>asfasf </td>
                                    <td>asfasf </td>
                                    <td>10.2.21</td>
                                    <td>Assigned by</td>
                                    <td>Assigned on</td>

                                    <td><span class="badge badge-warning">Pending</span></td>
                                    <td class="actionInput">
                                        <span data-toggle="modal" data-target="#assignpop"><i class="fa fa-user-plus assig" data-toggle="tooltip" data-placement="left" title="Assign"></i></span>
                                        <span data-toggle="modal" data-target="#viewDetails" onclick="ViewDetails()"><i class="fa fa-eye det" data-toggle="tooltip" data-placement="left" title="Details"></i></span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="margin-top: 15px;">
        <div class="col-md-12">
        </div>
    </div>

    <!-- Modal assignpop-->



    <div class="modal fade pmsModal w30" id="assignpop" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <% if (rights.CanAssignTo)
                       { %>
                    <h5 class="modal-title">Assign Technician</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <%} %>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12">
                            <label class="deep">Choose Technician</label>
                            <div class="fullWidth" id="DivTechnician">

                                <%-- <asp:DropDownList ID="ddlAssignTechnician" runat="server" CssClass="js-example-basic-single" DataTextField="cnt_firstName" DataValueField="cnt_id" Width="100%">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtRemarks" maxlength="500" rows="5"></textarea>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnReceipt_ID" runat="server" />
                </div>
                <div class="modal-footer" id="divsave">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <% if (rights.CanAssignTo)
                       { %>
                    <button type="button" class="btn btn-success" onclick="SingleJobSave();">Confirm</button>
                    <%} %>
                </div>
            </div>
        </div>
    </div>

    <!--Mass add popup -->
    <div class="modal fade pmsModal" id="exampleModal" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Mass Job Assign </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body font-pp" style="padding-top: 10px">
                    <div class="row ">
                        <div class="col-md-4">
                            <label class="deep">Choose Technician</label>
                            <div class="fullWidth">

                                <asp:DropDownList ID="ddlTechnicianMass" runat="server" CssClass="js-example-basic-single" DataTextField="cnt_firstName" DataValueField="cnt_id" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row mTop5">

                        <div class="col-md-12">
                            <div id="divMassAssign">
                                <%--  <table id="dataTable3" class="table table-striped table-bordered" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>Select</th>
                                            <th>Receipt Challan</th>
                                            <th>Type</th>
                                            <th>Entity Code </th>
                                            <th>Network Name</th>
                                            <th>Contact Person</th>
                                            <th>Received on </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <div class="form-check">
                                                    <input class="form-check-input" type="checkbox" value="" id="" />
                                                    <label class="form-check-label" for="defaultCheck1">
                                                    </label>
                                                </div>
                                            </td>
                                            <td>154610</td>
                                            <td>asfasf asf afs</td>
                                            <td>Not Assigned</td>
                                            <td>03.03.2020 </td>
                                            <td>03.03.2020 </td>
                                            <td>03.03.2020 </td>
                                        </tr>
                                    </tbody>
                                </table>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <% if (rights.CanAssignTo)
                       { %>
                    <button type="button" class="btn btn-primary" onclick="MassJobSave();">Confirm</button>
                    <%} %>
                </div>
            </div>
        </div>
    </div>
    <!--Mass add popup -->
    <div class="modal fade pmsModal w70" id="viewDetails" role="dialog" aria-labelledby="viewDetails" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="">View Details</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body font-pp" style="padding-top: 10px;">
                    <div class="row ">
                        <div class="col-md-3">
                            <label class="deep"><b>Entity Code</b> </label>
                            <div class="fullWidth" id="DivEntityCode">
                                <%--1524523--%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="deep"><b>Network Name</b></label>
                            <div class="fullWidth" id="DivNetworkName">
                                <%--sdhdhsdhsdh--%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="deep"><b>Contact Person</b> </label>
                            <div class="fullWidth" id="DivContactPerson">
                                <%--hdfhfsdhsdh--%>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="deep"><b>Received By</b></label>
                            <div class="fullWidth" id="DivReceivedBy">
                                <%--Susanta Kundu--%>
                            </div>
                        </div>
                    </div>

                    <div class="row mTop5">

                        <div class="col-md-12 mTop5">
                            <div id="DivViewDetails">
                                <%--<table id="dataTabl" class="table table-striped table-bordered" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>STB Number</th>
                                            <th>Model Number</th>
                                            <th>Problem </th>
                                            <th>AC Cord/Adaptor</th>
                                            <th>Remote</th>
                                            <th>Others</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>454564
                                            </td>
                                            <td>154610</td>
                                            <td>asfasf asf afs</td>
                                            <td>Not Assigned</td>
                                            <td>03.03.2020 </td>
                                            <td>03.03.2020 </td>
                                            <td>03.03.2020 </td>
                                        </tr>
                                    </tbody>
                                </table>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-primary">Confirm</button>--%>
                </div>
            </div>
        </div>
    </div>

    <style>
        #ASPXPopupControl_PW-1 .closeApprove {
            float: right;
            margin-right: 7px;
        }

        #ASPXPopupControl_PW-1 .closeApprove {
            float: right;
            margin-right: 7px;
        }
    </style>
    <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ClientInstanceName="popup"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="630px"
        Width="1200px" HeaderText="Service Entry" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <headertemplate>
                <span>Add Service Entry</span>
                <div class="closeApprove" onclick="closeUserApproval();"><i class="fa fa-close"></i></div>
            </headertemplate>
        <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </contentcollection>
    </dxe:ASPxPopupControl>
    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <contentcollection>
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
            </contentcollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>

     <dxe:ASPxLoadingPanel ID="LoadingPanel1" runat="server" ClientInstanceName="LoadingPanel1" ContainerElementID="LodingId" Modal="True">
         </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdnUserType" runat="server" />
    <asp:HiddenField ID="hdnSearchType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />
</asp:Content>
