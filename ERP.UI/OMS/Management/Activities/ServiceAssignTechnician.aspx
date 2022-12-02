<%@ Page Title="Service Assign Technician" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceAssignTechnician.aspx.cs" Inherits="ERP.OMS.Management.Activities.ServiceAssignTechnician" %>

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
        .padTab {
            margin-bottom: 4px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                }

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
                display: block;
            }

            .MobiledivReceiptChallanDtls {
                display: block;
                border: 1px solid #3db191;
                border-radius: 5px;
                padding: 10px 0;
            }
        }
    </style>
    <script type="text/javascript">
        function TotalTotal() {
            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d1').addClass('activeCrcl');
            $("#hdnType").val("Assign");
            grid.PerformCallback('BindGrid');
        }
        function TotalSt() {
            $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
            $('#d2').addClass('activeCrcl');
            $("#hdnType").val("UnAssign");
            grid.PerformCallback('BindGrid');
        }

        function updateGridByDate() {
            if ($('#d2').hasClass('activeCrcl')) {
                $("#hdnType").val("Assign");
            }
            else {
                $("#hdnType").val("UnAssign");

            }
            grid.PerformCallback('BindGrid');
        }
        function EndCallback(s, e) {
            if (s.cpMsg != null && s.cpMsg != "") {
                jAlert(s.cpMsg, 'Alert');
                s.cpMsg = null;
            }

        }

        function BranchChange(s, e) {
            cddlTechnician.PerformCallback();
            checkListBox.PerformCallback();
            grid.PerformCallback('BlankGrid');
        }

        function AssignTechnician() {
            if (cddlTechnician.GetValue() == "") {
                jAlert('Please select a Technician to assign', 'Alert');
            }
            else {
                grid.PerformCallback('AssignAll');
            }
        }

        function UnAssignTechnician() {
            grid.PerformCallback('UnAssignAll');
        }

        var textSeparator = ";";
        function updateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(getSelectedItemsText(selectedItems));

            if (selectedItems.length > 0) {
                for (var i = 0; i < selectedItems.length; i++) {
                    if (i == 0)
                        $("#hdnSubTech").val(selectedItems[i].value)
                    else
                        $("#hdnSubTech").val($("#hdnSubTech").val() + "," + selectedItems[i].value)
                }
            }

        }
        function synchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            checkListBox.SelectValues(values);
            updateText(); // for remove non-existing texts
        }
        function getSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function getValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Service Assign Technician</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px;">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                        <ClientSideEvents SelectedIndexChanged="BranchChange" />
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>
            </tr>
        </table>
    </div>
    <div class="form_main">
        <div class="">
            <div class="clearfix font-pp">
                <div class="clearfix text-center ">
                    <ul class="circulerUl">
                        <li id="d1">
                            <div class="crcl" onclick="TotalTotal();">
                                <div id="divTotal" runat="server"></div>
                                <i class="fa fa-check-circle icn"></i>
                            </div>
                            <div>Assigned</div>
                        </li>
                        <li id="d2">
                            <div class="crcl" onclick="TotalSt();">
                                <div id="div1" runat="server"></div>
                                <i class="fa fa-check-circle icn"></i>
                            </div>
                            <div>un-Assigned</div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="clearfix relative row">
            <div class="col-sm-12 " style="padding-top: 15px">


                <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="DETAILS_ID" TabIndex="6" OnDataBinding="grid_DataBinding" OnCustomCallback="grid_CustomCallback"
                    Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="250" Width="100%"
                    Settings-HorizontalScrollBarMode="Auto" SettingsBehavior-ColumnResizeMode="Control" Styles-SearchPanel-CssClass="searchBoxSmall">
                    <SettingsPager Visible="false"></SettingsPager>
                    <SettingsBehavior AllowDragDrop="true" AllowSort="False"></SettingsBehavior>
                    <Columns>

                        <dxe:GridViewDataTextColumn FieldName="ID" Caption="DETAILS_ID" VisibleIndex="0" Width="0" ReadOnly="true" Visible="false"></dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn SelectAllCheckboxMode="AllPages" FixedStyle="Left" VisibleIndex="1" ShowSelectCheckbox="true"></dxe:GridViewCommandColumn>

                        <dxe:GridViewDataTextColumn FieldName="SCH_CODE" FixedStyle="Left" Caption="Schedule No." VisibleIndex="2" Width="200" ReadOnly="true">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CONTRACT_NO" Caption="Contract No." VisibleIndex="3" Width="150" ReadOnly="true">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Customer" VisibleIndex="4" Width="200">
                           <%--  ReadOnly="true"--%>
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="SEGMENT1" Caption="State" VisibleIndex="5" Width="200" ReadOnly="true">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT2" Caption="Service Point" ReadOnly="true" Width="200" VisibleIndex="6">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT3" Caption="Segment 3" ReadOnly="true" Width="200" VisibleIndex="7">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT4" Caption="Segment 4" ReadOnly="true" Width="200" VisibleIndex="8">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT5" Caption="Segment 5" VisibleIndex="9" ReadOnly="True" Width="200">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SERVICE" Caption="Service" VisibleIndex="10" ReadOnly="True" Width="200">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="QUANTITY" Caption="Quantity" VisibleIndex="11" ReadOnly="True" Width="200">
                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.0000">
                                <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" AllowMouseWheel="false" />

                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UOM." VisibleIndex="12" ReadOnly="True" Width="200">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataDateColumn FieldName="SCHEDULE_DATE" Caption="Schedule Date" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" VisibleIndex="13" ReadOnly="True" Width="200">
                        </dxe:GridViewDataDateColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT1_CODE" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT2_CODE" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT3_CODE" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT4_CODE" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT5_CODE" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CUSTOMER_ID" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="PRODUCT_ID" VisibleIndex="-1" ReadOnly="True" Width="0" Visible="false">
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    <ClientSideEvents EndCallback="EndCallback" />
                    <SettingsDataSecurity AllowEdit="true" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                </dxe:ASPxGridView>
            </div>
        </div>
        <div class="">
            <div class="clearfix" style="margin-top: 13px; background: #f9f9f9; border: 1px solid #ccc; padding: 8px 0; border-radius: 5px;">
                <div class="col-md-3 assignBranch">
                    <div>Head Technician</div>
                    <dxe:ASPxComboBox ID="ddlTechnician" OnCallback="ddlTechnician_Callback" ClientInstanceName="cddlTechnician" runat="server" Width="100%">
                    </dxe:ASPxComboBox>
                </div>
                <div class="col-md-3 assignBranch">
                    <div>Subordinate Technician</div>
                    <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="285px" runat="server" AnimationType="None">
                        <DropDownWindowStyle BackColor="#EDEDED" />
                        <DropDownWindowTemplate>
                            <dxe:ASPxListBox OnCallback="listBox_Callback" Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                runat="server" Height="200" EnableSelectAll="true">
                                <Border BorderStyle="None" />
                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />

                                <ClientSideEvents SelectedIndexChanged="updateText" Init="updateText" />
                            </dxe:ASPxListBox>
                            <table style="width: 100%">
                                <tr>
                                    <td style="padding: 4px">
                                        <dxe:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                            <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </DropDownWindowTemplate>
                        <ClientSideEvents TextChanged="synchronizeListBoxValues" DropDown="synchronizeListBoxValues" />
                    </dxe:ASPxDropDownEdit>
                </div>
                <div class="col-md-3 assignBranch" style="padding-top: 13px;">
                    <button type="button" class="btn btn-success" onclick="AssignTechnician()">Assign </button>
                </div>
                <div class="col-md-3 unassignBranch1 hide" style="padding-top: 13px;">
                    <button type="button" class="btn btn-warning" onclick="UnAssignTechnician()">Un-Assign </button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnType" runat="server" />
    <asp:HiddenField ID="hdnSubTech" runat="server" />

</asp:Content>
