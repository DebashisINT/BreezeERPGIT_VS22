<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryServiceDetails.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.Delivery.DeliveryServiceDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js" integrity="sha512-894YE6QWD5I59HgZOGReFYm4dnWc1Qt5NtvYSaNcOP+u1T9qYdvdihz0PPSiiqn/+/3e7Jo4EaG7TubfWGUrMQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdn.rawgit.com/davidshimjs/qrcodejs/gh-pages/qrcode.min.js" ></script>
    <style>
        body{
            margin-top:20px;
            color: #484b51;
            font-size:0.75rem !important
        }
        .invoiceHeader {
            font-weight:700
        }
        .text-secondary-d1 {
            color: #728299!important;
        }
        .page-header {
            margin: 0 0 1rem;
            padding-bottom: 1rem;
            padding-top: .5rem;
            border-bottom: 1px dotted #e2e2e2;
            display: -ms-flexbox;
            display: flex;
            -ms-flex-pack: justify;
            justify-content: space-between;
            -ms-flex-align: center;
            align-items: center;
        }
        .page-title {
            padding: 0;
            margin: 0;
            font-size: 1.75rem;
            font-weight: 300;
        }
        .brc-default-l1 {
            border-color: #dce9f0!important;
        }

        .ml-n1, .mx-n1 {
            margin-left: -.25rem!important;
        }
        .mr-n1, .mx-n1 {
            margin-right: -.25rem!important;
        }
        .mb-4, .my-4 {
            margin-bottom: 1.5rem!important;
        }

        hr {
            margin-top: 1rem;
            margin-bottom: 1rem;
            border: 0;
            border-top: 1px solid rgba(0,0,0,.1);
        }

        .text-grey-m2 {
            color: #888a8d!important;
        }

        .text-success-m2 {
            color: #86bd68!important;
        }

        .font-bolder, .text-600 {
            font-weight: 600!important;
        }

        .text-110 {
            font-size: 110%!important;
        }
        .text-blue {
            color: #478fcc!important;
        }
        .pb-25, .py-25 {
            padding-bottom: .75rem!important;
        }

        .pt-25, .py-25 {
            padding-top: .75rem!important;
        }
        .bgc-default-tp1 {
            background-color: rgba(121,169,197,.92)!important;
        }
        .bgc-default-l4, .bgc-h-default-l4:hover {
            background-color: #f3f8fa!important;
        }
        .page-header .page-tools {
            -ms-flex-item-align: end;
            align-self: flex-end;
        }

        .btn-light {
            color: #757984;
            background-color: #f5f6f9;
            border-color: #dddfe4;
        }
        .w-2 {
            width: 1rem;
        }

        .text-120 {
            font-size: 120%!important;
        }
        .text-primary-m1 {
            color: #4087d4!important;
        }

        .text-danger-m1 {
            color: #dd4949!important;
        }
        .text-blue-m2 {
            color: #68a3d5!important;
        }
        .text-150 {
            font-size: 150%!important;
        }
        .text-60 {
            font-size: 60%!important;
        }
        .text-grey-m1 {
            color: #7b7d81!important;
        }
        .align-bottom {
            vertical-align: bottom!important;
        }
        .fn100 tr>th {
            font-size:100%;
            white-space: nowrap;
            padding:5px 4px
        }
        .fn100 tr>td {
            padding:5px 4px
        }
    </style>

    <script>
        function addScript(url) {
            var script = document.createElement('script');
            script.type = 'application/javascript';
            script.src = url;
            document.head.appendChild(script);
        }
        addScript('https://raw.githack.com/eKoopmans/html2pdf/master/dist/html2pdf.bundle.js');
        function downloadPDF() {
            var element = document.getElementById('pdf');
            var name = $("#ch").val() +".pdf"
            var opt = {
                margin:0.3,
                filename: name,
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: { scale: 2 },
                jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
            };

            // New Promise-based usage:
            html2pdf().set(opt).from(element).save();
        }
        var qrObj = {}
        $(function () {

            GetTop()
        })
        var url_string = window.location.href;
        var url = new URL(url_string);
       
        var obj = {
            FINYEAR: url.searchParams.get("FINYEAR"),
            COMPANYID: url.searchParams.get("COMPANYID"),
            FULLPATH: "",
            RCID: url.searchParams.get("RCID"),
            ISCREATEORPREVIEW: url.searchParams.get("ISCREATEORPREVIEW")
        }
        
        function GetTop() {
            
            //var id = url.searchParams.get("id");
            //var AU = url.searchParams.get("AU");
            //var dbname = url.searchParams.get("UniqueKey");
            
            $.ajax({
                type: "POST",
                url: "DeliveryServiceDetails.aspx/GetTopData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                //async: false,
                success: function (data) {
                    console.log('GetTopData', data)
                    var res = data.d[0];
                    qrObj.Companyname = res.upper_CompanyName;
                    $("#upper_CompanyName").text(res.upper_CompanyName)
                    $("#Address").text(res.Address)
                    ChallanDetailsData() 
                }
            })
        }
        function ChallanDetailsData() {
            var url_string = window.location.href;
            var url = new URL(url_string);
            //var id = url.searchParams.get("id");
            //var AU = url.searchParams.get("AU");
            //var dbname = url.searchParams.get("UniqueKey");
            $.ajax({
                type: "POST",
                url: "DeliveryServiceDetails.aspx/ChallanDetailsData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                //async: false,
                success: function (data) {
                    console.log("ChallanDetailsData", data)
                    var res = data.d[0]
                    $("#LCO_Name").text(res.LCO_Name)
                    $("#EntityCode").text(res.EntityCode)
                    $("#DeliveredTo").text(res.DeliveredTo)
                    $("#DeliveredOn").text(res.DeliveredOn)
                    $("#Receipt_Challan_No").text(res.Receipt_Challan_No)
                    $("#Receipt_Challan_NoTop").text(res.Receipt_Challan_No)
                    qrObj.LCOName = res.LCO_Name;
                    qrObj.DeliveredOn = res.DeliveredOn;
                    GetTableData();
                    $("#ch").val(res.Receipt_Challan_No)
                    $("#imageLogo img").attr("src", res.cmp_bigLogo)
                }
            })
        }
        function GetTableData() {
            var url_string = window.location.href;
            var url = new URL(url_string);
            //var id = url.searchParams.get("id");
            //var AU = url.searchParams.get("AU");
            //var dbname = url.searchParams.get("UniqueKey");
            $.ajax({
                type: "POST",
                url: "DeliveryServiceDetails.aspx/GetTableData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(obj),
                //async: false,
                success: function (data) {
                    var data = data.d;
                    var tableRow = "";
                    qrObj.Model = [];
                    //qrObj.DeviceNumber = res.DeviceNumber;
                    for (i = 0; i < data.length; i++) {
                        tableRow += "<tr><td>" + data[i].Model + "</td>"
                        tableRow += "<td>" + data[i].DeviceNumber + "</td>"
                        tableRow += "<td>" + data[i].Service_Action + "</td>"
                        tableRow += "<td>" + data[i].Reason + "</td>"
                        tableRow += "<td>" + data[i].Billable + "</td>"
                        tableRow += "<td>" + data[i].NewSerialNo + "</td>"
                        tableRow += "<td>" + data[i].Warranty_Upto + "</td></tr>"
                        qrObj.Model.push(data[i].Model)
                    }
                    
                    $("#tableInfo").html(tableRow)
                    genQr(qrObj)
                }
            })
        }
        

    </script>
    <script type="text/javascript">
        $(function () {
           
        })
        function genQr(obj) {
            
            var text = JSON.stringify(obj)
            console.log("genQr", text)
            var qrcode = new QRCode("qrcode", {
                text: text,
                width: 150,
                height: 150,
                colorDark: "#000000",
                colorLight: "#ffffff",
                correctLevel: QRCode.CorrectLevel.H
            });
        }
        let myPromise = new Promise(function (myResolve, myReject) {
            let x = 0;
            if (x == 0) {
                myResolve("OK");
            } else {
                myReject("Error");
            }
        });
        myPromise.then(
          function (value) { console.log(value); },
          function (error) { console.log(error); }
        );
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="ch" />
        <div >
        <div class="page-content container">
            <div class="page-header text-blue-d2">
                <h1 class="page-title text-secondary-d1">
                    Challan 
                    <small class="page-info">
                        <i class="fa fa-angle-double-right text-80"></i>
                        No: #<span id="Receipt_Challan_NoTop"></span>
                    </small>
                </h1>

                <div class="page-tools">
                    <div class="action-buttons">
                        <a class="btn bg-white btn-light mx-1px text-95 d-print-none" href="javascript:window.print()" data-title="Print">
                            <i class="mr-1 fa fa-print text-primary-m1 text-120 w-2"></i>
                            Print
                        </a>
                        <a class="btn bg-white btn-light mx-1px text-95 d-print-none" href="#" onclick="downloadPDF()" data-title="PDF">
                            <i class="mr-1 fa fa-file-pdf-o text-danger-m1 text-120 w-2"></i>
                            Export
                        </a>
                    </div>
                </div>
            </div>

            
        </div>
    </div>
        <div id="pdf">
            <div class="container px-0">
                <div class=" mt-4">
                    <div class="col-12 col-lg-12">
                        <div class="">
                            <table class="table">
                                <tr>
                                    <td width="150px"><div id="imageLogo"><img src="" /></div></td>
                                    <td class="text-center">
                                        <h4 class="invoiceHeader" id="upper_CompanyName"></h4>
                                        <p id="Address"></p>
                                        <h5><u>DELIVERY CHALLAN</u></h5>
                                    </td>
                                    <td width="150px"><div id="qrcode"></div></td>
                                </tr>
                            </table>
                        </div>
                        <!-- .row -->
                        <hr class="row brc-default-l1 mx-n1 mb-4" />

                        <div class="row">
                            <div class="col-sm-6">
                                <div>
                                    <span class="text-sm text-grey-m2 align-middle">LCO Name: </span>
                                    <span class="text-600 text-110 text-blue align-middle" id="LCO_Name"></span>
                                </div>
                                <div class="text-grey-m2">
                                    <div class="my-1">
                                        Entity Code: <strong><span id="EntityCode"></span></strong>
                                    </div>
                                    <div class="my-1">
                                        Delivered To: <strong><span id="DeliveredTo"></span></strong>
                                    </div>
                                  
                                </div>
                            </div>
                            <!-- /.col -->
                            
                            <div class="text-95 col-sm-6 align-self-start d-sm-flex justify-content-end">
                                <hr class="d-sm-none" />
                                <div class="text-grey-m2">
                                    <div class="my-2"> <span class="text-600 text-90">Challan No :</span> <strong><span id="Receipt_Challan_No"></span></strong></div>
                                    <div class="my-2"> <span class="text-600 text-90">Delivered On:</span> <strong><span id="DeliveredOn"></span></strong></div>                                   
                                </div>
                            </div>
                            <!-- /.col -->
                        </div>

                   <div class="mt-4">
                        <div class="row border-b-2 brc-default-l2"></div>
                            <!-- or use a table instead -->
                            <div style="min-height:500px">      
                                <div class="table-responsive">
                                    <table class="table table-striped table-borderless border-0 border-b-2 brc-default-l1 fn100">
                                        <thead class="bg-none bgc-default-tp1">
                                            <tr class="text-white">
                                                <th class="opacity-2">STB Model </th>
                                                <th>S/N</th>
                                                <th>Service Action</th>
                                                <th>Unbillable Reason</th>
                                                <th width="140">Billable</th>
                                                <th width="140">New Serial No</th>
                                                <th width="140">Warrenty Upto</th>
                                            </tr>
                                        </thead>

                                        <tbody class="text-95 text-secondary-d3" id="tableInfo">
                                             
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <hr />
                            <div style="padding-top:0px; padding-bottom:5px">
                                <span class="text-secondary-d1 text-105">Received By </span>
                                <span class="float-right">Operator's Signature</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
