<%@ Page Language="C#" AutoEventWireup="true" Inherits="pLogin"
    EnableEventValidation="false" CodeBehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login to BreezeERP</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <script type="text/javascript" src="/assests/js/jquery.min.js"></script>
     
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/assests/bootstrap/css/bootstrap-theme.min.css" />
    <link rel="stylesheet"  href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.3.2/jquery-confirm.min.css" />
    <link href="../assests/css/jquery-ui.css" rel="stylesheet" />
    
    
    <link rel="stylesheet" type="text/css" href="/assests/fonts/font-awesome/css/font-awesome.min.css" />
    <script type="text/javascript" src="/assests/js/modernizr-2.8.3-respond-1.4.2.min.js"></script>
    <meta name="theme-color" content="#0C78B1" />
    <!-- Windows Phone -->
    <link href="MasterPage/css/MasterPage.css" rel="stylesheet" />
    <meta name="msapplication-navbutton-color" content="#0C78B1" />
    <!-- iOS Safari -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent" />
    <script type="text/javascript" src="/assests/js/mobile-detect.js"></script>
    <script type="text/javascript" src="/assests/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/assests/js/jquery.confirm.min.js"></script>
    <script src="/assests/js/modalmsg.js"></script>
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
   
<link  href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;500;600;700;800;900&family=Poppins:wght@100;200;300;400;500;600;700;800;900&display=swap" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
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
            background: #e4dff4;
            display: inline-block;
            border-radius: 9px;
            padding: 9px 10px;
            text-align: center;
            margin-top: 20px;
            box-sizing: border-box;
        }

        .mLogo > img {
           width: 135px;
        }

        .sectionWraper {
            display: flex;
            justify-content: space-between;
            z-index: 100;
            position: relative;
        }

        .textSection h1 {
            font-weight: 400;
            font-size: 38px;
            margin-bottom: 10px;
            color: #323030;
                font-family: revert;
        }

        .scndSize {
           font-size: 22px;
            margin: 0px;
            font-weight:normal;
            margin-bottom: 14px;
            color: #505050;
            margin-top: 16px;
            margin-left: 10px;
            font-family: revert;
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
            width: 345px;
            margin-top: 80px;
            margin-right:50px;
            box-shadow: 0px 8px 18px 4px rgb(0 0 0 / 10%);
        }

        .wlHeader {
                font-family: "Playfair Display", serif;
    color: #41326c;
    margin-bottom: 0px !important;
    font-size: 26px;
        }

        .f2Para {
            font-family: revert;
            margin-top: 5px !important;
            font-size: 13px;
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
            margin-top: 80px;
        }

        .inputwraps {
            position:relative;
            margin-bottom:15px
        }
        .inputL {
            width:100%;
            border:2px solid #b2baeb;
            height:48px;
            box-sizing:border-box;
            border-radius:7px;
            padding-left:35px
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
            background:#8a57f1;
            border:1px solid #8a57f1;
            padding:8px  15px 8px 25px;
            color:#fff;
            border-radius:6px;
        }
        .submitButton:hover {
                background: #7035E6;
            border: 1px solid #642bd5;
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
            border-top:1px solid #eee;
            padding:12px;
            font-size:13px;
            line-height:25px
        }
        .inputImgs {
            position:absolute;
            left: 15px;
            top: 14px;
            max-width:18px;
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
            padding:50px 0
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
                opacity: 0;
                -webkit-transition: all 0.3s ease-in-out;
                transition: all 0.3s ease-in-out;
                box-shadow: 0px 0px 15px rgb(0 0 0 / 20%);
            }

                .droper.active {
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
            box-shadow:0px 10px 10px rgba(0,0,0,0.08)
        }
        .boxDB>img {
            max-width:80px
        }
        .boxDB .hding {
            font-weight:700;
            font-size:22px;
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
            if (theme != '' || theme !=  undefined) {
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
            line-height:35px;
            font-size:1.7rem;
            padding-left:30px;
            background:#fff url("/assests/images/verified.png") no-repeat center left
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
            
        }
        @media only screen and (min-width: 763px){ 

            .onlySm {
                display:none;
            }
        }
    </style>
</head>
<body onload="noBack();setInterval('blinkIt()',500);" onpageshow="if (event.persisted) noBack();"
    onunload="" class="">

    <!--New login -->
    
    <div id="switchArea">
        <label class="switch">
          <input type="checkbox" checked />
          <span class="slider round"></span>
        </label>
    </div>
    <div id="themeClass" class="screenLight">
      <div class="relative setHeight bgforDark" style="margin-bottom:35px;min-height:100vh;overflow:hidden">
        <div id="scene" style="position:absolute;right:-50px;top:-50px;">
            <%--<div class="layer" data-depth="0.1"><img src="/assests/images/bkfive.png" alt="" style="max-width: 600px;" /></div>  
          <div class="layer" data-depth="0.6"><img src="/assests/images/bkone.png" alt="" style="max-width: 600px;" /></div>
          <div class="layer" data-depth="0.2"><img src="/assests/images/bktwo.png" alt="" style="max-width: 600px;" /></div>  
            <div class="layer" data-depth="0.8"><img src="/assests/images/bkthree.png" alt="" style="max-width: 600px;" /></div>
            <div class="layer" data-depth="0.1"><img src="/assests/images/bkfour.png" alt="" style="max-width: 600px;" /></div>     --%>
             <div class="layer" data-depth="0.3"><img src="/assests/images/NLoginIcon/milky.png" alt="" style="max-width: 800px;" /></div>
            <div class="layer" data-depth="0.1"><img src="/assests/images/NLoginIcon/bg1Gr.png" alt="" style="max-width: 800px;" /></div>
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
            <img class="animatedIco a2" src="/assests/images/NLoginIcon/a2.png" alt="" />
            <img class="animatedIco a3" src="/assests/images/NLoginIcon/a3.png" alt="" />
            <img class="animatedIco a4" src="/assests/images/NLoginIcon/a4.png" alt="" />
            <img class="animatedIco a5" src="/assests/images/NLoginIcon/a5.png" alt="" />
            <img class="animatedIco a6" src="/assests/images/NLoginIcon/a6.png" alt="" />
            <img class="animatedIco a7" src="/assests/images/NLoginIcon/a7.png" alt="" />
        </div>
      <div class="container" >
          
        <div class="sectionWraper">
          <div class="textSection">
            <div class="mLogo">
              <img src="/assests/images/logo.png"  />
            </div>
            <h1>Welcome to <span class="emphasized">Breeze ERP</span></h1>
            <h3 class="scndSize">Ready to Harness The Power of E-Invoicing?</h3>
            <h3 class="scndSize">Fast,  easy and Customized solution for</h3>
            <h3 class="scndSize"> <span class="bliner txt-rotate" 
                data-period="2000"
                data-rotate='[ "Payroll Management ", "Service Management ", "STB Management", "Employee Self Service", "Sales and Purchase" ]'</span></h3>
            <div style="margin-top:20px">
                <span class="iconBXX"><img src="/assests/images/NLoginIcon/invoice1.png" /></span>
                <span class="iconBXX"><img src="/assests/images/NLoginIcon/export.png" /></span>
                <span class="iconBXX"><img src="/assests/images/NLoginIcon/global.png" /></span>
            </div>
          </div>
          <div class="formBox relative">
              <div class="onlySm aCenter" style="z-index:9999;"><div class="mLogo"><img src="/assests/images/logo.png"  /></div></div>
              <div style="padding: 10px 30px;">
                <h1 class="wlHeader">Welcome Back....</h1>
                <p class="f2Para">Please enter your username and password</p>

                  <div style="margin-top: 20px">
                      <div>
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" CssClass="hide"></asp:Label>
                      </div>
                      <form action="" method="post" runat="server" novalidate="novalidate">
                        <input id="rurl" name="rurl" runat="server" type="hidden" value="" />
                      <div class="inputwraps">
                          <img  class="inputImgs" src="/assests/images/NLoginIcon/user.png" alt="arr" />
                          <%--<input type="text" class="inputL" />--%>
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
                          <img class="inputImgs" src="/assests/images/NLoginIcon/password.png" alt="arr" />
                          <%--<input type="text" class="inputL" />--%>
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
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlConn" CssClass="form-control smalltext fontAwesome" placeholder=" Company" runat="server" TabIndex="3"></asp:DropDownList>
                                </div>
                                <div style="text-align: left">
                                    <em style="color: red; position: relative; right: 55px; top: 160px;"></em>
                                    <span style="color: red">
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="ddlConn"
                                            ErrorMessage="Please select company."
                                            ForeColor="Red" ValidationGroup="login" Display="Dynamic">
                                        </asp:RequiredFieldValidator>--%></span>
                                </div>
                            </div>
                    
                     <%-- <div>By login, you agree to our <a href="#">Terms and Conditions</a></div>--%>
                    <div style="font-size:12px">Application best viewed at <a href="#">1280 x 720</a> resolution in <a href="#">Google Chrome</a> 59 or above</div>
                    <div id="btnProceed" runat="server" class="callArea" style="text-align: center">
                        <asp:Button ID="ShowDB" runat="server" CssClass="btn submitButton " Text="Proceed" OnClick="ShowDB_Click" TabIndex="3" UseSubmitBehavior="false" />
                        <a href="#" class="fPass">Forgot password? </a>
                    </div>

                    <div id="btnLoginCls" runat="server" class="callArea" style="text-align: center">
                       <%-- <button  type="button" ID="Submit1" runat="server" onserverclick="Login_User">Submit <img src="/assests/images/NLoginIcon/right-arrow.png"  /></button>--%>
                        <asp:Button ID="Submit1" ValidationGroup="login" runat="server" CssClass="btn submitButton" Text="Submit " OnClick="Login_User" TabIndex="3" UseSubmitBehavior="false" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" TabIndex="4" CssClass="compemail hide" OnClick="LinkButton1_Click1">Forgot  Password?</asp:LinkButton>
                        <a href="#" class="fPass">Forgot password? </a>
                    </div>
                      
                        <asp:HiddenField runat="server" ID="hdnCompanyCount" />
                        <asp:HiddenField runat="server" ID="ismobile" />
                      </form>
                  </div>
              </div>
              <div class="ftFooter">
                  © Copyright 2022 Indus Net Technologies.<br />
                    [BreezeERP Version 2.0.26]
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
    <section>
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <img src="/assests/images/NLoginIcon/er6.jpg" style="width:100%" />
                </div>
                <div class="col-md-6">
                    <h3 class="hd" style="font-family:Poppins;margin-top: 100px;font-size: 30px;">Cloud Based ERP Management</h3>
                    <h4 style="font-family: poppins;color: #727070;">Simple , Intuitive and Easy to Use</h4>
                    <ul class="styleList">
                        <li> Track all your Purchase and Sales Transactions</li>
                        <li> Keep an eye on your quotation to order conversion</li>
                        <li> Track overall health of your business in real time</li>
                        <li> Faster documentation and proactive tracking</li>
                        <li> Complete visibility of your stock movement</li>
                        <li> Get timely payment by automated notifications</li>
                    </ul>
                </div>
            </div>
            
        </div>
    </section>
    <div class="bg11" style="margin-bottom:100px">
        <div class="container sectionHeader" style="margin-bottom:35px">
            <h3 class="aCenter hd">Industry Specific ERP Solutions</h3>
            <p class="aCenter">Enterprise Resources Planning solutions, built to meet your industry needs.</p>
        </div>
        <div class="container aCenter" style="margin-bottom:-110px">
            <ul class="lister">
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/004-sales.png" alt="arr" />
                        <div class="hding">SALES</div>
                    </div>
                </li>
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/005-crm.png" alt="arr" />
                        <div class="hding">CRM</div>
                    </div>
                </li>
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/001-project-management.png" alt="arr" />
                        <div class="hding">PROJECT</div>
                    </div>
                </li>
                <li>
                    <div class="boxDB">
                        <img src="/assests/images/NLoginIcon/003-accounts.png" alt="arr" />
                        <div class="hding">ACCOUNT</div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="section">
        <div class="container sectionHeader" style="margin-bottom:35px">
            <h3 class="aCenter hd">Industry Specific ERP Solutions</h3>
            <p class="aCenter">Enterprise Resources Planning solutions, built to meet your industry needs.</p>
        </div>
    </div>
    <div style="height:100px"></div>







    <div class="greetWrap hide">
        <img src="/assests/images/greet2.gif"  />
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
                    © Copyright <%= DateTime.Now.Year.ToString() %> Indus Net Technologies. [BreezeERP Version
                     <asp:Label ID="lblVersion" runat="server" Text="1.0.4" /> <!--<a href="Management/Master/view-version-features.aspx" target="_blank">( What's New )</a>--><br />
                    <%=PageFooterTag2%> 
                </p>
                
            </div>
        </footer>
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/parallax/2.1.3/parallax.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function(){
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


