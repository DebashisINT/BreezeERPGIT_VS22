<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchTransferOutView.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.BranchRequisitionView.BranchTransferOutView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
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
        .form-control{
            width:100%
        }
        .inp{
            border:1px solid #C1C1C1;
            padding:3px 10px;
            border-radius:4px;
            display:inline-block;
            margin-bottom:5px
        }
    </style>
    <style>
        .table-striped>thead>tr>th {
            white-space:nowrap
        }
    </style>
    <script>
        function convertDate(date) {
            var d = new Date(date)
            console.log(d)
        }
        $(function () {

            var url_string = window.location.href;
            var url = new URL(url_string);
            var id = url.searchParams.get("id");

            //var AU = url.searchParams.get("AU");
            var dbname = url.searchParams.get("UniqueKey");
            $("#indentIdHidden").val(id)
            // console.log(AU);
            var obj = {
                TransferOut_Id: id,
                dbname: dbname
            }
            var obj2 = {
                TransferOut_Id: id
            }
            $.ajax({
                type: "POST",
                url: "BranchTransferOutView.aspx/GetData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                //async: false,
                success: function (data) {
                    var selectedData = data.d;
                    //console.log('GetData', data)
                    var topData = data.d;
                    convertDate(topData[0].Indent_RequisitionDate)
                    $("#DocumentNo").text(topData[0].Stk_TransferNumber);
                    $("#Purpose").text(topData[0].Stk_Purpose);
                    $("#Branch").text(topData[0].Stk_TransferToBranch);
                    $("#Project").text(topData[0].Proj_Name);
                    $("#RequisitionNumber").text(topData[0].Indent_RequisitionNumber);
                    var date = topData[0].Stk_TransferDate;
                    var dateAct = date.split(" ");
                    var MDate = dateAct[0];
                    var datearray = MDate.split("-");
                    var newdate = datearray[0] + '/' + datearray[1] + '/' + datearray[2];
                    $("#PostingDate").text(newdate);

                    $.ajax({
                        type: "POST",
                        url: "BranchTransferOutView.aspx/GetListData",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(obj2),
                        //async: false,
                        success: function (data) {
                            console.log('GetListData', data)
                            var GetListData = data.d;
                            createRows(GetListData)
                        }

                    });
                    //HideDoneRej(topData[0].ApprovalSettings);
                    
                    //AddRejButtons(topData[0].BranchRequisition_ApproveStatus)
                    
                }

            });

        })

        function createRows(data) {
            
            var tableRow;
            for (i = 0; i < data.length; i++) {
                tableRow += "<tr>";
                tableRow += "<td>" + data[i].StkDetails_Id + "</td>"
                tableRow += "<td>" + data[i].ProductDescription + "</td>"
                tableRow += "<td>" + parseInt(data[i].Quantity).toFixed(4) + "</td>"
                tableRow += "<td>" + data[i].UOM + "</td>"
                tableRow += "</tr>"
            }


            $("#problemsTable").html(tableRow);
            $('#problemsTable td:nth-child(1)').hide();
        }


        //function AddRejButtons(data) {
        //    if (data == "Yes") {
        //        $("#AddRejButtons").hide();
        //        $("btnDone").attr("disabled", true);
        //        $("btnReject").attr("disabled", true);
        //    }
        //    else {
        //        $("#AddRejButtons").show();
        //        $("btnDone").attr("disabled", false);
        //        $("btnReject").attr("disabled", false);
        //    }
        //}

        //function DoneProblemTable() {
        //    $("#hdnAppRej").val(1);
        //    TableToArray();
        //}
        //function RejectProblemTable() {
        //    $("#hdnAppRej").val(2);
        //    TableToArray();
        //}
        //var myTableArray = [];
        //function TableToArray() {
        //    myTableArray = [];
        //    $("#problemsTable tr").each(function () {
        //        var arrayOfThisRow = [];
        //        var tableData = $(this).find('td');
        //        if (tableData.length > 0) {
        //            tableData.each(function () {
        //                var cell = $(this).closest('td');
        //                var cellIndex = cell[0].cellIndex
        //                if (cellIndex == 2) {
        //                    arrayOfThisRow.push($(this).find("input").val());
        //                }
        //                else {
        //                    arrayOfThisRow.push($(this).text());
        //                }
        //            });
        //            myTableArray.push(arrayOfThisRow);
        //            console.log("myTableArray", myTableArray)

        //        }
        //    });
        //    DoneRejectSubmit();
        //}
        //function DoneRejectSubmit() {
        //    var validate = true;
        //    var AppRej = $("#hdnAppRej").val();

        //    if (validate == true) {
        //        var url_string = window.location.href;
        //        var url = new URL(url_string);
        //        var Indent_Id = url.searchParams.get("id");

        //        $.ajax({
        //            type: "POST",
        //            url: "BranchRequisitionView.aspx/JobStatusUpdate",
        //            data: JSON.stringify({ AppRej: AppRej, udtIndentDetailsAction: myTableArray, Indent_Id: Indent_Id }),
        //            async: false,
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            success: function (response) {
        //                if (response.d == "Success") {
        //                    if (AppRej == 'Reject')
        //                        alert("Rejected Successfully.");
        //                    else
        //                        alert("Done Successfully.");

        //                    AddRejButtons("Yes");

        //                }
        //                else {

        //                    alert("Done/Reject Failed.");
        //                    return
        //                }
        //            },
        //            error: function (response) {
        //                console.log(response);
        //            }
        //        });
        //    }

        //}
       
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="indentIdHidden" />
        <%--Mantis Issue 25237--%>
        <input type="hidden" id="hdnAppRej" />
        <%--End of Mantis Issue 25237--%>
        <div>
            <div class="box">
                <div>
                    <span class="hdtec">Document No : </span> 
                    <span id="DocumentNo" class="inp"></span>
                </div>
                <div>
                    <span class="hdtec">Posting Date: </span> 
                    <span id="PostingDate" class="inp"></span>
                </div>
                <div>
                    <span class="hdtec">Branch Req # : </span> 
                    <span id="RequisitionNumber" class="inp"></span>
                </div>
                <div>
                    <span class="hdtec">Purpose : </span> 
                    <span id="Purpose" class="inp"></span>
                </div>
                <div>
                    <span class="hdtec">To Branch : </span> 
                    <span id="Branch" class="inp"></span>
                </div>
                <div>
                    <span class="hdtec">Project: </span> 
                    <span id="Project" class="inp"></span>
                </div>
                
            </div>
            <div class="tableContent">
                <table class="table-striped" cellspacing="0" cellspadding="0">
                    <thead >
                        <tr>
                            <th>Product Name</th>
                            <th>Quantity</th>
                            <th>UOM</th>
                        </tr>
                    </thead>
                    <tbody id="problemsTable">
                        <!-- Dynamic table data -->
                       
                        
                    </tbody>
                </table>
            </div>
            
           <%-- <div class="clltoActions" id="AddRejButtons">
                <button type="button" id="btnDone" class="btn done" onclick="DoneProblemTable()">Approve</button>
                <button type="button" id="btnReject" class="btn reject" onclick="RejectProblemTable()">Reject</button>
            </div>--%>
           
        </div>
    </form>
</body>
</html>
