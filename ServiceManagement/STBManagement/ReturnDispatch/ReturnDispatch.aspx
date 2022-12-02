<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ReturnDispatch.aspx.cs" Inherits="ServiceManagement.STBManagement.ReturnDispatch.ReturnDispatch" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

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
        function PendingDispatch() {

            WorkingRoster();
            if (rosterstatus) {
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d3').addClass('activeCrcl');
                $("#hdnSearchType").val('Dispatch');
                cgridStatus.Refresh();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function DispatchAcknowledgment() {

            WorkingRoster();
            if (rosterstatus) {
                $("#hfIsFilter").val('Y');
                $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
                $('#d4').addClass('activeCrcl');
                $("#hdnSearchType").val('DispatchAcknowledgment');
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

        function ClickDispatchAcknowledgment(val) {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnmodule_ID").val(val);
                $("#DispatchAcknowledgmentPopUp").modal('toggle');
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function ClickUpdateDispatch(val) {
            WorkingRoster();
            if (rosterstatus) {
                $("#hdnmodule_ID").val(val);
                $("#UpdateDispatchpop").modal('toggle');
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function DispatchAcknowledgmentUpdate() {
            var DispatchAcknowledgment = $("#ddlDispatchAcknowledgment").val();
            var Remarks = $("#txtDispatchAcknowledgment").val();
            var module_id = $("#hdnmodule_ID").val();

            if (DispatchAcknowledgment == "0") {
                jAlert("Please select Dispatch Acknowledgment", "Alert", function () {
                    setTimeout(function () {
                        $("#ddlDispatchAcknowledgment").focus();
                        return
                    }, 200);
                });
                return
            }

            $.ajax({
                type: "POST",
                url: "ReturnDispatch.aspx/DispatchAcknowledgmentUpdate",
                data: JSON.stringify({ Document_ID: module_id, Remarks: Remarks, DispatchAcknowledgment: DispatchAcknowledgment }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Update Dispatch Acknowledgment Successfully.", "Alert", function () {

                                $("#txtDispatchAcknowledgment").val("");
                                $("#hdnmodule_ID").val("");
                                $("#ddlDispatchAcknowledgment").val("0");
                                $("#DispatchAcknowledgmentPopUp").toggle();
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

        function UpdateDispatch() {
            var Remarks = $("#txtUpdateDispatchRemarks").val();
            var module_id = $("#hdnmodule_ID").val();
            $.ajax({
                type: "POST",
                url: "ReturnDispatch.aspx/UpdateDispatch",
                data: JSON.stringify({ Document_ID: module_id, Remarks: Remarks }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Updated Dispatch Successfully.", "Alert", function () {

                                $("#txtUpdateDispatchRemarks").val("");
                                $("#hdnmodule_ID").val("");
                                $("#UpdateDispatchpop").toggle();
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
                url: "ReturnDispatch.aspx/CountButton",
                data: JSON.stringify({ UserType: '' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $("#divPendingDispatch").text(msg.d.split('~')[0]);
                    $("#divDispatchAcknowledgment").text(msg.d.split('~')[1]);
                }
            });
        }

        function ClickDeleteInventory(val) {
            WorkingRoster();
            if (rosterstatus) {
                jConfirm('Confirm Delete?', 'Alert', function (r) {
                    if (r) {
                        $.ajax({
                            type: "POST",
                            url: "STBInventory.aspx/DeleteInventory",
                            data: JSON.stringify({ Document_ID: val }),
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (response.d) {
                                    if (response.d == "true") {
                                        jAlert("Inventory Delete Successfully.", "Alert", function () {
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
                });
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        var rosterstatus = false;
        function WorkingRoster() {
            $.ajax({
                type: "POST",
                url: 'ReturnDispatch.aspx/CheckWorkingRoster',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ module_ID: '12' }),
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


        var filenames = [];
        function SaveAttachment() {
            var process = 0;

            var fileUploadCheck = $("#ReceiptChallanDoc").get(0);
            var docFileName = "STBReturnRequisition";
            var GatepassNo = $("#txtGatepassNo").val();

            for (var i = 0; i < $('#ReceiptChallanDoc').get(0).files.length; ++i) {
                filenames.push($('#ReceiptChallanDoc').get(0).files[i].name);
            }


            if (process == 0) {
                if (GatepassNo != "") {
                    if (window.FormData !== undefined) {
                        var fileUpload = $("#ReceiptChallanDoc").get(0);
                        var files = fileUpload.files;
                        var fileData = new FormData();

                        for (var i = 0; i < files.length; i++) {
                            fileData.append(files[i].name, files[i]);
                        }

                        fileData.append('docFileName', docFileName);

                        $.ajax({
                            type: "POST",
                            url: '/SRVFileuploadDelivery/AttachmentDocumentAddUpdate',
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (response) {

                                if (response) {
                                    // jAlert("Documents Added Successfully!");
                                }
                                else {
                                    jAlert("Upload failed!");
                                    return
                                }
                            }
                        });
                    } else {
                        jAlert("Upload failed Please try again later!");
                        return
                    }
                }
                else {
                    jAlert("Upload failed Please try again later!");
                    return
                }
            }

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
    </style>
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
    <span class="hddd font-pp">Return Dispatch</span>
    <div class="clearfix font-pp">
        <div class="clearfix text-center ">
            <% if (rights.CanPendingDispatch)
               { %>
            <ul class="circulerUl">
                <li id="d3">
                    <div class="crcl red" onclick="PendingDispatch();">
                        <div id="divPendingDispatch" runat="server">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div class="colRed">Pending Dispatch</div>
                </li>
            </ul>
            <% } %>
            <% if (rights.CanDispatchAcknowledgment)
               { %>
            <ul class="circulerUl">
                <li id="d4">
                    <div class="crcl" onclick="DispatchAcknowledgment();">
                        <div id="divDispatchAcknowledgment" runat="server">0</div>
                        <i class="fa fa-check-circle icn"></i>
                    </div>
                    <div class="">Dispatch Acknowledgment</div>
                </li>
            </ul>
            <% } %>
        </div>
    </div>
    <div class="mtop8" id="LodingId">
        <div class="clearfix">
            <div class="relative">
                <span class="togglerSlide btn btn-warning hide" style="position: absolute; right: 99px; z-index: 10;" data-toggle="tooltip" data-placement="top" title="Filter"><i class="fa fa-filter"></i></span>

                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true"
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

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="DocumentNumber"  Width="200px"
                                Caption="Document No." FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="DocDate" Width="100px"
                                Caption="Doc Date" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="CancelStatus" Width="200px"
                                Caption="Status" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="IsDispatch" Width="200px"
                                Caption="Dispatch" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="IsDispatchAcknowledgment" Width="200px"
                                Caption="Dispt. Ack." Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                                
                             <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ModelDesc" Width="100px"
                                Caption="Model" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="QTY" Width="100px"
                                Caption="Qty" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="UnitPrice" Width="100px"
                                Caption="Unit Price" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Details_Amount" Width="100px"
                                Caption="Amount" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                                
                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DocType" Width="200px"
                                Caption="Doc Type">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="EntityCode" Width="200px"
                                Caption="Entity Code" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="NetworkName" Width="200px"
                                Caption="Network Name" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="ContactPerson" Width="200px"
                                Caption="Contact Person" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="ContactNo" Width="150px"
                                Caption="Contact No" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="branch_description" Width="200px"
                                Caption="Location" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="Enteredby" Width="200px"
                                Caption="Entered by" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="EnteredOn" Width="150px"
                                Caption="Entered on" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

  
                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="1px">
                        <DataItemTemplate>
                           <div class="floatedIcons">
                            <div class='floatedBtnArea'>
                           <%if (rights.CanEdit)%>
                            <% { %>
                                <a href="javascript:void(0);" onclick="ClickUpdateDispatch('<%# Container.KeyValue %>')" id="a_UpdateDispatch" class="pad" title="Dispatch" style='<%#Eval("DispatchAction")%>'>
                                <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Dispatch</span></a>
                           <% } %>
                            <%if (rights.CanEdit)
                              { %>
                                <a href="javascript:void(0);" onclick="ClickDispatchAcknowledgment('<%# Container.KeyValue %>')" id="a_DispatchAcknowledgment" class="pad" title="Dispatch Acknowledgment" style='<%#Eval("DispatchAcknowledgmentAction")%>'>
                                    <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Dispatch Acknowledgment</span></a>
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

                       </columns>
                            <clientsideevents rowclick="gridRowclick" />
                            <settingscontextmenu enabled="true"></settingscontextmenu>
                            <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>
                            <settingssearchpanel visible="True" />
                            <settings showgrouppanel="True" showstatusbar="Hidden" showfilterrow="true" showfilterrowmenu="True" />

                            <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                             <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
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


    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
    </dxe:ASPxLoadingPanel>

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

    <div class="modal fade pmsModal w40" id="DispatchAcknowledgmentPopUp" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Dispatch Acknowledgment</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-5 mTop5">
                            <label class="deep">Dispatch Acknowledgment <span class="red">*</span> </label>
                            <div class="fullWidth">
                                <select id="ddlDispatchAcknowledgment" class="form-control">
                                    <option value="0">Select</option>
                                    <option value="1">Close</option>
                                    <option value="2">Reject</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtDispatchAcknowledgment" maxlength="500" rows="5"></textarea>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success" onclick="DispatchAcknowledgmentUpdate();">Confirm</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w40" id="UpdateDispatchpop" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Update Dispatch</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtUpdateDispatchRemarks" maxlength="500" rows="5"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success" onclick="UpdateDispatch();">Confirm</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
