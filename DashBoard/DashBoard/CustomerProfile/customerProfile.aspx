<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customerProfile.aspx.cs" Inherits="DashBoard.DashBoard.CustomerProfile.customerProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../assests/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />

     
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../Js/datatable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Js/datatable/jquery.dataTables.min.js"></script>
    <style>
        .srvTbl  >thead th{
            background:#333399;
            color:#fff
        }
        .mkFade {
            opacity:0;
            -webkit-transition:all 0.2s ease-out;
            -moz-transition:all 0.2s ease-out;
            transition:all 0.2s ease-out;
        }
        #ServiceStatusDetails .modal-body {
            overflow:hidden
        }
        #SrvTable>thead>tr>th {
            white-space:nowrap;
        }
        .modal-backdrop {
            z-index:auto;
        }
        .mainProfile hr{
            margin-top: 5px;
            margin-bottom: 8px;
        }
        .mt-3{
            margin-top:15px
        }
        .main-body {
            padding: 15px;
        }
        .card {
            box-shadow: 0 1px 3px 0 rgba(0,0,0,.1), 0 1px 2px 0 rgba(0,0,0,.06);
        }

        .card {
            position: relative;
            display: flex;
            flex-direction: column;
            min-width: 0;
            word-wrap: break-word;
            background-color: #fff;
            background-clip: border-box;
            border: 0 solid rgba(0,0,0,.125);
            border-radius: .25rem;
        }

        .card-body {
            flex: 1 1 auto;
            min-height: 1px;
            padding: 1rem;
        }

        .gutters-sm {
            margin-right: -8px;
            margin-left: -8px;
        }

        .gutters-sm>.col, .gutters-sm>[class*=col-] {
            padding-right: 8px;
            padding-left: 8px;
        }
        .mb-3, .my-3 {
            margin-bottom: 1rem!important;
        }

        .bg-gray-300 {
            background-color: #e2e8f0;
        }
        .h-100 {
            height: 100%!important;
        }
        .shadow-none {
            box-shadow: none!important;
        }
    </style>
    <link href="../Js/bootstratSelect/css/bootstrap-select.min.css" rel="stylesheet" />
    <script src="../Js/bootstratSelect/js/bootstrap-select.min.js"></script>
 
    <script>
        var selcted = '';
        var srvData = [];
        function getSearch() {
            $.ajax({
                type: "POST",
                url: "customerProfile.aspx/GetCustomerProfileSearch",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var mainDta = data.d;
                   // console.log("GetCustomerProfileSearch", data);
                    // customer name
                    var html = "<option value=''>Select</option>";
                    for (var i = 0; i < mainDta.length; i++) {
                        html += "<option value='" + mainDta[i].cnt_internalId + "'>" + mainDta[i].Name + "</option>" 
                    }
                    $("#Custname").html(html);
                    $("#Custname").selectpicker('refresh');

                    // cust phone
                    var htmlPhone = "<option value=''>Select</option>";
                    for (var a = 0; a < mainDta.length; a++) {
                        if (mainDta[a].Phone == "") {} else {
                            htmlPhone += "<option value='" + mainDta[a].cnt_internalId + "'>" + mainDta[a].Phone + "</option>"
                        }
                    }
                    $("#CustPhone").html(htmlPhone);
                    $("#CustPhone").selectpicker('refresh');
                }
            });
        }

        function getService() {
            $.ajax({
                type: "POST",
                url: "customerProfile.aspx/GetCustomerService",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ "cntID": selcted }),
                success: function (data) {
                    console.log("sr", data)
                    var dt = data.d;

                    var table = "<thead><tr><th>Customer Id</th><th>Customer Name</th><th>Assigned Branch</th><th>Assigned On</th><th>Technician</th><th>Tech Assigned On</th><th>Product Name</th><th>Description</th><th>Code</th><th>Quantity</th><th>Schedule Date</th><th>SCH Code</th><th>Sch Status</th><th>Segment 1</th><th>Segment 2</th><th>Service</th><th>Status</th></tr></thead><tbody>"
                    for (var i = 0; i < dt.length; i++) {
                        table += "<tr>"
                        table += "<td>" + dt[i].CUSTOMER_ID + "</td>"
                        table += "<td>" + dt[i].CUSTOMER + "</td>"
                        table += "<td>" + dt[i].ASSIGNEDBRANCH + "</td>"
                        table += "<td>" + dt[i].BRANCH_ASSIGNED_ON + "</td>"
                        table += "<td>" + dt[i].ASSIGNEDTECHNICIAN + "</td>"
                        table += "<td>" + dt[i].TECHNICIAN_ASSIGNED_ON + "</td>"
                        table += "<td>" + dt[i].sProducts_Name + "</td>"
                        table += "<td>" + dt[i].sProducts_Description + "</td>"
                        table += "<td>" + dt[i].sProducts_Code + "</td>"
                        table += "<td>" + dt[i].QUANTITY + "</td>"
                        table += "<td>" + dt[i].SCHEDULE_DATE + "</td>"
                        table += "<td>" + dt[i].SCH_CODE + "</td>"
                        table += "<td>" + dt[i].SCH_STATUS + "</td>"
                        table += "<td>" + dt[i].SEGMENT1 + "</td>"
                        table += "<td>" + dt[i].SEGMENT2 + "</td>"
                        table += "<td>" + dt[i].SERVICE + "</td>"
                        table += "<td>" + dt[i].STATUS + "</td>"

                        table += "</tr>"
                    }
                    $("#SrvTable").html(table);
                }
            });
        }
        getSearch();
        $(document).ready(function () {
            $("#Custname").on("change", function () {
                selcted = $(this).val();
                $('#CustPhone').selectpicker('val', '');
            });
            $("#CustPhone").on("change", function () {
                selcted = $(this).val();
                $('#Custname').selectpicker('val', '');
            });

            
            $('#ServiceStatusDetails').on('shown.bs.modal', function () {
                $('#SrvTable').DataTable({
                    "scrollX": true,
                    "scrollY": "300px",
                    "paging": false, 
                });
                $(".mkFade").css({"opacity": "1"})
                //tableModal = $("#example").DataTable({
                //        //ajax:{
                //        //    url:[my_url],
                //        //    dataSrc:function(data){
                //        //        //Do something maybe with the data...
                //        //    }
                //        //},
                //        //"initComplete":function(settings,json){
                //        //    //When table is initialized...
                //        //},
                //        //columns:[
                //        //    //columns data mapping
                //        //],
                //        "scrollX":true
                //    });
            });
        });
        function getProfile() {
            console.log(selcted)
            if (selcted == "") {
                return;
                alert("Please select Customer or Phone")
            } else {
                $.ajax({
                    type: "POST",
                    url: "customerProfile.aspx/GetCustomer",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ "cntID": selcted }),
                    success: function (data) {
                        var Dta = data.d;
                        console.log("GetCustomer", Dta);
                        if (Dta.length > 0) {
                            $("#LeadName").text(Dta[0].Lead_Name)
                            $("#CustomerId").text(Dta[0].cnt_internalId)
                            $("#CustomerEmail").text(Dta[0].Email)
                            $("#CustomerPhone").text(Dta[0].Mobile_no)
                            $("#CustomerRaing").text(Dta[0].RATING)
                            $("#ContactPerson").text(Dta[0].contactperson_Name)
                            $("#ContactPersonEmail").text(Dta[0].contactperson_Email)
                            $("#ContactPersonPhone").text(Dta[0].contactperson_Phone)
                            $("#CustomerSince").text(Dta[0].Customer_Since)
                            $("#Clientcategory").text(Dta[0].Client_Category)
                            $("#Contractdate").text(Dta[0].Contract_date)
                            $("#TypeofService").text(Dta[0].Type_Of_Service)
                            $("#Frequency").text(Dta[0].Frequency)
                            $("#Value").text(Dta[0].Value).css({ "font-weight": "bold" });
                            $("#ExistingWarrenty").text(Dta[0].Existing_Warrenty)
                            $("#NumberofBillsMonthly").text(Dta[0].No_Of_Bills_Monthly)
                            $("#NoofServicePoints").text(Dta[0].No_Of_Service_Pointy)
                            $("#NextServiceSchedule").text(Dta[0].Next_Servcie_Schedule)
                            $("#Salesperson").text(Dta[0].Sales_Person)
                            $("#CustomerCoordinator").text(Dta[0].Custom_Cordinator)
                            $("#MainServiceBranch").text(Dta[0].Main_Service_Brannch)
                            $("#CollectionCoordinator").text(Dta[0].Collection_Cordinator)
                            $("#InfoonOutgoingworklikeTerminate").text(Dta[0].Info_On_Outgoing_Work_Like_Termite)
                            $("#ServiceCompletedfortheMonth").text(Dta[0].Service_Completed_For_tHe_Month);

                            getService();
                        }
                        
                        
                    }
                });
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">

