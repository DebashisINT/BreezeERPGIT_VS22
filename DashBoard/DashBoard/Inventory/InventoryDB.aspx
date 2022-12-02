<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryDB.aspx.cs" Inherits="DashBoard.DashBoard.Inventory.InventoryDB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@200;300;400;500;600;700;800&display=swap" rel="stylesheet">
   <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />
    <link href="../css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <script src="../Js/jquery.3.3.1.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <script src="../Js/moment.min.js"></script>
    <link href="../css/SearchPopup.css" rel="stylesheet" />
    <script src="../js/SearchMultiPopup.js"></script>

    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="../Js/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../Js/datePicker/bootstrap-datepicker.js"></script>
    <script src="../Js/amChart/core.js"></script>
    <script src="../Js/amChart/charts.js"></script>
    <script src="../Js/amChart/animated.js"></script>
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>
    <style>
    .ctxt1 {
        color: #2061d6;
    }.ctxt2 {
        color: #258836;
    }
     .ctxt3 {
        color: #bb4d23;
    }
     .ctxt4 {
        color: #7d0859;
    }
     .ctxt5 {
        color: #3c148c;
    }
     .ctxt6 {
        color: #b30909;
    }
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
        margin-top: 12px;
    }
    ul.nopad {
        list-style-type: none;
        padding: 0;
    }

        ul.nopad > li {
            display: inline-block;
            min-width: 18.4%;
        }

            ul.nopad > li:not(:last-child) {
                margin-right: 1%;
            }

    .colorBox {
        background: #4f4e94;
        color: #fff;
        border-radius: 8px;
        overflow: hidden;
    }

        .colorBox.c1 {
            background: #309b60;
        }

        .colorBox.c2 {
            background: #3798c5;
        }

        .colorBox.c3 {
            background: #e68929;
        }

        .colorBox.c4 {
            background: #882626;
        }
        .colorBox.c5{
            background: #2646bb;
        }
        .colorBox.c6{
            background: #8355bf;
        }
        

    .infoWrp {
        padding: 12px 15px;
        position: relative;
    }

        .infoWrp > .nums {
            font-size: 20px;
            font-weight: 600;
        }

        .infoWrp > .txts {
            font-size: 80%;
        }

    .mrLinks {
        cursor: pointer;
        background: rgba(0, 0, 0, 0.34);
        padding: 5px 10px;
        font-size: 14px;
    }

        .mrLinks > .fa {
            float: right;
            margin-top: 4px;
        }

    .fltd {
        position: absolute;
        right: 20px;
        top: 12px;
        font-size: 44px !important;
        color: rgba(0, 0, 0, 0.54);
    }

    .box-holder {
        border: 1px solid #ccc;
        border-radius: 4px;
        border-top: 4px solid rgb(13, 105, 67);
        background: #fff;
        -webkit-transition: all 0.2s ease-in;
        -moz-transition: all 0.2s ease-in;
        transition: all 0.2s ease-in;
    }

    .chartHolder {
        height: 300px;
    }

    
    .box-holder.full-screen .toggleFullScreen {
        right: 25px;
        top: 19px;
    }

    .box-holder .hdd-rp {
        font-size: 15px;
        font-weight: 600;
        padding: 8px 15px;
    }

    .box-holder .bxd-cont {
        padding: 15px;
    }

    .table {
        width: 100%;
    }

        .table .thead-light th {
            color: #495057;
            background-color: #e9ecef;
            border-color: #dee2e6;
        }

    .badge {
        font-weight: 500;
        font-size: 12px;
    }

    .badge-warning {
        color: #212529;
        background-color: #ffc107;
    }

    .badge-success {
        color: #fff;
        background-color: #28a745;
    }

    .badge-danger {
        color: #fff;
        background-color: #dc3545;
    }

    #chartdiv {
        width: 100%;
        height: 450px;
    }

    .mTop10 {
        margin-top: 10px;
    }

    .pignose-calendar {
        width: 100%;
        max-width: 100% !important;
    }

        .pignose-calendar .pignose-calendar-top,
        .pignose-calendar {
            box-shadow: none !important;
        }

    .tbleChoosedate {
        margin-right: 15px;
        font-size: 14px;
        margin-top: 10px;
        margin-bottom:15px;
    }
    .tbleChoosedate  .btn {
        margin:0 !important
    }

        .tbleChoosedate > tbody > tr > td:not(:last-child) {
            padding-right: 15px;

        }

    .dateWidth {
        max-width: 140px;
    }

    .chartHolder {
        border: 1px solid #ccc;
    }

    .usrTabl {
        width: 100%;
    }

        .usrTabl > thead > tr > th {
            background: #3798c5;
            color: #fff;
            padding: 8px 5px;
        }

        .usrTabl > tbody > tr > td {
            padding: 5px;
        }

        .usrTabl > tbody > tr:nth-child(even) > td {
            background: #f1f1f1;
        }

    .nCheck {
        font-size: 18px;
        display: flex;
        width: 100%;
        height: 100%;
        justify-content: center;
        align-items: center;
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
            padding:5px 12px;
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
        font-size: 14px;
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
    #chartdiv1 , #chartdiv2, #chartdiv3, #chartdiv4{
      width: 100%;
      height: 500px;
    }
    .spaceSeparate {
        margin: 17px 0;
        border-top: 2px dashed #f5f0f0;
    }
    .shadowBox {
        padding: 15px 10px;
        text-align: center;
        border-radius: 10px;
        box-shadow: 0 0 8px rgba(0,0,0,0.26);
        background: #fff;
    }
    .bigHeading {
        font-size: 15px;
        text-transform: uppercase;
        padding: 10px 20px;
        color: #2337ad;
    }
    .chartInput {
        border-radius: 3px !important;
        height: 29px !important;
        margin-bottom: 0 !important;
    }

    .backgroundedBoxes {
        padding: 30px 0;
        margin: 15px 0;
        background: #ebe7db;
        border-radius: 15px;
    }
    .mainDashBoxes {
        flex-wrap: wrap;
    }
    .d-flex {
        display: -ms-flexbox!important;
        display: flex!important;
    }
    .justify-content-center {
        justify-content: center;
    }
    .mainDashBoxes .flex-itm {
    position: relative;
    min-width: 196px;
}
    .mainDashBoxes .flex-itm:not(:last-child) {
    margin-right: 24px;
}
    @media only screen and (min-width: 889px) {
        .mainDashBoxes .flex-itm:not(:last-child) {
            margin-right: 24px;
        }
    }
    .widgBox {
    background: #fff;
    border-radius: 40px;
    padding: 10px;
    -webkit-transition: transform 0.3s ease-in-out;
    -moz-transition: transform 0.3s ease-in-out;
    transition: transform 0.3s ease-in-out;
    box-shadow: 0px 5px 20px rgba(0,0,0,0.23);
    cursor: pointer !important;
    margin-bottom: 15px;
    padding-bottom: 40px;
}
    .widgBox .Numb {
    text-align: center;
    font-size: 4.9em;
    font-weight: 200;
    font-family: 'Roboto', sans-serif;
}
    .widgBox .dwn {
    font-size: 26px;
    position: absolute;
    right: 18px;
    bottom: 40px;
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
 .widgBox .icon {
    color: #ffffff;
    background: #ee0067;
}
.widgBox .icon>img {
    max-width: 32px;
}
.widgBox .txt {
    font-size: 15px;
    padding-left: 10px;
    max-width: 114px;
    font-weight: 600;
    color: #8e8787;
    padding-top: 16px;
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
.mainDashBoxes .flex-itm.inlineWidth{
    min-width: 127px;
    
}
.mainDashBoxes .flex-itm.inlineWidth .widgBox{
    text-align: center;
    border-radius:5px;
}


</style>
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
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
    <script src="inventory.js?v1.0.14"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="clearfix" style="padding:0 30px">
       <div>
           <div class="row">

                    <div class="col-md-12 text-right clearfix py-3">
                        <div class="panel-title pull-left">
                            <h3>Inventory Analytics </h3>
                        </div>
                        <span class="pull-right closeBtn">  <a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
                        <div style="float: right;margin: 12px;">
                            <table>
                                <tr>
                                    <td style="padding-right:8px">Branch</td>
                                    <td>
                                        <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td style="width:55px;padding-right:8px"><div style="color: #b5285f;" class="clsFrom">
                                            <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                                        </div></td>
                                     <td>
                                        <div>
                                            <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                        </div>
                                    </td>
                                    <td style="width:60px;padding-right:8px">Product</td>
                                    <td>
                       
                                        <div>
                                            <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                                                <Buttons>
                                                    <dxe:EditButton>
                                                    </dxe:EditButton>
                                                </Buttons>
                                                <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                                            </dxe:ASPxButtonEdit>
                                        </div>
                                    </td> 
                                </tr>
                            </table>
                        </div>  
                    </div>
                </div>
       </div>
      <div class="form_main">
        <div class="row">
            <div class="col-md-12">
                <div class="tabSlideContainer relative">
                    <div class="swNav snavPrev">
                        <i class="fa fa-arrow-left"></i>
                    </div>
                    <div class="swNav snavNext"><i class="fa fa-arrow-right"></i></div>
                
                    <div class="swiper-container vTabWrap fontPop ">
                    <div class="swiper-wrapper abc">
                        <div class="swiper-slide" id="divStockStatus" runat="server">
                            <div class="widget c1 zoom DisableClass" data-click="fTab">
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Stock Status</div>
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide" id="divTopNProduct" runat="server">
                            <div class="widget c2 zoom" data-click="STab">
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Top N Product</div>
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide " id="divActualvsDemand" runat="server">
                            <div class="widget c3 zoom" data-click="tTab">
                               
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Actual vs Demand</div>
                                    
                                </div>
                                 <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div>
                        <div class="swiper-slide " id="divStockAlert" runat="server" >
                            <div class="widget c4 zoom" data-click="frTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Stock Alert</div>
                                   
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div> 
                        <div class="swiper-slide " id="divStockRequisition" runat="server">
                            <div class="widget c6 zoom" data-click="fvTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Stock Requisition</div>
                                   
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div> 
                        <div class="swiper-slide " id="divProcurementRequisition" runat="server">
                            <div class="widget c3 zoom" data-click="fsTab">
                                
                                <div class="textInbox">
                                    <div style="padding:10px"><img src="../images/bx1.png" /></div>
                                    <div class="wdgLabel">Procurement Requisition</div>
                                   
                                </div>
                                <div class="iconBox" style="font-size:18px"><i class="fa fa-arrow-down"></i></div>
                            </div>
                        </div> 
                    </div>
                </div>
             </div>
                <div class="spaceSeparate"></div>
                <%--Tabb Panels--%>
                <div class="container-fluid">
                    <div class="panelClass" id="fTab" runat="server">
                        <div class="">
                            <div class="row">
                            <div class="col-md-12 relativeLoader boxLoad">
                                <div id="loaderP">
                                    <img src="../images/bars.svg" style="max-width: 20px;" />
                                    <div>Fetching data ....</div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <table class="tbleChoosedate">
                                            <tr>
                                                
                                                <td>Date</td>
                                                <td class="dateWidth">
                                                    <input type="text" id="toDateRE" class="form-control chartInput datepicker" />
                                                </td>
                                                <td><button type="button" class="btn btn-success" onclick="getdataForBox()">Show Analysis</button></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <div class="colorBox">
                                            <div class="infoWrp">
                                                <i class="fa fa-inr fltd hide" aria-hidden="true"></i>
                                                <div class="nums" id="TotalItems">0</div>
                                            </div>
                                            <div class="mrLinks">
                                                Total Items
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="colorBox c1">
                                            <div class="infoWrp">
                                                <i class="fa fa-inr fltd hide" aria-hidden="true"></i>
                                                <div class="nums"><span id="StInHand">0</span> </div>

                                            </div>
                                            <div class="mrLinks">
                                                Stock on Hand  
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="colorBox c2">
                                            <div class="infoWrp">
                                                <i class="fa fa-inr fltd hide" aria-hidden="true"></i>
                                                <div class="nums" id="AvforSale">0</div>

                                            </div>
                                            <div class="mrLinks">
                                                Available for Sale
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="colorBox c3">
                                            <div class="infoWrp">
                                                <i class="fa fa-truck fltd hide"
                                                   aria-hidden="true"></i>
                                                <div class="nums" id="CmforSale">0</div>

                                            </div>
                                            <div class="mrLinks">
                                                Commited for Sale
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="colorBox c4">
                                            <div class="infoWrp">
                                                <i class="fa fa-truck fltd hide"
                                                   aria-hidden="true"></i>
                                                <div class="nums" id="ItQtyIn">0</div>

                                            </div>
                                            <div class="mrLinks">
                                                Items Qty IN
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <div class="colorBox c5">
                                            <div class="infoWrp">
                                                <i class="fa fa-truck fltd hide"
                                                   aria-hidden="true"></i>
                                                <div class="nums" id="ItQtyOut">0</div>

                                            </div>
                                            <div class="mrLinks">
                                               Items Qty OUT
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                    <div class="panelClass" id="STab" runat="server" >
                        <div class="relative ">
                            <div class="row">
                                <div class="col-md-12">
                                    <table class="tbleChoosedate">
                                        <tr>
                                
                                            <td>Date</td>
                                            <td class="dateWidth">
                                                <input type="text" id="toDateChart" class="form-control chartInput datepicker" />
                                            </td>
                                            <td><button type="button" class="btn btn-info" onclick="getdataForChart()">Generate Charts</button></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                        <div class="col-md-6">
                                            <div class="shadowBox mTop10 box-full">
                    
                                                <div class="clearfix">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="bigHeading">
                                                                Top 10 Most Selling Inventories
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="bxd-cont chartLoad">
                                                    <div class="loaderPC l1">
                                                        <img src="../images/bars.svg" style="max-width: 20px;" />
                                                        <div>Fetching data ....</div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div id="chartdiv1" class="chartHolder font-normal"><div class="nCheck">Select date to refresh data</div></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="shadowBox mTop10 box-full">
                    
                                                <div class="clearfix">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="bigHeading">
                                                                Top 10 Most Purchased Inventories
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="bxd-cont chartLoad">
                                                    <div class="loaderPC l2">
                                                        <img src="../images/bars.svg" style="max-width: 20px;" />
                                                        <div>Fetching data ....</div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div id="chartdiv2" class="chartHolder font-normal"><div class="nCheck">Select date to refresh data</div></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                        </div>
                        <div class="relative ">
                            <div class="row">
                                        <div class="col-md-6">
                                            <div class="shadowBox mTop10 box-full clearfix">
                                                <div class="col-md-12">
                        
                                                    <div class="clearfix">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="bigHeading">
                                        
                                                                    <div>Top 10 Most Sales return Inventories</div>
                                                                </div>
                            
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="bxd-cont chartLoad">
                                                        <div class="loaderPC l3">
                                                            <img src="../images/bars.svg" style="max-width: 20px;" />
                                                            <div>Fetching data ....</div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12 ">
                                                
                                                                <div id="chartdiv3" class="chartHolder font-normal"><div class="nCheck">Select date to refresh data</div></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
            
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="shadowBox mTop10 box-full clearfix">
                                                <div class="col-md-12">
                                                    <div class="clearfix">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="bigHeading">
                                                                    <div>Top 10 Most Purchase Return Inventories</div>
                                                                </div>
                            
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="bxd-cont chartLoad">
                                                        <div class="loaderPC l4">
                                                            <img src="../images/bars.svg" style="max-width: 20px;" />
                                                            <div>Fetching data ....</div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div id="chartdiv4" class="chartHolder font-normal"><div class="nCheck">Select date to refresh data</div></div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                        </div>
                    </div>
                    <div class="panelClass" id="tTab" runat="server" >
                        <div class="row">
                            <div class="col-md-12">
                                <table class="tbleChoosedate">
                                    <tr>
                                        <td>Date</td>
                                        <td class="dateWidth">
                                            <input type="text" id="" class="form-control chartInput datepicker" />
                                        </td>
                                        <td><button type="button" class="btn btn-info" onclick="">Generate</button></td>
                                    </tr>
                                </table>
                            </div>
                        </div> 
                        <div class="chartContainer">
                             <div class="row">
                                  <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading" style="padding:0 !important">Current Stock</div>
                                        <div class="mainAreachart">
                                            <div id="totalActivityChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--second chart--%>
                                 <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading" style="padding:0 !important">Open Sales Order</div>
                                        <div class="mainAreachart">
                                            <div id="dueTodayChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--third chart--%>
                                 <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading" style="padding:0 !important">Open Manufaturing Order</div>
                                        <div class="mainAreachart">
                                            <div id="completedtodayChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                                 <%--Fourth chart--%>
                                 <div class="col-sm-3">
                                      <div class="shadowBox text-left">
                                        <div class="bigHeading" style="padding:0 !important">Open Branch Requisation</div>
                                        <div class="mainAreachart">
                                            <div id="pendingTodayChart" style="height: 220px"></div>
                                        </div>
                                    </div>
                                  </div>
                             </div>
                            <div class="text-center" style="font-size: 18px;font-weight: 500;margin: 19px 0;">Stock In Hand vs Current Demand by Quantity</div>
                            <div class="row">
                                <div class="col-sm-12">
                                        <div class="shadowBox text-left">
                                            <div class="bigHeading" style="padding:0 !important">Stock In Hand vs Demand by Quantity</div>
                                            <div class="mainAreachart">
                                                <div id="bottomChart" style="height: 260px"></div>
                                            </div>
                                        </div>
                                </div>
                            </div>
                        </div>  
                    </div>
                    <div class="panelClass" id="frTab" runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="tbleChoosedate">
                                    <tr>
                                        <td>Product</td>
                                        <td class="dateWidth">
                                            <select class="form-control">
                                                <option>Select</option>
                                            </select>
                                        </td>
                                        <td>Date</td>
                                        <td class="dateWidth">
                                            <input type="text" id="" class="form-control chartInput datepicker" />
                                        </td>
                                        <td><button type="button" class="btn btn-info" onclick="">Generate</button></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="backgroundedBoxes">
                            <div class="text-center" style="font-size: 19px;font-weight: 500;margin: 0 0 15px 0;text-transform: uppercase;">Stock Alert</div>
                            <div class="d-flex justify-content-center mainDashBoxes">
                                <div class="flex-itm scr inlineWidth" data-scroll="">
                                    <div class="widgBox" id="">
                                        <div class="Numb ctxt1" id="">0</div>
                                        <div class="text-center">
                                          <div >Total Products</div>
                                        </div>
                                        
                                    </div>
                                </div>
                                <div class="flex-itm scr inlineWidth" data-scroll="">
                                    <div class="widgBox c2" id="">
                                        <div class="Numb ctxt2" id="">0</div>
                                        <div class="text-center">
                                          <div>Total Value</div>
                                        </div>
                                        
                                    </div>
                                </div>
                                <div class="flex-itm scr inlineWidth" data-scroll="">
                                    <div class="widgBox c1" id="">
                                        <div class="Numb ctxt3" id="">0</div>
                                        <div class="text-center">
                                          <div>Above Max</div>
                                        </div> 
                                    </div>
                                </div>
                                <div class="flex-itm scr inlineWidth" data-scroll="">
                                    <div class="widgBox c3" id="">
                                        <div class="Numb ctxt4" id="">0</div>
                                        <div class="text-center">
                                          <div>Below Min</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="flex-itm scr inlineWidth" data-scroll="">
                                    <div class="widgBox c6" id="">
                                        <div class="Numb ctxt5" id="">0</div>
                                        <div class="text-center">
                                          <div>Below Max & Above Min</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="flex-itm scr inlineWidth " data-scroll="">
                                    <div class="widgBox c4" id="">
                                        <div class="Numb ctxt6" id="">0</div>
                                        <div class="text-center">
                                          <div>Reorder</div>
                                        </div>  
                                    </div>
                                </div>
                           </div>
                        </div>
                    </div>
                    <div class="panelClass" id="fvTab" runat="server">
                        <div class="backgroundedBoxes">
                            <div class="text-center" style="font-size: 19px;font-weight: 500;margin: 0 0 15px 0;text-transform: uppercase;">Stock Requisition</div>
                            <div class="d-flex justify-content-center mainDashBoxes">
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c2" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/TotalLead.png" /></div>
                                            <div class="flex-grow-1 txt"> Branch Requisition</div>
                                        </div>
                                        <div class="Numb" id="brReq">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c4" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/OpenLead.png" /></div>
                                            <div class="flex-grow-1 txt">Approval Pending</div>
                                        </div>
                                        <div class="Numb" id="brReqAP">0</div>
                                        <div class="text-center">Total</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox " id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/QaLead.png" /></div>
                                            <div class="flex-grow-1 txt">Open Requisation</div>
                                        </div>
                                        <div class="Numb" id="brReqOP">0</div>
                                        <div class="text-center">Total</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c3" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/ConvertedLead.png" /></div>
                                            <div class="flex-grow-1 txt">Close Requisition</div>
                                        </div>
                                        <div class="Numb" id="brReqCL">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>

                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c6" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/LostLead.png" /></div>
                                            <div class="flex-grow-1 txt">Approved Requisition</div>
                                        </div>
                                        <div class="Numb" id="brReqAR">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                           </div>
                        </div>

                    </div>
                    <div class="panelClass" id="fsTab" runat="server">
                        <div class="backgroundedBoxes">
                            <div class="text-center" style="font-size: 19px;font-weight: 500;margin: 0 0 15px 0;text-transform: uppercase;">Procurement Requisition</div>
                            <div class="d-flex justify-content-center mainDashBoxes">
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c2" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/TotalLead.png" /></div>
                                            <div class="flex-grow-1 txt">Purchase Requisition</div>
                                        </div>
                                        <div class="Numb" id="PURCHASEREQ">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c4" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/OpenLead.png" /></div>
                                            <div class="flex-grow-1 txt">Open purc. Requisition</div>
                                        </div>
                                        <div class="Numb" id="OPENREQ">0</div>
                                        <div class="text-center">Total</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox " id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/QaLead.png" /></div>
                                            <div class="flex-grow-1 txt">Purchase Order</div>
                                        </div>
                                        <div class="Numb" id="TOTPO">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c6" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/LostLead.png" /></div>
                                            <div class="flex-grow-1 txt">Order Approved</div>
                                        </div>
                                        <div class="Numb" id="APPRPO">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
                                    </div>
                                </div>
                                <div class="flex-itm scr " data-scroll="accordion">
                                    <div class="widgBox c4" id="">
                                        <div class="d-flex  align-items-center">
                                            <div class="icon"><img src="../images/LostLead.png" /></div>
                                            <div class="flex-grow-1 txt">Approved Requisition</div>
                                        </div>
                                        <div class="Numb" id="APPRREQ">0</div>
                                        <div class="text-center">Today</div>
                                        <div class="text-right"><span class="lnr lnr-arrow-down dwn arrD"></span></div>
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
    <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Code</th>
                                 <th>Name</th>
                                <th>HSN</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>

        <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnClassId" runat="server" />
      <asp:HiddenField ID="hdnProductId" runat="server" />   
    <!--Class Modal -->



        <script>
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



                var chart = am4core.create('bottomChart', am4charts.XYChart)
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
                        category: 'Place #1',
                        first: 40,
                        second: 55
                    },
                    {
                        category: 'Place #2',
                        first: 30,
                        second: 78
                    },
                    {
                        category: 'Place #3',
                        first: 27,
                        second: 40
                    },
                    {
                        category: 'Place #4',
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


