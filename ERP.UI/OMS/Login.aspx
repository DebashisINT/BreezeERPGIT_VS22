
<%--====================================================== Revision History ===========================================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                12-01-2023        2.0.35           Pallab              Change login url attached image(Image show when url share in social media)
====================================================== Revision History ===========================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="pLogin"
    EnableEventValidation="false" CodeBehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login to BreezeERP</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <%--<script type="text/javascript" src="/assests/js/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.1.min.js"></script>

     
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@700;800&family=Playfair+Display:wght@900&display=swap" rel="stylesheet">

    <link rel="stylesheet"  href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css" />
    <link href="../assests/css/jquery-ui.css" rel="stylesheet" />
    <link href="/assests/pluggins/slick/slick.css" rel="stylesheet" />
    
  
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
    <%--<script type="text/javascript" src="/assests/js/modernizr-2.8.3-respond-1.4.2.min.js"></script>--%>
    <meta name="theme-color" content="#0C78B1" />
    <!-- Windows Phone -->
    
    <meta name="msapplication-navbutton-color" content="#0C78B1" />
    <!-- iOS Safari -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <script type="text/javascript" src="/assests/js/mobile-detect.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.11.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/assests/js/jquery.confirm.min.js"></script>
    <script type="text/javascript" src="/assests/js/modalmsg.js"></script>
    <script type="text/javascript" src="/assests/pluggins/slick/slick.min.js"></script>
    
    <script language="javascript" type="text/javascript">
        window.history.forward();
        function noBack() { window.history.forward(); }
        function ForNextPage() {
            //window.open('management/ProjectMainPage.aspx','windowname1','fullscreen=yes,titlebar=no,toolbar=no,statusbar=no');
        }

        $(function () {
            localStorage.removeItem("hdnCopymenu");
            localStorage.removeItem("TaxDetailsForSale");
            localStorage.removeItem("fv");
            localStorage.removeItem("verisonUpdate");
            localStorage.removeItem("versionData");

        });

        function blinkIt() {
            if (!document.all) return;
            else {
                for (i = 0; i < document.all.tags('blink').length; i++) {
                    s = document.all.tags('blink')[i];
                    s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
                }
            }
        }

        function removeCookiesKeyFromStorage() {
            var data = localStorage.getItem("GridCookiesId");
            if (data != null) {
                var splitCookiesData = data.split(',');
                for (var i = 1; i < splitCookiesData.length; i++) {
                    eraseCookie(splitCookiesData[i]);
                }
            }

            localStorage.setItem("GridCookiesId", "");
        }

        function eraseCookie(name) {
            createCookie(name, "", -1);
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }
        function removeMultitabCookies() {
            eraseCookie("ERPACTIVEURL");
        }
        removeMultitabCookies();
        removeCookiesKeyFromStorage();


        function ChangeCompany() {

            document.getElementById("divConn").style.display = "none";
            if ($("#hdnCompanyCount").val()=="2"){
                document.getElementById("btnLoginCls").style.display = "none";
                document.getElementById("btnProceed").style.display = "block";
            }
            else
            {
                document.getElementById("btnProceed").style.display = "none";
                document.getElementById("btnLoginCls").style.display = "block";
            }
        }

        function button_click(objTextBox, objBtnID) {
            if (window.event.keyCode == 13) {
                document.getElementById(objBtnID).focus();
                document.getElementById(objBtnID).click();
            }
        }

        $.ajax({
            type: "POST",
            url: "Login.aspx/getevent",
            contentType: "application/json; charset=utf-8",
            //data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                console.log('login', data)
                var data = data.d;
                var dateToday = new Date(); 
                for (i = 0; i < data.length; i++) {
                    var makeTostrS = (new Date(parseInt(data[i].EVENT_START_DATE.substr(6))));
                    var makeTostrE = (new Date(parseInt(data[i].EVENT_END_DATE.substr(6))));
                    var imageUri = data[i].EVENT_IMG_URL;
                    if (makeTostrS.setHours(0, 0, 0, 0) <= dateToday.setHours(0, 0, 0, 0) && makeTostrE.setHours(0, 0, 0, 0) >= dateToday.setHours(0, 0, 0, 0) && data[i].ISACTIVE == true) {
                        $('.greetWrap').removeClass('hide')
                        $('.greetWrap').find('img').attr('src', imageUri)
                    } else {
                       
                    }
                }  
            }
        })
        $.ajax({
            type: "POST",
            url: "Login.aspx/getEnt",
            contentType: "application/json; charset=utf-8",
            //data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                console.log('ent', data)
                var name = data.d[0].en;
                if (name == "Lite") {
                    $("#privilage").show().text("Lite Edition");
                } else {
                    $("#privilage").show().text("Enterprise Edition");
                }
            }
        })
    </script>
   
    <script type="text/javascript">
        $(document).ready(function () {
            $('.multiple-items').slick({
                infinite: false,
                slidesToShow: 5,
                autoplay: true,
                slidesToScroll: 2,
                prevArrow: '<button class="button-7 slide-arrow prev-arrow"><i class="fa fa-arrow-left"></i></button>',
                nextArrow: '<button class="button-7 slide-arrow next-arrow"><i class="fa fa-arrow-right"></i></button>'
            });
            $('#ismobile').val('0');
            //var md = new MobileDetect(window.navigator.userAgent);
            //if (md.mobile()) {
            //    if ($('#ismobile').val() == '0' || $('#ismobile').val() == '') {
            //        $.confirm({
            //            title: 'What do you want to visit',
            //            content: 'Choose one to continue',
            //            buttons: {
            //                Report: {
            //                    text: 'MIS Reports',
            //                    btnClass: 'btn-purple',
            //                    keys: ['enter', 'shift'],
            //                    action: function () {
            //                        $('#ismobile').val('1');
            //                    }
            //                },
            //                Erp: {
            //                    text: 'BreezeERP',
            //                    btnClass: 'btn-dark',
            //                    keys: ['enter', 'shift'],
            //                    action: function () {
            //                        $('#ismobile').val('0');
            //                    }
            //                }
            //            }
            //        });
            //    }else{} 

            //} else {
            //    $('#ismobile').val('0');
            //}
           
        });  
    </script>
   <link rel="stylesheet" href="/assests/css/login.css?1.0.2" />
    <style type="text/css">
        .slick-slide {
          margin: 0 20px;
        }
        @font-face {
            font-family: 'opcen';
            src: url('/assests/fonts/OpenSans-CondBold.ttf') format('truetype');
        }
        .centerd-box {
                top: auto !important;
                    padding: 24px 50px;
        }
        .greetWrap {
            position: fixed;
            right: 15px;
            top: 15px;
            width: 121px;
            height: 121px;
            border-radius: 50%;
            overflow: hidden;
            z-index: 99;
            box-shadow: 0px 3px 5px rgba(0,0,0,0.12);
        }
        .greetWrap >img {
            max-width: 100%;
            max-height: 100%;
            position: absolute;
        }
    </style>
    <style type="text/css">
        #privilage {
            display: inline-block;
            background: #343f90;
            padding: 3px 13px;
            border-radius: 4px;
            color: #e4e1e1;
            display:none;
        }
        .passWordView {
            position: absolute;
            right: 5px;
            top: 7px;
            width: 20px;
            height: 20px;
            font-size:16px;
            display: flex;
           
            justify-content: center;
            align-items: center;
            border-radius: 5px;
            cursor:pointer;
        }
        
        .passWordView:hover {
            background:#dbd7ec
        }
        
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".passWordView").click(function () {
                $(this).toggleClass("shActive");
                var icon = $(this).find(".fa");
                if (icon.hasClass("fa-eye")) {
                    icon.removeClass("fa-eye").addClass("fa-eye-slash");
                } else {
                    icon.removeClass("fa-eye-slash").addClass("fa-eye");
                }
                var x = document.getElementById("txtPassword");
                if (x.type === "password") {
                    x.type = "text";
                } else {
                    x.type = "password";
                }
            });

            $(".secToggler").on("click", function () {
                $(".droper").toggleClass("active");
            });
            $("#clCr").click(function () {
                $(".droper").removeClass("active");
            });
        });
    </script>
   


    <!--New login -->
    <style type="text/css">
        

        body {
            font-family: "Poppins", sans-serif;
            overflow-x:hidden
        }

        .container {
            max-width: 1160px;
            margin: 0 auto;
        }

        .relative {
            position: relative;
        }

        .setHeight {
            min-height: calc(100vh - 100px);
        }

        .mLogo {
            background: #fff;
            display: inline-block;
            border-radius: 9px;
            padding: 9px 10px;
            text-align: center;
            margin-top: 20px;
            box-sizing: border-box;
                box-shadow: 1px 1px 10px #1111112e;
        }

        .mLogo > img {
           width: 180px;
        }

        .sectionWraper {
            display: flex;
            /*justify-content: space-between;*/
            z-index: 100;
            position: relative;
        }

        .textSection h1 {
            /*font-family: 'opcen', Montserrat, sans-serif !important;*/
            font-family: 'Open Sans', sans-serif;
            font-weight: 800;
            font-size: 41px;
            margin-bottom: 10px;
            color: #1a1a1a;
            /*font-family: revert;*/
            line-height: 51px;
        }

        .scndSize {
           font-size: 20px;
            margin: 0px;
            font-weight:normal;
            margin-bottom: 14px;
            color: #505050;
            margin-top: 16px;
            margin-left: 0px;
            font-family: revert;
                font-family: "Segoe UI","Helvetica Neue",Helvetica,Verdana,"san-serif";
        }

        .textSection h1 .emphasized {
            font-weight: 800;
            color: #f35c6d;
        }

        .bgimage {
            position: absolute;
            right: -7px;
            top: 5px;
            max-width: 600px;
        }

        .formBox {
            background: #fff;
            border-radius: 24px;
            width: 35%;
            /*margin-top: 80px;*/
            /*margin-right:50px;*/
            margin-right:0;
            /*box-shadow: 0px 8px 18px 4px rgb(0 0 0 / 10%);*/
            box-shadow: 0px 8px 18px 4px rgb(25 3 88 / 30%);
            margin-left: 50px;
        }

        .wlHeader {
            font-family: "Playfair Display", serif;
            color: #062aa5;
            margin-bottom: 0px !important;
            font-size: 30px;
            font-weight: 700;
                text-align: center;
        }

        .f2Para {
            font-family: revert;
            margin-top: 5px !important;
            font-size: 14px;
                text-align: center;
        }

        .bliner {
            display: inline-block;
            background: #f35c6d;
            padding: 5px 15px;
            color: #fff;
            font-weight: 600;
            font-size: 21px;
        }
        .textSection {
            /*margin-top: 30px;*/
            width: 55%;
        }

        .inputwraps {
            position:relative;
            margin-bottom:15px
        }
        .inputL {
            width:100%;
            border:1px solid #d1d1d1;
            height:45px;
            box-sizing:border-box;
            border-radius:50px;
            padding-left:45px
        }
        .inputL:focus {
            outline:none;
            border:2px solid #9AA4E6;
        }
        .callArea {
            padding:15px 0;
            margin-bottom:15px
        }
        .submitButton {
            /*background:#8a57f1;
            border:none;
            padding:8px  15px 8px 15px;
            color:#fff;
            border-radius:6px;*/
            background: #5d23d1;
            border: none;
            padding: 11px 15px 11px 15px;
            color: #fff;
            border-radius: 50px;
            width: 100%;
            font-size: 16px;
            font-weight: 500;
            height: 45px;
        }
        .submitButton:hover {
                background: #7035E6;
            /*border: 1px solid #642bd5;*/
            box-shadow: 0px 4px 5px rgb(0 0 0 / 20%);
            color: #fff;
        }
        .submitButton:focus
        {
            color: #fff;
            border:none;
            outline: none;
            background: #7035E6;
            box-shadow: 0px 4px 5px rgb(0 0 0 / 20%);
        }
        .submitButton>img{
            max-width: 28px;
            /* float: right; */
            margin-left: 10px;
        }
        .fPass {
            
            color:#444242;
            margin-left:20px;
            text-decoration:underline;
            
            font-size:12px
        }
        .ftFooter {
            text-align:center;
            border-top:1px solid #d7d7d7;
            padding:12px;
            font-size:13px;
            line-height:25px;
            background: #e7e7e7;
    border-radius: 0 0 25px 25px;
        }
        .inputImgs {
            position:absolute;
            left: 12px;
            top: 11px;
            /*max-width:18px;*/
        }
        .passWordView {
            position: absolute;
            right: 14px;
            
            top: 14px;
            width: 21px;
            height: 20px;
            font-size: 13px;
            display: flex;
            justify-content: center;
            align-items: center;
            border-radius: 5px;
            cursor: pointer;
        }
        .bg11 {
            min-height:250px;
            background:#333 url('/assests/images/NLoginIcon/bg11.jpg');
            background-size: cover;
            background-position: center;
            padding:30px 0
        }

        @media screen and (min-width: 480px) {
            .nameCR {
                font-weight: 500;
            }

            .creditsSec {
                position: absolute;
                left: 20px;
                bottom: 20px;
                z-index: 109;
            }

            .droper {
                position: absolute;
                bottom: 90%;
                color: #dfdfdf;
                background: #192b4c;
                min-width: 250px;
                border-radius: 11px;
                overflow: hidden;
                display:none;
                opacity: 0;
                -webkit-transition: all 0.3s ease-in-out;
                transition: all 0.3s ease-in-out;
                box-shadow: 0px 0px 15px rgb(0 0 0 / 20%);
            }

                .droper.active {
                    display:block;
                    opacity: 1;
                    bottom: 120%;
                }

            .mnHeader {
                padding: 7px 15px;
                border-bottom: 1px solid #0b1322;
                font-size: 1.6rem;
                font-weight: 500;
                text-transform: uppercase;
            }

            .scrCnter {
                padding: 8px 12px;
                max-height: 300px;
                overflow-y: auto;
            }

                .scrCnter ul {
                    list-style-type: none;
                    padding: 0;
                }

                    .scrCnter ul > li {
                        padding: 7px 0 7px 7px;
                        border-bottom: 1px solid #273958;
                    }

                        .scrCnter ul > li.crhdss {
                            background: #e2f5e7;
                            font-weight: 500;
                            border-bottom: 1px dashed;
                        }

            

            .bage {
                display: inline-block;
                font-size: 80%;
                border: .5px solid #63dca2;
                color: #2eaf71;
                padding: 0px 2px;
                border-radius: 4px;
                float: right;
            }

                .bage.c1 {
                    border: 1px solid #63dca2;
                    color: #2eaf71;
                }

                .bage.c2 {
                    border: 1px solid #709e25;
                    color: #709e25;
                }

                .bage.c3 {
                    border: 1px solid #e28c20;
                    color: #e28c20;
                }

                .bage.c4 {
                    border: 1px solid #ea5c09;
                    color: #ea5c09;
                }

                .bage.c5 {
                    border: 1px solid #0939ea;
                    color: #0939ea;
                }

                .bage.c6 {
                    border: 1px solid #7f09ea;
                    color: #7f09ea;
                }

                .bage.c7 {
                    border: 1px solid #e02bca;
                    color: #e02bca;
                }

            #clCr {
                float: right;
                cursor: pointer;
            }
        }
        .secToggler {
            width: 35px;
            height: 35px;
            text-align: center;
            border: 1px solid #9ab2ef;
            border-radius: 50%;
            display: flex;
            justify-content: center;
            align-items: center;
            transition: all 0.3s ease-in;
            cursor: pointer;
            background: #333;
        }

        .secToggler i {
            font-size: 23px;
            cursor: pointer;
            display: inline-block;
        }

        .secToggler:hover {
            background: #333;
            border-color: #fff;
            box-shadow: 0 0 0 3px #288fce, 0 0 0 4px rgb(255 255 255 / 20%);
        }
        .aCenter {
            text-align:center
        }
        .sectionHeader .hd, .sectionHeader p {
            color:#fff;
            font-family: revert;
        }
        .lister li {
            display: inline-block;
            min-width: 180px;
        }
        .lister li:not(:last-child) {
            margin-right: 35px;
        }
        .boxDB {
            background:#fff;
            border-radius:15px;
            padding:25px 15px;
            box-shadow:0px 10px 10px rgba(0,0,0,0.08);
            min-height:180px
        }
        .boxDB>img {
            max-width:70px
        }
        .boxDB .hding {
            font-weight:700;
            font-size:16px;
            margin-top:15px
        }
        .animatedIco {
            position:absolute;
        }
        .a1 {
            top: 0;
            left: -46px;
            /* transform: rotate(90deg); */
            opacity: 0.2;
        }
        .a2 {
            top: 100px;
            left: 40%;
            opacity: 0.2;
            -webkit-animation: movable 15s infinite linear;
        }
        .a3 {
            bottom: 100px;
            left: 54%;
            opacity: 0.3;
        }
        .a4 {
                bottom: 102px;
                left: 20%;
                opacity: 0.3;
        }
        .a5 {
            top: 190px;
            left: 20%;
            opacity: 0.1;
        }
        .a6 {
            top: 190px;
            left: 58%;
            opacity: 0.2;
            -webkit-animation: rotation 6s infinite linear;
        }
        .a7 {
            bottom: 200px;
            left: -50px;
            transform: rotate(90deg);
            opacity: 0.2
        }


        @-webkit-keyframes rotation {
		    from {
				        -webkit-transform: rotate(0deg);
		    }
		    to {
				        -webkit-transform: rotate(359deg);
		    }
        }
        @-webkit-keyframes movable {
		    from {
                -webkit-transform: scale(0.5);
              }
              25% {
                  top:-40px;
                  left:40%;
                  -webkit-transform: scale(0.7);
              }  
              75% {
                  top:100px;
                  left:50%;
                  -webkit-transform: scale(0.8);
              }
              to {
                -webkit-transform: scale(1);
              }
        }
    </style>
    <style type="text/css">
        #scene, #sceneDark {
            display:none
        }

         .screenLight #scene,  .screenDark #sceneDark{
            display:block;
        }
        .iconBXX {
            display: inline-block;
            width: 50px;
            height: 50px;
            background: #2d5a7a;
            margin-left: 10px;
            border-radius: 10px;
            /* box-shadow: 0px 3px 5px rgb(0 0 0 / 30%); */
            padding: 8px;
            text-align: center;
        }
        .screenLight .iconBXX {
            background: #d9e2e9;
        }
        .iconBXX>img {
            max-width:100%
        }
        .screenDark  .bgforDark {
            background:url('/assests/images/NLoginIcon/loginDark_bg.png') no-repeat top left;
            background-size: cover;
        }
        .screenDark  .animatedIcc {
            display:none
        }
        .screenDark .textSection h1, .screenDark .scndSize {
            color:#fff
        }
        .nRota {
            -webkit-animation: rotation 10s infinite linear;
        }

        /*switch*/
        #switchArea {
            position: absolute;
            bottom: 11px;
            left: 60px;
            z-index: 9;
            padding: 10px;
        }
        .switch {
              position: relative;
                display: inline-block;
                width: 40px;
                height: 26px;
            }

            .switch input { 
              opacity: 0;
              width: 0;
              height: 0;
            }

            .slider {
                position: absolute;
                cursor: pointer;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background-color: #c9c9c9;
                -webkit-transition: .4s;
                transition: .4s;
                border: 1px solid #163f5f;
            }

            .slider:before {
              position: absolute;
                content: "";
                height: 18px;
                width: 18px;
                left: 4px;
                bottom: 3px;
                background-color: #133e5e;
                -webkit-transition: .4s;
                transition: .4s;
            }

            input:checked + .slider, input:checked:focus + .slider {
              background-color: #f7f96a;
            }
            
            input:focus + .slider {
              background-color: #fff;
            }

            input:checked + .slider:before {
                  -webkit-transform: translateX(14px);
    -ms-transform: translateX(14px);
    transform: translateX(14px);
            }

            /* Rounded sliders */
            .slider.round {
              border-radius: 24px;
            }

            .slider.round:before {
              border-radius: 50%;
            }
            .spEmpa {
                font-weight: 600;
                color: #3a5ac6;
                font-family: 'Playfair Display', serif;
            }
            .screenDark  .spEmpa {
                color: #fff !important;
                border-bottom: 3px solid #dbdb2b;
            }
            .screenDark  .mainlist li {
                color:#fff
            }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".switch input").click(function () {
                var input = $(".switch input").is(':checked');
                var bodyClass = $("#themeClass").hasClass("screenDark");
                var theme = localStorage.getItem('theme');

                //alert(bodyClass)
                if (bodyClass) {
                    $("#themeClass").removeClass("screenDark").addClass("screenLight");
                    localStorage.setItem('theme', 'screenLight');
                } else {
                    $("#themeClass").removeClass("screenLight").addClass("screenDark");
                    localStorage.setItem('theme', 'screenDark');
                }
            })
        })
        window.addEventListener('load', function (event) {
            var theme = localStorage.getItem('theme').toString();
            console.log(theme)
            if (theme != '' || theme != undefined) {
                $("#themeClass").attr('class', '').addClass(theme);
            } else {
                $("#themeClass").addClass('screenDark');
            }

        });
    </script>
    <style>
        .styleList {
            list-style-type:none;
            margin-top:23px;
            display:block;
            padding-left:15px
        }
        .styleList li {
                font-family: "Segoe UI","Helvetica Neue",Helvetica,Verdana,"san-serif";
            line-height:35px;
            font-size:1.7rem;
            padding-left:30px;
            background:transparent url("/assests/images/verified.png") no-repeat center left
        }
        .socialIcons {
            list-style-type:none;
            margin-top:25px
        }
        .socialIcons>li {
            display: inline-block;
        }
         .socialIcons>li {
             margin-right:20px
         }
        .socialIcons>li>div{
            width: 25px;
            display: flex;
            height: 25px;
            background: #ebebeb;
            border-radius: 4px;
            /* line-height: 118px; */
            justify-content: center;
            align-items: center;;
        }
        .lgLink {
            background: #6967bf;
            color: #fff;
            display: inline-block;
            padding: 5px 15px;
        }
        @media only screen and (max-width: 762px){
            .textSection {
                display:none
            }
            .formBox {
                margin:50px auto
            }
            .container {
                width:100%
            }
            .lister >li {
                margin-bottom:10px
            }
            .creditsSec#rc_app_1562 {
                display:none !important

            }
            .lister li:last-child {
                margin-right: 35px ;
            }
        }
        @media only screen and (min-width: 763px){ 

            .onlySm {
                display:none;
            }
        }
        /*login button*/
        .cta {
          position: relative;
          font-size:18px;
          margin: auto;
          padding: 19px 22px;
          transition: all 0.2s ease;
          text-decoration: none;
            color: inherit;
        }
        .cta:before {
          content: "";
          position: absolute;
          top: 0;
          left: 0;
          display: block;
          border-radius: 28px;
          background: rgba(119, 118, 208, 0.5);
          width: 56px;
          height: 56px;
          transition: all 0.3s ease;
        }
        .cta span {
          position: relative;
            font-size: 13px;
            line-height: 18px;
            font-weight: 600;
            letter-spacing: 0.25em;
            text-transform: uppercase;
            vertical-align: middle;
        }
        .cta svg {
          position: relative;
          top: 0;
          margin-left: 10px;
          fill: none;
          stroke-linecap: round;
          stroke-linejoin: round;
          stroke: #111;
          stroke-width: 2;
          transform: translateX(-5px);
          transition: all 0.3s ease;
        }
        .cta:hover {
            text-decoration:none;
            color:#fff
        }
        .cta:hover:before {
          width: 100%;
          background: #6d6cdd;
          top: 4px;
          height: 55px;
        }
        .cta:hover svg {
          transform: translateX(0);
        }
        .cta:active {
          transform: scale(0.96);
        }
        /*Effect Social*/
        .effect .buttons {
          margin-top: 50px;
          display: flex;
          justify-content: center;
        }
       
        .effect a {
            text-decoration: none !important;
            color: #fff;
            width: 40px;
            height: 40px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 10px;
            margin-right: 20px;
            font-size: 17px;
            overflow: hidden;
            position: relative;
        }
        .effect a i {
          position: relative;
          z-index: 3;
        }
        .effect a.fb {
          background-color: #3b5998;
        }
        .effect a.tw {
          background-color: #00aced;
        }
        .effect a.g-plus {
          background-color: #dd4b39;
        }
        .effect a.dribbble {
          background-color: #ea4c89;
        }
        .effect a.pinterest {
          background-color: #cb2027;
        }
        .effect a.insta {
          background-color: #bc2a8d;
        }
        .effect a.in {
          background-color: #007bb6;
        }
        .effect a.vimeo {
          background-color: #1ab7ea;
        }
        .effect.egeon a {
          transition: transform 0.2s linear 0s, border-radius 0.2s linear 0.2s;
        }
        .effect.egeon a i {
          transition: transform 0.2s linear 0s;
        }
        .effect.egeon a:hover {
          transform: rotate(-90deg);
          border-top-left-radius: 50%;
          border-top-right-radius: 50%;
          border-bottom-left-radius: 50%;
        }
        .effect.egeon a:hover i {
          transform: rotate(90deg);
        }
        .slickCont {
            position:relative;
            margin-bottom:-50px;
            
        }
        .slickCont .boxDB>img {
           margin:auto
        } 
        .slickCont .slide-arrow {
            position:absolute;
            top:-40px;
            right:20px;
        }
        .slickCont .slide-arrow.prev-arrow {
            right:55px
        }
        .slickCont .slick-track{
            padding-bottom:50px
        }
        
        /* button */
        .button-7 {
          background-color: #0095ff;
          border: 1px solid transparent;
          border-radius: 3px;
          box-shadow: rgba(255, 255, 255, .4) 0 1px 0 0 inset;
          box-sizing: border-box;
          color: #fff;
          cursor: pointer;
          display: inline-block;
          font-family: -apple-system,system-ui,"Segoe UI","Liberation Sans",sans-serif;
          font-size: 11px;
          font-weight: 400;
          line-height: 1.15385;
          margin: 0;
          outline: none;
          padding: 8px .8em;
          position: relative;
          text-align: center;
          text-decoration: none;
          user-select: none;
          -webkit-user-select: none;
          touch-action: manipulation;
          vertical-align: baseline;
          white-space: nowrap;
        }

        .button-7:hover,
        .button-7:focus {
          background-color: #07c;
        }

        .button-7:focus {
          box-shadow: 0 0 0 4px rgba(0, 149, 255, .15);
        }

        .button-7:active {
          background-color: #0064bd;
          box-shadow: none;
        }

