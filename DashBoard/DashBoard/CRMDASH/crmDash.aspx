<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="crmDash.aspx.cs" Inherits="DashBoard.DashBoard.CRMDASH.crmDash" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CRM Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,400i,500,600,700,800,900&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />

     
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="crmDash.css?1.0.1" rel="stylesheet" />
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>
    <script type="text/javascript">
    function showAll() {
        
        $.ajax({
            type: "POST",
            url: "crmDash.aspx/GetReportsLink",
            contentType: "application/json; charset=utf-8",
            
            dataType: "json",
            success: function (data) {
                var mainDta = data.d;
                console.log(data)
                // watchlistDataLoop(data.d);
                $('#lblTotal').text(mainDta.TotalLeads);
                $('#lblQl').text(mainDta.QualifiedL);
                $('#lblOpen').text(mainDta.openL);
                $('#lblLost').text(mainDta.LostL);
            }
        });
    }
    showAll();
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
            autoplay:false
        });
        var tabfirst =$('.abc').find('.swiper-slide').first().find('.zoom').data('click')
       
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
    function getActivities() {
        var asDate = new Date().toJSON().slice(0,10).replace(/-/g,'-');
        var dt = {};
        dt.ASONDATE = asDate;
        dt.BRANCH_ID = "";
        dt.EMPID = "";
        $.ajax({
            type: "POST",
            url: "../KPISummary/kpisummary.aspx/GetActivitiesBox",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dt),
            dataType: "json",
            success: function (data) {
                console.log('GetActivitiesBox', data);
                var Data = data.d;
                //act
                if (Data[0].ACTIVCNT == "" || Data[0].ACTIVCNT == null) {
                    $('#actValue').text("0");
                } else {
                    $('#actValue').text(Data[0].ACTIVCNT);
                }
                //email
                if (Data[0].EMAILCNT == "" || Data[0].EMAILCNT == null) {
                    $('#emailValue').text("0");
                } else {
                    $('#emailValue').text(Data[0].EMAILCNT);
                }
                //call
                if (Data[0].CALLSMSCNT == "" || Data[0].CALLSMSCNT == null) {
                    $('#callsmsValue').text("0");
                } else {
                    $('#callsmsValue').text(Data[0].CALLSMSCNT);
                }
                //visitValue
                if (Data[0].VISITCNT == "" || Data[0].VISITCNT == null) {
                    $('#visitValue').text("0");
                } else {
                    $('#visitValue').text(Data[0].VISITCNT);
                }
                //socialValue
                if (Data[0].SOCIALCNT == "" || Data[0].SOCIALCNT == null) {
                    $('#socialValue').text("0");
                } else {
                    $('#socialValue').text(Data[0].SOCIALCNT);
                }
                //otherValue
                if (Data[0].OTHERSCNT == "" || Data[0].OTHERSCNT == null) {
                    $('#otherValue').text("0");
                } else {
                    $('#otherValue').text(Data[0].OTHERSCNT);
                }
            }
        });
    };
    getActivities();
