<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ServiceManagement.ServiceManagement.Management.management_ProjectMainPage" CodeBehind="ProjectMainPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        ModuleNameForHelp = "Dashboard"

    </script>
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,300i,400,400i,500,600,700,800&display=swap" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        #CustomNewNotify {
            visibility: hidden;
            min-width: 250px;
            margin-left: -125px;
            background-color: #ff1f1f;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            right: 5%;
            bottom: 30px;
            font-size: 17px;
            border: 5px solid #ff1f1f;
            box-shadow: 3px 4px rgba(0,0,0,0.5);
            color: #fff;
        }

            #CustomNewNotify.show {
                visibility: visible;
                -webkit-animation: fadeinAlert 0.5s, fadeoutAlert 0.5s 15.5s;
                animation: fadeinAlert 0.5s, fadeoutAlert 0.5s 15.5s;
            }

        @-webkit-keyframes fadeinAlert {
            from {
                bottom: 0;
                opacity: 0;
            }

            to {
                bottom: 30px;
                opacity: 1;
            }
        }

        @keyframes fadeinAlert {
            from {
                bottom: 0;
                opacity: 0;
            }

            to {
                bottom: 30px;
                opacity: 1;
            }
        }

        @-webkit-keyframes fadeoutAlert {
            from {
                bottom: 30px;
                opacity: 1;
            }

            to {
                bottom: 0;
                opacity: 0;
            }
        }

        @keyframes fadeoutAlert {
            from {
                bottom: 30px;
                opacity: 1;
            }

            to {
                bottom: 0;
                opacity: 0;
            }
        }




        .alert-message-success {
            background-color: #F4FDF0;
            border-color: #3C763D;
        }

            .alert-message-success h4 {
                color: #3C763D;
            }

        .alert-message {
            margin: 20px 0;
            padding-left: 5px;
            margin-bottom: 10px;
        }

            .alert-message h4 {
                margin-top: 0;
                margin-bottom: 8px;
                BORDER-BOTTOM: 2PX SOLID #f3f3f3;
                padding-bottom: 13px;
            }

            .alert-message p:last-child {
                margin-bottom: 0;
            }



        .wdgLabel {
            padding: 7px;
            font-size: 14px;
            text-transform: capitalize;
            font-family: 'Poppins', sans-serif;
        }

        .wdgNumber {
            font-size: 33px;
            font-weight: 600;
        }

        .widget {
            margin-top: 10px;
            margin-bottom: 15px;
            color: #333;
            cursor: pointer;
            border-radius: 3px;
            -webkit-transition: all 0.3s ease-out;
            transition: all 0.3s ease-out;
            position: relative;
            border-radius: 3px;
        }

        .iconH {
            padding: 15px;
        }

        .widget label {
            /*position:absolute;*/
            width: 100%;
            left: 0;
            margin-bottom: 0;
            background: rgba(0,0,0,0.4);
            cursor: pointer;
        }

        .widget:hover {
            -webkit-transform: scale(1.02) translateY(-5px);
            transform: scale(1.02) translateY(-5px);
            box-shadow: 0px 10px 15px rgba(0,0,0,0.15);
        }

        .widget.c1 {
            background: #23C398;
            color: #fff;
        }

        .widget.c2 {
            background: #34679c;
            color: #fff;
        }

        .widget.c3 {
            background: #3562da;
            color: #fff;
        }

        .widget.c4 {
            background: #dac461;
            color: #fff;
        }

        .widget.c5 {
            background: #ac74de;
            color: #fff;
        }

        .widget.c6 {
            background: #85b770;
            color: #fff;
        }

        .widget.c7 {
            background: #dc6a6a;
            color: #fff;
        }

        .widget.c8 {
            background: #424448;
            color: #fff;
        }

        .widget.c9 {
            background: #5f5f61;
            color: #fff;
        }

        .widget.c10 {
            background: #461616bf;
            color: #fff;
        }

        .iconDash {
            width: 46px;
            height: 46px;
            display: inline-block;
            /* border: 1px solid #e9e9e9; */
            /* border-radius: 50%; */
            line-height: 46px;
            font-size: 27px;
            margin-bottom: 13px;
            margin-top: 17px;
            text-align: center;
            background: rgba(0,0,0,0.12);
            border-radius: 3px;
        }
        /*.widget.c1 {
                    background: #23C398;
                    background-image: -webkit-linear-gradient(45deg,#23C398,#d0c034);
                    background-image: linear-gradient(45deg,#23C398,#d0c034);
                    background-repeat: repeat-x;
                    border-radius: 3px;
                    color:#fff;
            }

            .widget.c2 {
                background: #34679c;
                background-image: -webkit-linear-gradient(45deg,#1c69dc,#219bca);
                background-image: linear-gradient(45deg,#1c69dc,#219bca);
                background-repeat: repeat-x;
                border-radius: 3px;
                color:#fff;
            }

            .widget.c3 {
                    background: #34679c;
                    background-image: -webkit-linear-gradient(45deg,#1cdc9e,#9f4aec);
                    background-image: linear-gradient(45deg,#1cdc9e,#9f4aec);
                    background-repeat: repeat-x;
                    border-radius: 3px;
                    color:#fff;
            }

              .widget.c4 {
                    background: #f0cb27;
                    background-image: -webkit-linear-gradient(45deg,#9c7664,#fbd61e);
                    background-image: linear-gradient(45deg,#9c7664,#fbd61e);
                    background-repeat: repeat-x;
                    border-radius: 3px;
                    color:#fff;
            }

                .widget.c5 {
                    background: #9f4aec;
    background-image: -webkit-linear-gradient(45deg,#9c7664,#fbd61e);
    background-image: linear-gradient(45deg,#5f26d0,#c344ec);
    background-repeat: repeat-x;
    border-radius: 3px;
    color:#fff;
            }
                .widget.c6 {
                        background: #9abf52;
    background-image: -webkit-linear-gradient(45deg,#d0c326,#9abf52);
    background-image: linear-gradient(45deg,#d0c326,#9abf52);
    background-repeat: repeat-x;
    border-radius: 3px;
    color: #fff;
            }
                .widget.c7 {
                    background: #bb6dd8;
                    background-image: -webkit-linear-gradient(45deg,#e0b76b,#b15af3);
                    background-image: linear-gradient(45deg,#e0b76b,#b15af3);
                    background-repeat: repeat-x;
                    border-radius: 3px;
                    color: #fff;
                }*/

        .MakeRound {
            -webkit-transform: rotateY(150deg); /* Safari */
            transform: rotateX(360deg); /* Standard syntax */
            -webkit-transition: all 2s ease;
            transition: all 2s ease;
        }

        .zoom {
            transition: transform .2s;
            margin: 0 auto;
            cursor: pointer;
        }

            .zoom:hover {
                -ms-transform: scale(1.02);
                -webkit-transform: scale(0.5);
                transform: scale(1.05);
                z-index: 9999;
                animation: 5s shake infinite alternate;
            }

        .wdgLabel:hover {
        }


        @keyframes shake {
            0% {
                transform: skewX(-15deg);
            }

            5% {
                transform: skewX(15deg);
            }

            10% {
                transform: skewX(-15deg);
            }

            15% {
                transform: skewX(15deg);
            }

            20% {
                transform: skewX(0deg);
            }

            100% {
                transform: skewX(0deg);
            }
        }

        .cont {
            padding: 15px;
            padding-top: 0;
            border: 1px solid #d8d8d8;
            border-radius: 11px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.15);
            position: relative;
            /* margin: 15px 15px 15px 15px; */
            margin-top: 15px;
        }

        .comntLink {
            display: block;
            text-align: right;
        }

        .commentSecOld {
            margin-left: 59px;
            line-height: 4px;
            padding: 5px 15px 15px 10px;
            border: 1px solid #dedede;
            z-index: 9;
            position: relative;
            padding-bottom: 2px;
            background: #f6fbfa;
        }

            .commentSecOld > h5 {
                margin-top: 4px;
            }

            .commentSecOld:before {
                content: '';
                width: 0;
                height: 0;
                border-top: 10px solid transparent;
                border-bottom: 17px solid transparent;
                border-right: 9px solid rgba(183, 183, 183, 0.5);
                position: absolute;
                left: -9px;
            }

        commentSecOld > h5 {
            margin-bottom: 12px;
        }

        .comntList {
            padding: 0;
            padding: 0;
            padding-top: 12px;
            border-top: 2px solid #f3f3f3;
        }

            .comntList li {
                list-style-type: none;
                margin-bottom: 15px;
            }


        .comntList1 {
            padding: 0;
            padding: 0;
            padding-top: 12px;
            border-top: 2px solid #f3f3f3;
        }

            .comntList1 li {
                list-style-type: none;
                margin-bottom: 15px;
            }


        .iconHolder {
            font-size: 25px;
            width: 45px;
            height: 49px;
            border: 1px solid #ccc;
            text-align: center;
            border-radius: 50%;
            display: inline-block;
            line-height: 44px;
            overflow: hidden;
        }

        .CommentaddBox {
            border-radius: 6px;
        }

        .msgbody {
            line-height: 20px;
        }

        .loadallcmt {
            cursor: pointer;
            color: #080877;
        }

        .timeClass {
            font-size: 11px;
            font-weight: 100;
            padding-left: 10px;
            color: #918fa0;
        }

        #allcmt {
            max-height: 60vh;
            overflow-y: auto;
        }

        .mcomntsHeader {
            font-size: 15px;
            padding-left: 5px;
        }

        .navBoxes {
            background: #637db9;
            color: #fff;
            display: inline-block;
            margin-right: 20px;
            margin-top: 20px;
            padding: 20px 20px;
            text-align: center;
            cursor: pointer;
            border: 1px dashed #ffffff;
            font-size: 15px;
            background: rgb(180, 70, 255);
            -webkit-transition: all 0.5s;
            -o-transition: all 0.5s;
            -ms-transition: all 0.5s;
            -moz-transition: all 0.5s;
            transition: all 0.5s;
        }

            .navBoxes:nth-child(1) {
                background: -moz-linear-gradient(142deg, rgb(180, 70, 255) 28%, rgb(109, 104, 254) 100%);
                background: -webkit-linear-gradient(142deg, rgb(180, 70, 255) 28%, rgb(109, 104, 254) 100%);
                background: linear-gradient(142deg, rgb(180, 70, 255) 28%, rgb(109, 104, 254) 100%);
            }

            .navBoxes:nth-child(2) {
                background: -moz-linear-gradient(142deg, rgb(220, 173, 68) 25%, rgb(212, 129, 80) 100%);
                background: -webkit-linear-gradient(142deg, rgb(220, 173, 68) 25%, rgb(212, 129, 80) 100%);
                background: linear-gradient(142deg, rgb(220, 173, 68) 25%, rgb(212, 129, 80) 100%);
            }

            .navBoxes:nth-child(3) {
                background: -moz-linear-gradient(90deg, rgb(89, 155, 197) 12%, rgb(31, 127, 183) 100%);
                background: -webkit-linear-gradient(90deg, rgb(89, 155, 197) 12%, rgb(31, 127, 183) 100%);
                background: linear-gradient(90deg, rgb(89, 155, 197) 12%, rgb(31, 127, 183) 100%);
            }

            .navBoxes:nth-child(4) {
                background: -moz-linear-gradient(142deg, rgb(96, 202, 162) 25%, rgb(104, 171, 254) 100%);
                background: -webkit-linear-gradient(142deg, rgb(96, 202, 162) 25%, rgb(104, 171, 254) 100%);
                background: linear-gradient(142deg, rgb(96, 202, 162) 25%, rgb(104, 171, 254) 100%);
            }

            .navBoxes > div.icoWraper {
                font-size: 21px;
                /* display: inline-block; */
                /* margin: 0 auto; */
                margin-bottom: 10px;
            }

                .navBoxes > div.icoWraper i {
                    width: 38px;
                    height: 38px;
                    background: #fff;
                    border-radius: 50%;
                    color: #7764ff;
                    line-height: 39px;
                }
    </style>
   <%-- <link rel="stylesheet" href="https://daneden.github.io/animate.css/animate.min.css" />--%>
    <script>
        $(document).ready(function () {
            $('.childNavContainer .navBoxes').css({ 'opacity': '0' });
            $('.parentNavContainer .navBoxes').click(function () {
                $('.parentNavContainer').hide();
                $('.childNavContainer').removeClass('hide');
                $('.childNavContainer').find('.navBoxes').each(function (i) {
                    $(this).delay(500 * i).addClass('animated fast fadeInUp', 100);
                    //setTimeout(function () {
                    //    $(t).css({ 'opacity': '1' });
                    //}, 3000);
                });
            });

            $('button[data-function="pushSlide"]').click(function () {
                var SliderId = $(this).attr('data-target');
                //console.log('you clicked on slide' + SliderId);
                $(SliderId).removeClass('hidden');

            });
            $('.pushslider-close').click(function () {

                //console.log('you clicked on slide' + SliderId);
                $(this).closest('.pushslider').addClass('hidden');

            });


            //$.ajax({
            //    type: "POST",
            //    url: "ProjectMainPage.aspx/AnnouncementDetails",
            //    data: JSON.stringify({ reqStr: "" }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (msg) {
            //        var list = msg.d;
            //        var listItems = [];
            //        if (list.length > 0) {

            //            for (var i = 0; i < list.length; i++) {
            //                var head = '';
            //                var body = '';
            //                head = list[i].split('|')[0];
            //                body = list[i].split('|')[1];
            //            }
            //            $("#h1heading").text(head);
            //            $("#pParagraph").text(body);
            //        }
            //        else {
            //            $('#divPopHead').addClass("hide");
            //        }
            //    }
            //});

            
        });
    </script>
    <style>
        .pushslider {
            position: fixed;
            top: 0;
            bottom: 0;
            background: #ffffff;
            z-index: 10;
            right: 0;
            min-width: 500px;
            max-height: 100vh;
            overflow-y: auto;
            box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.23);
            -webkit-transition: all 0.3s ease-out;
            -moz-transition: all 0.3s ease-out;
            transition: all 0.3s ease-out;
        }

            .pushslider.hidden {
                opacity: 0;
                right: -100%;
            }

            .pushslider.full {
                width: 100%;
            }

        .pushslider-header {
            padding: 8px 15px;
            font-size: 16px;
            border-bottom: 1px solid #e8e8e8;
        }

        .pushslider-close {
            position: absolute;
            right: 15px;
            top: 7px;
            font-size: 18px;
            color: #72a7de;
            cursor: pointer;
        }

        .pushslider-content {
            padding: 15px;
        }

        .dashitems {
            display: flex;
            /* justify-content: space-around; */
            /* flex-basis: 250px; */
            flex-wrap: wrap;
        }

            .dashitems > div {
                margin-right: 2%;
                width: 18%;
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

    <!-- POPUP -->
    <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <h1 id="h1heading" runat="server"><%--Discussion heading--%></h1>
            <p id="pParagraph" runat="server">
                <%--Then system moves to ‘Dashboard’ or landing page where the entire control and
                    activity component will be available for service management.--%>
            </p>
            <button type="button" class="btn btn-sign" onclick="return PopupOk()">OK</button>
        </div>
    </div>
    <!-- POPUP -->


    <iframe id="loadeddiv" style="position: absolute; height: 100%; width: 98%; border: none; display: none;"></iframe>
    <button type="button " class="hide" data-target="#slideSider" data-function="pushSlide">Open from Side</button>
    <div class="pushslider col-md-8 hidden " id="slideSider">
        <div class="pushslider-header">Header </div>
        <div class="pushslider-close" data-close="#slideSider"><i class="fa fa-close"></i></div>
        <div class="pushslider-content">
            <p>Something from here</p>
        </div>
    </div>

    <div class="padding " id="fullpageDiv">
        <script src="Js/ProjectMainPage.js?v=0.13"></script>

        <div class="clearfix dashitems">
            <div class="widget c1" runat="server" id="SalesDbButton" visible="false" onclick="SalesDbButtonClick()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds1.png" />
                    </div>
                    <label class="wdgLabel">
                        Sales Analytics

                        <i class="fa fa-arrow-circle-right pull-right mTop5"></i>
                    </label>

                </div>
            </div>


            <div class="widget c2" runat="server" id="PurchaseDbButton" visible="false" onclick="PurchaseDbButtonClick()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds2.png" />
                    </div>
                    <label class="wdgLabel">
                        Purchase Analytics
                         <i class="fa fa-arrow-circle-right pull-right mTop5"></i>
                    </label>
                </div>
            </div>


            <div class="widget c3" runat="server" id="CRMButton" visible="false" onclick="CRMButtonClick()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds4.png" />
                    </div>
                    <label class="wdgLabel">CRM Analytics  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>

            <div class="widget c4" runat="server" id="Attbtn" visible="false" onclick="AttbtnButtonClick()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds5.png" />
                    </div>
                    <label class="wdgLabel">Today's Attendance  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>

            <div class="widget c5" runat="server" id="followupBtn" visible="false" onclick="FollowupButtonClick()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds6.png" />
                    </div>
                    <label class="wdgLabel">Payment Followup  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>
            <div class="widget c6" runat="server" id="AccountsBtn" visible="false" onclick="showDashboard()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds7.png" />
                    </div>
                    <label class="wdgLabel">Account Dashboard  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>
            <div class="widget c7" runat="server" id="tasklistbtn" visible="false" onclick="showTasklist()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds8.png" />
                    </div>
                    <label class="wdgLabel">Task List  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>
            <div class="widget c3 hide" onclick="showProjectlist()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds3.png" />
                    </div>
                    <label class="wdgLabel">Project Analytics  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>
            <div class="widget c9 hide" onclick="showProjectDB()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds3.png" />
                    </div>
                    <label class="wdgLabel">Project Dashboard  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>
            <div class="widget c10 hide" onclick="showFinancialDB()">
                <div class="">
                    <div class="iconH">
                        <img src="../../assests/images/ds3.png" />
                    </div>
                    <label class="wdgLabel">Financial Dashboard  <i class="fa fa-arrow-circle-right pull-right mTop5"></i></label>
                </div>
            </div>
        </div>




        <div class="mTop5 col-md-12 pTop10" style="display: none" id="mainAnc">
            <h3>Today's Announcement <i class="fa fa-bullhorn"></i></h3>

            <div class="row" id="announcementDiv">
            </div>
        </div>
    </div>

    <div class="parentNavContainer hide">
        <div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>Sales Analytics</div>
            </div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>Purchase Analytics</div>
            </div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>CRM Analytics</div>
            </div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>Payment Followup</div>
            </div>
        </div>
    </div>
    <div class="childNavContainer hide">
        <div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>Sales Analytics</div>
            </div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>Purchase Analytics</div>
            </div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>CRM Analytics</div>
            </div>
            <div class="navBoxes">
                <div class="icoWraper"><i class="fa fa-users"></i></div>
                <div>Payment Followup</div>
            </div>
        </div>
    </div>
    <div class="childNavContainer"></div>
    <div class="modal fade" id="allComment" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <div class="modal-body">
                    <div>
                        <table width="100%">
                            <tr>
                                <td width="90%" class="mcomntsHeader">All Comment(s)</td>
                                <td width="10%" style="text-align: right;"><span data-toggle="tooltip" title="Click to close" data-dismiss="modal" style="cursor: pointer"><i class="fa fa-times closePopup"></i></span></td>
                            </tr>
                        </table>
                    </div>
                    <div id="allcmt"></div>
                </div>
            </div>

        </div>
    </div>

    <div id="CustomNewNotify" class="clearfix">Some text some message..</div>
    <asp:HiddenField ID="hdnDefaultDashboardService" runat="server" />

</asp:Content>
