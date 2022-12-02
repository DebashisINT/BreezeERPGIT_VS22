<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialDB.aspx.cs" Inherits="DashBoard.DashBoard.FinancialDB.FinancialDB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Financial Dashboard</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />

    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@200;300;400;500;600;700;800&display=swap" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="financialDb.css?1.0.3" rel="stylesheet" />
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>
    <script src="finance.js?v1.0.1"></script>
    <link href="finance.css?v1.0.1" rel="stylesheet" />
    
    <script>
        function reloadParent(e) {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>
</head>
<body>
<form id="form1" runat="server">
<div class="clearfix lightBack">
    <div class="col-md-12 clearfix padding  " style="padding: 10px;">
        <h3 class="pull-left HeaderStyleCRM fontPop">Financial KPI</h3>
        <div class="pull-right">
            <table>
                <tr>
                    <td>
                        <div class="calecaleCnt">
                             <div class="dbLd hide">
                                <div class="db-spinner"></div>
                              </div>
                            <div class="cmp hide">Done</div>
                        </div>
                    </td>
                    <td><button type="button" class="btn btn-success" style="margin-bottom:0" onclick="GetonRefresh()">Calculate</button></td>
                    <td valign="middle"><span class=" closeBtn" style="z-index:999"><a href="#" onclick="reloadParent()" ><i class="fa fa-times"></i></a></span></td>
                </tr>
            </table>
        </div>  
    </div>
</div>
  <div class="container-fluid form_main relativeLoader">
     
    <div class="totalContainer popi">
        <div class="highlighter">
          <div class="hdTag">REVENUE</div>
          <div class="value">
              <span id="revenue" title=""></span>
          </div>
          <div>Last Month 1.52 M ( +135%)</div>
        </div>
        <div class="fl-1 dataCont relative">
            <div class="tabSlideContainer ">
                    <div class="swNav snavPrev">
                        <i class="fa fa-arrow-left"></i>
                    </div>
                    <div class="swNav snavNext"><i class="fa fa-arrow-right"></i></div>
                    <div class="swiper-container SLIDEsCROLL fontPop ">
                        <div class="swiper-wrapper">
                            <div class="swiper-slide">
                                <div class="text-center">
                                  <div class="hdTag">Total COGS</div>
                                  <div class="value dataColor1">
                                      <span id="tlCogs" title=""></span><span class="sIcon"><i class="fa fa-exclamation"></i></span>
                                  </div>
                                  <div class="smallFont">Last Month 1.06 M ( -102.33%)</div>
                                </div>
                             </div>
                            <div class="swiper-slide">
                                <div class="text-center">
                                    <div class="hdTag">Expenses</div>
                                    <div class="value dataColor1">
                                         <span id="expens" title=""></span><span class="sIcon"><i class="fa fa-exclamation"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month 0.46 M ( +219.4%)</div>
                                </div>
                             </div>
                            <div class="swiper-slide">
                                <div class="text-center">
                                    <div class="hdTag">Gross Profit</div>
                                    <div class="value clPrim">
                                        <span id="grsProfit" title=""></span><span class="sIcon"><i class="fa fa-check"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month 30.42%</div>	
                                </div>
                             </div>
                            <div class="swiper-slide">
                               <div class="text-center">
                                    <div class="hdTag">Gross Profit %</div>
                                    <div class="value clPrim">
                                        <span id="grsPrPer"></span><span>%</span><span class="sIcon"><i class="fa fa-check"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month -0.35 M ( +135%)</div>
                                </div>
                             </div>
                            <div class="swiper-slide">
                               <div class="text-center">
                                    <div class="hdTag">Other Incomes</div>
                                    <div class="value clPrim">
                                        <span id="otherIncomes" title=""></span><span class="sIcon"><i class="fa fa-check"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month 23%</div>
                                </div>
                             </div>
                            <div class="swiper-slide">
                               <div class="text-center">
                                    <div class="hdTag">Indirect Expenses</div>
                                    <div class="value dataColor1">
                                         <span id="otherExpenses" title=""></span><span class="sIcon"><i class="fa fa-check"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month 23%</div>
                                </div>
                             </div>
                            <div class="swiper-slide">
                               <div class="text-center">
                                    <div class="hdTag">Net Profit</div>
                                    <div class="value clPrim">
                                        <span id="netProfit" title=""></span><span class="sIcon"><i class="fa fa-check"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month 23%</div>
                                </div>
                             </div>
                            <div class="swiper-slide">
                               <div class="text-center">
                                    <div class="hdTag">Net Profit %</div>
                                    <div class="value clPrim">
                                        <span id="netPrPer"></span><span>%</span><span class="sIcon"><i class="fa fa-check"></i></span>
                                    </div>
                                    <div class="smallFont">Last Month 23%</div>
                                </div>
                             </div>
                        </div>
                    </div>
            </div>  
        </div>
    </div>  
    <div class=" popi">
        <div class="col-md-12 "> 
          <div class="cardContainer "> 
              
              <div class="cardHeader relative">Operating Profit over Time <i class="fa fa-arrows-alt fullmake" style="position:absolute;right:30px;cursor:pointer"></i></div> 
            <div id="chartdiv"></div> 
          </div>
        </div>
        <div class="col-md-12" style="margin-top:15px"> 
            <div class="cardContainer "> 
                
                <div class="cardHeader relative">Net Profit over Time <i class="fa fa-arrows-alt fullmake" style="position:absolute;right:30px;cursor:pointer"></i></div> 
                <div id="chartdiv2"></div> 
            </div>
        </div>
    </div>
    <div style="clear:both"></div>
      <div class="space"></div>
        <div class="popi">
        <div class="col-md-6">
            <div class="cardContainer">
                <div class="cardHeader">Assets And Liabilities</div>
                <div class="row" style="margin-bottom: 13px;">
                    <div class="col-sm-9">
                      <div class="d-flex justify-content-between col1">
                          <div>
                            <div id="Total_Asset" class="bigValue">0</div>
                            <div class="textS cll">Total Assets</div>
                            <div style="padding:5px"></div>
                            <div id="Cashbank_Amount" class="bigValue">0</div>
                            <div class="textS cll">Cash and Bank</div>
                          </div>
                          <div>
                              <div id="Current_Asset" class="bigValue">0</div>
                              <div class="textS cll">Current Assets</div>
                              <div style="padding:5px"></div>
                              <div id="Deposit" class="bigValue">0</div>
                              <div class="textS cll">Deposit,  adv and...</div>
                          </div>
                          <div>
                              <div id="Fixed_Asset" class="bigValue">0</div>
                              <div class="textS cll">Fixed Assets</div>
                              <div style="padding:5px"></div>
                              <div id="Closing_Stock" class="bigValue">0</div>
                              <div class="textS cll">Inventory</div>
                          </div>
                      </div>
                      
                    </div>
                    <div class="col-sm-3">
                      <div class="col1Revert">
                        <div id="TOTAL_REC" class="bigValue">0</div>
                        <div class="textS">Trade Receivables</div>
                      </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-9">
                      <div class="d-flex justify-content-between col2">
                          <div>
                            <div id="LIABILITY" class="bigValue">0</div>
                            <div class="textS cll">Liabilities & Equity</div>
                              <div style="padding:5px"></div>
                              <div id="EQUITY" class="bigValue">0</div>
                            <div class="textS cll">Equity</div>
                          </div>
                          <div>
                              <div id="CUR_LIABILITY" class="bigValue">0</div>
                              <div class="textS cll">Current Liability</div>
                              <div style="padding:5px"></div>
                              <div id="PROVISION" class="bigValue">0</div>
                              <div class="textS cll">Prov. & Accruals </div>
                          </div>
                          <div>
                              <div id="NON_CUR_LIABILITY" class="bigValue">0</div>
                              <div class="textS cll">Non-Cur Liability</div>
                              <div style="padding:5px"></div>
                              <div id="RELATED_PARTY" class="bigValue">0</div>
                              <div class="textS cll">Related Party Pybl</div>
                          </div>
                      </div>
                      
                    </div>
                    <div class="col-sm-3">
                      <div class="col2Revert">
                        <div id="TOTAL_PAY" class="bigValue">0</div>
                        <div class="textS">Trade Payables</div>
                      </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="cardContainer">
                <div class="cardHeader relative">KPI Funnel <i class="fa fa-arrows-alt fullmake" style="position:absolute;right:30px;cursor:pointer"></i></div>
                <div>
                    <div id="funnelCahrt"></div>
                </div>
            </div>
        </div>
    </div>
    </div>

        <script src="..\js\amChart\core.js"></script>
  <script src="..\js\amChart\charts.js"></script>
  <script src="..\js\amChart\maps.js"></script>
  <script src="..\js\amChart\animated.js"></script>
  <script type="text/javascript">
      function funnelgenerate(arrY){
          var sortedA = arrY.sort()
          am4core.ready(function () {
              // Themes begin
              am4core.useTheme(am4themes_animated);
              // Themes end
              var chart = am4core.create("funnelCahrt", am4charts.SlicedChart);
              chart.data = sortedA;
              var series = chart.series.push(new am4charts.FunnelSeries());
              series.dataFields.value = "value";
              series.dataFields.category = "name";
              series.alignLabels = true;
          }); // end am4core.ready()
      };
      
      
  </script>
<script type="text/javascript">

    
</script>
    </form>
</body>
</html>
