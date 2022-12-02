<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRMDb.aspx.cs" Inherits="DashBoard.DashBoard.CRM.CRMDb" %>

<%@ Register Src="~/DashBoard/CRM/UserControl/PhoneCallCount.ascx" TagPrefix="ucPhCall" TagName="phCall" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/SalesVisitCount.ascx" TagPrefix="ucSv" TagName="Sv" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/TotEntries.ascx" TagPrefix="uctotAct" TagName="totAct" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/PendingAct.ascx" TagPrefix="ucpAct" TagName="pAct" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/SmenOrdCount.ascx" TagPrefix="ucOrderCnt" TagName="OrderCnt" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/QuoteCount.ascx" TagPrefix="ucQuoteCount" TagName="QuoteCnt" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/efficiencyratio.ascx" TagPrefix="ucEF" TagName="EFi" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/ActivityHistory.ascx" TagPrefix="ucAh" TagName="Ah" %>
<%@ Register Src="~/DashBoard/CRM/UserControl/NewVsRep.ascx" TagPrefix="ucNr" TagName="Nr" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CRM Analytics</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />
    <%--<link href="../css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />--%>
    <meta name="google" content="notranslate" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />


    <link href="../css/SearchPopup.css" rel="stylesheet" />
    <script src="../Js/jquery.3.3.1.js"></script>
   
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />

    <script src="../Js/amC3/amcharts.js"></script>
    <script src="../Js/amC3/serial.js"></script>
    <script src="../Js/amC3/pie.js"></script>
    <script src="../Js/amC3/export.min.js"></script>
    <link href="../Js/amC3/export.css" rel="stylesheet" />
    <script src="../Js/amC3/light.js"></script>
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>

    <script>
function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>
     
    <style>
       #EfModel .modal-dialog {
            width: 100%;
        }
        #pendActModel .modal-dialog {
            width: 100%;
        }
        #CustomNewNotify {
            visibility: hidden;
            min-width: 250px;
            margin-left: -125px;
            background-color: #54749d;
            color: #fff;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            left: 50%;
            top: 230px;
            font-size: 17px;
        }

        .amcharts-chart-div > svg + a {
            display: none !important;
        }

        #CustomNewNotify.show {
            visibility: visible;
            -webkit-animation: fadein 0.5s, fadeout 0.5s 5.5s;
            animation: fadein 0.5s, fadeout 0.5s 5.5s;
        }

        @-webkit-keyframes fadein {
            from {
                top: 0;
                opacity: 0;
            }

            to {
                top: 230px;
                opacity: 1;
            }
        }

        @keyframes fadein {
            from {
                top: 0;
                opacity: 0;
            }

            to {
                top: 230px;
                opacity: 1;
            }
        }

        @-webkit-keyframes fadeout {
            from {
                top: 230px;
                opacity: 1;
            }

            to {
                top: 0;
                opacity: 0;
            }
        }

        @keyframes fadeout {
            from {
                top: 230px;
                opacity: 1;
            }

            to {
                top: 0;
                opacity: 0;
            }
        }
    </style>

    <style>
        
        .popover-content {
            color: #333;
        }

        .popover-content {
            width: 450px !important;
            color: #4869a7;
            font-size: 12px;
        }

        .popover {
            max-width: 450px !important;
        }

        .hdBoder {
            border-bottom: 1px solid #848792;
            padding-bottom: 10px;
            margin-bottom: 15px;
        }

        .colWraper {
            padding: 5px 5px 5px 5px;
            border: 1px solid #2c4182;
            margin-bottom: 5px;
            background: #efefef;
            min-height: 428px; /*405px;*/
        }

        .mcus {
            margin: 12px 0;
        }

            .mcus > tbody > tr > td {
                padding-bottom: 5px;
                padding-right: 15px;
            }

        .diverh {
            font-size: 14px;
            background: #54749d;
            color: #fff;
            padding: 5px 10px;
            margin-bottom: 1px;
        }

        .white, .white:hover, .white:focus, .white:active {
            color: #fff;
            font-size: 30px;
        }

        .pad5 {
            padding: 5px;
        }

        .trSpan {
            transform: translateY(-5px);
            position: relative;
            display: block;
        }

        .panelClass {
            margin-bottom: 31px;
            top: 50px;
            display: none;
            opacity: 0;
        }

        .serverMsg {
            color: maroon;
            font-size: 22px;
        }

        .firstCell {
            text-align: center;
            padding: 0;
        }

        .besidepart {
            display: inline-block;
            margin-left: 10px;
        }

        .excelIco {
            height: 25px;
            margin-top: -8px;
        }

       

        .zoomImg {
            transition: transform .2s;
            margin: 0 auto;
            cursor: pointer;
            padding: 0 !important;
            border: 1px solid #e9e9e9;
        }

        .zoomImg:hover {
            -ms-transform: scale(1.02);
            -webkit-transform: scale(0.5);
            transform: scale(1.05);
            z-index: 9999;
            animation: 5s shake infinite alternate;
            border: 1px solid #2d2626;
        }



        .HeaderStyleCRM {
            color: #111e4e;
            font-size: 30px;
        }

        .scroller {
            max-height: 82vh;
            overflow-y: auto;
            overflow-x: hidden;
        }

        ::-webkit-scrollbar {
            width: 5px; /* for vertical scrollbars */
        }

        ::-webkit-scrollbar-track {
            background: rgba(0, 0, 0, 0.1);
        }

        ::-webkit-scrollbar-thumb {
            background: rgba(0, 0, 0, 0.5);
        }


        .arrowPointer {
        color:green;
            padding-left: 5px;
         /*animation: ArrowleftRight 5s infinite;*/ 
        }


        @-webkit-keyframes ArrowleftRight {
    0%   {transform:translateX(10px)}
    25%  {transform:translateX(0px)}
    50%  {transform:translateX(10px)}
    75%  {transform:translateX(0px)}
    100% {transform:translateX(10px)}
}

        @keyframes ArrowleftRight {
            0% {
                transform: translateX(10px);
            }

            25% {
                transform: translateX(0px);
            }

            50% {
                transform: translateX(10px);
            }

            75% {
                transform: translateX(0px);
            }

            100% {
                transform: translateX(10px);
            }
        }
    .scroller{
        padding-bottom:10px
    }