</script>
   
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboardWraper">
            
            <div class="clearfix">
                <div class="col-md-12 clearfix padding  " style="padding: 10px;">
                    <h3 class="pull-left HeaderStyleCRM fontPop">CUSTOMER RELATIONSHIP MANAGEMENT (CRM)</h3>
                     <span class="pull-right closeBtn" style="z-index:999">  <a href="#" onclick="reloadParent()" ><i class="fa fa-times"></i></a></span>
                </div>
            </div>
            <div class="form_main">
                <div class="tabSlideContainer relative">
                    <div class="swNav snavPrev">
                        <i class="fa fa-arrow-left"></i>
                    </div>
                    <div class="swNav snavNext"><i class="fa fa-arrow-right"></i></div>
                
                    <div class="swiper-container vTabWrap fontPop ">
                    <div class="swiper-wrapper abc">
                        <div class="swiper-slide" id="DivActivities" runat="server">
                            <div class="widget c1 zoom DisableClass" data-click="fTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Activities</div>
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivCampaigns" runat="server">
                            <div class="widget c2 zoom" data-click="STab">
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Campaigns</div>
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivLeads" runat="server">
                            <div class="widget c3 zoom" data-click="tTab">
                               
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Leads</div>
                                    
                                </div>
                                 <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivOpportunities" runat="server">
                            <div class="widget c4 zoom" data-click="frTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Opportunities</div>
                                   
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivAccounts" runat="server">
                            <div class="widget c5 zoom" data-click="fvTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Accounts</div>
                                </div>
                                <div class="iconBox " style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivContacts" runat="server">
                            <div class="widget c6 zoom" data-click="sxTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Contacts</div>
                                    
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivCases" runat="server">
                            <div class="widget c7 zoom" data-click="seTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Cases</div>
                                    
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivServices" runat="server">
                            <div class="widget c8 zoom" data-click="etTab">
                               
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Services</div>
                                </div>
                                 <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="DivCollateral" runat="server">
                            <div class="widget c2 zoom" data-click="enTab">
                               
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Collateral</div>
                                </div>
                                 <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
                <div class="container-fluid">
                    <div class="panelClass" id="fTab" runat="server">
                        <div class="backgroundedBoxes">
                            
                            <div class="clearfix col-md-12">
                              <div class="flex-row space-between align-items-center">
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/online-activity.png" style="width:40px;margin-bottom: 12px;">
                                          </div>
                                          <div class="valRound semiRound">
                                              <span id="actValue">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Activities</div>
                                         
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/gmail.png" style="width:40px;margin-bottom: 12px;">
                                          </div>
                                          <div class="valRound semiRound c1">
                                              <span id="emailValue">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Emails</div>
                                         
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/smartphone.png" style="width:40px;margin-bottom: 12px;">
                                          </div>
                                          <div class="valRound semiRound c2">
                                              <span id="callsmsValue">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Call/SMS</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/visitor.png" style="width:40px;margin-bottom: 12px;">
                                          </div>
                                          <div class="valRound semiRound c3">
                                              <span id="visitValue">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Visits</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/account.png" style="width:40px;margin-bottom: 12px;">
                                          </div>
                                          <div class="valRound semiRound c4">
                                              <span id="socialValue">0</span>
                                          </div>
                                          <div class="smallmuted ">Total</div>
                                          <div class="hdTag">Social</div>
                                          
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative">
                                      <div class="">
                                          <div class="text-center">
                                              <img src="../images/icons/clipboards.png" style="width:40px;margin-bottom: 12px;">
                                          </div>
                                          <div class="valRound semiRound c5">
                                              <span id="otherValue">0</span>
                                          </div>
                                          <div class="smallmuted">Total</div>
                                          <div class="hdTag">Others</div>
                                          
                                      </div>
                                  </div>
                              </div>
                          </div>
                        </div>
                        <%--charts--%>
                        <div class="chartContainer hide">
                             <div class="row">
                                  <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Activities Today</div>
                                        <div class="mainAreachart">
                                            <div id="totalActivityChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--second chart--%>
                                 <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Due Today</div>
                                        <div class="mainAreachart">
                                            <div id="dueTodayChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--third chart--%>
                                 <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Completed Today</div>
                                        <div class="mainAreachart">
                                            <div id="completedtodayChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--Fourth chart--%>
                                 <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Pending Today</div>
                                        <div class="mainAreachart">
                                            <div id="pendingTodayChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                             </div>
                        </div>
                    </div>
                    <div class="panelClass" id="STab" runat="server">
                        <div class="chartContainer">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Campaign by Cost
                                            
                                        </div>
                                        <div class="mainAreachart">
                                            <div id="chartdiv2" style="height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Campaign by Revenue
                                            
                                        </div>
                                        <div class="mainAreachart">
                                            <div id="chartdivRev2" style="height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                              <div class="row" style="margin-top:15px">
                                <div class="col-sm-6">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Campaign by Types
                                            
                                        </div>
                                        <div class="mainAreachart">
                                            <div id="campChart3" style="height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Campaign by Status
                                            
                                        </div>
                                        <div class="mainAreachart">
                                            <div id="campChart4" style="height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-top:15px">
                                <div class="col-sm-12">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Cost of Campaign vs Estemated Revenue
                                            
                                        </div>
                                        <div class="mainAreachart">
                                            <div id="campChart5" style="height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panelClass" id="tTab" runat="server">
                        <div class="backgroundedBoxes">
                            <div class="d-flex justify-content-center mainDashBoxes">
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c2" id="a4" >
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/TotalLead.png" /></div>
                                            <div class="flex-grow-1 txt"> Total Lead(s)</div>
                                        </div>
                                        <div class="Numb" id="lblTotal">0</div>
                                        <div class="text-center">As on Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                        
                        
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c4" id="" >
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/OpenLead.png" /></div>
                                            <div class="flex-grow-1 txt"> Open Lead(s)</div>
                                        </div>
                                        <div class="Numb" id="lblOpen">0</div>
                                        <div class="text-center">As on Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox " id="a2">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/QaLead.png" /></div>
                                            <div class="flex-grow-1 txt"> Qualified Lead(s)</div>
                                        </div>
                                        <div class="Numb" id="lblQl">0</div>
                                        <div class="text-center">As on Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr hide" data-scroll="accordion">
                                    <div class="widgBox c3" id="a1" >
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/ConvertedLead.png" /></div>
                                            <div class="flex-grow-1 txt"> Converted & Closed</div>
                                        </div>
                                        <div class="Numb" id="">0</div>
                                        <div class="text-center">As on Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c6" id="a1" >
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/LostLead.png" /></div>
                                            <div class="flex-grow-1 txt"> Lost & Closed</div>
                                        </div>
                                        <div class="Numb" id="lblLost">0</div>
                                        <div class="text-center">As on Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                           </div>
                        </div>
                        <div class="chartContainer">
                            <div class="row">
                                
                                <div class="col-sm-6">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Leads by Source Campaign</div>
                                        <div class="mainAreachart">
                                            <div id="chartdiv3" style="height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="shadowBox text-left">
                                        <div class="bigHeading">Leads by Industry (Top 10)</div>
                                        <div class="mainAreachart">
                                            <div id="chartdiv4" style="min-height: 300px"></div>
                                        </div>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <div class="chartContainer">
                             <div class="row">
                                  <div class="col-sm-4">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Leads by Rating</div>
                                        <div class="mainAreachart">
                                            <div id="lead1Chart" style="height: 300px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--second chart--%>
                                 <div class="col-sm-4">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Leads by Amount</div>
                                        <div class="mainAreachart">
                                            <div id="lead2Chart" style="height: 300px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--third chart--%>
                                 <div class="col-sm-4">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading">Leads by Stage Amount</div>
                                        <div class="mainAreachart">
                                            <div id="lead3Chart" style="height: 300px"></div>
                                        </div>
                                    </div>
                                  </div>
                               
                             </div>
                        </div>
                    </div>
                    <div class="panelClass" id="frTab" runat="server">
                        <div class="chartContainer">
                            <div class="row mbot100" style="margin-top: 50px">
                            
                                    
                                    <div class="col-sm-6">
                                        <div class="shadowBox text-left">
                                            <div class="bigHeading">Opportunity</div>
                                            <div class="mainAreachart">
                                                <div id="chartdiv" style="height: 450px"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                    </div>
                    <div class="panelClass" id="fvTab" runat="server">fjgfj</div>
                    <div class="panelClass" id="sxTab" runat="server">fjgjf</div>
                    <div class="panelClass" id="seTab" runat="server">fgjfgj</div>
                    <div class="panelClass" id="etTab" runat="server">fjfgj</div>
                    <div class="panelClass" id="enTab" runat="server">fjfgj</div>
                

            </div>
            </div>
        </div>

        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://www.amcharts.com/lib/4/core.js"></script>
        <script src="https://www.amcharts.com/lib/4/charts.js"></script>
        <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>
        <script>
            $.ajax({
                type: "POST",
                url: "crmDash.aspx/GetLeadbyIndustry",
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                success: function (data) {
                    var mainDta = data.d;
                    console.log(data)
                    am4core.ready(function () {

                        // Themes begin
                        am4core.useTheme(am4themes_animated);
                        // Themes end

                        // Create chart
                        var chart4 = am4core.create("chartdiv4", am4charts.PieChart);
                        chart4.hiddenState.properties.opacity = 0; // this creates initial fade-in

                        chart4.data = data.d;

                        var series = chart4.series.push(new am4charts.PieSeries());
                        series.dataFields.value = "IndutryCost";
                        series.ticks.template.disabled = true;
                        series.labels.template.fill = am4core.color("white");
                        //chart4.legend.maxHeight = 150;
                        //chart4.legend.scrollable = true;

                        series.dataFields.category = "IndutryName";
                        series.slices.template.cornerRadius = 2;
                        series.colors.step = 3;

                        series.hiddenState.properties.endAngle = -90;

                        chart4.legend = new am4charts.Legend();
                        chart4.legend.position = "right";
                    }); // end am4core.ready()
                }
            });

            

            $.ajax({
                type: "POST",
                url: "crmDash.aspx/GetCampaignCost",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var mainDta = data.d;
                    //console.log(data)
                    // watchlistDataLoop(data.d);
                    am4core.ready(function () {

                        // Themes begin
                        am4core.useTheme(am4themes_animated);
                        // Themes end

                        // Create chart instance
                        var chart = am4core.create("chartdiv2", am4charts.XYChart);

                        // Add data
                        chart.data = data.d;

                        // Create axes
                        var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                        yAxis.dataFields.category = "CampaignName";
                        yAxis.renderer.grid.template.location = 0;
                        yAxis.renderer.labels.template.fontSize = 10;
                        yAxis.renderer.minGridDistance = 10;

                        var xAxis = chart.xAxes.push(new am4charts.ValueAxis());

                        // Create series
                        var series = chart.series.push(new am4charts.ColumnSeries());
                        series.dataFields.valueX = "CampaignCost";
                        series.dataFields.categoryY = "CampaignName";
                        series.columns.template.tooltipText = " [bold]{valueX}[/]";
                        series.columns.template.strokeWidth = 0;


                        // Add ranges
                        function addRange(label, start, end, color) {
                            var range = yAxis.axisRanges.create();
                            range.category = start;
                            range.endCategory = end;
                            range.label.text = label;
                            range.label.disabled = false;
                            range.label.fill = color;
                            range.label.location = 0;
                            range.label.dx = -130;
                            range.label.dy = 12;
                            range.label.fontWeight = "bold";
                            range.label.fontSize = 12;
                            range.label.horizontalCenter = "left"
                            range.label.inside = true;

                            range.grid.stroke = am4core.color("#396478");
                            range.grid.strokeOpacity = 1;
                            range.tick.length = 200;
                            range.tick.disabled = false;
                            range.tick.strokeOpacity = 0.6;
                            range.tick.stroke = am4core.color("#396478");
                            range.tick.location = 0;

                            range.locations.category = 1;
                        }

                        chart.cursor = new am4charts.XYCursor();

                    });

                    am4core.ready(function () {

                        // Themes begin
                        am4core.useTheme(am4themes_animated);
                        // Themes end

                        // Create chart instance
                        var chart = am4core.create("chartdivRev2", am4charts.XYChart);

                        // Add data
                        chart.data = data.d;

                        // Create axes
                        var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                        yAxis.dataFields.category = "CampaignName";
                        yAxis.renderer.grid.template.location = 0;
                        yAxis.renderer.labels.template.fontSize = 10;
                        yAxis.renderer.minGridDistance = 10;

                        var xAxis = chart.xAxes.push(new am4charts.ValueAxis());

                        // Create series
                        var series = chart.series.push(new am4charts.ColumnSeries());
                        series.dataFields.valueX = "CampaignCost";
                        series.dataFields.categoryY = "CampaignName";
                        series.columns.template.tooltipText = " [bold]{valueX}[/]";
                        series.columns.template.strokeWidth = 0;


                        // Add ranges
                        function addRange(label, start, end, color) {
                            var range = yAxis.axisRanges.create();
                            range.category = start;
                            range.endCategory = end;
                            range.label.text = label;
                            range.label.disabled = false;
                            range.label.fill = color;
                            range.label.location = 0;
                            range.label.dx = -130;
                            range.label.dy = 12;
                            range.label.fontWeight = "bold";
                            range.label.fontSize = 12;
                            range.label.horizontalCenter = "left"
                            range.label.inside = true;

                            range.grid.stroke = am4core.color("#396478");
                            range.grid.strokeOpacity = 1;
                            range.tick.length = 200;
                            range.tick.disabled = false;
                            range.tick.strokeOpacity = 0.6;
                            range.tick.stroke = am4core.color("#396478");
                            range.tick.location = 0;

                            range.locations.category = 1;
                        }

                        chart.cursor = new am4charts.XYCursor();

                    });
                }
            });
            // end am4core.ready()

            $.ajax({
                type: "POST",
                url: "crmDash.aspx/LeadbyCampaign",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var mainDta = data.d;
                    am4core.ready(function () {

                        // Themes begin
                        am4core.useTheme(am4themes_animated);
                        // Themes end

                        // Create chart instance
                        var chart3 = am4core.create("chartdiv3", am4charts.XYChart);

                        // Add data
                        chart3.data = data.d;

                        // Create axes
                        var yAxis = chart3.yAxes.push(new am4charts.CategoryAxis());
                        yAxis.dataFields.category = "Campaign_Nam";
                        yAxis.renderer.grid.template.location = 0;
                        yAxis.renderer.labels.template.fontSize = 10;
                        yAxis.renderer.minGridDistance = 10;

                        var xAxis = chart3.xAxes.push(new am4charts.ValueAxis());

                        // Create series
                        var series = chart3.series.push(new am4charts.ColumnSeries());
                        series.dataFields.valueX = "Campaigncnt";
                        series.dataFields.categoryY = "Campaign_Nam";
                        series.columns.template.tooltipText = " [bold]{valueX}[/]";
                        series.columns.template.strokeWidth = 0;


                        // Add ranges
                        function addRange(label, start, end, color) {
                            var range = yAxis.axisRanges.create();
                            range.category = start;
                            range.endCategory = end;
                            range.label.text = label;
                            range.label.disabled = false;
                            range.label.fill = color;
                            range.label.location = 0;
                            range.label.dx = -130;
                            range.label.dy = 12;
                            range.label.fontWeight = "bold";
                            range.label.fontSize = 12;
                            range.label.horizontalCenter = "left"
                            range.label.inside = true;

                            range.grid.stroke = am4core.color("#396478");
                            range.grid.strokeOpacity = 1;
                            range.tick.length = 200;
                            range.tick.disabled = false;
                            range.tick.strokeOpacity = 0.6;
                            range.tick.stroke = am4core.color("#396478");
                            range.tick.location = 0;

                            range.locations.category = 1;
                        }

                        chart3.cursor = new am4charts.XYCursor();

                    }); // end am4core.ready()
                }
            });

            
        </script>
        <script>
            $.ajax({
                type: "POST",
                url: "crmDash.aspx/getOpportunity",
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                success: function (data) {
                    var mainDta = data.d;
                    console.log(data)
                    am4core.useTheme(am4themes_animated);

                    var charta = am4core.create("chartdiv", am4charts.SlicedChart);
                    charta.data = mainDta;

                    let series = charta.series.push(new am4charts.PyramidSeries());
                    series.dataFields.value = "opportunityCount";
                    series.dataFields.category = "ratingName";
                    series.alignLabels = true;
                    series.topWidth = am4core.percent(100);
                    series.bottomWidth = am4core.percent(0);
                    series.colors.step = 7;
                    charta.colors.list = [
                        am4core.color("#845EC2"),
                        am4core.color("#D65DB1"),
                        am4core.color("#FF6F91"),
                        am4core.color("#FF9671"),
                        am4core.color("#FFC75F"),
                        am4core.color("#F9F871")
                    ];
                    charta.legend = new am4charts.Legend();
                    charta.legend.position = "bottom";
                }
            });
            // 
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("totalActivityChart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Agriculture", "size": 6.6 },
                  { "sector": "Mining and Quarrying", "size": 0.6 },
                  { "sector": "Manufacturing", "size": 23.2 },
                  { "sector": "Electricity and Water", "size": 2.2 },
                  { "sector": "Construction", "size": 4.5 },
                  { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
                  { "sector": "Transport and Communication", "size": 9.3 },
                  { "sector": "Finance, real estate and business services", "size": 22.5 }
                ];

                // Add label
                chart.innerRadius = 60;
                var label = chart.seriesContainer.createChild(am4core.Label);
                label.text = "28";
                label.horizontalCenter = "middle";
                label.verticalCenter = "middle";
                label.fontSize = 25;

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";
                //pieSeries.slices.template.stroke = am4core.color("#fff");
                //pieSeries.slices.template.strokeWidth = 2;
                //pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.ticks.template.disabled = true;
                pieSeries.labels.template.disabled = true;

                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover");
                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;


                //var colorSet = new am4core.ColorSet();
                //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
                //    return new am4core.color(color);
                //});
                //pieSeries.colors = colorSet;
            }); // end am4core.ready()
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("dueTodayChart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Agriculture", "size": 6.6 },
                  { "sector": "Mining and Quarrying", "size": 0.6 },
                  { "sector": "Manufacturing", "size": 23.2 },
                  { "sector": "Electricity and Water", "size": 2.2 },
                  { "sector": "Construction", "size": 4.5 },
                  { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
                  { "sector": "Transport and Communication", "size": 9.3 },
                  { "sector": "Finance, real estate and business services", "size": 22.5 }
                ];

                // Add label
                chart.innerRadius = 60;
                var label = chart.seriesContainer.createChild(am4core.Label);
                label.text = "28";
                label.horizontalCenter = "middle";
                label.verticalCenter = "middle";
                label.fontSize = 25;

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";
                //pieSeries.slices.template.stroke = am4core.color("#fff");
                //pieSeries.slices.template.strokeWidth = 2;
                //pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.ticks.template.disabled = true;
                pieSeries.labels.template.disabled = true;

                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover");
                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;


                //var colorSet = new am4core.ColorSet();
                //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
                //    return new am4core.color(color);
                //});
                //pieSeries.colors = colorSet;
            }); // end am4core.ready()
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("completedtodayChart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Agriculture", "size": 6.6 },
                  { "sector": "Mining and Quarrying", "size": 0.6 },
                  { "sector": "Manufacturing", "size": 23.2 },
                  { "sector": "Electricity and Water", "size": 2.2 },
                  { "sector": "Construction", "size": 4.5 },
                  { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
                  { "sector": "Transport and Communication", "size": 9.3 },
                  { "sector": "Finance, real estate and business services", "size": 22.5 }
                ];

                // Add label
                chart.innerRadius = 60;
                var label = chart.seriesContainer.createChild(am4core.Label);
                label.text = "28";
                label.horizontalCenter = "middle";
                label.verticalCenter = "middle";
                label.fontSize = 25;

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";
                //pieSeries.slices.template.stroke = am4core.color("#fff");
                //pieSeries.slices.template.strokeWidth = 2;
                //pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.ticks.template.disabled = true;
                pieSeries.labels.template.disabled = true;

                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover");
                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;


                //var colorSet = new am4core.ColorSet();
                //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
                //    return new am4core.color(color);
                //});
                //pieSeries.colors = colorSet;
            }); // end am4core.ready()

            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("pendingTodayChart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Agriculture", "size": 6.6 },
                  { "sector": "Mining and Quarrying", "size": 0.6 },
                  { "sector": "Manufacturing", "size": 23.2 },
                  { "sector": "Electricity and Water", "size": 2.2 },
                  { "sector": "Construction", "size": 4.5 },
                  { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
                  { "sector": "Transport and Communication", "size": 9.3 },
                  { "sector": "Finance, real estate and business services", "size": 22.5 }
                ];

                // Add label
                chart.innerRadius = 60;
                var label = chart.seriesContainer.createChild(am4core.Label);
                label.text = "28";
                label.horizontalCenter = "middle";
                label.verticalCenter = "middle";
                label.fontSize = 25;

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";
                //pieSeries.slices.template.stroke = am4core.color("#fff");
                //pieSeries.slices.template.strokeWidth = 2;
                //pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.ticks.template.disabled = true;
                pieSeries.labels.template.disabled = true;

                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover");
                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;


                //var colorSet = new am4core.ColorSet();
                //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
                //    return new am4core.color(color);
                //});
                //pieSeries.colors = colorSet;
            }); // end am4core.ready()

            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("lead1Chart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Hot", "size": 5 },
                  { "sector": "WARM", "size": 21 },
                  { "sector": "Cold", "size": 3 }
                ];

                // Add label
                chart.innerRadius = 0;
                var label = chart.seriesContainer.createChild(am4core.Label);
                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());

                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(0);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                
                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.labels.template.text = "";
                pieSeries.slices.template.tooltipText = "{category}: {value}";

                pieSeries.ticks.template.disabled = true;



                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();
                chart.legend.maxWidth = 100;
                pieSeries.legendSettings.valueText = "{ }";
                pieSeries.legendSettings.labelText = "{category}: {value}";
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];

                //var colorSet = new am4core.ColorSet();
                //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
                //    return new am4core.color(color);
                //});
                //pieSeries.colors = colorSet;
            }); // end am4core.ready()
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("lead2Chart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Hot", "size": 45000 },
                  { "sector": "WARM", "size": 54562 },
                  { "sector": "Cold", "size": 4582 }
                ];

                // Add label
               
                var label = chart.seriesContainer.createChild(am4core.Label);
                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());

                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(40);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.labels.template.text = "";
                pieSeries.slices.template.tooltipText = "{category}: {value}";

                pieSeries.ticks.template.disabled = true;



                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();
                chart.legend.labels.template.maxWidth = 60;
                chart.legend.contentWidth = 60;
                
                pieSeries.legendSettings.valueText = "{ }";
                pieSeries.legendSettings.labelText = "{category}: {value}";
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];

                //var colorSet = new am4core.ColorSet();
                //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
                //    return new am4core.color(color);
                //});
                //pieSeries.colors = colorSet;
            }); // end am4core.ready()
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("lead3Chart", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "New", "size": 45000 },
                  { "sector": "Qualified", "size": 54562 },
                  { "sector": "Close", "size": 4582 },
                  { "sector": "On Progress", "size": 522 }
                ];

                // Add label
                chart.innerRadius = 0;
                var label = chart.seriesContainer.createChild(am4core.Label);
                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());

                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(40);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.labels.template.text = "";
                pieSeries.slices.template.tooltipText = "{category}: {value}";

                pieSeries.ticks.template.disabled = true;



                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();
                chart.legend.maxWidth = 100;
                chart.legend.contentWidth = 80;
                pieSeries.legendSettings.valueText = "{ }";
                pieSeries.legendSettings.labelText = "{category}: {value}";
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];
            }); // end am4core.ready()

            // campChart3 
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("campChart3", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Advertisement", "size": 8 },
                  { "sector": "Co-Branding", "size": 12 },
                  { "sector": "Event", "size": 5 },
                  { "sector": "Others", "size": 9 }
                ];

                // Add label
                chart.innerRadius = 0;
                var label = chart.seriesContainer.createChild(am4core.Label);
                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());

                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(5);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.labels.template.text = "";
                pieSeries.slices.template.tooltipText = "{category}: {value}";

                pieSeries.ticks.template.disabled = true;



                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();
                chart.legend.maxWidth = 100;
                chart.legend.contentWidth = 80;
                pieSeries.legendSettings.valueText = "{ }";
                pieSeries.legendSettings.labelText = "{category}: {value}";
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];
            }); // end am4core.ready()

            // campChart4 
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                // Create chart instance
                var chart = am4core.create("campChart4", am4charts.PieChart);

                // Add data
                chart.data = [
                  { "sector": "Advertisement", "size": 8 },
                  { "sector": "Co-Branding", "size": 12 },
                  { "sector": "Event", "size": 5 },
                  { "sector": "Others", "size": 9 }
                ];

                // Add label
                chart.innerRadius = 0;
                var label = chart.seriesContainer.createChild(am4core.Label);
                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());

                pieSeries.dataFields.value = "size";
                pieSeries.dataFields.category = "sector";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(5);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;

                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.labels.template.text = "";
                pieSeries.slices.template.tooltipText = "{category}: {value}";

                pieSeries.ticks.template.disabled = true;



                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();
                chart.legend.maxWidth = 100;
                chart.legend.contentWidth = 80;
                pieSeries.legendSettings.valueText = "{ }";
                pieSeries.legendSettings.labelText = "{category}: {value}";
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];
            }); // end am4core.ready()

            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end



                var chart = am4core.create('campChart5', am4charts.XYChart)
                chart.colors.step = 2;

                chart.legend = new am4charts.Legend()
                chart.legend.position = 'top'
                chart.legend.paddingBottom = 20
                chart.legend.labels.template.maxWidth = 95

                var xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
                xAxis.dataFields.category = 'category'
                xAxis.renderer.cellStartLocation = 0.1
                xAxis.renderer.cellEndLocation = 0.9
                xAxis.renderer.grid.template.location = 0;

                var yAxis = chart.yAxes.push(new am4charts.ValueAxis());
                yAxis.min = 0;

                function createSeries(value, name) {
                    var series = chart.series.push(new am4charts.ColumnSeries())
                    series.dataFields.valueY = value
                    series.dataFields.categoryX = 'category'
                    series.name = name
                    //series.tooltipText = " {categoryX}: [bold]{valueY}[/]";
                    series.events.on("hidden", arrangeColumns);
                    series.events.on("shown", arrangeColumns);

                    var bullet = series.bullets.push(new am4charts.LabelBullet())
                    bullet.interactionsEnabled = false
                    bullet.dy = 30;
                    bullet.label.text = '{valueY}'
                    bullet.label.fill = am4core.color('#ffffff')

                    return series;
                }

                chart.data = [
                    {
                        category: 'Branding',
                        first: 40,
                        second: 55
                    },
                    {
                        category: 'Marketing',
                        first: 30,
                        second: 78
                    },
                    {
                        category: 'Events',
                        first: 27,
                        second: 40
                    },
                    {
                        category: 'Others',
                        first: 50,
                        second: 33
                    }
                ]


                createSeries('first', 'The Thirst');
                createSeries('second', 'The Second');
               

                function arrangeColumns() {

                    var series = chart.series.getIndex(0);

                    var w = 1 - xAxis.renderer.cellStartLocation - (1 - xAxis.renderer.cellEndLocation);
                    if (series.dataItems.length > 1) {
                        var x0 = xAxis.getX(series.dataItems.getIndex(0), "categoryX");
                        var x1 = xAxis.getX(series.dataItems.getIndex(1), "categoryX");
                        var delta = ((x1 - x0) / chart.series.length) * w;
                        if (am4core.isNumber(delta)) {
                            var middle = chart.series.length / 2;

                            var newIndex = 0;
                            chart.series.each(function (series) {
                                if (!series.isHidden && !series.isHiding) {
                                    series.dummyData = newIndex;
                                    newIndex++;
                                }
                                else {
                                    series.dummyData = chart.series.indexOf(series);
                                }
                            })
                            var visibleCount = newIndex;
                            var newMiddle = visibleCount / 2;

                            chart.series.each(function (series) {
                                var trueIndex = chart.series.indexOf(series);
                                var newIndex = series.dummyData;

                                var dx = (newIndex - trueIndex + middle - newMiddle) * delta

                                series.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                                series.bulletsContainer.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                            })
                        }
                    }
                }

            }); // end am4core.ready()
        </script>
    </form>
</body>
</html>
