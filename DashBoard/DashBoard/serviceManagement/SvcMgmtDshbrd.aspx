<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SvcMgmtDshbrd.aspx.cs" Inherits="DashBoard.DashBoard.serviceManagement.SvcMgmtDshbrd" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,400i,500,600,700,800,900&display=swap" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />

    <style>
            .jumbrton , .jumbrton >h1, .jumbrton >h3, .infoitm>h5, .nType, .itm-menu > h4 {
                font-family: 'Poppins', sans-serif !important;
            }
            .jumbrton {
                background: #323eda;
                text-align: center;
                padding: 70px 0;
               
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
        }
        .mainDashBoxes .flex-itm:not(:last-child) {
            margin-right:16px;
        }
        @media only screen and (min-width: 889px) {
            .mainDashBoxes .flex-itm:not(:last-child) {
                margin-right:16px;
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
    font-size: 4.9em;
    font-weight: 200;
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

 /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        .popBox {
                width: 670px;
                background: #fff;
                padding: 35px;
                text-align: center;
                min-height: 350px;
                display: flex;
                align-items: center;
                flex-direction: column;
                justify-content: center;
                background:#fff url("../../images/popupBack.png") no-repeat top left;
                box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        
            }
        .popBox  h1, .popBox p{
            font-family: 'Poppins', sans-serif !important;
            margin-bottom:20px !important;
        }
        .popBox p {
            font-size: 15px;
        }
        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

        .btn-sign:hover {
            background: #2e71e1;
            color: #fff;
           }
    </style>
     <script>
         $(document).ready(function () {

             //$('.itmWidth').mouseover(function () {
             //    var iconTag = $(this).find('.icon-style img');
             //    var dataTag = $(this).find('.icon-style>img').attr('data-in');
             //    //alert(dataTag)
             //    $(this).find('.icon-style img').attr('src', dataTag);
             //})
             //$('.itmWidth').mouseout(function () {
             //    var iconTag = $(this).find('.icon-style img');
             //    var dataTag = $(this).find('.icon-style>img').attr('data-out');
             //    //alert(dataTag)
             //    $(this).find('.icon-style img').attr('src', dataTag);
             //})
         })

         function SwitchtoERP() {
             var OtherDetails = {}
             OtherDetails.comment = "ERP";
             $.ajax({
                 type: "POST",
                 url: "SvcMgmtDshbrd.aspx/SrvSession",
                 data: JSON.stringify(OtherDetails),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     parent.document.location.href = '/oms/management/projectmainpage.aspx'
                 }
             });

            
         }

         function PopupOk() {
             $('#divPopHead').addClass("hide");
         }

         function ShowOpup() {
             $.ajax({
                 type: "POST",
                 url: "SvcMgmtDshbrd.aspx/AnnouncementDetails",
                 data: JSON.stringify({ reqStr: "" }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {
                     var list = msg.d;
                     var listItems = [];
                     if (list != null && list.length > 0) {

                         for (var i = 0; i < list.length; i++) {
                             var head = '';
                             var body = '';
                             head = list[i].split('|')[0];
                             body = list[i].split('|')[1];
                             htm = list[i].split('|')[2];
                         }
                         $("#h1heading").text(head);
                         $("#pParagraph").html(htm);
                         $("#divPopHead").removeClass("hide");
                     }
                     else {
                         $("#h1heading").text('');
                         $("#pParagraph").text('No notification for today.');
                         $('#divPopHead').removeClass("hide");
                     }
                 }
             });
         }

         function ReceiptChallan() {
             parent.document.location.href = '/ServiceManagement/Transaction/ReceiptChallanList.aspx';
         }

         function AssignJob() {
             parent.document.location.href = '/ServiceManagement/Transaction/AssignedJob.aspx';
         }

         function ServiceData() {
             parent.document.location.href = '/ServiceManagement/Transaction/serviceData/serviceDataList.aspx';
         }

         function DeliveryClick() {
             parent.document.location.href = '/ServiceManagement/Transaction/Delivery/DeliveryList.aspx';
         } 
         function warrantyUpdate() {
             parent.document.location.href = '/ServiceManagement/Transaction/Warranty/WarrantyList.aspx';
         }
         function SearchClick() {
             parent.document.location.href = '/ServiceManagement/Transaction/search/searchqueries.aspx';
         }

         function JobsheetEntry() {
             parent.document.location.href = '/ServiceManagement/Transaction/jobsheet/jobsheetList.aspx';
         }

         function ReportClick() {
             parent.document.location.href = '/Reports/GridReports/SRVServiceRegisterReport.aspx';
         }

         function STBReceiving() {
             parent.document.location.href = '/ServiceManagement/Transaction/STB/stbList.aspx';
         }

        </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('body').click(function (e) {
                
                $('.sidenav ul.dropdown-menu').hide();
                $('.sidenav>ul>li a').removeClass('active');
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="dashboardWraper">
            <div class="jumbrton" style="padding-bottom: 20px;padding-top: 31px;border-radius:15px">
                <div class="navRightDr">
                    <ul class="nav">
                        <li><div class="pl" onclick="SwitchtoERP()"><img src="../images/arrow.png" style="width:16px;margin-right:5px"  />Switch to ERP</div></li>
                        <li class="nav-item dropdown relative">
                          <a class="nav-link text-light " href="javascript:void(0)" id="navbarDropdown" role="button" onclick="ShowOpup()" >
                            <img src="../images/notification.png" style="width:26px" />
                          </a>
                           <%-- <div class="dropdown-menu dropdown-menu-right hide">
                                  <div class="dropdown-header">
                                    <span class="triangle"></span>
                                    <span class="heading">Notifications</span>
                                  </div> 
                                  <div class="dropdown-body">
                                      <p>Son sldfn sdln sdf lnsdnlgsdg</p>
                                  </div>
                            </div>--%>
                        </li>
                    </ul>
                </div>
                <h1 style="margin-bottom: 37px !important;text-transform:uppercase">Service management</h1>
                <h3 style="color: #c4c4c4;margin-bottom:20px !important"></h3>
                <div>
                    <div class="d-flex justify-content-center mainDashBoxes">
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c2" id="a4" onclick="OnTotalClick(this);">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/chall.png" /></div>
                                    <div class="flex-grow-1 txt"> Challan</div>
                                </div>
                                <div class="Numb" runat="server" id="DivTotalChallan">0</div>
                                <div class="text-center">Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c3" id="a1" onclick="AtWorkClick(this);">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/token.png" /></div>
                                    <div class="flex-grow-1 txt"> Token</div>
                                </div>
                                <div class="Numb" runat="server" id="DivTotalToken">0</div>
                                <div class="text-center">Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c5" id="" onclick="NotLoggedInClick(this);">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/topen.png" /></div>
                                    <div class="flex-grow-1 txt">  Open</div>
                                </div>
                                <div class="Numb" runat="server" id="DivToptalOpen">0</div>
                                <div class="text-center">Total</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c4" id="" onclick="NotLoggedInClick(this);">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/ready.png" /></div>
                                    <div class="flex-grow-1 txt"> Ready</div>
                                </div>
                                <div class="Numb" runat="server" id="DivTotalReady">0</div>
                                <div class="text-center">Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox " id="a2" onclick="OnLeaveClick(this);">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/deliv.png" /></div>
                                    <div class="flex-grow-1 txt"> Delivered</div>
                                </div>
                                <div class="Numb" runat="server" id="DivTotalDelivered">0</div>
                                <div class="text-center"> Today</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                        
                        
                        <div class="flex-itm scr " data-scroll="accordion">
                            <div class="widgBox c" id="a3" onclick="NotLoggedInClick(this);">
                                <div class="d-flex  align-items-center">
                                    <div class="icon"><img src="../images/undel.png" /></div>
                                    <div class="flex-grow-1 txt">  Undelivered</div>
                                </div>
                                <div class="Numb" runat="server" id="DivTotalUndelivered">0</div>
                                <div class="text-center">Total</div>
                                <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                            </div>
                        </div>
                   </div>
                </div>
                
            </div>
            <div class="container mt-5 ">
                
                <div class="d-flex wrapFlex justify-content-center">
                    <div class="itmWidth" id="DivReceiptChallan" runat="server">
                        <div class="itm-menu c1" onclick="ReceiptChallan();">
                            <div>
                                <span class="icon-style"><img src="../images/paperW.png" data-in="../images/paper.png" data-out="../images/paperW.png" /></span>
                            </div>
                            <h4>Receipt Challan</h4>
                        </div>
                    </div>
                    <div class="itmWidth" id="DivAssignJob" runat="server" onclick="AssignJob();">
                        <div class="itm-menu c3">
                                <div>
                                    <span class="icon-style "><img src="../images/salesmanW.png" data-in="../images/salesman.png" data-out="../images/salesmanW.png" /></span>
                                </div>
                                <h4>Assign Job</h4>
                        </div>
                    </div>
                    
                <div class="itmWidth" id="DivServiceDatas" runat="server" onclick="ServiceData();">
                    <div class="itm-menu c4">
                        <div>
                            <span class="icon-style "><img src="../images/cogst.png" data-in="../images/plumbing.png" data-out="../images/plumbingW.png" /></span>
                        </div>
                        <h4>Service Entry</h4>
                    </div>
                </div>
                <div class="itmWidth" id="warrantyDiv" runat="server" onclick="warrantyUpdate();">
                    <div class="itm-menu c9">
                        <div>
                            
                            <span class="icon-style "><img src="../images/icons/warranty.png" data-in="../images/icons/warranty.png" data-out="../images/icons/warranty.png" /></span>
                        </div>
                        <h4>Warranty Update</h4>
                    </div>
                </div>
                <div class="itmWidth" id="DivDelivery" runat="server" onclick="DeliveryClick();">
                    <div class="itm-menu c5">
                        <div>
                            <span class="icon-style "><img src="../images/boxW.png" data-in="../images/box.png" data-out="../images/boxW.png" /></span>
                        </div>
                        <h4>Delivery</h4>
                    </div>
                </div>
                <div class="itmWidth" id="DivSearch" runat="server" onclick="SearchClick();">
                    <div class="itm-menu c6">
                        <div>
                            <span class="icon-style "><img src="../images/searchW.png" data-in="../images/search.png" data-out="../images/searchW.png" /></span>
                        </div>
                        <h4>Search</h4>
                    </div>
                </div>
                <div class="itmWidth" id="DivJobsheetEntry" runat="server" onclick="JobsheetEntry();">
                    <div class="itm-menu c7">
                        <div>
                            <span class="icon-style "><img src="../images/EntryW.png" data-in="../images/Entry.png" data-out="../images/EntryW.png" /></span>
                        </div>
                        <h4>Jobsheet Entry</h4>
                    </div>
                </div>
                     <div class="itmWidth" id="DivSTBReceiving" runat="server" onclick="STBReceiving();">
                    <div class="itm-menu c1">
                        <div>
                            <span class="icon-style " style="background: #f1993f !important;"><img src="../images/router-D.png" data-in="../images/router-D.png" data-out="../images/router-D.png" /></span>
                        </div>
                        <h4>STB Receiving</h4>
                    </div>
                </div>
                <div class="itmWidth" id="DivReport" runat="server" onclick="ReportClick();">
                    <div class="itm-menu c8">
                        <div>
                            <span class="icon-style "><img src="../images/reportW.png" data-in="../images/report.png" data-out="../images/reportW.png"  /></span>
                        </div>
                        <h4>Report</h4>
                    </div>
                </div>
                </div>
            </div>
            
        </div>

        <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="../../assests/images/pop_notification.png" class="mBot10" style="width: 50px;" />
            <h1 id="h1heading" runat="server"><%--Discussion heading--%></h1>
            <div id="pParagraph" runat="server" style="max-height: 50vh;overflow-y: auto;text-align: initial">
               <%-- Then system moves to ‘Dashboard’ or landing page where the entire control and
                    activity component will be available for service management.--%>
            </div>
            <button type="button" class="btn btn-sign" onclick="return PopupOk()">OK</button>
        </div>
    </div>

    </form>
</body>
</html>
