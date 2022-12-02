<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectDB.aspx.cs" Inherits="DashBoard.DashBoard.ProjectDB.projectDB" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Dashboard</title>
   <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,200;0,500;0,600;0,700;0,800;0,900;1,400&display=swap" rel="stylesheet" />

    <script src="../Js/SearchPopup.js?v1.001"></script>
    <link href="../css/SearchPopup.css" rel="stylesheet" />

    <script src="../Js/jquery3.3.1.min.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="../css/projectDB.css?v1.002" rel="stylesheet" />
    <link href="../Js/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../Js/datePicker/bootstrap-datepicker.js"></script>
    <link href="../css/jquery.alerts.css" rel="stylesheet" />
    <script src="../Js/jquery.alerts.js"></script>
    <script src="../Js/tether.min.js"></script>
    <link href="../Js/jsgnatt/JSgnattNew.css" rel="stylesheet" />
    <script src="../Js/jsgnatt/JSgnattNew.js"></script>  
    <style>
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
    </style>
    <style>

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
    </style>
    
    <script src="projectDb.js?v=1.02"></script>
    <script>

        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>

    
</head>
    
<body>

    <h3 class="kpiHeading relative">
        Project Dashboard
        <span class="pull-right closeBtn"><a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
    </h3>
    <form id="form2" runat="server">
        
          <div class="container-fluid">
              <div class="clearfix boxSelection">
                  <div class="row">
                      <div class="col-md-2">
                          <label><dxe:ASPxLabel ID="lbl_Customer" runat="server" Text="Customer">
                          </dxe:ASPxLabel></label>
                          <div><dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                                     <Buttons>
                                      <dxe:EditButton>
                                     </dxe:EditButton>
                                       </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                          </dxe:ASPxButtonEdit>
                              </div>
                      </div>
                      <div class="col-md-2">
                          <label><dxe:ASPxLabel ID="lblProject" runat="server" Text="Project">
                           </dxe:ASPxLabel><span style="color: red">*</span></label>
                          <div>
                            
                                        <dxe:ASPxGridLookup ID="lookup_Project" runat="server" SelectionMode="Multiple" ClientInstanceName="clookup_Project" DataSourceID="EntityServerModeDataDashBoard"
                                                    KeyFieldName="Proj_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                    <Columns>
                                                         <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                                        <dxe:GridViewDataColumn FieldName="Proj_Code" Visible="true" VisibleIndex="1" Caption="Project Code" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Proj_Name" Visible="true" VisibleIndex="2" Caption="Project Name" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                        <dxe:GridViewDataColumn FieldName="Customer" Visible="true" VisibleIndex="3" Caption="Customer" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>
                                                       <%-- <dxe:GridViewDataColumn FieldName="HIERARCHY_NAME" Visible="true" VisibleIndex="4" Caption="Hierarchy" Settings-AutoFilterCondition="Contains" Width="200px">
                                                        </dxe:GridViewDataColumn>--%>
                                                    </Columns>
                                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                                                        <Templates>
                                                            <StatusBar>
                                                                <table class="OptionsTable" style="float: right">
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </StatusBar>
                                                        </Templates>
                                                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                                    </GridViewProperties>
                                                    <ClientSideEvents GotFocus="function(s,e){clookup_Project.ShowDropDown();}" />
                                                    <ClearButton DisplayMode="Always">
                                                    </ClearButton>
                                            </dxe:ASPxGridLookup>
                                               <dx:LinqServerModeDataSource ID="EntityServerModeDataDashBoard" runat="server" OnSelecting="EntityServerModeDataDashBoard_Selecting"
                                                    ContextTypeName="DashBoardDataContext" TableName="ProjectCodeBind" />
                              </div>
                      </div>
                      
                      <div class="col-md-2">
                          <label>Date <span style="color: red">*</span></label>
                          <div>
                              <input type="text" id="toDateRE" class="form-control datepicker" />
                          </div>
                      </div>
                      <div class="col-md-2 " style="padding-top: 17px;">
                         <dxe:ASPxButton ID="btn_Search" ClientInstanceName="cbtn_Search" runat="server" AutoPostBack="False" Text="Search" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                     <ClientSideEvents Click="function(s, e) {ProjectCustomerWiseSearch();}" />
                          </dxe:ASPxButton>
                      </div>
                      
                  </div>
              </div>
          </div>

          <div class="container-fluid clearfix">
              <div class="col-md-12">
                  <ul class=" tabluLer clearfix " id="nav-tab" role="tablist">
                  <li class="active" id="LiProjectSummary" runat="server">
                     <a  href="#tab1" data-toggle="tab" role="tab" aria-controls="Performance" aria-selected="true">
                       <div class="">
                           <span class="ic"><i class="fa fa-bolt"></i></span>
                       </div>
                       <div class="tb_txt">Project  Summary</div>
                     </a>

                  </li>
                  <li onclick="" id="LiProjectDetails" runat="server">
                      <a href="#tab2" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                       <div class="" >
                           <span class="ic"><i class="fa fa-tasks"></i></span>
                       </div>
                       <div class="tb_txt">Project Details</div>
                     </a>
                  </li>
                  <li onclick="" id="LiTimeline" runat="server">
                      <a href="#tab3" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                       <div class="">
                           <span class="ic"><i class="fa fa-clock"></i></span>
                       </div>
                       <div class="tb_txt">Timeline </div>
                     </a>
                  </li>
                  <li onclick="" id="LiCostBreakup" runat="server">
                      <a href="#tab4" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                       <div class="">
                           <span class="ic"><i class="fa fa-calculator"></i></span>
                       </div>
                       <div class="tb_txt">Cost Breakup</div>
                     </a>
                  </li>
                </ul>
              </div>
          </div>
            

          <div class="container-fluid">
              <div class="col-md-12">
                  <div class="BoxTypeGrey relativeLoader">
                   <div id="loaderP">
                        <img src="../images/bars.svg" style="max-width: 20px;" />
                        <div>Fetching data ....</div>
                    </div>
                    <div class="tab-content" id="nav-tabContent">
                      <div class="tab-pane  active" id="tab1" role="tabpanel" aria-labelledby="nav-home-tab">
                          <div class="clearfix">
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c10">
                                              <div class="showFullInfo" id="intCostF">0</div>
                                              <div id="intCost">0</div>
                                          </div>
                                         <div class="hdTag">Initial Est. cost</div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c4">
                                              <div class="showFullInfo" id="rvsCostF">0</div>
                                              <div id="rvsCost">0</div>
                                          </div>
                                         <div class="hdTag">Revised Est. cost</div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c5">
                                              <div class="showFullInfo" id="estCostF">0</div>
                                              <div id="estCost">0</div>
                                          </div>
                                          <div class="hdTag">ORDER PLACED AGST. ESTIMATE </div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c6">
                                              <div class="showFullInfo" id="costBookedF">0</div>
                                              <div id="costBooked">0</div>
                                          </div>
                                          <div class="hdTag">Cost Booked</div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c8">
                                              <div class="showFullInfo" id="orderBalanceF">0</div>
                                              <div id="orderBalance">0</div>
                                          </div>
                                          <div class="hdTag">total order balance</div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                              </div>
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c11">
                                              <div class="showFullInfo" id="intRevF">0</div>
                                              <div id="intRev">0</div>
                                          </div>
                                          
                                          <div class="hdTag">initial revenue</div>
                                          <div class="smallmuted">As on Date</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound">
                                              <div class="showFullInfo" id="rvsRevF">0</div>
                                             <div id="rvsRev">0</div>
                                          </div>
                                          
                                          <div class="hdTag">revised revenue</div>
                                          <div class="smallmuted">As on Date</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c1">
                                              <div class="showFullInfo" id="rvBookedF">0</div>
                                              <div id="rvBooked">0</div>
                                          </div>
                                          <div class="hdTag">revenue BOOKED</div>
                                          <div class="smallmuted">As on Date</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c2">
                                              <div class="showFullInfo" id="odBalanceF">0</div>
                                              <div id="odBalance">0</div>

                                          </div>
                                          <div class="hdTag">order BALANCE</div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c3">
                                              <div class="showFullInfo" id="intProfitF">0</div>
                                              <div id="intProfit">0</div>
                                          </div>
                                          <div class="hdTag">initial ESt. PROFIT</div>
                                          <div class="smallmuted">As on Date</div>
                                      </div>
                                  </div>
                                  
                              </div>
                              <div class="flex-row space-between align-items-center ">
                                  <div class="flex-item itemType w18 relative">
                                      <div class="">
                                          <div class="valRound c9">
                                              <div class="showFullInfo" id="rvProfitF">0</div>
                                              <div id="rvProfit">0</div>
                                          </div>
                                          
                                          <div class="hdTag">revised est. profit </div>
                                          <div class="smallmuted">As on Date</div>
                                          
                                      </div>
                                  </div>
                              </div>
                          </div>
                          
                      </div>
                      <div class="tab-pane " id="tab2" role="tabpanel" aria-labelledby="nav-profile-tab">
                          <div class="clearfix mTop20">
                              <div class="chartContainer">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="shadowBox text-left">
                                            <div class="pull-right calltoActionicons" style="margin: 15px;">
                                                <a href="#" class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                            </div>
                                            <div class="bigHeading">Projects Stagewise</div>
                                            <div class="mainAreachart">
                                                <div id="chartdiv" class="flexViewCenter" style="height: 364px;">No Data to Display</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="shadowBox text-left">
                                             <div class="pull-right calltoActionicons" style="margin: 15px;">
                                                <a href="#" class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                            </div>
                                            <div class="bigHeading">Project Details</div>
                                            <div class="mainAreachart">
                                                <div id="chartdiv2" class="flexViewCenter">No Data to Display</div>
                                            </div>

                                            <div class="stageIndicator">
                                                <ul class="clearfix">
                                                    <li><span class="indi color1"></span><span>New</span></li>
                                                    <li><span class="indi color2"></span><span>Qualify</span></li>
                                                    <li><span class="indi color3"></span><span>Planning</span></li>
                                                    <li><span class="indi color4"></span><span>Execution</span></li>
                                                    <li><span class="indi color5"></span><span>Deliver</span></li>
                                                    <li><span class="indi color6"></span><span>Complete</span></li>
                                                    <li><span class="indi color7"></span><span>Close</span></li>                                                 
                                                </ul>
                                            </div>
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
                                          <div class="bigHeading">Project Details</div>
                                          <div class="table-responsive">
                                              <table class="table styledTble rightGap">
                                                  <thead>
                                                    <tr>
                                                        <th scope="col"> Code</th>
                                                        <th scope="col"> Name</th>
                                                        <th scope="col"> Party Id</th>
                                                        <th scope="col"> Manager</th>
                                                        <th scope="col"> Stage</th>
                                                        <th scope="col"> Customer Name</th>
                                                        <th scope="col"> Actual Start date</th>
                                                        <th scope="col"> Estimated Start date</th>
                                                        <th scope="col"> Actual End date</th>
                                                        <th scope="col"> Est. Hours</th>
                                                        <th scope="col"> Actual Hours</th>
                                                        <th scope="col"> Est. Labour Cost</th>
                                                        <th scope="col"> Actual Labour Cost</th>
                                                        <th scope="col"> Est. Total Cost</th>
                                                    </tr>
                                                  </thead>
                                                  <tbody id="showTbleInfo">
                                                    
                                                  </tbody>
                                                </table>
                                          </div>
                                      </div>
                                  </div>
                                  
                              </div>
                          </div>
                      </div>
                      <div class="tab-pane " id="tab3" role="tabpanel" aria-labelledby="nav-contact-tab">
                          <div class="clearfix">
                              <div class="row">
                                  <div class="col-md-12">
                                      <div class="shadowBox">
                                          <div class="bigHeading">Projects</div>
                                          <div class="table-responsive">
                                              <table class="table styledTble">
                                                  <thead>
                                                    <tr>
                                                        <th scope="col">Party Id</th>
                                                        <th scope="col">Project Code</th>
                                                        <th scope="col">Project Name</th>
                                                        <th scope="col">Customer Name</th>
                                                        <th scope="col">Project Manager</th>
                                                        <th scope="col">Project Stage</th>
                                                        <th scope="col">Project Status</th>
                                                        <th scope="col">Actual Start Date</th>
                                                        <th scope="col">Est. Start  Date</th>
                                                        <th scope="col">End  Date</th>
                                                        <th scope="col">Est. Hours</th>
                                                        <th scope="col">Actual Hours</th>
                                                        <th scope="col">Est. Labour Cost</th>
                                                        <th scope="col">Actual Labour Cost</th>
                                                       <th scope="col">Est. Total Cost</th>
                                                 
                                                    </tr>
                                                  </thead>
                                                  <tbody id="showTbleInfoTimeline"></tbody>
                                                </table>
                                          </div>
                                      </div>

                                      <div class="shadowBox" id="TmChart" style="margin-top:10px">
                                          <div class="bigHeading">Timeline</div>
                                          <div>
                                              <div id="gnatt" class="hide" style="height:350px"></div>
                                              <div class="gantt" id="GanttChartDIV" style="margin-top:20px"></div>
                                          </div>
                                      </div>
                                  </div>
                                  
                              </div>
                          </div>
                      </div>
                      <div class="tab-pane " id="tab4" role="tabpanel" aria-labelledby="nav-contact-tab">
                          <div class="clearfix mTop20">
                              <div class="chartContainer">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="shadowBox text-left">
                                            <div class="pull-right calltoActionicons" style="margin: 15px;">
                                                <a href="#" class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                            </div>
                                            <div class="bigHeading">Initial Cost Breakup</div>
                                            <div class="mainAreachart">
                                                <div id="chartcosting1" class="flexViewCenter">No Data to Display</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 ">
                                        <div class="shadowBox text-left">
                                            <div class="pull-right calltoActionicons" style="margin: 15px;">
                                                <a href="#" class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                            </div>
                                            <div class="bigHeading" >Revised Cost Breakup</div>
                                            <div class="mainAreachart">
                                                <div id="chartcosting2" class="flexViewCenter">No Data to Display</div>
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
       
  <script src="..\js\amChart\core.js"></script>
  <script src="..\js\amChart\charts.js"></script>
  <script src="..\js\amChart\maps.js"></script>
  <script src="..\js\amChart\animated.js"></script>
      

