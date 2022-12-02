<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesDueAnalysis.aspx.cs" Inherits="DashBoard.DashBoard.DueAnalysis.SalesDueAnalysis" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,200;0,500;0,600;0,700;0,800;0,900;1,400&display=swap" rel="stylesheet" />
    <script src="../Js/jquery3.3.1.min.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="../css/projectDB.css" rel="stylesheet" />
    <link href="../Js/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../Js/datePicker/bootstrap-datepicker.js"></script>
    <link href="../css/jquery.alerts.css" rel="stylesheet" />
    <script src="../Js/jquery.alerts.js"></script>
    <script src="../Js/tether.min.js"></script>
    <!-- DevExtreme themes -->
    <link rel="stylesheet" href="https://cdn3.devexpress.com/jslib/20.1.6/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/20.1.6/css/dx.light.compact.css" />
 
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/20.1.6/js/dx.all.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/babel-polyfill/7.4.0/polyfill.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/3.3.1/exceljs.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.8/FileSaver.min.js"></script>
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>
    <style>
        .tabluLer>li.rd.active .ic {
            background: #F65447;
            border: 1px solid #F65447;
            color: #fff;
            box-shadow: 0px 3px 2px rgba(0,0,0,0.19);
        }
        .tabluLer>li.grn.active .ic {
            background: #3fc776;
            border: 1px solid #4dde87;
            color: #fff;
            box-shadow: 0px 3px 2px rgba(0,0,0,0.19);
        }
        .tabluLer>li.bl.active .ic {
            background: #3783f5;
            border: 1px solid #3783f5;
            color: #fff;
            box-shadow: 0px 3px 2px rgba(0,0,0,0.19);
        }
        .relativeLoader {
        position:relative;
    }
    .relativeLoader #loaderP, .loaderPC{
        background: rgba(255,255,255,0.95);
        position: absolute;
        top: 0;
        bottom: 0;
        width: 100%;
        z-index: 9999;
        left: 0;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        
    }
         .gminorheading>div{
            display:inline-block;
        }
        .table-responsive {
            width: 100%;
            margin-bottom: 15px;
            overflow-x: scroll;
            overflow-y: hidden;
            border: 1px solid #ddd;
        }
        .table-responsive>.table>thead>tr>th, .table-responsive>.table>tbody>tr>th, .table-responsive>.table>tfoot>tr>th, .table-responsive>.table>thead>tr>td, .table-responsive>.table>tbody>tr>td, .table-responsive>.table>tfoot>tr>td {
            white-space: nowrap;
        }
        #rightGap>thead>tr>th {
            padding-right:25px
        }
        #showTbleInfoTimeline > tr {
            cursor:pointer;
        }
        td.gmajorheading div{
            display:inline-block
        }
        tr.rActive{
            background:#dae2ff;
        }
        .mainAreachart{
            padding:15px 0
        }
        .flexViewCenter{
            display: flex;
            justify-content: center;
            align-items: center;
        }
         .calltoActionicons a {
            font-size: 13px;
            color: #ffffff;
            background: #42c08b;
            display: inline-block;
            width: 25px;
            height: 25px;
            text-align: center;
            border-radius: 50%;
            line-height: 25px;
        }
        .calltoActionicons a.ful {
            color: #ffffff;
            background: #d89985;
        }
        .full-screen{
            position:fixed;
            top:0;
            left:0;
            width:100%;
            height:100%;
            z-index:55
        }
        #gridContainer {
          height: 300px;
        }
        .dexCntrl input, .dexCntrl input:focus, .dx-texteditor-input, .dx-texteditor-input:focus{
            border: none !important;
            border-radius: 6px !important;
        }
        .ic>img {
            max-width: 53%;
        }
        .ic>img.white {
            display:none;
        }
        .tabluLer>li.active .normal{
            display: none;
        }
        .tabluLer>li.active .white{
            display: inline-block;
        }
        .dx-header-row{
            background: #545dc1 !important;
            color: #fff;
        }
        .dx-header-row>td{
            border-color: #545dc1 !important;
        }
        
    </style>