/* CSS */
.button-26 {
    margin-top:15px;
  appearance: button;
  background-color: #201f66;
  border: 1px solid #201f66;
  /*border-radius: 4px;*/
  border-radius:50px;
  box-sizing: border-box;
  color: #FFFFFF;
  cursor: pointer;
  font-family: Graphik,-apple-system,system-ui,"Segoe UI",Roboto,Oxygen,Ubuntu,Cantarell,"Fira Sans","Droid Sans","Helvetica Neue",sans-serif;
  font-size: 14px;
  line-height: 1.15;
  overflow: visible;
  padding: 12px 16px;
  position: relative;
  text-align: center;
  text-transform: none;
  transition: all 80ms ease-in-out;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  width: fit-content;
}

.button-26:disabled {
  opacity: .5;
}

.button-26:focus {
  outline: 0;
  color:#fff;
  text-decoration:none;
}

.button-26:hover {
  background-color: #0A46E4;
  border-color: #0A46E4;
  color:#fff;
  text-decoration:none;
}

.button-26:active {
  background-color: #0039D7;
  border-color: #0039D7;
  text-decoration:none;
}

.screenDark .button-26{
    background-color: #dbdb2b;
    border: 1px solid #cbcb23;
    color: #0a0a0a;
}
.screenDark .button-26:hover {
  background-color: #e9e914;
  border-color: #e9e914;
  color:#0a0a0a;
}
.screenDark .button-26:focus {
  outline: 0;
  color:#0a0a0a;
  text-decoration:none;
}

