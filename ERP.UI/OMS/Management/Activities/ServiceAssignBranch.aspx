<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceAssignBranch.aspx.cs" Inherits="ERP.OMS.Management.Activities.ServiceAssignBranch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn3.devexpress.com/jslib/20.2.3/js/dx.all.js"></script>
    <link href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.light.css" />
    <script>

        function ShowList() {
            grid.PerformCallback('BindGrid');
        }

        function EndCallback(s,e)
        {
            if(grid.cpMsg!="")
            {               
                jAlert(grid.cpMsg, 'Alert');
                grid.cpMsg = "";
            }
        }

        function TypeChange(s,e)
        {
            if (cddlStatus.GetValue() == "Assign")
            {
                if (!$(".assignBranch").hasClass('hide'))
                    $(".assignBranch").addClass('hide');
                if ($(".unassignBranch").hasClass('hide'))
                    $(".unassignBranch").removeClass('hide');

            }
            else
            {
                if (!$(".unassignBranch").hasClass('hide'))
                    $(".unassignBranch").addClass('hide');
                if ($(".assignBranch").hasClass('hide'))
                    $(".assignBranch").removeClass('hide');
            }
            grid.PerformCallback('BlankGrid');
        }

        function AssignBranch()
        {
            if (cddlBranch.GetValue() == "") {
                jAlert('Please select a branch to assign', 'Alert');
            }
            else {
                grid.PerformCallback('AssignAll');
            }
        }

        function UnAssignBranch()
        {
            grid.PerformCallback('UnAssignAll');
        }


        function Segment1ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment1Model').modal('show');

                var OtherDetails = {}
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.orderdetails_id = $("#hdnServiceContractDetailsId").val();
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment1Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleAssignSegment1",
                                //data: JSON.stringify(Filters), 
                                data: JSON.stringify(OtherDetails),
                                contentType: "application/json",
                                dataType: "JSON",
                                //async: false,  
                                success: function (result) {
                                    console.log('before resolve');
                                    deferred.resolve(result.d);
                                    console.log('after resolve');
                                }
                            });
                            return deferred.promise();
                        }
                    }),


                    keyExpr: "id",
                    selection: {
                        mode: "multiple"
                    },
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    hoverStateEnabled: true,
                    showBorders: true,
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 25, 50, 100]
                    },
                    columnAutoWidth: true,
                    remoteOperations: false,
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true
                    },

                    allowColumnReordering: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    columns: [
                        {
                            dataField: "Segment1",
                            caption: "Segment",
                            dataType: "string"
                        },
                        {
                            dataField: "SegmentName",
                            caption: "Segment Name",
                            dataType: "string"
                        }
                    ],
                    onSelectionChanged: function (selectedItems) {
                        var data = selectedItems.selectedRowsData;

                        var segmentName = "";
                        var segmentId = "";



                        for (var i = 0; i < selectedItems.selectedRowsData.length; i++) {
                            segmentName = segmentName + ',' + selectedItems.selectedRowsData[i].SegmentName;
                            segmentId = segmentId + ',' + selectedItems.selectedRowsData[i].id;
                        }

                        segmentName = segmentName.replace(/^,/, '');;
                        segmentId = segmentId.replace(/^,/, '');;

                        if (data) {
                            ctxtSegment1.SetText(segmentName);
                            $('#hdnSegment1').val(segmentId);
                            // this.clearSelection();
                        }
                    }
                });




            }
            else {
                jAlert("Please Select Customer");
            }

        }

        function Segment3ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment3Model').modal('show');

                var OtherDetails = {}
                OtherDetails.SearchKey = '';
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.Segment2_Id = $("#hdnSegment2").val();
                OtherDetails.orderdetails_id = $("#hdnServiceContractDetailsId").val();
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment3Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleAssignSegment3",
                                //data: JSON.stringify(Filters), 
                                data: JSON.stringify(OtherDetails),
                                contentType: "application/json",
                                dataType: "JSON",
                                //async: false,  
                                success: function (result) {
                                    console.log('before resolve');
                                    deferred.resolve(result.d);
                                    console.log('after resolve');
                                }
                            });
                            return deferred.promise();
                        }
                    }),


                    keyExpr: "id",
                    selection: {
                        mode: "multiple"
                    },
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    hoverStateEnabled: true,
                    showBorders: true,
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 25, 50, 100]
                    },
                    columnAutoWidth: true,
                    remoteOperations: false,
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true
                    },

                    allowColumnReordering: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    columns: [
                        {
                            dataField: "Segment1",
                            caption: "Segment",
                            dataType: "string"
                        },
                        {
                            dataField: "SegmentName",
                            caption: "Segment Name",
                            dataType: "string"
                        }
                    ],
                    onSelectionChanged: function (selectedItems) {
                        var data = selectedItems.selectedRowsData;

                        var segmentName = "";
                        var segmentId = "";



                        for (var i = 0; i < selectedItems.selectedRowsData.length; i++) {
                            segmentName = segmentName + ',' + selectedItems.selectedRowsData[i].SegmentName;
                            segmentId = segmentId + ',' + selectedItems.selectedRowsData[i].id;
                        }

                        segmentName = segmentName.replace(/^,/, '');;
                        segmentId = segmentId.replace(/^,/, '');;

                        if (data) {
                            ctxtSegment3.SetText(segmentName);
                            $('#hdnSegment3').val(segmentId);
                            // this.clearSelection();
                        }
                    }
                });




            }
            else {
                jAlert("Please Select Customer");
            }

        }

        function Segment4ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment4Model').modal('show');

                var OtherDetails = {}
                OtherDetails.SearchKey = '';
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.Segment3_Id = $("#hdnSegment3").val();
                OtherDetails.orderdetails_id = $("#hdnServiceContractDetailsId").val();
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment4Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleAssignSegment4",
                                //data: JSON.stringify(Filters), 
                                data: JSON.stringify(OtherDetails),
                                contentType: "application/json",
                                dataType: "JSON",
                                //async: false,  
                                success: function (result) {
                                    console.log('before resolve');
                                    deferred.resolve(result.d);
                                    console.log('after resolve');
                                }
                            });
                            return deferred.promise();
                        }
                    }),


                    keyExpr: "id",
                    selection: {
                        mode: "multiple"
                    },
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    hoverStateEnabled: true,
                    showBorders: true,
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 25, 50, 100]
                    },
                    columnAutoWidth: true,
                    remoteOperations: false,
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true
                    },

                    allowColumnReordering: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    columns: [
                        {
                            dataField: "Segment1",
                            caption: "Segment",
                            dataType: "string"
                        },
                        {
                            dataField: "SegmentName",
                            caption: "Segment Name",
                            dataType: "string"
                        }
                    ],
                    onSelectionChanged: function (selectedItems) {
                        var data = selectedItems.selectedRowsData;

                        var segmentName = "";
                        var segmentId = "";



                        for (var i = 0; i < selectedItems.selectedRowsData.length; i++) {
                            segmentName = segmentName + ',' + selectedItems.selectedRowsData[i].SegmentName;
                            segmentId = segmentId + ',' + selectedItems.selectedRowsData[i].id;
                        }

                        segmentName = segmentName.replace(/^,/, '');;
                        segmentId = segmentId.replace(/^,/, '');;

                        if (data) {
                            ctxtSegment4.SetText(segmentName);
                            $('#hdnSegment4').val(segmentId);
                            // this.clearSelection();
                        }
                    }
                });




            }
            else {
                jAlert("Please Select Customer");
            }

        }
        function Segment5ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment5Model').modal('show');

                var OtherDetails = {}
                OtherDetails.SearchKey = '';
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.Segment4_Id = $("#hdnSegment4").val();
                OtherDetails.orderdetails_id = $("#hdnServiceContractDetailsId").val();
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment5Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleAssignSegment5",
                                //data: JSON.stringify(Filters), 
                                data: JSON.stringify(OtherDetails),
                                contentType: "application/json",
                                dataType: "JSON",
                                //async: false,  
                                success: function (result) {
                                    console.log('before resolve');
                                    deferred.resolve(result.d);
                                    console.log('after resolve');
                                }
                            });
                            return deferred.promise();
                        }
                    }),


                    keyExpr: "id",
                    selection: {
                        mode: "multiple"
                    },
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    hoverStateEnabled: true,
                    showBorders: true,
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [10, 25, 50, 100]
                    },
                    columnAutoWidth: true,
                    remoteOperations: false,
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true
                    },

                    allowColumnReordering: true,
                    rowAlternationEnabled: true,
                    showBorders: true,
                    columns: [
                        {
                            dataField: "Segment1",
                            caption: "Segment",
                            dataType: "string"
                        },
                        {
                            dataField: "SegmentName",
                            caption: "Segment Name",
                            dataType: "string"
                        }
                    ],
                    onSelectionChanged: function (selectedItems) {
                        var data = selectedItems.selectedRowsData;

                        var segmentName = "";
                        var segmentId = "";



                        for (var i = 0; i < selectedItems.selectedRowsData.length; i++) {
                            segmentName = segmentName + ',' + selectedItems.selectedRowsData[i].SegmentName;
                            segmentId = segmentId + ',' + selectedItems.selectedRowsData[i].id;
                        }

                        segmentName = segmentName.replace(/^,/, '');;
                        segmentId = segmentId.replace(/^,/, '');;

                        if (data) {
                            ctxtSegment5.SetText(segmentName);
                            $('#hdnSegment5').val(segmentId);
                            // this.clearSelection();
                        }
                    }
                });




            }
            else {
                jAlert("Please Select Customer");
            }

        }
        function Segment2ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {


                if ($("#hdnSegment1").val() != "") {

                    $('#Segment2Model').modal('show');
                    var OtherDetails = {}
                    OtherDetails.Segment1_Id = $("#hdnSegment1").val();
                    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                    OtherDetails.orderdetails_id = $("#hdnServiceContractDetailsId").val();
                    OtherDetails.order_id = $("#hdnServiceContractId").val();

                    $("#Segment2Container").dxDataGrid({
                        dataSource: new DevExpress.data.CustomStore({
                            load: function () {
                                var deferred = $.Deferred();

                                var xhr = $.ajax({
                                    method: "POST",
                                    url: "Services/Master.asmx/GetScheduleAssignSegment2",
                                    //data: JSON.stringify(Filters), 
                                    data: JSON.stringify(OtherDetails),
                                    contentType: "application/json",
                                    dataType: "JSON",
                                    //async: false,  
                                    success: function (result) {
                                        console.log('before resolve');
                                        deferred.resolve(result.d);
                                        console.log('after resolve');
                                    }
                                });
                                return deferred.promise();
                            }
                        }),


                        keyExpr: "id",
                        selection: {
                            mode: "multiple"
                        },
                        filterRow: {
                            visible: true,
                            applyFilter: "auto"
                        },
                        hoverStateEnabled: true,
                        showBorders: true,
                        paging: {
                            pageSize: 10
                        },
                        pager: {
                            showPageSizeSelector: true,
                            allowedPageSizes: [10, 25, 50, 100]
                        },
                        columnAutoWidth: true,
                        remoteOperations: false,
                        searchPanel: {
                            visible: true,
                            highlightCaseSensitive: true
                        },

                        allowColumnReordering: true,
                        rowAlternationEnabled: true,
                        showBorders: true,
                        columns: [
                            {
                                dataField: "Segment1",
                                caption: "Segment",
                                dataType: "string"
                            },
                            {
                                dataField: "SegmentName",
                                caption: "Segment Name",
                                dataType: "string"
                            }
                        ],
                        onSelectionChanged: function (selectedItems) {
                            var data = selectedItems.selectedRowsData;

                            var segmentName = "";
                            var segmentId = "";



                            for (var i = 0; i < selectedItems.selectedRowsData.length; i++) {
                                segmentName = segmentName + ',' + selectedItems.selectedRowsData[i].SegmentName;
                                segmentId = segmentId + ',' + selectedItems.selectedRowsData[i].id;
                            }

                            segmentName = segmentName.replace(/^,/, '');;
                            segmentId = segmentId.replace(/^,/, '');;

                            if (data) {
                                ctxtSegment2.SetText(segmentName);
                                $('#hdnSegment2').val(segmentId);
                                // this.clearSelection();
                            }
                        }
                    });




                }
                else {
                    jAlert("Please Select Segment 1.");
                }











            }
            else {
                jAlert("Please Select Customer");
            }
        }

    </script>
    <style>
        .padTabD>tbody>tr>td {
            padding-right:10px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title clearfix">
        <h3 class="pull-left">Service Schedule Add
        </h3>

        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="SerViceScheduleList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
    <div class="form_main ">
        <div style="background: #f5f4f3; padding: 0 15px 8px 15px; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;">
            <table class="padTabD " style="margin-top: 7px; width: 100%;">

                <tr>
                    <td>
                        <label>State</label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtSegment1" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment1" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){Segment1ButnClick();}" KeyDown="function(s,e){Segment1_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                    <td>
                        <label>Service Point</label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtSegment2" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment2" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){Segment2ButnClick();}" KeyDown="function(s,e){Segment2_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                    <td>
                        <label>Segmnent 3</label>
                        <div>

                            <dxe:ASPxButtonEdit ID="txtSegment3" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment3" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){Segment3ButnClick();}" KeyDown="function(s,e){Segment3_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                    <td>
                        <label>Segmnent 4</label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtSegment4" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment4" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){Segment4ButnClick();}" KeyDown="function(s,e){Segment4_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                    <td>
                        <label>Segmnent 5</label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtSegment5" runat="server" ReadOnly="true" ClientInstanceName="ctxtSegment5" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){Segment5ButnClick();}" KeyDown="function(s,e){Segment5_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>

                    <td>
                        <label>Status</label>
                        <div>
                            <dxe:ASPxComboBox ID="ddlStatus" runat="server" ClientInstanceName="cddlStatus" Width="100%" TabIndex="5">
                                <Items>
                                    <dxe:ListEditItem Selected="true" Value="Assign" Text="Assign" />
                                    <dxe:ListEditItem Value="Un-Assign" Text="Un-Assign" />
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="TypeChange" />
                            </dxe:ASPxComboBox>
                        </div>
                    </td>

                    <td style="width: 200px; padding-top: 21px;">
                        <button type="button" class="btn btn-success" onclick="ShowList();">Show</button>

                    </td>

                </tr>
            </table>
        </div>



        <div class="row">
            <div class="col-sm-12 " style="padding-top: 15px">

                <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="DETAILS_ID" TabIndex="6" OnDataBinding="grid_DataBinding" OnCustomCallback="grid_CustomCallback"
                    Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="240"
                    Settings-HorizontalScrollBarMode="Auto" Width="100%">
                    <SettingsPager Visible="false"></SettingsPager>
                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                    <Columns>

                        <dxe:GridViewDataTextColumn FieldName="ID" Caption="DETAILS_ID" VisibleIndex="0" Width="0" ReadOnly="true" Visible="false"></dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn SelectAllCheckboxMode="AllPages" FixedStyle="Left" VisibleIndex="1" ShowSelectCheckbox="true"></dxe:GridViewCommandColumn>

                        <dxe:GridViewDataTextColumn FieldName="SCH_CODE" FixedStyle="Left" Caption="Schedule No." VisibleIndex="2" Width="200" ReadOnly="true">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CONTRACT_NO" Caption="Contract No." VisibleIndex="3" Width="150" ReadOnly="true">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Customer" VisibleIndex="4" Width="200" ReadOnly="true">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="SEGMENT1" Caption="State" VisibleIndex="5" Width="200" ReadOnly="true">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SEGMENT2" Caption="Service Point" ReadOnly="true" Width="200" VisibleIndex="6">
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

                </dxe:ASPxGridView>

            </div>
        </div>
        <div class="row">
            <div class="" style="padding-top: 15px">
                <div class="col-md-3 assignBranch hide">
                    <div>Branch</div>
                    <dxe:ASPxComboBox ID="ddlBranch" ClientInstanceName="cddlBranch" runat="server" Width="100%">
                    </dxe:ASPxComboBox>
                </div>
                <div class="col-md-3 assignBranch hide" style="padding-top: 13px;">
                    <button type="button" class="btn btn-success" onclick="AssignBranch()">Assign </button>
                </div>
                <div class="col-md-3 unassignBranch " style="padding-top: 13px;">
                    <button type="button" class="btn btn-warning" onclick="UnAssignBranch()">Un-Assign </button>
                </div>
            </div>
        </div>
    </div>


    <asp:HiddenField runat="server" ID="hdnScheduleid" />
    <asp:HiddenField runat="server" ID="hdnCustomerId" />
    <asp:HiddenField runat="server" ID="hdnServiceContractId" />
    <asp:HiddenField runat="server" ID="hdnServiceContractDetailsId" />
    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />
    <asp:HiddenField runat="server" ID="hdnValueSegment1" />
    <asp:HiddenField runat="server" ID="hdnValueSegment2" />

    <div class="modal fade" id="Segment1Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment1header"></h4>
                </div>
                <div class="modal-body">
                    <%--<input type="text" onkeydown="Segment1keydown(event)" id="txtSegment1Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />--%>
                    <div id="Segment1Container">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Segment2Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment2Header"></h4>
                </div>
                <div class="modal-body">
                    <%--<input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />--%>
                    <div id="Segment2Container">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Segment3Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment3Header"></h4>
                </div>
                <div class="modal-body">
                    <%--<input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />--%>
                    <div id="Segment3Container">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Segment4Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment4Header"></h4>
                </div>
                <div class="modal-body">
                    <%--<input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />--%>
                    <div id="Segment4Container">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="Segment5Model" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleSegment5Header"></h4>
                </div>
                <div class="modal-body">
                    <%--<input type="text" onkeydown="Segment2keydown(event)" id="txtSegment2Search" autofocus width="100%" placeholder="Search By Segment Name,Segment Code" />--%>
                    <div id="Segment5Container">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