<script>
        $(function () {
            $('#loaderP, .loaderPC').hide();
            $.fn.datepicker.defaults.format = "dd-mm-yyyy";
            $(".datepicker").datepicker({
                //format: 'yyyy-mm-dd',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());
            $('body').on('click', '.toggleFullScreen', function (e) {
                $(this).parent(".box-full").toggleClass("full-screen");
            });
            $("#calendarContainer").dxDateBox({
                value: new Date(),
                width: "100%"
            }).dxDateBox("instance");
        });
        
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
        $(function () {
            $('#gridContainer').dxDataGrid({
                dataSource: employees,
                showBorders: true,
                searchPanel: { visible: true },
                selection: {
                    mode: 'multiple'
                },
                rowAlternationEnabled: true,
                columnHidingEnabled: true,
                selection: {
                    mode: "single"
                },
                paging: {
                    pageSize: 8
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                groupPanel: {
                    visible: true
                },
                export: {
                    enabled: true,
                    allowExportSelectedData: true
                },
                onExporting: function (e) {
                    var workbook = new ExcelJS.Workbook();
                    var worksheet = workbook.addWorksheet('Employees');

                    DevExpress.excelExporter.exportDataGrid({
                        component: e.component,
                        worksheet: worksheet,
                        autoFilterEnabled: true
                    }).then(function () {
                        // https://github.com/exceljs/exceljs#writing-xlsx
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Employees.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                columns: [
                  
                  {
                      dataField: 'Slaesman'
                  }, {
                      dataField: 'Customer'
                  }, {
                      dataField: 'Invoice'
                  }, {
                      dataField: 'Due_amount',
                      dataType: 'number',
                      alignment: "right"
                  }, {
                      dataField: 'Date',
                      dataType: 'date'
                      
                  }
                ]
            });

            var selectedDate = $("#selected-date").dxDateBox({
                value: new Date(),
                width: "100%",
                displayFormat: "dd-MM-yyyy",
                onValueChanged: function (data) {
                    
                }
            }).dxDateBox("instance");
            $("#label-branch").dxSelectBox({
                items: ["Select", "Barnch Name"],
                value: "top",
                onValueChanged: function (data) {
                    
                }
            });
            $("#label-salesman").dxSelectBox({
                items: ["Select", "Salesman Name"],
                value: "top",
                onValueChanged: function (data) {
                   
                }
            });
            $("#label-customer").dxSelectBox({
                items: ["Select", "Customer Name"],
                value: "top",
                onValueChanged: function (data) {
                    
                }
            });
        });

        var employees = [{
            "ID": 1,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 2,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 3,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 4,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 5,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 6,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 7,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 8,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 9,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }, {
            "ID": 10,
            "Slaesman": "John",
            "Customer": "Heart",
            "Invoice": "1524523",
            "Due_amount": 156245,
            "Date": "12-07-2020"
        }];
    </script>
    <style>
        .panelClass {
            margin-bottom: 31px;
            top: 50px;
            display: none;
            opacity: 0;
        }
        .widget {
            text-align: center;
            color: #fff;
            display:flex;
            border-radius:3px;
            overflow: hidden;
            font-family: 'Poppins', sans-serif;
            cursor:pointer;
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        .widget.DisableClass{
            border-bottom: 4px dashed #f4f7f5;
        }
        .widget .iconBox {
            width: 80px;
            background: rgba(0,0,0,0.5);
            display: flex;
            justify-content: center;
            align-items: center;
            font-size: 28px;
        }
        .widget .textInbox {
            text-align: left;
                flex: 1;
                padding:5px 20px;
        }
        .widget.c1 {
            background: #8076e9;
        }

        .widget.c2 {
            background: #d8b861;
        }

        .widget.c3 {
            background: #1a8a9b;
        }

        .widget.c4 {
            background: #f46b4b;
        }
        .widget.c5 {
            background: #F4C043;
        }
        .widget.c6 {
            background: #33B7E8;
        }
        .widget.c7 {
            background: #773EEA;
        }
        .widget.c8 {
            background: #2EBF72;
        }
        .wdgLabel {
            font-size: 16px;
        }

        .wdgNumber {
            font-size: 26px;
            font-weight: 600;
        }
        .tabSlideContainer {
            overflow: hidden;
        }
        .tabSlideContainer .swNav{
            position: absolute;
            height: 100%;
            min-width: 30px;
            /* text-align: center; */
            display: flex;
            justify-content: center;
            align-items: center;
            background: rgba(255,255,255,0.5);
            z-index: 5;
            -moz-transition:all 0.3s ease;
            -webkit-transition:all 0.3s ease;
            transition:all 0.3s ease;
        }
        .tabSlideContainer .swNav:hover {
            background: #1a8a9b;
            color:#fff;
            cursor:pointer;
        }
        .tabSlideContainer .swNav.snavPrev{
            left:-35px;
        }
        .tabSlideContainer .swNav.snavNext{
            right:-35px;
        }
        .tabSlideContainer:hover .swNav.snavPrev{
            left:0;
        }
        .tabSlideContainer:hover .swNav.snavNext{
            right:0;
        }
        .flex-row {
            display: flex;
        }
        .itemType {
            min-width: 150px;
            width: 14%;
            min-height: 200px;
            background: #fff;
            padding: 10px;
            box-sizing: border-box;
            border-radius: 4px;
            text-align: center;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
        }
        .space-between {
            justify-content: space-between;
        }
        .valRound {
            margin: 0 auto;
            width: 80px;
            height: 80px;
            background: #565454;
            color: #fff;
            border-radius: 50%;
            margin-bottom: 15px;
            font-size: 30px;
            line-height: 81px;
        }
        .semiRound {
            border-radius: 10px !important;
            height: 40px !important;
            line-height: 38px;
            font-size: 24px;
        }
        .valRound.c1 {
            background: #f76d6d;
        }
        .valRound.c2 {
            background: #6e98f5;
        }
        .valRound.c3 {
            background: #44c8cd;
        }
        .valRound.c4 {
            background: #9db53c;
        }
        .valRound.c5 {
            background: #8846c5;
        }
        .smallmuted, .hdTag {
            text-transform: uppercase;
        }
        .hdTag {
            font-size: 18px;
            margin-top: 5px;
        }
    </style>
    <script>
        $(function () {
            var swiper = new Swiper('.swiper-container', {
                slidesPerView: 4,
                spaceBetween: 30,
                pagination: {
                    el: '.swiper-pagination',
                    clickable: true,
                },
                navigation: {
                    nextEl: '.snavNext',
                    prevEl: '.snavPrev',
                },
                autoplay: false
            });
            var tabfirst = $('.abc').find('.swiper-slide').first().find('.zoom').data('click')

            $('.zoom').click(function (e) {
                //$('.arrowPointer').remove();
                $('.zoom').removeClass('DisableClass');
                var divid = $(this).attr('data-click');
                this.className = this.className + ' DisableClass';
                $('.panelClass').hide();
                $('.panelClass').css({ top: '50px', opacity: '0' });
                $('#' + divid).show();
                $('#' + divid).animate({ top: '0px', opacity: '1' });

                //this.children[0].children[0].innerHTML = this.children[0].children[0].innerHTML + '<i class="far fa-hand-point-left "></i>';
            });
            //$('#fTab').show().animate({ top: '0px', opacity: '1' });
            $('#' + tabfirst).show().animate({ top: '0px', opacity: '1' });
            $('.abc').find('.swiper-slide').first().find('.zoom').addClass('DisableClass');

        })
    </script>
</head>
<body>
    <div class="dashboardWraper"> 
        <div class="clearfix">
            <div class="col-md-12 clearfix padding  " style="padding: 10px;">
                <h3 class="pull-left HeaderStyleCRM fontPop">Sales Due Analysis</h3>
                    <span class="pull-right closeBtn" style="z-index:999">  <a href="#" onclick="reloadParent()" ><i class="fa fa-times"></i></a></span>
            </div>
        </div>
        <div class="form_main"></div>
        <form id="form1" runat="server">
            <div class="col-md-12">
                <div class="tabSlideContainer relative">
                    <div class="swNav snavPrev hide">
                        <i class="fa fa-arrow-left"></i>
                    </div>
                    <div class="swNav snavNext hide"><i class="fa fa-arrow-right"></i></div>
                
                    <div class="swiper-container vTabWrap fontPop ">
                        <div class="swiper-wrapper abc">
                                <div class="swiper-slide" id="DivActivities" runat="server">
                                    <div class="widget c4 zoom DisableClass" data-click="tab1">
                                
                                        <div class="textInbox">
                                            <div style="padding:10px"><img src="../images/icons/overdueW.png" style="max-width:35px" /></div>
                                            <div class="wdgLabel">OverDue</div>
                                        </div>
                                        <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                                    </div>
                                </div>
                                <div class="swiper-slide" id="DivCampaigns" runat="server">
                                    <div class="widget c7 zoom" data-click="tab2">
                                        <div class="textInbox">
                                            <div style="padding:10px"><img src="../images/icons/dueTodayW.png" style="max-width:35px" /></div>
                                            <div class="wdgLabel">Due Today</div>
                                        </div>
                                        <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                                    </div>
                                </div>
                                <div class="swiper-slide" id="DivLeads" runat="server">
                                    <div class="widget c3 zoom" data-click="tab3">
                               
                                        <div class="textInbox">
                                            <div style="padding:10px 0"><img src="../images/icons/allDueW.png" style="max-width:35px" /></div>
                                            <div class="wdgLabel">Due</div>
                                    
                                        </div>
                                         <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                                    </div>
                                </div> 
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix">
                <div class="container-fluid clearfix" style="margin:15px 0">
                  <div class="col-md-12 hide">
                      <ul class=" tabluLer clearfix " id="nav-tab" role="tablist">
                      <li class="active rd">
                         <a  href="#tab1" data-toggle="tab" role="tab" aria-controls="Performance" aria-selected="true">
                           <div class="">
                               <span class="ic">
                                   <img src="../images/icons/overdue.png" class="normal" />
                                   <img src="../images/icons/overdueW.png" class="white" />
                               </span>
                           </div>
                           <div class="tb_txt">OverDue</div>
                         </a>

                      </li>
                      <li class="grn">
                          <a href="#tab2" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                           <div class="" >
                               <span class="ic">
                                    <img src="../images/icons/dueToday.png" class="normal" />
                                    <img src="../images/icons/dueTodayW.png" class="white" />
                               </span>
                           </div>
                           <div class="tb_txt">Due Today</div>
                         </a>
                      </li>
                      <li class="bl">
                          <a href="#tab3" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                           <div class="">
                               <span class="ic">
                                    <img src="../images/icons/allDue.png" class="normal" />
                                    <img src="../images/icons/allDueW.png" class="white" />
                               </span>
                           </div>
                           <div class="tb_txt">Due </div>
                         </a>
                      </li>
                    </ul>
                  </div>
                </div>


                <div class="container-fluid" >
                  <div class="col-md-12">
                      <div class="BoxTypeGrey relativeLoader">
                       <div id="loaderP" class="hide">
                            <img src="../images/bars.svg" style="max-width: 20px;" />
                            <div>Fetching data ....</div>
                        </div>
                        <div class="tab-content" id="nav-tabContent">
                            <div class="tab-pane  panelClass" id="tab1" role="tabpanel" aria-labelledby="nav-home-tab">
                                <div class="container-fluid">
                                    <div class="clearfix boxSelection" style="margin:0">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <label>Select Branch</label>
                                                <div class="dexCntrl">
                                                    <div id="label-branch"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Customer</label>
                                                <div class="dexCntrl">
                                                    <div id="label-customer"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Salesman</label>
                                                <div class="dexCntrl">
                                                    <div id="label-salesman"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>As on Date</label>
                                                <div class="dexCntrl">
                                                    <div id="selected-date"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            
                                                <div style="padding-top: 18px;">
                                                    <button class="btn btn-success">Search</button>
                                                    <select  onchange="" id="" class="btn btn-primary">
                                                        <option selected="selected" value="0">Export to</option>
                                                        <option value="1">PDF</option>
                                                        <option value="2">XLS</option>
                                                        <option value="3">RTF</option>
                                                        <option value="4">CSV</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="space"></div>
                                <div class="clearfix">
                                  <div class="row">
                                      <div class="col-md-12">
                                          <div class="shadowBox">
                                              <div class="bigHeading">List of OverDue </div>
                                              <div class="demo-container" style="padding:0 15px">
                                                <div id="gridContainer"></div>
                                             </div>
                                              <div class="table-responsive hide">
                                                  <table class="table styledTble rightGap">
                                                      <thead>
                                                        <tr>
                                                            <th scope="col"> Salesman </th>
                                                            <th scope="col"> Customer</th>
                                                            <th scope="col"> Invoice #</th>
                                                            <th scope="col"> Due Amount</th>
                                                            <th scope="col"> Days</th>
                                                        </tr>
                                                      </thead>
                                                      <tbody id="">
                                                        <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>5</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>5</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>5</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>5</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>5</td>
                                                        </tr>
                                                      </tbody>
                                                    </table>
                                              </div>
                                          </div>
                                      </div>
                                  
                                  </div>
                              </div>
                            </div>
                            <div class="tab-pane panelClass" id="tab2" role="tabpanel" aria-labelledby="nav-home-tab">
                                <div class="container-fluid">
                                    <div class="clearfix boxSelection" style="margin:0">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <label>Select Branch</label>
                                                <div>
                                                    <select class="form-control">
                                                        <option>Select</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Customer</label>
                                                <div>
                                                    <select class="form-control">
                                                        <option>Select</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Salesman</label>
                                                <div>
                                                    <select class="form-control">
                                                        <option>Select</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Today</label>
                                                <div>
                                                    <input type="text" id="toDateRE" class="form-control datepicker" disabled />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                           
                                                <div style="padding-top: 18px;">
                                                    <button class="btn btn-success">Search</button>
                                                    <select  onchange="" id="" class="btn btn-primary">
                                                        <option selected="selected" value="0">Export to</option>
                                                        <option value="1">PDF</option>
                                                        <option value="2">XLS</option>
                                                        <option value="3">RTF</option>
                                                        <option value="4">CSV</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="space"></div>
                                <div class="clearfix">
                                  <div class="row">
                                      <div class="col-md-12">
                                          <div class="shadowBox">
                                              <div class="bigHeading">List of Due Today </div>
                                              <div class="table-responsive">
                                                  <table class="table styledTble rightGap">
                                                      <thead>
                                                        <tr>
                                                            <th scope="col"> Salesman </th>
                                                            <th scope="col"> Customer</th>
                                                            <th scope="col"> Invoice #</th>
                                                            <th scope="col"> Due Amount</th>
                                                      
                                                        </tr>
                                                      </thead>
                                                      <tbody id="">
                                                        <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                      
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                        
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                       
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                       
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                        
                                                        </tr>
                                                      </tbody>
                                                    </table>
                                              </div>
                                          </div>
                                      </div>
                                  
                                  </div>
                              </div>
                            </div>
                            <div class="tab-pane panelClass" id="tab3" role="tabpanel" aria-labelledby="nav-home-tab">
                                <div class="container-fluid">
                                    <div class="clearfix boxSelection" style="margin:0">
                                        <div class="row">
                                            <div class="col-md-2">
                                                <label>Select Branch</label>
                                                <div>
                                                    <select class="form-control">
                                                        <option>Select</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Customer</label>
                                                <div>
                                                    <select class="form-control">
                                                        <option>Select</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>Salesman</label>
                                                <div>
                                                    <select class="form-control">
                                                        <option>Select</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>From</label>
                                                <div>
                                                    <input type="text" id="" class="form-control datepicker" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <label>To</label>
                                                <div>
                                                    <input type="text" id="" class="form-control datepicker" />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                            
                                                <div style="padding-top: 18px;">
                                                    <button class="btn btn-success">Search</button>
                                                    <select  onchange="" id="" class="btn btn-primary">
                                                        <option selected="selected" value="0">Export to</option>
                                                        <option value="1">PDF</option>
                                                        <option value="2">XLS</option>
                                                        <option value="3">RTF</option>
                                                        <option value="4">CSV</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="space"></div>
                                <div class="clearfix">
                                  <div class="row">
                                      <div class="col-md-12">
                                          <div class="shadowBox">
                                              <div class="bigHeading">List of all Due</div>
                                              <div class="table-responsive">
                                                  <table class="table styledTble rightGap">
                                                      <thead>
                                                        <tr>
                                                            <th scope="col"> Salesman </th>
                                                            <th scope="col"> Customer</th>
                                                            <th scope="col"> Invoice #</th>
                                                            <th scope="col"> Due Amount</th>
                                                            <th scope="col"> Due Date</th>
                                                        </tr>
                                                      </thead>
                                                      <tbody id="">
                                                        <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>12-07-2020</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>12-07-2020</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>12-07-2020</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>12-07-2020</td>
                                                        </tr>
                                                          <tr>
                                                            <td>Susanta</td>
                                                            <td>Susanta</td>
                                                            <td>41546451346</td>
                                                            <td>12,500</td>
                                                            <td>12-07-2020</td>
                                                        </tr>
                                                      </tbody>
                                                    </table>
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
            </div>
        </form>


    </div>
    
    
</body>
</html>
