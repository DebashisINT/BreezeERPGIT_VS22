<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectDB.aspx.cs" Inherits="DashBoard.DashBoard.ProjectDB.projectDB" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Dashboard</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />

     
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />

    <style>
        .mbot50 {
            margin-bottom:50px;
        }
        
        .jumbrton >h1 {
            text-transform: uppercase;
            color: #fff;
            font-size: 24px;
        }
        .shadowBox {
            padding: 15px 10px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 0 8px rgba(0,0,0,0.26);
            background: #fff;
        }
        .heading {
            font-size: 18px;
            text-transform: uppercase;
        }
        .shadowBox .amt {
            font-size: 22px;
            font-weight: 600;
            margin-top: 8px;
            display: inline-block;
            color: #2337ad;
        }
        .colorGreen {
            color: #23ad3d !important;
        }
        .colorSky {
            color: #199f9a !important;
        }
        .colorMegenta {
            color: #c47208 !important;
        }
        .revBoxSec {
            margin-top: -45px;
        }
        .cstCnt {
            padding: 25px 0;
        }
        .costing {
            margin-top: 50px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 0 8px rgba(0,0,0,0.26);
            background: #fff;
            overflow: hidden;
        }
        .costing-header {
            padding: 10px;
            background: #f2f2f2;
            color: #2337ad;
            text-transform: uppercase;
            font-size: 18px;
            margin-bottom: 15px;
        }
        .amtBadge {
            font-size: 20px;
            background: #d8ffe5;
            display: inline-block;
            padding: 5px 10px;
            color: #1a793a;
            border-radius: 4px;
            margin-bottom: 8px
        }
        .amtBadge.C1 {
            background: #ffe6e6;
            color: #964e4e;
        }
        .amtBadge.C2 {
            background: #d8ffe5;
            color: #31844b;
        }
        .amtBadge.C3 {
            background: #ddefff;
            color: #164a79;
        }
        .amtBadge.C4 {
            background: #e1ddff;
            color: #281e76;
        }
        .chartContainer {
            margin-top: 40px;
        }
        .bigHeading{
            font-size: 22px;
            text-transform: uppercase;
            padding: 10px 20px;
            color: #2337ad;
        }
        #chartdiv, #chartdiv2  {
            width: 100%;
            height: 500px;
        }
        .scroller {
           max-height:82vh;
           overflow-y:auto;
           overflow-x:hidden;
        }
        ::-webkit-scrollbar
        {
            width: 5px;  /* for vertical scrollbars */
            
        }

        ::-webkit-scrollbar-track
        {
            background: rgba(0, 0, 0, 0.1);
        }

        ::-webkit-scrollbar-thumb
        {
            background: rgba(0, 0, 0, 0.5);
        }
    </style>
    <style>
        .mbot50 {
            margin-bottom:50px;
        }
        .jumbrton {
            background: #323eda;
            text-align: center;
            padding: 21px 0;
        }
        .jumbrton >h1 {
            text-transform: uppercase;
            color: #fff;
            font-size: 24px;
        }
        .shadowBox {
            padding: 15px 10px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 0 8px rgba(0,0,0,0.26);
            background: #fff;
        }
        .heading {
            font-size: 18px;
            text-transform: uppercase;
        }
        .shadowBox .amt {
            font-size: 22px;
            font-weight: 600;
            margin-top: 8px;
            display: inline-block;
            color: #2337ad;
        }
        .colorGreen {
            color: #23ad3d !important;
        }
        .colorSky {
            color: #199f9a !important;
        }
        .colorMegenta {
            color: #c47208 !important;
        }
        .revBoxSec {
            margin-top: -45px;
        }
        .cstCnt {
            padding: 25px 0;
        }
        .costing {
            margin-top: 50px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 0 8px rgba(0,0,0,0.26);
            background: #fff;
            overflow: hidden;
        }
        .costing-header {
            padding: 10px;
            background: #f2f2f2;
            color: #2337ad;
            text-transform: uppercase;
            font-size: 18px;
            margin-bottom: 15px;
        }
        .amtBadge {
            font-size: 20px;
            background: #d8ffe5;
            display: inline-block;
            padding: 5px 10px;
            color: #1a793a;
            border-radius: 4px;
            margin-bottom: 8px
        }
        .amtBadge.C1 {
            background: #ffe6e6;
            color: #964e4e;
        }
        .amtBadge.C2 {
            background: #d8ffe5;
            color: #31844b;
        }
        .amtBadge.C3 {
            background: #ddefff;
            color: #164a79;
        }
        .amtBadge.C4 {
            background: #e1ddff;
            color: #281e76;
        }
        .chartContainer {
            margin-top: 40px;
        }
        .bigHeading{
            font-size: 22px;
            text-transform: uppercase;
            padding: 10px 20px;
            color: #2337ad;
        }
        #chartdiv, #chartdiv2  {
            width: 100%;
            height: 500px;
        }
        .scroller {
           max-height:82vh;
           overflow-y:auto;
           overflow-x:hidden;
        }
        ::-webkit-scrollbar
        {
            width: 5px;  /* for vertical scrollbars */
            
        }

        ::-webkit-scrollbar-track
        {
            background: rgba(0, 0, 0, 0.1);
        }

        ::-webkit-scrollbar-thumb
        {
            background: rgba(0, 0, 0, 0.5);
        }
    </style>
    <!-- Resources -->
    <style>
            .rightSelet {
                float: right;
                font-size: 15px !important;
            }
            .mbot100 {
                margin-bottom: 100px;
            }

            @-webkit-keyframes
                load { 0% {
                stroke-dashoffset:0
                }
            }
            @-moz-keyframes

            load { 0% {

            stroke-dashoffset:0

            }

            }

            @keyframes

            load { 0% {

            stroke-dashoffset:0

            }

            }

            .progress>li {

  display: inline-block;

  position: relative;

  text-align: center;

  color: rgb(92, 92, 92);

  font-family: Lato;

  font-weight: 100;

  margin: 2rem;

}

