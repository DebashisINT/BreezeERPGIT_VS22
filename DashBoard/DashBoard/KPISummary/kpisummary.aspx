<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kpisummary.aspx.cs" Inherits="DashBoard.DashBoard.KPISummary.kpisummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kpi Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,200;0,500;0,600;0,700;0,800;0,900;1,400&display=swap" rel="stylesheet" /> 
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="../css/kpiDB.css" rel="stylesheet" />
    <link href="../Js/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../Js/datePicker/bootstrap-datepicker.js"></script>
    <link href="../../assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../../assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="../../assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>
    <script src="kpiSummary.js?1.0.14"></script>
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>
    <style>
        .closeBtn.whd>a {
            color: #ccc;
        }
        .shwDet {
            cursor:pointer;
        }
        .showFullInfo {
        position: absolute;
        top: -15px;
        width: 100%;
        padding: 0 20px;
        left: 0;
        background: rgba(56, 42, 42, 0.75);
        height: 30px;
        color: #f7f7f7;
        font-size: 16px;
        line-height: 33px;
        border-radius: 4px;
        visibility: hidden;
    }
        .chartSec {
            visibility:hidden;
        }
        .table-background{
            width: 100% !important;
        }
        .table-background>thead>tr>th{
            background: #58cad6;
        }
        .valRound:hover > .showFullInfo {
            visibility: visible;
        }
        .valRound.semiRound {
            padding: 0 10px;
            display: inline-block;
            width:auto !important
        }
    </style>
</head>
    