.sectionWraper
{
    align-items: center;
    margin-top: 10%;
}

.event-img
{
    /*display: none;*/
    border-radius: 10px;
    margin-top: 15px;
    width: 340px;
    height: 152px;
    overflow: hidden;
    box-shadow: 1px 1px 15px #00000063;
}
.event-img img
{
    border-radius: 10px;
    box-shadow: 1px 1px 10px #11111160;
    width: 100%;
}

.cust-position
{
    left: -30px !important;
    top: -20px !important;
}

#ddlConn.form-control
{
    border-radius: 50px;
    min-height: 45px;
    padding-left: 41px;
}

.database-dropdown
{
    position: relative;
}

.screenLight .bgforDark
{
    /*background: linear-gradient(158deg, rgb(44 9 150 / 10%) 0%, rgb(116 77 233 / 10%) 100%);*/
    background-image: url(/assests/images/login-total-bg.jpg);
}
    </style>


</head>
<body onload="noBack();setInterval('blinkIt()',500);" onpageshow="if (event.persisted) noBack();"
    onunload="" class="">

    <!--New login -->
    
    <div id="switchArea" class="">
        <label class="switch">
          <input type="checkbox" checked />
          <span class="slider round"></span>
        </label>
    </div>
    <div id="themeClass" class="screenLight">
      <div class="relative setHeight bgforDark" style="margin-bottom:35px;min-height:100vh;overflow:hidden;">
            <%--<div id="scene" style="position:absolute;right:-50px;top:-50px;">--%>
            <div id="scene" style="position:absolute;right:-100px;top: 0;">
            <%--<div class="layer" data-depth="0.1"><img src="/assests/images/bkfive.png" alt="" style="max-width: 600px;" /></div>  
          <div class="layer" data-depth="0.6"><img src="/assests/images/bkone.png" alt="" style="max-width: 600px;" /></div>
          <div class="layer" data-depth="0.2"><img src="/assests/images/bktwo.png" alt="" style="max-width: 600px;" /></div>  
            <div class="layer" data-depth="0.8"><img src="/assests/images/bkthree.png" alt="" style="max-width: 600px;" /></div>
            <div class="layer" data-depth="0.1"><img src="/assests/images/bkfour.png" alt="" style="max-width: 600px;" /></div>     --%>
             <%--<div class="layer" data-depth="0.3"><img src="/assests/images/NLoginIcon/milky.png" alt="" style="max-width: 800px;" /></div>--%>
                <div class="layer cust-position" data-depth="0.3"><img src="/assests/images/NLoginIcon/milky-2.png" alt="" style="height: 100vh;" /></div>
            <%--<div class="layer" data-depth="0.1"><img src="/assests/images/NLoginIcon/bg1Gr.png" alt="" style="max-width: 800px;" /></div>--%>
            <div class="layer" data-depth="0.1"><img src="/assests/images/NLoginIcon/login-fm-bg.png" alt="" style="height: 100vh;" /></div>
        </div>
        <div id="sceneDark" style="position:absolute;right:0px;top:0px;">
            <%--<div class="layer" data-depth="0.1"><img src="/assests/images/bkfive.png" alt="" style="max-width: 600px;" /></div>  
          <div class="layer" data-depth="0.6"><img src="/assests/images/bkone.png" alt="" style="max-width: 600px;" /></div>
          <div class="layer" data-depth="0.2"><img src="/assests/images/bktwo.png" alt="" style="max-width: 600px;" /></div>  
            <div class="layer" data-depth="0.8"><img src="/assests/images/bkthree.png" alt="" style="max-width: 600px;" /></div>
            <div class="layer" data-depth="0.1"><img src="/assests/images/bkfour.png" alt="" style="max-width: 600px;" /></div>     --%>
             <div class="layer" data-depth="0.5"><img src="/assests/images/NLoginIcon/bgg2.png" class="" alt="" style="max-width: 620px;" /></div>
            <div class="layer" data-depth="0.1"><img src="/assests/images/NLoginIcon/bgg3.png" alt="" style="max-width: 620px;" /></div>
        </div>
        <div class="animatedIcc">
            <img class="animatedIco a1" src="/assests/images/NLoginIcon/a1.png" alt="" />
            <img class="animatedIco a2" src="/assests/images/NLoginIcon/a2-2.png" alt="" />
            <img class="animatedIco a3" src="/assests/images/NLoginIcon/a3.png" alt="" />
            <img class="animatedIco a4" src="/assests/images/NLoginIcon/a4.png" alt="" />
            <img class="animatedIco a5" src="/assests/images/NLoginIcon/a5.png" alt="" />
            <img class="animatedIco a6" src="/assests/images/NLoginIcon/a6.png" alt="" />
            <img class="animatedIco a7" src="/assests/images/NLoginIcon/a7.png" alt="" />
        </div>
      <div class="container" >
          
        <div class="sectionWraper">
          <div class="textSection">
              <div class="d-flex">
                  <div class="mLogo">
                    <img src="/assests/images/logo.png"  />
                  </div>
                  <div class="event-img">
                    <img src="/assests/images/event-banner.jpg"  />
                </div>
            </div>
            <h1>Power-packed <span class="spEmpa" style="font-weight: 600;color: #062aa5;">Digital ERP</span> For <br />
                Indian Businesses.</h1>
            <h3 class="scndSize" style="display:none">We are focussed on getting your business disruption-ready.</h3>
              <h3 class="scndSize" style="line-height: 27px;font-size: 16px;">BreezeERP is digital first CLOUD ERP that AUTOMATES & CONNECTS <br />
                  all aspects of your business with a built in INTELLIGENCE that proactively manages<br />
                   your business process and tasks letting you focus on the core of your business. <br />
                  Lead your team from a single platform with real time updates and analytics.</h3>

              <a href="https://www.breezeerp.in/contact-us/" target="_blank" class="button-26">START MY FREE TRIAL</a>
            
            <h3 class="scndSize hide"> <span class="bliner txt-rotate" 
                data-period="2000"
                data-rotate='[ "Payroll Management ", "Service Management ", "STB Management", "Employee Self Service", "Sales and Purchase" ]'></span></h3>
            <div style="margin-top:20px" class="hide">
                <span class="iconBXX"><img src="/assests/images/NLoginIcon/invoice1.png" /></span>
                <span class="iconBXX"><img src="/assests/images/NLoginIcon/export.png" /></span>
                <span class="iconBXX"><img src="/assests/images/NLoginIcon/global.png" /></span>
            </div>
          </div>
          <div class="formBox relative">
              <div class="onlySm aCenter" style="z-index:9999;"><div class="mLogo"><img src="/assests/images/logo.png"  /></div></div>
              <div style="padding: 10px 30px;">
                <%--<h1 class="wlHeader">Welcome ...</h1>--%>
                  <h1 class="wlHeader">Login</h1>
                <p class="f2Para">Please enter your username and password</p>

                  <div style="margin-top: 20px">
                      <div>
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" CssClass="hide"></asp:Label>
                      </div>
                      <form action="" method="post" runat="server" novalidate="novalidate">
                        <input id="rurl" name="rurl" runat="server" type="hidden" value="" />
                      <div class="inputwraps">
                          <%--<img  class="inputImgs" src="/assests/images/NLoginIcon/user.png" alt="arr" />--%>
                          <img  class="inputImgs" src="/assests/images/NLoginIcon/user-2.png" alt="arr" />
                          
                          <asp:TextBox ID="txtUserName" onblur="return ChangeCompany();" CssClass="inputL" runat="server" placeholder="" TabIndex="1"></asp:TextBox>
                      </div>
                      <div style="text-align: left">
                            <em style="color: red; position: relative; right: -160px; top: -35px;"></em>
                            <span style="color: red;">
                                <asp:RequiredFieldValidator ID="rqvUserName" runat="server"
                                    ControlToValidate="txtUserName"
                                    ErrorMessage="Please enter User Name."
                                    ForeColor="Red" ValidationGroup="login" Display="Dynamic">
                                </asp:RequiredFieldValidator></span>
                        </div>
                      <div class="inputwraps">
                          <%--<img class="inputImgs" src="/assests/images/NLoginIcon/password.png" alt="arr" />--%>
                          <img class="inputImgs" src="/assests/images/NLoginIcon/password-2.png" alt="arr" />
                          
                          <asp:TextBox ID="txtPassword" CssClass="inputL" placeholder="" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox>
                          <span class="passWordView"><i class="fa fa-eye-slash"></i></span>
                      </div>
                        <div style="text-align: left">
                            <em style="color: red; position: relative; right: 55px; top: 160px;"></em>
                            <span style="color: red">
                                <asp:RequiredFieldValidator ID="rqvPassword" runat="server"
                                    ControlToValidate="txtPassword"
                                    ErrorMessage="Please enter Password."
                                    ForeColor="Red" ValidationGroup="login" Display="Dynamic">
                                </asp:RequiredFieldValidator></span>
                        </div>

                           <div runat="server" id="divConn">
                                <div class="form-group database-dropdown">
                                    <img class="inputImgs" src="/assests/images/NLoginIcon/database.png" alt="arr" />
                                    <asp:DropDownList ID="ddlConn" CssClass="form-control smalltext fontAwesome" placeholder=" Company" runat="server" TabIndex="3"></asp:DropDownList>
                                </div>
                                <div style="text-align: left">
                                    <em style="color: red; position: relative; right: 55px; top: 160px;"></em>
                                    <span style="color: red">
                                        </span>
                                </div>
                            </div>
                    
                     
                    <div style="font-size:12px; text-align: center;">Application best viewed at <a href="#">1280 x 720</a> resolution in<br /><a href="#">Google Chrome</a> 59 or above</div>
                    <div id="btnProceed" runat="server" class="callArea" style="text-align: center">
                        <asp:Button ID="ShowDB" runat="server" CssClass="btn submitButton " Text="Proceed" OnClick="ShowDB_Click" TabIndex="3" UseSubmitBehavior="false" />
                        <a href="#" class="fPass hide">Forgot password? </a>
                    </div>

                    <div id="btnLoginCls" runat="server" class="callArea" style="text-align: center">
                      
                        <asp:Button ID="Submit1" ValidationGroup="login" runat="server" CssClass="btn submitButton" Text="Submit " OnClick="Login_User" TabIndex="3" UseSubmitBehavior="false" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" TabIndex="4" CssClass="compemail hide" OnClick="LinkButton1_Click1">Forgot  Password?</asp:LinkButton>
                        <a href="#" class="fPass hide">Forgot password? </a>
                    </div>
                      
                        <asp:HiddenField runat="server" ID="hdnCompanyCount" />
                        <asp:HiddenField runat="server" ID="ismobile" />
                      </form>
                  </div>
              </div>
              <div class="ftFooter">
                  © Copyright <%= DateTime.Now.Year.ToString() %> Indus Net Technologies.<br />
                    [BreezeERP Version <asp:Label ID="lblVersion" runat="server" Text="1.0.4" />
              </div>
          </div>
        </div>
      </div> 
        <!--Credit-->
         <div class="creditsSec " id="rc_app_1562">
            <div class="secToggler" id="rc_app_15652"><img src="/assests/images/group.png" style="width:24px" /></div>
            <div class="droper">
                <div class="mnHeader">Team Behind <span id="clCr"><i class="fa fa-close"></i></span></div>
                <div class="scrCnter" id="rc_app_156655">
                    <ul class="tc_s5621f_fg">
                         <li><span class="nameCR">Ankan Das</span></li>
                        <li><span class="nameCR">Arindam Ghosal</span></li>
                         <li><span class="nameCR">Arunabha Saha</span></li>
                        <li><span class="nameCR">Abhishek Munshi</span></li>
                        <li><span class="nameCR">Ashmitar Chowdhury</span></li>
                        <li><span class="nameCR">Ananya Deb</span></li>
                        <li><span class="nameCR">Bhaskar Chatterjee</span></li>
                        <li><span class="nameCR">Bapi Dutta</span></li>
                        <li><span class="nameCR">Chinmoy Maiti</span></li>
                        <li class="tc_sfsf_fg"><span class="nameCR">Debashis Talukder</span></li>
                        <li><span class="nameCR">Debjyoti Dhar</span></li>
                        <li><span class="nameCR">Deep Narayan Mahajan</span></li>
                        <li><span class="nameCR">Goutam Kumar Das</span></li>
                        <li><span class="nameCR">Indranil Dey</span></li>
                         <li><span class="nameCR">Jitendra Jha</span></li>
                         <li><span class="nameCR">Kaushik Gupta</span></li>
                         <li><span class="nameCR">Maynak Nandi</span></li>
                         <li class="tc_s5221f_fg"><span class="nameCR">Priti Ghosh</span></li>
                         <li><span class="nameCR">Pijush Kumar Bhattacharya</span></li>
                         <li><span class="nameCR">Pratik Ghosh</span></li>
                         <li><span class="nameCR">Rajdip Mukherjee</span></li>
                         <li><span class="nameCR">Susanta Kundu</span></li>
                         <li><span class="nameCR">Sanchita Saha</span></li>
                         <li><span class="nameCR">Saheli Bhattacharya</span></li>
                         <li><span class="nameCR">Suman Bachar</span></li>
                         <li><span class="nameCR">Suman Roy</span></li>
                         <li><span class="nameCR">Shantanu Saha</span></li>                   
                         <li><span class="nameCR">Saikat Das</span></li>      
                         <li><span class="nameCR">Sanjoy Ganguly</span></li>
                         <li><span class="nameCR">Sayantani Mandal</span></li>
                         <li><span class="nameCR">Subhra Mukherjee</span></li>
                         <li><span class="nameCR">Sudip Biswas</span></li>
                         <li><span class="nameCR">Sudip Kumar Pal</span></li>
                         <li><span class="nameCR">Surojit Chatterjee</span></li>
                         <li><span class="nameCR">Suvankar Dey</span></li>
                        <li><span class="nameCR">Shantanu Saha</span></li>
                        <li><span class="nameCR">Swatilekha Mukherjee</span></li>   
                        <li><span class="nameCR">Tanmoy Ghosh</span></li>
                    </ul>
                </div>
            </div>
        </div>

    </div>
    </div>
    
    <section style="padding-bottom:80px">
        <div class="container">
            <div class="row">
                <div class="col-md-6 aCenter">
                    <img src="/assests/images/NLoginIcon/avatar1.png" style="max-width: 53%" />
                </div>
                <div class="col-md-6">
                    <h3 class="hd" style="font-family: 'opcen', Montserrat, sans-serif !important;margin-top: 50px;font-size: 28px;">ERP that is As Unique As your Business</h3>
                    <h4 style="font-family: 'opcen', Montserrat, sans-serif !important;color: #727070;font-size:24px">Simple , Intuitive and Easy to Use</h4>
                    <ul class="styleList">
                        <li>Enhanced Data Security </li>
                        <li>Flexible customizations and integrations </li>
                        <li>Easy Inter-departmental Collaborations </li>
                        <li> Track all your Purchase and Sales Transactions</li>
                        <li> Keep an eye on your quotation to order conversion</li>
                        <li> Track overall health of your business in real time</li>
                        <li> Complete visibility of your stock movement</li>
                    </ul>
                </div>
            </div>
            
        </div>
    </section>
    <div class="bg11" style="margin-bottom:100px">
        <div class="container sectionHeader" style="margin-bottom:35px">
            <h3 class="aCenter hd" style="font-family: 'opcen', Montserrat, sans-serif !important;font-size: 33px;">Industry Specific ERP Solutions</h3>
            <p class="aCenter" style="font-family: 'Segoe UI','Helvetica Neue',Helvetica,Verdana,'san-serif';" >Enterprise Resources Planning solutions, built to meet your industry needs.</p>
        </div>
        <div class="container aCenter slickCont" style="margin-bottom:-110px">
            <div class="multiple-items">
              <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/003-accounts.png" alt="arr" />
                    <div class="hding">Accounts & Finance</div>
                </div>
                <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/005-crm.png" alt="arr" />
                    <div class="hding">CRM</div>
                </div>
              <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/004-sales.png" alt="arr" />
                    <div class="hding">SALES</div>
                </div>
                <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/shopping-cart.png" alt="arr" />
                    <div class="hding">Purchases</div>
                </div>
              <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/inventorG.png" alt="arr" />
                    <div class="hding">Invetory & Warehouse</div>
                </div>
                
                <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/factory.png" alt="arr" />
                    <div class="hding">Manufacturing</div>
                </div>
              <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/005-project-management.png" alt="arr" />
                    <div class="hding">Project Management</div>
                </div>
              <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/payroll.png" alt="arr" />
                    <div class="hding">Payroll Management</div>
                </div>
                <div class="boxDB">
                    <img src="/assests/images/NLoginIcon/teamwork.png" alt="arr" />
                    <div class="hding">FIELD SALES MANAGEMENT</div>
                </div>
            </div>

            <ul class="lister hide">
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/003-accounts.png" alt="arr" />
                        <div class="hding">ACCOUNTING</div>
                    </div>
                </li>
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/004-sales.png" alt="arr" />
                        <div class="hding">SALES</div>
                    </div>
                </li>
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/inventorG.png" alt="arr" />
                        <div class="hding">INVENTORY</div>
                    </div>
                </li>
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/005-crm.png" alt="arr" />
                        <div class="hding">CRM</div>
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <section>
        <div class="container aCenter">
            <h3 style="font-family: 'opcen', Montserrat, sans-serif !important;font-size:26px">Log in to Start Your Digital Journey with Breeze .</h3>
            <div style="height:30px"></div>
            <a href="#" class="cta" id="toLogin">
              <span>Login Now</span>
              <svg width="13px" height="10px" viewBox="0 0 13 10">
                <path d="M1,5 L11,5"></path>
                <polyline points="8 1 12 5 8 9"></polyline>
              </svg>
            </a>
            <div ></div>
            <div class="socialArea">
                 <div class="effect egeon">
                    <div class="buttons">
                      <a href="https://www.facebook.com/BreezeERPforSMBs/?source=breezeerp_contact" target="_blank" class="fb" title="Join us on Facebook"><i class="fa fa-facebook" aria-hidden="true"></i></a>
                      <a href="https://www.youtube.com/channel/UC56pxWV5D4VB2mrO-Xi-S1g" target="_blank" class="g-plus" title="Join us on Google+"><i class="fa fa-youtube" aria-hidden="true"></i></a>
                      <a href="https://www.linkedin.com/company/breezeerp/?source=breezeerp_contact" target="_blank" class="in" title="Join us on Linked In"><i class="fa fa-linkedin" aria-hidden="true"></i></a>
                    </div>
                  </div>
            </div>
            
        </div>
    </section>
    <div style="height:100px"></div>







    <div class="greetWrap hide">
        <%--Rev 1.0 --%>
        <%--<img src="/assests/images/greet2.gif" />--%>
        <img src="/assests/images/event-banner.jpg" />
        <%--Rev end 1.0 --%>
    </div>
    
   <div class="backColor hide"></div>
    <%--<img src="/assests/images/diwali.png" class="festiveImage" alt="Happy Diwali" />--%>
    <div class="bgImage hide">
       <%-- <img src="/assests/images/bglogin.jpg" style="width: 100%; height: 100%;" alt="" />--%>
         <img src="/assests/images/webP/bglogin.webp" style="width: 100%; height: 100%;" alt="" />
        
    </div>
   
    <div class="container boxWraper loginFlex hide">
        <div class="centerd-box">
            <div class="logo-wrap relative">
                <img src="/assests/images/webP/logo.webp" width="230" height="70" alt="" style="margin-left: 19px;" />
                <div class="text-center"><span id="privilage"></span></div>
                <%--<dxe:ASPxImage runat="server" ShowLoadingImage="true" ID="urlLogo" CssClass="img-responsive bigLogo"></dxe:ASPxImage>--%>
            </div>
            
            <div class="loginArea">
                
                    
                    <div class="form-group inp mT40">
                        <label for="txtUserName">User Name </label>
                        <div>
                            
                        </div>
                        
                    </div>
                    <div class="form-group inp">
                        <label for="txtPassword">Password </label>
                        <div class="relative">
                            <span class="passWordView"><i class="fa fa-eye"></i></span>
                            
                        </div>
                    </div>






                    

                    
                    <%--<div style="text-align: left">
                            <input data-val="true" data-val-required="The RememberMe field is required." id="RememberMe" name="RememberMe" type="checkbox" value="true"><input name="RememberMe" type="hidden" value="false">
                            Remember Me
                        </div>--%>

                    
                    
                
            </div>
        </div>
    </div>
    <footer class="fixedfooter hide">
            <div class="container">
                <p class="copyright">
                    © Copyright  Indus Net Technologies. [BreezeERP Version
                      <!--<a href="Management/Master/view-version-features.aspx" target="_blank">( What's New )</a>--><br />
                    <%=PageFooterTag2%> 
                </p>
                
            </div>
        </footer>
    
   
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/parallax/2.1.3/parallax.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("a#toLogin").click(function () {
                $("html, body").animate({ scrollTop: 0 }, 300);
                return false;
            });
            if ($("#scene").length) {
                var scene = document.getElementById('scene');
                var parallaxInstance = new Parallax(scene);
            }
            if ($("#sceneDark").length) {
                var sceneDark = document.getElementById('sceneDark');
                var parallaxInstance = new Parallax(sceneDark);
            }
        })
    </script>
    <script type="text/javascript">
        $(function () { });
        var TxtRotate = function (el, toRotate, period) {
            this.toRotate = toRotate;
            this.el = el;
            this.loopNum = 0;
            this.period = parseInt(period, 10) || 2000;
            this.txt = "";
            this.tick();
            this.isDeleting = false;
        };

        TxtRotate.prototype.tick = function () {
            var i = this.loopNum % this.toRotate.length;
            var fullTxt = this.toRotate[i];

            if (this.isDeleting) {
                this.txt = fullTxt.substring(0, this.txt.length - 1);
            } else {
                this.txt = fullTxt.substring(0, this.txt.length + 1);
            }

            this.el.innerHTML = '<span class="wrap">' + this.txt + "</span>";

            var that = this;
            var delta = 100 - Math.random() * 100;

            if (this.isDeleting) {
                delta /= 2;
            }

            if (!this.isDeleting && this.txt === fullTxt) {
                delta = this.period;
                this.isDeleting = true;
            } else if (this.isDeleting && this.txt === "") {
                this.isDeleting = false;
                this.loopNum++;
                delta = 300;
            }

            setTimeout(function () {
                that.tick();
            }, delta);
        };

        window.onload = function () {
            var elements = document.getElementsByClassName("txt-rotate");
            for (var i = 0; i < elements.length; i++) {
                var toRotate = elements[i].getAttribute("data-rotate");
                var period = elements[i].getAttribute("data-period");
                if (toRotate) {
                    new TxtRotate(elements[i], JSON.parse(toRotate), period);
                }
            }
            // INJECT CSS
            var css = document.createElement("style");
            css.type = "text/css";
            css.innerHTML =
              ".txt-rotate > .wrap { border-right: 0.08em solid #666 }";
            document.body.appendChild(css);
        };
    </script>
    <script type="text/javascript" src="/assests/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/assests/js/main.js"></script>
    
</body>
</html>


