<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WarrantyList.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.Warranty.WarrantyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

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
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dataTable').DataTable({
                scrollX: true,
                ordering: false,
                sortable: false,
                fixedColumns: {
                    rightColumns: 1
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
        .tblPad {
            margin-top: 5px;
        }

            .tblPad > tbody > tr > td {
                vertical-align: middle;
                padding-right: 15px;
            }

                .tblPad > tbody > tr > td > .date input {
                    height: 29px;
                }

        #ddlBranch {
            border-radius: 3px;
            height: 30px !important;
        }

        #divListData .dt-buttons {
            display: none;
        }
    </style>

    <script>
        function AddWarranty() {
            var url = "WarrantyEntry.aspx?Key=Add";
            window.location.href = url;
        }

        function btnShow_Click() {
            LoadingPanel.Show();
            var brnch = "";//gridbranchLookup.gridView.GetSelectedKeysOnPage();
            var branchss = "";
            //for (var i = 0; i < brnch.length; i++) {
            //    branchss += ',' + brnch[i];
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
                if (branchss == "") {
                    jAlert("Please select branch.");
                    LoadingPanel.Hide();
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "WarrantyList.aspx/WarrantyListDetails",
                    data: JSON.stringify({ Branch: branchss, FromDate: $("#Fromdt").val(), ToDate: $("#Todt").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        var status = "";
                        status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Receipt Challan</th><th>Serial No</th><th>New Serial No </th><th>Warranty Date</th>";
                        status = status + " <th>Problem Found</th><th>Location </th><th>Entity Code </th><th>Network Name</th>";
                        status = status + " <th>Entered By</th><th>Entered On</th><th>Updated By</th><th>Updated On</th>";
                        status = status + " <th>Action</th></tr></thead>";

                        status = status + " </table>";

                        $('#divListData').html(status);

                        $('#dataTable').DataTable({
                            scrollX: true,
                            fixedColumns: {
                                rightColumns: 1
                            },
                            data: msg.d,
                            columns: [
                               { 'data': 'ReceiptChallanNo' },
                               { 'data': 'SerialNo' },
                               { 'data': 'NewSerialNo' },
                               { 'data': 'UpdateWarrantyDate' },
                               { 'data': 'ProblemDesc' },
                               { 'data': 'branch_description' },
                               { 'data': 'EntityCode' },
                               { 'data': 'NetworkName' },
                               { 'data': 'EnteredBy' },
                               { 'data': 'EnteredOn' },
                               { 'data': 'UpdatedBy' },
                               { 'data': 'UpdatedOn' },
                               { 'data': 'Actions' },
                            ],
                            dom: 'Bfrtip',
                            buttons: [
                                {
                                    extend: 'excel',
                                    title: null,
                                    filename: 'Warranty Update',
                                    text: 'Save as Excel',
                                    customize: function (xlsx) {
                                        var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                        $('row:first c', sheet).attr('s', '42');
                                    },

                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
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
        }

        function Edit(values) {
            location.href = "WarrantyEntry.aspx?key=Edit&id=" + values;
        }

        function View(values) {
            location.href = "WarrantyEntry.aspx?key=View&id=" + values;
        }

        function Delete(values) {

            jConfirm('Confirm Delete?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "WarrantyList.aspx/DeleteWarranty",
                        data: JSON.stringify({ WarrantyUpdateID: values }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            console.log(response);
                            if (response.d) {
                                if (response.d == "true") {
                                    jAlert("Delete Successfully.", "Alert", function () {
                                        btnShow_Click();
                                    });
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

        $(function () {
            cBranchPanel.PerformCallback('BindComponentGrid' + '~' + "All");
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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Warranty </h3>
        </div>

        <div class="pull-right">
            <table class="tblPad">
                <tr>
                    <td>From</td>
                    <td style="width: 140px">
                        <div class="input-group date" data-provide="datepicker">
                            <input type="text" class="form-control" id="Fromdt" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </td>
                    <td>To</td>
                    <td style="width: 140px">
                        <div class="input-group date" data-provide="datepicker">
                            <input type="text" class="form-control" id="Todt" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </td>
                    <td>Unit</td>
                    <td style="width: 140px">
                        <%--<select class="form-control">
                            <option>Select</option>
                        </select>--%>
                        <%--<asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" DataTextField="branch_description" DataValueField="branch_id" Width="100%">
                        </asp:DropDownList>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="BranchPanel" ClientInstanceName="cBranchPanel" OnCallback="Componentbranch_Callback">
                    <PanelCollection>
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
                    </PanelCollection>
                </dxe:ASPxCallbackPanel>
                    </td>
                    <td>
                        <button type="button" id="btnShow" onclick="btnShow_Click();" class="btn btn-primary">Show</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="form_main pTop5" id="LodingId">
        <div class="clearfix" style="margin-bottom: 8px;">
            <% if (rights.CanAdd)
               { %>
            <button class="btn btn-success btn-radius" type="button" onclick="AddWarranty();">
                <span class="btn-icon"><i class="fa fa-plus"></i></span>Warranty
            </button>
            <%} %>
            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" onchange="ExportChange();">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <%--<asp:ListItem Value="1">PDF</asp:ListItem>--%>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <%--<asp:ListItem Value="4">CSV</asp:ListItem>--%>
            </asp:DropDownList>
            <%} %>
        </div>
        <div class="clearfix">
            <div id="divListData">
                <table id="dataTable" class="table table-striped table-bordered display nowrap">
                    <thead>
                        <tr>
                            <th>Receipt Challan</th>
                            <th>Serial No</th>
                            <th>New Serial No </th>
                            <th>Warranty Date</th>
                            <th>Problem Found</th>
                            <th>Location </th>
                            <th>Entity Code </th>
                            <th>Network Name</th>
                            <th>Entered By</th>
                            <th>Entered On</th>
                            <th>Updated By</th>
                            <th>Updated On</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <%-- <tbody>
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
                        <td class="actionInput text-center">
                            <span><i class="fa fa-pencil-square-o assig" data-toggle="tooltip" data-placement="bottom" title="Edit" ></i> </span>
                            <span><i class="fa fa-print det" data-toggle="tooltip" data-placement="bottom" title="Print" ></i> </span>
                        </td>
                    </tr>    
                </tbody>--%>
                </table>
            </div>
        </div>
    </div>

     <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId" Modal="True">
         </dxe:ASPxLoadingPanel>
</asp:Content>