.progress>li:before {

  content: attr(data-name);

  position: absolute;

  width: 100%;

  bottom: -2rem;

  font-weight: 400;

}
.progress {
    height: auto !important;
    background-color: transparent;
    justify-content: center;
}
.progress>li:after {

  content: attr(data-percent);

  position: absolute;

  width: 100%;

  top: 3.7rem;

  left: 0;

  font-size: 2rem;

  text-align: center;

}

.progress svg {

  width: 10rem;

  height: 10rem;

}

 

.progress svg:nth-child(2) {

  position: absolute;

  left: 0;

  top: 0;

  transform: rotate(-90deg);

  -webkit-transform: rotate(-90deg);

  -moz-transform: rotate(-90deg);

  -ms-transform: rotate(-90deg);

}

 

.progress svg:nth-child(2) path {

  fill: none;

  stroke-width: 5;

  stroke-dasharray: 629;

  stroke: rgba(233, 233, 233, 0.9);

  -webkit-animation: load 10s;
  -moz-animation: load 10s;
  -o-animation: load 10s;
  animation: load 10s;

}

        </style>

    <style>
            .jumbrton , .jumbrton >h1, .jumbrton >h3, .infoitm>h5, .nType, .itm-menu > h4 {
                font-family: 'Poppins', sans-serif !important;
            }
            
            .jumbrton >h1 {
                color: #fff;
                font-size: 24px;
                
            }
            .mt-5, .my-5 {
                margin-top: 3rem!important;
            }
            .wrapFlex {
                flex-wrap: wrap;
                align-items: center;
               
            }
            .justify-content-center {
                justify-content: center;
            }
            .itmWidth{
                min-width: 128px;
                margin: 10px;
                cursor: pointer;
            }
            .itm-menu{
                
                padding: 15px;
                border-radius: 4px;
                text-align: center;
                
                font-family: 'Poppins', sans-serif;
                
            }
            .itm-menu>h4 {
                font-size: 15px;
                font-family: 'Poppins', sans-serif !important;
                color: #7e7979;
                font-weight: 500;
            }
            
            .icon-style {
                display: flex;
                margin: 0 auto;
                width: 70px;
                height: 70px;
                border-radius: 8px;
                background: #5345d8;
                justify-content: center;
                align-items: center;
                margin:10px auto 20px auto;
                -webkit-transition: all 0.3s ease-in;
                transition: all 0.16s cubic-bezier(0.57, 1.87, 1, 1);
            }
            .c1 .icon-style{
                background: #5345d8;
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#635bd3+1,5345d8+100 */
                background: #635bd3; /* Old browsers */
               

            }
            .c2 .icon-style{
                background: #40d0b0;
            }
            .c3 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#635bd3+1,a345d8+100 */
                background:#37b35d; /* Old browsers */
               

            }
            .c4 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#a85fd8+1,f06838+100 */
                background: #8235b5; /* Old browsers */   
            }
            .c5 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#bbcea1+1,9ea369+100 */
                background: #7d9957; /* Old browsers */
               

            }
            .c6 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#5f5687+0,3a3941+100 */
                background: #3a2a7a; /* Old browsers */
            }
            .c7 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#22a57c+0,391c8b+99 */
                background: #3d55ad; /* Old browsers */
            }
            .c8 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#d3949e+0,d63653+100 */
                background: #da203e; /* Old browsers */
               

            }
            .c9 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#646edb+1,5345d8+100 */
                background: #5345d8; /* Old browsers */
               
            }
            .c10 .icon-style{
                /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#d6a062+2,133f96+100 */
                background: #d6a062; /* Old browsers */
            }
            .itmWidth:hover .itm-menu .icon-style {
                transform: scale(1.3);
                color: #fff;
                box-shadow:0 3px 5px rgba(0,0,0,0.15);
            }
            
            .icon-style>img {
                max-width: 40px;
            }
            .clear {
                clear:both
            }
            .info-item > .infoitm{
                background: #fff;
                border-radius: 9px;
                margin: 15px 15px 0 15px ;
                padding: 10px 25px;
                border-bottom: 4px solid #d1d6d1
            }
            .info-item > .infoitm >h5 {
                font-size: 15px;
            }
            .plBadge {
                background: #fff;
                border-radius: 10px;
                padding: 0 12px;
                color: #e33333;
                font-weight: 600;
                font-family: 'Poppins', sans-serif;
                
            }
            .totalG {
                border-bottom: 4px solid #24bc24 !important;
            }
            .totalR {
                border-bottom: 4px solid #e14f23 !important;
            }
            .nType{
                    font-size: 24px;
                    font-weight: 600;
                    color: #4c4b4b;
                }
        </style>
    <style>
        .d-flex {
            display: -ms-flexbox!important;
            display: flex!important
        }
        .d-inline-flex {
            display: -ms-inline-flexbox!important;
            display: inline-flex!important
        }


        .mainDashBoxes {
            flex-wrap:wrap;
        }
        .mainDashBoxes .flex-itm{
            position:relative;
            min-width: 196px;
        }
        .mainDashBoxes .flex-itm:not(:last-child) {
            margin-right:24px;
        }
        @media only screen and (min-width: 889px) {
            .mainDashBoxes .flex-itm:not(:last-child) {
                margin-right:24px;
            }
        }
        .widgBox {
    background: #fff;
    border-radius: 40px;
    padding: 20px;  
    -webkit-transition: transform 0.3s ease-in-out;
    -moz-transition: transform 0.3s ease-in-out;
    transition: transform 0.3s ease-in-out;
    box-shadow: 0px 5px 20px rgba(0,0,0,0.23);
    cursor:pointer !important;
    margin-bottom:15px;
    padding-bottom:40px;
}
.widgBox .icon {
    width: 60px;
    height: 60px;
    display: flex;
    justify-content: center;
    align-items: center;
    background: #fff;
    border-radius: 50%;
    -webkit-box-shadow: 7px 8px 10px rgba(0, 0, 0, 0.22);
    box-shadow: 7px 8px 10px rgba(0, 0, 0, 0.22);
    font-size: 24px;
}
.widgBox .icon>img {
    max-width:32px;
}
.widgBox .txt{
    font-size: 15px;
    padding-left: 10px;
    max-width: 114px;
    font-weight: 600;
    color: #8e8787;
    padding-top: 16px;
}
.widgBox .Numb {
    text-align: center;
    font-size: 30px;
    padding: 15px 0;
    font-weight: 400;
    font-family: 'Roboto', sans-serif;
}
.widgBox .Numb + div{
    font-size: 15px;
}
.widgBox .dwn {
    font-size: 26px;
    position: absolute;
    right: 18px;
    bottom: 40px;
}
.widgBox.active {
    background: #ee0067;
    -webkit-transform: scale(1.14);
    -moz-transform: scale(1.14);
    transform: scale(1.14);
    color: #fff;
}
.widgBox.active .txt {
    color:#fff;
}
.widgBox .icon {
    color: #ffffff;
    background: #ee0067;
}
.widgBox.active .icon {
    background: #ee0067;
}

