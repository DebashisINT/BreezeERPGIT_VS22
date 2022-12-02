<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechnicianAssign.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.TechnicianAssign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
<head runat="server">
    <title>Assign Technician</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;1,900&display=swap" rel="stylesheet" />
    <script src="/assests/js/jquery.min.js"></script>
    <style>
        body {
            font-family: 'Poppins', sans-serif;
        }
        .box {
            padding:15px;
            background:#ddd;
            padding: 15px;
            background: #f1f1f1;
            border-radius: 7px;
            border: 1px solid #e3e3e3;
            margin-bottom:20px;
        }
        .hdtec {
            display:inline-block;
            min-width:100px;
            font-weight:500;
            color: #26509d;
        }
        .box p {
            margin-top:5px;
            margin-bottom:8px;
            font-size:14px
        }
        .tableContent {
            margin-bottom:50px;
        }
        .tableContent table {
            width:100%
        }
        .tableContent table>thead>tr>th {
            text-align:left;
            font-size:13px;
            font-weight:500;
            background:blue;
            color:#fff;
            padding:4px 5px
        }
        .tableContent table>tbody>tr>td {
            font-size:13px;
            padding:4px 5px
        }
        .table-striped tbody tr:nth-of-type(odd) {
              background-color: rgba(0,0,0,.05);
        }
        .table-striped th, .table-striped td{
            border: 1px solid #dee2e6;
        }
        .table-striped th {
            border-color:#0000af
        }
        .clltoActions {
            position:fixed;
            bottom:0;
            left:0;
            width:100%;
            text-align:center;
        
            background: #fff;
        }
        .clltoActions .btn:first-child {
            margin-right:8px
        }
        .btn {
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            border: 1px solid transparent;
            padding: 0.375rem 0.75rem;
            font-size: 1rem;
            line-height: 1.5;
            border-radius: 0.25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            cursor:pointer;
        }
        .btn.done {
            color: #fff;
            background-color: #28a745;
            border-color: #28a745;
        }
        .btn.done:hover {
            color: #fff;
            background-color: #218838;
            border-color: #1e7e34;
        }
        .btn.reject {
            color: #fff;
            background-color: #dc3545;
            border-color: #dc3545;
        }
        .btn.reject:hover {
            color: #fff;
            background-color: #c82333;
            border-color: #bd2130;
        }
    </style>
    <script>
        function setSelection(data) {
            var i = 0;
            $("#problemsTable tr").each(function () {
                var dataS = data[i].ServiceAction;
                $(this).find('select').val(dataS);
                i++;
                
            })
            
        }
        $(function () {

            var url_string = window.location.href;
            var url = new URL(url_string);
            var id = url.searchParams.get("id");
            var AU = url.searchParams.get("AU");
            var dbname = url.searchParams.get("UniqueKey");
            console.log(AU);
            var obj = {
                ReceiptChallan_ID: id,
                technician_Id: AU,
                dbname: dbname
            }
            $.ajax({
                type: "POST",
                url: "TechnicianAssign.aspx/GetData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                //async: false,
                success: function (data) {
                    var selectedData = data.d;
                    console.log('GetData', data)
                    var topData = data.d;
                    $.ajax({
                        type: "POST",
                        url: "TechnicianAssign.aspx/GetListData",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(obj),
                        //async: false,
                        success: function (data) {
                            console.log('GetListData', data)
                            var GetListData = data.d;
                            var selectBox = "<select class='actionBox' required><option value=''>Select</option>";
                            for (i = 0; i < GetListData.length; i++) {
                                selectBox += "<option value='" + GetListData[i].ServiceMaster_ID + "'>" + GetListData[i].RepairingServiceDesc + "</option>"
                            }
                            selectBox += "</select>"
                            console.log(selectBox)
                            createRows(topData, selectBox)
                            if (selectedData.length) {
                                setSelection(selectedData)
                            }
                            
                        }

                    });
                }

            });
            HideDoneRej();
        })
        function createRows(topData, selectBox) {
            var tableRow;
            for (i = 0; i < topData.length; i++) {
                tableRow += "<tr>";
                tableRow += "<td>" + topData[i].RepPendingDetails_ID + "</td>"
                tableRow += "<td>" + topData[i].ProblemDesc + "</td>"
                tableRow += "<td>" + topData[i].SerialNo + "</td>"
                tableRow += "<td>" + selectBox + "</td>"
                tableRow += "</tr>"
            }
            
            //console.log("tableRow", tableRow)
            $("#problemsTable").html(tableRow);
            $("#challanNo").text(topData[0].ChallanNo)
            $("#Assignedon").text(topData[0].AssignedOn)
            $('td:nth-child(1)').hide();
        }
        function DoneProblemTable() {
            $("#hdnAppRej").val("Done");
            TableToArray();
            //alert($("#hdnAppRej").val())
        }
        function RejectProblemTable() {
            $("#hdnAppRej").val("Reject");
            TableToArray();
            //alert($("#hdnAppRej").val())
        }
        var myTableArray = [];
        function TableToArray() {
            myTableArray = [];
            $("#problemsTable tr").each(function () {
                var arrayOfThisRow = [];
                var tableData = $(this).find('td');
                if (tableData.length > 0) {
                    tableData.each(function () {
                        var cell = $(this).closest('td');
                        var cellIndex = cell[0].cellIndex
                        if (cellIndex==3) {
                            arrayOfThisRow.push($(this).find("option:selected").val());
                        }
                        else {
                            arrayOfThisRow.push($(this).text());
                        }
                    });
                    myTableArray.push(arrayOfThisRow);
                    //alert(myTableArray.toString())
                    
                }
            });
            DoneRejectSubmit();
        }
        function DoneRejectSubmit() {
            //var actionBox = $(".actionBox").val();
            //if (actionBox == '') {
            //    alert('required')
            //    return;
            //}
            var validate = true;
            var AppRej = $("#hdnAppRej").val();

            var selection = document.querySelectorAll('.actionBox');
            if (AppRej == 'Done') {
                selection.forEach(function (item) {
                    if (item.value == '') {
                        alert('Service Action cannot be blank.');
                        validate = false;
                    }

                });
            }
            
            if (validate == true) {
                //var AppRej = $("#hdnAppRej").val();
                var url_string = window.location.href;
                var url = new URL(url_string);
                var RecChallan_id = url.searchParams.get("id");
                //rev Pratik
                var user_id = url.searchParams.get("user_id");
                //End of rev Pratik
                //var strArr = myTableArray.toString();
                //alert(myTableArray)
                //console.log(myTableArray)
                //console.log("A",JSON.stringify({ AppRej: AppRej, ReceiptChallan_ID: RecChallan_id, myTableArray: myTableArray }))
                //var mAr = [{ a: "29", b: "POWER OFF / DEAD", c: "FCBBSCGKB", d: "1" }, { a: "29", b: "POWER OFF / DEAD", c: "FCBBSCGKB", d: "1" }]
                $.ajax({
                    type: "POST",
                    url: "TechnicianAssign.aspx/JobStatusUpdate",
                    data: JSON.stringify({ AppRej: AppRej, ReceiptChallan_ID: RecChallan_id, myTableArray: myTableArray, user_id: user_id }),
                    //data: JSON.stringify({ AppRej: AppRej, ReceiptChallan_ID: RecChallan_id }),
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        //console.log(response);
                        //alert(response.d)
                        //alert(response)
                        if (response.d == "Success") {
                            //jAlert("Saved Successfully.", "Alert", function () {
                            //    //window.location.href = "serviceDataList.aspx";

                            //});
                            if (AppRej=='Reject')
                                alert("Rejected Successfully.");
                            else
                                alert("Done Successfully.");

                            var ispopup = url.searchParams.get("UniqueKey");
                            if (!ispopup) { window.location.href = "serviceDataList.aspx"; }

                            HideDoneRej();
                            //window.location.reload();
                        }
                        else {
                            //alert(response.d);
                            alert("Done/Reject Failed.");
                            return
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }
            
        }
        $(document).ready(function () {
            //var url_string = window.location.href;
            //var url = new URL(url_string);
            //var id = url.searchParams.get("id");
            //var AU = url.searchParams.get("AU");
            //var dbname = url.searchParams.get("UniqueKey");
            //console.log(AU);
            //var obj = {
            //    ReceiptChallan_ID: id,
            //    technician_Id: AU,
            //    dbname: dbname
            //}
            //$.ajax({
            //    type: "POST",
            //    url: "TechnicianAssign.aspx/GetResponseStatus",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: JSON.stringify(obj),
            //    //async: false,
            //    success: function (data) {
            //        //console.log('GetData', data)
            //        //alert(data.d)
            //        //var topData = data.d;
                    
            //        if (data.d != "Pending") {
            //            alert(data.d)
            //            $("#clltoActions").hide();
            //        }
            //    }

            //});
        });
        function HideDoneRej() {
            var url_string = window.location.href;
            var url = new URL(url_string);
            var id = url.searchParams.get("id");
            var AU = url.searchParams.get("AU");
            var dbname = url.searchParams.get("UniqueKey");
            console.log(AU);
            var obj = {
                ReceiptChallan_ID: id,
                technician_Id: AU,
                dbname: dbname
            }
            $.ajax({
                type: "POST",
                url: "TechnicianAssign.aspx/GetResponseStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                //async: false,
                success: function (data) {
                    //console.log('GetData', data)
                    //alert(data.d)
                    //var topData = data.d;

                    if (data.d != "Pending") {
                        //alert(data.d)
                        $("#AddRejButtons").hide();
                        $("btnDone").attr("disabled", true);
                        $("btnReject").attr("disabled", true);
                    }
                    else {
                        $("btnDone").removeAttr("disabled");
                        $("btnReject").removeAttr("disabled");
                    }
                }

            });
        }
    </script>
</head>
<body>
    <%--<form id="form1" runat="server">--%>
    <div>
        <div class="box">
            <p><span class="hdtec">Challan No : </span> <span id="challanNo"></span></p>
            <p><span class="hdtec">Assigned on : </span> <span id="Assignedon"></span></p>
            <input type="hidden" id="hdnAppRej" />
        </div>
        <div class="tableContent">
            <table class="table-striped" cellspacing="0" cellspadding="0">
                <thead >
                    <tr>
                        <th>Problems Reported </th>
                        <th>Serial No</th>
                        <th>Service Action</th>
                    </tr>
                </thead>
                <tbody id="problemsTable">
                    <!-- Dynamic table data -->
                </tbody>
            </table>
        </div>
        <div class="clltoActions" id="AddRejButtons">
            <button id="btnDone" class="btn done" onclick="DoneProblemTable()">Done</button>
            <button id="btnReject" class="btn reject" onclick="RejectProblemTable()">Reject</button>
        </div>
    </div>
    <%--</form>--%>
</body>
</html>