<div class="clearfix mainProfile">
    <div class="main-body">

        <div class="card" style="margin-bottom: 15px;">
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3">
                        <label>Select Customer</label>
                        <div>
                            <select class="selectpicker form-control" data-live-search="true" id="Custname">
                            </select>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Select Phone</label>
                        <div>
                            <select class="selectpicker form-control" data-live-search="true" id="CustPhone">
                            </select>
                        </div>
                    </div>
                    <div class="col-md-3 hide">
                        <label>Select Email</label>
                        <div>
                            <select class="selectpicker" data-live-search="true" id="CustEmail">
                            </select>
                        </div>
                    </div>
                    <div class="col-md-3" style="padding-top:23px">
                        <button type="button" class="btn btn-success" onclick="getProfile()">Search</button>
                    </div>
                </div>
            </div>
        </div>

          <div class="row gutters-sm">
            <div class="col-md-4 mb-3">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex flex-column align-items-center text-center">
                    <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="Admin" class="rounded-circle" width="150" />
                    <div class="mt-3">
                      <h4 id="LeadName">Customer Name</h4>
                      <p class="text-secondary mb-1" id="CustomerId">Customer Id</p>
                      <p class="text-muted font-size-sm" id="CustomerEmail"> ---- -- --- --</p>
                      <p class="text-muted font-size-sm" id="CustomerPhone">--- ---</p>
                      <p class="text-muted font-size-sm" >Rating : <span id="CustomerRaing">-- --</span></p>
                      <button class="btn btn-primary" type="button" data-toggle="modal" data-target="#ServiceStatusDetails">Service Status Details</button>
                    </div>
                  </div>
                </div>
              </div>            
            </div>
            <div class="col-md-8">
              <div class="card mb-3">
                <div class="card-body">
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Full Name</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="ContactPerson">
                      
                    </div>
                  </div>
                  <hr>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Email</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="ContactPersonEmail">
                      ---- ---- -----
                    </div>
                  </div>
                  <hr>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Phone</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="ContactPersonPhone">
                      ---- --- --- --
                    </div>
                  </div>
                  <hr>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Customer Since</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="CustomerSince">
                      --- -- -- 
                    </div>
                  </div>
                  <hr/>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Client category</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="Clientcategory">
                       ---- -- --- --
                    </div>
                  </div>
                  <hr/>
                    <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Contract date</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="Contractdate">
                      ----- --- -- 
                    </div>
                  </div>
                  <hr>
                    <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Type of Service</h6>
                    </div>
                    <div class="col-sm-9 text-secondary" id="TypeofService">
                      ----- ----- --- 
                    </div>
                  </div>
                  <hr>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Frequency</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="Frequency">
                      ----- --- --- --
                    </div>
                    <div class="col-sm-3">
                      <h6 class="mb-0">Value</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="Value">
                      ---- --- --- 
                    </div>
                  </div>
                  <hr>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Existing Warrenty</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="ExistingWarrenty">
                       ---- -- --- --
                    </div>
                    <div class="col-sm-3">
                      <h6 class="mb-0">Number of Bills Monthly</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="NumberofBillsMonthly">
                       ---- -- --- --
                    </div>
                  </div>
                  <hr>
                  <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">No of Service Points</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="NoofServicePoints">
                       ---- -- --- --
                    </div>
                    <div class="col-sm-3">
                      <h6 class="mb-0">Next Service Schedule</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="NextServiceSchedule">
                       ---- -- --- --
                    </div>
                  </div>
                  <hr />
                    <div class="row">
                    <div class="col-sm-3">
                      <h6 class="mb-0">Sales person</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="Salesperson">
                       ---- -- --- --
                    </div>
                    <div class="col-sm-3">
                      <h6 class="mb-0">Customer Coordinator</h6>
                    </div>
                    <div class="col-sm-3 text-secondary" id="CustomerCoordinator">
                       ---- -- --- --
                    </div>
                  </div>
                  <hr/>
                </div>
              </div>

              <div class="row gutters-sm">
                <div class="col-sm-6 mb-3">
                  <div class="card h-100">
                    <div class="card-body">
                      <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Main Service Branch</h6>
                        </div>
                        <div class="col-sm-6 text-secondary" id="MainServiceBranch">
                             ---- -- --- --
                        </div>
                        </div>
                        <hr/>
                        <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Collection Coordinator</h6>
                        </div>
                        <div class="col-sm-6 text-secondary" id="CollectionCoordinator">
                            ---- -- --- --
                        </div>
                        </div>
                        <hr/>
                        <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Customer Comunication Details</h6>
                        </div>
                        <div class="col-sm-6 text-secondary">
                            <button class="btn btn-info btn-xs">Click to see Details</button>
                        </div>
                        </div>
                        <hr/>
                        <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Bills and Dockets</h6>
                        </div>
                        <div class="col-sm-6 text-secondary">
                            <button class="btn btn-info btn-xs">Click to see Details</button>
                        </div>
                        </div>
                        <hr/>
                    </div>
                  </div>
                </div>
                <div class="col-sm-6 mb-3">
                  <div class="card h-100">
                    <div class="card-body">
                      <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Info on Outgoing work like Terminate</h6>
                        </div>
                        <div class="col-sm-6 text-secondary" id="InfoonOutgoingworklikeTerminate">
                            ---- -- --- --
                        </div>
                        </div>
                        <hr/>
                        <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Service Completed for the Month</h6>
                        </div>
                        <div class="col-sm-6 text-secondary" id="ServiceCompletedfortheMonth">
                            ---- -- --- --
                        </div>
                        </div>
                        <hr/>
                        <div class="row">
                        <div class="col-sm-6">
                            <h6 class="mb-0">Outstanding and Payment Received Details with Amount and Date</h6>
                        </div>
                        <div class="col-sm-6 text-secondary">
                            <button class="btn btn-info btn-xs">Click to see Details</button>
                        </div>
                        </div>
                    </div>
                  </div>
                </div>
              </div>



            </div>
          </div>

        </div>
    </div>

        <!-- ServiceStatusDetails-->
        <div id="ServiceStatusDetails" class="modal fade" role="dialog">
          <div class="modal-dialog" style="width:90%">
            <!-- Modal content-->
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Service Status Details</h4>
              </div>
              <div class="modal-body">
                  <div class="mkFade">
                    <table id="SrvTable" class="display srvTbl" style="width:100%"></table>
                   </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>






    </form>
</body>
</html>