.scroller>div>.col-md-12 {
    display:flex;
    align-items: center;
    justify-content: center;
}
.scroller>div>.col-md-12:after{
    content: '';
    border-top: 8px solid transparent;
    border-bottom: 8px solid transparent;
    border-left: 9px solid #28303c;
    position: absolute;
    right: -9px;
    visibility:hidden;
}
.scroller>div>.col-md-12.DisableClass:after {
    visibility:visible;
}


    </style>
    <script>

        $(document).ready(function () {
            $('a .white').click(function (e) {
                e.preventDefault();
            });

            $('[data-toggle="tooltip"]').tooltip();
            $('[data-toggle="popover"]').popover({ html: true });


            $('.zoom').click(function (e) {
                $('.arrowPointer').remove();
                $('.zoom').removeClass('DisableClass');
                var divid = $(this).attr('data-click');
                this.className = this.className + ' DisableClass';
                $('.panelClass').hide();
                $('.panelClass').css({ top: '50px', opacity: '0' });
                $('#' + divid).show();
                $('#' + divid).animate({ top: '0px', opacity: '1' });

                this.children[0].children[0].innerHTML = this.children[0].children[0].innerHTML + '<i class="far fa-hand-point-left arrowPointer"></i>';



                if (divid == "CallDiv") {
                    cgridPhone.Refresh();
                } else if (divid == "SVDiv") {
                    cgridSV.Refresh();
                } else if (divid == "totEntDiv") {
                    cgridTE.Refresh();
                } else if (divid == "pendingActDiv") {
                    cgridpact.Refresh();
                } else if (divid == "OrderCntdiv") {
                    cgridOCount.Refresh();
                } else if (divid == "QuoteCountdiv") {
                    cgridQt.Refresh();
                } else if (divid == "EFDiv") {
                    cgridEF.Refresh();
                }



            });

            var swiper = new Swiper('.swiper-container', {
                slidesPerView: 'auto',
                centeredSlides: false,
                spaceBetween: 10,
                navigation: {
                    nextEl: '.snavNext',
                    prevEl: '.snavPrev',
                }
            });
            $('#CallDiv').show().animate({ top: '0px', opacity: '1' });
        });


        function showCustomNewNotify(text) {
            var x = document.getElementById("CustomNewNotify");
            x.innerHTML = text;
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 6000);
        }


    </script>
    <link href="../css/dashboard.css" rel="stylesheet" />
    <style>
    .swiper-container {
      width: 97%;
      height: 100%;
    }
    .swiper-slide {
      text-align: center;
      /* Center slide text vertically */
      display: -webkit-box;
      display: -ms-flexbox;
      display: -webkit-flex;
      display: flex;
      -webkit-box-pack: center;
      -ms-flex-pack: center;
      -webkit-justify-content: center;
      justify-content: center;
      -webkit-box-align: center;
      -ms-flex-align: center;
      -webkit-align-items: center;
      align-items: center;
      width: auto;
    }
  </style>

