<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="serviceScheduleList.aspx.cs" Inherits="ERP.OMS.Management.Activities.serviceScheduleList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=0.02"></script>

    <script src="https://cdn3.devexpress.com/jslib/20.2.3/js/dx.all.js"></script>
    <link href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.light.css" />
    <script>
        var isFirstTime = true;
        $(document).ready(function () {
            //var fromdate = new Date();
            //cFormDate.SetDate(fromdate);
            //ctoDate.SetDate(fromdate);
        });

        function OnAddButtonClick() {
            window.location.href = "/OMS/Management/Activities/serviceScheduleAdd.aspx";
        }
    </script>
    <style>
        .padTabD {
            width: 80%;
        }

            .padTabD > tbody > tr > td {
                padding-right: 20px;
            }

                .padTabD > tbody > tr > td .form-control {
                    margin-bottom: 0;
                }
    </style>
    <script>
        function AssignBranch(orderid, Details_id, EntityID, Schedule_Id) {
            window.location.href = 'ServiceAssignBranch.aspx?EntityID=' + EntityID + '&ID=' + orderid + '&Details_Id=' + Details_id + '&Schedule_Id=' + Schedule_Id;
        }

        function ShowDetails() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else {
                localStorage.setItem("ScheduleDetailsFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ScheduleDetailsToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
                //localStorage.setItem("ScheduleDetailsState", ccmbStatefilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                //$("#hfStateID").val(ccmbStatefilter.GetValue());
                $("#hfIsFilter").val("Y");

                $.ajax({
                    type: "POST",
                    url: "serviceScheduleList.aspx/GetSServiceScheduleList",
                    data: JSON.stringify({
                        FromDate: $("#hfFromDate").val(), ToDate: $("#hfToDate").val(), CustomerId: $("#hdnCustomerId").val(), segment1: $("#hdnSegment1").val(),
                        segment2: $("#hdnSegment2").val(), segment3: $("#hdnSegment3").val(), segment4: $("#hdnSegment4").val(), segment5: $("#hdnSegment5").val()
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        //OutStandingAmount = msg.d;
                        grid.Refresh();
                    }
                });               
            }
        }

        function AllControlInitilize() {
            if (isFirstTime) {

                if (localStorage.getItem('ScheduleDetailsFromDate')) {
                    var fromdatearray = localStorage.getItem('ScheduleDetailsFromDate').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ScheduleDetailsToDate')) {
                    var todatearray = localStorage.getItem('ScheduleDetailsToDate').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
               

                isFirstTime = false;
            }

        }

        function gridRowclick(s, e) {


            $('#grid').find('tr').removeClass('rowActive');
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
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);


        }
    </script>
    <script>
        function CustomerButnClick(s, e) {
            //if (ccmbBranchfilter.GetValue() == "0") {
            //    ccmbBranchfilter.Focus();

            //}
            //else {
            //    $('#CustModel').modal('show');
            //}
            $('#CustModel').modal('show');
        }
        function CustomerKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#CustModel').modal('show');
            }
        }
        function Customerkeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtCustSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }

        }
        function SetCustomer(Id, Name) {
            if (Id) {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                GetObjectID('hdnCustomerId').value = Id;

                ctxtSegment1.SetText("");
                $('#hdnSegment1').val("");
                ctxtSegment2.SetText("");
                $('#hdnSegment2').val("");
                ctxtSegment3.SetText("");
                $('#hdnSegment3').val("");
                ctxtSegment4.SetText("");
                $('#hdnSegment4').val("");
                ctxtSegment5.SetText("");
                $('#hdnSegment5').val("");

            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "customerIndex")
                        SetCustomer(Id, name);

                }

            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "customerIndex")
                        $('#txtCustSearch').focus();
                }
            }

        }

    </script>
    <script>

        function Segment1ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment1Model').modal('show');

                var OtherDetails = {}
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.orderdetails_id = "0";
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment1Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment1List",
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

        function Segment2ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                if ($("#hdnSegment1").val() != "") {
                    $('#Segment2Model').modal('show');
                    var OtherDetails = {}
                    OtherDetails.Segment1_Id = $("#hdnSegment1").val();
                    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                    OtherDetails.orderdetails_id = "0";
                    OtherDetails.order_id = $("#hdnServiceContractId").val();

                    $("#Segment2Container").dxDataGrid({
                        dataSource: new DevExpress.data.CustomStore({
                            load: function () {
                                var deferred = $.Deferred();

                                var xhr = $.ajax({
                                    method: "POST",
                                    url: "Services/Master.asmx/GetScheduleSegment2List",
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

        function Segment3ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment3Model').modal('show');

                var OtherDetails = {}
                OtherDetails.SearchKey = '';
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.Segment2_Id = $("#hdnSegment2").val();
                OtherDetails.orderdetails_id = "0";
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment3Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment3List",
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
                OtherDetails.orderdetails_id = "0";
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment4Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment4List",
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
                OtherDetails.orderdetails_id = "0";
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment5Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment5List",
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Service Schedule List </h3>
        </div>


    </div>
    <div class="form_main">
        <div class="clearfix">
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius">
                <span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span>

            </a>
            <table class="padTabD " style="margin-top: 7px; width:100%">
                <tr>
                    <td >
                        <label>Customer</label>
                        <div>
                            <%--<select class="form-control">
                                <option>Unit Select</option>
                            </select>--%>
                            <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>

                    <td>
                        <label>From</label>
                        <div>
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                    </td>

                    <td >
                        <label>To</label>
                        <div>
                            <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                    </td>
                    <td class="hide">
                        <label>State</label>
                        <div>
                            <%--<select class="form-control">
                                <option>Select</option>
                            </select>--%>
                            <dxe:ASPxComboBox ID="cmbStatefilter" runat="server" ClientInstanceName="ccmbStatefilter" Width="100%">
                                <%--<ClientSideEvents SelectedIndexChanged="BranchChange" />--%>
                            </dxe:ASPxComboBox>
                        </div>
                    </td>
                    <td class="hide">
                        <label>Service Point</label>
                        <div>
                            <select class="form-control">
                                <option>Select</option>
                            </select>
                        </div>
                    </td>
                     <td id="DivSegment1" runat="server" >
                        <label id="lblSegment1">State</label>
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
                    <td id="DivSegment2" runat="server" >
                        <label id="lblSegment2">Service Point</label>
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
                    

                </tr>
                <tr>
                    <td id="DivSegment3" runat="server" >
                        <label id="lblSegment3">Segment 3</label>
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
                    <td id="DivSegment4" runat="server"  >
                        <label id="lblSegment4">Segment 4</label>
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
                    <td id="DivSegment5" runat="server"  >
                        <label id="lblSegment5">Segment 5</label>
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

                    <td style="width: 250px; padding-top: 21px;" colspan="2">
                        <button class="btn btn-success" type="button" onclick="ShowDetails()">Show</button>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" AutoPostBack="true">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>






        </div>
    </div>

    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="grid" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="EntityServerModeDataSource"
            Width="100%" ClientInstanceName="grid" OnCustomCallback="Grdstockjournal_CustomCallback"
            OnDataBinding="gridJournal_DataBinding" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>


                <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" CellStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle"
                    VisibleIndex="1" Width="200">
                    <Settings AllowAutoFilter="False"
                        AllowSort="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderStyle HorizontalAlign="Center" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Order Number" FieldName="Order_Number"
                    VisibleIndex="2" Width="200">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataDateColumn Caption="Doc. Date" FieldName="Order_Date" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy"
                    VisibleIndex="3" Width="200">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataDateColumn>


                <dxe:GridViewDataTextColumn Caption="Service" FieldName="OrderDetails_ProductDescription"
                    VisibleIndex="4" Width="200">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Created By" FieldName="user_name"
                    VisibleIndex="4" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataDateColumn Caption="Created On" FieldName="CREATED_ON" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy"
                    VisibleIndex="5" Width="200">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Right" />
                    <Settings AllowAutoFilter="False" />
                </dxe:GridViewDataDateColumn>






                <dxe:GridViewDataTextColumn VisibleIndex="11" Width="1px" CellStyle-HorizontalAlign="Right">
                    <CellStyle CssClass="gridcellleft" Wrap="true" HorizontalAlign="Center">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <DataItemTemplate>

                        <div class='floatedBtnArea'>

                            <a href="javascript:void(0);" onclick="AssignBranch('<%# Eval("Order_Id") %>', '<%# Eval("Details_Id") %>', '<%# Eval("cnt_internalid") %>', '<%# Eval("ID") %>')" title="">
                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>

                        </div>
                    </DataItemTemplate>

                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents />



            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>

            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />


            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <ClientSideEvents RowClick="gridRowclick" />
            <TotalSummary>

                <dxe:ASPxSummaryItem FieldName="Quantity" SummaryType="Sum" />
                <dxe:ASPxSummaryItem FieldName="Journal_Number" SummaryType="count" />





            </TotalSummary>

        </dxe:ASPxGridView>

        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hidden_replacementId" runat="server" />

    </div>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoiceList" />
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridjournal" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />

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



    <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfStateID" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hdnCustomerId" runat="server" />


    <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />  
     <asp:HiddenField runat="server" ID="hdnServiceContractId" />
    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />
</asp:Content>
