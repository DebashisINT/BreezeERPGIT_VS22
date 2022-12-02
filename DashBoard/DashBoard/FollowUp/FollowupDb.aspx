<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FollowupDb.aspx.cs" Inherits="DashBoard.DashBoard.FollowUp.FollowupDb" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/DashBoard/FollowUp/UserControl/PendingFollowup.ascx" TagPrefix="ucpend" TagName="pend" %>
<%@ Register Src="~/DashBoard/FollowUp/UserControl/allFollowup.ascx" TagPrefix="ucallFollowup" TagName="allFollow" %>
<%@ Register Src="~/DashBoard/FollowUp/UserControl/closedUc.ascx" TagPrefix="ucclosedUc" TagName="closedcont" %>
<%@ Register Src="~/DashBoard/FollowUp/UserControl/RationUc.ascx" TagPrefix="ucRatio" TagName="ratiocnt" %>
<%@ Register Src="~/DashBoard/FollowUp/UserControl/followuphistory.ascx" TagPrefix="uHistory" TagName="historyCnt" %>
<%@ Register Src="~/DashBoard/FollowUp/UserControl/FollowupUsing.ascx" TagPrefix="uUsing" TagName="usningcnt" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css"   crossorigin="anonymous"> 

    <link href="../css/SearchPopup.css" rel="stylesheet" />
    <script src="../Js/jquery.3.3.1.js"></script>
   
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
     <script>
         function reloadParent() {
             parent.document.location.href = '/oms/management/projectmainpage.aspx'
         }
    </script>
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>
    <link href="FollowupDb.css" rel="stylesheet" />
    
  <link href="../css/dashboard.css" rel="stylesheet" />
  <script src="../Js/Followupdb.js?v=1.0"></script>
  <script src="../Js/moment.min.js"></script>
  <script src="../Js/SearchPopup.js"></script>
   <script type="text/javascript">
       $(document).ready(function () {

           $('#totFollowupDiv').show().animate({ top: '0px', opacity: '1' });

           var swiper = new Swiper('.swiper-container', {
               slidesPerView: 'auto',
               centeredSlides: false,
               spaceBetween: 10,
               navigation: {
                   nextEl: '.snavNext',
                   prevEl: '.snavPrev',
               }
           });
       });
   </script>
</head>
<body>

     <form id="form1" runat="server">


        <div class="clearfix">
            <div class="col-md-12 clearfix padding bdBot hdBoder">
                <h3 class="pull-left fontPop">Followup Analytics</h3>
                <span class="pull-right ">  <a href="#" onclick="reloadParent()" class="pageClose"><i class="fa fa-times"></i></a></span>
                  <table class="pull-left flLeftTbl hide">
                         <tr>
                     
                    <td>

                        <dx:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    
                    <td>

                        <dx:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <input type="button" class="btn btn-primary" value="Show" onclick="RefreshWidget()" /></td>
                </tr>
                  </table>
            </div>
        </div>



        <div id="CustomNewNotify">Some text some message..</div>



        <div class="clearfix">
            <div class="col-md-12 relative">
            <div class="swNav snavPrev">
                <i class="fa fa-arrow-left"></i>
            </div>
           
            <div class="middl">
                <div class="swiper-container vTabWrap fontPop ">
                <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="zoom " runat="server" id="PendingDivbtn" data-click="PendingDiv">
                        <div  class="wdgLabel">Pending Followup</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class="zoom DisableClass" runat="server" id="totFollowup" data-click="totFollowupDiv">
                        <div  class="wdgLabel">Total Followup</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="closedFollowupBtn" data-click="closedFollowupDiv">
                        <div class="wdgLabel">Closed Followup</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class="zoom " runat="server" id="Conversionbtn" data-click="ConversionDiv">
                        <div  class="wdgLabel">Customer Realisation Ratio</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class="zoom" runat="server" id="followhisBtn" data-click="followhisDiv">
                        <div  class="wdgLabel">Followup History</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class="zoom " runat="server" id="followUnisngbtn" data-click="followusingdiv">
                        <div class="wdgLabel">Followup Using</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="swNav snavNext"><i class="fa fa-arrow-right"></i></div>
    </div>

    <div class="col-md-12" style="padding-top:15px;">
            <div class="col-md-12 panelClass " runat="server" id="totFollowupDiv" style="">
                 
                    <ucallFollowup:allFollow runat="server" ID="allfollowuc" class="mainpanel" />
                  
            </div>


             <div class="col-md-12 panelClass " runat="server" id="ConversionDiv" style=""> 
                  <ucRatio:ratiocnt runat="server" ID="ratiocntid" class="mainpanel" /> 
            </div>


             <div class="col-md-12 panelClass " runat="server" id="PendingDiv" style="">
                
                <ucpend:pend runat="server" ID="Pending" class="mainpanel" />
                   

            </div>



              <div class="col-md-12 panelClass " runat="server" id="closedFollowupDiv" style="">
                
                <ucclosedUc:closedcont runat="server" ID="closedcontid" class="mainpanel" />
                   

            </div>

             <div class="col-md-12 panelClass " runat="server" id="followhisDiv" style="">
                
                <uHistory:historyCnt runat="server" ID="historyCntid" class="mainpanel" />
                   

            </div>

             <div class="col-md-12 panelClass " runat="server" id="followusingdiv" style=""> 
                <uUsing:usningcnt runat="server" ID="usningcntid" class="mainpanel" /> 
            </div>
            
        </div>
</div>
    </form>
</body>
</html>