<!-- Chart code -->
        <script>
            // gnatt chart
           
                    am4core.ready(function() {

                        // Themes begin
                        am4core.useTheme(am4themes_animated);
                        // Themes end

                        var chart = am4core.create("gnatt", am4charts.XYChart);
                        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

                        chart.paddingRight = 30;
                        chart.dateFormatter.inputDateFormat = "yyyy-MM-dd HH:mm";

                        var colorSet = new am4core.ColorSet();
                        colorSet.saturation = 0.4;

                        chart.data = [
                          {
                              name: "John",
                              fromDate: "2018-01-01 08:00",
                              toDate: "2018-01-01 10:00",
                              color: colorSet.getIndex(0).brighten(0)
                          },
                          {
                              name: "John",
                              fromDate: "2018-01-01 12:00",
                              toDate: "2018-01-01 15:00",
                              color: colorSet.getIndex(0).brighten(0.4)
                          },
                          {
                              name: "John",
                              fromDate: "2018-01-01 15:30",
                              toDate: "2018-01-01 21:30",
                              color: colorSet.getIndex(0).brighten(0.8)
                          },

                          {
                              name: "Jane",
                              fromDate: "2018-01-01 09:00",
                              toDate: "2018-01-01 12:00",
                              color: colorSet.getIndex(2).brighten(0)
                          },
                          {
                              name: "Jane",
                              fromDate: "2018-01-01 13:00",
                              toDate: "2018-01-01 17:00",
                              color: colorSet.getIndex(2).brighten(0.4)
                          },

                          {
                              name: "Peter",
                              fromDate: "2018-01-01 11:00",
                              toDate: "2018-01-01 16:00",
                              color: colorSet.getIndex(4).brighten(0)
                          },
                          {
                              name: "Peter",
                              fromDate: "2018-01-01 16:00",
                              toDate: "2018-01-01 19:00",
                              color: colorSet.getIndex(4).brighten(0.4)
                          },

                          {
                              name: "Melania",
                              fromDate: "2018-01-01 16:00",
                              toDate: "2018-01-01 20:00",
                              color: colorSet.getIndex(6).brighten(0)
                          },
                          {
                              name: "Melania",
                              fromDate: "2018-01-01 20:30",
                              toDate: "2018-01-01 24:00",
                              color: colorSet.getIndex(6).brighten(0.4)
                          },

                          {
                              name: "Donald",
                              fromDate: "2018-01-01 13:00",
                              toDate: "2018-01-01 24:00",
                              color: colorSet.getIndex(8).brighten(0)
                          }
                        ];

                        var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                        categoryAxis.dataFields.category = "name";
                        categoryAxis.renderer.grid.template.location = 0;
                        categoryAxis.renderer.inversed = true;

                        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
                        dateAxis.dateFormatter.dateFormat = "yyyy-MM-dd HH:mm";
                        dateAxis.renderer.minGridDistance = 70;
                        dateAxis.baseInterval = { count: 30, timeUnit: "minute" };
                        dateAxis.max = new Date(2018, 0, 1, 24, 0, 0, 0).getTime();
                        dateAxis.strictMinMax = true;
                        dateAxis.renderer.tooltipLocation = 0;

                        var series1 = chart.series.push(new am4charts.ColumnSeries());
                        series1.columns.template.width = am4core.percent(80);
                        series1.columns.template.tooltipText = "{name}: {openDateX} - {dateX}";

                        series1.dataFields.openDateX = "fromDate";
                        series1.dataFields.dateX = "toDate";
                        series1.dataFields.categoryY = "name";
                        series1.columns.template.propertyFields.fill = "color"; // get color from data
                        series1.columns.template.propertyFields.stroke = "color";
                        series1.columns.template.strokeOpacity = 1;

                        chart.scrollbarX = new am4core.Scrollbar();

                    }); // end am4core.ready()


            
        </script>
        <script>
            
        </script>
         <asp:HiddenField ID="hdnCustomerId" runat="server" />

    </form>

     <!--Customer Modal -->
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
   
</body>
</html>