</head>
<body class="bodyBg">
    <form id="form1" runat="server">
        <script src="../Js/moment.min.js"></script>
        <script src="../Js/SearchPopup.js"></script>





        <div class"clearfix">
            <div class="col-md-12 clearfix padding bdBot hdBoder">
                <h3 class="pull-left fontPop">CRM Analytics</h3>
                <span class="pull-right"><a href="#" onclick="reloadParent()" class="pageClose"><i class="fa fa-times"></i></a></span>
            </div>
        </div>

        <div id="CustomNewNotify">Some text some message..</div>



       <div class="clearfix ">
          <div class="col-md-12 relative">
            <div class="swNav snavPrev">
                <i class="fa fa-arrow-left"></i>
            </div>
           
            <div class="middl">
                <div class="swiper-container vTabWrap fontPop ">
                <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class=" zoom DisableClass" runat="server" id="CallDivbtn" data-click="CallDiv">
                    <div class="wdgLabel">Call Log</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="SVDivbtn"  data-click="SVDiv"  >
                        <div class="wdgLabel">Visit Log</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="totEntDivbtn"   data-click="totEntDiv"  >
                        <div class="wdgLabel">Activities Salesmen Wise</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="pendingActDivbtn"   data-click="pendingActDiv" >
                    <div class="wdgLabel">Pending Activities</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="OrderCntdivbtn"  data-click="OrderCntdiv"   >
                    <div  class="wdgLabel">Order Count Salesmen Wise
                    </div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="QuoteCountdivbtn" data-click="QuoteCountdiv">
                    <div  class="wdgLabel">Proforma/Quotation Salesman Wise
                    </div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="EFDivbtn" data-click="EFDiv">
                    <div class="wdgLabel">Efficiency Ratio  Salesmen Wise
                    </div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="AhDivbtn"   data-click="AhDiv">
                    <div class="wdgLabel">Activity History
                    </div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom" runat="server" id="nrDivbtn"  data-click="nrDiv" >
                    <div  class="wdgLabel">New Vs Repeat Sale
                    </div>
                    </div>
                </div>
            </div>
                </div>
            </div>
           
           <div class="swNav snavNext"><i class="fa fa-arrow-right"></i></div>
          </div>

        <div class="col-md-12" style="padding-top:15px;">
            <div class="col-md-12 panelClass " runat="server" id="CallDiv" style="">
                <ucPhCall:phCall runat="server" ID="phCall" class="mainpanel" />
            </div>
            <div class="col-md-12 panelClass" runat="server" id="SVDiv" >
                    <ucSv:Sv runat="server" ID="Sv" /> 
            </div>

            <div class="col-md-12 panelClass" runat="server" id="totEntDiv" >
                    <uctotAct:totAct runat="server" ID="totEnt" />
            </div>

            <div class="col-md-12 panelClass" runat="server" id="pendingActDiv" >
                <ucpAct:pAct runat="server" ID="pAct" />
            </div>

            <div class="col-md-12 panelClass" runat="server" id="OrderCntdiv" >
                <ucOrderCnt:OrderCnt runat="server" ID="OrderCnt" />
            </div>

            <div class="col-md-12 panelClass" runat="server" id="QuoteCountdiv" >
                <ucQuoteCount:QuoteCnt runat="server" ID="QuoteCount" />
            </div>

            <div class="col-md-12 panelClass" runat="server" id="EFDiv" >
                <ucEF:EFi runat="server" ID="ef" />
            </div>

            <div class="col-md-12 panelClass" runat="server" id="AhDiv" >
                <ucAh:Ah runat="server" ID="ahid" />
            </div>

            <div class="col-md-12 panelClass" runat="server" id="nrDiv" >
                <ucNr:Nr runat="server" ID="nrid" />
            </div>
        </div>
</div>
    </form>
</body>
</html>