.widgBox.active.c2 {
    background: #0068bd;
    color: #fff;
}
.widgBox.c2 .icon {
    color: #ffffff;
    background: #0068bd;
}
.widgBox.active.c2 .icon {
    background: #0068bd;
}

.widgBox.active.c3 {
    background: #9c3dec;
    color: #fff;
}
.widgBox.c3 .icon {
    color: #ffffff;
    background: #9c3dec;
}
.widgBox.active.c3 .icon {
    background: #3eac3f;
}
.widgBox.active.c4 {
    background: #5b4a4a;
    color: #fff;
}
.widgBox.c4 .icon {
    color: #ffffff;
    background: #5b4a4a;
}
.widgBox.c5 .icon {
    color: #ffffff;
    background: #27bd24;
}
.widgBox.c6 .icon {
    color: #ffffff;
    background: #f14b2f;
}
.widgBox.active.c4 .icon {
    background: #5b4a4a;
}
.widgBox.active.c7 {
    background: #383d82;
    color: #fff;
}
.widgBox.c7 .icon {
    color: #ffffff;
    background: #383d82;
}
.widgBox.active.c8 {
    background: #88792c;
    color: #fff;
}
.widgBox.c8 .icon {
    color: #ffffff;
    background: #88792c;
}
.widgBox:not(.active):hover {
    -webkit-transform: scale(1.06);
    -moz-transform: scale(1.06);
    transform: scale(1.06);
}
.widgBox:not(.active):hover .arrD{
    -moz-animation: bounce 1s infinite;
	-webkit-animation: bounce 1s infinite;
	animation: bounce 1s infinite;
}
.dpvsl {
    float:right;
}
.translate-y{
    -webkit-transform: translateY(4px);
    -moz-transform: translateY(4px);
    transform: translateY(4px);
    display: inline-block;
}
.translate-y-big{
    -webkit-transform: translateY(11px);
    -moz-transform: translateY(11px);
    transform: translateY(11px);
    display: inline-block;
}
.navRightDr>ul{
    display: inline-block;
    position: absolute;
    right: 25px;
    top: 18px
}
.navRightDr>ul>li{
float:left;
    }
