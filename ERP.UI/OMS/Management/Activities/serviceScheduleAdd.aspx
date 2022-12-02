<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="serviceScheduleAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.serviceScheduleAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js?v=2.0"></script>
    <link href="CSS/Stock-JournalTransfer.css" rel="stylesheet" />

    <script src="https://cdn3.devexpress.com/jslib/20.2.3/js/dx.all.js"></script>
    <link href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.2.3/css/dx.light.css" />
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
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function Customer_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
            }
        }

        function SaveRecord() {
            grid.batchEditApi.StartEdit(1)
            grid.UpdateEdit();

        }

        function ReturnToListing() {
            window.location.href = "SerViceScheduleList.aspx";
        }

        function ServiceContractButnClick(s, e) {
            $('#ServiceContractModel').modal('show');
        }

        function ServiceContract_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#ServiceContractModel').modal('show');
            }
        }

        function ServiceContractkeydown(e) {

            var OtherDetails = {}
            OtherDetails.CustomerId = GetObjectID('hdnCustomerId').value;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");
                if ($("#txtCustSearch").val() != '') {
                    callonServer("Services/Master.asmx/GetServiceContract", OtherDetails, "ServiceContractTable", HeaderCaption, "ServiceContractIndex", "SetServiceContract");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
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
                $('#CustModel').modal('hide');

                if ($('#hdnDocumentSegmentSettings').val() == "1") {


                    $.ajax({
                        type: "POST",
                        url: "serviceScheduleAdd.aspx/GetSegmentDetails",
                        data: JSON.stringify({ CustomerId: Id }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            OutStandingAmount = msg.d;
                            if (OutStandingAmount != null) {
                                if (OutStandingAmount.Segment1 != "") {
                                    var Segment1 = OutStandingAmount.Segment1;
                                    var Segment2 = OutStandingAmount.Segment2;
                                    var Segment3 = OutStandingAmount.Segment3;
                                    var Segment4 = OutStandingAmount.Segment4;
                                    var Segment5 = OutStandingAmount.Segment5;

                                    if (Segment1 == "0") {
                                        var div = document.getElementById('DivSegment1');
                                        div.style.display = 'none';

                                    }
                                    else {
                                        $('#lblSegment1').text(OutStandingAmount.SegmentName1);
                                        $('#ModuleSegment1header').text(OutStandingAmount.SegmentName1);
                                        
                                        $('#hdnValueSegment1').val("1");
                                    }
                                    if (Segment2 == "0") {
                                        var div = document.getElementById('DivSegment2');
                                        div.style.display = 'none';
                                         $('#hdnValueSegment2').val("0");
                                    }
                                    else {
                                        $('#lblSegment2').text(OutStandingAmount.SegmentName2);
                                        $('#ModuleSegment2Header').text(OutStandingAmount.SegmentName2);
                                        
                                        $('#hdnValueSegment2').val("1");
                                    }

                                    if (Segment3 == "0") {
                                        var div = document.getElementById('DivSegment3');
                                        div.style.display = 'none';
                                         $('#hdnValueSegment3').val("0");
                                    }
                                    else {
                                        $('#lblSegment3').text(OutStandingAmount.SegmentName3);
                                        $('#ModuleSegment3Header').text(OutStandingAmount.SegmentName3);
                                        $('#hdnValueSegment3').val("1");
                                    }

                                    if (Segment4 == "0") {
                                        var div = document.getElementById('DivSegment4');
                                        div.style.display = 'none';
                                         $('#hdnValueSegment4').val("0");
                                    }
                                    else {
                                        $('#lblSegment4').text(OutStandingAmount.SegmentName4);
                                        $('#ModuleSegment4Header').text(OutStandingAmount.SegmentName4);
                                        $('#hdnValueSegment4').val("1");
                                    }

                                    if (Segment5 == "0") {
                                        var div = document.getElementById('DivSegment5');
                                        div.style.display = 'none';
                                         $('#hdnValueSegment5').val("0");
                                    }
                                    else {
                                        $('#lblSegment5').text(OutStandingAmount.SegmentName5);
                                        $('#ModuleSegment5Header').text(OutStandingAmount.SegmentName5);
                                        $('#hdnValueSegment5').val("1");
                                    }
                                }
                            }
                            else {

                                document.getElementById('DivSegment1').style.display = 'none';
                                document.getElementById('DivSegment2').style.display = 'none';
                                document.getElementById('DivSegment3').style.display = 'none';
                                document.getElementById('DivSegment4').style.display = 'none';
                                document.getElementById('DivSegment5').style.display = 'none';
                            }
                        }

                    });

                }
            }
        }

        function SetServiceContract(Id, Name) {

            if (Id) {
                $('#ServiceContractModel').modal('hide');
                ctxtServiceContract.SetText(Name);
                GetObjectID('hdnServiceContractId').value = Id;
                cddlProduct.PerformCallback(Id);

            }
        }

        function ChangeProduct(s, e) {
            var idFrequency = cddlProduct.GetValue();
            var Frequency = idFrequency.split('~')[1];
            var NoOfService = idFrequency.split('~')[2];
            ctxtFrequency.SetText(Frequency);
            ctxtNoOfService.SetText(NoOfService);


            if (Frequency == "DAILY") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if (!$("#tdDate").hasClass('hide'))
                    $("#tdDate").addClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');

            }
            else if (Frequency == "HALF-YEARLY") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if ($("#tdDate").hasClass('hide'))
                    $("#tdDate").removeClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "MONTHLY ONCE") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if ($("#tdDate").hasClass('hide'))
                    $("#tdDate").removeClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "MONTHLY THRICE") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if ($("#tdDate").hasClass('hide'))
                    $("#tdDate").removeClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "MONTHLY TWISE") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if ($("#tdDate").hasClass('hide'))
                    $("#tdDate").removeClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "QUARTERLY ONCE") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if ($("#tdDate").hasClass('hide'))
                    $("#tdDate").removeClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "SINGLE-ONCE") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if (!$("#tdDate").hasClass('hide'))
                    $("#tdDate").addClass('hide');
                if ($("#tdOnce").hasClass('hide'))
                    $("#tdOnce").removeClass('hide');


            }
            else if (Frequency == "WEEKLY ONCE") {
                if ($("#tdDay").hasClass('hide'))
                    $("#tdDay").removeClass('hide');
                if (!$("#tdDate").hasClass('hide'))
                    $("#tdDate").addClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "WEEKLY THRICE") {
                if ($("#tdDay").hasClass('hide'))
                    $("#tdDay").removeClass('hide');
                if (!$("#tdDate").hasClass('hide'))
                    $("#tdDate").addClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "WEEKLY TWISE") {
                if ($("#tdDay").hasClass('hide'))
                    $("#tdDay").removeClass('hide');
                if (!$("#tdDate").hasClass('hide'))
                    $("#tdDate").addClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
            else if (Frequency == "YEARLY THRICE") {
                if (!$("#tdDay").hasClass('hide'))
                    $("#tdDay").addClass('hide');
                if ($("#tdDate").hasClass('hide'))
                    $("#tdDate").removeClass('hide');
                if (!$("#tdOnce").hasClass('hide'))
                    $("#tdOnce").addClass('hide');
            }
        }

        function doSchedule() {
            grid.PerformCallback('BindSchedule')
        }

        function ValueSelected(e, indexName) {
            if ((indexName == "ProdIndex") || (indexName == "customerIndex")) {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Id = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                    if (Id) {
                        if (indexName == "ProdIndex")
                            SetProduct(Id, name);
                        else if (indexName == "customerIndex")
                            SetCustomer(Id, name);
                        else if (indexName == "segment1Index") {
                            Setsegment1(Id, name);
                        }
                        else if (indexName == "segment2Index") {
                            Setsegment2(Id, name);
                        }
                        else if (indexName == "ServiceContractIndex") {
                            SetServiceContract(Id, name);
                        }
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
                        if (indexName == "ProdIndex")
                            $('#txtProdSearch').focus();
                        else if (indexName == "customerIndex")
                            $('#txtCustSearch').focus();
                        else if (indexName == "customerIndex")
                            $('#txtCustSearch').focus();
                        else if (indexName == "customerIndex")
                            $('#txtCustSearch').focus();
                    }
                }
            }

            else if (indexName == "MainAccountSIIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnSIMainAccount").val(Code);
                    cSIMainAccount.SetText(name);
                    cSIMainAccount_active.SetText(name);
                    cMainAccountModelSI.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountSearch').focus();
                    }
                }

            }

            else if (indexName == "HSNIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnHSN").val(Code);
                    cHSNCode.SetText(name);
                    cCmbTradingLotUnits.SetFocus();
                    cPopHSN.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtHSNSearch').focus();
                    }
                }

            }



            else if (indexName == "classIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#ClassId").val(Code);
                    cProClassCode.SetText(name);

                    cPopClass.Hide();
                    cCmbStatus.SetFocus();

                } else if (e.code == "ArrowDown") {
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
                        $('#txtClassNameSearch').focus();
                    }
                }

            }




            else if (indexName == "MainAccountSRIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnSRMainAccount").val(Code);
                    cSRMainAccount.SetText(name);
                    cSRMainAccount_active.SetText(name)
                    cMainAccountModelSR.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountSRSearch').focus();
                    }
                }

            }
            else if (indexName == "MainAccountPIIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnPIMainAccount").val(Code);
                    cPIMainAccount.SetText(name);
                    cPIMainAccount_active.SetText(name);
                    cMainAccountModelPI.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountPISearch').focus();
                    }
                }

            }
            else if (indexName == "MainAccountPRIndex") {
                if (e.code == "Enter" || e.code == "NumpadEnter") {
                    var Code = e.target.parentElement.parentElement.cells[0].innerText;
                    var name = e.target.parentElement.parentElement.cells[1].children[0].value;

                    $("#hdnPRMainAccount").val(Code);
                    cPRMainAccount.SetText(name);
                    cPRMainAccount_active.SetText(name);
                    cMainAccountModelPR.Hide();
                } else if (e.code == "ArrowDown") {
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
                        $('#txtMainAccountPRSearch').focus();
                    }
                }

            }
        }

        function Segment1ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {
                $('#Segment1Model').modal('show');

                var OtherDetails = {}
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                OtherDetails.orderdetails_id = cddlProduct.GetValue().split('~')[0];
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment1Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment1",
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
                OtherDetails.orderdetails_id = cddlProduct.GetValue().split('~')[0];
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment3Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment3",
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
                OtherDetails.orderdetails_id = cddlProduct.GetValue().split('~')[0];
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment4Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment4",
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
                OtherDetails.orderdetails_id = cddlProduct.GetValue().split('~')[0];
                OtherDetails.order_id = $("#hdnServiceContractId").val();

                $("#Segment5Container").dxDataGrid({
                    dataSource: new DevExpress.data.CustomStore({
                        load: function () {
                            var deferred = $.Deferred();

                            var xhr = $.ajax({
                                method: "POST",
                                url: "Services/Master.asmx/GetScheduleSegment5",
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

        function Segment1keydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSegment1Search").val();
            OtherDetails.CustomerIds = $("#hdnCustomerId").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Name");
                callonServer("Services/Master.asmx/GetSegment1", OtherDetails, "Segment1Table", HeaderCaption, "segment1Index", "Setsegment1");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[segment1Index=0]"))
                    $("input[segment1Index=0]").focus();
            }
            else if (e.code == "Escape") {
                ctxtSegment1.Focus();
            }
        }

        function Segment1_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#Segment1Model').modal('show');
                $("#txtSegment1Search").focus();
            }
        }

        function Setsegment1(Id, Name, e) {

            var LookUpData = Id;
            var ProductCode = Name;
            if (!ProductCode) {
                LookUpData = null;
            }
            $('#Segment1Model').modal('hide');
            ctxtSegment1.SetText(ProductCode);
            $('#hdnSegment1').val(LookUpData);

            if ($('#hdnValueSegment2').val() == "1") {
                var OtherDetails = {}
                OtherDetails.SearchKey = $("#txtSegment2Search").val();
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Name");
                callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
                $('#Segment2Model').modal('show');
            }




        }
        function Segment2ButnClick(s, e) {
            if ($("#hdnCustomerId").val() != "") {


                if ($("#hdnSegment1").val() != "") {

                    $('#Segment2Model').modal('show');
                    var OtherDetails = {}
                    OtherDetails.Segment1_Id = $("#hdnSegment1").val();
                    OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                    OtherDetails.orderdetails_id = cddlProduct.GetValue().split('~')[0];
                    OtherDetails.order_id = $("#hdnServiceContractId").val();

                    $("#Segment2Container").dxDataGrid({
                        dataSource: new DevExpress.data.CustomStore({
                            load: function () {
                                var deferred = $.Deferred();

                                var xhr = $.ajax({
                                    method: "POST",
                                    url: "Services/Master.asmx/GetScheduleSegment2",
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
        function Segment2keydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSegment1Search").val();
            OtherDetails.CustomerIds = $("#hdnCustomerId").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Name");
                callonServer("Services/Master.asmx/GetSegment2", OtherDetails, "Segment2Table", HeaderCaption, "segment2Index", "Setsegment2");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[segment2Index=0]"))
                    $("input[segment2Index=0]").focus();
            }
            else if (e.code == "Escape") {
                ctxtSegment2.Focus();
            }
        }
        function Segment2_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#Segment2Model').modal('show');
                $("#txtSegment2Search").focus();
            }
        }
        function Setsegment2(Id, Name, e) {

            var LookUpData = Id;
            var ProductCode = Name;
            if (!ProductCode) {
                LookUpData = null;
            }
            $('#Segment2Model').modal('hide');
            ctxtSegment2.SetText(ProductCode);
            $('#hdnSegment2').val(LookUpData);

            SetDefaultSegmentBillingShippingAddress($("#hdnCustomerId").val(), Id);
            if ($('#hdnValueSegment3').val() == "1") {
                var OtherDetails = {}
                OtherDetails.SearchKey = $("#txtSegment1Search").val();
                OtherDetails.CustomerIds = $("#hdnCustomerId").val();
                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Name");
                callonServer("Services/Master.asmx/GetSegment3", OtherDetails, "Segment3Table", HeaderCaption, "segment3Index", "Setsegment3");
                $('#Segment3Model').modal('show');
            }


        }

        var textSeparator = ";";
        function updateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(getSelectedItemsText(selectedItems));
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



        var textSeparator1 = ";";
        function updateText1() {
            var selectedItems = checkListBox1.GetSelectedItems();
            checkComboBox1.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValues1(dropDown, args) {
            checkListBox1.UnselectAll();
            var texts = dropDown1.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            checkListBox1.SelectValues(values);
            updateText(); // for remove non-existing texts
        }

        function EndCallback(s, e) {
            if (grid.cpmsg != null) {
                jAlert(grid.cpmsg, 'Alert', function () {
                    if (grid.cpmsg == "Saved Sucessfully.") {
                        window.location.href = "SerViceScheduleList.aspx";
                    }

                });

                grid.cpmsg = null;
            }
        }



    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Service Schedule Add
            </h3>

            <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
            <div id="divcross" runat="server" class="crossBtn"><a href="SerViceScheduleList.aspx"><i class="fa fa-times"></i></a></div>

        </div>
    </div>


    <div class="form_main ">
        <div style="background: #f5f4f3; padding: 0 15px 8px 15px; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;">
            <table class="padTabD " style="margin-top: 7px; width: 100%;">
                <tr>
                    <td>
                        <label>Customer</label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                    <td>
                        <label>Service Contract</label>
                        <div>
                            <dxe:ASPxButtonEdit ID="txtServiceContract" runat="server" ReadOnly="true" ClientInstanceName="ctxtServiceContract" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){ServiceContractButnClick();}" KeyDown="function(s,e){ServiceContract_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                    <td>
                        <label>Select Product</label>
                        <div>
                            <dxe:ASPxComboBox ID="ddlProduct" runat="server" ClientSideEvents-SelectedIndexChanged="ChangeProduct" ClientInstanceName="cddlProduct" OnCallback="ddlProduct_Callback" Width="100%" TabIndex="5">
                            </dxe:ASPxComboBox>
                        </div>
                    </td>
                    <td>
                        <label>Frequency</label>
                        <div>
                            <dxe:ASPxTextBox ID="txtFrequency" runat="server" ReadOnly="true" ClientInstanceName="ctxtFrequency" Width="100%" TabIndex="5">
                            </dxe:ASPxTextBox>
                        </div>
                    </td>
                    <td>
                        <label># of Service</label>
                        <div>
                            <dxe:ASPxTextBox ID="txtNoOfService" runat="server" ReadOnly="true" ClientInstanceName="ctxtNoOfService" Width="100%" TabIndex="5">
                            </dxe:ASPxTextBox>
                        </div>
                    </td>
                    <td class="" id="tdOnce1">
                        <label>Start Date<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxDateEdit ID="OnceDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cOnceDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                           
                        </div>
                    </td>

                    <td style="width: 150px" id="tdDay1" class="hide" >
                        <label>Select Day</label>
                        <div>
                            <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit2" Width="285px" runat="server" AnimationType="None">
                                <DropDownWindowStyle BackColor="#EDEDED" />
                                <DropDownWindowTemplate>
                                    <dxe:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                        runat="server" Height="200" EnableSelectAll="true">

                                        <Border BorderStyle="None" />
                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                        <Items>
                                            <dxe:ListEditItem Value="Monday" Text="Monday" />
                                            <dxe:ListEditItem Value="Tuesday" Text="Tuesday" />
                                            <dxe:ListEditItem Value="Wednesday" Text="Wednesday" />
                                            <dxe:ListEditItem Value="Thursday" Text="Thursday" />
                                            <dxe:ListEditItem Value="Friday" Text="Friday" />
                                            <dxe:ListEditItem Value="Saturday" Text="Saturday" />
                                            <dxe:ListEditItem Value="Sunday" Text="Sunday" />
                                        </Items>
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
                    </td>

                    <td style="width: 150px" id="tdDate1" class="hide">
                        <label>Select Date</label>
                        <div>
                            <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox1" ID="ASPxDropDownEdit1" Width="285px" runat="server" AnimationType="None">
                                <DropDownWindowStyle BackColor="#EDEDED" />
                                <DropDownWindowTemplate>
                                    <dxe:ASPxListBox Width="100%" ID="listBox1" ClientInstanceName="checkListBox1" SelectionMode="CheckColumn"
                                        runat="server" Height="200" EnableSelectAll="true">

                                        <Border BorderStyle="None" />
                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                        <Items>
                                            <dxe:ListEditItem Value="1" Text="1" />
                                            <dxe:ListEditItem Value="2" Text="2" />
                                            <dxe:ListEditItem Value="3" Text="3" />
                                            <dxe:ListEditItem Value="4" Text="4" />
                                            <dxe:ListEditItem Value="5" Text="5" />
                                            <dxe:ListEditItem Value="6" Text="6" />
                                            <dxe:ListEditItem Value="7" Text="7" />
                                            <dxe:ListEditItem Value="8" Text="8" />
                                            <dxe:ListEditItem Value="9" Text="9" />
                                            <dxe:ListEditItem Value="10" Text="10" />
                                            <dxe:ListEditItem Value="11" Text="11" />
                                            <dxe:ListEditItem Value="12" Text="12" />
                                            <dxe:ListEditItem Value="13" Text="13" />
                                            <dxe:ListEditItem Value="14" Text="14" />
                                            <dxe:ListEditItem Value="15" Text="15" />
                                            <dxe:ListEditItem Value="16" Text="16" />
                                            <dxe:ListEditItem Value="17" Text="17" />
                                            <dxe:ListEditItem Value="18" Text="18" />
                                            <dxe:ListEditItem Value="19" Text="19" />
                                            <dxe:ListEditItem Value="20" Text="20" />
                                            <dxe:ListEditItem Value="21" Text="21" />
                                            <dxe:ListEditItem Value="22" Text="22" />
                                            <dxe:ListEditItem Value="23" Text="23" />
                                            <dxe:ListEditItem Value="24" Text="24" />
                                            <dxe:ListEditItem Value="25" Text="25" />
                                            <dxe:ListEditItem Value="26" Text="26" />
                                            <dxe:ListEditItem Value="27" Text="27" />
                                            <dxe:ListEditItem Value="28" Text="28" />
                                            <dxe:ListEditItem Value="29" Text="29" />
                                            <dxe:ListEditItem Value="30" Text="30" />
                                            <dxe:ListEditItem Value="31" Text="31" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="updateText1" Init="updateText1" />
                                    </dxe:ASPxListBox>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="padding: 4px">
                                                <dxe:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                    <ClientSideEvents Click="function(s, e){ checkComboBox1.HideDropDown(); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </DropDownWindowTemplate>
                                <ClientSideEvents TextChanged="synchronizeListBoxValues1" DropDown="synchronizeListBoxValues1" />
                            </dxe:ASPxDropDownEdit>









                        </div>
                    </td>
                </tr>
                <tr>
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
                    <td id="DivSegment4" runat="server" >
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
                    <td id="DivSegment5" runat="server" >
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

                    <td style="width: 200px; padding-top: 21px;">
                        <button type="button" class="btn btn-success" onclick="doSchedule();">Schedule</button>

                    </td>

                </tr>
            </table>
        </div>



        <div class="row">
            <div class="col-sm-12 " style="padding-top: 15px">

                <dxe:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="ID" TabIndex="6" OnDataBinding="grid_DataBinding"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" OnCustomCallback="grid_CustomCallback"
                    OnRowInserting="Grid_RowInserting"
                    OnRowUpdating="Grid_RowUpdating"
                    OnRowDeleting="Grid_RowDeleting"
                    Settings-ShowFooter="false" SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="500"
                    Settings-HorizontalScrollBarMode="Auto" OnCellEditorInitialize="grid_CellEditorInitialize" OnBatchUpdate="grid_BatchUpdate">
                    <SettingsPager Visible="false"></SettingsPager>
                    <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                    <Columns>

                        <dxe:GridViewDataTextColumn FieldName="ID" FixedStyle="Left" Caption="ID" VisibleIndex="1" Width="100" ReadOnly="true" Visible="false">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="SCH_CODE" FixedStyle="Left" Caption="Schedule No." VisibleIndex="2" Width="200" ReadOnly="true">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CONTRACT_NO" FixedStyle="Left" Caption="Contract No." VisibleIndex="3" Width="150" ReadOnly="true">
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
                        <dxe:GridViewDataTextColumn FieldName="QUANTITY" Caption="Quantity" VisibleIndex="11" ReadOnly="false" Width="200">
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

                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                    </SettingsEditing>
                </dxe:ASPxGridView>

            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="padding-top: 15px">
                <button type="button" class="btn btn-success" onclick="SaveRecord();">Save & Exit </button>
                <button type="button" class="btn btn-danger" onclick="ReturnToListing()">Cancel </button>
            </div>
        </div>
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
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />
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
    <asp:HiddenField runat="server" ID="hdnCustomerId" />
    <asp:HiddenField runat="server" ID="hdnServiceContractId" />
    <asp:HiddenField runat="server" ID="hdnSegment1" />
    <asp:HiddenField runat="server" ID="hdnSegment2" />
    <asp:HiddenField runat="server" ID="hdnSegment3" />
    <asp:HiddenField runat="server" ID="hdnSegment4" />
    <asp:HiddenField runat="server" ID="hdnSegment5" />
    <asp:HiddenField runat="server" ID="hdnValueSegment1" />
    <asp:HiddenField runat="server" ID="hdnValueSegment2" />
     <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />  
    <asp:HiddenField runat="server" ID="hdnValueSegment3" />
    <asp:HiddenField runat="server" ID="hdnValueSegment4" />
    <asp:HiddenField runat="server" ID="hdnValueSegment5" />

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

    <div class="modal fade" id="ServiceContractModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title" id="ModuleServiceContract"></h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="ServiceContractkeydown(event)" id="txtServiceContractSearch" autofocus width="100%" placeholder="Search By Service contract number." />
                    <div id="ServiceContractTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
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
    
</asp:Content>