<body>

    <h3 class="kpiHeading">
        KPI Summary
        <span class="pull-right closeBtn whd"><a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
    </h3>
    <form id="form1" runat="server">
        
          <div class="container-fluid">
              <div class="clearfix boxSelection">
                  <div class="row">
                      <div class="col-md-2">
                          <label>As on Date <span style="color: red">*</span></label>
                          <div>
                              <input type="text" id="toDateRE" class="form-control datepicker" style="height: 30px;" />
                          </div>
                      </div>
                      <div class="col-md-2">
                          <label>Branch</label>
                          <div >
                              <select class="form-control bigSelect" id="branchSelect">
                                  <option value="">All</option>
                              </select>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <label>Employe</label>
                          <div >
                              <select class="form-control bigSelect" id="empSelect">
                                  <option value="">All</option>
                              </select>
                          </div>
                      </div>
                      <div class="col-md-2">
                          <label>&nbsp;</label>
                          <div>
                              <button type="button" class="btn btn-success" id="getData">Search</button>
                          </div>
                      </div>
                  </div>
              </div>
          </div>

          <div class="container-fluid clearfix">
              <div class="col-md-12">
                  <ul class=" tabluLer clearfix" id="nav-tab" role="tablist">
                  <li class="active" id="liPerformance" runat="server">
                     <a  href="#Performance" data-toggle="tab" role="tab" aria-controls="Performance" aria-selected="true">
                       <div class="text-center">
                           <span class="ic"><i class="fa fa-bolt"></i></span>
                       </div>
                       <div class="tb_txt">Performance</div>
                     </a>

                  </li>
                  <li  id="liActivities" runat="server">
                      <a href="#Activities" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                       <div class="text-center">
                           <span class="ic"><i class="fa fa-tasks"></i></span>
                       </div>
                       <div class="tb_txt">Activities</div>
                     </a>
                  </li>
                  <li id="liEmployeeInfo" runat="server">
                      <a href="#EmployeeI" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                       <div class="text-center">
                           <span class="ic"><i class="fa fa-users"></i></span>
                       </div>
                       <div class="tb_txt">Employee Info</div>
                     </a>
                  </li>
                  <li id="liResolution" runat="server">
                      <a href="#Achivement" data-toggle="tab" role="tab" aria-controls="" aria-selected="true">
                       <div class="text-center">
                           <span class="ic"><i class="fa fa-database"></i></span>
                       </div>
                       <div class="tb_txt">Resolution</div>
                     </a>
                  </li>
                  
                </ul>
              </div>
          </div>
            

          <div class="container-fluid">
              <div class="col-md-12">
                  <div class="BoxTypeGrey">
                    <div class="tab-content" id="nav-tabContent">
                      <div class="tab-pane  active" id="Performance" role="tabpanel" aria-labelledby="nav-home-tab">
                          <div class="clearfix">
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound" >
                                              <div class="showFullInfo" id="ldAmtF">0</div>
                                              <div id="ldAmt">0</div>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Leads</div>
                                          <div class=" vSm">Count : <span id="ldcnt">0</span></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c1">
                                              <div class="showFullInfo" id="InqAmtF">0</div>
                                              <div id="InqAmt">0</div>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Inquiry</div>
                                           <div class=" vSm">Count : <span id="Inqcnt">0</span></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c2">
                                              <div class="showFullInfo" id="qAmtF">0</div>
                                              <div id="qAmt">0</div>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Quotation</div>
                                           <div class=" vSm">Count : <span id="qcnt">0</span></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c3">
                                              <div class="showFullInfo" id="oAmtF">0</div>
                                              <div id="oAmt">0</div>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Order</div>
                                           <div class=" vSm">Count : <span id="ocnt">0</span></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c4">
                                              <div class="showFullInfo" id="sAmtF">0</div>
                                              <div id="sAmt">0</div>
                                          </div>
                                          <div class="smallmuted ">Total</div>
                                          <div class="hdTag">Sales</div>
                                          <div class=" vSm">Count : <span id="scnt">0</span></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c5">
                                              <div class="showFullInfo" id="cAmtF">0</div>
                                              <div id="cAmt">0</div>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Collection</div>
                                           <div class=" vSm">Count : <span id="ccnt">0</span></div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                          <div class="clearfix mTop20 chartSec" >
                              <div class="flex-row space-between">
                                  <div class="flex-item itemTypeChart w25 relative">
                                      <h4><b>Lead Status</b></h4>
                                      <div>
                                          <div id="chartdivDonut"></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemTypeChart w25 relative">
                                      <h4><b>Inquiry Breakdown</b></h4>
                                      <div>
                                          <div id="chartdivL1"></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemTypeChart w25 relative">
                                      <h4><b>Quotation Breakdown</b></h4>
                                      <div>
                                          <div id="chartdivL2"></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemTypeChart w25 relative">
                                      <h4><b>Order Breakdown</b></h4>
                                      <div>
                                          <div id="chartdivL3"></div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemTypeChart w25 relative">
                                      <h4><b>Invoice Breakdown</b></h4>
                                      <div>
                                          <div id="chartdivL4"></div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                      <div class="tab-pane " id="Activities" role="tabpanel" aria-labelledby="nav-profile-tab">
                          <div class="clearfix">
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/online-activity.png" style="width:40px;margin-bottom: 12px;" />
                                          </div>
                                          <div class="valRound semiRound">
                                              <span id="actValue"></span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Activities</div>
                                         
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/gmail.png" style="width:40px;margin-bottom: 12px;" />
                                          </div>
                                          <div class="valRound semiRound c1">
                                              <span id="emailValue"></span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Emails</div>
                                         
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/smartphone.png" style="width:40px;margin-bottom: 12px;" />
                                          </div>
                                          <div class="valRound semiRound c2">
                                              <span id="callsmsValue"></span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Call/SMS</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/visitor.png" style="width:40px;margin-bottom: 12px;" />
                                          </div>
                                          <div class="valRound semiRound c3">
                                              <span id="visitValue"></span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Visits</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/account.png" style="width:40px;margin-bottom: 12px;" />
                                          </div>
                                          <div class="valRound semiRound c4">
                                              <span id="socialValue"></span>
                                          </div>
                                          <div class="smallmuted ">Total</div>
                                          <div class="hdTag">Social</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/clipboards.png" style="width:40px;margin-bottom: 12px;" />
                                          </div>
                                          <div class="valRound semiRound c5">
                                              <span id="otherValue"></span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Others</div>
                                          
                                      </div>
                                  </div>
                              </div>
                          </div>
                          
                          <div class="clearfix">
                              <div class="row">
                                  <div class="col-md-6">
                                      <div class="backBoxx">
                                          <h4>Transaction Volumn</h4>
                                          <div>
                                              <table class="table display table-background" id="TransacVolumeTAble">
                                                  <thead>
                                                    <tr>
                                                      <th scope="col">Module</th>
                                                      <th scope="col">Todays</th>
                                                      <th scope="col" style="text-align:right">Total</th>
                                                    </tr>
                                                  </thead>
                                                  
                                                </table>
                                          </div>
                                      </div>
                                  </div>
                                  <div class="col-md-6">
                                      <div class="backBoxx">
                                          <h4>Task Volumn</h4>
                                          <div>
                                              <table class="table display table-background" id="TaskVolumeTAble">
                                                  <thead>
                                                    <tr>
                                                      <th scope="col">Topic</th>
                                                      <th scope="col">Points</th>
                                                      <th scope="col" >Rating</th>
                                                    </tr>
                                                  </thead>
                                                </table>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                          </div>
                          <div style="padding:10px"></div>
                          <div class="clearfix hide">
                              <div class="flex-row space-between align-items-center">
                                  
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c2">
                                              15
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">OnTime</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c3">
                                              15
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Before Time</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c4">
                                              15
                                          </div>
                                          <div class="smallmuted ">Total</div>
                                          <div class="hdTag">After Time</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative trans">
                                      
                                  </div>
                                  <div class="flex-item itemType relative trans">
                                      
                                  </div>
                                  <div class="flex-item itemType relative trans">
                                      
                                  </div>
                              </div>
                          </div>  
                      </div>
                      <div class="tab-pane " id="EmployeeI" role="tabpanel" aria-labelledby="nav-contact-tab">
                          <div class="clearfix">
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound semiRound" >
                                              <span id="WORKINGDAYS">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Working Days</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound semiRound c1">
                                              <span id="PRESENTS">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Presents</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound semiRound c2">
                                              <span id="LEAVES">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Leaves</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound semiRound c3">
                                              <span id="HALFDAYS">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Half Days</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound semiRound c4">
                                              <span id="EMPCTC">0</span>
                                          </div>
                                          <div class="smallmuted ">Total</div>
                                          <div class="hdTag">CTC</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound semiRound c5">
                                              <span id="EXPAMT">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Others Expences</div>
                                          
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                      <div class="tab-pane " id="Achivement" role="tabpanel" aria-labelledby="nav-contact-tab">
                          <div class="clearfix">
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound">
                                              155
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Cases</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c1">
                                              15
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Resolved</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c2">
                                              15
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Pending</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c3">
                                              15
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">On Process</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c4">
                                              15
                                          </div>
                                          <div class="smallmuted ">Total</div>
                                          <div class="hdTag">Under Subs</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="valRound c5">
                                              15
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Out of Service</div>
                                          
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


    </form>
</body>
</html>