.navRightDr>ul>li .pl{
    background:#fff;
    border-radius:4px;
    padding:5px 14px;
    cursor:pointer;
}
.navRightDr>ul>li .pl:hover {
    background:#ebee21;
}
.navRightDr>ul .dropdown > a {
    color:#fff;
    padding: 0;
    padding-left: 15px;
}
.navRightDr>ul .dropdown > a:hover,
.navRightDr>ul .dropdown > a:focus,
.navRightDr>ul .dropdown > a:active,
.navRightDr>ul .dropdown > a:visited {
    background:transparent !important;
    background-color:transparent !important;
}
.navRightDr .dropdown-menu.dropdown-menu-right{
    left: auto;
    right: 0;
    border:none;
    min-width:350px;
        top: 130%;
}

.navRightDr .dropdown-menu .dropdown-header {
    background: #FFF;
    padding: 15px;
    position: relative;
    text-align: center;
    color: #747F8B;
    font-weight: bold;
    border-radius: 10px 10px 0 0;
    border: 0px;
    border-style: solid;
    border-bottom-width: 1px;
    -moz-border-image: -moz-linear-gradient(right, white, #cedae0, #cedae0, white) 1 20%;
    -o-border-image: -o-linear-gradient(right, white, #cedae0, #cedae0, white) 1 20%;
    border-image: linear-gradient(to right, white 0%, #cedae0 40%, #cedae0 60%, white 100%) 1 20%;
    box-shadow: 0px 2px 10px -2px #cedae0;
}
.navRightDr .dropdown-menu .dropdown-header .triangle {
    position: absolute;
    top: -10px;
    right: 7px;
    height: 18px;
    width: 18px;
    border-radius: 4px 0px 0px 0px;
    transform: rotate(45deg);
    background: #FFF;
}
.navRightDr .dropdown-menu .dropdown-body {
    padding-top: 12px;
}

.dashboardWraper {
   padding: 0 15px;
}

.backgroundedBoxes {
    padding: 30px 0;
    margin: 15px 0;
    background: #ebe7db;
    border-radius: 15px;
}
</style>
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>
<style>
    .closeBtn a {
        font-size: 14px;
        /* padding: 8px; */
        color: #e55656;
        background: #e35353;
        color: #fff !important;
        width: 23px;
        height: 23px;
        display: inline-block;
        text-align: center;
        line-height: 25px;
        border-radius: 50%;
        margin-top: 20px;
        margin-right: 14px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboardWraper">
            <span class="pull-right closeBtn">  <a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
            <div class="jumbrton" style="padding-top:20px;border-radius:10px">
                <h1>PROJECT MANAGEMENT</h1>

                <div class="clearfix" style="margin-top:25px">
                        <div class="d-flex justify-content-center mainDashBoxes">
                        
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox " id="a2">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/bx9.png" /></div>
                                    <div class="flex-grow-1 txt">REVENUE</div>
                                </div>
                                <div class="Numb" >11,55,205 <span>cr</span></div>
                                <div class="text-center">As on Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c4" id="" >
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/bx9.png" /></div>
                                    <div class="flex-grow-1 txt">BOOKED</div>
                                </div>
                                <div class="Numb" >11,55,205 <span>cr</span></div>
                                <div class="text-center">As on Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c3" id="a1" >
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/bx9.png" /></div>
                                    <div class="flex-grow-1 txt">BALANCE</div>
                                </div>
                                <div class="Numb" >11,55,205 <span>cr</span></div>
                                <div class="text-center">As on Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c6" id="" >
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/bx9.png" /></div>
                                    <div class="flex-grow-1 txt">PROFIT</div>
                                </div>
                                <div class="Numb" >11,55,205 <span>cr</span></div>
                                <div class="text-center">As on Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                   </div>
                    </div>
            </div>
            <div class="container-fluid mbot50" style="padding: 0 25px;">
                
                <div class="row">
                    
                    <div class="clearfix cstCnt">
                        <div class="costing-header text-center">Project Costing</div>
                        <div class="clearfix" style="margin-top:18px">
                            <div class="d-flex justify-content-center mainDashBoxes">
                        
                            <div class="flex-itm scr " data-scroll="accordion">
                                <div class="widgBox c2" id="a2">
                                    <div class="d-flex  align-items-center">
                                        <div class="icon"><img src="../images/bx9.png" /></div>
                                        <div class="flex-grow-1 txt">Estemated Cost</div>
                                    </div>
                                    <div class="Numb" >11,55,205 <span>cr</span></div>
                                    <div class="text-center">As on Today</div>
                                    <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                </div>
                            </div>
                        
                            <div class="flex-itm scr " data-scroll="accordion">
                                <div class="widgBox c7" id="" >
                                    <div class="d-flex  align-items-center">
                                        <div class="icon"><img src="../images/bx9.png" /></div>
                                        <div class="flex-grow-1 txt">Order Value</div>
                                    </div>
                                    <div class="Numb" >11,55,205 <span>cr</span></div>
                                    <div class="text-center">As on Today</div>
                                    <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                </div>
                            </div>
                            <div class="flex-itm scr " data-scroll="accordion">
                                <div class="widgBox c8" id="a1" >
                                    <div class="d-flex  align-items-center">
                                        <div class="icon"><img src="../images/bx9.png" /></div>
                                        <div class="flex-grow-1 txt">Cost Booked</div>
                                    </div>
                                    <div class="Numb" >11,55,205 <span>cr</span></div>
                                    <div class="text-center">As on Today</div>
                                    <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                </div>
                            </div>
                            <div class="flex-itm scr " data-scroll="accordion">
                                <div class="widgBox c3" id="" >
                                    <div class="d-flex  align-items-center">
                                        <div class="icon"><img src="../images/bx9.png" /></div>
                                        <div class="flex-grow-1 txt">Balance</div>
                                    </div>
                                    <div class="Numb" >11,55,205 <span>cr</span></div>
                                    <div class="text-center">As on Today</div>
                                    <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                </div>
                            </div>
                       </div>
                    </div>
                    </div>
                    
                </div>

                <div class="chartContainer">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="shadowBox text-left">
                                <div class="bigHeading">Projects</div>
                                <div class="mainAreachart">
                                    <div id="chartdiv"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="shadowBox text-left">
                                <div class="bigHeading">Project Details</div>
                                <div class="mainAreachart">
                                    <div id="chartdiv2"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            
        </div>

        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://www.amcharts.com/lib/4/core.js"></script>
        <script src="https://www.amcharts.com/lib/4/charts.js"></script>
        <script src="https://www.amcharts.com/lib/4/themes/animated.js"></script>
        <!-- Chart code -->
        <script>
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdiv2", am4charts.XYChart);

                // Add data
                chart.data = [
                  {
                      "state": "AMC Project",
                      "sales": 30
                  },
                  {
                      "state": "Marketing",
                      "sales": 60
                  },
                  {
                      "state": "NZ Health and Saftey",
                      "sales": 45
                  },
                  {
                      "state": "Abcd project",
                      "sales": 82
                  },
                  {
                      "state": "Somthing new",
                      "sales": 24
                  }
                ];

                // Create axes
                var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                yAxis.dataFields.category = "state";
                yAxis.renderer.grid.template.location = 0;
                yAxis.renderer.labels.template.fontSize = 10;
                yAxis.renderer.minGridDistance = 10;

                var xAxis = chart.xAxes.push(new am4charts.ValueAxis());

                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueX = "sales";
                series.dataFields.categoryY = "state";
                series.columns.template.tooltipText = "{categoryY}: [bold]{valueX}[/]";
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

            }); // end am4core.ready()
        </script>
        <script>
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdiv", am4charts.PieChart);

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "litres";
                pieSeries.dataFields.category = "country";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(30);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                pieSeries.slices.template
                // change the cursor on hover to make it apparent the object can be interacted with
                .cursorOverStyle = [
                    {
                        "property": "cursor",
                        "value": "pointer"
                    }
                ];

                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);

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
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];
                chart.data = [{
                    "country": "Completed",
                    "litres": 28
                }, {
                    "country": "In Progress",
                    "litres": 55
                }, {
                    "country": "Not Started",
                    "litres": 11
                }];
            }); // end am4core.ready()
        </script>
    </form>
</body>
</html>

